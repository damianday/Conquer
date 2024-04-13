namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 528, Length = 6, Description = "删除仇人")]
public sealed class 玩家删除仇人 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
