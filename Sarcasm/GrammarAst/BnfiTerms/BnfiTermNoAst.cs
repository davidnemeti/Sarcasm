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

        public static BnfiTermNoAst For_(BnfTerm bnfTerm, ValueCreatorFromNoAst<object> valueCreatorFromNoAst)
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
