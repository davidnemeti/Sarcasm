using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Globalization;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.GrammarAst;
using Sarcasm.Utility;

using Grammar = Sarcasm.GrammarAst.Grammar;

namespace Sarcasm.Unparsing
{
    public class Formatting
    {
        #region Default values

        private static readonly string newLineDefault = Environment.NewLine;
        private const string spaceDefault = " ";
        private const string tabDefault = "\t";
        private static readonly string indentUnitDefault = string.Concat(Enumerable.Repeat(spaceDefault, 4));
        private const string whiteSpaceBetweenUtokensDefault = spaceDefault;
        private const bool indentEmptyLinesDefault = false;
        private static readonly IFormatProvider formatProviderDefault = CultureInfo.InvariantCulture;

        #endregion

        #region State

        public string NewLine { get; set; }
        public string Space { get; set; }
        public string Tab { get; set; }
        public string IndentUnit { get; set; }
        public string WhiteSpaceBetweenUtokens { get; set; }
        public bool IndentEmptyLines { get; set; }
        public IFormatProvider FormatProvider { get; private set; }

        #endregion

        public CultureInfo CultureInfo { get { return FormatProvider as CultureInfo; } }

        #region Construction

        public Formatting()
            : this(formatProviderDefault)
        {
        }

        public Formatting(Grammar grammar)
            : this(grammar.DefaultCulture)
        {
        }

        public Formatting(Parser parser)
            : this(parser.Context.Culture)
        {
        }

        protected Formatting(IFormatProvider formatProvider)
        {
            this.FormatProvider = formatProvider;

            this.NewLine = newLineDefault;
            this.Space = spaceDefault;
            this.Tab = tabDefault;
            this.IndentUnit = indentUnitDefault;
            this.WhiteSpaceBetweenUtokens = whiteSpaceBetweenUtokensDefault;
            this.IndentEmptyLines = indentEmptyLinesDefault;
        }

        #endregion

        #region Interface to grammar

        public void SetFormatProviderIndependentlyFromParser(IFormatProvider formatProvider)
        {
            this.FormatProvider = formatProvider;

        }

        public void SetCultureInfoIndependentlyFromParser(CultureInfo cultureInfo)
        {
            SetFormatProviderIndependentlyFromParser(cultureInfo);
        }

        public void SetCultureInfo(CultureInfo cultureInfo, Parser parser)
        {
            SetCultureInfoIndependentlyFromParser(cultureInfo);
            parser.Context.Culture = cultureInfo;
        }

        #endregion

        #region Interface to unparser

        internal InsertedUtokens _GetUtokensLeft(UnparsableObject target)
        {
            return GetUtokensLeft(target)
                .SetKind(InsertedUtokens.Kind.Left)
                .SetAffected(target);
        }

        internal InsertedUtokens _GetUtokensRight(UnparsableObject target)
        {
            return GetUtokensRight(target)
                .SetKind(InsertedUtokens.Kind.Right)
                .SetAffected(target);
        }

        internal InsertedUtokens _GetUtokensBetween(UnparsableObject leftTarget, UnparsableObject rightTarget)
        {
            return GetUtokensBetween(leftTarget, rightTarget)
                .SetKind(InsertedUtokens.Kind.Between)
                .SetAffected(leftTarget, rightTarget);
        }

        internal BlockIndentation _GetBlockIndentation(UnparsableObject leftIfAny, UnparsableObject target)
        {
            return GetBlockIndentation(leftIfAny, target);
        }

        internal IDecoration _GetDecoration(UnparsableObject target)
        {
            return GetDecoration(target);
        }

        #endregion

        #region Virtuals

        protected virtual InsertedUtokens GetUtokensLeft(UnparsableObject target)
        {
            return InsertedUtokens.None;
        }

        protected virtual InsertedUtokens GetUtokensRight(UnparsableObject target)
        {
            return InsertedUtokens.None;
        }

        protected virtual InsertedUtokens GetUtokensBetween(UnparsableObject leftTarget, UnparsableObject rightTarget)
        {
            return InsertedUtokens.None;
        }

