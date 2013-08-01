using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Sarcasm.DomainCore;
using Sarcasm.GrammarAst;
using Sarcasm.Unparsing;
using Sarcasm.Utility;

namespace Sarcasm.CodeGeneration
{
    public interface ICodeGenerator
    {
        string Generate(object root);
    }

    public static class CodeGeneratorExtensions
    {
        public static async Task<string> GenerateAsync(this ICodeGenerator codeGenerator, object root)
        {
            return await Task.Run(() => codeGenerator.Generate(root));
        }
    }
}
