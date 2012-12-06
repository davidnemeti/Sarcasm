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
        private static readonly Type collectionBaseGenericTypeDefinition = typeof(ICollection<>);

        protected Type collectionType { get { return base.type; } }
        private readonly Type elementType;

        [Obsolete("Use collectionType instead", error: true)]
        protected new Type type { get { return base.type; } }

        protected TypeForCollection(Type collectionDynamicType, string errorAlias)
            : base(collectionDynamicType, errorAlias)
        {
            if (collectionDynamicType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, binder: null, types: Type.EmptyTypes, modifiers: null) == null)
                throw new ArgumentException("Type has no default constructor (neither public nor nonpublic)", "type");

            Type collectionDynamicBaseGenericDefinition = collectionDynamicType.GetInterfaces().FirstOrDefault(interfaceType => interfaceType.GetGenericTypeDefinition() == collectionBaseGenericTypeDefinition);

            if (collectionDynamicBaseGenericDefinition == null)
                throw new ArgumentException(string.Format("Type does not implement {0}", collectionBaseGenericTypeDefinition.FullName), "type");

            this.elementType = collectionDynamicBaseGenericDefinition.GetGenericArguments()[0];
        }

        public static TypeForCollection<TCollectionStaticType> Of<TCollectionStaticType>(string errorAlias = null)
            where TCollectionStaticType : new()
        {
            return new TypeForCollection<TCollectionStaticType>(typeof(TCollectionStaticType), errorAlias);
        }

        public static TypeForCollection<TCollectionStaticType> Of<TCollectionStaticType>(Type collectionDynamicType, string errorAlias = null)
        {
            return new TypeForCollection<TCollectionStaticType>(collectionDynamicType, errorAlias);
        }

        public static TypeForCollection Of(Type collectionDynamicType, string errorAlias = null)
        {
            return new TypeForCollection(collectionDynamicType, errorAlias);
        }

        public override BnfExpression Rule
        {
            get { return base.Rule; }
            set
            {
                AstConfig.NodeCreator = (context, parseTreeNode) =>
                {
                    dynamic collection = Activator.CreateInstance(collectionType, nonPublic: true);

                    foreach (var parseTreeChild in parseTreeNode.ChildNodes)
                    {
                        if (parseTreeChild.AstNode.GetType() == elementType)
                        {
                            collection.Add(parseTreeChild.AstNode);
                        }
                        else if (!parseTreeChild.Term.Flags.IsSet(TermFlags.NoAstNode))
                        {
                            context.AddMessage(ErrorLevel.Error, parseTreeChild.Token.Location, "Term '{0}' should be type of '{1}' but found '{2}' instead",
                                parseTreeChild.Term, elementType.FullName, parseTreeChild.AstNode.GetType().FullName);
                        }
                    }

                    parseTreeNode.AstNode = collection;
                };

                base.Rule = value;
            }
        }
    }

    public class TypeForCollection<TCollectionStaticType> : TypeForCollection, IBnfTerm<TCollectionStaticType>
    {
        internal TypeForCollection(Type collectionDynamicType, string errorAlias)
            : base(collectionDynamicType, errorAlias)
        {
            if (collectionDynamicType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, binder: null, types: Type.EmptyTypes, modifiers: null) == null)
                throw new ArgumentException("Type has no default constructor (neither public nor nonpublic)", "type");
        }

        BnfTerm IBnfTerm<TCollectionStaticType>.AsTypeless()
        {
            return this;
        }
    }
}
