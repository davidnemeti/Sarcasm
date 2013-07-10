﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.IO;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm.Unparsing;
using Sarcasm.Utility;

namespace Sarcasm.GrammarAst
{
    public enum ListKind { Star, Plus }

    #region IBnfiTermCollection

    public interface IBnfiTermCollection : IBnfiTerm
    {
    }

    public interface IBnfiTermCollectionTL : IBnfiTermCollection, IBnfiTermTL
    {
    }

    public interface IBnfiTermCollection<out TElementType> : IBnfiTermCollection, IBnfiTerm<IEnumerable<TElementType>>, INonTerminal<IEnumerable<TElementType>>
    {
    }

    #endregion

    public abstract partial class BnfiTermCollection : BnfiTermNonTerminal, IBnfiTermCollection, IUnparsableNonTerminal
    {
        #region Types

        private class CollectionInfo
        {
            public readonly Type collectionType;
            public readonly Type elementType;

            public CollectionInfo(Type collectionType, Type elementType)
            {
                this.collectionType = collectionType;
                this.elementType = elementType;
            }
        }

        #endregion

        #region State

        private ListKind? listKind = null;

        private readonly Type elementType;
        private readonly MethodInfo addMethod;

        private BnfTerm element;
        private BnfTerm delimiter;

        public EmptyCollectionHandling EmptyCollectionHandling { get; set; }

        #endregion

        protected Type collectionType { get { return base.type; } }

        [Obsolete("Use collectionType instead", error: true)]
        public new Type type { get { return base.type; } }

        private const BindingFlags bindingAttrInstanceAll = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        #region Construction

        protected BnfiTermCollection(Type collectionTypeOrTypeDefinition, Type elementTypeHint, string name, bool runtimeCheck, bool isReferable)
            : base(GetCollectionInfo(collectionTypeOrTypeDefinition, elementTypeHint, runtimeCheck).collectionType, name, isReferable)
        {
            CollectionInfo collectionInfo = GetCollectionInfo(collectionTypeOrTypeDefinition, elementTypeHint, runtimeCheck);
            Type collectionType = collectionInfo.collectionType;
            this.elementType = collectionInfo.elementType;

            if (runtimeCheck)
            {
                if (collectionType.GetConstructor(bindingAttrInstanceAll, System.Type.DefaultBinder, types: System.Type.EmptyTypes, modifiers: null) == null)
                    throw new ArgumentException("Collection type has no default constructor (neither public nor nonpublic)", "type");

                this.addMethod = collectionType.GetMethod("Add", bindingAttrInstanceAll, System.Type.DefaultBinder, new[] { elementType }, modifiers: null);

                if (this.addMethod == null)
                    throw new ArgumentException("Collection type has proper 'Add' method (neither public nor nonpublic)", "collectionType");
            }

            SetNodeCreator();

            this.EmptyCollectionHandling = Sarcasm.GrammarAst.Grammar.CurrentGrammar.EmptyCollectionHandling;
        }

        private static CollectionInfo GetCollectionInfo(Type collectionTypeOrTypeDefinition, Type elementTypeHint, bool runtimeCheck)
        {
            if (collectionTypeOrTypeDefinition == null)
                throw new ArgumentNullException("collectionTypeOrTypeDefinition", "collectionTypeOrTypeDefinition should not be null");

            if (!runtimeCheck)
            {
                if (elementTypeHint == null)
                    throw new ArgumentNullException("elementTypeHint", "elementTypeHint should not be null if runtime check is turned off");

                return new CollectionInfo(collectionTypeOrTypeDefinition, elementTypeHint);
            }

            if (collectionTypeOrTypeDefinition.IsGenericTypeDefinition)
            {
                if (elementTypeHint == null)
                    throw new ArgumentNullException("elementType", "elementType should not be null if collectionTypeOrTypeDefinition is a generic type definition");

                return new CollectionInfo(
                    collectionTypeOrTypeDefinition.MakeGenericType(elementTypeHint),
                    elementTypeHint
                    );
            }
            else
            {
                return new CollectionInfo(
                    collectionTypeOrTypeDefinition,
                    _GetElementType(collectionTypeOrTypeDefinition, elementTypeHint, runtimeCheck)
                    );
            }
        }

