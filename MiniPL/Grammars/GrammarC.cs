﻿using System;
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
using Sarcasm.Unparsing.Styles;
using Sarcasm.DomainCore;
using Sarcasm.Reflection;
using Sarcasm.Utility;

using MiniPL.DomainDefinitions;
using D = MiniPL.DomainDefinitions;
using DC = Sarcasm.DomainCore;
using Type = MiniPL.DomainDefinitions.Type;
using NumberLiteral = MiniPL.DomainDefinitions.NumberLiteral;
using StringLiteral = MiniPL.DomainDefinitions.StringLiteral;

namespace MiniPL.Grammars
{
    [Grammar(typeof(Domain), "C-like")]
    public class GrammarC : Sarcasm.GrammarAst.Grammar<MiniPL.DomainDefinitions.Program>
    {
        #region BnfTerms

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
                this.ELSE = TerminalFactoryS.CreateKeyTerm("else");
                this.RETURN = TerminalFactoryS.CreateKeyTerm("return");
                this.WRITE = TerminalFactoryS.CreateKeyTerm("Write");
                this.WRITELN = TerminalFactoryS.CreateKeyTerm("WriteLn");

                this.DOT = TerminalFactoryS.CreateKeyTerm(".");
                this.LET = TerminalFactoryS.CreateKeyTerm("=");
                this.SEMICOLON = TerminalFactoryS.CreateKeyTerm(";");
                this.COLON = TerminalFactoryS.CreateKeyTerm(":");
                this.COMMA = TerminalFactoryS.CreateKeyTerm(",");
                this.LEFT_PAREN = TerminalFactoryS.CreateKeyTerm("(");
                this.RIGHT_PAREN = TerminalFactoryS.CreateKeyTerm(")");
                this.QUESTION_MARK = TerminalFactoryS.CreateKeyTerm("?");

                this.ADD_OP = TerminalFactoryS.CreateKeyTerm("+", DomainDefinitions.BinaryOperator.Add);
                this.SUB_OP = TerminalFactoryS.CreateKeyTerm("-", DomainDefinitions.BinaryOperator.Sub);
                this.MUL_OP = TerminalFactoryS.CreateKeyTerm("*", DomainDefinitions.BinaryOperator.Mul);
                this.DIV_OP = TerminalFactoryS.CreateKeyTerm("/", DomainDefinitions.BinaryOperator.Div);
                this.POW_OP = TerminalFactoryS.CreateKeyTerm("^", DomainDefinitions.BinaryOperator.Pow);
                this.MOD_OP = TerminalFactoryS.CreateKeyTerm("%", DomainDefinitions.BinaryOperator.Mod);

                this.POS_OP = TerminalFactoryS.CreateKeyTerm("+", DomainDefinitions.UnaryOperator.Pos);
                this.NEG_OP = TerminalFactoryS.CreateKeyTerm("-", DomainDefinitions.UnaryOperator.Neg);

                this.EQ_OP = TerminalFactoryS.CreateKeyTerm("==", DomainDefinitions.BinaryOperator.Eq);
                this.NEQ_OP = TerminalFactoryS.CreateKeyTerm("<>", DomainDefinitions.BinaryOperator.Neq);
                this.LT_OP = TerminalFactoryS.CreateKeyTerm("<", DomainDefinitions.BinaryOperator.Lt);
                this.LTE_OP = TerminalFactoryS.CreateKeyTerm("<=", DomainDefinitions.BinaryOperator.Lte);
                this.GT_OP = TerminalFactoryS.CreateKeyTerm(">", DomainDefinitions.BinaryOperator.Gt);
                this.GTE_OP = TerminalFactoryS.CreateKeyTerm(">=", DomainDefinitions.BinaryOperator.Gte);

                this.AND_OP = TerminalFactoryS.CreateKeyTerm("&&", DomainDefinitions.BinaryOperator.And);
                this.OR_OP = TerminalFactoryS.CreateKeyTerm("||", DomainDefinitions.BinaryOperator.Or);

