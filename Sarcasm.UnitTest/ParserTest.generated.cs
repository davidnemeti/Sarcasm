 

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
        public void Parse_Binary1()
        {
            ParseFileSaveXmlAndCheck(exprParser, "Binary1.expr");
        }

        [TestMethod]
        public void Parse_Binary2()
        {
            ParseFileSaveXmlAndCheck(exprParser, "Binary2.expr");
        }

        [TestMethod]
        public void Parse_MiniPL()
        {
            ParseFileSaveXmlAndCheck(parser, "MiniPL.mplp");
        }

        [TestMethod]
        public void Parse_Unary1()
        {
            ParseFileSaveXmlAndCheck(exprParser, "Unary1.expr");
        }

        [TestMethod]
        public void Parse_Unary2()
        {
            ParseFileSaveXmlAndCheck(exprParser, "Unary2.expr");
        }

	}
}
