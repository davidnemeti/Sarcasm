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
        protected static Formatting formattingDefault { get { return CommonTest.grammar.UnparseControl.DefaultFormatting; } }
        protected static Formatting formatting2;
        protected static Formatting formatting3;

        [ClassInitialize]
        public static void InitializeUnparser(TestContext testContext)
        {
            CommonTest.InitializeParser();
            Directory.CreateDirectory(actualUnparsedFilesDir);
            unparser = new Unparser(grammar);

            formatting2 = new Formatting() { IndentEmptyLines = false };
            InitializeFormatting(formatting2);

            formatting3 = new Formatting() { IndentEmptyLines = true };
            InitializeFormatting(formatting3);
        }

        private static void InitializeFormatting(Formatting formatting)
        {
            formatting.InsertUtokensAround(B.DOT, UtokenInsert.NoWhitespace);
            formatting.InsertUtokensRightOf(B.LEFT_PAREN, UtokenInsert.NoWhitespace);
            formatting.InsertUtokensLeftOf(B.RIGHT_PAREN, UtokenInsert.NoWhitespace);
            formatting.InsertUtokensLeftOf(B.SEMICOLON, UtokenInsert.NoWhitespace);
            formatting.InsertUtokensLeftOf(B.COMMA, UtokenInsert.NoWhitespace);
            formatting.InsertUtokensRightOf(B.UnaryOperator, UtokenInsert.NoWhitespace);
            formatting.InsertUtokensBetweenOrdered(B.Name, B.LEFT_PAREN, UtokenInsert.NoWhitespace);
            formatting.InsertUtokensBetweenOrdered(B.NameRef, B.LEFT_PAREN, UtokenInsert.NoWhitespace);
            formatting.InsertUtokensBetweenOrdered(B.WRITE, B.LEFT_PAREN, UtokenInsert.NoWhitespace);
            formatting.InsertUtokensBetweenOrdered(B.WRITELN, B.LEFT_PAREN, UtokenInsert.NoWhitespace);

            formatting.InsertUtokensRightOf(B.Statement, UtokenInsert.NewLine);
            formatting.InsertUtokensLeftOf(B.Statement, UtokenInsert.NewLine, UtokenInsert.NewLine);
            formatting.SetBlockIndentationOn(B.Statement, BlockIndentation.Indent);
            formatting.InsertUtokensBetweenOrdered(B.ELSE, B.If, UtokenInsert.Space);
            formatting.SetBlockIndentationOn(B.ELSE, B.If, BlockIndentation.Unindent);
            formatting.InsertUtokensRightOf(new BnfTermPartialContext(B.Program, B.Name), UtokenInsert.EmptyLine);
            formatting.InsertUtokensRightOf(new BnfTermPartialContext(B.Program, B.NamespaceName), UtokenInsert.EmptyLine);
            formatting.InsertUtokensLeftOf(B.BEGIN, UtokenInsert.NewLine);
            formatting.InsertUtokensRightOf(B.BEGIN, UtokenInsert.NewLine);
            formatting.InsertUtokensLeftOf(B.END, UtokenInsert.NewLine);
            formatting.InsertUtokensRightOf(B.END, UtokenInsert.NewLine);
            formatting.InsertUtokensRightOf(new BnfTermPartialContext(B.Function, B.END), UtokenInsert.EmptyLine);
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
    }
}
