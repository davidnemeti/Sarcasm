using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.GrammarAst;
using Sarcasm.Unparsing;
using Sarcasm.DomainCore;

using MiniPL.DomainModel;

using Type = MiniPL.DomainModel.Type;
using NumberLiteral = MiniPL.DomainModel.NumberLiteral;
using StringLiteral = MiniPL.DomainModel.StringLiteral;

namespace MiniPL.Grammars
{
    [Grammar(typeof(Program), "Pascal-like")]
    public class GrammarP : Sarcasm.GrammarAst.Grammar<MiniPL.DomainModel.Program>
    {
        public class BnfTerms
        {
            internal BnfTerms(TerminalFactoryS TerminalFactoryS)
            {
                this.PROGRAM = TerminalFactoryS.CreateKeyTerm("program");
                this.NAMESPACE = TerminalFactoryS.CreateKeyTerm("namespace");
                this.BEGIN = TerminalFactoryS.CreateKeyTerm("begin");
                this.END = TerminalFactoryS.CreateKeyTerm("end");
                this.FUNCTION = TerminalFactoryS.CreateKeyTerm("function");
                this.WHILE = TerminalFactoryS.CreateKeyTerm("while");
                this.FOR = TerminalFactoryS.CreateKeyTerm("for");
                this.IF = TerminalFactoryS.CreateKeyTerm("if");
                this.THEN = TerminalFactoryS.CreateKeyTerm("then");
                this.ELSE = TerminalFactoryS.CreateKeyTerm("else");
                this.DO = TerminalFactoryS.CreateKeyTerm("do");
                this.RETURN = TerminalFactoryS.CreateKeyTerm("return");
                this.WRITE = TerminalFactoryS.CreateKeyTerm("Write");
                this.WRITELN = TerminalFactoryS.CreateKeyTerm("WriteLn");
                this.VAR = TerminalFactoryS.CreateKeyTerm("var");

                this.DOT = TerminalFactoryS.CreatePunctuation(".");
                this.LET = TerminalFactoryS.CreatePunctuation(":=");
                this.SEMICOLON = TerminalFactoryS.CreatePunctuation(";");
                this.COLON = TerminalFactoryS.CreatePunctuation(":");
                this.COMMA = TerminalFactoryS.CreatePunctuation(",");
                this.LEFT_PAREN = TerminalFactoryS.CreatePunctuation("(");
                this.RIGHT_PAREN = TerminalFactoryS.CreatePunctuation(")");
                this.QUESTION_MARK = TerminalFactoryS.CreatePunctuation("?");

                this.ADD_OP = TerminalFactoryS.CreateKeyTerm("+", DomainModel.BinaryOperator.Add);
                this.SUB_OP = TerminalFactoryS.CreateKeyTerm("-", DomainModel.BinaryOperator.Sub);
                this.MUL_OP = TerminalFactoryS.CreateKeyTerm("*", DomainModel.BinaryOperator.Mul);
                this.DIV_OP = TerminalFactoryS.CreateKeyTerm("/", DomainModel.BinaryOperator.Div);
                this.POW_OP = TerminalFactoryS.CreateKeyTerm("^", DomainModel.BinaryOperator.Pow);
                this.MOD_OP = TerminalFactoryS.CreateKeyTerm("%", DomainModel.BinaryOperator.Mod);

                this.POS_OP = TerminalFactoryS.CreateKeyTerm("+", DomainModel.UnaryOperator.Pos);
                this.NEG_OP = TerminalFactoryS.CreateKeyTerm("-", DomainModel.UnaryOperator.Neg);

                this.EQ_OP = TerminalFactoryS.CreateKeyTerm("=", DomainModel.BinaryOperator.Eq);
                this.NEQ_OP = TerminalFactoryS.CreateKeyTerm("<>", DomainModel.BinaryOperator.Neq);
                this.LT_OP = TerminalFactoryS.CreateKeyTerm("<", DomainModel.BinaryOperator.Lt);
                this.LTE_OP = TerminalFactoryS.CreateKeyTerm("<=", DomainModel.BinaryOperator.Lte);
                this.GT_OP = TerminalFactoryS.CreateKeyTerm(">", DomainModel.BinaryOperator.Gt);
                this.GTE_OP = TerminalFactoryS.CreateKeyTerm(">=", DomainModel.BinaryOperator.Gte);

                this.AND_OP = TerminalFactoryS.CreateKeyTerm("and", DomainModel.BinaryOperator.And);
                this.OR_OP = TerminalFactoryS.CreateKeyTerm("or", DomainModel.BinaryOperator.Or);

                this.NOT_OP = TerminalFactoryS.CreateKeyTerm("not", DomainModel.UnaryOperator.Not);

                this.INTEGER_TYPE = TerminalFactoryS.CreateKeyTerm("integer", DomainModel.Type.Integer);
                this.REAL_TYPE = TerminalFactoryS.CreateKeyTerm("real", DomainModel.Type.Real);
                this.STRING_TYPE = TerminalFactoryS.CreateKeyTerm("string", DomainModel.Type.String);
                this.CHAR_TYPE = TerminalFactoryS.CreateKeyTerm("char", DomainModel.Type.Char);
                this.BOOL_TYPE = TerminalFactoryS.CreateKeyTerm("boolean", DomainModel.Type.Bool);

                this.BOOL_CONSTANT = new BnfiTermConstant<bool>()
                {
                    { "true", true },
                    { "false", false }
                };

                this.IDENTIFIER = TerminalFactoryS.CreateIdentifier();
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

            public readonly BnfiTermChoice<BinaryOperator> BinaryOperator = new BnfiTermChoice<BinaryOperator>();
            public readonly BnfiTermChoice<UnaryOperator> UnaryOperator = new BnfiTermChoice<UnaryOperator>();

            public readonly BnfiTermType<NumberLiteral> NumberLiteral = new BnfiTermType<NumberLiteral>();
            public readonly BnfiTermType<StringLiteral> StringLiteral = new BnfiTermType<StringLiteral>();
            public readonly BnfiTermType<BoolLiteral> BoolLiteral = new BnfiTermType<BoolLiteral>();

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
            public readonly BnfiTermKeyTerm VAR;
            public readonly BnfiTermKeyTermPunctuation DOT;
            public readonly BnfiTermKeyTermPunctuation LET;
            public readonly BnfiTermKeyTermPunctuation SEMICOLON;
            public readonly BnfiTermKeyTermPunctuation COLON;
            public readonly BnfiTermKeyTermPunctuation COMMA;
            public readonly BnfiTermKeyTermPunctuation LEFT_PAREN;
            public readonly BnfiTermKeyTermPunctuation RIGHT_PAREN;
            public readonly BnfiTermKeyTermPunctuation QUESTION_MARK;

            public readonly BnfiTermValue<BinaryOperator> ADD_OP;
            public readonly BnfiTermValue<BinaryOperator> SUB_OP;
            public readonly BnfiTermValue<BinaryOperator> MUL_OP;
            public readonly BnfiTermValue<BinaryOperator> DIV_OP;
            public readonly BnfiTermValue<BinaryOperator> POW_OP;
            public readonly BnfiTermValue<BinaryOperator> MOD_OP;

            public readonly BnfiTermValue<UnaryOperator> POS_OP;
            public readonly BnfiTermValue<UnaryOperator> NEG_OP;

            public readonly BnfiTermValue<BinaryOperator> EQ_OP;
            public readonly BnfiTermValue<BinaryOperator> NEQ_OP;
            public readonly BnfiTermValue<BinaryOperator> LT_OP;
            public readonly BnfiTermValue<BinaryOperator> LTE_OP;
            public readonly BnfiTermValue<BinaryOperator> GT_OP;
            public readonly BnfiTermValue<BinaryOperator> GTE_OP;

            public readonly BnfiTermValue<BinaryOperator> AND_OP;
            public readonly BnfiTermValue<BinaryOperator> OR_OP;

            public readonly BnfiTermValue<UnaryOperator> NOT_OP;

            public readonly BnfiTermValue<Type> INTEGER_TYPE;
            public readonly BnfiTermValue<Type> REAL_TYPE;
            public readonly BnfiTermValue<Type> STRING_TYPE;
            public readonly BnfiTermValue<Type> CHAR_TYPE;
            public readonly BnfiTermValue<Type> BOOL_TYPE;

            public readonly BnfiTermConstant<bool> BOOL_CONSTANT;

            public readonly BnfiTermValue<string> IDENTIFIER;
        }

