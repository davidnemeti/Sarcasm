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

using Irony.Parsing;
using Sarcasm.GrammarAst;
using Sarcasm.Unparsing;
using Sarcasm.Unparsing.Styles;
using Sarcasm.DomainCore;
using Sarcasm.Reflection;

using D = Expr.DomainDefinitions;

namespace Expr.Grammars
{
    [Grammar(typeof(Domain), "Function-style")]
    public class GrammarFunction : Sarcasm.GrammarAst.Grammar<D.Expression>
    {
        #region BnfTerms

        public class BnfTerms
        {
            internal BnfTerms(TerminalFactoryS TerminalFactoryS)
            {
                this.ADD_OP = TerminalFactoryS.CreateKeyTerm("Add", D.BinaryOperator.Add);
                this.SUB_OP = TerminalFactoryS.CreateKeyTerm("Sub", D.BinaryOperator.Sub);
                this.MUL_OP = TerminalFactoryS.CreateKeyTerm("Mul", D.BinaryOperator.Mul);
                this.DIV_OP = TerminalFactoryS.CreateKeyTerm("Div", D.BinaryOperator.Div);
                this.POW_OP = TerminalFactoryS.CreateKeyTerm("Pow", D.BinaryOperator.Pow);
                this.MOD_OP = TerminalFactoryS.CreateKeyTerm("Mod", D.BinaryOperator.Mod);

                this.POS_OP = TerminalFactoryS.CreateKeyTerm("Pos", D.UnaryOperator.Pos);
                this.NEG_OP = TerminalFactoryS.CreateKeyTerm("Neg", D.UnaryOperator.Neg);

                this.EQ_OP = TerminalFactoryS.CreateKeyTerm("Eq", D.BinaryOperator.Eq);
                this.NEQ_OP = TerminalFactoryS.CreateKeyTerm("Neq", D.BinaryOperator.Neq);
                this.LT_OP = TerminalFactoryS.CreateKeyTerm("Lt", D.BinaryOperator.Lt);
                this.LTE_OP = TerminalFactoryS.CreateKeyTerm("Lte", D.BinaryOperator.Lte);
                this.GT_OP = TerminalFactoryS.CreateKeyTerm("Gt", D.BinaryOperator.Gt);
                this.GTE_OP = TerminalFactoryS.CreateKeyTerm("Gte", D.BinaryOperator.Gte);

                this.AND_OP = TerminalFactoryS.CreateKeyTerm("And", D.BinaryOperator.And);
                this.OR_OP = TerminalFactoryS.CreateKeyTerm("Or", D.BinaryOperator.Or);

                this.NOT_OP = TerminalFactoryS.CreateKeyTerm("Not", D.UnaryOperator.Not);

                this.CONDITIONAL_TERNARY = TerminalFactoryS.CreateKeyTerm("Cond");

                this.COMMA = TerminalFactoryS.CreateKeyTerm(",");

                this.BOOL_CONSTANT = new BnfiTermConstant<bool>()
                {
                    { "true", true },
                    { "false", false }
                };

                this.LEFT_PAREN = TerminalFactoryS.CreateKeyTerm("(");
                this.RIGHT_PAREN = TerminalFactoryS.CreateKeyTerm(")");
            }

            public readonly BnfiTermChoice<D.Expression> Expression = new BnfiTermChoice<D.Expression>();
            public readonly BnfiTermRecord<D.BinaryExpression> BinaryExpression = new BnfiTermRecord<D.BinaryExpression>();
            public readonly BnfiTermRecord<D.UnaryExpression> UnaryExpression = new BnfiTermRecord<D.UnaryExpression>();
            public readonly BnfiTermRecord<D.ConditionalTernaryExpression> ConditionalTernaryExpression = new BnfiTermRecord<D.ConditionalTernaryExpression>();

            public readonly BnfiTermChoice<D.BinaryOperator> BinaryOperator = new BnfiTermChoice<D.BinaryOperator>();
            public readonly BnfiTermChoice<D.UnaryOperator> UnaryOperator = new BnfiTermChoice<D.UnaryOperator>();

