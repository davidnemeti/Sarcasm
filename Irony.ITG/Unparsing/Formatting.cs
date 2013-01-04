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
        const int defaultPriority = 0;

        private IDictionary<BnfTerm, InsertedUtokens> bnfTermToUtokensBefore = new Dictionary<BnfTerm, InsertedUtokens>();
        private IDictionary<BnfTerm, InsertedUtokens> bnfTermToUtokensAfter = new Dictionary<BnfTerm, InsertedUtokens>();

        public void InsertUtokensBefore(BnfTerm bnfTerm, params Utoken[] utokensBefore)
        {
            InsertUtokensBefore(bnfTerm, priority: defaultPriority, utokensBefore: utokensBefore);
        }

        public void InsertUtokensBefore(BnfTerm bnfTerm, int priority, params Utoken[] utokensBefore)
        {
            bnfTermToUtokensBefore.Add(bnfTerm, new InsertedUtokens(utokensBefore, priority, InsertedUtokens._Kind.Before));
        }

        public void InsertUtokensAfter(BnfTerm bnfTerm, params Utoken[] utokensAfter)
        {
            InsertUtokensAfter(bnfTerm, priority: defaultPriority, utokensAfter: utokensAfter);
        }

        public void InsertUtokensAfter(BnfTerm bnfTerm, int priority, params Utoken[] utokensAfter)
        {
            bnfTermToUtokensAfter.Add(bnfTerm, new InsertedUtokens(utokensAfter, priority, InsertedUtokens._Kind.After));
        }

        public void InsertUtokensAround(BnfTerm bnfTerm, params Utoken[] utokensAround)
        {
            InsertUtokensAround(bnfTerm, priority: defaultPriority, utokensAround: utokensAround);
        }

        public void InsertUtokensAround(BnfTerm bnfTerm, int priority, params Utoken[] utokensAround)
        {
            InsertUtokensBefore(bnfTerm, priority, utokensAround);
            InsertUtokensAfter(bnfTerm, priority, utokensAround);
        }

        internal bool HasUtokensBefore(BnfTerm bnfTerm, out InsertedUtokens insertedUtokensBefore)
        {
            return bnfTermToUtokensBefore.TryGetValue(bnfTerm, out insertedUtokensBefore);
        }

        internal bool HasUtokensAfter(BnfTerm bnfTerm, out InsertedUtokens insertedUtokensAfter)
        {
            return bnfTermToUtokensAfter.TryGetValue(bnfTerm, out insertedUtokensAfter);
        }

        public string NewLine { get; set; }
        public string Space { get; set; }
        public string Tab { get; set; }
        public string IndentUnit { get; set; }
    }
}
