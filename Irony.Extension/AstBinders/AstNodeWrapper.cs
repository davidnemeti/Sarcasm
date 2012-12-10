using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Irony.Extension.AstBinders
{
    public static class AstNodeWrapper
    {
        public static object ValueToAstNode<T>(T value, AstContext context, ParseTreeNodeWithOutAst parseTreeNode)
        {
            return GrammarHelper.Properties[context.Language.Grammar, BoolProperty.BrowsableAstNodes] && !(value is IBrowsableAstNode)
                ? new AstNodeWrapper<T>(value, context, parseTreeNode)
                : value;
        }

        public static object ValueToAstNode<T>(T value, AstContext context, ParseTreeNode parseTreeNode)
        {
            return ValueToAstNode(value, context, (ParseTreeNodeWithOutAst)parseTreeNode);
        }

        public static T AstNodeToValue<T>(object astNode)
        {
            AstNodeWrapper<T> astNodeWrapper = astNode as AstNodeWrapper<T>;

            if (astNodeWrapper == null && astNode.GetType().IsGenericType && astNode.GetType().GetGenericTypeDefinition() == typeof(AstNodeWrapper<>))
                throw new ArgumentException(
                    string.Format("AstNodeWrapper with the wrong generic type argument: {0} was found, but {1} was expected",
                        astNode.GetType().GenericTypeArguments[0].FullName, typeof(T).FullName),
                    "astNode"
                    );

            return astNodeWrapper != null ? astNodeWrapper.Value : (T)astNode;
        }
    }

    public class AstNodeWrapper<T> : IBrowsableAstNode
    {
        public T Value { get; private set; }

        internal AstNodeWrapper(T value, AstContext context, ParseTreeNodeWithOutAst treeNode)
        {
            this.Value = value;

            //this.Term = treeNode.Term;
            //this.Span = treeNode.Span;
            //this.ErrorAnchor = this.Location;
            //            this.AsString = (Term == null ? this.GetType().Name : Term.Name);
        }

        public static implicit operator T(AstNodeWrapper<T> astNode)
        {
            return astNode.Value;
        }

        System.Collections.IEnumerable IBrowsableAstNode.GetChildNodes()
        {
            return null;
        }

        int IBrowsableAstNode.Position
        {
            get { return -1/*Span.Location.Position*/; }
        }
    }
}
