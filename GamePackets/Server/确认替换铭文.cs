namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 265, Length = 3, Description = "确认替换铭文")]
public sealed class 确认替换铭文 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public bool 确定替换;
}
