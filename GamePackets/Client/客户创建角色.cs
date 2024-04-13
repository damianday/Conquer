namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 1002, Length = 40, Description = "创建角色")]
public sealed class 客户创建角色 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 32)]
    public string Name;

    [FieldAttribute(Position = 34, Length = 1)]
    public byte Gender;

    [FieldAttribute(Position = 35, Length = 1)]
    public byte Job;

    [FieldAttribute(Position = 36, Length = 1)]
    public byte HairStyle;

    [FieldAttribute(Position = 37, Length = 1)]
    public byte HairColor;

    [FieldAttribute(Position = 38, Length = 1)]
    public byte FaceStyle;
}
