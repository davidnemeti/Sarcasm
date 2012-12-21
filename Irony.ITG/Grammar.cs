using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony;
using Irony.Ast;
using Irony.ITG;
using Irony.Parsing;

namespace Irony.ITG
{
    public partial class Grammar : Irony.Parsing.Grammar
    {
        #region Construction

        public Grammar(AstCreation astCreation)
            : base()
        {
            Init(astCreation);
        }

        public Grammar(AstCreation astCreation, bool caseSensitive)
            : base(caseSensitive)
        {
            Init(astCreation);
        }

        void Init(AstCreation astCreation)
        {
            LanguageFlags = astCreation == AstCreation.CreateAst || astCreation == AstCreation.CreateAstWithAutoBrowsableAstNodes
                ? LanguageFlags.CreateAst
                : LanguageFlags.Default;

            AutoBrowsableAstNodes = astCreation == AstCreation.CreateAstWithAutoBrowsableAstNodes;
        }

        #endregion

        #region

        public enum AstCreation { NoAst, CreateAst, CreateAstWithAutoBrowsableAstNodes }

        #endregion

        #region Properties

        public bool AutoBrowsableAstNodes { get; set; }

        #endregion

        #region Misc

        public static void RegisterBracePair(KeyTerm openBrace, KeyTerm closeBrace)
        {
            openBrace.SetFlag(TermFlags.IsOpenBrace);
            openBrace.IsPairFor = closeBrace;
            closeBrace.SetFlag(TermFlags.IsCloseBrace);
            closeBrace.IsPairFor = openBrace;
        }

        public static BnfiTermKeyTermPunctuation ToPunctuation(string text)
        {
            return new BnfiTermKeyTermPunctuation(text);
        }

        public new KeyTerm ToTerm(string text)
        {
            return base.ToTerm(text, string.Format("\"{0}\"", text));
        }

        public static BnfiTermValue<string> ToIdentifier(string name = "identifier")
        {
            return new IdentifierTerminal(name).CreateIdentifier();
        }

        public static BnfiTermValue<string> ToIdentifier(string name, IdOptions options)
        {
            return new IdentifierTerminal(name, options).CreateIdentifier();
        }

        public static BnfiTermValue<string> ToIdentifier(string name, string extraChars)
        {
            return new IdentifierTerminal(name, extraChars).CreateIdentifier();
        }

        public static BnfiTermValue<string> ToIdentifier(string name, string extraChars, string extraFirstChars = "")
        {
            return new IdentifierTerminal(name, extraChars, extraFirstChars).CreateIdentifier();
        }

        public static BnfiTermValue<T> ToNumber<T>(string name = "number")
        {
            return new NumberLiteral(name).CreateNumber<T>();
        }

        public static BnfiTermValue<T> ToNumber<T>(string name, NumberOptions options)
        {
            return new NumberLiteral(name, options).CreateNumber<T>();
        }

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

        #endregion
    }
}
