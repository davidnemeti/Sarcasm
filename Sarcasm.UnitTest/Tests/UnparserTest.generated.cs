 

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
            ReunparseCheckTS(B.Expression, "Binary1.expr");
            ReunparseCheck(B.Expression, "Binary1.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary2()
        {
            ReunparseCheckTS(B.Expression, "Binary2.expr");
            ReunparseCheck(B.Expression, "Binary2.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary3()
        {
            ReunparseCheckTS(B.Expression, "Binary3.expr");
            ReunparseCheck(B.Expression, "Binary3.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary4()
        {
            ReunparseCheckTS(B.Expression, "Binary4.expr");
            ReunparseCheck(B.Expression, "Binary4.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL()
        {
            ReunparseCheckTS(B.Program, "MiniPL.mplp");
            ReunparseCheck(B.Program, "MiniPL.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary1()
        {
            ReunparseCheckTS(B.Expression, "Unary1.expr");
            ReunparseCheck(B.Expression, "Unary1.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary2()
        {
            ReunparseCheckTS(B.Expression, "Unary2.expr");
            ReunparseCheck(B.Expression, "Unary2.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary3()
        {
            ReunparseCheckTS(B.Expression, "Unary3.expr");
            ReunparseCheck(B.Expression, "Unary3.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary4()
        {
            ReunparseCheckTS(B.Expression, "Unary4.expr");
            ReunparseCheck(B.Expression, "Unary4.expr");
        }

	}
}
