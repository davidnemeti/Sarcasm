using System;
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

namespace Irony.ITG
{
    public static class GrammarHelper
    {
        #region StarList and PlusList

        #region Typesafe (TCollectionType, TElementType)

        public static BnfiTermCollection<TCollectionType, TElementType> StarList<TCollectionType, TElementType>(this IBnfiTerm<TElementType> bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return BnfiTermCollection.StarList<TCollectionType, TElementType>(bnfTermElement, delimiter);
        }

        public static BnfiTermCollection<List<TElementType>, TElementType> StarList<TElementType>(this IBnfiTerm<TElementType> bnfTermElement, BnfTerm delimiter = null)
        {
            return BnfiTermCollection.StarList(bnfTermElement, delimiter);
        }

        public static BnfiTermCollection<TCollectionType, TElementType> PlusList<TCollectionType, TElementType>(this IBnfiTerm<TElementType> bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return BnfiTermCollection.PlusList<TCollectionType, TElementType>(bnfTermElement, delimiter);
        }

        public static BnfiTermCollection<List<TElementType>, TElementType> PlusList<TElementType>(this IBnfiTerm<TElementType> bnfTermElement, BnfTerm delimiter = null)
        {
            return BnfiTermCollection.PlusList(bnfTermElement, delimiter);
        }

        #endregion

        #region Typeless converted to typesafe (TCollectionType, TElementType)

        public static BnfiTermCollection<TCollectionType, TElementType> StarList<TCollectionType, TElementType>(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return BnfiTermCollection.StarList<TCollectionType, TElementType>(bnfTermElement, delimiter);
        }

        public static BnfiTermCollection<List<TElementType>, TElementType> StarList<TElementType>(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return BnfiTermCollection.StarList<TElementType>(bnfTermElement, delimiter);
        }

        public static BnfiTermCollection<TCollectionType, TElementType> PlusList<TCollectionType, TElementType>(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return BnfiTermCollection.PlusList<TCollectionType, TElementType>(bnfTermElement, delimiter);
        }

        public static BnfiTermCollection<List<TElementType>, TElementType> PlusList<TElementType>(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return BnfiTermCollection.PlusList<TElementType>(bnfTermElement, delimiter);
        }

        #endregion

        #region Typeless converted to semi-typesafe (TCollectionType)

