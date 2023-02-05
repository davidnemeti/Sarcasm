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

//#define SEPARATE_IFELSE

using System;

using Irony.Parsing;
using Sarcasm.GrammarAst;
using Sarcasm.Unparsing;
using Sarcasm.Unparsing.Styles;
using Sarcasm.DomainCore;
using Sarcasm.Utility;
using Sarcasm.Reflection;
using System.Globalization;

using D = MiniPL.DomainDefinitions;
using DE = Expr.DomainDefinitions;
using DC = Sarcasm.DomainCore;

namespace MiniPL.Grammars
{
    [Grammar(typeof(Domain), "Pascal-like")]
    public class GrammarP : Sarcasm.GrammarAst.Grammar<D.Program>
    {
        #region BnfTerms

        public class BnfTerms
        {
            internal BnfTerms(TerminalFactoryS TerminalFactoryS, CultureInfo cultureInfo)
            {
                if (cultureInfo.Name == "hu")
                {
                    this.PROGRAM = TerminalFactoryS.CreateKeyTerm("program");
                    this.NAMESPACE = TerminalFactoryS.CreateKeyTerm("névtér");
                    this.BEGIN = TerminalFactoryS.CreateKeyTerm("eleje");
                    this.END = TerminalFactoryS.CreateKeyTerm("vége");
                    this.FUNCTION = TerminalFactoryS.CreateKeyTerm("függvény");
                    this.WHILE = TerminalFactoryS.CreateKeyTerm("amíg");
                    this.FOR = TerminalFactoryS.CreateKeyTerm("ciklus");
                    this.IF = TerminalFactoryS.CreateKeyTerm("ha");
                    this.THEN = TerminalFactoryS.CreateKeyTerm("akkor");
                    this.ELSE = TerminalFactoryS.CreateKeyTerm("egyébként");
                    this.DO = TerminalFactoryS.CreateKeyTerm("csináld");
                    this.RETURN = TerminalFactoryS.CreateKeyTerm("visszatér");
                    this.WRITE = TerminalFactoryS.CreateKeyTerm("Kiír");
                    this.WRITELN = TerminalFactoryS.CreateKeyTerm("KiírSor");
                    this.VAR = TerminalFactoryS.CreateKeyTerm("változó");
                    this.ASYNC = TerminalFactoryS.CreateKeyTerm("aszink", true);

                    this.AND_OP = TerminalFactoryS.CreateKeyTerm("és", DE.BinaryOperator.And);
                    this.OR_OP = TerminalFactoryS.CreateKeyTerm("vagy", DE.BinaryOperator.Or);

                    this.NOT_OP = TerminalFactoryS.CreateKeyTerm("nem", DE.UnaryOperator.Not);

                    this.INTEGER_TYPE = TerminalFactoryS.CreateKeyTerm("egész", D.Type.Integer);
                    this.REAL_TYPE = TerminalFactoryS.CreateKeyTerm("valós", D.Type.Real);
                    this.STRING_TYPE = TerminalFactoryS.CreateKeyTerm("karakterlánc", D.Type.String);
                    this.CHAR_TYPE = TerminalFactoryS.CreateKeyTerm("karakter", D.Type.Char);
                    this.BOOL_TYPE = TerminalFactoryS.CreateKeyTerm("logikai", D.Type.Bool);
                    this.COLOR_TYPE = TerminalFactoryS.CreateKeyTerm("szín", D.Type.Color);
                    this.DATE_TYPE = TerminalFactoryS.CreateKeyTerm("dátum", D.Type.Date);

                    this.BOOL_CONSTANT = new BnfiTermConstant<bool>()
                    {
                        { "igaz", true },
                        { "hamis", false }
                    };

                    this.COLOR_CONSTANT = new BnfiTermConstant<D.Color>()
                    {
                        { "szín_fekete", D.Color.Black },
                        { "szín_kék", D.Color.Blue },
                        { "szín_barna", D.Color.Brown },
                        { "szín_szürke", D.Color.Gray },
                        { "szín_zöld", D.Color.Green },
                        { "szín_narancs", D.Color.Orange },
                        { "szín_piros", D.Color.Red },
                        { "szín_fehér", D.Color.White },
                        { "szín_sárga", D.Color.Yellow }
                    };
                }
                else if (cultureInfo.Name == "de")
                {
                    this.PROGRAM = TerminalFactoryS.CreateKeyTerm("Programm");
                    this.NAMESPACE = TerminalFactoryS.CreateKeyTerm("NameRaum");
                    this.BEGIN = TerminalFactoryS.CreateKeyTerm("Beginn");
                    this.END = TerminalFactoryS.CreateKeyTerm("Ende");
                    this.FUNCTION = TerminalFactoryS.CreateKeyTerm("Funktion");
                    this.WHILE = TerminalFactoryS.CreateKeyTerm("während");
                    this.FOR = TerminalFactoryS.CreateKeyTerm("Zyklus");
                    this.IF = TerminalFactoryS.CreateKeyTerm("wenn");
                    this.THEN = TerminalFactoryS.CreateKeyTerm("dann");
                    this.ELSE = TerminalFactoryS.CreateKeyTerm("sonst");
                    this.DO = TerminalFactoryS.CreateKeyTerm("tun");
                    this.RETURN = TerminalFactoryS.CreateKeyTerm("zurückholen");
                    this.WRITE = TerminalFactoryS.CreateKeyTerm("Schreiben");
                    this.WRITELN = TerminalFactoryS.CreateKeyTerm("SchreibenLeine");
                    this.VAR = TerminalFactoryS.CreateKeyTerm("Variable");
                    this.ASYNC = TerminalFactoryS.CreateKeyTerm("async", true);

                    this.AND_OP = TerminalFactoryS.CreateKeyTerm("und", DE.BinaryOperator.And);
                    this.OR_OP = TerminalFactoryS.CreateKeyTerm("oder", DE.BinaryOperator.Or);

                    this.NOT_OP = TerminalFactoryS.CreateKeyTerm("nicht", DE.UnaryOperator.Not);

                    this.INTEGER_TYPE = TerminalFactoryS.CreateKeyTerm("Ganzzahl", D.Type.Integer);
                    this.REAL_TYPE = TerminalFactoryS.CreateKeyTerm("ReeleZahl", D.Type.Real);
                    this.STRING_TYPE = TerminalFactoryS.CreateKeyTerm("Schnur", D.Type.String);
                    this.CHAR_TYPE = TerminalFactoryS.CreateKeyTerm("Charakter", D.Type.Char);
                    this.BOOL_TYPE = TerminalFactoryS.CreateKeyTerm("Boolsche", D.Type.Bool);
                    this.COLOR_TYPE = TerminalFactoryS.CreateKeyTerm("Farbe", D.Type.Color);
                    this.DATE_TYPE = TerminalFactoryS.CreateKeyTerm("Datum", D.Type.Date);

                    this.BOOL_CONSTANT = new BnfiTermConstant<bool>()
                    {
                        { "richtig", true },
                        { "falsch", false }
                    };

                    this.COLOR_CONSTANT = new BnfiTermConstant<D.Color>()
                    {
                        { "Farbe_schwarz", D.Color.Black },
                        { "Farbe_blau", D.Color.Blue },
                        { "Farbe_braun", D.Color.Brown },
                        { "Farbe_grau", D.Color.Gray },
                        { "Farbe_grün", D.Color.Green },
                        { "Farbe_orange", D.Color.Orange },
                        { "Farbe_rot", D.Color.Red },
                        { "Farbe_weiss", D.Color.White },
                        { "Farbe_gelb", D.Color.Yellow }
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
                    this.ASYNC = TerminalFactoryS.CreateKeyTerm("async", true);

                    this.AND_OP = TerminalFactoryS.CreateKeyTerm("and", DE.BinaryOperator.And);
                    this.OR_OP = TerminalFactoryS.CreateKeyTerm("or", DE.BinaryOperator.Or);

                    this.NOT_OP = TerminalFactoryS.CreateKeyTerm("not", DE.UnaryOperator.Not);

                    this.INTEGER_TYPE = TerminalFactoryS.CreateKeyTerm("integer", D.Type.Integer);
                    this.REAL_TYPE = TerminalFactoryS.CreateKeyTerm("real", D.Type.Real);
                    this.STRING_TYPE = TerminalFactoryS.CreateKeyTerm("string", D.Type.String);
                    this.CHAR_TYPE = TerminalFactoryS.CreateKeyTerm("char", D.Type.Char);
                    this.BOOL_TYPE = TerminalFactoryS.CreateKeyTerm("boolean", D.Type.Bool);
                    this.COLOR_TYPE = TerminalFactoryS.CreateKeyTerm("color", D.Type.Color);
                    this.DATE_TYPE = TerminalFactoryS.CreateKeyTerm("date", D.Type.Date);

                    this.BOOL_CONSTANT = new BnfiTermConstant<bool>()
                    {
                        { "True", true },
                        { "False", false }
                    };

                    this.COLOR_CONSTANT = new BnfiTermConstant<D.Color>()
                    {
                        { "Color_Black", D.Color.Black },
                        { "Color_Blue", D.Color.Blue },
                        { "Color_Brown", D.Color.Brown },
                        { "Color_Gray", D.Color.Gray },
                        { "Color_Green", D.Color.Green },
                        { "Color_Orange", D.Color.Orange },
                        { "Color_Red", D.Color.Red },
                        { "Color_White", D.Color.White },
                        { "Color_Yellow", D.Color.Yellow }
                    };
                }

                this.DOT = TerminalFactoryS.CreateKeyTerm(".");
                this.LET = TerminalFactoryS.CreateKeyTerm(":=");
                this.SEMICOLON = TerminalFactoryS.CreateKeyTerm(";");
                this.COLON = TerminalFactoryS.CreateKeyTerm(":");
                this.COMMA = TerminalFactoryS.CreateKeyTerm(",");
                this.LEFT_PAREN = TerminalFactoryS.CreateKeyTerm("(");
                this.RIGHT_PAREN = TerminalFactoryS.CreateKeyTerm(")");
                this.QUESTION_MARK = TerminalFactoryS.CreateKeyTerm("?");

                this.ADD_OP = TerminalFactoryS.CreateKeyTerm("+", DE.BinaryOperator.Add);
                this.SUB_OP = TerminalFactoryS.CreateKeyTerm("-", DE.BinaryOperator.Sub);
                this.MUL_OP = TerminalFactoryS.CreateKeyTerm("*", DE.BinaryOperator.Mul);
                this.DIV_OP = TerminalFactoryS.CreateKeyTerm("/", DE.BinaryOperator.Div);
                this.POW_OP = TerminalFactoryS.CreateKeyTerm("^", DE.BinaryOperator.Pow);
                this.MOD_OP = TerminalFactoryS.CreateKeyTerm("%", DE.BinaryOperator.Mod);

                this.POS_OP = TerminalFactoryS.CreateKeyTerm("+", DE.UnaryOperator.Pos);
                this.NEG_OP = TerminalFactoryS.CreateKeyTerm("-", DE.UnaryOperator.Neg);

                this.EQ_OP = TerminalFactoryS.CreateKeyTerm("=", DE.BinaryOperator.Eq);
                this.NEQ_OP = TerminalFactoryS.CreateKeyTerm("<>", DE.BinaryOperator.Neq);
                this.LT_OP = TerminalFactoryS.CreateKeyTerm("<", DE.BinaryOperator.Lt);
                this.LTE_OP = TerminalFactoryS.CreateKeyTerm("<=", DE.BinaryOperator.Lte);
                this.GT_OP = TerminalFactoryS.CreateKeyTerm(">", DE.BinaryOperator.Gt);
                this.GTE_OP = TerminalFactoryS.CreateKeyTerm(">=", DE.BinaryOperator.Gte);

                // NOTE: to parse keyterms with international characters properly we need to allow international characters in identifiers as well:
                //       CreateCSharpIdentifier creates an identifier terminal that allows internation characters
//                this.IDENTIFIER = TerminalFactoryS.CreateIdentifier();
                this.IDENTIFIER = TerminalFactory.CreateCSharpIdentifier("identifier").IntroIdentifier();
            }

            public readonly BnfiTermRecord<D.Program> Program = new BnfiTermRecord<D.Program>();
            public readonly BnfiTermRecord<D.Function> Function = new BnfiTermRecord<D.Function>();
            public readonly BnfiTermChoice<D.Type> Type = new BnfiTermChoice<D.Type>();
            public readonly BnfiTermRecord<D.LocalVariable> LocalVariable = new BnfiTermRecord<D.LocalVariable>();
            public readonly BnfiTermRecord<D.Parameter> Parameter = new BnfiTermRecord<D.Parameter>();
            public readonly BnfiTermRecord<D.Argument> Argument = new BnfiTermRecord<D.Argument>();
            public readonly BnfiTermChoice<D.Statement> Statement = new BnfiTermChoice<D.Statement>();
            public readonly BnfiTermRecord<D.StatementList> StatementList = new BnfiTermRecord<D.StatementList>();
            public readonly BnfiTermRecord<D.While> While = new BnfiTermRecord<D.While>();
            public readonly BnfiTermRecord<D.For> For = new BnfiTermRecord<D.For>();
            public readonly BnfiTermRecord<D.If> If = new BnfiTermRecord<D.If>();
#if SEPARATE_IFELSE
            public readonly BnfiTermRecord<D.IfElse> IfElse = new BnfiTermRecord<D.IfElse>();
#endif
            public readonly BnfiTermRecord<D.Return> Return = new BnfiTermRecord<D.Return>();
            public readonly BnfiTermRecord<D.Assignment> Assignment = new BnfiTermRecord<D.Assignment>();
            public readonly BnfiTermConversion<Reference<D.Function>> FunctionReference = new BnfiTermConversion<Reference<D.Function>>();
            public readonly BnfiTermRecord<D.FunctionCall> FunctionCall = new BnfiTermRecord<D.FunctionCall>();
            public readonly BnfiTermRecord<D.Write> Write = new BnfiTermRecord<D.Write>();
            public readonly BnfiTermRecord<D.WriteLn> WriteLn = new BnfiTermRecord<D.WriteLn>();
            public readonly BnfiTermRecord<D.VariableReference> VariableReference = new BnfiTermRecord<D.VariableReference>();

            public readonly BnfiTermChoice<DE.Expression> Expression = new BnfiTermChoice<DE.Expression>();
            public readonly BnfiTermRecord<DE.BinaryExpression> BinaryExpression = new BnfiTermRecord<DE.BinaryExpression>();
            public readonly BnfiTermRecord<DE.UnaryExpression> UnaryExpression = new BnfiTermRecord<DE.UnaryExpression>();
            public readonly BnfiTermRecord<DE.ConditionalTernaryExpression> ConditionalTernaryExpression = new BnfiTermRecord<DE.ConditionalTernaryExpression>();

            public readonly BnfiTermChoice<DE.BinaryOperator> BinaryOperator = new BnfiTermChoice<DE.BinaryOperator>();
            public readonly BnfiTermChoice<DE.UnaryOperator> UnaryOperator = new BnfiTermChoice<DE.UnaryOperator>();

            public readonly BnfiTermConversion<DE.NumberLiteral> NumberLiteral = new BnfiTermConversion<DE.NumberLiteral>();
            public readonly BnfiTermRecord<D.StringLiteral> StringLiteral = new BnfiTermRecord<D.StringLiteral>();
            public readonly BnfiTermRecord<DE.BoolLiteral> BoolLiteral = new BnfiTermRecord<DE.BoolLiteral>();
            public readonly BnfiTermRecord<D.ColorLiteral> ColorLiteral = new BnfiTermRecord<D.ColorLiteral>();
            public readonly BnfiTermRecord<D.DateLiteral> DateLiteral = new BnfiTermRecord<D.DateLiteral>();

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
            public readonly BnfiTermConversion<bool> ASYNC;

            public readonly BnfiTermKeyTerm DOT;
            public readonly BnfiTermKeyTerm LET;
            public readonly BnfiTermKeyTerm SEMICOLON;
            public readonly BnfiTermKeyTerm COLON;
            public readonly BnfiTermKeyTerm COMMA;
            public readonly BnfiTermKeyTerm LEFT_PAREN;
            public readonly BnfiTermKeyTerm RIGHT_PAREN;
            public readonly BnfiTermKeyTerm QUESTION_MARK;

            public readonly BnfiTermConversion<DE.BinaryOperator> ADD_OP;
            public readonly BnfiTermConversion<DE.BinaryOperator> SUB_OP;
            public readonly BnfiTermConversion<DE.BinaryOperator> MUL_OP;
            public readonly BnfiTermConversion<DE.BinaryOperator> DIV_OP;
            public readonly BnfiTermConversion<DE.BinaryOperator> POW_OP;
            public readonly BnfiTermConversion<DE.BinaryOperator> MOD_OP;

            public readonly BnfiTermConversion<DE.UnaryOperator> POS_OP;
            public readonly BnfiTermConversion<DE.UnaryOperator> NEG_OP;

            public readonly BnfiTermConversion<DE.BinaryOperator> EQ_OP;
            public readonly BnfiTermConversion<DE.BinaryOperator> NEQ_OP;
            public readonly BnfiTermConversion<DE.BinaryOperator> LT_OP;
            public readonly BnfiTermConversion<DE.BinaryOperator> LTE_OP;
            public readonly BnfiTermConversion<DE.BinaryOperator> GT_OP;
            public readonly BnfiTermConversion<DE.BinaryOperator> GTE_OP;

            public readonly BnfiTermConversion<DE.BinaryOperator> AND_OP;
            public readonly BnfiTermConversion<DE.BinaryOperator> OR_OP;

            public readonly BnfiTermConversion<DE.UnaryOperator> NOT_OP;

            public readonly BnfiTermConversion<D.Type> INTEGER_TYPE;
            public readonly BnfiTermConversion<D.Type> REAL_TYPE;
            public readonly BnfiTermConversion<D.Type> STRING_TYPE;
            public readonly BnfiTermConversion<D.Type> CHAR_TYPE;
            public readonly BnfiTermConversion<D.Type> BOOL_TYPE;
            public readonly BnfiTermConversion<D.Type> COLOR_TYPE;
            public readonly BnfiTermConversion<D.Type> DATE_TYPE;

            public readonly BnfiTermConstant<bool> BOOL_CONSTANT;
            public readonly BnfiTermConstant<D.Color> COLOR_CONSTANT;

            public readonly BnfiTermConversion<string> IDENTIFIER;
        }

