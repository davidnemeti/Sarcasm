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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sarcasm.Utility
{
    public class AutoCleanup : IDisposable
    {
        public static readonly AutoCleanup None = new AutoCleanup(executeOnDispose: null);

        private bool disposed;
        private readonly Action executeOnDispose;

        public AutoCleanup(Action executeOnConstruct, Action executeOnDispose)
        {
            if (null != executeOnConstruct)
                executeOnConstruct();

            this.executeOnDispose = executeOnDispose;
        }

        public AutoCleanup(Action executeOnDispose)
        {
            this.executeOnDispose = executeOnDispose;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.executeOnDispose != null)
                        this.executeOnDispose();
                }

                this.disposed = true;
            }
        }

        #endregion
    }

    public class AutoCleanupValue<T>
    {
        private T value;

        public AutoCleanupValue(T initValue = default(T))
        {
            this.value = initValue;
        }

        public AutoCleanup SetAutoUnset(T newValue)
        {
            T oldValue = this.value;

            return new AutoCleanup(
                () => this.value = newValue,
                () => this.value = oldValue
                );
        }

        public static implicit operator T(AutoCleanupValue<T> counter)
        {
            return counter.Value;
        }

        public T Value { get { return value; } }
    }

    public class AutoCleanupCounter
    {
        private int value;

        public AutoCleanupCounter(int initValue = 0)
        {
            this.value = initValue;
        }

        public AutoCleanup IncrAutoDecr()
        {
            return new AutoCleanup(
                () => this.value++,
                () => this.value--
                );
        }

        public static implicit operator int(AutoCleanupCounter counter)
        {
            return counter.Value;
        }

        public int Value { get { return value; } }
    }

    public class AutoCleanupStack<T> : IEnumerable<T>, ICollection
    {
        private Stack<T> stack;

        public AutoCleanupStack()
        {
            stack = new Stack<T>();
        }

        public AutoCleanupStack(int capacity)
        {
            stack = new Stack<T>(capacity);
        }

        public AutoCleanupStack(IEnumerable<T> items)
        {
            this.stack = new Stack<T>(items);
        }

        public AutoCleanupStack(params T[] items)
            : this((IEnumerable<T>)items)
        {
        }

        public void Clear()
        {
            stack.Clear();
        }

        public bool Contains(T item)
        {
            return stack.Contains(item);
        }

        // NOTE: separate Push and Pop are missing intentionally
        public AutoCleanup PushAutoPop(T itemToPush)
        {
            return new AutoCleanup(
                () => stack.Push(itemToPush),
                () => stack.Pop()
                );
        }

        public T Peek()
        {
            return stack.Peek();
        }

        public T[] ToArray()
        {
            return stack.ToArray();
        }

        public void TrimExcess()
        {
            stack.TrimExcess();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.stack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.stack).GetEnumerator();
        }

        public void CopyTo(T[] array, int index)
        {
            stack.CopyTo(array, index);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)stack).CopyTo(array, index);
        }

        public int Count
        {
            get { return stack.Count; }
        }

        public bool IsSynchronized
        {
            get { return ((ICollection)stack).IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return ((ICollection)stack).SyncRoot; }
        }
    }

    public static class AutoCleanupExtensions
    {
        public static AutoCleanup PushAutoPop<T>(this Stack<T> stack, T itemToPush)
        {
            return new AutoCleanup(
                () => stack.Push(itemToPush),
                () => stack.Pop()
                );
        }
    }
}
