 

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
        public void Unparse_Binary1_TS()
        {
            ReunparseCheckTS(B.Expression, "Binary1.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary1()
        {
            ReunparseCheck(B.Expression, "Binary1.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary2_TS()
        {
            ReunparseCheckTS(B.Expression, "Binary2.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary2()
        {
            ReunparseCheck(B.Expression, "Binary2.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary3_TS()
        {
            ReunparseCheckTS(B.Expression, "Binary3.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary3()
        {
            ReunparseCheck(B.Expression, "Binary3.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary4_TS()
        {
            ReunparseCheckTS(B.Expression, "Binary4.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary4()
        {
            ReunparseCheck(B.Expression, "Binary4.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL_TS()
        {
            ReunparseCheckTS(B.Program, "MiniPL.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL()
        {
            ReunparseCheck(B.Program, "MiniPL.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary1_TS()
        {
            ReunparseCheckTS(B.Expression, "Unary1.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary1()
        {
            ReunparseCheck(B.Expression, "Unary1.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary2_TS()
        {
            ReunparseCheckTS(B.Expression, "Unary2.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary2()
        {
            ReunparseCheck(B.Expression, "Unary2.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary3_TS()
        {
            ReunparseCheckTS(B.Expression, "Unary3.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary3()
        {
            ReunparseCheck(B.Expression, "Unary3.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary4_TS()
        {
            ReunparseCheckTS(B.Expression, "Unary4.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary4()
        {
            ReunparseCheck(B.Expression, "Unary4.expr");
        }

	}
}

