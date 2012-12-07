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

namespace Irony.AstBinders
{
    public class TypeForCollection : TypeForNonTerminal
    {
        //private static readonly Type iCollectionGenericTypeDefinition = typeof(ICollection<>);
        //private static readonly Type iCollectionType = typeof(ICollection<>);

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
            where TCollectionType : System.Collections.IList, new()
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
                 * NOTE: ICollection does not have an Add method (only ICollection<> has), so we cannot use ICollection here to enforce the existance of an 'Add' method,
                 * IList would be an overkill to enforce an Add method, and we are dealing here with totally typeless collection and using reflection anyway,
                 * so we just working here with an object type and require that the collection has an 'Add' method during runtime
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
                    TElementStaticType element = (TElementStaticType)parseTreeChild.AstNode;

                    if (parseTreeChild.AstNode.GetType() == elementType)
                    {
                        addElementToCollection(collection, element);
                    }
                    else if (!parseTreeChild.Term.Flags.IsSet(TermFlags.NoAstNode))
                    {
                        context.AddMessage(ErrorLevel.Error, parseTreeChild.Token.Location, "Term '{0}' should be type of '{1}' but found '{2}' instead",
                            parseTreeChild.Term, elementType.FullName, element.GetType().FullName);
                    }
                }

                parseTreeNode.AstNode = collection;
            };
        }
    }

    /*
     * NOTE: (non-generic) Collection does not have an 'Add' method, that is why we are inheriting from IList.
     * 
     * IList : ICollection, IEnumerable
     * */
    public class TypeForCollection<TCollectionType> : TypeForCollection, IBnfTerm<TCollectionType>
        where TCollectionType : System.Collections.IList, new()
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
    }

    /*
     * NOTE: TypeForCollection<TCollectionType, TElementType> cannot inherit from TypeForCollection<TCollectionType> because constraints on TCollectionType are incompatible
     * since ICollection<T> (type constraint here) and IList (type constraint in TypeForCollection<TCollectionType>) are incompatible types.
     * ICollection<T> has an 'Add' method, so using IList<T> would be an overkill, besides, it would not work either since IList<T> and IList are incompatible as well.
     * 
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
    }
}
