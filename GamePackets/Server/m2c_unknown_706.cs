namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 706, Length = 0, Description = "m2c_unknown_706")]
public sealed class m2c_unknown_706 : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] 字节描述;
}
