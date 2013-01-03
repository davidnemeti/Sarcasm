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
using Irony.ITG.Unparsing;

namespace Irony.ITG.Ast
{
    public partial class BnfiTermCollection : BnfiTermNonTerminal, IBnfiTerm, IUnparsable
    {
        #region Types

        protected enum ListKind { Star, Plus }

        #endregion

        #region State

        private ListKind? listKind = null;

        private readonly Type elementType;
        private readonly MethodInfo addMethod;

        private BnfTerm element;
        private BnfTerm delimiter;

        public EmptyCollectionHandling EmptyCollectionHandling { get; set; }

        #endregion

        protected Type collectionType { get { return base.Type; } }

        [Obsolete("Use collectionDynamicType instead", error: true)]
        public new Type Type { get { return base.Type; } }

        private const BindingFlags bindingAttrInstanceAll = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        #region Construction

        public BnfiTermCollection(Type collectionType, string errorAlias = null)
            : this(collectionType, elementType: null, errorAlias: errorAlias, runtimeCheck: true)
        {
        }

        public BnfiTermCollection(Type collectionType, Type elementType, string errorAlias = null)
            : this(collectionType, elementType, errorAlias, runtimeCheck: true)
        {
        }

        protected BnfiTermCollection(Type collectionType, Type elementType, string errorAlias, bool runtimeCheck)
            : base(collectionType, errorAlias)
        {
            if (runtimeCheck && collectionType.GetConstructor(bindingAttrInstanceAll, System.Type.DefaultBinder, types: System.Type.EmptyTypes, modifiers: null) == null)
                throw new ArgumentException("Collection type has no default constructor (neither public nor nonpublic)", "type");

            if (runtimeCheck && elementType == null && collectionType.IsGenericType)
            {   // we try to guess the elementType
                Type iCollectionGenericType = collectionType.GetInterfaces().FirstOrDefault(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(ICollection<>));
                if (iCollectionGenericType != null)
                    elementType = iCollectionGenericType.GenericTypeArguments[0];
            }
            this.elementType = elementType ?? typeof(object);

            if (runtimeCheck)
            {
                addMethod = collectionType.GetMethod("Add", bindingAttrInstanceAll, System.Type.DefaultBinder, new[] { elementType }, modifiers: null);
                if (addMethod == null)
                    throw new ArgumentException("Collection type has proper 'Add' method (neither public nor nonpublic)", "collectionType");
            }

            SetNodeCreator();

            this.EmptyCollectionHandling = Irony.ITG.Ast.Grammar.CurrentGrammar.EmptyCollectionHandling;
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
                 * but we want to give a proper error message (see below) instead of throwing a cast exception
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
                    context.AddMessage(ErrorLevel.Error, parseTreeChild.Span.Location, "Term '{0}' should be type of '{1}' but found '{2}' instead",
                        parseTreeChild.Term, elementType.FullName, childValue != null ? childValue.GetType().FullName : "<<NULL>>");
                }
            }
        }

        #endregion

        #region StarList and PlusList

        #region Typesafe (TCollectionType, TElementType)

        public static BnfiTermCollection<TCollectionType, TElementType> StarList<TCollectionType, TElementType>(IBnfiTerm<TElementType> bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<TElementType>, new()
        {
            var bnfiTermCollection = new BnfiTermCollection<TCollectionType, TElementType>();
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
            var bnfiTermCollection = new BnfiTermCollection<TCollectionType, TElementType>();
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
            var bnfiTermCollection = new BnfiTermCollection<TCollectionType, TElementType>();
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
            var bnfiTermCollection = new BnfiTermCollection<TCollectionType, TElementType>();
            MakePlusRule(bnfiTermCollection, delimiter, bnfTermElement);
            return bnfiTermCollection;
        }

        public static BnfiTermCollection<List<TElementType>, TElementType> PlusList<TElementType>(BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return PlusList<List<TElementType>, TElementType>(bnfTermElement, delimiter);
        }

        #endregion

        #region Typeless converted to semi-typesafe (TCollectionType, object)

        public static BnfiTermCollection<TCollectionType, object> StarListST<TCollectionType>(BnfTerm bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<object>, new()
        {
            var bnfiTermCollection = new BnfiTermCollection<TCollectionType, object>();
            MakeStarRule(bnfiTermCollection, delimiter, bnfTermElement);
            return bnfiTermCollection;
        }

        public static BnfiTermCollection<List<object>, object> StarListST(BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return StarListST<List<object>>(bnfTermElement, delimiter);
        }

        public static BnfiTermCollection<TCollectionType, object> PlusListST<TCollectionType>(BnfTerm bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<object>, new()
        {
            var bnfiTermCollection = new BnfiTermCollection<TCollectionType, object>();
            MakePlusRule(bnfiTermCollection, delimiter, bnfTermElement);
            return bnfiTermCollection;
        }

        public static BnfiTermCollection<List<object>, object> PlusListST(BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return PlusListST<List<object>>(bnfTermElement, delimiter);
        }

        #endregion

        #region Typeless

        public static BnfiTermCollection StarListTL(BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            var bnfiTermCollection = new BnfiTermCollection(typeof(ICollection<object>));
            MakeStarRule(bnfiTermCollection, delimiter, bnfTermElement);
            return bnfiTermCollection;
        }

        public static BnfiTermCollection PlusListTL(BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            var bnfiTermCollection = new BnfiTermCollection(typeof(ICollection<object>));
            MakePlusRule(bnfiTermCollection, delimiter, bnfTermElement);
            return bnfiTermCollection;
        }

        #endregion

        #endregion

        #region MakePlusRule and MakeStarRule

        internal static BnfiExpressionCollection MakePlusRule(BnfiTermCollection bnfiTermCollection, BnfTerm delimiter, BnfTerm element)
        {
            return (BnfiExpressionCollection)_MakePlusRule(bnfiTermCollection, delimiter, element);
        }

        internal static BnfiExpressionCollection MakeStarRule(BnfiTermCollection bnfiTermCollection, BnfTerm delimiter, BnfTerm element)
        {
            return (BnfiExpressionCollection)_MakeStarRule(bnfiTermCollection, delimiter, element);
        }

        internal static BnfiExpressionCollection<TCollectionType> MakePlusRule<TCollectionType>(BnfiTermCollection<TCollectionType, object> bnfiTermCollection, BnfTerm delimiter, BnfTerm element)
            where TCollectionType : ICollection<object>, new()
        {
            return (BnfiExpressionCollection<TCollectionType>)_MakePlusRule(bnfiTermCollection, delimiter, element);
        }

        internal static BnfiExpressionCollection<TCollectionType> MakeStarRule<TCollectionType>(BnfiTermCollection<TCollectionType, object> bnfiTermCollection, BnfTerm delimiter, BnfTerm element)
            where TCollectionType : ICollection<object>, new()
        {
            return (BnfiExpressionCollection<TCollectionType>)_MakeStarRule(bnfiTermCollection, delimiter, element);
        }

        internal static BnfiExpressionCollection<TCollectionType> MakePlusRule<TCollectionType, TElementType>(BnfiTermCollection<TCollectionType, TElementType> bnfiTermCollection, BnfTerm delimiter, IBnfiTerm<TElementType> element)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return (BnfiExpressionCollection<TCollectionType>)_MakePlusRule(bnfiTermCollection, delimiter, element.AsBnfTerm());
        }

        internal static BnfiExpressionCollection<TCollectionType> MakeStarRule<TCollectionType, TElementType>(BnfiTermCollection<TCollectionType, TElementType> bnfiTermCollection, BnfTerm delimiter, IBnfiTerm<TElementType> element)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return (BnfiExpressionCollection<TCollectionType>)_MakeStarRule(bnfiTermCollection, delimiter, element.AsBnfTerm());
        }

        protected static BnfExpression _MakePlusRule(BnfiTermCollection bnfiTermCollection, BnfTerm delimiter, BnfTerm element)
        {
            bnfiTermCollection.SetList(ListKind.Plus, element, delimiter);
            return Irony.Parsing.Grammar.CurrentGrammar.MakePlusRule(bnfiTermCollection, delimiter, element);
        }

        protected static BnfExpression _MakeStarRule(BnfiTermCollection bnfiTermCollection, BnfTerm delimiter, BnfTerm element)
        {
            bnfiTermCollection.SetList(ListKind.Star, element, delimiter);
            return Irony.Parsing.Grammar.CurrentGrammar.MakeStarRule(bnfiTermCollection, delimiter, element);
        }

        protected void SetList(ListKind listKind, BnfTerm element, BnfTerm delimiter)
        {
            this.listKind = listKind;
            this.element = element;
            this.delimiter = delimiter;
            this.Name = GetName();
        }

        protected string GetName()
        {
            return string.Format("{0}List<{1}>", listKind, element.Name);
        }

        #endregion

        public BnfExpression RuleRaw { get { return base.Rule; } set { base.Rule = value; } }

        public new BnfiExpressionCollection Rule { set { base.Rule = value; } }

        BnfTerm IBnfiTerm.AsBnfTerm()
        {
            return this;
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

        public IEnumerable<Utoken> Unparse(Unparser unparser, object obj)
        {
            System.Collections.IEnumerable collection = (System.Collections.IEnumerable)obj;
            bool firstElement = true;
            foreach (object element in collection)
            {
                if (!firstElement && delimiter != null)
                {
                    foreach (Utoken utoken in unparser.Unparse(obj: null, bnfTerm: delimiter))
                        yield return utoken;
                }

                foreach (Utoken utoken in unparser.Unparse(element, this.element))
                    yield return utoken;

                firstElement = false;
            }
        }
    }

    public abstract partial class BnfiTermCollection<TCollectionType> : BnfiTermCollection, IBnfiTerm<TCollectionType>
    {
        public BnfiTermCollection(string errorAlias = null)
            : base(typeof(TCollectionType), typeof(object), errorAlias: errorAlias, runtimeCheck: false)
        {
        }
    }

    public partial class BnfiTermCollection<TCollectionType, TElementType> : BnfiTermCollection<TCollectionType>
        where TCollectionType : ICollection<TElementType>, new()
    {
        public BnfiTermCollection(string errorAlias = null)
            : base(errorAlias)
        {
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

        public BnfiExpressionCollection RuleTypeless { set { base.Rule = value; } }

        public new BnfiExpressionCollection<TCollectionType> Rule { set { base.Rule = value; } }

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
