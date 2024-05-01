namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1289, Length = 15, Description = "开始更新文件")]
public sealed class AccountSendUpdateInfoSuccessPacket : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte DocumentID;

	[FieldAttribute(Position = 3, Length = 4)]
	public int DocumentCount;

	[FieldAttribute(Position = 7, Length = 8)]
	public ulong DocumentChecksum;

	public override bool Encrypted => false;
}
