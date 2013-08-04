using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sarcasm.DomainCore;
using Sarcasm.Reflection;

namespace MiniPL
{
    [Domain("MiniPL")]
    public class Domain : Sarcasm.DomainCore.Domain<DomainDefinitions.Program>
    {
        public Domain()
        {
            this.EmptyCollectionHandling = EmptyCollectionHandling.ReturnEmpty;
        }
    }

    namespace DomainDefinitions
    {
        public class Program
        {
            public Name Name { get; set; }
            public NameRef Namespace { get; set; }
            [NonEmptyList]
            public IList<Statement> Body { get; set; }
            public IList<Function> Functions { get; set; }
        }

        public class Function
        {
            public Name Name { get; set; }
            public Type? ReturnType { get; set; }
            public IList<Parameter> Parameters { get; set; }
            [NonEmptyList]
            public IList<Statement> Body { get; set; }
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
            [NonEmptyList]
            public IList<Statement> Body { get; set; }
        }

        public class LocalVariable : Statement, IVariable
        {
            public Name Name { get; set; }
            public Type Type { get; set; }
            [Optional]
            public Expression InitValue { get; set; }
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
            public Expression Condition { get; set; }
            public Statement Body { get; set; }
        }

        public class For : Statement
        {
            public IList<LocalVariable> Init { get; set; }
            public Expression Condition { get; set; }
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
            public Expression Condition { get; set; }
            public Statement Body { get; set; }
            [Optional]
            public Statement ElseBody { get; set; }
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

        public class FunctionCall : Statement, Expression
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

        public class VariableReference : Expression
        {
            public Reference<IVariable> Target { get; set; }
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
