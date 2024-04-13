namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 582, Length = 6, Description = "申请解除敌对")]
public sealed class 申请解除敌对 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 行会编号;
}
