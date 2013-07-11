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

namespace Sarcasm.GrammarAst
{
    public partial class TerminalFactoryS
    {
        private readonly Grammar grammar;

        public TerminalFactoryS(Grammar grammar)
        {
            this.grammar = grammar;
        }

        #region KeyTerms

        public BnfiTermKeyTerm CreateKeyTerm(string text)
        {
            return grammar.ToTerm(text);
        }

        public BnfiTermKeyTerm CreateKeyTerm(string text, string name)
        {
            return grammar.ToTerm(text, name);
        }

        public BnfiTermConversion<T> CreateKeyTerm<T>(string text, T value)
        {
            return grammar.ToTerm(text, value);
        }

        public static BnfiTermKeyTermPunctuation CreatePunctuation(string text)
        {
            return Grammar.ToPunctuation(text);
        }

        #endregion

        #region Identifiers

        public static BnfiTermConversion<string> CreateIdentifier(string name = "identifier")
        {
            return new IdentifierTerminal(name).IntroIdentifier();
        }

        public static BnfiTermConversion<string> CreateIdentifier(string name, IdOptions options)
        {
            return new IdentifierTerminal(name, options).IntroIdentifier();
        }

        public static BnfiTermConversion<string> CreateIdentifier(string name, string extraChars)
        {
            return new IdentifierTerminal(name, extraChars).IntroIdentifier();
        }

        public static BnfiTermConversion<string> CreateIdentifier(string name, string extraChars, string extraFirstChars = "")
        {
            return new IdentifierTerminal(name, extraChars, extraFirstChars).IntroIdentifier();
        }

        #endregion

        #region String literals

        public static BnfiTermConversion<string> CreateStringLiteral(string name, string startEndSymbol)
        {
            return new StringLiteral(name, startEndSymbol).IntroStringLiteral();
        }

        public static BnfiTermConversion<string> CreateStringLiteral(string name, string startEndSymbol, StringOptions options)
        {
            return new StringLiteral(name, startEndSymbol, options).IntroStringLiteral();
        }

        #endregion
    }
}
