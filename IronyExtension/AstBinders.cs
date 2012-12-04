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
    //class BnfTermProperty : BnfExpression
    //{
    //    readonly PropertyInfo propertyInfo;

    //    public BnfTermProperty(PropertyInfo propertyInfo, BnfTerm element)
    //        : base(element)
    //    {
    //        this.propertyInfo = propertyInfo;
    //    }
    //}

    //class BnfTermProperty : BnfTerm
    //{
    //    readonly PropertyInfo propertyInfo;
    //    public readonly BnfTerm bnfTerm;

    //    public BnfTermProperty(PropertyInfo propertyInfo, BnfTerm bnfTerm)
    //        : base(propertyInfo.Name)
    //    {
    //        this.propertyInfo = propertyInfo;
    //        this.bnfTerm = bnfTerm;
    //    }
    //}

    public class PropertyForBnfTerm : NonTerminal
    {
        public PropertyInfo PropertyInfo { get; private set; }
        public BnfTerm BnfTerm { get; private set; }

        private PropertyForBnfTerm(PropertyInfo propertyInfo, BnfTerm bnfTerm)
            : base(name: string.Format("{0}.{1}", Helper.TypeNameWithDeclaringTypes(propertyInfo.DeclaringType), propertyInfo.Name.ToLower()))
        {
            this.PropertyInfo = propertyInfo;
            this.BnfTerm = bnfTerm;
            this.Flags |= TermFlags.IsTransient | TermFlags.NoAstNode;
            this.Rule = new BnfExpression(bnfTerm);
        }

        public static PropertyForBnfTerm Bind(PropertyInfo propertyInfo, BnfTerm bnfTerm)
        {
            return new PropertyForBnfTerm(propertyInfo, bnfTerm);
        }

        public static PropertyForBnfTerm Bind<T>(Expression<Func<T>> exprForPropertyAccess, BnfTerm bnfTerm)
        {
            return new PropertyForBnfTerm(Helper.GetProperty(exprForPropertyAccess), bnfTerm);
        }
    }

    public static class Helper
    {
        public static PropertyInfo GetProperty<T>(Expression<Func<T>> exprForPropertyAccess)
        {
            var member = exprForPropertyAccess.Body as MemberExpression;
            if (member == null)
                throw new InvalidOperationException("Expression is not a member access expression.");
            var property = member.Member as PropertyInfo;
            if (property == null)
                throw new InvalidOperationException("Member in expression is not a property.");
            return property;
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

        public static string GetNonTerminalsAsText(LanguageData language, bool omitProperties = false)
        {
            var sw = new StringWriter();
            foreach (var nonTerminal in language.GrammarData.NonTerminals.OrderBy(nonTerminal => nonTerminal.Name))
            {
                if (omitProperties && nonTerminal is PropertyForBnfTerm)
                    continue;

                sw.WriteLine("{0}{1}", nonTerminal.Name, nonTerminal.Flags.IsSet(TermFlags.IsNullable) ? "  (Nullable) " : string.Empty);
                foreach (Production pr in nonTerminal.Productions)
                {
                    sw.WriteLine("   {0}", ProductionToString(pr, omitProperties));
                }
            }
            return sw.ToString();
        }

        private static string ProductionToString(Production production, bool omitProperties)
        {
            var sw = new StringWriter();
            sw.Write("{0} -> ", production.LValue.Name);
            foreach (BnfTerm bnfTerm in production.RValues)
            {
                BnfTerm bnfTermToWrite = omitProperties && bnfTerm is PropertyForBnfTerm
                    ? ((PropertyForBnfTerm)bnfTerm).BnfTerm
                    : bnfTerm;

                sw.Write("{0} ", bnfTermToWrite.Name);
            }
            return sw.ToString();
        }
    }

    public class TypeForNonTerminal : NonTerminal
    {
        readonly Type type;

        //IDictionary<int, PropertyInfo> parseTreeChildIndexToProperty = new Dictionary<int, PropertyInfo>();
        //ISet<BnfTerm> bnfTermsPunctuationOrEmptyTransient = new HashSet<BnfTerm>();

        //void NonTerminalType_Reduced(object sender, ReducedEventArgs e)
        //{
        //    int parseTreeChildIndex = 0;
        //    foreach (BnfTerm bnfTerm in e.ReducedProduction.RValues)
        //    {
        //        // NOTE: we can recognize empty transient terms only by using bnfTermsPunctuationOrEmptyTransient (since they were eliminated earlier)
        //        if (bnfTerm.Flags.IsSet(TermFlags.IsPunctuation) || bnfTermsPunctuationOrEmptyTransient.Contains(bnfTerm))
        //            continue;

        //        if (bnfTerm is BnfTermProperty)
        //        {
        //            BnfTermProperty bnfTermProperty = (BnfTermProperty)bnfTerm;
        //            parseTreeChildIndexToProperty[parseTreeChildIndex] = bnfTermProperty.PropertyInfo;
        //        }

        //        parseTreeChildIndex++;
        //    }
        //    bnfTermsPunctuationOrEmptyTransient.Clear();
        //}

        public TypeForNonTerminal(Type type)
            : base(Helper.TypeNameWithDeclaringTypes(type))
        {
            this.type = type;
            //            this.Reduced += NonTerminalType_Reduced;
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
                        PropertyInfo propertyInfo = (PropertyInfo)parseTreeChild.Tag;
                        if (propertyInfo != null)
                        {
                            propertyInfo.SetValue(parseTreeNode.AstNode, parseTreeChild.AstNode);
                        }
                        else if (!parseTreeChild.Term.Flags.IsSet(TermFlags.NoAstNode))
                        {
                            // NOTE: we shouldn't get here since the Rule setter should have handle this kind of error
                            context.AddMessage(ErrorLevel.Warning, parseTreeChild.Token.Location, "No property assigned for term: {0}", parseTreeChild.Term);
                        }
                    }

                    //foreach (var parseTreeChild in parseTreeNode.ChildNodes.Select((parseTreeChild, parseTreeChildIndex) =>
                    //    new { Value = parseTreeChild, Index = parseTreeChildIndex }))
                    //{
                    //    PropertyInfo propertyInfo;
                    //    if (parseTreeChildIndexToProperty.TryGetValue(parseTreeChild.Index, out propertyInfo))
                    //    {
                    //        propertyInfo.SetValue(parseTreeNode.AstNode, parseTreeChild.Value.AstNode);
                    //    }
                    //    else if (!parseTreeChild.Value.Term.Flags.IsSet(TermFlags.NoAstNode))
                    //    {
                    //        context.AddMessage(ErrorLevel.Warning, parseTreeChild.Value.Token.Location, "No property assigned for term: {0}", parseTreeChild.Value.Term);
                    //    }
                    //}

                    //parseTreeChildIndexToProperty.Clear();
                };

                foreach (var bnfTermList in value.Data)
                {
                    foreach (var bnfTerm in bnfTermList)
                    {
                        if (bnfTerm is PropertyForBnfTerm)
                            ((PropertyForBnfTerm)bnfTerm).Reduced += nonTerminal_Reduced;
                        else if (!bnfTerm.Flags.IsSet(TermFlags.NoAstNode))
                            Helper.ThrowGrammarError(GrammarErrorLevel.Error, "No property assigned for term: {0}", bnfTerm);
                    }
                }

                base.Rule = value;
            }
        }

        void nonTerminal_Reduced(object sender, ReducedEventArgs e)
        {
            e.ResultNode.Tag = ((PropertyForBnfTerm)sender).PropertyInfo;
            //if (e.ResultNode.IsPunctuationOrEmptyTransient())
            //    bnfTermsPunctuationOrEmptyTransient.Add(e.ResultNode.Term);
        }
    }

    public class ObjectForTerminal : NonTerminal
    {
        readonly Terminal terminal;

        public ObjectForTerminal(Terminal terminal, AstNodeCreator nodeCreator)
            : base(terminal.Name)
        {
            this.terminal = terminal;
            this.AstConfig.NodeCreator = nodeCreator;
            this.Rule = terminal;
        }
    }
}
