namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 67, Length = 93, Description = "同步玩家数据")]
public sealed class 同步玩家数据 : GamePacket
{
}
