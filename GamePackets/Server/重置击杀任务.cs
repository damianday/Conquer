namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 177, Length = 4, Description = "g2c_reset_kill_npc_quest")]
public sealed class 重置击杀任务 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort QuestID;
}
