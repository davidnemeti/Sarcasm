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

using Sarcasm.DomainCore;
using Sarcasm.Reflection;

using DE = Expr.DomainDefinitions;

namespace MiniPL
{
    [Domain("MiniPL")]
    public class Domain : Sarcasm.DomainCore.Domain<DomainDefinitions.Program> { }

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
            Bool,
            Date,
            Color
        }

        public enum Color
        {
            Black,
            Blue,
            Brown,
            Gray,
            Green,
            Orange,
            Red,
            White,
            Yellow 
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
            public DE.Expression Expression { get; set; }
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
            public DE.Expression InitValue { get; set; }
        }

        public class Assignment : Statement
        {
            public VariableReference LValue { get; set; }
            public DE.Expression RValue { get; set; }
        }

        public class Return : Statement
        {
            public DE.Expression Value { get; set; }
        }

        public class While : Statement
        {
            public DE.Expression Condition { get; set; }
            public Statement Body { get; set; }
        }

        public class For : Statement
        {
            public IList<LocalVariable> Init { get; set; }
            public DE.Expression Condition { get; set; }
            public IList<Assignment> Update { get; set; }
            public Statement Body { get; set; }
        }

        public class Write : Statement
        {
            public IList<DE.Expression> Arguments;
        }

        public class WriteLn : Statement
        {
            public IList<DE.Expression> Arguments;
        }

#if true
        public class If : Statement
        {
            public DE.Expression Condition { get; set; }
            public Statement Body { get; set; }
            [Optional]
            public Statement ElseBody { get; set; }
        }
#else
    public class If : Statement
    {
        public DE.Expression Condition { get; set; }
        public Statement Body { get; set; }
    }

    public class IfElse : If
    {
        public Statement ElseBody { get; set; }
    }
#endif

        public class FunctionCall : Statement, DE.Expression
        {
            public Reference<Function> FunctionReference { get; set; }
            public IList<Argument> Arguments { get; set; }
        }

        public class StringLiteral : DE.Expression
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

        public class DateLiteral : DE.Expression
        {
            public const string Format = "d";

            public DateLiteral() { }

            public DateLiteral(DateTime value)
            {
                this.Value = value;
            }

            public DateTime Value { get; set; }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public class ColorLiteral : DE.Expression
        {
            public ColorLiteral() { }

            public ColorLiteral(Color value)
            {
                this.Value = value;
            }

            public Color Value { get; set; }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public class VariableReference : DE.Expression
        {
            public Reference<IVariable> Target { get; set; }
        }
    }
}
