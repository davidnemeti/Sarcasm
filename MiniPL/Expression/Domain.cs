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

using Sarcasm.DomainCore;
using Sarcasm.Reflection;

namespace Expr
{
    [Domain("Expression")]
    public class Domain : Sarcasm.DomainCore.Domain<DomainDefinitions.Expression> { }

    namespace DomainDefinitions
    {
        public interface Expression
        {
        }

        public class BinaryExpression : Expression
        {
            public Expression Term1 { get; set; }
            public BinaryOperator Op { get; set; }
            public Expression Term2 { get; set; }

            public override string ToString()
            {
                return string.Format("({0} {1} {2})", Term1, Op, Term2);
            }
        }

        public class UnaryExpression : Expression
        {
            public UnaryOperator Op { get; set; }
            public Expression Term { get; set; }

            public override string ToString()
            {
                return string.Format("({0} {1})", Op, Term);
            }
        }

        public class ConditionalTernaryExpression : Expression
        {
            public Expression Cond { get; set; }
            public Expression Term1 { get; set; }
            public Expression Term2 { get; set; }

            public override string ToString()
            {
                return string.Format("({0} ? {1} : {2})", Cond, Term1, Term2);
            }
        }

        public class NumberLiteral : Expression, INumberLiteral
        {
            public const NumberLiteralBase DefaultBase = NumberLiteralBase.Decimal;

            public NumberLiteral()
            {
                this.Base = DefaultBase;
            }

            public NumberLiteral(object value)
                : this()
            {
                this.Value = value;
            }

            public object Value { get; set; }
            public NumberLiteralBase Base { get; set; }
            public bool HasExplicitTypeModifier { get; set; }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public class BoolLiteral : Expression
        {
            public BoolLiteral() { }

            public BoolLiteral(bool value)
            {
                this.Value = value;
            }

            public bool Value { get; set; }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public enum BinaryOperator
        {
            Add,
            Sub,
            Mul,
            Div,
            Pow,
            Mod,
            Eq,
            Neq,
            Lt,
            Lte,
            Gt,
            Gte,
            And,
            Or
        }

        public enum UnaryOperator
        {
            Pos,
            Neg,
            Not
        }
    }
}
