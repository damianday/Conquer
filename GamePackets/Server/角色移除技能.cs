namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 90, Length = 4, Description = "角色移除技能")]
public sealed class 角色移除技能 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort 技能编号;
}