        private static Type _GetElementType(Type collectionType, Type elementTypeHint, bool runtimeCheck)
        {
            if (!runtimeCheck)
            {
                if (elementTypeHint == null)
                    throw new ArgumentNullException("elementTypeHint", "elementTypeHint should not be null if runtime check is turned off");

                return elementTypeHint;
            }

            if (collectionType.IsGenericType)
            {
                // we try to guess the elementType

                Type iCollectionGenericType = collectionType.GetInterfaces().FirstOrDefault(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(ICollection<>));

                if (iCollectionGenericType != null)
                {
                    Type guessedElementType = iCollectionGenericType.GenericTypeArguments[0];

                    if (elementTypeHint != null && elementTypeHint != guessedElementType)
                        throw new ArgumentException("elementType should be null or equal to the generic parameter of the collectionTypeOrTypeDefinition if collectionTypeOrTypeDefinition is a generic type", "elementType");

                    return guessedElementType;
                }
                else
                    throw new ArgumentException("although collectionTypeOrTypeDefinition is a generic type, elementType could not be guessed", "elementType");
            }
            else if (elementTypeHint != null)
                return elementTypeHint;
            else
                return typeof(object);
        }

        protected virtual void SetNodeCreator()
        {
            /*
             * NOTE: We are dealing here with totally typeless collection and using reflection anyway, so we are not forcing the created object to be
             * an IList, ICollection, etc., so we are just working here with an object type and require that the collection has an 'Add' method during runtime.
             * */
            SetNodeCreator<object, object>(
                () => Activator.CreateInstance(collectionType, nonPublic: true),
                (collection, element) => addMethod.Invoke(obj: collection, parameters: new[] { element })
                );
        }

        protected void SetNodeCreator<TCollectionStaticType, TElementStaticType>(Func<TCollectionStaticType> createCollection, Action<TCollectionStaticType, TElementStaticType> addElementToCollection)
        {
            this.AstConfig.NodeCreator = (context, parseTreeNode) =>
            {
                Lazy<TCollectionStaticType> collection = new Lazy<TCollectionStaticType>(() => createCollection());

                bool collectionHasElements = false;
                foreach (var element in GetFlattenedElements<TElementStaticType>(parseTreeNode, context))
                {
                    collectionHasElements = true;
                    addElementToCollection(collection.Value, element);
                }

                TCollectionStaticType astValue = !collectionHasElements && this.EmptyCollectionHandling == EmptyCollectionHandling.ReturnNull
                    ? default(TCollectionStaticType)
                    : collection.Value;

                parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(astValue, context, parseTreeNode);
            };
        }

        protected IEnumerable<TElementStaticType> GetFlattenedElements<TElementStaticType>(ParseTreeNode parseTreeNode, AstContext context)
        {
            foreach (var parseTreeChild in parseTreeNode.ChildNodes)
            {
                /*
                 * The type of childValue is 'object' because childValue can be other than an element, which causes error,
                 * but we want to give a proper error message (see below) instead of throwing a simple cast exception
                 * */
                object childValue = GrammarHelper.AstNodeToValue(parseTreeChild.AstNode);

                if (elementType.IsInstanceOfType(childValue))
                {
                    yield return (TElementStaticType)childValue;
                }
                else if (parseTreeChild.Term.Flags.IsSet(TermFlags.IsList))
                {
                    foreach (var descendantElement in GetFlattenedElements<TElementStaticType>(parseTreeChild, context))
                        yield return descendantElement;
                }
                else if (parseTreeChild.Term.Flags.IsSet(TermFlags.NoAstNode))
                {
                    // simply omit children with no ast (they are probably delimiters)
                }
                else
                {
                    GrammarHelper.GrammarError(
                        context,
                        parseTreeChild.Span.Location,
                        ErrorLevel.Error,
                        "Term '{0}' should be type of '{1}' but found '{2}' instead", parseTreeChild.Term, elementType.FullName, childValue != null ? childValue.GetType().FullName : "<<NULL>>");
                }
            }
        }

        #endregion

        #region StarList and PlusList

        #region Typesafe (TCollectionType, TElementType)

