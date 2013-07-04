using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.GrammarAst;
using Sarcasm.Unparsing;

namespace Sarcasm
{
    public static class Misc
    {
        internal static int? DebugWriteLinePriority(this int? priority, TraceSource ts, UnparsableObject unparsableObject, string messageBefore = "", string messageAfter = "", string messageInside = "")
        {
            ts.Debug(
                "{0}{1}{2} obj: {3}; priority = {4}{5}",
                messageBefore,
                unparsableObject.BnfTerm,
                messageInside != "" ? " " + messageInside : messageInside,
                unparsableObject.Obj != null ? unparsableObject.Obj.ToString() : "<<NULL>>",
                priority.HasValue ? priority.ToString() : "NULL",
                messageAfter
                );

            return priority;
        }

        internal static ExpressionUnparser.BnfTermKind DebugWriteLineBnfTermKind(this ExpressionUnparser.BnfTermKind bnfTermKind, TraceSource ts, BnfTerm bnfTerm, string extraMessage = null)
        {
            ts.Debug(
                "{0}, kind: {1}{2}",
                bnfTerm,
                bnfTermKind,
                extraMessage != null ? extraMessage : string.Empty
                );

            return bnfTermKind;
        }
    }
}
