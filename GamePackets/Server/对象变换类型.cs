namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 73, Length = 7, Description = "Npc变换类型", Broadcast = true)]
public sealed class 对象变换类型 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte 改变类型;
}
