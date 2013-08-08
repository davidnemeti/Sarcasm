using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm.Unparsing;

namespace Sarcasm.GrammarAst
{
    public partial class BnfiTermNoAst : BnfiTermNonTerminal, IBnfiTerm, IUnparsableNonTerminal
    {
        #region State

        private readonly ValueCreatorFromNoAst<object> valueCreatorFromNoAst;

        #endregion

        public BnfiTermNoAst(string name = null)
            : base(name)
        {
            this.SetFlag(TermFlags.NoAstNode);
        }

        protected BnfiTermNoAst(BnfTerm bnfTerm, ValueCreatorFromNoAst<object> valueCreatorFromNoAst)
            : this(string.Format("{0}->no_ast", bnfTerm.Name))
        {
            base.Rule = bnfTerm.ToBnfExpression();
            this.valueCreatorFromNoAst = valueCreatorFromNoAst;
        }

        public static BnfiTermNoAst For(KeyTerm keyTerm)
        {
            return new BnfiTermNoAst(keyTerm, valueCreatorFromNoAst: null);
        }

        public static BnfiTermNoAst For(BnfTerm bnfTerm, ValueCreatorFromNoAst<object> valueCreatorFromNoAst)
        {
            return new BnfiTermNoAst(bnfTerm, valueCreatorFromNoAst);
        }

        public static BnfiTermNoAst For(BnfiTermKeyTerm bnfiTermKeyTerm)
        {
            return new BnfiTermNoAst(bnfiTermKeyTerm, valueCreatorFromNoAst: null);
        }

        public static BnfiTermNoAst For<T>(IBnfiTerm<T> bnfTerm, ValueCreatorFromNoAst<T> valueCreatorFromNoAst)
        {
            return new BnfiTermNoAst(bnfTerm.AsBnfTerm(), CastValueCreator(valueCreatorFromNoAst));
        }

        BnfTerm IBnfiTerm.AsBnfTerm()
        {
            return this;
        }

        NonTerminal INonTerminal.AsNonTerminal()
        {
            return this;
        }

        #region Unparse

        protected override bool TryGetUtokensDirectly(IUnparser unparser, UnparsableAst self, out IEnumerable<UtokenValue> utokens)
        {
            utokens = null;
            return false;
        }

        protected override IEnumerable<UnparsableAst> GetChildren(Unparser.ChildBnfTerms childBnfTerms, object astValue, Unparser.Direction direction)
        {
            return childBnfTerms.Select(childBnfTerm => new UnparsableAst(childBnfTerm, astValue: valueCreatorFromNoAst != null ? valueCreatorFromNoAst() : null));
        }

        protected override int? GetChildrenPriority(IUnparser unparser, object astValue, Unparser.Children children, Unparser.Direction direction)
        {
            return 0;
        }

        #endregion

        protected static ValueCreatorFromNoAst<object> CastValueCreator<T>(ValueCreatorFromNoAst<T> valueCreator)
        {
            return () => valueCreator();
        }

        public new BnfiExpressionNoAst Rule { set { base.Rule = value; } }
    }
}
