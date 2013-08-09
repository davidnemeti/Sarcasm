using System;
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
using Sarcasm.DomainCore;

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

        protected class Dummy
        {
            public static readonly Dummy Instance = new Dummy();
        }

        #endregion

        #region State

        private ListKind? listKind = null;

        private readonly Type elementType;
        private readonly MethodInfo addMethod;

        private BnfTerm element;
        private BnfTerm delimiter;

        public EmptyCollectionHandling EmptyCollectionHandling { get; private set; }

        #endregion

        protected Type collectionType { get { return base.type; } }

        [Obsolete("Use collectionType instead", error: true)]
        public new Type type { get { return base.type; } }

        private const BindingFlags bindingAttrInstanceAll = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        #region Construction

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        protected BnfiTermCollection(Type collectionTypeOrTypeDefinition, Type elementTypeHint, string name, bool runtimeCheck)
            : base(GetCollectionInfo(collectionTypeOrTypeDefinition, elementTypeHint, runtimeCheck).collectionType, name)
        {
            CollectionInfo collectionInfo = GetCollectionInfo(collectionTypeOrTypeDefinition, elementTypeHint, runtimeCheck);
            Type collectionType = collectionInfo.collectionType;
            this.elementType = collectionInfo.elementType;

            if (runtimeCheck)
            {
#if PCL
                if (collectionType.GetConstructor(new Type[0]) == null)
#else
                if (collectionType.GetConstructor(bindingAttrInstanceAll, System.Type.DefaultBinder, types: System.Type.EmptyTypes, modifiers: null) == null)
#endif
                throw new ArgumentException("Collection type has no default constructor (neither public nor nonpublic)", "type");

#if PCL
                this.addMethod = collectionType.GetMethod("Add", new[] { elementType });
#else
                this.addMethod = collectionType.GetMethod("Add", bindingAttrInstanceAll, System.Type.DefaultBinder, new[] { elementType }, modifiers: null);
#endif

                if (this.addMethod == null)
                    throw new ArgumentException("Collection type has proper 'Add' method (neither public nor nonpublic)", "collectionType");
            }

            this.EmptyCollectionHandling = Sarcasm.GrammarAst.Grammar.CurrentGrammar.EmptyCollectionHandling;

            SetNodeCreator();
        }

        private static CollectionInfo GetCollectionInfo(Type collectionTypeOrTypeDefinition, Type elementTypeHint, bool runtimeCheck)
        {
            if (collectionTypeOrTypeDefinition == null)
                throw new ArgumentNullException("collectionTypeOrTypeDefinition", "collectionTypeOrTypeDefinition should not be null");

            if (collectionTypeOrTypeDefinition.IsInterface && collectionTypeOrTypeDefinition.IsAbstract)
                throw new ArgumentNullException("collectionTypeOrTypeDefinition", "collectionTypeOrTypeDefinition should not be an interface or abstract class (a concrete class is needed)");

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
                    Type guessedElementType = iCollectionGenericType.GetGenericArguments()[0];

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
#if PCL
                () => Activator.CreateInstance(collectionType),
#else
                () => Activator.CreateInstance(collectionType, nonPublic: true),
#endif
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
                    // throw exception only if this situation cannot be the consequence of another ast error
                    if (!GrammarHelper.HasError(context))
                    {
                        string errorMessage = string.Format("Term '{0}' should be type of '{1}' but found '{2}' instead",
                            parseTreeChild.Term,
                            elementType.FullName,
                            childValue != null ? childValue.GetType().FullName : "<<NULL>>");

                        throw new InvalidOperationException(errorMessage);
                    }
                }
            }
        }

        #endregion

        #region StarList and PlusList

        #region Typesafe (TCollectionType, TElementType)

        public static BnfiTermCollection<TCollectionType, TElementType> StarList<TCollectionType, TElementType>(IBnfiTerm<TElementType> bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<TElementType>, new()
        {
            var bnfiTermCollection = BnfiTermCollection<TCollectionType, TElementType>.CreateContractible();
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
            var bnfiTermCollection = BnfiTermCollection<TCollectionType, TElementType>.CreateContractible();
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
            var bnfiTermCollection = BnfiTermCollection<TCollectionType, TElementType>.CreateContractible();
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
            var bnfiTermCollection = BnfiTermCollection<TCollectionType, TElementType>.CreateContractible();
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
            var bnfiTermCollection = BnfiTermCollectionTL.CreateContractible(typeof(List<object>));   // could be "CreateMovable(typeof(List<>), typeof(object))" as well
            MakeStarRule(bnfiTermCollection, delimiter, bnfTermElement);
            return bnfiTermCollection;
        }

        public static BnfiTermCollectionTL PlusListTL(BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            var bnfiTermCollection = BnfiTermCollectionTL.CreateContractible(typeof(List<object>));   // could be "CreateMovable(typeof(List<>), typeof(object))" as well
            MakePlusRule(bnfiTermCollection, delimiter, bnfTermElement);
            return bnfiTermCollection;
        }

        public static BnfiTermCollectionTL StarListTL(Type elementType, BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            var bnfiTermCollection = BnfiTermCollectionTL.CreateContractible(typeof(List<>), elementType);
            MakeStarRule(bnfiTermCollection, delimiter, bnfTermElement);
            return bnfiTermCollection;
        }

        public static BnfiTermCollectionTL PlusListTL(Type elementType, BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            var bnfiTermCollection = BnfiTermCollectionTL.CreateContractible(typeof(List<>), elementType);
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
            bnfiTermCollection.CheckAfterRuleHasBeenSetThatChildrenAreNotContracted();
            return bnfiTermCollection;
        }

        protected static BnfiTermCollection _MakeStarRule(BnfiTermCollection bnfiTermCollection, BnfTerm delimiter, BnfTerm element)
        {
            bnfiTermCollection.SetState(ListKind.Star, element, delimiter);
            Irony.Parsing.Grammar.CurrentGrammar.MakeStarRule(bnfiTermCollection, delimiter, element);
            bnfiTermCollection.CheckAfterRuleHasBeenSetThatChildrenAreNotContracted();
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

        protected void ContractTo(BnfiTermCollection target)
        {
            if (!this.IsContractible)
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
            this.hasBeenContracted = true;
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
                value.ContractTo(this);
            }
        }

        #region Unparse

        protected override bool TryGetUtokensDirectly(IUnparser unparser, UnparsableAst self, out IEnumerable<UtokenValue> utokens)
        {
            utokens = null;
            return false;
        }

        protected override IEnumerable<UnparsableAst> GetChildren(Unparser.ChildBnfTerms childBnfTerms, object astValue, Unparser.Direction direction)
        {
            System.Collections.IEnumerable astCollection = (System.Collections.IEnumerable)astValue;

            if (astCollection == null && this.EmptyCollectionHandling == EmptyCollectionHandling.ReturnNull)
                yield break;    // this null value should be handled as an empty collection

            if (direction == Unparser.Direction.RightToLeft)
                astCollection = astCollection.ReverseNonGenericOptimized();

            bool firstElement = true;

            foreach (object astElement in astCollection)
            {
                if (!firstElement && this.delimiter != null)
                    yield return new UnparsableAst(this.delimiter, astCollection);

                yield return new UnparsableAst(this.element, astElement);

                firstElement = false;
            }
        }

        protected override int? GetChildrenPriority(IUnparser unparser, object astValue, Unparser.Children children, Unparser.Direction direction)
        {
            System.Collections.IEnumerable collection = (System.Collections.IEnumerable)astValue;

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
            : this(collectionType, elementType: null, name: name, dummy: Dummy.Instance)
        {
        }

        public BnfiTermCollectionTL(Type collectionTypeDefinition, Type elementType, string name = null)
            : this(collectionTypeDefinition, elementType, name, Dummy.Instance)
        {
        }

        private BnfiTermCollectionTL(Type collectionTypeOrTypeDefinition, Type elementType, string name, Dummy dummy)
            : base(collectionTypeOrTypeDefinition, elementType, name: name, runtimeCheck: true)
        {
        }

        internal static BnfiTermCollectionTL CreateContractible(Type collectionTypeDefinition, Type elementType)
        {
            return new BnfiTermCollectionTL(collectionTypeDefinition, elementType, name: null, dummy: Dummy.Instance).MakeContractible();
        }

        internal static BnfiTermCollectionTL CreateContractible(Type collectionType)
        {
            return new BnfiTermCollectionTL(collectionType, elementType: null, name: null, dummy: Dummy.Instance).MakeContractible();
        }

        public BnfiTermCollectionTL MakeContractible()
        {
            this.IsContractible = true;
            return this;
        }

        public BnfiTermCollectionTL MakeUncontractible()
        {
            this.IsContractible = false;
            return this;
        }

        public new BnfiTermCollectionTL Rule { set { base.Rule = value; } }
    }

    public abstract partial class BnfiTermCollectionWithCollectionType<TCollectionType> : BnfiTermCollection
    {
        protected BnfiTermCollectionWithCollectionType(Type elementType, string name = null)
            : base(typeof(TCollectionType), elementType, name: name, runtimeCheck: false)
        {
        }
    }

    public partial class BnfiTermCollection<TCollectionType, TElementType> : BnfiTermCollectionWithCollectionType<TCollectionType>, IBnfiTermCollection<TElementType>,
        IBnfiTermOrAbleForChoice<TCollectionType>
        where TCollectionType : ICollection<TElementType>, new()
    {
        public BnfiTermCollection(string name = null)
            : base(typeof(TElementType), name: name)
        {
        }

        internal static BnfiTermCollection<TCollectionType, TElementType> CreateContractible()
        {
            return new BnfiTermCollection<TCollectionType, TElementType>(name: null).MakeContractible();
        }

        public BnfiTermCollection<TCollectionType, TElementType> MakeContractible()
        {
            this.IsContractible = true;
            return this;
        }

        public BnfiTermCollection<TCollectionType, TElementType> MakeUncontractible()
        {
            this.IsContractible = false;
            return this;
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
    }
}
