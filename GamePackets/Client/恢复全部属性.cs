namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 238, Length = 4, Description = "恢复全部属性")]
public sealed class 恢复全部属性 : GamePacket
{
    [FieldAttribute(Position = 3, Length = 1)]
    public byte 索引;
}
