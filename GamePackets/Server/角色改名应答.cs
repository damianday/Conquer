namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1011, Length = 38, Description = "更改角色姓名")]
public sealed class 角色改名应答 : GamePacket
{
}
