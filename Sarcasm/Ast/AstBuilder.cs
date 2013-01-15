using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Sarcasm.Ast
{
    public class AstBuilder : Irony.Ast.AstBuilder
    {
        public AstBuilder(AstContext context)
            : base(context)
        {
        }

        public override void BuildAst(ParseTreeNode parseNode)
        {
            if (parseNode.Term.Flags.IsSet(TermFlags.IsList) && parseNode.Term.Flags.IsSet(TermFlags.NoAstNode))
            {   // HACK: the default BuildAst does not proceed the children of the list node (is it a bug in Irony, or what?)
                var mappedChildNodes = parseNode.GetMappedChildNodes();
                foreach (var mappedChildNode in mappedChildNodes)
                    BuildAst(mappedChildNode);
            }
            else
                base.BuildAst(parseNode);
        }
    }
}
