using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Database;

public sealed class HashMonitor<T> : IEnumerable<T>, IEnumerable
{
    public delegate void ChangedDelegate(List<T> 更改列表);

    private readonly HashSet<T> m_Value;
    private readonly DBObject m_Data;

    public int Count => m_Value.Count;
    public ISet<T> ISet => m_Value;
    public event ChangedDelegate Changed;

    public HashMonitor(DBObject data)
    {
        m_Value = new HashSet<T>();
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

    public void Clear()
    {
        if (m_Value.Count > 0)
        {
            m_Value.Clear();
            Changed?.Invoke(m_Value.ToList());
            Updated();
        }
    }

    public bool Add(T item)
    {
        if (m_Value.Add(item))
        {
            Changed?.Invoke(m_Value.ToList());
            Updated();
            return true;
        }
        return false;
    }

    public bool Remove(T item)
    {
        if (m_Value.Remove(item))
        {
            Changed?.Invoke(m_Value.ToList());
            Updated();
            return true;
        }
        return false;
    }

    public void QuietlyAdd(T item)
    {
        m_Value.Add(item);
    }

    public bool Contains(T item)
    {
        return m_Value.Contains(item);
    }
}
