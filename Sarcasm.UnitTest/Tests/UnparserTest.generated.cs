 

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

using MiniPL.DomainModel;

using Grammar = Sarcasm.GrammarAst.Grammar;

namespace Sarcasm.UnitTest
{
    public partial class UnparserTest
    {
        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary1_Sequential_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Binary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary1_Sequential()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Expression, "Binary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary1_Sequential_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Binary1.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary1_Parallel_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Binary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary1_Parallel()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Expression, "Binary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary1_Parallel_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Binary1.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary2_Sequential_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Binary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary2_Sequential()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Expression, "Binary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary2_Sequential_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Binary2.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary2_Parallel_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Binary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary2_Parallel()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Expression, "Binary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary2_Parallel_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Binary2.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary3_Sequential_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Binary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary3_Sequential()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Expression, "Binary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary3_Sequential_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Binary3.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary3_Parallel_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Binary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary3_Parallel()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Expression, "Binary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary3_Parallel_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Binary3.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary4_Sequential_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Binary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary4_Sequential()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Expression, "Binary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary4_Sequential_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Binary4.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary4_Parallel_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Binary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary4_Parallel()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Expression, "Binary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary4_Parallel_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Binary4.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL_Sequential_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Program, "MiniPL.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL_Sequential()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Program, "MiniPL.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL_Sequential_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Program, "MiniPL.mplp", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL_Parallel_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Program, "MiniPL.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL_Parallel()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Program, "MiniPL.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL_Parallel_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Program, "MiniPL.mplp", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL2_Sequential_Typesafe()
        {
            unparser.Formatting = formatting2;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Program, "MiniPL2.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL2_Sequential()
        {
            unparser.Formatting = formatting2;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Program, "MiniPL2.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL2_Sequential_Typesafe_Reversed()
        {
            unparser.Formatting = formatting2;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Program, "MiniPL2.mplp", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL2_Parallel_Typesafe()
        {
            unparser.Formatting = formatting2;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Program, "MiniPL2.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL2_Parallel()
        {
            unparser.Formatting = formatting2;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Program, "MiniPL2.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL2_Parallel_Typesafe_Reversed()
        {
            unparser.Formatting = formatting2;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Program, "MiniPL2.mplp", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL3_Sequential_Typesafe()
        {
            unparser.Formatting = formatting3;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Program, "MiniPL3.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL3_Sequential()
        {
            unparser.Formatting = formatting3;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Program, "MiniPL3.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL3_Sequential_Typesafe_Reversed()
        {
            unparser.Formatting = formatting3;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Program, "MiniPL3.mplp", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL3_Parallel_Typesafe()
        {
            unparser.Formatting = formatting3;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Program, "MiniPL3.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL3_Parallel()
        {
            unparser.Formatting = formatting3;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Program, "MiniPL3.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL3_Parallel_Typesafe_Reversed()
        {
            unparser.Formatting = formatting3;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Program, "MiniPL3.mplp", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary1_Sequential_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Unary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary1_Sequential()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Expression, "Unary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary1_Sequential_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Unary1.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary1_Parallel_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Unary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary1_Parallel()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Expression, "Unary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary1_Parallel_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Unary1.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary2_Sequential_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Unary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary2_Sequential()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Expression, "Unary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary2_Sequential_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Unary2.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary2_Parallel_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Unary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary2_Parallel()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Expression, "Unary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary2_Parallel_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Unary2.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary3_Sequential_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Unary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary3_Sequential()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Expression, "Unary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary3_Sequential_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Unary3.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary3_Parallel_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Unary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary3_Parallel()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Expression, "Unary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary3_Parallel_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Unary3.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary4_Sequential_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Unary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary4_Sequential()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Expression, "Unary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary4_Sequential_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Unary4.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary4_Parallel_Typesafe()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Unary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary4_Parallel()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Expression, "Unary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary4_Parallel_Typesafe_Reversed()
        {
            unparser.Formatting = formattingDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Unary4.expr", leftToRight: false);
        }

	}
}

