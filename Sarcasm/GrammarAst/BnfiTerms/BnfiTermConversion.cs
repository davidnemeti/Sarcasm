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

namespace Sarcasm.GrammarAst
{
    public abstract partial class BnfiTermConversion : BnfiTermNonTerminal, IBnfiTerm, IUnparsableNonTerminal
    {
        #region State

        private BnfTerm bnfTerm;
        private object value;

        private ValueConverter<object, object> inverseValueConverterForUnparse;
        public ValueUtokenizer<object> UtokenizerForUnparse { private get; set; }

        #endregion

        #region Construction

        protected BnfiTermConversion(string name)
            : this(typeof(object), name)
        {
        }

        protected BnfiTermConversion(Type type, string name)
            : base(type, name)
        {
            this.inverseValueConverterForUnparse = IdentityFunction;
            GrammarHelper.MarkTransientForced(this);    // default "transient" behavior (the Rule of this BnfiTermConversion will contain the BnfiTermConversion which actually does something)
        }

        protected BnfiTermConversion(Type type, BnfTerm bnfTerm, object value, bool isOptionalValue, string name, bool astForChild)
            : this(type, bnfTerm, (context, parseNode) => value, IdentityFunction, isOptionalValue, name, astForChild)
        {
            this.value = value;
        }

        [Obsolete("Pass either a 'value', or a valueIntroducer with an inverseValueConverterForUnparse", error: true)]
        protected BnfiTermConversion(Type type, BnfTerm bnfTerm, ValueIntroducer<object> valueIntroducer, bool isOptionalValue, string name, bool astForChild)
            : this(type, bnfTerm, valueIntroducer, NoUnparseByInverse(), isOptionalValue, name, astForChild)
        {
        }

        protected BnfiTermConversion(Type type, BnfTerm bnfTerm, ValueIntroducer<object> valueIntroducer, ValueConverter<object, object> inverseValueConverterForUnparse,
            bool isOptionalValue, string name, bool astForChild)
            : base(type, name)
        {
            this.IsContractible = true;
            this.bnfTerm = bnfTerm;

            if (!astForChild)
                bnfTerm.SetFlag(TermFlags.NoAstNode);

            this.RuleRawWithMove = isOptionalValue
                ? GrammarHelper.PreferShiftHere() + bnfTerm | Irony.Parsing.Grammar.CurrentGrammar.Empty
                : bnfTerm.ToBnfExpression();

            this.AstConfig.NodeCreator = (context, parseTreeNode) =>
                parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(valueIntroducer(context, new ParseTreeNodeWithoutAst(parseTreeNode)), context, parseTreeNode);

            this.inverseValueConverterForUnparse = inverseValueConverterForUnparse;
        }

        #endregion

        #region Intro

        public static BnfiTermConversionTL Intro(Terminal terminal, object value, bool astForChild = true)
        {
            return Intro(typeof(object), terminal, value, astForChild);
        }

        public static BnfiTermConversionTL Intro(Type type, Terminal terminal, object value, bool astForChild = true)
        {
            return new BnfiTermConversionTL(type, terminal, value, isOptionalValue: false, name: terminal.Name + "_parse", astForChild: astForChild).MakeUncontractible();
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
        public static BnfiTermConversionTL Intro(Type type, Terminal terminal, ValueIntroducer<object> valueIntroducer, bool astForChild = true)
        {
            return Intro(type, terminal, valueIntroducer, NoUnparseByInverse(), astForChild);
        }

        public static BnfiTermConversionTL Intro(Type type, Terminal terminal, ValueIntroducer<object> valueIntroducer, ValueConverter<object, object> inverseValueConverterForUnparse, bool astForChild = true)
        {
            return new BnfiTermConversionTL(
                type,
                terminal,
                (context, parseNode) => valueIntroducer(context, parseNode),
                inverseValueConverterForUnparse,
                isOptionalValue: false,
                name: terminal.Name + "_intro",
                astForChild: astForChild
                )
                .MakeUncontractible();
        }

        public static BnfiTermConversion<T> Intro<T>(Terminal terminal, T value, bool astForChild = true)
        {
            return new BnfiTermConversion<T>(terminal, value, isOptionalValue: false, name: terminal.Name + "_parse", astForChild: astForChild).MakeUncontractible();
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
                isOptionalValue: false,
                name: terminal.Name + "_introAs_" + typeof(T).Name.ToLower(),
                astForChild: astForChild
                )
                .MakeUncontractible();
        }

