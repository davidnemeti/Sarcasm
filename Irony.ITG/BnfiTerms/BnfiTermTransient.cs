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
        protected BnfiTermTransient(Type type, string errorAlias)
            : base(type, errorAlias)
        {
            this.Flags |= TermFlags.IsTransient | TermFlags.NoAstNode;      // the child node already contains the created ast node
        }

        public static BnfiTermTransient<TType> Of<TType>(string errorAlias = null)
        {
            return new BnfiTermTransient<TType>(errorAlias);
        }

        public static BnfiTermTransient Of(Type type, string errorAlias = null)
        {
            return new BnfiTermTransient(type, errorAlias);
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
        internal BnfiTermTransient(string errorAlias)
            : base(typeof(TType), errorAlias)
        {
        }

        [Obsolete(typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        public new BnfiExpressionTransient<TType> Rule { set { base.Rule = value; } }

        public BnfiExpressionTransient<TType> SetRuleOr(params IBnfiTerm<TType>[] bnfTerms)
        {
            return (BnfiExpressionTransient<TType>)bnfTerms
                .Aggregate(
                new BnfExpression(),
                (bnfExpressionProcessed, bnfTermToBeProcess) => bnfExpressionProcessed | bnfTermToBeProcess.AsBnfTerm()
                );
        }
    }
}
