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
using Sarcasm;
using Sarcasm.Ast;

using Grammar = Sarcasm.Ast.Grammar;

namespace Sarcasm.Unparsing
{
    public abstract class Utoken
    {
        [ThreadStatic]
        private static bool isInsideCreateYinIf;

        private bool isYin;
        private Utoken yin;

        protected Utoken()
        {
            this.isYin = false;
            this.yin = null;
        }

        public abstract string ToString(Formatting formatting);

        protected string ToString(object obj)
        {
            return ToString(obj.ToString());
        }

        protected string ToString(string str)
        {
            return string.Format("[{0}{1}{2}]", str, IsYin() ? " yin" : string.Empty, IsYang() ? " yang for: " + GetYin() : string.Empty);
        }

        protected string ToString(string format, params object[] args)
        {
            return ToString(string.Format(format, args));
        }

        public virtual IEnumerable<Utoken> Flatten()
        {
            yield return this;
        }

        public Utoken CreateYinIfOrReturnSelf(Predicate<Utoken> shouldBeYin)
        {
            if (isInsideCreateYinIf)
                throw new InvalidOperationException("Reentry for CreateYinIfOrReturnSelf is not allowed");

            isInsideCreateYinIf = true;

            try
            {
                Utoken createdYin;
                bool hasCreatedYin = CreateYinIf(shouldBeYin, out createdYin);
                return hasCreatedYin ? createdYin : this;
            }
            finally
            {
                isInsideCreateYinIf = false;
            }
        }

        internal abstract bool CreateYinIf(Predicate<Utoken> shouldBeYin, out Utoken createdYin);

        public bool IsYin()
        {
            return this.isYin;
        }

        public bool IsYang()
        {
            return this.yin != null;
        }

        public bool IsYang(out Utoken yin)
        {
            yin = this.yin;
            return IsYang();
        }

        public Utoken GetYin()
        {
            return this.yin;
        }

        internal void MakeYin()
        {
            if (!isInsideCreateYinIf)
                throw new InvalidOperationException("MakeYin can be used only inside CreateYinIf");

            if (this.isYin)
                throw new InvalidOperationException("Double call of MakeYin on the same object");

            this.isYin = true;
        }

        internal void MakeYang(Utoken yin)
        {
            if (yin == null)
                throw new ArgumentNullException("yin");

            if (this.yin != null)
                throw new InvalidOperationException("Double call of MakeYang on the same object");

            this.yin = yin;
        }
    }

    public abstract class UtokenValue : Utoken
    {
        public static UtokenValue CreateText(object obj)
        {
            return new UtokenText(obj);
        }

        public static UtokenValue CreateText(string text, object obj = null)
        {
            return new UtokenText(text, obj);
        }

        public static UtokenValue FurtherUnparse(UnparsableObject unparsableObject)
        {
            return new UtokenToUnparse(unparsableObject);
        }
    }

    public class UtokenText : UtokenValue
    {
        public string Text { get; private set; }
        public object Reference { get; private set; }

        internal UtokenText(object reference)
            : this(text: null, reference: reference)
        {
        }

        internal UtokenText(string text, object reference = null)
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
            return this.Text ?? Util.ToString(formatting.FormatProvider, this.Reference);
        }

        public override string ToString()
        {
            string text = this.Text ?? this.Reference.ToString();
            return ToString("\"{0}\"{1}", text, Reference != null ? " (with ref)" : string.Empty);
        }

