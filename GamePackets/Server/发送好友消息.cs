namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 537, Length = 0, Description = "好友聊天")]
public sealed class 发送好友消息 : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] Description;
    // TODO: Need to redo this packet see Player.玩家好友聊天
}