using System;

namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 28, Length = 0, Description = "同步坐骑数据")]
public sealed class 同步坐骑数据 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 2)]
    public short 坐骑编号1;

    [FieldAttribute(Position = 6, Length = 2)]
    public short 坐骑编号2;

    [FieldAttribute(Position = 8, Length = 2)]
    public short 坐骑编号3;

    [FieldAttribute(Position = 10, Length = 2)]
    public short 坐骑编号4;

    [FieldAttribute(Position = 12, Length = 2)]
    public short 坐骑编号5;

    [FieldAttribute(Position = 14, Length = 0)]
    public byte[] Description;
}
