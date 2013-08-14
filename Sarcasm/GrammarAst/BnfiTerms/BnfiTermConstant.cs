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
    public abstract partial class BnfiTermConstant : ConstantTerminal, IBnfiTerm
    {
        protected readonly Type type;

        protected BnfiTermConstant(Type type)
            : base(GrammarHelper.TypeNameWithDeclaringTypes(type))
        {
            this.type = type;
            this.AstConfig.NodeCreator = (context, parseTreeNode) => parseTreeNode.AstNode = parseTreeNode.Token.Value;
        }

        BnfTerm IBnfiTerm.AsBnfTerm()
        {
            return this;
        }

        public Type Type
        {
            get { return type; }
        }
    }

    public partial class BnfiTermConstantTL : BnfiTermConstant, IBnfiTermTL, IEnumerable<KeyValuePair<string, object>>
    {
        public BnfiTermConstantTL()
            : base(typeof(object))
        {
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return base.Constants.Select(keyValuePair => new KeyValuePair<string, object>(keyValuePair.Key, keyValuePair.Value)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public partial class BnfiTermConstant<T> : BnfiTermConstant, IBnfiTerm<T>, IBnfiTermOrAbleForChoice<T>, IEnumerable<KeyValuePair<string, T>>
    {
        public BnfiTermConstant()
            : base(typeof(T))
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

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return base.Constants.Select(keyValuePair => new KeyValuePair<string, T>(keyValuePair.Key, (T)keyValuePair.Value)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
