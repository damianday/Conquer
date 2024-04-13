namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 19, Length = 0, Description = "同步技能栏位")]
public sealed class SyncSkillSlotPacket : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] Description;
}
