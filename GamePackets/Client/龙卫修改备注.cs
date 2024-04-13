namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 233, Length = 19, Description = "龙卫修改备注")]
public sealed class 龙卫修改备注 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 记录序号;

    [FieldAttribute(Position = 3, Length = 16)]
    public byte[] 文本信息;
}
