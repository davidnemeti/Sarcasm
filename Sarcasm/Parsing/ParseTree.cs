using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.GrammarAst;

namespace Sarcasm.Parsing
{
    public class ParseTree
    {
        private readonly Irony.Parsing.ParseTree parseTree;

        public ParseTree(Irony.Parsing.ParseTree parseTree)
        {
            this.parseTree = parseTree;
        }

        public string FileName { get { return parseTree.FileName; } }
        public TokenList OpenBraces { get { return parseTree.OpenBraces; } }
        public LogMessageList ParserMessages { get { return parseTree.ParserMessages; } }
        public long ParseTimeMilliseconds { get { return parseTree.ParseTimeMilliseconds; } }
        public string SourceText { get { return parseTree.SourceText; } }
        public object Tag { get { return parseTree.Tag; } }
        public TokenList Tokens { get { return parseTree.Tokens; } }
        public ParseTreeStatus Status { get { return parseTree.Status; } }

        public ParseTreeNode Root { get { return parseTree.Root; } }
        public object RootAstValue { get { return GrammarHelper.AstNodeToValue(parseTree.Root.AstNode); } }

        public bool HasErrors()
        {
            return parseTree.HasErrors();
        }

        public static explicit operator ParseTree(Irony.Parsing.ParseTree parseTree)
        {
            return new ParseTree(parseTree);
        }

        public static implicit operator Irony.Parsing.ParseTree(ParseTree parseTree)
        {
            return parseTree.parseTree;
        }

        public ParseTree<TRoot> ConvertToTypesafe<TRoot>()
        {
            return new ParseTree<TRoot>(this.parseTree);
        }
    }

    public class ParseTree<TRoot> : ParseTree
    {
        public ParseTree(Irony.Parsing.ParseTree parseTree)
            : base(parseTree)
        {
        }

        public new TRoot RootAstValue { get { return (TRoot)base.RootAstValue; } }

        public static explicit operator ParseTree<TRoot>(Irony.Parsing.ParseTree parseTree)
        {
            return new ParseTree<TRoot>(parseTree);
        }

        public static implicit operator Irony.Parsing.ParseTree(ParseTree<TRoot> parseTree)
        {
            return parseTree;
        }
    }
}
