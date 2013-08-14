﻿extern alias globalMiniPL;

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

using globalMiniPL::MiniPL.DomainDefinitions;

using Grammar = Sarcasm.GrammarAst.Grammar;
using Sarcasm.DomainCore;

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

            formatter2 = new SpecialFormatter(grammar) { IndentEmptyLines = false };
            formatter3 = new SpecialFormatter(grammar) { IndentEmptyLines = true };

            grammar.UnparseControl.ClearPrecedenceBasedParenthesesForExpressions();     // we want to test the automatic parentheses stuff
        }

        protected void ReunparseCheck(NonTerminal root, string parseFileName, bool leftToRight)
        {
            string sourceText = File.ReadAllText(GetParseFilePath(parseFileName));
            Document document = ParseTextAndCheck(root, sourceText, parseFileName).GetDocument();
            ReunparseCheck(root, document, sourceText, parseFileName, leftToRight);
        }

        protected void ReunparseCheckTS<TRoot>(INonTerminal<TRoot> root, string parseFileName, bool leftToRight)
        {
            string sourceText = File.ReadAllText(GetParseFilePath(parseFileName));
            Document document = ParseTextAndCheckTS(root, sourceText, parseFileName).GetDocument();
            ReunparseCheck(root.AsNonTerminal(), document, sourceText, parseFileName, leftToRight);
        }

        private void ReunparseCheck(NonTerminal root, Document document, string originalSourceText, string parseFileName, bool leftToRight)
        {
            var utokens = unparser.Unparse(document, root, leftToRight ? Unparser.Direction.LeftToRight : Unparser.Direction.RightToLeft);

            if (!leftToRight)
                utokens = utokens.Reverse();

            string unparsedSourceText = utokens.AsText();

            string actualUnparsedFilePath = Path.Combine(actualUnparsedFilesDir, parseFileName);
            File.WriteAllText(actualUnparsedFilePath, unparsedSourceText);

            // NOTE: Assert.AreEqual handles format string incorrectly (.NET bug), that's why we use string.Format here
            Assert.AreEqual(expected: originalSourceText, actual: unparsedSourceText, message: string.Format("Original and unparsed text differs for file: '{0}'", parseFileName));
        }

        private class SpecialFormatter : globalMiniPL::MiniPL.Grammars.GrammarP.Formatter
        {
            public SpecialFormatter(globalMiniPL::MiniPL.Grammars.GrammarP grammar)
                : base(grammar)
            {
            }

            protected override void GetUtokensAround(UnparsableAst target, out InsertedUtokens leftInsertedUtokens, out InsertedUtokens rightInsertedUtokens)
            {
                base.GetUtokensAround(target, out leftInsertedUtokens, out rightInsertedUtokens);

                if (target.BnfTerm == B.Statement)
                    leftInsertedUtokens = new[] { UtokenInsert.NewLine(), UtokenInsert.NewLine() };
            }
        }
    }
}
