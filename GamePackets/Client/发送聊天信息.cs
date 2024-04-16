namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 131, Length = 0, Description = "发送聊天信息(附近|广播|传音)")]
public sealed class 发送聊天信息 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort Param1;

    [FieldAttribute(Position = 4, Length = 4)]
    public uint ChannelID;

    [FieldAttribute(Position = 8, Length = 1)]
    public byte MsgType;

    [FieldAttribute(Position = 9, Length = 0)]
    public byte[] Description;
}
