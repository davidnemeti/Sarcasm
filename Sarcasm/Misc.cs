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
using Irony.ITG;
using Irony.ITG.Ast;
using Irony.ITG.Unparsing;

namespace Irony.ITG
{
    public static class Misc
    {
        public static void SetFormatting(this Parser parser, Formatting formatting)
        {
            parser.Context.Culture = (CultureInfo)formatting.FormatProvider;    // note: this will throw an InvalidCastException if the value is not a CultureInfo
        }

        internal static int? DebugWriteLinePriority(this int? priority, TraceSource ts, BnfTerm bnfTerm, object obj, string messageBefore = "", string messageAfter = "", string messageInside = "")
        {
            ts.Debug(
                "{0}{1}{2} obj: {3}; priority = {4}{5}",
                messageBefore,
                bnfTerm,
                messageInside != "" ? " " + messageInside : messageInside,
                obj != null ? obj.ToString() : "<<NULL>>",
                priority.HasValue ? priority.ToString() : "NULL",
                messageAfter
                );

            return priority;
        }
    }
}