                this.NOT_OP = TerminalFactoryS.CreateKeyTerm("!", DomainDefinitions.UnaryOperator.Not);

                this.INTEGER_TYPE = TerminalFactoryS.CreateKeyTerm("int", DomainDefinitions.Type.Integer);
                this.REAL_TYPE = TerminalFactoryS.CreateKeyTerm("double", DomainDefinitions.Type.Real);
                this.STRING_TYPE = TerminalFactoryS.CreateKeyTerm("string", DomainDefinitions.Type.String);
                this.CHAR_TYPE = TerminalFactoryS.CreateKeyTerm("char", DomainDefinitions.Type.Char);
                this.BOOL_TYPE = TerminalFactoryS.CreateKeyTerm("bool", DomainDefinitions.Type.Bool);

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
            public readonly BnfiTermKeyTerm ELSE;
            public readonly BnfiTermKeyTerm RETURN;
            public readonly BnfiTermKeyTerm WRITE;
            public readonly BnfiTermKeyTerm WRITELN;
            public readonly BnfiTermKeyTerm DOT;
            public readonly BnfiTermKeyTerm LET;
            public readonly BnfiTermKeyTerm SEMICOLON;
            public readonly BnfiTermKeyTerm COLON;
            public readonly BnfiTermKeyTerm COMMA;
            public readonly BnfiTermKeyTerm LEFT_PAREN;
            public readonly BnfiTermKeyTerm RIGHT_PAREN;
            public readonly BnfiTermKeyTerm QUESTION_MARK;

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

        #endregion

        public readonly BnfTerms B;

        public GrammarC()
            : base(new Domain())
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
                .ConvertValue(_nameRef => ReferenceFactory.Get<IVariable>(_nameRef), _variableReference => _variableReference.NameRef)
                .BindTo(B.VariableReference, t => t.Target)
                ;

            B.FunctionReference.Rule =
                B.NameRef.ConvertValue(_nameRef => ReferenceFactory.Get<Function>(_nameRef), _variableReference => _variableReference.NameRef)
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
                + B.Statement.BindTo(B.For, t => t.Body)
                ;

            B.If.Rule =
                B.IF
                + B.LEFT_PAREN
                + B.Expression.BindTo(B.If, t => t.Condition)
                + B.RIGHT_PAREN
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

            CommentTerminal DelimitedComment = new CommentTerminal("DelimitedComment", "/*", "*/");
            CommentTerminal SingleLineComment = new CommentTerminal("SingleLineComment", "//", Environment.NewLine);

            NonGrammarTerminals.Add(DelimitedComment);
            NonGrammarTerminals.Add(SingleLineComment);
        }

        #region Unparse

        protected override UnparseControl GetUnparseControl()
        {
            return UnparseControl.Create(this, new Formatter(this), new ParenthesizedExpression(B.Expression, B.LEFT_PAREN, B.RIGHT_PAREN));
        }

        #endregion

        #region Formatter

        public enum SyntaxHighlight { Color, BlackAndWhite }
        public enum IndentStyle { Allman, Whitesmiths, Stroustrup, KernighanAndRitchie }

        [Formatter(typeof(GrammarC), "Default")]
        public class Formatter : Sarcasm.Unparsing.Formatter
        {
            #region Settings

            public SyntaxHighlight SyntaxHighlight { get; set; }
            public bool FlattenIfHierarchy { get; set; }
            public IndentStyle IndentStyle { get; set; }

            public Color ForeColorOfKeyword { get; set; }
            public Color ForeColorOfOperator { get; set; }
            public Color ForeColorOfType { get; set; }
            public Color ForeColorOfLiteral { get; set; }
            public Color ForeColorOfComment { get; set; }

            public bool ShowNamesInBold { get; set; }
            public bool ShowNameRefsInItalic { get; set; }

            #endregion

            private readonly BnfTerms B;

