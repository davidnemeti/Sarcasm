#region License
/*
    This file is part of Sarcasm.

    Copyright 2012-2013 Dávid Németi

    Sarcasm is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Sarcasm is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Sarcasm.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.IO;
using System.Diagnostics;

using Sarcasm.Parsing;
using Sarcasm.Unparsing;
using Sarcasm.Utility;

using MiniPLG = MiniPL.Grammars;

namespace Playground
{
    class Program
    {
        static string path = @"MiniPL_long.mplp";
//        static string path2 = @"..\..\..\Sarcasm.UnitTest\Test files\Binary1.expr";

        static void Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();

            stopwatch.Start();
            var grammar = new MiniPLG.GrammarP();
            ShowTimeAndRestart(stopwatch, "Creation of grammar");

            var parser = MultiParser.Create(grammar);
            ShowTimeAndRestart(stopwatch, "Creation of parser");

            var parseTree = parser.Parse(File.ReadAllText(path), path);
            ShowTimeAndRestart(stopwatch, "Parsing");

            var astRootValue = parseTree.RootAstValue;

//            string generatedCode = new MiniPLC.CSharpGenerator().Generate(astRootValue);
//            string generatedCode = new MiniPLC.CppGenerator().Generate(astRootValue);

#if false
            var jsonGrammar = new JsonGrammar();

            Unparser universalUnparser = new Unparser(jsonGrammar);
            var universalParser = MultiParser.Create(jsonGrammar);
            string text = universalUnparser.Unparse(astRootValue).AsText(universalUnparser);

            var foo = universalUnparser.Unparse(astRootValue).Select(utoken => utoken.GetDecoration()).ToList();

            var parseTree2 = universalParser.Parse(text);
            var astRootValue2 = parseTree2.RootAstValue;

            string text2 = universalUnparser.Unparse(astRootValue2).AsText(universalUnparser);

            bool eq = text == text2;
#endif

            Unparser unparser = new Unparser(grammar);
            ShowTimeAndRestart(stopwatch, "Creation of unparser");

            //var utokens = unparser.Unparse(astRootValue).ToList();
            //ShowTimeAndRestart(stopwatch, "Unparsing to utokens");

            //unparser.EnableParallelProcessing = false;
            //string unparsedText = unparser.Unparse(astRootValue).AsText(unparser);
            //var document = parseTree.GetDocument();
            //string unparsedText = unparser.Unparse(document).AsText(unparser);
            //ShowTimeAndRestart(stopwatch, "Converting utokens to string");

            //unparser.EnableParallelProcessing = false;
            //foreach (Utoken utoken in unparser.Unparse(astRootValue))
            //    Console.WriteLine(utoken.ToString());
            unparser.Unparse(astRootValue).ConsumeAll();
            ShowTimeAndRestart(stopwatch, "Sequential unparsing to string");

            //unparser.EnableParallelProcessing = true;
            //foreach (Utoken utoken in unparser.Unparse(astRootValue))
            //    Console.WriteLine(utoken.ToString());
            unparser.Unparse(astRootValue).ConsumeAll();
            ShowTimeAndRestart(stopwatch, "Parallel unparsing to string");

            //var utokensReverse = unparser.Unparse(astRootValue, Unparser.Direction.RightToLeft).ToList();
            //ShowTimeAndRestart(stopwatch, "Reverse unparsing to utokens");

            //unparser.EnableParallelProcessing = false;
            unparser.Unparse(astRootValue, Unparser.Direction.RightToLeft).ConsumeAll();
            ShowTimeAndRestart(stopwatch, "Reverse sequential unparsing to string");

            //unparser.EnableParallelProcessing = true;
            unparser.Unparse(astRootValue, Unparser.Direction.RightToLeft).ConsumeAll();
            ShowTimeAndRestart(stopwatch, "Reverse parallel unparsing to string");

            //stopwatch.Stop();
            //Console.WriteLine(stopwatch.ElapsedMilliseconds);
            //stopwatch.Start();
            //var parser2 = MultiParser.Create(grammar);
            //stopwatch.Stop();
            //Console.WriteLine(stopwatch.ElapsedMilliseconds);

            //var parseTree2 = parser.Parse(File.ReadAllText(path2), path2, grammar.B.Expression);
            //var parseTree3 = parser.Parse(File.ReadAllText(path2), path2, (NonTerminal)grammar.B.Expression);
            //MiniPL.DomainDefinitions.Expression astValue2 = parseTree2.RootAstValue;

            //Unparser unparser = new Unparser(grammar);

            //Directory.CreateDirectory("unparse_logs");
            //var stream = File.Create(@"unparse_logs\unparsed_text");
            //unparser.Unparse(parseTree.Root.AstNode).WriteToStream(stream, unparser);

            ////string str = unparser.Unparse(parseTree.Root.AstNode).AsString(unparser);
            ////Console.WriteLine(str);
        }

        private static TimeSpan ShowTimeAndRestart(Stopwatch stopwatch, string message)
        {
            stopwatch.Stop();
            var timeSpan = stopwatch.Elapsed;
            Console.WriteLine("{0}: time elapsed = {1}", message, stopwatch.Elapsed);
            stopwatch.Restart();
            return timeSpan;
        }
    }
}
