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
using D = MiniPL.DomainModel;
using DC = Sarcasm.DomainCore;
using Type = MiniPL.DomainModel.Type;
using NumberLiteral = MiniPL.DomainModel.NumberLiteral;
using StringLiteral = MiniPL.DomainModel.StringLiteral;
using System.Windows;
using System.Windows.Media;

namespace MiniPL.Grammars
{
    [Grammar(typeof(Program), "C-like")]
    public class GrammarC : Sarcasm.GrammarAst.Grammar<MiniPL.DomainModel.Program>
    {
        public class BnfTerms
        {
            internal BnfTerms(TerminalFactoryS TerminalFactoryS)
            {
                this.PROGRAM = TerminalFactoryS.CreateKeyTerm("program");
                this.NAMESPACE = TerminalFactoryS.CreateKeyTerm("namespace");
                this.BEGIN = TerminalFactoryS.CreateKeyTerm("{");
                this.END = TerminalFactoryS.CreateKeyTerm("}");
                this.WHILE = TerminalFactoryS.CreateKeyTerm("while");
                this.FOR = TerminalFactoryS.CreateKeyTerm("for");
                this.IF = TerminalFactoryS.CreateKeyTerm("if");
                this.THEN = TerminalFactoryS.CreateKeyTerm("then");
                this.ELSE = TerminalFactoryS.CreateKeyTerm("else");
                this.DO = TerminalFactoryS.CreateKeyTerm("do");
                this.RETURN = TerminalFactoryS.CreateKeyTerm("return");
                this.WRITE = TerminalFactoryS.CreateKeyTerm("Write");
                this.WRITELN = TerminalFactoryS.CreateKeyTerm("WriteLn");

                this.DOT = TerminalFactoryS.CreatePunctuation(".");
                this.LET = TerminalFactoryS.CreatePunctuation("=");
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

                this.EQ_OP = TerminalFactoryS.CreateKeyTerm("==", DomainModel.BinaryOperator.Eq);
                this.NEQ_OP = TerminalFactoryS.CreateKeyTerm("<>", DomainModel.BinaryOperator.Neq);
                this.LT_OP = TerminalFactoryS.CreateKeyTerm("<", DomainModel.BinaryOperator.Lt);
                this.LTE_OP = TerminalFactoryS.CreateKeyTerm("<=", DomainModel.BinaryOperator.Lte);
                this.GT_OP = TerminalFactoryS.CreateKeyTerm(">", DomainModel.BinaryOperator.Gt);
                this.GTE_OP = TerminalFactoryS.CreateKeyTerm(">=", DomainModel.BinaryOperator.Gte);

                this.AND_OP = TerminalFactoryS.CreateKeyTerm("&&", DomainModel.BinaryOperator.And);
                this.OR_OP = TerminalFactoryS.CreateKeyTerm("||", DomainModel.BinaryOperator.Or);

                this.NOT_OP = TerminalFactoryS.CreateKeyTerm("!", DomainModel.UnaryOperator.Not);

                this.INTEGER_TYPE = TerminalFactoryS.CreateKeyTerm("int", DomainModel.Type.Integer);
                this.REAL_TYPE = TerminalFactoryS.CreateKeyTerm("double", DomainModel.Type.Real);
                this.STRING_TYPE = TerminalFactoryS.CreateKeyTerm("string", DomainModel.Type.String);
                this.CHAR_TYPE = TerminalFactoryS.CreateKeyTerm("char", DomainModel.Type.Char);
                this.BOOL_TYPE = TerminalFactoryS.CreateKeyTerm("bool", DomainModel.Type.Bool);

                this.BOOL_CONSTANT = new BnfiTermConstant<bool>()
                {
                    { "true", true },
                    { "false", false }
                };

                this.IDENTIFIER = TerminalFactoryS.CreateIdentifier();
            }

