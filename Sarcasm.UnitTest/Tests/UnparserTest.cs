using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.GrammarAst;
using Sarcasm.Parsing;
using Sarcasm.Unparsing;

using MiniPL.DomainModel;

using Grammar = Sarcasm.GrammarAst.Grammar;

namespace Sarcasm.UnitTest
{
    [TestClass]
    public partial class UnparserTest : CommonTest
    {
        protected const string category = "UnparserTest";

        protected readonly static string actualUnparsedFilesDir = Path.Combine(actualResultsDir, @"Unparsed files");

        protected static Unparser unparser;
        protected static Formatter formatterDefault { get { return CommonTest.grammar.UnparseControl.DefaultFormatter; } }
        protected static Formatter formatter2;
        protected static Formatter formatter3;

        [ClassInitialize]
        public static void InitializeUnparser(TestContext testContext)
        {
            CommonTest.InitializeParser();
            Directory.CreateDirectory(actualUnparsedFilesDir);
            unparser = new Unparser(grammar);

            formatter2 = new SpecialFormatter(grammar.B) { IndentEmptyLines = false };
            formatter3 = new SpecialFormatter(grammar.B) { IndentEmptyLines = true };

            grammar.UnparseControl.ClearPrecedenceBasedParenthesesForExpressions();     // we want to test the automatic parentheses stuff
        }

        protected void ReunparseCheck(NonTerminal root, string parseFileName, bool leftToRight)
        {
            string sourceText = File.ReadAllText(GetParseFilePath(parseFileName));
            object astValue = ParseTextAndCheck(root, sourceText, parseFileName).RootAstValue;
            ReunparseCheck(root, astValue, sourceText, parseFileName, leftToRight);
        }

        protected void ReunparseCheckTS<TRoot>(INonTerminal<TRoot> root, string parseFileName, bool leftToRight)
        {
            string sourceText = File.ReadAllText(GetParseFilePath(parseFileName));
            object astValue = ParseTextAndCheckTS(root, sourceText, parseFileName).RootAstValue;
            ReunparseCheck(root.AsNonTerminal(), astValue, sourceText, parseFileName, leftToRight);
        }

        private void ReunparseCheck(NonTerminal root, object astValue, string originalSourceText, string parseFileName, bool leftToRight)
        {
            var utokens = unparser.Unparse(astValue, root, leftToRight ? Unparser.Direction.LeftToRight : Unparser.Direction.RightToLeft);

            if (!leftToRight)
                utokens = utokens.Reverse();

            string unparsedSourceText = utokens.AsText(unparser);

            string actualUnparsedFilePath = Path.Combine(actualUnparsedFilesDir, parseFileName);
            File.WriteAllText(actualUnparsedFilePath, unparsedSourceText);

            // NOTE: Assert.AreEqual handles format string incorrectly (.NET bug), that's why we use string.Format here
            Assert.AreEqual(expected: originalSourceText, actual: unparsedSourceText, message: string.Format("Original and unparsed text differs for file: '{0}'", parseFileName));
        }

        private class SpecialFormatter : MiniPL.Grammars.GrammarP.Formatter
        {
            public SpecialFormatter(MiniPL.Grammars.GrammarP.BnfTerms b)
                : base(b)
            {
            }

            protected override InsertedUtokens GetUtokensLeft(UnparsableAst target)
            {
                if (target.BnfTerm == B.Statement)
                    return new[] { UtokenInsert.NewLine, UtokenInsert.NewLine };
                else
                    return base.GetUtokensLeft(target);
            }
        }
    }
}