        #endregion

        public readonly BnfTerms B;

        public GrammarP()
            : this(CultureInfo.InvariantCulture)
        {
        }

        public GrammarP(CultureInfo cultureInfo)
            : base(new Domain())
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
                + B.ASYNC.QVal(false).BindTo(B.Function, t => t.IsAsync)
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
#if SEPARATE_IFELSE
                B.IfElse,
#endif
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
                .ConvertValue(_nameRef => ReferenceFactory.Get<D.IVariable>(_nameRef), _variableReference => _variableReference.NameRef)
                .BindTo(B.VariableReference, t => t.Target)
                ;

            B.FunctionReference.Rule =
                B.NameRef.ConvertValue(_nameRef => ReferenceFactory.Get<D.Function>(_nameRef), _functionReference => _functionReference.NameRef)
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

#if SEPARATE_IFELSE
            B.If.Rule =
                B.IF
                + B.LEFT_PAREN
                + B.Expression.BindTo(B.If, t => t.Condition)
                + B.RIGHT_PAREN
                + B.THEN
                + B.Statement.BindTo(B.If, t => t.Body)
                ;

            B.IfElse.Rule =
                B.If.Copy(B.IfElse)
                + B.ELSE
                + B.Statement.BindTo(B.IfElse, t => t.ElseBody)
                ;
#else
            B.If.Rule =
                B.IF
                + B.LEFT_PAREN
                + B.Expression.BindTo(B.If, t => t.Condition)
                + B.RIGHT_PAREN
                + B.THEN
                + B.Statement.BindTo(B.If, t => t.Body)
                + (B.ELSE + B.Statement).QRef().BindTo(B.If, t => t.ElseBody)
                ;
#endif

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
                B.DateLiteral,
                B.StringLiteral,
                B.BoolLiteral,
                B.ColorLiteral,
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