        internal override bool CreateYinIf(Predicate<Utoken> shouldBeYin, out Utoken createdYin)
        {
            if (shouldBeYin(this))
            {
                createdYin = new UtokenText(this.Text, this.Reference);
                createdYin.MakeYin();
                return true;
            }
            else
            {
                createdYin = null;
                return false;
            }
        }
    }

    public class UtokenToUnparse : UtokenValue
    {
        public UnparsableObject UnparsableObject { get; private set; }

        internal UtokenToUnparse(UnparsableObject unparsableObject)
        {
            this.UnparsableObject = unparsableObject;
        }

        public override string ToString(Formatting formatting)
        {
            throw new InvalidOperationException("Cannot convert an UtokenToUnparse to string");
        }

        public override string ToString()
        {
            return ToString("UtokenToUnparse: " + UnparsableObject.ToString());
        }

        internal override bool CreateYinIf(Predicate<Utoken> shouldBeYin, out Utoken createdYin)
        {
            throw new InvalidOperationException("Should not call CreateYinIf on an UtokenToUnparse");
        }
    }

    public abstract class UtokenInsert : Utoken
    {
        public static UtokenInsert NewLine { get { return UtokenWhitespace.NewLine; } }
        public static UtokenInsert EmptyLine { get { return UtokenWhitespace.EmptyLine; } }
        public static UtokenInsert Space { get { return UtokenWhitespace.Space; } }
        public static UtokenInsert Tab { get { return UtokenWhitespace.Tab; } }

        public static UtokenInsert NoWhitespace { get { return UtokenControl.NoWhitespace; } }

        public static UtokenInsert IndentBlock { get { return UtokenControl.IndentBlock; } }
        public static UtokenInsert UnindentBlock { get { return UtokenControl.UnindentBlock; } }
        public static UtokenInsert IndentThisLine { get { return UtokenControl.IndentThisLine; } }
        public static UtokenInsert UnindentThisLine { get { return UtokenControl.UnindentThisLine; } }
        public static UtokenInsert NoIndentForThisLine { get { return UtokenControl.NoIndentForThisLine; } }
    }

    public class UtokenWhitespace : UtokenInsert
    {
        internal enum Kind { NewLine, EmptyLine, Space, Tab, WhiteSpaceBetweenUtokens }

        internal readonly Kind kind;

        private UtokenWhitespace(Kind kind)
        {
            this.kind = kind;
        }

        internal static new readonly UtokenWhitespace NewLine = new UtokenWhitespace(Kind.NewLine);
        internal static new readonly UtokenWhitespace EmptyLine = new UtokenWhitespace(Kind.EmptyLine);
        internal static new readonly UtokenWhitespace Space = new UtokenWhitespace(Kind.Space);
        internal static new readonly UtokenWhitespace Tab = new UtokenWhitespace(Kind.Tab);

        // for internal use only
        internal static readonly UtokenWhitespace WhiteSpaceBetweenUtokens = new UtokenWhitespace(Kind.WhiteSpaceBetweenUtokens);

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

                case Kind.WhiteSpaceBetweenUtokens:
                    return formatting.WhiteSpaceBetweenUtokens;

                default:
                    throw new InvalidOperationException("Unknown UtokenWhitespace");
            }
        }

        public override string ToString()
        {
            return ToString("." + kind);
        }

        internal override bool CreateYinIf(Predicate<Utoken> shouldBeYin, out Utoken createdYin)
        {
            if (shouldBeYin(this))
            {
                createdYin = new UtokenWhitespace(this.kind);
                createdYin.MakeYin();
                return true;
            }
            else
            {
                createdYin = null;
                return false;
            }
        }
    }

    internal class UtokenControl : UtokenInsert
    {
        internal enum Kind
        {
            NoWhitespace,
            IndentBlock,
            UnindentBlock,
            IndentThisLine,
            UnindentThisLine,
            NoIndentForThisLine,

            #region For internal use only (not useful for the user, moreover it breaks the ability of local unparse)

            IncreaseIndentLevel,
            DecreaseIndentLevel,
            SetIndentLevelToNone

            #endregion
        }

        internal readonly Kind kind;

        internal UtokenControl(Kind kind)
        {
            this.kind = kind;
        }

        internal static new readonly UtokenControl IndentBlock = new UtokenControl(Kind.IndentBlock);
        internal static new readonly UtokenControl UnindentBlock = new UtokenControl(Kind.UnindentBlock);
        internal static new readonly UtokenControl IndentThisLine = new UtokenControl(Kind.IndentThisLine);
        internal static new readonly UtokenControl UnindentThisLine = new UtokenControl(Kind.UnindentThisLine);
        internal static new readonly UtokenControl NoIndentForThisLine = new UtokenControl(Kind.NoIndentForThisLine);

        internal static new readonly UtokenControl NoWhitespace = new UtokenControl(Kind.NoWhitespace);

        public override string ToString(Formatting formatting)
        {
            throw new InvalidOperationException("Cannot convert an UtokenControl to string");
        }

        public override string ToString()
        {
            return ToString("." + kind);
        }

        internal override bool CreateYinIf(Predicate<Utoken> shouldBeYin, out Utoken createdYin)
        {
            if (shouldBeYin(this))
            {
                createdYin = new UtokenControl(this.kind);
                createdYin.MakeYin();
                return true;
            }
            else
            {
                createdYin = null;
                return false;
            }
        }
    }

    internal class UtokenIndent : Utoken
    {
        public int IndentLevel { get; private set; }

        internal UtokenIndent(int indentLevel)
        {
            if (indentLevel < 0)
                throw new ArgumentOutOfRangeException("indentLevel");

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

        internal override bool CreateYinIf(Predicate<Utoken> shouldBeYin, out Utoken createdYin)
        {
            if (shouldBeYin(this))
            {
                createdYin = new UtokenIndent(this.IndentLevel);
                createdYin.MakeYin();
                return true;
            }
            else
            {
                createdYin = null;
                return false;
            }
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

        internal override bool CreateYinIf(Predicate<Utoken> shouldBeYin, out Utoken createdYin)
        {
            Utoken createdYinChild;
            bool isYinChild = this.utoken.CreateYinIf(shouldBeYin, out createdYinChild);

            if (shouldBeYin(this) || isYinChild)
            {
                createdYin = new UtokenRepeat(isYinChild ? createdYinChild : this.utoken, this.count);
                createdYin.MakeYin();
                return true;
            }
            else
            {
                createdYin = null;
                return false;
            }
        }
    }

    public enum Behavior { Overridable, NonOverridableSkipThrough, NonOverridableSeparator }

    internal class InsertedUtokens : Utoken, IComparable<InsertedUtokens>
    {
        public enum Kind { Before, After, Between }

        public readonly Kind kind;
        public readonly double priority;
        public readonly Behavior behavior;
        public readonly IEnumerable<UtokenInsert> utokens;

        private readonly IEnumerable<BnfTermPartialContext> affectedContexts;

        internal InsertedUtokens(Kind kind, double priority, Behavior behavior, IEnumerable<UtokenInsert> utokens, params BnfTermPartialContext[] affectedContexts)
            : this(kind, priority, behavior, utokens, (IEnumerable<BnfTermPartialContext>)affectedContexts)
        {
        }

        internal InsertedUtokens(Kind kind, double priority, Behavior behavior, IEnumerable<UtokenInsert> utokens, IEnumerable<BnfTermPartialContext> affectedContexts)
        {
            this.priority = priority;
            this.kind = kind;
            this.behavior = behavior;
            this.utokens = utokens.ToList();
            this.affectedContexts = affectedContexts.ToList();
        }

        public override string ToString(Formatting formatting)
        {
            throw new InvalidOperationException("Should not convert an InsertedUtokens to string");
        }

        public override string ToString()
        {
            return string.Format("{0}, behavior {1}, priority {2}: {{ {3} }}; affected contexts: {{ {4} }}",
                kind,
                behavior,
                priority,
                string.Join(", ", utokens),
                string.Join(", ", affectedContexts)
                );
        }

        public override IEnumerable<Utoken> Flatten()
        {
            return utokens.SelectMany(utoken => utoken.Flatten());
        }

        public int CompareTo(InsertedUtokens that)
        {
            if (that == null || this.priority > that.priority || this.specificScore > that.specificScore)
                return 1;
            else if (this.priority < that.priority || this.specificScore < that.specificScore)
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

        private int? _specificScore;
        private int specificScore
        {
            get
            {
                if (!_specificScore.HasValue)
                    _specificScore = affectedContexts.Sum(context => context.Length);

                // NOTE: BnfTermPartialContext.Any.Length == 0

                return _specificScore.Value;
            }
        }

        internal override bool CreateYinIf(Predicate<Utoken> shouldBeYin, out Utoken createdYin)
        {
            bool existYinChild = false;
            var createdYinChildOrSelfList = new List<UtokenInsert>();

            foreach (UtokenInsert child in this.utokens)
            {
                Utoken createdYinChild;     // it's an UtokenInsert actually

                if (child.CreateYinIf(shouldBeYin, out createdYinChild))
                {
                    createdYinChildOrSelfList.Add((UtokenInsert)createdYinChild);
                    existYinChild = true;
                }
                else
                    createdYinChildOrSelfList.Add(child);
            }

            if (shouldBeYin(this) || existYinChild)
            {
                createdYin = new InsertedUtokens(kind, priority, behavior, createdYinChildOrSelfList, affectedContexts);
                createdYin.MakeYin();
                return true;
            }
            else
            {
                createdYin = null;
                return false;
            }
        }

        // just to avoid cast where using CreateYinIfOrReturnSelf
        public new InsertedUtokens CreateYinIfOrReturnSelf(Predicate<Utoken> shouldBeYin)
        {
            return (InsertedUtokens)base.CreateYinIfOrReturnSelf(utoken => shouldBeYin(utoken));
        }
    }
}
