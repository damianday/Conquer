namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 117, Length = 6, Description = "点击Npc开始与之对话")]
public sealed class 开始Npcc对话 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;
}
