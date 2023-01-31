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
    public partial class ParserTest
    {
        [TestMethod]
        [TestCategory(category)]
        public void Parse_Binary1_TS()
        {
            ParseFileSaveAstAndCheckTS(B.Expression, "Binary1.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Binary1()
        {
            ParseFileSaveAstAndCheck(B.Expression, "Binary1.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Binary2_TS()
        {
            ParseFileSaveAstAndCheckTS(B.Expression, "Binary2.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Binary2()
        {
            ParseFileSaveAstAndCheck(B.Expression, "Binary2.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Binary3_TS()
        {
            ParseFileSaveAstAndCheckTS(B.Expression, "Binary3.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Binary3()
        {
            ParseFileSaveAstAndCheck(B.Expression, "Binary3.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Binary4_TS()
        {
            ParseFileSaveAstAndCheckTS(B.Expression, "Binary4.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Binary4()
        {
            ParseFileSaveAstAndCheck(B.Expression, "Binary4.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_MiniPL_TS_Root()
        {
            ParseFileSaveAstAndCheck("MiniPL.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_MiniPL_TS()
        {
            ParseFileSaveAstAndCheckTS(B.Program, "MiniPL.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_MiniPL_Root()
        {
            ParseFileSaveAstAndCheck("MiniPL.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_MiniPL()
        {
            ParseFileSaveAstAndCheck(B.Program, "MiniPL.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_MiniPL2_TS_Root()
        {
            ParseFileSaveAstAndCheck("MiniPL2.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_MiniPL2_TS()
        {
            ParseFileSaveAstAndCheckTS(B.Program, "MiniPL2.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_MiniPL2_Root()
        {
            ParseFileSaveAstAndCheck("MiniPL2.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_MiniPL2()
        {
            ParseFileSaveAstAndCheck(B.Program, "MiniPL2.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_MiniPL3_TS_Root()
        {
            ParseFileSaveAstAndCheck("MiniPL3.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_MiniPL3_TS()
        {
            ParseFileSaveAstAndCheckTS(B.Program, "MiniPL3.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_MiniPL3_Root()
        {
            ParseFileSaveAstAndCheck("MiniPL3.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_MiniPL3()
        {
            ParseFileSaveAstAndCheck(B.Program, "MiniPL3.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_MiniPLWithComments_TS_Root()
        {
            ParseFileSaveAstAndCheck("MiniPLWithComments.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_MiniPLWithComments_TS()
        {
            ParseFileSaveAstAndCheckTS(B.Program, "MiniPLWithComments.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_MiniPLWithComments_Root()
        {
            ParseFileSaveAstAndCheck("MiniPLWithComments.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_MiniPLWithComments()
        {
            ParseFileSaveAstAndCheck(B.Program, "MiniPLWithComments.mplp");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Unary1_TS()
        {
            ParseFileSaveAstAndCheckTS(B.Expression, "Unary1.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Unary1()
        {
            ParseFileSaveAstAndCheck(B.Expression, "Unary1.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Unary2_TS()
        {
            ParseFileSaveAstAndCheckTS(B.Expression, "Unary2.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Unary2()
        {
            ParseFileSaveAstAndCheck(B.Expression, "Unary2.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Unary3_TS()
        {
            ParseFileSaveAstAndCheckTS(B.Expression, "Unary3.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Unary3()
        {
            ParseFileSaveAstAndCheck(B.Expression, "Unary3.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Unary4_TS()
        {
            ParseFileSaveAstAndCheckTS(B.Expression, "Unary4.expr");
        }

        [TestMethod]
        [TestCategory(category)]
        public void Parse_Unary4()
        {
            ParseFileSaveAstAndCheck(B.Expression, "Unary4.expr");
        }

    }
}

