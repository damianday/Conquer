namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 515, Length = 10, Description = "请求角色数据")]
public sealed class 请求角色数据 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int CharacterID;
}
