namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 184, Length = 0, Description = "同步场景变量")]
public sealed class SyncSceneVariablesPacket : GamePacket
{
    [FieldAttribute(Position = 4, Length = 6)]
    public byte[] Description = new byte[6] { 20, 0, 19, 18, 228, 98 };
}
