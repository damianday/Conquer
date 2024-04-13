namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 132, Length = 0, Description = "玩家掉落装备")]
public sealed class 玩家掉落装备 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 4)]
    public byte[] Description;
}
