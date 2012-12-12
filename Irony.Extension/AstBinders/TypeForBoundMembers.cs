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
    public class TypeForBoundMembers : TypeForNonTerminal
    {
        protected TypeForBoundMembers(Type type, string errorAlias)
            : base(type, errorAlias)
        {
        }

        public static TypeForBoundMembers<TType> Of<TType>(string errorAlias = null)
            where TType : new()
        {
            return new TypeForBoundMembers<TType>(errorAlias);
        }

        public static TypeForBoundMembers Of(Type type, string errorAlias = null)
        {
            if (type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, binder: null, types: Type.EmptyTypes, modifiers: null) == null)
                throw new ArgumentException("Type has no default constructor (neither public nor nonpublic)", "type");

            return new TypeForBoundMembers(type, errorAlias);
        }

        public override BnfExpression Rule
        {
            get { return base.Rule; }
            set
            {
                AstConfig.NodeCreator = (context, parseTreeNode) =>
                {
                    parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(Activator.CreateInstance(type, nonPublic: true), context, parseTreeNode);

                    foreach (var parseTreeChild in parseTreeNode.ChildNodes)
                    {
                        MemberInfo memberInfo = parseTreeChild.Tag as MemberInfo;

                        if (memberInfo is PropertyInfo)
                        {
                            ((PropertyInfo)memberInfo).SetValue(GrammarHelper.AstNodeToValue<object>(parseTreeNode.AstNode), GrammarHelper.AstNodeToValue<object>(parseTreeChild.AstNode));
                        }
                        else if (memberInfo is FieldInfo)
                        {
                            ((FieldInfo)memberInfo).SetValue(GrammarHelper.AstNodeToValue<object>(parseTreeNode.AstNode), GrammarHelper.AstNodeToValue<object>(parseTreeChild.AstNode));
                        }
                    }
                };

                foreach (var bnfTermList in value.Data)
                {
                    foreach (var bnfTerm in bnfTermList)
                    {
                        if (bnfTerm is MemberBoundToBnfTerm)
                            ((MemberBoundToBnfTerm)bnfTerm).Reduced += nonTerminal_Reduced;
                    }
                }

                base.Rule = value;
            }
        }

        void nonTerminal_Reduced(object sender, ReducedEventArgs e)
        {
            e.ResultNode.Tag = ((MemberBoundToBnfTerm)sender).MemberInfo;
        }
    }

    public class TypeForBoundMembers<TType> : TypeForBoundMembers, IBnfTerm<TType>, INonTerminalWithMultipleTypesafeRule<TType>
        where TType : new()
    {
        public static TType __ { get; private set; }

        static TypeForBoundMembers()
        {
            __ = new TType();
        }

        public TType _ { get { return TypeForBoundMembers<TType>.__; } }

        internal TypeForBoundMembers(string errorAlias)
            : base(typeof(TType), errorAlias)
        {
        }

        BnfTerm IBnfTerm<TType>.AsTypeless()
        {
            return this;
        }

        [Obsolete(typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        public new IBnfTerm<TType> Rule { set { this.SetRule(value); } }

        public BnfExpression RuleTL { get { return base.Rule; } set { base.Rule = value; } }

        public static BnfExpression<TType> operator |(TypeForBoundMembers<TType> term1, TypeForBoundMembers<TType> term2)
        {
            return GrammarHelper.Op_Pipe<TType>(term1, term2);
        }

        public static BnfExpression<TType> operator |(BnfExpression<TType> term1, TypeForBoundMembers<TType> term2)
        {
            return GrammarHelper.Op_Pipe<TType>(term1, term2);
        }

        public static BnfExpression<TType> operator |(TypeForBoundMembers<TType> term1, BnfExpression<TType> term2)
        {
            return GrammarHelper.Op_Pipe<TType>(term1, term2);
        }

        public static BnfExpression<TType> operator +(BnfExpression<TType> term1, TypeForBoundMembers<TType> term2)
        {
            return GrammarHelper.Op_Plus<TType>(term1, term2);
        }

        public static BnfExpression<TType> operator +(TypeForBoundMembers<TType> term1, BnfExpression<TType> term2)
        {
            return GrammarHelper.Op_Plus<TType>(term1, term2);
        }

        public static BnfExpression<TType> operator +(BnfExpression term1, TypeForBoundMembers<TType> term2)
        {
            return GrammarHelper.Op_Plus<TType>(term1, term2);
        }

        public static BnfExpression<TType> operator +(TypeForBoundMembers<TType> term1, BnfExpression term2)
        {
            return GrammarHelper.Op_Plus<TType>(term1, term2);
        }
    }
}
