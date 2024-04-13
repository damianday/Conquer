namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 119, Length = 22, Description = "BUFF效果", Broadcast = true)]
public sealed class 触发状态效果 : GamePacket
{
    [FieldAttribute(Position = 6, Length = 2)]
    public ushort BuffID;

    [FieldAttribute(Position = 8, Length = 4)]
    public int CasterID;

    [FieldAttribute(Position = 12, Length = 4)]
    public int TargetID;

    [FieldAttribute(Position = 16, Length = 4)]
    public int HealthAmount;
}
