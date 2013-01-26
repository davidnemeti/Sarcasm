using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainCore;

namespace MiniPL.DomainModel
{
    public class Program
    {
        public Name Name { get; set; }
        public IList<Statement> Body { get; set; }
        public IList<Function> Functions { get; set; }
    }

    public class Function
    {
        public Name Name { get; set; }
        public Type ReturnType { get; set; }
        public IList<Parameter> ParameterDefinitions { get; set; }
        public IList<Statement> Body { get; set; }
    }

    public enum Type
    {
        Integer,
        Real
    }

    public interface IVariable
    {
        Name Name { get; set; }
        Type Type { get; set; }
    }

    public class Parameter : IVariable
    {
        public Name Name { get; set; }
        public Type Type { get; set; }
    }

    public class Argument
    {
        public Expression Expression { get; set; }
    }

    public abstract class Statement
    {
    }

    public class LocalVariable : Statement, IVariable
    {
        public Name Name { get; set; }
        public Type Type { get; set; }
        public Expression InitValue { get; set; }
    }

    public class Assignment : Statement
    {
        public Reference<IVariable> VariableReference { get; set; }
        public Expression Expression { get; set; }
    }

    public class While : Statement
    {
        public Expression Condition { get; set; }
        public IList<Statement> Body { get; set; }
    }

    public class For : Statement
    {
        public IList<Statement> Init { get; set; }
        public Expression Condition { get; set; }
        public IList<Statement> Update { get; set; }
        public IList<Statement> Body { get; set; }
    }

    public class If : Statement
    {
        public Expression Condition { get; set; }
        public IList<Statement> Body { get; set; }
    }

    public class IfElse : If
    {
        public IList<Statement> ElseBody { get; set; }
    }

    public class FunctionCall : Statement
    {
        public Reference<Function> FunctionReference { get; set; }
        public IList<Argument> Arguments { get; set; }
    }

    public abstract class Expression
    {
        public class Binary : Expression
        {
            public Expression Term1 { get; set; }
            public BinaryOperator Op { get; set; }
            public Expression Term2 { get; set; }

            public override string ToString()
            {
                return string.Format("({0} {1} {2})", Term1, Op, Term2);
            }
        }

        public class Unary : Expression
        {
            public UnaryOperator Op { get; set; }
            public Expression Term { get; set; }

            public override string ToString()
            {
                return string.Format("({0} {1})", Op, Term);
            }
        }

        public class Number : Expression
        {
            public Number() { }

            public Number(object value)
            {
                this.Value = value;
            }

            public object Value { get; set; }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public class VariableReference : Expression
        {
            public Reference<IVariable> Target { get; set; }
        }
    }

    public enum BinaryOperator
    {
        Add,
        Sub,
        Mul,
        Div,
        Pow
    }

    public enum UnaryOperator
    {
        Pos,
        Neg
    }
}
