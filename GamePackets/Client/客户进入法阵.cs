namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 22, Length = 6, Description = "进入传送门触发")]
public sealed class 客户进入法阵 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 法阵编号;
}
