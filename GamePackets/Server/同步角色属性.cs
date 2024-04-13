namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 14, Length = 0, Description = "同步角色属性")]
public sealed class 同步角色属性 : GamePacket
{
    [FieldAttribute(Position = 6, Length = 0)]
    public byte[] Description;
}
