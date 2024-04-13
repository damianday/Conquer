namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 141, Length = 4, Description = "武器幸运变化")]
public sealed class 武器幸运变化 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public sbyte 幸运变化;
}
