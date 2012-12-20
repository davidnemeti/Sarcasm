using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Diagnostics;
using System.IO;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Irony.ITG
{
    public partial class TypeForBoundMembers : TypeForNonTerminal, IBnfTerm
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

        public new BnfExpressionBoundMembers Rule
        {
            get { return (BnfExpressionBoundMembers)base.Rule; }
            set
            {
                AstConfig.NodeCreator = (context, parseTreeNode) =>
                {
                    object obj = Activator.CreateInstance(type, nonPublic: true);

                    foreach (var parseTreeChild in parseTreeNode.ChildNodes)
                    {
                        MemberInfo memberInfo = parseTreeChild.Tag as MemberInfo;

                        if (memberInfo is PropertyInfo)
                        {
                            ((PropertyInfo)memberInfo).SetValue(obj, GrammarHelper.AstNodeToValue<object>(parseTreeChild.AstNode));
                        }
                        else if (memberInfo is FieldInfo)
                        {
                            ((FieldInfo)memberInfo).SetValue(obj, GrammarHelper.AstNodeToValue<object>(parseTreeChild.AstNode));
                        }
                    }

                    parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(obj, context, parseTreeNode);
                };

                foreach (var bnfTermList in ((BnfExpression)value).Data)
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

        public BnfExpression RuleTL { get { return base.Rule; } set { base.Rule = value; } }

        void nonTerminal_Reduced(object sender, ReducedEventArgs e)
        {
            if (e.ResultNode.Tag != null && !object.Equals(e.ResultNode.Tag, ((MemberBoundToBnfTerm)sender).MemberInfo))
            {
                throw new ApplicationException(string.Format("Internal error in binding framework. Reduce of {0} was bound to {1} and now to {2}",
                    ((MemberBoundToBnfTerm)sender).Name,
                    ((MemberInfo)e.ResultNode.Tag).Name,
                    ((MemberBoundToBnfTerm)sender).MemberInfo.Name));
            }

            e.ResultNode.Tag = ((MemberBoundToBnfTerm)sender).MemberInfo;
        }

        public BnfTerm AsBnfTerm()
        {
            return this;
        }
    }

    public partial class TypeForBoundMembers<TType> : TypeForBoundMembers, IBnfTerm<TType>, INonTerminalWithMultipleTypesafeRule<TType>
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

        [Obsolete(typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        public new BnfExpressionBoundMembers<TType> Rule { set { base.Rule = value; } }

    }
}
