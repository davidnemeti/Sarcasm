﻿using System;
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
    public class Discriminator { }

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

        internal Unparser UnparserForAsyncLock;
    }

    public interface Utoken
    {
        string Text { get; }
        IDecoration Decoration { get; }
        bool HasDecoration();
        Discriminator Discriminator { get; }
    }

    internal interface IHasTextToBeSet
    {
        void SetText(Formatter formatter);
    }

    public abstract class UtokenValue : UtokenBase
    {
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

    public class UtokenText : UtokenValue, Utoken, IHasTextToBeSet
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

        void IHasTextToBeSet.SetText(Formatter formatter)
        {
            if (this.Text == null)
                this.Text = Util.ToString(formatter.FormatProvider, this.Reference.AstValue);
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

    public class UtokenWhitespace : UtokenInsert, Utoken, IHasTextToBeSet
    {
        internal enum Kind { NewLine, EmptyLine, Space, Tab, WhiteSpaceBetweenUtokens }

        public string Text { get; private set; }
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

        public override string ToString()
        {
            return ToString("." + kind);
        }

        void IHasTextToBeSet.SetText(Formatter formatter)
        {
            switch (kind)
            {
                case Kind.NewLine:
                    this.Text = formatter.NewLine;
                    break;

                case Kind.EmptyLine:
                    this.Text = formatter.NewLine + formatter.NewLine;
                    break;

                case Kind.Space:
                    this.Text = formatter.Space;
                    break;

                case Kind.Tab:
                    this.Text = formatter.Tab;
                    break;

                case Kind.WhiteSpaceBetweenUtokens:
                    this.Text = formatter.WhiteSpaceBetweenUtokens;
                    break;

                default:
                    throw new InvalidOperationException("Unknown UtokenWhitespace");
            }
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

    public class UtokenIndent : UtokenBase, Utoken, IHasTextToBeSet
    {
        public int IndentLevel { get; private set; }
        public string Text { get; private set; }

        internal UtokenIndent(int indentLevel)
        {
            if (indentLevel < 0)
                throw new ArgumentOutOfRangeException("indentLevel");

            this.IndentLevel = indentLevel;
        }

        public override string ToString()
        {
            return ToString("indent level: {0}", IndentLevel);
        }

        void IHasTextToBeSet.SetText(Formatter formatter)
        {
            this.Text = string.Concat(Enumerable.Repeat(formatter.IndentUnit, IndentLevel));
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
