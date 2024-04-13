namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1005, Length = 96, Description = "角色创建成功")]
public sealed class 角色创建成功 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 94)]
    public byte[] Description;
}
