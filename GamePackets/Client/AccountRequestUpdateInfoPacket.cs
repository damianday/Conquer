namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 1287, Length = 0, Description = "请求更新文件")]
public sealed class AccountRequestUpdateInfoPacket : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] UpdateInformation;

	public override bool Encrypted => false;
}
