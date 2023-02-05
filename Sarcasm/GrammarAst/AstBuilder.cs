#region License
/*
    This file is part of Sarcasm.

    Copyright 2012-2013 Dávid Németi

    Sarcasm is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Sarcasm is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Sarcasm.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using Irony.Ast;
using Irony.Parsing;

namespace Sarcasm.GrammarAst
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
