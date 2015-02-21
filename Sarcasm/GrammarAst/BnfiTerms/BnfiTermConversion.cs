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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.IO;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm.Parsing;
using Sarcasm.Unparsing;
using Sarcasm.Utility;
using Sarcasm.DomainCore;

namespace Sarcasm.GrammarAst
{
    public abstract partial class BnfiTermConversion : BnfiTermNonTerminal, IBnfiTerm, IUnparsableNonTerminal
    {
        #region State

        private BnfTerm bnfTerm;
        private object value;
        private object defaultValue;
        internal bool isOptionalValue { get; private set; }

        private ValueConverter<object, object> inverseValueConverterForUnparse;
        public ValueUtokenizer<object> UtokenizerForUnparse { private get; set; }

        #endregion

        #region Construction

        protected BnfiTermConversion(string name)
            : this(typeof(object), name)
        {
        }

        protected BnfiTermConversion(Type domainType, string name)
            : base(domainType, name)
        {
            this.inverseValueConverterForUnparse = IdentityFunction;
            GrammarHelper.MarkTransientForced(this);    // default "transient" behavior (the Rule of this BnfiTermConversion will contain the BnfiTermConversion which actually does something)
        }

        protected BnfiTermConversion(Type domainType, BnfTerm bnfTerm, object value, object defaultValue, bool isOptionalValue, string name, bool astForChild)
            : this(domainType, bnfTerm, (context, parseNode) => value, IdentityFunction, defaultValue, isOptionalValue, name, astForChild)
        {
            this.value = value;
        }

        [Obsolete("Pass either a 'value', or a valueIntroducer with an inverseValueConverterForUnparse", error: true)]
        protected BnfiTermConversion(Type domainType, BnfTerm bnfTerm, ValueIntroducer<object> valueIntroducer, object defaultValue, bool isOptionalValue, string name, bool astForChild)
            : this(domainType, bnfTerm, valueIntroducer, NoUnparseByInverse(), defaultValue, isOptionalValue, name, astForChild)
        {
        }

        protected BnfiTermConversion(Type domainType, BnfTerm bnfTerm, ValueIntroducer<object> valueIntroducer, ValueConverter<object, object> inverseValueConverterForUnparse,
            object defaultValue, bool isOptionalValue, string name, bool astForChild)
            : base(domainType, name)
        {
            this.IsContractible = true;
            this.bnfTerm = bnfTerm;
            this.isOptionalValue = isOptionalValue;
            this.defaultValue = defaultValue;

            if (!astForChild)
                bnfTerm.SetFlag(TermFlags.NoAstNode);

            this.RuleRawWithMove = isOptionalValue
                ? GrammarHelper.PreferShiftHere() + bnfTerm | Irony.Parsing.Grammar.CurrentGrammar.Empty
                : bnfTerm.ToBnfExpression();

            this.AstConfig.NodeCreator =
                (context, parseTreeNode) =>
                {
                    try
                    {
                        parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(valueIntroducer(context, new ParseTreeNodeWithoutAst(parseTreeNode)), context, parseTreeNode);
                    }
                    catch (AstException e)
                    {
                        context.AddMessage(AstException.ErrorLevel, parseTreeNode.Span.Location, e.Message);
                    }
                    catch (FatalAstException e)
                    {
                        context.AddMessage(FatalAstException.ErrorLevel, parseTreeNode.Span.Location, e.Message);   // although it will be abandoned anyway
                        e.Location = parseTreeNode.Span.Location;
                        throw;	// handle in MultiParser
                    }
                };

            this.inverseValueConverterForUnparse = inverseValueConverterForUnparse;
        }

        #endregion

        #region Intro

        public static BnfiTermConversionTL Intro(Terminal terminal, object value, bool astForChild = true)
        {
            return Intro(typeof(object), terminal, value, astForChild);
        }

