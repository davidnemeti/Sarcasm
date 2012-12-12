using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Irony.Extension;
using Irony.Extension.AstBinders;

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
        }
    }
}
