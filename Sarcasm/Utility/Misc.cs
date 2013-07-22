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

namespace Sarcasm.Utility
{
    internal static class Misc
    {
        internal static int? DebugWriteLinePriority(this int? priority, TraceSource ts, UnparsableAst unparsableAst, string messageBefore = "", string messageAfter = "", string messageInside = "")
        {
            ts.Debug(
                "{0}{1}{2} obj: {3}; priority = {4}{5}",
                messageBefore,
                unparsableAst.BnfTerm,
                messageInside != "" ? " " + messageInside : messageInside,
                unparsableAst.AstValue != null ? unparsableAst.AstValue.ToString() : "<<NULL>>",
                priority.HasValue ? priority.ToString() : "NULL",
                messageAfter
                );

            return priority;
        }

        internal static Unparser.Priority DebugWriteLinePriority(this Unparser.Priority priority, TraceSource ts, UnparsableAst unparsableAst, string messageBefore = "", string messageAfter = "", string messageInside = "")
        {
            ts.Debug(
                "{0}{1}{2} obj: {3}; priority = {4}{5}{6}",
                messageBefore,
                unparsableAst.BnfTerm,
                messageInside != "" ? " " + messageInside : messageInside,
                unparsableAst.AstValue != null ? unparsableAst.AstValue.ToString() : "<<NULL>>",
                priority.Kind,
                priority.Value.HasValue ? priority.Value.ToString() : "NULL",
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
