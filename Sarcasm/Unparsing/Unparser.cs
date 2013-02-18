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
            this.expressionUnparser = new ExpressionUnparser(this, unparseControl);
        }

        #endregion

        #region Unparse logic

        public IEnumerable<Utoken> Unparse(object obj)
        {
            return Unparse(obj, Grammar.Root);
        }

        public IEnumerable<Utoken> Unparse(object obj, BnfTerm bnfTerm)
        {
            return UnparseRaw(new UnparsableObject(bnfTerm, obj))
                .Cook();
        }

        internal IEnumerable<UtokenBase> UnparseRaw(UnparsableObject unparsableObject)
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

                foreach (UtokenBase utoken in utokens)
                    yield return utoken;
            }
            finally
            {
                formatter.EndBnfTerm(unparsableObject.bnfTerm);
            }
        }

        private IEnumerable<UtokenBase> UnparseRawMiddle(UnparsableObject unparsableObject)
        {
            BnfTerm bnfTerm = unparsableObject.bnfTerm;
            object obj = unparsableObject.obj;

            if (bnfTerm is KeyTerm)
            {
                tsUnparse.Debug("keyterm: [{0}]", ((KeyTerm)bnfTerm).Text);
                yield return UtokenValue.CreateText(((KeyTerm)bnfTerm).Text, unparsableObject);
            }
            else if (bnfTerm is Terminal)
            {
                tsUnparse.Debug("terminal: [\"{0}\"]", obj.ToString());
                yield return UtokenValue.CreateText(unparsableObject);
            }
            else if (bnfTerm is NonTerminal)
            {
                tsUnparse.Debug("nonterminal: {0}", bnfTerm.Name);
                tsUnparse.Indent();

                try
                {
                    IUnparsableNonTerminal unparsable = bnfTerm as IUnparsableNonTerminal;

                    if (unparsable == null)
                        throw new UnparseException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' is not IUnparsable.", obj, obj.GetType().Name, bnfTerm.Name));

                    IEnumerable<UtokenValue> directUtokens;

                    if (unparsable.TryGetUtokensDirectly(this, obj, out directUtokens))
                    {
                        foreach (UtokenValue directUtoken in directUtokens)
                        {
                            if (directUtoken is UtokenToUnparse)
                            {
                                foreach (UtokenBase utoken in UnparseRaw(((UtokenToUnparse)directUtoken).UnparsableObject))
                                    yield return utoken;
                            }
                            else
                                yield return directUtoken;
                        }

                        tsUnparse.Debug("utokenized: [{0}]", obj != null ? string.Format("\"{0}\"", obj) : "<<NULL>>");
                    }
                    else
                    {
                        IEnumerable<UnparsableObject> chosenChildren = ChooseChildrenByPriority(unparsableObject);

                        if (chosenChildren == null)
                        {
                            throw new UnparseException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' has no appropriate production rule.",
                                obj, obj.GetType().Name, bnfTerm.Name));
                        }

                        if (expressionUnparser.NeedsExpressionUnparse(bnfTerm))
                        {
                            foreach (UtokenBase utoken in expressionUnparser.Unparse(unparsableObject, chosenChildren))
                                yield return utoken;
                        }
                        else
                        {
                            foreach (UnparsableObject childValue in chosenChildren)
                                foreach (UtokenBase utoken in UnparseRaw(childValue))
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

        private static IEnumerable<UtokenBase> ConcatIfAnyMiddle(IEnumerable<UtokenBase> utokensBefore, IEnumerable<UtokenBase> utokensMiddle, IEnumerable<UtokenBase> utokensAfter)
        {
            bool isAnyUtokenMiddle = false;

            foreach (UtokenBase utokenMiddle in utokensMiddle)
            {
                if (!isAnyUtokenMiddle)
                {
                    foreach (UtokenBase utokenBefore in utokensBefore)
                        yield return utokenBefore;
                }

                isAnyUtokenMiddle = true;

                yield return utokenMiddle;
            }

            if (isAnyUtokenMiddle)
            {
                foreach (UtokenBase utokenAfter in utokensAfter)
                    yield return utokenAfter;
            }
        }

        private IEnumerable<UnparsableObject> ChooseChildrenByPriority(UnparsableObject unparsableObject)
        {
            // TODO: we should check whether any bnfTermList has an UnparseHint

            object obj = unparsableObject.obj;
            IUnparsableNonTerminal unparsable = (IUnparsableNonTerminal)unparsableObject.bnfTerm;

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
                    .FirstOrDefault(unparsableObjects => !unparsableObjects.Contains(unparsableObject));    // NOTE: filter here (after ordering) to minimize the number of comparisons
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

        #endregion

        #region IUnparser implementation

        int? IUnparser.GetBnfTermPriority(BnfTerm bnfTerm, object obj)
        {
            if (bnfTerm is NonTerminal && bnfTerm is IUnparsableNonTerminal)
            {
                NonTerminal nonTerminal = (NonTerminal)bnfTerm;
                IUnparsableNonTerminal unparsable = (IUnparsableNonTerminal)bnfTerm;

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
