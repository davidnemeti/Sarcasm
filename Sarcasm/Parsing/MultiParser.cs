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
using Sarcasm.Utility;

using Grammar = Sarcasm.GrammarAst.Grammar;

namespace Sarcasm.Parsing
{
    public class MultiParser
    {
        protected const bool makeParsableEveryNonTerminalDefault = true;

        #region State

        private readonly Grammar grammar;
        private readonly LanguageData language;
        private readonly Parser mainParser;
        private IDictionary<NonTerminal, Parser> rootToParser = new Dictionary<NonTerminal, Parser>();    // mainParser is included as well
        public ICommentCleaner CommentCleaner { get; set; }
        public readonly AsyncLock Lock = new AsyncLock();

        #endregion

        #region Construction

        /// <summary>
        /// Create a multiparser which consists of multiple parsers, each of them can parse a specific nonterminal of the grammar.
        /// Nonterminals are specified by grammar.SnippetRoots, which may be extended regarding the value of <paramref name="makeParsableEveryNonTerminal"/>
        /// </summary>
        /// <param name="grammar">
        /// The grammar which will drive the multiparser.
        /// </param>
        /// <param name="makeParsableEveryNonTerminal">
        /// If true then gather all nonterminals from grammar (going down from root recursively), and add them to SnippetRoots if needed.
        /// If false then SnippetRoots will not be changed.
        /// </param>
        internal MultiParser(Grammar grammar, bool makeParsableEveryNonTerminal = makeParsableEveryNonTerminalDefault)
        {
            grammar.SetDecimalSeparatorOnNumberLiterals();

            // NOTE: grammar.SnippetRoots should be extended before creating LanguageData
            if (makeParsableEveryNonTerminal)
            {
                foreach (NonTerminal nonTerminal in GrammarHelper.GetDescendantBnfTermsExcludingSelf(grammar.Root).OfType<NonTerminal>())
                    if (!grammar.SnippetRoots.Contains(nonTerminal))
                        grammar.SnippetRoots.Add(nonTerminal);
            }

            this.grammar = grammar;
            this.language = new LanguageData(grammar);

            if (this.language.ErrorLevel >= GrammarErrorLevel.Conflict && grammar.ErrorHandling == ErrorHandling.ThrowException)
                GrammarHelper.ThrowGrammarErrorException(this.language.ErrorLevel, string.Join("\n", this.language.Errors));

            this.mainParser = new Parser(language);

            foreach (NonTerminal nonTerminal in grammar.SnippetRoots)
                this.rootToParser.Add(nonTerminal, new Parser(language, nonTerminal));
        }

        public static MultiParser Create(Grammar grammar, bool makeParsableEveryNonTerminal = makeParsableEveryNonTerminalDefault)
        {
            return new MultiParser(grammar, makeParsableEveryNonTerminal);
        }

        public static MultiParser<TMainRoot> Create<TMainRoot>(Grammar<TMainRoot> grammar, bool makeParsableEveryNonTerminal = makeParsableEveryNonTerminalDefault)
        {
            return new MultiParser<TMainRoot>(grammar, makeParsableEveryNonTerminal);
        }

        #endregion

        #region Parse

        public ParseTree Parse(string sourceText)
        {
            return Parse(mainParser, sourceText);
        }

        public ParseTree Parse(string sourceText, NonTerminal root)
        {
            return Parse(GetParser(root), sourceText);
        }

        public ParseTree Parse(string sourceText, string fileName)
        {
            return Parse(mainParser, sourceText, fileName);
        }

        public ParseTree Parse(string sourceText, string fileName, NonTerminal root)
        {
            return Parse(GetParser(root), sourceText, fileName);
        }

        public ParseTree ScanOnly(string sourceText, string fileName)
        {
            return ScanOnly(mainParser, sourceText, fileName);
        }

        public ParseTree ScanOnly(string sourceText, string fileName, NonTerminal root)
        {
            return ScanOnly(GetParser(root), sourceText, fileName);
        }

