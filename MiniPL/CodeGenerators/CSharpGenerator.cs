using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniPL.DomainModel;
using Sarcasm.CodeGeneration;

namespace MiniPL.CodeGenerators
{
    public class CSharpGenerator : ICodeGenerator
    {
        private readonly CSharpGeneratorTemplate csharpGeneratorTemplate;

        public CSharpGenerator()
        {
            csharpGeneratorTemplate = new CSharpGeneratorTemplate();
        }

        public string Generate(Program program)
        {
            csharpGeneratorTemplate.Program = program;
            return csharpGeneratorTemplate.TransformText();
        }

        string ICodeGenerator.Generate(object root)
        {
            return Generate((Program)root);
        }
    }

    public partial class CSharpGeneratorTemplate
    {
        public Program Program { get; set; }
    }
}
