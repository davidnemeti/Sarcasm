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
    public class MemberBoundToBnfTerm : NonTerminal
    {
        public MemberInfo MemberInfo { get; private set; }
        public BnfTerm BnfTerm { get; private set; }

        private MemberBoundToBnfTerm(MemberInfo memberInfo, BnfTerm bnfTerm)
            : base(name: string.Format("{0}.{1}", Helper.TypeNameWithDeclaringTypes(memberInfo.DeclaringType), memberInfo.Name.ToLower()))
        {
            this.MemberInfo = memberInfo;
            this.BnfTerm = bnfTerm;
            this.Flags |= TermFlags.IsTransient | TermFlags.NoAstNode;
            this.Rule = new BnfExpression(bnfTerm);
        }

        public static MemberBoundToBnfTerm Bind(PropertyInfo propertyInfo, BnfTerm bnfTerm)
        {
            return new MemberBoundToBnfTerm(propertyInfo, bnfTerm);
        }

        public static MemberBoundToBnfTerm Bind(FieldInfo fieldInfo, BnfTerm bnfTerm)
        {
            return new MemberBoundToBnfTerm(fieldInfo, bnfTerm);
        }

        public static MemberBoundToBnfTerm Bind<TMemberType>(Expression<Func<TMemberType>> exprForFieldOrPropertyAccess, BnfTerm bnfTerm)
        {
            MemberInfo memberInfo = Helper.GetMember(exprForFieldOrPropertyAccess);

            if (memberInfo is FieldInfo || memberInfo is PropertyInfo)
                return new MemberBoundToBnfTerm(memberInfo, bnfTerm);
            else
                throw new ArgumentException("Field or property not found", memberInfo.Name);

        }

        public static MemberBoundToBnfTerm Bind<TDeclaringType>(string fieldOrPropertyName, BnfTerm bnfTerm)
        {
            Type declaringType = typeof(TDeclaringType);
            MemberInfo memberInfo = (MemberInfo)declaringType.GetField(fieldOrPropertyName) ?? (MemberInfo)declaringType.GetProperty(fieldOrPropertyName);

            if (memberInfo == null)
                throw new ArgumentException("Field or property not found", fieldOrPropertyName);

            return new MemberBoundToBnfTerm(memberInfo, bnfTerm);
        }
    }

    public class NonTerminalWithType : NonTerminal
    {
        private readonly Type type;

        protected NonTerminalWithType(Type type)
            : base(Helper.TypeNameWithDeclaringTypes(type))
        {
            this.type = type;
        }

        public static NonTerminalWithType Of<T>()
            where T: new()
        {
            return new NonTerminalWithType(typeof(T));
        }

        public new BnfExpression Rule
        {
            get { return base.Rule; }
            set
            {
                AstConfig.NodeCreator = (context, parseTreeNode) =>
                {
                    parseTreeNode.AstNode = Activator.CreateInstance(type);

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
                            context.AddMessage(ErrorLevel.Warning, parseTreeChild.Token.Location, "No property or field assigned for term: {0}", parseTreeChild.Term);
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
                            Helper.ThrowGrammarError(GrammarErrorLevel.Error, "No property or field assigned for term: {0}", bnfTerm);
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

    public class ObjectBoundToTerminal : NonTerminal
    {
        private readonly Terminal terminal;

        public ObjectBoundToTerminal(Terminal terminal, AstNodeCreator nodeCreator)
            : base(terminal.Name)
        {
            this.terminal = terminal;
            this.AstConfig.NodeCreator = nodeCreator;
            this.Rule = terminal;
        }
    }

    public static class Helper
    {
        public static MemberBoundToBnfTerm Bind<T>(this BnfTerm bnfTerm, Expression<Func<T>> exprForPropertyAccess)
        {
            return MemberBoundToBnfTerm.Bind(exprForPropertyAccess, bnfTerm);
        }

        public static MemberBoundToBnfTerm Bind<T>(this BnfTerm bnfTerm, PropertyInfo propertyInfo)
        {
            return MemberBoundToBnfTerm.Bind(propertyInfo, bnfTerm);
        }

        public static PropertyInfo GetProperty<T>(Expression<Func<T>> exprForPropertyAccess)
        {
            var memberExpression = exprForPropertyAccess.Body as MemberExpression;
            if (memberExpression == null)
                throw new InvalidOperationException("Expression is not a member access expression.");

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new InvalidOperationException("Member in expression is not a property.");

            return propertyInfo;
        }

        public static FieldInfo GetField<T>(Expression<Func<T>> exprForFieldAccess)
        {
            var memberExpression = exprForFieldAccess.Body as MemberExpression;
            if (memberExpression == null)
                throw new InvalidOperationException("Expression is not a member access expression.");

            var fieldInfo = memberExpression.Member as FieldInfo;
            if (fieldInfo == null)
                throw new InvalidOperationException("Member in expression is not a field.");

            return fieldInfo;
        }

        public static MemberInfo GetMember<T>(Expression<Func<T>> exprForMemberAccess)
        {
            var memberExpression = exprForMemberAccess.Body as MemberExpression;
            if (memberExpression == null)
                throw new InvalidOperationException("Expression is not a member access expression.");

            var memberInfo = memberExpression.Member as MemberInfo;
            if (memberInfo == null)
                throw new InvalidOperationException("Member in expression is not a member.");

            return memberInfo;
        }

        public static void ThrowGrammarError(GrammarErrorLevel grammarErrorLevel, string message, params object[] args)
        {
            if (args.Length > 0)
                message = string.Format(message, args);

            throw new GrammarErrorException(message, new GrammarError(grammarErrorLevel, null, message));
        }

        public static string TypeNameWithDeclaringTypes(Type type)
        {
            return type.IsNested
                ? string.Format("{0}_{1}", TypeNameWithDeclaringTypes(type.DeclaringType), type.Name.ToLower())
                : type.Name.ToLower();
        }

        public static string GetNonTerminalsAsText(LanguageData language, bool omitBoundMembers = false)
        {
            var sw = new StringWriter();
            foreach (var nonTerminal in language.GrammarData.NonTerminals.OrderBy(nonTerminal => nonTerminal.Name))
            {
                if (omitBoundMembers && nonTerminal is MemberBoundToBnfTerm)
                    continue;

                sw.WriteLine("{0}{1}", nonTerminal.Name, nonTerminal.Flags.IsSet(TermFlags.IsNullable) ? "  (Nullable) " : string.Empty);
                foreach (Production pr in nonTerminal.Productions)
                {
                    sw.WriteLine("   {0}", ProductionToString(pr, omitBoundMembers));
                }
            }
            return sw.ToString();
        }

        private static string ProductionToString(Production production, bool omitBoundMembers)
        {
            var sw = new StringWriter();
            sw.Write("{0} -> ", production.LValue.Name);
            foreach (BnfTerm bnfTerm in production.RValues)
            {
                BnfTerm bnfTermToWrite = omitBoundMembers && bnfTerm is MemberBoundToBnfTerm
                    ? ((MemberBoundToBnfTerm)bnfTerm).BnfTerm
                    : bnfTerm;

                sw.Write("{0} ", bnfTermToWrite.Name);
            }
            return sw.ToString();
        }
    }
}