        public async Task<ParseTree> ParseAsync(string sourceText)
        {
            return await ParseAsync(mainParser, sourceText);
        }

        public async Task<ParseTree> ParseAsync(string sourceText, NonTerminal root)
        {
            return await ParseAsync(GetParser(root), sourceText);
        }

        public async Task<ParseTree> ParseAsync(string sourceText, string fileName)
        {
            return await ParseAsync(mainParser, sourceText, fileName);
        }

        public async Task<ParseTree> ParseAsync(string sourceText, string fileName, NonTerminal root)
        {
            return await ParseAsync(GetParser(root), sourceText, fileName);
        }

        public async Task<ParseTree> ScanOnlyAsync(string sourceText, string fileName)
        {
            return await ScanOnlyAsync(mainParser, sourceText, fileName);
        }

        public async Task<ParseTree> ScanOnlyAsync(string sourceText, string fileName, NonTerminal root)
        {
            return await ScanOnlyAsync(GetParser(root), sourceText, fileName);
        }

        #endregion

        #region Misc

        public GrammarErrorList GrammarErrors { get { return language.Errors; } }

        public GrammarErrorLevel GrammarErrorLevel { get { return language.ErrorLevel; } }

        public NonTerminal MainRoot { get { return grammar.Root; } }

        public Scanner GetMainScanner()
        {
            return mainParser.Scanner;
        }

        public Scanner GetScanner(NonTerminal root)
        {
            return GetParser(root).Scanner;
        }

        public ParsingContext GetMainContext()
        {
            return mainParser.Context;
        }

        public ParsingContext GetContext(NonTerminal root)
        {
            return GetParser(root).Context;
        }

        public IEnumerable<Parser> GetParsers()
        {
            return Util.Concat(mainParser, rootToParser.Values);
        }

        public Parser GetMainParser()
        {
            return mainParser;
        }

        #endregion

        /*
         * Parser.ReadInput() and Parser.RecoverFromError() methods are being used inside methods of ParserAction and its derived classes.
         * There we always have a ParsingContext and therefore the actual parser. Thus no need to define e.g. a ReadInput() and
         * ReadInput(NonTerminal root) methods, etc. in MultiParser.
         * */

        #region Helpers

        private Parser GetParser(NonTerminal root)
        {
            if (root == this.MainRoot)
                return mainParser;
            else
            {
                Parser parser;

                if (rootToParser.TryGetValue(root, out parser))
                    return parser;
                else
                {
                    parser = new Parser(language, root);
                    rootToParser.Add(root, parser);
                    return parser;
                }
            }
        }

        private ParseTree Parse(Parser parser, string sourceText)
        {
            return new ParseTree(parser.ParsePlus(sourceText))
                .SetCommentCleaner(this.CommentCleaner);
        }

        private ParseTree Parse(Parser parser, string sourceText, string fileName)
        {
            return new ParseTree(parser.ParsePlus(sourceText, fileName))
                .SetCommentCleaner(this.CommentCleaner);
        }

        private ParseTree ScanOnly(Parser parser, string sourceText, string fileName)
        {
            return new ParseTree(parser.ScanOnlyPlus(sourceText, fileName))
                .SetCommentCleaner(this.CommentCleaner);
        }

        private async Task<ParseTree> ParseAsync(Parser parser, string sourceText)
        {
            return new ParseTree(await parser.ParsePlusAsync(sourceText, this.Lock))
                .SetCommentCleaner(this.CommentCleaner);
        }

        private async Task<ParseTree> ParseAsync(Parser parser, string sourceText, string fileName)
        {
            return new ParseTree(await parser.ParsePlusAsync(sourceText, fileName, this.Lock))
                .SetCommentCleaner(this.CommentCleaner);
        }

        private async Task<ParseTree> ScanOnlyAsync(Parser parser, string sourceText, string fileName)
        {
            return new ParseTree(await parser.ScanOnlyPlusAsync(sourceText, fileName, this.Lock))
                .SetCommentCleaner(this.CommentCleaner);
        }

