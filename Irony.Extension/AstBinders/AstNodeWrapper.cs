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
        public static AstNodeWrapper<T> Create<T>(T astObject, AstContext context, ParseTreeNodeWithOutAst treeNode)
        {
            return new AstNodeWrapper<T>(astObject, context, treeNode);
        }
    }

    public class AstNodeWrapper<T> : IBrowsableAstNode
    {
        private readonly T astObject;

        public AstNodeWrapper(T astObject, AstContext context, ParseTreeNodeWithOutAst treeNode)
        {
            this.astObject = astObject;

            //this.Term = treeNode.Term;
            //this.Span = treeNode.Span;
            //this.ErrorAnchor = this.Location;
            //            this.AsString = (Term == null ? this.GetType().Name : Term.Name);
        }

        public static implicit operator T(AstNodeWrapper<T> astNode)
        {
            return astNode.astObject;
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
