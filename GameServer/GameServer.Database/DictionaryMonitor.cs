using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Database;

public sealed class DictionaryMonitor<TK, TV> : IEnumerable<KeyValuePair<TK, TV>>, IEnumerable
{
    public delegate void ChangedDelegate(List<KeyValuePair<TK, TV>> 更改字典);

    private readonly Dictionary<TK, TV> m_Value;
    private readonly DBObject m_Data;

    public TV this[TK key]
    {
        get
        {
            if (m_Value.TryGetValue(key, out var value))
                return value;
            return default(TV);
        }
        set
        {
            m_Value[key] = value;
            Changed?.Invoke(m_Value.ToList());
            Updated();
        }
    }

    public ICollection<TK> Keys => m_Value.Keys;
    public ICollection<TV> Values => m_Value.Values;
    public IDictionary IDictionary_0 => m_Value;
    public int Count => m_Value.Count;

    public event ChangedDelegate Changed;

    public DictionaryMonitor(DBObject data)
    {
        m_Value = new Dictionary<TK, TV>();
        m_Data = data;
    }

    public bool ContainsKey(TK key)
    {
        return m_Value.ContainsKey(key);
    }

    public bool TryGetValue(TK key, out TV value)
    {
        return m_Value.TryGetValue(key, out value);
    }

    public void Add(TK key, TV value)
    {
        m_Value.Add(key, value);
        Changed?.Invoke(m_Value.ToList());
        Updated();
    }

    public bool Remove(TK key)
    {
        if (m_Value.Remove(key))
        {
            Changed?.Invoke(m_Value.ToList());
            Updated();
            return true;
        }
        return false;
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

    public void AddInternal(TK key, TV value)
    {
        m_Value.Add(key, value);
    }

    public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
    {
        return m_Value.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)m_Value).GetEnumerator();
    }

    IEnumerator<KeyValuePair<TK, TV>> IEnumerable<KeyValuePair<TK, TV>>.GetEnumerator()
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
}
