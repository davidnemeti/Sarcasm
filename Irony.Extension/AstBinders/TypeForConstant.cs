using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Irony.ITG
{
    public static class TypeForConstant
    {
        public static TypeForConstant<T> Of<T>()
        {
            return new TypeForConstant<T>();
        }

        public static ConstantTerminal Of(Type type)
        {
            return new ConstantTerminal(GrammarHelper.TypeNameWithDeclaringTypes(type));
        }
    }

    public partial class TypeForConstant<T> : ConstantTerminal, IBnfTerm<T>
    {
        internal TypeForConstant()
            : base(GrammarHelper.TypeNameWithDeclaringTypes(typeof(T)))
        {
        }

        public void Add(string lexeme, T value)
        {
            base.Add(lexeme, value);
        }

        [Obsolete("Type of value does not match", error: true)]
        public new void Add(string lexeme, object value)
        {
            base.Add(lexeme, value);
        }

        public BnfTerm AsBnfTerm()
        {
            return this;
        }
    }
}
