using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Diagnostics;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.Ast;

using Grammar = Sarcasm.Ast.Grammar;

namespace Sarcasm.Unparsing
{
    public class Unparser : IUnparser
    {
        #region Types
        private class BnfTermsWithPriority
        {
            public readonly IEnumerable<UnparsableObject> UnparsableObjects;
            public readonly int? Priority;

            public BnfTermsWithPriority(IEnumerable<UnparsableObject> unparsableObjects, int? priority)
            {
                this.UnparsableObjects = unparsableObjects;
                this.Priority = priority;
            }
        }

        #endregion

        internal const string logDirectoryName = "unparse_logs";

        internal readonly static TraceSource tsUnparse = new TraceSource("UNPARSE", SourceLevels.Verbose);
        internal readonly static TraceSource tsPriorities = new TraceSource("PRIORITIES", SourceLevels.Verbose);

#if DEBUG
        static Unparser()
        {
            Directory.CreateDirectory(logDirectoryName);

            Trace.AutoFlush = true;

            tsUnparse.Listeners.Clear();
            tsUnparse.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(logDirectoryName, "00_unparse.log"))));

            tsPriorities.Listeners.Clear();
            tsPriorities.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(logDirectoryName, "priorities.log"))));
        }
#endif

        public Grammar Grammar { get; private set; }
        public Formatting Formatting { get; private set; }

        private readonly Formatter formatter;

        public Unparser(Grammar grammar)
            : this(grammar, grammar.DefaultFormatting)
        {
        }

        public Unparser(Grammar grammar, Formatting formatting)
        {
            this.Grammar = grammar;
            this.Formatting = formatting;

            this.formatter = new Formatter(formatting);
        }

        public IEnumerable<Utoken> Unparse(object obj, Context context = null)
        {
            BnfTerm bnfTerm = GetBnfTerm(obj, context);
            return Unparse(obj, bnfTerm);
        }

        public IEnumerable<Utoken> Unparse(object obj, BnfTerm bnfTerm)
        {
            return UnparseRaw(obj, bnfTerm)
                .Cook();
        }

        private IEnumerable<Utoken> UnparseRaw(object obj, BnfTerm bnfTerm)
        {
            if (bnfTerm == null)
                throw new ArgumentNullException("bnfTerm must not be null", "bnfTerm");

            Formatter.BeginParams beginParams;

            formatter.Begin(bnfTerm, out beginParams);

            try
            {
                /*
                 * NOTE that we cannot return the result of ConcatIfAnyMiddle because that would result in calling
                 * formatter.End immediately (before any utokens would be yielded) which is not the expected behavior.
                 * So we have to iterate through the resulted utokens and yield them one-by-one.
                 * */

                var utokens = ConcatIfAnyMiddle(
                    formatter.YieldBefore(bnfTerm, beginParams),
                    UnparseRawMiddle(obj, bnfTerm),
                    formatter.YieldAfter(bnfTerm)
                    );

                foreach (Utoken utoken in utokens)
                    yield return utoken;
            }
            finally
            {
                formatter.End(bnfTerm);
            }
        }

        private static IEnumerable<Utoken> ConcatIfAnyMiddle(IEnumerable<Utoken> utokensBefore, IEnumerable<Utoken> utokensMiddle, IEnumerable<Utoken> utokensAfter)
        {
            bool isAnyUtokenMiddle = false;

            foreach (Utoken utokenMiddle in utokensMiddle)
            {
                if (!isAnyUtokenMiddle)
                {
                    foreach (Utoken utokenBefore in utokensBefore)
                        yield return utokenBefore;
                }

                isAnyUtokenMiddle = true;

                yield return utokenMiddle;
            }

            if (isAnyUtokenMiddle)
            {
                foreach (Utoken utokenAfter in utokensAfter)
                    yield return utokenAfter;
            }
        }

        private IEnumerable<Utoken> UnparseRawMiddle(object obj, BnfTerm bnfTerm)
        {
            if (bnfTerm is KeyTerm)
            {
                Unparser.tsUnparse.Debug("keyterm: [{0}]", ((KeyTerm)bnfTerm).Text);
                yield return Utoken.CreateText(((KeyTerm)bnfTerm).Text, obj);
            }
            else if (bnfTerm is Terminal)
            {
                Unparser.tsUnparse.Debug("terminal: [\"{0}\"]", obj.ToString());
                yield return Utoken.CreateText(obj);
            }
            else if (bnfTerm is NonTerminal)
            {
                Unparser.tsUnparse.Debug("nonterminal: {0}", bnfTerm.Name);

                Unparser.tsUnparse.Indent();

                try
                {
                    NonTerminal nonTerminal = (NonTerminal)bnfTerm;
                    IUnparsable unparsable = nonTerminal as IUnparsable;

                    if (unparsable == null)
                        throw new CannotUnparseException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' is not IUnparsable.", obj, obj.GetType().Name, nonTerminal.Name));

                    IEnumerable<Utoken> directUtokens;

                    if (unparsable.TryGetUtokensDirectly(this, obj, out directUtokens))
                    {
                        foreach (Utoken utoken in directUtokens)
                            yield return utoken;

                        Unparser.tsUnparse.Debug("utokenized: [{0}]", obj != null ? string.Format("\"{0}\"", obj) : "<<NULL>>");
                    }
                    else
                    {
                        BnfTermsWithPriority chosenChildBnfTermsWithPriority = ChooseBnfTermsbyPriority(obj, unparsable);

                        if (chosenChildBnfTermsWithPriority == null)
                            throw new CannotUnparseException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' has no appropriate production rule.", obj, obj.GetType().Name, bnfTerm.Name));

                        foreach (UnparsableObject childValue in chosenChildBnfTermsWithPriority.UnparsableObjects)
                            foreach (Utoken utoken in UnparseRaw(childValue.obj, childValue.bnfTerm))
                                yield return utoken;
                    }
                }
                finally
                {
                    Unparser.tsUnparse.Unindent();
                }
            }
            else if (bnfTerm is GrammarHint)
            {
                // GrammarHint is legal, but it does not need any unparse
            }
            else
            {
                throw new ArgumentException(string.Format("bnfTerm '{0}' with unknown type: '{1}'" + bnfTerm.Name, bnfTerm.GetType().Name));
            }
        }

        private BnfTermsWithPriority ChooseBnfTermsbyPriority(object obj, IUnparsable unparsable)
        {
            // TODO: we should check whether any bnfTermList has an UnparseHint

            tsPriorities.Debug("{0} BEGIN priorities", unparsable.AsNonTerminal());
            tsPriorities.Indent();

            try
            {
                return GetChildBnfTermLists(unparsable.AsNonTerminal())
                    .Select(childBnfTerms =>
                        {
                            var childUnparsableObjects = unparsable.GetChildUnparsableObjects(childBnfTerms, obj);
                            return new BnfTermsWithPriority(
                                    childUnparsableObjects,
                                    unparsable.GetChildBnfTermListPriority(this, obj, childUnparsableObjects)
                                        .DebugWriteLinePriority(tsPriorities, unparsable.AsNonTerminal(), obj)
                                );
                        }
                    )
                    .Where(childBnfTermsWithPriority => childBnfTermsWithPriority.Priority.HasValue)
                    .OrderByDescending(childBnfTermsWithPriority => childBnfTermsWithPriority.Priority.Value)
                    .FirstOrDefault();
            }
            finally
            {
                tsPriorities.Unindent();
                tsPriorities.Debug("{0} END priorities", unparsable.AsNonTerminal());
                tsPriorities.Debug("");
            }
        }

        internal static IEnumerable<BnfTermList> GetChildBnfTermLists(NonTerminal nonTerminal)
        {
            // TODO: which one to choose?
//            return nonTerminal.Rule.Data;
            return nonTerminal.Productions.Select(production => production.RValues);
        }

        private BnfTerm GetBnfTerm(object obj, Context context)
        {
            return Grammar.Root;
            // TODO: do this by choosing by context
        }

        int? IUnparser.GetBnfTermPriority(BnfTerm bnfTerm, object obj)
        {
            if (bnfTerm is NonTerminal && bnfTerm is IUnparsable)
            {
                NonTerminal nonTerminal = (NonTerminal)bnfTerm;
                IUnparsable unparsable = (IUnparsable)bnfTerm;

                tsPriorities.Indent();

                int? priority = GetChildBnfTermLists(nonTerminal)
                    .Max(childBnfTerms => unparsable.GetChildBnfTermListPriority(this, obj, unparsable.GetChildUnparsableObjects(childBnfTerms, obj))
                        .DebugWriteLinePriority(tsPriorities, bnfTerm, obj));

                tsPriorities.Unindent();

                priority.DebugWriteLinePriority(tsPriorities, bnfTerm, obj, messageAfter: " (MAX)");

                return priority;
            }
            else
            {
                Misc.DebugWriteLinePriority(0, tsPriorities, bnfTerm, obj, messageAfter: " (for terminal)");
                return 0;
            }
        }

        IFormatProvider IUnparser.FormatProvider { get { return this.FormatProvider; } }

        protected IFormatProvider FormatProvider { get { return this.Formatting.FormatProvider; } }

        public static string ToString(IFormatProvider formatProvider, object obj)
        {
            return string.Format(formatProvider, "{0}", obj);
        }
    }

    public static class UnparserExtensions
    {
        public static string AsString(this IEnumerable<Utoken> utokens, Unparser unparser)
        {
            return string.Concat(utokens.Select(utoken => utoken.ToString(unparser.Formatting)));
        }

        public static async void WriteToStreamAsync(this IEnumerable<Utoken> utokens, Stream stream, Unparser unparser)
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                foreach (Utoken utoken in utokens)
                    await sw.WriteAsync(utoken.ToString(unparser.Formatting));
            }
        }

        internal static IEnumerable<Utoken> Cook(this IEnumerable<Utoken> utokens)
        {
            return Formatter.PostProcess(utokens);
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

    public interface IUnparsable : INonTerminal
    {
        bool TryGetUtokensDirectly(IUnparser unparser, object obj, out IEnumerable<Utoken> utokens);
        IEnumerable<UnparsableObject> GetChildUnparsableObjects(BnfTermList childBnfTerms, object obj);
        int? GetChildBnfTermListPriority(IUnparser unparser, object obj, IEnumerable<UnparsableObject> childUnparsableObjects);
    }

    public interface IUnparser
    {
        int? GetBnfTermPriority(BnfTerm bnfTerm, object obj);
        IFormatProvider FormatProvider { get; }
    }

    public class UnparsableObject
    {
        public readonly BnfTerm bnfTerm;
        public readonly object obj;

        public UnparsableObject(BnfTerm bnfTerm, object obj)
        {
            this.bnfTerm = bnfTerm;
            this.obj = obj;
        }
    }

    public delegate IEnumerable<Utoken> ValueUtokenizer<T>(IFormatProvider formatProvider, T obj);
}
