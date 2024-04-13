namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 514, Length = 14, Description = "聊天服务器错误提示")]
public sealed class SocialErrorPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ErrorCode;

    [FieldAttribute(Position = 6, Length = 4)]
    public int Param1;

    [FieldAttribute(Position = 10, Length = 4)]
    public int Param2;
}
