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
using System.Globalization;
using System.Linq;

using Irony.Parsing;
using Sarcasm.DomainCore;
using Sarcasm.GrammarAst;
using Sarcasm.Parsing;

using Grammar = Sarcasm.GrammarAst.Grammar;

namespace Sarcasm.Unparsing
{
    public interface ICommentFormatter : ICommentCleaner
    {
        string[] GetDecoratedCommentTextLines(UnparsableAst owner, Comment comment);
    }

    public class Formatter : ICommentFormatter
    {
        static Formatter()
        {
            CommentContent = new Discriminator("CommentContent");
            CommentStartSymbol = new Discriminator("CommentStartSymbol");
            CommentEndSymbol = new Discriminator("CommentEndSymbol");

            StringLiteralContent = new Discriminator("StringLiteralContent");
            StringLiteralStartSymbol = new Discriminator("StringLiteralStartSymbol");
            StringLiteralEndSymbol = new Discriminator("StringLiteralEndSymbol");

            DataLiteralContent = new Discriminator("DataLiteralContent");
            DsvLiteralTerminator = new Discriminator("DsvLiteralTerminator");
            QuotedValueLiteralStartSymbol = new Discriminator("QuotedValueLiteralStartSymbol");
            QuotedValueLiteralEndSymbol = new Discriminator("QuotedValueLiteralEndSymbol");

            NumberLiteralContent = new Discriminator("NumberLiteralContent");
            NumberLiteralBasePrefix = new Discriminator("NumberLiteralBasePrefix");
            NumberLiteralTypeModifierSuffix = new Discriminator("NumberLiteralTypeModifierSuffix");
        }

        #region Constants

        public static Discriminator CommentContent { get; private set; }
        public static Discriminator CommentStartSymbol { get; private set; }
        public static Discriminator CommentEndSymbol { get; private set; }

        public static Discriminator StringLiteralContent { get; private set; }
        public static Discriminator StringLiteralStartSymbol { get; private set; }
        public static Discriminator StringLiteralEndSymbol { get; private set; }

        public static Discriminator DataLiteralContent { get; private set; }
        public static Discriminator DsvLiteralTerminator { get; private set; }
        public static Discriminator QuotedValueLiteralStartSymbol { get; private set; }
        public static Discriminator QuotedValueLiteralEndSymbol { get; private set; }

        public static Discriminator NumberLiteralContent { get; private set; }
        public static Discriminator NumberLiteralBasePrefix { get; private set; }
        public static Discriminator NumberLiteralTypeModifierSuffix { get; private set; }

        #endregion

        #region Default values

        private static readonly string newLineDefault = Environment.NewLine;
        private const string spaceDefault = " ";
        private const string tabDefault = "\t";
        private static readonly string indentUnitDefault = string.Concat(Enumerable.Repeat(spaceDefault, 4));
        private const string whiteSpaceBetweenUtokensDefault = spaceDefault;
        private const bool indentEmptyLinesDefault = false;
        private const string multiLineCommentDecoratorDefault = "";
        private static readonly IFormatProvider formatProviderDefault = CultureInfo.InvariantCulture;

        #endregion

        #region State

        #region Settings

        public string NewLine { get; set; }
        public string Space { get; set; }
        public string Tab { get; set; }
        public string IndentUnit { get; set; }
        public string WhiteSpaceBetweenUtokens { get; set; }
        public bool IndentEmptyLines { get; set; }
        public string MultiLineCommentDecorator { get; set; }
        public IFormatProvider FormatProvider { get; private set; }

        public CultureInfo CultureInfo
        {
            get { return FormatProvider as CultureInfo; }

            set
            {
                FormatProvider = value;

                foreach (Parser parser in attachedParsers)
                    parser.Context.Culture = value;
            }
        }

        #endregion

        private List<Parser> attachedParsers;

        #endregion

        #region Construction

        public Formatter(Grammar grammar)
            : this(grammar.DefaultCulture)
        {
            this.attachedParsers = new List<Parser>();
        }

        public Formatter(Parser parserToAttach)
            : this(parserToAttach.Context.Culture)
        {
            this.attachedParsers = new List<Parser>();
            AttachParser(parserToAttach);
        }

        public Formatter(MultiParser multiParserToAttach)
            : this(multiParserToAttach.GetMainParser().Context.Culture)
        {
            this.attachedParsers = new List<Parser>();
            AttachParser(multiParserToAttach);
        }

        protected Formatter(IFormatProvider formatProvider)
        {
            this.FormatProvider = formatProvider;

            this.NewLine = newLineDefault;
            this.Space = spaceDefault;
            this.Tab = tabDefault;
            this.IndentUnit = indentUnitDefault;
            this.WhiteSpaceBetweenUtokens = whiteSpaceBetweenUtokensDefault;
            this.IndentEmptyLines = indentEmptyLinesDefault;
            this.MultiLineCommentDecorator = multiLineCommentDecoratorDefault;
        }

        #endregion

        #region Interface to grammar

        public void AttachParser(MultiParser multiParserToAttach)
        {
            foreach (Parser parser in multiParserToAttach.GetParsers())
                AttachParser(parser);
        }

