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

        protected DataForBnfTerm(BnfTerm bnfTerm, AstNodeCreator nodeCreator)
            : base(bnfTerm.Name)
        {
            this.bnfTerm = bnfTerm;
            this.AstConfig.NodeCreator = nodeCreator;
            this.Rule = new BnfExpression(bnfTerm);
        }

        public static DataForBnfTerm SetValue(BnfTerm bnfTerm, AstNodeCreator nodeCreator)
        {
            return new DataForBnfTerm(bnfTerm, nodeCreator);
        }

        public static DataForBnfTerm<TOut> SetValue<TOut>(BnfTerm bnfTerm, AstObjectCreator<TOut> astObjectCreator)
        {
            return new DataForBnfTerm<TOut>(bnfTerm, (context, parseNode) => parseNode.AstNode = astObjectCreator(context, parseNode));
        }

        public static DataForBnfTerm<TOut> SetValue<TIn, TOut>(IBnfTerm<TIn> bnfTerm, AstObjectCreator<TIn, TOut> astObjectCreator)
        {
            return new DataForBnfTerm<TOut>(bnfTerm.AsTypeless(),
                (context, parseNode) => parseNode.AstNode = (TOut)astObjectCreator((TIn)parseNode.ChildNodes.Find(parseTreeChild => parseTreeChild.Term == bnfTerm).AstNode));
        }

        public static DataForBnfTerm<TOut> SetValue<TOut>(BnfTerm bnfTerm, TOut astObject)
        {
            return new DataForBnfTerm<TOut>(bnfTerm, (context, parseNode) => parseNode.AstNode = astObject);
        }
    }

    public class DataForBnfTerm<T> : DataForBnfTerm, IBnfTerm<T>
    {
        internal DataForBnfTerm(BnfTerm bnfTerm, AstNodeCreator nodeCreator)
            : base(bnfTerm, nodeCreator)
        {
        }

        BnfTerm IBnfTerm<T>.AsTypeless()
        {
            return this;
        }
    }
}