            var numberLiteralInfo = new NumberLiteralInfo()
                .AddSuffix("D", TypeCode.Double)
                .AddSuffix("M", TypeCode.Decimal)
                .AddPrefix("#b", NumberLiteralBase.Binary)
                .AddPrefix("#o", NumberLiteralBase.Octal)
                .AddPrefix("#x", NumberLiteralBase.Hexadecimal);

            B.NumberLiteral.Rule = TerminalFactoryS.CreateNumberLiteral<DE.NumberLiteral>(numberLiteralInfo);
//            B.NumberLiteral.Rule = TerminalFactoryS.CreateNumberLiteral().BindTo(B.NumberLiteral, t => t.Value);   // B.NumberLiteral used to be a BnfiTermRecord
            B.StringLiteral.Rule = TerminalFactoryS.CreateStringLiteral(name: "stringliteral", startEndSymbol: "'").BindTo(B.StringLiteral, t => t.Value);
            B.BoolLiteral.Rule = B.BOOL_CONSTANT.BindTo(B.BoolLiteral, t => t.Value);
            B.ColorLiteral.Rule = B.COLOR_CONSTANT.BindTo(B.ColorLiteral, t => t.Value);
            B.DateLiteral.Rule = TerminalFactoryS.CreateDataLiteralDateTimeQuoted(name: "dateliteral", startEndSymbol: "$", dateTimeFormat: D.DateLiteral.Format).BindTo(B.DateLiteral, t => t.Value);