        public static BnfiTermConversionTL Intro(Type domainType, Terminal terminal, object value, bool astForChild = true)
        {
            return new BnfiTermConversionTL(domainType, terminal, value, defaultValue: null, isOptionalValue: false, name: terminal.Name + "_parse", astForChild: astForChild).MakeUncontractible();
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermConversionTL Intro(Terminal terminal, ValueIntroducer<object> valueIntroducer, bool astForChild = true)
        {
            return Intro(terminal, valueIntroducer, NoUnparseByInverse(), astForChild);
        }

        public static BnfiTermConversionTL Intro(Terminal terminal, ValueIntroducer<object> valueIntroducer, ValueConverter<object, object> inverseValueConverterForUnparse, bool astForChild = true)
        {
            return Intro(typeof(object), terminal, valueIntroducer, inverseValueConverterForUnparse, astForChild);
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermConversionTL Intro(Type domainType, Terminal terminal, ValueIntroducer<object> valueIntroducer, bool astForChild = true)
        {
            return Intro(domainType, terminal, valueIntroducer, NoUnparseByInverse(), astForChild);
        }

        public static BnfiTermConversionTL Intro(Type domainType, Terminal terminal, ValueIntroducer<object> valueIntroducer, ValueConverter<object, object> inverseValueConverterForUnparse, bool astForChild = true)
        {
            return new BnfiTermConversionTL(
                domainType,
                terminal,
                (context, parseNode) => valueIntroducer(context, parseNode),
                inverseValueConverterForUnparse,
                defaultValue: null,
                isOptionalValue: false,
                name: terminal.Name + "_intro",
                astForChild: astForChild
                )
                .MakeUncontractible();
        }

        public static BnfiTermConversion<T> Intro<T>(Terminal terminal, T value, bool astForChild = true)
        {
            return new BnfiTermConversion<T>(terminal, value, defaultValue: default(T), isOptionalValue: false, name: terminal.Name + "_parse", astForChild: astForChild).MakeUncontractible();
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermConversion<T> Intro<T>(Terminal terminal, ValueIntroducer<T> valueIntroducer, bool astForChild = true)
        {
            return Intro(terminal, valueIntroducer, NoUnparseByInverse<T>(), astForChild);
        }

        public static BnfiTermConversion<T> Intro<T>(Terminal terminal, ValueIntroducer<T> valueIntroducer, ValueConverter<T, object> inverseValueConverterForUnparse, bool astForChild = true)
        {
            return new BnfiTermConversion<T>(
                terminal,
                (context, parseNode) => valueIntroducer(context, parseNode),
                inverseValueConverterForUnparse,
                defaultValue: default(T),
                isOptionalValue: false,
                name: terminal.Name + "_introAs_" + typeof(T).Name.ToLower(),
                astForChild: astForChild
                )
                .MakeUncontractible();
        }

        public static BnfiTermConversion<string> IntroIdentifier(IdentifierTerminal identifierTerminal)
        {
            return Intro<string>(identifierTerminal, (context, parseNode) => (string)parseNode.FindToken().Value, IdentityFunction, astForChild: false);
        }

        public static BnfiTermConversion<string> IntroStringLiteral(StringLiteral stringLiteral)
        {
            return Intro<string>(stringLiteral, (context, parseNode) => (string)parseNode.FindToken().Value, IdentityFunction, astForChild: false);
        }

        public static BnfiTermConversion<TNumberLiteral> IntroNumberLiteral<TNumberLiteral>(NumberLiteral numberLiteral, NumberLiteralInfo numberLiteralInfo)
            where TNumberLiteral : INumberLiteral, new()
        {
            var _numberLiteral = Intro(
                numberLiteral,
                (context, parseNode) =>
                {
                    string tokenText = parseNode.FindTokenAndGetText();
                    bool hasExplicitTypeModifier = numberLiteralInfo.HasTokenTextExplicitTypeSuffix(tokenText);
                    NumberLiteralBase @base = numberLiteralInfo.GetBaseFromTokenText(tokenText);
                    return new TNumberLiteral() { Value = parseNode.FindToken().Value, HasExplicitTypeModifier = hasExplicitTypeModifier, Base = @base };
                },
                NoUnparseByInverse<TNumberLiteral>(),
                astForChild: false
                );

            _numberLiteral.UtokenizerForUnparse = (formatProvider, reference, astValue) => numberLiteralInfo.NumberLiteralToText(reference, formatProvider);

            return _numberLiteral.MarkLiteral();
        }

        public static BnfiTermConversion<T> IntroConstantTerminal<T>(ConstantTerminal constantTerminal)
        {
            // NOTE: unparse for constant terminal is handled specifically in the unparser
            return Intro<T>(constantTerminal, (context, parseNode) => (T)parseNode.FindToken().Value, IdentityFunctionForceCast<T, object>, astForChild: false);
        }

        [Obsolete(BnfiTermConversion.messageForIntroForBnfiTermConstant, error: true)]
        public static BnfiTermConversion<T> IntroConstantTerminal<T>(BnfiTermConstant<T> bnfiTermConstant)
        {
            return IntroConstantTerminal<T>(bnfiTermConstant);
        }

        #endregion

        #region Convert

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermConversionTL Convert(IBnfiTermTL bnfiTerm, ValueConverter<object, object> valueConverter)
        {
            return Convert(bnfiTerm, valueConverter, NoUnparseByInverse());
        }

        public static BnfiTermConversionTL Convert(IBnfiTermTL bnfiTerm, ValueConverter<object, object> valueConverter, ValueConverter<object, object> inverseValueConverterForUnparse)
        {
            return Convert(typeof(object), bnfiTerm, valueConverter, inverseValueConverterForUnparse);
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermConversionTL Convert(Type domainType, IBnfiTermTL bnfiTerm, ValueConverter<object, object> valueConverter)
        {
            return Convert(domainType, bnfiTerm, valueConverter, NoUnparseByInverse());
        }

        public static BnfiTermConversionTL Convert(Type domainType, IBnfiTermTL bnfiTerm, ValueConverter<object, object> valueConverter, ValueConverter<object, object> inverseValueConverterForUnparse)
        {
            return new BnfiTermConversionTL(
                domainType,
                bnfiTerm.AsBnfTerm(),
                ConvertValueConverterToValueIntroducer(valueConverter),
                inverseValueConverterForUnparse,
                defaultValue: null,
                isOptionalValue: false,
                name: bnfiTerm.AsBnfTerm().Name + "_convert",
                astForChild: true
                );
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermConversion<TDOut> Convert<TDOut>(IBnfiTermTL bnfiTerm, ValueConverter<object, TDOut> valueConverter)
        {
            return Convert<TDOut>(bnfiTerm, valueConverter, NoUnparseByInverse<TDOut, object>());
        }

        public static BnfiTermConversion<TDOut> Convert<TDOut>(IBnfiTermTL bnfiTerm, ValueConverter<object, TDOut> valueConverter, ValueConverter<TDOut, object> inverseValueConverterForUnparse)
        {
            return new BnfiTermConversion<TDOut>(
                bnfiTerm.AsBnfTerm(),
                ConvertValueConverterToValueIntroducer(valueConverter),
                inverseValueConverterForUnparse,
                defaultValue: default(TDOut),
                isOptionalValue: false,
                name: bnfiTerm.AsBnfTerm().Name + "_convertTo_" + typeof(TDOut).Name.ToLower(),
                astForChild: true
                );
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermConversion<TDOut> Convert<TDIn, TDOut>(IBnfiTerm<TDIn> bnfiTerm, ValueConverter<TDIn, TDOut> valueConverter)
        {
            return Convert<TDIn, TDOut>(bnfiTerm, valueConverter, NoUnparseByInverse<TDOut, TDIn>());
        }

        public static BnfiTermConversion<TDOut> Convert<TDIn, TDOut>(IBnfiTerm<TDIn> bnfiTerm, ValueConverter<TDIn, TDOut> valueConverter, ValueConverter<TDOut, TDIn> inverseValueConverterForUnparse)
        {
            return new BnfiTermConversion<TDOut>(
                bnfiTerm.AsBnfTerm(),
                ConvertValueConverterToValueIntroducer(valueConverter),
                CastValueConverter<TDOut, TDIn, TDOut, object>(inverseValueConverterForUnparse),
                defaultValue: default(TDOut),
                isOptionalValue: false,
                name: typeof(TDIn).Name.ToLower() + "_convertTo_" + typeof(TDOut).Name.ToLower(),
                astForChild: true
                );
        }

        #endregion

        #region ConvertOpt

        public static BnfiTermConversion<T?> ConvertOptVal<T>(IBnfiTerm<T> bnfTerm)
            where T : struct
        {
            return ConvertOptVal(bnfTerm, value => value, value => value);
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermConversion<TDOut?> ConvertOptVal<TDIn, TDOut>(IBnfiTerm<TDIn> bnfTerm, ValueConverter<TDIn, TDOut> valueConverter)
            where TDIn : struct
            where TDOut : struct
        {
            return ConvertOptVal<TDIn, TDOut>(bnfTerm, valueConverter, NoUnparseByInverse<TDOut, TDIn>());
        }

        public static BnfiTermConversion<TDOut?> ConvertOptVal<TDIn, TDOut>(IBnfiTerm<TDIn> bnfTerm, ValueConverter<TDIn, TDOut> valueConverter, ValueConverter<TDOut, TDIn> inverseValueConverterForUnparse)
            where TDIn : struct
            where TDOut : struct
        {
            return ConvertOpt<TDIn, TDOut?>(bnfTerm, value => valueConverter(value), value => inverseValueConverterForUnparse(value.Value));
        }

        public static BnfiTermConversion<T> ConvertOptRef<T>(IBnfiTerm<T> bnfTerm)
            where T : class
        {
            return ConvertOptRef(bnfTerm, value => value, value => value);
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermConversion<TDOut> ConvertOptRef<TDIn, TDOut>(IBnfiTerm<TDIn> bnfTerm, ValueConverter<TDIn, TDOut> valueConverter)
            where TDIn : class
            where TDOut : class
        {
            return ConvertOptRef<TDIn, TDOut>(bnfTerm, valueConverter, NoUnparseByInverse<TDOut, TDIn>());
        }

        public static BnfiTermConversion<TDOut> ConvertOptRef<TDIn, TDOut>(IBnfiTerm<TDIn> bnfTerm, ValueConverter<TDIn, TDOut> valueConverter, ValueConverter<TDOut, TDIn> inverseValueConverterForUnparse)
            where TDIn : class
            where TDOut : class
        {
            return ConvertOpt<TDIn, TDOut>(bnfTerm, valueConverter, inverseValueConverterForUnparse);
        }

        public static BnfiTermConversion<T> ConvertOptVal<T>(IBnfiTerm<T> bnfiTerm, T defaultValue)
            where T : struct
        {
            return ConvertOptVal(bnfiTerm, value => value, value => value, defaultValue);
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermConversion<TDOut> ConvertOptVal<TDIn, TDOut>(IBnfiTerm<TDIn> bnfiTerm, ValueConverter<TDIn, TDOut> valueConverter, TDOut defaultValue)
            where TDIn : struct
            where TDOut : struct
        {
            return ConvertOptVal<TDIn, TDOut>(bnfiTerm, valueConverter, NoUnparseByInverse<TDOut, TDIn>(), defaultValue);
        }

        public static BnfiTermConversion<TDOut> ConvertOptVal<TDIn, TDOut>(IBnfiTerm<TDIn> bnfiTerm, ValueConverter<TDIn, TDOut> valueConverter,
            ValueConverter<TDOut, TDIn> inverseValueConverterForUnparse, TDOut defaultValue)
            where TDIn : struct
            where TDOut : struct
        {
            return ConvertOpt<TDIn, TDOut>(bnfiTerm, valueConverter, inverseValueConverterForUnparse, defaultValue);
        }

        public static BnfiTermConversion<T> ConvertOptRef<T>(IBnfiTerm<T> bnfiTerm, T defaultValue)
            where T : class
        {
            return ConvertOptRef(bnfiTerm, value => value, value => value, defaultValue);
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermConversion<TDOut> ConvertOptRef<TDIn, TDOut>(IBnfiTerm<TDIn> bnfiTerm, ValueConverter<TDIn, TDOut> valueConverter, TDOut defaultValue)
            where TDIn : class
            where TDOut : class
        {
            return ConvertOptRef<TDIn, TDOut>(bnfiTerm, valueConverter, NoUnparseByInverse<TDOut, TDIn>(), defaultValue);
        }

        public static BnfiTermConversion<TDOut> ConvertOptRef<TDIn, TDOut>(IBnfiTerm<TDIn> bnfiTerm, ValueConverter<TDIn, TDOut> valueConverter,
            ValueConverter<TDOut, TDIn> inverseValueConverterForUnparse, TDOut defaultValue)
            where TDIn : class
            where TDOut : class
        {
            return ConvertOpt(bnfiTerm, valueConverter, inverseValueConverterForUnparse, defaultValue);
        }

        protected static BnfiTermConversion<TDOut> ConvertOpt<TDIn, TDOut>(IBnfiTerm<TDIn> bnfiTerm, ValueConverter<TDIn, TDOut> valueConverter,
            ValueConverter<TDOut, TDIn> inverseValueConverterForUnparse, TDOut defaultValue = default(TDOut))
        {
            return new BnfiTermConversion<TDOut>(
                bnfiTerm.AsBnfTerm(),
                ConvertValueConverterToValueIntroducerOpt(valueConverter, defaultValue),
                CastValueConverter<TDOut, TDIn, TDOut, object>(inverseValueConverterForUnparse),
                defaultValue: defaultValue,
                isOptionalValue: true,
                name: typeof(TDIn).Name.ToLower() + "_convertOptTo_" + typeof(TDOut).Name.ToLower(),
                astForChild: true
                );
        }

        #endregion

        #region Cast

        public static BnfiTermConversion<TDOut> Cast<TDIn, TDOut>(IBnfiTerm<TDIn> bnfTerm)
        {
            return Convert(bnfTerm, IdentityFunctionForceCast<TDIn, TDOut>, IdentityFunctionForceCast<TDOut, TDIn>);
        }

        public static BnfiTermConversion<TDOut> Cast<TDOut>(Terminal terminal)
        {
            return Intro<TDOut>(terminal, (context, parseNode) => GrammarHelper.AstNodeToValue<TDOut>(parseNode.Token.Value), IdentityFunctionForceCast<TDOut, object>);
        }

        #endregion

        #region Helpers for Convert and ConvertOpt

        protected static ValueIntroducer<TDOut> ConvertValueConverterToValueIntroducer<TDIn, TDOut>(ValueConverter<TDIn, TDOut> valueConverter)
        {
            return ConvertValueConverterToValueIntroducer(valueConverter, isOptionalValue: false, defaultValue: default(TDOut));   // defaultValue won't be used
        }

        protected static ValueIntroducer<TDOut> ConvertValueConverterToValueIntroducerOpt<TDIn, TDOut>(ValueConverter<TDIn, TDOut> valueConverter, TDOut defaultValue)
        {
            return ConvertValueConverterToValueIntroducer(valueConverter, isOptionalValue: true, defaultValue: defaultValue);
        }

        private static ValueIntroducer<TDOut> ConvertValueConverterToValueIntroducer<TDIn, TDOut>(ValueConverter<TDIn, TDOut> valueConverter, bool isOptionalValue, TDOut defaultValue)
        {
            return (context, parseTreeNode) =>
            {
                Func<IEnumerable<ParseTreeNode>, Func<ParseTreeNode, bool>, ParseTreeNode> chooser;
                if (isOptionalValue)
                    chooser = Enumerable.SingleOrDefault<ParseTreeNode>;
                else
                    chooser = Enumerable.Single<ParseTreeNode>;

                ParseTreeNode parseTreeChild;

                try
                {
                    parseTreeChild = chooser(parseTreeNode.ChildNodes, childNode => childNode.AstNode != null);
                }
                catch (InvalidOperationException)
                {
                    if (isOptionalValue)
                        throw new ArgumentException("Only zero or one child with ast node is allowed for an optional BnfiTermConversion term: {0}", parseTreeNode.Term.Name);
                    else
                        throw new ArgumentException("Exactly one child with ast node is allowed for a non-optional BnfiTermConversion term: {0}", parseTreeNode.Term.Name);
                }

                return parseTreeChild != null
                    ? valueConverter(GrammarHelper.AstNodeToValue<TDIn>(parseTreeChild.AstNode))
                    : defaultValue;
            };
        }

        #endregion

        protected static object IdentityFunction(object astValue)
        {
            return astValue;
        }

        protected static TDOut IdentityFunctionForceCast<TDIn, TDOut>(TDIn astValue)
        {
            return (TDOut)(object)astValue;
        }

        protected void SetState(BnfiTermConversion source)
        {
            this.bnfTerm = source.bnfTerm;
            this.value = source.value;
            this.defaultValue = source.defaultValue;
            this.isOptionalValue = source.isOptionalValue;
            this.Flags = source.Flags;
            this.AstConfig = source.AstConfig;

            this.inverseValueConverterForUnparse = source.inverseValueConverterForUnparse;

            if (this.UtokenizerForUnparse != null)
                this.UtokenizerForUnparse = source.UtokenizerForUnparse;
        }

        protected void ClearState()
        {
            this.bnfTerm = null;
            this.value = null;
            this.defaultValue = null;
            this.isOptionalValue = false;
            this.Flags = TermFlags.None;
            this.AstConfig = null;
            this.inverseValueConverterForUnparse = null;
            this.UtokenizerForUnparse = null;
        }

        protected void ContractTo(BnfiTermConversion target)
        {
            if (!this.IsContractible)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "This value should not be a right-value: {0}", this.Name);

            target.RuleRaw = this.RuleRaw;
            target.SetState(this);

            this.RuleRaw = null;
            this.ClearState();
            this.hasBeenContracted = true;
        }

        protected new BnfiExpression Rule { set { RuleRawWithMove = value; } }

        public BnfExpression RuleRaw { set { base.Rule = value; } get { return base.Rule; } }

        protected BnfExpression RuleRawWithMove
        {
            set
            {
                if (value == null)
                    goto LDefaultRuleSetting;

                /*
                 * Examine whether there is only one single BnfiTermConversion inside 'value', and if it is true, then move the state of that BnfiTermConversion
                 * so we can "destroy" that BnfiTermConversion in order to eliminate unneccessary, extra "rule-embedding"
                 * */
                var bnfTerms = value.GetBnfTermsList().SingleOrDefaultNoException();
                if (bnfTerms == null)
                    goto LDefaultRuleSetting;

                BnfiTermConversion onlyBnfTermInValue = bnfTerms.SingleOrDefaultNoException() as BnfiTermConversion;

                if (onlyBnfTermInValue != null && onlyBnfTermInValue.IsContractible)
                    onlyBnfTermInValue.ContractTo(this);
                else
                    goto LDefaultRuleSetting;

                return;

            LDefaultRuleSetting:
                // there are more than one bnfTerms inside 'value' -> "rule-embedding" is necessary
                base.Rule = value;
                return;
            }
            get
            {
                return base.Rule;
            }
        }

        #region Unparse

        protected override bool TryGetUtokensDirectly(IUnparser unparser, UnparsableAst self, out IEnumerable<UtokenValue> utokens)
        {
            if (this.UtokenizerForUnparse != null)
            {
                utokens = this.UtokenizerForUnparse(unparser.FormatProvider, self, self.AstValue);
                return true;
            }
            else if (this.isOptionalValue && object.Equals(self.AstValue, this.defaultValue))
            {
                utokens = Enumerable.Empty<UtokenValue>();
                return true;
            }
            else if (this.inverseValueConverterForUnparse != null)
            {
                /*
                 * NOTE that we *don't have to* check for "this.inverseValueConverterForUnparse != noUnparseByInverse" here because
                 * IUnparsableNonTerminal.GetChildren will call this.inverseValueConverterForUnparse anyway and it will throw an
                 * UnparseException if it equals to NoUnparseByInverse().
                 * 
                 * Moreover, we *cannot* check for "this.inverseValueConverterForUnparse != noUnparseByInverse" here properly because
                 * of the usages of the generic versions of NoUnparseByInverse<...>() methods which wrap the original noUnparseByInverse
                 * by calling CastValueConverter.
                 * */

                utokens = null;
                return false;
            }
            else
                throw new UnparseException(string.Format("Cannot unparse. '{0}' has neither UtokenizerForUnparse nor ValueConverterForUnparse", this.Name));
        }

        protected override IEnumerable<UnparsableAst> GetChildren(Unparser.ChildBnfTerms childBnfTerms, object astValue, Unparser.Direction direction)
        {
            return childBnfTerms.Select(childBnfTerm => new UnparsableAst(childBnfTerm, ConvertAstValueForChild(astValue, childBnfTerm)));
        }

        protected override int? GetChildrenPriority(IUnparser unparser, object astValue, Unparser.Children children, Unparser.Direction direction)
        {
            if (this.isOptionalValue && object.Equals(astValue, this.defaultValue))
                return 0;
            else if (this.value != null)
                return this.value.Equals(astValue) ? (int?)1 : null;
            else if (this.UtokenizerForUnparse != null)
                return 1;
            else
                return unparser.GetPriority(GetMainChild(astValue, children));
        }

        private static bool IsMainChild(BnfTerm bnfTerm)
        {
            return !(bnfTerm is KeyTerm) && !(bnfTerm is GrammarHint);
        }

        private UnparsableAst GetMainChild(object astValue, IEnumerable<UnparsableAst> children)
        {
            return this.bnfTerm != null
                ? new UnparsableAst(this.bnfTerm, ConvertAstValueForChild(astValue, this.bnfTerm))
                : children.Single(child => IsMainChild(child.BnfTerm));    // "transient" unparse with the actual BnfiTermConversion(s) under the current one (set by Rule)
        }

        private object ConvertAstValueForChild(object astValue, BnfTerm childBnfTerm)
        {
            /*
             * If UtokenizerForUnparse is not null then this method is being called only for child priority calculations, and not for
             * actually getting the children for unparse. However, if we are being called for priority calculations, we shouldn't
             * throw an exception because of the unimplemented inverseValueConverterForUnparse.
             * */

            if (IsMainChild(childBnfTerm) && UtokenizerForUnparse == null)
                return this.inverseValueConverterForUnparse(astValue);
            else
                return astValue;
        }

        private static readonly ValueConverter<object, object> noUnparseByInverse =
            value =>
            {
                throw new UnparseException("Cannot unparse. Inverse value converter parameter for unparse is missing");
            };

        private static readonly ValueCreatorFromNoAst<object> noUnparseByInverseCreatorFromNoAst =
            () =>
            {
                throw new UnparseException("Cannot unparse. Inverse \"value creator from no ast\" parameter for unparse is missing");
            };

        public static ValueConverter<object, object> NoUnparseByInverse()
        {
            return noUnparseByInverse;
        }

        public static ValueConverter<TD, object> NoUnparseByInverse<TD>()
        {
            return CastValueConverter<object, object, TD, object>(noUnparseByInverse);
        }

        public static ValueConverter<TDIn, TDOut> NoUnparseByInverse<TDIn, TDOut>()
        {
            return CastValueConverter<object, object, TDIn, TDOut>(noUnparseByInverse);
        }

        public static ValueCreatorFromNoAst<TD> NoUnparseByInverseCreatorFromNoAst<TD>()
        {
            return CastValueCreatorFromNoAst<object, TD>(noUnparseByInverseCreatorFromNoAst);
        }

        public static ValueCreatorFromNoAst<object> NoUnparseByInverseCreatorFromNoAst()
        {
            return noUnparseByInverseCreatorFromNoAst;
        }

        internal const string messageForMissingUnparseValueConverter = "Value converter parameter for unparse is missing. "
            + "You should do any of the followings: [ "
            + "1. Specify a value converter for unparse in order to get a functional unparse behavior | "
            + "2. Specify explicitly that you do not want unparse for this BnfiTermConversion by passing NoUnparse | "
            + "3. Disable the warning with \"#pragma warning disable 618\" ]";

        internal const string messageForIntroForBnfiTermConstant = "You do not have to use IntroConstantTerminal for a BnfiTermConstant, since it has a built-in ast creation handling";

        internal const bool errorForMissingUnparseValueConverter = false;    // if not error, then warning

        #endregion

        protected override string GetExtraStrForToString()
        {
            return string.Format("child bnfterm: {0}, value: {1}, isOptionalValue: {2}, utokenizer: {3}, inverse value converter: {4}",
                this.bnfTerm.Name,
                this.value != null ? value.ToString() : "<<NULL>>",
                this.isOptionalValue,
                this.UtokenizerForUnparse != null,
                this.inverseValueConverterForUnparse != null);
        }

        protected static ValueConverter<TInTo, TOutTo> CastValueConverter<TInFrom, TOutFrom, TInTo, TOutTo>(ValueConverter<TInFrom, TOutFrom> valueConverter)
        {
            return obj => (TOutTo)(object)valueConverter((TInFrom)(object)obj);
        }

        protected static ValueCreatorFromNoAst<TTo> CastValueCreatorFromNoAst<TFrom, TTo>(ValueCreatorFromNoAst<TFrom> valueCreator)
        {
            return () => (TTo)(object)valueCreator();
        }

        protected static ValueUtokenizer<object> CastUtokenizerToObject<T>(ValueUtokenizer<T> utokenizer)
        {
            return (formatProvider, reference, obj) => utokenizer(formatProvider, reference, (T)obj);
        }
    }

    public partial class BnfiTermConversionTL : BnfiTermConversion, IBnfiTermTL
    {
        #region Construction

        public BnfiTermConversionTL(string name = null)
            : base(name)
        {
        }

        public BnfiTermConversionTL(Type domainType, string name = null)
            : base(domainType, name)
        {
        }

        internal BnfiTermConversionTL(Type domainType, BnfTerm bnfTerm, object value, object defaultValue, bool isOptionalValue, string name, bool astForChild)
            : base(domainType, bnfTerm, value, defaultValue, isOptionalValue, name, astForChild)
        {
        }

        [Obsolete("Pass either a 'value', or a valueIntroducer with an inverseValueConverterForUnparse", error: true)]
        internal BnfiTermConversionTL(Type domainType, BnfTerm bnfTerm, ValueIntroducer<object> valueIntroducer, object defaultValue, bool isOptionalValue, string name, bool astForChild)
            : this(domainType, bnfTerm, valueIntroducer, NoUnparseByInverse(), defaultValue, isOptionalValue, name, astForChild)
        {
        }

        internal BnfiTermConversionTL(Type domainType, BnfTerm bnfTerm, ValueIntroducer<object> valueIntroducer, ValueConverter<object, object> inverseValueConverterForUnparse, object defaultValue, bool isOptionalValue, string name, bool astForChild)
            : base(domainType, bnfTerm, valueIntroducer, inverseValueConverterForUnparse, defaultValue, isOptionalValue, name, astForChild)
        {
        }

        #endregion

        public new BnfiExpressionConversionTL Rule { set { base.Rule = value; } }

        public BnfiTermConversionTL MakeContractible()
        {
            this.IsContractible = true;
            return this;
        }

        public BnfiTermConversionTL MakeUncontractible()
        {
            this.IsContractible = false;
            return this;
        }
    }

    public partial class BnfiTermConversion<TD> : BnfiTermConversion, IBnfiTerm<TD>, IBnfiTermOrAbleForChoice<TD>, INonTerminal<TD>
    {
        public BnfiTermConversion(string name = null)
            : base(typeof(TD), name)
        {
        }

        internal BnfiTermConversion(BnfTerm bnfTerm, TD value, TD defaultValue, bool isOptionalValue, string name, bool astForChild)
            : base(typeof(TD), bnfTerm, value, defaultValue, isOptionalValue, name, astForChild)
        {
        }

        [Obsolete("Pass either a 'value', or a valueIntroducer with an inverseValueConverterForUnparse", error: true)]
        internal BnfiTermConversion(BnfTerm bnfTerm, ValueIntroducer<TD> valueIntroducer, TD defaultValue, bool isOptionalValue, string name, bool astForChild)
            : this(bnfTerm, valueIntroducer, NoUnparseByInverse<TD>(), defaultValue, isOptionalValue, name, astForChild)
        {
        }

        internal BnfiTermConversion(BnfTerm bnfTerm, ValueIntroducer<TD> valueIntroducer, ValueConverter<TD, object> inverseValueConverterForUnparse, TD defaultValue, bool isOptionalValue, string name, bool astForChild)
            : base(typeof(TD), bnfTerm, (context, parseNode) => valueIntroducer(context, parseNode), CastValueConverter<TD, object, object, object>(inverseValueConverterForUnparse), defaultValue, isOptionalValue, name, astForChild)
        {
        }

        public new ValueUtokenizer<TD> UtokenizerForUnparse
        {
            set
            {
                base.UtokenizerForUnparse = CastUtokenizerToObject(value);
            }
        }

        public BnfiExpressionConversionTL RuleTypeless { set { base.Rule = value; } }

        public new BnfiExpressionConversion<TD> Rule { set { base.Rule = value; } }

        [Obsolete(BnfiTermNonTerminal.typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        public BnfiTermConversion<TD> MakeContractible()
        {
            this.IsContractible = true;
            return this;
        }

        public BnfiTermConversion<TD> MakeUncontractible()
        {
            this.IsContractible = false;
            return this;
        }
    }
}
