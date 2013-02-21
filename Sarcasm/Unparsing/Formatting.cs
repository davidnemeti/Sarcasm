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
        private static readonly IFormatProvider formatProviderDefault = CultureInfo.InvariantCulture;

        internal static BnfTerm AnyBnfTerm { get { return _AnyBnfTerm.Instance; } }

        #endregion

        #region State

        private IDictionary<BnfTermPartialContext, BlockIndentation> contextToBlockIndentation = new Dictionary<BnfTermPartialContext, BlockIndentation>();
        private IDictionary<Tuple<BnfTerm, BnfTermPartialContext>, BlockIndentation> contextToBlockIndentation2 = new Dictionary<Tuple<BnfTerm, BnfTermPartialContext>, BlockIndentation>();
        private IDictionary<BnfTermPartialContext, InsertedUtokens> contextToUtokensBefore = new Dictionary<BnfTermPartialContext, InsertedUtokens>();
        private IDictionary<BnfTermPartialContext, InsertedUtokens> contextToUtokensAfter = new Dictionary<BnfTermPartialContext, InsertedUtokens>();
        private IDictionary<Tuple<BnfTerm, BnfTermPartialContext>, InsertedUtokens> contextToUtokensBetween = new Dictionary<Tuple<BnfTerm, BnfTermPartialContext>, InsertedUtokens>();
        private ISet<BnfTerm> leftBnfTerms = new HashSet<BnfTerm>();
        private ISet<BnfTermPartialContext> rightContexts = new HashSet<BnfTermPartialContext>();
        private int maxContextLength = 0;

        #endregion

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
            this.NewLine = newLineDefault;
            this.Space = spaceDefault;
            this.Tab = tabDefault;
            this.IndentUnit = indentUnitDefault;
            this.WhiteSpaceBetweenUtokens = whiteSpaceBetweenUtokensDefault;
            this.FormatProvider = formatProvider;
        }

        #endregion

        #region Interface to grammar

        #region Settings

        public string NewLine { get; set; }
        public string Space { get; set; }
        public string Tab { get; set; }
        public string IndentUnit { get; set; }
        public string WhiteSpaceBetweenUtokens { get; set; }
        public IFormatProvider FormatProvider { get; private set; }
        public CultureInfo CultureInfo { get { return FormatProvider as CultureInfo; } }

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

        #region Before

        public void InsertUtokensBeforeAny(params UtokenInsert[] utokensBefore)
        {
            InsertUtokensBefore(BnfTermPartialContext.Any, priority: anyPriorityDefault, behavior: anyBehaviorDefault, utokensBefore: utokensBefore);
        }

        public void InsertUtokensBefore(BnfTermPartialContext context, params UtokenInsert[] utokensBefore)
        {
            InsertUtokensBefore(context, priority: priorityDefault, behavior: behaviorDefault, utokensBefore: utokensBefore);
        }

        public void InsertUtokensBefore(BnfTermPartialContext context, double priority, Behavior behavior, params UtokenInsert[] utokensBefore)
        {
            contextToUtokensBefore.Add(context, new InsertedUtokens(InsertedUtokens.Kind.Before, priority, behavior, utokensBefore, context));
            RegisterContext(context);
        }

        #endregion

        #region After

        public void InsertUtokensAfterAny(params UtokenInsert[] utokensAfter)
        {
            InsertUtokensAfter(BnfTermPartialContext.Any, priority: anyPriorityDefault, behavior: anyBehaviorDefault, utokensAfter: utokensAfter);
        }

        public void InsertUtokensAfter(BnfTermPartialContext context, params UtokenInsert[] utokensAfter)
        {
            InsertUtokensAfter(context, priority: priorityDefault, behavior: behaviorDefault, utokensAfter: utokensAfter);
        }

        public void InsertUtokensAfter(BnfTermPartialContext context, double priority, Behavior behavior, params UtokenInsert[] utokensAfter)
        {
            contextToUtokensAfter.Add(context, new InsertedUtokens(InsertedUtokens.Kind.After, priority, behavior, utokensAfter, context));
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
            InsertUtokensBefore(context, priority, behavior, utokensAround);
            InsertUtokensAfter(context, priority, behavior, utokensAround);
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

        internal bool HasBlockIndentation(IEnumerable<BnfTerm> targetAndAncestors, out BlockIndentation blockIndentation)
        {
            return HasValue(contextToBlockIndentation, targetAndAncestors, context => context, out blockIndentation);
        }

        internal bool HasBlockIndentation(BnfTerm leftBnfTerm, IEnumerable<BnfTerm> targetAndAncestors, out BlockIndentation blockIndentation)
        {
            var leftCandidates = new[] { leftBnfTerm, AnyBnfTerm };
            return HasValue(contextToBlockIndentation2, leftCandidates, targetAndAncestors, out blockIndentation);
        }

        internal bool HasUtokensBefore(IEnumerable<BnfTerm> targetAndAncestors, out InsertedUtokens insertedUtokensBefore)
        {
            return HasValue(contextToUtokensBefore, targetAndAncestors, context => context, out insertedUtokensBefore);
        }

        internal bool HasUtokensAfter(IEnumerable<BnfTerm> targetAndAncestors, out InsertedUtokens insertedUtokensAfter)
        {
            return HasValue(contextToUtokensAfter, targetAndAncestors, context => context, out insertedUtokensAfter);
        }

        internal bool HasUtokensBetween(BnfTerm leftBnfTerm, IEnumerable<BnfTerm> rightTargetAndAncestors, out InsertedUtokens insertedUtokensBetween)
        {
            var leftCandidates = new[] { leftBnfTerm, AnyBnfTerm };
            return HasValue(contextToUtokensBetween, leftCandidates, rightTargetAndAncestors, out insertedUtokensBetween);
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

        private bool HasValue<TValue>(IDictionary<Tuple<BnfTerm, BnfTermPartialContext>, TValue> keyToValue, IEnumerable<BnfTerm> leftBnfTerms,
            IEnumerable<BnfTerm> targetAndAncestors, out TValue value)
        {
            foreach (BnfTerm leftBnfTerm in leftBnfTerms)
            {
                if (HasValue(keyToValue, targetAndAncestors, context => Tuple.Create(leftBnfTerm, context), out value))
                    return true;
            }

            // NOTE that (Any,*), (*,Any) and (Any,Any) has been already processed

            value = default(TValue);
            return false;
        }

        private bool HasValue<TKey, TValue>(IDictionary<TKey, TValue> keyToValue, IEnumerable<BnfTerm> targetAndAncestors,
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
                that != null &&
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

    public class BlockIndentation
    {
        internal enum Kind
        {
            Indent,
            Unindent,
            NoIndent,
        }

        internal readonly Kind kind;

        internal BlockIndentation(Kind kind)
        {
            this.kind = kind;
        }

        public static readonly BlockIndentation Indent = new BlockIndentation(Kind.Indent);
        public static readonly BlockIndentation Unindent = new BlockIndentation(Kind.Unindent);
        public static readonly BlockIndentation NoIndent = new BlockIndentation(Kind.NoIndent);

        public override string ToString()
        {
            return ":" + kind + ":";
        }
    }

    #endregion
}
