namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 538, Length = 7, Description = "玩家申请收徒")]
public sealed class 玩家申请收徒 : GamePacket
{
    [FieldAttribute(Position = 3, Length = 4)]
    public int 对象编号;
}