            public readonly BnfiTermConversion<D.NumberLiteral> NumberLiteral = new BnfiTermConversion<D.NumberLiteral>();
            public readonly BnfiTermRecord<D.BoolLiteral> BoolLiteral = new BnfiTermRecord<D.BoolLiteral>();

            public readonly BnfiTermConversion<D.BinaryOperator> ADD_OP;
            public readonly BnfiTermConversion<D.BinaryOperator> SUB_OP;
            public readonly BnfiTermConversion<D.BinaryOperator> MUL_OP;
            public readonly BnfiTermConversion<D.BinaryOperator> DIV_OP;
            public readonly BnfiTermConversion<D.BinaryOperator> POW_OP;
            public readonly BnfiTermConversion<D.BinaryOperator> MOD_OP;

            public readonly BnfiTermConversion<D.UnaryOperator> POS_OP;
            public readonly BnfiTermConversion<D.UnaryOperator> NEG_OP;

            public readonly BnfiTermConversion<D.BinaryOperator> EQ_OP;
            public readonly BnfiTermConversion<D.BinaryOperator> NEQ_OP;
            public readonly BnfiTermConversion<D.BinaryOperator> LT_OP;
            public readonly BnfiTermConversion<D.BinaryOperator> LTE_OP;
            public readonly BnfiTermConversion<D.BinaryOperator> GT_OP;
            public readonly BnfiTermConversion<D.BinaryOperator> GTE_OP;

            public readonly BnfiTermConversion<D.BinaryOperator> AND_OP;
            public readonly BnfiTermConversion<D.BinaryOperator> OR_OP;

            public readonly BnfiTermConversion<D.UnaryOperator> NOT_OP;

            public readonly BnfiTermConstant<bool> BOOL_CONSTANT;

            public readonly BnfiTermKeyTerm LEFT_PAREN;
            public readonly BnfiTermKeyTerm RIGHT_PAREN;
            public readonly BnfiTermKeyTerm CONDITIONAL_TERNARY;
            public readonly BnfiTermKeyTerm COMMA;
        }

        #endregion

        public readonly BnfTerms B;

        public GrammarFunction()
            : base(new Domain())
        {
            B = new BnfTerms(new TerminalFactoryS(this));

            this.Root = B.Expression;

            B.Expression.SetRuleOr(
                B.BinaryExpression,
                B.UnaryExpression,
                B.ConditionalTernaryExpression,
                B.NumberLiteral,
                B.BoolLiteral
                );

            B.BinaryExpression.Rule =
                B.BinaryOperator.BindTo(B.BinaryExpression, t => t.Op)
                + B.LEFT_PAREN
                + B.Expression.BindTo(B.BinaryExpression, t => t.Term1)
                + B.COMMA
                + B.Expression.BindTo(B.BinaryExpression, t => t.Term2)
                + B.RIGHT_PAREN
                ;

            B.UnaryExpression.Rule =
                B.UnaryOperator.BindTo(B.UnaryExpression, t => t.Op)
                + B.LEFT_PAREN
                + B.Expression.BindTo(B.UnaryExpression, t => t.Term)
                + B.RIGHT_PAREN
                ;

            B.ConditionalTernaryExpression.Rule =
                B.CONDITIONAL_TERNARY
                + B.LEFT_PAREN
                + B.Expression.BindTo(B.ConditionalTernaryExpression, t => t.Cond)
                + B.COMMA
                + B.Expression.BindTo(B.ConditionalTernaryExpression, t => t.Term1)
                + B.COMMA
                + B.Expression.BindTo(B.ConditionalTernaryExpression, t => t.Term2)
                + B.RIGHT_PAREN
                ;

            var numberLiteralInfo = new NumberLiteralInfo()
                .AddPrefix("#b", NumberLiteralBase.Binary)
                .AddPrefix("#o", NumberLiteralBase.Octal)
                .AddPrefix("#x", NumberLiteralBase.Hexadecimal);

            B.NumberLiteral.Rule = TerminalFactoryS.CreateNumberLiteral<D.NumberLiteral>(numberLiteralInfo);
            B.BoolLiteral.Rule = B.BOOL_CONSTANT.BindTo(B.BoolLiteral, t => t.Value);

            B.BinaryOperator.Rule = B.ADD_OP | B.SUB_OP | B.MUL_OP | B.DIV_OP | B.POW_OP | B.MOD_OP | B.EQ_OP | B.NEQ_OP | B.LT_OP | B.LTE_OP | B.GT_OP | B.GTE_OP | B.AND_OP | B.OR_OP;
            B.UnaryOperator.Rule = B.POS_OP | B.NEG_OP | B.NOT_OP;

            RegisterBracePair(B.LEFT_PAREN, B.RIGHT_PAREN);
        }

