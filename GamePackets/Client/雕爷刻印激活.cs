namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 242, Length = 7, Description = "解锁铭文栏位")]
public sealed class 雕爷刻印激活 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 位置;

    [FieldAttribute(Position = 3, Length = 4)]
    public int 铭文ID;
}