        public static BnfiTermCollection<TCollectionType> StarListST<TCollectionType>(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<object>, new()
        {
            return BnfiTermCollection.StarListST<TCollectionType>(bnfTermElement, delimiter);
        }

        public static BnfiTermCollection<List<object>> StarListST(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return BnfiTermCollection.StarListST(bnfTermElement, delimiter);
        }

        public static BnfiTermCollection<TCollectionType> PlusListST<TCollectionType>(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<object>, new()
        {
            return BnfiTermCollection.PlusListST<TCollectionType>(bnfTermElement, delimiter);
        }

        public static BnfiTermCollection<List<object>> PlusListST(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return BnfiTermCollection.PlusListST(bnfTermElement, delimiter);
        }

        #endregion

        #region Typeless

        public static BnfiTermCollection StarListTL(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return BnfiTermCollection.StarListTL(bnfTermElement, delimiter);
        }

        public static BnfiTermCollection PlusListTL(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return BnfiTermCollection.PlusListTL(bnfTermElement, delimiter);
        }

        #endregion

        #endregion

        #region BindMember

        public static BnfiTermMember BindMember<TBnfTermType, TMemberType>(this IBnfiTerm<TBnfTermType> bnfTerm, Expression<Func<TMemberType>> exprForFieldOrPropertyAccess)
            where TBnfTermType : TMemberType
        {
            return BnfiTermMember.Bind(exprForFieldOrPropertyAccess, bnfTerm);
        }

        public static BnfiTermMember<TDeclaringType> BindMember<TDeclaringType, TBnfTermType, TMemberType>(this IBnfiTerm<TBnfTermType> bnfTerm,
            Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess)
            where TBnfTermType : TMemberType
        {
            return BnfiTermMember.Bind<TDeclaringType, TMemberType, TBnfTermType>(exprForFieldOrPropertyAccess, bnfTerm);
        }

        public static BnfiTermMember<TDeclaringType> BindMember<TDeclaringType, TBnfTermType, TMemberType>(this IBnfiTerm<TBnfTermType> bnfTerm,
            IBnfiTerm<TDeclaringType> dummyBnfTerm, Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess)
            where TBnfTermType : TMemberType
        {
            return BnfiTermMember.Bind<TDeclaringType, TMemberType, TBnfTermType>(dummyBnfTerm, exprForFieldOrPropertyAccess, bnfTerm);
        }

        public static BnfiTermMember BindMember(this BnfTerm bnfTerm, PropertyInfo propertyInfo)
        {
            return BnfiTermMember.Bind(propertyInfo, bnfTerm);
        }

        public static BnfiTermMember BindMember(this BnfTerm bnfTerm, FieldInfo fieldInfo)
        {
            return BnfiTermMember.Bind(fieldInfo, bnfTerm);
        }

        public static BnfiTermMember BindToNone(BnfTerm bnfTerm)
        {
            return BnfiTermMember.BindToNone(bnfTerm);
        }

        #endregion

        #region Create/ConvertValue

        public static BnfiTermValue CreateValue(this Terminal terminal, object value)
        {
            return BnfiTermValue.Create(terminal, value);
        }

        public static BnfiTermValue CreateValue(this Terminal terminal, AstValueCreator<object> astValueCreator)
        {
            return BnfiTermValue.Create(terminal, astValueCreator);
        }

        public static BnfiTermValue<TOut> CreateValue<TOut>(this Terminal terminal, TOut value)
        {
            return BnfiTermValue.Create(terminal, value);
        }

        public static BnfiTermValue<TOut> CreateValue<TOut>(this Terminal terminal, AstValueCreator<TOut> astValueCreator)
        {
            return BnfiTermValue.Create(terminal, astValueCreator);
        }

        public static BnfiTermValue<TOut> ConvertValue<TIn, TOut>(this IBnfiTerm<TIn> bnfTerm, ValueConverter<TIn, TOut> valueConverter)
        {
            return BnfiTermValue.Convert(bnfTerm, valueConverter);
        }

        #endregion

        #region Cast

        public static BnfiTermValue<TOut> Cast<TIn, TOut>(this IBnfiTerm<TIn> bnfTerm)
        {
            return BnfiTermValue.Cast<TIn, TOut>(bnfTerm);
        }

        public static BnfiTermValue<TOut> Cast<TOut>(this Terminal terminal)
        {
            return BnfiTermValue.Cast<TOut>(terminal);
        }

        public static BnfiTermValue<TOut> Cast<TIn, TOut>(this IBnfiTerm<TIn> bnfTerm, IBnfiTerm<TOut> dummyBnfTerm)
        {
            return BnfiTermValue.Cast<TIn, TOut>(bnfTerm);
        }

        #endregion

        #region Typesafe Q

        public static BnfiTermValue<T?> QVal<T>(this IBnfiTerm<T> bnfTerm)
            where T : struct
        {
            return BnfiTermValue.ConvertValueOptVal(bnfTerm);
        }

        public static BnfiTermValue<T> QRef<T>(this IBnfiTerm<T> bnfTerm)
            where T : class
        {
            return BnfiTermValue.ConvertValueOptRef(bnfTerm);
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

        public static MemberInfo GetMember<TDeclaringType, TMemberType>(Expression<Func<TDeclaringType, TMemberType>> exprForMemberAccess)
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

        #region Value <-> AstNode conversion

        public static object ValueToAstNode<T>(T value, AstContext context, ParseTreeNode parseTreeNode)
        {
            return ((Grammar)context.Language.Grammar).AutoBrowsableAstNodes && !(value is IBrowsableAstNode)
                ? AstNodeWrapper.Create(value, context, parseTreeNode)
                : value;
        }

        public static T AstNodeToValue<T>(object astNode)
        {
            AstNodeWrapper<T> astNodeWrapper = astNode as AstNodeWrapper<T>;

            if (astNodeWrapper == null && astNode.GetType().IsGenericType && astNode.GetType().GetGenericTypeDefinition() == typeof(AstNodeWrapper<>))
                throw new ArgumentException(
                    string.Format("AstNodeWrapper with the wrong generic type argument: {0} was found, but {1} was expected",
                        astNode.GetType().GenericTypeArguments[0].FullName, typeof(T).FullName),
                    "astNode"
                    );

            return astNodeWrapper != null ? astNodeWrapper.Value : (T)astNode;
        }

        #endregion

        #region Misc

        public static void ThrowGrammarError(GrammarErrorLevel grammarErrorLevel, string message, params object[] args)
        {
            if (args.Length > 0)
                message = string.Format(message, args);

            throw new GrammarErrorException(message, new GrammarError(grammarErrorLevel, null, message));
        }

        public static string TypeNameWithDeclaringTypes(Type type)
        {
            string typeName = type.Name.ToLower();

            if (type.IsGenericType)
            {
                typeName = string.Format("{0}<{1}>",
                    typeName.Remove(typeName.IndexOf('`')),
                    string.Join(",", type.GetGenericArguments().Select(genericArgumentType => TypeNameWithDeclaringTypes(genericArgumentType))));
            }

            if (type.IsNested)
                typeName = string.Format("{0}_{1}", TypeNameWithDeclaringTypes(type.DeclaringType), typeName);

            return typeName;
        }

        internal static IdentifierTerminal SetNodeCreator(this IdentifierTerminal identifierTerminal)
        {
            identifierTerminal.Flags |= TermFlags.NoAstNode;
            return identifierTerminal;
        }

        internal static NumberLiteral SetNodeCreator(this NumberLiteral identifierTerminal)
        {
            identifierTerminal.Flags |= TermFlags.NoAstNode;
            return identifierTerminal;
        }

        #endregion
    }
}
