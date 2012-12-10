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

namespace Irony.Extension.AstBinders
{
    public class TypeForCollection : TypeForNonTerminal
    {
        protected Type collectionType { get { return base.type; } }
        private readonly Type elementType;
        private readonly MethodInfo addMethodInfo;

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

        public override BnfExpression Rule
        {
            get { return base.Rule; }
            set
            {
                /*
                 * NOTE: We are dealing here with totally typeless collection and using reflection anyway, so we are not forcing the created object to be
                 * an IList, ICollection, etc., so we are just working here with an object type and require that the collection has an 'Add' method during runtime.
                 * */
                SetNodeCreator<object, object>(
                    () => Activator.CreateInstance(collectionType, nonPublic: true),
                    (collection, element) => addMethodInfo.Invoke(obj: collection, parameters: new[] { element })
                    );

                base.Rule = value;
            }
        }

        protected void SetNodeCreator<TCollectionStaticType, TElementStaticType>(Func<TCollectionStaticType> createCollection, Action<TCollectionStaticType, TElementStaticType> addElementToCollection)
        {
            AstConfig.NodeCreator = (context, parseTreeNode) =>
            {
                TCollectionStaticType collection = createCollection();

                foreach (var parseTreeChild in parseTreeNode.ChildNodes)
                {
                    TElementStaticType element = GrammarHelper.AstNodeToValue<TElementStaticType>(parseTreeChild.AstNode);

                    if (GrammarHelper.AstNodeToValue<object>(parseTreeChild.AstNode).GetType() == elementType)
                    {
                        addElementToCollection(collection, element);
                    }
                    else if (!parseTreeChild.Term.Flags.IsSet(TermFlags.NoAstNode))
                    {
                        context.AddMessage(ErrorLevel.Error, parseTreeChild.Token.Location, "Term '{0}' should be type of '{1}' but found '{2}' instead",
                            parseTreeChild.Term, elementType.FullName, element.GetType().FullName);
                    }
                }

                parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(collection, context, parseTreeNode);
            };
        }
    }

    public class TypeForCollection<TCollectionType> : TypeForCollection, IBnfTerm<TCollectionType>
        where TCollectionType : ICollection<object>, new()
    {
        internal TypeForCollection(string errorAlias)
            : base(typeof(TCollectionType), typeof(object), errorAlias: errorAlias, runtimeCheck: false)
        {
        }

        protected TypeForCollection(Type collectionType, Type elementType, string errorAlias)
            : base(collectionType, elementType, errorAlias, runtimeCheck: false)
        {
        }

        BnfTerm IBnfTerm<TCollectionType>.AsTypeless()
        {
            return this;
        }

        public override BnfExpression Rule
        {
            get { return base.Rule; }
            set
            {
                SetNodeCreator<TCollectionType, object>(
                    () => new TCollectionType(),
                    (collection, element) => collection.Add(element)
                    );

                base.Rule = value;
            }
        }

        [Obsolete(obsoleteQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        public void SetRule(params IBnfTerm<TCollectionType>[] bnfExpressions)
        {
            base.Rule = GetRuleWithOrBetweenTypesafeExpressions(bnfExpressions);
        }
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
    public class TypeForCollection<TCollectionType, TElementType> : TypeForCollection, IBnfTerm<TCollectionType>
        where TCollectionType : ICollection<TElementType>, new()
    {
        internal TypeForCollection(string errorAlias)
            : base(typeof(TCollectionType), typeof(TElementType), errorAlias, runtimeCheck: false)
        {
        }

        BnfTerm IBnfTerm<TCollectionType>.AsTypeless()
        {
            return this;
        }

        public override BnfExpression Rule
        {
            get { return base.Rule; }
            set
            {
                SetNodeCreator<TCollectionType, TElementType>(
                    () => new TCollectionType(),
                    (collection, element) => collection.Add(element)
                    );

                base.Rule = value;
            }
        }

        [Obsolete(obsoleteQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        public void SetRule(params IBnfTerm<TCollectionType>[] bnfExpressions)
        {
            base.Rule = GetRuleWithOrBetweenTypesafeExpressions(bnfExpressions);
        }
    }
}