        protected virtual BlockIndentation GetBlockIndentation(UnparsableObject leftIfAny, UnparsableObject target)
        {
            return BlockIndentation.IndentNotNeeded;
        }

        protected virtual IDecoration GetDecoration(UnparsableObject target)
        {
            return Decoration.None;
        }

        #endregion
    }

    #region BlockIndentation

    public class BlockIndentation : IEquatable<BlockIndentation>
    {
        internal enum Kind
        {
            ToBeSet,
            IndentNotNeeded,
            Deferred,
            Indent,
            Unindent,
            ZeroIndent,
        }

        private Kind kind;
        private readonly bool isReadonly;
        private DeferredUtokens leftDeferred;
        private DeferredUtokens rightDeferred;

        private BlockIndentation(Kind kind, bool isReadonly)
        {
            this.kind = kind;
            this.isReadonly = isReadonly;
        }

        internal static readonly BlockIndentation ToBeSet = new BlockIndentation(Kind.ToBeSet, isReadonly: true);
        internal static readonly BlockIndentation IndentNotNeeded = new BlockIndentation(Kind.IndentNotNeeded, isReadonly: true);
        public static readonly BlockIndentation Indent = new BlockIndentation(Kind.Indent, isReadonly: true);
        public static readonly BlockIndentation Unindent = new BlockIndentation(Kind.Unindent, isReadonly: true);
        public static readonly BlockIndentation ZeroIndent = new BlockIndentation(Kind.ZeroIndent, isReadonly: true);

        internal static BlockIndentation Defer()
        {
            return new BlockIndentation(Kind.Deferred, isReadonly: false);
        }

        internal void CopyKindFrom(BlockIndentation source)
        {
            if (this.isReadonly)
                throw new InvalidOperationException("Internal error: cannot change a readonly BlockIndentation");

            this.kind = source.kind;
        }

        public override string ToString()
        {
            return ":" + kind + ":";
        }

        internal bool IsDeferred()
        {
            return kind == Kind.Deferred;
        }

        internal DeferredUtokens LeftDeferred
        {
            set
            {
                if (leftDeferred != null)
                    throw new InvalidOperationException("Double set is not allowed for LeftDeferred");

                leftDeferred = value;
            }
            get { return leftDeferred; }
        }

        internal DeferredUtokens RightDeferred
        {
            set
            {
                if (rightDeferred != null)
                    throw new InvalidOperationException("Double set is not allowed for RightDeferred");

                rightDeferred = value;
            }
            get { return rightDeferred; }
        }

        public bool Equals(BlockIndentation that)
        {
            return object.ReferenceEquals(this, that)
                ||
                !object.ReferenceEquals(that, null) &&
                this.kind == that.kind;
        }

        public override bool Equals(object obj)
        {
            return obj is BlockIndentation && Equals((BlockIndentation)obj);
        }

        public override int GetHashCode()
        {
            return this.kind.GetHashCode();
        }

        public static bool operator ==(BlockIndentation context1, BlockIndentation context2)
        {
            return object.ReferenceEquals(context1, context2) || !object.ReferenceEquals(context1, null) && context1.Equals(context2);
        }

        public static bool operator !=(BlockIndentation context1, BlockIndentation context2)
        {
            return !(context1 == context2);
        }
    }

    #endregion

    internal static class InsertedUtokensExtensions
    {
        public static InsertedUtokens SetKind(this InsertedUtokens insertedUtokens, InsertedUtokens.Kind kind)
        {
            if (insertedUtokens != null)
                insertedUtokens.kind = kind;

            return insertedUtokens;
        }

        public static InsertedUtokens SetAffected(this InsertedUtokens insertedUtokens, params UnparsableObject[] affectedUnparsableObjects)
        {
            if (insertedUtokens != null)
                insertedUtokens.affectedUnparsableObjects = affectedUnparsableObjects;

            return insertedUtokens;
        }
    }
}
