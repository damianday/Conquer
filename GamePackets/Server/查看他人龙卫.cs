namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 328, Length = 0, Description = "查看他人龙卫")]
public sealed class 查看他人龙卫 : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] 描述信息;
}
