using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Irony.ITG
{
    public class AstNodeWrapper : IBrowsableAstNode
    {
        public object Value { get; private set; }

        private readonly AstContext context;
        private readonly ParseTreeNode parseTreeNode;

        internal AstNodeWrapper(object value, AstContext context, ParseTreeNode parseTreeNode)
        {
            this.Value = value;
            this.context = context;
            this.parseTreeNode = parseTreeNode;
        }

        System.Collections.IEnumerable IBrowsableAstNode.GetChildNodes()
        {
            return parseTreeNode.ChildNodes.Select(parseTreeChild => parseTreeChild.AstNode);
        }

        int IBrowsableAstNode.Position
        {
            get { return parseTreeNode.Span.Location.Position; }
        }
    }
}
