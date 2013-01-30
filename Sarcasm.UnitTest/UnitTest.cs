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

namespace Sarcasm.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        const string unparseDir = @"unparsed";
        const string parseFileName = @"MiniPL_testfile.mplp";

        [TestMethod]
        public void Parse_Unparse()
        {
            var grammar = new MiniPL.GrammarP();
            //Console.WriteLine(grammar.GetNonTerminalsAsText());
            ////Console.WriteLine();
            ////Console.WriteLine(grammar.GetNonTerminalsAsText(omitBoundMembers: true));

            Assert.IsTrue(grammar.Root != null, "Root is null");

            var parser = new Parser(grammar);

            Assert.IsTrue(parser.Language.ErrorLevel < GrammarErrorLevel.Error, "Grammar error:\n{0}", string.Join("\n", parser.Language.Errors));

            ParseTree parseTree = parser.Parse(ConvertTabsToSpaces(File.ReadAllText(parseFileName)), parseFileName);

            Assert.IsTrue(parseTree.Status == ParseTreeStatus.Parsed, "Parser error:\n{0}",
                string.Join("\n",
                    parseTree.ParserMessages.Select(parserMessage => string.Format("{0} {1}: {2}", parseTree.FileName, parserMessage.Location.ToString(), parserMessage.Message))
                    )
                );

            Unparser unparser = new Unparser(grammar);

            Directory.CreateDirectory(unparseDir);
            var stream = File.Create(Path.Combine(unparseDir, @"unparsed_text"));
            unparser.Unparse(parseTree.Root.AstNode).WriteToStream(stream, unparser);

            ////string str = unparser.Unparse(parseTree.Root.AstNode).AsString(unparser);
            ////Console.WriteLine(str);
        }

        // Irony does not handle tabs properly, so we convert them: the "Ch" in VS matches parserMessage.Location.Column
        private string ConvertTabsToSpaces(string sourceText)
        {
            return sourceText.Replace("\t", " ");
        }
    }
}