        public static BnfiTermConversion<string> IntroIdentifier(IdentifierTerminal identifierTerminal)
        {
            return Intro<string>(identifierTerminal, (context, parseNode) => parseNode.FindTokenAndGetText(), IdentityFunction, astForChild: false);
        }

        public static BnfiTermConversion<string> IntroStringLiteral(StringLiteral stringLiteral)
        {
            var bnfiTermConversion = Intro<string>(
                stringLiteral,
                (context, parseNode) =>
                {
                    string quotedStr = parseNode.FindTokenAndGetText();
                    var subType = stringLiteral._subtypes.First(_subType => quotedStr.StartsWith(_subType.Start) && quotedStr.EndsWith(_subType.End));
                    return quotedStr
                        .Remove(quotedStr.Length - subType.End.Length)
                        .Remove(0, subType.Start.Length);
                },
//                NoUnparseByInverse<string>(),
                str =>
                {
                    var subType = stringLiteral._subtypes.First();
                    return subType.Start + str + subType.End;
                },
                astForChild: false
                );

            //bnfiTermConversion.UtokenizerForUnparse =
            //    (formatProvider, reference, astValue) =>
            //    {
            //        var subType = stringLiteral._subtypes.First();
            //        return new UtokenValue[] { UtokenValue.CreateText(subType.Start), UtokenValue.CreateText(reference), UtokenValue.CreateText(subType.End) };
            //    };

            return bnfiTermConversion;
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
        public static BnfiTermConversionTL Convert(Type type, IBnfiTermTL bnfiTerm, ValueConverter<object, object> valueConverter)
        {
            return Convert(type, bnfiTerm, valueConverter, NoUnparseByInverse());
        }

        public static BnfiTermConversionTL Convert(Type type, IBnfiTermTL bnfiTerm, ValueConverter<object, object> valueConverter, ValueConverter<object, object> inverseValueConverterForUnparse)
        {
            return new BnfiTermConversionTL(
                type,
                bnfiTerm.AsBnfTerm(),
                ConvertValueConverterToValueIntroducer(valueConverter),
                inverseValueConverterForUnparse,
                isOptionalValue: false,
                name: bnfiTerm.AsBnfTerm().Name + "_convert",
                astForChild: true
                );
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermConversion<TOut> Convert<TOut>(IBnfiTermTL bnfiTerm, ValueConverter<object, TOut> valueConverter)
        {
            return Convert<TOut>(bnfiTerm, valueConverter, NoUnparseByInverse<TOut, object>());
        }

        public static BnfiTermConversion<TOut> Convert<TOut>(IBnfiTermTL bnfiTerm, ValueConverter<object, TOut> valueConverter, ValueConverter<TOut, object> inverseValueConverterForUnparse)
        {
            return new BnfiTermConversion<TOut>(
                bnfiTerm.AsBnfTerm(),
                ConvertValueConverterToValueIntroducer(valueConverter),
                inverseValueConverterForUnparse,
                isOptionalValue: false,
                name: bnfiTerm.AsBnfTerm().Name + "_convertTo_" + typeof(TOut).Name.ToLower(),
                astForChild: true
                );
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermConversion<TOut> Convert<TIn, TOut>(IBnfiTerm<TIn> bnfiTerm, ValueConverter<TIn, TOut> valueConverter)
        {
            return Convert<TIn, TOut>(bnfiTerm, valueConverter, NoUnparseByInverse<TOut, TIn>());
        }

        public static BnfiTermConversion<TOut> Convert<TIn, TOut>(IBnfiTerm<TIn> bnfiTerm, ValueConverter<TIn, TOut> valueConverter, ValueConverter<TOut, TIn> inverseValueConverterForUnparse)
        {
            return new BnfiTermConversion<TOut>(
                bnfiTerm.AsBnfTerm(),
                ConvertValueConverterToValueIntroducer(valueConverter),
                CastValueConverter<TOut, TIn, TOut, object>(inverseValueConverterForUnparse),
                isOptionalValue: false,
                name: typeof(TIn).Name.ToLower() + "_convertTo_" + typeof(TOut).Name.ToLower(),
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
        public static BnfiTermConversion<TOut?> ConvertOptVal<TIn, TOut>(IBnfiTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
            where TIn : struct
            where TOut : struct
        {
            return ConvertOptVal<TIn, TOut>(bnfTerm, valueConverter, NoUnparseByInverse<TOut, TIn>());
        }

        public static BnfiTermConversion<TOut?> ConvertOptVal<TIn, TOut>(IBnfiTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter, ValueConverter<TOut, TIn> inverseValueConverterForUnparse)
            where TIn : struct
            where TOut : struct
        {
            return ConvertOpt<TIn, TOut?>(bnfTerm, value => valueConverter(value), value => inverseValueConverterForUnparse(value.Value));
        }

        public static BnfiTermConversion<T> ConvertOptRef<T>(IBnfiTerm<T> bnfTerm)
            where T : class
        {
            return ConvertOptRef(bnfTerm, value => value, value => value);
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermConversion<TOut> ConvertOptRef<TIn, TOut>(IBnfiTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
            where TIn : class
            where TOut : class
        {
            return ConvertOptRef<TIn, TOut>(bnfTerm, valueConverter, NoUnparseByInverse<TOut, TIn>());
        }

        public static BnfiTermConversion<TOut> ConvertOptRef<TIn, TOut>(IBnfiTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter, ValueConverter<TOut, TIn> inverseValueConverterForUnparse)
            where TIn : class
            where TOut : class
        {
            return ConvertOpt<TIn, TOut>(bnfTerm, valueConverter, inverseValueConverterForUnparse);
        }

        public static BnfiTermConversion<T> ConvertOptVal<T>(IBnfiTerm<T> bnfiTerm, T defaultValue)
            where T : struct
        {
            return ConvertOptVal(bnfiTerm, value => value, value => value, defaultValue);
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermConversion<TOut> ConvertOptVal<TIn, TOut>(IBnfiTerm<TIn> bnfiTerm, ValueConverter<TIn, TOut> valueConverter, TOut defaultValue)
            where TIn : struct
            where TOut : struct
        {
            return ConvertOptVal<TIn, TOut>(bnfiTerm, valueConverter, NoUnparseByInverse<TOut, TIn>(), defaultValue);
        }

        public static BnfiTermConversion<TOut> ConvertOptVal<TIn, TOut>(IBnfiTerm<TIn> bnfiTerm, ValueConverter<TIn, TOut> valueConverter,
            ValueConverter<TOut, TIn> inverseValueConverterForUnparse, TOut defaultValue)
            where TIn : struct
            where TOut : struct
        {
            return ConvertOpt<TIn, TOut>(bnfiTerm, valueConverter, inverseValueConverterForUnparse, defaultValue);
        }

        public static BnfiTermConversion<T> ConvertOptRef<T>(IBnfiTerm<T> bnfiTerm, T defaultValue)
            where T : class
        {
            return ConvertOptRef(bnfiTerm, value => value, value => value, defaultValue);
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermConversion<TOut> ConvertOptRef<TIn, TOut>(IBnfiTerm<TIn> bnfiTerm, ValueConverter<TIn, TOut> valueConverter, TOut defaultValue)
            where TIn : class
            where TOut : class
        {
            return ConvertOptRef<TIn, TOut>(bnfiTerm, valueConverter, NoUnparseByInverse<TOut, TIn>(), defaultValue);
        }

        public static BnfiTermConversion<TOut> ConvertOptRef<TIn, TOut>(IBnfiTerm<TIn> bnfiTerm, ValueConverter<TIn, TOut> valueConverter,
            ValueConverter<TOut, TIn> inverseValueConverterForUnparse, TOut defaultValue)
            where TIn : class
            where TOut : class
        {
            return ConvertOpt(bnfiTerm, valueConverter, inverseValueConverterForUnparse, defaultValue);
        }

        protected static BnfiTermConversion<TOut> ConvertOpt<TIn, TOut>(IBnfiTerm<TIn> bnfiTerm, ValueConverter<TIn, TOut> valueConverter,
            ValueConverter<TOut, TIn> inverseValueConverterForUnparse, TOut defaultValue = default(TOut))
        {
            return new BnfiTermConversion<TOut>(
                bnfiTerm.AsBnfTerm(),
                ConvertValueConverterToValueIntroducerOpt(valueConverter, defaultValue),
                CastValueConverter<TOut, TIn, TOut, object>(inverseValueConverterForUnparse),
                isOptionalValue: true,
                name: typeof(TIn).Name.ToLower() + "_convertOptTo_" + typeof(TOut).Name.ToLower(),
                astForChild: true
                );
        }

        #endregion

        #region Cast

        public static BnfiTermConversion<TOut> Cast<TIn, TOut>(IBnfiTerm<TIn> bnfTerm)
        {
            return Convert(bnfTerm, IdentityFunctionForceCast<TIn, TOut>, IdentityFunctionForceCast<TOut, TIn>);
        }

        public static BnfiTermConversion<TOut> Cast<TOut>(Terminal terminal)
        {
            return Intro<TOut>(terminal, (context, parseNode) => GrammarHelper.AstNodeToValue<TOut>(parseNode.Token.Value), IdentityFunctionForceCast<TOut, object>);
        }

        #endregion

        #region Helpers for Convert and ConvertOpt

        protected static ValueIntroducer<TOut> ConvertValueConverterToValueIntroducer<TIn, TOut>(ValueConverter<TIn, TOut> valueConverter)
        {
            return ConvertValueConverterToValueIntroducer(valueConverter, isOptionalValue: false, defaultValue: default(TOut));   // defaultValue won't be used
        }

        protected static ValueIntroducer<TOut> ConvertValueConverterToValueIntroducerOpt<TIn, TOut>(ValueConverter<TIn, TOut> valueConverter, TOut defaultValue)
        {
            return ConvertValueConverterToValueIntroducer(valueConverter, isOptionalValue: true, defaultValue: defaultValue);
        }

        private static ValueIntroducer<TOut> ConvertValueConverterToValueIntroducer<TIn, TOut>(ValueConverter<TIn, TOut> valueConverter, bool isOptionalValue, TOut defaultValue)
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
                    ? valueConverter(GrammarHelper.AstNodeToValue<TIn>(parseTreeChild.AstNode))
                    : defaultValue;
            };
        }

        #endregion

        protected static object IdentityFunction(object astValue)
        {
            return astValue;
        }

        protected static TOut IdentityFunctionForceCast<TIn, TOut>(TIn astValue)
        {
            return (TOut)(object)astValue;
        }

        protected void SetState(BnfiTermConversion source)
        {
            this.bnfTerm = source.bnfTerm;
            this.value = source.value;
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

        bool IUnparsableNonTerminal.TryGetUtokensDirectly(IUnparser unparser, UnparsableAst self, out IEnumerable<UtokenValue> utokens)
        {
            if (this.UtokenizerForUnparse != null)
            {
                utokens = this.UtokenizerForUnparse(unparser.FormatProvider, self, self.AstValue);
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

        IEnumerable<UnparsableAst> IUnparsableNonTerminal.GetChildren(Unparser.ChildBnfTerms childBnfTerms, object astValue, Unparser.Direction direction)
        {
            return childBnfTerms.Select(childBnfTerm => new UnparsableAst(childBnfTerm, ConvertAstValueForChild(astValue, childBnfTerm)));
        }

        int? IUnparsableNonTerminal.GetChildrenPriority(IUnparser unparser, object astValue, Unparser.Children children, Unparser.Direction direction)
        {
            if (this.value != null)
                return this.value.Equals(astValue) ? (int?)1 : null;
            else if (this.UtokenizerForUnparse != null)
                return 1;
            else
                return unparser.GetPriority(GetMainChild(astValue, children));
        }

        private static bool IsMainChild(BnfTerm bnfTerm)
        {
            return !bnfTerm.Flags.IsSet(TermFlags.IsPunctuation) && !(bnfTerm is GrammarHint);
        }

        private UnparsableAst GetMainChild(object astValue, IEnumerable<UnparsableAst> children)
        {
            return this.bnfTerm != null
                ? new UnparsableAst(this.bnfTerm, ConvertAstValueForChild(astValue, this.bnfTerm))
                : children.Single(child => IsMainChild(child.BnfTerm));    // "transient" unparse with the actual BnfiTermConversion(s) under the current one (set by Rule)
        }

        private object ConvertAstValueForChild(object astValue, BnfTerm childBnfTerm)
        {
            if (IsMainChild(childBnfTerm))
                return this.inverseValueConverterForUnparse(astValue);
            else
                return astValue;
        }

        private static readonly ValueConverter<object, object> noUnparseByInverse =
            value =>
            {
                throw new UnparseException("Cannot unparse. Inverse value converter parameter for unparse is missing");
            };

        public static ValueConverter<object, object> NoUnparseByInverse()
        {
            return noUnparseByInverse;
        }

        public static ValueConverter<T, object> NoUnparseByInverse<T>()
        {
            return CastValueConverter<object, object, T, object>(noUnparseByInverse);
        }

        public static ValueConverter<TIn, TOut> NoUnparseByInverse<TIn, TOut>()
        {
            return CastValueConverter<object, object, TIn, TOut>(noUnparseByInverse);
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
            return string.Format("child bnfterm: {0}, value: {1}, utokenizer: {2}, inverse value converter: {3}",
                this.bnfTerm.Name, this.value != null ? value.ToString() : "<<NULL>>", this.UtokenizerForUnparse != null, this.inverseValueConverterForUnparse != null);
        }

        protected static ValueConverter<TInTo, TOutTo> CastValueConverter<TInFrom, TOutFrom, TInTo, TOutTo>(ValueConverter<TInFrom, TOutFrom> valueConverter)
        {
            return obj => (TOutTo)(object)valueConverter((TInFrom)(object)obj);
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

        public BnfiTermConversionTL(Type type, string name = null)
            : base(type, name)
        {
        }

        internal BnfiTermConversionTL(Type type, BnfTerm bnfTerm, object value, bool isOptionalValue, string name, bool astForChild)
            : base(type, bnfTerm, value, isOptionalValue, name, astForChild)
        {
        }

        [Obsolete("Pass either a 'value', or a valueIntroducer with an inverseValueConverterForUnparse", error: true)]
        internal BnfiTermConversionTL(Type type, BnfTerm bnfTerm, ValueIntroducer<object> valueIntroducer, bool isOptionalValue, string name, bool astForChild)
            : this(type, bnfTerm, valueIntroducer, NoUnparseByInverse(), isOptionalValue, name, astForChild)
        {
        }

        internal BnfiTermConversionTL(Type type, BnfTerm bnfTerm, ValueIntroducer<object> valueIntroducer, ValueConverter<object, object> inverseValueConverterForUnparse, bool isOptionalValue, string name, bool astForChild)
            : base(type, bnfTerm, valueIntroducer, inverseValueConverterForUnparse, isOptionalValue, name, astForChild)
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

    public partial class BnfiTermConversion<T> : BnfiTermConversion, IBnfiTerm<T>, IBnfiTermOrAbleForChoice<T>, INonTerminal<T>
    {
        public BnfiTermConversion(string name = null)
            : base(typeof(T), name)
        {
        }

        internal BnfiTermConversion(BnfTerm bnfTerm, T value, bool isOptionalValue, string name, bool astForChild)
            : base(typeof(T), bnfTerm, value, isOptionalValue, name, astForChild)
        {
        }

        [Obsolete("Pass either a 'value', or a valueIntroducer with an inverseValueConverterForUnparse", error: true)]
        internal BnfiTermConversion(BnfTerm bnfTerm, ValueIntroducer<T> valueIntroducer, bool isOptionalValue, string name, bool astForChild)
            : this(bnfTerm, valueIntroducer, NoUnparseByInverse<T>(), isOptionalValue, name, astForChild)
        {
        }

        internal BnfiTermConversion(BnfTerm bnfTerm, ValueIntroducer<T> valueIntroducer, ValueConverter<T, object> inverseValueConverterForUnparse, bool isOptionalValue, string name, bool astForChild)
            : base(typeof(T), bnfTerm, (context, parseNode) => valueIntroducer(context, parseNode), CastValueConverter<T, object, object, object>(inverseValueConverterForUnparse), isOptionalValue, name, astForChild)
        {
        }

        public new ValueUtokenizer<T> UtokenizerForUnparse
        {
            set
            {
                base.UtokenizerForUnparse = CastUtokenizerToObject(value);
            }
        }

        public BnfiExpressionConversionTL RuleTypeless { set { base.Rule = value; } }

        public new BnfiExpressionConversion<T> Rule { set { base.Rule = value; } }

        [Obsolete(BnfiTermNonTerminal.typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        public BnfiTermConversion<T> MakeContractible()
        {
            this.IsContractible = true;
            return this;
        }

        public BnfiTermConversion<T> MakeUncontractible()
        {
            this.IsContractible = false;
            return this;
        }
    }
}
