namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 111, Length = 6, Description = "打开角色摊位")]
public sealed class 打开角色摊位 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
