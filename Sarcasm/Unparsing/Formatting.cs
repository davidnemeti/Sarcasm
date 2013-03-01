using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Globalization;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.Ast;

using Grammar = Sarcasm.Ast.Grammar;

namespace Sarcasm.Unparsing
{
    public class Formatting
    {
        #region Types

        private class _AnyBnfTerm : BnfTerm
        {
            private _AnyBnfTerm()
                : base("AnyBnfTerm")
            {
            }

            public static readonly _AnyBnfTerm Instance = new _AnyBnfTerm();
        }

        #endregion

        #region Default values

        private const double priorityDefault = 0;
        private const double anyPriorityDefault = double.NegativeInfinity;

        private const Behavior behaviorDefault = Behavior.Overridable;
        private const Behavior anyBehaviorDefault = Behavior.Overridable;

        private static readonly string newLineDefault = Environment.NewLine;
        private const string spaceDefault = " ";
        private const string tabDefault = "\t";
        private static readonly string indentUnitDefault = string.Concat(Enumerable.Repeat(spaceDefault, 4));
        private const string whiteSpaceBetweenUtokensDefault = spaceDefault;
        private const bool indentEmptyLinesDefault = false;
        private static readonly IFormatProvider formatProviderDefault = CultureInfo.InvariantCulture;

        internal static BnfTerm AnyBnfTerm { get { return _AnyBnfTerm.Instance; } }

        #endregion

        #region State

        private IDictionary<BnfTermPartialContext, BlockIndentation> contextToBlockIndentation = new Dictionary<BnfTermPartialContext, BlockIndentation>();
        private IDictionary<Tuple<BnfTerm, BnfTermPartialContext>, BlockIndentation> contextToBlockIndentation2 = new Dictionary<Tuple<BnfTerm, BnfTermPartialContext>, BlockIndentation>();
        private IDictionary<BnfTermPartialContext, InsertedUtokens> contextToUtokensLeft = new Dictionary<BnfTermPartialContext, InsertedUtokens>();
        private IDictionary<BnfTermPartialContext, InsertedUtokens> contextToUtokensRight = new Dictionary<BnfTermPartialContext, InsertedUtokens>();
        private IDictionary<Tuple<BnfTerm, BnfTermPartialContext>, InsertedUtokens> contextToUtokensBetween = new Dictionary<Tuple<BnfTerm, BnfTermPartialContext>, InsertedUtokens>();
        private ISet<BnfTerm> leftBnfTerms = new HashSet<BnfTerm>();
        private ISet<BnfTermPartialContext> rightContexts = new HashSet<BnfTermPartialContext>();
        private int maxContextLength = 0;

        public string NewLine { get; set; }
        public string Space { get; set; }
        public string Tab { get; set; }
        public string IndentUnit { get; set; }
        public string WhiteSpaceBetweenUtokens { get; set; }
        public bool IndentEmptyLines { get; set; }
        public IFormatProvider FormatProvider { get; private set; }

        #endregion

        public CultureInfo CultureInfo { get { return FormatProvider as CultureInfo; } }

        #region Construction

        public Formatting()
            : this(formatProviderDefault)
        {
        }

        public Formatting(Grammar grammar)
            : this(grammar.DefaultCulture)
        {
        }

        public Formatting(Parser parser)
            : this(parser.Context.Culture)
        {
        }

        protected Formatting(IFormatProvider formatProvider)
        {
            this.FormatProvider = formatProvider;

            this.NewLine = newLineDefault;
            this.Space = spaceDefault;
            this.Tab = tabDefault;
            this.IndentUnit = indentUnitDefault;
            this.WhiteSpaceBetweenUtokens = whiteSpaceBetweenUtokensDefault;
            this.IndentEmptyLines = indentEmptyLinesDefault;
        }

        #endregion

        #region Interface to grammar

        #region Settings

        public void SetFormatProviderIndependentlyFromParser(IFormatProvider formatProvider)
        {
            this.FormatProvider = formatProvider;

        }