            public readonly BnfiTermRecord<Program> Program = new BnfiTermRecord<Program>();
            public readonly BnfiTermRecord<Function> Function = new BnfiTermRecord<Function>();
            public readonly BnfiTermChoice<Type> Type = new BnfiTermChoice<Type>();
            public readonly BnfiTermRecord<LocalVariable> LocalVariable = new BnfiTermRecord<LocalVariable>();
            public readonly BnfiTermRecord<Parameter> Parameter = new BnfiTermRecord<Parameter>();
            public readonly BnfiTermRecord<Argument> Argument = new BnfiTermRecord<Argument>();
            public readonly BnfiTermChoice<Statement> Statement = new BnfiTermChoice<Statement>();
            public readonly BnfiTermRecord<StatementList> StatementList = new BnfiTermRecord<StatementList>();
            public readonly BnfiTermRecord<While> While = new BnfiTermRecord<While>();
            public readonly BnfiTermRecord<For> For = new BnfiTermRecord<For>();
            public readonly BnfiTermRecord<If> If = new BnfiTermRecord<If>();
//            public readonly BnfiTermRecord<IfElse> IfElse = new BnfiTermRecord<IfElse>();
            public readonly BnfiTermRecord<Return> Return = new BnfiTermRecord<Return>();
            public readonly BnfiTermRecord<Assignment> Assignment = new BnfiTermRecord<Assignment>();
            public readonly BnfiTermConversion<Reference<Function>> FunctionReference = new BnfiTermConversion<Reference<Function>>();
            public readonly BnfiTermRecord<FunctionCall> FunctionCall = new BnfiTermRecord<FunctionCall>();
            public readonly BnfiTermRecord<Write> Write = new BnfiTermRecord<Write>();
            public readonly BnfiTermRecord<WriteLn> WriteLn = new BnfiTermRecord<WriteLn>();
            public readonly BnfiTermRecord<VariableReference> VariableReference = new BnfiTermRecord<VariableReference>();

            public readonly BnfiTermChoice<D.Expression> Expression = new BnfiTermChoice<D.Expression>();
            public readonly BnfiTermRecord<BinaryExpression> BinaryExpression = new BnfiTermRecord<BinaryExpression>();
            public readonly BnfiTermRecord<UnaryExpression> UnaryExpression = new BnfiTermRecord<UnaryExpression>();
            public readonly BnfiTermRecord<ConditionalTernaryExpression> ConditionalTernaryExpression = new BnfiTermRecord<ConditionalTernaryExpression>();

            public readonly BnfiTermChoice<BinaryOperator> BinaryOperator = new BnfiTermChoice<BinaryOperator>();
            public readonly BnfiTermChoice<UnaryOperator> UnaryOperator = new BnfiTermChoice<UnaryOperator>();

            public readonly BnfiTermRecord<NumberLiteral> NumberLiteral = new BnfiTermRecord<NumberLiteral>();
            public readonly BnfiTermRecord<StringLiteral> StringLiteral = new BnfiTermRecord<StringLiteral>();
            public readonly BnfiTermRecord<BoolLiteral> BoolLiteral = new BnfiTermRecord<BoolLiteral>();

            public readonly BnfiTermRecord<Name> Name = new BnfiTermRecord<Name>();
            public readonly BnfiTermConversion<NameRef> NamespaceName = new BnfiTermConversion<NameRef>("namespace_name");
            public readonly BnfiTermConversion<NameRef> NameRef = new BnfiTermConversion<NameRef>();

            public readonly BnfiTermKeyTerm PROGRAM;
            public readonly BnfiTermKeyTerm NAMESPACE;
            public readonly BnfiTermKeyTerm BEGIN;
            public readonly BnfiTermKeyTerm END;
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
            public readonly BnfiTermKeyTermPunctuation SEMICOLON;
            public readonly BnfiTermKeyTermPunctuation COLON;
            public readonly BnfiTermKeyTermPunctuation COMMA;
            public readonly BnfiTermKeyTermPunctuation LEFT_PAREN;
            public readonly BnfiTermKeyTermPunctuation RIGHT_PAREN;
            public readonly BnfiTermKeyTermPunctuation QUESTION_MARK;

