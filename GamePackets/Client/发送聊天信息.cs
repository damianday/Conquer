namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 131, Length = 0, Description = "发送聊天信息(附近|广播|传音)")]
public sealed class 发送聊天信息 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 0)]
    public byte[] Description;
}
