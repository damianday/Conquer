namespace GameServer.Database;

public sealed class DataMonitor<T>
{
    public delegate void ChangedDelegate(T value);

    private T m_Value;
    private readonly DBObject m_Data;

    public T V
    {
        get { return m_Value; }
        set
        {
            m_Value = value;
            this.Changed?.Invoke(value);
            if (m_Data != null)
            {
                m_Data.IsModified = true;
                Session.Modified = true;
            }
        }
    }

    public event ChangedDelegate Changed;

    public void SetValueInternal(T value)
    {
        m_Value = value;
    }

    public DataMonitor(DBObject data)
    {
        m_Data = data;
    }

    public override string ToString()
    {
        return m_Value?.ToString();
    }
}
