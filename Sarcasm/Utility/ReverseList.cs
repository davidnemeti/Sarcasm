using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sarcasm.Utility
{
    public abstract class ReverseListBase<T>
#if !PCL
        : IReadOnlyCollection<T>
#else
        : IEnumerable
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

#if !PCL
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
