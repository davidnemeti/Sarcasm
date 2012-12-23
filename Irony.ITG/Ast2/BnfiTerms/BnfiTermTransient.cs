using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.IO;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Irony.ITG
{
    public partial class BnfiTermTransient : BnfiTermNonTerminal, IBnfiTerm
    {
        public BnfiTermTransient(Type type, string errorAlias = null)
            : base(type, errorAlias)
        {
            GrammarHelper.MarkTransient(this);      // the child node already contains the created ast node
        }

        public new BnfiExpressionTransient Rule { set { base.Rule = value; } }

        public BnfExpression RuleTL { get { return base.Rule; } set { base.Rule = value; } }

        public BnfTerm AsBnfTerm()
        {
            return this;
        }
    }

    public partial class BnfiTermTransient<TType> : BnfiTermTransient, IBnfiTerm<TType>, ITransientWithMultipleTypesafeRule<TType>
    {
        public BnfiTermTransient(string errorAlias = null)
            : base(typeof(TType), errorAlias)
        {
        }

        [Obsolete(typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        public new BnfiExpressionTransient<TType> Rule { set { base.Rule = value; } }

        public void SetRuleOr(params IBnfiTerm<TType>[] bnfiTerms)
        {
            this.Rule = Or(bnfiTerms);
        }

        public BnfiExpressionTransient<TType> Or(params IBnfiTerm<TType>[] bnfiTerms)
        {
            return (BnfiExpressionTransient<TType>)bnfiTerms
                .Select(bnfiTerm => bnfiTerm.AsBnfTerm())
                .Aggregate(
                new BnfExpression(),
                (bnfExpressionProcessed, bnfTermToBeProcess) => bnfExpressionProcessed | bnfTermToBeProcess
                );
        }
    }
}
