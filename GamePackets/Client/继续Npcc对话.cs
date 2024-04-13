namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 119, Length = 6, Description = "提交选项继续NPC对话")]
public sealed class 继续Npcc对话 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对话编号;
}
