using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Irony.ITG;
using Irony.ITG.Ast;

using Grammar = Irony.ITG.Ast.Grammar;

namespace Irony.ITG.Unparsing
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

        private const bool overridableDefault = true;
        private const bool anyOverridableDefault = true;

        private const string newLineDefault = "\n";
        private const string spaceDefault = " ";
        private const string tabDefault = "\t";
        private static readonly string indentUnitDefault = string.Concat(Enumerable.Repeat(spaceDefault, 4));
        private const string whiteSpaceBetweenUtokensDefault = spaceDefault;

        internal static BnfTerm AnyBnfTerm { get { return _AnyBnfTerm.Instance; } }

        #endregion

        #region State

        private IDictionary<BnfTerm, InsertedUtokens> bnfTermToUtokensBefore = new Dictionary<BnfTerm, InsertedUtokens>();
        private IDictionary<BnfTerm, InsertedUtokens> bnfTermToUtokensAfter = new Dictionary<BnfTerm, InsertedUtokens>();
        private IDictionary<Tuple<BnfTerm, BnfTerm>, InsertedUtokens> bnfTermToUtokensBetween = new Dictionary<Tuple<BnfTerm, BnfTerm>, InsertedUtokens>();
        private ISet<BnfTerm> leftBnfTerms = new HashSet<BnfTerm>();

        #endregion

        #region Construction

        public Formatting()
        {
            this.NewLine = newLineDefault;
            this.Space = spaceDefault;
            this.Tab = tabDefault;
            this.IndentUnit = indentUnitDefault;
            this.WhiteSpaceBetweenUtokens = whiteSpaceBetweenUtokensDefault;
        }

        #endregion

        #region Interface to grammar

        #region Settings

        public string NewLine { get; set; }
        public string Space { get; set; }
        public string Tab { get; set; }
        public string IndentUnit { get; set; }
        public string WhiteSpaceBetweenUtokens { get; set; }

        #endregion

        #region Insert utokens

        public void InsertUtokensBeforeAny(params Utoken[] utokensBefore)
        {
            InsertUtokensBefore(AnyBnfTerm, priority: anyPriorityDefault, overridable: anyOverridableDefault, utokensBefore: utokensBefore);
        }

        public void InsertUtokensBefore(BnfTerm bnfTerm, params Utoken[] utokensBefore)
        {
            InsertUtokensBefore(bnfTerm, priority: priorityDefault, overridable: overridableDefault, utokensBefore: utokensBefore);
        }

        public void InsertUtokensBefore(BnfTerm bnfTerm, double priority, bool overridable, params Utoken[] utokensBefore)
        {
            bnfTermToUtokensBefore.Add(bnfTerm, new InsertedUtokens(InsertedUtokens.Kind.Before, priority, GetAnyCount(bnfTerm), overridable, utokensBefore));
        }

        public void InsertUtokensAfterAny(params Utoken[] utokensAfter)
        {
            InsertUtokensAfter(AnyBnfTerm, priority: anyPriorityDefault, overridable: anyOverridableDefault, utokensAfter: utokensAfter);
        }

        public void InsertUtokensAfter(BnfTerm bnfTerm, params Utoken[] utokensAfter)
        {
            InsertUtokensAfter(bnfTerm, priority: priorityDefault, overridable: overridableDefault, utokensAfter: utokensAfter);
        }

        public void InsertUtokensAfter(BnfTerm bnfTerm, double priority, bool overridable, params Utoken[] utokensAfter)
        {
            bnfTermToUtokensAfter.Add(bnfTerm, new InsertedUtokens(InsertedUtokens.Kind.After, priority, GetAnyCount(bnfTerm), overridable, utokensAfter));
        }

        public void InsertUtokensAroundAny(params Utoken[] utokensAround)
        {
            InsertUtokensAround(AnyBnfTerm, priority: anyPriorityDefault, overridable: anyOverridableDefault, utokensAround: utokensAround);
        }

        public void InsertUtokensAround(BnfTerm bnfTerm, params Utoken[] utokensAround)
        {
            InsertUtokensAround(bnfTerm, priority: priorityDefault, overridable: overridableDefault, utokensAround: utokensAround);
        }

        public void InsertUtokensAround(BnfTerm bnfTerm, double priority, bool overridable, params Utoken[] utokensAround)
        {
            InsertUtokensBefore(bnfTerm, priority, overridable, utokensAround);
            InsertUtokensAfter(bnfTerm, priority, overridable, utokensAround);
        }

        public void InsertUtokensBetweenLeftAndAny(BnfTerm leftBnfTerm, params Utoken[] utokensBetween)
        {
            InsertUtokensBetween(leftBnfTerm, AnyBnfTerm, priority: anyPriorityDefault, overridable: anyOverridableDefault, utokensBetween: utokensBetween);
        }

        public void InsertUtokensBetweenAnyAndRight(BnfTerm rightBnfTerm, params Utoken[] utokensBetween)
        {
            InsertUtokensBetween(AnyBnfTerm, rightBnfTerm, priority: anyPriorityDefault, overridable: anyOverridableDefault, utokensBetween: utokensBetween);
        }

        public void InsertUtokensBetweenAny(params Utoken[] utokensBetween)
        {
            InsertUtokensBetween(AnyBnfTerm, AnyBnfTerm, priority: anyPriorityDefault, overridable: anyOverridableDefault, utokensBetween: utokensBetween);
        }

        public void InsertUtokensBetween(BnfTerm leftBnfTerm, BnfTerm rightBnfTerm, params Utoken[] utokensBetween)
        {
            InsertUtokensBetween(leftBnfTerm, rightBnfTerm, priority: priorityDefault, overridable: overridableDefault, utokensBetween: utokensBetween);
        }

        public void InsertUtokensBetween(BnfTerm leftBnfTerm, BnfTerm rightBnfTerm, double priority, bool overridable, params Utoken[] utokensBetween)
        {
            bnfTermToUtokensBetween.Add(
                Tuple.Create(leftBnfTerm, rightBnfTerm),
                new InsertedUtokens(InsertedUtokens.Kind.Between, priority, GetAnyCount(leftBnfTerm, rightBnfTerm), overridable, utokensBetween)
                );

            leftBnfTerms.Add(leftBnfTerm);
        }

        private static int GetAnyCount(params BnfTerm[] bnfTerms)
        {
            return bnfTerms.Count(bnfTerm => bnfTerm == AnyBnfTerm);
        }

        #endregion

        #endregion

        #region Interface to unparser

        internal bool HasUtokensBefore(BnfTerm bnfTerm, out InsertedUtokens insertedUtokensBefore)
        {
            return bnfTermToUtokensBefore.TryGetValue(bnfTerm, out insertedUtokensBefore)
                || bnfTermToUtokensBefore.TryGetValue(AnyBnfTerm, out insertedUtokensBefore);
        }

        internal bool HasUtokensAfter(BnfTerm bnfTerm, out InsertedUtokens insertedUtokensAfter)
        {
            return bnfTermToUtokensAfter.TryGetValue(bnfTerm, out insertedUtokensAfter)
                || bnfTermToUtokensAfter.TryGetValue(AnyBnfTerm, out insertedUtokensAfter);
        }

        internal bool HasUtokensBetween(BnfTerm leftBnfTerm, BnfTerm rightBnfTerm, out InsertedUtokens insertedUtokensBetween)
        {
            return bnfTermToUtokensBetween.TryGetValue(Tuple.Create(leftBnfTerm, rightBnfTerm), out insertedUtokensBetween)
                || bnfTermToUtokensBetween.TryGetValue(Tuple.Create(leftBnfTerm, AnyBnfTerm), out insertedUtokensBetween)
                || bnfTermToUtokensBetween.TryGetValue(Tuple.Create(AnyBnfTerm, rightBnfTerm), out insertedUtokensBetween)
                || bnfTermToUtokensBetween.TryGetValue(Tuple.Create(AnyBnfTerm, AnyBnfTerm), out insertedUtokensBetween);
        }

        internal bool IsLeftBnfTermOfABetweenPair(BnfTerm leftBnfTerm)
        {
            return leftBnfTerms.Contains(leftBnfTerm);
        }

    	#endregion
    }
}
