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
            if (makeParsableEveryNonTerminal)
            {
                // NOTE: grammar.SnippetRoots should be extended before creating LanguageData
                foreach (NonTerminal nonTerminal in GetDescendantNonTerminalsExcludingSelf(grammar.Root))
                {
                    if (!grammar.SnippetRoots.Contains(nonTerminal))
                        grammar.SnippetRoots.Add(nonTerminal);
                }
            }

            this.grammar = grammar;
            this.language = new LanguageData(grammar);

            if (!this.language.CanParse())
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
            return (ParseTree)mainParser.Parse(sourceText);
        }

        public ParseTree Parse(string sourceText, NonTerminal root)
        {
            return (ParseTree)GetParser(root).Parse(sourceText);
        }

        public ParseTree Parse(string sourceText, string fileName)
        {
            return (ParseTree)mainParser.Parse(sourceText, fileName);
        }

        public ParseTree Parse(string sourceText, string fileName, NonTerminal root)
        {
            return (ParseTree)GetParser(root).Parse(sourceText, fileName);
        }

        public ParseTree ScanOnly(string sourceText, string fileName)
        {
            return (ParseTree)mainParser.ScanOnly(sourceText, fileName);
        }

        public ParseTree ScanOnly(string sourceText, string fileName, NonTerminal root)
        {
            return (ParseTree)GetParser(root).ScanOnly(sourceText, fileName);
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

        private static IEnumerable<NonTerminal> GetDescendantNonTerminalsExcludingSelf(NonTerminal nonTerminal)
        {
            return GetDescendantNonTerminalsExcludingSelf(nonTerminal, new HashSet<NonTerminal>());
        }

        private static IEnumerable<NonTerminal> GetDescendantNonTerminalsExcludingSelf(NonTerminal nonTerminal, ISet<NonTerminal> visitedNonTerminals)
        {
            visitedNonTerminals.Add(nonTerminal);

            return nonTerminal.Rule.Data
                .SelectMany(children => children)
                .OfType<NonTerminal>()
                .Where(child => !visitedNonTerminals.Contains(child))
                .SelectMany(child => Util.Concat(child, GetDescendantNonTerminalsExcludingSelf(child, visitedNonTerminals)));
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
}
