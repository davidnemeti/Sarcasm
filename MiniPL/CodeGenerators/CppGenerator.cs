using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniPL.DomainDefinitions;
using Sarcasm.CodeGeneration;
using Sarcasm.Reflection;
using Sarcasm.Utility;

namespace MiniPL.CodeGenerators
{
    [CodeGenerator(typeof(Domain), "C++")]
    public class CppGenerator : ICodeGenerator
    {
        public string Generate(Program program)
        {
            return new CppGeneratorTemplate() { Program = program }.TransformText();
        }

        string ICodeGenerator.Generate(object root)
        {
            return Generate((Program)root);
        }

        private readonly AsyncLock _lock = new AsyncLock();
        public AsyncLock Lock { get { return _lock; } }
    }

    public partial class CppGeneratorTemplate
    {
        public Program Program { get; set; }
    }
}
