namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 666, Length = 34, Description = "攻城获胜公告")]
public sealed class 攻城获胜公告 : GamePacket
{
}
