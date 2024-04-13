namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 641, Length = 10, Description = "解除敌对列表")]
public sealed class 解除敌对列表 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 申请类型;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 行会编号;
}
