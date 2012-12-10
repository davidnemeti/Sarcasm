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
        public static object Create<T>(T value, AstContext context, ParseTreeNode parseTreeNode)
        {
            return new AstNodeWrapper<T>(value, context, parseTreeNode);
        }
    }

    public class AstNodeWrapper<T> : IBrowsableAstNode
    {
        public T Value { get; private set; }

        internal AstNodeWrapper(T value, AstContext context, ParseTreeNode treeNode)
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
