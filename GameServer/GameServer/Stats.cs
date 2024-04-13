using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GameServer;

public sealed class Stats : IEquatable<Stats>
{
    public SortedDictionary<Stat, int> Values { get; set; } = new SortedDictionary<Stat, int>();
    public int Count => Values.Sum(pair => Math.Abs(pair.Value));

    public int this[Stat stat]
    {
        get
        {
            return !Values.TryGetValue(stat, out int result) ? 0 : result;
        }
        set
        {
            if (value == 0)
            {
                if (Values.ContainsKey(stat))
                {
                    Values.Remove(stat);
                }

                return;
            }

            Values[stat] = value;
        }
    }

    public Stats() { }
    public Stats(Stats stats)
    {
        Add(stats);
    }

    public Stats(BinaryReader reader, int version = int.MaxValue, int customVersion = int.MaxValue)
    {
        var count = reader.ReadInt32();

        for (var i = 0; i < count; i++)
            Values[(Stat)reader.ReadByte()] = reader.ReadInt32();
    }

    public void Add(Stats stats)
    {
        foreach (var pair in stats.Values)
            this[pair.Key] += pair.Value;
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Values.Count);

        foreach (var pair in Values)
        {
            writer.Write((byte)pair.Key);
            writer.Write(pair.Value);
        }
    }

    public bool ContainsKey(Stat key) => Values.ContainsKey(key);

    public void Clear()
    {
        Values.Clear();
    }

    public bool Equals(Stats other)
    {
        if (Values.Count != other.Values.Count) return false;

        foreach (var pair in Values)
            if (other[pair.Key] != pair.Value) return false;

        return true;
    }
}

