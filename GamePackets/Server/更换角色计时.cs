namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 279, Length = 4, Description = "更换角色计时")]
public sealed class 更换角色计时 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public bool 成功;
}
