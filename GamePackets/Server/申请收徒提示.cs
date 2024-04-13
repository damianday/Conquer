namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 558, Length = 12, Description = "申请收徒提示")]
public sealed class 申请收徒提示 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 面板类型;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 对象等级;

    [FieldAttribute(Position = 4, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 8, Length = 4)]
    public int 对象声望; // change to uint
}