            B.BinaryOperator.Rule = B.ADD_OP | B.SUB_OP | B.MUL_OP | B.DIV_OP | B.POW_OP | B.MOD_OP | B.EQ_OP | B.NEQ_OP | B.LT_OP | B.LTE_OP | B.GT_OP | B.GTE_OP | B.AND_OP | B.OR_OP;
            B.UnaryOperator.Rule = B.POS_OP | B.NEG_OP | B.NOT_OP;

            B.Type.Rule = B.INTEGER_TYPE | B.REAL_TYPE | B.STRING_TYPE | B.CHAR_TYPE | B.BOOL_TYPE | B.COLOR_TYPE | B.DATE_TYPE;

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

            CommentTerminal DelimitedComment = new CommentTerminal("DelimitedComment", "(@", "@)");
            CommentTerminal SingleLineComment = new CommentTerminal("SingleLineComment", "@@", Environment.NewLine, "\n", "\r");

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

        public enum SyntaxHighlight { Color, BlackAndWhite, Crazy }

        [Formatter(typeof(GrammarP), "Default")]
        public class Formatter : Sarcasm.Unparsing.Formatter
        {
            #region Settings

            public SyntaxHighlight SyntaxHighlight { get; set; }
            public bool FlattenIfHierarchy { get; set; }

