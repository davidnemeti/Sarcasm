 

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
    public partial class ParserTest
    {
        [TestMethod]
        [TestCategory(category)]
        public void Parse_Binary1()
        {
            ParseFileSaveAstAndCheckTS(B.Expression, "Binary1.expr");
            ParseFileSaveAstAndCheck(B.Expression, "Binary1.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Binary2()
        {
            ParseFileSaveAstAndCheckTS(B.Expression, "Binary2.expr");
            ParseFileSaveAstAndCheck(B.Expression, "Binary2.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Binary3()
        {
            ParseFileSaveAstAndCheckTS(B.Expression, "Binary3.expr");
            ParseFileSaveAstAndCheck(B.Expression, "Binary3.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Binary4()
        {
            ParseFileSaveAstAndCheckTS(B.Expression, "Binary4.expr");
            ParseFileSaveAstAndCheck(B.Expression, "Binary4.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_MiniPL()
        {
            ParseFileSaveAstAndCheckTS(B.Program, "MiniPL.mplp");
            ParseFileSaveAstAndCheck(B.Program, "MiniPL.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Unary1()
        {
            ParseFileSaveAstAndCheckTS(B.Expression, "Unary1.expr");
            ParseFileSaveAstAndCheck(B.Expression, "Unary1.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Unary2()
        {
            ParseFileSaveAstAndCheckTS(B.Expression, "Unary2.expr");
            ParseFileSaveAstAndCheck(B.Expression, "Unary2.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Unary3()
        {
            ParseFileSaveAstAndCheckTS(B.Expression, "Unary3.expr");
            ParseFileSaveAstAndCheck(B.Expression, "Unary3.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Unary4()
        {
            ParseFileSaveAstAndCheckTS(B.Expression, "Unary4.expr");
            ParseFileSaveAstAndCheck(B.Expression, "Unary4.expr");
        }

	}
}
