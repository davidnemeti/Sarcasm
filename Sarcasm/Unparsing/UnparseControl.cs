using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.GrammarAst;

using Grammar = Sarcasm.GrammarAst.Grammar;

namespace Sarcasm.Unparsing
{
    public class UnparseControl
    {
        private readonly Grammar grammar;
        private Dictionary<BnfTerm, ParenthesizedExpression> expressionToParentheses;
        internal bool ExpressionToParenthesesHasBeenSet { get; private set; }

        private UnparseControl(Grammar grammar)
        {
            this.grammar = grammar;
        }

        public static UnparseControl Create_NoUnparse(Grammar grammar)
        {
            return new UnparseControl(grammar) { ExpressionToParenthesesHasBeenSet = false };
        }

        public static UnparseControl Create(Grammar grammar, params ParenthesizedExpression[] parenthesizedExpressionsForPrecedenceBasedUnparse)
        {
            return Create(grammar, new Formatter(grammar), parenthesizedExpressionsForPrecedenceBasedUnparse);
        }

        public static UnparseControl Create(Grammar grammar, Formatter defaultFormatter, params ParenthesizedExpression[] parenthesizedExpressionsForPrecedenceBasedUnparse)
        {
            return new UnparseControl(grammar)
            {
                ExpressionToParenthesesHasBeenSet = true,
                DefaultFormatter = defaultFormatter,
                expressionToParentheses = parenthesizedExpressionsForPrecedenceBasedUnparse.ToDictionary(parenthesizedExpression => parenthesizedExpression.Expression)
            };
        }

        internal IEnumerable<KeyValuePair<BnfTerm, ParenthesizedExpression>> GetExpressionToParentheses()
        {
            return expressionToParentheses;
        }

        internal void SetExpressionToParentheses(IEnumerable<KeyValuePair<BnfTerm, ParenthesizedExpression>> expressionToParentheses)
        {
            this.expressionToParentheses = expressionToParentheses.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.ExpressionToParenthesesHasBeenSet = true;
        }

        public Formatter DefaultFormatter { get; set; }

        public void ClearPrecedenceBasedParenthesesForExpressions()
        {
            expressionToParentheses.Clear();
            ExpressionToParenthesesHasBeenSet = false;
        }
    }
}
