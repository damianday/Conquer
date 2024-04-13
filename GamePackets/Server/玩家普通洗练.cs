namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 255, Length = 6, Description = "普通铭文洗练")]
public sealed class 玩家普通洗练 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort 铭文位一;

    [FieldAttribute(Position = 4, Length = 2)]
    public ushort 铭文位二;
}
