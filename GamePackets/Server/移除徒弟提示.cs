namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 569, Length = 6, Description = "移除徒弟提示")]
public sealed class 移除徒弟提示 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
