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

using System.Collections.Generic;

namespace Sarcasm.Utility
{
    public class Bag<T>
    {
        private IDictionary<T, int> element_count;
        private int countAll = 0;

        public Bag()
        {
            element_count = new Dictionary<T, int>();
        }

        public Bag(IEqualityComparer<T> comparer)
        {
            element_count = new Dictionary<T, int>(comparer);
        }

        public Bag(IEnumerable<T> rgelement)
        {
            element_count = new Dictionary<T, int>();
            AddRange(rgelement);
        }

        public Bag(IEnumerable<T> rgelement, IEqualityComparer<T> comparer)
        {
            element_count = new Dictionary<T, int>(comparer);
            AddRange(rgelement);
        }

        public bool Contains(T element)
        {
            return element_count.ContainsKey(element);
        }

        public void Add(T element)
        {
            if (element_count.ContainsKey(element))
                element_count[element]++;
            else
                element_count[element] = 1;

            countAll++;
        }

        public void AddRange(IEnumerable<T> rgelement)
        {
            foreach (T key in rgelement)
                Add(key);
        }

        public bool Remove(T element)
        {
            if (element_count.ContainsKey(element))
            {
                int count = element_count[element];

                countAll--;

                if (count <= 1)
                    return element_count.Remove(element);
                else
                {
                    element_count[element] = count - 1;
                    return true;
                }
            }
            else
                return false;
        }

        public void Clear()
        {
            element_count.Clear();
        }

        public int CountAll { get { return countAll; } }

        public int Count { get { return element_count.Count; } }

        public int GetCount(T element)
        {
            if (element_count.ContainsKey(element))
                return element_count[element];
            else
                return 0;
        }

        public IEnumerable<T> Elements()
        {
            return element_count.Keys;
        }

        public IEnumerable<T> ElementsAll()
        {
            foreach (T element in element_count.Keys)
            {
                int count = element_count[element];
                for (int i = 0; i < count; i++)
                    yield return element;
            }
        }

        public IEnumerable<KeyValuePair<T, int>> ElementsWithCount()
        {
            return element_count;
        }
    }
}
