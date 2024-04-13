namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 85, Length = 8, Description = "泡泡提示")]
public sealed class 同步气泡提示 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort 泡泡类型;

    [FieldAttribute(Position = 4, Length = 4)]
    public int 泡泡参数;
}