            public Formatter(GrammarC grammar)
                : base(grammar)
            {
                this.B = grammar.B;

                SyntaxHighlight = GrammarC.SyntaxHighlight.Color;
                FlattenIfHierarchy = true;
                IndentStyle = GrammarC.IndentStyle.Allman;

                ForeColorOfKeyword = Color.Blue;
                ForeColorOfOperator = Color.Red;
                ForeColorOfType = Color.Cyan;
                ForeColorOfLiteral = Color.ForestGreen;
                ForeColorOfComment = Color.DarkGreen;

                ShowNamesInBold = true;
                ShowNameRefsInItalic = false;

                MultiLineCommentDecorator = " * ";
            }

            protected override IDecoration GetDecoration(Utoken utoken, UnparsableAst target)
            {
                var decoration = base.GetDecoration(utoken, target);

                decoration.Add(DecorationKey.FontFamily, FontFamily.GenericMonospace);

                if (target != null)
                {
                    if (SyntaxHighlight == GrammarC.SyntaxHighlight.Color)
                        NormalSyntaxHighlight(utoken, target, decoration);
                }

                return decoration;
            }

            private void NormalSyntaxHighlight(Utoken utoken, UnparsableAst target, IDecoration decoration)
            {
                if (target.BnfTerm is KeyTerm)
                {
                    if (target.BnfTerm.IsOperator() || target.BnfTerm.IsBrace())
                    {
                        decoration
                            .Add(DecorationKey.Foreground, ForeColorOfOperator)
                            ;
                    }
                    else
                    {
                        decoration
                            .Add(DecorationKey.Foreground, ForeColorOfKeyword)
                            ;
                    }
                }
                else if (target.AstValue is D.Type)
                {
                    decoration
                        .Add(DecorationKey.Foreground, ForeColorOfType)
                        ;
                }
                else if (target.BnfTerm.IsLiteral() || target.BnfTerm.IsConstant())
                {
                    decoration
                        .Add(DecorationKey.Foreground, ForeColorOfLiteral)
                        ;
                }
                else if (ShowNamesInBold && target.AstParent != null && target.AstParent.AstValue is Name)
                {
                    decoration
                        .Add(DecorationKey.FontWeight, FontWeight.Bold)
                        ;
                }
                else if (ShowNameRefsInItalic && target.AstParent != null && target.AstParent.AstValue is NameRef)
                {
                    decoration
                        .Add(DecorationKey.FontStyle, FontStyle.Italic)
                        ;
                }
                else if (utoken.Discriminator.EqualToAny(CommentContent, CommentStartSymbol, CommentEndSymbol))
                {
                    decoration
                        .Add(DecorationKey.Foreground, ForeColorOfComment)
                        ;
                }
            }

