namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 40, Length = 2, Description = "离开场景(包括商店/随机卷)")]
public sealed class ObjectLeaveScenePacket : GamePacket
{
}
