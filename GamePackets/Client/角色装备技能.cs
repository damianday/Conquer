namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 39, Length = 5, Description = "装备技能")]
public sealed class 角色装备技能 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 技能栏位;

    [FieldAttribute(Position = 3, Length = 2)]
    public ushort 技能编号;
}
