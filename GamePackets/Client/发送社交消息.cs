namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 529, Length = 0, Description = "发送聊天信息(公会/队伍/私人)")]
public sealed class 发送社交消息 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 0)]
    public byte[] Description;
}