        public void SetCultureInfoIndependentlyFromParser(CultureInfo cultureInfo)
        {
            SetFormatProviderIndependentlyFromParser(cultureInfo);
        }

        public void SetCultureInfo(CultureInfo cultureInfo, Parser parser)
        {
            SetCultureInfoIndependentlyFromParser(cultureInfo);
            parser.Context.Culture = cultureInfo;
        }

        #endregion

        #region Set BlockIndentation

        public void SetBlockIndentationOn(BnfTermPartialContext context, BlockIndentation blockIndentation)
        {
            contextToBlockIndentation.Add(context, blockIndentation);
            RegisterContext(context);
        }

        public void SetBlockIndentationOn(BnfTerm leftBnfTerm, BnfTermPartialContext context, BlockIndentation blockIndentation)
        {
            contextToBlockIndentation2.Add(Tuple.Create(leftBnfTerm, context), blockIndentation);
            RegisterContext(leftBnfTerm, context);
        }

        #endregion

        #region Insert

        #region Left

        public void InsertUtokensLeftOfAny(params UtokenInsert[] utokensLeft)
        {
            InsertUtokensLeftOf(BnfTermPartialContext.Any, priority: anyPriorityDefault, behavior: anyBehaviorDefault, utokensLeft: utokensLeft);
        }

        public void InsertUtokensLeftOf(BnfTermPartialContext context, params UtokenInsert[] utokensLeft)
        {
            InsertUtokensLeftOf(context, priority: priorityDefault, behavior: behaviorDefault, utokensLeft: utokensLeft);
        }

        public void InsertUtokensLeftOf(BnfTermPartialContext context, double priority, Behavior behavior, params UtokenInsert[] utokensLeft)
        {
            contextToUtokensLeft.Add(context, new InsertedUtokens(InsertedUtokens.Kind.Left, priority, behavior, utokensLeft, context));
            RegisterContext(context);
        }

        #endregion

        #region Right

        public void InsertUtokensRightOfAny(params UtokenInsert[] utokensRight)
        {
            InsertUtokensRightOf(BnfTermPartialContext.Any, priority: anyPriorityDefault, behavior: anyBehaviorDefault, utokensRight: utokensRight);
        }

        public void InsertUtokensRightOf(BnfTermPartialContext context, params UtokenInsert[] utokensRight)
        {
            InsertUtokensRightOf(context, priority: priorityDefault, behavior: behaviorDefault, utokensRight: utokensRight);
        }

        public void InsertUtokensRightOf(BnfTermPartialContext context, double priority, Behavior behavior, params UtokenInsert[] utokensRight)
        {
            contextToUtokensRight.Add(context, new InsertedUtokens(InsertedUtokens.Kind.Right, priority, behavior, utokensRight, context));
            RegisterContext(context);
        }

        #endregion

        #region Around

        public void InsertUtokensAroundAny(params UtokenInsert[] utokensAround)
        {
            InsertUtokensAround(BnfTermPartialContext.Any, priority: anyPriorityDefault, behavior: anyBehaviorDefault, utokensAround: utokensAround);
        }

        public void InsertUtokensAround(BnfTermPartialContext context, params UtokenInsert[] utokensAround)
        {
            InsertUtokensAround(context, priority: priorityDefault, behavior: behaviorDefault, utokensAround: utokensAround);
        }

        public void InsertUtokensAround(BnfTermPartialContext context, double priority, Behavior behavior, params UtokenInsert[] utokensAround)
        {
            InsertUtokensLeftOf(context, priority, behavior, utokensAround);
            InsertUtokensRightOf(context, priority, behavior, utokensAround);
        }

        #endregion

        #region Between

        public void InsertUtokensBetweenOrderedLeftAndAny(BnfTerm leftBnfTerm, params UtokenInsert[] utokensBetween)
        {
            InsertUtokensBetweenOrdered(leftBnfTerm, BnfTermPartialContext.Any, priority: anyPriorityDefault, behavior: anyBehaviorDefault, utokensBetween: utokensBetween);
        }

