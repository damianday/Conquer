namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 532, Length = 82, Description = "同步好友列表")]
public sealed class 好友上线下线 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
	public int 对象编号;

	[FieldAttribute(Position = 6, Length = 32)]
	public string 对象名字;

	[FieldAttribute(Position = 75, Length = 1)]
	public byte 对象职业;

	[FieldAttribute(Position = 76, Length = 1)]
	public byte 对象性别;

	[FieldAttribute(Position = 77, Length = 1)]
	public byte 上线下线;
}
