#region License
/*
    This file is part of Sarcasm.

    Copyright 2012-2013 Dávid Németi

    Sarcasm is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Sarcasm is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Sarcasm.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

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
using Sarcasm.GrammarAst;
using Sarcasm.Utility;

using Grammar = Sarcasm.GrammarAst.Grammar;

namespace Sarcasm.Unparsing
{
    internal static class FormatterExtensions
    {
        public static IEnumerable<UtokenBase> CalculateDeferredUtokens(this IEnumerable<UtokenBase> utokens, IPostProcessHelper postProcessHelper)
        {
#if DEBUG
            int maxBufferSizeForDebug = 0;
#endif

            var utokensBuffer = new Queue<UtokenBase>();

            /*
             * When dealing with right-to-left unparsing, in case of left-context-specific indentation (which is deferred in this case)
             * we have to consume more than one utokens in order to be able to calculate the indentation. Thus, we prefetch some more utokens
             * instead of consuming them one-by-one. This prefetch behavior causes a significant speed-up.
             * */
            const int initialPrefetchCountForDeferred = 100;
            int prefetchCount = 0;
            int prefetchedCount = 0;

            /*
             * NOTE: During unparsing from right-to-left the leftmost child's left sibling will be set _after_ the last child was consumed,
             * therefore we might have unprocessed elements (deferred and normal utokens) in the buffer which could not be calculated
             * when we were consuming the last utoken, but became calculable after the last utoken has been consumed
             * (despite the fact that we did not consume another utoken in this step).
             * 
             * Basically, we should execute the body of the loop one more time after the loop. This can be done the most easily by consuming
             * an extra null element after the real utokens.
             * */

            foreach (UtokenBase _utoken in utokens.Concat((UtokenBase)null))
            {
                utokensBuffer.Enqueue(_utoken);

                #region Prefetch when right-to-left

                if (postProcessHelper.Direction == Unparser.Direction.RightToLeft && prefetchedCount < prefetchCount && _utoken != null)
                {
                    prefetchedCount++;
                    continue;
                }
                else
                    prefetchedCount = 0;

                #endregion

#if DEBUG
                maxBufferSizeForDebug = Math.Max(maxBufferSizeForDebug, utokensBuffer.Count);
#endif

                Formatter.tsCalculatedDeferredDetailed.Debug("Consumed and enqueued utoken: {0} (queue size is now {1})",
                    _utoken != null ? _utoken.ToString() : "extra <<NULL>> utoken after last real utoken", utokensBuffer.Count);

            LProcessBufferWithoutConsumingNewUtoken:

                if (utokensBuffer.Peek() is DeferredUtokens)
                {
                    DeferredUtokens deferredUtokens = (DeferredUtokens)utokensBuffer.Peek();

                    IEnumerable<UtokenBase> calculatedUtokens;

                    try
                    {
                        calculatedUtokens = deferredUtokens.GetUtokens();
                        utokensBuffer.Dequeue();
//                        postProcessHelper.UnlinkChildFromChildPrevSiblingIfNotFullUnparse(deferredUtokens.Self);
                        prefetchCount = 0;

                        Formatter.tsCalculatedDeferredDetailed.Debug("Calculated: {0}", deferredUtokens);
                    }
                    catch (NonCalculatedException)
                    {
                        Formatter.tsCalculatedDeferredDetailed.Debug("Tried to calculate but failed: {0}", deferredUtokens);

                        if (_utoken == null)
                        {
                            // after the last real utoken every deferred utokens should be calculable
                            Formatter.tsCalculatedDeferredDetailed.Debug("ERROR: Calculate should not fail after last real token");
#if DEBUG
                            DebugUnprocessedBuffer(utokensBuffer, maxBufferSizeForDebug);
#endif
                            throw;
                        }

                        if (prefetchCount == 0)
                            prefetchCount = initialPrefetchCountForDeferred;
                        else
                            prefetchCount *= 2;

                        continue;
                    }

                    Formatter.tsCalculatedDeferredDetailed.Debug("Successfully calculated: {0}", deferredUtokens);
                    Formatter.tsCalculatedDeferredDetailed.Indent();

                    foreach (UtokenBase calculatedUtoken in calculatedUtokens)
                    {
                        Formatter.tsCalculatedDeferredDetailed.Debug("Yielded calculated: {0}", calculatedUtoken);
                        yield return calculatedUtoken;
                    }

                    Formatter.tsCalculatedDeferredDetailed.Unindent();
                }
                else if (utokensBuffer.Peek() == null)
                {
                    // the extra null element after the real elements -> just remove it from the queue
                    utokensBuffer.Dequeue();
                }
                else
                {
                    Formatter.tsCalculatedDeferredDetailed.Debug("Yielded normal: {0}", utokensBuffer.Peek());
                    yield return utokensBuffer.Dequeue();
                }

                if (utokensBuffer.Count > 0)
                {
                    Formatter.tsCalculatedDeferredDetailed.Debug("Queue has elements -> process without consuming new utoken (queue size is {0})", utokensBuffer.Count);
                    goto LProcessBufferWithoutConsumingNewUtoken;
                }
            }

