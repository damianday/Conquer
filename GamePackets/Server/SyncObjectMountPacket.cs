namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 145, Length = 7, Description = "玩家骑乘上马", Broadcast = true)]
public sealed class SyncObjectMountPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 1)]
	public byte 坐骑编号; // TODO:
}
