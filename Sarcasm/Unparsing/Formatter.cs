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
    internal class Formatter
    {
        #region Types

        public class Params
        {
            public readonly IEnumerable<InsertedUtokens> utokensBetweenAndBefore;

            public Params(ICollection<InsertedUtokens> utokensBetweenAndBefore)
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
        Stack<BnfTerm> selfAndAncestors = new Stack<BnfTerm>();
        State lastState = State.Begin;

        public Formatter(Formatting formatting)
        {
            this.formatting = formatting;
        }

        public void BeginBnfTerm(BnfTerm bnfTerm, out Params beginParams)
        {
            ICollection<InsertedUtokens> utokensBetweenAndBefore = new List<InsertedUtokens>();

            lastState = State.Begin;
            selfAndAncestors.Push(bnfTerm);

            foreach (BnfTerm leftBnfTerm in leftBnfTermsFromTopToBottom)
            {
                InsertedUtokens insertedUtokensBetween;
                if (formatting.HasUtokensBetween(leftBnfTerm, selfAndAncestors, out insertedUtokensBetween))
                    utokensBetweenAndBefore.Add(insertedUtokensBetween.CreateYinIfOrReturnSelf(NeedsAutoCloseIndent));
            }

            InsertedUtokens insertedUtokensBefore;
            if (formatting.HasUtokensBefore(selfAndAncestors, out insertedUtokensBefore))
                utokensBetweenAndBefore.Add(insertedUtokensBefore.CreateYinIfOrReturnSelf(NeedsAutoCloseIndent));

            beginParams = new Params(utokensBetweenAndBefore);
        }

        public void EndBnfTerm(BnfTerm bnfTerm)
        {
            if (lastState != State.End)
                leftBnfTermsFromTopToBottom.Clear();

            if (formatting.IsLeftBnfTermOfABetweenPair(bnfTerm))
                leftBnfTermsFromTopToBottom.Insert(0, bnfTerm);  // for efficiency reasons we only store bnfTerm as leftBnfTerm if it is really a leftBnfTerm

            lastState = State.End;
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

            foreach (InsertedUtokens utokenBetweenOrAfter in @params.utokensBetweenAndBefore)
            {
                var autoCloseIndents = new List<UtokenInsert>();

                foreach (UtokenInsert utoken in utokenBetweenOrAfter.utokens)
                {
                    UtokenControl closeIndent;

                    if (TryCreateAutoCloseIndent(utoken, out closeIndent))
                    {
                        Unparser.tsUnparse.Debug("automatic close indent: {0}", closeIndent);

                        autoCloseIndents.Add(closeIndent);
                    }
                }

                if (autoCloseIndents.Count > 0)
                    yield return new InsertedUtokens(InsertedUtokens.Kind.After, utokenBetweenOrAfter.priority, Behavior.NonOverridableSkipThrough, autoCloseIndents, bnfTerm);
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

        private static bool NeedsAutoCloseIndent(Utoken utoken)
        {
            UtokenControl closeIndent;
            return TryCreateAutoCloseIndent(utoken, preventCreation: true, closeIndent: out closeIndent);
        }

        private static bool TryCreateAutoCloseIndent(Utoken utoken, out UtokenControl closeIndent)
        {
            return TryCreateAutoCloseIndent(utoken, preventCreation: false, closeIndent: out closeIndent);
        }

        private static bool TryCreateAutoCloseIndent(Utoken utoken, bool preventCreation, out UtokenControl closeIndent)
        {
            UtokenControl utokenControl = utoken as UtokenControl;

            if (utokenControl == null)
            {
                closeIndent = null;
                return false;
            }
            else if (utokenControl.kind == UtokenControl.Kind.IndentBlock)
            {
                if (preventCreation)
                    closeIndent = null;
                else
                {
                    closeIndent = new UtokenControl(UtokenControl.Kind.DecreaseIndentLevel);
                    closeIndent.MakeYang(utoken);
                }

                return true;
            }
            else if (utokenControl.kind == UtokenControl.Kind.UnindentBlock)
            {
                if (preventCreation)
                    closeIndent = null;
                else
                {
                    closeIndent = new UtokenControl(UtokenControl.Kind.IncreaseIndentLevel);
                    closeIndent.MakeYang(utoken);
                }

                return true;
            }
            else
            {
                closeIndent = null;
                return false;
            }
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

                    var switchToNewInsertedUtokens = new Lazy<bool>(() => IsRightStronger(leftInsertedUtokensToBeYield, rightInsertedUtokens));

                    if (rightInsertedUtokens.behavior == Behavior.Overridable)
                    {
                        leftInsertedUtokensToBeYield = switchToNewInsertedUtokens.Value ? rightInsertedUtokens : leftInsertedUtokensToBeYield;
                    }
                    else if (rightInsertedUtokens.behavior == Behavior.NonOverridableSkipThrough)
                    {
                        yield return rightInsertedUtokens;
                    }
                    else if (rightInsertedUtokens.behavior == Behavior.NonOverridableSeparator)
                    {
                        if (!switchToNewInsertedUtokens.Value)
                            yield return leftInsertedUtokensToBeYield;

                        yield return rightInsertedUtokens;
                        leftInsertedUtokensToBeYield = null;
                    }
                }
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

            foreach (Utoken utoken in utokens)
            {
                if (utoken.IsYin())
                {
                    seenDependers.Add(utoken);
                    yield return utoken;
                }
                else if (utoken.IsYang())
                {
                    if (seenDependers.Contains(utoken.GetYin()))
                        yield return utoken;
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
            bool allowWhitespaceBetweenUtokens = true;
            Utoken prevUtoken = null;

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
                            allowWhitespaceBetweenUtokens = false;
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
                        else if (allowWhitespaceBetweenUtokens && !(prevUtoken is UtokenWhitespace) && !(utoken is UtokenWhitespace))
                            yield return UtokenWhitespace.WhiteSpaceBetweenUtokens;

                        firstUtokenInLine = false;
                    }

                    yield return utoken;
                    allowWhitespaceBetweenUtokens = true;
                }

                prevUtoken = utoken;
            }
        }
    }
}
