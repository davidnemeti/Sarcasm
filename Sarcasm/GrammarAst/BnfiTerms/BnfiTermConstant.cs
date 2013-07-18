using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Sarcasm.GrammarAst
{
    public abstract class BnfiTermConstantBase<T> : ConstantTerminal, IBnfiTerm<T>, IHasType, IEnumerable<KeyValuePair<string, T>>, IBnfiTermOrAbleForChoice<T>
    {
        public BnfiTermConstantBase()
            : base(GrammarHelper.TypeNameWithDeclaringTypes(typeof(T)))
        {
            this.AstConfig.NodeCreator = (context, parseTreeNode) => parseTreeNode.AstNode = parseTreeNode.Token.Value;
        }

        BnfTerm IBnfiTerm.AsBnfTerm()
        {
            return this;
        }

        Type IHasType.Type
        {
            get { return typeof(T); }
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return base.Constants.Select(keyValuePair => new KeyValuePair<string, T>(keyValuePair.Key, (T)keyValuePair.Value)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public partial class BnfiTermConstant<T> : BnfiTermConstantBase<T>
    {
        public void Add(string lexeme, T value)
        {
            base.Add(lexeme, value);
        }

        [Obsolete("Type of value does not match", error: true)]
        public new void Add(string lexeme, object value)
        {
            base.Add(lexeme, value);
        }
    }

    public partial class BnfiTermConstant : BnfiTermConstantBase<object>
    {
    }
}
