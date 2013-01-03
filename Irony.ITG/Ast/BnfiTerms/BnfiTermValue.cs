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
        private readonly BnfTerm bnfTerm;

        private ValueConverter<object, object> _inverseValueConverterForUnparse;
        public ValueConverter<object, object> InverseValueConverterForUnparse
        {
            private get { return _inverseValueConverterForUnparse; }
            set
            {
                if (value != null && UtokenizerForUnparse != null)
                    throw new InvalidOperationException("Cannot set ValueConverterForUnparse because UtokenizerForUnparse has been already set.");

                _inverseValueConverterForUnparse = value;
            }
        }

        private Func<object, IEnumerable<Utoken>> _utokenizerForUnparse;
        public Func<object, IEnumerable<Utoken>> UtokenizerForUnparse
        {
            private get { return _utokenizerForUnparse; }
            set
            {
                if (value != null && InverseValueConverterForUnparse != null)
                    throw new InvalidOperationException("Cannot set UtokenizerForUnparse because ValueConverterForUnparse has been already set.");

                _utokenizerForUnparse = value;
            }
        }

        public BnfiTermValue(string errorAlias = null)
            : this(typeof(object), errorAlias)
        {
        }

        public BnfiTermValue(Type type, string errorAlias = null)
            : base(type, errorAlias)
        {
            GrammarHelper.MarkTransientForced(this);    // default "transient" behavior (the Rule of this BnfiTermValue will contain the BnfiTermValue which actually does something)
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
            BnfiTermValue bnfiTermValue = new BnfiTermValue(type, terminal, (context, parseNode) => value, isOptionalValue: false, errorAlias: null, astForChild: astForChild);
            bnfiTermValue.InverseValueConverterForUnparse = GetIdentityValueConverter();
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
            BnfiTermValue<T> bnfiTermValue = new BnfiTermValue<T>(terminal, (context, parseNode) => value, isOptionalValue: false, errorAlias: null, astForChild: astForChild);
            bnfiTermValue.InverseValueConverterForUnparse = GetIdentityValueConverter();
            return bnfiTermValue;
        }

        public static BnfiTermValue<T> Create<T>(Terminal terminal, ValueCreator<T> valueCreator, bool astForChild = true)
        {
            return new BnfiTermValue<T>(terminal, (context, parseNode) => valueCreator(context, parseNode), isOptionalValue: false, errorAlias: null, astForChild: astForChild);
        }

        public static BnfiTermValue<string> CreateIdentifier(IdentifierTerminal identifierTerminal)
        {
            BnfiTermValue<string> bnfiTermValue = Create<string>(identifierTerminal, (context, parseNode) => parseNode.FindTokenAndGetText(), astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = GetIdentityValueConverter();
            return bnfiTermValue;
        }

        public static BnfiTermValue<T> CreateNumber<T>(NumberLiteral numberLiteral)
        {
            BnfiTermValue<T> bnfiTermValue = Create<T>(numberLiteral, (context, parseNode) => { return (T)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = GetIdentityValueConverter();
            return bnfiTermValue;
        }

        public static BnfiTermValue CreateNumber(NumberLiteral numberLiteral)
        {
            return CreateNumber<object>(numberLiteral);
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
            BnfiTermValue<TOut> bnfiTermValue = Convert(bnfTerm, inValue => (TOut)(object)inValue);
            bnfiTermValue.InverseValueConverterForUnparse = GetIdentityValueConverter();
            return bnfiTermValue;
        }

        public static BnfiTermValue<TOut> Cast<TOut>(Terminal terminal)
        {
            BnfiTermValue<TOut> bnfiTermValue = Create<TOut>(terminal, (context, parseNode) => (TOut)GrammarHelper.AstNodeToValue(parseNode.Token.Value));
            bnfiTermValue.InverseValueConverterForUnparse = GetIdentityValueConverter();
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

        protected static ValueConverter<object, object> GetIdentityValueConverter()
        {
            return obj => obj;
        }

        protected BnfExpression RuleRaw { get { return base.Rule; } set { base.Rule = value; } }

        public new BnfiExpressionValue Rule { set { base.Rule = value; } }

        BnfTerm IBnfiTerm.AsBnfTerm()
        {
            return this;
        }

        public IEnumerable<Utoken> Unparse(Unparser unparser, object obj)
        {
            IEnumerable<Utoken> utokens;

            if (this.UtokenizerForUnparse != null)
            {
                utokens = this.UtokenizerForUnparse(obj);
            }
            else if (this.InverseValueConverterForUnparse != null)
            {
                BnfTerm childBnfTerm = this.bnfTerm ?? Unparser.GetChildBnfTermLists(this).First().Single(bnfTerm => bnfTerm is BnfiTermValue);
                object childObj = this.InverseValueConverterForUnparse(obj);

                utokens = unparser.Unparse(childObj, childBnfTerm);
            }
            else
                throw new CannotUnparseException(string.Format("BnfiTermValue '{0}' has neither UtokenizerForUnparse nor ValueConverterForUnparse set.", this.Name));

            foreach (Utoken utoken in utokens)
                yield return utoken;
        }
    }

    public partial class BnfiTermValue<T> : BnfiTermValue, IBnfiTerm<T>
    {
        public BnfiTermValue(string errorAlias = null)
            : base(typeof(T), errorAlias)
        {
        }

        internal BnfiTermValue(BnfTerm bnfTerm, ValueCreator<T> valueCreator, bool isOptionalValue, string errorAlias, bool astForChild)
            : base(typeof(T), bnfTerm, (context, parseNode) => valueCreator(context, parseNode), isOptionalValue, errorAlias, astForChild)
        {
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