        #endregion
    }

    public class MultiParser<TMainRoot> : MultiParser
    {
        #region Construction

        /// <see cref="MultiParser.MultiParser(Grammar, bool)"/>
        internal MultiParser(Grammar<TMainRoot> grammar, bool makeParsableEveryNonTerminal = makeParsableEveryNonTerminalDefault)
            : base(grammar, makeParsableEveryNonTerminal)
        {
        }

        #endregion

        #region Parse

        public new ParseTree<TMainRoot> Parse(string sourceText)
        {
            return base.Parse(sourceText).ConvertToTypesafe<TMainRoot>();
        }

        public ParseTree<TRoot> Parse<TRoot>(string sourceText, INonTerminal<TRoot> root)
        {
            return base.Parse(sourceText, root.AsNonTerminal()).ConvertToTypesafe<TRoot>();
        }

        public new ParseTree<TMainRoot> Parse(string sourceText, string fileName)
        {
            return base.Parse(sourceText, fileName).ConvertToTypesafe<TMainRoot>();
        }

        public ParseTree<TRoot> Parse<TRoot>(string sourceText, string fileName, INonTerminal<TRoot> root)
        {
            return base.Parse(sourceText, fileName, root.AsNonTerminal()).ConvertToTypesafe<TRoot>();
        }

        public new ParseTree<TMainRoot> ScanOnly(string sourceText, string fileName)
        {
            return base.ScanOnly(sourceText, fileName).ConvertToTypesafe<TMainRoot>();
        }

        public ParseTree<TRoot> ScanOnly<TRoot>(string sourceText, string fileName, INonTerminal<TRoot> root)
        {
            return base.ScanOnly(sourceText, fileName, root.AsNonTerminal()).ConvertToTypesafe<TRoot>();
        }

        #endregion

        #region Misc

        public new INonTerminal<TMainRoot> MainRoot { get { return (INonTerminal<TMainRoot>)base.MainRoot; } }

        #endregion
    }

    public static class ParserExtensions
    {
        public static Irony.Parsing.ParseTree ParsePlus(this Parser parser, string sourceText)
        {
            return HandleParseErrors(() => parser.Parse(sourceText), sourceText);
        }

        public static Irony.Parsing.ParseTree ParsePlus(this Parser parser, string sourceText, string fileName)
        {
            return HandleParseErrors(() => parser.Parse(sourceText, fileName), sourceText);
        }

        public static Irony.Parsing.ParseTree ScanOnlyPlus(this Parser parser, string sourceText, string fileName)
        {
            return HandleParseErrors(() => parser.ScanOnly(sourceText, fileName), sourceText);
        }

        public static async Task<Irony.Parsing.ParseTree> ParsePlusAsync(this Parser parser, string sourceText, AsyncLock @lock = null)
        {
            return await Task.Run(async () => { using (await @lock.LockAsync()) return parser.ParsePlus(sourceText); });
        }

        public static async Task<Irony.Parsing.ParseTree> ParsePlusAsync(this Parser parser, string sourceText, string fileName, AsyncLock @lock = null)
        {
            return await Task.Run(async () => { using (await @lock.LockAsync()) return parser.ParsePlus(sourceText, fileName); });
        }

        public static async Task<Irony.Parsing.ParseTree> ScanOnlyPlusAsync(this Parser parser, string sourceText, string fileName, AsyncLock @lock = null)
        {
            return await Task.Run(async () => { using (await @lock.LockAsync()) return parser.ScanOnlyPlus(sourceText, fileName); });
        }

        private static Irony.Parsing.ParseTree HandleParseErrors(Func<Irony.Parsing.ParseTree> action, string sourceText)
        {
            try
            {
                return action();
            }
            catch (FatalParseException e)
            {
                return new Irony.Parsing.ParseTree(sourceText, "Source") { ParserMessages = { new LogMessage(ErrorLevel.Error, e.Location, e.Message, null) } };
            }
        }
    }
}