            public readonly BnfiTermConversion<BinaryOperator> ADD_OP;
            public readonly BnfiTermConversion<BinaryOperator> SUB_OP;
            public readonly BnfiTermConversion<BinaryOperator> MUL_OP;
            public readonly BnfiTermConversion<BinaryOperator> DIV_OP;
            public readonly BnfiTermConversion<BinaryOperator> POW_OP;
            public readonly BnfiTermConversion<BinaryOperator> MOD_OP;

            public readonly BnfiTermConversion<UnaryOperator> POS_OP;
            public readonly BnfiTermConversion<UnaryOperator> NEG_OP;

            public readonly BnfiTermConversion<BinaryOperator> EQ_OP;
            public readonly BnfiTermConversion<BinaryOperator> NEQ_OP;
            public readonly BnfiTermConversion<BinaryOperator> LT_OP;
            public readonly BnfiTermConversion<BinaryOperator> LTE_OP;
            public readonly BnfiTermConversion<BinaryOperator> GT_OP;
            public readonly BnfiTermConversion<BinaryOperator> GTE_OP;

            public readonly BnfiTermConversion<BinaryOperator> AND_OP;
            public readonly BnfiTermConversion<BinaryOperator> OR_OP;

            public readonly BnfiTermConversion<UnaryOperator> NOT_OP;

            public readonly BnfiTermConversion<Type> INTEGER_TYPE;
            public readonly BnfiTermConversion<Type> REAL_TYPE;
            public readonly BnfiTermConversion<Type> STRING_TYPE;
            public readonly BnfiTermConversion<Type> CHAR_TYPE;
            public readonly BnfiTermConversion<Type> BOOL_TYPE;

            public readonly BnfiTermConstant<bool> BOOL_CONSTANT;

            public readonly BnfiTermConversion<string> IDENTIFIER;
        }

        public readonly BnfTerms B;

        public GrammarC()
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
                ;

            B.Function.Rule =
                B.Type.QVal().BindTo(B.Function, t => t.ReturnType)
                + B.Name.BindTo(B.Function, t => t.Name)
                + B.LEFT_PAREN
                + B.Parameter.StarList(B.COMMA).BindTo(B.Function, t => t.Parameters)
                + B.RIGHT_PAREN
                + B.BEGIN
                + B.Statement.PlusList().BindTo(B.Function, t => t.Body)
                + B.END
                ;

            B.Parameter.Rule =
                B.Type.BindTo(B.Parameter, t => t.Type)
                + B.Name.BindTo(B.Parameter, t => t.Name)
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
                B.Type.BindTo(B.LocalVariable, t => t.Type)
                + B.Name.BindTo(B.LocalVariable, t => t.Name)
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

            UnparseControl.DefaultFormatter = new Formatter(B);
            UnparseControl.SetAutomaticParenthesesExplicitlyForExpression(B.Expression, B.LEFT_PAREN, B.RIGHT_PAREN);

            #endregion
        }

        #region Formatter

        public class Formatter : Sarcasm.Unparsing.Formatter
        {
            private readonly BnfTerms B;

            public Formatter(BnfTerms b)
            {
                this.B = b;
            }

            protected override IDecoration GetDecoration(UnparsableAst target)
            {
                if (target.BnfTerm is KeyTerm)
                {
                    return new Decoration()
                        .Add(DecorationKey.Foreground, Colors.Blue)
                        ;
                }
                else if (target.BnfTerm.IsOperator())
                {
                    return new Decoration()
                        .Add(DecorationKey.Foreground, Colors.Purple)
                        ;
                }
                else if (target.AstValue is D.Type)
                {
                    return new Decoration()
                        .Add(DecorationKey.Foreground, Colors.Cyan)
                        ;
                }
                else if (target.BnfTerm.IsLiteral())
                {
                    return new Decoration()
                        .Add(DecorationKey.Foreground, Colors.Red)
                        ;
                }
                else
                    return Decoration.None;
            }

