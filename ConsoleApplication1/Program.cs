using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;

using ETUS.DomainModel;
using ETUS.Grammar;

namespace ConsoleApplication1
{
    class Person
    {
        public int n = 5;
    }

    class Program
    {
        static void Main(string[] args)
        {
            var grammar = new UDLGrammar();
            var parser = new Parser(grammar);
            ParseTree parseTree = parser.Parse("5 + 3");
        }
    }
}
