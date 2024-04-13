namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 322, Length = 0, Description = "同步特权信息")]
public sealed class 同步特权信息 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] 字节数组;

    public 同步特权信息()
    {
        字节数组 = new byte[1] { 2 };
    }
}
