using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm.Unparsing;

namespace Sarcasm.Ast
{
    public enum AstCreation { NoAst, CreateAst, CreateAstWithAutoBrowsableAstNodes }
    public enum EmptyCollectionHandling { ReturnNull, ReturnEmpty }
    public enum ErrorHandling { ThrowException, ErrorMessage }

    public partial class Grammar : Irony.Parsing.Grammar
    {
        #region Construction

        public Grammar(AstCreation astCreation, EmptyCollectionHandling emptyCollectionHandling, ErrorHandling errorHandling)
            : base()
        {
            Init(astCreation, emptyCollectionHandling, errorHandling);
        }

        public Grammar(AstCreation astCreation, EmptyCollectionHandling emptyCollectionHandling, ErrorHandling errorHandling, bool caseSensitive)
            : base(caseSensitive)
        {
            Init(astCreation, emptyCollectionHandling, errorHandling);
        }

        void Init(AstCreation astCreation, EmptyCollectionHandling emptyCollectionHandling, ErrorHandling errorHandling)
        {
            this.LanguageFlags = astCreation == AstCreation.CreateAst || astCreation == AstCreation.CreateAstWithAutoBrowsableAstNodes
                ? LanguageFlags.CreateAst
                : LanguageFlags.Default;

            this.AstCreation = astCreation;
            this.EmptyCollectionHandling = emptyCollectionHandling;
            this.ErrorHandling = errorHandling;
        }

        #endregion

        #region Properties

        public AstCreation AstCreation { get; set; }
        public EmptyCollectionHandling EmptyCollectionHandling { get; set; }
        public ErrorHandling ErrorHandling { get; set; }

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
                text = text.ToLower(CultureInfo.InvariantCulture);
            string.Intern(text);
            term = new BnfiTermKeyTerm(text, name);
            KeyTerms[text] = term;
            return (BnfiTermKeyTerm)term;

            #endregion
        }

        public BnfiTermValue<T> ToTerm<T>(string text, T value)
        {
            return ToTerm(text).ParseValue(value);
        }

        public static BnfiTermKeyTermPunctuation ToPunctuation(string text)
        {
            return new BnfiTermKeyTermPunctuation(text);
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
                    empty = base.Empty.NoAst();

                return empty;
            }
        }

        #endregion

        #region Operators

        public new void RegisterOperators(int precedence, params BnfTerm[] operators)
        {
            RegisterTerminalOperators(operators.OfType<Terminal>(), precedence, associativity: null);
            RegisterNonTerminalOperators(operators.OfType<NonTerminal>(), precedence, associativity: null);
        }

        public new void RegisterOperators(int precedence, Associativity associativity, params BnfTerm[] operators)
        {
            RegisterTerminalOperators(operators.OfType<Terminal>(), precedence, associativity);
            RegisterNonTerminalOperators(operators.OfType<NonTerminal>(), precedence, associativity);
        }

        private void RegisterTerminalOperators(IEnumerable<Terminal> terminalOperators, int precedence, Associativity? associativity)
        {
            foreach (Terminal terminalOperator in terminalOperators)
                RegisterTerminalOperator(terminalOperator, precedence, associativity);
        }

        private void RegisterTerminalOperator(Terminal terminalOperator, int precedence, Associativity? associativity)
        {
            if (terminalOperator.Precedence != BnfTerm.NoPrecedence)
                throw new InvalidOperationException(string.Format("Double call of RegisterOperators on terminal '{0}'", terminalOperator));

            if (associativity.HasValue)
                base.RegisterOperators(precedence, associativity.Value, terminalOperator);
            else
                base.RegisterOperators(precedence, terminalOperator);
        }

        private void RegisterNonTerminalOperators(IEnumerable<NonTerminal> nonTerminalOperators, int precedence, Associativity? associativity)
        {
            foreach (NonTerminal nonTerminalOperator in nonTerminalOperators)
                RegisterNonTerminalOperator(nonTerminalOperator, precedence, associativity);
        }

        private void RegisterNonTerminalOperator(NonTerminal nonTerminalOperator, int precedence, Associativity? associativity)
        {
            if (nonTerminalOperator.Precedence != BnfTerm.NoPrecedence)
                throw new InvalidOperationException(string.Format("Double call of RegisterOperators on non-terminal '{0}'", nonTerminalOperator));

            if (nonTerminalOperator.Rule == null)
            {
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error,
                    "Rule is needed to have been set for nonterminal operator '{0}' before calling RegisterOperators", nonTerminalOperator);
            }

            foreach (var bnfTerms in nonTerminalOperator.Rule.GetBnfTermsList())
            {
                foreach (var bnfTerm in bnfTerms)
                {
                    if (bnfTerm is Terminal)
                        RegisterTerminalOperator((Terminal)bnfTerm, precedence, associativity);
                    else if (bnfTerm is NonTerminal && !bnfTerm.Flags.IsSet(TermFlags.NoAstNode))
                        RegisterNonTerminalOperator((NonTerminal)bnfTerm, precedence, associativity);
                }
            }

            nonTerminalOperator.SetFlag(TermFlags.InheritPrecedence);
        }

        #endregion

        #region Identifiers

        public static BnfiTermValue<string> CreateIdentifier(string name = "identifier")
        {
            return new IdentifierTerminal(name).ParseIdentifier();
        }

        public static BnfiTermValue<string> CreateIdentifier(string name, IdOptions options)
        {
            return new IdentifierTerminal(name, options).ParseIdentifier();
        }

        public static BnfiTermValue<string> CreateIdentifier(string name, string extraChars)
        {
            return new IdentifierTerminal(name, extraChars).ParseIdentifier();
        }

        public static BnfiTermValue<string> CreateIdentifier(string name, string extraChars, string extraFirstChars = "")
        {
            return new IdentifierTerminal(name, extraChars, extraFirstChars).ParseIdentifier();
        }

        #endregion

        #region String literals

        public static BnfiTermValue<string> CreateStringLiteral(string name, string startEndSymbol)
        {
            return new StringLiteral(name, startEndSymbol).ParseStringLiteral();
        }

        public static BnfiTermValue<string> CreateStringLiteral(string name, string startEndSymbol, StringOptions options)
        {
            return new StringLiteral(name, startEndSymbol, options).ParseStringLiteral();
        }

        #endregion

        #region Misc

        public static new Grammar CurrentGrammar
        {
            get { return (Grammar)Irony.Parsing.Grammar.CurrentGrammar; }
        }

        #endregion

        #region Unparsing

        private Formatting _defaultFormatting;
        public Formatting DefaultFormatting
        {
            get
            {
                if (_defaultFormatting == null)
                    _defaultFormatting = Formatting.CreateDefaultFormattingForGrammar(this);

                return _defaultFormatting;
            }
        }

        public static ValueConverter<object, object> NoUnparseByInverse()
        {
            return BnfiTermValue.NoUnparseByInverse();
        }

        public static ValueConverter<T, object> NoUnparseByInverse<T>()
        {
            return BnfiTermValue.NoUnparseByInverse<T>();
        }

        public static ValueConverter<TIn, TOut> NoUnparseByInverse<TIn, TOut>()
        {
            return BnfiTermValue.NoUnparseByInverse<TIn, TOut>();
        }

        #endregion
    }
}
