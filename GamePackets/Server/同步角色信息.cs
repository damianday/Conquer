namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 513, Length = 66, Description = "同步角色信息")]
public sealed class 同步角色信息 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int Index;

    [FieldAttribute(Position = 6, Length = 32)]
    public string Name;

    [FieldAttribute(Position = 38, Length = 1)]
    public byte Job;

    [FieldAttribute(Position = 39, Length = 1)]
    public byte Gender;

    [FieldAttribute(Position = 40, Length = 1)]
    public byte 会员等级;

    [FieldAttribute(Position = 41, Length = 25)]
    public string GuildName;
}
