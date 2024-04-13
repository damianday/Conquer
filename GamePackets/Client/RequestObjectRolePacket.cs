namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 19, Length = 10, Description = "请求对象数据, 对应服务端0041封包")]
public sealed class RequestObjectRolePacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 4)]
    public int StatusID;
}
