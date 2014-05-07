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
using Sarcasm.Unparsing;
using Sarcasm.Unparsing.Styles;
using Sarcasm.DomainCore;
using Sarcasm.Utility;
using Sarcasm.Reflection;
using System.Globalization;

using D = Expr.DomainDefinitions;
using DC = Sarcasm.DomainCore;

namespace Expr.Grammars
{
    [Grammar(typeof(Domain), "Postfix")]
    public class GrammarPostfix : Sarcasm.GrammarAst.Grammar<D.Expression>
    {
        #region BnfTerms

        public class BnfTerms
        {
            internal BnfTerms(TerminalFactoryS TerminalFactoryS)
            {
                this.ADD_OP = TerminalFactoryS.CreateKeyTerm("+", D.BinaryOperator.Add);
                this.SUB_OP = TerminalFactoryS.CreateKeyTerm("-", D.BinaryOperator.Sub);
                this.MUL_OP = TerminalFactoryS.CreateKeyTerm("*", D.BinaryOperator.Mul);
                this.DIV_OP = TerminalFactoryS.CreateKeyTerm("/", D.BinaryOperator.Div);
                this.POW_OP = TerminalFactoryS.CreateKeyTerm("^", D.BinaryOperator.Pow);
                this.MOD_OP = TerminalFactoryS.CreateKeyTerm("%", D.BinaryOperator.Mod);

                this.POS_OP = TerminalFactoryS.CreateKeyTerm("+", D.UnaryOperator.Pos);
                this.NEG_OP = TerminalFactoryS.CreateKeyTerm("-", D.UnaryOperator.Neg);

                this.EQ_OP = TerminalFactoryS.CreateKeyTerm("==", D.BinaryOperator.Eq);
                this.NEQ_OP = TerminalFactoryS.CreateKeyTerm("<>", D.BinaryOperator.Neq);
                this.LT_OP = TerminalFactoryS.CreateKeyTerm("<", D.BinaryOperator.Lt);
                this.LTE_OP = TerminalFactoryS.CreateKeyTerm("<=", D.BinaryOperator.Lte);
                this.GT_OP = TerminalFactoryS.CreateKeyTerm(">", D.BinaryOperator.Gt);
                this.GTE_OP = TerminalFactoryS.CreateKeyTerm(">=", D.BinaryOperator.Gte);

                this.AND_OP = TerminalFactoryS.CreateKeyTerm("&&", D.BinaryOperator.And);
                this.OR_OP = TerminalFactoryS.CreateKeyTerm("||", D.BinaryOperator.Or);

                this.NOT_OP = TerminalFactoryS.CreateKeyTerm("!", D.UnaryOperator.Not);

                this.QUESTION_MARK_COLON = TerminalFactoryS.CreateKeyTerm("?:");

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
            public readonly BnfiTermKeyTerm QUESTION_MARK_COLON;
        }

        #endregion

        public readonly BnfTerms B;

        public GrammarPostfix()
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
                B.Expression.BindTo(B.BinaryExpression, t => t.Term1)
                + B.Expression.BindTo(B.BinaryExpression, t => t.Term2)
                + B.BinaryOperator.BindTo(B.BinaryExpression, t => t.Op)
                ;

            B.UnaryExpression.Rule =
                B.LEFT_PAREN
                + B.Expression.BindTo(B.UnaryExpression, t => t.Term)
                + B.UnaryOperator.BindTo(B.UnaryExpression, t => t.Op)
                + B.RIGHT_PAREN
                ;

            B.ConditionalTernaryExpression.Rule =
                B.Expression.BindTo(B.ConditionalTernaryExpression, t => t.Cond)
                + B.Expression.BindTo(B.ConditionalTernaryExpression, t => t.Term1)
                + B.Expression.BindTo(B.ConditionalTernaryExpression, t => t.Term2)
                + B.QUESTION_MARK_COLON
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

        [Formatter(typeof(GrammarPostfix), "Default")]
        public class Formatter : Sarcasm.Unparsing.Formatter
        {
            #region Settings

            public SyntaxHighlight SyntaxHighlight { get; set; }

            public Color ForeColorOfOperator { get; set; }
            public Color ForeColorOfLiteral { get; set; }

            #endregion

            private readonly BnfTerms B;

            public Formatter(GrammarPostfix grammar)
                : base(grammar)
            {
                this.B = grammar.B;

                SyntaxHighlight = GrammarPostfix.SyntaxHighlight.Color;

                ForeColorOfOperator = Color.Red;
                ForeColorOfLiteral = Color.ForestGreen;
            }

            public override IDecoration GetDecoration(Utoken utoken, UnparsableAst target)
            {
                var decoration = base.GetDecoration(utoken, target);

                decoration.Add(DecorationKey.FontFamily, FontFamily.GenericMonospace);

                if (target != null)
                {
                    if (SyntaxHighlight == GrammarPostfix.SyntaxHighlight.Color)
                        NormalSyntaxHighlight(utoken, target, decoration);
                }

                return decoration;
            }

            private void NormalSyntaxHighlight(Utoken utoken, UnparsableAst target, IDecoration decoration)
            {
                if (target.BnfTerm is KeyTerm)
                {
                    decoration
                        .Add(DecorationKey.Foreground, ForeColorOfOperator)
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

                else if (target.BnfTerm == B.UnaryOperator)
                    rightInsertedUtokens = UtokenInsert.NoWhitespace();
            }
        }

        #endregion
    }
}
