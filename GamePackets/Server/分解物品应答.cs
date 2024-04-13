namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 259, Length = 31, Description = "分解物品应答")]
public sealed class 分解物品应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
	public byte 分解数量;

	[FieldAttribute(Position = 3, Length = 4)]
	public int 分解物品;

	[FieldAttribute(Position = 7, Length = 4)]
	public int 分解物一;

	[FieldAttribute(Position = 11, Length = 4)]
	public int 分解物二;

	[FieldAttribute(Position = 15, Length = 4)]
	public int 分解物三;

	[FieldAttribute(Position = 19, Length = 4)]
	public int 物品数一;

	[FieldAttribute(Position = 23, Length = 4)]
	public int 物品数二;

	[FieldAttribute(Position = 27, Length = 4)]
	public int 物品数三;
}
