namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 17, Length = 0, Description = "同步背包信息")]
public sealed class 同步背包信息 : GamePacket
{
    [FieldAttribute(Position = 6, Length = 4)]
    public int 未知标志;

    [FieldAttribute(Position = 10, Length = 0)]
    public byte[] 物品描述;
}
