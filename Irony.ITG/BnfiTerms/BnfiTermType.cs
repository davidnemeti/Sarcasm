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
    public partial class BnfiTermType : BnfiTermNonTerminal, IBnfiTerm
    {
        public BnfiTermType(Type type, string errorAlias = null)
            : base(type, errorAlias)
        {
            if (type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, binder: null, types: Type.EmptyTypes, modifiers: null) == null)
                throw new ArgumentException("Type has no default constructor (neither public nor nonpublic)", "type");
        }

        public new BnfiExpressionType Rule
        {
            get { return (BnfiExpressionType)base.Rule; }
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
                            ((PropertyInfo)memberInfo).SetValue(obj, GrammarHelper.AstNodeToValue(parseTreeChild.AstNode));
                        }
                        else if (memberInfo is FieldInfo)
                        {
                            ((FieldInfo)memberInfo).SetValue(obj, GrammarHelper.AstNodeToValue(parseTreeChild.AstNode));
                        }
                    }

                    parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(obj, context, parseTreeNode);
                };

                foreach (var bnfTermList in ((BnfExpression)value).Data)
                {
                    foreach (var bnfTerm in bnfTermList)
                    {
                        if (bnfTerm is BnfiTermMember)
                            ((BnfiTermMember)bnfTerm).Reduced += nonTerminal_Reduced;
                    }
                }

                base.Rule = value;
            }
        }

        public BnfExpression RuleTL { get { return base.Rule; } set { base.Rule = value; } }

        void nonTerminal_Reduced(object sender, ReducedEventArgs e)
        {
            if (e.ResultNode.Tag != null && !object.Equals(e.ResultNode.Tag, ((BnfiTermMember)sender).MemberInfo))
            {
                throw new ApplicationException(string.Format("Internal error in binding framework. Reduce of {0} was bound to {1} and now to {2}",
                    ((BnfiTermMember)sender).Name,
                    ((MemberInfo)e.ResultNode.Tag).Name,
                    ((BnfiTermMember)sender).MemberInfo.Name));
            }

            e.ResultNode.Tag = ((BnfiTermMember)sender).MemberInfo;
        }

        public BnfTerm AsBnfTerm()
        {
            return this;
        }
    }

    public partial class BnfiTermType<TType> : BnfiTermType, IBnfiTerm<TType>, INonTerminalWithMultipleTypesafeRule<TType>
        where TType : new()
    {
        public static TType __ { get; private set; }

        static BnfiTermType()
        {
            __ = new TType();
        }

        public TType _ { get { return BnfiTermType<TType>.__; } }

        public BnfiTermType(string errorAlias = null)
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
