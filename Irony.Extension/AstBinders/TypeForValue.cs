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
    public class TypeForValue : TypeForNonTerminal
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

        protected TypeForValue(Type type, BnfTerm bnfTerm, bool isOptionalData, string errorAlias)
            : base(type, errorAlias)
        {
            this.Flags |= TermFlags.IsTransient | TermFlags.NoAstNode;

            this.RuleTL = isOptionalData
                ? bnfTerm | Grammar.CurrentGrammar.Empty
                : new BnfExpression(bnfTerm);
        }

        protected TypeForValue(Type type, BnfTerm bnfTerm, AstValueCreator<object> astValueCreator, bool isOptionalData, string errorAlias)
            : base(type, errorAlias)
        {
            //if (bnfTerm is Terminal)
            //    bnfTerm.Flags |= TermFlags.IsTransient | TermFlags.NoAstNode;   // instead of the terminal let the current TypeForValue read from Token

            AstNodeCreator nodeCreator = (context, parseTreeNode) =>
                parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(astValueCreator(context, new ParseTreeNodeWithOutAst(parseTreeNode)), context, parseTreeNode);

            this.AstConfig.NodeCreator = nodeCreator;

            this.RuleTL = isOptionalData
                ? bnfTerm | Grammar.CurrentGrammar.Empty
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
                (context, parseNode) =>
                    {
                        if (parseNode.ChildNodes.Count != 1)
                            throw new ArgumentException("Only one child is allowed for a TypeForValue term: {0}", parseNode.Term.Name);

                        return valueConverter(GrammarHelper.AstNodeToValue<object>(parseNode.ChildNodes[0].AstNode));
                    },
                isOptionalData: false,
                errorAlias: null
                );
        }

        public static TypeForValue<TOut> Convert<TIn, TOut>(IBnfTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
        {
            return new TypeForValue<TOut>(
                bnfTerm.AsTypeless(),
                (context, parseNode) =>
                    {
                        if (parseNode.ChildNodes.Count != 1)
                            throw new ArgumentException("Only one child is allowed for a TypeForValue term: {0}", parseNode.Term.Name);

                        return valueConverter(GrammarHelper.AstNodeToValue<TIn>(parseNode.ChildNodes[0].AstNode));
                    },
                isOptionalData: false,
                errorAlias: null
                );
        }

        public static TypeForValue<TOut> Cast<TIn, TOut>(IBnfTerm<TIn> bnfTerm)
        {
            return new TypeForValue<TOut>(
                bnfTerm.AsTypeless(),
                isOptionalData: false,
                errorAlias: null
                );
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
                bnfTerm.AsTypeless(),
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
    }

    public class TypeForValue<T> : TypeForValue, IBnfTerm<T>
    {
        internal TypeForValue(string errorAlias)
            : base(typeof(T), errorAlias)
        {
        }

        internal TypeForValue(BnfTerm bnfTerm, bool isOptionalData, string errorAlias)
            : base(typeof(T), bnfTerm, isOptionalData, errorAlias)
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

        BnfTerm IBnfTerm<T>.AsTypeless()
        {
            return this;
        }

        [Obsolete(TypeForNonTerminal.typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        //[Obsolete("blabla", error: true)]
        //public static BnfExpression<T> operator +(TypeForValue<T> bnfTerm1, TypeForValue<T> bnfTerm2)
        //{
        //    return Op_Plus(bnfTerm1, bnfTerm2);
        //}

        //[Obsolete("blabla", error: true)]
        //public static BnfExpression<T> operator +(BnfTerm bnfTerm1, TypeForValue<T> bnfTerm2)
        //{
        //    return Op_Plus(bnfTerm1, bnfTerm2);
        //}

        //[Obsolete("blabla", error: true)]
        //public static BnfExpression<T> operator +(TypeForValue<T> bnfTerm1, BnfTerm bnfTerm2)
        //{
        //    return Op_Plus(bnfTerm1, bnfTerm2);
        //}

        //public static BnfExpression<T> operator +(ValueForBnfTerm<T> bnfTerm1, IBnfTerm<T> bnfTerm2)
        //{
        //    return Op_Plus(bnfTerm1, (BnfExpression)bnfTerm2);
        //}

        //public static BnfExpression<T> operator +(IBnfTerm<T> bnfTerm1, ValueForBnfTerm<T> bnfTerm2)
        //{
        //    return Op_Plus((BnfExpression)bnfTerm1, bnfTerm2);
        //}

        //public static BnfExpression<T> operator +(ValueForBnfTerm<T> bnfTerm1, BnfExpression bnfTerm2)
        //{
        //    return Op_Plus(bnfTerm1, bnfTerm2);
        //}

        //public static BnfExpression<T> operator +(BnfExpression bnfTerm1, ValueForBnfTerm<T> bnfTerm2)
        //{
        //    return Op_Plus(bnfTerm1, bnfTerm2);
        //}

        public static BnfExpression<T> operator |(TypeForValue<T> term1, TypeForValue<T> term2)
        {
            return GrammarHelper.Op_Pipe<T>(term1, term2);
        }

        //public static BnfExpression<T> operator |(ValueForBnfTerm<T> term1, IBnfTerm<T> term2)
        //{
        //    return Op_Pipe(term1, term2.AsTypeless());
        //}

        //public static BnfExpression<T> operator |(IBnfTerm<T> term1, ValueForBnfTerm<T> term2)
        //{
        //    return Op_Pipe(term1.AsTypeless(), term2);
        //}

        public static BnfExpression<T> operator |(TypeForValue<T> term1, BnfTerm term2)
        {
            return Op_Pipe(term1, term2);
        }

        public static BnfExpression<T> operator |(BnfTerm term1, TypeForValue<T> term2)
        {
            return Op_Pipe(term1, term2);
        }

        protected new static BnfExpression<T> Op_Plus(BnfTerm bnfTerm1, BnfTerm bnfTerm2)
        {
            return GrammarHelper.Op_Plus<T>(bnfTerm1, bnfTerm2);
        }

        protected new static BnfExpression<T> Op_Pipe(BnfTerm bnfTerm1, BnfTerm bnfTerm2)
        {
            return GrammarHelper.Op_Pipe<T>(bnfTerm1, bnfTerm2);
        }
    }
}