        public void AttachParser(Parser parserToAttach)
        {
            attachedParsers.Add(parserToAttach);
        }

        #endregion

        #region Interface to unparser

        internal IDecoration GetDecoration(Utoken utoken)
        {
            UnparsableAst target = utoken is UtokenText
                ? ((UtokenText)utoken).Reference
                : null;

            return GetDecoration(utoken, target);
        }

        #endregion

        #region Overridables for derived formatter

        public virtual void GetUtokensAround(UnparsableAst target, out InsertedUtokens leftInsertedUtokens, out InsertedUtokens rightInsertedUtokens)
        {
            leftInsertedUtokens = InsertedUtokens.None;
            rightInsertedUtokens = InsertedUtokens.None;
        }

        public virtual InsertedUtokens GetUtokensBetween(UnparsableAst leftTerminalLeaveTarget, UnparsableAst rightTarget)
        {
            return InsertedUtokens.None;
        }

        public virtual InsertedUtokens GetUtokensBetweenCommentAndOwner(UnparsableAst owner, Comment comment, int commentIndex, int commentCount,
            IEnumerable<Comment> commentsBetweenThisAndOwner)
        {
            if (comment.LineIndexDistanceFromOwner > 0)
            {
                int newLinesCount;

                if (comment.Placement == CommentPlacement.OwnerLeft)
                {
                    newLinesCount = comment.LineIndexDistanceFromOwner - (comment.TextLines.Length - 1);

                    Comment nextComment = commentsBetweenThisAndOwner.FirstOrDefault();
                    if (nextComment != null)
                        newLinesCount -= nextComment.LineIndexDistanceFromOwner;
                }
                else
                {
                    newLinesCount = comment.LineIndexDistanceFromOwner;

                    Comment prevComment = commentsBetweenThisAndOwner.FirstOrDefault();
                    if (prevComment != null)
                        newLinesCount -= prevComment.LineIndexDistanceFromOwner + (prevComment.TextLines.Length - 1);
                }

                if (comment.Placement == CommentPlacement.OwnerLeft && comment.Kind == CommentKind.SingleLine)
                {
                    /*
                     * We have already yielded a NewLine in Unparser.UnparseComment (see comment there),
                     * thus we have to yield one less "formatting" NewLine here.
                     * */
                    newLinesCount--;
                }

                return new UtokenRepeat(UtokenWhitespace.NewLine(), newLinesCount);
            }
            else
                return UtokenWhitespace.Space();
        }

        public virtual string[] GetDecoratedCommentTextLines(UnparsableAst owner, Comment comment)
        {
            if (comment.TextLines.Length > 1)
            {
                return comment.TextLines
                    .Select((textLine, lineIndex) => lineIndex > 0 && comment.IsDecorated ? MultiLineCommentDecorator + textLine : textLine)
                    .ToArray();
            }
            else
                return comment.TextLines;
        }

        public virtual string[] GetCleanedUpCommentTextLines(string[] commentTextLines, int columnIndex, CommentTerminal commentTerminal, out bool isDecorated)
        {
            if (commentTextLines.Length > 1)
            {
                bool _isDecorated = false;

                commentTextLines = commentTextLines
                    .Select(
                        (commentTextLine, lineIndex) =>
                        {
                            string trimmedStartCommentTextLine = commentTextLine.TrimStart();
                            string trimmedStartMultiLineCommentDecorator = MultiLineCommentDecorator.TrimStart();
                            string trimmedMultiLineCommentDecorator = trimmedStartMultiLineCommentDecorator.TrimEnd();

                            if (trimmedStartCommentTextLine.StartsWith(trimmedStartMultiLineCommentDecorator))
                            {
                                _isDecorated = true;
                                return trimmedStartCommentTextLine.Remove(0, trimmedStartMultiLineCommentDecorator.Length);   // decorated
                            }
                            else if (trimmedStartCommentTextLine.StartsWith(trimmedMultiLineCommentDecorator))
                            {
                                _isDecorated = true;
                                return trimmedStartCommentTextLine.Remove(0, trimmedMultiLineCommentDecorator.Length);    // decorated with missing whitespaces at the right of decorator
                            }
                            else if (commentTextLine.Length >= columnIndex && commentTextLine.Take(columnIndex).All(ch => char.IsWhiteSpace(ch)))
                                return commentTextLine.Remove(0, columnIndex);    // undecorated -> remove indentation
                            else
                                return commentTextLine;   // undecorated -> could not remove indentation (the indentation of this line is smaller then the indentation of the start of the comment)
                        }
                    )
                    .ToArray();

                isDecorated = _isDecorated;
                return commentTextLines;
            }
            else
            {
                isDecorated = false;
                return commentTextLines;
            }
        }

        public virtual BlockIndentation GetBlockIndentation(UnparsableAst leftTerminalLeaveIfAny, UnparsableAst target)
        {
            return BlockIndentation.IndentNotNeeded;
        }

        public virtual IDecoration GetDecoration(Utoken utoken, UnparsableAst target)
        {
            return Decoration.None;
        }

        #endregion
    }
}
