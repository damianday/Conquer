namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 665, Length = 226, Description = "树立城主雕像")]
public sealed class 树立城主雕像 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 224)]
    public byte[] 字节描述;
}
