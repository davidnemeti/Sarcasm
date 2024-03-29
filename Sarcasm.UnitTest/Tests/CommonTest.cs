﻿#region License
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

extern alias globalMiniPL;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Irony.Parsing;
using Sarcasm.GrammarAst;
using Sarcasm.Parsing;

using globalMiniPL::MiniPL.DomainDefinitions;

using MiniPLG = globalMiniPL::MiniPL.Grammars;
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
        protected static MultiParser<Program> parser;

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

        protected static ParseTree<Program> ParseFileAndCheck(string parseFileName)
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

        protected static ParseTree<Program> ParseTextAndCheck(string sourceText, string parseFileName = null)
        {
            ParseTree<Program> parseTree = parseFileName != null
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
