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
using System.Collections;
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
using Sarcasm.Utility;

namespace Sarcasm.Parsing
{
    public class ParseTree
    {
        private readonly Irony.Parsing.ParseTree parseTree;

        private Dictionary<object, ParseTreeNode> astValueToParseTreeNode;
        private HashSet<Token> decoratedComments;
        private Dictionary<ParseTreeNode, ParseTreeNode> parseTreeNodeToParent;
        private ParseTreeNode prevLine;
        private ParseTreeNode currentLine;

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

        public ParseTree SetCommentCleaner(ICommentCleaner commentCleaner)
        {
            this.CommentCleaner = commentCleaner;
            return this;
        }

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

        public ParseTreeNode GetParent(ParseTreeNode parseTreeNode)
        {
            if (parseTreeNodeToParent == null)
                BuildAstValueToParseTreeNode();

            return parseTreeNodeToParent[parseTreeNode];
        }

        public Document GetDocument()
        {
            return Root != null ? new Document(RootAstValue, astValueToDomainComments) : null;
        }

        private void BuildAstValueToParseTreeNode()
        {
            astValueToParseTreeNode = new Dictionary<object, ParseTreeNode>();
            parseTreeNodeToParent = new Dictionary<ParseTreeNode, ParseTreeNode>() { { parseTree.Root, null } };
            astValueToDomainComments = new Dictionary<object, Comments>();
            decoratedComments = new HashSet<Token>();
            prevLine = null;
            currentLine = null;

            if (Root != null)
            {
                StoreAstValueToParseTreeNodeRecursive(parseTree.Root, nodeIndex: 0);

                // decorate comments at the end (Irony does not put these comments into the parse tree)
                var commentsAtTheEnd = Tokens
                    .ReverseOptimized()
                    .SkipWhile(token => token.Terminal == token.Terminal.Grammar.Eof)
                    .TakeWhile(token => token.Terminal is CommentTerminal);

                var lastNonCommentToken = Tokens
                    .ReverseOptimized()
                    .First(token => token.Terminal != token.Terminal.Grammar.Eof && !(token.Terminal is CommentTerminal));

                foreach (Token commentAtTheEnd in commentsAtTheEnd.Reverse())
                    AddCommentToRightOfAstValue(parseTree.Root, commentAtTheEnd, lineIndexDistanceFromOwnerExplicit: Math.Abs(commentAtTheEnd.Location.Line - lastNonCommentToken.Location.Line));
            }
        }