#if DEBUG
            DebugUnprocessedBuffer(utokensBuffer, maxBufferSizeForDebug);
#endif

            Debug.Assert(utokensBuffer.Count == 0, "unprocessed items in buffer (non-calculated unparsable objects remained)");
        }

        [Conditional("DEBUG")]
        private static void DebugUnprocessedBuffer(IEnumerable<UtokenBase> utokensBuffer, int maxBufferSizeForDebug)
        {
            Formatter.tsCalculatedDeferredDetailed.Debug("");
            Formatter.tsCalculatedDeferredDetailed.Debug("");
            Formatter.tsCalculatedDeferredDetailed.Debug("Max buffer size was: {0}", maxBufferSizeForDebug);

            if (utokensBuffer.Any())
            {
                Formatter.tsCalculatedDeferredDetailed.Debug("");
                Formatter.tsCalculatedDeferredDetailed.Debug("Buffer has unprocessed elements: ", utokensBuffer.Count());

                Formatter.tsCalculatedDeferredDetailed.Indent();

                foreach (UtokenBase utokenUnprocessed in utokensBuffer)
                {
                    Formatter.tsCalculatedDeferredDetailed.Debug(utokenUnprocessed);
                }

                Formatter.tsCalculatedDeferredDetailed.Unindent();
            }
        }

        /*
         * utokens contains several InsertedUtokens "sessions" which consist of "Right", "Left" and "Between" InsertedUtokens.
         * A session looks like this: (Right)*((Between)?(Left)?)*
         * 
         * Note that "Between" and "Left" InsertedUtokens are mixed with each other.
         * 
         * "Right" InsertedUtokens are handled so that in case of equal priorities we will choose the right one.
         * "Between" InsertedUtokens are handled so that in case of equal priorities we will choose the left one.
         * "Left" InsertedUtokens are handled so that in case of equal priorities we will choose the left one.
         * 
         * Since "Between" and "Left" InsertedUtokens are handled in the same way, we can handle them mixed.
         * 
         * We handle the InsertedUtokens like this in order to ensure that the InsertedUtokens belonging to the
         * outer (in sense of structure) bnfterms always being preferred against inner bnfterms in case of equal priorities.
         * 
         * InsertedUtokens belonging to the same bnfterm with equal priorities are handled so that the several kind
         * of InsertedUtokens have strength in descending order: Between, Left, Right
         * */
        public static IEnumerable<UtokenBase> FilterInsertedUtokens(this IEnumerable<UtokenBase> utokens, IPostProcessHelper postProcessHelper)
        {
            InsertedUtokens prevInsertedUtokensToBeYield = null;
            var nonOverridableSkipThroughBuffer = new Queue<UtokenBase>();

            foreach (UtokenBase utoken in utokens.Concat((UtokenBase)null))
            {
                if (utoken is InsertedUtokens)
                {
                    InsertedUtokens nextInsertedUtokens = (InsertedUtokens)utoken;

                    var switchToNextInsertedUtokens = new Lazy<bool>(() => IsNextStronger(prevInsertedUtokensToBeYield, nextInsertedUtokens, postProcessHelper.Direction));

                    if (nextInsertedUtokens.Behavior == Behavior.Overridable)
                    {
                        prevInsertedUtokensToBeYield = switchToNextInsertedUtokens.Value ? nextInsertedUtokens : prevInsertedUtokensToBeYield;
                    }
                    else if (nextInsertedUtokens.Behavior == Behavior.NonOverridableSkipThrough)
                    {
                        if (postProcessHelper.Direction == Unparser.Direction.LeftToRight)
                            yield return nextInsertedUtokens;
                        else
                            nonOverridableSkipThroughBuffer.Enqueue(nextInsertedUtokens);
                    }
                    else if (nextInsertedUtokens.Behavior == Behavior.NonOverridableSeparator)
                    {
                        if (!switchToNextInsertedUtokens.Value)
                            yield return prevInsertedUtokensToBeYield;

                        while (nonOverridableSkipThroughBuffer.Count > 0)
                            yield return nonOverridableSkipThroughBuffer.Dequeue();

                        yield return nextInsertedUtokens;
                        prevInsertedUtokensToBeYield = null;
                    }
                }
                else if (utoken is UtokenControl && ((UtokenControl)utoken).IsIndent())
                {
                    // handle it as it were a NonOverridableSkipThrough
                    if (postProcessHelper.Direction == Unparser.Direction.LeftToRight)
                        yield return utoken;
                    else
                        nonOverridableSkipThroughBuffer.Enqueue(utoken);
                }
                else
                {
                    if (prevInsertedUtokensToBeYield != null)
                    {
                        yield return prevInsertedUtokensToBeYield;
                        prevInsertedUtokensToBeYield = null;
                    }

                    while (nonOverridableSkipThroughBuffer.Count > 0)
                        yield return nonOverridableSkipThroughBuffer.Dequeue();

                    if (utoken != null)
                        yield return utoken;
                }
            }
        }

        private static bool IsNextStronger(InsertedUtokens prevInsertedUtokens, InsertedUtokens nextInsertedUtokens, Unparser.Direction direction)
        {
            int compareResult = InsertedUtokens.Compare(prevInsertedUtokens, nextInsertedUtokens);

            if (compareResult == 0)
            {
                if (direction == Unparser.Direction.LeftToRight)
                    return prevInsertedUtokens.kind == InsertedUtokens.Kind.Right;
                else
                    return nextInsertedUtokens.kind != InsertedUtokens.Kind.Right;
            }
            else
                return compareResult < 0;
        }

        public static IEnumerable<UtokenBase> Flatten(this IEnumerable<UtokenBase> utokens)
        {
            return utokens.SelectMany(utoken => utoken.Flatten());
        }

        public static IEnumerable<Utoken> ProcessControls(this IEnumerable<UtokenBase> utokens, IPostProcessHelper postProcessHelper)
        {
            var formatter = postProcessHelper.Formatter;

            int indentLevel = 0;
            int indentLevelForCurrentLine = 0;
            Stack<int> storedIndentLevel = new Stack<int>();
            bool allowWhitespaceBetweenUtokens = true;
            UtokenBase prevNotControlUtoken = null;

            /*
             * We have to yield the indentation after consuming the last utoken (regardless of left-to-right or right-to-left unparse),
             * so we use an extra null utoken.
             * 
             * e.g.: In case of right-to-left unparse if utokens enumerable is not empty and the last processed utoken (which is the leftmost utoken)
             * is not a line separator (which is the common case) then we didn't yielded the utokenindent for the last processed line
             * (which is the topmost line), so we yield it now when processing the extra null utoken.
             * */
            foreach (UtokenBase utoken in utokens.Concat((UtokenBase)null))
            {
                if (IsControl(utoken))
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
                else if (utoken == UtokenValueControl.NoWhitespace)
                {
                    allowWhitespaceBetweenUtokens = false;
                }
                else
                {
                    if (postProcessHelper.Direction == Unparser.Direction.RightToLeft && IsLineSeparator(utoken) && (formatter.IndentEmptyLines || !IsLineSeparator(prevNotControlUtoken)))
                        yield return new UtokenIndent(indentLevelForCurrentLine);

                    indentLevelForCurrentLine = indentLevel;

                    if (postProcessHelper.Direction == Unparser.Direction.LeftToRight && IsLineSeparator(prevNotControlUtoken) && (formatter.IndentEmptyLines || !IsLineSeparator(utoken)))
                        yield return new UtokenIndent(indentLevelForCurrentLine);

                    if (allowWhitespaceBetweenUtokens && prevNotControlUtoken != null && utoken != null && !IsWhitespace(prevNotControlUtoken) && !IsWhitespace(utoken))
                        yield return UtokenWhitespace.WhiteSpaceBetweenUtokens();

                    if (utoken != null)
                        yield return (Utoken)utoken;

                    allowWhitespaceBetweenUtokens = true;
                    prevNotControlUtoken = utoken;
                }
            }
        }

        private static bool IsLineSeparator(UtokenBase utoken)
        {
            return utoken.IsNewLine() || utoken.IsEmptyLine() || utoken == null;
        }

        private static bool IsWhitespace(UtokenBase utoken)
        {
            return utoken is UtokenWhitespace;
        }

        private static bool IsControl(UtokenBase utoken)
        {
            return utoken is UtokenControl;
        }

        public static IEnumerable<Utoken> Decorate(this IEnumerable<Utoken> utokens, IPostProcessHelper postProcessHelper)
        {
            var formatter = postProcessHelper.Formatter;

            foreach (Utoken utoken in utokens)
            {
                ((UtokenBase)utoken).Decoration = formatter.GetDecoration(utoken);
                yield return utoken;
            }
        }
    }
}
