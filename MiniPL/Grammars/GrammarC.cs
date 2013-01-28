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
                this.NAMESPACE = grammar.ToTerm("namespace");
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
                this.VAR = ToPunctuation("var");
                this.SEMICOLON = ToPunctuation(";");
                this.COLON = ToPunctuation(":");
                this.COMMA = ToPunctuation(",");
                this.LEFT_PAREN = ToPunctuation("(");
                this.RIGHT_PAREN = ToPunctuation(")");

                this.NUMBER = ParseNumber();
                this.IDENTIFIER = ParseIdentifier();
            }

            public readonly BnfiTermType<Program> Program = new BnfiTermType<Program>();
            public readonly BnfiTermType<Function> Function = new BnfiTermType<Function>();
            public readonly BnfiTermValue<Type> Type = new BnfiTermValue<Type>();
            public readonly BnfiTermType<LocalVariable> LocalVariable = new BnfiTermType<LocalVariable>();
            public readonly BnfiTermType<Parameter> Parameter = new BnfiTermType<Parameter>();
            public readonly BnfiTermType<Argument> Argument = new BnfiTermType<Argument>();
            public readonly BnfiTermChoice<Statement> Statement = new BnfiTermChoice<Statement>();
            public readonly BnfiTermType<While> While = new BnfiTermType<While>();
            public readonly BnfiTermType<For> For = new BnfiTermType<For>();
            public readonly BnfiTermType<If> If = new BnfiTermType<If>();
//            public readonly BnfiTermType<IfElse> IfElse = new BnfiTermType<IfElse>();
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

            public readonly BnfiTermValue<Type> INTEGER_TYPE = new BnfiTermValue<Type>();
            public readonly BnfiTermValue<Type> REAL_TYPE = new BnfiTermValue<Type>();

            public readonly BnfiTermValueTL NUMBER;
            public readonly BnfiTermValue<string> IDENTIFIER;

            public readonly BnfiTermType<Name> Name = new BnfiTermType<Name>();
            public readonly BnfiTermValue<NameRef> NamespaceName = new BnfiTermValue<NameRef>("namespace_name");
            public readonly BnfiTermValue<NameRef> NameRef = new BnfiTermValue<NameRef>();

            public readonly BnfiTermKeyTerm PROGRAM;
            public readonly BnfiTermKeyTerm NAMESPACE;
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
            public readonly BnfiTermKeyTerm VAR;
            public readonly BnfiTermKeyTerm SEMICOLON;
            public readonly BnfiTermKeyTerm COLON;
            public readonly BnfiTermKeyTerm COMMA;
            public readonly BnfiTermKeyTerm LEFT_PAREN;
            public readonly BnfiTermKeyTerm RIGHT_PAREN;
        }

        public readonly BnfTerms B;

        public GrammarC()
            : base(AstCreation.CreateAst, EmptyCollectionHandling.ReturnEmpty, ErrorHandling.ThrowException)
        {
            B = new BnfTerms(this);

            B.Program.Rule =
                B.PROGRAM
                + B.Name.BindMember(B.Program, t => t.Name)
                + (B.NAMESPACE + B.NamespaceName).QRef().BindMember(B.Program, t => t.Namespace)
                + B.Function.StarList().BindMember(B.Program, t => t.Functions)
                + B.BEGIN
                + B.Statement.PlusList().BindMember(B.Program, t => t.Body)
                + B.END
                + B.DOT
                ;

            B.Function.Rule =
                B.FUNCTION
                + B.Name.BindMember(B.Function, t => t.Name)
                + B.LEFT_PAREN
                + B.Parameter.StarList(B.COMMA).BindMember(B.Function, t => t.Parameters)
                + B.RIGHT_PAREN
                + (B.COLON + B.Type).QVal().BindMember(B.Function, t => t.ReturnType)
                + B.Statement.PlusList().BindMember(B.Function, t => t.Body)
                ;

            B.Statement.SetRuleOr(
                B.LocalVariable,
                B.Assignment,
                B.While,
                B.For,
                B.If,
                B.FunctionCall
                );

            B.LocalVariable.Rule =
                B.VAR
                + B.Name.BindMember(B.LocalVariable, t => t.Name)
                + B.SEMICOLON
                + B.Type.BindMember(B.LocalVariable, t => t.Type)
                + (B.LET + B.Expression).QRef().BindMember(B.LocalVariable, t => t.InitValue)
                ;

            B.Name.Rule = B.IDENTIFIER.BindMember(B.Name, t => t.Value);
            B.NameRef.Rule = B.IDENTIFIER.ConvertValue(_identifier => new NameRef(_identifier), _nameRef => _nameRef.Value);
            B.NamespaceName.Rule =
                B.IDENTIFIER
                .PlusList(B.DOT)
                .ConvertValue(
                    _identifiers => new NameRef(string.Join(B.DOT.Text, _identifiers)),
                    _nameRef => _nameRef.Value.Split(new string[] { B.DOT.Text }, StringSplitOptions.None)
                );
        }
    }
}
