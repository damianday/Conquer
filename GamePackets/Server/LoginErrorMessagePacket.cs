namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1001, Length = 14, Description = "登陆错误提示")]
public sealed class LoginErrorMessagePacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public uint ErrorCode;

    [FieldAttribute(Position = 6, Length = 4)]
    public int Param1;

    [FieldAttribute(Position = 10, Length = 4)]
    public int Param2;
}
