using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameServer.Database;

public abstract class DBCollection
{
    public int Index { get; set; }
    public bool Version { get; set; }
    public Type Type { get; set; }
    public FieldInfo SearchField { get; set; }
    public DBMapping Mapping { get; set; }

    public Dictionary<int, DBObject> DataSheet;
    public Dictionary<string, DBObject> SearchTable;

    internal DBObject this[int id]
    {
        get
        {
            if (DataSheet.TryGetValue(id, out var value))
                return value;
            return null;
        }
    }

    internal DBObject this[string name]
    {
        get
        {
            if (SearchTable.TryGetValue(name, out var value))
                return value;
            return null;
        }
    }

    public override string ToString() => Type?.Name;

    public abstract void Load(byte[] data, DBMapping mapping);
    public abstract void Save();
    public abstract void ForcedSave();
    public abstract void Remove(DBObject data);
    public abstract void Add(DBObject data, bool index = false);
    public abstract byte[] ToArray();
}
