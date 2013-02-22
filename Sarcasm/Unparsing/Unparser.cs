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

        #region Constants

        private const bool allowPartialInvalidationDefault = false;

        #endregion

        #region State

        #region Immutable after initialization

        public Grammar Grammar { get; private set; }
        private Formatting formatting;
        private readonly UnparseControl unparseControl;
        private readonly bool allowPartialInvalidation;

        #endregion

        #region Mutable

        private Formatter formatter;
        private ExpressionUnparser expressionUnparser;

        #endregion

        #endregion

        #region Construction

        public Unparser(Grammar grammar, bool allowPartialInvalidation = allowPartialInvalidationDefault)
            : this(grammar, grammar.UnparseControl.DefaultFormatting, allowPartialInvalidation)
        {
        }

        public Unparser(Grammar grammar, Formatting formatting, bool allowPartialInvalidation = allowPartialInvalidationDefault)
        {
            this.Grammar = grammar;
            this.Formatting = formatting;   // also sets Formatter
            this.unparseControl = grammar.UnparseControl;
            this.expressionUnparser = new ExpressionUnparser(this, grammar.UnparseControl);
            this.allowPartialInvalidation = allowPartialInvalidation;
        }

        #endregion

        #region Unparse logic

        public IEnumerable<Utoken> Unparse(object obj)
        {
            return Unparse(obj, Grammar.Root);
        }

        public IEnumerable<Utoken> Unparse(object obj, BnfTerm bnfTerm)
        {
            var root = new UnparsableObject(bnfTerm, obj);
            root.SetAsRoot();
            return UnparseRaw(root)
                .Cook();
        }

        internal IEnumerable<UtokenBase> UnparseRaw(UnparsableObject self)
        {
            if (self.BnfTerm == null)
                throw new ArgumentNullException("bnfTerm must not be null", "bnfTerm");

            Formatter.Params @params;

            formatter.BeginBnfTerm(self, out @params);

            try
            {
                /*
                 * NOTE that we cannot return the result of ConcatIfAnyMiddle because that would result in calling
                 * formatter.End immediately (before any utokens would be yielded) which is not the expected behavior.
                 * So we have to iterate through the resulted utokens and yield them one-by-one.
                 * */

                var utokens = ConcatIfAnyMiddle(
                    formatter.YieldBefore(@params),
                    UnparseRawMiddle(self),
                    formatter.YieldAfter(self, @params)
                    );

                foreach (UtokenBase utoken in utokens)
                    yield return utoken;
            }
            finally
            {
                formatter.EndBnfTerm(self);
            }
        }

        private IEnumerable<UtokenBase> UnparseRawMiddle(UnparsableObject self)
        {
            if (self.BnfTerm is KeyTerm)
            {
                tsUnparse.Debug("keyterm: [{0}]", ((KeyTerm)self.BnfTerm).Text);
                yield return UtokenValue.CreateText(((KeyTerm)self.BnfTerm).Text, self);
            }
            else if (self.BnfTerm is Terminal)
            {
                tsUnparse.Debug("terminal: [\"{0}\"]", self.Obj.ToString());
                yield return UtokenValue.CreateText(self);
            }
            else if (self.BnfTerm is NonTerminal)
            {
                tsUnparse.Debug("nonterminal: {0}", self.BnfTerm.Name);
                tsUnparse.Indent();

                try
                {
                    IUnparsableNonTerminal unparsableSelf = self.BnfTerm as IUnparsableNonTerminal;

                    if (unparsableSelf == null)
                        throw new UnparseException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' is not IUnparsable.", self.Obj, self.Obj.GetType().Name, self.BnfTerm.Name));

                    IEnumerable<UtokenValue> directUtokens;

                    if (unparsableSelf.TryGetUtokensDirectly(this, self.Obj, out directUtokens))
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

                        tsUnparse.Debug("utokenized: [{0}]", self.Obj != null ? string.Format("\"{0}\"", self.Obj) : "<<NULL>>");
                    }
                    else
                    {
                        IEnumerable<UnparsableObject> chosenChildren = ChooseChildrenByPriority(self);

                        if (chosenChildren == null)
                        {
                            throw new UnparseException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' has no appropriate production rule.",
                                self.Obj, self.Obj.GetType().Name, self.BnfTerm.Name));
                        }

                        chosenChildren = LinkChildrenToEachOthersAndToCurrent(self, chosenChildren);

                        if (expressionUnparser.NeedsExpressionUnparse(self.BnfTerm))
                        {
                            foreach (UtokenBase utoken in expressionUnparser.Unparse(self, chosenChildren))
                                yield return utoken;
                        }
                        else
                        {
                            foreach (UnparsableObject chosenChild in chosenChildren)
                                foreach (UtokenBase utoken in UnparseRaw(chosenChild))
                                    yield return utoken;
                        }
                    }
                }
                finally
                {
                    tsUnparse.Unindent();
                }
            }
            else if (self.BnfTerm is GrammarHint)
            {
                // GrammarHint is legal, but it does not need any unparse
            }
            else
            {
                throw new ArgumentException(string.Format("bnfTerm '{0}' with unknown type: '{1}'" + self.BnfTerm.Name, self.BnfTerm.GetType().Name));
            }
        }

        private static IEnumerable<UnparsableObject> LinkChildrenToEachOthersAndToCurrent(UnparsableObject self, IEnumerable<UnparsableObject> children)
        {
            UnparsableObject childLeftSibling = null;

            return children
                .ForAll(
                    executeBeforeEachIteration:
                        child =>
                        {
                            child.Parent = self;

                            if (self.LeftMostChild == null)
                                self.LeftMostChild = child;

                            child.LeftSibling = childLeftSibling;

                            if (childLeftSibling != null)
                                childLeftSibling.RightSibling = child;

                            childLeftSibling = child;
                        },

                    executeAfterFinished:
                        () =>
                        {
                            self.RightMostChild = childLeftSibling;

                            if (childLeftSibling != null)
                                childLeftSibling.RightSibling = null;
                        }
                );
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

            object obj = unparsableObject.Obj;
            IUnparsableNonTerminal unparsable = (IUnparsableNonTerminal)unparsableObject.BnfTerm;

            tsPriorities.Debug("{0} BEGIN priorities", unparsable.AsNonTerminal());
            tsPriorities.Indent();

            try
            {
                return GetChildBnfTermLists(unparsable.AsNonTerminal())
                    .Select(childBnfTerms =>
                        {
                            var children = unparsable.GetChildren(childBnfTerms, obj);
                            return new
                            {
                                UnparsableObjects = children,
                                Priority = unparsable.GetChildrenPriority(this, obj, children)
                                    .DebugWriteLinePriority(tsPriorities, unparsableObject)
                            };
                        }
                    )
                    .Where(childrenWithPriority => childrenWithPriority.Priority.HasValue)
                    .OrderByDescending(childrenWithPriority => childrenWithPriority.Priority.Value)
                    .Select(childrenWithPriority => childrenWithPriority.UnparsableObjects)
                    .FirstOrDefault(children => !children.Contains(unparsableObject));    // NOTE: filter here (after ordering) to minimize the number of comparisons
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

        private bool buildFullUnparseTree { get { return allowPartialInvalidation; } }

        #endregion

        #region IUnparser implementation

        int? IUnparser.GetPriority(UnparsableObject unparsableObject)
        {
            IUnparsableNonTerminal unparsable = unparsableObject.BnfTerm as IUnparsableNonTerminal;

            if (unparsable != null)
            {

                tsPriorities.Indent();

                int? priority = GetChildBnfTermLists(unparsable.AsNonTerminal())
                    .Max(childBnfTerms => unparsable.GetChildrenPriority(this, unparsableObject.Obj, unparsable.GetChildren(childBnfTerms, unparsableObject.Obj))
                        .DebugWriteLinePriority(tsPriorities, unparsableObject));

                tsPriorities.Unindent();

                priority.DebugWriteLinePriority(tsPriorities, unparsableObject, messageAfter: " (MAX)");

                return priority;
            }
            else
            {
                Misc.DebugWriteLinePriority(0, tsPriorities, unparsableObject, messageAfter: " (for terminal)");
                return 0;
            }
        }

        IFormatProvider IUnparser.FormatProvider { get { return this.Formatting.FormatProvider; } }

        #endregion

        #region Misc

        public Formatting Formatting
        {
            get { return formatting; }

            private set
            {
                formatting = value;
                formatter = new Formatter(value);
            }
        }

        #endregion
    }
}
