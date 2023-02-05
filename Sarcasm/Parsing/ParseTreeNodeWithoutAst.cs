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

using Irony.Parsing;

namespace Sarcasm.Parsing
{
    public class ParseTreeNodeWithoutAst
    {
        private readonly ParseTreeNode parseTreeNode;

        public ParseTreeNodeWithoutAst(ParseTreeNode parseTreeNode)
        {
            this.parseTreeNode = parseTreeNode;
        }

        public Token Token { get { return parseTreeNode.Token; } set { parseTreeNode.Token = value; } }
        public BnfTerm Term { get { return parseTreeNode.Term; } set { parseTreeNode.Term = value; } }
        public int Precedence { get { return parseTreeNode.Precedence; } set { parseTreeNode.Precedence = value; } }
        public Associativity Associativity { get { return parseTreeNode.Associativity; } set { parseTreeNode.Associativity = value; } }
        public SourceSpan Span { get { return parseTreeNode.Span; } set { parseTreeNode.Span = value; } }
        public bool IsError { get { return parseTreeNode.IsError; } set { parseTreeNode.IsError = value; } }
        public object Tag { get { return parseTreeNode.Tag; } set { parseTreeNode.Tag = value; } }
        public TokenList Comments { get { return parseTreeNode.Comments; } set { parseTreeNode.Comments = value; } }

        public ParseTreeNodeList ChildNodes { get { return parseTreeNode.ChildNodes; } }

        public static explicit operator ParseTreeNodeWithoutAst(ParseTreeNode parseTreeNode)
        {
            return new ParseTreeNodeWithoutAst(parseTreeNode);
        }

        public static implicit operator ParseTreeNode(ParseTreeNodeWithoutAst parseTreeNodeWithoutAst)
        {
            return parseTreeNodeWithoutAst.parseTreeNode;
        }

        public override string ToString()
        {
            return parseTreeNode.ToString();
        }

        public string FindTokenAndGetText()
        {
            return parseTreeNode.FindTokenAndGetText();
        }

        public Token FindToken()
        {
            return parseTreeNode.FindToken();
        }

        public bool IsPunctuationOrEmptyTransient()
        {
            return parseTreeNode.IsPunctuationOrEmptyTransient();
        }

        public bool IsOperator()
        {
            return parseTreeNode.IsOperator();
        }
    }
}
