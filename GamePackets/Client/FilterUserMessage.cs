namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 651, Length = 0, Description = "敏感字符过滤")]
public sealed class FilterUserMessage : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort Param1;

    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] Description;
}
