namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 615, Length = 0, Description = "行会仓库转入")]
public sealed class 转入行会仓库 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 1)]
	public byte 仓库页面;

	[FieldAttribute(Position = 5, Length = 1)]
	public byte 仓库位置;

	[FieldAttribute(Position = 8, Length = 0)]
	public byte[] 物品详情;
}
