namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 100, Length = 7, Description = "角色拖动技能")]
public sealed class 角色拖动技能 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 技能栏位;

    [FieldAttribute(Position = 3, Length = 2)]
    public ushort 技能编号;

    [FieldAttribute(Position = 5, Length = 1)]
    public byte 铭文编号;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte 技能等级;
}
