namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 619, Length = 203, Description = "添加仓库公告")]
public sealed class 添加仓库公告 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
	public byte 操作类型;

	[FieldAttribute(Position = 3, Length = 4)]
	public int 对象编号;

	[FieldAttribute(Position = 7, Length = 4)]
	public int 操作时间;

	[FieldAttribute(Position = 11, Length = 192)]
	public byte[] 物品描述;
}
