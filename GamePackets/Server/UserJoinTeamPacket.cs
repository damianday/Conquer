namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 516, Length = 0, Description = "玩家加入队伍")]
public sealed class UserJoinTeamPacket : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] Description;
}
