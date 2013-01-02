using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Irony.ITG;
using Irony.ITG.Ast;

using Grammar = Irony.ITG.Ast.Grammar;

namespace Irony.ITG.Unparsing
{
    public class Unparser
    {
        public Grammar Grammar { get; private set; }

        public Unparser(Grammar grammar)
        {
        }

        public IEnumerable<Utoken> Unparse(object obj, Context context = null)
        {
            BnfTerm bnfTerm = GetBnfTerm(obj, context);
            return Unparse(obj, bnfTerm);
        }

        public IEnumerable<Utoken> Unparse(object obj, BnfTerm bnfTerm)
        {
            if (bnfTerm is KeyTerm)
                yield return ((KeyTerm)bnfTerm).Text;
            else if (bnfTerm is Terminal)
                yield return obj.ToString();
            else if (bnfTerm is NonTerminal)
            {
                NonTerminal nonTerminal = (NonTerminal)bnfTerm;
                Unparsable unparsable = nonTerminal as Unparsable;

                if (unparsable == null)
                    throw new UnparsableException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' is not unparsable.", obj, obj.GetType().Name, nonTerminal.Name));

                Production production = unparsable.ChooseProduction(nonTerminal.Productions, obj);

                foreach (BnfTerm childBnfTerm in production.RValues)
                {
                    foreach (Utoken utoken in Unparse(unparsable.GetObjectForChildBnfTerm(childBnfTerm)))
                    {
                        yield return utoken;
                    }
                }
            }
        }

        private BnfTerm GetBnfTerm(object obj, Context context)
        {
            return Grammar.Root;
            // TODO: do this by choosing by context
        }
    }

    public class Context
    {
        public MemberInfo MemberInfo { get; private set; }

        public Context(MemberInfo memberInfo)
        {
            this.MemberInfo = memberInfo;
        }
    }

    public interface Unparsable
    {
        Production ChooseProduction(IEnumerable<Production> productions, object obj);
        object GetObjectForChildBnfTerm(BnfTerm childBnfTerm);
    }

    public class Utoken
    {
        public string Text { get; private set; }
        public object Reference { get; private set; }

        public Utoken(string text, object reference = null)
        {
            this.Text = text;
            this.Reference = reference;
        }

        public static explicit operator string(Utoken utoken)
        {
            return utoken.Text;
        }

        public static implicit operator Utoken(string text)
        {
            return new Utoken(text);
        }

        public override string ToString()
        {
            return this.Text;
        }
    }

    public class UnparsableException : Exception
    {
        public UnparsableException()
        {
        }

        public UnparsableException(string message)
            : base(message)
        {
        }
    }
}
