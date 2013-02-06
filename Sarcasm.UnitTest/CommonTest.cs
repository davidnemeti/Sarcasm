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
using Sarcasm.Ast;
using Sarcasm.Unparsing;

using MiniPL.DomainModel;

using Grammar = Sarcasm.Ast.Grammar;

namespace Sarcasm.UnitTest
{
    [TestClass]
    public abstract class CommonTest
    {
        internal const string testFilesDir = @"Test files";
        protected const string expectedResultsDir = @"Expected results";
        protected const string actualResultsDir = @"Actual results";

        protected static MiniPL.GrammarP grammar;
        protected static Parser parser;
        protected static Parser exprParser;

        protected static void Initialize()
        {
            Directory.Delete(actualResultsDir, recursive: true);
            Directory.CreateDirectory(actualResultsDir);

            grammar = new MiniPL.GrammarP();

            //Console.WriteLine(grammar.GetNonTerminalsAsText());
            ////Console.WriteLine();
            ////Console.WriteLine(grammar.GetNonTerminalsAsText(omitBoundMembers: true));

            Assert.IsNotNull(grammar.Root, "Root is null");

            parser = new Parser(grammar);
            exprParser = new Parser(parser.Language, grammar.B.Expression);

            Assert.IsTrue(parser.Language.ErrorLevel <= GrammarErrorLevel.Info, "Grammar error:\n{0}", string.Join("\n", parser.Language.Errors));
        }

        protected static ParseTree ParseFileAndCheck(Parser parser, string parseFileName)
        {
            return ParseTextAndCheck(parser, ConvertTabsToSpaces(File.ReadAllText(GetParseFilePath(parseFileName))), parseFileName);
        }

        protected static ParseTree ParseTextAndCheck(Parser parser, string sourceText, string parseFileName = null)
        {
            ParseTree parseTree = parseFileName != null
                ? parser.Parse(sourceText, parseFileName)
                : parser.Parse(sourceText);

            Assert.IsNotNull(parseTree, "Parser error: parse tree is null");

            Assert.AreEqual(ParseTreeStatus.Parsed, parseTree.Status, "Parser error:\n{0}",
                string.Join("\n",
                    parseTree.ParserMessages.Select(parserMessage => string.Format("{0} {1}: {2}", parseTree.FileName, parserMessage.Location.ToString(), parserMessage.Message))
                    )
                );

            return parseTree;
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
        public static string ToJson(this object astNode)
        {
            object value = GrammarHelper.AstNodeToValue(astNode);

            var settings = new JsonSerializerSettings()
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.All,
                };
            settings.Converters.Add(new StringEnumConverter());

            return JsonConvert.SerializeObject(value, settings);
        }
    }
}
