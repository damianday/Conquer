namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 185, Length = 0, Description = "同步角色变量")]
public sealed class 同步角色变量 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
	public byte[] 字节描述;

    // TODO: Redo this packet..
    /*[FieldAttribute(Position = 4, Length = 2)]
    public ushort 变量1;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 变量2;

    [FieldAttribute(Position = 10, Length = 0)]
    public byte[] 变量3;*/
}
