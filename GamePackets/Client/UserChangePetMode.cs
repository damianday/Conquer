namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 190, Length = 3, Description = "更改宠物模式")]
public sealed class UserChangePetMode : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 宠物模式;
}
