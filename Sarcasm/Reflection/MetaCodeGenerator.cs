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
        public Type DomainType { get; private set; }

        public string Name { get { return CodeGeneratorAttribute.Name; } }

        public MetaCodeGenerator(Type codeGeneratorType)
        {
            var codeGeneratorAttribute = codeGeneratorType.GetCustomAttribute<CodeGeneratorAttribute>();

            if (!IsCodeGeneratorType(codeGeneratorType))
                throw new ArgumentException("Type should be a codeGenerator type, i.e. an implementer of ICodeGenerator with CodeGeneratorAttribute", "type");

            this.CodeGeneratorType = codeGeneratorType;
            this.CodeGeneratorAttribute = codeGeneratorAttribute;
            this.DomainType = codeGeneratorAttribute.DomainType;
        }

        public static bool IsCodeGeneratorType(Type type)
        {
            return type.GetInterfaces().Contains(typeof(ICodeGenerator)) && type.GetCustomAttribute<CodeGeneratorAttribute>() != null;
        }

        public ICodeGenerator CreateCodeGenerator()
        {
            return (ICodeGenerator)Activator.CreateInstance(CodeGeneratorType);
        }
    }
}
