namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 179, Length = 0, Description = "接收聊天信息(附近)", Broadcast = true)]
public sealed class 接收聊天信息 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] Description;
}
