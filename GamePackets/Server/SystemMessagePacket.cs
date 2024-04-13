using System.Text;

namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 541, Length = 0, Description = "接收聊天消息(系统/私聊/广播/传音/公会/队伍)")]
public sealed class SystemMessagePacket : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] Description;

    public int ObjectID;
    public int SystemID;
    public int MsgType;
    public int SenderLevel;

    public string Message;
    public string UserName = string.Empty;

    public void WritePacket()
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        writer.Write(ObjectID);
        writer.Write(SystemID);
        writer.Write(MsgType);
        writer.Write(SenderLevel);
        writer.Write(Encoding.UTF8.GetBytes(Message + "\0"));
        writer.Write(Encoding.UTF8.GetBytes(UserName + "\0"));
    }
}
