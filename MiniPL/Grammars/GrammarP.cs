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
using NumberLiteral = MiniPL.DomainModel.NumberLiteral;
using StringLiteral = MiniPL.DomainModel.StringLiteral;

namespace MiniPL
{
    [Grammar(typeof(MiniPLDomain), "Pascal-like grammar")]
    public class GrammarP : Sarcasm.Ast.Grammar
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
                this.RETURN = grammar.ToTerm("return");
                this.WRITE = grammar.ToTerm("Write");
                this.WRITELN = grammar.ToTerm("WriteLn");
                this.DOT = ToPunctuation(".");
                this.LET = ToPunctuation(":=");
                this.VAR = ToPunctuation("var");
                this.SEMICOLON = ToPunctuation(";");
                this.COLON = ToPunctuation(":");
                this.COMMA = ToPunctuation(",");
                this.LEFT_PAREN = ToPunctuation("(");
                this.RIGHT_PAREN = ToPunctuation(")");

                this.ADD_OP = grammar.ToTerm("+", DomainModel.MathBinaryOperator.Add);
                this.SUB_OP = grammar.ToTerm("-", DomainModel.MathBinaryOperator.Sub);
                this.MUL_OP = grammar.ToTerm("*", DomainModel.MathBinaryOperator.Mul);
                this.DIV_OP = grammar.ToTerm("/", DomainModel.MathBinaryOperator.Div);
                this.POW_OP = grammar.ToTerm("^", DomainModel.MathBinaryOperator.Pow);
                this.MOD_OP = grammar.ToTerm("%", DomainModel.MathBinaryOperator.Mod);

                this.POS_OP = grammar.ToTerm("+", DomainModel.MathUnaryOperator.Pos);
                this.NEG_OP = grammar.ToTerm("-", DomainModel.MathUnaryOperator.Neg);

                this.EQ_OP = grammar.ToTerm("=", DomainModel.RelationalBinaryOperator.Eq);
                this.NEQ_OP = grammar.ToTerm("<>", DomainModel.RelationalBinaryOperator.Neq);
                this.LT_OP = grammar.ToTerm("<", DomainModel.RelationalBinaryOperator.Lt);
                this.LTE_OP = grammar.ToTerm("<=", DomainModel.RelationalBinaryOperator.Lte);
                this.GT_OP = grammar.ToTerm(">", DomainModel.RelationalBinaryOperator.Gt);
                this.GTE_OP = grammar.ToTerm(">=", DomainModel.RelationalBinaryOperator.Gte);

                this.AND_OP = grammar.ToTerm("and", DomainModel.LogicalBinaryOperator.And);
                this.OR_OP = grammar.ToTerm("or", DomainModel.LogicalBinaryOperator.Or);

                this.NOT_OP = grammar.ToTerm("not", DomainModel.LogicalUnaryOperator.Not);

                this.INTEGER_TYPE = grammar.ToTerm("integer", DomainModel.Type.Integer);
                this.REAL_TYPE = grammar.ToTerm("real", DomainModel.Type.Real);
                this.STRING_TYPE = grammar.ToTerm("string", DomainModel.Type.String);
                this.CHAR_TYPE = grammar.ToTerm("char", DomainModel.Type.Char);
                this.BOOL_TYPE = grammar.ToTerm("boolean", DomainModel.Type.Bool);

                this.IDENTIFIER = CreateIdentifier();

//                this.ADD_OP = grammar.ToTerm();
                this.BOOL_CONSTANT = new BnfiTermConstant<bool>()
                {
                    { "true", true },
                    { "false", false }
                };
            }

            public readonly BnfiTermType<Program> Program = new BnfiTermType<Program>();
            public readonly BnfiTermType<Function> Function = new BnfiTermType<Function>();
            public readonly BnfiTermChoice<Type> Type = new BnfiTermChoice<Type>();
            public readonly BnfiTermType<LocalVariable> LocalVariable = new BnfiTermType<LocalVariable>();
            public readonly BnfiTermType<Parameter> Parameter = new BnfiTermType<Parameter>();
            public readonly BnfiTermType<Argument> Argument = new BnfiTermType<Argument>();
            public readonly BnfiTermChoice<Statement> Statement = new BnfiTermChoice<Statement>();
            public readonly BnfiTermType<StatementList> StatementList = new BnfiTermType<StatementList>();
            public readonly BnfiTermType<While> While = new BnfiTermType<While>();
            public readonly BnfiTermType<For> For = new BnfiTermType<For>();
            public readonly BnfiTermType<If> If = new BnfiTermType<If>();
