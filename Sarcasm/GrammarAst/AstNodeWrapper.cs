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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Sarcasm.GrammarAst
{
    public sealed class AstNodeWrapper : IBrowsableAstNode
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

        public override string ToString()
        {
            return Value != null ? Value.ToString() : "<<NULL>>";
        }
    }
}
