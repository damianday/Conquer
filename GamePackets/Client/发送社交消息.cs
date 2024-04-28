namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 529, Length = 0, Description = "发送聊天信息(公会/队伍/私人)")]
public sealed class 发送社交消息 : GamePacket
{
    // TODO: This could be wrong..
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort Param1;

    [FieldAttribute(Position = 4, Length = 4)]
    public int Param2;

    [FieldAttribute(Position = 8, Length = 0)]
    public byte[] Description;
}
