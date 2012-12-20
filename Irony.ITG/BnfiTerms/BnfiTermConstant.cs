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
    public static class BnfiTermConstant
    {
        public static BnfiTermConstant<T> Of<T>()
        {
            return new BnfiTermConstant<T>();
        }

        public static ConstantTerminal Of(Type type)
        {
            return new ConstantTerminal(GrammarHelper.TypeNameWithDeclaringTypes(type));
        }
    }

    public partial class BnfiTermConstant<T> : ConstantTerminal, IBnfiTerm<T>
    {
        internal BnfiTermConstant()
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