        private void StoreAstValueToParseTreeNodeRecursive(ParseTreeNode currentNode, int nodeIndex)
        {
            object astValue = GrammarHelper.AstNodeToValue(currentNode.AstNode);

            if (astValue != null && astValue.GetType().IsClass && !astValueToParseTreeNode.ContainsKey(astValue))
                astValueToParseTreeNode.Add(astValue, currentNode);

            foreach (var parseTreeChild in currentNode.ChildNodes.Select((parseTreeChild, childIndex) => new { Value = parseTreeChild, Index = childIndex }))
            {
                parseTreeNodeToParent.Add(parseTreeChild.Value, currentNode);
                StoreAstValueToParseTreeNodeRecursive(parseTreeChild.Value, parseTreeChild.Index);
            }

            if (currentNode.Comments.Count > 0)
            {
                bool isTextInLineAtLeftOfFirstComment = IsTextInLineAtLeftOfComment(currentNode.Comments[0]);
                int lineIndexForFirstComment = currentNode.Comments[0].Location.Line;

                foreach (Token comment in currentNode.Comments)
                {
                    if (decoratedComments.Contains(comment))
                        continue;   // we have already decorated this comment

                    ParseTreeNode parentNode = GetParent(currentNode);

                    bool isCommentInLineOfFirstComment = comment.Location.Line == lineIndexForFirstComment;
                    bool isTextInLineAtLeftOfComment = isTextInLineAtLeftOfFirstComment && isCommentInLineOfFirstComment;

                    if (!isTextInLineAtLeftOfComment && parentNode != null && parentNode.Comments.Contains(comment) &&
                        parentNode.Span.Location.Line == currentNode.Span.Location.Line && !(GrammarHelper.AstNodeToValue(parentNode.AstNode) is IEnumerable))
                    {
                        /*
                         * Try to go up so we decorate the comment on the largest "subparsetree", but stop before lists
                         * */
                        continue;
                    }

                    if (isTextInLineAtLeftOfComment && comment.Location.Line == currentLine.Span.Location.Line && currentLine.AstNode != null)
                    {
                        /*
                         * The comment belongs to the previous node (they are in the same line).
                         * 
                         * Now we have one of the followings:
                         * 
                         * code_prev (*comment*) code_this
                         * */

                        AddCommentToRightOfAstValue(currentLine, comment);
                    }
                    else if (isTextInLineAtLeftOfComment && comment.Location.Line == prevLine.Span.Location.Line && prevLine.AstNode != null)
                    {
                        /*
                         * The comment belongs to the previous node (they are in the same line).
                         * 
                         * code_prev (*comment*)
                         * code_this
                         * */

                        AddCommentToRightOfAstValue(prevLine, comment);
                    }
                    else if (currentNode.AstNode != null)
                        AddCommentToLeftOfAstValue(currentNode, comment);         // the comment belongs to this node

                    else if (nodeIndex > 0)
                    {
                        // could not decorate comment, because we have no ast (if nodeIndex == 0 then Irony has already copied it onto the parent)

                        Debug.Assert(parentNode != null);

                        if (currentLine != null)
                            AddCommentToRightOfAstValue(currentLine, comment);     // decorate the comment onto currentLine
                        else
                            parentNode.Comments.Add(comment);   // copy the comment onto the parent and handle it there
                    }
                }
            }

            if (currentLine == null || currentNode.Term is Terminal || currentNode.Span.Location.Line == currentLine.Span.Location.Line)
            {
                if (currentLine != null && currentNode.Span.Location.Line > currentLine.Span.Location.Line)
                    prevLine = currentLine;

                currentLine = currentNode;
            }
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

        private void AddCommentToLeftOfAstValue(ParseTreeNode owner, Token comment, int? lineIndexDistanceFromOwnerExplicit = null)
        {
            AddCommentToSideOfAstValue(owner, comment, CommentPlacement.OwnerLeft, lineIndexDistanceFromOwnerExplicit);
        }

        private void AddCommentToRightOfAstValue(ParseTreeNode owner, Token comment, int? lineIndexDistanceFromOwnerExplicit = null)
        {
            AddCommentToSideOfAstValue(owner, comment, CommentPlacement.OwnerRight, lineIndexDistanceFromOwnerExplicit);
        }

        private void AddCommentToSideOfAstValue(ParseTreeNode owner, Token comment, CommentPlacement commentPlacement, int? lineIndexDistanceFromOwnerExplicit)
        {
            // first we calculate the lineIndexDistanceFromOwner, then we find a proper node with a non-null astNode to decorate (it can be in another line)
            int lineIndexDistanceFromOwner = lineIndexDistanceFromOwnerExplicit ?? Math.Abs(comment.Location.Line - owner.Span.Location.Line);
            owner = Util.Recurse(owner, GetParent).First(_parseTreeNode => _parseTreeNode.AstNode != null);
            object astValue = GrammarHelper.AstNodeToValue(owner.AstNode);

            Comments domainComments;

            if (!astValueToDomainComments.TryGetValue(astValue, out domainComments))
            {
                domainComments = new Comments();
                astValueToDomainComments.Add(astValue, domainComments);
            }

            var commentList = commentPlacement == CommentPlacement.OwnerLeft
                ? domainComments.left
                : domainComments.right;

            commentList.Add(CommentToDomainComment(comment, owner, commentPlacement, lineIndexDistanceFromOwner));

            decoratedComments.Add(comment);
        }

        private Comment CommentToDomainComment(Token comment, ParseTreeNode parseTreeNodeOwner, CommentPlacement placement, int lineIndexDistanceFromOwner)
        {
            CommentTerminal commentTerminal = (CommentTerminal)comment.Terminal;
            bool isDecorated;

            return new Comment(
                CommentTextToDomainCommentTextLines(comment, out isDecorated),
                CommentCategoryToDomainCommentCategory(comment.Category),
                placement,
                lineIndexDistanceFromOwner,
                GrammarHelper.GetCommentKind(commentTerminal, GetCommentCleaner(commentTerminal).NewLine),
                isDecorated
            );
        }

        private string[] CommentTextToDomainCommentTextLines(Token comment, out bool isDecorated)
        {
            CommentTerminal commentTerminal = (CommentTerminal)comment.Terminal;

            CommentKind commentKind = GrammarHelper.GetCommentKind(commentTerminal, GetCommentCleaner(commentTerminal).NewLine);

            string startSymbol = commentTerminal.StartSymbol;

            string endSymbol = commentKind == CommentKind.Delimited
                ? commentTerminal.EndSymbols.First(_endSymbol => comment.Text.EndsWith(_endSymbol))
                : string.Empty;

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
            return Root != null ? new Document<TRoot>(RootAstValue, astValueToDomainComments) : null;
        }
    }
}
