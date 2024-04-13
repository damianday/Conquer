namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 186, Length = 514, Description = "同步客户变量(物品快捷键)")]
public sealed class 同步客户变量 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 512)]
    public byte[] Description;
}
