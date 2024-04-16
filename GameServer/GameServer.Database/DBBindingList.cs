using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace GameServer.Database;

public sealed class DBBindingList<T> : DBCollection where T : DBObject, new()
{
    public DBBindingList()
    {
        var comparer = StringComparer.OrdinalIgnoreCase;

        Mapping = new DBMapping(Type = typeof(T));
        DataSheet = new Dictionary<int, DBObject>();
        SearchTable = new Dictionary<string, DBObject>(comparer);

        SearchAttribute attr = Type.GetCustomAttribute<SearchAttribute>();
        if (attr != null)
        {
            SearchField = Type.GetField(attr.SearchName, BindingFlags.Instance | BindingFlags.Public);
        }
        if (Type == typeof(GuildInfo))
            Index = 1610612736;
        if (Type == typeof(TeamInfo))
            Index = 1879048192;
    }

    public override void Add(DBObject data, bool indexed = false)
    {
        if (indexed)
            data.Index.V = ++Index;

        if (data.Index.V == 0)
            MessageBox.Show("The data table is added with a data exception, and the index is zero.");

        data.Collection = this;
        DataSheet.Add(data.Index.V, data);
        if (SearchField != null)
        {
            SearchTable.Add((SearchField.GetValue(data) as DataMonitor<string>).V, data);
        }
        Session.Modified = true;
    }

    public override void Remove(DBObject data)
    {
        DataSheet.Remove(data.Index.V);
        if (SearchField != null)
        {
            SearchTable.Remove((SearchField.GetValue(data) as DataMonitor<string>).V);
        }
        Session.Modified = true;
    }

    public override void Save()
    {
        foreach (var kvp in DataSheet)
        {
            if (!Version || kvp.Value.IsModified)
                kvp.Value.Save();
        }
        Version = true;
    }

    public override void ForcedSave()
    {
        foreach (var kvp in DataSheet)
            kvp.Value.Save();
        Version = true;
    }

    public override void Load(byte[] data, DBMapping mapping)
    {
        Version = mapping.IsMatch(Mapping);

        using MemoryStream ms = new MemoryStream(data);
        using BinaryReader reader = new BinaryReader(ms);
        Index = reader.ReadInt32();
        int count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            T val = new T { Collection = this };
            val.RawData = reader.ReadBytes(reader.ReadInt32());
            val.Load(mapping);
            DataSheet[val.Index.V] = val;
            if (SearchField != null && SearchField.GetValue(val) is DataMonitor<string> monitor && monitor.V != null)
            {
                SearchTable[monitor.V] = val;
            }
        }
        SMain.AddSystemLog($"{Type.Name} Loaded, Quantity: {count}");
    }

    public override byte[] ToArray()
    {
        using MemoryStream ms = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(ms);
        writer.Write(Index);
        writer.Write(DataSheet.Count);
        foreach (var kvp in DataSheet)
        {
            writer.Write(kvp.Value.RawData.Length);
            writer.Write(kvp.Value.RawData);
        }
        ms.Seek(4, SeekOrigin.Begin);
        return ms.ToArray();
    }
}
