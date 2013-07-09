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
    }

    public interface Utoken
    {
        string ToText(Formatting formatting);
    }

    public abstract class UtokenValue : UtokenBase
    {
        public static UtokenValue CreateText(UnparsableObject reference)
        {
            return new UtokenText(reference);
        }

        public static UtokenValue CreateText(string text)
        {
            return new UtokenText(text);
        }

        public static UtokenValue CreateText(string text, UnparsableObject reference)
        {
            return new UtokenText(text, reference);
        }

        public static UtokenValue FurtherUnparse(UnparsableObject unparsableObject)
        {
            return new UtokenToUnparse(unparsableObject);
        }

        public static implicit operator UtokenValue(string text)
        {
            return CreateText(text);
        }
    }

    public class UtokenText : UtokenValue, Utoken
    {
        public string Text { get; private set; }
        public UnparsableObject Reference { get; private set; }
        public object Tag { get; set; }

        internal UtokenText(UnparsableObject reference)
            : this(text: null, reference: reference)
        {
        }

        internal UtokenText(string text, UnparsableObject reference = null)
        {
            this.Text = text;
            this.Reference = reference;
        }

        public static implicit operator UtokenText(string text)
        {
            return new UtokenText(text);
        }

        public string ToText(Formatting formatting)
        {
            return this.Text ?? Util.ToString(formatting.FormatProvider, this.Reference.Obj);
        }

        public override string ToString()
        {
            string text = this.Text ?? this.Reference.ToString();
            return ToString("\"{0}\"{1}", text, Reference != null ? " (with ref)" : string.Empty);
        }
    }

    public class UtokenToUnparse : UtokenValue
    {
        public UnparsableObject UnparsableObject { get; private set; }

        internal UtokenToUnparse(UnparsableObject unparsableObject)
        {
            this.UnparsableObject = unparsableObject;
        }

        public override string ToString()
        {
            return ToString("UtokenToUnparse: " + UnparsableObject.ToString());
        }
    }

    public abstract class UtokenInsert : UtokenBase
    {
        public static UtokenInsert NewLine { get { return UtokenWhitespace.NewLine; } }
        public static UtokenInsert EmptyLine { get { return UtokenWhitespace.EmptyLine; } }
        public static UtokenInsert Space { get { return UtokenWhitespace.Space; } }
        public static UtokenInsert Tab { get { return UtokenWhitespace.Tab; } }

        public static UtokenInsert NoWhitespace { get { return UtokenControl.NoWhitespace; } }
    }

    public class UtokenWhitespace : UtokenInsert, Utoken
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

        public string ToText(Formatting formatting)
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

        public string ToText(Formatting formatting)
        {
            return string.Concat(Enumerable.Repeat(formatting.IndentUnit, IndentLevel));
        }

        public override string ToString()
        {
            return ToString("indent level: {0}", IndentLevel);
        }
    }

    internal class UtokenRepeat : UtokenBase
    {
        private readonly UtokenBase utoken;
        private readonly int count;

        public UtokenRepeat(UtokenBase utoken, int count)
        {
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

    internal class InsertedUtokens : UtokenBase, IComparable<InsertedUtokens>
    {
        public enum Kind { Left, Right, Between }

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

        public override IEnumerable<UtokenBase> Flatten()
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
    }

    internal class DeferredUtokens : UtokenBase
    {
        private readonly Func<IEnumerable<UtokenBase>> utokenYielder;
        private IReadOnlyList<UtokenBase> calculatedUtokens;

        public UnparsableObject Self { get; private set; }
        private readonly string helpMessage;
        private readonly object helpCalculatedObject;

        public DeferredUtokens(Func<IEnumerable<UtokenBase>> utokenYielder, UnparsableObject self, string helpMessage = null, object helpCalculatedObject = null)
        {
            this.utokenYielder = utokenYielder;
            this.Self = self;
            this.helpMessage = helpMessage;
            this.helpCalculatedObject = helpCalculatedObject;
            this.calculatedUtokens = null;

            self.IsLeftSiblingNeededForDeferredCalculation = true;
        }

        public IReadOnlyList<UtokenBase> GetUtokens()
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
}
