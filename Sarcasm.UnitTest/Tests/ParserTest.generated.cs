 

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
            ParseFileSaveAstAndCheck(exprParser, "Binary1.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Binary2()
        {
            ParseFileSaveAstAndCheck(exprParser, "Binary2.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Binary3()
        {
            ParseFileSaveAstAndCheck(exprParser, "Binary3.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Binary4()
        {
            ParseFileSaveAstAndCheck(exprParser, "Binary4.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_MiniPL()
        {
            ParseFileSaveAstAndCheck(parser, "MiniPL.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Unary1()
        {
            ParseFileSaveAstAndCheck(exprParser, "Unary1.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Unary2()
        {
            ParseFileSaveAstAndCheck(exprParser, "Unary2.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Unary3()
        {
            ParseFileSaveAstAndCheck(exprParser, "Unary3.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Unary4()
        {
            ParseFileSaveAstAndCheck(exprParser, "Unary4.expr");
        }

	}
}
