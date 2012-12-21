using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using System.IO;

namespace Irony.ITG
{
    public interface IBnfiTermCollection : IBnfiTerm { }
    public interface IBnfiTermCollection<out TCollectionType> : IBnfiTerm<TCollectionType> { }

    public partial class BnfiTermCollection : BnfiTermNonTerminal, IBnfiTermCollection
    {
        protected enum ListKind { Star, Plus }

        private ListKind? listKind = null;

        protected Type collectionType { get { return base.type; } }
        private readonly Type elementType;
        private readonly MethodInfo addMethodInfo;

        private BnfTerm bnfTermElement;

        [Obsolete("Use collectionDynamicType instead", error: true)]
        protected new Type type { get { return base.type; } }

        private const BindingFlags bindingAttrInstanceAll = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

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
            if (runtimeCheck && collectionType.GetConstructor(bindingAttrInstanceAll, Type.DefaultBinder, types: Type.EmptyTypes, modifiers: null) == null)
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
                addMethodInfo = collectionType.GetMethod("Add", bindingAttrInstanceAll, Type.DefaultBinder, new[]{elementType}, modifiers: null);
                if (addMethodInfo == null)
                    throw new ArgumentException("Collection type has proper 'Add' method (neither public nor nonpublic)", "collectionType");
            }

            SetNodeCreator();
        }

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

        protected void SetList(BnfTerm bnfTermElement, ListKind listKind)
        {
            this.listKind = listKind;
            this.bnfTermElement = bnfTermElement;
            this.Name = GetName();
        }

        public static BnfExpression MakePlusRule(BnfiTermCollection bnfiTermCollection, BnfTerm delimiter, BnfTerm bnfTermElement)
        {
            bnfiTermCollection.SetList(bnfTermElement, ListKind.Plus);
            return Irony.Parsing.Grammar.CurrentGrammar.MakePlusRule(bnfiTermCollection, delimiter, bnfTermElement);
        }

        public static BnfExpression MakeStarRule(BnfiTermCollection bnfiTermCollection, BnfTerm delimiter, BnfTerm bnfTermElement)
        {
            bnfiTermCollection.SetList(bnfTermElement, ListKind.Star);
            return Irony.Parsing.Grammar.CurrentGrammar.MakeStarRule(bnfiTermCollection, delimiter, bnfTermElement);
        }

        public static BnfiExpressionCollection<TCollectionType> MakePlusRule<TCollectionType>(BnfiTermCollection<TCollectionType, object> bnfiTermCollection, BnfTerm delimiter, BnfTerm bnfTermElement)
            where TCollectionType : ICollection<object>, new()
        {
            return (BnfiExpressionCollection<TCollectionType>)MakePlusRule(bnfiTermCollection, delimiter, bnfTermElement);
        }

        public static BnfiExpressionCollection<TCollectionType> MakeStarRule<TCollectionType>(BnfiTermCollection<TCollectionType, object> bnfiTermCollection, BnfTerm delimiter, BnfTerm bnfTermElement)
            where TCollectionType : ICollection<object>, new()
        {
            return (BnfiExpressionCollection<TCollectionType>)MakeStarRule(bnfiTermCollection, delimiter, bnfTermElement);
        }

        public static BnfiExpressionCollection<TCollectionType> MakePlusRule<TCollectionType, TElementType>(BnfiTermCollection<TCollectionType, TElementType> bnfiTermCollection, BnfTerm delimiter, IBnfiTerm<TElementType> bnfTermElement)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return (BnfiExpressionCollection<TCollectionType>)MakePlusRule(bnfiTermCollection, delimiter, bnfTermElement.AsBnfTerm());
        }

        public static BnfiExpressionCollection<TCollectionType> MakeStarRule<TCollectionType, TElementType>(BnfiTermCollection<TCollectionType, TElementType> bnfiTermCollection, BnfTerm delimiter, IBnfiTerm<TElementType> bnfTermElement)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return (BnfiExpressionCollection<TCollectionType>)MakeStarRule(bnfiTermCollection, delimiter, bnfTermElement.AsBnfTerm());
        }

        protected virtual void SetNodeCreator()
        {
            /*
             * NOTE: We are dealing here with totally typeless collection and using reflection anyway, so we are not forcing the created object to be
             * an IList, ICollection, etc., so we are just working here with an object type and require that the collection has an 'Add' method during runtime.
             * */
            SetNodeCreator<object, object>(
                () => Activator.CreateInstance(collectionType, nonPublic: true),
                (collection, element) => addMethodInfo.Invoke(obj: collection, parameters: new[] { element })
                );
        }

        protected void SetNodeCreator<TCollectionStaticType, TElementStaticType>(Func<TCollectionStaticType> createCollection, Action<TCollectionStaticType, TElementStaticType> addElementToCollection)
        {
            AstConfig.NodeCreator = (context, parseTreeNode) =>
            {
                TCollectionStaticType collection = createCollection();

                foreach (var parseTreeChild in parseTreeNode.ChildNodes)
                {
                    TElementStaticType element = GrammarHelper.AstNodeToValue<TElementStaticType>(parseTreeChild.AstNode);

                    if (elementType.IsInstanceOfType(GrammarHelper.AstNodeToValue<object>(parseTreeChild.AstNode)))
                    {
                        addElementToCollection(collection, element);
                    }
                    else if (!parseTreeChild.Term.Flags.IsSet(TermFlags.NoAstNode))
                    {
                        context.AddMessage(ErrorLevel.Error, parseTreeChild.Span.Location, "Term '{0}' should be type of '{1}' but found '{2}' instead",
                            parseTreeChild.Term, elementType.FullName, element.GetType().FullName);
                    }
                }

                parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(collection, context, parseTreeNode);
            };
        }

        protected string GetName()
        {
            return string.Format("{0}List<{1}>", listKind, bnfTermElement.Name);
        }

        public BnfExpression RuleTL { get { return base.Rule; } set { base.Rule = value; } }

        public new BnfiExpressionCollection Rule { set { base.Rule = value; } }

        public BnfTerm AsBnfTerm()
        {
            return this;
        }
    }

    public abstract partial class BnfiTermCollection<TCollectionType> : BnfiTermCollection, IBnfiTerm<TCollectionType>, IBnfiTermCollection<TCollectionType>, INonTerminalWithSingleTypesafeRule<TCollectionType>
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

        public new BnfiExpressionCollection<TCollectionType> Rule { set { base.Rule = value; } }
    }
}
