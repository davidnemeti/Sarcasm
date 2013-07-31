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
using Sarcasm.Unparsing.Styles;
using Sarcasm.DomainCore;
using Sarcasm.Utility;
using Sarcasm.Reflection;

using MiniPL.DomainModel;
using D = MiniPL.DomainModel;
using DC = Sarcasm.DomainCore;
using Type = MiniPL.DomainModel.Type;
using NumberLiteral = MiniPL.DomainModel.NumberLiteral;
using StringLiteral = MiniPL.DomainModel.StringLiteral;
using System.Globalization;

namespace MiniPL.Grammars
{
    [Grammar(typeof(Program), "Pascal-like")]
    public class GrammarP : Sarcasm.GrammarAst.Grammar<MiniPL.DomainModel.Program>
    {
        public class BnfTerms
        {
            internal BnfTerms(TerminalFactoryS TerminalFactoryS, CultureInfo cultureInfo)
            {
                if (cultureInfo.Name == "hu")
                {
                    this.PROGRAM = TerminalFactoryS.CreateKeyTerm("program");
                    this.NAMESPACE = TerminalFactoryS.CreateKeyTerm("nevter");
                    this.BEGIN = TerminalFactoryS.CreateKeyTerm("eleje");
                    this.END = TerminalFactoryS.CreateKeyTerm("vege");
                    this.FUNCTION = TerminalFactoryS.CreateKeyTerm("fuggveny");
                    this.WHILE = TerminalFactoryS.CreateKeyTerm("amig");
                    this.FOR = TerminalFactoryS.CreateKeyTerm("ciklus");
                    this.IF = TerminalFactoryS.CreateKeyTerm("ha");
                    this.THEN = TerminalFactoryS.CreateKeyTerm("akkor");
                    this.ELSE = TerminalFactoryS.CreateKeyTerm("egyebkent");
                    this.DO = TerminalFactoryS.CreateKeyTerm("csinald");
                    this.RETURN = TerminalFactoryS.CreateKeyTerm("visszater");
                    this.WRITE = TerminalFactoryS.CreateKeyTerm("Kiir");
                    this.WRITELN = TerminalFactoryS.CreateKeyTerm("KiirUjsor");
                    this.VAR = TerminalFactoryS.CreateKeyTerm("valtozo");

                    this.AND_OP = TerminalFactoryS.CreateKeyTerm("es", DomainModel.BinaryOperator.And);
                    this.OR_OP = TerminalFactoryS.CreateKeyTerm("vagy", DomainModel.BinaryOperator.Or);

                    this.NOT_OP = TerminalFactoryS.CreateKeyTerm("nem", DomainModel.UnaryOperator.Not);

                    this.INTEGER_TYPE = TerminalFactoryS.CreateKeyTerm("egesz", DomainModel.Type.Integer);
                    this.REAL_TYPE = TerminalFactoryS.CreateKeyTerm("valos", DomainModel.Type.Real);
                    this.STRING_TYPE = TerminalFactoryS.CreateKeyTerm("karakterlanc", DomainModel.Type.String);
                    this.CHAR_TYPE = TerminalFactoryS.CreateKeyTerm("karakter", DomainModel.Type.Char);
                    this.BOOL_TYPE = TerminalFactoryS.CreateKeyTerm("logikai_ertek", DomainModel.Type.Bool);

                    this.BOOL_CONSTANT = new BnfiTermConstant<bool>()
                    {
                        { "Igaz", true },
                        { "Hamis", false }
                    };
                }
                else
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
                        { "True", true },
                        { "False", false }
                    };
                }

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

        public readonly BnfTerms B;

        public GrammarP()
            : this(CultureInfo.InvariantCulture)
        {
        }

        public GrammarP(CultureInfo cultureInfo)
            : base(AstCreation.CreateAstWithAutoBrowsableAstNodes, EmptyCollectionHandling.ReturnEmpty, ErrorHandling.ThrowException)
        {
            B = new BnfTerms(new TerminalFactoryS(this), cultureInfo);

            this.DefaultCulture = cultureInfo;

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

            UnparseControl.DefaultFormatter = new Formatter(this);
            UnparseControl.SetPrecedenceBasedParenthesesForExpression(B.Expression, B.LEFT_PAREN, B.RIGHT_PAREN);

            #endregion
        }

        #region Formatter

        public enum SyntaxHighlight { Color, BlackAndWhite, Crazy }

        [Formatter(typeof(GrammarP), "Default")]
        public class Formatter : Sarcasm.Unparsing.Formatter
        {
            #region Settings

            public SyntaxHighlight SyntaxHighlight { get; set; }
            public bool FlattenIfHierarchy { get; set; }

            #endregion

            private readonly BnfTerms B;

            public Formatter(GrammarP grammar)
                : base(grammar)
            {
                this.B = grammar.B;

                SyntaxHighlight = GrammarP.SyntaxHighlight.Color;
                FlattenIfHierarchy = true;
            }

            protected override IDecoration GetDecoration(Utoken utoken, UnparsableAst target)
            {
                var decoration = base.GetDecoration(utoken, target);

                decoration.Add(DecorationKey.FontFamily, FontFamily.GenericMonospace);

                if (target != null)
                {
                    if (SyntaxHighlight == GrammarP.SyntaxHighlight.Color)
                        NormalSyntaxHighlight(target, decoration);
                    else if (SyntaxHighlight == GrammarP.SyntaxHighlight.Crazy)
                        CrazySyntaxHighlight(target, decoration);
                }

                return decoration;
            }