        public readonly BnfTerms B;

        public GrammarP()
            : base(AstCreation.CreateAstWithAutoBrowsableAstNodes, EmptyCollectionHandling.ReturnEmpty, ErrorHandling.ThrowException)
        {
            B = new BnfTerms(new TerminalFactoryS(this));

            this.Root = B.Program;

            B.Program.Rule =
                B.PROGRAM
                + B.Name.BindTo(B.Program, t => t.Name)
                + (B.NAMESPACE + B.NamespaceName).QRef().BindTo(B.Program, t => t.Namespace)
                + B.Function.StarList().BindTo(B.Program, t => t.Functions)
                + B.BEGIN
                + B.Statement.PlusList().BindTo(B.Program, t => t.Body)
                + B.END
                + B.DOT
                ;

            B.Function.Rule =
                B.FUNCTION
                + B.Name.BindTo(B.Function, t => t.Name)
                + B.LEFT_PAREN
                + B.Parameter.StarList(B.COMMA).BindTo(B.Function, t => t.Parameters)
                + B.RIGHT_PAREN
                + (B.COLON + B.Type).QVal().BindTo(B.Function, t => t.ReturnType)
                + B.BEGIN
                + B.Statement.PlusList().BindTo(B.Function, t => t.Body)
                + B.END
                ;

            B.Parameter.Rule =
                B.VAR
                + B.Name.BindTo(B.Parameter, t => t.Name)
                + B.COLON
                + B.Type.BindTo(B.Parameter, t => t.Type)
                ;

            B.Statement.SetRuleOr(
                B.LocalVariable + B.SEMICOLON,
                B.Assignment + B.SEMICOLON,
                B.While,
                B.For,
                B.If,
                B.FunctionCall + B.SEMICOLON,
                B.Write + B.SEMICOLON,
                B.WriteLn + B.SEMICOLON,
                B.Return + B.SEMICOLON,
                B.StatementList
                );

            B.Return.Rule =
                B.RETURN
                + B.Expression.BindTo(B.Return, t => t.Value)
                ;

            B.LocalVariable.Rule =
                B.VAR
                + B.Name.BindTo(B.LocalVariable, t => t.Name)
                + B.COLON
                + B.Type.BindTo(B.LocalVariable, t => t.Type)
                + (B.LET + B.Expression).QRef().BindTo(B.LocalVariable, t => t.InitValue)
                ;

            B.Assignment.Rule =
                B.VariableReference.BindTo(B.Assignment, t => t.LValue)
                + B.LET 
                + B.Expression.BindTo(B.Assignment, t => t.RValue)
                ;

            B.VariableReference.Rule =
                B.NameRef
                .ConvertValue(_nameRef => Reference.Get<IVariable>(_nameRef), _variableReference => _variableReference.NameRef)
                .BindTo(B.VariableReference, t => t.Target)
                ;

            B.FunctionReference.Rule =
                B.NameRef.ConvertValue(_nameRef => Reference.Get<Function>(_nameRef), _variableReference => _variableReference.NameRef)
                ;

            B.StatementList.Rule =
                B.BEGIN
                + B.Statement.PlusList().BindTo(B.StatementList, t => t.Body)
                + B.END
                ;

            B.While.Rule =
                B.WHILE
                + B.LEFT_PAREN
                + B.Expression.BindTo(B.While, t => t.Condition)
                + B.RIGHT_PAREN
                + B.DO
                + B.Statement.BindTo(B.While, t => t.Body)
                ;

            B.For.Rule =
                B.FOR
                + B.LEFT_PAREN
                + B.LocalVariable.StarList(B.COMMA).BindTo(B.For, t => t.Init)
                + B.SEMICOLON
                + B.Expression.BindTo(B.For, t => t.Condition)
                + B.SEMICOLON
                + B.Assignment.StarList(B.COMMA).BindTo(B.For, t => t.Update)
                + B.RIGHT_PAREN
                + B.DO
                + B.Statement.BindTo(B.For, t => t.Body)
                ;

            B.If.Rule =
                B.IF
                + B.LEFT_PAREN
                + B.Expression.BindTo(B.If, t => t.Condition)
                + B.RIGHT_PAREN
                + B.THEN
                + B.Statement.BindTo(B.If, t => t.Body)
                + (B.ELSE + B.Statement).QRef().BindTo(B.If, t => t.ElseBody)
                ;

            B.FunctionCall.Rule =
                B.FunctionReference.BindTo(B.FunctionCall, t => t.FunctionReference)
                + B.LEFT_PAREN
                + B.Argument.StarList(B.COMMA).BindTo(B.FunctionCall, t => t.Arguments)
                + B.RIGHT_PAREN
                ;

            B.Argument.Rule =
                B.Expression.BindTo(B.Argument, t => t.Expression)
                ;

            B.Write.Rule =
                B.WRITE
                + B.LEFT_PAREN
                + B.Expression.StarList(B.COMMA).BindTo(B.Write, t => t.Arguments)
                + B.RIGHT_PAREN
                ;

            B.WriteLn.Rule =
                B.WRITELN
                + B.LEFT_PAREN
                + B.Expression.StarList(B.COMMA).BindTo(B.WriteLn, t => t.Arguments)
                + B.RIGHT_PAREN
                ;

            B.Name.Rule = B.IDENTIFIER.BindTo(B.Name, t => t.Value);
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
                B.ConditionalTernaryExpression,
                B.NumberLiteral,
                B.StringLiteral,
                B.BoolLiteral,
                B.FunctionCall,
                B.VariableReference,
                B.LEFT_PAREN + B.Expression + B.RIGHT_PAREN
                );

