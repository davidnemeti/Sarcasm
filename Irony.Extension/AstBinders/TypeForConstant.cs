using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Irony.Extension.AstBinders
{
    public static class TypeForConstant
    {
        public static TypeForConstant<TType> Of<TType>()
        {
            return new TypeForConstant<TType>();
        }

        public static ConstantTerminal Of(Type type)
        {
            return new ConstantTerminal(GrammarHelper.TypeNameWithDeclaringTypes(type));
        }
    }

    public class TypeForConstant<T> : ConstantTerminal, IBnfTerm<T>
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

        BnfTerm IBnfTerm<T>.AsTypeless()
        {
            return this;
        }
    }
}