            protected override void GetUtokensAround(UnparsableAst target, out InsertedUtokens leftInsertedUtokens, out InsertedUtokens rightInsertedUtokens)
            {
                base.GetUtokensAround(target, out leftInsertedUtokens, out rightInsertedUtokens);

                if (target.BnfTerm == B.DOT)
                    leftInsertedUtokens = rightInsertedUtokens = UtokenInsert.NoWhitespace();

                else if (target.BnfTerm == B.RIGHT_PAREN)
                    leftInsertedUtokens = UtokenInsert.NoWhitespace();

                else if (target.BnfTerm == B.LEFT_PAREN)
                    rightInsertedUtokens = UtokenInsert.NoWhitespace();

                else if (target.BnfTerm == B.SEMICOLON)
                    leftInsertedUtokens = UtokenInsert.NoWhitespace();

                else if (target.BnfTerm == B.COMMA)
                    leftInsertedUtokens = UtokenInsert.NoWhitespace();

                else if (target.BnfTerm == B.Statement)
                    leftInsertedUtokens = rightInsertedUtokens = UtokenInsert.NewLine();

                else if (target.BnfTerm == B.BEGIN)
                {
                    rightInsertedUtokens = UtokenInsert.NewLine();

                    if (IndentStyle.EqualToAny(GrammarC.IndentStyle.Allman, GrammarC.IndentStyle.Whitesmiths))
                        leftInsertedUtokens = UtokenInsert.NewLine();
                    else if (IndentStyle.EqualToAny(GrammarC.IndentStyle.Stroustrup, GrammarC.IndentStyle.KernighanAndRitchie))
                        leftInsertedUtokens = UtokenInsert.Space();
                }

                else if (target.BnfTerm == B.END)
                {
                    leftInsertedUtokens = rightInsertedUtokens = UtokenInsert.NewLine();

                    if (target.AstValue is D.Function)
                        rightInsertedUtokens = UtokenInsert.EmptyLine().SetPriority(10);
                }

                else if (target.BnfTerm == B.UnaryOperator)
                    rightInsertedUtokens = UtokenInsert.NoWhitespace();

                else if (target.BnfTerm == B.Name && target.AstParent != null && target.AstParent.AstValue is D.Program)
                    rightInsertedUtokens = UtokenInsert.EmptyLine();

                else if (target.BnfTerm == B.NamespaceName && target.AstParent != null && target.AstParent.AstValue is D.Program)
                    rightInsertedUtokens = UtokenInsert.EmptyLine();
            }

            protected override InsertedUtokens GetUtokensBetween(UnparsableAst leftTerminalLeaveTarget, UnparsableAst rightTarget)
            {
                if (leftTerminalLeaveTarget.AstParent != null && leftTerminalLeaveTarget.AstParent.AstValue is DC.Name && rightTarget.BnfTerm == B.LEFT_PAREN)
                    return UtokenInsert.NoWhitespace();

                else if (leftTerminalLeaveTarget.AstParent != null && leftTerminalLeaveTarget.AstParent.AstValue is DC.NameRef && rightTarget.BnfTerm == B.LEFT_PAREN)
                    return UtokenInsert.NoWhitespace();

                else if (leftTerminalLeaveTarget.BnfTerm == B.WRITE && rightTarget.BnfTerm == B.LEFT_PAREN)
                    return UtokenInsert.NoWhitespace();

                else if (leftTerminalLeaveTarget.BnfTerm == B.WRITELN && rightTarget.BnfTerm == B.LEFT_PAREN)
                    return UtokenInsert.NoWhitespace();

                else if (FlattenIfHierarchy && leftTerminalLeaveTarget.BnfTerm == B.ELSE && rightTarget.BnfTerm == B.If)
                    return UtokenInsert.Space().SetPriority(10);

                else if (rightTarget.BnfTerm == B.ELSE && leftTerminalLeaveTarget.BnfTerm == B.END && IndentStyle == GrammarC.IndentStyle.KernighanAndRitchie)
                    return UtokenInsert.Space().SetPriority(10);

                else if (leftTerminalLeaveTarget.BnfTerm == B.END && rightTarget.BnfTerm == B.DOT)
                    return UtokenInsert.NoWhitespace().SetPriority(10);

                else
                    return base.GetUtokensBetween(leftTerminalLeaveTarget, rightTarget);
            }

            protected override BlockIndentation GetBlockIndentation(UnparsableAst leftTerminalLeaveIfAny, UnparsableAst target)
            {
                if (target.BnfTerm == B.Statement && !(target.AstValue is D.StatementList))
                    return BlockIndentation.Indent;

                else if (IndentStyle == GrammarC.IndentStyle.Whitesmiths && target.BnfTerm.EqualToAny(B.BEGIN, B.END))
                    return BlockIndentation.Indent;

                else if (FlattenIfHierarchy && leftTerminalLeaveIfAny != null && leftTerminalLeaveIfAny.BnfTerm == B.ELSE && target.BnfTerm == B.If)
                    return BlockIndentation.Unindent;

                else
                    return base.GetBlockIndentation(leftTerminalLeaveIfAny, target);
            }
        }

        #endregion
    }
}
