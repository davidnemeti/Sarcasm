#region License
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

using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.GrammarAst;
using Sarcasm.Unparsing;

using globalMiniPL::MiniPL.DomainDefinitions;

using Grammar = Sarcasm.GrammarAst.Grammar;

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
            string actualAstContent = ParseFileAndCheck(root, parseFileName).RootAstValue.ToJson();
            SaveAstAndCheck(actualAstContent, parseFileName);
        }

        protected void ParseFileSaveAstAndCheckTS<TRoot>(INonTerminal<TRoot> root, string parseFileName)
        {
            string actualAstContent = ParseFileAndCheckTS(root, parseFileName).RootAstValue.ToJson();
            SaveAstAndCheck(actualAstContent, parseFileName);
        }

        protected void ParseFileSaveAstAndCheck(string parseFileName)
        {
            string actualAstContent = ParseFileAndCheck(parseFileName).RootAstValue.ToJson();
            SaveAstAndCheck(actualAstContent, parseFileName);
        }

        private void SaveAstAndCheck(string actualAstContent, string parseFileName)
        {
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
            return ParseFileAndCheck(root, parseFileName).RootAstValue.ToJson();
        }

        protected string ParseFileToAstAndCheckTS<TRoot>(INonTerminal<TRoot> root, string parseFileName)
        {
            return ParseFileAndCheckTS(root, parseFileName).RootAstValue.ToJson();
        }

        protected string ParseTextToAstAndCheck(NonTerminal root, string sourceText, string parseFileName = null)
        {
            return ParseTextAndCheck(root, sourceText, parseFileName).RootAstValue.ToJson();
        }

        protected string ParseTextToAstAndCheckTS<TRoot>(INonTerminal<TRoot> root, string sourceText, string parseFileName = null)
        {
            return ParseTextAndCheckTS(root, sourceText, parseFileName).RootAstValue.ToJson();
        }
    }
}
