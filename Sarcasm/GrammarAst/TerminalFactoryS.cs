﻿#region License
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

using Irony.Parsing;
using Sarcasm.DomainCore;

namespace Sarcasm.GrammarAst
{
    public partial class TerminalFactoryS
    {
        private readonly Grammar grammar;

        public TerminalFactoryS(Grammar grammar)
        {
            this.grammar = grammar;
        }

        #region Default values

        private const string defaultDateTimeFormat = "d";   // it's the default in Irony
        private const int defaultIntRadix = 10;             // it's the default in Irony

        #endregion

        #region KeyTerms

        public BnfiTermKeyTerm CreateKeyTerm(string text)
        {
            return grammar.ToTerm(text);
        }

        public BnfiTermKeyTerm CreateKeyTerm(string text, string name)
        {
            return grammar.ToTerm(text, name);
        }

        public BnfiTermConversionTL CreateKeyTermTL(string text, object value)
        {
            return grammar.ToTermTL(text, value);
        }

        public BnfiTermConversion<T> CreateKeyTerm<T>(string text, T value)
        {
            return grammar.ToTerm(text, value);
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

        #region Number

        public static BnfiTermConversion<TNumberLiteral> CreateNumberLiteral<TNumberLiteral>(NumberLiteralInfo numberLiteralInfo, string name = "numberliteral")
            where TNumberLiteral : INumberLiteral, new()
        {
            return new NumberLiteral(name: name).AttachNumberLiteralInfo(numberLiteralInfo).IntroNumberLiteral<TNumberLiteral>(numberLiteralInfo);
        }

        public static BnfiTermConversion<TNumberLiteral> CreateNumberLiteral<TNumberLiteral>(NumberLiteralInfo numberLiteralInfo, NumberOptions options)
            where TNumberLiteral : INumberLiteral, new()
        {
            return new NumberLiteral(name: "numberliteral", options: options).AttachNumberLiteralInfo(numberLiteralInfo).IntroNumberLiteral<TNumberLiteral>(numberLiteralInfo);
        }

        public static BnfiTermConversion<TNumberLiteral> CreateNumberLiteral<TNumberLiteral>(NumberLiteralInfo numberLiteralInfo, string name, NumberOptions options)
            where TNumberLiteral : INumberLiteral, new()
        {
            return new NumberLiteral(name: name, options: options).AttachNumberLiteralInfo(numberLiteralInfo).IntroNumberLiteral<TNumberLiteral>(numberLiteralInfo);
        }

        #endregion
    }
}
