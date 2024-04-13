using System;

namespace GamePackets;

[AttributeUsage(AttributeTargets.Field)]
public class FieldAttribute : Attribute
{
    public ushort Position;
    public ushort Length;
    public bool Reversed;
}