        public void InsertUtokensBetweenOrderedAnyAndRight(BnfTermPartialContext rightContext, params UtokenInsert[] utokensBetween)
        {
            InsertUtokensBetweenOrdered(AnyBnfTerm, rightContext, priority: anyPriorityDefault, behavior: anyBehaviorDefault, utokensBetween: utokensBetween);
        }

        public void InsertUtokensBetweenAny(params UtokenInsert[] utokensBetween)
        {
            InsertUtokensBetweenOrdered(AnyBnfTerm, BnfTermPartialContext.Any, priority: anyPriorityDefault, behavior: anyBehaviorDefault, utokensBetween: utokensBetween);
        }

        public void InsertUtokensBetweenUnorderedAnyAndOther(BnfTerm bnfTerm, params UtokenInsert[] utokensBetween)
        {
            InsertUtokensBetweenUnordered(AnyBnfTerm, bnfTerm, priority: anyPriorityDefault, behavior: anyBehaviorDefault, utokensBetween: utokensBetween);
        }

        public void InsertUtokensBetweenOrdered(BnfTerm leftBnfTerm, BnfTermPartialContext rightContext, params UtokenInsert[] utokensBetween)
        {
            InsertUtokensBetweenOrdered(leftBnfTerm, rightContext, priority: priorityDefault, behavior: behaviorDefault, utokensBetween: utokensBetween);
        }

        public void InsertUtokensBetweenOrdered(BnfTerm leftBnfTerm, BnfTermPartialContext rightContext, double priority, Behavior behavior, params UtokenInsert[] utokensBetween)
        {
            contextToUtokensBetween.Add(
                Tuple.Create(leftBnfTerm, rightContext),
                new InsertedUtokens(InsertedUtokens.Kind.Between, priority, behavior, utokensBetween, leftBnfTerm, rightContext)
                );

            RegisterContext(leftBnfTerm, rightContext);
        }

        public void InsertUtokensBetweenUnordered(BnfTerm bnfTerm1, BnfTerm bnfTerm2, params UtokenInsert[] utokensBetween)
        {
            InsertUtokensBetweenUnordered(bnfTerm1, bnfTerm2, priority: priorityDefault, behavior: behaviorDefault, utokensBetween: utokensBetween);
        }

        public void InsertUtokensBetweenUnordered(BnfTerm bnfTerm1, BnfTerm bnfTerm2, double priority, Behavior behavior, params UtokenInsert[] utokensBetween)
        {
            InsertUtokensBetweenOrdered(bnfTerm1, bnfTerm2, priority, behavior, utokensBetween);
            InsertUtokensBetweenOrdered(bnfTerm2, bnfTerm1, priority, behavior, utokensBetween);
        }

        #endregion

        #endregion

        #endregion

        #region Interface to unparser

        internal bool TryGetBlockIndentation(IEnumerable<BnfTerm> targetAndAncestors, out BlockIndentation blockIndentation)
        {
            bool success = TryGetValue(contextToBlockIndentation, targetAndAncestors, context => context, out blockIndentation);
            if (blockIndentation == null) blockIndentation = BlockIndentation.Null;
            return success;
        }

        internal bool TryGetBlockIndentation(BnfTerm leftBnfTerm, IEnumerable<BnfTerm> targetAndAncestors, out BlockIndentation blockIndentation)
        {
            var leftCandidates = new[] { leftBnfTerm, AnyBnfTerm };
            bool success = TryGetValue(contextToBlockIndentation2, leftCandidates, targetAndAncestors, out blockIndentation);
            if (blockIndentation == null) blockIndentation = BlockIndentation.Null;
            return success;
        }

        internal bool TryGetUtokensLeft(IEnumerable<BnfTerm> targetAndAncestors, out InsertedUtokens insertedUtokensLeft)
        {
            return TryGetValue(contextToUtokensLeft, targetAndAncestors, context => context, out insertedUtokensLeft);
        }

        internal bool TryGetUtokensRight(IEnumerable<BnfTerm> targetAndAncestors, out InsertedUtokens insertedUtokensRight)
        {
            return TryGetValue(contextToUtokensRight, targetAndAncestors, context => context, out insertedUtokensRight);
        }

