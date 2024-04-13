using System;

namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 583, Length = 48, Description = "查询行会名字")]
public sealed class 行会名字应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 行会编号;

    [FieldAttribute(Position = 6, Length = 25)]
    public string 行会名字;

    [FieldAttribute(Position = 31, Length = 4)]
    public int 会长编号;

    [FieldAttribute(Position = 35, Length = 4)]
    public DateTime 创建时间;

    [FieldAttribute(Position = 39, Length = 1)]
    public byte 行会人数;

    [FieldAttribute(Position = 40, Length = 1)]
    public byte 行会等级;

    [FieldAttribute(Position = 41, Length = 1)]
	public byte 建筑等级;

	[FieldAttribute(Position = 46, Length = 1)]
	public byte 未知参数;
}
