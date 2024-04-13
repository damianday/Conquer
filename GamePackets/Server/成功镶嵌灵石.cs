namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 253, Length = 3, Description = "玩家镶嵌灵石")]
public sealed class 成功镶嵌灵石 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 灵石状态;
}
