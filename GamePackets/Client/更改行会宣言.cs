namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 563, Length = 0, Description = "更改行会宣言")]
public sealed class 更改行会宣言 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 0)]
    public byte[] 行会宣言;
}
