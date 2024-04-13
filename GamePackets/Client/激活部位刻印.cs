namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 243, Length = 7, Description = "激活部位刻印")]
public sealed class 激活部位刻印 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 激活部位;

	[FieldAttribute(Position = 3, Length = 4)]
	public int Unkonw;
}
