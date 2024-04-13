namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 571, Length = 7, Description = "祝贺徒弟升级")]
public sealed class 恭喜徒弟升级 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 徒弟编号;

    [FieldAttribute(Position = 6, Length = 1)]
    public int 祝贺等级;
}
