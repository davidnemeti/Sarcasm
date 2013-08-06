﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.DomainCore;
using Sarcasm.GrammarAst;

namespace Sarcasm.Parsing
{
    public class ParseTree
    {
        private readonly Irony.Parsing.ParseTree parseTree;

        private Dictionary<object, ParseTreeNode> astValueToParseTreeNode;
        private HashSet<Token> decoratedComments;
        private Dictionary<ParseTreeNode, ParseTreeNode> parseTreeNodeToParent;
        private ParseTreeNode lastVisitedParseTreeNode;

        private Dictionary<object, Comments> _astValueToDomainComments;
        protected Dictionary<object, Comments> astValueToDomainComments
        {
            get
            {
                if (_astValueToDomainComments == null)
                    BuildAstValueToParseTreeNode();

                return _astValueToDomainComments;
            }

            set { _astValueToDomainComments = value; }
        }

        internal ICommentCleaner CommentCleaner { get; set; }

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

        public ParseTreeNode GetParseTreeNodeForAstValue(object astValue)
        {
            if (astValueToParseTreeNode == null)
                BuildAstValueToParseTreeNode();

            return astValueToParseTreeNode[astValue];
        }

        public Document GetDocument()
        {
            return new Document(RootAstValue, astValueToDomainComments);
        }

        private void BuildAstValueToParseTreeNode()
        {
            astValueToParseTreeNode = new Dictionary<object, ParseTreeNode>();
            parseTreeNodeToParent = new Dictionary<ParseTreeNode, ParseTreeNode>();
            astValueToDomainComments = new Dictionary<object, Comments>();
            decoratedComments = new HashSet<Token>();
            lastVisitedParseTreeNode = null;

            StoreAstValueToParseTreeNodeRecursive(parseTree.Root, nodeIndex: 0);
        }

        private void StoreAstValueToParseTreeNodeRecursive(ParseTreeNode parseTreeNode, int nodeIndex)
        {
            object astValue = GrammarHelper.AstNodeToValue(parseTreeNode.AstNode);

            if (astValue != null && astValue.GetType().IsClass && !astValueToParseTreeNode.ContainsKey(astValue))
                astValueToParseTreeNode.Add(astValue, parseTreeNode);

            ParseTreeNode lastVisitedParseTreeNodeLocal = this.lastVisitedParseTreeNode;

            foreach (var parseTreeChild in parseTreeNode.ChildNodes.Select((parseTreeChild, childIndex) => new { Value = parseTreeChild, Index = childIndex }))
            {
                parseTreeNodeToParent.Add(parseTreeChild.Value, parseTreeNode);
                StoreAstValueToParseTreeNodeRecursive(parseTreeChild.Value, parseTreeChild.Index);
            }

            if (parseTreeNode.Comments.Count > 0)
            {
                bool isTextInLineAtLeftOfFirstComment = IsTextInLineAtLeftOfComment(parseTreeNode.Comments[0]);
                int lineIndexForFirstComment = parseTreeNode.Comments[0].Location.Line;

                foreach (Token comment in parseTreeNode.Comments)
                {
                    if (decoratedComments.Contains(comment))
                        continue;

                    bool isCommentInLineOfFirstComment = comment.Location.Line == lineIndexForFirstComment;

                    if (isTextInLineAtLeftOfFirstComment && isCommentInLineOfFirstComment && lastVisitedParseTreeNodeLocal.AstNode != null)
                        AddCommentToRightOfAstValue(lastVisitedParseTreeNodeLocal, comment);     // the comment belongs to the previous node (is in line with it)

                    else if (astValue != null)
                        AddCommentToLeftOfAstValue(parseTreeNode, comment);               // the comment belongs to this node (is in line with it)

                    else if (nodeIndex > 0)
                        parseTreeNodeToParent[parseTreeNode].Comments.Add(comment);     // could not decorate comment, because we have no ast, so copy it on the parent and handle it there (if nodeIndex == 0 then Irony has already copied it on the parent)
                }
            }

            this.lastVisitedParseTreeNode = parseTreeNode;
        }

