namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 161, Length = 0, Description = "同步摊位数据")]
public sealed class 同步摊位数据 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 8, Length = 0)]
    public byte[] Description;
}
