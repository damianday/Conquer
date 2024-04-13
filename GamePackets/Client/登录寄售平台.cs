namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 614, Length = 3, Description = "登录寄售平台")]
public sealed class 登录寄售平台 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 操作标识;
}
