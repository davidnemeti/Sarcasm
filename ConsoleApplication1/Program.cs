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
using Irony.ITG;
using Irony.ITG.Ast;
using Irony.ITG.Unparsing;

using ETUS.DomainModel;
using ETUS.Grammar;

namespace ConsoleApplication1
{
    class Program
    {
        static string path = @"C:\Users\dave\Documents\Programming\ETUS\ETUS.Predefined\Length.units";

        static void Main(string[] args)
        {
            var grammar = new UDLGrammar();
            Console.WriteLine(grammar.GetNonTerminalsAsText());
            Console.WriteLine();
            Console.WriteLine(grammar.GetNonTerminalsAsText(omitBoundMembers: true));

            var parser = new Parser(grammar);
            ParseTree parseTree = parser.Parse(File.ReadAllText(path), path);

            Unparser unparser = new Unparser(grammar);
//            string str = unparser.Unparse(parseTree.Root.AstNode).AsString(unparser);
            Directory.CreateDirectory("unparse_logs");
            var stream = File.Create(@"unparse_logs\unparsed_text");
            unparser.Unparse(parseTree.Root.AstNode).WriteToStreamAsync(stream, unparser);
//            Console.WriteLine(str);
        }
    }
}