//            public readonly BnfiTermType<IfElse> IfElse = new BnfiTermType<IfElse>();
            public readonly BnfiTermType<Return> Return = new BnfiTermType<Return>();
            public readonly BnfiTermType<Assignment> Assignment = new BnfiTermType<Assignment>();
            public readonly BnfiTermValue<Reference<Function>> FunctionReference = new BnfiTermValue<Reference<Function>>();
            public readonly BnfiTermType<FunctionCall> FunctionCall = new BnfiTermType<FunctionCall>();
            public readonly BnfiTermType<Write> Write = new BnfiTermType<Write>();
            public readonly BnfiTermType<WriteLn> WriteLn = new BnfiTermType<WriteLn>();
            public readonly BnfiTermType<VariableReference> VariableReference = new BnfiTermType<VariableReference>();

            public readonly BnfiTermChoice<Expression> Expression = new BnfiTermChoice<Expression>();
            public readonly BnfiTermType<BinaryExpression> BinaryExpression = new BnfiTermType<BinaryExpression>();
            public readonly BnfiTermType<UnaryExpression> UnaryExpression = new BnfiTermType<UnaryExpression>();
            public readonly BnfiTermType<ConditionalTernaryExpression> ConditionalTernaryExpression = new BnfiTermType<ConditionalTernaryExpression>();

            public readonly BnfiTermChoice<BoolExpression> BoolExpression = new BnfiTermChoice<BoolExpression>();
            public readonly BnfiTermType<RelationalBinaryBoolExpression> RelationalBinaryBoolExpression = new BnfiTermType<RelationalBinaryBoolExpression>();
            public readonly BnfiTermType<LogicalBinaryBoolExpression> LogicalBinaryBoolExpression = new BnfiTermType<LogicalBinaryBoolExpression>();
            public readonly BnfiTermType<LogicalUnaryBoolExpression> LogicalUnaryBoolExpression = new BnfiTermType<LogicalUnaryBoolExpression>();

            public readonly BnfiTermChoice<MathBinaryOperator> MathBinaryOperator = new BnfiTermChoice<MathBinaryOperator>();
            public readonly BnfiTermChoice<MathUnaryOperator> MathUnaryOperator = new BnfiTermChoice<MathUnaryOperator>();
            public readonly BnfiTermChoice<RelationalBinaryOperator> RelationalBinaryOperator = new BnfiTermChoice<RelationalBinaryOperator>();
            public readonly BnfiTermChoice<LogicalBinaryOperator> LogicalBinaryOperator = new BnfiTermChoice<LogicalBinaryOperator>();
            public readonly BnfiTermChoice<LogicalUnaryOperator> LogicalUnaryOperator = new BnfiTermChoice<LogicalUnaryOperator>();

            public readonly BnfiTermValue<MathBinaryOperator> ADD_OP = new BnfiTermValue<MathBinaryOperator>();
            public readonly BnfiTermValue<MathBinaryOperator> SUB_OP = new BnfiTermValue<MathBinaryOperator>();
            public readonly BnfiTermValue<MathBinaryOperator> MUL_OP = new BnfiTermValue<MathBinaryOperator>();
            public readonly BnfiTermValue<MathBinaryOperator> DIV_OP = new BnfiTermValue<MathBinaryOperator>();
            public readonly BnfiTermValue<MathBinaryOperator> POW_OP = new BnfiTermValue<MathBinaryOperator>();
            public readonly BnfiTermValue<MathBinaryOperator> MOD_OP = new BnfiTermValue<MathBinaryOperator>();

            public readonly BnfiTermValue<MathUnaryOperator> POS_OP = new BnfiTermValue<MathUnaryOperator>();
            public readonly BnfiTermValue<MathUnaryOperator> NEG_OP = new BnfiTermValue<MathUnaryOperator>();

            public readonly BnfiTermValue<RelationalBinaryOperator> EQ_OP = new BnfiTermValue<RelationalBinaryOperator>();
            public readonly BnfiTermValue<RelationalBinaryOperator> NEQ_OP = new BnfiTermValue<RelationalBinaryOperator>();
            public readonly BnfiTermValue<RelationalBinaryOperator> LT_OP = new BnfiTermValue<RelationalBinaryOperator>();
            public readonly BnfiTermValue<RelationalBinaryOperator> LTE_OP = new BnfiTermValue<RelationalBinaryOperator>();
            public readonly BnfiTermValue<RelationalBinaryOperator> GT_OP = new BnfiTermValue<RelationalBinaryOperator>();
            public readonly BnfiTermValue<RelationalBinaryOperator> GTE_OP = new BnfiTermValue<RelationalBinaryOperator>();

            public readonly BnfiTermValue<LogicalBinaryOperator> AND_OP = new BnfiTermValue<LogicalBinaryOperator>();
            public readonly BnfiTermValue<LogicalBinaryOperator> OR_OP = new BnfiTermValue<LogicalBinaryOperator>();

            public readonly BnfiTermValue<LogicalUnaryOperator> NOT_OP = new BnfiTermValue<LogicalUnaryOperator>();

            public readonly BnfiTermValue<Type> INTEGER_TYPE = new BnfiTermValue<Type>();
            public readonly BnfiTermValue<Type> REAL_TYPE = new BnfiTermValue<Type>();
            public readonly BnfiTermValue<Type> STRING_TYPE = new BnfiTermValue<Type>();
            public readonly BnfiTermValue<Type> CHAR_TYPE = new BnfiTermValue<Type>();
            public readonly BnfiTermValue<Type> BOOL_TYPE = new BnfiTermValue<Type>();

            public readonly BnfiTermType<NumberLiteral> NumberLiteral;
            public readonly BnfiTermType<StringLiteral> StringLiteral;
            public readonly BnfiTermType<BoolLiteral> BoolLiteral;
            public readonly BnfiTermValue<string> IDENTIFIER;
            public readonly BnfiTermConstant<bool> BOOL_CONSTANT;

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
            public readonly BnfiTermKeyTerm RETURN;
            public readonly BnfiTermKeyTerm WRITE;
            public readonly BnfiTermKeyTerm WRITELN;
            public readonly BnfiTermKeyTermPunctuation DOT;
            public readonly BnfiTermKeyTermPunctuation LET;
            public readonly BnfiTermKeyTermPunctuation VAR;
            public readonly BnfiTermKeyTermPunctuation SEMICOLON;
            public readonly BnfiTermKeyTermPunctuation COLON;
            public readonly BnfiTermKeyTermPunctuation COMMA;
            public readonly BnfiTermKeyTermPunctuation LEFT_PAREN;
            public readonly BnfiTermKeyTermPunctuation RIGHT_PAREN;
        }

        public readonly BnfTerms B;

        public GrammarP()
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

            B.Parameter.Rule =
                B.VAR
                + B.Name.BindMember(B.Parameter, t => t.Name)
                + B.COLON
                + B.Type.BindMember(B.Parameter, t => t.Type)
                ;

            B.Statement.SetRuleOr(
                B.LocalVariable + B.SEMICOLON,
                B.Assignment + B.SEMICOLON,
                B.While,
                B.For,
                B.If,
                B.FunctionCall + B.SEMICOLON,
                B.Return + B.SEMICOLON,
                B.StatementList
                );

            B.Return.Rule =
                B.RETURN
                + B.Expression.BindMember(B.Return, t => t.Value)
                ;

            B.LocalVariable.Rule =
                B.VAR
                + B.Name.BindMember(B.LocalVariable, t => t.Name)
                + B.COLON
                + B.Type.BindMember(B.LocalVariable, t => t.Type)
                + (B.LET + B.Expression).QRef().BindMember(B.LocalVariable, t => t.InitValue)
                ;

            B.Assignment.Rule =
                B.VariableReference.BindMember(B.Assignment, t => t.LValue)
                + B.Expression.BindMember(B.Assignment, t => t.RValue)
                ;

            B.VariableReference.Rule =
                B.NameRef
                .ConvertValue(_nameRef => Reference.Get<IVariable>(_nameRef), _variableReference => _variableReference.NameRef)
                .BindMember(B.VariableReference, t => t.Target)
                ;

            B.FunctionReference.Rule =
                B.NameRef.ConvertValue(_nameRef => Reference.Get<Function>(_nameRef), _variableReference => _variableReference.NameRef)
                ;

            B.StatementList.Rule =
                B.BEGIN
                + B.Statement.PlusList().BindMember(B.StatementList, t => t.Body)
                + B.END
                ;

            B.While.Rule =
                B.WHILE
                + B.LEFT_PAREN
                + B.BoolExpression.BindMember(B.While, t => t.Condition)
                + B.RIGHT_PAREN
                + B.DO
                + B.BEGIN
                + B.Statement.BindMember(B.While, t => t.Body)
                + B.END
                ;

            B.For.Rule =
                B.FOR
                + B.LEFT_PAREN
                + B.LocalVariable.StarList(B.COMMA).BindMember(B.For, t => t.Init)
                + B.SEMICOLON
                + B.BoolExpression.BindMember(B.For, t => t.Condition)
                + B.SEMICOLON
                + B.Assignment.StarList(B.COMMA).BindMember(B.For, t => t.Update)
                + B.RIGHT_PAREN
                + B.DO
                + B.BEGIN
                + B.Statement.BindMember(B.For, t => t.Body)
                + B.END
                ;

            B.If.Rule =
                B.IF
                + B.LEFT_PAREN
                + B.BoolExpression.BindMember(B.If, t => t.Condition)
                + B.RIGHT_PAREN
                + B.THEN
                + B.BEGIN
                + B.Statement.BindMember(B.If, t => t.Body)
                + B.END
                + (B.ELSE + B.Statement).QRef().BindMember(B.If, t => t.ElseBody)
                ;

            B.FunctionCall.Rule =
                B.FunctionReference.BindMember(B.FunctionCall, t => t.FunctionReference)
                + B.LEFT_PAREN
                + B.Argument.StarList(B.COMMA).BindMember(B.FunctionCall, t => t.Arguments)
                + B.RIGHT_PAREN
                ;

            B.Write.Rule =
                B.WRITE
                + B.LEFT_PAREN
                + B.Expression.StarList(B.COMMA).BindMember(B.Write, t => t.Arguments)
                + B.RIGHT_PAREN
                ;

            B.WriteLn.Rule =
                B.WRITELN
                + B.LEFT_PAREN
                + B.Expression.StarList(B.COMMA).BindMember(B.WriteLn, t => t.Arguments)
                + B.RIGHT_PAREN
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

            B.Expression.SetRuleOr(
                B.BinaryExpression,
                B.UnaryExpression,
                B.NumberLiteral,
                B.StringLiteral,
                B.VariableReference,
                B.FunctionCall,
                B.LEFT_PAREN + B.Expression + B.RIGHT_PAREN
                );

            B.BinaryExpression.Rule =
                B.Expression.BindMember(B.BinaryExpression, t => t.Term1)
                + B.MathBinaryOperator.BindMember(B.BinaryExpression, t => t.Op)
                + B.Expression.BindMember(B.BinaryExpression, t => t.Term2);

            B.UnaryExpression.Rule =
                B.MathUnaryOperator.BindMember(B.UnaryExpression, t => t.Op)
                + B.Expression.BindMember(B.UnaryExpression, t => t.Term);

            B.BoolExpression.SetRuleOr(
                B.RelationalBinaryBoolExpression,
                B.LogicalBinaryBoolExpression,
                B.LogicalUnaryBoolExpression,
                B.BoolLiteral,
                B.VariableReference,
                B.FunctionCall,
                B.LEFT_PAREN + B.BoolExpression + B.RIGHT_PAREN
                );

            B.RelationalBinaryBoolExpression.Rule =
                B.Expression.BindMember(B.RelationalBinaryBoolExpression, t => t.Term1)
                + B.RelationalBinaryOperator.BindMember(B.RelationalBinaryBoolExpression, t => t.Op)
                + B.Expression.BindMember(B.RelationalBinaryBoolExpression, t => t.Term2);

            B.LogicalBinaryBoolExpression.Rule =
                B.BoolExpression.BindMember(B.LogicalBinaryBoolExpression, t => t.Term1)
                + B.LogicalBinaryOperator.BindMember(B.LogicalBinaryBoolExpression, t => t.Op)
                + B.BoolExpression.BindMember(B.LogicalBinaryBoolExpression, t => t.Term2);

            B.LogicalUnaryBoolExpression.Rule =
                B.LogicalUnaryOperator.BindMember(B.LogicalUnaryBoolExpression, t => t.Op)
                + B.BoolExpression.BindMember(B.LogicalUnaryBoolExpression, t => t.Term);

            B.NumberLiteral.Rule = CreateNumberLiteral().BindMember(B.NumberLiteral, t => t.Value);
            B.StringLiteral.Rule = CreateStringLiteral().BindMember(B.StringLiteral, t => t.Value);
            B.BoolLiteral.Rule = B.BOOL_CONSTANT.BindMember(B.BoolLiteral, t => t.Value);

            B.MathBinaryOperator.Rule = B.ADD_OP | B.SUB_OP | B.MUL_OP | B.DIV_OP | B.POW_OP | B.MOD_OP;
            B.MathUnaryOperator.Rule = B.POS_OP | B.NEG_OP;
            B.RelationalBinaryOperator.Rule = B.EQ_OP | B.NEQ_OP | B.LT_OP | B.LTE_OP | B.GT_OP | B.GTE_OP;
            B.LogicalBinaryOperator.Rule = B.AND_OP | B.OR_OP;
            B.LogicalUnaryOperator.Rule = B.NOT_OP;

            B.Type.Rule = B.INTEGER_TYPE | B.REAL_TYPE | B.STRING_TYPE | B.CHAR_TYPE | B.BOOL_TYPE;
        }
    }
}
