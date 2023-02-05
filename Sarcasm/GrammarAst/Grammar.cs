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
using System.Globalization;
using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm.DomainCore;
using Sarcasm.Unparsing;
using Sarcasm.Utility;

namespace Sarcasm.GrammarAst
{
    public enum AstCreation { NoAst, CreateAst, CreateAstWithAutoBrowsableAstNodes }
    public enum ErrorHandling { ThrowException, ErrorMessage }

    public interface ICommentCleaner
    {
        string[] GetCleanedUpCommentTextLines(string[] commentTextLines, int columnIndex, CommentTerminal commentTerminal, out bool isDecorated);
        string NewLine { get; }
    }

    public abstract partial class Grammar : Irony.Parsing.Grammar
    {
        #region Defaults

        private const AstCreation astCreationDefault = AstCreation.CreateAstWithAutoBrowsableAstNodes;
        private const ErrorHandling errorHandlingDefault = ErrorHandling.ThrowException;

        #endregion

        #region Construction

        public Grammar(Domain domain)
            : base()
        {
            this.domain = domain;

            Init();
        }

        public Grammar(Domain domain, bool caseSensitive)
            : base(caseSensitive)
        {
            this.domain = domain;

            Init();
        }

        void Init()
        {
            this.AstCreation = astCreationDefault;
            this.ErrorHandling = errorHandlingDefault;
            this.EmptyCollectionHandling = domain.EmptyCollectionHandling;
        }

        #endregion

        #region State

        private readonly Domain domain;

        #endregion

        #region Properties

        public AstCreation _astCreation;
        public AstCreation AstCreation
        {
            get { return _astCreation; }

            set
            {
                this._astCreation = value;

                this.LanguageFlags = value.EqualToAny(AstCreation.CreateAst, AstCreation.CreateAstWithAutoBrowsableAstNodes)
                    ? LanguageFlags.CreateAst
                    : LanguageFlags.Default;
            }
        }

        public ErrorHandling ErrorHandling { get; set; }
        public EmptyCollectionHandling EmptyCollectionHandling { get; private set; }

        #endregion

        #region AST construction

        public override void BuildAst(LanguageData language, ParseTree parseTree)
        {
            if (!LanguageFlags.IsSet(LanguageFlags.CreateAst))
                return;
            var astContext = new AstContext(language);
            var astBuilder = new AstBuilder(astContext);
            astBuilder.BuildAst(parseTree);
        }

        #endregion

        #region MakePlusRule and MakeStarRule

        public static BnfiTermCollectionTL MakePlusRule(BnfiTermCollectionTL bnfiTermCollection, BnfTerm delimiter, BnfTerm element)
        {
            return BnfiTermCollection.MakePlusRule(bnfiTermCollection, delimiter, element);
        }

        public static BnfiTermCollectionTL MakeStarRule(BnfiTermCollectionTL bnfiTermCollection, BnfTerm delimiter, BnfTerm element)
        {
            return BnfiTermCollection.MakeStarRule(bnfiTermCollection, delimiter, element);
        }

        public static BnfiTermCollection<TCollectionType, TElementType> MakePlusRule<TCollectionType, TElementType>(BnfiTermCollection<TCollectionType, TElementType> bnfiTermCollection, BnfTerm delimiter, BnfTerm element)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return BnfiTermCollection.MakePlusRule(bnfiTermCollection, delimiter, element);
        }

