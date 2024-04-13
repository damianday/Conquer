namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 35, Length = 0, Description = "同步角色装备")]
public sealed class 同步角色装备 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 40, Length = 1)]
    public byte 装备数量;

    [FieldAttribute(Position = 41, Length = 0)]
    public byte[] Description;
}
