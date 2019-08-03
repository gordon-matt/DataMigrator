using System;
using System.Collections.Generic;

namespace DataMigrator.Common.Collections
{
    [Serializable]
    public class StandardCollection<T> : ICollection<T>
    {
        private List<T> items = new List<T>();

        public T this[int index]
        {
            get { return items[index]; }
            set { items[index] = value; }
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                this.Add(item);
            }
        }

        public void Sort(IComparer<T> comparer) => items.Sort(comparer);

        #region ICollection<T> Members

        public void Add(T item) => items.Add(item);

        public void Clear() => items.Clear();

        public bool Contains(T item) => items.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

        public int Count => items.Count;

        public bool IsReadOnly => false;

        public bool Remove(T item) => items.Remove(item);

        #endregion ICollection<T> Members

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in items)
            {
                yield return item;
            }
        }

        #endregion IEnumerable<T> Members

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (T item in items)
            {
                yield return item;
            }
        }

        #endregion IEnumerable Members

        public override string ToString() => string.Concat("Count: ", this.Count);
    }
}