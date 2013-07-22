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
    public delegate int? ChildrenPriorityGetter(object astValue, IEnumerable<object> childAstValues);

    public class UnparseHint : GrammarHint
    {
        public ChildrenPriorityGetter GetChildrenPriority { get; private set; }

        public UnparseHint(ChildrenPriorityGetter getChildrenPriority)
        {
            this.GetChildrenPriority = getChildrenPriority;
        }
    }
}
