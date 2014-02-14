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
    public class Discriminator
    {
        public string Name { get; private set; }

        public Discriminator(string name)
        {
            this.Name = name;
        }
    }

    public abstract class UtokenBase
    {
        public virtual IEnumerable<UtokenBase> Flatten()
        {
            yield return this;
        }

        #region ToString

        protected string ToString(object obj)
        {
            return ToString(obj.ToString());
        }

        protected string ToString(string str)
        {
            return string.Format("[{0}]", str);
        }

        protected string ToString(string format, params object[] args)
        {
            return ToString(string.Format(format, args));
        }

        #endregion

        private IDecoration _decoration;
        public IDecoration Decoration
        {
            get { return _decoration ?? (_decoration = Unparsing.Decoration.None); }
            internal set { _decoration = value; }
        }

        public bool HasDecoration()
        {
            return _decoration != null;
        }

        public Discriminator Discriminator { get; protected set; }
    }

    public abstract class UtokenInsert : UtokenBase
    {
        public static UtokenInsert NewLine()
        {
            return UtokenWhitespace.NewLine();
        }

        public static UtokenInsert EmptyLine()
        {
            return UtokenWhitespace.EmptyLine();
        }

        public static UtokenInsert Space()
        {
            return UtokenWhitespace.Space();
        }

        public static UtokenInsert Tab()
        {
            return UtokenWhitespace.Tab();
        }

        public static UtokenInsert NoWhitespace()
        {
            return UtokenControl.NoWhitespace;
        }
    }

    public interface Utoken
    {
        string ToText(Formatter formatter);
        IDecoration Decoration { get; }
        bool HasDecoration();
        Discriminator Discriminator { get; }
    }

    public abstract class UtokenValue : UtokenInsert
    {
        public static UtokenValue NoWhitespace()
        {
            return UtokenValueControl.NoWhitespace;
        }

        public static UtokenValue CreateText(UnparsableAst reference)
        {
            return new UtokenText(reference);
        }

        public static UtokenValue CreateText(string text)
        {
            return new UtokenText(text);
        }

        public static UtokenValue CreateText(string text, UnparsableAst reference)
        {
            return new UtokenText(text, reference);
        }

        public static UtokenValue FurtherUnparse(UnparsableAst unparsableAst)
        {
            return new UtokenToUnparse(unparsableAst);
        }

        public static implicit operator UtokenValue(string text)
        {
            return CreateText(text);
        }

        public UtokenValue SetDiscriminator(Discriminator discriminator)
        {
            this.Discriminator = discriminator;
            return this;
        }
    }

    public class UtokenValueControl : UtokenValue
    {
        internal static new readonly UtokenValueControl NoWhitespace = new UtokenValueControl();
    }

    public class UtokenText : UtokenValue, Utoken
    {
        public string Text { get; private set; }
        public UnparsableAst Reference { get; private set; }

        internal UtokenText(UnparsableAst reference)
            : this(text: null, reference: reference)
        {
        }

        internal UtokenText(string text, UnparsableAst reference = null)
        {
            this.Text = text;
            this.Reference = reference;
        }

        public static implicit operator UtokenText(string text)
        {
            return new UtokenText(text);
        }

        public string ToText(Formatter formatter)
        {
            return this.Text ?? ConvertToString(this.Reference, formatter.FormatProvider);
        }

        public new UtokenText SetDiscriminator(Discriminator discriminator)
        {
            this.Discriminator = discriminator;
            return this;
        }

        public override string ToString()
        {
            string text = this.Text ?? this.Reference.ToString();
            return ToString("\"{0}\"{1}", text, Reference != null ? " (with ref)" : string.Empty);
        }

        private static string ConvertToString(UnparsableAst reference, IFormatProvider formatProvider)
        {
            if (reference.BnfTerm is DataLiteralBase)
            {
                DataLiteralBase dataLiteral = (DataLiteralBase)reference.BnfTerm;

                string text;

                if (reference.AstValue is DateTime)
                    text = ((DateTime)reference.AstValue).ToString(dataLiteral.DateTimeFormat, formatProvider);

                else if (NumberLiteralInfo.IsNumber(reference.AstValue))
                    text = NumberLiteralInfo.NumberToText(reference.AstValue, dataLiteral.IntRadix, formatProvider);

                else
                    text = Convert.ToString(reference.AstValue, formatProvider);

                return text;
            }

            else if (reference.BnfTerm is NumberLiteral)
                return NumberLiteralInfo.NumberToText(reference.AstValue, @base: 10, formatProvider: formatProvider);

            else
                return Convert.ToString(reference.AstValue, formatProvider);
        }
    }

    public class UtokenToUnparse : UtokenValue
    {
        public UnparsableAst UnparsableAst { get; private set; }

        internal UtokenToUnparse(UnparsableAst unparsableAst)
        {
            this.UnparsableAst = unparsableAst;
        }

        public override string ToString()
        {
            return ToString("UtokenToUnparse: " + UnparsableAst.ToString());
        }
    }

    public class UtokenWhitespace : UtokenInsert, Utoken
    {
        internal enum Kind { NewLine, EmptyLine, Space, Tab, WhiteSpaceBetweenUtokens }

        internal readonly Kind kind;

        private UtokenWhitespace(Kind kind)
        {
            this.kind = kind;
        }

        public static new UtokenWhitespace NewLine()
        {
            return new UtokenWhitespace(UtokenWhitespace.Kind.NewLine);
        }

        public static new UtokenWhitespace EmptyLine()
        {
            return new UtokenWhitespace(UtokenWhitespace.Kind.EmptyLine);
        }

        public static new UtokenWhitespace Space()
        {
            return new UtokenWhitespace(UtokenWhitespace.Kind.Space);
        }

        public static new UtokenWhitespace Tab()
        {
            return new UtokenWhitespace(UtokenWhitespace.Kind.Tab);
        }

        internal static UtokenWhitespace WhiteSpaceBetweenUtokens()
        {
            return new UtokenWhitespace(UtokenWhitespace.Kind.WhiteSpaceBetweenUtokens);
        }

        public string ToText(Formatter formatter)
        {
            switch (kind)
            {
                case Kind.NewLine:
                    return formatter.NewLine;

                case Kind.EmptyLine:
                    return formatter.NewLine + formatter.NewLine;

                case Kind.Space:
                    return formatter.Space;

                case Kind.Tab:
                    return formatter.Tab;

                case Kind.WhiteSpaceBetweenUtokens:
                    return formatter.WhiteSpaceBetweenUtokens;

                default:
                    throw new InvalidOperationException("Unknown UtokenWhitespace");
            }
        }

        public override string ToString()
        {
            return ToString("." + kind);
        }
    }

    internal class UtokenControl : UtokenInsert
    {
        internal enum Kind
        {
            NoWhitespace,

            #region For internal use only (not useful for the user, moreover it breaks the ability of local unparse)

            IncreaseIndentLevel,
            DecreaseIndentLevel,
            SetIndentLevelToNone,
            RestoreIndentLevel

            #endregion
        }

        internal readonly Kind kind;

        private UtokenControl(Kind kind)
        {
            this.kind = kind;
        }

        internal static new readonly UtokenControl NoWhitespace = new UtokenControl(Kind.NoWhitespace);

        #region For internal use only (not useful for the user, moreover it breaks the ability of local unparse)

        internal static readonly UtokenControl IncreaseIndentLevel = new UtokenControl(Kind.IncreaseIndentLevel);
        internal static readonly UtokenControl DecreaseIndentLevel = new UtokenControl(Kind.DecreaseIndentLevel);
        internal static readonly UtokenControl SetIndentLevelToNone = new UtokenControl(Kind.SetIndentLevelToNone);
        internal static readonly UtokenControl RestoreIndentLevel = new UtokenControl(Kind.RestoreIndentLevel);

        #endregion

        public override string ToString()
        {
            return ToString("." + kind);
        }

        internal bool IsIndent()
        {
            return kind.EqualToAny(Kind.IncreaseIndentLevel, Kind.DecreaseIndentLevel, Kind.SetIndentLevelToNone, Kind.RestoreIndentLevel);
        }
    }

    public class UtokenIndent : UtokenBase, Utoken
    {
        public int IndentLevel { get; private set; }

        internal UtokenIndent(int indentLevel)
        {
            if (indentLevel < 0)
                throw new ArgumentOutOfRangeException("indentLevel");

            this.IndentLevel = indentLevel;
        }

        public string ToText(Formatter formatter)
        {
            return string.Concat(Enumerable.Repeat(formatter.IndentUnit, IndentLevel));
        }

        public override string ToString()
        {
            return ToString("indent level: {0}", IndentLevel);
        }
    }

    internal class UtokenRepeat : UtokenInsert
    {
        private readonly UtokenBase utoken;
        private readonly int count;

        public UtokenRepeat(UtokenBase utoken, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            this.utoken = utoken;
            this.count = count;
        }

        public override IEnumerable<UtokenBase> Flatten()
        {
            return Enumerable.Repeat(utoken, count).SelectMany(_utoken => _utoken.Flatten());
        }

        public override string ToString()
        {
            return ToString("repeat {0} {1} times", utoken, count);
        }
    }

    public enum Behavior { Overridable, NonOverridableSkipThrough, NonOverridableSeparator }

    public class InsertedUtokens : UtokenBase, IComparable<InsertedUtokens>
    {
        internal enum Kind { Left, Right, Between }

        internal Kind kind { get; set; }

        public double Priority { get; set; }
        public Behavior Behavior { get; set; }
        public IEnumerable<UtokenInsert> Utokens { get; private set; }

        internal IEnumerable<UnparsableAst> affectedUnparsableAsts_FOR_DEBUG;

        public const InsertedUtokens None = null;
        private const double priorityDefault = 0;
        private const Behavior behaviorDefault = Behavior.Overridable;

        public InsertedUtokens(double priority, Behavior behavior, IEnumerable<UtokenInsert> utokens)
            : this(priority, behavior, utokens.ToArray())
        {
        }

        public InsertedUtokens(double priority, Behavior behavior, params UtokenInsert[] utokens)
        {
            this.Priority = priority;
            this.Behavior = behavior;
            this.Utokens = utokens;
        }

        public InsertedUtokens(params UtokenInsert[] utokens)
            : this(priorityDefault, behaviorDefault, utokens)
        {
        }

        public InsertedUtokens(UtokenInsert utoken)
            : this(priorityDefault, behaviorDefault, new UtokenInsert[] {utoken})
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, behavior {1}, priority {2}: {{ {3} }}; affected objects: {{ {4} }}",
                kind,
                Behavior,
                Priority,
                string.Join(", ", Utokens),
                affectedUnparsableAsts_FOR_DEBUG != null ? string.Join(", ", affectedUnparsableAsts_FOR_DEBUG) : "<<NO DEBUG INFO>>"
                );
        }

        public override IEnumerable<UtokenBase> Flatten()
        {
            return Utokens.SelectMany(utoken => utoken.Flatten());
        }

        public int CompareTo(InsertedUtokens that)
        {
            if (that == null || this.Priority > that.Priority)
                return 1;
            else if (this.Priority < that.Priority)
                return -1;
            else
                return 0;
        }

        public static int Compare(InsertedUtokens insertedUtokens1, InsertedUtokens insertedUtokens2)
        {
            if (insertedUtokens1 == null && insertedUtokens2 == null)
                return 0;
            else if (insertedUtokens1 == null && insertedUtokens2 != null)
                return -1;
            else
                return insertedUtokens1.CompareTo(insertedUtokens2);
        }

        public static implicit operator InsertedUtokens(UtokenInsert utokenInsert)
        {
            return new InsertedUtokens(utokenInsert);
        }

        public static implicit operator InsertedUtokens(UtokenInsert[] utokens)
        {
            return new InsertedUtokens(utokens);
        }

        public InsertedUtokens SetPriority(double priority)
        {
            this.Priority = priority;
            return this;
        }

        public InsertedUtokens SetBehavior(Behavior behavior)
        {
            this.Behavior = behavior;
            return this;
        }
    }

    public static class UtokenInsertExtensions
    {
        public static InsertedUtokens SetPriority(this UtokenInsert utoken, double priority)
        {
            InsertedUtokens insertedUtokens = utoken;
            insertedUtokens.Priority = priority;
            return insertedUtokens;
        }

        public static InsertedUtokens SetBehavior(this UtokenInsert utoken, Behavior behavior)
        {
            InsertedUtokens insertedUtokens = utoken;
            insertedUtokens.Behavior = behavior;
            return insertedUtokens;
        }
    }

    internal class DeferredUtokens : UtokenBase
    {
        private readonly Func<IEnumerable<UtokenBase>> utokenYielder;
        private IEnumerable<UtokenBase> calculatedUtokens;

        public UnparsableAst Self { get; private set; }
        private readonly string helpMessage;
        private readonly object helpCalculatedObject;

        public DeferredUtokens(Func<IEnumerable<UtokenBase>> utokenYielder, UnparsableAst self, string helpMessage = null, object helpCalculatedObject = null)
        {
            this.utokenYielder = utokenYielder;
            this.Self = self;
            this.helpMessage = helpMessage;
            this.helpCalculatedObject = helpCalculatedObject;
            this.calculatedUtokens = null;

            self.IsLeftSiblingNeededForDeferredCalculation = true;
        }

        public IEnumerable<UtokenBase> GetUtokens()
        {
            CalculateUtokens();
            return calculatedUtokens;
        }

        public void CalculateUtokens()
        {
            if (calculatedUtokens == null)
                calculatedUtokens = utokenYielder().ToList();
        }

        public override string ToString()
        {
            return string.Format("deferred utokens for '{0}'{1}{2}",
                Self != null ? Self.ToString() : "<<UNKNOWN>>",
                helpMessage != null ? " [" + helpMessage + "]" : string.Empty,
                HelpCalculatedObjectToString()
                );
        }

        private string HelpCalculatedObjectToString()
        {
            if (helpCalculatedObject != null)
                return (calculatedUtokens != null ? " calculated" : " non-calculated") + " object: {" + helpCalculatedObject + "}";
            else
                return string.Empty;
        }
    }

    public static class UtokenExtensions
    {
        public static bool IsIndent(this Utoken utoken)
        {
            return utoken is UtokenIndent;
        }

        public static bool IsIndent(this UtokenBase utoken)
        {
            return utoken is UtokenIndent;
        }

        public static bool IsNewLine(this Utoken utoken)
        {
            return utoken is UtokenWhitespace && ((UtokenWhitespace)utoken).kind == UtokenWhitespace.Kind.NewLine;
        }

        public static bool IsNewLine(this UtokenBase utoken)
        {
            return utoken is UtokenWhitespace && ((UtokenWhitespace)utoken).kind == UtokenWhitespace.Kind.NewLine;
        }

        public static bool IsEmptyLine(this Utoken utoken)
        {
            return utoken is UtokenWhitespace && ((UtokenWhitespace)utoken).kind == UtokenWhitespace.Kind.EmptyLine;
        }

        public static bool IsEmptyLine(this UtokenBase utoken)
        {
            return utoken is UtokenWhitespace && ((UtokenWhitespace)utoken).kind == UtokenWhitespace.Kind.EmptyLine;
        }

        public static bool IsSpace(this Utoken utoken)
        {
            return utoken is UtokenWhitespace && ((UtokenWhitespace)utoken).kind == UtokenWhitespace.Kind.Space;
        }

        public static bool IsSpace(this UtokenBase utoken)
        {
            return utoken is UtokenWhitespace && ((UtokenWhitespace)utoken).kind == UtokenWhitespace.Kind.Space;
        }

        public static bool IsTab(this Utoken utoken)
        {
            return utoken is UtokenWhitespace && ((UtokenWhitespace)utoken).kind == UtokenWhitespace.Kind.Tab;
        }

        public static bool IsTab(this UtokenBase utoken)
        {
            return utoken is UtokenWhitespace && ((UtokenWhitespace)utoken).kind == UtokenWhitespace.Kind.Tab;
        }
    }
}
