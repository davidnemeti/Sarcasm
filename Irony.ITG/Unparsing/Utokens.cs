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
            return string.Join(separator: null, values: Enumerable.Repeat(utoken, count).Select(_utoken => _utoken.ToString(formatting)));
        }
    }

    internal class InsertedUtokens : Utoken
    {
        public enum _Kind { Before, After, Between }

        public IEnumerable<Utoken> Utokens { get; private set; }
        public int Priority { get; private set; }
        public _Kind Kind { get; private set; }

        public InsertedUtokens(IEnumerable<Utoken> utokens, int priority, _Kind kind)
        {
            this.Utokens = utokens;
            this.Priority = priority;
            this.Kind = kind;
        }

        public override string ToString(Formatting formatting)
        {
            throw new InvalidOperationException("Cannot convert an InsertedUtokens to string");
        }
    }
}
