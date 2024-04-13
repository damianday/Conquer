using System;
using System.IO;
using System.Reflection;

namespace GameServer.Database;

public abstract class DBObject
{
    public readonly DataMonitor<int> Index;
    public readonly Type DataType;
    public readonly MemoryStream MemStream;
    public readonly BinaryWriter Writer;

    public byte[] RawData { get; set; }
    public bool IsModified { get; set; }
    public DBCollection Collection { get; set; }

    protected void CreateBindings()
    {
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (FieldInfo fieldInfo in fields)
        {
            if (fieldInfo.FieldType.IsGenericType)
            {
                Type genericTypeDefinition = fieldInfo.FieldType.GetGenericTypeDefinition();
                if (!(genericTypeDefinition != typeof(DataMonitor<>)) || !(genericTypeDefinition != typeof(ListMonitor<>)) || !(genericTypeDefinition != typeof(HashMonitor<>)) || !(genericTypeDefinition != typeof(DictionaryMonitor<,>)))
                {
                    fieldInfo.SetValue(this, Activator.CreateInstance(fieldInfo.FieldType, this));
                }
            }
        }
    }

    public override string ToString() => DataType?.Name;

    public DBObject()
    {
        DataType = GetType();
        MemStream = new MemoryStream();
        Writer = new BinaryWriter(MemStream);
        CreateBindings();
    }

    public void Save()
    {
        MemStream.SetLength(0);
        foreach (DBValue val in Collection.Mapping.Properties)
        {
            val.WriteValue(Writer, val.Field.GetValue(this));
        }
        RawData = MemStream.ToArray();
        IsModified = false;
    }

    public void Load(DBMapping mapping)
    {
        using var ms = new MemoryStream(RawData);
        using var reader = new BinaryReader(ms);
        foreach (DBValue item in mapping.Properties)
        {
            object value = item.ReadValue(reader, this, item);
            if (!(item.Field == null) && item.FieldType == item.Field.FieldType)
            {
                item.Field.SetValue(this, value);
            }
        }
    }

    public virtual void Remove()
    {
        Collection?.Remove(this);
    }

    public virtual void OnLoaded() { }
}
