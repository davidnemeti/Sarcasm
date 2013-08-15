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
    public partial class UnparserTest
    {
        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary1_Sequential_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Binary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary1_Sequential()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Expression, "Binary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary1_Sequential_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Binary1.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary1_Parallel_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Binary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary1_Parallel()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Expression, "Binary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary1_Parallel_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Binary1.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary2_Sequential_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Binary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary2_Sequential()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Expression, "Binary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary2_Sequential_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Binary2.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary2_Parallel_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Binary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary2_Parallel()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Expression, "Binary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary2_Parallel_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Binary2.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary3_Sequential_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Binary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary3_Sequential()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Expression, "Binary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary3_Sequential_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Binary3.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary3_Parallel_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Binary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary3_Parallel()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Expression, "Binary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary3_Parallel_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Binary3.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary4_Sequential_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Binary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary4_Sequential()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Expression, "Binary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary4_Sequential_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Binary4.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary4_Parallel_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Binary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary4_Parallel()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Expression, "Binary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Binary4_Parallel_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Binary4.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL_Sequential_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Program, "MiniPL.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL_Sequential()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Program, "MiniPL.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL_Sequential_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Program, "MiniPL.mplp", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL_Parallel_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Program, "MiniPL.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL_Parallel()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Program, "MiniPL.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL_Parallel_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Program, "MiniPL.mplp", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL2_Sequential_Typesafe()
        {
            unparser.Formatter = formatter2;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Program, "MiniPL2.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL2_Sequential()
        {
            unparser.Formatter = formatter2;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Program, "MiniPL2.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL2_Sequential_Typesafe_Reversed()
        {
            unparser.Formatter = formatter2;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Program, "MiniPL2.mplp", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL2_Parallel_Typesafe()
        {
            unparser.Formatter = formatter2;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Program, "MiniPL2.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL2_Parallel()
        {
            unparser.Formatter = formatter2;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Program, "MiniPL2.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL2_Parallel_Typesafe_Reversed()
        {
            unparser.Formatter = formatter2;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Program, "MiniPL2.mplp", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL3_Sequential_Typesafe()
        {
            unparser.Formatter = formatter3;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Program, "MiniPL3.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL3_Sequential()
        {
            unparser.Formatter = formatter3;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Program, "MiniPL3.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL3_Sequential_Typesafe_Reversed()
        {
            unparser.Formatter = formatter3;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Program, "MiniPL3.mplp", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL3_Parallel_Typesafe()
        {
            unparser.Formatter = formatter3;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Program, "MiniPL3.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL3_Parallel()
        {
            unparser.Formatter = formatter3;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Program, "MiniPL3.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPL3_Parallel_Typesafe_Reversed()
        {
            unparser.Formatter = formatter3;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Program, "MiniPL3.mplp", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPLWithComments_Sequential_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Program, "MiniPLWithComments.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPLWithComments_Sequential()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Program, "MiniPLWithComments.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPLWithComments_Sequential_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Program, "MiniPLWithComments.mplp", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPLWithComments_Parallel_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Program, "MiniPLWithComments.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPLWithComments_Parallel()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Program, "MiniPLWithComments.mplp", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_MiniPLWithComments_Parallel_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Program, "MiniPLWithComments.mplp", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary1_Sequential_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Unary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary1_Sequential()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Expression, "Unary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary1_Sequential_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Unary1.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary1_Parallel_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Unary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary1_Parallel()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Expression, "Unary1.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary1_Parallel_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Unary1.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary2_Sequential_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Unary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary2_Sequential()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Expression, "Unary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary2_Sequential_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Unary2.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary2_Parallel_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Unary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary2_Parallel()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Expression, "Unary2.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary2_Parallel_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Unary2.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary3_Sequential_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Unary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary3_Sequential()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Expression, "Unary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary3_Sequential_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Unary3.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary3_Parallel_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Unary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary3_Parallel()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Expression, "Unary3.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary3_Parallel_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Unary3.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary4_Sequential_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Unary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary4_Sequential()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheck(B.Expression, "Unary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary4_Sequential_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = false;
            ReunparseCheckTS(B.Expression, "Unary4.expr", leftToRight: false);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary4_Parallel_Typesafe()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Unary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary4_Parallel()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheck(B.Expression, "Unary4.expr", leftToRight: true);
        }

        [TestMethod]
        [TestCategory(category)]
        public void Unparse_Unary4_Parallel_Typesafe_Reversed()
        {
            unparser.Formatter = formatterDefault;
            unparser.EnableParallelProcessing = true;
            ReunparseCheckTS(B.Expression, "Unary4.expr", leftToRight: false);
        }

	}
}

