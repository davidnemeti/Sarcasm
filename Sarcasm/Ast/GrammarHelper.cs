using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.IO;
using System.Diagnostics;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Sarcasm.Ast
{
    public static partial class GrammarHelper
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

        #region Typeless

        public static BnfiTermCollectionTL StarListTL(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return BnfiTermCollection.StarListTL(bnfTermElement, delimiter);
        }

        public static BnfiTermCollectionTL PlusListTL(this BnfTerm bnfTermElement, BnfTerm delimiter = null)
        {
            return BnfiTermCollection.PlusListTL(bnfTermElement, delimiter);
        }

        public static BnfiTermCollectionTL StarListTL(this BnfTerm bnfTermElement, Type elementType, BnfTerm delimiter = null)
        {
            return BnfiTermCollection.StarListTL(elementType, bnfTermElement, delimiter);
        }

        public static BnfiTermCollectionTL PlusListTL(this BnfTerm bnfTermElement, Type elementType, BnfTerm delimiter = null)
        {
            return BnfiTermCollection.PlusListTL(elementType, bnfTermElement, delimiter);
        }

        #endregion

        #endregion

        #region BindMember

        // NOTE: the method's name is BindMember_ instead of BindMember to avoid ambiguous calls
        public static BnfiTermMember<TDeclaringType> BindMember_<TDeclaringType, TMemberType>(this BnfTerm bnfTerm, IBnfiTerm<TDeclaringType> dummyBnfiTerm,
            Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess)
        {
            return BnfiTermMember.Bind_(exprForFieldOrPropertyAccess, dummyBnfiTerm, bnfTerm);
        }

        public static BnfiTermMember<TDeclaringType> BindMember<TDeclaringType, TMemberType>(this IBnfiTermTL bnfiTerm, IBnfiTerm<TDeclaringType> dummyBnfiTerm,
            Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess)
        {
            return BnfiTermMember.Bind(exprForFieldOrPropertyAccess, dummyBnfiTerm, bnfiTerm);
        }

        public static BnfiTermMemberTL BindMember<TMemberType, TValueType>(this IBnfiTerm<TValueType> bnfiTerm, Expression<Func<TMemberType>> exprForFieldOrPropertyAccess)
            where TValueType : TMemberType
        {
            return BnfiTermMember.Bind(exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static BnfiTermMemberTL BindMember<TMemberElementType, TValueElementType>(this IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm,
            Expression<Func<ICollection<TMemberElementType>>> exprForFieldOrPropertyAccess)
            where TValueElementType : TMemberElementType
        {
            return BnfiTermMember.Bind(exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static BnfiTermMemberTL BindMember<TMemberElementType, TValueElementType>(this IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm,
            Expression<Func<IList<TMemberElementType>>> exprForFieldOrPropertyAccess)
            where TValueElementType : TMemberElementType
        {
            return BnfiTermMember.Bind(exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static BnfiTermMember<TDeclaringType> BindMember<TDeclaringType, TMemberType, TValueType>(this IBnfiTerm<TValueType> bnfiTerm,
            Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess)
            where TValueType : TMemberType
        {
            return BnfiTermMember.Bind(exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static BnfiTermMember<TDeclaringType> BindMember<TDeclaringType, TMemberElementType, TValueElementType>(this IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm,
            Expression<Func<TDeclaringType, ICollection<TMemberElementType>>> exprForFieldOrPropertyAccess)
            where TValueElementType : TMemberElementType
        {
            return BnfiTermMember.Bind<TDeclaringType, TMemberElementType, TValueElementType>(exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static BnfiTermMember<TDeclaringType> BindMember<TDeclaringType, TMemberElementType, TValueElementType>(this IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm,
            Expression<Func<TDeclaringType, IList<TMemberElementType>>> exprForFieldOrPropertyAccess)
            where TValueElementType : TMemberElementType
        {
            return BnfiTermMember.Bind(exprForFieldOrPropertyAccess, bnfiTerm);
        }

        /*
         * NOTE: separate TMemberType and TValueType, and the constraint are necessary.
         * With only TMemberType (without TValueType) the following situation does not result in compile error, which is bad:
         * 
         * B.Expression.BindMember(B.LogicalBinaryBoolExpression, t => t.Term1)
         * 
         * where Term1 is a BoolExpression (which is BTW a derived class of Expression), so this could not be happened.
         * The opposite situation is fine:
         * 
         * B.BoolExpression.BindMember(B.BinaryExpression, t => t.Term1)
         * 
         * where Term1 is a Expression.
         * */
        public static BnfiTermMember<TDeclaringType> BindMember<TDeclaringType, TMemberType, TValueType>(this IBnfiTerm<TValueType> bnfiTerm,
            IBnfiTerm<TDeclaringType> dummyBnfiTerm, Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess)
            where TValueType : TMemberType
        {
            return BnfiTermMember.Bind(dummyBnfiTerm, exprForFieldOrPropertyAccess, bnfiTerm);
        }

        /*
         * NOTE: however, separate TMemberType and TValueType causes unwanted compile errors in a simple collection binding:
         * 
         * B.Statement.StarList().BindMember(B.If, t => t.Body)
         * 
         * So we should handle it separately.
         * */
        public static BnfiTermMember<TDeclaringType> BindMember<TDeclaringType, TMemberElementType, TValueElementType>(this IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm,
            IBnfiTerm<TDeclaringType> dummyBnfiTerm, Expression<Func<TDeclaringType, ICollection<TMemberElementType>>> exprForFieldOrPropertyAccess)
            where TValueElementType : TMemberElementType
        {
            return BnfiTermMember.Bind(dummyBnfiTerm, exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static BnfiTermMember<TDeclaringType> BindMember<TDeclaringType, TMemberElementType, TValueElementType>(this IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm,
            IBnfiTerm<TDeclaringType> dummyBnfiTerm, Expression<Func<TDeclaringType, IList<TMemberElementType>>> exprForFieldOrPropertyAccess)
            where TValueElementType : TMemberElementType
        {
            return BnfiTermMember.Bind(dummyBnfiTerm, exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static BnfiTermMemberTL BindMember(this BnfTerm bnfTerm, PropertyInfo propertyInfo)
        {
            return BnfiTermMember.Bind(propertyInfo, bnfTerm);
        }

        public static BnfiTermMemberTL BindMember(this BnfTerm bnfTerm, FieldInfo fieldInfo)
        {
            return BnfiTermMember.Bind(fieldInfo, bnfTerm);
        }

        public static BnfiTermNoAst NoAst(this BnfTerm bnfTerm)
        {
            return BnfiTermNoAst.Create(bnfTerm);
        }

        #endregion

        #region Parse/ConvertValue

        public static BnfiTermValueTL ParseValue(this Terminal terminal, object value, bool astForChild = true)
        {
            return BnfiTermValue.Parse(terminal, value, astForChild);
        }

        [Obsolete(BnfiTermValue.messageForMissingUnparseValueConverter, BnfiTermValue.errorForMissingUnparseValueConverter)]
        public static BnfiTermValueTL ParseValue(this Terminal terminal, ValueParser<object> valueParser, bool astForChild = true)
        {
            return BnfiTermValue.Parse(terminal, valueParser, astForChild);
        }

        public static BnfiTermValueTL ParseValue(this Terminal terminal, ValueParser<object> valueParser, ValueConverter<object, object> inverseValueConverterForUnparse, bool astForChild = true)
        {
            return BnfiTermValue.Parse(terminal, valueParser, inverseValueConverterForUnparse, astForChild);
        }

        public static BnfiTermValue<string> ParseIdentifier(this IdentifierTerminal identifierTerminal)
        {
            return BnfiTermValue.ParseIdentifier(identifierTerminal);
        }

        public static BnfiTermValue<string> ParseStringLiteral(this StringLiteral stringLiteral)
        {
            return BnfiTermValue.ParseStringLiteral(stringLiteral);
        }

        public static BnfiTermValue<TOut> ParseValue<TOut>(this Terminal terminal, TOut value, bool astForChild = true)
        {
            return BnfiTermValue.Parse(terminal, value, astForChild);
        }

        [Obsolete(BnfiTermValue.messageForMissingUnparseValueConverter, BnfiTermValue.errorForMissingUnparseValueConverter)]
        public static BnfiTermValue<TOut> ParseValue<TOut>(this Terminal terminal, ValueParser<TOut> valueParser, bool astForChild = true)
        {
            return BnfiTermValue.Parse(terminal, valueParser, astForChild);
        }

        public static BnfiTermValue<TOut> ParseValue<TOut>(this Terminal terminal, ValueParser<TOut> valueParser,
            ValueConverter<TOut, object> inverseValueConverterForUnparse, bool astForChild = true)
        {
            return BnfiTermValue.Parse(terminal, valueParser, inverseValueConverterForUnparse, astForChild);
        }

        [Obsolete(BnfiTermValue.messageForMissingUnparseValueConverter, BnfiTermValue.errorForMissingUnparseValueConverter)]
        public static BnfiTermValue<TOut> ConvertValue<TIn, TOut>(this IBnfiTerm<TIn> bnfiTerm, ValueConverter<TIn, TOut> valueConverter)
        {
            return BnfiTermValue.Convert(bnfiTerm, valueConverter);
        }

        public static BnfiTermValue<TOut> ConvertValue<TIn, TOut>(this IBnfiTerm<TIn> bnfiTerm, ValueConverter<TIn, TOut> valueConverter,
            ValueConverter<TOut, TIn> inverseValueConverterForUnparse)
        {
            return BnfiTermValue.Convert(bnfiTerm, valueConverter, inverseValueConverterForUnparse);
        }

        [Obsolete(BnfiTermValue.messageForMissingUnparseValueConverter, BnfiTermValue.errorForMissingUnparseValueConverter)]
        public static BnfiTermValue<TOut> ConvertValue<TOut>(this IBnfiTermTL bnfiTerm, ValueConverter<object, TOut> valueConverter)
        {
            return BnfiTermValue.Convert(bnfiTerm, valueConverter);
        }

        public static BnfiTermValue<TOut> ConvertValue<TOut>(this IBnfiTermTL bnfiTerm, ValueConverter<object, TOut> valueConverter, ValueConverter<TOut, object> inverseValueConverterForUnparse)
        {
            return BnfiTermValue.Convert(bnfiTerm, valueConverter, inverseValueConverterForUnparse);
        }

        [Obsolete(BnfiTermValue.messageForMissingUnparseValueConverter, BnfiTermValue.errorForMissingUnparseValueConverter)]
        public static BnfiTermValueTL ConvertValue(this IBnfiTermTL bnfiTerm, ValueConverter<object, object> valueConverter)
        {
            return BnfiTermValue.Convert(bnfiTerm, valueConverter);
        }

        public static BnfiTermValueTL ConvertValue(this IBnfiTermTL bnfiTerm, ValueConverter<object, object> valueConverter, ValueConverter<object, object> inverseValueConverterForUnparse)
        {
            return BnfiTermValue.Convert(bnfiTerm, valueConverter, inverseValueConverterForUnparse);
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

        #region Copy

        public static BnfiTermCopy Copy(this IBnfiTermCopyable bnfiTerm)
        {
            return BnfiTermCopy.Copy(bnfiTerm);
        }

        public static BnfiTermCopy<T> Copy<T>(this IBnfiTermCopyable<T> bnfiTerm)
        {
            return BnfiTermCopy.Copy<T>(bnfiTerm);
        }

        #endregion

        #region Typesafe Q

        public static BnfiTermValue<T?> QVal<T>(this IBnfiTerm<T> bnfTerm)
            where T : struct
        {
            return BnfiTermValue.ConvertOptVal(bnfTerm);
        }

        public static BnfiTermValue<T> QRef<T>(this IBnfiTerm<T> bnfTerm)
            where T : class
        {
            return BnfiTermValue.ConvertOptRef(bnfTerm);
        }

        public static BnfiTermValue<T> QVal<T>(this IBnfiTerm<T> bnfTerm, T defaultValue)
            where T : struct
        {
            return BnfiTermValue.ConvertOptVal(bnfTerm, defaultValue);
        }

        public static BnfiTermValue<T> QRef<T>(this IBnfiTerm<T> bnfTerm, T defaultValue)
            where T : class
        {
            return BnfiTermValue.ConvertOptRef(bnfTerm, defaultValue);
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
            return ((Grammar)context.Language.Grammar).AstCreation == AstCreation.CreateAstWithAutoBrowsableAstNodes && !(value is IBrowsableAstNode)
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
            return (T)AstNodeToValue(astNode);
        }

        #endregion

        #region Misc

        public static bool IsReferable(this BnfTerm bnfTerm)
        {
            if (bnfTerm is BnfiTermNonTerminal)
                return ((BnfiTermNonTerminal)bnfTerm).IsReferable;
            else if (bnfTerm is BnfiTermMember)
                return false;
            else
                return true;
        }

        public static bool IsOperator(this BnfTerm bnfTerm)
        {
            return bnfTerm.Flags.IsSet(TermFlags.IsOperator);
        }

        public static bool IsBrace(this BnfTerm bnfTerm)
        {
            return bnfTerm.Flags.IsSet(TermFlags.IsBrace);
        }

        public static bool IsOpenBrace(this BnfTerm bnfTerm)
        {
            return bnfTerm.Flags.IsSet(TermFlags.IsOpenBrace);
        }

        public static bool IsCloseBrace(this BnfTerm bnfTerm)
        {
            return bnfTerm.Flags.IsSet(TermFlags.IsCloseBrace);
        }

        public static bool IsPunctuation(this BnfTerm bnfTerm)
        {
            return bnfTerm.Flags.IsSet(TermFlags.IsPunctuation);
        }

        internal static void MarkTransient(NonTerminal nonTerminal)
        {
            nonTerminal.SetFlag(TermFlags.IsTransient | TermFlags.NoAstNode);
        }

        /// <summary>
        /// Practically the same as marking with MarkTransient. It is used in those cases when MarkTransient would not work due to technical issues,
        /// or when there are multiple children and only one of them has ast node.
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
                try
                {
                    parseTreeNode.AstNode = parseTreeNode.ChildNodes.Single(childNode => childNode.AstNode != null).AstNode;
                }
                catch (InvalidOperationException e)
                {
                    throw new ArgumentException(string.Format("Only one child with astnode is allowed for a forced transient node: {0}", parseTreeNode.Term.Name), "nonTerminal", e);
                }
            };
        }

        internal static GrammarHint PreferShiftHere()
        {
            return new PreferredActionHint(PreferredActionType.Shift);
        }

        internal static GrammarHint ReduceHere()
        {
            return new PreferredActionHint(PreferredActionType.Reduce);
        }

        internal static TokenPreviewHint ReduceIf(string thisSymbol, params string[] comesBefore)
        {
            return new TokenPreviewHint(PreferredActionType.Reduce, thisSymbol, comesBefore);
        }

        internal static TokenPreviewHint ReduceIf(Terminal thisSymbol, params Terminal[] comesBefore)
        {
            return new TokenPreviewHint(PreferredActionType.Reduce, thisSymbol, comesBefore);
        }

        internal static TokenPreviewHint ShiftIf(string thisSymbol, params string[] comesBefore)
        {
            return new TokenPreviewHint(PreferredActionType.Shift, thisSymbol, comesBefore);
        }

        internal static TokenPreviewHint ShiftIf(Terminal thisSymbol, params Terminal[] comesBefore)
        {
            return new TokenPreviewHint(PreferredActionType.Shift, thisSymbol, comesBefore);
        }

        internal static GrammarHint ImplyPrecedenceHere(int precedence)
        {
            return ImplyPrecedenceHere(precedence, Associativity.Left);
        }

        internal static GrammarHint ImplyPrecedenceHere(int precedence, Associativity associativity)
        {
            return new ImpliedPrecedenceHint(precedence, associativity);
        }

        internal static CustomActionHint CustomActionHere(ExecuteActionMethod executeMethod, PreviewActionMethod previewMethod = null)
        {
            return new CustomActionHint(executeMethod, previewMethod);
        }

        [DebuggerStepThrough()]
        public static void GrammarError(AstContext context, SourceLocation location, ErrorLevel errorLevel, string format, params object[] args)
        {
            GrammarError(context, location, errorLevel, string.Format(format, args));
        }

        [DebuggerStepThrough()]
        public static void GrammarError(AstContext context, SourceLocation location, ErrorLevel errorLevel, string message)
        {
            context.AddMessage(errorLevel, location, message);

            if (((Grammar)context.Language.Grammar).ErrorHandling == ErrorHandling.ThrowException)
            {
                GrammarErrorLevel grammarErrorLevel;
                switch (errorLevel)
                {
                    case ErrorLevel.Error:
                        grammarErrorLevel = GrammarErrorLevel.Error;
                        break;

                    case ErrorLevel.Warning:
                        grammarErrorLevel = GrammarErrorLevel.Warning;
                        break;

                    case ErrorLevel.Info:
                        grammarErrorLevel = GrammarErrorLevel.Info;
                        break;

                    default:
                        throw new ArgumentException(string.Format("Unknown errorLevel: {0}", errorLevel), "errorLevel");
                }

                ThrowGrammarErrorException(grammarErrorLevel, message);
            }
        }

        [DebuggerStepThrough()]
        public static void ThrowGrammarErrorException(GrammarErrorLevel grammarErrorLevel, string format, params object[] args)
        {
            ThrowGrammarErrorException(grammarErrorLevel, string.Format(format, args));
        }

        [DebuggerStepThrough()]
        public static void ThrowGrammarErrorException(GrammarErrorLevel grammarErrorLevel, string message)
        {
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
