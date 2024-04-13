namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 10, Length = 6, Description = "更换角色")]
public sealed class 客户更换角色 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 编号;
}
