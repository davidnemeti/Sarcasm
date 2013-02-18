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
using Sarcasm.Unparsing;

namespace Sarcasm.Ast
{
    public abstract partial class BnfiTermValue : BnfiTermNonTerminal, IBnfiTerm, IUnparsableNonTerminal
    {
        #region State

        private BnfTerm bnfTerm;
        private object value;

        private ValueConverter<object, object> inverseValueConverterForUnparse;
        public ValueUtokenizer<object> UtokenizerForUnparse { private get; set; }

        #endregion

        #region Construction

        protected BnfiTermValue(string name)
            : this(typeof(object), name)
        {
        }

        protected BnfiTermValue(Type type, string name)
            : base(type, name, isReferable: true)
        {
            this.inverseValueConverterForUnparse = IdentityFunction;
            GrammarHelper.MarkTransientForced(this);    // default "transient" behavior (the Rule of this BnfiTermValue will contain the BnfiTermValue which actually does something)
        }

        protected BnfiTermValue(Type type, BnfTerm bnfTerm, object value, bool isOptionalValue, string name, bool astForChild)
            : this(type, bnfTerm, (context, parseNode) => value, IdentityFunction, isOptionalValue, name, astForChild)
        {
            this.value = value;
        }

        [Obsolete("Pass either a 'value', or a valueParser with an inverseValueConverterForUnparse", error: true)]
        protected BnfiTermValue(Type type, BnfTerm bnfTerm, ValueParser<object> valueParser, bool isOptionalValue, string name, bool astForChild)
            : this(type, bnfTerm, valueParser, NoUnparseByInverse(), isOptionalValue, name, astForChild)
        {
        }

        protected BnfiTermValue(Type type, BnfTerm bnfTerm, ValueParser<object> valueParser, ValueConverter<object, object> inverseValueConverterForUnparse,
            bool isOptionalValue, string name, bool astForChild)
            : base(type, name, isReferable: false)
        {
            this.bnfTerm = bnfTerm;

            if (!astForChild)
                bnfTerm.SetFlag(TermFlags.NoAstNode);

            this.RuleRawWithMove = isOptionalValue
                ? GrammarHelper.PreferShiftHere() + bnfTerm | Irony.Parsing.Grammar.CurrentGrammar.Empty
                : bnfTerm.ToBnfExpression();

            this.AstConfig.NodeCreator = (context, parseTreeNode) =>
                parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(valueParser(context, new ParseTreeNodeWithOutAst(parseTreeNode)), context, parseTreeNode);

            this.inverseValueConverterForUnparse = inverseValueConverterForUnparse;
        }

        #endregion

        #region Parse

        public static BnfiTermValueTL Parse(Terminal terminal, object value, bool astForChild = true)
        {
            return Parse(typeof(object), terminal, value, astForChild);
        }

