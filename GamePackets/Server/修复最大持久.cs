namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 344, Length = 3, Description = "修复最大持久")]
public sealed class 修复最大持久 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public bool 修复失败;
}