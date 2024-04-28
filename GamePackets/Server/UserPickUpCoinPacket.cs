namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 136, Length = 10, Description = "拾取金币")]
public sealed class UserPickUpCoinPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort Param1;

    [FieldAttribute(Position = 4, Length = 2)]
    public ushort Param2;

    [FieldAttribute(Position = 6, Length = 4)]
    public int Amount;
}
