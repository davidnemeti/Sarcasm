using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarcasmizer
{
    enum PlayingOption { Normal, Slow, Fast }

    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = new Parameter<FileInfo>
            {
                ShortName = "i",
                LongName = "input",
                ValueName = "filename",
                Description = "The path of the input file.",
            };

            var outputFile = new Parameter<FileInfo>()
            {
                ShortName = "o",
                LongName = "output",
                ValueName = "filename",
                Description = "The path of the output file.",
            };

            var playingOption = new Parameter<PlayingOption>()
            {
                ShortName = "p",
                LongName = "playing-option",
                Description = "The options for playing.",
            };

            var randomNumbers = new ParameterList<double>
            {
                ShortName = "r",
                LongName = "random",
                Description = "Random numbers.",
            };

            var fast = new Parameter<bool>
            {
                ShortName = "f",
                LongName = "fast",
                Description = "Make it fast.",
            };

            var culture = new Parameter<CultureInfo>
            {
                ShortName = "cu",
                LongName = "culture",
                Description = "The culture.",
                IsOptional = true,
                DefaultValue = CultureInfo.InvariantCulture
            };

            var count = new Parameter<int>()
            {
                ShortName = "c",
                LongName = "count",
                Description = "Count of something.",
                IsOptional = true,
                DefaultValue = 1,
                PossibleValues = { 1, 2, 3, 4, 5 },
                StaticContextParameters =
                {
                    randomNumbers
                }
            };

            var commandLine = new CommandLine
            {
                ProgramName = "sarcasmizer",

                Description = "This is a good program.",

                Parameters =
                {
                    inputFile,
                    outputFile,
                    playingOption,
                    count,
                    fast,
                    culture
                },

                Examples =
                {
                    new Example
                    {
                        Parameters =
                        {
                            inputFile.WithValue(new FileInfo("foo.txt")),
                            outputFile.WithValue(new FileInfo("bar.txt"))
                        },
                        Description = "Simple example."
                    },
                    new Example
                    {
                        Parameters =
                        {
                            inputFile.WithValue(new FileInfo("foo.txt")),
                            outputFile.WithValue(new FileInfo("bar.txt")),
                            count.WithValue(8)
                        },
                        Description = "Example."
                    },
                    new Example
                    {
                        Parameters =
                        {
                            inputFile.WithValue(new FileInfo("foo.txt")),
                            outputFile.WithValue(new FileInfo("bar.txt")),
                            count.WithValue(3),
                            randomNumbers.WithValue(3.1415, 7, 8.9)
                        },
                        Description = "Complex example."
                    }
                }
            };

            try
            {
                commandLine.FillParameterValues(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();

                commandLine.ShowHelp(Console.Out);
            }
        }
    }
}
