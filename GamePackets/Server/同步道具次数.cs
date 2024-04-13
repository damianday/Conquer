namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 70, Length = 11, Description = "同步道具次数")]
public sealed class 同步道具次数 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 玩家编号;

    [FieldAttribute(Position = 10, Length = 1)]
    public byte Unknown;
}
