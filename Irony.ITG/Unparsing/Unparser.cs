using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

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
            this.Grammar = grammar;
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
                IUnparsable unparsable = nonTerminal as IUnparsable;

                if (unparsable == null)
                    throw new CannotUnparseException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' is not IUnparsable.", obj, obj.GetType().Name, nonTerminal.Name));

                foreach (Utoken utoken in unparsable.Unparse(this, obj))
                    yield return utoken;
            }
        }

        internal static IEnumerable<BnfTermList> GetChildBnfTermLists(NonTerminal nonTerminal)
        {
            return nonTerminal.Productions.Select(production => production.RValues);
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

    public interface IUnparsable
    {
        IEnumerable<Utoken> Unparse(Unparser unparser, object obj);
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

    public static class UnparserExtensions
    {
        public static string AsString(this IEnumerable<Utoken> utokens)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Utoken utoken in utokens)
                sb.Append(utoken.ToString());
            return sb.ToString();
        }

        public static async void WriteToStreamAsync(this IEnumerable<Utoken> utokens, Stream stream)
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                foreach (Utoken utoken in utokens)
                    await sw.WriteAsync(utokens.ToString());
            }
        }
    }

    public abstract class UnparseException : Exception
    {
        public UnparseException()
        {
        }

        public UnparseException(string message)
            : base(message)
        {
        }
    }

    public class CannotUnparseException : UnparseException
    {
        public CannotUnparseException()
        {
        }

        public CannotUnparseException(string message)
            : base(message)
        {
        }
    }

    internal class ValueMismatchException : UnparseException
    {
        public ValueMismatchException()
        {
        }

        public ValueMismatchException(string message)
            : base(message)
        {
        }
    }
}