            B.BinaryExpression.Rule =
                B.Expression.BindTo(B.BinaryExpression, t => t.Term1)
                + B.BinaryOperator.BindTo(B.BinaryExpression, t => t.Op)
                + B.Expression.BindTo(B.BinaryExpression, t => t.Term2)
                ;

            /*
             * NOTE: ImplyPrecedenceHere does not work properly, so we do not use it (it parsed operator NEG as operator POS, and omitted the expression after).
             * So we use ReduceHere instead, which means that unary operators has the highest precedence among operators when used inside a unary expressions.
             * */
            B.UnaryExpression.Rule =
                B.UnaryOperator.BindTo(B.UnaryExpression, t => t.Op)
                + B.Expression.BindTo(B.UnaryExpression, t => t.Term)
                + ReduceHere()      // this is needed for implying precedence (see note above)
                ;

            B.ConditionalTernaryExpression.Rule =
                B.Expression.BindTo(B.ConditionalTernaryExpression, t => t.Cond)
                + B.QUESTION_MARK
                + B.Expression.BindTo(B.ConditionalTernaryExpression, t => t.Term1)
                + B.COLON
                + B.Expression.BindTo(B.ConditionalTernaryExpression, t => t.Term2)
                ;

            B.NumberLiteral.Rule = TerminalFactoryS.CreateNumberLiteral().MakeContractible().BindTo(B.NumberLiteral, t => t.Value);
            B.StringLiteral.Rule = TerminalFactoryS.CreateStringLiteral(name: "stringliteral", startEndSymbol: "\"").MakeContractible().BindTo(B.StringLiteral, t => t.Value);
            B.BoolLiteral.Rule = B.BOOL_CONSTANT.BindTo(B.BoolLiteral, t => t.Value);