        public static BnfiTermValueTL Parse(Type type, Terminal terminal, object value, bool astForChild = true)
        {
            return new BnfiTermValueTL(type, terminal, value, isOptionalValue: false, name: terminal.Name + "_parse", astForChild: astForChild);
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermValueTL Parse(Terminal terminal, ValueParser<object> valueParser, bool astForChild = true)
        {
            return Parse(terminal, valueParser, NoUnparseByInverse(), astForChild);
        }

        public static BnfiTermValueTL Parse(Terminal terminal, ValueParser<object> valueParser, ValueConverter<object, object> inverseValueConverterForUnparse, bool astForChild = true)
        {
            return Parse(typeof(object), terminal, valueParser, inverseValueConverterForUnparse, astForChild);
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermValueTL Parse(Type type, Terminal terminal, ValueParser<object> valueParser, bool astForChild = true)
        {
            return Parse(type, terminal, valueParser, NoUnparseByInverse(), astForChild);
        }

        public static BnfiTermValueTL Parse(Type type, Terminal terminal, ValueParser<object> valueParser, ValueConverter<object, object> inverseValueConverterForUnparse, bool astForChild = true)
        {
            return new BnfiTermValueTL(type, terminal, (context, parseNode) => valueParser(context, parseNode), inverseValueConverterForUnparse, isOptionalValue: false,
                name: terminal.Name + "_parse", astForChild: astForChild);
        }

        public static BnfiTermValue<T> Parse<T>(Terminal terminal, T value, bool astForChild = true)
        {
            return new BnfiTermValue<T>(terminal, value, isOptionalValue: false, name: terminal.Name + "_parse", astForChild: astForChild);
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermValue<T> Parse<T>(Terminal terminal, ValueParser<T> valueParser, bool astForChild = true)
        {
            return Parse(terminal, valueParser, NoUnparseByInverse<T>(), astForChild);
        }

        public static BnfiTermValue<T> Parse<T>(Terminal terminal, ValueParser<T> valueParser, ValueConverter<T, object> inverseValueConverterForUnparse, bool astForChild = true)
        {
            return new BnfiTermValue<T>(terminal, (context, parseNode) => valueParser(context, parseNode), inverseValueConverterForUnparse, isOptionalValue: false,
                name: terminal.Name + "_parse", astForChild: astForChild);
        }

        public static BnfiTermValue<string> ParseIdentifier(IdentifierTerminal identifierTerminal)
        {
            return Parse<string>(identifierTerminal, (context, parseNode) => parseNode.FindTokenAndGetText(), IdentityFunction, astForChild: false);
        }

        public static BnfiTermValue<string> ParseStringLiteral(StringLiteral stringLiteral)
        {
            return Parse<string>(stringLiteral, (context, parseNode) => parseNode.FindTokenAndGetText(), IdentityFunction, astForChild: false);
        }

        #endregion

        #region Convert

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermValueTL Convert(IBnfiTermTL bnfiTerm, ValueConverter<object, object> valueConverter)
        {
            return Convert(bnfiTerm, valueConverter, NoUnparseByInverse());
        }

        public static BnfiTermValueTL Convert(IBnfiTermTL bnfiTerm, ValueConverter<object, object> valueConverter, ValueConverter<object, object> inverseValueConverterForUnparse)
        {
            return Convert(typeof(object), bnfiTerm, valueConverter, inverseValueConverterForUnparse);
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermValueTL Convert(Type type, IBnfiTermTL bnfiTerm, ValueConverter<object, object> valueConverter)
        {
            return Convert(type, bnfiTerm, valueConverter, NoUnparseByInverse());
        }

        public static BnfiTermValueTL Convert(Type type, IBnfiTermTL bnfiTerm, ValueConverter<object, object> valueConverter, ValueConverter<object, object> inverseValueConverterForUnparse)
        {
            return new BnfiTermValueTL(
                type,
                bnfiTerm.AsBnfTerm(),
                ConvertValueConverterToValueParser(valueConverter),
                inverseValueConverterForUnparse,
                isOptionalValue: false,
                name: bnfiTerm.AsBnfTerm().Name + "_convert",
                astForChild: true
                );
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermValue<TOut> Convert<TOut>(IBnfiTermTL bnfiTerm, ValueConverter<object, TOut> valueConverter)
        {
            return Convert<TOut>(bnfiTerm, valueConverter, NoUnparseByInverse<TOut, object>());
        }

        public static BnfiTermValue<TOut> Convert<TOut>(IBnfiTermTL bnfiTerm, ValueConverter<object, TOut> valueConverter, ValueConverter<TOut, object> inverseValueConverterForUnparse)
        {
            return new BnfiTermValue<TOut>(
                bnfiTerm.AsBnfTerm(),
                ConvertValueConverterToValueParser(valueConverter),
                inverseValueConverterForUnparse,
                isOptionalValue: false,
                name: bnfiTerm.AsBnfTerm().Name + "_convert",
                astForChild: true
                );
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermValue<TOut> Convert<TIn, TOut>(IBnfiTerm<TIn> bnfiTerm, ValueConverter<TIn, TOut> valueConverter)
        {
            return Convert<TIn, TOut>(bnfiTerm, valueConverter, NoUnparseByInverse<TOut, TIn>());
        }

        public static BnfiTermValue<TOut> Convert<TIn, TOut>(IBnfiTerm<TIn> bnfiTerm, ValueConverter<TIn, TOut> valueConverter, ValueConverter<TOut, TIn> inverseValueConverterForUnparse)
        {
            return new BnfiTermValue<TOut>(
                bnfiTerm.AsBnfTerm(),
                ConvertValueConverterToValueParser(valueConverter),
                CastValueConverter<TOut, TIn, TOut, object>(inverseValueConverterForUnparse),
                isOptionalValue: false,
                name: bnfiTerm.AsBnfTerm().Name + "_convertfrom_" + typeof(TIn).Name.ToLower(),
                astForChild: true
                );
        }

        #endregion

        #region ConvertOpt

        public static BnfiTermValue<T?> ConvertOptVal<T>(IBnfiTerm<T> bnfTerm)
            where T : struct
        {
            return ConvertOptVal(bnfTerm, value => value, value => value);
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermValue<TOut?> ConvertOptVal<TIn, TOut>(IBnfiTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
            where TIn : struct
            where TOut : struct
        {
            return ConvertOptVal<TIn, TOut>(bnfTerm, valueConverter, NoUnparseByInverse<TOut, TIn>());
        }

        public static BnfiTermValue<TOut?> ConvertOptVal<TIn, TOut>(IBnfiTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter, ValueConverter<TOut, TIn> inverseValueConverterForUnparse)
            where TIn : struct
            where TOut : struct
        {
            return ConvertOpt<TIn, TOut?>(bnfTerm, value => valueConverter(value), value => inverseValueConverterForUnparse(value.Value));
        }

        public static BnfiTermValue<T> ConvertOptRef<T>(IBnfiTerm<T> bnfTerm)
            where T : class
        {
            return ConvertOptRef(bnfTerm, value => value, value => value);
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermValue<TOut> ConvertOptRef<TIn, TOut>(IBnfiTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
            where TIn : class
            where TOut : class
        {
            return ConvertOptRef<TIn, TOut>(bnfTerm, valueConverter, NoUnparseByInverse<TOut, TIn>());
        }

        public static BnfiTermValue<TOut> ConvertOptRef<TIn, TOut>(IBnfiTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter, ValueConverter<TOut, TIn> inverseValueConverterForUnparse)
            where TIn : class
            where TOut : class
        {
            return ConvertOpt<TIn, TOut>(bnfTerm, valueConverter, inverseValueConverterForUnparse);
        }

        public static BnfiTermValue<T> ConvertOptVal<T>(IBnfiTerm<T> bnfiTerm, T defaultValue)
            where T : struct
        {
            return ConvertOptVal(bnfiTerm, value => value, value => value, defaultValue);
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermValue<TOut> ConvertOptVal<TIn, TOut>(IBnfiTerm<TIn> bnfiTerm, ValueConverter<TIn, TOut> valueConverter, TOut defaultValue)
            where TIn : struct
            where TOut : struct
        {
            return ConvertOptVal<TIn, TOut>(bnfiTerm, valueConverter, NoUnparseByInverse<TOut, TIn>(), defaultValue);
        }

        public static BnfiTermValue<TOut> ConvertOptVal<TIn, TOut>(IBnfiTerm<TIn> bnfiTerm, ValueConverter<TIn, TOut> valueConverter,
            ValueConverter<TOut, TIn> inverseValueConverterForUnparse, TOut defaultValue)
            where TIn : struct
            where TOut : struct
        {
            return ConvertOpt<TIn, TOut>(bnfiTerm, valueConverter, inverseValueConverterForUnparse, defaultValue);
        }

        public static BnfiTermValue<T> ConvertOptRef<T>(IBnfiTerm<T> bnfiTerm, T defaultValue)
            where T : class
        {
            return ConvertOptRef(bnfiTerm, value => value, value => value, defaultValue);
        }

        [Obsolete(messageForMissingUnparseValueConverter, errorForMissingUnparseValueConverter)]
        public static BnfiTermValue<TOut> ConvertOptRef<TIn, TOut>(IBnfiTerm<TIn> bnfiTerm, ValueConverter<TIn, TOut> valueConverter, TOut defaultValue)
            where TIn : class
            where TOut : class
        {
            return ConvertOptRef<TIn, TOut>(bnfiTerm, valueConverter, NoUnparseByInverse<TOut, TIn>(), defaultValue);
        }

        public static BnfiTermValue<TOut> ConvertOptRef<TIn, TOut>(IBnfiTerm<TIn> bnfiTerm, ValueConverter<TIn, TOut> valueConverter,
            ValueConverter<TOut, TIn> inverseValueConverterForUnparse, TOut defaultValue)
            where TIn : class
            where TOut : class
        {
            return ConvertOptRef(bnfiTerm, valueConverter, inverseValueConverterForUnparse, defaultValue);
        }

        protected static BnfiTermValue<TOut> ConvertOpt<TIn, TOut>(IBnfiTerm<TIn> bnfiTerm, ValueConverter<TIn, TOut> valueConverter,
            ValueConverter<TOut, TIn> inverseValueConverterForUnparse, TOut defaultValue = default(TOut))
        {
            return new BnfiTermValue<TOut>(
                bnfiTerm.AsBnfTerm(),
                ConvertValueConverterToValueParserOpt(valueConverter, defaultValue),
                CastValueConverter<TOut, TIn, TOut, object>(inverseValueConverterForUnparse),
                isOptionalValue: true,
                name: bnfiTerm.AsBnfTerm().Name + "_convertoptfrom_" + typeof(TIn).Name.ToLower(),
                astForChild: true
                );
        }

        #endregion

        #region Cast

        public static BnfiTermValue<TOut> Cast<TIn, TOut>(IBnfiTerm<TIn> bnfTerm)
        {
            return Convert(bnfTerm, IdentityFunctionForceCast<TIn, TOut>, IdentityFunctionForceCast<TOut, TIn>);
        }

        public static BnfiTermValue<TOut> Cast<TOut>(Terminal terminal)
        {
            return Parse<TOut>(terminal, (context, parseNode) => (TOut)GrammarHelper.AstNodeToValue(parseNode.Token.Value), IdentityFunctionForceCast<TOut, object>);
        }

        #endregion

        #region Helpers for Convert and ConvertOpt

        protected static ValueParser<TOut> ConvertValueConverterToValueParser<TIn, TOut>(ValueConverter<TIn, TOut> valueConverter)
        {
            return ConvertValueConverterToValueParser(valueConverter, isOptionalValue: false, defaultValue: default(TOut));   // defaultValue won't be used
        }

        protected static ValueParser<TOut> ConvertValueConverterToValueParserOpt<TIn, TOut>(ValueConverter<TIn, TOut> valueConverter, TOut defaultValue)
        {
            return ConvertValueConverterToValueParser(valueConverter, isOptionalValue: true, defaultValue: defaultValue);
        }

        private static ValueParser<TOut> ConvertValueConverterToValueParser<TIn, TOut>(ValueConverter<TIn, TOut> valueConverter, bool isOptionalValue, TOut defaultValue)
        {
            return (context, parseTreeNode) =>
            {
                try
                {
                    Func<IEnumerable<ParseTreeNode>, Func<ParseTreeNode, bool>, ParseTreeNode> chooser;
                    if (isOptionalValue)
                        chooser = Enumerable.SingleOrDefault<ParseTreeNode>;
                    else
                        chooser = Enumerable.Single<ParseTreeNode>;

                    ParseTreeNode parseTreeChild = chooser(parseTreeNode.ChildNodes, childNode => childNode.AstNode != null);

                    return parseTreeChild != null
                        ? valueConverter(GrammarHelper.AstNodeToValue<TIn>(parseTreeChild.AstNode))
                        : defaultValue;
                }
                catch (InvalidOperationException)
                {
                    if (isOptionalValue)
                        throw new ArgumentException("Only zero or one child with ast node is allowed for an optional BnfiTermValue term: {0}", parseTreeNode.Term.Name);
                    else
                        throw new ArgumentException("Exactly one child with ast node is allowed for a non-optional BnfiTermValue term: {0}", parseTreeNode.Term.Name);
                }
            };
        }

        #endregion

        protected static object IdentityFunction(object obj)
        {
            return obj;
        }

        protected static TOut IdentityFunctionForceCast<TIn, TOut>(TIn obj)
        {
            return (TOut)(object)obj;
        }

        protected void SetState(BnfiTermValue source)
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

        protected void MoveTo(BnfiTermValue target)
        {
            if (!this.IsMovable)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "This value should not be a right-value: {0}", this.Name);

            target.RuleRaw = this.RuleRaw;
            target.SetState(this);

            this.RuleRaw = null;
            this.ClearState();
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
                 * Examine whether there is only one single BnfiTermValue inside 'value', and if it is true, then move the state of that BnfiTermValue
                 * so we can "destroy" that BnfiTermValue in order to eliminate unneccessary, extra "rule-embedding"
                 * */
                var bnfTerms = value.GetBnfTermsList().SingleOrDefaultNoException();
                if (bnfTerms == null)
                    goto LDefaultRuleSetting;

                BnfiTermValue onlyBnfTermInValue = bnfTerms.SingleOrDefaultNoException() as BnfiTermValue;

                if (onlyBnfTermInValue != null && onlyBnfTermInValue.IsMovable)
                    onlyBnfTermInValue.MoveTo(this);
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

        bool IUnparsableNonTerminal.TryGetUtokensDirectly(IUnparser unparser, object obj, out IEnumerable<UtokenValue> utokens)
        {
            if (this.UtokenizerForUnparse != null)
            {
                utokens = this.UtokenizerForUnparse(unparser.FormatProvider, obj);
                return true;
            }
            else if (this.inverseValueConverterForUnparse != null && this.inverseValueConverterForUnparse != NoUnparseByInverse())
            {
                utokens = null;
                return false;
            }
            else
                throw new UnparseException(string.Format("Cannot unparse. '{0}' has neither UtokenizerForUnparse nor ValueConverterForUnparse", this.Name));
        }

        IEnumerable<UnparsableObject> IUnparsableNonTerminal.GetChildren(BnfTermList childBnfTerms, object obj)
        {
            return childBnfTerms.Select(childBnfTerm => new UnparsableObject(childBnfTerm, ConvertObjectForChild(obj, childBnfTerm)));
        }

        int? IUnparsableNonTerminal.GetChildrenPriority(IUnparser unparser, object obj, IEnumerable<UnparsableObject> children)
        {
            if (this.value != null)
                return this.value.Equals(obj) ? (int?)1 : null;
            else
                return unparser.GetPriority(GetMainChild(obj, children));
        }

        private static bool IsMainChild(BnfTerm bnfTerm)
        {
            return !bnfTerm.Flags.IsSet(TermFlags.IsPunctuation) && !(bnfTerm is GrammarHint);
        }

        private UnparsableObject GetMainChild(object obj, IEnumerable<UnparsableObject> children)
        {
            return this.bnfTerm != null
                ? new UnparsableObject(this.bnfTerm, ConvertObjectForChild(obj, this.bnfTerm))
                : children.Single(child => IsMainChild(child.BnfTerm));    // "transient" unparse with the actual BnfiTermValue(s) under the current one (set by Rule)
        }

        private object ConvertObjectForChild(object obj, BnfTerm childBnfTerm)
        {
            if (IsMainChild(childBnfTerm))
                return this.inverseValueConverterForUnparse(obj);
            else
                return obj;
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
            + "2. Specify explicitly that you do not want unparse for this BnfiTermValue by passing NoUnparse | "
            + "3. Disable the warning with \"#pragma warning disable 618\" ]";

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
            return (formatProvider, obj) => utokenizer(formatProvider, (T)obj);
        }
    }

    public partial class BnfiTermValueTL : BnfiTermValue, IBnfiTermTL
    {
        #region Construction

        public BnfiTermValueTL(string name = null)
            : base(name)
        {
        }

        public BnfiTermValueTL(Type type, string name = null)
            : base(type, name)
        {
        }

        internal BnfiTermValueTL(Type type, BnfTerm bnfTerm, object value, bool isOptionalValue, string name, bool astForChild)
            : base(type, bnfTerm, value, isOptionalValue, name, astForChild)
        {
        }

        [Obsolete("Pass either a 'value', or a valueParser with an inverseValueConverterForUnparse", error: true)]
        internal BnfiTermValueTL(Type type, BnfTerm bnfTerm, ValueParser<object> valueParser, bool isOptionalValue, string name, bool astForChild)
            : this(type, bnfTerm, valueParser, NoUnparseByInverse(), isOptionalValue, name, astForChild)
        {
        }

        internal BnfiTermValueTL(Type type, BnfTerm bnfTerm, ValueParser<object> valueParser, ValueConverter<object, object> inverseValueConverterForUnparse, bool isOptionalValue, string name, bool astForChild)
            : base(type, bnfTerm, valueParser, inverseValueConverterForUnparse, isOptionalValue, name, astForChild)
        {
        }

        #endregion

        public new BnfiExpressionValueTL Rule { set { base.Rule = value; } }
    }

    public partial class BnfiTermValue<T> : BnfiTermValue, IBnfiTerm<T>, IBnfiTermOrAbleForChoice<T>
    {
        public BnfiTermValue(string name = null)
            : base(typeof(T), name)
        {
        }

        internal BnfiTermValue(BnfTerm bnfTerm, T value, bool isOptionalValue, string name, bool astForChild)
            : base(typeof(T), bnfTerm, value, isOptionalValue, name, astForChild)
        {
        }

        [Obsolete("Pass either a 'value', or a valueParser with an inverseValueConverterForUnparse", error: true)]
        internal BnfiTermValue(BnfTerm bnfTerm, ValueParser<T> valueParser, bool isOptionalValue, string name, bool astForChild)
            : this(bnfTerm, valueParser, NoUnparseByInverse<T>(), isOptionalValue, name, astForChild)
        {
        }

        internal BnfiTermValue(BnfTerm bnfTerm, ValueParser<T> valueParser, ValueConverter<T, object> inverseValueConverterForUnparse, bool isOptionalValue, string name, bool astForChild)
            : base(typeof(T), bnfTerm, (context, parseNode) => valueParser(context, parseNode), CastValueConverter<T, object, object, object>(inverseValueConverterForUnparse), isOptionalValue, name, astForChild)
        {
        }

        public new ValueUtokenizer<T> UtokenizerForUnparse
        {
            set
            {
                base.UtokenizerForUnparse = CastUtokenizerToObject(value);
            }
        }

        public BnfiExpressionValueTL RuleTypeless { set { base.Rule = value; } }

        public new BnfiExpressionValue<T> Rule { set { base.Rule = value; } }

        [Obsolete(BnfiTermNonTerminal.typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }
    }
}
