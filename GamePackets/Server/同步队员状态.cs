namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 523, Length = 7, Description = "同步队员状态")]
public sealed class 同步队员状态 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte 状态编号;
}
