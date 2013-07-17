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
        private Dictionary<BnfTerm, ExpressionUnparser.Parentheses> expressionToParentheses = new Dictionary<BnfTerm, ExpressionUnparser.Parentheses>();
        internal bool ExpressionToParenthesesHasBeenSet { get; private set; }

        public UnparseControl(Grammar grammar)
        {
            this.grammar = grammar;
            this.ExpressionToParenthesesHasBeenSet = false;
        }

        internal IReadOnlyDictionary<BnfTerm, ExpressionUnparser.Parentheses> GetExpressionToParentheses()
        {
            return expressionToParentheses;
        }

        internal void SetExpressionToParentheses(IReadOnlyDictionary<BnfTerm, ExpressionUnparser.Parentheses> expressionToParentheses)
        {
            this.expressionToParentheses = expressionToParentheses.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.ExpressionToParenthesesHasBeenSet = true;
        }

        private Formatter _defaultFormatter;
        public Formatter DefaultFormatter
        {
            get
            {
                if (_defaultFormatter == null)
                    _defaultFormatter = new Formatter(grammar);

                return _defaultFormatter;
            }
            set
            {
                _defaultFormatter = value;
            }
        }

        public void SetPrecedenceBasedParenthesesForExpression(BnfTerm expression, BnfTerm leftParenthesis, BnfTerm rightParenthesis)
        {
            expressionToParentheses.Add(expression, new ExpressionUnparser.Parentheses() { Expression = expression, Left = leftParenthesis, Right = rightParenthesis });
            ExpressionToParenthesesHasBeenSet = true;
        }

        public void NoPrecedenceBasedParenthesesNeededForExpressions()
        {
            ExpressionToParenthesesHasBeenSet = true;
        }

        public void ClearPrecedenceBasedParenthesesForExpressions()
        {
            expressionToParentheses.Clear();
            ExpressionToParenthesesHasBeenSet = false;
        }
    }
}
