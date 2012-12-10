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

        protected ValueForBnfTerm(BnfTerm bnfTerm, AstObjectCreator<object> astObjectCreator, bool isOptionalData)
            : base(bnfTerm.Name)
        {
            this.bnfTerm = bnfTerm;
            this.AstConfig.NodeCreator = (AstContext context, ParseTreeNode parseNode) => parseNode.AstNode = astObjectCreator(context, new ParseTreeNodeWithOutAst(parseNode));
            this.Rule = isOptionalData
                ? bnfTerm | Grammar.CurrentGrammar.Empty
                : new BnfExpression(bnfTerm);
        }

        public static ValueForBnfTerm<TOut> Create<TOut>(BnfTerm bnfTerm, AstObjectCreator<TOut> astObjectCreator)
        {
            return new ValueForBnfTerm<TOut>(bnfTerm, (context, parseNode) => astObjectCreator(context, parseNode), isOptionalData: false);
        }

        public static ValueForBnfTerm<TOut> Create<TIn, TOut>(IBnfTerm<TIn> bnfTerm, AstObjectConverter<TIn, TOut> astObjectConverter)
        {
            return new ValueForBnfTerm<TOut>(
                bnfTerm.AsTypeless(),
                (context, parseNode) => astObjectConverter((TIn)parseNode.ChildNodes.First(parseTreeChild => parseTreeChild.Term == bnfTerm).AstNode),
                isOptionalData: false
                );
        }

        public static ValueForBnfTerm<T?> SetValueOptVal<T>(IBnfTerm<T> bnfTerm)
            where T : struct
        {
            return SetValueOptVal(bnfTerm, value => value);
        }

        public static ValueForBnfTerm<TOut?> SetValueOptVal<TIn, TOut>(IBnfTerm<TIn> bnfTerm, AstObjectConverter<TIn, TOut> astObjectConverter)
            where TIn : struct
            where TOut : struct
        {
            return SetValueOpt<TIn, TOut?>(bnfTerm, value => astObjectConverter(value));
        }

        public static ValueForBnfTerm<T> SetValueOptRef<T>(IBnfTerm<T> bnfTerm)
            where T : class
        {
            return SetValueOptRef(bnfTerm, value => value);
        }

        public static ValueForBnfTerm<TOut> SetValueOptRef<TIn, TOut>(IBnfTerm<TIn> bnfTerm, AstObjectConverter<TIn, TOut> astObjectConverter)
            where TIn : class
            where TOut : class
        {
            return SetValueOpt<TIn, TOut>(bnfTerm, value => astObjectConverter(value));
        }

        private static ValueForBnfTerm<TOutData> SetValueOpt<TIn, TOutData>(IBnfTerm<TIn> bnfTerm, AstObjectConverter<TIn, TOutData> astObjectConverter)
        {
            return new ValueForBnfTerm<TOutData>(
                bnfTerm.AsTypeless(),
                (context, parseNode) =>
                {
                    object astNode = parseNode.ChildNodes.FirstOrDefault(parseTreeChild => parseTreeChild.Term == bnfTerm).AstNode;
                    return astObjectConverter((TIn)astNode);
                },
                isOptionalData: true
                );
        }

        public static ValueForBnfTerm<TOut> Create<TOut>(BnfTerm bnfTerm, TOut astObject)
        {
            return new ValueForBnfTerm<TOut>(bnfTerm, (context, parseNode) => astObject, isOptionalData: false);
        }
    }

    public class ValueForBnfTerm<T> : ValueForBnfTerm, IBnfTerm<T>
    {
        internal ValueForBnfTerm(BnfTerm bnfTerm, AstObjectCreator<T> astObjectCreator, bool isOptionalData)
            : base(bnfTerm, (context, parseNode) => astObjectCreator(context, parseNode), isOptionalData)
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
