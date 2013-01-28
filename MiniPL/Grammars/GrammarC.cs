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

using DomainCore;

using MiniPL.DomainModel;

using Type = MiniPL.DomainModel.Type;

namespace MiniPL
{
    [Grammar(typeof(MiniPLDomain))]
    public class GrammarC : Sarcasm.Ast.Grammar
    {
        public class BnfTerms
        {
            internal BnfTerms(Sarcasm.Ast.Grammar grammar)
            {
                this.PROGRAM = grammar.ToTerm("program");
                this.BEGIN = grammar.ToTerm("begin");
                this.END = grammar.ToTerm("end");
                this.FUNCTION = grammar.ToTerm("function");
                this.WHILE = grammar.ToTerm("while");
                this.FOR = grammar.ToTerm("for");
                this.IF = grammar.ToTerm("if");
                this.THEN = grammar.ToTerm("then");
                this.ELSE = grammar.ToTerm("else");
                this.DO = grammar.ToTerm("do");
                this.DOT = grammar.ToTerm(".");
                this.LET = ToPunctuation(":=");
                this.SEMICOLON = ToPunctuation(";");
                this.COMMA = ToPunctuation(",");
                this.LEFT_PAREN = ToPunctuation("(");
                this.RIGHT_PAREN = ToPunctuation(")");

                this.NUMBER = ParseNumber();
                this.IDENTIFIER = ParseIdentifier();
            }

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
            public readonly BnfiTermValue<UnaryOperator> MOD_OP = new BnfiTermValue<UnaryOperator>();

            public readonly BnfiTermValue<UnaryOperator> POS_OP = new BnfiTermValue<UnaryOperator>();
            public readonly BnfiTermValue<UnaryOperator> NEG_OP = new BnfiTermValue<UnaryOperator>();

            public readonly BnfiTermValueTL NUMBER;
            public readonly BnfiTermValue<string> IDENTIFIER;

            public readonly BnfiTermType<Name> name = new BnfiTermType<Name>();
            public readonly BnfiTermValue<NameRef> namespace_name = new BnfiTermValue<NameRef>("namespace_name");
            public readonly BnfiTermValue<NameRef> nameref = new BnfiTermValue<NameRef>();

            public readonly BnfiTermKeyTerm PROGRAM;
            public readonly BnfiTermKeyTerm BEGIN;
            public readonly BnfiTermKeyTerm END;
            public readonly BnfiTermKeyTerm FUNCTION;
            public readonly BnfiTermKeyTerm WHILE;
            public readonly BnfiTermKeyTerm FOR;
            public readonly BnfiTermKeyTerm IF;
            public readonly BnfiTermKeyTerm THEN;
            public readonly BnfiTermKeyTerm ELSE;
            public readonly BnfiTermKeyTerm DO;
            public readonly BnfiTermKeyTerm DOT;
            public readonly BnfiTermKeyTerm LET;
            public readonly BnfiTermKeyTerm SEMICOLON;
            public readonly BnfiTermKeyTerm COMMA;
            public readonly BnfiTermKeyTerm LEFT_PAREN;
            public readonly BnfiTermKeyTerm RIGHT_PAREN;
        }

        public readonly BnfTerms B;

        public GrammarC()
            : base(AstCreation.CreateAst, EmptyCollectionHandling.ReturnEmpty, ErrorHandling.ThrowException)
        {
            B = new BnfTerms(this);

//            B.Program.Rule = B.PROGRAM + B.

            B.name.Rule = B.IDENTIFIER.BindMember(B.name, t => t.Value);
            B.nameref.Rule = B.IDENTIFIER.ConvertValue(_identifier => new NameRef(_identifier), _nameRef => _nameRef.Value);
            B.namespace_name.Rule =
                B.IDENTIFIER
                .PlusList(B.DOT)
                .ConvertValue(
                    _identifiers => new NameRef(string.Join(B.DOT.Text, _identifiers)),
                    _nameRef => _nameRef.Value.Split(new string[] { B.DOT.Text }, StringSplitOptions.None)
                );
        }
    }
}
