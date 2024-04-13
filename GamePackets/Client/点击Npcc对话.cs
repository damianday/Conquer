namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 113, Length = 14, Description = "点击Npcc对话")]
public sealed class 点击Npcc对话 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 任务编号;

    [FieldAttribute(Position = 10, Length = 4)]
    public int 未知参数;
}
