namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 38, Length = 3, Description = "切换攻击模式")]
public sealed class UserChangeAttackMode : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 攻击模式;
}
