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
        public Formatting Formatting { get { return this.Grammar.Formatting; } }

        private int indentLevel = 0;

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
            BnfTerm prevBnfTerm = null;
            foreach (var item in UnparseUnfiltered(obj, bnfTerm))
            {
                yield return item.Item1;
                prevBnfTerm = item.Item2;
            }
        }

        private IEnumerable<Tuple<Utoken, BnfTerm>> UnparseUnfiltered(object obj, BnfTerm bnfTerm)
        {
            InsertedUtokens insertedUtokensBefore;
            if (Formatting.HasUtokensBefore(bnfTerm, out insertedUtokensBefore))
                yield return new Tuple<Utoken, BnfTerm>(insertedUtokensBefore, bnfTerm);

            if (bnfTerm is KeyTerm)
                yield return new Tuple<Utoken, BnfTerm>(((KeyTerm)bnfTerm).Text, bnfTerm);
            else if (bnfTerm is Terminal)
                yield return new Tuple<Utoken, BnfTerm>(obj.ToString(), bnfTerm);
            else if (bnfTerm is NonTerminal)
            {
                NonTerminal nonTerminal = (NonTerminal)bnfTerm;
                IUnparsable unparsable = nonTerminal as IUnparsable;

                if (unparsable == null)
                    throw new CannotUnparseException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' is not IUnparsable.", obj, obj.GetType().Name, nonTerminal.Name));

                foreach (Utoken utoken in unparsable.Unparse(this, obj))
                    yield return Tuple.Create(utoken, bnfTerm);
            }

            InsertedUtokens insertedUtokensAfter;
            if (Formatting.HasUtokensAfter(bnfTerm, out insertedUtokensAfter))
                yield return new Tuple<Utoken, BnfTerm>(insertedUtokensAfter, bnfTerm);
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

        private Utoken GetIndent()
        {
            return Utoken.GetIndent(Formatting, indentLevel);
        }
    }

    public static class UnparserExtensions
    {
        public static string AsString(this IEnumerable<Utoken> utokens, Unparser unparser)
        {
            return string.Join(separator: null, values: utokens.Select(utoken => utoken.ToString(unparser.Formatting)));
        }

        public static async void WriteToStreamAsync(this IEnumerable<Utoken> utokens, Stream stream, Unparser unparser)
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                foreach (Utoken utoken in utokens)
                    await sw.WriteAsync(utoken.ToString(unparser.Formatting));
            }
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
}
