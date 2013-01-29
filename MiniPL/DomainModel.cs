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
        public Type? ReturnType { get; set; }
        public IList<Parameter> Parameters { get; set; }
        [NonEmptyList] public IList<Statement> Body { get; set; }
    }

    public enum Type
    {
        Integer,
        Real,
        String,
        Char,
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

    public class StatementList : Statement
    {
        [NonEmptyList] public IList<Statement> Body { get; set; }
    }

    public class LocalVariable : Statement, IVariable
    {
        public Name Name { get; set; }
        public Type Type { get; set; }
        [Optional] public Expression InitValue { get; set; }
    }

    public class Assignment : Statement
    {
        public VariableReference LValue { get; set; }
        public Expression RValue { get; set; }
    }

    public class Return : Statement
    {
        public Expression Value { get; set; }
    }

    public class While : Statement
    {
        public BoolExpression Condition { get; set; }
        public Statement Body { get; set; }
    }

    public class For : Statement
    {
        public IList<LocalVariable> Init { get; set; }
        public BoolExpression Condition { get; set; }
        public IList<Assignment> Update { get; set; }
        public Statement Body { get; set; }
    }

    public class Write : Statement
    {
        public IList<Expression> Arguments;
    }

    public class WriteLn : Statement
    {
        public IList<Expression> Arguments;
    }

#if true
    public class If : Statement
    {
        public BoolExpression Condition { get; set; }
        public Statement Body { get; set; }
        [Optional] public Statement ElseBody { get; set; }
    }
#else
    public class If : Statement
    {
        public BoolExpression Condition { get; set; }
        public Statement Body { get; set; }
    }

    public class IfElse : If
    {
        public Statement ElseBody { get; set; }
    }
#endif

    public class FunctionCall : Statement, Expression, BoolExpression
    {
        public Reference<Function> FunctionReference { get; set; }
        public IList<Argument> Arguments { get; set; }
    }

    public interface Expression
    {
    }

    public class BinaryExpression : Expression
    {
        public Expression Term1 { get; set; }
        public MathBinaryOperator Op { get; set; }
        public Expression Term2 { get; set; }

        public override string ToString()
        {
            return string.Format("({0} {1} {2})", Term1, Op, Term2);
        }
    }

    public class UnaryExpression : Expression
    {
        public MathUnaryOperator Op { get; set; }
        public Expression Term { get; set; }

        public override string ToString()
        {
            return string.Format("({0} {1})", Op, Term);
        }
    }

    public class ConditionalTernaryExpression : Expression
    {
        public BoolExpression Cond { get; set; }
        public Expression Term1 { get; set; }
        public Expression Term2 { get; set; }

        public override string ToString()
        {
            return string.Format("({0} ? {1} : {2})", Cond, Term1, Term2);
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

    public interface BoolExpression : Expression
    {
    }

    public class RelationalBinaryBoolExpression : BoolExpression
    {
        public Expression Term1 { get; set; }
        public RelationalBinaryOperator Op { get; set; }
        public Expression Term2 { get; set; }

        public override string ToString()
        {
            return string.Format("({0} {1} {2})", Term1, Op, Term2);
        }
    }

    public class LogicalBinaryBoolExpression : BoolExpression
    {
        public BoolExpression Term1 { get; set; }
        public LogicalBinaryOperator Op { get; set; }
        public BoolExpression Term2 { get; set; }

        public override string ToString()
        {
            return string.Format("({0} {1} {2})", Term1, Op, Term2);
        }
    }

    public class LogicalUnaryBoolExpression : BoolExpression
    {
        public LogicalUnaryOperator Op { get; set; }
        public BoolExpression Term { get; set; }

        public override string ToString()
        {
            return string.Format("({0} {1})", Op, Term);
        }
    }

    public class BoolLiteral : BoolExpression
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

    public class VariableReference : Expression, BoolExpression
    {
        public Reference<IVariable> Target { get; set; }
    }

    public enum MathBinaryOperator
    {
        Add,
        Sub,
        Mul,
        Div,
        Pow,
        Mod
    }

    public enum MathUnaryOperator
    {
        Pos,
        Neg
    }

    public enum RelationalBinaryOperator
    {
        Eq,
        Neq,
        Lt,
        Lte,
        Gt,
        Gte,
    }

    public enum LogicalBinaryOperator
    {
        And,
        Or
    }

    public enum LogicalUnaryOperator
    {
        Not
    }
}
