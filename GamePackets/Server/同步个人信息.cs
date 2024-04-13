namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 530, Length = 39, Description = "m2c_self_info")]
public sealed class 同步个人信息 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 37)]
    public byte[] Description;
}