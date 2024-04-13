namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1004, Length = 849, Description = "同步角色列表")]
public sealed class 返回角色列表 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 846)]
    public byte[] Description;
}
