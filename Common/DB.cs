using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace SqliteDatabase;

public class Database : IDisposable
{
    private string mDataSource;
    private string mUserId;
    private string mPassword;
    private string mConnectionString;

    private List<SqliteConnection> mConnList;


    public Database()
        : base()
    {
        mDataSource = string.Empty;
        mUserId = string.Empty;
        mPassword = string.Empty;
        mConnectionString = string.Empty;

        mConnList = new List<SqliteConnection>();
    }

    public bool DoesDBExist(string flname)
    {
        return System.IO.File.Exists(flname);
    }

    public void SetConnectionAttributes(string flname, string id, string passwd)
    {
        mDataSource = flname;
        mUserId = id;
        mPassword = passwd;

        mConnectionString = string.Format("Data Source={0}; Password={1}; Mode=ReadWrite; Pooling=True",
            flname, passwd);
    }

    public bool IsConnected()
    {
        using var connection = GetConnection();
        if (connection == null)
            return false;

        return connection.State.HasFlag(ConnectionState.Open);
    }

    public bool Connect()
    {
        var connection = GetConnection();
        if (connection == null)
            return false;

        return connection.State.HasFlag(ConnectionState.Open);
    }

    public void Cleanup()
    {
        if (mConnList != null)
        {
            for (var i = 0; i < mConnList.Count; i++)
                DestroyConnection(mConnList[i]);

            mConnList.Clear();
        }
    }

    /// <summary>
    /// Allocate a new SqlConnection to use.
    /// </summary>
    public SqliteConnection AllocConnection()
    {
        var connection = new SqliteConnection(mConnectionString);
        // PDS: Internal pooling so not required, this causes oom error..
        //mConnList.Add(connection);

        return connection;
    }

    /// <summary>
    /// Destroy an existing SqlConnection that is no longer in use.
    /// </summary>
    /// <param name="connection">The connection to be destroyed.</param>
    public void DestroyConnection(SqliteConnection connection)
    {
        if (connection == null)
            return;

        connection.Close();

        connection.Dispose();
        connection = null;
    }

    public SqliteConnection GetConnection()
    {
        var connection = AllocConnection();
        connection.Open();

        return connection;
    }
    
    public int ExecuteNonQuery(string cmdText, params SqliteCommand[] arrParam)
    {
        using (var connection = AllocConnection())
        {
            // Open the connection
            connection.Open();

            // Define the command
            using (var command = GetCommand(connection, cmdText))
            {
                // Handle the parameters
                if (arrParam != null)
                    command.Parameters.AddRange(arrParam);

                var rowCount = command.ExecuteNonQuery();

                return rowCount;
            }
        }
    }

    public object ExecuteScalar(SqliteCommand command)
    {
        using (var connection = AllocConnection())
        {
            // Open the connection
            connection.Open();

            command.Connection = connection;
            return command.ExecuteScalar();
        }
    }

    public object ExecuteScalar(string cmdText, params SqliteParameter[] arrParam)
    {
        using (var connection = AllocConnection())
        {
            // Open the connection
            connection.Open();

            // Define the command
            using (var command = GetCommand(connection, cmdText))
            {
                // Handle the parameters
                if (arrParam != null)
                    command.Parameters.AddRange(arrParam);

                return command.ExecuteScalar();
            }
        }
    }

    public SqliteCommand GetCommand(SqliteConnection connection, string cmdText)
    {
        // Define the command
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = cmdText;

        return command;
    }

    /// <summary>
    /// Execute a query that returns a SqlDataReader.
    /// </summary>
    /// <param name="cmdText"></param>
    /// <param name="arrParam"></param>
    /// <returns></returns>
    public SqliteDataReader ExecuteDataReader(string cmdText, params SqliteParameter[] arrParam)
    {
        // NOTE: Cannot use using statement because the connection will get
        // killed before the reader has chance to read anything by the calling function..
        var connection = GetConnection();
        if (connection == null)
            return null;

        var command = GetCommand(connection, cmdText);

        // Handle the parameters
        if (arrParam != null)
            command.Parameters.AddRange(arrParam);

        // Execute the reader
        return command.ExecuteReader(CommandBehavior.CloseConnection);
    }

    #region "IDisposable Support"
    private bool disposedValue;      // To detect redundant calls

    public void Dispose()
    {
        if (!disposedValue)
        {
            Cleanup();

            mDataSource = string.Empty;
            mUserId = string.Empty;
            mPassword = string.Empty;
            mConnectionString = string.Empty;

            disposedValue = true;
        }
    }
    #endregion
}
