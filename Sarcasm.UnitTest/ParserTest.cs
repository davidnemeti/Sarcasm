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
        protected readonly static string expectedParseTreesDir = Path.Combine(expectedResultsDir, @"Parse trees");
        protected readonly static string actualParseTreesDir = Path.Combine(actualResultsDir, @"Parse trees");

        [ClassInitialize]
        public static void InitializeParser(TestContext testContext)
        {
            CommonTest.Initialize();

            Directory.CreateDirectory(actualParseTreesDir);
        }

        protected void ParseFileSaveXmlAndCheck(Parser parser, string parseFileName)
        {
            string actualXmlContent = ParseFileAndCheck(parser, parseFileName).ToXml();

            string xmlFileName = Path.GetFileNameWithoutExtension(parseFileName) + ".xml";

            string actualXmlPath = Path.Combine(actualParseTreesDir, xmlFileName);
            File.WriteAllText(actualXmlPath, actualXmlContent);

            string expectedXmlPath = Path.Combine(expectedParseTreesDir, xmlFileName);
            string expectedXmlContent = File.ReadAllText(expectedXmlPath);

            Assert.AreEqual(expectedXmlContent, actualXmlContent, "Expected and actual parsed tree differs for file: '{0}'", parseFileName);
        }

        protected string ParseFileToXmlAndCheck(Parser parser, string parseFileName)
        {
            return ParseFileAndCheck(parser, parseFileName).ToXml();
        }

        protected string ParseTextToXmlAndCheck(Parser parser, string sourceText, string parseFileName = null)
        {
            return ParseTextAndCheck(parser, sourceText, parseFileName).ToXml();
        }
    }
}