            B.BinaryOperator.Rule = B.ADD_OP | B.SUB_OP | B.MUL_OP | B.DIV_OP | B.POW_OP | B.MOD_OP | B.EQ_OP | B.NEQ_OP | B.LT_OP | B.LTE_OP | B.GT_OP | B.GTE_OP | B.AND_OP | B.OR_OP;
            B.UnaryOperator.Rule = B.POS_OP | B.NEG_OP | B.NOT_OP;

            B.Type.Rule = B.INTEGER_TYPE | B.REAL_TYPE | B.STRING_TYPE | B.CHAR_TYPE | B.BOOL_TYPE;

            /*
             * NOTE: RegisterOperators in Irony is string-based, therefore it is impossible to specify different precedences
             * for binary '+' and unary '+', and for binary '-' and unary '-', so we encode the precedences of unary operators
             * into the grammar by specifying a ReduceHere() hint after unary expressions.
             * */
            RegisterOperators(10, Associativity.Right, B.QUESTION_MARK, B.COLON);
            RegisterOperators(20, B.OR_OP);
            RegisterOperators(30, B.AND_OP);
            RegisterOperators(40, B.EQ_OP, B.NEQ_OP);
            RegisterOperators(50, B.LT_OP, B.LTE_OP, B.GT_OP, B.GTE_OP);
            RegisterOperators(60, B.ADD_OP, B.SUB_OP);
            RegisterOperators(70, B.MUL_OP, B.DIV_OP, B.MOD_OP);
            RegisterOperators(80, Associativity.Right, B.POW_OP);
            RegisterOperators(90, Associativity.Neutral, recurse: false, operators: new[] { B.NEG_OP, B.POS_OP, B.NOT_OP });
            // NOTE: for the parser the unary operators precedences are encoded into the grammar, but for the unparser we have to specify the precedences
            // NOTE: we must not recurse, since NEG_OP and POS_OP has the same terminals as SUB_OP and ADD_OP, respectively ('-' and '+').

