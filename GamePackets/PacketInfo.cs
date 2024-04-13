using System;

namespace GamePackets;

[AttributeUsage(AttributeTargets.Class)]
public class PacketInfo : Attribute
{
    public PacketSource Source;
    public ushort ID;
    public ushort Length;
    public string Description;

    public bool NoDebug;
    public bool Broadcast;
    public bool UseIntSize;
}
