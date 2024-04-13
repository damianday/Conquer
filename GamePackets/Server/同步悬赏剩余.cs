namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 169, Length = 14/*18*/, Description = "g2c_sync_reward_quest_remain_count")]
public sealed class 同步悬赏剩余 : GamePacket
{
	// TODO: Chinese has commented version
	/*[FieldAttribute(Position = 2, Length = 4)]
	public int 悬赏类型;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 已经完成;

	[FieldAttribute(Position = 10, Length = 4)]
	public int 还能完成;

	[FieldAttribute(Position = 14, Length = 4)]
	public int 日程进度;*/

    [FieldAttribute(Position = 2, Length = 4)]
    public int 刷新类别;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 剩余可接取数量;

    [FieldAttribute(Position = 10, Length = 4)]
    public int 剩余可完成数量;
}
