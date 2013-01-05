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

        public static Utoken CreateIndent(int indentLevel)
        {
            return new UtokenRepeat(UtokenPrimitive.IndentUnit, indentLevel);
        }

        public static readonly Utoken NewLine = UtokenPrimitive.NewLine;
        public static readonly Utoken EmptyLine = UtokenPrimitive.EmptyLine;
        public static readonly Utoken Space = UtokenPrimitive.Space;
        public static readonly Utoken Tab = UtokenPrimitive.Tab;

        public static readonly Utoken IncreaseIndentLevel = new UtokenControl();
        public static readonly Utoken DecreaseIndentLevel = new UtokenControl();
        public static readonly Utoken IncreaseIndentLevelForThisLine = new UtokenControl();
        public static readonly Utoken DecreaseIndentLevelForThisLine = new UtokenControl();
        public static readonly Utoken SetIndentLevelToNone = new UtokenControl();
        public static readonly Utoken SetIndentLevelToNoneForThisLine = new UtokenControl();
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
    }

    internal class UtokenPrimitive : Utoken
    {
        private UtokenPrimitive() { }

        public static new readonly UtokenPrimitive NewLine = new UtokenPrimitive();
        public static new readonly UtokenPrimitive EmptyLine = new UtokenPrimitive();
        public static new readonly UtokenPrimitive Space = new UtokenPrimitive();
        public static new readonly UtokenPrimitive Tab = new UtokenPrimitive();
        public static readonly UtokenPrimitive IndentUnit = new UtokenPrimitive();

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
            else if (this == IndentUnit)
                return formatting.IndentUnit;
            else
                throw new InvalidOperationException("Unknown utoken primitive");
        }
    }

    internal class UtokenControl : Utoken
    {
        public override string ToString(Formatting formatting)
        {
            throw new InvalidOperationException("Cannot convert an UtokenControl to string");
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
    }

    internal class InsertedUtokens : Utoken, IComparable<InsertedUtokens>
    {
        public enum Kind { Before, After, Between }

        public readonly Kind kind;
        public readonly int priority;
        public readonly bool overridable;
        public readonly IEnumerable<Utoken> utokens;

        public InsertedUtokens(Kind kind, int priority, bool overridable, IEnumerable<Utoken> utokens)
        {
            this.utokens = utokens.ToList();
            this.priority = priority;
            this.kind = kind;
            this.overridable = overridable;
        }

        public override string ToString(Formatting formatting)
        {
            throw new InvalidOperationException("Should not convert an InsertedUtokens to string");
        }

        public override IEnumerable<Utoken> Flatten()
        {
            return utokens.SelectMany(utoken => utoken.Flatten());
        }

        public int CompareTo(InsertedUtokens that)
        {
            if (that == null || this.priority > that.priority)
                return 1;
            else if (this.priority < that.priority)
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
