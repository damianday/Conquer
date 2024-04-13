namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 211, Length = 50, Description = "发布通缉榜单")]
public sealed class 发布通缉榜单 : GamePacket
{
}
