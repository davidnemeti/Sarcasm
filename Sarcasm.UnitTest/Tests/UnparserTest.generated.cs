 

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
    public partial class UnparserTest
    {
        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary1()
        {
            UnparseSaveUnparsedAndCheck(exprParser, B.Expression, "Binary1.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary2()
        {
            UnparseSaveUnparsedAndCheck(exprParser, B.Expression, "Binary2.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary3()
        {
            UnparseSaveUnparsedAndCheck(exprParser, B.Expression, "Binary3.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary4()
        {
            UnparseSaveUnparsedAndCheck(exprParser, B.Expression, "Binary4.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL()
        {
            UnparseSaveUnparsedAndCheck(parser, B.Program, "MiniPL.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary1()
        {
            UnparseSaveUnparsedAndCheck(exprParser, B.Expression, "Unary1.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary2()
        {
            UnparseSaveUnparsedAndCheck(exprParser, B.Expression, "Unary2.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary3()
        {
            UnparseSaveUnparsedAndCheck(exprParser, B.Expression, "Unary3.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary4()
        {
            UnparseSaveUnparsedAndCheck(exprParser, B.Expression, "Unary4.expr");
        }

	}
}
