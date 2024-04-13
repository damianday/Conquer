namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 562, Length = 0, Description = "更改行会公告")]
public sealed class 更改行会公告 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 0)]
    public byte[] 行会公告;
}
