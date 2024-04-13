namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 610, Length = 9, Description = "验证动态密码")]
public sealed class 验证动态密码 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 7)]
	public string 动态密码;
}
