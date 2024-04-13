namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1002, Length = 0, Description = "账号成功登陆")]
public sealed class LoginSuccessPacket : GamePacket
{
    /*[FieldAttribute(Position = 19, Length = 17)]
    public string MACAddress; 

    [FieldAttribute(Position = 34, Length = 35)]
    public string LoginTicket;

    [FieldAttribute(Position = 162, Length = 2)]
    public ushort Version;*/

    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] 协议数据;

    public LoginSuccessPacket()
    {
        Encrypted = false;
    }
}
