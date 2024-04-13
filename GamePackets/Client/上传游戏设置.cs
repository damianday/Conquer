namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 137, Length = 0, Description = "上传游戏设置")]
public sealed class 上传游戏设置 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 0)]
    public byte[] Description;
}
