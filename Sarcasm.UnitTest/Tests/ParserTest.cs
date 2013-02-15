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
    [TestClass]
    public partial class GrammarTest : CommonTest
    {
        [ClassInitialize]
        public static void InitializeGrammar(TestContext testContext)
        {
            CommonTest.InitializeGrammar();
        }
    }
}
