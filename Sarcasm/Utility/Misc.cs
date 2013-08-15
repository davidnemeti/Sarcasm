#region License
/*
    This file is part of Sarcasm.

    Copyright 2012-2013 Dávid Németi

    Sarcasm is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Sarcasm is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Sarcasm.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

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