        public static BnfiTermCollection<TCollectionType, TElementType> StarList<TCollectionType, TElementType>(IBnfiTerm<TElementType> bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<TElementType>, new()
        {
            var bnfiTermCollection = BnfiTermCollection<TCollectionType, TElementType>.CreateMovable();
            MakeStarRule(bnfiTermCollection, delimiter, bnfTermElement);
            return bnfiTermCollection;
        }

        public static BnfiTermCollection<List<TElementType>, TElementType> StarList<TElementType>(IBnfiTerm<TElementType> bnfTermElement, BnfTerm delimiter = null)
        {
            return StarList<List<TElementType>, TElementType>(bnfTermElement, delimiter);
        }

        public static BnfiTermCollection<TCollectionType, TElementType> PlusList<TCollectionType, TElementType>(IBnfiTerm<TElementType> bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<TElementType>, new()
        {
            var bnfiTermCollection = BnfiTermCollection<TCollectionType, TElementType>.CreateMovable();
            MakePlusRule(bnfiTermCollection, delimiter, bnfTermElement);
            return bnfiTermCollection;
        }

        public static BnfiTermCollection<List<TElementType>, TElementType> PlusList<TElementType>(IBnfiTerm<TElementType> bnfTermElement, BnfTerm delimiter = null)
        {
            return PlusList<List<TElementType>, TElementType>(bnfTermElement, delimiter);
        }

        #endregion

        #region Typeless converted to typesafe (TCollectionType, TElementType)

        public static BnfiTermCollection<TCollectionType, TElementType> StarList<TCollectionType, TElementType>(BnfTerm bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<TElementType>, new()
        {
            var bnfiTermCollection = BnfiTermCollection<TCollectionType, TElementType>.CreateMovable();
            MakeStarRule(bnfiTermCollection, delimiter, bnfTermElement);
            return bnfiTermCollection;
        }

        public static BnfiTermCollection<List<TElementType>, TElementType> StarList<TElementType>(BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return StarList<List<TElementType>, TElementType>(bnfTermElement, delimiter);
        }

        public static BnfiTermCollection<TCollectionType, TElementType> PlusList<TCollectionType, TElementType>(BnfTerm bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<TElementType>, new()
        {
            var bnfiTermCollection = BnfiTermCollection<TCollectionType, TElementType>.CreateMovable();
            MakePlusRule(bnfiTermCollection, delimiter, bnfTermElement);
            return bnfiTermCollection;
        }

        public static BnfiTermCollection<List<TElementType>, TElementType> PlusList<TElementType>(BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return PlusList<List<TElementType>, TElementType>(bnfTermElement, delimiter);
        }

        #endregion

        #region Typeless

        public static BnfiTermCollectionTL StarListTL(BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            var bnfiTermCollection = BnfiTermCollectionTL.CreateMovable(typeof(ICollection<object>));   // could be "CreateMovable(typeof(ICollection<>), typeof(object))" as well
            MakeStarRule(bnfiTermCollection, delimiter, bnfTermElement);
            return bnfiTermCollection;
        }

        public static BnfiTermCollectionTL PlusListTL(BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            var bnfiTermCollection = BnfiTermCollectionTL.CreateMovable(typeof(ICollection<object>));   // could be "CreateMovable(typeof(ICollection<>), typeof(object))" as well
            MakePlusRule(bnfiTermCollection, delimiter, bnfTermElement);
            return bnfiTermCollection;
        }

        public static BnfiTermCollectionTL StarListTL(Type elementType, BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            var bnfiTermCollection = BnfiTermCollectionTL.CreateMovable(typeof(ICollection<>), elementType);
            MakeStarRule(bnfiTermCollection, delimiter, bnfTermElement);
            return bnfiTermCollection;
        }

        public static BnfiTermCollectionTL PlusListTL(Type elementType, BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            var bnfiTermCollection = BnfiTermCollectionTL.CreateMovable(typeof(ICollection<>), elementType);
            MakePlusRule(bnfiTermCollection, delimiter, bnfTermElement);
            return bnfiTermCollection;
        }

        #endregion

        #endregion

        #region MakePlusRule and MakeStarRule

        internal static BnfiTermCollectionTL MakePlusRule(BnfiTermCollectionTL bnfiTermCollection, BnfTerm delimiter, BnfTerm element)
        {
            return (BnfiTermCollectionTL)_MakePlusRule(bnfiTermCollection, delimiter, element);
        }

        internal static BnfiTermCollectionTL MakeStarRule(BnfiTermCollectionTL bnfiTermCollection, BnfTerm delimiter, BnfTerm element)
        {
            return (BnfiTermCollectionTL)_MakeStarRule(bnfiTermCollection, delimiter, element);
        }

        internal static BnfiTermCollection<TCollectionType, TElementType> MakePlusRule<TCollectionType, TElementType>(BnfiTermCollection<TCollectionType, TElementType> bnfiTermCollection, BnfTerm delimiter, BnfTerm element)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return (BnfiTermCollection<TCollectionType, TElementType>)_MakePlusRule(bnfiTermCollection, delimiter, element);
        }

        internal static BnfiTermCollection<TCollectionType, TElementType> MakeStarRule<TCollectionType, TElementType>(BnfiTermCollection<TCollectionType, TElementType> bnfiTermCollection, BnfTerm delimiter, BnfTerm element)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return (BnfiTermCollection<TCollectionType, TElementType>)_MakeStarRule(bnfiTermCollection, delimiter, element);
        }

        internal static BnfiTermCollection<TCollectionType, TElementType> MakePlusRule<TCollectionType, TElementType>(BnfiTermCollection<TCollectionType, TElementType> bnfiTermCollection, BnfTerm delimiter, IBnfiTerm<TElementType> element)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return (BnfiTermCollection<TCollectionType, TElementType>)_MakePlusRule(bnfiTermCollection, delimiter, element.AsBnfTerm());
        }

        internal static BnfiTermCollection<TCollectionType, TElementType> MakeStarRule<TCollectionType, TElementType>(BnfiTermCollection<TCollectionType, TElementType> bnfiTermCollection, BnfTerm delimiter, IBnfiTerm<TElementType> element)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return (BnfiTermCollection<TCollectionType, TElementType>)_MakeStarRule(bnfiTermCollection, delimiter, element.AsBnfTerm());
        }