        public static BnfiTermCollection<TCollectionType, TElementType> MakeStarRule<TCollectionType, TElementType>(BnfiTermCollection<TCollectionType, TElementType> bnfiTermCollection, BnfTerm delimiter, BnfTerm element)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return BnfiTermCollection.MakeStarRule(bnfiTermCollection, delimiter, element);
        }

        public static BnfiTermCollection<TCollectionType, TElementType> MakePlusRule<TCollectionType, TElementType>(BnfiTermCollection<TCollectionType, TElementType> bnfiTermCollection, BnfTerm delimiter, IBnfiTerm<TElementType> element)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return BnfiTermCollection.MakePlusRule(bnfiTermCollection, delimiter, element);
        }

        public static BnfiTermCollection<TCollectionType, TElementType> MakeStarRule<TCollectionType, TElementType>(BnfiTermCollection<TCollectionType, TElementType> bnfiTermCollection, BnfTerm delimiter, IBnfiTerm<TElementType> element)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return BnfiTermCollection.MakeStarRule(bnfiTermCollection, delimiter, element);
        }

        #endregion

        #region KeyTerms

        public new BnfiTermKeyTerm ToTerm(string text)
        {
            return ToTerm(text, string.Format("\"{0}\"", text));
        }

        public new BnfiTermKeyTerm ToTerm(string text, string name)
        {
            #region Copied from Irony.Parsing.Grammar.ToTerm(string text, string name)

            KeyTerm term;
            if (KeyTerms.TryGetValue(text, out term))
            {
                //update name if it was specified now and not before
                if (string.IsNullOrEmpty(term.Name) && !string.IsNullOrEmpty(name))
                    term.Name = name;
                return (BnfiTermKeyTerm)term;
            }
            //create new term
            if (!CaseSensitive)
                text = text.ToLowerInvariant();
            string.Intern(text);
            term = new BnfiTermKeyTerm(text, name);
            KeyTerms[text] = term;
            return (BnfiTermKeyTerm)term;

            #endregion
        }

        public BnfiTermConversion<T> ToTerm<T>(string text, T value)
        {
            return ToTerm(text).IntroValue(value);
        }

        public BnfiTermConversionTL ToTermTL(string text, object value)
        {
            return ToTerm(text).IntroValue(value);
        }

        public static void RegisterBracePair(KeyTerm openBrace, KeyTerm closeBrace)
        {
            openBrace.SetFlag(TermFlags.IsOpenBrace);
            openBrace.IsPairFor = closeBrace;
            closeBrace.SetFlag(TermFlags.IsCloseBrace);
            closeBrace.IsPairFor = openBrace;
        }

        #endregion

        #region Empty

        private BnfiTermNoAst empty = null;
        public new BnfiTermNoAst Empty
        {
            get
            {
                if (empty == null)
                    empty = base.Empty.NoAst_(valueCreatorFromNoAst: null);

                return empty;
            }
        }

        #endregion

        #region Operators

        public new void RegisterOperators(int precedence, params BnfTerm[] operators)
        {
            _RegisterOperators(precedence, associativity: null, recurse: true, operators: operators);
        }

        public new void RegisterOperators(int precedence, Associativity associativity, params BnfTerm[] operators)
        {
            _RegisterOperators(precedence, associativity, recurse: true, operators: operators);
        }

        public void RegisterOperators(int precedence, Associativity associativity, bool recurse, params BnfTerm[] operators)
        {
            _RegisterOperators(precedence, associativity, recurse, operators: operators);
        }

        private void _RegisterOperators(int precedence, Associativity? associativity, bool recurse, params BnfTerm[] operators)
        {
            RegisterTerminalOperators(operators.OfType<Terminal>(), precedence, associativity, recurse);
            RegisterNonTerminalOperators(operators.OfType<NonTerminal>(), precedence, associativity, recurse);
        }

        private void RegisterTerminalOperators(IEnumerable<Terminal> terminalOperators, int precedence, Associativity? associativity, bool recurse)
        {
            foreach (Terminal terminalOperator in terminalOperators)
                RegisterTerminalOperator(terminalOperator, precedence, associativity, recurse);
        }

        private void RegisterTerminalOperator(Terminal terminalOperator, int precedence, Associativity? associativity, bool recurse)
        {
            if (terminalOperator.Precedence != BnfTerm.NoPrecedence)
                throw new InvalidOperationException(string.Format("Double call of RegisterOperators on terminal '{0}'", terminalOperator));

            BaseRegisterOperator(terminalOperator, precedence, associativity);
        }

        private void RegisterNonTerminalOperators(IEnumerable<NonTerminal> nonTerminalOperators, int precedence, Associativity? associativity, bool recurse)
        {
            foreach (NonTerminal nonTerminalOperator in nonTerminalOperators)
                RegisterNonTerminalOperator(nonTerminalOperator, precedence, associativity, recurse);
        }

        private void RegisterNonTerminalOperator(NonTerminal nonTerminalOperator, int precedence, Associativity? associativity, bool recurse)
        {
            if (nonTerminalOperator.Precedence != BnfTerm.NoPrecedence)
                throw new InvalidOperationException(string.Format("Double call of RegisterOperators on non-terminal '{0}'", nonTerminalOperator));

            if (nonTerminalOperator.Rule == null)
            {
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error,
                    "Rule is needed to have been set for nonterminal operator '{0}' before calling RegisterOperators", nonTerminalOperator);
            }

            if (recurse)
            {
                foreach (var bnfTerms in nonTerminalOperator.Rule.GetBnfTermsList())
                {
                    foreach (var bnfTerm in bnfTerms)
                    {
                        if (bnfTerm is Terminal)
                            RegisterTerminalOperator((Terminal)bnfTerm, precedence, associativity, recurse);
                        else if (bnfTerm is NonTerminal && !bnfTerm.Flags.IsSet(TermFlags.NoAstNode))
                            RegisterNonTerminalOperator((NonTerminal)bnfTerm, precedence, associativity, recurse);
                    }
                }

                nonTerminalOperator.SetFlag(TermFlags.InheritPrecedence);
            }
            else
                BaseRegisterOperator(nonTerminalOperator, precedence, associativity);
        }

        private void BaseRegisterOperator(BnfTerm bnfTerm, int precedence, Associativity? associativity)
        {
            if (associativity.HasValue)
                base.RegisterOperators(precedence, associativity.Value, bnfTerm);
            else
                base.RegisterOperators(precedence, bnfTerm);
        }

        #endregion

        #region Misc

        protected void IncludeGrammar(Irony.Parsing.Grammar grammarToInclude, bool includeNonGrammarTerminals = true)
        {
            foreach (var keyTerm in grammarToInclude.KeyTerms)
                this.KeyTerms.Add(keyTerm.Key, keyTerm.Value);

            if (includeNonGrammarTerminals)
            {
                foreach (Terminal nonGrammarTerminal in grammarToInclude.NonGrammarTerminals)
                    this.NonGrammarTerminals.Add(nonGrammarTerminal);
            }
        }

        public override void OnGrammarDataConstructed(LanguageData language)
        {
            SetDecimalSeparatorOnNumberLiterals();      // it works with Irony.Parser as well, not just with Sarcasm.MultiParser

            base.OnGrammarDataConstructed(language);
        }

        public new CultureInfo DefaultCulture
        {
            get { return base.DefaultCulture; }

            set
            {
                base.DefaultCulture = value;

                /*
                 * NOTE: If DefaultCulture is being set inside the Grammar's constructor then it might be set before any bnfTerms
                 * has been added to grammar, so this SetDecimalSeparator will be called in MultiParser too.
                 * 
                 * If DefaultCulture is being set outside the Grammar's constructor then it would be okay to call SetDecimalSeparator
                 * only once, but we cannot know it, so we call SetDecimalSeparator in MultiParser anyway.
                 * */
                SetDecimalSeparatorOnNumberLiterals();
            }
        }

        internal void SetDecimalSeparatorOnNumberLiterals()
        {
            // see DefaultCulture's setter for more information
            if (Root == null)
                return;

            char numberDecimalSeparator = DefaultCulture.NumberFormat.NumberDecimalSeparator[0];    // NOTE: Irony handles numberDecimalSeparator only as character

            foreach (NumberLiteral numberLiteral in GrammarHelper.GetDescendantBnfTermsExcludingSelf(Root).OfType<NumberLiteral>())
                numberLiteral.DecimalSeparator = numberDecimalSeparator;    // it seems this is the only way in Irony to set the DecimalSeparator which corresponds to DefaultCulture
        }

        public static new Grammar CurrentGrammar
        {
            get { return (Grammar)Irony.Parsing.Grammar.CurrentGrammar; }
        }

        public static readonly object GrammarCreationLock = new object();

        internal ICommentCleaner GetDefaultCommentCleaner()
        {
            return UnparseControl.DefaultFormatter;
        }

        #endregion

        #region Unparsing

        private UnparseControl _unparseControl;
        public UnparseControl UnparseControl
        {
            get
            {
                if (_unparseControl == null)
                    _unparseControl = GetUnparseControl();

                return _unparseControl;
            }
        }

        protected abstract UnparseControl GetUnparseControl();

        public static ValueConverter<object, object> NoUnparseByInverse()
        {
            return BnfiTermConversion.NoUnparseByInverse();
        }

        public static ValueConverter<T, object> NoUnparseByInverse<T>()
        {
            return BnfiTermConversion.NoUnparseByInverse<T>();
        }

        public static ValueConverter<TDIn, TDOut> NoUnparseByInverse<TDIn, TDOut>()
        {
            return BnfiTermConversion.NoUnparseByInverse<TDIn, TDOut>();
        }

        public static ValueCreatorFromNoAst<TD> NoUnparseByInverseCreatorFromNoAst<TD>()
        {
            return BnfiTermConversion.NoUnparseByInverseCreatorFromNoAst<TD>();
        }

        public static ValueCreatorFromNoAst<object> NoUnparseByInverseCreatorFromNoAst()
        {
            return BnfiTermConversion.NoUnparseByInverseCreatorFromNoAst();
        }

        public static UnparseHint SetUnparsePriority(ChildrenPriorityGetter getChildrenPriority)
        {
            return new UnparseHint(getChildrenPriority);
        }

        #endregion
    }

    public abstract class Grammar<TRoot> : Grammar
    {
        #region Construction

        public Grammar(Domain<TRoot> domain)
            : base(domain)
        {
        }

        public Grammar(Domain<TRoot> domain, bool caseSensitive)
            : base(domain, caseSensitive)
        {
        }

        #endregion

        public new INonTerminal<TRoot> Root
        {
            get { return (INonTerminal<TRoot>)base.Root; }
            protected set { base.Root = value.AsNonTerminal(); }
        }
    }
}
