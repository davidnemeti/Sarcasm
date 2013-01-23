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
using Sarcasm.Unparsing;

namespace ConsoleApplication1
{
    class Program
    {
//        static string path = @"";

        static void Main(string[] args)
        {
            //var grammar = new Grammar();
            //Console.WriteLine(grammar.GetNonTerminalsAsText());
            ////Console.WriteLine();
            ////Console.WriteLine(grammar.GetNonTerminalsAsText(omitBoundMembers: true));

            //var parser = new Parser(grammar);
            //ParseTree parseTree = parser.Parse(File.ReadAllText(path), path);

            //Unparser unparser = new Unparser(grammar);

            //Directory.CreateDirectory("unparse_logs");
            //var stream = File.Create(@"unparse_logs\unparsed_text");
            //unparser.Unparse(parseTree.Root.AstNode).WriteToStream(stream, unparser);

            ////string str = unparser.Unparse(parseTree.Root.AstNode).AsString(unparser);
            ////Console.WriteLine(str);
        }
    }
}
