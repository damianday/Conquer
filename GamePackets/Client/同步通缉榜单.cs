namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 212, Length = 2, Description = "同步通缉榜单")]
public sealed class 同步通缉榜单 : GamePacket
{
}
