namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 161, Length = 0, Description = "申请创建行会")]
public sealed class 申请创建行会 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 0)]
    public byte[] 字节数据;
}
