using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sarcasm.CodeGeneration;
using Sarcasm.DomainCore;
using Sarcasm.GrammarAst;
using Sarcasm.Unparsing;
using Sarcasm.Utility;

namespace Sarcasm.Reflection
{
    public class MetaCodeGenerator
    {
        public Type CodeGeneratorType { get; private set; }
        public CodeGeneratorAttribute CodeGeneratorAttribute { get; private set; }
        public Type DomainRoot { get; private set; }

        public string Name { get { return CodeGeneratorAttribute.Name; } }

        public MetaCodeGenerator(Type codeGeneratorType)
        {
            var codeGeneratorAttribute = codeGeneratorType.GetCustomAttribute<CodeGeneratorAttribute>();

            if (!IsCodeGeneratorType(codeGeneratorType))
                throw new ArgumentException("Type should be a codeGenerator type, i.e. an implementer of ICodeGenerator with CodeGeneratorAttribute", "type");

            this.CodeGeneratorType = codeGeneratorType;
            this.CodeGeneratorAttribute = codeGeneratorAttribute;
            this.DomainRoot = codeGeneratorAttribute.DomainRoot;
        }

        public static bool IsCodeGeneratorType(Type type)
        {
            return type.GetInterface(typeof(ICodeGenerator).Name) != null && type.GetCustomAttribute<CodeGeneratorAttribute>() != null;
        }

        public ICodeGenerator CreateCodeGenerator()
        {
            return (ICodeGenerator)Activator.CreateInstance(CodeGeneratorType);
        }
    }
}
