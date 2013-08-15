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
                .ContinueWith(task => { using (task.Result) return TaskEx.Run(() => codeGenerator.Generate(root)); })
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
