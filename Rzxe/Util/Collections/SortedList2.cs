using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oddmatics.Rzxe.Util.Collections
{
    public class SortedList2<T> : ICollection<T>
    {
        public int Count { get { return BackingList.Count; } }

        public bool IsReadOnly { get { return false; } }


        private List<T> BackingList { get; set; }

        private IComparer<T> Comparer { get; set; }


        public SortedList2(
            IComparer<T> comparer
        )
        {
            BackingList = new List<T>();
            Comparer    = comparer;
        }

        public SortedList2(
            IEnumerable<T> collection,
            IComparer<T> comparer
        )
        {
            BackingList = new List<T>(collection);
            Comparer    = comparer;

            BackingList.Sort(Comparer);
        }


        public void Add(T item)
        {
            // TODO: Improve this (currently O(n))
            //
            int total = Count;

            for (int i = 0; i < total; i++)
            {
                if (Comparer.Compare(item, BackingList[i]) < 0)
                {
                    BackingList.Insert(i, item);
                }
            }

            BackingList.Add(item);
        }

        public void Clear()
        {
            BackingList.Clear();
        }

        public bool Contains(T item)
        {
            return BackingList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            BackingList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return BackingList.GetEnumerator();
        }

        public bool Remove(T item)
        {
            return BackingList.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return BackingList.GetEnumerator();
        }
    }
}
