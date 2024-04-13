namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 88, Length = 3, Description = "领取师门奖励(已经弃用, 出师时自动结算)")]
public sealed class 领取师门奖励 : GamePacket
{
}
