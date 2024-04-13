namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 176, Length = 0, Description = "发放行会福利")]
public sealed class 发放行会福利 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 1)]
    public byte 发放方式;

    [FieldAttribute(Position = 5, Length = 4)]
    public int 发放总额;

    [FieldAttribute(Position = 13, Length = 2)]
    public ushort 发放人数;

    [FieldAttribute(Position = 15, Length = 0)]
    public int 对象编号; // TODO: Missing Length?
}
