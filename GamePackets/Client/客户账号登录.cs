namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 1001, Length = 164, Description = "客户登录")]
public sealed class 客户账号登录 : GamePacket
{
    [FieldAttribute(Position = 72, Length = 38)]
    public string LoginTicket;

    [FieldAttribute(Position = 136, Length = 17)]
    public string MACAddress;

    [FieldAttribute(Position = 162, Length = 2)]
    public ushort Version;

    public override bool Encrypted { get; set; }

    public 客户账号登录()
    {
        Encrypted = false;
    }
}
