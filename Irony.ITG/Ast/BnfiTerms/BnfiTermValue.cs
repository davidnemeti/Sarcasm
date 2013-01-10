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
using Irony.ITG.Unparsing;

namespace Irony.ITG.Ast
{
    public partial class BnfiTermValue : BnfiTermNonTerminal, IBnfiTerm, IUnparsable
    {
        private BnfTerm bnfTerm;
        private object value;

        public ValueConverter<object, object> InverseValueConverterForUnparse { private get; set; }
        public Func<IFormatProvider, object, IEnumerable<Utoken>> UtokenizerForUnparse { private get; set; }

        private readonly bool movable = true;

        public BnfiTermValue(string errorAlias = null)
            : this(typeof(object), errorAlias)
        {
            this.movable = false;
        }

        public BnfiTermValue(Type type, string errorAlias = null)
            : base(type, errorAlias)
        {
            this.movable = false;
            GrammarHelper.MarkTransientForced(this);    // default "transient" behavior (the Rule of this BnfiTermValue will contain the BnfiTermValue which actually does something)
            this.InverseValueConverterForUnparse = IdentityFunction;
        }

        protected BnfiTermValue(Type type, BnfTerm bnfTerm, object value, bool isOptionalValue, string errorAlias, bool astForChild)
            : this(type, bnfTerm, (context, parseNode) => value, isOptionalValue, errorAlias, astForChild)
        {
            this.value = value;
        }

        protected BnfiTermValue(Type type, BnfTerm bnfTerm, ValueCreator<object> valueCreator, bool isOptionalValue, string errorAlias, bool astForChild)
            : base(type, errorAlias)
        {
            this.bnfTerm = bnfTerm;

            if (!astForChild)
                bnfTerm.Flags |= TermFlags.NoAstNode;

            this.RuleRaw = isOptionalValue
                ? bnfTerm | Irony.Parsing.Grammar.CurrentGrammar.Empty
                : new BnfExpression(bnfTerm);

            this.AstConfig.NodeCreator = (context, parseTreeNode) =>
                parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(valueCreator(context, new ParseTreeNodeWithOutAst(parseTreeNode)), context, parseTreeNode);
        }

        public static BnfiTermValue Create(Terminal terminal, object value, bool astForChild = true)
        {
            return Create(typeof(object), terminal, value, astForChild);
        }

        public static BnfiTermValue Create(Type type, Terminal terminal, object value, bool astForChild = true)
        {
            BnfiTermValue bnfiTermValue = new BnfiTermValue(type, terminal, value, isOptionalValue: false, errorAlias: null, astForChild: astForChild);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunction;
            return bnfiTermValue;
        }

        public static BnfiTermValue Create(Terminal terminal, ValueCreator<object> valueCreator, bool astForChild = true)
        {
            return Create(typeof(object), terminal, valueCreator, astForChild);
        }

        public static BnfiTermValue Create(Type type, Terminal terminal, ValueCreator<object> valueCreator, bool astForChild = true)
        {
            return new BnfiTermValue(type, terminal, (context, parseNode) => valueCreator(context, parseNode), isOptionalValue: false, errorAlias: null, astForChild: astForChild);
        }

        public static BnfiTermValue<T> Create<T>(Terminal terminal, T value, bool astForChild = true)
        {
            BnfiTermValue<T> bnfiTermValue = new BnfiTermValue<T>(terminal, value, isOptionalValue: false, errorAlias: null, astForChild: astForChild);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<T, object>;
            return bnfiTermValue;
        }

        public static BnfiTermValue<T> Create<T>(Terminal terminal, ValueCreator<T> valueCreator, bool astForChild = true)
        {
            return new BnfiTermValue<T>(terminal, (context, parseNode) => valueCreator(context, parseNode), isOptionalValue: false, errorAlias: null, astForChild: astForChild);
        }

