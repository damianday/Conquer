namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 605, Length = 10, Description = "属性修改通知")]
public sealed class 属性修改通知 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 属性类型;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 属性数值;
}
