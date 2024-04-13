namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 31, Length = 6, Description = "角色选中目标")]
public sealed class 角色选中目标 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;
}
