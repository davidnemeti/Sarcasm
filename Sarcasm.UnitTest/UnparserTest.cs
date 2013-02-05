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
    public class UnparserTest : CommonTest
    {
        protected readonly static string expectedUnparsedSourcesDir = Path.Combine(expectedResultsDir, @"Unparsed sources");
        protected readonly static string actualUnparsedSourcesDir = Path.Combine(actualResultsDir, @"Unparsed sources");

        [ClassInitialize]
        public static void InitializeUnparser(TestContext testContext)
        {
            CommonTest.Initialize();
//            Unparser unparser = new Unparser(grammar);
        }

        //[TestMethod]
        //public void Unparse()
        //{
        //    ParseTree parseTree = ParseFileAndCheck(parseFileName);

        //    Directory.CreateDirectory(unparseDir);
        //    var stream = File.Create(Path.Combine(unparseDir, @"unparsed_text"));
        //    unparser.Unparse(parseTree.Root.AstNode).WriteToStream(stream, unparser);

        //    ////string str = unparser.Unparse(parseTree.Root.AstNode).AsString(unparser);
        //    ////Console.WriteLine(str);
        //}
    }
}
