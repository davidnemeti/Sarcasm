using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.GrammarAst;
using Sarcasm.Parsing;
using Sarcasm.Unparsing;

using MiniPL.DomainDefinitions;

using MiniPLG = MiniPL.Grammars;
using Grammar = Sarcasm.GrammarAst.Grammar;
using ParseTree = Sarcasm.Parsing.ParseTree;

namespace Sarcasm.UnitTest
{
    [TestClass]
    public abstract class CommonTest
    {
        static CommonTest()
        {
            ExceptionThrowerTraceListener.Register();
        }

        internal const string testFilesDir = @"Test files";
        protected const string expectedResultsDir = @"Expected results";
        protected const string actualResultsDir = @"Actual results";

        protected static MiniPLG.GrammarP grammar;
        protected static MultiParser<MiniPL.DomainDefinitions.Program> parser;

        protected static MiniPLG.GrammarP.BnfTerms B { get { return grammar.B; } }

        private static bool initializedGrammar = false;
        private static bool initializedParser = false;

        protected static void InitializeGrammar()
        {
            if (initializedGrammar)
                return;

            grammar = new MiniPLG.GrammarP();

            //Console.WriteLine(grammar.GetNonTerminalsAsText());
            ////Console.WriteLine();
            ////Console.WriteLine(grammar.GetNonTerminalsAsText(omitBoundMembers: true));

            Assert.IsNotNull(grammar.Root, "Root is null");

            initializedGrammar = true;
        }

        protected static void InitializeParser()
        {
            if (initializedParser)
                return;

            InitializeGrammar();

            if (Directory.Exists(actualResultsDir))
                Directory.Delete(actualResultsDir, recursive: true);

            Directory.CreateDirectory(actualResultsDir);

            parser = MultiParser.Create(grammar);

            Assert.IsTrue(parser.GrammarErrorLevel <= GrammarErrorLevel.Info, "Grammar error(s):\n{0}", string.Join("\n", parser.GrammarErrors));

            initializedParser = true;
        }

        protected static ParseTree ParseFileAndCheck(NonTerminal root, string parseFileName)
        {
            return ParseTextAndCheck(root, ConvertTabsToSpaces(File.ReadAllText(GetParseFilePath(parseFileName))), parseFileName);
        }

        protected static ParseTree<TRoot> ParseFileAndCheckTS<TRoot>(INonTerminal<TRoot> root, string parseFileName)
        {
            return ParseTextAndCheckTS(root, ConvertTabsToSpaces(File.ReadAllText(GetParseFilePath(parseFileName))), parseFileName);
        }

        protected static ParseTree<MiniPL.DomainDefinitions.Program> ParseFileAndCheck(string parseFileName)
        {
            return ParseTextAndCheck(ConvertTabsToSpaces(File.ReadAllText(GetParseFilePath(parseFileName))), parseFileName);
        }

        protected static ParseTree ParseTextAndCheck(NonTerminal root, string sourceText, string parseFileName = null)
        {
            ParseTree parseTree = parseFileName != null
                ? parser.Parse(sourceText, parseFileName, root)
                : parser.Parse(sourceText, root);

            CheckParseTree(parseTree);

            return parseTree;
        }

        protected static ParseTree<TRoot> ParseTextAndCheckTS<TRoot>(INonTerminal<TRoot> root, string sourceText, string parseFileName = null)
        {
            ParseTree<TRoot> parseTree = parseFileName != null
                ? parser.Parse(sourceText, parseFileName, root)
                : parser.Parse(sourceText, root);

            CheckParseTree(parseTree);

            return parseTree;
        }

        protected static ParseTree<MiniPL.DomainDefinitions.Program> ParseTextAndCheck(string sourceText, string parseFileName = null)
        {
            ParseTree<MiniPL.DomainDefinitions.Program> parseTree = parseFileName != null
                ? parser.Parse(sourceText, parseFileName)
                : parser.Parse(sourceText);

            CheckParseTree(parseTree);

            return parseTree;
        }

        private static void CheckParseTree(ParseTree parseTree)
        {
            Assert.IsNotNull(parseTree, "Parser error: parse tree is null");

            string errorMessages = string.Join("\n",
                parseTree.ParserMessages.Select(parserMessage => string.Format("{0} {1}: {2}", parseTree.FileName, parserMessage.Location.ToString(), parserMessage.Message))
                );

            Assert.IsFalse(parseTree.HasErrors(), "Parser errors:\n{0}", errorMessages);
            Assert.AreEqual(ParseTreeStatus.Parsed, parseTree.Status, "Parser errors:\n{0}", errorMessages);
        }

        protected static string GetParseFilePath(string parseFileName)
        {
            return Path.Combine(testFilesDir, parseFileName);
        }

        // Irony does not handle tabs properly, so we convert them: the "Ch" in VS matches parserMessage.Location.Column
        protected static string ConvertTabsToSpaces(string sourceText)
        {
            return sourceText.Replace("\t", " ");
        }
    }

    internal static class JsonExtensions
    {
        public static string ToJson(this object astValue)
        {
            var settings = new JsonSerializerSettings()
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.All,
                    Converters = { new StringEnumConverter() }
                };

            return JsonConvert.SerializeObject(astValue, settings);
        }
    }
}
