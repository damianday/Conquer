using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace GameServer.Database;

public sealed class DBMapping
{
    public Type Type;
    public List<DBValue> Properties { get; } = new List<DBValue>();

    public override string ToString()
    {
        return Type?.Name;
    }

    public DBMapping(BinaryReader reader)
    {
        Properties = new List<DBValue>();

        var name = reader.ReadString();
        Type = Assembly.GetEntryAssembly().GetType(name) ?? Assembly.GetCallingAssembly().GetType(name);
        var count = reader.ReadInt32();
        for (var i = 0; i < count; i++)
        {
            Properties.Add(new DBValue(reader, Type));
        }
    }

    public DBMapping(Type type)
    {
        Type = type;

        var fields = Type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var fieldInfo in fields)
        {
            if (fieldInfo.FieldType.IsGenericType)
            {
                Type genericTypeDefinition = fieldInfo.FieldType.GetGenericTypeDefinition();
                if (!(genericTypeDefinition != typeof(DataMonitor<>)) || !(genericTypeDefinition != typeof(ListMonitor<>)) || !(genericTypeDefinition != typeof(HashMonitor<>)) || !(genericTypeDefinition != typeof(DictionaryMonitor<,>)))
                {
                    Properties.Add(new DBValue(fieldInfo));
                }
            }
        }
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Type.FullName);
        writer.Write(Properties.Count);
        foreach (DBValue item in Properties)
        {
            item.Save(writer);
        }
    }

    public bool IsMatch(DBMapping mapping)
    {
        if (Properties.Count != mapping.Properties.Count)
            return false;

        for (var i = 0; i < Properties.Count; i++)
            if (!Properties[i].IsMatch(mapping.Properties[i])) return false;

        return true;
    }
}
