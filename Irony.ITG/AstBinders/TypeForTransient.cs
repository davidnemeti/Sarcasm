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
    public partial class TypeForTransient : TypeForNonTerminal, IBnfTerm
    {
        protected TypeForTransient(Type type, string errorAlias)
            : base(type, errorAlias)
        {
            this.Flags |= TermFlags.IsTransient | TermFlags.NoAstNode;      // the child node already contains the created ast node
        }

        public static TypeForTransient<TType> Of<TType>(string errorAlias = null)
        {
            return new TypeForTransient<TType>(errorAlias);
        }

        public static TypeForTransient Of(Type type, string errorAlias = null)
        {
            return new TypeForTransient(type, errorAlias);
        }

        public new BnfExpressionTransient Rule { set { base.Rule = value; } }

        public BnfExpression RuleTL { get { return base.Rule; } set { base.Rule = value; } }

        public BnfTerm AsBnfTerm()
        {
            return this;
        }
    }

    public partial class TypeForTransient<TType> : TypeForTransient, IBnfTerm<TType>, ITransientWithMultipleTypesafeRule<TType>
    {
        internal TypeForTransient(string errorAlias)
            : base(typeof(TType), errorAlias)
        {
        }

        [Obsolete(typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        public new BnfExpressionTransient<TType> Rule { set { base.Rule = value; } }

        public BnfExpressionTransient<TType> SetRuleOr(params IBnfTerm<TType>[] bnfTerms)
        {
            return (BnfExpressionTransient<TType>)bnfTerms
                .Aggregate(
                new BnfExpression(),
                (bnfExpressionProcessed, bnfTermToBeProcess) => bnfExpressionProcessed | bnfTermToBeProcess.AsBnfTerm()
                );
        }
    }
}