        internal bool TryGetUtokensBetween(BnfTerm leftBnfTerm, IEnumerable<BnfTerm> rightTargetAndAncestors, out InsertedUtokens insertedUtokensBetween)
        {
            var leftCandidates = new[] { leftBnfTerm, AnyBnfTerm };
            return TryGetValue(contextToUtokensBetween, leftCandidates, rightTargetAndAncestors, out insertedUtokensBetween);
        }

        internal bool IsLeftBnfTermUsed(BnfTerm leftBnfTerm)
        {
            return leftBnfTerms.Contains(leftBnfTerm);
        }

        internal bool DoesTargetNeedsLeftBnfTerm(IEnumerable<BnfTerm> targetAndAncestors)
        {
            for (var context = new BnfTermPartialContext(targetAndAncestors.Where(IsImportant).Take(this.maxContextLength).Reverse());
                context.Length > 0;
                context = context.OmitTopAncestor())
            {
                if (rightContexts.Contains(context))
                    return true;
            }

            return false;
        }

        #endregion

        #region Helpers

        private void RegisterContext(BnfTermPartialContext context)
        {
            maxContextLength = Math.Max(maxContextLength, context.Length);
        }

        private void RegisterContext(BnfTerm leftBnfTerm, BnfTermPartialContext rightContext)
        {
            leftBnfTerms.Add(leftBnfTerm);
            rightContexts.Add(rightContext);
            RegisterContext(rightContext);
        }

        private bool TryGetValue<TValue>(IDictionary<Tuple<BnfTerm, BnfTermPartialContext>, TValue> keyToValue, IEnumerable<BnfTerm> leftBnfTerms,
            IEnumerable<BnfTerm> targetAndAncestors, out TValue value)
        {
            foreach (BnfTerm leftBnfTerm in leftBnfTerms)
            {
                if (TryGetValue(keyToValue, targetAndAncestors, context => Tuple.Create(leftBnfTerm, context), out value))
                    return true;
            }

            // NOTE that (Any,*), (*,Any) and (Any,Any) has been already processed

            value = default(TValue);
            return false;
        }

        private bool TryGetValue<TKey, TValue>(IDictionary<TKey, TValue> keyToValue, IEnumerable<BnfTerm> targetAndAncestors,
            Func<BnfTermPartialContext, TKey> contextToKey, out TValue value)
        {
            for (var context = new BnfTermPartialContext(targetAndAncestors.Where(IsImportant).Take(this.maxContextLength).Reverse());
                context.Length > 0;
                context = context.OmitTopAncestor())
            {
                if (keyToValue.TryGetValue(contextToKey(context), out value))
                    return true;
            }

            return keyToValue.TryGetValue(contextToKey(BnfTermPartialContext.Any), out value);
        }

        private static bool IsImportant(BnfTerm bnfTerm)
        {
            return bnfTerm.IsReferable() || bnfTerm is BnfiTermCollection;
        }

        #endregion
    }

    #region BnfTermPartialContext

    public class BnfTermPartialContext : IEquatable<BnfTermPartialContext>
    {
        internal static readonly BnfTermPartialContext Any = new BnfTermPartialContext();

        internal readonly BnfTerm[] ancestorsAndTarget;

        public BnfTermPartialContext(BnfTerm target)
        {
            this.ancestorsAndTarget = new BnfTerm[] { target };
        }

        public BnfTermPartialContext(params BnfTerm[] ancestorsAndTarget)
        {
            this.ancestorsAndTarget = ancestorsAndTarget.ToArray();     // copy the parameter array to protect the field array from external modifications through the parameter array
        }

        public BnfTermPartialContext(IEnumerable<BnfTerm> ancestorsAndTarget)
        {
            this.ancestorsAndTarget = ancestorsAndTarget.ToArray();
        }

        public static implicit operator BnfTermPartialContext(BnfTerm target)
        {
            return new BnfTermPartialContext(target);
        }