            RegisterBracePair(B.LEFT_PAREN, B.RIGHT_PAREN);

            #region Unparse

            UnparseControl.DefaultFormatting.InsertUtokensAround(B.DOT, UtokenInsert.NoWhitespace);
            UnparseControl.DefaultFormatting.InsertUtokensRightOf(B.LEFT_PAREN, UtokenInsert.NoWhitespace);
            UnparseControl.DefaultFormatting.InsertUtokensLeftOf(B.RIGHT_PAREN, UtokenInsert.NoWhitespace);
            UnparseControl.DefaultFormatting.InsertUtokensLeftOf(B.SEMICOLON, UtokenInsert.NoWhitespace);
            UnparseControl.DefaultFormatting.InsertUtokensLeftOf(B.COMMA, UtokenInsert.NoWhitespace);
            UnparseControl.DefaultFormatting.InsertUtokensRightOf(B.UnaryOperator, UtokenInsert.NoWhitespace);
            UnparseControl.DefaultFormatting.InsertUtokensBetweenOrdered(B.Name, B.LEFT_PAREN, UtokenInsert.NoWhitespace);
            UnparseControl.DefaultFormatting.InsertUtokensBetweenOrdered(B.NameRef, B.LEFT_PAREN, UtokenInsert.NoWhitespace);
            UnparseControl.DefaultFormatting.InsertUtokensBetweenOrdered(B.WRITE, B.LEFT_PAREN, UtokenInsert.NoWhitespace);
            UnparseControl.DefaultFormatting.InsertUtokensBetweenOrdered(B.WRITELN, B.LEFT_PAREN, UtokenInsert.NoWhitespace);

            UnparseControl.DefaultFormatting.InsertUtokensRightOf(B.Statement, UtokenInsert.NewLine);
            UnparseControl.DefaultFormatting.InsertUtokensLeftOf(B.Statement, UtokenInsert.NewLine);
            UnparseControl.DefaultFormatting.SetBlockIndentationOn(B.Statement, BlockIndentation.Indent);
            UnparseControl.DefaultFormatting.InsertUtokensBetweenOrdered(B.ELSE, B.If, UtokenInsert.Space);
            UnparseControl.DefaultFormatting.SetBlockIndentationOn(B.ELSE, B.If, BlockIndentation.Unindent);
            UnparseControl.DefaultFormatting.InsertUtokensRightOf(new BnfTermPartialContext(B.Program, B.Name), UtokenInsert.EmptyLine);
            UnparseControl.DefaultFormatting.InsertUtokensRightOf(new BnfTermPartialContext(B.Program, B.NamespaceName), UtokenInsert.EmptyLine);
            UnparseControl.DefaultFormatting.InsertUtokensLeftOf(B.BEGIN, UtokenInsert.NewLine);
            UnparseControl.DefaultFormatting.InsertUtokensRightOf(B.BEGIN, UtokenInsert.NewLine);
            UnparseControl.DefaultFormatting.InsertUtokensLeftOf(B.END, UtokenInsert.NewLine);
            UnparseControl.DefaultFormatting.InsertUtokensRightOf(B.END, UtokenInsert.NewLine);
            UnparseControl.DefaultFormatting.InsertUtokensRightOf(new BnfTermPartialContext(B.Function, B.END), UtokenInsert.EmptyLine);


            #endregion
        }
    }
}
