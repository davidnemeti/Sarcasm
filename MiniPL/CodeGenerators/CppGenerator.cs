using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniPL.DomainModel;
using Sarcasm.CodeGeneration;
using Sarcasm.Reflection;

namespace MiniPL.CodeGenerators
{
    [CodeGenerator(typeof(Program), "C++")]
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
    }

    public partial class CppGeneratorTemplate
    {
        public Program Program { get; set; }
    }
}
