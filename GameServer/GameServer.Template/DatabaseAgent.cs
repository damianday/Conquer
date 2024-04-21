using System;

using SDatabase = SqliteDatabase.Database;

namespace GameServer.Template;

public partial class DBAgent
{
    public static DBAgent X { get; } = new DBAgent();

    private SDatabase m_DB = new SDatabase();

    public SDatabase DB => m_DB;
    public bool Connected => m_DB.IsConnected();

    public bool InitDB(string dbName)
    {
        try
        {
            if (m_DB.DoesDBExist(dbName))
            {
                m_DB.SetConnectionAttributes(dbName, string.Empty, string.Empty);
                return m_DB.Connect();
            }
        }
        catch (Exception err)
        {
            SMain.AddSystemLog(err.ToString());
        }

        return false;
    }

    public bool UninitDB()
    {
        m_DB.Dispose();

        return true;
    }
}