using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using System.IO;

namespace Irony.ITG
{
    public partial class BnfiTermValue : BnfiTermNonTerminal, IBnfiTerm
    {
        public BnfiTermValue(Type type, string errorAlias = null)
            : base(type, errorAlias)
        {
            GrammarHelper.MarkTransientForced(this);    // default "transient" behavior (the Rule of this BnfiTermValue will contain the BnfiTermValue which actually does something)
        }

        protected BnfiTermValue(Type type, BnfTerm bnfTerm, AstValueCreator<object> astValueCreator, bool isOptionalData, string errorAlias, bool astForChild)
            : base(type, errorAlias)
        {
            if (!astForChild)
                bnfTerm.Flags |= TermFlags.NoAstNode;

            this.RuleTL = isOptionalData
                ? bnfTerm | Irony.Parsing.Grammar.CurrentGrammar.Empty
                : new BnfExpression(bnfTerm);

            this.AstConfig.NodeCreator = (context, parseTreeNode) =>
                parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(astValueCreator(context, new ParseTreeNodeWithOutAst(parseTreeNode)), context, parseTreeNode);
        }

        public static BnfiTermValue Create(Type type, Terminal terminal, object value, bool astForChild = true)
        {
            return new BnfiTermValue(type, terminal, (context, parseNode) => value, isOptionalData: false, errorAlias: null, astForChild: astForChild);
        }

        public static BnfiTermValue Create(Type type, Terminal terminal, AstValueCreator<object> astValueCreator, bool astForChild = true)
        {
            return new BnfiTermValue(type, terminal, (context, parseNode) => astValueCreator(context, parseNode), isOptionalData: false, errorAlias: null, astForChild: astForChild);
        }

        public static BnfiTermValue<T> Create<T>(Terminal terminal, T value, bool astForChild = true)
        {
            return new BnfiTermValue<T>(terminal, (context, parseNode) => value, isOptionalData: false, errorAlias: null, astForChild: astForChild);
        }

        public static BnfiTermValue<T> Create<T>(Terminal terminal, AstValueCreator<T> astValueCreator, bool astForChild = true)
        {
            return new BnfiTermValue<T>(terminal, (context, parseNode) => astValueCreator(context, parseNode), isOptionalData: false, errorAlias: null, astForChild: astForChild);
        }

        public static BnfiTermValue<string> CreateIdentifier(IdentifierTerminal identifierTerminal)
        {
            return Create<string>(identifierTerminal, (context, parseNode) => parseNode.FindTokenAndGetText(), astForChild: false);
        }

        public static BnfiTermValue<T> CreateNumber<T>(NumberLiteral numberLiteral)
        {
            return Create<T>(numberLiteral, (context, parseNode) => { return (T)parseNode.FindToken().Value; }, astForChild: false);
        }

        public static BnfiTermValue CreateNumber(NumberLiteral numberLiteral)
        {
            return Create(numberLiteral, (context, parseNode) => { return parseNode.FindToken().Value; }, astForChild: false);
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
                (context, parseTreeNode) =>
                    {
                        if (parseTreeNode.ChildNodes.Count != 1)
                            throw new ArgumentException("Only one child is allowed for a BnfiTermValue term: {0}", parseTreeNode.Term.Name);

                        return valueConverter(GrammarHelper.AstNodeToValue(parseTreeNode.ChildNodes[0].AstNode));
                    },
                isOptionalData: false,
                errorAlias: null,
                astForChild: true
                );
        }

        public static BnfiTermValue<TOut> Convert<TIn, TOut>(IBnfiTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
        {
            return new BnfiTermValue<TOut>(
                bnfTerm.AsBnfTerm(),
                (context, parseTreeNode) =>
                    {
                        if (parseTreeNode.ChildNodes.Count != 1)
                            throw new ArgumentException("Only one child is allowed for a BnfiTermValue term: {0}", parseTreeNode.Term.Name);

                        return valueConverter(GrammarHelper.AstNodeToValue<TIn>(parseTreeNode.ChildNodes[0].AstNode));
                    },
                isOptionalData: false,
                errorAlias: null,
                astForChild: true
                );
        }

        public static BnfiTermValue<TOut> Convert<TOut>(IBnfiTerm bnfiTerm, ValueConverter<object, TOut> valueConverter)
        {
            return new BnfiTermValue<TOut>(
                bnfiTerm.AsBnfTerm(),
                (context, parseTreeNode) =>
                {
                    if (parseTreeNode.ChildNodes.Count != 1)
                        throw new ArgumentException("Only one child is allowed for a BnfiTermValue term: {0}", parseTreeNode.Term.Name);

                    return valueConverter(GrammarHelper.AstNodeToValue<object>(parseTreeNode.ChildNodes[0].AstNode));
                },
                isOptionalData: false,
                errorAlias: null,
                astForChild: true
                );
        }

        public static BnfiTermValue<TOut> Cast<TIn, TOut>(IBnfiTerm<TIn> bnfTerm)
        {
            return Convert(bnfTerm, inValue => (TOut)(object)inValue);
        }

        public static BnfiTermValue<TOut> Cast<TOut>(Terminal terminal)
        {
            return Create<TOut>(terminal, (context, parseNode) => (TOut)GrammarHelper.AstNodeToValue(parseNode.Token.Value));
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
            return ConvertValueOpt<TIn, TOut>(bnfTerm, value => valueConverter(value));
        }

        private static BnfiTermValue<TOut> ConvertValueOpt<TIn, TOut>(IBnfiTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
        {
            return new BnfiTermValue<TOut>(
                bnfTerm.AsBnfTerm(),
                (context, parseNode) =>
                {
                    ParseTreeNode parseTreeChild = parseNode.ChildNodes.FirstOrDefault();
                    return parseTreeChild != null
                        ? valueConverter(GrammarHelper.AstNodeToValue<TIn>(parseTreeChild.AstNode))
                        : default(TOut);
                },
                isOptionalData: true,
                errorAlias: null,
                astForChild: true
                );
        }

        protected BnfExpression RuleTL { get { return base.Rule; } set { base.Rule = value; } }

        public new BnfiExpressionValue Rule { set { base.Rule = value; } }

        public BnfTerm AsBnfTerm()
        {
            return this;
        }
    }

    public partial class BnfiTermValue<T> : BnfiTermValue, IBnfiTerm<T>
    {
        public BnfiTermValue(string errorAlias = null)
            : base(typeof(T), errorAlias)
        {
        }

        internal BnfiTermValue(BnfTerm bnfTerm, AstValueCreator<T> astValueCreator, bool isOptionalData, string errorAlias, bool astForChild)
            : base(typeof(T), bnfTerm, (context, parseNode) => astValueCreator(context, parseNode), isOptionalData, errorAlias, astForChild)
        {
        }

        public new BnfiExpressionValue<T> Rule { set { base.Rule = value; } }

        [Obsolete(BnfiTermNonTerminal.typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }
    }
}
