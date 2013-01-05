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
        #region Constants

        private const int defaultPriority = 0;
        private const bool defaultOverridable = true;

        #endregion

        #region State

        private IDictionary<BnfTerm, InsertedUtokens> bnfTermToUtokensBefore = new Dictionary<BnfTerm, InsertedUtokens>();
        private IDictionary<BnfTerm, InsertedUtokens> bnfTermToUtokensAfter = new Dictionary<BnfTerm, InsertedUtokens>();
        private IDictionary<Tuple<BnfTerm, BnfTerm>, InsertedUtokens> bnfTermToUtokensBetween = new Dictionary<Tuple<BnfTerm, BnfTerm>, InsertedUtokens>();
        private ISet<BnfTerm> leftBnfTerms = new HashSet<BnfTerm>();

        #endregion

        #region Interface to grammar

        #region Settings

        public string NewLine { get; set; }
        public string Space { get; set; }
        public string Tab { get; set; }
        public string IndentUnit { get; set; }

        #endregion

        #region Insert utokens

        public void InsertUtokensBefore(BnfTerm bnfTerm, params Utoken[] utokensBefore)
        {
            InsertUtokensBefore(bnfTerm, priority: defaultPriority, overridable: defaultOverridable, utokensBefore: utokensBefore);
        }

        public void InsertUtokensBefore(BnfTerm bnfTerm, int priority, bool overridable, params Utoken[] utokensBefore)
        {
            bnfTermToUtokensBefore.Add(bnfTerm, new InsertedUtokens(InsertedUtokens.Kind.Before, priority, overridable, utokensBefore));
        }

        public void InsertUtokensAfter(BnfTerm bnfTerm, params Utoken[] utokensAfter)
        {
            InsertUtokensAfter(bnfTerm, priority: defaultPriority, overridable: defaultOverridable, utokensAfter: utokensAfter);
        }

        public void InsertUtokensAfter(BnfTerm bnfTerm, int priority, bool overridable, params Utoken[] utokensAfter)
        {
            bnfTermToUtokensAfter.Add(bnfTerm, new InsertedUtokens(InsertedUtokens.Kind.After, priority, overridable, utokensAfter));
        }

        public void InsertUtokensAround(BnfTerm bnfTerm, params Utoken[] utokensAround)
        {
            InsertUtokensAround(bnfTerm, priority: defaultPriority, overridable: defaultOverridable, utokensAround: utokensAround);
        }

        public void InsertUtokensAround(BnfTerm bnfTerm, int priority, bool overridable, params Utoken[] utokensAround)
        {
            InsertUtokensBefore(bnfTerm, priority, overridable, utokensAround);
            InsertUtokensAfter(bnfTerm, priority, overridable, utokensAround);
        }

        public void InsertUtokensBetween(BnfTerm leftBnfTerm, BnfTerm rightBnfTerm, params Utoken[] utokensBetween)
        {
            InsertUtokensBetween(leftBnfTerm, rightBnfTerm, priority: defaultPriority, overridable: defaultOverridable, utokensBetween: utokensBetween);
        }

        public void InsertUtokensBetween(BnfTerm leftBnfTerm, BnfTerm rightBnfTerm, int priority, bool overridable, params Utoken[] utokensBetween)
        {
            bnfTermToUtokensBetween.Add(Tuple.Create(leftBnfTerm, rightBnfTerm), new InsertedUtokens(InsertedUtokens.Kind.Between, priority, overridable, utokensBetween));
            leftBnfTerms.Add(leftBnfTerm);
        }

        #endregion

        #endregion

        #region Interface to unparser

        internal bool HasUtokensBefore(BnfTerm bnfTerm, out InsertedUtokens insertedUtokensBefore)
        {
            return bnfTermToUtokensBefore.TryGetValue(bnfTerm, out insertedUtokensBefore);
        }

        internal bool HasUtokensAfter(BnfTerm bnfTerm, out InsertedUtokens insertedUtokensAfter)
        {
            return bnfTermToUtokensAfter.TryGetValue(bnfTerm, out insertedUtokensAfter);
        }

        internal bool HasUtokensBetween(BnfTerm leftBnfTerm, BnfTerm rightBnfTerm, out InsertedUtokens insertedUtokensBetween)
        {
            return bnfTermToUtokensBetween.TryGetValue(Tuple.Create(leftBnfTerm, rightBnfTerm), out insertedUtokensBetween);
        }

        internal bool IsLeftBnfTermOfABetweenPair(BnfTerm leftBnfTerm)
        {
            return leftBnfTerms.Contains(leftBnfTerm);
        }

    	#endregion
    }
}
