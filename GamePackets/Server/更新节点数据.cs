namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 297, Length = 6, Description = "更新节点数据")]
public sealed class 更新节点数据 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
	public ushort 节点数值;

	[FieldAttribute(Position = 4, Length = 2)]
	public ushort 节点标志;
}
