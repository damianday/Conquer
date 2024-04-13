using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GamePackets;

public abstract class GamePacket
{
    public static byte EncryptionKey;

    public static Dictionary<Type, MethodInfo> PacketProcessMethodTable;
    public static SortedDictionary<ushort, Type> ServerPacketTable;
    public static SortedDictionary<ushort, Type> ClientPacketTable;
    public static Dictionary<Type, ushort> ServerPacketIDTable;
    public static Dictionary<Type, ushort> ClientPacketIDTable;
    public static Dictionary<ushort, ushort> ServerPacketLengthTable;
    public static Dictionary<ushort, ushort> ClientPacketLengthTable;
    public static Dictionary<Type, Func<BinaryReader, FieldAttribute, object>> PacketReadTable;
    public static Dictionary<Type, Action<BinaryWriter, FieldAttribute, object>> PacketWriteTable;

    public readonly Type PacketType;
    public readonly PacketInfo Info;
    private readonly ushort PacketID;
    private readonly ushort PacketLength;

    public virtual bool Encrypted { get; set; }

    public static void Configure(Type connectionType)
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in types)
        {
            if (!type.IsSubclassOf(typeof(GamePacket)))
                continue;

            var attr = type.GetCustomAttribute<PacketInfo>();
            if (attr != null)
            {
                if (attr.Source == PacketSource.Client)
                {
                    ClientPacketTable[attr.ID] = type;
                    ClientPacketIDTable[type] = attr.ID;
                    ClientPacketLengthTable[attr.ID] = attr.Length;
                }
                else
                {
                    ServerPacketTable[attr.ID] = type;
                    ServerPacketIDTable[type] = attr.ID;
                    ServerPacketLengthTable[attr.ID] = attr.Length;
                }

                PacketProcessMethodTable[type] = connectionType?.GetMethod("Process", [type]);
            }
        }

        /*
        string text = string.Empty;
        foreach (var kvp in ServerPacketTable)
        {
            text += $"{kvp.Value.Name}\t{kvp.Key}\t{ServerPacketLengthTable[kvp.Key]}\r\n";
        }
        string text2 = string.Empty;
        foreach (var kvp in ClientPacketTable)
        {
            text2 += $"{kvp.Value.Name}\t{kvp.Key}\t{ClientPacketLengthTable[kvp.Key]}\r\n";
        }
        File.WriteAllText("./ServerPackRule.txt", text);
        File.WriteAllText("./ClientPackRule.txt", text2);
        */
    }

    static GamePacket()
    {
        EncryptionKey = 129;

        PacketProcessMethodTable = new Dictionary<Type, MethodInfo>();
        ServerPacketTable = new SortedDictionary<ushort, Type>();
        ServerPacketIDTable = new Dictionary<Type, ushort>();
        ServerPacketLengthTable = new Dictionary<ushort, ushort>();
        ClientPacketTable = new SortedDictionary<ushort, Type>();
        ClientPacketIDTable = new Dictionary<Type, ushort>();
        ClientPacketLengthTable = new Dictionary<ushort, ushort>();

        PacketReadTable = new Dictionary<Type, Func<BinaryReader, FieldAttribute, object>>
        {
            [typeof(bool)] = delegate (BinaryReader reader, FieldAttribute attr)
            {
                reader.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                return Convert.ToBoolean(reader.ReadByte());
            },
            [typeof(byte)] = delegate (BinaryReader reader, FieldAttribute attr)
            {
                reader.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                return reader.ReadByte();
            },
            [typeof(sbyte)] = delegate (BinaryReader reader, FieldAttribute attr)
            {
                reader.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                return reader.ReadSByte();
            },
            [typeof(byte[])] = delegate (BinaryReader reader, FieldAttribute attr)
            {
                reader.BaseStream.Seek(attr.Position, SeekOrigin.Begin);

                //var n = (attr.Length == 0) ? (reader.ReadUInt16() - 4) : attr.Length;
                var n = (int)((attr.Length == 0) ? (reader.BaseStream.Length - 4) : attr.Length);
                return reader.ReadBytes(n);
            },
            [typeof(short)] = delegate (BinaryReader reader, FieldAttribute attr)
            {
                reader.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                return reader.ReadInt16();
            },
            [typeof(ushort)] = delegate (BinaryReader reader, FieldAttribute attr)
            {
                reader.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                return reader.ReadUInt16();
            },
            [typeof(int)] = delegate (BinaryReader reader, FieldAttribute attr)
            {
                reader.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                return reader.ReadInt32();
            },
            [typeof(uint)] = delegate (BinaryReader reader, FieldAttribute attr)
            {
                reader.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                return reader.ReadUInt32();
            },
            [typeof(string)] = delegate (BinaryReader reader, FieldAttribute attr)
            {
                reader.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                byte[] bytes = reader.ReadBytes(attr.Length);
                string str = Encoding.UTF8.GetString(bytes);
                int n = str.IndexOf('\0');
                return (n == -1) ? str : str.Substring(0, n);
            },
            [typeof(Point)] = delegate (BinaryReader reader, FieldAttribute attr)
            {
                reader.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                Point point2 = new Point(reader.ReadUInt16(), reader.ReadUInt16());
                return Compute.协议坐标转点阵坐标(attr.Reversed ? new Point(point2.Y, point2.X) : point2);
            }
        };
        PacketWriteTable = new Dictionary<Type, Action<BinaryWriter, FieldAttribute, object>>
        {
            [typeof(bool)] = delegate (BinaryWriter writer, FieldAttribute attr, object obj)
            {
                writer.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                writer.Write((bool)obj);
            },
            [typeof(byte)] = delegate (BinaryWriter writer, FieldAttribute attr, object obj)
            {
                writer.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                writer.Write((byte)obj);
            },
            [typeof(sbyte)] = delegate (BinaryWriter writer, FieldAttribute attr, object obj)
            {
                writer.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                writer.Write((sbyte)obj);
            },
            [typeof(byte[])] = delegate (BinaryWriter writer, FieldAttribute attr, object obj)
            {
                if (obj is byte[] buffer)
                {
                    writer.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                    /*if (attr.Length == 0)
                    {
                        int n = buffer.Length;
                        if (attr.Position == 2)
                            n += 4; // Include packet header (2 x ushort)
                        writer.Write((ushort)n);
                    }*/
                    writer.Write(buffer, 0, buffer.Length);
                }
            },
            [typeof(short)] = delegate (BinaryWriter writer, FieldAttribute attr, object obj)
            {
                writer.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                writer.Write((short)obj);
            },
            [typeof(ushort)] = delegate (BinaryWriter writer, FieldAttribute attr, object obj)
            {
                writer.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                writer.Write((ushort)obj);
            },
            [typeof(int)] = delegate (BinaryWriter writer, FieldAttribute attr, object obj)
            {
                writer.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                writer.Write((int)obj);
            },
            [typeof(uint)] = delegate (BinaryWriter writer, FieldAttribute attr, object obj)
            {
                writer.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                writer.Write((uint)obj);
            },
            [typeof(string)] = delegate (BinaryWriter writer, FieldAttribute attr, object obj)
            {
                if (obj is string s)
                {
                    var bytes = Encoding.UTF8.GetBytes(s);
                    var n = Math.Clamp(bytes.Length, 0, attr.Length);

                    writer.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                    writer.Write(bytes, 0, n);
                    //writer.Write(bytes, 0, (attr.Length == 0) ? bytes.Length : attr.Length);
                }
            },
            [typeof(Point)] = delegate (BinaryWriter writer, FieldAttribute attr, object obj)
            {
                Point point = Compute.点阵坐标转协议坐标((Point)obj);
                writer.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                if (!attr.Reversed)
                {
                    writer.Write((ushort)point.X);
                    writer.Write((ushort)point.Y);
                }
                else
                {
                    writer.Write((ushort)point.Y);
                    writer.Write((ushort)point.X);
                }
            },
            [typeof(DateTime)] = delegate (BinaryWriter writer, FieldAttribute attr, object obj)
            {
                writer.BaseStream.Seek(attr.Position, SeekOrigin.Begin);
                writer.Write(Compute.TimeSeconds((DateTime)obj));
            }
        };
    }

    public GamePacket()
    {
        Encrypted = true;

        PacketType = GetType();
        Info = PacketType.GetCustomAttribute<PacketInfo>();

        if (Info.Source == PacketSource.Server)
        {
            PacketID = ServerPacketIDTable[PacketType];
            PacketLength = ServerPacketLengthTable[PacketID];
        }
        else
        {
            PacketID = ClientPacketIDTable[PacketType];
            PacketLength = ClientPacketLengthTable[PacketID];
        }
    }

    public byte[] ReadPacket()
    {
        using var ms = (PacketLength == 0) ? new MemoryStream() : new MemoryStream(new byte[PacketLength]);
        using var writer = new BinaryWriter(ms);

        var fields = PacketType.GetFields();
        foreach (var fieldInfo in fields)
        {
            var attr = fieldInfo.GetCustomAttribute<FieldAttribute>();
            if (attr != null)
            {
                var fieldType = fieldInfo.FieldType;
                object value = fieldInfo.GetValue(this);
                if (PacketWriteTable.TryGetValue(fieldType, out var value2))
                {
                    value2(writer, attr, value);
                }
            }
        }

        writer.Seek(0, SeekOrigin.Begin);
        writer.Write(PacketID);
        if (PacketLength == 0)
        {
            if (Info.UseIntSize)
                writer.Write((uint)ms.Length);
            else
                writer.Write((ushort)ms.Length);
        }

        byte[] contents = ms.ToArray();
        if (Encrypted)
            EncodeDecode(ref contents);
        return contents;
    }

    private void AssignPacket(byte[] data)
    {
        using var ms = new MemoryStream(data);
        using var arg = new BinaryReader(ms);

        var fields = PacketType.GetFields();
        foreach (var fieldInfo in fields)
        {
            var attr = fieldInfo.GetCustomAttribute<FieldAttribute>();
            if (attr != null)
            {
                var fieldType = fieldInfo.FieldType;
                if (PacketReadTable.TryGetValue(fieldType, out var value))
                {
                    fieldInfo.SetValue(this, value(arg, attr));
                }
            }
        }
    }

    public static GamePacket GetClientPacket(byte[] data, out byte[] remaining)
    {
        remaining = data;
        if (data.Length < 2)
            return null;

        ushort number = BitConverter.ToUInt16(data, 0);
        if (!ClientPacketTable.TryGetValue(number, out var value))
            throw new Exception($"Failed to find packet! Packet number:{number:X4}");

        uint length;
        if (!ClientPacketLengthTable.TryGetValue(number, out var len))
            throw new Exception($"Failed to get packet Length! Packet number:{number:X4}");
        length = len;
        /*if (length == 0 && data.Length < 4)
            return null;

        length = ((length == 0) ? BitConverter.ToUInt16(data, 2) : length);
        if (data.Length < length)
            return null;*/

        GamePacket packet = (GamePacket)Activator.CreateInstance(value);

        var useIntSize = packet.Info.UseIntSize;

        if (length == 0 && data.Length < (useIntSize ? 6 : 4))
            return null;

        if (length == 0)
        {
            if (useIntSize)
            {
                var buff = data.Take(6).ToArray();
                if (packet.Encrypted)
                    EncodeDecode(ref buff);
                length = BitConverter.ToUInt32(buff, 2);
            }
            else
                length = BitConverter.ToUInt16(data, 2);
        }

        if (data.Length < length)
            return null;

        byte[] contents = data.Take((int)length).ToArray();
        if (packet.Encrypted)
            EncodeDecode(ref contents);
        packet.AssignPacket(contents);
        remaining = data.Skip((int)length).ToArray();
        return packet;
    }

    public static GamePacket GetServerPacket(byte[] data, out byte[] remaining)
    {
        remaining = data;
        if (data.Length < 2)
            return null;

        ushort number = BitConverter.ToUInt16(data, 0);
        if (!ServerPacketTable.TryGetValue(number, out var value))
            throw new Exception($"Failed to find packet! Packet number:{number:X4}");

        uint length;
        if (!ServerPacketLengthTable.TryGetValue(number, out var len))
            throw new Exception($"Failed to get packet Length! Packet number:{number:X4}");
        length = len;
        /*if (length == 0 && data.Length < 4)
            return null;

        length = ((length == 0) ? BitConverter.ToUInt16(data, 2) : length);
        if (data.Length < length)
            return null;*/

        GamePacket packet = (GamePacket)Activator.CreateInstance(value);

        var useIntSize = packet.Info.UseIntSize;

        if (length == 0 && data.Length < (useIntSize ? 6 : 4))
            return null;

        if (length == 0)
        {
            if (useIntSize)
            {
                var buff = data.Take(6).ToArray();
                if (packet.Encrypted)
                    EncodeDecode(ref buff);
                length = BitConverter.ToUInt32(buff, 2);
            }
            else
                length = BitConverter.ToUInt16(data, 2);
        }

        if (data.Length < length)
            return null;

        byte[] contents = data.Take((int)length).ToArray();
        if (packet.Encrypted)
            EncodeDecode(ref contents);
        packet.AssignPacket(contents);
        remaining = data.Skip((int)length).ToArray();
        return packet;

        remaining = data;
        if (data.Length < 2)
            return null;
    }

    public static void EncodeDecode(ref byte[] buffer)
    {
        for (var i = 4; i < buffer.Length; i++)
            buffer[i] ^= EncryptionKey;
    }

    public override string ToString()
    {
        var fields = PacketType.GetFields();
        var validFieldTypes = new string[] { 
            "String", "Int64", "UInt64", "Int32", "UInt32", "Int16", "UInt16", "SByte", "Byte", "Boolean", "Byte[]", 
            "Point" };

        var sb = new StringBuilder();

        sb.Append($"[{PacketType.Name}]");
        sb.AppendLine();
        sb.Append('{');
        sb.AppendLine();
        foreach (var field in fields)
        {
            if (validFieldTypes.Contains(field.FieldType.Name))
            {
                var val = field.GetValue(this);
                if (val is byte[] buffer)
                {
                    sb.Append($"{field.Name}: ");
                    var lineCount = 0;
                    for (var i = 0; i < buffer.Length; i++)
                    {
                        sb.Append(buffer[i]);

                        if (i == buffer.Length - 1)
                            continue;
                        sb.Append(", ");
                        lineCount++;
                        if (lineCount >= 15)
                        {
                            lineCount = 0;
                            sb.AppendLine();
                        }
                    }
                }
                else
                    sb.Append($"{field.Name}: {val}");

                sb.AppendLine();
            }
        }

        sb.Append('}');

        return sb.ToString();
    }
}
