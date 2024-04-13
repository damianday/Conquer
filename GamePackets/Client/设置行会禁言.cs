namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 564, Length = 7, Description = "设置行会禁言")]
public sealed class 设置行会禁言 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 禁言状态;

    [FieldAttribute(Position = 3, Length = 4)]
    public int 对象编号;
}
