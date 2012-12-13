using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony;
using Irony.Ast;
using Irony.Extension.AstBinders;
using Irony.Parsing;

namespace Irony.Extension
{
    public class GrammarExtension : Grammar
    {
        #region Construction

        public GrammarExtension()
            : base()
        {
            Init();
        }

        public GrammarExtension(bool caseSensitive)
            : base(caseSensitive)
        {
            Init();
        }

        void Init()
        {
            // default values for properties
            BrowsableAstNodes = false;
        }

        #endregion

        #region Properties

        public bool BrowsableAstNodes { get; set; }

        #endregion

        #region Misc

        public void RegisterBracePair(KeyTerm openBrace, KeyTerm closeBrace)
        {
            openBrace.SetFlag(TermFlags.IsOpenBrace);
            openBrace.IsPairFor = closeBrace;
            closeBrace.SetFlag(TermFlags.IsCloseBrace);
            closeBrace.IsPairFor = openBrace;
        }

        public new KeyTerm ToTerm(string text)
        {
            return base.ToTerm(text, string.Format("\"{0}\"", text));
        }

        public IdentifierTerminal ToIdentifier(string name)
        {
            return new IdentifierTerminal(name).SetNodeCreator();
        }

        public IdentifierTerminal ToIdentifier(string name, IdOptions options)
        {
            return new IdentifierTerminal(name, options).SetNodeCreator();
        }

        public IdentifierTerminal ToIdentifier(string name, string extraChars)
        {
            return new IdentifierTerminal(name, extraChars).SetNodeCreator();
        }

        public IdentifierTerminal ToIdentifier(string name, string extraChars, string extraFirstChars = "")
        {
            return new IdentifierTerminal(name, extraChars, extraFirstChars).SetNodeCreator();
        }

        public NumberLiteral ToNumber(string name)
        {
            return new NumberLiteral(name).SetNodeCreator();
        }

        public NumberLiteral ToNumber(string name, NumberOptions options)
        {
            return new NumberLiteral(name, options).SetNodeCreator();
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
                if (omitBoundMembers && nonTerminal is MemberBoundToBnfTerm)
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
                BnfTerm bnfTermToWrite = omitBoundMembers && bnfTerm is MemberBoundToBnfTerm
                    ? ((MemberBoundToBnfTerm)bnfTerm).BnfTerm
                    : bnfTerm;

                sw.Write("{0} ", bnfTermToWrite.Name);
            }
            return sw.ToString();
        }

        #endregion
    }
}
