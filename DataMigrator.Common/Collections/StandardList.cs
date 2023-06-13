namespace DataMigrator.Common.Collections;

[Serializable]
public class StandardList<T> : StandardCollection<T>, IList<T>
{
    private readonly List<T> list = new();

    #region IList<T> Members

    public int IndexOf(T item)
    {
        return list.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        list.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        list.RemoveAt(index);
    }

    #endregion IList<T> Members
}