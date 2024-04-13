namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 61, Length = 0, Description = "同步角色列表, 适用于角色位移时同步视野")]
public sealed class 同步角色列表 : GamePacket
{
}
