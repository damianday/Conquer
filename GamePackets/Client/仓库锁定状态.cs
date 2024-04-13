namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 98, Length = 3, Description = "仓库锁定状态")]
public sealed class 仓库锁定状态 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public bool 锁定状态;
}
