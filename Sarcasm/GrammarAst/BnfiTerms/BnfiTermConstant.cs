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
        protected readonly Type domainType;

        protected BnfiTermConstant(Type domainType)
            : base(GrammarHelper.TypeNameWithDeclaringTypes(domainType))
        {
            this.domainType = domainType;
            this.AstConfig.NodeCreator = (context, parseTreeNode) => parseTreeNode.AstNode = parseTreeNode.Token.Value;
        }

        BnfTerm IBnfiTerm.AsBnfTerm()
        {
            return this;
        }

        public Type DomainType
        {
            get { return domainType; }
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

    public partial class BnfiTermConstant<TD> : BnfiTermConstant, IBnfiTerm<TD>, IBnfiTermOrAbleForChoice<TD>, IEnumerable<KeyValuePair<string, TD>>
    {
        public BnfiTermConstant()
            : base(typeof(TD))
        {
        }

        public void Add(string lexeme, TD value)
        {
            base.Add(lexeme, value);
        }

        [Obsolete("Type of value does not match", error: true)]
        public new void Add(string lexeme, object value)
        {
            base.Add(lexeme, value);
        }

        public IEnumerator<KeyValuePair<string, TD>> GetEnumerator()
        {
            return base.Constants.Select(keyValuePair => new KeyValuePair<string, TD>(keyValuePair.Key, (TD)keyValuePair.Value)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
