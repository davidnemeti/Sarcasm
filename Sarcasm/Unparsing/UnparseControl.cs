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
