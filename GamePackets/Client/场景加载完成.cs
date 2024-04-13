namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 12, Length = 10, Description = "场景加载完成")]
public sealed class 场景加载完成 : GamePacket
{
}
