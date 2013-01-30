﻿using System;
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
    internal class Formatter
    {
        #region Types

        public class Params
        {
            public readonly IEnumerable<Utoken> utokensBetweenAndBefore;

            public Params(ICollection<Utoken> utokensBetweenAndBefore)
            {
                this.utokensBetweenAndBefore = utokensBetweenAndBefore;
            }
        }

        #endregion

        internal readonly static TraceSource tsUnfiltered = new TraceSource("UNFILTERED", SourceLevels.Verbose);
        internal readonly static TraceSource tsFiltered = new TraceSource("FILTERED", SourceLevels.Verbose);
        internal readonly static TraceSource tsFlattened = new TraceSource("FLATTENED", SourceLevels.Verbose);
        internal readonly static TraceSource tsProcessedDependents = new TraceSource("PROCESSED_DEPENDENTS", SourceLevels.Verbose);
        internal readonly static TraceSource tsProcessedControls = new TraceSource("PROCESSED_CONTROLS", SourceLevels.Verbose);

#if DEBUG
        static Formatter()
        {
            tsUnfiltered.Listeners.Clear();
            tsUnfiltered.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(Unparser.logDirectoryName, "01_unfiltered.log"))));

            tsFiltered.Listeners.Clear();
            tsFiltered.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(Unparser.logDirectoryName, "02_filtered.log"))));

            tsFlattened.Listeners.Clear();
            tsFlattened.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(Unparser.logDirectoryName, "03_flattened.log"))));

            tsProcessedDependents.Listeners.Clear();
            tsProcessedDependents.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(Unparser.logDirectoryName, "04_processed_dependents.log"))));

            tsProcessedControls.Listeners.Clear();
            tsProcessedControls.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(Unparser.logDirectoryName, "05_processed_controls.log"))));
        }
