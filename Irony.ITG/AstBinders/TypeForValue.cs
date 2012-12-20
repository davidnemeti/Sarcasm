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
    public partial class TypeForValue : TypeForNonTerminal, IBnfTerm
    {
        protected TypeForValue(Type type, string errorAlias)
            : base(type, errorAlias)
        {
        }

        public static TypeForValue<TType> Of<TType>(string errorAlias = null)
        {
            return new TypeForValue<TType>(errorAlias);
        }

        public static TypeForValue Of(Type type, string errorAlias = null)
        {
            return new TypeForValue(type, errorAlias);
        }

        protected TypeForValue(Type type, BnfTerm bnfTerm, AstValueCreator<object> astValueCreator, bool isOptionalData, string errorAlias)
            : base(type, errorAlias)
        {
            this.AstConfig.NodeCreator = (context, parseTreeNode) =>
                parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(astValueCreator(context, new ParseTreeNodeWithOutAst(parseTreeNode)), context, parseTreeNode);

            this.RuleTL = isOptionalData
                ? bnfTerm | Irony.Parsing.Grammar.CurrentGrammar.Empty
                : new BnfExpression(bnfTerm);
        }

        public static TypeForValue Create(Type type, Terminal terminal, object value)
        {
            return new TypeForValue(type, terminal, (context, parseNode) => value, isOptionalData: false, errorAlias: null);
        }

        public static TypeForValue Create(Type type, Terminal terminal, AstValueCreator<object> astValueCreator)
        {
            return new TypeForValue(type, terminal, (context, parseNode) => astValueCreator(context, parseNode), isOptionalData: false, errorAlias: null);
        }

        public static TypeForValue<T> Create<T>(Terminal terminal, T value)
        {
            return new TypeForValue<T>(terminal, (context, parseNode) => value, isOptionalData: false, errorAlias: null);
        }

        public static TypeForValue<T> Create<T>(Terminal terminal, AstValueCreator<T> astValueCreator)
        {
            return new TypeForValue<T>(terminal, (context, parseNode) => astValueCreator(context, parseNode), isOptionalData: false, errorAlias: null);
        }

        public static TypeForValue Convert(Type type, BnfTerm bnfTerm, ValueConverter<object, object> valueConverter)
        {
            return new TypeForValue(
                type,
                bnfTerm,
                (context, parseTreeNode) =>
                    {
                        if (parseTreeNode.ChildNodes.Count != 1)
                            throw new ArgumentException("Only one child is allowed for a TypeForValue term: {0}", parseTreeNode.Term.Name);

                        return valueConverter(GrammarHelper.AstNodeToValue<object>(parseTreeNode.ChildNodes[0].AstNode));
                    },
                isOptionalData: false,
                errorAlias: null
                );
        }

        public static TypeForValue<TOut> Convert<TIn, TOut>(IBnfTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
        {
            return new TypeForValue<TOut>(
                bnfTerm.AsBnfTerm(),
                (context, parseTreeNode) =>
                    {
                        if (parseTreeNode.ChildNodes.Count != 1)
                            throw new ArgumentException("Only one child is allowed for a TypeForValue term: {0}", parseTreeNode.Term.Name);

                        return valueConverter(GrammarHelper.AstNodeToValue<TIn>(parseTreeNode.ChildNodes[0].AstNode));
                    },
                isOptionalData: false,
                errorAlias: null
                );
        }

        public static TypeForValue<TOut> Cast<TIn, TOut>(IBnfTerm<TIn> bnfTerm)
        {
            return Convert(bnfTerm, inValue => (TOut)(object)inValue);
        }

        public static TypeForValue<TOut> Cast<TOut>(Terminal terminal)
        {
            return Create<TOut>(terminal, (context, parseNode) => (TOut)GrammarHelper.AstNodeToValue<object>(parseNode.Token.Value));
        }

        public static TypeForValue<T?> ConvertValueOptVal<T>(IBnfTerm<T> bnfTerm)
            where T : struct
        {
            return ConvertValueOptVal(bnfTerm, value => value);
        }

        public static TypeForValue<TOut?> ConvertValueOptVal<TIn, TOut>(IBnfTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
            where TIn : struct
            where TOut : struct
        {
            return ConvertValueOpt<TIn, TOut?>(bnfTerm, value => valueConverter(value));
        }

        public static TypeForValue<T> ConvertValueOptRef<T>(IBnfTerm<T> bnfTerm)
            where T : class
        {
            return ConvertValueOptRef(bnfTerm, value => value);
        }

        public static TypeForValue<TOut> ConvertValueOptRef<TIn, TOut>(IBnfTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
            where TIn : class
            where TOut : class
        {
            return ConvertValueOpt<TIn, TOut>(bnfTerm, value => valueConverter(value));
        }

        private static TypeForValue<TOutData> ConvertValueOpt<TIn, TOutData>(IBnfTerm<TIn> bnfTerm, ValueConverter<TIn, TOutData> valueConverter)
        {
            return new TypeForValue<TOutData>(
                bnfTerm.AsBnfTerm(),
                (context, parseNode) =>
                {
                    TIn value = GrammarHelper.AstNodeToValue<TIn>(parseNode.ChildNodes.FirstOrDefault(parseTreeChild => parseTreeChild.Term == bnfTerm).AstNode);
                    return valueConverter(value);
                },
                isOptionalData: true,
                errorAlias: null
                );
        }

        protected BnfExpression RuleTL { get { return base.Rule; } set { base.Rule = value; } }

        public new TypeForValue Rule
        {
            get { return this; }
            set
            {
                // copy the TypeForValue object from 'value' to 'this'

                this.AstConfig.NodeCreator = value.AstConfig.NodeCreator;
                this.RuleTL = value.RuleTL;
            }
        }

        public BnfTerm AsBnfTerm()
        {
            return this;
        }
    }

    public partial class TypeForValue<T> : TypeForValue, IBnfTerm<T>
    {
        internal TypeForValue(string errorAlias)
            : base(typeof(T), errorAlias)
        {
        }

        internal TypeForValue(BnfTerm bnfTerm, AstValueCreator<object> astValueCreator, bool isOptionalData, string errorAlias)
            : base(typeof(T), bnfTerm, (context, parseNode) => astValueCreator(context, parseNode), isOptionalData, errorAlias)
        {
        }

        public new TypeForValue<T> Rule
        {
            get { return this; }
            set
            {
                // copy the TypeForValue<T> object from 'value' to 'this'
                base.Rule = value.Rule;
            }
        }

        [Obsolete(TypeForNonTerminal.typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }
    }
}
