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
    public delegate TOut AstObjectCreator<TOut>(AstContext context, ParseTreeNode parseNode);
    public delegate TOut AstObjectCreator<TIn, TOut>(TIn inputObject);

    public interface IBnfTerm<T>
    {
        BnfTerm AsTypeless();
    }

    public class MemberBoundToBnfTerm : NonTerminal
    {
        public MemberInfo MemberInfo { get; private set; }
        public BnfTerm BnfTerm { get; private set; }

        protected MemberBoundToBnfTerm(MemberInfo memberInfo, BnfTerm bnfTerm)
            : base(name: string.Format("{0}.{1}", GrammarHelper.TypeNameWithDeclaringTypes(memberInfo.DeclaringType), memberInfo.Name.ToLower()))
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

        public static MemberBoundToBnfTerm<TMemberType, TBnfTermType> Bind<TMemberType, TBnfTermType>(Expression<Func<TMemberType>> exprForFieldOrPropertyAccess, IBnfTerm<TBnfTermType> bnfTerm)
            where TBnfTermType : TMemberType
        {
            MemberInfo memberInfo = GrammarHelper.GetMember(exprForFieldOrPropertyAccess);

            if (memberInfo is FieldInfo || memberInfo is PropertyInfo)
                return new MemberBoundToBnfTerm<TMemberType, TBnfTermType>(memberInfo, bnfTerm);
            else
                throw new ArgumentException("Field or property not found", memberInfo.Name);
        }

        public static MemberBoundToBnfTerm Bind<TDeclaringType>(string fieldOrPropertyName, BnfTerm bnfTerm)
        {
            return Bind(typeof(TDeclaringType), fieldOrPropertyName, bnfTerm);
        }

        public static MemberBoundToBnfTerm Bind(Type declaringType, string fieldOrPropertyName, BnfTerm bnfTerm)
        {
            MemberInfo memberInfo = (MemberInfo)declaringType.GetField(fieldOrPropertyName) ?? (MemberInfo)declaringType.GetProperty(fieldOrPropertyName);

            if (memberInfo == null)
                throw new ArgumentException("Field or property not found", fieldOrPropertyName);

            return new MemberBoundToBnfTerm(memberInfo, bnfTerm);
        }
    }

    public class MemberBoundToBnfTerm<TMemberType, TBnfTermType> : MemberBoundToBnfTerm, IBnfTerm<TMemberType>
    {
        internal MemberBoundToBnfTerm(MemberInfo memberInfo, IBnfTerm<TBnfTermType> bnfTerm)
            : base(memberInfo, bnfTerm.AsTypeless())
        {
        }

        BnfTerm IBnfTerm<TMemberType>.AsTypeless()
        {
            return this;
        }
    }

    public abstract class TypeForNonTerminal : NonTerminal
    {
        protected Type type { get; private set; }

        protected TypeForNonTerminal(Type type, string errorAlias)
            : base(GrammarHelper.TypeNameWithDeclaringTypes(type), errorAlias)
        {
            this.type = type;
        }

        public new virtual BnfExpression Rule
        {
            get { return base.Rule; }
            set { base.Rule = value; }
        }
    }

    public class TypeForCollection : TypeForNonTerminal
    {
        private static readonly Type collectionBaseGenericTypeDefinition = typeof(ICollection<>);

        protected Type collectionType { get { return base.type; } }
        private readonly Type elementType;

        [Obsolete("Use collectionType instead", error: true)]
        protected new Type type { get { return base.type; } }

        protected TypeForCollection(Type collectionType, string errorAlias)
            : base(collectionType, errorAlias)
        {
            Type collectionBaseGenericDefinition = collectionType.GetInterfaces().FirstOrDefault(interfaceType => interfaceType.GetGenericTypeDefinition() == collectionBaseGenericTypeDefinition);

            if (collectionBaseGenericDefinition == null)
                throw new ArgumentException(string.Format("Type does not implement {0}", collectionBaseGenericTypeDefinition.FullName), "type");

            this.elementType = collectionBaseGenericDefinition.GetGenericArguments()[0];
        }

        public static TypeForCollection<TCollectionType> Of<TCollectionType>(string errorAlias = null)
            where TCollectionType : new()
        {
            return new TypeForCollection<TCollectionType>(typeof(TCollectionType), errorAlias);
        }

        public static TypeForCollection Of(Type collectionType, string errorAlias = null)
        {
            if (collectionType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, binder: null, types: Type.EmptyTypes, modifiers: null) == null)
                throw new ArgumentException("Type has no default constructor (neither public nor nonpublic)", "type");

            return new TypeForCollection(collectionType, errorAlias);
        }

        public override BnfExpression Rule
        {
            get { return base.Rule; }
            set
            {
                AstConfig.NodeCreator = (context, parseTreeNode) =>
                {
                    dynamic collection = Activator.CreateInstance(collectionType, nonPublic: true);

                    foreach (var parseTreeChild in parseTreeNode.ChildNodes)
                    {
                        if (parseTreeChild.AstNode.GetType() == elementType)
                        {
                            collection.Add(parseTreeChild.AstNode);
                        }
                        else if (!parseTreeChild.Term.Flags.IsSet(TermFlags.NoAstNode))
                        {
                            context.AddMessage(ErrorLevel.Error, parseTreeChild.Token.Location, "Term '{0}' should be type of '{1}' but found '{2}' instead",
                                parseTreeChild.Term, elementType.FullName, parseTreeChild.AstNode.GetType().FullName);
                        }
                    }

                    parseTreeNode.AstNode = collection;
                };

                base.Rule = value;
            }
        }
    }

    public class TypeForCollection<TCollectionType> : TypeForCollection, IBnfTerm<TCollectionType>
    {
        internal TypeForCollection(Type collectionType, string errorAlias)
            : base(collectionType, errorAlias)
        {
        }

        BnfTerm IBnfTerm<TCollectionType>.AsTypeless()
        {
            return this;
        }
    }

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
    {
        internal TypeForBoundMembers(Type type, string errorAlias)
            : base(type, errorAlias)
        {
        }

        BnfTerm IBnfTerm<TType>.AsTypeless()
        {
            return this;
        }
    }

    public class TypeForTransient : TypeForNonTerminal
    {
        protected TypeForTransient(Type type, string errorAlias)
            : base(type, errorAlias)
        {
            this.Flags |= TermFlags.IsTransient | TermFlags.NoAstNode;
        }

        public static TypeForTransient<TType> Of<TType>(string errorAlias = null)
        {
            return new TypeForTransient<TType>(typeof(TType), errorAlias);
        }

        public static TypeForTransient Of(Type type, string errorAlias = null)
        {
            return new TypeForTransient(type, errorAlias);
        }
    }

    public class TypeForTransient<TType> : TypeForTransient, IBnfTerm<TType>
    {
        internal TypeForTransient(Type type, string errorAlias)
            : base(type, errorAlias)
        {
        }

        BnfTerm IBnfTerm<TType>.AsTypeless()
        {
            return this;
        }
    }

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
                (context, parseNode) => parseNode.AstNode = (TOut) astObjectCreator((TIn) parseNode.ChildNodes.Find(parseTreeChild => parseTreeChild.Term == bnfTerm).AstNode));
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

    public static class GrammarHelper
    {
        public static MemberBoundToBnfTerm<TMemberType, TBnfTermType> Bind<TBnfTermType, TMemberType>(this IBnfTerm<TBnfTermType> bnfTerm, Expression<Func<TMemberType>> exprForFieldOrPropertyAccess)
            where TBnfTermType : TMemberType
        {
            return MemberBoundToBnfTerm.Bind(exprForFieldOrPropertyAccess, bnfTerm);
        }

        public static MemberBoundToBnfTerm Bind(this BnfTerm bnfTerm, PropertyInfo propertyInfo)
        {
            return MemberBoundToBnfTerm.Bind(propertyInfo, bnfTerm);
        }

        public static MemberBoundToBnfTerm Bind(this BnfTerm bnfTerm, FieldInfo fieldInfo)
        {
            return MemberBoundToBnfTerm.Bind(fieldInfo, bnfTerm);
        }

        public static DataForBnfTerm SetValue(this BnfTerm bnfTerm, AstNodeCreator nodeCreator)
        {
            return DataForBnfTerm.SetValue(bnfTerm, nodeCreator);
        }

        public static DataForBnfTerm<TOut> SetValue<TOut>(this BnfTerm bnfTerm, AstObjectCreator<TOut> astObjectCreator)
        {
            return DataForBnfTerm.SetValue(bnfTerm, astObjectCreator);
        }

        public static DataForBnfTerm<TOut> SetValue<TIn, TOut>(this IBnfTerm<TIn> bnfTerm, AstObjectCreator<TIn, TOut> astObjectCreator)
        {
            return DataForBnfTerm.SetValue(bnfTerm, astObjectCreator);
        }

        public static DataForBnfTerm<TOut> SetValue<TOut>(this BnfTerm bnfTerm, TOut astObject)
        {
            return DataForBnfTerm.SetValue(bnfTerm, astObject);
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
