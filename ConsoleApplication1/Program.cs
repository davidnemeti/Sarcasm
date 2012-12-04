using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Irony.AstBinders;

using ETUS.DomainModel;
using ETUS.Grammar;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var grammar = new UDLGrammar();
            var languageData = new LanguageData(grammar);
            Console.WriteLine(Helper.GetNonTerminalsAsText(languageData));
            Console.WriteLine();
            Console.WriteLine(Helper.GetNonTerminalsAsText(languageData, omitProperties: true));
            var parser = new Parser(languageData);
            ParseTree parseTree = parser.Parse("boo 5 +");
            ParseTree parseTree2 = parser.Parse("soo 6 + 7");
        }
    }
}