        #region Unparse

        protected override UnparseControl GetUnparseControl()
        {
            return UnparseControl.Create(this, new Formatter(this), new ParenthesizedExpression(B.Expression, B.LEFT_PAREN, B.RIGHT_PAREN));
        }

        #endregion

        #region Formatter

        public enum SyntaxHighlight { Color, BlackAndWhite }

        [Formatter(typeof(GrammarFunction), "Default")]
        public class Formatter : Sarcasm.Unparsing.Formatter
        {
            #region Settings

            public SyntaxHighlight SyntaxHighlight { get; set; }

            public Color ForeColorOfParentheses { get; set; }
            public Color ForeColorOfLiteral { get; set; }

            #endregion

            private readonly BnfTerms B;

            public Formatter(GrammarFunction grammar)
                : base(grammar)
            {
                this.B = grammar.B;

                SyntaxHighlight = GrammarFunction.SyntaxHighlight.Color;

                ForeColorOfParentheses = Color.Red;
                ForeColorOfLiteral = Color.ForestGreen;
            }

            public override IDecoration GetDecoration(Utoken utoken, UnparsableAst target)
            {
                var decoration = base.GetDecoration(utoken, target);

                decoration.Add(DecorationKey.FontFamily, FontFamily.GenericMonospace);

                if (target != null)
                {
                    if (SyntaxHighlight == GrammarFunction.SyntaxHighlight.Color)
                        NormalSyntaxHighlight(utoken, target, decoration);
                }

                return decoration;
            }

            private void NormalSyntaxHighlight(Utoken utoken, UnparsableAst target, IDecoration decoration)
            {
                if (target.BnfTerm.IsBrace())
                {
                    decoration
                        .Add(DecorationKey.Foreground, ForeColorOfParentheses)
                        ;
                }
                else if (target.BnfTerm.IsLiteral() || target.BnfTerm.IsConstant())
                {
                    decoration
                        .Add(DecorationKey.Foreground, ForeColorOfLiteral)
                        ;
                }
            }

            public override void GetUtokensAround(UnparsableAst target, out InsertedUtokens leftInsertedUtokens, out InsertedUtokens rightInsertedUtokens)
            {
                base.GetUtokensAround(target, out leftInsertedUtokens, out rightInsertedUtokens);

                if (target.BnfTerm == B.RIGHT_PAREN)
                    leftInsertedUtokens = UtokenInsert.NoWhitespace();

                else if (target.BnfTerm == B.LEFT_PAREN)
                    rightInsertedUtokens = UtokenInsert.NoWhitespace();

                else if (target.BnfTerm == B.COMMA)
                    leftInsertedUtokens = UtokenInsert.NoWhitespace();

                else if (target.BnfTerm == B.UnaryOperator)
                    rightInsertedUtokens = UtokenInsert.NoWhitespace();
            }

            public override InsertedUtokens GetUtokensBetween(UnparsableAst leftTerminalLeaveTarget, UnparsableAst rightTarget)
            {
                if (leftTerminalLeaveTarget.BnfTerm is KeyTerm && rightTarget.BnfTerm == B.LEFT_PAREN)
                    return UtokenInsert.NoWhitespace();

                else
                    return base.GetUtokensBetween(leftTerminalLeaveTarget, rightTarget);
            }
        }

        #endregion
    }
}
