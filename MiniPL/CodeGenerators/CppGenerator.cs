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

        /*
         * NOTE: this generator class does not have any instance fields, therefore instances of this class don't have any state
         * (the Generate method creates a CppGeneratorTemplate every time it's being called), thus we don't have to really block
         * concurrent calls of Generate method. So we return a new instance of AsyncLock on every access of the Lock property,
         * thus those who access this property and lock on it will find the lock always open, so they won't block.
         * */
        public AsyncLock Lock { get { return new AsyncLock(); } }
    }

    public partial class CppGeneratorTemplate
    {
        public Program Program { get; set; }
    }
}
