namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 264, Length = 2, Description = "请求武技签到")]
public sealed class 请求武技签到 : GamePacket
{
}