        public static BnfiTermValue<string> CreateIdentifier(IdentifierTerminal identifierTerminal)
        {
            BnfiTermValue<string> bnfiTermValue = Create<string>(identifierTerminal, (context, parseNode) => parseNode.FindTokenAndGetText(), astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<string, object>;
            return bnfiTermValue;
        }

        public static BnfiTermValue<T> CreateNumber<T>(NumberLiteral numberLiteral)
        {
            BnfiTermValue<T> bnfiTermValue = Create<T>(numberLiteral, (context, parseNode) => { return (T)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<T, object>;
            return bnfiTermValue;
        }

        public static BnfiTermValue CreateNumber(NumberLiteral numberLiteral)
        {
            return CreateNumber<object>(numberLiteral);
        }

        public static BnfiTermValue<DateTime> CreateDateTime(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.DateTime)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a DateTime", dataLiteral.Name);

            BnfiTermValue<DateTime> bnfiTermValue = Create<DateTime>(dataLiteral, (context, parseNode) => { return (DateTime)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<DateTime, object>;
            return bnfiTermValue;
        }

        public static BnfiTermValue Convert(IBnfiTerm bnfiTerm, ValueConverter<object, object> valueConverter)
        {
            return Convert(typeof(object), bnfiTerm, valueConverter);
        }

        public static BnfiTermValue Convert(Type type, IBnfiTerm bnfiTerm, ValueConverter<object, object> valueConverter)
        {
            return new BnfiTermValue(
                type,
                bnfiTerm.AsBnfTerm(),
                ConvertValueConverterToValueCreator(valueConverter),
                isOptionalValue: false,
                errorAlias: null,
                astForChild: true
                );
        }

        public static BnfiTermValue<TOut> Convert<TIn, TOut>(IBnfiTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
        {
            return new BnfiTermValue<TOut>(
                bnfTerm.AsBnfTerm(),
                ConvertValueConverterToValueCreator(valueConverter),
                isOptionalValue: false,
                errorAlias: null,
                astForChild: true
                );
        }

        public static BnfiTermValue<TOut> Convert<TOut>(IBnfiTerm bnfiTerm, ValueConverter<object, TOut> valueConverter)
        {
            return new BnfiTermValue<TOut>(
                bnfiTerm.AsBnfTerm(),
                ConvertValueConverterToValueCreator(valueConverter),
                isOptionalValue: false,
                errorAlias: null,
                astForChild: true
                );
        }

        public static BnfiTermValue<TOut> Cast<TIn, TOut>(IBnfiTerm<TIn> bnfTerm)
        {
            BnfiTermValue<TOut> bnfiTermValue = Convert(bnfTerm, value => IdentityFunctionForceCast<TIn, TOut>(value));
            bnfiTermValue.InverseValueConverterForUnparse = value => IdentityFunctionForceCast<TOut, TIn>(value);
            return bnfiTermValue;
        }

        public static BnfiTermValue<TOut> Cast<TOut>(Terminal terminal)
        {
            BnfiTermValue<TOut> bnfiTermValue = Create<TOut>(terminal, (context, parseNode) => (TOut)GrammarHelper.AstNodeToValue(parseNode.Token.Value));
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<TOut, object>;
            return bnfiTermValue;
        }

        public static BnfiTermValue<T?> ConvertValueOptVal<T>(IBnfiTerm<T> bnfTerm)
            where T : struct
        {
            return ConvertValueOptVal(bnfTerm, value => value);
        }

        public static BnfiTermValue<TOut?> ConvertValueOptVal<TIn, TOut>(IBnfiTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
            where TIn : struct
            where TOut : struct
        {
            return ConvertValueOpt<TIn, TOut?>(bnfTerm, value => valueConverter(value));
        }

        public static BnfiTermValue<T> ConvertValueOptRef<T>(IBnfiTerm<T> bnfTerm)
            where T : class
        {
            return ConvertValueOptRef(bnfTerm, value => value);
        }

        public static BnfiTermValue<TOut> ConvertValueOptRef<TIn, TOut>(IBnfiTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
            where TIn : class
            where TOut : class
        {
            return ConvertValueOpt<TIn, TOut>(bnfTerm, valueConverter);
        }

        public static BnfiTermValue<T> ConvertValueOptVal<T>(IBnfiTerm<T> bnfiTerm, T defaultValue)
            where T : struct
        {
            return ConvertValueOptVal(bnfiTerm, value => value, defaultValue);
        }

        public static BnfiTermValue<TOut> ConvertValueOptVal<TIn, TOut>(IBnfiTerm<TIn> bnfiTerm, ValueConverter<TIn, TOut> valueConverter, TOut defaultValue)
            where TIn : struct
            where TOut : struct
        {
            return ConvertValueOpt<TIn, TOut>(bnfiTerm, valueConverter, defaultValue);
        }

        public static BnfiTermValue<T> ConvertValueOptRef<T>(IBnfiTerm<T> bnfiTerm, T defaultValue)
            where T : class
        {
            return ConvertValueOptRef(bnfiTerm, value => value, defaultValue);
        }

        public static BnfiTermValue<TOut> ConvertValueOptRef<TIn, TOut>(IBnfiTerm<TIn> bnfiTerm, ValueConverter<TIn, TOut> valueConverter, TOut defaultValue)
            where TIn : class
            where TOut : class
        {
            return ConvertValueOptRef(bnfiTerm, valueConverter, defaultValue);
        }

        protected static BnfiTermValue<TOut> ConvertValueOpt<TIn, TOut>(IBnfiTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter, TOut defaultValue = default(TOut))
        {
            return new BnfiTermValue<TOut>(
                bnfTerm.AsBnfTerm(),
                ConvertValueConverterToValueCreatorOpt(valueConverter, defaultValue),
                isOptionalValue: true,
                errorAlias: null,
                astForChild: true
                );
        }

        protected static ValueCreator<TOut> ConvertValueConverterToValueCreator<TIn, TOut>(ValueConverter<TIn, TOut> valueConverter)
        {
            return ConvertValueConverterToValueCreator(valueConverter, isOptionalValue: false, defaultValue: default(TOut));   // defaultValue won't be used
        }

        protected static ValueCreator<TOut> ConvertValueConverterToValueCreatorOpt<TIn, TOut>(ValueConverter<TIn, TOut> valueConverter, TOut defaultValue)
        {
            return ConvertValueConverterToValueCreator(valueConverter, isOptionalValue: true, defaultValue: defaultValue);
        }

        private static ValueCreator<TOut> ConvertValueConverterToValueCreator<TIn, TOut>(ValueConverter<TIn, TOut> valueConverter, bool isOptionalValue, TOut defaultValue)
        {
            return (context, parseTreeNode) =>
            {
                if (!isOptionalValue && parseTreeNode.ChildNodes.Count != 1)
                    throw new ArgumentException("Only one child is allowed for a non-optional BnfiTermValue term: {0}", parseTreeNode.Term.Name);
                else if (isOptionalValue && parseTreeNode.ChildNodes.Count > 1)
                    throw new ArgumentException("Only zero or one child is allowed for an optional BnfiTermValue term: {0}", parseTreeNode.Term.Name);

                ParseTreeNode parseTreeChild = parseTreeNode.ChildNodes.SingleOrDefault();

                return parseTreeChild != null
                    ? valueConverter(GrammarHelper.AstNodeToValue<TIn>(parseTreeChild.AstNode))
                    : defaultValue;
            };
        }

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
            this.InverseValueConverterForUnparse = source.InverseValueConverterForUnparse;
            this.UtokenizerForUnparse = source.UtokenizerForUnparse;
        }

        protected void ClearState()
        {
            this.bnfTerm = null;
            this.value = null;
            this.Flags = TermFlags.None;
            this.AstConfig = null;
            this.InverseValueConverterForUnparse = null;
            this.UtokenizerForUnparse = null;
        }

        protected void MoveTo(BnfiTermValue target)
        {
            if (!this.movable)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "This value should not be a right-value: {0}", this.Name);

            target.RuleRaw = this.RuleRaw;
            target.SetState(this);

            this.RuleRaw = null;
            this.ClearState();
        }

        protected BnfExpression RuleRaw { get { return base.Rule; } set { base.Rule = value; } }

        public new BnfiExpressionValue Rule
        {
            set
            {
                try
                {
                    /*
                     * Examine whether there is only one single BnfiTermValue inside 'value', and if it is true, then move the state of that BnfiTermValue
                     * so we can "destroy" that BnfiTermValue in order to eliminate unneccessary, extra "rule-embedding"
                     * 
                     * (note that if there is a single bnfTerm inside 'value' then it must be a BnfiTermValue (enforced by BnfiExpressionValue's typesafety),
                     * that's why we can cast here)
                     * */
                    BnfiTermValue onlyBnfTermInValue = (BnfiTermValue)value.GetBnfTermsList().Single().Single();
                    onlyBnfTermInValue.MoveTo(this);
                }
                catch (InvalidOperationException)
                {
                    // there are more than one bnfTerms inside 'value' -> "rule-embedding" is necessary
                    base.Rule = value;
                }
            }
        }

        BnfTerm IBnfiTerm.AsBnfTerm()
        {
            return this;
        }

        public IEnumerable<Utoken> Unparse(IUnparser unparser, object obj)
        {
            if (this.value != null && !this.value.Equals(obj))
                throw new ValueMismatchException(string.Format("BnfiTermValue has value '{0}' which is not equal to the to-be-unparsed object '{1}'", this.value, obj));

            if (this.UtokenizerForUnparse != null)
            {
                return this.UtokenizerForUnparse(unparser.FormatProvider, obj);
            }
            else if (this.InverseValueConverterForUnparse != null)
            {
                BnfTerm childBnfTerm = this.bnfTerm ?? Unparser.GetChildBnfTermLists(this).First().Single(bnfTerm => bnfTerm is BnfiTermValue);
                object childObj = this.InverseValueConverterForUnparse(obj);

                return unparser.Unparse(childObj, childBnfTerm);
            }
            else if (this.bnfTerm == null)
            {
                // "transient" unparse with the actual BnfiTermValue(s) under the current one (set by Rule)

                BnfTermListToPriority bnfTermListToPriority = (BnfTermList bnfTerms, out ICollection<Utoken> preYieldedUtokens) =>
                {
                    try
                    {
                        // we need to do the full unparse non-lazy in order to catch ValueMismatchException (that's why we use ToList here)
                        preYieldedUtokens = bnfTerms.SelectMany(bnfTerm => unparser.Unparse(obj, bnfTerm)).ToList();
                        return int.MaxValue;
                    }
                    catch (ValueMismatchException)
                    {
                        preYieldedUtokens = null;
                        return null;
                    }
                };

                return Unparser.UnparseBestChildBnfTermList(this, unparser, obj, bnfTermListToPriority);
            }
            else
                throw new CannotUnparseException(string.Format("'{0}' has no UtokenizerForUnparse, no ValueConverterForUnparse, and no other BnfiTermValue under", this.Name));
        }
    }

    public partial class BnfiTermValue<T> : BnfiTermValue, IBnfiTerm<T>
    {
        public BnfiTermValue(string errorAlias = null)
            : base(typeof(T), errorAlias)
        {
            this.InverseValueConverterForUnparse = IdentityFunctionForceCast<T, object>;
        }

        internal BnfiTermValue(BnfTerm bnfTerm, T value, bool isOptionalValue, string errorAlias, bool astForChild)
            : base(typeof(T), bnfTerm, value, isOptionalValue, errorAlias, astForChild)
        {
        }

        internal BnfiTermValue(BnfTerm bnfTerm, ValueCreator<T> valueCreator, bool isOptionalValue, string errorAlias, bool astForChild)
            : base(typeof(T), bnfTerm, (context, parseNode) => valueCreator(context, parseNode), isOptionalValue, errorAlias, astForChild)
        {
        }

        public new ValueConverter<T, object> InverseValueConverterForUnparse
        {
            set
            {
                base.InverseValueConverterForUnparse = obj => value((T)obj);
            }
        }

        public new Func<IFormatProvider, T, IEnumerable<Utoken>> UtokenizerForUnparse
        {
            set
            {
                base.UtokenizerForUnparse = (formatProvider, obj) => value(formatProvider, (T)obj);
            }
        }

        public BnfiExpressionValue RuleTypeless { set { base.Rule = value; } }

        public new BnfiExpressionValue<T> Rule { set { base.Rule = value; } }

        [Obsolete(BnfiTermNonTerminal.typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }
    }
}
