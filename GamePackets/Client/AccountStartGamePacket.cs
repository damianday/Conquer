namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 1286, Length = 0, Description = "申请进入游戏")]
public sealed class AccountStartGamePacket : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] LoginInformation;

	public override bool Encrypted => false;
}
