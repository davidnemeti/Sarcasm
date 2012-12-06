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

        protected TypeForCollection(Type collectionType, string errorAlias)
            : base(collectionType, errorAlias)
        {
            Type collectionBaseGenericDefinition = collectionType.GetInterfaces().FirstOrDefault(interfaceType => interfaceType.GetGenericTypeDefinition() == collectionBaseGenericTypeDefinition);

            if (collectionBaseGenericDefinition == null)
                throw new ArgumentException(string.Format("Type does not implement {0}", collectionBaseGenericTypeDefinition.FullName), "type");

            this.elementType = collectionBaseGenericDefinition.GetGenericArguments()[0];
        }

        public static TypeForCollection<TCollectionType> Of<TCollectionType>(string errorAlias = null)
            where TCollectionType : new()
        {
            return new TypeForCollection<TCollectionType>(typeof(TCollectionType), errorAlias);
        }

        public static TypeForCollection Of(Type collectionType, string errorAlias = null)
        {
            if (collectionType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, binder: null, types: Type.EmptyTypes, modifiers: null) == null)
                throw new ArgumentException("Type has no default constructor (neither public nor nonpublic)", "type");

            return new TypeForCollection(collectionType, errorAlias);
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

    public class TypeForCollection<TCollectionType> : TypeForCollection, IBnfTerm<TCollectionType>
    {
        internal TypeForCollection(Type collectionType, string errorAlias)
            : base(collectionType, errorAlias)
        {
        }

        BnfTerm IBnfTerm<TCollectionType>.AsTypeless()
        {
            return this;
        }
    }
}
