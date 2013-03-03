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
        #region Tracing

        internal readonly static TraceSource tsRaw = new TraceSource("RAW", SourceLevels.Verbose);
        internal readonly static TraceSource tsCalculatedDeferred = new TraceSource("CALCULATED_DEFERRED", SourceLevels.Verbose);
        internal readonly static TraceSource tsCalculatedDeferredDetailed = new TraceSource("CALCULATED_DEFERRED_DETAILED", SourceLevels.Verbose);
        internal readonly static TraceSource tsFiltered = new TraceSource("FILTERED", SourceLevels.Verbose);
        internal readonly static TraceSource tsFlattened = new TraceSource("FLATTENED", SourceLevels.Verbose);
        internal readonly static TraceSource tsProcessedControls = new TraceSource("PROCESSED_CONTROLS", SourceLevels.Verbose);

#if DEBUG
        static Formatter()
        {
            tsRaw.Listeners.Clear();
            tsRaw.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(Unparser.logDirectoryName, "01_raw.log"))));

            tsCalculatedDeferred.Listeners.Clear();
            tsCalculatedDeferred.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(Unparser.logDirectoryName, "02_calculated_deferred.log"))));

            tsCalculatedDeferredDetailed.Listeners.Clear();
            tsCalculatedDeferredDetailed.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(Unparser.logDirectoryName, "02_calculated_deferred_detailed.log"))));

            tsFiltered.Listeners.Clear();
            tsFiltered.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(Unparser.logDirectoryName, "03_filtered.log"))));

            tsFlattened.Listeners.Clear();
            tsFlattened.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(Unparser.logDirectoryName, "04_flattened.log"))));

            tsProcessedControls.Listeners.Clear();
            tsProcessedControls.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(Unparser.logDirectoryName, "05_processed_controls.log"))));
        }