            protected override InsertedUtokens GetUtokensLeft(UnparsableAst target)
            {
                if (target.BnfTerm == B.DOT)
                    return UtokenInsert.NoWhitespace;
                else if (target.BnfTerm == B.RIGHT_PAREN)
                    return UtokenInsert.NoWhitespace;
                else if (target.BnfTerm == B.SEMICOLON)
                    return UtokenInsert.NoWhitespace;
                else if (target.BnfTerm == B.COMMA)
                    return UtokenInsert.NoWhitespace;
                else if (target.BnfTerm == B.Statement)
                    return UtokenInsert.NewLine;
                else if (target.BnfTerm == B.BEGIN)
                    return UtokenInsert.NewLine;
                else if (target.BnfTerm == B.END)
                    return UtokenInsert.NewLine;
                else
                    return base.GetUtokensLeft(target);
            }

            protected override InsertedUtokens GetUtokensRight(UnparsableAst target)
            {
                if (target.BnfTerm == B.DOT)
                    return UtokenInsert.NoWhitespace;
                else if (target.BnfTerm == B.LEFT_PAREN)
                    return UtokenInsert.NoWhitespace;
                else if (target.BnfTerm == B.UnaryOperator)
                    return UtokenInsert.NoWhitespace;
                else if (target.BnfTerm == B.Statement)
                    return UtokenInsert.NewLine;
                else if (target.BnfTerm == B.Name && target.AstParent != null && target.AstParent.AstValue is D.Program)
                    return UtokenInsert.EmptyLine;
                else if (target.BnfTerm == B.NamespaceName && target.AstParent != null && target.AstParent.AstValue is D.Program)
                    return UtokenInsert.EmptyLine;
                else if (target.BnfTerm == B.END && target.AstValue is D.Function)
                    return UtokenInsert.EmptyLine.SetPriority(10);
                else if (target.BnfTerm == B.BEGIN)
                    return UtokenInsert.NewLine;
                else if (target.BnfTerm == B.END)
                    return UtokenInsert.NewLine;
                else
                    return base.GetUtokensRight(target);
            }

            protected override InsertedUtokens GetUtokensBetween(UnparsableAst leftTerminalLeaveTarget, UnparsableAst rightTarget)
            {
                if (leftTerminalLeaveTarget.AstParent != null && leftTerminalLeaveTarget.AstParent.AstValue is DC.Name && rightTarget.BnfTerm == B.LEFT_PAREN)
                    return UtokenInsert.NoWhitespace;
                else if (leftTerminalLeaveTarget.AstParent != null && leftTerminalLeaveTarget.AstParent.AstValue is DC.NameRef && rightTarget.BnfTerm == B.LEFT_PAREN)
                    return UtokenInsert.NoWhitespace;
                else if (leftTerminalLeaveTarget.BnfTerm == B.WRITE && rightTarget.BnfTerm == B.LEFT_PAREN)
                    return UtokenInsert.NoWhitespace;
                else if (leftTerminalLeaveTarget.BnfTerm == B.WRITELN && rightTarget.BnfTerm == B.LEFT_PAREN)
                    return UtokenInsert.NoWhitespace;
                else if (leftTerminalLeaveTarget.BnfTerm == B.ELSE && rightTarget.BnfTerm == B.If)
                    return UtokenInsert.Space.SetPriority(10);
                else if (leftTerminalLeaveTarget.BnfTerm == B.END && rightTarget.BnfTerm == B.DOT)
                    return UtokenInsert.NoWhitespace.SetPriority(10);
                else
                    return base.GetUtokensBetween(leftTerminalLeaveTarget, rightTarget);
            }

            protected override BlockIndentation GetBlockIndentation(UnparsableAst leftTerminalLeaveIfAny, UnparsableAst target)
            {
                if (target.BnfTerm == B.Statement)
                    return BlockIndentation.Indent;
                else if (leftTerminalLeaveIfAny != null && leftTerminalLeaveIfAny.BnfTerm == B.ELSE && target.BnfTerm == B.If)
                    return BlockIndentation.Unindent;
                else
                    return base.GetBlockIndentation(leftTerminalLeaveIfAny, target);
            }
        }

        #endregion
    }
}
