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

        private const string newLineDefault = "\n";
        private const string spaceDefault = " ";
        private const string tabDefault = "\t";
        private static readonly string indentUnitDefault = string.Concat(Enumerable.Repeat(spaceDefault, 4));
        private const string whiteSpaceBetweenUtokensDefault = spaceDefault;
        private static readonly IFormatProvider formatProviderDefault = CultureInfo.InvariantCulture;

        internal static BnfTerm AnyBnfTerm { get { return _AnyBnfTerm.Instance; } }

        #endregion

        #region State

        private readonly Grammar grammar;

        private IDictionary<BnfTermPartialContext, InsertedUtokens> contextToUtokensBefore = new Dictionary<BnfTermPartialContext, InsertedUtokens>();
        private IDictionary<BnfTermPartialContext, InsertedUtokens> contextToUtokensAfter = new Dictionary<BnfTermPartialContext, InsertedUtokens>();
        private IDictionary<Tuple<BnfTerm, BnfTermPartialContext>, InsertedUtokens> contextToUtokensBetween = new Dictionary<Tuple<BnfTerm, BnfTermPartialContext>, InsertedUtokens>();
        private ISet<BnfTerm> leftBnfTerms = new HashSet<BnfTerm>();
        private int maxContextLength = 0;

        #endregion

        #region Construction

        public Formatting()
            : this(grammar: null)
        {
        }

        protected Formatting(Grammar grammar)
        {
            this.grammar = grammar;

            this.NewLine = newLineDefault;
            this.Space = spaceDefault;
            this.Tab = tabDefault;
            this.IndentUnit = indentUnitDefault;
            this.WhiteSpaceBetweenUtokens = whiteSpaceBetweenUtokensDefault;

            if (this.FormatProvider == null)
                this.FormatProvider = formatProviderDefault;
        }

        internal static Formatting CreateDefaultFormattingForGrammar(Grammar grammar)
        {
            return new Formatting(grammar);
        }

        #endregion

        #region Interface to grammar

        #region Settings

        public string NewLine { get; set; }
        public string Space { get; set; }
        public string Tab { get; set; }
        public string IndentUnit { get; set; }
        public string WhiteSpaceBetweenUtokens { get; set; }

        IFormatProvider formatProvider = null;

        public IFormatProvider FormatProvider
        {
            get
            {
                return grammar != null ? grammar.DefaultCulture : this.formatProvider;
            }
            set
            {
                if (grammar != null)
                    grammar.DefaultCulture = (CultureInfo)value;    // note: this will throw an InvalidCastException if the value is not a CultureInfo
                else
                    this.formatProvider = value;
            }
        }

        #endregion

        #region Insert utokens

        public void InsertUtokensBeforeAny(params Utoken[] utokensBefore)
        {
            InsertUtokensBefore(BnfTermPartialContext.Any, priority: anyPriorityDefault, behavior: anyBehaviorDefault, utokensBefore: utokensBefore);
        }

        public void InsertUtokensBefore(BnfTermPartialContext context, params Utoken[] utokensBefore)
        {
            InsertUtokensBefore(context, priority: priorityDefault, behavior: behaviorDefault, utokensBefore: utokensBefore);
        }

        public void InsertUtokensBefore(BnfTermPartialContext context, double priority, Behavior behavior, params Utoken[] utokensBefore)
        {
            contextToUtokensBefore.Add(context, new InsertedUtokens(InsertedUtokens.Kind.Before, priority, behavior, utokensBefore, context));
            UpdateMaxContextLength(context);
        }

        public void InsertUtokensAfterAny(params Utoken[] utokensAfter)
        {
            InsertUtokensAfter(BnfTermPartialContext.Any, priority: anyPriorityDefault, behavior: anyBehaviorDefault, utokensAfter: utokensAfter);
        }

        public void InsertUtokensAfter(BnfTermPartialContext context, params Utoken[] utokensAfter)
        {
            InsertUtokensAfter(context, priority: priorityDefault, behavior: behaviorDefault, utokensAfter: utokensAfter);
        }

        public void InsertUtokensAfter(BnfTermPartialContext context, double priority, Behavior behavior, params Utoken[] utokensAfter)
        {
            contextToUtokensAfter.Add(context, new InsertedUtokens(InsertedUtokens.Kind.After, priority, behavior, utokensAfter, context));
            UpdateMaxContextLength(context);
        }

        public void InsertUtokensAroundAny(params Utoken[] utokensAround)
        {
            InsertUtokensAround(BnfTermPartialContext.Any, priority: anyPriorityDefault, behavior: anyBehaviorDefault, utokensAround: utokensAround);
        }

        public void InsertUtokensAround(BnfTermPartialContext context, params Utoken[] utokensAround)
        {
            InsertUtokensAround(context, priority: priorityDefault, behavior: behaviorDefault, utokensAround: utokensAround);
        }

        public void InsertUtokensAround(BnfTermPartialContext context, double priority, Behavior behavior, params Utoken[] utokensAround)
        {
            InsertUtokensBefore(context, priority, behavior, utokensAround);
            InsertUtokensAfter(context, priority, behavior, utokensAround);
        }

        public void InsertUtokensBetweenOrderedLeftAndAny(BnfTerm leftBnfTerm, params Utoken[] utokensBetween)
        {
            InsertUtokensBetweenOrdered(leftBnfTerm, BnfTermPartialContext.Any, priority: anyPriorityDefault, behavior: anyBehaviorDefault, utokensBetween: utokensBetween);
        }

        public void InsertUtokensBetweenOrderedAnyAndRight(BnfTermPartialContext rightContext, params Utoken[] utokensBetween)
        {
            InsertUtokensBetweenOrdered(AnyBnfTerm, rightContext, priority: anyPriorityDefault, behavior: anyBehaviorDefault, utokensBetween: utokensBetween);
        }

        public void InsertUtokensBetweenAny(params Utoken[] utokensBetween)
        {
            InsertUtokensBetweenOrdered(AnyBnfTerm, BnfTermPartialContext.Any, priority: anyPriorityDefault, behavior: anyBehaviorDefault, utokensBetween: utokensBetween);
        }

        public void InsertUtokensBetweenUnorderedAnyAndOther(BnfTerm bnfTerm, params Utoken[] utokensBetween)
        {
            InsertUtokensBetweenUnordered(AnyBnfTerm, bnfTerm, priority: anyPriorityDefault, behavior: anyBehaviorDefault, utokensBetween: utokensBetween);
        }

        public void InsertUtokensBetweenOrdered(BnfTerm leftBnfTerm, BnfTermPartialContext rightContext, params Utoken[] utokensBetween)
        {
            InsertUtokensBetweenOrdered(leftBnfTerm, rightContext, priority: priorityDefault, behavior: behaviorDefault, utokensBetween: utokensBetween);
        }

        public void InsertUtokensBetweenOrdered(BnfTerm leftBnfTerm, BnfTermPartialContext rightContext, double priority, Behavior behavior, params Utoken[] utokensBetween)
        {
            contextToUtokensBetween.Add(
                Tuple.Create(leftBnfTerm, rightContext),
                new InsertedUtokens(InsertedUtokens.Kind.Between, priority, behavior, utokensBetween, leftBnfTerm, rightContext)
                );

            leftBnfTerms.Add(leftBnfTerm);

            UpdateMaxContextLength(rightContext);
        }

        public void InsertUtokensBetweenUnordered(BnfTerm bnfTerm1, BnfTerm bnfTerm2, params Utoken[] utokensBetween)
        {
            InsertUtokensBetweenUnordered(bnfTerm1, bnfTerm2, priority: priorityDefault, behavior: behaviorDefault, utokensBetween: utokensBetween);
        }

        public void InsertUtokensBetweenUnordered(BnfTerm bnfTerm1, BnfTerm bnfTerm2, double priority, Behavior behavior, params Utoken[] utokensBetween)
        {
            InsertUtokensBetweenOrdered(bnfTerm1, bnfTerm2, priority, behavior, utokensBetween);
            InsertUtokensBetweenOrdered(bnfTerm2, bnfTerm1, priority, behavior, utokensBetween);
        }

        private void UpdateMaxContextLength(BnfTermPartialContext context)
        {
            maxContextLength = Math.Max(maxContextLength, context.Length);
        }

        #endregion

        #endregion

        #region Interface to unparser

        internal bool HasUtokensBefore(IEnumerable<BnfTerm> targetAndAncestors, out InsertedUtokens insertedUtokensBefore)
        {
            return HasUtokens(contextToUtokensBefore, targetAndAncestors, context => context, out insertedUtokensBefore);
        }

        internal bool HasUtokensAfter(IEnumerable<BnfTerm> targetAndAncestors, out InsertedUtokens insertedUtokensAfter)
        {
            return HasUtokens(contextToUtokensAfter, targetAndAncestors, context => context, out insertedUtokensAfter);
        }

        internal bool HasUtokensBetween(BnfTerm leftBnfTerm, IEnumerable<BnfTerm> rightTargetAndAncestors, out InsertedUtokens insertedUtokensBetween)
        {
            var leftCandidates = new[] { leftBnfTerm, AnyBnfTerm };

            foreach (BnfTerm leftCandidate in leftCandidates)
            {
                if (HasUtokens(contextToUtokensBetween, rightTargetAndAncestors, rightContext => Tuple.Create(leftCandidate, rightContext), out insertedUtokensBetween))
                    return true;
            }

            // NOTE that (Any,*), (*,Any) and (Any,Any) has been already processed

            insertedUtokensBetween = null;
            return false;
        }

        private bool HasUtokens<TKey>(IDictionary<TKey, InsertedUtokens> contextToUtokens, IEnumerable<BnfTerm> targetAndAncestors,
            Func<BnfTermPartialContext, TKey> contextToKey, out InsertedUtokens insertedUtokens)
        {
            for (var context = new BnfTermPartialContext(targetAndAncestors.Where(IsImportant).Take(this.maxContextLength).Reverse());
                context.Length > 0;
                context = context.OmitTopAncestor())
            {
                if (contextToUtokens.TryGetValue(contextToKey(context), out insertedUtokens))
                    return true;
            }

            return contextToUtokens.TryGetValue(contextToKey(BnfTermPartialContext.Any), out insertedUtokens);
        }

        private static bool IsImportant(BnfTerm bnfTerm)
        {
            return bnfTerm.IsReferable() || bnfTerm is BnfiTermCollection;
        }

        internal bool IsLeftBnfTermOfABetweenPair(BnfTerm leftBnfTerm)
        {
            return leftBnfTerms.Contains(leftBnfTerm);
        }

    	#endregion
    }

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
            return this.ancestorsAndTarget.Length == that.ancestorsAndTarget.Length &&
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
}