#endif

        #endregion

        #region Types

        private enum State { Begin, End }

        public enum NodeLocation { FirstChild, MiddleChildOrUnknown, LastChild }

        internal class Params
        {
            public readonly BlockIndentation blockIndentation;

            public Params(BlockIndentation blockIndentation)
            {
                this.blockIndentation = blockIndentation;
            }
        }

        private delegate bool HasUtokensBeforeAfter(IEnumerable<BnfTerm> targetAndAncestors, out InsertedUtokens insertedUtokensBeforeAfter);

        #endregion

        #region State

        #region Immutable after initialization

        private readonly Formatting formatting;

        #endregion

        #region Mutable

        private UnparsableObject topAncestorCacheForLeft = UnparsableObject.NonCalculated;
        private Unparser.Direction direction;

        #endregion

        #endregion

        #region Construction

        public Formatter(Formatting formatting)
        {
            this.formatting = formatting;
        }

        private Formatter(Formatter formatter)
        {
            this.formatting = formatter.formatting;
        }

        public Formatter Spawn(NodeLocation nodeLocation = NodeLocation.MiddleChildOrUnknown)
        {
            bool isLeftMostChild =
                direction == Unparser.Direction.LeftToRight && nodeLocation == NodeLocation.FirstChild
                ||
                direction == Unparser.Direction.RightToLeft && nodeLocation == NodeLocation.LastChild;

            return new Formatter(this)
            {
                topAncestorCacheForLeft = isLeftMostChild
                    ? this.topAncestorCacheForLeft
                    : UnparsableObject.NonCalculated
            };
        }

        #endregion

        #region Public interface

        internal Unparser.Direction Direction
        {
            set
            {
                if (direction != value)
                    topAncestorCacheForLeft = UnparsableObject.NonCalculated;

                this.direction = value;
            }
        }

        /// <summary>
        /// This method needs to be fully executed before UnparseRawMiddle because this method modifies the state of Unparser and,
        /// which state is used by UnparseRawMiddle. Thus, always call this method prior to UnparseRawMiddle.
        /// </summary>
        public IReadOnlyList<UtokenBase> YieldBefore(UnparsableObject self, out Params @params)
        {
            /*
             * To achieve fully execution before UnparseRawMiddle, this method is not an iterator block rather populates a list.
             * Returning IReadOnlyList instead of IEnumerable is just an explicit guarantee to the caller to ensure that
             * the returned utokens does not need to be iterated through e.g. by converting it to a list in order to achieve full execution.
             * */

            Unparser.tsUnparse.Debug("YieldBetweenAndBefore");

            var utokens = new List<UtokenBase>();

            BlockIndentation blockIndentation;

            if (direction == Unparser.Direction.LeftToRight)
            {
                utokens.AddRange(YieldBetween(self));

                blockIndentation = BlockIndentation.Null;
                utokens.AddRange(YieldIndentationLeft(self, ref blockIndentation));
            }
            else
            {
                // TODO
//                blockIndentation = IsRight(self) ? BlockIndentation.Defer() : null;
                blockIndentation = BlockIndentation.Defer();
                utokens.AddRange(YieldIndentationRight(self, blockIndentation));
            }

            @params = new Params(blockIndentation);

            HasUtokensBeforeAfter hasUtokensBefore = direction == Unparser.Direction.LeftToRight
                ? (HasUtokensBeforeAfter)formatting.TryGetUtokensLeft
                : (HasUtokensBeforeAfter)formatting.TryGetUtokensRight;

            InsertedUtokens insertedUtokensBefore;
            if (hasUtokensBefore(GetSelfAndAncestorsB(self), out insertedUtokensBefore))
            {
                Unparser.tsUnparse.Debug("inserted utokens: {0}", insertedUtokensBefore);
                utokens.Add(insertedUtokensBefore);
            }

            return utokens;
        }

        private void UpdateTopAncestorCacheForLeftOnTheFly(UnparsableObject self)
        {
            if (self.Parent == null)
                topAncestorCacheForLeft = null;     // self is root node
            else if (!self.IsLeftSiblingCalculated || self.LeftSibling != null)
                topAncestorCacheForLeft = self;
        }

        private IReadOnlyList<UtokenBase> YieldBetween(UnparsableObject self)
        {
            UpdateTopAncestorCacheForLeftOnTheFly(self);

            try
            {
                // NOTE: topAncestorCacheForLeft may get updated by _YieldBetween
                // NOTE: ToList is needed to fully evaluate the called function, so we can catch the exception
                return _YieldBetween(self, formatter: this, formatting: this.formatting).ToList();
            }
            catch (NonCalculatedException)
            {
                // top left node or someone in the chain is non-calculated -> defer execution
                return new[]
                {
                    new DeferredUtokens(
                        () => _YieldBetween(self, formatter: null, formatting: this.formatting),
                        helpSelf: self,
                        helpMessage: "YieldBetween"
                        )
                };
            }
        }

        /// <exception cref="UnparsableObject.NonCalculatedException">
        /// If topLeft is non-calculated or thrown out.
        /// </exception>
        private static IEnumerable<UtokenBase> _YieldBetween(UnparsableObject self, Formatter formatter, Formatting formatting)
        {
            // NOTE: topAncestorCacheForLeft may get updated by GetUsedLeftsFromTopToBottomB

            foreach (BnfTerm leftBnfTerm in GetUsedLeftsFromTopToBottomB(self, formatter, formatting))
            {
                InsertedUtokens insertedUtokensBetween;
                if (formatting.TryGetUtokensBetween(leftBnfTerm, GetSelfAndAncestorsB(self), out insertedUtokensBetween))
                {
                    Unparser.tsUnparse.Debug("inserted utokens: {0}", insertedUtokensBetween);
                    yield return insertedUtokensBetween;
                }
            }
        }

        private IReadOnlyList<UtokenBase> YieldIndentationLeft(UnparsableObject self, ref BlockIndentation blockIndentation)
        {
            UpdateTopAncestorCacheForLeftOnTheFly(self);

            try
            {
                // NOTE: topAncestorCacheForLeft may get updated by _YieldBetween
                // NOTE: ToList is needed to fully evaluate the called function, so we can catch the exception
                return _YieldIndentationLeft(self, ref blockIndentation, direction, this.formatting, formatter: this).ToList();
            }
            catch (NonCalculatedException)
            {
                // top left node or someone in the chain is non-calculated -> defer execution
                Debug.Assert(blockIndentation.IsNull() || blockIndentation.IsDeferred());

                if (blockIndentation.IsNull())
                    blockIndentation = BlockIndentation.Defer();

                Debug.Assert(blockIndentation.IsDeferred());

                BlockIndentation _blockIndentation = blockIndentation;

                return new[]
                {
                    blockIndentation.LeftDeferred = new DeferredUtokens(
                        () => _YieldIndentationLeft(self, _blockIndentation, direction, this.formatting, formatter: null),
                        helpSelf: self,
                        helpMessage: "YieldIndentationLeft",
                        helpCalculatedObject: _blockIndentation
                        )
                };
            }
        }

        private static IEnumerable<UtokenBase> _YieldIndentationLeft(UnparsableObject self, BlockIndentation blockIndentationParameter, Unparser.Direction direction,
            Formatting formatting, Formatter formatter)
        {
#if DEBUG
            BlockIndentation originalBlockIndentationParameter = blockIndentationParameter;
#endif
            var utokens = _YieldIndentationLeft(self, ref blockIndentationParameter, direction, formatting, formatter);
#if DEBUG
            Debug.Assert(object.ReferenceEquals(blockIndentationParameter, originalBlockIndentationParameter), "unwanted change of blockIndentationParameter reference in _YieldIndentationLeft");
#endif
            return utokens;
        }

        private static IEnumerable<UtokenBase> _YieldIndentationLeft(UnparsableObject self, ref BlockIndentation blockIndentationParameter, Unparser.Direction direction,
            Formatting formatting, Formatter formatter)
        {
            BlockIndentation blockIndentation = BlockIndentation.Null;

            // NOTE: topAncestorCacheForLeft gets updated by GetUsedLeftsFromTopToBottomB

            foreach (BnfTerm leftBnfTerm in GetUsedLeftsFromTopToBottomB(self, formatter, formatting))
            {
                if (blockIndentation.IsNull() && formatting.TryGetBlockIndentation(leftBnfTerm, GetSelfAndAncestorsB(self), out blockIndentation))
                {
                    Unparser.tsUnparse.Debug("blockindentation {0} for leftBnfTerm '{1}' and for unparsable object '{2}'", blockIndentation, leftBnfTerm, self);
                    break;
                }
            }

            if (blockIndentation.IsNull() && formatting.TryGetBlockIndentation(GetSelfAndAncestorsB(self), out blockIndentation))
                Unparser.tsUnparse.Debug("blockindentation {0} for unparsable object '{1}'", blockIndentation, self);

            Debug.Assert(!blockIndentation.IsDeferred());

            if (blockIndentationParameter.IsNull())
                blockIndentationParameter = blockIndentation;
            else
            {
                Debug.Assert(blockIndentationParameter.IsDeferred());
                blockIndentationParameter.CopyKindFrom(blockIndentation);
            }

            return direction == Unparser.Direction.LeftToRight
                ? BlockIndentationToUtokenControlBefore(blockIndentation)
                : BlockIndentationToUtokenControlAfter(blockIndentation);
        }

        private static IEnumerable<UtokenControl> BlockIndentationToUtokenControlBefore(BlockIndentation blockIndentation)
        {
            if (blockIndentation == BlockIndentation.Indent)
                yield return UtokenControl.IncreaseIndentLevel;
            else if (blockIndentation == BlockIndentation.Unindent)
                yield return UtokenControl.DecreaseIndentLevel;
            else if (blockIndentation == BlockIndentation.ZeroIndent)
                yield return UtokenControl.SetIndentLevelToNone;
        }

        private static IEnumerable<UtokenControl> BlockIndentationToUtokenControlAfter(BlockIndentation blockIndentation)
        {
            if (blockIndentation == BlockIndentation.Indent)
                yield return UtokenControl.DecreaseIndentLevel;
            else if (blockIndentation == BlockIndentation.Unindent)
                yield return UtokenControl.IncreaseIndentLevel;
            else if (blockIndentation == BlockIndentation.ZeroIndent)
                yield return UtokenControl.RestoreIndentLevel;
        }

        public IEnumerable<UtokenBase> YieldAfter(UnparsableObject self, Params @params)
        {
            Unparser.tsUnparse.Debug("YieldAfter");

            HasUtokensBeforeAfter hasUtokensAfter = direction == Unparser.Direction.LeftToRight
                ? (HasUtokensBeforeAfter)formatting.TryGetUtokensRight
                : (HasUtokensBeforeAfter)formatting.TryGetUtokensLeft;

            InsertedUtokens insertedUtokensAfter;
            if (hasUtokensAfter(GetSelfAndAncestorsB(self), out insertedUtokensAfter))
            {
                Unparser.tsUnparse.Debug("inserted utokens: {0}", insertedUtokensAfter);
                yield return insertedUtokensAfter;
            }

            BlockIndentation blockIndentation = @params.blockIndentation;

            if (direction == Unparser.Direction.LeftToRight)
            {
                foreach (UtokenBase utoken in YieldIndentationRight(self, blockIndentation))
                    yield return utoken;
            }
            else
            {
                foreach (UtokenBase utoken in YieldIndentationLeft(self, ref blockIndentation))
                    yield return utoken;

                foreach (UtokenBase utoken in YieldBetween(self))
                    yield return utoken;
            }
        }

        private IEnumerable<UtokenBase> YieldIndentationRight(UnparsableObject self, BlockIndentation blockIndentation)
        {
            if (blockIndentation.IsNull())
                return new UtokenBase[0];
            else if (!blockIndentation.IsDeferred())
                return _YieldIndentationRight(self, blockIndentation, direction, this.formatting, formatter: this);
            else
            {
                return new[]
                {
                    blockIndentation.RightDeferred = new DeferredUtokens(
                        () => _YieldIndentationRight(self, blockIndentation, direction, this.formatting, formatter: null),
                        helpSelf: self,
                        helpMessage: "YieldIndentationRight",
                        helpCalculatedObject: blockIndentation
                        )
                };
            }
        }

        private static IEnumerable<UtokenBase> _YieldIndentationRight(UnparsableObject self, BlockIndentation blockIndentation, Unparser.Direction direction,
            Formatting formatting, Formatter formatter)
        {
            if (blockIndentation.IsDeferred())
            {
                // this can happen during parallel unparse or during right-to-left unparse

                if (direction == Unparser.Direction.RightToLeft && blockIndentation.LeftDeferred != null)
                {
                    // 
                    /*
                     * We are in a right-to-left unparse and this deferred right indentation depends on a deferred left indentation,
                     * so let's try to calculate the deferred left indentation, which - if succeed - will change the shared blockIndentation
                     * object state from deferred to a valid indentation.
                     * 
                     * (If the left indentation wasn't deferred then it would be calculated which means that the shared blockIndentation
                     * object won't be deferred.)
                     * */
                    blockIndentation.LeftDeferred.CalculateUtokens();

                    // either CalculateUtokens succeeded, and blockIndentation is not deferred, or CalculateUtokens threw a NonCalculatedException exception

                    Debug.Assert(!blockIndentation.IsDeferred(), "CalculateUtokens succeeded, but blockIndentation remained deferred");
                }
                else
                    throw new NonCalculatedException("YieldIndentationRight");
            }

            return direction == Unparser.Direction.LeftToRight
                ? BlockIndentationToUtokenControlAfter(blockIndentation)
                : BlockIndentationToUtokenControlBefore(blockIndentation);
        }

        public static IEnumerable<Utoken> PostProcess(IEnumerable<UtokenBase> utokens, Unparser.Direction direction, bool indentEmptyLines)
        {
            return utokens
                .DebugWriteLines(Formatter.tsRaw)
                .CalculateDeferredUtokens()
                .DebugWriteLines(Formatter.tsCalculatedDeferred)
                .FilterInsertedUtokens(direction)
                .DebugWriteLines(Formatter.tsFiltered)
                .Flatten()
                .DebugWriteLines(Formatter.tsFlattened)
                .ProcessControls(direction, indentEmptyLines)
                .DebugWriteLines(Formatter.tsProcessedControls)
                .Cast<Utoken>()
                ;
        }

        public void ResetMutableState()
        {
            topAncestorCacheForLeft = UnparsableObject.NonCalculated;
        }

        #endregion

        #region Helpers

        private static IEnumerable<UnparsableObject> GetSelfAndAncestors(UnparsableObject self)
        {
            return Util.RecurseStopBeforeNull(self, current => current.Parent);
        }

        private static IEnumerable<BnfTerm> GetSelfAndAncestorsB(UnparsableObject self)
        {
            return GetSelfAndAncestors(self).Select(current => current.BnfTerm);
        }

        private static UnparsableObject GetTopAncestorForLeft(UnparsableObject self, Formatter formatter = null)
        {
            // NOTE: topAncestorCacheForLeft will not be changed if canUseTopAncestorCacheForLeft is false, that's why we use topAncestorForLeft + the static vs. instance behavior
            UnparsableObject topAncestorForLeft;

            if (formatter == null || !UnparsableObject.IsCalculated(formatter.topAncestorCacheForLeft))
            {
                topAncestorForLeft = CalculateTopAncestorForLeft(self);

                if (formatter != null)
                    formatter.topAncestorCacheForLeft = topAncestorForLeft;
            }
            else
            {
                topAncestorForLeft = formatter.topAncestorCacheForLeft;
                //Unparser.tsUnparse.Debug(formatter.topAncestorCacheForLeft != CalculateTopAncestorForLeft(self)),
                //    "!!!!!!!! should be equal for {0}, but topAncestorCacheForLeft is '{1}' and calculated value is '{2}'", self, formatter.topAncestorCacheForLeft, CalculateTopAncestorForLeft(self));
                Debug.Assert(formatter.topAncestorCacheForLeft == CalculateTopAncestorForLeft(self));
            }

            return topAncestorForLeft;
        }

        private static UnparsableObject CalculateTopAncestorForLeft(UnparsableObject self)
        {
            return GetSelfAndAncestors(self).FirstOrDefault(current => current.LeftSibling != null);
        }

        /// <exception cref="UnparsableObject.NonCalculatedException">
        /// If topLeft is non-calculated.
        /// </exception>
        private static IEnumerable<UnparsableObject> GetLeftsFromTopToBottom(UnparsableObject self, Formatter formatter = null)
        {
            UnparsableObject topAncestorForLeft = GetTopAncestorForLeft(self, formatter);

            return topAncestorForLeft != null
                ? Util.RecurseStopBeforeNull(topAncestorForLeft.LeftSibling, current => current.RightMostChild)
                : new UnparsableObject[0];
        }

        /// <exception cref="UnparsableObject.NonCalculatedException">
        /// If topLeft is non-calculated or thrown out.
        /// </exception>
        private static IEnumerable<BnfTerm> GetLeftsFromTopToBottomB(UnparsableObject self, Formatter formatter = null)
        {
            return GetLeftsFromTopToBottom(self, formatter).Select(current => current.BnfTerm);
        }

        /// <exception cref="UnparsableObject.NonCalculatedException">
        /// If topLeft is non-calculated or thrown out.
        /// </exception>
        /// <remarks>
        /// for efficiency reasons we examine leftBnfTerms if they are really used as leftBnfTerms
        /// </remarks>
        private static IEnumerable<BnfTerm> GetUsedLeftsFromTopToBottomB(UnparsableObject self, Formatter formatter, Formatting formatting)
        {
            return GetLeftsFromTopToBottomB(self, formatter).Where(bnfTerm => formatting.IsLeftBnfTermUsed(bnfTerm));
        }

        #endregion
    }

    internal static class FormatterExtensions
    {
        public static IEnumerable<UtokenBase> CalculateDeferredUtokens(this IEnumerable<UtokenBase> utokens)
        {
#if DEBUG
            int maxBufferSizeForDebug = 0;
#endif

            var utokensBuffer = new Queue<UtokenBase>();

            /*
             * NOTE: During unparsing from right-to-left the leftmost child's left sibling will be set _after_ the last child was consumed,
             * therefore we might have unprocessed elements (deferred and normal utokens) in the buffer which could not be calculated
             * when we were consuming the last utoken, but became calculable after the last utoken has been consumed
             * (despite the fact that we did not consume another utoken in this step).
             * 
             * Basically, we should execute the body of the loop one more time after the loop. This can be done the most easily by consuming
             * an extra null element after the real utokens.
             * */

            foreach (UtokenBase _utoken in utokens.Concat(null))
            {
                utokensBuffer.Enqueue(_utoken);

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
        public static IEnumerable<UtokenBase> FilterInsertedUtokens(this IEnumerable<UtokenBase> utokens, Unparser.Direction direction)
        {
            InsertedUtokens prevInsertedUtokensToBeYield = null;
            var nonOverridableSkipThroughBuffer = new Queue<UtokenBase>();

            foreach (UtokenBase utoken in utokens.Concat(null))
            {
                if (utoken is InsertedUtokens)
                {
                    InsertedUtokens nextInsertedUtokens = (InsertedUtokens)utoken;

                    var switchToNextInsertedUtokens = new Lazy<bool>(() => IsNextStronger(prevInsertedUtokensToBeYield, nextInsertedUtokens, direction));

                    if (nextInsertedUtokens.behavior == Behavior.Overridable)
                    {
                        prevInsertedUtokensToBeYield = switchToNextInsertedUtokens.Value ? nextInsertedUtokens : prevInsertedUtokensToBeYield;
                    }
                    else if (nextInsertedUtokens.behavior == Behavior.NonOverridableSkipThrough)
                    {
                        if (direction == Unparser.Direction.LeftToRight)
                            yield return nextInsertedUtokens;
                        else
                            nonOverridableSkipThroughBuffer.Enqueue(nextInsertedUtokens);
                    }
                    else if (nextInsertedUtokens.behavior == Behavior.NonOverridableSeparator)
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
                    if (direction == Unparser.Direction.LeftToRight)
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

        public static IEnumerable<UtokenBase> ProcessControls(this IEnumerable<UtokenBase> utokens, Unparser.Direction direction, bool indentEmptyLines)
        {
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
            foreach (UtokenBase utoken in utokens.Concat(null))
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
                else
                {
                    if (direction == Unparser.Direction.RightToLeft && IsLineSeparator(utoken) && (indentEmptyLines || !IsLineSeparator(prevNotControlUtoken)))
                        yield return new UtokenIndent(indentLevelForCurrentLine);

                    indentLevelForCurrentLine = indentLevel;

                    if (direction == Unparser.Direction.LeftToRight && IsLineSeparator(prevNotControlUtoken) && (indentEmptyLines || !IsLineSeparator(utoken)))
                        yield return new UtokenIndent(indentLevelForCurrentLine);

                    if (allowWhitespaceBetweenUtokens && prevNotControlUtoken != null && utoken != null && !IsWhitespace(prevNotControlUtoken) && !IsWhitespace(utoken))
                        yield return UtokenWhitespace.WhiteSpaceBetweenUtokens;

                    if (utoken != null)
                        yield return utoken;

                    allowWhitespaceBetweenUtokens = true;
                    prevNotControlUtoken = utoken;
                }
            }
        }

        private static bool IsLineSeparator(UtokenBase utoken)
        {
            return utoken == UtokenWhitespace.NewLine || utoken == UtokenWhitespace.EmptyLine || utoken == null;
        }

        private static bool IsWhitespace(UtokenBase utoken)
        {
            return utoken is UtokenWhitespace;
        }

        private static bool IsControl(UtokenBase utoken)
        {
            return utoken is UtokenControl;
        }
    }
}
