using System;

namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 142, Length = 0, Description = "同步商店版本")]
public sealed class 同步商店数据 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 4)]
    public int 版本编号;

    [FieldAttribute(Position = 8, Length = 4)]
    public int 商品数量;

    [FieldAttribute(Position = 12, Length = 0)]
    public byte[] 文件内容 = Array.Empty<byte>();
}
