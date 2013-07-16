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

        public enum ChildLocation { Unknown, First, Middle, Last, Only }

        internal class Params
        {
            public readonly BlockIndentation blockIndentation;

            public Params(BlockIndentation blockIndentation)
            {
                this.blockIndentation = blockIndentation;
            }
        }

        private delegate InsertedUtokens GetUtokensBeforeAfter(UnparsableObject target);

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

        private Formatter(Formatter that)
        {
            this.formatting = that.formatting;
            this.direction = that.direction;
        }

        public Formatter Spawn(ChildLocation childLocation = ChildLocation.Unknown)
        {
            return Spawn(child: null, childLocation: childLocation);
        }

        public Formatter Spawn(UnparsableObject child, ChildLocation childLocation = ChildLocation.Unknown)
        {
            if (childLocation == ChildLocation.Unknown)
            {
                return new Formatter(this)
                {
                    topAncestorCacheForLeft = UnparsableObject.NonCalculated
                };
            }
            else
            {
                bool isLeftMostChild =
                    childLocation == ChildLocation.Only
                    ||
                    direction == Unparser.Direction.LeftToRight && childLocation == ChildLocation.First
                    ||
                    direction == Unparser.Direction.RightToLeft && childLocation == ChildLocation.Last;

                return new Formatter(this)
                {
                    topAncestorCacheForLeft = isLeftMostChild
                        ? this.topAncestorCacheForLeft
                        : (child ?? UnparsableObject.NonCalculated)
                };
            }
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

            Unparser.tsUnparse.Debug("YieldBefore");

            UpdateTopAncestorCacheForLeftOnTheFly(self);

            var utokens = new List<UtokenBase>();
            BlockIndentation blockIndentation = BlockIndentation.ToBeSet;

            if (direction == Unparser.Direction.LeftToRight)
            {
                utokens.AddRange(YieldBetween(self));
                utokens.AddRange(YieldIndentationLeft(self, ref blockIndentation));
            }
            else
            {
                utokens.AddRange(YieldIndentationRight(self, ref blockIndentation));
            }

            @params = new Params(blockIndentation);

            GetUtokensBeforeAfter getUtokensBefore = direction == Unparser.Direction.LeftToRight
                ? (GetUtokensBeforeAfter)formatting._GetUtokensLeft
                : (GetUtokensBeforeAfter)formatting._GetUtokensRight;

            InsertedUtokens insertedUtokensBefore = getUtokensBefore(self);

            if (insertedUtokensBefore != InsertedUtokens.None)
            {
                Unparser.tsUnparse.Debug("inserted utokens: {0}", insertedUtokensBefore);
                utokens.Add(insertedUtokensBefore);
            }

            return utokens;
        }

        public IEnumerable<UtokenBase> YieldAfter(UnparsableObject self, Params @params)
        {
            Unparser.tsUnparse.Debug("YieldAfter");

            UpdateTopAncestorCacheForLeftOnTheFly(self);

            GetUtokensBeforeAfter getUtokensAfter = direction == Unparser.Direction.LeftToRight
                ? (GetUtokensBeforeAfter)formatting._GetUtokensRight
                : (GetUtokensBeforeAfter)formatting._GetUtokensLeft;

            InsertedUtokens insertedUtokensAfter = getUtokensAfter(self);

            if (insertedUtokensAfter != InsertedUtokens.None)
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
                foreach (UtokenBase utoken in YieldIndentationLeft(self, blockIndentation))
                    yield return utoken;

                foreach (UtokenBase utoken in YieldBetween(self))
                    yield return utoken;
            }
        }

        public static IEnumerable<Utoken> PostProcess(IEnumerable<UtokenBase> utokens, IPostProcessHelper postProcessHelper)
        {
            return utokens                                      .DebugWriteLines(tsRaw)
                .CalculateDeferredUtokens(postProcessHelper)    .DebugWriteLines(tsCalculatedDeferred)
                .FilterInsertedUtokens(postProcessHelper)       .DebugWriteLines(tsFiltered)
                .Flatten()                                      .DebugWriteLines(tsFlattened)
                .ProcessControls(postProcessHelper)             .DebugWriteLines(tsProcessedControls)
                .Decorate(postProcessHelper)
                .Cast<Utoken>()
                ;
        }

        public void ResetMutableState()
        {
            topAncestorCacheForLeft = UnparsableObject.NonCalculated;
        }

        #endregion

        #region Yield logic for indentation and between

        private void UpdateTopAncestorCacheForLeftOnTheFly(UnparsableObject self)
        {
            if (self.SyntaxParent == null)
                topAncestorCacheForLeft = null;     // self is root node
            else if (self.IsLeftSiblingCalculated && self.LeftSibling != null)
                topAncestorCacheForLeft = self;
            else
                topAncestorCacheForLeft = UnparsableObject.NonCalculated;
        }

        private IReadOnlyList<UtokenBase> YieldBetween(UnparsableObject self)
        {
            Unparser.tsUnparse.Debug("YieldBetween");

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
                        self: self,
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

            UnparsableObject leftObject = GetLeft(self);

            if (leftObject != null)
            {
                InsertedUtokens insertedUtokensBetween = formatting._GetUtokensBetween(leftObject, self);

                if (insertedUtokensBetween != InsertedUtokens.None)
                {
                    Unparser.tsUnparse.Debug("inserted utokens: {0}", insertedUtokensBetween);
                    yield return insertedUtokensBetween;
                }
            }
        }

        private IReadOnlyList<UtokenBase> YieldIndentationLeft(UnparsableObject self, BlockIndentation blockIndentation)
        {
#if DEBUG
            BlockIndentation originalBlockIndentation = blockIndentation;
#endif
            var utokens = YieldIndentationLeft(self, ref blockIndentation);
#if DEBUG
            Debug.Assert(object.ReferenceEquals(blockIndentation, originalBlockIndentation), "unwanted change of blockIndentationParameter reference in YieldIndentationLeft");
#endif
            return utokens;
        }

        private IReadOnlyList<UtokenBase> YieldIndentationLeft(UnparsableObject self, ref BlockIndentation blockIndentation)
        {
            Unparser.tsUnparse.Debug("YieldIndentationLeft");

            try
            {
                // NOTE: topAncestorCacheForLeft may get updated by YieldIndentation
                // NOTE: ToList is needed to fully evaluate the called function, so we can catch the exception
                return _YieldIndentation(self, ref blockIndentation, direction, this.formatting, formatter: this, left: true).ToList();
            }
            catch (NonCalculatedException)
            {
                // top left node or someone in the chain is non-calculated -> defer execution
                Debug.Assert(blockIndentation == BlockIndentation.ToBeSet || blockIndentation.IsDeferred());

                if (blockIndentation == BlockIndentation.ToBeSet)
                    blockIndentation = BlockIndentation.Defer();

                Debug.Assert(blockIndentation.IsDeferred());

                BlockIndentation _blockIndentation = blockIndentation;

                return new[]
                {
                    blockIndentation.LeftDeferred = new DeferredUtokens(
                        () => _YieldIndentation(self, _blockIndentation, direction, this.formatting, formatter: null, left: true),
                        self: self,
                        helpMessage: "YieldIndentationLeft",
                        helpCalculatedObject: _blockIndentation
                        )
                };
            }
        }

        private IEnumerable<UtokenBase> YieldIndentationRight(UnparsableObject self, BlockIndentation blockIndentation)
        {
#if DEBUG
            BlockIndentation originalBlockIndentation = blockIndentation;
#endif
            var utokens = YieldIndentationRight(self, ref blockIndentation);
#if DEBUG
            Debug.Assert(object.ReferenceEquals(blockIndentation, originalBlockIndentation), "unwanted change of blockIndentationParameter reference in YieldIndentationRight");
#endif
            return utokens;
        }

        private IEnumerable<UtokenBase> YieldIndentationRight(UnparsableObject self, ref BlockIndentation blockIndentation)
        {
            Unparser.tsUnparse.Debug("YieldIndentationRight");

            try
            {
                // NOTE: topAncestorCacheForLeft may get updated by YieldIndentation
                // NOTE: ToList is needed to fully evaluate the called function, so we can catch the exception
                return _YieldIndentation(self, ref blockIndentation, direction, this.formatting, formatter: this, left: false).ToList();
            }
            catch (NonCalculatedException)
            {
                // top left node or someone in the chain is non-calculated -> defer execution
                Debug.Assert(blockIndentation == BlockIndentation.ToBeSet || blockIndentation.IsDeferred());

                if (blockIndentation == BlockIndentation.ToBeSet)
                    blockIndentation = BlockIndentation.Defer();

                Debug.Assert(blockIndentation.IsDeferred());

                BlockIndentation _blockIndentation = blockIndentation;

                return new[]
                {
                    blockIndentation.RightDeferred = new DeferredUtokens(
                        () => _YieldIndentation(self, _blockIndentation, direction, this.formatting, formatter: null, left: false),
                        self: self,
                        helpMessage: "YieldIndentationRight",
                        helpCalculatedObject: _blockIndentation
                        )
                };
            }
        }

        /// <exception cref="UnparsableObject.NonCalculatedException">
        /// If topLeft is non-calculated or thrown out.
        /// </exception>
        private static IEnumerable<UtokenBase> _YieldIndentation(UnparsableObject self, BlockIndentation blockIndentationParameter, Unparser.Direction direction,
            Formatting formatting, Formatter formatter, bool left)
        {
#if DEBUG
            BlockIndentation originalBlockIndentationParameter = blockIndentationParameter;
#endif
            var utokens = _YieldIndentation(self, ref blockIndentationParameter, direction, formatting, formatter, left);
#if DEBUG
            Debug.Assert(object.ReferenceEquals(blockIndentationParameter, originalBlockIndentationParameter),
                string.Format("unwanted change of blockIndentationParameter reference in _YieldIndentation ({0})", left ? "left" : "right"));
#endif
            return utokens;
        }

        /// <exception cref="UnparsableObject.NonCalculatedException">
        /// If topLeft is non-calculated or thrown out.
        /// </exception>
        private static IEnumerable<UtokenBase> _YieldIndentation(UnparsableObject self, ref BlockIndentation blockIndentationParameter, Unparser.Direction direction,
            Formatting formatting, Formatter formatter, bool left)
        {
            if (!left && direction == Unparser.Direction.RightToLeft && blockIndentationParameter.IsDeferred() && blockIndentationParameter.LeftDeferred != null)
            {
                /*
                 * We are in a right-to-left unparse and this deferred right indentation depends on a deferred left indentation,
                 * so let's try to calculate the deferred left indentation, which - if succeed - will change the shared blockIndentation
                 * object state from deferred to a valid indentation.
                 * 
                 * (If the left indentation wasn't deferred then it would be calculated which means that the shared blockIndentation
                 * object won't be deferred.)
                 * */
                blockIndentationParameter.LeftDeferred.CalculateUtokens();

                // either CalculateUtokens succeeded, and blockIndentation is not deferred, or CalculateUtokens threw a NonCalculatedException exception

                Debug.Assert(!blockIndentationParameter.IsDeferred(), "CalculateUtokens succeeded, but blockIndentation remained deferred");
            }

            if (blockIndentationParameter == BlockIndentation.ToBeSet || blockIndentationParameter.IsDeferred())
            {
                UnparsableObject leftObject = GetLeft(self);
                BlockIndentation blockIndentation = formatting._GetBlockIndentation(leftObject, self);

                // NOTE: topAncestorCacheForLeft gets updated by GetUsedLeftsFromTopToBottomB

                if (blockIndentation != BlockIndentation.IndentNotNeeded)
                    Unparser.tsUnparse.Debug("blockindentation {0} for leftTarget '{1}' and for target '{2}'", blockIndentation, leftObject, self);

                Debug.Assert(!blockIndentation.IsDeferred());

                if (blockIndentationParameter == BlockIndentation.ToBeSet)
                    blockIndentationParameter = blockIndentation;
                else
                    blockIndentationParameter.CopyKindFrom(blockIndentation);
            }

            if (direction == Unparser.Direction.LeftToRight)
            {
                return left
                    ? BlockIndentationToUtokenControlBefore(blockIndentationParameter)
                    : BlockIndentationToUtokenControlAfter(blockIndentationParameter);
            }
            else
            {
                return left
                    ? BlockIndentationToUtokenControlAfter(blockIndentationParameter)
                    : BlockIndentationToUtokenControlBefore(blockIndentationParameter);
            }
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

        #endregion

        #region Helpers

        private static IEnumerable<UnparsableObject> GetSelfAndAncestors(UnparsableObject self)
        {
            return Util.RecurseStopBeforeNull(self, current => current.SyntaxParent);
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
                : Enumerable.Empty<UnparsableObject>();
        }

        /// <exception cref="UnparsableObject.NonCalculatedException">
        /// If topLeft is non-calculated.
        /// </exception>
        private static UnparsableObject GetLeft(UnparsableObject self, Formatter formatter = null)
        {
            return GetLeftsFromTopToBottom(self, formatter).LastOrDefault();
        }

        #endregion
    }
}
