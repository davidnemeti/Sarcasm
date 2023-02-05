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
