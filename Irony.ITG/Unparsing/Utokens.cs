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
    public abstract class Utoken
    {
        public abstract string ToString(Formatting formatting);

        public virtual IEnumerable<Utoken> Flatten()
        {
            yield return this;
        }

        public override string ToString()
        {
            throw new InvalidOperationException("Use 'ToString(Formatting formatting)' instead");
        }

        public static implicit operator Utoken(string text)
        {
            return (UtokenText)text;
        }

        public static Utoken CreateText(string text, object reference = null)
        {
            return new UtokenText(text, reference);
        }

        public static readonly Utoken NewLine = UtokenPrimitive.NewLine;
        public static readonly Utoken EmptyLine = UtokenPrimitive.EmptyLine;
        public static readonly Utoken Space = UtokenPrimitive.Space;
        public static readonly Utoken Tab = UtokenPrimitive.Tab;

        public static readonly Utoken IncreaseIndentLevel = UtokenControl.IncreaseIndentLevel;
        public static readonly Utoken DecreaseIndentLevel = UtokenControl.DecreaseIndentLevel;
        public static readonly Utoken IncreaseIndentLevelForThisLine = UtokenControl.IncreaseIndentLevelForThisLine;
        public static readonly Utoken DecreaseIndentLevelForThisLine = UtokenControl.DecreaseIndentLevelForThisLine;
        public static readonly Utoken SetIndentLevelToNone = UtokenControl.SetIndentLevelToNone;
        public static readonly Utoken SetIndentLevelToNoneForThisLine = UtokenControl.SetIndentLevelToNoneForThisLine;
    }

    public class UtokenText : Utoken
    {
        public string Text { get; private set; }
        public object Reference { get; private set; }

        public UtokenText(string text, object reference = null)
        {
            this.Text = text;
            this.Reference = reference;
        }

        public static implicit operator UtokenText(string text)
        {
            return new UtokenText(text);
        }

        public override string ToString(Formatting formatting)
        {
            return this.Text;
        }

        public override string ToString()
        {
            return string.Format("\"{0}\"{1}", Text, Reference != null ? " (with ref)" : string.Empty);
        }
    }

    public class UtokenPrimitive : Utoken
    {
        internal enum Kind { NewLine, EmptyLine, Space, Tab }

        internal readonly Kind kind;

        private UtokenPrimitive(Kind kind)
        {
            this.kind = kind;
        }

        internal static new readonly UtokenPrimitive NewLine = new UtokenPrimitive(Kind.NewLine);
        internal static new readonly UtokenPrimitive EmptyLine = new UtokenPrimitive(Kind.EmptyLine);
        internal static new readonly UtokenPrimitive Space = new UtokenPrimitive(Kind.Space);
        internal static new readonly UtokenPrimitive Tab = new UtokenPrimitive(Kind.Tab);

        public override string ToString(Formatting formatting)
        {
            if (this == NewLine)
                return formatting.NewLine;
            else if (this == EmptyLine)
                return formatting.NewLine + formatting.NewLine;
            else if (this == Space)
                return formatting.Space;
            else if (this == Tab)
                return formatting.Tab;
            else
                throw new InvalidOperationException("Unknown utoken primitive");
        }

        public override string ToString()
        {
            return kind.ToString();
        }
    }

    public class UtokenIndent : Utoken
    {
        public int IndentLevel { get; private set; }

        internal UtokenIndent(int indentLevel)
        {
            this.IndentLevel = indentLevel;
        }

        public override string ToString(Formatting formatting)
        {
            return string.Join(string.Empty, Enumerable.Repeat(formatting.IndentUnit, IndentLevel));
        }

        public override string ToString()
        {
            return string.Format("indent level: {0}", IndentLevel);
        }
    }

    internal class UtokenControl : Utoken
    {
        internal enum Kind
        {
            IncreaseIndentLevel,
            DecreaseIndentLevel,
            IncreaseIndentLevelForThisLine,
            DecreaseIndentLevelForThisLine,
            SetIndentLevelToNone,
            SetIndentLevelToNoneForThisLine
        }

        internal readonly Kind kind;

        private UtokenControl(Kind kind)
        {
            this.kind = kind;
        }

        public static new readonly UtokenControl IncreaseIndentLevel = new UtokenControl(Kind.IncreaseIndentLevel);
        public static new readonly UtokenControl DecreaseIndentLevel = new UtokenControl(Kind.DecreaseIndentLevel);
        public static new readonly UtokenControl IncreaseIndentLevelForThisLine = new UtokenControl(Kind.IncreaseIndentLevelForThisLine);
        public static new readonly UtokenControl DecreaseIndentLevelForThisLine = new UtokenControl(Kind.DecreaseIndentLevelForThisLine);
        public static new readonly UtokenControl SetIndentLevelToNone = new UtokenControl(Kind.SetIndentLevelToNone);
        public static new readonly UtokenControl SetIndentLevelToNoneForThisLine = new UtokenControl(Kind.SetIndentLevelToNoneForThisLine);

        public override string ToString(Formatting formatting)
        {
            throw new InvalidOperationException("Cannot convert an UtokenControl to string");
        }

        public override string ToString()
        {
            return kind.ToString();
        }
    }

    internal class UtokenRepeat : Utoken
    {
        private readonly Utoken utoken;
        private readonly int count;

        public UtokenRepeat(Utoken utoken, int count)
        {
            this.utoken = utoken;
            this.count = count;
        }

        public override string ToString(Formatting formatting)
        {
            return string.Join(
                separator: null,
                values: Enumerable.Repeat(utoken, count).Select(_utoken => _utoken.ToString(formatting))
                );
        }

        public override IEnumerable<Utoken> Flatten()
        {
            return Enumerable.Repeat(utoken, count).SelectMany(_utoken => _utoken.Flatten());
        }

        public override string ToString()
        {
            return string.Format("repeat {0} {1} times", utoken, count);
        }
    }

    internal class InsertedUtokens : Utoken, IComparable<InsertedUtokens>
    {
        public enum Kind { Before, After, Between }

        public readonly Kind kind;
        public readonly double priority;
        public readonly int anyCount;
        public readonly bool overridable;
        public readonly IEnumerable<Utoken> utokens;

        internal InsertedUtokens(Kind kind, double priority, int anyCount, bool overridable, IEnumerable<Utoken> utokens)
        {
            this.utokens = utokens.ToList();
            this.priority = priority;
            this.anyCount = anyCount;
            this.kind = kind;
            this.overridable = overridable;
        }

        public override string ToString(Formatting formatting)
        {
            throw new InvalidOperationException("Should not convert an InsertedUtokens to string");
        }

        public override string ToString()
        {
            return string.Format("{0}{1}, priority {2}, anyCount {3}: {{ {4} }}",
                kind,
                overridable ? ", overridable" : string.Empty,
                priority,
                anyCount,
                string.Join(", ", utokens)
                );
        }

        public override IEnumerable<Utoken> Flatten()
        {
            return utokens.SelectMany(utoken => utoken.Flatten());
        }

        public int CompareTo(InsertedUtokens that)
        {
            if (that == null || this.priority > that.priority || this.anyCount < that.anyCount)
                return 1;
            else if (this.priority < that.priority || this.anyCount > that.anyCount)
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
    }
}
