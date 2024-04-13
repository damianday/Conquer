namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 693, Length = 6, Description = "m2c_ex_group_setting")]
public sealed class 附加组队设置 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 整数变量;
}