            public Color ForeColorOfKeyword { get; set; }
            public Color ForeColorOfOperator { get; set; }
            public Color ForeColorOfType { get; set; }
            public Color ForeColorOfLiteral { get; set; }
            public Color ForeColorOfComment { get; set; }

            public bool SpecialSyntaxHighlightForColorLiterals { get; set; }

            #endregion

            private readonly BnfTerms B;

            public Formatter(GrammarP grammar)
                : base(grammar)
            {
                this.B = grammar.B;

                SyntaxHighlight = GrammarP.SyntaxHighlight.Color;
                FlattenIfHierarchy = true;

                ForeColorOfKeyword = Color.Blue;
                ForeColorOfOperator = Color.Red;
                ForeColorOfType = Color.Cyan;
                ForeColorOfLiteral = Color.Purple;
                ForeColorOfComment = Color.DarkGreen;

                SpecialSyntaxHighlightForColorLiterals = true;

                MultiLineCommentDecorator = " @";
            }

            public override IDecoration GetDecoration(Utoken utoken, UnparsableAst target)
            {
                var decoration = base.GetDecoration(utoken, target);

                decoration.Add(DecorationKey.FontFamily, FontFamily.GenericMonospace);

                if (target != null)
                {
                    if (SyntaxHighlight == GrammarP.SyntaxHighlight.Color)
                        NormalSyntaxHighlight(utoken, target, decoration);
                    else if (SyntaxHighlight == GrammarP.SyntaxHighlight.Crazy)
                        CrazySyntaxHighlight(utoken, target, decoration);
                }

                return decoration;
            }

