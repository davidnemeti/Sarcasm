using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.Ast;
using Sarcasm.Unparsing;

using MiniPL.DomainModel;

using Type = MiniPL.DomainModel.Type;

namespace MiniPL
{
    public class GrammarC : Irony.Parsing.Grammar
    {
        public class BnfTerms
        {
            internal BnfTerms() { }

            public readonly BnfiTermType<Program> Program = new BnfiTermType<Program>();
            public readonly BnfiTermType<Function> Function = new BnfiTermType<Function>();
            public readonly BnfiTermType<Type> Type = new BnfiTermType<Type>();
            public readonly BnfiTermType<LocalVariable> LocalVariable = new BnfiTermType<LocalVariable>();
            public readonly BnfiTermType<Parameter> Parameter = new BnfiTermType<Parameter>();
            public readonly BnfiTermType<Argument> Argument = new BnfiTermType<Argument>();
            public readonly BnfiTermChoice<Statement> Statement = new BnfiTermChoice<Statement>();
            public readonly BnfiTermType<While> While = new BnfiTermType<While>();
            public readonly BnfiTermType<For> For = new BnfiTermType<For>();
            public readonly BnfiTermType<If> If = new BnfiTermType<If>();
            public readonly BnfiTermType<IfElse> IfElse = new BnfiTermType<IfElse>();
            public readonly BnfiTermType<Assignment> Assignment = new BnfiTermType<Assignment>();
            public readonly BnfiTermType<FunctionCall> FunctionCall = new BnfiTermType<FunctionCall>();

            public readonly BnfiTermChoice<Expression> Expression = new BnfiTermChoice<Expression>();
            public readonly BnfiTermType<Expression.Binary> BinaryExpression = new BnfiTermType<Expression.Binary>();
            public readonly BnfiTermType<Expression.Unary> UnaryExpression = new BnfiTermType<Expression.Unary>();
            public readonly BnfiTermType<Expression.Number> NumberExpression = new BnfiTermType<Expression.Number>();
            public readonly BnfiTermType<Expression.VariableReference> VariableReferenceExpression = new BnfiTermType<Expression.VariableReference>();

            public readonly BnfiTermChoice<BinaryOperator> BinaryOperator = new BnfiTermChoice<BinaryOperator>();
            public readonly BnfiTermChoice<UnaryOperator> UnaryOperator = new BnfiTermChoice<UnaryOperator>();

            public readonly BnfiTermValue<UnaryOperator> ADD_OP = new BnfiTermValue<UnaryOperator>();
            public readonly BnfiTermValue<UnaryOperator> SUB_OP = new BnfiTermValue<UnaryOperator>();
            public readonly BnfiTermValue<UnaryOperator> MUL_OP = new BnfiTermValue<UnaryOperator>();
            public readonly BnfiTermValue<UnaryOperator> DIW_OP = new BnfiTermValue<UnaryOperator>();
            public readonly BnfiTermValue<UnaryOperator> POW_OP = new BnfiTermValue<UnaryOperator>();

            public readonly BnfiTermValue<UnaryOperator> POS_OP = new BnfiTermValue<UnaryOperator>();
            public readonly BnfiTermValue<UnaryOperator> NEG_OP = new BnfiTermValue<UnaryOperator>();
        }

        public readonly BnfTerms B = new BnfTerms();

        public GrammarC()
        {
        }
    }
}