#endif

        private enum State { Begin, End }

        private readonly Formatting formatting;

        IList<BnfTerm> leftBnfTermsFromTopToBottom = new List<BnfTerm>();
        Stack<List<UtokenControl>> dependers = new Stack<List<UtokenControl>>();
        Stack<BnfTerm> selfAndAncestors = new Stack<BnfTerm>();
        State lastState = State.Begin;

        public Formatter(Formatting formatting)
        {
            this.formatting = formatting;
        }

        public void BeginBnfTerm(BnfTerm bnfTerm, out Params beginParams)
        {
            ICollection<Utoken> utokensBetweenAndBefore = new List<Utoken>();

            lastState = State.Begin;
            List<UtokenControl> _dependers = new List<UtokenControl>();

            if (bnfTerm.IsReferable())
                selfAndAncestors.Push(bnfTerm);

            foreach (BnfTerm leftBnfTerm in leftBnfTermsFromTopToBottom)
            {
                InsertedUtokens insertedUtokensBetween;
                if (formatting.HasUtokensBetween(leftBnfTerm, selfAndAncestors, out insertedUtokensBetween))
                {
                    bool existDepender;
                    _dependers.AddRange(CollectIndents(insertedUtokensBetween, out existDepender));

                    if (existDepender)
                        utokensBetweenAndBefore.Add((Utoken)new UtokenDepender(insertedUtokensBetween));
                    else
                        utokensBetweenAndBefore.Add((Utoken)insertedUtokensBetween);
                }
            }

            InsertedUtokens insertedUtokensBefore;
            if (formatting.HasUtokensBefore(selfAndAncestors, out insertedUtokensBefore))
            {
                bool existDepender;
                _dependers.AddRange(CollectIndents(insertedUtokensBefore, out existDepender));

                if (existDepender)
                    utokensBetweenAndBefore.Add((Utoken)new UtokenDepender(insertedUtokensBefore));
                else
                    utokensBetweenAndBefore.Add((Utoken)insertedUtokensBefore);
            }

            dependers.Push(_dependers);

            beginParams = new Params(utokensBetweenAndBefore);
        }

        public void EndBnfTerm(BnfTerm bnfTerm)
        {
            if (lastState != State.End)
                leftBnfTermsFromTopToBottom.Clear();

            if (formatting.IsLeftBnfTermOfABetweenPair(bnfTerm))
                leftBnfTermsFromTopToBottom.Insert(0, bnfTerm);  // for efficiency reasons we only store bnfTerm as leftBnfTerm if it is really a leftBnfTerm

            lastState = State.End;
            dependers.Pop();

            if (bnfTerm.IsReferable())
                selfAndAncestors.Pop();
        }

        public IEnumerable<Utoken> YieldBefore(BnfTerm bnfTerm, Params @params)
        {
            foreach (Utoken utokenBetweenOrBefore in @params.utokensBetweenAndBefore)
            {
                Unparser.tsUnparse.Debug("inserted utokens: {0}", utokenBetweenOrBefore);
                yield return utokenBetweenOrBefore;
            }
        }

        public IEnumerable<Utoken> YieldAfter(BnfTerm bnfTerm, Params @params)
        {
            InsertedUtokens insertedUtokensAfter;
            if (formatting.HasUtokensAfter(selfAndAncestors, out insertedUtokensAfter))
            {
                Unparser.tsUnparse.Debug("inserted utokens: {0}", insertedUtokensAfter);
                yield return insertedUtokensAfter;
            }

            foreach (UtokenControl depender in dependers.Peek())
            {
                UtokenDependent utokenDependent;

                if (depender == UtokenControl.IndentBlock)
                    utokenDependent = new UtokenDependent(UtokenControl.DecreaseIndentLevel, depender);
                else if (depender == UtokenControl.UnindentBlock)
                    utokenDependent = new UtokenDependent(UtokenControl.IncreaseIndentLevel, depender);
                else
                    throw new InvalidOperationException("Unknown depender utoken: " + depender.ToString());

                Unparser.tsUnparse.Debug("inserted utokens: {0}", utokenDependent);

                yield return utokenDependent;
            }
        }

        public static IEnumerable<Utoken> PostProcess(IEnumerable<Utoken> utokens)
        {
            return utokens
                .DebugWriteLines(Formatter.tsUnfiltered)
                .FilterInsertedUtokens()
                .DebugWriteLines(Formatter.tsFiltered)
                .Flatten()
                .DebugWriteLines(Formatter.tsFlattened)
                .ProcessDependents()
                .DebugWriteLines(Formatter.tsProcessedDependents)
                .ProcessControls()
                .DebugWriteLines(Formatter.tsProcessedControls)
                ;
        }

        private static IList<UtokenControl> CollectIndents(InsertedUtokens insertedUtokens, out bool existIndent)
        {
            IList<UtokenControl> indents = CollectIndents(insertedUtokens).ToList();
            existIndent = indents.Count > 0;
            return indents;
        }

        private static IEnumerable<UtokenControl> CollectIndents(InsertedUtokens insertedUtokens)
        {
            return insertedUtokens.utokens.Where(utoken => utoken.EqualToAny(UtokenControl.IndentBlock, UtokenControl.UnindentBlock)).Cast<UtokenControl>();
        }
    }

    internal static class FormatterExtensions
    {
        #region FilterInsertedUtokens

        /*
         * utokens contains several InsertedUtokens "sessions" which consist of "After", "Before" and "Between" InsertedUtokens.
         * A session looks like this: (After)*((Between)?(Before)?)*
         * 
         * Note that "Between" and "Before" InsertedUtokens are mixed with each other.
         * 
         * "After" InsertedUtokens are handled so that in case of equal priorities we will choose the right one.
         * "Between" InsertedUtokens are handled so that in case of equal priorities we will choose the left one.
         * "Before" InsertedUtokens are handled so that in case of equal priorities we will choose the left one.
         * 
         * Since "Between" and "Before" InsertedUtokens are handled in the same way, we can handle them mixed.
         * 
         * We handle the InsertedUtokens like this in order to ensure that the InsertedUtokens belonging to the
         * outer (in sense of structure) bnfterms always being preferred against inner bnfterms in case of equal priorities.
         * 
         * InsertedUtokens belonging to the same bnfterm with equal priorities are handled so that the several kind
         * of InsertedUtokens have strength in descending order: Between, Before, After
         * */

        public static IEnumerable<Utoken> FilterInsertedUtokens(this IEnumerable<Utoken> utokens)
        {
            InsertedUtokens leftInsertedUtokensToBeYield = null;

            foreach (Utoken utoken in utokens)
            {
                if (utoken is InsertedUtokens)
                {
                    InsertedUtokens rightInsertedUtokens = (InsertedUtokens)utoken;

                    bool switchToNewInsertedUtokens = IsRightStronger(leftInsertedUtokensToBeYield, rightInsertedUtokens);

                    if (rightInsertedUtokens.overridable == Overridable.Yes)
                        leftInsertedUtokensToBeYield = switchToNewInsertedUtokens ? rightInsertedUtokens : leftInsertedUtokensToBeYield;
                    else
                    {
                        if (!switchToNewInsertedUtokens)
                            yield return leftInsertedUtokensToBeYield;

                        leftInsertedUtokensToBeYield = null;

                        yield return rightInsertedUtokens;
                    }
                }
                else if (utoken is UtokenDependent)
                    yield return utoken;
                else
                {
                    if (leftInsertedUtokensToBeYield != null)
                    {
                        yield return leftInsertedUtokensToBeYield;
                        leftInsertedUtokensToBeYield = null;
                    }

                    yield return utoken;
                }
            }

            if (leftInsertedUtokensToBeYield != null)
                yield return leftInsertedUtokensToBeYield;
        }

        private static bool IsRightStronger(InsertedUtokens leftInsertedUtokens, InsertedUtokens rightInsertedUtokens)
        {
            int compareResult = InsertedUtokens.Compare(leftInsertedUtokens, rightInsertedUtokens);

            if (compareResult == 0)
                return leftInsertedUtokens.kind == InsertedUtokens.Kind.After;
            else
                return compareResult < 0;
        }

        #endregion

        public static IEnumerable<Utoken> Flatten(this IEnumerable<Utoken> utokens)
        {
            return utokens.SelectMany(utoken => utoken.Flatten());
        }

        public static IEnumerable<Utoken> ProcessDependents(this IEnumerable<Utoken> utokens)
        {
            ISet<Utoken> seenDependers = new HashSet<Utoken>();

            foreach (Utoken _utoken in utokens)
            {
                Utoken utoken = _utoken;

            LProcessUtoken:

                if (utoken is UtokenDepender)
                {
                    UtokenDepender utokenDepender = (UtokenDepender)utoken;

                    seenDependers.Add(utokenDepender.utoken);

                    // handle depender tokens recursively (utokenDepender.utoken might be a dependent, a depender or a normal utoken as well)
                    utoken = utokenDepender.utoken;
                    goto LProcessUtoken;
                }
                else if (utoken is UtokenDependent)
                {
                    UtokenDependent utokenDependent = (UtokenDependent)utoken;

                    if (seenDependers.Contains(utokenDependent.depender))
                    {
                        // handle dependent tokens recursively (utokenDependent.utoken might be a dependent, a depender or a normal utoken as well)
                        utoken = utokenDependent.utoken;
                        goto LProcessUtoken;
                    }
                }
                else
                    yield return utoken;
            }
        }

        public static IEnumerable<Utoken> ProcessControls(this IEnumerable<Utoken> utokens)
        {
            int indentLevel = 0;
            int? temporaryIndentLevel = null;
            bool firstUtokenInLine = true;
            bool allowedWhitespaceBetweenUtokens = true;

            foreach (Utoken utoken in utokens)
            {
                if (utoken is UtokenControl)
                {
                    UtokenControl utokenControl = (UtokenControl)utoken;

                    switch (utokenControl.kind)
                    {
                        case UtokenControl.Kind.IndentBlock:
                        case UtokenControl.Kind.IncreaseIndentLevel:
                            indentLevel++;
                            break;

                        case UtokenControl.Kind.UnindentBlock:
                        case UtokenControl.Kind.DecreaseIndentLevel:
                            indentLevel--;
                            break;

                        case UtokenControl.Kind.SetIndentLevelToNone:
                            indentLevel = 0;
                            break;

                        case UtokenControl.Kind.IndentThisLine:
                            temporaryIndentLevel = indentLevel + 1;
                            break;

                        case UtokenControl.Kind.UnindentThisLine:
                            temporaryIndentLevel = indentLevel - 1;
                            break;

                        case UtokenControl.Kind.NoIndentForThisLine:
                            temporaryIndentLevel = 0;
                            break;

                        case UtokenControl.Kind.NoWhitespace:
                            allowedWhitespaceBetweenUtokens = false;
                            break;

                        default:
                            throw new InvalidOperationException(string.Format("Unknown UtokenControl '{0}'", utokenControl.kind));
                    }
                }
                else
                {
                    if (utoken == UtokenWhitespace.NewLine || utoken == UtokenWhitespace.EmptyLine)
                    {
                        firstUtokenInLine = true;
                        temporaryIndentLevel = null;
                    }
                    else
                    {
                        if (firstUtokenInLine)
                            yield return new UtokenIndent(temporaryIndentLevel ?? indentLevel);
                        else if (allowedWhitespaceBetweenUtokens)
                            yield return UtokenWhitespace.WhiteSpaceBetweenUtokens;

                        firstUtokenInLine = false;
                    }

                    yield return utoken;
                    allowedWhitespaceBetweenUtokens = true;
                }
            }
        }
    }
}
