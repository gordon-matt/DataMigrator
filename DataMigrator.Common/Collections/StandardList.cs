using System;
using System.Collections.Generic;

namespace DataMigrator.Common.Collections
{
    [Serializable]
    public class StandardList<T> : StandardCollection<T>, IList<T>
    {
        private List<T> list = new List<T>();

        #region IList<T> Members

        public int IndexOf(T item) => list.IndexOf(item);

        public void Insert(int index, T item) => list.Insert(index, item);

        public void RemoveAt(int index) => list.RemoveAt(index);

        #endregion IList<T> Members
    }
}