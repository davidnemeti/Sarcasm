using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainCore;

namespace MiniPL.DomainModel
{
    [Domain("MiniPL", typeof(MiniPL.DomainModel.Program))]
    public static class MiniPLDomain { }

    public class Program
    {
        public Name Name { get; set; }
        public NameRef Namespace { get; set; }
        [NonEmptyList] public IList<Statement> Body { get; set; }
        public IList<Function> Functions { get; set; }
    }

    public class Function
    {
        public Name Name { get; set; }
        [Optional] public Type ReturnType { get; set; }
        public IList<Parameter> Parameters { get; set; }
        [NonEmptyList] public IList<Statement> Body { get; set; }
    }

    public enum Type
    {
        Integer,
        Real,
        String,
        Bool
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
        [Optional] public Expression InitValue { get; set; }
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
        public IList<LocalVariable> Init { get; set; }
        public Expression Condition { get; set; }
        public IList<Assignment> Update { get; set; }
        public IList<Statement> Body { get; set; }
    }

    public class Write : Statement
    {
    }

#if true
    public class If : Statement
    {
        public Expression Condition { get; set; }
        public IList<Statement> Body { get; set; }
        [Optional] public IList<Statement> ElseBody { get; set; }
    }
#else
    public class If : Statement
    {
        public Expression Condition { get; set; }
        public IList<Statement> Body { get; set; }
    }

    public class IfElse : If
    {
        public IList<Statement> ElseBody { get; set; }
    }
#endif

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

        public class NumberLiteral : Expression
        {
            public NumberLiteral() { }

            public NumberLiteral(object value)
            {
                this.Value = value;
            }

            public object Value { get; set; }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public class StringLiteral : Expression
        {
            public StringLiteral() { }

            public StringLiteral(string value)
            {
                this.Value = value;
            }

            public string Value { get; set; }

            public override string ToString()
            {
                return Value;
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
        Pow,
        Mod
    }

    public enum UnaryOperator
    {
        Pos,
        Neg
    }
}
