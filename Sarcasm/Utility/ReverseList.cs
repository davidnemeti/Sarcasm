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
    public abstract class ReverseListBase<T>
#if NET4_0
        : IEnumerable<T>
#else
        : IReadOnlyCollection<T>
#endif
    {
        public abstract int Count { get; }

        public IEnumerator<T> GetEnumerator()
        {
            for (int reverseIndex = 0; reverseIndex < Count; reverseIndex++)
                yield return GetItem(reverseIndex);
        }

        protected abstract T GetItem(int reverseIndex);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected int IndexToReverseIndex(int index)
        {
            return this.Count - 1 - index;
        }

        protected int ReverseIndexToIndex(int reverseIndex)
        {
            return this.Count - 1 - reverseIndex;
        }
    }

#if !NET4_0
    public class ReverseReadOnlyList<T> : ReverseListBase<T>, IReadOnlyList<T>
    {
        private readonly IReadOnlyList<T> items;

        public ReverseReadOnlyList(IReadOnlyList<T> items)
        {
            this.items = items;
        }

        public T this[int reverseIndex]
        {
            get { return items[ReverseIndexToIndex(reverseIndex)]; }
        }

        public override int Count
        {
            get { return items.Count; }
        }

        protected override T GetItem(int reverseIndex)
        {
            return this[reverseIndex];
        }
    }
#endif

    public class ReverseList<T> : ReverseListBase<T>, IList<T>
    {
        private readonly IList<T> items;

        public ReverseList(IList<T> items)
        {
            this.items = items;
        }

        public int IndexOf(T item)
        {
            for (int index = items.Count - 1; index >= 0; index--)
            {
                if (EqualityComparer<T>.Default.Equals(items[index], item))
                    return IndexToReverseIndex(index);
            }

            return -1;
        }

        public void Insert(int reverseIndex, T item)
        {
            int index = ReverseIndexToIndex(reverseIndex) + 1;

            if (index < items.Count)
                items.Insert(index, item);
            else
                items.Add(item);
        }

        public void RemoveAt(int reverseIndex)
        {
            items.RemoveAt(ReverseIndexToIndex(reverseIndex));
        }

        public T this[int reverseIndex]
        {
            get { return items[ReverseIndexToIndex(reverseIndex)]; }
            set { items[ReverseIndexToIndex(reverseIndex)] = value; }
        }

        public void Add(T item)
        {
            items.Insert(0, item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (T item in this)
                array[arrayIndex++] = item;
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public bool IsReadOnly
        {
            get { return items.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            for (int index = items.Count - 1; index >= 0; index--)
            {
                if (EqualityComparer<T>.Default.Equals(items[index], item))
                {
                    RemoveAt(IndexToReverseIndex(index));
                    return true;
                }
            }

            return false;
        }

        protected override T GetItem(int reverseIndex)
        {
            return this[reverseIndex];
        }
    }
}
