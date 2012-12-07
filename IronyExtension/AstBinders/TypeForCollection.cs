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

        protected const BindingFlags bindingAttrInstanceAll = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

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
                AstConfig.NodeCreator = (context, parseTreeNode) =>
                {
                    /*
                     * NOTE: ICollection does not have an Add method (only ICollection<> has), IList might be overkill to enforce an Add method,
                     * so we just working here with an object type and require that the collection has an Add method during runtime (we are using reflection here anyway)
                     * */
                    object collection = Activator.CreateInstance(collectionType, nonPublic: true);

                    foreach (var parseTreeChild in parseTreeNode.ChildNodes)
                    {
                        if (parseTreeChild.AstNode.GetType() == elementType)
                        {
                            addMethodInfo.Invoke(obj: collection, parameters: new[]{parseTreeChild.AstNode});
                        }
                        else if (!parseTreeChild.Term.Flags.IsSet(TermFlags.NoAstNode))
                        {
                            context.AddMessage(ErrorLevel.Error, parseTreeChild.Token.Location, "Term '{0}' should be type of '{1}' but found '{2}' instead",
                                parseTreeChild.Term, elementType.FullName, parseTreeChild.AstNode.GetType().FullName);
                        }
                    }

                    parseTreeNode.AstNode = collection;
                };

                //CreateAndFillCollection<object, object>(
                //    () => Activator.CreateInstance(collectionType, nonPublic: true),
                //    (collection, element) => addMethodInfo.Invoke(obj: collection, parameters: new[]{element}),
                //    elementDynamicType: elementType);

                base.Rule = value;
            }
        }

        //protected void CreateAndFillCollection<TCollectionStaticType, TElementStaticType>(Func<TCollectionStaticType> createCollection, Action<TCollectionStaticType, TElementStaticType> addElementToCollection)
        //{
        //    CreateAndFillCollection(createCollection, addElementToCollection, elementDynamicType: typeof(TElementStaticType));
        //}

        //protected void CreateAndFillCollection<TCollectionStaticType, TElementStaticType>(Func<TCollectionStaticType> createCollection, Action<TCollectionStaticType, TElementStaticType> addElementToCollection, Type elementDynamicType)
        //{
        //}
    }

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
                AstConfig.NodeCreator = (context, parseTreeNode) =>
                {
                    TCollectionType collection = new TCollectionType();

                    foreach (var parseTreeChild in parseTreeNode.ChildNodes)
                    {
                        if (parseTreeChild.AstNode.GetType() == typeof(object))
                        {
                            collection.Add(parseTreeChild.AstNode);
                        }
                        else if (!parseTreeChild.Term.Flags.IsSet(TermFlags.NoAstNode))
                        {
                            context.AddMessage(ErrorLevel.Error, parseTreeChild.Token.Location, "Term '{0}' should be type of '{1}' but found '{2}' instead",
                                parseTreeChild.Term, typeof(object).FullName, parseTreeChild.AstNode.GetType().FullName);
                        }
                    }

                    parseTreeNode.AstNode = collection;
                };

                base.Rule = value;
            }
        }
    }

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
                AstConfig.NodeCreator = (context, parseTreeNode) =>
                {
                    TCollectionType collection = new TCollectionType();

                    foreach (var parseTreeChild in parseTreeNode.ChildNodes)
                    {
                        if (parseTreeChild.AstNode.GetType() == typeof(TElementType))
                        {
                            collection.Add((TElementType)parseTreeChild.AstNode);
                        }
                        else if (!parseTreeChild.Term.Flags.IsSet(TermFlags.NoAstNode))
                        {
                            context.AddMessage(ErrorLevel.Error, parseTreeChild.Token.Location, "Term '{0}' should be type of '{1}' but found '{2}' instead",
                                parseTreeChild.Term, typeof(TElementType).FullName, parseTreeChild.AstNode.GetType().FullName);
                        }
                    }

                    parseTreeNode.AstNode = collection;
                };

                base.Rule = value;
            }
        }
    }
}
