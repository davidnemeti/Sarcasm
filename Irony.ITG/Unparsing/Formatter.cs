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
using Irony.ITG;
using Irony.ITG.Ast;

using Grammar = Irony.ITG.Ast.Grammar;

namespace Irony.ITG.Unparsing
{
    internal class Formatter
    {
        internal const string unfilteredDebugCategory = "UNFILTERED";
        internal const string filteredDebugCategory = "FILTERED";
        internal const string flattenedDebugCategory = "FLATTENED";
        internal const string processedDebugCategory = "PROCESSED";

        private enum State { Begin, End }

        private readonly Formatting formatting;

        IList<BnfTerm> leftBnfTermsFromTopToBottom = new List<BnfTerm>();
        State lastState = State.Begin;

        public Formatter(Formatting formatting)
        {
            this.formatting = formatting;
        }

        public IEnumerable<Utoken> Begin(BnfTerm bnfTerm)
        {
            lastState = State.Begin;

            foreach (BnfTerm leftBnfTerm in leftBnfTermsFromTopToBottom)
            {
                InsertedUtokens insertedUtokensBetween;
                if (formatting.HasUtokensBetween(leftBnfTerm, bnfTerm, out insertedUtokensBetween))
                {
                    yield return insertedUtokensBetween;
                    Debug.WriteLine(string.Format("inserted utokens: {0}", insertedUtokensBetween), Unparser.unparseDebugCategory);
                }
            }

            InsertedUtokens insertedUtokensBefore;
            if (formatting.HasUtokensBefore(bnfTerm, out insertedUtokensBefore))
            {
                yield return insertedUtokensBefore;
                Debug.WriteLine(string.Format("inserted utokens: {0}", insertedUtokensBefore), Unparser.unparseDebugCategory);
            }
        }

        public IEnumerable<Utoken> End(BnfTerm bnfTerm)
        {
            if (lastState != State.End)
                leftBnfTermsFromTopToBottom.Clear();

            if (formatting.IsLeftBnfTermOfABetweenPair(bnfTerm))
                leftBnfTermsFromTopToBottom.Insert(0, bnfTerm);  // for efficiency reasons we only store bnfTerm as leftBnfTerm if it is really a leftBnfTerm

            lastState = State.End;

            InsertedUtokens insertedUtokensAfter;
            if (formatting.HasUtokensAfter(bnfTerm, out insertedUtokensAfter))
            {
                yield return insertedUtokensAfter;
                Debug.WriteLine(string.Format("inserted utokens: {0}", insertedUtokensAfter), Unparser.unparseDebugCategory);
            }
        }

        public static IEnumerable<Utoken> PostProcess(IEnumerable<Utoken> utokens)
        {
            return utokens
                .FilterInsertedUtokens()
                .Flatten()
                .ProcessControls()
#if DEBUG
                .DebugWriteLines(Formatter.processedDebugCategory)
#endif
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
         * outer bnfterms always being preferred against inner bnfterms in case of equal priorities.
         * 
         * InsertedUtokens belonging to the same bnfterm with equal priorities are handled so that the several kind
         * of InsertedUtokens have strength in descending order: Between, Before, After
         * */

        public static IEnumerable<Utoken> FilterInsertedUtokens(this IEnumerable<Utoken> utokens)
        {
            InsertedUtokens leftInsertedUtokensToBeYield = null;

            foreach (Utoken utoken in utokens)
            {
                Debug.WriteLine(utoken, Formatter.unfilteredDebugCategory);

                if (utoken is InsertedUtokens)
                {
                    InsertedUtokens rightInsertedUtokens = (InsertedUtokens)utoken;

                    bool switchToNewInsertedUtokens = IsRightStronger(leftInsertedUtokensToBeYield, rightInsertedUtokens);

                    if (rightInsertedUtokens.overridable)
                        leftInsertedUtokensToBeYield = switchToNewInsertedUtokens ? rightInsertedUtokens : leftInsertedUtokensToBeYield;
                    else
                    {
                        if (!switchToNewInsertedUtokens)
                            yield return leftInsertedUtokensToBeYield;

                        yield return rightInsertedUtokens;
                        leftInsertedUtokensToBeYield = null;
                    }
                }
                else
                {
                    if (leftInsertedUtokensToBeYield != null)
                        yield return leftInsertedUtokensToBeYield;

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
            return utokens
#if DEBUG
                .DebugWriteLines(Formatter.filteredDebugCategory)
#endif
                .SelectMany(utoken => utoken.Flatten());
        }

        public static IEnumerable<Utoken> ProcessControls(this IEnumerable<Utoken> utokens)
        {
            int indentLevel = 0;
            int? temporaryIndentLevel = null;
            bool firstUtokenInLine = true;

            foreach (Utoken utoken in utokens)
            {
                Debug.WriteLine(utoken, Formatter.flattenedDebugCategory);

                if (utoken is UtokenControl)
                {
                    ProcessControl((UtokenControl)utoken, ref indentLevel, ref temporaryIndentLevel);
                }
                else
                {
                    if (utoken == UtokenPrimitive.NewLine || utoken == UtokenPrimitive.EmptyLine)
                    {
                        firstUtokenInLine = true;
                        temporaryIndentLevel = null;
                    }
                    else
                    {
                        if (firstUtokenInLine)
                            yield return new UtokenIndent(temporaryIndentLevel ?? indentLevel);

                        firstUtokenInLine = false;
                    }

                    yield return utoken;
                }
            }
        }

        private static void ProcessControl(UtokenControl utokenControl, ref int indentLevel, ref int? temporaryIndentLevel)
        {
            switch (utokenControl.kind)
            {
                case UtokenControl.Kind.IncreaseIndentLevel:
                    indentLevel++;
                    break;

                case UtokenControl.Kind.DecreaseIndentLevel:
                    indentLevel--;
                    break;

                case UtokenControl.Kind.SetIndentLevelToNone:
                    indentLevel = 0;
                    break;

                case UtokenControl.Kind.IncreaseIndentLevelForThisLine:
                    temporaryIndentLevel = indentLevel + 1;
                    break;

                case UtokenControl.Kind.DecreaseIndentLevelForThisLine:
                    temporaryIndentLevel = indentLevel - 1;
                    break;

                case UtokenControl.Kind.SetIndentLevelToNoneForThisLine:
                    temporaryIndentLevel = 0;
                    break;

                default:
                    throw new InvalidOperationException(string.Format("Unknown UtokenControl '{0}'", utokenControl.kind));
            }
        }
    }
}
