namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 264, Length = 16, Description = "高级铭文洗练")]
public sealed class 玩家高级洗练 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort 洗练结果;

    [FieldAttribute(Position = 4, Length = 2)]
    public ushort 铭文位一;

    [FieldAttribute(Position = 6, Length = 2)]
    public ushort 铭文位二;
}
