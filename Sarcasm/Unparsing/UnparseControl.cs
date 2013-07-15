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
        internal bool ExpressionToParenthesesHasBeenManuallySet { get; private set; }

        public UnparseControl(Grammar grammar)
        {
            this.grammar = grammar;
            this.ExpressionToParenthesesHasBeenManuallySet = false;
        }

        internal IReadOnlyDictionary<BnfTerm, ExpressionUnparser.Parentheses> ExpressionToParentheses { get { return expressionToParentheses; } }

        private Formatting _defaultFormatting;
        public Formatting DefaultFormatting
        {
            get
            {
                if (_defaultFormatting == null)
                    _defaultFormatting = new Formatting(grammar);

                return _defaultFormatting;
            }
            set
            {
                _defaultFormatting = value;
            }
        }

        public void SetAutomaticParenthesesExplicitlyForExpression(BnfTerm expression, BnfTerm leftParenthesis, BnfTerm rightParenthesis)
        {
            expressionToParentheses.Add(expression, new ExpressionUnparser.Parentheses() { Expression = expression, Left = leftParenthesis, Right = rightParenthesis });
            ExpressionToParenthesesHasBeenManuallySet = true;
        }

        public void NoAutomaticParenthesesNeededForExpressions()
        {
            ExpressionToParenthesesHasBeenManuallySet = true;
        }

        public void ClearManualSetOfParenthesesForExpressions()
        {
            expressionToParentheses.Clear();
            ExpressionToParenthesesHasBeenManuallySet = false;
        }
    }
}