            private void NormalSyntaxHighlight(Utoken utoken, UnparsableAst target, IDecoration decoration)
            {
                if (target.AstValue is D.Color && SpecialSyntaxHighlightForColorLiterals)
                {
                    Color color;

                    if (Enum.TryParse<Color>(target.AstValue.ToString(), out color))
                    {
                        decoration
                            .Add(DecorationKey.Background, color)
                            ;

                        if (color.EqualToAny(Color.Black, Color.Blue, Color.Green, Color.Red))
                        {
                            decoration
                                .Add(DecorationKey.Foreground, Color.White)
                                ;
                        }
                        else
                        {
                            decoration
                                .Add(DecorationKey.Foreground, Color.Black)
                                ;
                        }
                    }
                }
                else if (utoken.Discriminator.EqualToAny(CommentContent, CommentStartSymbol, CommentEndSymbol))
                {
                    decoration
                        .Add(DecorationKey.Foreground, ForeColorOfComment)
                        ;
                }
                else if (target.BnfTerm is KeyTerm)
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
            }

            private void CrazySyntaxHighlight(Utoken utoken, UnparsableAst target, IDecoration decoration)
            {
                if (target.AstValue is D.Color)
                {
                    Color color;

                    if (Enum.TryParse<Color>(target.AstValue.ToString(), out color))
                    {
                        decoration
                            .Add(DecorationKey.Background, color)
                            ;

                        if (color.EqualToAny(Color.Black, Color.Blue, Color.Green, Color.Red))
                        {
                            decoration
                                .Add(DecorationKey.Foreground, Color.White)
                                ;
                        }
                        else
                        {
                            decoration
                                .Add(DecorationKey.Foreground, Color.Black)
                                ;
                        }
                    }
                }
                else if (utoken.Discriminator == CommentContent)
                {
                    decoration
                        .Add(DecorationKey.Foreground, Color.Pink)
                        .Add(DecorationKey.TextDecoration, TextDecoration.Strikethrough)
                        .Add(DecorationKey.FontStyle, FontStyle.Italic)
                        ;
                }
                else if (utoken.Discriminator == CommentStartSymbol)
                {
                    decoration
                        .Add(DecorationKey.Foreground, Color.Yellow)
                        .Add(DecorationKey.Background, Color.Violet)
                        ;
                }
                else if (utoken.Discriminator == CommentEndSymbol)
                {
                    decoration
                        .Add(DecorationKey.Foreground, Color.Blue)
                        .Add(DecorationKey.Background, Color.Yellow)
                        ;
                }
                else if (utoken.Discriminator == StringLiteralStartSymbol)
                {
                    decoration
                        .Add(DecorationKey.FontSizeRelativePercent, 2)
                        .Add(DecorationKey.Foreground, Color.Red)
                        .Add(DecorationKey.Background, Color.Blue)
                        ;
                }
                else if (target.AstValue is D.If)
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
                        .Add(DecorationKey.Foreground, Color.Red)
                        .Add(DecorationKey.Background, Color.Yellow)
                        .Add(DecorationKey.FontSize, 30);
                }
                else if (target.AstValue is D.Type)
                {
                    decoration
                        .Add(DecorationKey.BaselineAlignment, BaselineAlignment.Subscript)
                        .Add(DecorationKey.FontSizeRelativePercent, 0.75);
                }
                else if (target.AstValue is INumberLiteral)
                {
                    INumberLiteral number = (INumberLiteral)target.AstValue;

                    if (number.Value is int)
                    {
                        if (((int)number.Value) % 2 == 0)
                            decoration
                                .Add(DecorationKey.Foreground, Color.Red)
                                ;
                        else
                            decoration
                                .Add(DecorationKey.Foreground, Color.Green)
                                .Add(DecorationKey.Background, Color.Yellow)
                                ;
                    }
                }
            }

            public override void GetUtokensAround(UnparsableAst target, out InsertedUtokens leftInsertedUtokens, out InsertedUtokens rightInsertedUtokens)
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

            public override InsertedUtokens GetUtokensBetween(UnparsableAst leftTerminalLeaveTarget, UnparsableAst rightTarget)
            {
                if (leftTerminalLeaveTarget.AstImage != null && leftTerminalLeaveTarget.AstImage.AstValue is DC.Name && rightTarget.BnfTerm == B.LEFT_PAREN)
                    return UtokenInsert.NoWhitespace();

                else if (leftTerminalLeaveTarget.AstImage != null && leftTerminalLeaveTarget.AstImage.AstValue is DC.NameRef && rightTarget.BnfTerm == B.LEFT_PAREN)
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

            public override BlockIndentation GetBlockIndentation(UnparsableAst leftTerminalLeaveIfAny, UnparsableAst target)
            {
                if (target.BnfTerm == B.Statement && !(target.AstValue is D.StatementList))
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
