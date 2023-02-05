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

namespace Sarcasm.Unparsing
{
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
}
