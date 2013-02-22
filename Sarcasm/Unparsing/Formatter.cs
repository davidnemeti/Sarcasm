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
        internal readonly static TraceSource tsUnfiltered = new TraceSource("UNFILTERED", SourceLevels.Verbose);
        internal readonly static TraceSource tsFiltered = new TraceSource("FILTERED", SourceLevels.Verbose);
        internal readonly static TraceSource tsFlattened = new TraceSource("FLATTENED", SourceLevels.Verbose);
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

            tsProcessedControls.Listeners.Clear();
            tsProcessedControls.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(Unparser.logDirectoryName, "04_processed_controls.log"))));
        }
#endif

        private enum State { Begin, End }

        #region State

        #region Immutable after initialization

        private readonly Formatting formatting;

        #endregion

        #region Mutable

        private State lastState = State.Begin;
        private UnparsableObject topLeftCache = null;
        private Stack<BlockIndentation> blockIndentations = new Stack<BlockIndentation>();

        #endregion

        #endregion

        public Formatter(Formatting formatting)
        {
            this.formatting = formatting;
        }

        /// <summary>
        /// This method needs to be fully executed before UnparseRawMiddle because this method modifies the state of Unparser and UnparsableObject tree,
        /// which state is used by UnparseRawMiddle. Thus, always call this method prior to UnparseRawMiddle.
        /// </summary>
        public IReadOnlyList<UtokenBase> YieldBetweenAndBefore(UnparsableObject self)
        {
            /*
             * To achieve fully execution before UnparseRawMiddle, this method is not an iterator block rather populates a list.
             * Returning IReadOnlyList instead of IEnumerable is just an explicit guarantee to the caller to ensure that
             * the returned utokens does not need to be iterated through e.g. by converting it to a list in order to achieve full execution.
             * */

            var utokens = new List<UtokenBase>();

            lastState = State.Begin;

            BlockIndentation blockIndentation = null;

            foreach (BnfTerm leftBnfTerm in GetUsedLeftsFromTopToBottomB(self))
            {
                if (blockIndentation == null && formatting.HasBlockIndentation(leftBnfTerm, GetSelfAndAncestorsB(self), out blockIndentation))
                    Unparser.tsUnparse.Debug("blockindentation {0} for leftBnfTerm '{1}'", blockIndentation, leftBnfTerm);

                InsertedUtokens insertedUtokensBetween;
                if (formatting.HasUtokensBetween(leftBnfTerm, GetSelfAndAncestorsB(self), out insertedUtokensBetween))
                {
                    Unparser.tsUnparse.Debug("inserted utokens: {0}", insertedUtokensBetween);
                    utokens.Add(insertedUtokensBetween);
                }
            }

            if (blockIndentation == null && formatting.HasBlockIndentation(GetSelfAndAncestorsB(self), out blockIndentation))
                Unparser.tsUnparse.Debug("blockindentation {0}", blockIndentation);

            if (blockIndentation == BlockIndentation.Indent)
                utokens.Add(UtokenControl.IncreaseIndentLevel);
            else if (blockIndentation == BlockIndentation.Unindent)
                utokens.Add(UtokenControl.DecreaseIndentLevel);
            else if (blockIndentation == BlockIndentation.NoIndent)
                utokens.Add(UtokenControl.SetIndentLevelToNone);

            InsertedUtokens insertedUtokensBefore;
            if (formatting.HasUtokensBefore(GetSelfAndAncestorsB(self), out insertedUtokensBefore))
            {
                Unparser.tsUnparse.Debug("inserted utokens: {0}", insertedUtokensBefore);
                utokens.Add(insertedUtokensBefore);
            }

            blockIndentations.Push(blockIndentation);

            return utokens;
        }

        public IReadOnlyList<UtokenBase> YieldAfter(UnparsableObject self)
        {
            var utokens = new List<UtokenBase>();

            InsertedUtokens insertedUtokensAfter;
            if (formatting.HasUtokensAfter(GetSelfAndAncestorsB(self), out insertedUtokensAfter))
            {
                Unparser.tsUnparse.Debug("inserted utokens: {0}", insertedUtokensAfter);
                utokens.Add(insertedUtokensAfter);
            }

            BlockIndentation blockIndentation = blockIndentations.Pop();

            if (blockIndentation == BlockIndentation.Indent)
                utokens.Add(UtokenControl.DecreaseIndentLevel);
            else if (blockIndentation == BlockIndentation.Unindent)
                utokens.Add(UtokenControl.IncreaseIndentLevel);
            else if (blockIndentation == BlockIndentation.NoIndent)
                utokens.Add(UtokenControl.RestoreIndentLevel);

            if (lastState != State.End)
                topLeftCache = null;

            if (topLeftCache == null)
                topLeftCache = self;

            lastState = State.End;

            return utokens;
        }

        public static IEnumerable<Utoken> PostProcess(IEnumerable<UtokenBase> utokens)
        {
            return utokens
                .DebugWriteLines(Formatter.tsUnfiltered)
                .FilterInsertedUtokens()
                .DebugWriteLines(Formatter.tsFiltered)
                .Flatten()
                .DebugWriteLines(Formatter.tsFlattened)
                .ProcessControls()
                .DebugWriteLines(Formatter.tsProcessedControls)
                .Cast<Utoken>()
                ;
        }

        private static IEnumerable<UnparsableObject> GetSelfAndAncestors(UnparsableObject self)
        {
            for (UnparsableObject current = self; current != null; current = current.Parent)
                yield return current;
        }

        private static IEnumerable<BnfTerm> GetSelfAndAncestorsB(UnparsableObject self)
        {
            return GetSelfAndAncestors(self).Select(current => current.BnfTerm);
        }

        private UnparsableObject GetTopLeft(UnparsableObject self)
        {
            // TODO: read cached value from Formatter if exists

            for (UnparsableObject current = self; current != null; current = current.Parent)
            {
                if (current.LeftSibling != null)
                    return current.LeftSibling;
            }

            return null;
        }

        private IEnumerable<UnparsableObject> GetLeftsFromTopToBottom(UnparsableObject self)
        {
            UnparsableObject topLeft = GetTopLeft(self);

            for (UnparsableObject current = topLeft; current != null; current = current.RightMostChild)
                yield return current;
        }

        private IEnumerable<BnfTerm> GetLeftsFromTopToBottomB(UnparsableObject self)
        {
            return GetLeftsFromTopToBottom(self).Select(current => current.BnfTerm);
        }

        // for efficiency reasons we examine leftBnfTerms if they are really used as leftBnfTerms
        private IEnumerable<BnfTerm> GetUsedLeftsFromTopToBottomB(UnparsableObject self)
        {
            return GetLeftsFromTopToBottomB(self).Where(bnfTerm => formatting.IsLeftBnfTermUsed(bnfTerm));
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

        public static IEnumerable<UtokenBase> FilterInsertedUtokens(this IEnumerable<UtokenBase> utokens)
        {
            InsertedUtokens leftInsertedUtokensToBeYield = null;

            foreach (UtokenBase utoken in utokens)
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
                else if (utoken is UtokenControl && ((UtokenControl)utoken).IsIndent())
                    yield return utoken;    // handle it as it were a NonOverridableSkipThrough
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

        public static IEnumerable<UtokenBase> Flatten(this IEnumerable<UtokenBase> utokens)
        {
            return utokens.SelectMany(utoken => utoken.Flatten());
        }

        public static IEnumerable<UtokenBase> ProcessControls(this IEnumerable<UtokenBase> utokens)
        {
            int indentLevel = 0;
            Stack<int> storedIndentLevel = new Stack<int>();
            bool firstUtokenInLine = true;
            bool allowWhitespaceBetweenUtokens = true;
            UtokenBase prevUtoken = null;

            foreach (UtokenBase utoken in utokens)
            {
                if (utoken is UtokenControl)
                {
                    UtokenControl utokenControl = (UtokenControl)utoken;

                    switch (utokenControl.kind)
                    {
                        case UtokenControl.Kind.IncreaseIndentLevel:
                            indentLevel++;
                            break;

                        case UtokenControl.Kind.DecreaseIndentLevel:
                            indentLevel--;
                            break;

                        case UtokenControl.Kind.SetIndentLevelToNone:
                            storedIndentLevel.Push(indentLevel);
                            indentLevel = 0;
                            break;

                        case UtokenControl.Kind.RestoreIndentLevel:
                            indentLevel = storedIndentLevel.Pop();
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
                        firstUtokenInLine = true;
                    else
                    {
                        if (firstUtokenInLine)
                            yield return new UtokenIndent(indentLevel);
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
