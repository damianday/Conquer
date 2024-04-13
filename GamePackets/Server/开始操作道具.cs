namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 154, Length = 12, Description = "开始操作道具")]
public sealed class 开始操作道具 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 玩家编号;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 10, Length = 2)]
    public ushort Duration;
}