        public bool Equals(BnfTermPartialContext that)
        {
            return object.ReferenceEquals(this, that)
                ||
                !object.ReferenceEquals(that, null) &&
                this.ancestorsAndTarget.Length == that.ancestorsAndTarget.Length &&
                this.ancestorsAndTarget.SequenceEqual(that.ancestorsAndTarget);
        }

        public override bool Equals(object obj)
        {
            return obj is BnfTermPartialContext && Equals((BnfTermPartialContext)obj);
        }

        public override int GetHashCode()
        {
            return this.ancestorsAndTarget.GetHashCodeMulti();
        }

        public static bool operator ==(BnfTermPartialContext context1, BnfTermPartialContext context2)
        {
            return object.ReferenceEquals(context1, context2) || !object.ReferenceEquals(context1, null) && context1.Equals(context2);
        }

        public static bool operator !=(BnfTermPartialContext context1, BnfTermPartialContext context2)
        {
            return !(context1 == context2);
        }

        public override string ToString()
        {
            return string.Join(".", (IEnumerable<BnfTerm>)ancestorsAndTarget);
        }

        public BnfTermPartialContext OmitTopAncestor()
        {
            return new BnfTermPartialContext(this.ancestorsAndTarget.Skip(1));
        }

        public int Length { get { return ancestorsAndTarget.Length; } }
    }

    #endregion

    #region BlockIndentation

    public class BlockIndentation : IEquatable<BlockIndentation>
    {
        internal enum Kind
        {
            Null,
            Deferred,
            Indent,
            Unindent,
            ZeroIndent,
        }

        private Kind kind;
        private readonly bool isReadonly;
        private DeferredUtokens leftDeferred;
        private DeferredUtokens rightDeferred;

        private BlockIndentation(Kind kind, bool isReadonly)
        {
            this.kind = kind;
            this.isReadonly = isReadonly;
        }

        internal static readonly BlockIndentation Null = new BlockIndentation(Kind.Null, isReadonly: true);
        public static readonly BlockIndentation Indent = new BlockIndentation(Kind.Indent, isReadonly: true);
        public static readonly BlockIndentation Unindent = new BlockIndentation(Kind.Unindent, isReadonly: true);
        public static readonly BlockIndentation ZeroIndent = new BlockIndentation(Kind.ZeroIndent, isReadonly: true);

        internal static BlockIndentation Defer()
        {
            return new BlockIndentation(Kind.Deferred, isReadonly: false);
        }

        internal void CopyKindFrom(BlockIndentation source)
        {
            if (this.isReadonly)
                throw new InvalidOperationException("Internal error: cannot change a readonly BlockIndentation");

            this.kind = source.kind;
        }

        public override string ToString()
        {
            return ":" + kind + ":";
        }

        internal bool IsDeferred()
        {
            return kind == Kind.Deferred;
        }

        internal DeferredUtokens LeftDeferred
        {
            set
            {
                if (leftDeferred != null)
                    throw new InvalidOperationException("Double set is not allowed for LeftDeferred");

                leftDeferred = value;
            }
            get { return leftDeferred; }
        }

        internal DeferredUtokens RightDeferred
        {
            set
            {
                if (rightDeferred != null)
                    throw new InvalidOperationException("Double set is not allowed for RightDeferred");

                rightDeferred = value;
            }
            get { return rightDeferred; }
        }

        internal bool IsNull()
        {
            return kind == Kind.Null;
        }

        public bool Equals(BlockIndentation that)
        {
            return object.ReferenceEquals(this, that)
                ||
                !object.ReferenceEquals(that, null) &&
                this.kind == that.kind;
        }

        public override bool Equals(object obj)
        {
            return obj is BlockIndentation && Equals((BlockIndentation)obj);
        }

        public override int GetHashCode()
        {
            return this.kind.GetHashCode();
        }

        public static bool operator ==(BlockIndentation context1, BlockIndentation context2)
        {
            return object.ReferenceEquals(context1, context2) || !object.ReferenceEquals(context1, null) && context1.Equals(context2);
        }

        public static bool operator !=(BlockIndentation context1, BlockIndentation context2)
        {
            return !(context1 == context2);
        }
    }

    #endregion
}