            private void NormalSyntaxHighlight(UnparsableAst target, IDecoration decoration)
            {
                if (target.BnfTerm is KeyTerm)
                {
                    decoration
                        .Add(DecorationKey.Foreground, Color.Blue)
                        ;
                }
                else if (target.BnfTerm.IsOperator())
                {
                    decoration
                        .Add(DecorationKey.Foreground, Color.Purple)
                        ;
                }
                else if (target.AstValue is D.Type)
                {
                    decoration
                        .Add(DecorationKey.Foreground, Color.Cyan)
                        ;
                }
                else if (target.BnfTerm.IsLiteral())
                {
                    decoration
                        .Add(DecorationKey.Foreground, Color.Red)
                        ;
                }
            }

            private void CrazySyntaxHighlight(UnparsableAst target, IDecoration decoration)
            {
                if (target.AstValue is D.If)
                {
                    if (target.BnfTerm == B.LEFT_PAREN)
                    {
                        decoration
                            .Add(DecorationKey.FontWeight, FontWeight.Bold)
                            .Add(DecorationKey.FontSizeRelativePercent, 2)
                            .Add(DecorationKey.Foreground, Color.Blue)
                            ;
                    }
                    else
                    {
                        decoration
                            .Add(DecorationKey.FontWeight, FontWeight.Bold)
                            .Add(DecorationKey.TextDecoration, TextDecoration.Underline)
                            ;
                    }
                }
                else if (target.BnfTerm == B.PROGRAM)
                {
                    decoration
                        .Add(DecorationKey.FontStyle, FontStyle.Italic)
                        .Add(DecorationKey.Foreground, Color.White)
                        .Add(DecorationKey.Background, Color.Red)
                        .Add(DecorationKey.FontSize, 30);
                }
                else if (target.AstValue is D.Type)
                {
                    decoration
                        .Add(DecorationKey.BaselineAlignment, BaselineAlignment.Subscript)
                        .Add(DecorationKey.FontSizeRelativePercent, 0.75);
                }
                else if (target.AstValue is int)
                {
                    int number = (int)target.AstValue;

                    if (number % 2 == 0)
                        decoration.Add(DecorationKey.Foreground, Color.Red);
                    else
                        decoration.Add(DecorationKey.Background, Color.Yellow);
                }
            }

            protected override void GetUtokensAround(UnparsableAst target, out InsertedUtokens leftInsertedUtokens, out InsertedUtokens rightInsertedUtokens)
            {
                base.GetUtokensAround(target, out leftInsertedUtokens, out rightInsertedUtokens);

#if false       // alternative way to handle "else if" spacing
                if (target.AstValue is D.If &&
                    target.AstParent != null && target.AstParent.AstValue is D.If &&
//                    target.ParentMember != null && target.ParentMember.MemberInfo == Util.GetType<D.If>().GetMember(@if => @if.ElseBody))
                    target.ParentMember != null && target.ParentMember.MemberInfo == Util.GetMember(() => Util.GetType<D.If>().ElseBody) &&
                    target.BnfTerm == B.ELSE)
                {
                    var foo = target.BnfTerm;
                    rightInsertedUtokens = UtokenInsert.Space.SetPriority(10);
                    return;
                }
#endif

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
                    leftInsertedUtokens = rightInsertedUtokens = UtokenInsert.NewLine();

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

                // alternative ways to handle "else if" spacing
                else if (FlattenIfHierarchy && leftTerminalLeaveTarget.BnfTerm == B.ELSE && rightTarget.BnfTerm == B.If)
                    return UtokenInsert.Space().SetPriority(10);
                //else if (FlattenIfHierarchy && leftTerminalLeaveTarget.BnfTerm == B.ELSE && rightTarget.BnfTerm == B.IF)
                //    return UtokenInsert.Space.SetPriority(10);
                //else if (FlattenIfHierarchy && leftTerminalLeaveTarget.BnfTerm == B.ELSE && rightTarget.AstValue is D.If)
                //    return UtokenInsert.Space.SetPriority(10);

                else if (leftTerminalLeaveTarget.BnfTerm == B.END && rightTarget.BnfTerm == B.DOT)
                    return UtokenInsert.NoWhitespace().SetPriority(10);

                else
                    return base.GetUtokensBetween(leftTerminalLeaveTarget, rightTarget);
            }

            protected override BlockIndentation GetBlockIndentation(UnparsableAst leftTerminalLeaveIfAny, UnparsableAst target)
            {
                if (target.BnfTerm == B.Statement)
                    return BlockIndentation.Indent;

                // alternative ways to handle "else if" indentation
                else if (FlattenIfHierarchy && leftTerminalLeaveIfAny != null && leftTerminalLeaveIfAny.BnfTerm == B.ELSE && target.BnfTerm == B.If)
                    return BlockIndentation.Unindent;
                //else if (FlattenIfHierarchy && leftTerminalLeaveIfAny != null && leftTerminalLeaveIfAny.BnfTerm == B.ELSE && target.AstValue is D.If)
                //    return BlockIndentation.Unindent;

                else
                    return base.GetBlockIndentation(leftTerminalLeaveIfAny, target);
            }
        }

        #endregion
    }
}
