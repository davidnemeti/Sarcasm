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

namespace Irony.AstBinders
{
    public class DataForBnfTerm : NonTerminal
    {
        private readonly BnfTerm bnfTerm;

        protected DataForBnfTerm(BnfTerm bnfTerm, AstNodeCreator nodeCreator, bool isOptionalData)
            : base(bnfTerm.Name)
        {
            this.bnfTerm = bnfTerm;
            this.AstConfig.NodeCreator = nodeCreator;
            this.Rule = isOptionalData
                ? bnfTerm | Grammar.CurrentGrammar.Empty
                : new BnfExpression(bnfTerm);
        }

        public static DataForBnfTerm SetValue(BnfTerm bnfTerm, AstNodeCreator nodeCreator)
        {
            return new DataForBnfTerm(bnfTerm, nodeCreator, isOptionalData: false);
        }

        public static DataForBnfTerm<TOut> SetValue<TOut>(BnfTerm bnfTerm, AstObjectCreator<TOut> astObjectCreator)
        {
            return new DataForBnfTerm<TOut>(bnfTerm, (context, parseNode) => parseNode.AstNode = astObjectCreator(context, parseNode), isOptionalData: false);
        }

        public static DataForBnfTerm<TOut> SetValue<TIn, TOut>(IBnfTerm<TIn> bnfTerm, AstObjectConverter<TIn, TOut> astObjectCreator)
        {
            return new DataForBnfTerm<TOut>(
                bnfTerm.AsTypeless(),
                (context, parseNode) => parseNode.AstNode = astObjectCreator((TIn)parseNode.ChildNodes.First(parseTreeChild => parseTreeChild.Term == bnfTerm).AstNode),
                isOptionalData: false
                );
        }

        public static DataForBnfTerm<T?> SetValueOptVal<T>(IBnfTerm<T> bnfTerm)
            where T : struct
        {
            return SetValueOptVal(bnfTerm, value => value);
        }

        public static DataForBnfTerm<TOut?> SetValueOptVal<TIn, TOut>(IBnfTerm<TIn> bnfTerm, AstObjectConverter<TIn, TOut> astObjectCreator)
            where TIn : struct
            where TOut : struct
        {
            return SetValueOpt<TIn, TOut, TOut?>(bnfTerm, value => astObjectCreator(value));
        }

        public static DataForBnfTerm<T> SetValueOptRef<T>(IBnfTerm<T> bnfTerm)
            where T : class
        {
            return SetValueOptRef(bnfTerm, value => value);
        }

        public static DataForBnfTerm<TOut> SetValueOptRef<TIn, TOut>(IBnfTerm<TIn> bnfTerm, AstObjectConverter<TIn, TOut> astObjectCreator)
            where TIn : class
            where TOut : class
        {
            return SetValueOpt<TIn, TOut, TOut>(bnfTerm, value => astObjectCreator(value));
        }

        private static DataForBnfTerm<TOutData> SetValueOpt<TIn, TOutAst, TOutData>(IBnfTerm<TIn> bnfTerm, AstObjectConverter<TIn, TOutAst> astObjectCreator)
        {
            return new DataForBnfTerm<TOutData>(
                bnfTerm.AsTypeless(),
                (context, parseNode) =>
                {
                    object astNode = parseNode.ChildNodes.FirstOrDefault(parseTreeChild => parseTreeChild.Term == bnfTerm).AstNode;
                    parseNode.AstNode = astNode != null
                        ? (object)astObjectCreator((TIn)astNode)
                        : null;
                },
                isOptionalData: true
                );
        }

        public static DataForBnfTerm<TOut> SetValue<TOut>(BnfTerm bnfTerm, TOut astObject)
        {
            return new DataForBnfTerm<TOut>(bnfTerm, (context, parseNode) => parseNode.AstNode = astObject, isOptionalData: false);
        }
    }

    public class DataForBnfTerm<T> : DataForBnfTerm, IBnfTerm<T>
    {
        internal DataForBnfTerm(BnfTerm bnfTerm, AstNodeCreator nodeCreator, bool isOptionalData)
            : base(bnfTerm, nodeCreator, isOptionalData)
        {
        }

        BnfTerm IBnfTerm<T>.AsTypeless()
        {
            return this;
        }

        [Obsolete("Use the typesafe QQ extension method instead", error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }
    }
}
