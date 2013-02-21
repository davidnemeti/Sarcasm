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
            public readonly BlockIndentation blockIndentation;
            public readonly IEnumerable<InsertedUtokens> utokensBetweenAndBefore;

            public Params(ICollection<InsertedUtokens> utokensBetweenAndBefore, BlockIndentation blockIndentation)
            {
                this.utokensBetweenAndBefore = utokensBetweenAndBefore;
                this.blockIndentation = blockIndentation;
            }
        }

        #endregion

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

            BlockIndentation blockIndentation = null;

            foreach (BnfTerm leftBnfTerm in leftBnfTermsFromTopToBottom)
            {
                if (blockIndentation == null && formatting.HasBlockIndentation(leftBnfTerm, selfAndAncestors, out blockIndentation))
                    Unparser.tsUnparse.Debug("blockindentation {0} for leftBnfTerm '{1}'", blockIndentation, leftBnfTerm);

                InsertedUtokens insertedUtokensBetween;
                if (formatting.HasUtokensBetween(leftBnfTerm, selfAndAncestors, out insertedUtokensBetween))
                    utokensBetweenAndBefore.Add(insertedUtokensBetween);
            }

            if (blockIndentation == null && formatting.HasBlockIndentation(selfAndAncestors, out blockIndentation))
                Unparser.tsUnparse.Debug("blockindentation {0}", blockIndentation);

            InsertedUtokens insertedUtokensBefore;
            if (formatting.HasUtokensBefore(selfAndAncestors, out insertedUtokensBefore))
                utokensBetweenAndBefore.Add(insertedUtokensBefore);

            beginParams = new Params(utokensBetweenAndBefore, blockIndentation);
        }

        public void EndBnfTerm(BnfTerm bnfTerm)
        {
            if (lastState != State.End)
                leftBnfTermsFromTopToBottom.Clear();

            if (formatting.IsLeftBnfTermUsed(bnfTerm))
                leftBnfTermsFromTopToBottom.Insert(0, bnfTerm);  // for efficiency reasons we only store bnfTerm as leftBnfTerm if it is really a leftBnfTerm

            selfAndAncestors.Pop();
            lastState = State.End;
        }

        public IEnumerable<UtokenBase> YieldBefore(Params @params)
        {
            if (@params.blockIndentation == BlockIndentation.Indent)
                yield return UtokenControl.IncreaseIndentLevel;
            else if (@params.blockIndentation == BlockIndentation.Unindent)
                yield return UtokenControl.DecreaseIndentLevel;
            else if (@params.blockIndentation == BlockIndentation.NoIndent)
                yield return UtokenControl.SetIndentLevelToNone;

            foreach (UtokenBase utokenBetweenOrBefore in @params.utokensBetweenAndBefore)
            {
                Unparser.tsUnparse.Debug("inserted utokens: {0}", utokenBetweenOrBefore);
                yield return utokenBetweenOrBefore;
            }
        }

        public IEnumerable<UtokenBase> YieldAfter(Params @params)
        {
            InsertedUtokens insertedUtokensAfter;
            if (formatting.HasUtokensAfter(selfAndAncestors, out insertedUtokensAfter))
            {
                Unparser.tsUnparse.Debug("inserted utokens: {0}", insertedUtokensAfter);
                yield return insertedUtokensAfter;
            }

            if (@params.blockIndentation == BlockIndentation.Indent)
                yield return UtokenControl.DecreaseIndentLevel;
            else if (@params.blockIndentation == BlockIndentation.Unindent)
                yield return UtokenControl.IncreaseIndentLevel;
            else if (@params.blockIndentation == BlockIndentation.NoIndent)
                yield return UtokenControl.RestoreIndentLevel;
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
