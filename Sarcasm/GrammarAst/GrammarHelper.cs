#region License
/*
    This file is part of Sarcasm.

    Copyright 2012-2013 Dávid Németi

    Sarcasm is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Sarcasm is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Sarcasm.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Diagnostics;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm.Utility;
using Sarcasm.DomainCore;

namespace Sarcasm.GrammarAst
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

        #region BindTo

        // NOTE: the method's name is BindTo_ instead of BindTo to avoid ambiguous calls
        public static Member<TDeclaringType> BindTo_<TDeclaringType, TMemberType>(this BnfTerm bnfTerm, IBnfiTerm<TDeclaringType> dummyBnfiTerm,
            Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess)
        {
            return Member.Bind_(exprForFieldOrPropertyAccess, dummyBnfiTerm, bnfTerm);
        }

        public static MemberTL BindTo<TMemberType>(this IBnfiTermTL bnfiTerm, Expression<Func<TMemberType>> exprForFieldOrPropertyAccess)
        {
            return Member.Bind(exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static Member<TDeclaringType> BindTo<TDeclaringType, TMemberType>(this IBnfiTermTL bnfiTerm, IBnfiTerm<TDeclaringType> dummyBnfiTerm,
            Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess)
        {
            return Member.Bind(exprForFieldOrPropertyAccess, dummyBnfiTerm, bnfiTerm);
        }

        public static MemberTL BindTo<TMemberType, TValueType>(this IBnfiTerm<TValueType> bnfiTerm, Expression<Func<TMemberType>> exprForFieldOrPropertyAccess)
            where TValueType : TMemberType
        {
            return Member.Bind(exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static MemberTL BindTo<TMemberElementType, TValueElementType>(this IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm,
            Expression<Func<ICollection<TMemberElementType>>> exprForFieldOrPropertyAccess)
            where TValueElementType : TMemberElementType
        {
            return Member.Bind(exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static MemberTL BindTo<TMemberElementType, TValueElementType>(this IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm,
            Expression<Func<IList<TMemberElementType>>> exprForFieldOrPropertyAccess)
            where TValueElementType : TMemberElementType
        {
            return Member.Bind(exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static Member<TDeclaringType> BindTo<TDeclaringType, TMemberType, TValueType>(this IBnfiTerm<TValueType> bnfiTerm,
            Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess)
            where TValueType : TMemberType
        {
            return Member.Bind(exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static Member<TDeclaringType> BindTo<TDeclaringType, TMemberElementType, TValueElementType>(this IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm,
            Expression<Func<TDeclaringType, ICollection<TMemberElementType>>> exprForFieldOrPropertyAccess)
            where TValueElementType : TMemberElementType
        {
            return Member.Bind<TDeclaringType, TMemberElementType, TValueElementType>(exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static Member<TDeclaringType> BindTo<TDeclaringType, TMemberElementType, TValueElementType>(this IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm,
            Expression<Func<TDeclaringType, IList<TMemberElementType>>> exprForFieldOrPropertyAccess)
            where TValueElementType : TMemberElementType
        {
            return Member.Bind(exprForFieldOrPropertyAccess, bnfiTerm);
        }

        /*
         * NOTE: separate TMemberType and TValueType, and the constraint are necessary.
         * With only TMemberType (without TValueType) the following situation does not result in compile error, which is bad:
         * 
         * B.Expression.BindTo(B.LogicalBinaryBoolExpression, t => t.Term1)
         * 
         * where Term1 is a BoolExpression (which is BTW a derived class of Expression), so this could not be happened.
         * The opposite situation is fine:
         * 
         * B.BoolExpression.BindTo(B.BinaryExpression, t => t.Term1)
         * 
         * where Term1 is a Expression.
         * */
        public static Member<TDeclaringType> BindTo<TDeclaringType, TMemberType, TValueType>(this IBnfiTerm<TValueType> bnfiTerm,
            IBnfiTerm<TDeclaringType> dummyBnfiTerm, Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess)
            where TValueType : TMemberType
        {
            return Member.Bind(dummyBnfiTerm, exprForFieldOrPropertyAccess, bnfiTerm);
        }

        /*
         * NOTE: however, separate TMemberType and TValueType causes unwanted compile errors in a simple collection binding:
         * 
         * B.Statement.StarList().BindTo(B.If, t => t.Body)
         * 
         * So we should handle it separately.
         * */
        public static Member<TDeclaringType> BindTo<TDeclaringType, TMemberElementType, TValueElementType>(this IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm,
            IBnfiTerm<TDeclaringType> dummyBnfiTerm, Expression<Func<TDeclaringType, ICollection<TMemberElementType>>> exprForFieldOrPropertyAccess)
            where TValueElementType : TMemberElementType
        {
            return Member.Bind(dummyBnfiTerm, exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static Member<TDeclaringType> BindTo<TDeclaringType, TMemberElementType, TValueElementType>(this IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm,
            IBnfiTerm<TDeclaringType> dummyBnfiTerm, Expression<Func<TDeclaringType, IList<TMemberElementType>>> exprForFieldOrPropertyAccess)
            where TValueElementType : TMemberElementType
        {
            return Member.Bind(dummyBnfiTerm, exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static MemberTL BindTo(this BnfTerm bnfTerm, PropertyInfo propertyInfo)
        {
            return Member.Bind(propertyInfo, bnfTerm);
        }

        public static MemberTL BindTo(this BnfTerm bnfTerm, FieldInfo fieldInfo)
        {
            return Member.Bind(fieldInfo, bnfTerm);
        }

        public static BnfiTermNoAst NoAst(this KeyTerm keyTerm)
        {
            return BnfiTermNoAst.For(keyTerm);
        }

        public static BnfiTermNoAst NoAst_(this BnfTerm bnfTerm, ValueCreatorFromNoAst<object> valueCreatorFromNoAst)
        {
            return BnfiTermNoAst.For_(bnfTerm, valueCreatorFromNoAst);
        }

        [Obsolete(BnfiTermConversion.messageForMissingUnparseValueConverter, BnfiTermConversion.errorForMissingUnparseValueConverter)]
        public static BnfiTermNoAst NoAst_(this BnfTerm bnfTerm)
        {
            return BnfiTermNoAst.For_(bnfTerm);
        }

        public static BnfiTermNoAst NoAst(this BnfiTermKeyTerm bnfiTermKeyTerm)
        {
            return BnfiTermNoAst.For(bnfiTermKeyTerm);
        }

        public static BnfiTermNoAst NoAst<T>(this IBnfiTerm<T> bnfTerm, ValueCreatorFromNoAst<T> valueCreatorFromNoAst)
        {
            return BnfiTermNoAst.For(bnfTerm, valueCreatorFromNoAst);
        }

        [Obsolete(BnfiTermConversion.messageForMissingUnparseValueConverter, BnfiTermConversion.errorForMissingUnparseValueConverter)]
        public static BnfiTermNoAst NoAst<T>(this IBnfiTerm<T> bnfTerm)
        {
            return BnfiTermNoAst.For(bnfTerm);
        }

        #endregion

        #region Intro/ConvertValue

        public static BnfiTermConversionTL IntroValue(this Terminal terminal, object value, bool astForChild = true)
        {
            return BnfiTermConversion.Intro(terminal, value, astForChild);
        }

        [Obsolete(BnfiTermConversion.messageForMissingUnparseValueConverter, BnfiTermConversion.errorForMissingUnparseValueConverter)]
        public static BnfiTermConversionTL IntroValue(this Terminal terminal, ValueIntroducer<object> valueIntroducer, bool astForChild = true)
        {
            return BnfiTermConversion.Intro(terminal, valueIntroducer, astForChild);
        }

        public static BnfiTermConversionTL IntroValue(this Terminal terminal, ValueIntroducer<object> valueIntroducer, ValueConverter<object, object> inverseValueConverterForUnparse, bool astForChild = true)
        {
            return BnfiTermConversion.Intro(terminal, valueIntroducer, inverseValueConverterForUnparse, astForChild);
        }

        public static BnfiTermConversion<TNumberLiteral> IntroNumberLiteral<TNumberLiteral>(this NumberLiteral numberLiteral, NumberLiteralInfo numberLiteralInfo)
            where TNumberLiteral : INumberLiteral, new()
        {
            return BnfiTermConversion.IntroNumberLiteral<TNumberLiteral>(numberLiteral, numberLiteralInfo);
        }

        public static BnfiTermConversion<string> IntroIdentifier(this IdentifierTerminal identifierTerminal)
        {
            return BnfiTermConversion.IntroIdentifier(identifierTerminal);
        }

        public static BnfiTermConversion<string> IntroStringLiteral(this StringLiteral stringLiteral)
        {
            return BnfiTermConversion.IntroStringLiteral(stringLiteral);
        }

        public static BnfiTermConversion<TDOut> IntroConstantTerminal<TDOut>(this ConstantTerminal constantTerminal)
        {
            return BnfiTermConversion.IntroConstantTerminal<TDOut>(constantTerminal);
        }

        [Obsolete(BnfiTermConversion.messageForIntroForBnfiTermConstant, error: true)]
        public static BnfiTermConversion<TDOut> IntroConstantTerminal<TDOut>(this BnfiTermConstant<TDOut> bnfiTermConstant)
        {
            return BnfiTermConversion.IntroConstantTerminal<TDOut>(bnfiTermConstant);
        }

        public static BnfiTermConversion<TDOut> IntroValue<TDOut>(this Terminal terminal, TDOut value, bool astForChild = true)
        {
            return BnfiTermConversion.Intro(terminal, value, astForChild);
        }

        [Obsolete(BnfiTermConversion.messageForMissingUnparseValueConverter, BnfiTermConversion.errorForMissingUnparseValueConverter)]
        public static BnfiTermConversion<TDOut> IntroValue<TDOut>(this Terminal terminal, ValueIntroducer<TDOut> valueIntroducer, bool astForChild = true)
        {
            return BnfiTermConversion.Intro(terminal, valueIntroducer, astForChild);
        }

        public static BnfiTermConversion<TDOut> IntroValue<TDOut>(this Terminal terminal, ValueIntroducer<TDOut> valueIntroducer,
            ValueConverter<TDOut, object> inverseValueConverterForUnparse, bool astForChild = true)
        {
            return BnfiTermConversion.Intro(terminal, valueIntroducer, inverseValueConverterForUnparse, astForChild);
        }

        [Obsolete(BnfiTermConversion.messageForMissingUnparseValueConverter, BnfiTermConversion.errorForMissingUnparseValueConverter)]
        public static BnfiTermConversion<TDOut> ConvertValue<TDIn, TDOut>(this IBnfiTerm<TDIn> bnfiTerm, ValueConverter<TDIn, TDOut> valueConverter)
        {
            return BnfiTermConversion.Convert(bnfiTerm, valueConverter);
        }

        public static BnfiTermConversion<TDOut> ConvertValue<TDIn, TDOut>(this IBnfiTerm<TDIn> bnfiTerm, ValueConverter<TDIn, TDOut> valueConverter,
            ValueConverter<TDOut, TDIn> inverseValueConverterForUnparse)
        {
            return BnfiTermConversion.Convert(bnfiTerm, valueConverter, inverseValueConverterForUnparse);
        }

        [Obsolete(BnfiTermConversion.messageForMissingUnparseValueConverter, BnfiTermConversion.errorForMissingUnparseValueConverter)]
        public static BnfiTermConversion<TDOut> ConvertValue<TDOut>(this IBnfiTermTL bnfiTerm, ValueConverter<object, TDOut> valueConverter)
        {
            return BnfiTermConversion.Convert(bnfiTerm, valueConverter);
        }

        public static BnfiTermConversion<TDOut> ConvertValue<TDOut>(this IBnfiTermTL bnfiTerm, ValueConverter<object, TDOut> valueConverter, ValueConverter<TDOut, object> inverseValueConverterForUnparse)
        {
            return BnfiTermConversion.Convert(bnfiTerm, valueConverter, inverseValueConverterForUnparse);
        }

        [Obsolete(BnfiTermConversion.messageForMissingUnparseValueConverter, BnfiTermConversion.errorForMissingUnparseValueConverter)]
        public static BnfiTermConversionTL ConvertValue(this IBnfiTermTL bnfiTerm, ValueConverter<object, object> valueConverter)
        {
            return BnfiTermConversion.Convert(bnfiTerm, valueConverter);
        }

        public static BnfiTermConversionTL ConvertValue(this IBnfiTermTL bnfiTerm, ValueConverter<object, object> valueConverter, ValueConverter<object, object> inverseValueConverterForUnparse)
        {
            return BnfiTermConversion.Convert(bnfiTerm, valueConverter, inverseValueConverterForUnparse);
        }

        #endregion

        #region Cast

        public static BnfiTermConversion<TDOut> Cast<TDIn, TDOut>(this IBnfiTerm<TDIn> bnfTerm)
        {
            return BnfiTermConversion.Cast<TDIn, TDOut>(bnfTerm);
        }

        public static BnfiTermConversion<TDOut> Cast<TDOut>(this Terminal terminal)
        {
            return BnfiTermConversion.Cast<TDOut>(terminal);
        }

        public static BnfiTermConversion<TDOut> Cast<TDIn, TDOut>(this IBnfiTerm<TDIn> bnfTerm, IBnfiTerm<TDOut> dummyBnfTerm)
        {
            return BnfiTermConversion.Cast<TDIn, TDOut>(bnfTerm);
        }

        #endregion

        #region Copy

        public static BnfiTermCopyTL Copy(this IBnfiTermCopyableTL bnfiTerm)
        {
            return BnfiTermCopy.Copy(bnfiTerm);
        }

        public static BnfiTermCopy<TDerived> Copy<TBase, TDerived>(this IBnfiTermCopyable<TBase> bnfiTerm, IBnfiTerm<TDerived> dummyBnfiTerm)
            where TDerived : TBase
        {
            return BnfiTermCopy.Copy(bnfiTerm, dummyBnfiTerm);
        }

        #endregion

        #region Typesafe Q

        public static BnfiTermConversion<T?> QVal<T>(this IBnfiTerm<T> bnfTerm)
            where T : struct
        {
            return BnfiTermConversion.ConvertOptVal(bnfTerm);
        }

        public static BnfiTermConversion<T> QRef<T>(this IBnfiTerm<T> bnfTerm)
            where T : class
        {
            return BnfiTermConversion.ConvertOptRef(bnfTerm);
        }

        public static BnfiTermConversion<T> QVal<T>(this IBnfiTerm<T> bnfTerm, T defaultValue)
            where T : struct
        {
            return BnfiTermConversion.ConvertOptVal(bnfTerm, defaultValue);
        }

        public static BnfiTermConversion<T> QRef<T>(this IBnfiTerm<T> bnfTerm, T defaultValue)
            where T : class
        {
            return BnfiTermConversion.ConvertOptRef(bnfTerm, defaultValue);
        }

        #endregion

        #region Value <-> AstNode conversion

        public static object ValueToAstNode(object astValue, AstContext context, ParseTreeNode parseTreeNode)
        {
            return ((Grammar)context.Language.Grammar).AstCreation == AstCreation.CreateAstWithAutoBrowsableAstNodes && !(astValue is IBrowsableAstNode)
                ? new AstNodeWrapper(astValue, context, parseTreeNode)
                : astValue;
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

        public static bool IsOperator(this BnfTerm bnfTerm)
        {
            return bnfTerm.Flags.IsSet(TermFlags.IsOperator);
        }

        public static bool IsConstant(this BnfTerm bnfTerm)
        {
            return bnfTerm.Flags.IsSet(TermFlags.IsConstant);
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

        public static bool IsLiteral(this BnfTerm bnfTerm)
        {
            return bnfTerm.Flags.IsSet(TermFlags.IsLiteral);
        }

        internal static TBnfTerm MarkLiteral<TBnfTerm>(this TBnfTerm bnfTerm)
            where TBnfTerm : BnfTerm
        {
            bnfTerm.SetFlag(TermFlags.IsLiteral);
            return bnfTerm;
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
        /// When creating a Member we should not use MarkTransient, because under this Member there can be a list (TermFlags.IsList),
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
                catch (InvalidOperationException)
                {
                    // throw exception only if this exception cannot be the consequence of another ast error
                    if (!GrammarHelper.HasError(context))
                        throw new ArgumentException(string.Format("Only one child with astnode is allowed for a forced transient node: {0}", parseTreeNode.Term.Name), "nonTerminal");
                }
            };

            nonTerminal.SetFlag(TermFlags.InheritPrecedence);
        }

        internal static bool HasError(AstContext context)
        {
            return context.Messages.Any(message => message.Level.EqualToAny(ErrorLevel.Error, ErrorLevel.Warning));
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
        public static void ThrowGrammarErrorException(GrammarErrorLevel grammarErrorLevel, string format, params object[] args)
        {
            ThrowGrammarErrorException(grammarErrorLevel, string.Format(format, args));
        }

        [DebuggerStepThrough()]
        public static void ThrowGrammarErrorException(GrammarErrorLevel grammarErrorLevel, string message)
        {
            throw new GrammarErrorException(message, new GrammarError(grammarErrorLevel, null, message));
        }

        public static string TypeNameWithDeclaringTypes(Type domainType)
        {
            string typeName = domainType.Name.ToLower();

            if (domainType.IsGenericType)
            {
                typeName = string.Format("{0}<{1}>",
                    typeName.Remove(typeName.IndexOf('`')),
                    string.Join(",", domainType.GetGenericArguments().Select(genericArgumentType => TypeNameWithDeclaringTypes(genericArgumentType))));
            }

            if (domainType.IsNested)
                typeName = string.Format("{0}_{1}", TypeNameWithDeclaringTypes(domainType.DeclaringType), typeName);

            return typeName;
        }

        internal static IEnumerable<BnfTerm> GetDescendantBnfTermsExcludingSelf(NonTerminal nonTerminal)
        {
            return GetDescendantBnfTermsExcludingSelf(nonTerminal, new HashSet<BnfTerm>());
        }

        internal static IEnumerable<BnfTerm> GetDescendantBnfTermsExcludingSelf(NonTerminal nonTerminal, ISet<BnfTerm> visitedBnfTerms)
        {
            visitedBnfTerms.Add(nonTerminal);

            return nonTerminal.Rule.Data
                .SelectMany(children => children)
                .Where(child => !visitedBnfTerms.Contains(child))
                .SelectMany(
                    child =>
                    {
                        if (child is Terminal)
                        {
                            visitedBnfTerms.Add(child);
                            return new[] { child };
                        }
                        else if (child is NonTerminal)
                            return Util.Concat(child, GetDescendantBnfTermsExcludingSelf((NonTerminal)child, visitedBnfTerms));
                        else
                            return Enumerable.Empty<BnfTerm>();
                    }
                    );
        }

        internal static CommentKind GetCommentKind(CommentTerminal commentTerminal, params string[] newLines)
        {
            return commentTerminal.EndSymbols.All(endSymbol => !endSymbol.EqualToAny(newLines))
                ? CommentKind.Delimited
                : CommentKind.SingleLine;
        }

        #endregion
    }
}
