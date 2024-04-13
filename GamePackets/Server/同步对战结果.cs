namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 81, Length = 13, Description = "发送PK的结果")]
public sealed class 同步对战结果 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte KillMethod;

    [FieldAttribute(Position = 3, Length = 4)]
    public int WinnerID;

    [FieldAttribute(Position = 7, Length = 4)]
    public int LoserID;

    [FieldAttribute(Position = 11, Length = 2)]
    public ushort PKPenalty;
}