        protected static BnfiTermCollection _MakePlusRule(BnfiTermCollection bnfiTermCollection, BnfTerm delimiter, BnfTerm element)
        {
            bnfiTermCollection.SetState(ListKind.Plus, element, delimiter);
            Irony.Parsing.Grammar.CurrentGrammar.MakePlusRule(bnfiTermCollection, delimiter, element);
            return bnfiTermCollection;
        }

        protected static BnfiTermCollection _MakeStarRule(BnfiTermCollection bnfiTermCollection, BnfTerm delimiter, BnfTerm element)
        {
            bnfiTermCollection.SetState(ListKind.Star, element, delimiter);
            Irony.Parsing.Grammar.CurrentGrammar.MakeStarRule(bnfiTermCollection, delimiter, element);
            return bnfiTermCollection;
        }

        protected void SetState(ListKind listKind, BnfTerm element, BnfTerm delimiter)
        {
            this.listKind = listKind;
            this.element = element;
            this.delimiter = delimiter;
            this.Name = GetName();
        }

        protected void ClearState()
        {
            this.listKind = null;
            this.element = null;
            this.delimiter = null;
            this.Name = "<<null list>>";
        }

        protected void MoveTo(BnfiTermCollection target)
        {
            if (!this.IsMovable)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "This collection should not be a right-value: {0}", this.Name);

            if (!this.listKind.HasValue)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "Right-value collection has not been initialized: {0}", this.Name);

            // note: target.RuleRaw is set and target.SetState is called by _MakePlusRule/_MakeStarRule
            if (this.listKind == ListKind.Plus)
                _MakePlusRule(target, this.delimiter, this.element);
            else if (this.listKind == ListKind.Star)
                _MakeStarRule(target, this.delimiter, this.element);
            else
                throw new InvalidOperationException(string.Format("Unknown listKind: {0}", this.listKind));

