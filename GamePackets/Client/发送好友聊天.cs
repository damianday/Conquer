namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 526, Length = 0, Description = "好友聊天")]
public sealed class 发送好友聊天 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 0)]
    public byte[] Description;
    // TODO: Need to redo this packet see Player.玩家好友聊天
}
