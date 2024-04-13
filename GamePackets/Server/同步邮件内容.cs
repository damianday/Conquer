namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 576, Length = 674, Description = "查询邮件内容")]
public sealed class 同步邮件内容 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 672)]
    public byte[] 字节数据;
}
