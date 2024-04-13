namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1012, Length = 3, Description = "返回服务器信息")]
public sealed class 同步服务状态 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte StatusCode;
}
