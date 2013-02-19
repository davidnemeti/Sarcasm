using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    public partial class ParserTest : CommonTest
    {
        protected const string category = "ParserTest";

        protected readonly static string expectedAstDir = Path.Combine(expectedResultsDir, @"ASTs");
        protected readonly static string actualAstTreesDir = Path.Combine(actualResultsDir, @"ASTs");

        [ClassInitialize]
        public static void InitializeParser(TestContext testContext)
        {
            CommonTest.InitializeParser();

            Directory.CreateDirectory(actualAstTreesDir);
        }

        protected void ParseFileSaveAstAndCheck(NonTerminal root, string parseFileName)
        {
            string actualAstContent = ParseFileAndCheck(root, parseFileName).Root.AstNode.ToJson();

            string astFileName = Path.GetFileNameWithoutExtension(parseFileName) + ".json";

            string actualAstPath = Path.Combine(actualAstTreesDir, astFileName);
            File.WriteAllText(actualAstPath, actualAstContent);

            string expectedAstPath = Path.Combine(expectedAstDir, astFileName);
            string expectedAstContent = File.ReadAllText(expectedAstPath);

            // NOTE: Assert.AreEqual handles format string incorrectly (.NET bug), that's why we use string.Format here
            Assert.AreEqual(expectedAstContent, actualAstContent, string.Format("Expected and actual parsed tree differs for file: '{0}'", parseFileName));
        }

        protected string ParseFileToAstAndCheck(NonTerminal root, string parseFileName)
        {
            return ParseFileAndCheck(root, parseFileName).Root.AstNode.ToJson();
        }

        protected string ParseTextToAstAndCheck(NonTerminal root, string sourceText, string parseFileName = null)
        {
            return ParseTextAndCheck(root, sourceText, parseFileName).Root.AstNode.ToJson();
        }
    }
}
