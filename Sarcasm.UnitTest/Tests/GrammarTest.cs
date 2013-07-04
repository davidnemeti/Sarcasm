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
    [TestClass]
    public class GrammarTest : CommonTest
    {
        protected const string category = "GrammarTest";

        [TestMethod]
        [TestCategory(category)]
        public void GrammarCheck()
        {
            CommonTest.InitializeGrammar();
        }
    }
}
