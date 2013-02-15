using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        #region Tracing

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

        #endregion

        #region State

        public Grammar Grammar { get; private set; }
        public Formatting Formatting
        {
            get { return formatting; }

            private set
            {
                formatting = value;
                formatter = new Formatter(value);
            }
        }

        private Formatting formatting;
        private Formatter formatter;
        private readonly UnparseControl unparseControl;
        private readonly ExpressionUnparser expressionUnparser;

        #endregion

        #region Construction

        public Unparser(Grammar grammar)
            : this(grammar, grammar.UnparseControl)
        {
        }

        public Unparser(Grammar grammar, UnparseControl unparseControl)
        {
            this.Grammar = grammar;
            this.Formatting = unparseControl.DefaultFormatting;     // also set Formatter
            this.unparseControl = unparseControl;
            this.expressionUnparser = new ExpressionUnparser(this);
        }

        #endregion

        #region Unparse

        public IEnumerable<Utoken> Unparse(object obj, Context context = null)
        {
            BnfTerm bnfTerm = GetBnfTerm(obj, context);
            return Unparse(obj, bnfTerm);
        }

        public IEnumerable<Utoken> Unparse(object obj, BnfTerm bnfTerm)
        {
            return UnparseRaw(new UnparsableObject(bnfTerm, obj))
                .Cook();
        }

        internal IEnumerable<Utoken> UnparseRaw(UnparsableObject unparsableObject)
        {
            if (unparsableObject.bnfTerm == null)
                throw new ArgumentNullException("bnfTerm must not be null", "bnfTerm");

            Formatter.Params @params;

            formatter.BeginBnfTerm(unparsableObject.bnfTerm, out @params);

            try
            {
                /*
                 * NOTE that we cannot return the result of ConcatIfAnyMiddle because that would result in calling
                 * formatter.End immediately (before any utokens would be yielded) which is not the expected behavior.
                 * So we have to iterate through the resulted utokens and yield them one-by-one.
                 * */

                var utokens = ConcatIfAnyMiddle(
                    formatter.YieldBefore(unparsableObject.bnfTerm, @params),
                    UnparseRawMiddle(unparsableObject),
                    formatter.YieldAfter(unparsableObject.bnfTerm, @params)
                    );

                foreach (Utoken utoken in utokens)
                    yield return utoken;
            }
            finally
            {
                formatter.EndBnfTerm(unparsableObject.bnfTerm);
            }
        }

        private IEnumerable<Utoken> UnparseRawMiddle(UnparsableObject unparsableObject)
        {
            BnfTerm bnfTerm = unparsableObject.bnfTerm;
            object obj = unparsableObject.obj;

            if (bnfTerm is KeyTerm)
            {
                tsUnparse.Debug("keyterm: [{0}]", ((KeyTerm)bnfTerm).Text);
                yield return Utoken.CreateText(((KeyTerm)bnfTerm).Text, obj);
            }
            else if (bnfTerm is Terminal)
            {
                tsUnparse.Debug("terminal: [\"{0}\"]", obj.ToString());
                yield return Utoken.CreateText(obj);
            }
            else if (bnfTerm is NonTerminal)
            {
                tsUnparse.Debug("nonterminal: {0}", bnfTerm.Name);
                tsUnparse.Indent();

                try
                {
                    IUnparsable unparsable = bnfTerm as IUnparsable;

                    if (unparsable == null)
                        throw new UnparseException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' is not IUnparsable.", obj, obj.GetType().Name, bnfTerm.Name));

                    IEnumerable<Utoken> directUtokens;

                    if (unparsable.TryGetUtokensDirectly(this, obj, out directUtokens))
                    {
                        foreach (Utoken utoken in directUtokens)
                            yield return utoken;

                        tsUnparse.Debug("utokenized: [{0}]", obj != null ? string.Format("\"{0}\"", obj) : "<<NULL>>");
                    }
                    else
                    {
                        IEnumerable<UnparsableObject> chosenChildren = ChooseChildrenByPriority(obj, unparsable);

                        if (chosenChildren == null)
                        {
                            throw new UnparseException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' has no appropriate production rule.",
                                obj, obj.GetType().Name, bnfTerm.Name));
                        }

                        if (expressionUnparser.NeedsExpressionUnparse(bnfTerm))
                        {
                            foreach (Utoken utoken in expressionUnparser.Unparse(unparsableObject, chosenChildren))
                                yield return utoken;
                        }
                        else
                        {
                            foreach (UnparsableObject childValue in chosenChildren)
                                foreach (Utoken utoken in UnparseRaw(childValue))
                                    yield return utoken;
                        }
                    }
                }
                finally
                {
                    tsUnparse.Unindent();
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

        private IEnumerable<UnparsableObject> ChooseChildrenByPriority(object obj, IUnparsable unparsable)
        {
            // TODO: we should check whether any bnfTermList has an UnparseHint

            // TODO: we should avoid circularity (e.g. expression -> LEFT_PAREN + expression + RIGHT_PAREN)

            tsPriorities.Debug("{0} BEGIN priorities", unparsable.AsNonTerminal());
            tsPriorities.Indent();

            try
            {
                return GetChildBnfTermLists(unparsable.AsNonTerminal())
                    .Select(childBnfTerms =>
                        {
                            var childUnparsableObjects = unparsable.GetChildUnparsableObjects(childBnfTerms, obj);
                            return new
                            {
                                UnparsableObjects = childUnparsableObjects,
                                Priority = unparsable.GetChildBnfTermListPriority(this, obj, childUnparsableObjects)
                                    .DebugWriteLinePriority(tsPriorities, unparsable.AsNonTerminal(), obj)
                            };
                        }
                    )
                    .Where(childBnfTermsWithPriority => childBnfTermsWithPriority.Priority.HasValue)
                    .OrderByDescending(childBnfTermsWithPriority => childBnfTermsWithPriority.Priority.Value)
                    .Select(childBnfTermsWithPriority => childBnfTermsWithPriority.UnparsableObjects)
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
            return nonTerminal.Productions.Select(production => production.RValues);
        }

        private BnfTerm GetBnfTerm(object obj, Context context)
        {
            return Grammar.Root;
            // TODO: do this by choosing by context
        }

        #endregion

        #region IUnparser

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

        IFormatProvider IUnparser.FormatProvider { get { return this.Formatting.FormatProvider; } }

        #endregion
    }
}
