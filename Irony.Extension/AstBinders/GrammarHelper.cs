﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.IO;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Irony.Extension.AstBinders
{
    public static class GrammarHelper
    {
        #region StarList and PlusList

        #region Typesafe

        public static IBnfTerm<TCollectionType> StarList<TCollectionType, TElementType>(this IBnfTerm<TElementType> bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<TElementType>, new()
        {
            var typeForCollection = TypeForCollection.Of<TCollectionType, TElementType>();
            typeForCollection.Rule = Grammar.CurrentGrammar.MakeStarRule(typeForCollection, delimiter, bnfTermElement.AsTypeless());
            return typeForCollection;
        }

        public static IBnfTerm<List<TElementType>> StarList<TElementType>(this IBnfTerm<TElementType> bnfTermElement, BnfTerm delimiter = null)
        {
            return StarList<List<TElementType>, TElementType>(bnfTermElement, delimiter);
        }

        public static IBnfTerm<TCollectionType> PlusList<TCollectionType, TElementType>(this IBnfTerm<TElementType> bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<TElementType>, new()
        {
            var typeForCollection = TypeForCollection.Of<TCollectionType, TElementType>();
            typeForCollection.Rule = Grammar.CurrentGrammar.MakePlusRule(typeForCollection, delimiter, bnfTermElement.AsTypeless());
            return typeForCollection;
        }

        public static IBnfTerm<List<TElementType>> PlusList<TElementType>(this IBnfTerm<TElementType> bnfTermElement, BnfTerm delimiter = null)
        {
            return PlusList<List<TElementType>, TElementType>(bnfTermElement, delimiter);
        }

        #endregion

        #region Typeless converted to typesafe

        public static IBnfTerm<TCollectionType> StarList<TCollectionType, TElementType>(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<TElementType>, new()
        {
            var typeForCollection = TypeForCollection.Of<TCollectionType, TElementType>();
            typeForCollection.Rule = Grammar.CurrentGrammar.MakeStarRule(typeForCollection, delimiter, bnfTermElement);
            return typeForCollection;
        }

        public static IBnfTerm<List<TElementType>> StarList<TElementType>(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return StarList<List<TElementType>, TElementType>(bnfTermElement, delimiter);
        }

        public static IBnfTerm<TCollectionType> PlusList<TCollectionType, TElementType>(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<TElementType>, new()
        {
            var typeForCollection = TypeForCollection.Of<TCollectionType, TElementType>();
            typeForCollection.Rule = Grammar.CurrentGrammar.MakePlusRule(typeForCollection, delimiter, bnfTermElement);
            return typeForCollection;
        }

        public static IBnfTerm<List<TElementType>> PlusList<TElementType>(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return PlusList<List<TElementType>, TElementType>(bnfTermElement, delimiter);
        }

        #endregion

        #region Typeless

        public static IBnfTerm<TCollectionType> StarListTL<TCollectionType>(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<object>, new()
        {
            var typeForCollection = TypeForCollection.Of<TCollectionType>();
            typeForCollection.Rule = Grammar.CurrentGrammar.MakeStarRule(typeForCollection, delimiter, bnfTermElement);
            return typeForCollection;
        }

        public static IBnfTerm<List<object>> StarListTL(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return StarListTL<List<object>>(bnfTermElement, delimiter);
        }

        public static IBnfTerm<TCollectionType> PlusListTL<TCollectionType>(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<object>, new()
        {
            var typeForCollection = TypeForCollection.Of<TCollectionType>();
            typeForCollection.Rule = Grammar.CurrentGrammar.MakePlusRule(typeForCollection, delimiter, bnfTermElement);
            return typeForCollection;
        }

        public static IBnfTerm<List<object>> PlusListTL(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return PlusListTL<List<object>>(bnfTermElement, delimiter);
        }

        #endregion

        #endregion

        #region Q

        public static IBnfTerm<T?> QVal<T>(this IBnfTerm<T> bnfTerm)
            where T : struct
        {
            return ValueForBnfTerm.SetValueOptVal(bnfTerm);
        }

        public static IBnfTerm<T> QRef<T>(this IBnfTerm<T> bnfTerm)
            where T : class
        {
            return ValueForBnfTerm.SetValueOptRef(bnfTerm);
        }

        #endregion

        #region BindMember

        public static MemberBoundToBnfTerm BindMember<TBnfTermType, TMemberType>(this IBnfTerm<TBnfTermType> bnfTerm, Expression<Func<TMemberType>> exprForFieldOrPropertyAccess)
            where TBnfTermType : TMemberType
        {
            return MemberBoundToBnfTerm.Bind(exprForFieldOrPropertyAccess, bnfTerm);
        }

        public static MemberBoundToBnfTerm<TDeclaringType> BindMember<TDeclaringType, TBnfTermType, TMemberType>(this IBnfTerm<TBnfTermType> bnfTerm, Expression<Func<TMemberType>> exprForFieldOrPropertyAccess)
            where TBnfTermType : TMemberType
        {
            return MemberBoundToBnfTerm.Bind<TDeclaringType, TMemberType, TBnfTermType>(exprForFieldOrPropertyAccess, bnfTerm);
        }

        public static MemberBoundToBnfTerm<TDeclaringType> BindMember<TDeclaringType, TBnfTermType, TMemberType>(this IBnfTerm<TBnfTermType> bnfTerm, IBnfTerm<TDeclaringType> dummyBnfTerm, Expression<Func<TMemberType>> exprForFieldOrPropertyAccess)
            where TBnfTermType : TMemberType
        {
            return MemberBoundToBnfTerm.Bind<TDeclaringType, TMemberType, TBnfTermType>(dummyBnfTerm, exprForFieldOrPropertyAccess, bnfTerm);
        }

        public static MemberBoundToBnfTerm BindMember(this BnfTerm bnfTerm, PropertyInfo propertyInfo)
        {
            return MemberBoundToBnfTerm.Bind(propertyInfo, bnfTerm);
        }

        public static MemberBoundToBnfTerm BindMember(this BnfTerm bnfTerm, FieldInfo fieldInfo)
        {
            return MemberBoundToBnfTerm.Bind(fieldInfo, bnfTerm);
        }

        #endregion

        #region SetValue

        public static ValueForBnfTerm<TOut> CreateValue<TOut>(this BnfTerm bnfTerm, AstValueCreator<TOut> astValueCreator)
        {
            return ValueForBnfTerm.Create(bnfTerm, astValueCreator);
        }

        public static ValueForBnfTerm<TOut> CreateValue<TIn, TOut>(this IBnfTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
        {
            return ValueForBnfTerm.Create(bnfTerm, valueConverter);
        }

        public static ValueForBnfTerm<TOut> CreateValue<TOut>(this BnfTerm bnfTerm, TOut value)
        {
            return ValueForBnfTerm.Create(bnfTerm, value);
        }

        #endregion

        #region GetMember

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

        #endregion

        #region Misc

        public static IBnfTerm<T> ToType<T>(this BnfTerm bnfTerm)
        {
            return new BnfExpression<T>(bnfTerm);
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

        #endregion

        #region "Extension Properties"

        public static Properties Properties { get { return Properties.Instance; } }

        #endregion
    }

    public class Properties
    {
        private readonly Grammar defaultGrammar = null;

        private readonly Properties instance = new Properties();

        public Properties Instance { get { return instance; } }

        private Properties()
        {
            this[defaultGrammar, BoolProperty.BrowsableAstNodes] = false;
        }

        public bool this[Grammar grammar, BoolProperty boolProperty]
        {
            get
            {
                return GetValue<BoolProperty, bool>(grammar, boolProperty);
            }
            set
            {
                SetValue(grammar, boolProperty, value);
            }
        }

        private TValue GetValue<TProperty, TValue>(Grammar grammar, TProperty property)
        {
            object value;
            return propertyToValue.TryGetValue(Tuple.Create(grammar, (object)property), out value)
                ? (TValue)value
                : (TValue)propertyToValue[Tuple.Create(defaultGrammar, (object)property)];
        }

        private void SetValue<TProperty, TValue>(Grammar grammar, TProperty property, TValue value)
        {
            propertyToValue[Tuple.Create(defaultGrammar, (object)property)] = value;
        }

        private Dictionary<Tuple<Grammar, object>, object> propertyToValue = new Dictionary<Tuple<Grammar, object>, object>();
    }

    public enum BoolProperty
    {
        BrowsableAstNodes
    }
}
