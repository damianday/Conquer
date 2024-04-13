namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 532, Length = 2, Description = "查询拜师名册(已弃用, 不推送)")]
public sealed class 查询拜师名册 : GamePacket
{
}
