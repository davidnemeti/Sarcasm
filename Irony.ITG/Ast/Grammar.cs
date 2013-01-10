#define IRONY_INTERNALS_VISIBLE

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
using Irony.ITG.Unparsing;

namespace Irony.ITG.Ast
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

        public static BnfiTermCollection MakePlusRule(BnfiTermCollection bnfiTermCollection, BnfTerm delimiter, BnfTerm bnfTermElement)
        {
            return BnfiTermCollection.MakePlusRule(bnfiTermCollection, delimiter, bnfTermElement);
        }

        public static BnfiTermCollection<TCollectionType> MakePlusRule<TCollectionType>(BnfiTermCollection<TCollectionType, object> bnfiTermCollection, BnfTerm delimiter, BnfTerm bnfTermElement)
            where TCollectionType : ICollection<object>, new()
        {
            return BnfiTermCollection.MakePlusRule(bnfiTermCollection, delimiter, bnfTermElement);
        }

        public static BnfiTermCollection<TCollectionType> MakePlusRule<TCollectionType, TElementType>(BnfiTermCollection<TCollectionType, TElementType> bnfiTermCollection, BnfTerm delimiter, IBnfiTerm<TElementType> bnfTermElement)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return BnfiTermCollection.MakePlusRule(bnfiTermCollection, delimiter, bnfTermElement);
        }

        public static BnfiTermCollection MakeStarRule(BnfiTermCollection bnfiTermCollection, BnfTerm delimiter, BnfTerm bnfTermElement)
        {
            return BnfiTermCollection.MakeStarRule(bnfiTermCollection, delimiter, bnfTermElement);
        }

        public static BnfiTermCollection<TCollectionType> MakeStarRule<TCollectionType>(BnfiTermCollection<TCollectionType, object> bnfiTermCollection, BnfTerm delimiter, BnfTerm bnfTermElement)
            where TCollectionType : ICollection<object>, new()
        {
            return BnfiTermCollection.MakeStarRule(bnfiTermCollection, delimiter, bnfTermElement);
        }

        public static BnfiTermCollection<TCollectionType> MakeStarRule<TCollectionType, TElementType>(BnfiTermCollection<TCollectionType, TElementType> bnfiTermCollection, BnfTerm delimiter, IBnfiTerm<TElementType> bnfTermElement)
            where TCollectionType : ICollection<TElementType>, new()
        {
            return BnfiTermCollection.MakeStarRule(bnfiTermCollection, delimiter, bnfTermElement);
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
            return ToTerm(text).CreateValue(value);
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

        #region Identifiers

        public static BnfiTermValue<string> CreateIdentifier(string name = "identifier")
        {
            return new IdentifierTerminal(name).CreateIdentifier();
        }

        public static BnfiTermValue<string> CreateIdentifier(string name, IdOptions options)
        {
            return new IdentifierTerminal(name, options).CreateIdentifier();
        }

        public static BnfiTermValue<string> CreateIdentifier(string name, string extraChars)
        {
            return new IdentifierTerminal(name, extraChars).CreateIdentifier();
        }

        public static BnfiTermValue<string> CreateIdentifier(string name, string extraChars, string extraFirstChars = "")
        {
            return new IdentifierTerminal(name, extraChars, extraFirstChars).CreateIdentifier();
        }

        #endregion

        #region Numbers

        public static BnfiTermValue CreateNumber()
        {
            return new NumberLiteral(name: null).CreateNumber();
        }

        public static BnfiTermValue CreateNumber(NumberOptions options)
        {
            return new NumberLiteral(name: null, options: options).CreateNumber();
        }

        #endregion

        #region Misc

#if IRONY_INTERNALS_VISIBLE

        public string GetNonTerminalsAsText(bool omitBoundMembers = false)
        {
            return GetNonTerminalsAsText(new LanguageData(this), omitBoundMembers);
        }

        public static string GetNonTerminalsAsText(LanguageData language, bool omitBoundMembers = false)
        {
            var sw = new StringWriter();
            foreach (var nonTerminal in language.GrammarData.NonTerminals.OrderBy(nonTerminal => nonTerminal.Name))
            {
                if (omitBoundMembers && nonTerminal is BnfiTermMember)
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
                BnfTerm bnfTermToWrite = omitBoundMembers && bnfTerm is BnfiTermMember
                    ? ((BnfiTermMember)bnfTerm).BnfTerm
                    : bnfTerm;

                sw.Write("{0} ", bnfTermToWrite.Name);
            }
            return sw.ToString();
        }

#endif

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

        #endregion
    }
}
