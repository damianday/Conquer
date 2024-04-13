namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 102, Length = 3, Description = "解锁铭文栏位")]
public sealed class 解锁铭文栏位 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 解锁参数;
}
