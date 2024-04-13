namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 158, Length = 3, Description = "更改收徒推送")]
public sealed class 更改收徒推送 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public bool 收徒推送;
}