        private bool IsTextInLineAtLeftOfComment(Token comment)
        {
            for (int position = comment.Location.Position - 1; !IsLineStart(position); position--)
                if (!char.IsWhiteSpace(this.SourceText, position))
                    return true;

            return false;
        }

        private bool IsLineStart(int position)
        {
            return position <= 0 || this.SourceText[position] == '\n';
        }

        private void AddCommentToLeftOfAstValue(ParseTreeNode parseTreeNode, Token comment)
        {
            object astValue = GrammarHelper.AstNodeToValue(parseTreeNode.AstNode);

            Comments domainComments;

            if (!astValueToDomainComments.TryGetValue(astValue, out domainComments))
            {
                domainComments = new Comments();
                astValueToDomainComments.Add(astValue, domainComments);
            }

            domainComments.left.Add(CommentToDomainComment(comment, parseTreeNode, CommentPlacement.OwnerLeft));

            decoratedComments.Add(comment);
        }

        private void AddCommentToRightOfAstValue(ParseTreeNode parseTreeNode, Token comment)
        {
            object astValue = GrammarHelper.AstNodeToValue(parseTreeNode.AstNode);

            Comments domainComments;

            if (!astValueToDomainComments.TryGetValue(astValue, out domainComments))
            {
                domainComments = new Comments();
                astValueToDomainComments.Add(astValue, domainComments);
            }

            domainComments.right.Add(CommentToDomainComment(comment, parseTreeNode, CommentPlacement.OwnerRight));

            decoratedComments.Add(comment);
        }

        private Comment CommentToDomainComment(Token comment, ParseTreeNode parseTreeNodeOwner, CommentPlacement placement)
        {
            CommentTerminal commentTerminal = (CommentTerminal)comment.Terminal;
            bool isDecorated;

            return new Comment(
                CommentTextToDomainCommentTextLines(comment, out isDecorated),
                CommentCategoryToDomainCommentCategory(comment.Category),
                placement,
                Math.Abs(comment.Location.Line - parseTreeNodeOwner.Span.Location.Line),
                GrammarHelper.GetCommentKind(commentTerminal, GetCommentCleaner(commentTerminal).NewLine),
                isDecorated
            );
        }

        private string[] CommentTextToDomainCommentTextLines(Token comment, out bool isDecorated)
        {
            CommentTerminal commentTerminal = (CommentTerminal)comment.Terminal;

            string startSymbol = commentTerminal.StartSymbol;
            string endSymbol = commentTerminal.EndSymbols.First(_endSymbol => comment.Text.EndsWith(_endSymbol));
            string text = comment.Text;

            text = text
                .Remove(text.Length - endSymbol.Length, endSymbol.Length)
                .Remove(0, startSymbol.Length);

            var commentCleaner = GetCommentCleaner(commentTerminal);
            string[] textLines = text.Split(new[] { commentCleaner.NewLine }, StringSplitOptions.None);
            textLines = commentCleaner.GetCleanedUpCommentTextLines(textLines, comment.Location.Column, commentTerminal, out isDecorated);

            return textLines;
        }

        private static CommentCategory CommentCategoryToDomainCommentCategory(TokenCategory category)
        {
            switch (category)
            {
                case TokenCategory.Outline:
                    return CommentCategory.Outline;

                case TokenCategory.Comment:
                    return CommentCategory.Comment;

                case TokenCategory.Directive:
                    return CommentCategory.Directive;

                default:
                    throw new ArgumentException("Invalid comment category: " + category, "category");
            }
        }

        private ICommentCleaner GetCommentCleaner(CommentTerminal commentTerminal)
        {
            return CommentCleaner ?? ((Sarcasm.GrammarAst.Grammar)commentTerminal.Grammar).GetDefaultCommentCleaner();
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

        public new Document<TRoot> GetDocument()
        {
            return new Document<TRoot>(RootAstValue, astValueToDomainComments);
        }
    }
}
