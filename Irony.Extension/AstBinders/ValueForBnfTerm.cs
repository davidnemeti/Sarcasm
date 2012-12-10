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
                parseTreeNode.AstNode = AstNodeWrapper.ValueToAstNode(astValueCreator(context, new ParseTreeNodeWithOutAst(parseTreeNode)), context, parseTreeNode);
            this.Rule = isOptionalData
                ? bnfTerm | Grammar.CurrentGrammar.Empty
                : new BnfExpression(bnfTerm);
        }

        public static ValueForBnfTerm<TOut> Create<TOut>(BnfTerm bnfTerm, AstValueCreator<TOut> astValueCreator)
        {
            return new ValueForBnfTerm<TOut>(bnfTerm, (context, parseNode) => astValueCreator(context, parseNode), isOptionalData: false);
        }

        public static ValueForBnfTerm<TOut> Create<TIn, TOut>(IBnfTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
        {
            return new ValueForBnfTerm<TOut>(
                bnfTerm.AsTypeless(),
                (context, parseNode) => valueConverter(AstNodeWrapper.AstNodeToValue<TIn>(parseNode.ChildNodes.First(parseTreeChild => parseTreeChild.Term == bnfTerm).AstNode)),
                isOptionalData: false
                );
        }

        public static ValueForBnfTerm<T?> SetValueOptVal<T>(IBnfTerm<T> bnfTerm)
            where T : struct
        {
            return SetValueOptVal(bnfTerm, value => value);
        }

        public static ValueForBnfTerm<TOut?> SetValueOptVal<TIn, TOut>(IBnfTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
            where TIn : struct
            where TOut : struct
        {
            return SetValueOpt<TIn, TOut?>(bnfTerm, value => valueConverter(value));
        }

        public static ValueForBnfTerm<T> SetValueOptRef<T>(IBnfTerm<T> bnfTerm)
            where T : class
        {
            return SetValueOptRef(bnfTerm, value => value);
        }

        public static ValueForBnfTerm<TOut> SetValueOptRef<TIn, TOut>(IBnfTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
            where TIn : class
            where TOut : class
        {
            return SetValueOpt<TIn, TOut>(bnfTerm, value => valueConverter(value));
        }

        private static ValueForBnfTerm<TOutData> SetValueOpt<TIn, TOutData>(IBnfTerm<TIn> bnfTerm, ValueConverter<TIn, TOutData> valueConverter)
        {
            return new ValueForBnfTerm<TOutData>(
                bnfTerm.AsTypeless(),
                (context, parseNode) =>
                {
                    TIn value = AstNodeWrapper.AstNodeToValue<TIn>(parseNode.ChildNodes.FirstOrDefault(parseTreeChild => parseTreeChild.Term == bnfTerm).AstNode);
                    return valueConverter(value);
                },
                isOptionalData: true
                );
        }

        public static ValueForBnfTerm<TOut> Create<TOut>(BnfTerm bnfTerm, TOut value)
        {
            return new ValueForBnfTerm<TOut>(bnfTerm, (context, parseNode) => value, isOptionalData: false);
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

        [Obsolete(TypeForNonTerminal.obsoleteQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }
    }
}
