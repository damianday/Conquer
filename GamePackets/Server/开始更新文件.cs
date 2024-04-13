namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1289, Length = 15, Description = "开始更新文件")]
public sealed class 开始更新文件 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 文件编号;

	[FieldAttribute(Position = 3, Length = 4)]
	public int 文件数量;

	[FieldAttribute(Position = 7, Length = 8)]
	public ulong 校验代码;

	public override bool Encrypted => false;
}
