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

        public static readonly Utoken NewLine = UtokenWhitespace.NewLine;
        public static readonly Utoken EmptyLine = UtokenWhitespace.EmptyLine;
        public static readonly Utoken Space = UtokenWhitespace.Space;
        public static readonly Utoken Tab = UtokenWhitespace.Tab;

        public static readonly Utoken IndentBlock = UtokenControl.IndentBlock;

        public static readonly Utoken IndentThisLine = UtokenControl.IndentThisLine;
        public static readonly Utoken UnindentThisLine = UtokenControl.UnindentThisLine;
        public static readonly Utoken NoIndentForThisLine = UtokenControl.NoIndentForThisLine;

        public static readonly Utoken IncreaseIndentLevel = UtokenControl.IncreaseIndentLevel;
        public static readonly Utoken DecreaseIndentLevel = UtokenControl.DecreaseIndentLevel;
        public static readonly Utoken SetIndentLevelToNone = UtokenControl.SetIndentLevelToNone;

        public static readonly Utoken NoWhitespace = UtokenControl.NoWhitespace;
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
            return ToString("\"{0}\"{1}", Text, Reference != null ? " (with ref)" : string.Empty);
        }
    }

    public class UtokenWhitespace : Utoken
    {
        internal enum Kind { NewLine, EmptyLine, Space, Tab, BetweenUtokens }

        internal readonly Kind kind;

        private UtokenWhitespace(Kind kind)
        {
            this.kind = kind;
        }

        internal static new readonly UtokenWhitespace NewLine = new UtokenWhitespace(Kind.NewLine);
        internal static new readonly UtokenWhitespace EmptyLine = new UtokenWhitespace(Kind.EmptyLine);
        internal static new readonly UtokenWhitespace Space = new UtokenWhitespace(Kind.Space);
        internal static new readonly UtokenWhitespace Tab = new UtokenWhitespace(Kind.Tab);
        internal static readonly UtokenWhitespace BetweenUtokens = new UtokenWhitespace(Kind.BetweenUtokens);

        public override string ToString(Formatting formatting)
        {
            switch (kind)
            {
                case Kind.NewLine:
                    return formatting.NewLine;

                case Kind.EmptyLine:
                    return formatting.NewLine + formatting.NewLine;

                case Kind.Space:
                    return formatting.Space;

                case Kind.Tab:
                    return formatting.Tab;

                case Kind.BetweenUtokens:
                    return formatting.WhiteSpaceBetweenUtokens;

                default:
                    throw new InvalidOperationException("Unknown UtokenWhitespace");
            }
        }

        public override string ToString()
        {
            return ToString("." + kind);
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
            return string.Concat(Enumerable.Repeat(formatting.IndentUnit, IndentLevel));
        }

        public override string ToString()
        {
            return ToString("indent level: {0}", IndentLevel);
        }
    }

    internal class UtokenControl : Utoken
    {
        internal enum Kind
        {
            IndentBlock,
            IndentThisLine,
            UnindentThisLine,
            NoIndentForThisLine,
            IncreaseIndentLevel,
            DecreaseIndentLevel,
            SetIndentLevelToNone,
            NoWhitespace
        }

        internal readonly Kind kind;

        private UtokenControl(Kind kind)
        {
            this.kind = kind;
        }

        public static new readonly UtokenControl IndentBlock = new UtokenControl(Kind.IndentBlock);

        public static new readonly UtokenControl IndentThisLine = new UtokenControl(Kind.IndentThisLine);
        public static new readonly UtokenControl UnindentThisLine = new UtokenControl(Kind.UnindentThisLine);
        public static new readonly UtokenControl NoIndentForThisLine = new UtokenControl(Kind.NoIndentForThisLine);

        public static new readonly UtokenControl IncreaseIndentLevel = new UtokenControl(Kind.IncreaseIndentLevel);
        public static new readonly UtokenControl DecreaseIndentLevel = new UtokenControl(Kind.DecreaseIndentLevel);
        public static new readonly UtokenControl SetIndentLevelToNone = new UtokenControl(Kind.SetIndentLevelToNone);

        public static new readonly UtokenControl NoWhitespace = new UtokenControl(Kind.NoWhitespace);

        public override string ToString(Formatting formatting)
        {
            throw new InvalidOperationException("Cannot convert an UtokenControl to string");
        }

        public override string ToString()
        {
            return ToString("." + kind);
        }
    }

    internal class UtokenDependent : Utoken
    {
        public readonly UtokenControl utoken;
        public readonly UtokenControl depender;

        public UtokenDependent(UtokenControl utoken, UtokenControl depender)
        {
            this.utoken = utoken;
            this.depender = depender;
        }

        public override string ToString(Formatting formatting)
        {
            throw new InvalidOperationException("Should not convert an UtokenDependent to string");
        }

        public override string ToString()
        {
            return ToString("{0} depends on {1}", utoken, depender);
        }

        public override IEnumerable<Utoken> Flatten()
        {
            return utoken.Flatten();
        }
    }

    internal class UtokenDepender : Utoken
    {
        public readonly Utoken utoken;

        public UtokenDepender(Utoken utoken)
        {
            this.utoken = utoken;
        }

        public override string ToString(Formatting formatting)
        {
            throw new InvalidOperationException("Should not convert an UtokenDepender to string");
        }

        public override string ToString()
        {
            return ToString("{0} (depender)", utoken);
        }

        public override IEnumerable<Utoken> Flatten()
        {
            return utoken.Flatten().Select(_utoken => new UtokenDepender(_utoken));
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
            return string.Concat(Enumerable.Repeat(utoken, count).Select(_utoken => _utoken.ToString(formatting)));
        }

        public override IEnumerable<Utoken> Flatten()
        {
            return Enumerable.Repeat(utoken, count).SelectMany(_utoken => _utoken.Flatten());
        }

        public override string ToString()
        {
            return ToString("repeat {0} {1} times", utoken, count);
        }
    }

    internal class InsertedUtokens : Utoken, IComparable<InsertedUtokens>
    {
        public enum Kind { Before, After, Between }

        public readonly Kind kind;
        public readonly double priority;
        public readonly bool overridable;
        public readonly IEnumerable<Utoken> utokens;

        private readonly IEnumerable<BnfTerm> affectedBnfTerms;

        internal InsertedUtokens(Kind kind, double priority, bool overridable, IEnumerable<Utoken> utokens, params BnfTerm[] affectedBnfTerms)
        {
            this.priority = priority;
            this.kind = kind;
            this.overridable = overridable;
            this.utokens = utokens.ToList();

            this.affectedBnfTerms = affectedBnfTerms;
        }

        public override string ToString(Formatting formatting)
        {
            throw new InvalidOperationException("Should not convert an InsertedUtokens to string");
        }

        public override string ToString()
        {
            return string.Format("{0}{1}, priority {2}: {{ {3} }}; affected bnfTerms: {{ {4} }}",
                kind,
                overridable ? ", overridable" : string.Empty,
                priority,
                string.Join(", ", utokens),
                string.Join(", ", affectedBnfTerms)
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

        private int? _anyCount;
        private int anyCount
        {
            get
            {
                if (!_anyCount.HasValue)
                    _anyCount = affectedBnfTerms.Count(bnfTerm => bnfTerm == Formatting.AnyBnfTerm);

                return _anyCount.Value;
            }
        }
    }
}
