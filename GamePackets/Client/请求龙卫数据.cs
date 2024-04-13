namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 224, Length = 6, Description = "请求龙卫数据")]
public sealed class 请求龙卫数据 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
