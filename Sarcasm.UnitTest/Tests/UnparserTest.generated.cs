 

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
        public void Unparse_Binary1_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheckTS(B.Expression, "Binary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary1()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheck(B.Expression, "Binary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary1_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheckTS(B.Expression, "Binary1.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary2_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheckTS(B.Expression, "Binary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary2()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheck(B.Expression, "Binary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary2_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheckTS(B.Expression, "Binary2.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary3_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheckTS(B.Expression, "Binary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary3()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheck(B.Expression, "Binary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary3_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheckTS(B.Expression, "Binary3.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary4_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheckTS(B.Expression, "Binary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary4()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheck(B.Expression, "Binary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary4_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheckTS(B.Expression, "Binary4.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheckTS(B.Program, "MiniPL.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheck(B.Program, "MiniPL.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheckTS(B.Program, "MiniPL.mplp", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL2_Typesafe()
        {
            unparser.Formatting = formatting2;
            ReunparseCheckTS(B.Program, "MiniPL2.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL2()
        {
            unparser.Formatting = formatting2;
            ReunparseCheck(B.Program, "MiniPL2.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL2_Typesafe_Reversed()
        {
            unparser.Formatting = formatting2;
            ReunparseCheckTS(B.Program, "MiniPL2.mplp", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL3_Typesafe()
        {
            unparser.Formatting = formatting3;
            ReunparseCheckTS(B.Program, "MiniPL3.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL3()
        {
            unparser.Formatting = formatting3;
            ReunparseCheck(B.Program, "MiniPL3.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL3_Typesafe_Reversed()
        {
            unparser.Formatting = formatting3;
            ReunparseCheckTS(B.Program, "MiniPL3.mplp", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary1_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheckTS(B.Expression, "Unary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary1()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheck(B.Expression, "Unary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary1_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheckTS(B.Expression, "Unary1.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary2_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheckTS(B.Expression, "Unary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary2()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheck(B.Expression, "Unary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary2_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheckTS(B.Expression, "Unary2.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary3_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheckTS(B.Expression, "Unary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary3()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheck(B.Expression, "Unary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary3_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheckTS(B.Expression, "Unary3.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary4_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheckTS(B.Expression, "Unary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary4()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheck(B.Expression, "Unary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary4_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            ReunparseCheckTS(B.Expression, "Unary4.expr", leftToRight: false);
        }

	}
}

