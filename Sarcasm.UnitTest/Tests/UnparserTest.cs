using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.Ast;
using Sarcasm.Parsing;
using Sarcasm.Unparsing;

using MiniPL.DomainModel;

using Grammar = Sarcasm.Ast.Grammar;

namespace Sarcasm.UnitTest
{
    [TestClass]
    public partial class UnparserTest : CommonTest
    {
        protected const string category = "UnparserTest";

        protected readonly static string actualUnparsedFilesDir = Path.Combine(actualResultsDir, @"Unparsed files");

        protected static Unparser unparser;

        [ClassInitialize]
        public static void InitializeUnparser(TestContext testContext)
        {
            CommonTest.InitializeParser();
            Directory.CreateDirectory(actualUnparsedFilesDir);
            unparser = new Unparser(grammar);
        }

        protected void UnparseSaveUnparsedAndCheck(NonTerminal root, string parseFileName)
        {
            string sourceText = File.ReadAllText(GetParseFilePath(parseFileName));

            object astValue = ParseTextAndCheck(root, sourceText, parseFileName).RootAstValue;
            string unparsedText = unparser.Unparse(astValue, root).AsString(unparser);

            string actualUnparsedFilePath = Path.Combine(actualUnparsedFilesDir, parseFileName);
            File.WriteAllText(actualUnparsedFilePath, unparsedText);

            // NOTE: Assert.AreEqual handles format string incorrectly (.NET bug), that's why we use string.Format here
            Assert.AreEqual(expected: sourceText, actual: unparsedText, message: string.Format("Original and unparsed text differs for file: '{0}'", parseFileName));
        }
    }
}
