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

using Irony.Parsing;
using Sarcasm.GrammarAst;
using Sarcasm.Unparsing;

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
            object astRootValue = ParseTextAndCheck(root, sourceText, parseFileName).RootAstValue;
            ReunparseCheck(root, astRootValue, sourceText, parseFileName, leftToRight);
        }

        protected void ReunparseCheckTS<TRoot>(INonTerminal<TRoot> root, string parseFileName, bool leftToRight)
        {
            string sourceText = File.ReadAllText(GetParseFilePath(parseFileName));
            TRoot astRootValue = ParseTextAndCheckTS(root, sourceText, parseFileName).RootAstValue;
            ReunparseCheck(root.AsNonTerminal(), astRootValue, sourceText, parseFileName, leftToRight);
        }

        private void ReunparseCheck<TRoot>(NonTerminal root, TRoot astRootValue, string originalSourceText, string parseFileName, bool leftToRight)
        {
            var utokens = unparser.Unparse(astRootValue, root, leftToRight ? Unparser.Direction.LeftToRight : Unparser.Direction.RightToLeft);

            if (!leftToRight)
                utokens = utokens.Reverse();

            string unparsedSourceText = utokens.AsText(unparser);

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

            public override void GetUtokensAround(UnparsableAst target, out InsertedUtokens leftInsertedUtokens, out InsertedUtokens rightInsertedUtokens)
            {
                base.GetUtokensAround(target, out leftInsertedUtokens, out rightInsertedUtokens);

                if (target.BnfTerm == B.Statement)
                    leftInsertedUtokens = new[] { UtokenInsert.NewLine(), UtokenInsert.NewLine() };
            }
        }
    }
}
