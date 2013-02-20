using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
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
