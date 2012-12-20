﻿using System;
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
    public interface IBnfTermCollection : IBnfTerm { }
    public interface IBnfTermCollection<out TCollectionType> : IBnfTerm<TCollectionType> { }

    public partial class TypeForCollection : TypeForNonTerminal, IBnfTermCollection
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

        protected TypeForCollection(Type collectionType, Type elementType, string errorAlias, bool runtimeCheck)
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

        public static TypeForCollection<TCollectionType> Of<TCollectionType>(string errorAlias = null)
            where TCollectionType : ICollection<object>, new()
        {
            return new TypeForCollection<TCollectionType>(errorAlias);
        }

        public static TypeForCollection<TCollectionType, TElementType> Of<TCollectionType, TElementType>(string errorAlias = null)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return new TypeForCollection<TCollectionType, TElementType>(errorAlias);
        }

        public static TypeForCollection Of(Type collectionType, string errorAlias = null)
        {
            return new TypeForCollection(collectionType, elementType: null, errorAlias: errorAlias, runtimeCheck: true);
        }

        public static TypeForCollection Of(Type collectionType, Type elementType, string errorAlias = null)
        {
            return new TypeForCollection(collectionType, elementType, errorAlias, runtimeCheck: true);
        }

        #region StarList and PlusList

        #region Typesafe (TCollectionType, TElementType)

        public static TypeForCollection<TCollectionType, TElementType> StarList<TCollectionType, TElementType>(IBnfTerm<TElementType> bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<TElementType>, new()
        {
            var typeForCollection = TypeForCollection.Of<TCollectionType, TElementType>();
            MakeStarRule(typeForCollection, delimiter, bnfTermElement);
            return typeForCollection;
        }

        public static TypeForCollection<List<TElementType>, TElementType> StarList<TElementType>(IBnfTerm<TElementType> bnfTermElement, BnfTerm delimiter = null)
        {
            return StarList<List<TElementType>, TElementType>(bnfTermElement, delimiter);
        }

        public static TypeForCollection<TCollectionType, TElementType> PlusList<TCollectionType, TElementType>(IBnfTerm<TElementType> bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<TElementType>, new()
        {
            var typeForCollection = TypeForCollection.Of<TCollectionType, TElementType>();
            MakePlusRule(typeForCollection, delimiter, bnfTermElement);
            return typeForCollection;
        }

        public static TypeForCollection<List<TElementType>, TElementType> PlusList<TElementType>(IBnfTerm<TElementType> bnfTermElement, BnfTerm delimiter = null)
        {
            return PlusList<List<TElementType>, TElementType>(bnfTermElement, delimiter);
        }

        #endregion

        #region Typeless converted to typesafe (TCollectionType, TElementType)

        public static TypeForCollection<TCollectionType, TElementType> StarList<TCollectionType, TElementType>(BnfTerm bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<TElementType>, new()
        {
            var typeForCollection = TypeForCollection.Of<TCollectionType, TElementType>();
            MakeStarRule(typeForCollection, delimiter, bnfTermElement);
            return typeForCollection;
        }

        public static TypeForCollection<List<TElementType>, TElementType> StarList<TElementType>(BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return StarList<List<TElementType>, TElementType>(bnfTermElement, delimiter);
        }

        public static TypeForCollection<TCollectionType, TElementType> PlusList<TCollectionType, TElementType>(BnfTerm bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<TElementType>, new()
        {
            var typeForCollection = TypeForCollection.Of<TCollectionType, TElementType>();
            MakePlusRule(typeForCollection, delimiter, bnfTermElement);
            return typeForCollection;
        }

        public static TypeForCollection<List<TElementType>, TElementType> PlusList<TElementType>(BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return PlusList<List<TElementType>, TElementType>(bnfTermElement, delimiter);
        }

        #endregion

        #region Typeless converted to semi-typesafe (TCollectionType)

        public static TypeForCollection<TCollectionType> StarListST<TCollectionType>(BnfTerm bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<object>, new()
        {
            var typeForCollection = TypeForCollection.Of<TCollectionType>();
            MakeStarRule(typeForCollection, delimiter, bnfTermElement);
            return typeForCollection;
        }

        public static TypeForCollection<List<object>> StarListST(BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return StarListST<List<object>>(bnfTermElement, delimiter);
        }

        public static TypeForCollection<TCollectionType> PlusListST<TCollectionType>(BnfTerm bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<object>, new()
        {
            var typeForCollection = TypeForCollection.Of<TCollectionType>();
            MakePlusRule(typeForCollection, delimiter, bnfTermElement);
            return typeForCollection;
        }

        public static TypeForCollection<List<object>> PlusListST(BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return PlusListST<List<object>>(bnfTermElement, delimiter);
        }

        #endregion

        #region Typeless

        public static TypeForCollection StarListTL(BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            var typeForCollection = TypeForCollection.Of(typeof(ICollection<object>));
            MakeStarRule(typeForCollection, delimiter, bnfTermElement);
            return typeForCollection;
        }

        public static TypeForCollection PlusListTL(BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            var typeForCollection = TypeForCollection.Of(typeof(ICollection<object>));
            MakePlusRule(typeForCollection, delimiter, bnfTermElement);
            return typeForCollection;
        }

        #endregion

        #endregion

        protected void SetList(BnfTerm bnfTermElement, ListKind listKind)
        {
            this.listKind = listKind;
            this.bnfTermElement = bnfTermElement;
            this.Name = GetName();
        }

        public static BnfExpression MakePlusRule(TypeForCollection typeForCollection, BnfTerm delimiter, BnfTerm bnfTermElement)
        {
            typeForCollection.SetList(bnfTermElement, ListKind.Plus);
            return Grammar.CurrentGrammar.MakePlusRule(typeForCollection, delimiter, bnfTermElement);
        }

        public static BnfExpression MakeStarRule(TypeForCollection typeForCollection, BnfTerm delimiter, BnfTerm bnfTermElement)
        {
            typeForCollection.SetList(bnfTermElement, ListKind.Star);
            return Grammar.CurrentGrammar.MakeStarRule(typeForCollection, delimiter, bnfTermElement);
        }

        public static BnfExpressionCollection<TCollectionType> MakePlusRule<TCollectionType>(TypeForCollection<TCollectionType> typeForCollection, BnfTerm delimiter, BnfTerm bnfTermElement)
            where TCollectionType : ICollection<object>, new()
        {
            return (BnfExpressionCollection<TCollectionType>)MakePlusRule(typeForCollection, delimiter, bnfTermElement);
        }

        public static BnfExpressionCollection<TCollectionType> MakeStarRule<TCollectionType>(TypeForCollection<TCollectionType> typeForCollection, BnfTerm delimiter, BnfTerm bnfTermElement)
            where TCollectionType : ICollection<object>, new()
        {
            return (BnfExpressionCollection<TCollectionType>)MakeStarRule(typeForCollection, delimiter, bnfTermElement);
        }

        public static BnfExpressionCollection<TCollectionType> MakePlusRule<TCollectionType, TElementType>(TypeForCollection<TCollectionType, TElementType> typeForCollection, BnfTerm delimiter, IBnfTerm<TElementType> bnfTermElement)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return (BnfExpressionCollection<TCollectionType>)MakePlusRule(typeForCollection, delimiter, bnfTermElement.AsBnfTerm());
        }

        public static BnfExpressionCollection<TCollectionType> MakeStarRule<TCollectionType, TElementType>(TypeForCollection<TCollectionType, TElementType> typeForCollection, BnfTerm delimiter, IBnfTerm<TElementType> bnfTermElement)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return (BnfExpressionCollection<TCollectionType>)MakeStarRule(typeForCollection, delimiter, bnfTermElement.AsBnfTerm());
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

        public new BnfExpressionCollection Rule { set { base.Rule = value; } }

        public BnfTerm AsBnfTerm()
        {
            return this;
        }
    }

    public partial class TypeForCollection<TCollectionType> : TypeForCollection, INonTerminalWithSingleTypesafeRule<TCollectionType>, IBnfTermCollection<TCollectionType>
        where TCollectionType : ICollection<object>, new()
    {
        internal TypeForCollection(string errorAlias)
            : base(typeof(TCollectionType), typeof(object), errorAlias: errorAlias, runtimeCheck: false)
        {
        }

        protected override void SetNodeCreator()
        {
            SetNodeCreator<TCollectionType, object>(
                () => new TCollectionType(),
                (collection, element) => collection.Add(element)
                );
        }

        [Obsolete(typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        public new BnfExpressionCollection<TCollectionType> Rule { set { base.Rule = value; } }
    }

    /*
     * NOTE: TypeForCollection<TCollectionType, TElementType> cannot inherit from TypeForCollection<TCollectionType> because constraints on TCollectionType are incompatible
     * since ICollection<T> (type constraint here) and IList/ICollection<object> (type constraint in TypeForCollection<TCollectionType>) are incompatible types.
     * ICollection<T> has an 'Add' method, so using IList<T> would be an overkill, besides, it would not work either since IList<T> and IList are incompatible as well.
     * 
     * IList : ICollection, IEnumerable
     * ICollection<T> : IEnumerable<T>, IEnumerable
     * IList<T> : ICollection<T>, IEnumerable<T>, IEnumerable
     * */
    public partial class TypeForCollection<TCollectionType, TElementType> : TypeForCollection, IBnfTerm<TCollectionType>, INonTerminalWithSingleTypesafeRule<TCollectionType>, IBnfTermCollection<TCollectionType>
        where TCollectionType : ICollection<TElementType>, new()
    {
        internal TypeForCollection(string errorAlias)
            : base(typeof(TCollectionType), typeof(object), errorAlias: errorAlias, runtimeCheck: false)
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

        public new BnfExpressionCollection<TCollectionType> Rule { set { base.Rule = value; } }
    }
}