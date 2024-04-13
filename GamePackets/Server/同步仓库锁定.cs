namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 273, Length = 3, Description = "g2c_lock_bank")]
public sealed class 同步仓库锁定 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public bool Locked;
}
