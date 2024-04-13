namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 642, Length = 258, Description = "行会外交公告")]
public sealed class 行会外交公告 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 256)]
    public byte[] Description;
}
