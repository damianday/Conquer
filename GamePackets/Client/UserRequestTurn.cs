namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 16, Length = 8, Description = "角色转动")]
public sealed class UserRequestTurn : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public short Direction;

    [FieldAttribute(Position = 4, Length = 4)]
    public uint ActionTime;
}
