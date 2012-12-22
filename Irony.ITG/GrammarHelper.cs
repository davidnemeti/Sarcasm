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

        #region Typeless converted to semi-typesafe (TCollectionType, object)

        public static BnfiTermCollection<TCollectionType, object> StarListST<TCollectionType>(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<object>, new()
        {
            return BnfiTermCollection.StarListST<TCollectionType>(bnfTermElement, delimiter);
        }

        public static BnfiTermCollection<List<object>, object> StarListST(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return BnfiTermCollection.StarListST(bnfTermElement, delimiter);
        }

        public static BnfiTermCollection<TCollectionType, object> PlusListST<TCollectionType>(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
            where TCollectionType : ICollection<object>, new()
        {
            return BnfiTermCollection.PlusListST<TCollectionType>(bnfTermElement, delimiter);
        }

        public static BnfiTermCollection<List<object>, object> PlusListST(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
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

        public static BnfiTermMember BindMember<TBnfTermType, TMemberType>(this IBnfiTerm<TBnfTermType> bnfiTerm, Expression<Func<TMemberType>> exprForFieldOrPropertyAccess)
            where TBnfTermType : TMemberType
        {
            return BnfiTermMember.Bind(exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static BnfiTermMember<TDeclaringType> BindMember<TDeclaringType, TBnfTermType, TMemberType>(this IBnfiTerm<TBnfTermType> bnfiTerm,
            Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess)
            where TBnfTermType : TMemberType
        {
            return BnfiTermMember.Bind<TDeclaringType, TMemberType, TBnfTermType>(exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static BnfiTermMember<TDeclaringType> BindMember<TDeclaringType, TBnfTermType, TMemberType>(this IBnfiTerm<TBnfTermType> bnfiTerm,
            IBnfiTerm<TDeclaringType> dummyBnfiTerm, Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess)
            where TBnfTermType : TMemberType
        {
            return BnfiTermMember.Bind<TDeclaringType, TMemberType, TBnfTermType>(dummyBnfiTerm, exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static BnfiTermMember BindMember(this BnfTerm bnfTerm, PropertyInfo propertyInfo)
        {
            return BnfiTermMember.Bind(propertyInfo, bnfTerm);
        }

        public static BnfiTermMember BindMember(this BnfTerm bnfTerm, FieldInfo fieldInfo)
        {
            return BnfiTermMember.Bind(fieldInfo, bnfTerm);
        }

        public static BnfiTermMember BindToNone(this BnfTerm bnfTerm)
        {
            return BnfiTermMember.BindToNone(bnfTerm);
        }

        public static BnfiTermMember BindToNone<TBnfTermType>(this IBnfiTerm<TBnfTermType> bnfiTerm)
        {
            return BnfiTermMember.BindToNone(bnfiTerm);
        }

        public static BnfiTermMember<TDeclaringType> BindToNone<TDeclaringType, TBnfTermType>(this IBnfiTerm<TBnfTermType> bnfiTerm, IBnfiTerm<TDeclaringType> dummyBnfiTerm)
        {
            return BnfiTermMember.BindToNone(bnfiTerm, dummyBnfiTerm);
        }

        #endregion

        #region Create/ConvertValue

        public static BnfiTermValue CreateValue(this Terminal terminal, object value, bool astForChild = true)
        {
            return BnfiTermValue.Create(terminal, value, astForChild);
        }

        public static BnfiTermValue CreateValue(this Terminal terminal, AstValueCreator<object> astValueCreator, bool astForChild = true)
        {
            return BnfiTermValue.Create(terminal, astValueCreator, astForChild);
        }

        public static BnfiTermValue<string> CreateIdentifier(this IdentifierTerminal identifierTerminal)
        {
            return BnfiTermValue.CreateIdentifier(identifierTerminal);
        }

        public static BnfiTermValue<T> CreateNumber<T>(this NumberLiteral numberLiteral)
        {
            return BnfiTermValue.CreateNumber<T>(numberLiteral);
        }

        public static BnfiTermValue CreateNumber(this NumberLiteral numberLiteral)
        {
            return BnfiTermValue.CreateNumber(numberLiteral);
        }

        public static BnfiTermValue<TOut> CreateValue<TOut>(this Terminal terminal, TOut value, bool astForChild = true)
        {
            return BnfiTermValue.Create(terminal, value, astForChild);
        }

        public static BnfiTermValue<TOut> CreateValue<TOut>(this Terminal terminal, AstValueCreator<TOut> astValueCreator, bool astForChild = true)
        {
            return BnfiTermValue.Create(terminal, astValueCreator, astForChild);
        }

        public static BnfiTermValue<TOut> ConvertValue<TIn, TOut>(this IBnfiTerm<TIn> bnfiTerm, ValueConverter<TIn, TOut> valueConverter)
        {
            return BnfiTermValue.Convert(bnfiTerm, valueConverter);
        }

        public static BnfiTermValue<TOut> ConvertValue<TOut>(this IBnfiTerm bnfiTerm, ValueConverter<object, TOut> valueConverter)
        {
            return BnfiTermValue.Convert(bnfiTerm, valueConverter);
        }

        public static BnfiTermValue ConvertValue(this IBnfiTerm bnfiTerm, Type type, ValueConverter<object, object> valueConverter)
        {
            return BnfiTermValue.Convert(type, bnfiTerm, valueConverter);
        }

        public static BnfiTermValue ConvertValue(this IBnfiTerm bnfiTerm, ValueConverter<object, object> valueConverter)
        {
            return BnfiTermValue.Convert(bnfiTerm, valueConverter);
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

        public static object ValueToAstNode(object value, AstContext context, ParseTreeNode parseTreeNode)
        {
            return ((Grammar)context.Language.Grammar).AutoBrowsableAstNodes && !(value is IBrowsableAstNode)
                ? new AstNodeWrapper(value, context, parseTreeNode)
                : value;
        }

        public static object AstNodeToValue(object astNode)
        {
            AstNodeWrapper astNodeWrapper = astNode as AstNodeWrapper;
            return astNodeWrapper != null ? astNodeWrapper.Value : astNode;
        }

        public static T AstNodeToValue<T>(object astNode)
        {
            AstNodeWrapper astNodeWrapper = astNode as AstNodeWrapper;
            return astNodeWrapper != null ? (T)astNodeWrapper.Value : (T)astNode;
        }

        #endregion

        #region Misc

        internal static void MarkTransient(NonTerminal nonTerminal)
        {
            nonTerminal.Flags |= TermFlags.IsTransient | TermFlags.NoAstNode;
        }

        /// <summary>
        /// Practically the same as marking with MarkTransient. It is used in those cases when MarkTransient would not work due to technical issues.
        /// </summary>
        /// <example>
        /// When creating a BnfiTermMember we should not use MarkTransient, because under this BnfiTermMember there can be a list (TermFlags.IsList),
        /// which makes this term to become a list container (TermFlags.IsListContainer), and this causes ReduceParserActionCreate to process this term
        /// with ReduceListContainerParserAction instead of the desired ReduceTransientParserAction, which causes the parseTreeNode of this term
        /// to remain in the parseTree despite it is being transient (TermFlags.IsTransient), and this results bad behavior when building the AST tree,
        /// because this term will not produce an ast node (TermFlags.NoAstNode), and therefore the AST builder will not process its children.
        /// </example>
        internal static void MarkTransientForced(NonTerminal nonTerminal)
        {
            nonTerminal.AstConfig.NodeCreator = (context, parseTreeNode) =>
                {
                    if (parseTreeNode.ChildNodes.Count != 1)
                        throw new ArgumentException("Only one child is allowed for a forced transient node: {0}", parseTreeNode.Term.Name);

                    parseTreeNode.AstNode = parseTreeNode.ChildNodes[0].AstNode;
                };
        }

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

        #endregion
    }
}
