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
    public class TypeForBoundMembers : TypeForNonTerminal
    {
        protected TypeForBoundMembers(Type type, string errorAlias)
            : base(type, errorAlias)
        {
        }

        public static TypeForBoundMembers<TType> Of<TType>(string errorAlias = null)
            where TType : new()
        {
            return new TypeForBoundMembers<TType>(typeof(TType), errorAlias);
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
                    parseTreeNode.AstNode = Activator.CreateInstance(type, nonPublic: true);

                    foreach (var parseTreeChild in parseTreeNode.ChildNodes)
                    {
                        MemberInfo memberInfo = parseTreeChild.Tag as MemberInfo;

                        if (memberInfo is PropertyInfo)
                        {
                            ((PropertyInfo)memberInfo).SetValue(parseTreeNode.AstNode, parseTreeChild.AstNode);
                        }
                        else if (memberInfo is FieldInfo)
                        {
                            ((FieldInfo)memberInfo).SetValue(parseTreeNode.AstNode, parseTreeChild.AstNode);
                        }
                        else if (!parseTreeChild.Term.Flags.IsSet(TermFlags.NoAstNode))
                        {
                            // NOTE: we shouldn't get here since the Rule setter should have handle this kind of error
                            context.AddMessage(ErrorLevel.Error, parseTreeChild.Token.Location, "No property or field assigned for term: {0}", parseTreeChild.Term);
                        }
                    }
                };

                foreach (var bnfTermList in value.Data)
                {
                    foreach (var bnfTerm in bnfTermList)
                    {
                        if (bnfTerm is MemberBoundToBnfTerm)
                            ((MemberBoundToBnfTerm)bnfTerm).Reduced += nonTerminal_Reduced;
                        else if (!bnfTerm.Flags.IsSet(TermFlags.NoAstNode))
                            GrammarHelper.ThrowGrammarError(GrammarErrorLevel.Error, "No property or field assigned for term: {0}", bnfTerm);
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

    public class TypeForBoundMembers<TType> : TypeForBoundMembers, IBnfTerm<TType>
        where TType : new()
    {
        public static TType __ { get; private set; }

        static TypeForBoundMembers()
        {
            __ = new TType();
        }

        public TType _ { get { return TypeForBoundMembers<TType>.__; } }

        internal TypeForBoundMembers(Type type, string errorAlias)
            : base(type, errorAlias)
        {
        }

        BnfTerm IBnfTerm<TType>.AsTypeless()
        {
            return this;
        }
    }
}
