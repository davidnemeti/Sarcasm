using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Globalization;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.Ast;
using Sarcasm.Parsing;
using Sarcasm.Unparsing;

using ParseTree = Sarcasm.Parsing.ParseTree;

namespace ConsoleApplication1
{
    class Program
    {
        static string path = @"..\..\..\Sarcasm.UnitTest\Test files\MiniPL.mplp";
        static string path2 = @"..\..\..\Sarcasm.UnitTest\Test files\Binary1.expr";

        static void Main(string[] args)
        {
            var grammar = new MiniPL.GrammarP();
            ////Console.WriteLine(grammar.GetNonTerminalsAsText());
            //////Console.WriteLine();
            //////Console.WriteLine(grammar.GetNonTerminalsAsText(omitBoundMembers: true));

            //var stopwatch = Stopwatch.StartNew();
            var parser = MultiParser.Create(grammar);
            //stopwatch.Stop();
            //Console.WriteLine(stopwatch.ElapsedMilliseconds);
            //stopwatch.Start();
            //var parser2 = MultiParser.Create(grammar);
            //stopwatch.Stop();
            //Console.WriteLine(stopwatch.ElapsedMilliseconds);
            var parseTree = parser.Parse(File.ReadAllText(path), path);
            MiniPL.DomainModel.Program astValue = parseTree.RootAstValue;

            var parseTree2 = parser.Parse(File.ReadAllText(path2), path2, grammar.B.Expression);
            var parseTree3 = parser.Parse(File.ReadAllText(path2), path2, (NonTerminal)grammar.B.Expression);
            MiniPL.DomainModel.Expression astValue2 = parseTree2.RootAstValue;

            //Unparser unparser = new Unparser(grammar);

            //Directory.CreateDirectory("unparse_logs");
            //var stream = File.Create(@"unparse_logs\unparsed_text");
            //unparser.Unparse(parseTree.Root.AstNode).WriteToStream(stream, unparser);

            ////string str = unparser.Unparse(parseTree.Root.AstNode).AsString(unparser);
            ////Console.WriteLine(str);
        }
    }
}