            this.RuleRaw = null;
            this.ClearState();
        }

        protected string GetName()
        {
            return string.Format("{0}List<{1}>", listKind, element.Name);
        }

        #endregion

        public BnfExpression RuleRaw { get { return base.Rule; } set { base.Rule = value; } }

        protected new BnfiTermCollection Rule
        {
            set
            {
                value.MoveTo(this);
            }
        }

        public BnfiTermCollection ReturnNullInsteadOfEmptyCollection()
        {
            this.EmptyCollectionHandling = EmptyCollectionHandling.ReturnNull;
            return this;
        }

        public BnfiTermCollection ReturnEmptyCollectionInsteadOfNull()
        {
            this.EmptyCollectionHandling = EmptyCollectionHandling.ReturnEmpty;
            return this;
        }

        #region Unparse

        bool IUnparsableNonTerminal.TryGetUtokensDirectly(IUnparser unparser, object obj, out IEnumerable<UtokenValue> utokens)
        {
            utokens = null;
            return false;
        }

        IEnumerable<UnparsableObject> IUnparsableNonTerminal.GetChildren(IList<BnfTerm> childBnfTerms, object obj, Unparser.Direction direction)
        {
            System.Collections.IEnumerable collection = (System.Collections.IEnumerable)obj;

            if (collection == null && this.EmptyCollectionHandling == EmptyCollectionHandling.ReturnNull)
                yield break;    // this null value should be handled as an empty collection

            if (direction == Unparser.Direction.RightToLeft)
                collection = collection.ReverseNonGenericOptimized();

            bool firstElement = true;

            foreach (object element in collection)
            {
                if (!firstElement && this.delimiter != null)
                    yield return new UnparsableObject(this.delimiter, obj);

                yield return new UnparsableObject(this.element, element);

                firstElement = false;
            }
        }

        int? IUnparsableNonTerminal.GetChildrenPriority(IUnparser unparser, object obj, IEnumerable<UnparsableObject> children)
        {
            System.Collections.IEnumerable collection = (System.Collections.IEnumerable)obj;

            if (collection != null || this.EmptyCollectionHandling == EmptyCollectionHandling.ReturnNull)
                return 1;
            else
                return null;
        }

        #endregion
    }

    public partial class BnfiTermCollectionTL : BnfiTermCollection, IBnfiTermCollectionTL
    {
        public BnfiTermCollectionTL(Type collectionType, string name = null)
            : this(collectionType, elementType: null, name: name, isReferable: true)
        {
        }

        public BnfiTermCollectionTL(Type collectionTypeDefinition, Type elementType, string name = null)
            : this(collectionTypeDefinition, elementType, name, isReferable: true)
        {
        }

        private BnfiTermCollectionTL(Type collectionTypeOrTypeDefinition, Type elementType, string name, bool isReferable)
            : base(collectionTypeOrTypeDefinition, elementType, name: name, runtimeCheck: true, isReferable: isReferable)
        {
        }

        internal static BnfiTermCollectionTL CreateMovable(Type collectionTypeDefinition, Type elementType)
        {
            return new BnfiTermCollectionTL(collectionTypeDefinition, elementType, name: null, isReferable: false);
        }

        internal static BnfiTermCollectionTL CreateMovable(Type collectionType)
        {
            return new BnfiTermCollectionTL(collectionType, elementType: null, name: null, isReferable: false);
        }

        public new BnfiTermCollectionTL Rule { set { base.Rule = value; } }
    }

    public abstract partial class BnfiTermCollectionWithCollectionType<TCollectionType> : BnfiTermCollection
    {
        protected BnfiTermCollectionWithCollectionType(Type elementType, string name = null)
            : this(elementType, name: name, isReferable: true)
        {
        }

        protected BnfiTermCollectionWithCollectionType(Type elementType, string name, bool isReferable)
            : base(typeof(TCollectionType), elementType, name: name, runtimeCheck: false, isReferable: isReferable)
        {
        }
    }

    public partial class BnfiTermCollection<TCollectionType, TElementType> : BnfiTermCollectionWithCollectionType<TCollectionType>, IBnfiTermCollection<TElementType>,
        IBnfiTermOrAbleForChoice<TCollectionType>
        where TCollectionType : ICollection<TElementType>, new()
    {
        public BnfiTermCollection(string name = null)
            : this(name: name, isReferable: true)
        {
        }

        private BnfiTermCollection(string name, bool isReferable)
            : base(typeof(TElementType), name: name, isReferable: isReferable)
        {
        }

        internal static BnfiTermCollection<TCollectionType, TElementType> CreateMovable()
        {
            return new BnfiTermCollection<TCollectionType, TElementType>(name: null, isReferable: false);
        }

        protected override void SetNodeCreator()
        {
            SetNodeCreator<TCollectionType, TElementType>(
                () => new TCollectionType(),
                (collection, element) => collection.Add(element)
                );
        }

        [Obsolete(typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        public BnfiTermCollection RuleTypeless { set { base.Rule = value; } }

        public new BnfiTermCollection<TCollectionType, TElementType> Rule { set { base.Rule = value; } }

        public new BnfiTermCollection<TCollectionType, TElementType> ReturnNullInsteadOfEmptyCollection()
        {
            this.EmptyCollectionHandling = EmptyCollectionHandling.ReturnNull;
            return this;
        }

        public new BnfiTermCollection<TCollectionType, TElementType> ReturnEmptyCollectionInsteadOfNull()
        {
            this.EmptyCollectionHandling = EmptyCollectionHandling.ReturnEmpty;
            return this;
        }
    }
}