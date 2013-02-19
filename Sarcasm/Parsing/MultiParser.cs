using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.Ast;

using Grammar = Sarcasm.Ast.Grammar;

namespace Sarcasm.Parsing
{
    public class MultiParser
    {
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
        public MultiParser(Grammar grammar, bool makeParsableEveryNonTerminal = true)
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

        #endregion

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

        #region Parse

        public ParseTree Parse(string sourceText)
        {
            return mainParser.Parse(sourceText);
        }

        public ParseTree Parse(string sourceText, NonTerminal root)
        {
            return GetParser(root).Parse(sourceText);
        }

        public ParseTree Parse(string sourceText, string fileName)
        {
            return mainParser.Parse(sourceText, fileName);
        }

        public ParseTree Parse(string sourceText, string fileName, NonTerminal root)
        {
            return GetParser(root).Parse(sourceText, fileName);
        }

        public ParseTree ScanOnly(string sourceText, string fileName)
        {
            return mainParser.ScanOnly(sourceText, fileName);
        }

        public ParseTree ScanOnly(string sourceText, string fileName, NonTerminal root)
        {
            return GetParser(root).ScanOnly(sourceText, fileName);
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
}
