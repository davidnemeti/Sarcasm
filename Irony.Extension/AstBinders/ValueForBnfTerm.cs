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

namespace Irony.Extension.AstBinders
{
    public class ValueForBnfTerm : NonTerminal
    {
        private readonly BnfTerm bnfTerm;

        protected ValueForBnfTerm(BnfTerm bnfTerm, AstValueCreator<object> astValueCreator, bool isOptionalData)
            : base(bnfTerm.Name)
        {
            this.bnfTerm = bnfTerm;
            this.AstConfig.NodeCreator = (AstContext context, ParseTreeNode parseTreeNode) =>
                parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(astValueCreator(context, new ParseTreeNodeWithOutAst(parseTreeNode)), context, parseTreeNode);
            this.Rule = isOptionalData
                ? bnfTerm | Grammar.CurrentGrammar.Empty
                : new BnfExpression(bnfTerm);
        }

        public static ValueForBnfTerm<TOut> Create<TOut>(BnfTerm bnfTerm, TOut value)
        {
            return new ValueForBnfTerm<TOut>(bnfTerm, (context, parseNode) => value, isOptionalData: false);
        }

        public static ValueForBnfTerm<TOut> Create<TOut>(BnfTerm bnfTerm, AstValueCreator<TOut> astValueCreator)
        {
            return new ValueForBnfTerm<TOut>(bnfTerm, (context, parseNode) => astValueCreator(context, parseNode), isOptionalData: false);
        }

        public static ValueForBnfTerm<TOut> Convert<TIn, TOut>(IBnfTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
        {
            return new ValueForBnfTerm<TOut>(
                bnfTerm.AsTypeless(),
                (context, parseNode) => valueConverter(GrammarHelper.AstNodeToValue<TIn>(parseNode.ChildNodes.First(parseTreeChild => parseTreeChild.Term == bnfTerm).AstNode)),
                isOptionalData: false
                );
        }

        public static ValueForBnfTerm<T?> ConvertValueOptVal<T>(IBnfTerm<T> bnfTerm)
            where T : struct
        {
            return ConvertValueOptVal(bnfTerm, value => value);
        }

        public static ValueForBnfTerm<TOut?> ConvertValueOptVal<TIn, TOut>(IBnfTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
            where TIn : struct
            where TOut : struct
        {
            return ConvertValueOpt<TIn, TOut?>(bnfTerm, value => valueConverter(value));
        }

        public static ValueForBnfTerm<T> ConvertValueOptRef<T>(IBnfTerm<T> bnfTerm)
            where T : class
        {
            return ConvertValueOptRef(bnfTerm, value => value);
        }

        public static ValueForBnfTerm<TOut> ConvertValueOptRef<TIn, TOut>(IBnfTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
            where TIn : class
            where TOut : class
        {
            return ConvertValueOpt<TIn, TOut>(bnfTerm, value => valueConverter(value));
        }

        private static ValueForBnfTerm<TOutData> ConvertValueOpt<TIn, TOutData>(IBnfTerm<TIn> bnfTerm, ValueConverter<TIn, TOutData> valueConverter)
        {
            return new ValueForBnfTerm<TOutData>(
                bnfTerm.AsTypeless(),
                (context, parseNode) =>
                {
                    TIn value = GrammarHelper.AstNodeToValue<TIn>(parseNode.ChildNodes.FirstOrDefault(parseTreeChild => parseTreeChild.Term == bnfTerm).AstNode);
                    return valueConverter(value);
                },
                isOptionalData: true
                );
        }
    }

    public class ValueForBnfTerm<T> : ValueForBnfTerm, IBnfTerm<T>
    {
        internal ValueForBnfTerm(BnfTerm bnfTerm, AstValueCreator<T> astValueCreator, bool isOptionalData)
            : base(bnfTerm, (context, parseNode) => astValueCreator(context, parseNode), isOptionalData)
        {
        }

        BnfTerm IBnfTerm<T>.AsTypeless()
        {
            return this;
        }

        [Obsolete(TypeForNonTerminal.typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }
    }
}
