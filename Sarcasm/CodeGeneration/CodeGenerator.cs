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
        AsyncLock Lock { get; }
    }

    public static class CodeGeneratorExtensions
    {
#if NET4_0
        public static Task<string> GenerateAsync(this ICodeGenerator codeGenerator, object root)
        {
            return codeGenerator.Lock.LockAsync()
                .ContinueWith(task => TaskEx.Run(() => { using (task.Result) return codeGenerator.Generate(root); }))
                .Unwrap();
        }
#else
        public static async Task<string> GenerateAsync(this ICodeGenerator codeGenerator, object root)
        {
            using (await codeGenerator.Lock.LockAsync())
                return await Task.Run(() => codeGenerator.Generate(root));
        }
#endif
    }
}
