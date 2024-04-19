using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Database;

public sealed class ListMonitor<T> : IEnumerable<T>, IEnumerable
{
    public delegate void ChangedDelegate(List<T> 更改列表);

    private List<T> m_Value;
    private readonly DBObject m_Data;

    public T Last
    {
        get
        {
            if (m_Value.Count != 0)
                return m_Value.Last();
            return default(T);
        }
    }

    public T this[int index]
    {
        get
        {
            if (index >= m_Value.Count)
                return default(T);
            return m_Value[index];
        }
        set
        {
            if (index < m_Value.Count)
            {
                m_Value[index] = value;
                Changed?.Invoke(m_Value.ToList());
                Updated();
            }
        }
    }

    public IList IList => m_Value;
    public int Count => m_Value.Count;
    public event ChangedDelegate Changed;

    public ListMonitor(DBObject data)
    {
        m_Value = new List<T>();
        m_Data = data;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return m_Value.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)m_Value).GetEnumerator();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return m_Value.GetEnumerator();
    }

    public override string ToString()
    {
        return m_Value?.Count.ToString();
    }

    private void Updated()
    {
        if (m_Data != null)
        {
            m_Data.IsModified = true;
            Session.Modified = true;
        }
    }

    public List<T> GetRange(int index, int count)
    {
        return m_Value.GetRange(index, count);
    }

    public void Add(T item)
    {
        m_Value.Add(item);
        this.Changed?.Invoke(m_Value.ToList());
        Updated();
    }

    public void Insert(int index, T item)
    {
        m_Value.Insert(index, item);
        this.Changed?.Invoke(m_Value.ToList());
        Updated();
    }

    public void Remove(T item)
    {
        if (m_Value.Remove(item))
        {
            this.Changed?.Invoke(m_Value.ToList());
            Updated();
        }
    }

    public void RemoveAt(int i)
    {
        if (m_Value.Count > i)
        {
            m_Value.RemoveAt(i);
            this.Changed?.Invoke(m_Value.ToList());
            Updated();
        }
    }

    public void Clear()
    {
        if (m_Value.Count > 0)
        {
            m_Value.Clear();
            this.Changed?.Invoke(m_Value.ToList());
            Updated();
        }
    }

    public void SetValue(List<T> list)
    {
        m_Value = list;
        this.Changed?.Invoke(m_Value.ToList());
        Updated();
    }

    public void QuietlyAdd(T item)
    {
        m_Value.Add(item);
    }
}
