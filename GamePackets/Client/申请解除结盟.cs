namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 571, Length = 6, Description = "申请解除结盟")]
public sealed class 申请解除结盟 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 行会编号;
}
