using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using GameServer.Template;

namespace GameServer.Database;

public sealed class DBValue
{
    internal static readonly Dictionary<Type, Func<BinaryReader, DBObject, DBValue, object>> TypeRead;
    internal static readonly Dictionary<Type, Action<BinaryWriter, object>> TypeWrite;

    public string FieldName { get; }
    public Type FieldType { get; }
    public FieldInfo Field { get; }

    static DBValue()
    {
        TypeRead = new Dictionary<Type, Func<BinaryReader, DBObject, DBValue, object>>
        {
            [typeof(DataMonitor<int>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<int> monitor = new DataMonitor<int>(o);
                monitor.SetValueInternal(r.ReadInt32());
                return monitor;
            },
            [typeof(DataMonitor<uint>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<uint> monitor = new DataMonitor<uint>(o);
                monitor.SetValueInternal(r.ReadUInt32());
                return monitor;
            },
            [typeof(DataMonitor<long>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<long> monitor = new DataMonitor<long>(o);
                monitor.SetValueInternal(r.ReadInt64());
                return monitor;
            },
            [typeof(DataMonitor<bool>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<bool> monitor = new DataMonitor<bool>(o);
                monitor.SetValueInternal(r.ReadBoolean());
                return monitor;
            },
            [typeof(DataMonitor<byte>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<byte> monitor = new DataMonitor<byte>(o);
                monitor.SetValueInternal(r.ReadByte());
                return monitor;
            },
            [typeof(DataMonitor<sbyte>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<sbyte> monitor = new DataMonitor<sbyte>(o);
                monitor.SetValueInternal(r.ReadSByte());
                return monitor;
            },
            [typeof(DataMonitor<string>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<string> monitor = new DataMonitor<string>(o);
                monitor.SetValueInternal(r.ReadString());
                return monitor;
            },
            [typeof(DataMonitor<ushort>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<ushort> monitor = new DataMonitor<ushort>(o);
                monitor.SetValueInternal(r.ReadUInt16());
                return monitor;
            },
            [typeof(DataMonitor<Point>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<Point> monitor = new DataMonitor<Point>(o);
                monitor.SetValueInternal(new Point(r.ReadInt32(), r.ReadInt32()));
                return monitor;
            },
            [typeof(DataMonitor<TimeSpan>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<TimeSpan> monitor = new DataMonitor<TimeSpan>(o);
                monitor.SetValueInternal(TimeSpan.FromTicks(r.ReadInt64()));
                return monitor;
            },
            [typeof(DataMonitor<DateTime>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<DateTime> monitor = new DataMonitor<DateTime>(o);
                monitor.SetValueInternal(DateTime.FromBinary(r.ReadInt64()));
                return monitor;
            },
            [typeof(DataMonitor<RandomStats>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<RandomStats> monitor = new DataMonitor<RandomStats>(o);
                monitor.SetValueInternal(RandomStats.DataSheet.TryGetValue(r.ReadInt32(), out var value9) ? value9 : null);
                return monitor;
            },
            [typeof(DataMonitor<InscriptionSkill>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<InscriptionSkill> monitor = new DataMonitor<InscriptionSkill>(o);
                monitor.SetValueInternal(InscriptionSkill.DataSheet.TryGetValue(r.ReadUInt16(), out var value8) ? value8 : null);
                return monitor;
            },
            [typeof(DataMonitor<GameItem>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<GameItem> monitor = new DataMonitor<GameItem>(o);
                monitor.SetValueInternal(GameItem.DataSheet.TryGetValue(r.ReadInt32(), out var value7) ? value7 : null);
                return monitor;
            },
            [typeof(DataMonitor<PetMode>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<PetMode> monitor = new DataMonitor<PetMode>(o);
                monitor.SetValueInternal((PetMode)r.ReadInt32());
                return monitor;
            },
            [typeof(DataMonitor<AttackMode>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<AttackMode> monitor = new DataMonitor<AttackMode>(o);
                monitor.SetValueInternal((AttackMode)r.ReadInt32());
                return monitor;
            },
            [typeof(DataMonitor<GameDirection>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<GameDirection> monitor = new DataMonitor<GameDirection>(o);
                monitor.SetValueInternal((GameDirection)r.ReadInt32());
                return monitor;
            },
            [typeof(DataMonitor<ObjectHairStyle>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<ObjectHairStyle> monitor = new DataMonitor<ObjectHairStyle>(o);
                monitor.SetValueInternal((ObjectHairStyle)r.ReadInt32());
                return monitor;
            },
            [typeof(DataMonitor<ObjectHairColor>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<ObjectHairColor> monitor = new DataMonitor<ObjectHairColor>(o);
                monitor.SetValueInternal((ObjectHairColor)r.ReadInt32());
                return monitor;
            },
            [typeof(DataMonitor<ObjectFaceStyle>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<ObjectFaceStyle> monitor = new DataMonitor<ObjectFaceStyle>(o);
                monitor.SetValueInternal((ObjectFaceStyle)r.ReadInt32());
                return monitor;
            },
            [typeof(DataMonitor<GameObjectGender>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<GameObjectGender> monitor = new DataMonitor<GameObjectGender>(o);
                monitor.SetValueInternal((GameObjectGender)r.ReadInt32());
                return monitor;
            },
            [typeof(DataMonitor<GameObjectRace>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<GameObjectRace> monitor = new DataMonitor<GameObjectRace>(o);
                monitor.SetValueInternal((GameObjectRace)r.ReadInt32());
                return monitor;
            },
            [typeof(DataMonitor<MentorInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<MentorInfo> monitor = new DataMonitor<MentorInfo>(o);
                DataLinkTable.添加任务(o, f, monitor, typeof(MentorInfo), r.ReadInt32());
                return monitor;
            },
            [typeof(DataMonitor<GuildInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<GuildInfo> monitor = new DataMonitor<GuildInfo>(o);
                DataLinkTable.添加任务(o, f, monitor, typeof(GuildInfo), r.ReadInt32());
                return monitor;
            },
            [typeof(DataMonitor<TeamInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<TeamInfo> monitor = new DataMonitor<TeamInfo>(o);
                DataLinkTable.添加任务(o, f, monitor, typeof(TeamInfo), r.ReadInt32());
                return monitor;
            },
            [typeof(DataMonitor<BuffInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<BuffInfo> monitor = new DataMonitor<BuffInfo>(o);
                DataLinkTable.添加任务(o, f, monitor, typeof(BuffInfo), r.ReadInt32());
                return monitor;
            },
            [typeof(DataMonitor<MailInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<MailInfo> monitor = new DataMonitor<MailInfo>(o);
                DataLinkTable.添加任务(o, f, monitor, typeof(MailInfo), r.ReadInt32());
                return monitor;
            },
            [typeof(DataMonitor<AccountInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<AccountInfo> monitor = new DataMonitor<AccountInfo>(o);
                DataLinkTable.添加任务(o, f, monitor, typeof(AccountInfo), r.ReadInt32());
                return monitor;
            },
            [typeof(DataMonitor<CharacterInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<CharacterInfo> monitor = new DataMonitor<CharacterInfo>(o);
                DataLinkTable.添加任务(o, f, monitor, typeof(CharacterInfo), r.ReadInt32());
                return monitor;
            },
            [typeof(DataMonitor<EquipmentInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<EquipmentInfo> monitor = new DataMonitor<EquipmentInfo>(o);
                DataLinkTable.添加任务(o, f, monitor, typeof(EquipmentInfo), r.ReadInt32());
                return monitor;
            },
            [typeof(DataMonitor<ItemInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DataMonitor<ItemInfo> monitor = new DataMonitor<ItemInfo>(o);
                DataLinkTable.添加任务(o, f, monitor, r.ReadBoolean() ? typeof(EquipmentInfo) : typeof(ItemInfo), r.ReadInt32());
                return monitor;
            },
            [typeof(ListMonitor<int>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                ListMonitor<int> monitor = new ListMonitor<int>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    monitor.QuietlyAdd(r.ReadInt32());
                }
                return monitor;
            },
            [typeof(ListMonitor<uint>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                ListMonitor<uint> monitor = new ListMonitor<uint>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    monitor.QuietlyAdd(r.ReadUInt32());
                }
                return monitor;
            },
            [typeof(ListMonitor<bool>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                ListMonitor<bool> monitor = new ListMonitor<bool>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    monitor.QuietlyAdd(r.ReadBoolean());
                }
                return monitor;
            },
            [typeof(ListMonitor<byte>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                ListMonitor<byte> monitor = new ListMonitor<byte>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    monitor.QuietlyAdd(r.ReadByte());
                }
                return monitor;
            },
            [typeof(ListMonitor<CharacterInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                ListMonitor<CharacterInfo> monitor = new ListMonitor<CharacterInfo>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    DataLinkTable.添加任务(o, f, monitor.IList, typeof(CharacterInfo), r.ReadInt32());
                }
                return monitor;
            },
            [typeof(ListMonitor<PetInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                ListMonitor<PetInfo> monitor = new ListMonitor<PetInfo>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    DataLinkTable.添加任务(o, f, monitor.IList, typeof(PetInfo), r.ReadInt32());
                }
                return monitor;
            },
            [typeof(ListMonitor<GuildInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                ListMonitor<GuildInfo> monitor = new ListMonitor<GuildInfo>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    DataLinkTable.添加任务(o, f, monitor.IList, typeof(GuildInfo), r.ReadInt32());
                }
                return monitor;
            },
            [typeof(ListMonitor<GuildLog>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                ListMonitor<GuildLog> monitor = new ListMonitor<GuildLog>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    monitor.QuietlyAdd(new GuildLog
                    {
                        LogType = (GuildLogType)r.ReadByte(),
                        Param1 = r.ReadInt32(),
                        Param2 = r.ReadInt32(),
                        Param3 = r.ReadInt32(),
                        Param4 = r.ReadInt32(),
                        LogTime = r.ReadInt32()
                    });
                }
                return monitor;
            },
            [typeof(ListMonitor<RandomStats>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                ListMonitor<RandomStats> monitor = new ListMonitor<RandomStats>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    if (RandomStats.DataSheet.TryGetValue(r.ReadInt32(), out var value6))
                    {
                        monitor.QuietlyAdd(value6);
                    }
                }
                return monitor;
            },
            [typeof(ListMonitor<EquipSlotColor>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                ListMonitor<EquipSlotColor> monitor = new ListMonitor<EquipSlotColor>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    monitor.QuietlyAdd((EquipSlotColor)r.ReadInt32());
                }
                return monitor;
            },
            [typeof(HashMonitor<PetInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                HashMonitor<PetInfo> monitor = new HashMonitor<PetInfo>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    DataLinkTable.添加任务(o, f, monitor.ISet, r.ReadInt32());
                }
                return monitor;
            },
            [typeof(HashMonitor<CharacterInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                HashMonitor<CharacterInfo> monitor = new HashMonitor<CharacterInfo>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    DataLinkTable.添加任务(o, f, monitor.ISet, r.ReadInt32());
                }
                return monitor;
            },
            [typeof(HashMonitor<MailInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                HashMonitor<MailInfo> monitor = new HashMonitor<MailInfo>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    DataLinkTable.添加任务(o, f, monitor.ISet, r.ReadInt32());
                }
                return monitor;
            },
            [typeof(DictionaryMonitor<byte, int>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DictionaryMonitor<byte, int> monitor = new DictionaryMonitor<byte, int>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    byte key10 = r.ReadByte();
                    int value5 = r.ReadInt32();
                    monitor.AddInternal(key10, value5);
                }
                return monitor;
            },
            [typeof(DictionaryMonitor<int, int>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DictionaryMonitor<int, int> monitor = new DictionaryMonitor<int, int>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    int key9 = r.ReadInt32();
                    int value4 = r.ReadInt32();
                    monitor.AddInternal(key9, value4);
                }
                return monitor;
            },
            [typeof(DictionaryMonitor<int, DateTime>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DictionaryMonitor<int, DateTime> monitor = new DictionaryMonitor<int, DateTime>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    int key8 = r.ReadInt32();
                    long dateData6 = r.ReadInt64();
                    monitor.AddInternal(key8, DateTime.FromBinary(dateData6));
                }
                return monitor;
            },
            [typeof(DictionaryMonitor<byte, DateTime>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DictionaryMonitor<byte, DateTime> monitor = new DictionaryMonitor<byte, DateTime>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    byte key7 = r.ReadByte();
                    long dateData5 = r.ReadInt64();
                    monitor.AddInternal(key7, DateTime.FromBinary(dateData5));
                }
                return monitor;
            },
            [typeof(DictionaryMonitor<string, DateTime>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DictionaryMonitor<string, DateTime> monitor = new DictionaryMonitor<string, DateTime>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    string key6 = r.ReadString();
                    long dateData4 = r.ReadInt64();
                    monitor.AddInternal(key6, DateTime.FromBinary(dateData4));
                }
                return monitor;
            },
            [typeof(DictionaryMonitor<byte, GameItem>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DictionaryMonitor<byte, GameItem> monitor = new DictionaryMonitor<byte, GameItem>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    byte key4 = r.ReadByte();
                    int key5 = r.ReadInt32();
                    if (GameItem.DataSheet.TryGetValue(key5, out var value3))
                    {
                        monitor.AddInternal(key4, value3);
                    }
                }
                return monitor;
            },
            [typeof(DictionaryMonitor<byte, InscriptionSkill>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DictionaryMonitor<byte, InscriptionSkill> monitor = new DictionaryMonitor<byte, InscriptionSkill>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    byte key2 = r.ReadByte();
                    ushort key3 = r.ReadUInt16();
                    if (InscriptionSkill.DataSheet.TryGetValue(key3, out var value2))
                    {
                        monitor.AddInternal(key2, value2);
                    }
                }
                return monitor;
            },
            [typeof(DictionaryMonitor<ushort, BuffInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DictionaryMonitor<ushort, BuffInfo> monitor = new DictionaryMonitor<ushort, BuffInfo>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    ushort num23 = r.ReadUInt16();
                    int 值索引8 = r.ReadInt32();
                    DataLinkTable.添加任务(o, f, monitor.IDictionary, num23, null, typeof(ushort), typeof(BuffInfo), 0, 值索引8);
                }
                return monitor;
            },
            [typeof(DictionaryMonitor<ushort, SkillInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DictionaryMonitor<ushort, SkillInfo> monitor = new DictionaryMonitor<ushort, SkillInfo>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    ushort num20 = r.ReadUInt16();
                    int 值索引7 = r.ReadInt32();
                    DataLinkTable.添加任务(o, f, monitor.IDictionary, num20, null, typeof(ushort), typeof(SkillInfo), 0, 值索引7);
                }
                return monitor;
            },
            [typeof(DictionaryMonitor<byte, SkillInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DictionaryMonitor<byte, SkillInfo> monitor = new DictionaryMonitor<byte, SkillInfo>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    byte b4 = r.ReadByte();
                    int 值索引6 = r.ReadInt32();
                    DataLinkTable.添加任务(o, f, monitor.IDictionary, b4, null, typeof(byte), typeof(SkillInfo), 0, 值索引6);
                }
                return monitor;
            },
            [typeof(DictionaryMonitor<byte, EquipmentInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DictionaryMonitor<byte, EquipmentInfo> monitor = new DictionaryMonitor<byte, EquipmentInfo>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    byte b3 = r.ReadByte();
                    int 值索引5 = r.ReadInt32();
                    DataLinkTable.添加任务(o, f, monitor.IDictionary, b3, null, typeof(byte), typeof(EquipmentInfo), 0, 值索引5);
                }
                return monitor;
            },
            [typeof(DictionaryMonitor<byte, ItemInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DictionaryMonitor<byte, ItemInfo> monitor = new DictionaryMonitor<byte, ItemInfo>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    byte b2 = r.ReadByte();
                    bool flag = r.ReadBoolean();
                    int 值索引4 = r.ReadInt32();
                    DataLinkTable.添加任务(o, f, monitor.IDictionary, b2, null, typeof(byte), flag ? typeof(EquipmentInfo) : typeof(ItemInfo), 0, 值索引4);
                }
                return monitor;
            },
            [typeof(DictionaryMonitor<int, CharacterInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DictionaryMonitor<int, CharacterInfo> monitor = new DictionaryMonitor<int, CharacterInfo>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    int num11 = r.ReadInt32();
                    int 值索引3 = r.ReadInt32();
                    DataLinkTable.添加任务(o, f, monitor.IDictionary, num11, null, typeof(int), typeof(CharacterInfo), 0, 值索引3);
                }
                return monitor;
            },
            [typeof(DictionaryMonitor<int, MailInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DictionaryMonitor<int, MailInfo> monitor = new DictionaryMonitor<int, MailInfo>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    int num8 = r.ReadInt32();
                    int 值索引2 = r.ReadInt32();
                    DataLinkTable.添加任务(o, f, monitor.IDictionary, num8, null, typeof(int), typeof(MailInfo), 0, 值索引2);
                }
                return monitor;
            },
            [typeof(DictionaryMonitor<CurrencyType, int>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DictionaryMonitor<CurrencyType, int> monitor = new DictionaryMonitor<CurrencyType, int>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    int key = r.ReadInt32();
                    int value = r.ReadInt32();
                    monitor.AddInternal((CurrencyType)key, value);
                }
                return monitor;
            },
            [typeof(DictionaryMonitor<GuildInfo, DateTime>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DictionaryMonitor<GuildInfo, DateTime> monitor = new DictionaryMonitor<GuildInfo, DateTime>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    int 键索引3 = r.ReadInt32();
                    long dateData3 = r.ReadInt64();
                    DataLinkTable.添加任务(o, f, monitor.IDictionary, null, DateTime.FromBinary(dateData3), typeof(GuildInfo), typeof(DateTime), 键索引3, 0);
                }
                return monitor;
            },
            [typeof(DictionaryMonitor<CharacterInfo, DateTime>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DictionaryMonitor<CharacterInfo, DateTime> monitor = new DictionaryMonitor<CharacterInfo, DateTime>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    int 键索引2 = r.ReadInt32();
                    long dateData2 = r.ReadInt64();
                    DataLinkTable.添加任务(o, f, monitor.IDictionary, null, DateTime.FromBinary(dateData2), typeof(CharacterInfo), typeof(DateTime), 键索引2, 0);
                }
                return monitor;
            },
            [typeof(DictionaryMonitor<CharacterInfo, GuildRank>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DictionaryMonitor<CharacterInfo, GuildRank> monitor = new DictionaryMonitor<CharacterInfo, GuildRank>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    int 键索引 = r.ReadInt32();
                    int num3 = r.ReadInt32();
                    DataLinkTable.添加任务(o, f, monitor.IDictionary, null, (GuildRank)num3, typeof(CharacterInfo), typeof(GuildRank), 键索引, 0);
                }
                return monitor;
            },
            [typeof(DictionaryMonitor<DateTime, GuildInfo>)] = delegate (BinaryReader r, DBObject o, DBValue f)
            {
                DictionaryMonitor<DateTime, GuildInfo> monitor = new DictionaryMonitor<DateTime, GuildInfo>(o);
                int count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    long k = r.ReadInt64();
                    int v = r.ReadInt32();
                    DataLinkTable.添加任务(o, f, monitor.IDictionary, DateTime.FromBinary(k), null, typeof(DateTime), typeof(GuildInfo), 0, v);
                }
                return monitor;
            }
        };
        TypeWrite = new Dictionary<Type, Action<BinaryWriter, object>>
        {
            [typeof(DataMonitor<int>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<int>)o).V);
            },
            [typeof(DataMonitor<uint>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<uint>)o).V);
            },
            [typeof(DataMonitor<long>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<long>)o).V);
            },
            [typeof(DataMonitor<bool>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<bool>)o).V);
            },
            [typeof(DataMonitor<byte>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<byte>)o).V);
            },
            [typeof(DataMonitor<sbyte>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<sbyte>)o).V);
            },
            [typeof(DataMonitor<string>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<string>)o).V ?? "");
            },
            [typeof(DataMonitor<ushort>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<ushort>)o).V);
            },
            [typeof(DataMonitor<Point>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<Point>)o).V.X);
                b.Write(((DataMonitor<Point>)o).V.Y);
            },
            [typeof(DataMonitor<TimeSpan>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<TimeSpan>)o).V.Ticks);
            },
            [typeof(DataMonitor<DateTime>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<DateTime>)o).V.ToBinary());
            },
            [typeof(DataMonitor<RandomStats>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<RandomStats>)o).V?.StatID ?? 0);
            },
            [typeof(DataMonitor<InscriptionSkill>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<InscriptionSkill>)o).V?.Index ?? 0);
            },
            [typeof(DataMonitor<GameItem>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<GameItem>)o).V?.ID ?? 0);
            },
            [typeof(DataMonitor<PetMode>)] = delegate (BinaryWriter b, object o)
            {
                b.Write((int)((DataMonitor<PetMode>)o).V);
            },
            [typeof(DataMonitor<AttackMode>)] = delegate (BinaryWriter b, object o)
            {
                b.Write((int)((DataMonitor<AttackMode>)o).V);
            },
            [typeof(DataMonitor<GameDirection>)] = delegate (BinaryWriter b, object o)
            {
                b.Write((int)((DataMonitor<GameDirection>)o).V);
            },
            [typeof(DataMonitor<ObjectHairStyle>)] = delegate (BinaryWriter b, object o)
            {
                b.Write((int)((DataMonitor<ObjectHairStyle>)o).V);
            },
            [typeof(DataMonitor<ObjectHairColor>)] = delegate (BinaryWriter b, object o)
            {
                b.Write((int)((DataMonitor<ObjectHairColor>)o).V);
            },
            [typeof(DataMonitor<ObjectFaceStyle>)] = delegate (BinaryWriter b, object o)
            {
                b.Write((int)((DataMonitor<ObjectFaceStyle>)o).V);
            },
            [typeof(DataMonitor<GameObjectGender>)] = delegate (BinaryWriter b, object o)
            {
                b.Write((int)((DataMonitor<GameObjectGender>)o).V);
            },
            [typeof(DataMonitor<GameObjectRace>)] = delegate (BinaryWriter b, object o)
            {
                b.Write((int)((DataMonitor<GameObjectRace>)o).V);
            },
            [typeof(DataMonitor<MentorInfo>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<MentorInfo>)o).V?.Index.V ?? 0);
            },
            [typeof(DataMonitor<GuildInfo>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<GuildInfo>)o).V?.Index.V ?? 0);
            },
            [typeof(DataMonitor<TeamInfo>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<TeamInfo>)o).V?.Index.V ?? 0);
            },
            [typeof(DataMonitor<BuffInfo>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<BuffInfo>)o).V?.Index.V ?? 0);
            },
            [typeof(DataMonitor<MailInfo>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<MailInfo>)o).V?.Index.V ?? 0);
            },
            [typeof(DataMonitor<AccountInfo>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<AccountInfo>)o).V?.Index.V ?? 0);
            },
            [typeof(DataMonitor<CharacterInfo>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<CharacterInfo>)o).V?.Index.V ?? 0);
            },
            [typeof(DataMonitor<EquipmentInfo>)] = delegate (BinaryWriter b, object o)
            {
                b.Write(((DataMonitor<EquipmentInfo>)o).V?.Index.V ?? 0);
            },
            [typeof(DataMonitor<ItemInfo>)] = delegate (BinaryWriter b, object o)
            {
                DataMonitor<ItemInfo> 数据监视器2 = (DataMonitor<ItemInfo>)o;
                b.Write(数据监视器2.V is EquipmentInfo);
                b.Write(数据监视器2.V?.Index.V ?? 0);
            },
            [typeof(ListMonitor<int>)] = delegate (BinaryWriter b, object o)
            {
                ListMonitor<int> 列表监视器11 = (ListMonitor<int>)o;
                b.Write(列表监视器11?.Count ?? 0);
                foreach (int item in 列表监视器11)
                {
                    b.Write(item);
                }
            },
            [typeof(ListMonitor<uint>)] = delegate (BinaryWriter b, object o)
            {
                ListMonitor<uint> 列表监视器10 = (ListMonitor<uint>)o;
                b.Write(列表监视器10?.Count ?? 0);
                foreach (uint item2 in 列表监视器10)
                {
                    b.Write(item2);
                }
            },
            [typeof(ListMonitor<bool>)] = delegate (BinaryWriter b, object o)
            {
                ListMonitor<bool> 列表监视器9 = (ListMonitor<bool>)o;
                b.Write(列表监视器9?.Count ?? 0);
                foreach (bool item3 in 列表监视器9)
                {
                    b.Write(item3);
                }
            },
            [typeof(ListMonitor<byte>)] = delegate (BinaryWriter b, object o)
            {
                ListMonitor<byte> 列表监视器8 = (ListMonitor<byte>)o;
                b.Write(列表监视器8?.Count ?? 0);
                foreach (byte item4 in 列表监视器8)
                {
                    b.Write(item4);
                }
            },
            [typeof(ListMonitor<CharacterInfo>)] = delegate (BinaryWriter b, object o)
            {
                ListMonitor<CharacterInfo> 列表监视器7 = (ListMonitor<CharacterInfo>)o;
                b.Write(列表监视器7?.Count ?? 0);
                foreach (CharacterInfo item5 in 列表监视器7)
                {
                    b.Write(item5.Index.V);
                }
            },
            [typeof(ListMonitor<PetInfo>)] = delegate (BinaryWriter b, object o)
            {
                ListMonitor<PetInfo> 列表监视器6 = (ListMonitor<PetInfo>)o;
                b.Write(列表监视器6?.Count ?? 0);
                foreach (PetInfo item6 in 列表监视器6)
                {
                    b.Write(item6.Index.V);
                }
            },
            [typeof(ListMonitor<GuildInfo>)] = delegate (BinaryWriter b, object o)
            {
                ListMonitor<GuildInfo> 列表监视器5 = (ListMonitor<GuildInfo>)o;
                b.Write(列表监视器5?.Count ?? 0);
                foreach (GuildInfo item7 in 列表监视器5)
                {
                    b.Write(item7.Index.V);
                }
            },
            [typeof(ListMonitor<GuildLog>)] = delegate (BinaryWriter b, object o)
            {
                ListMonitor<GuildLog> 列表监视器4 = (ListMonitor<GuildLog>)o;
                b.Write(列表监视器4?.Count ?? 0);
                foreach (GuildLog item8 in 列表监视器4)
                {
                    b.Write((byte)item8.LogType);
                    b.Write(item8.Param1);
                    b.Write(item8.Param2);
                    b.Write(item8.Param3);
                    b.Write(item8.Param4);
                    b.Write(item8.LogTime);
                }
            },
            [typeof(ListMonitor<RandomStats>)] = delegate (BinaryWriter b, object o)
            {
                ListMonitor<RandomStats> 列表监视器3 = (ListMonitor<RandomStats>)o;
                b.Write(列表监视器3?.Count ?? 0);
                foreach (RandomStats item9 in 列表监视器3)
                {
                    b.Write(item9.StatID);
                }
            },
            [typeof(ListMonitor<EquipSlotColor>)] = delegate (BinaryWriter b, object o)
            {
                ListMonitor<EquipSlotColor> 列表监视器2 = (ListMonitor<EquipSlotColor>)o;
                b.Write(列表监视器2?.Count ?? 0);
                foreach (EquipSlotColor item10 in 列表监视器2)
                {
                    b.Write((int)item10);
                }
            },
            [typeof(HashMonitor<PetInfo>)] = delegate (BinaryWriter b, object o)
            {
                HashMonitor<PetInfo> 哈希监视器4 = (HashMonitor<PetInfo>)o;
                b.Write(哈希监视器4?.Count ?? 0);
                foreach (PetInfo item11 in 哈希监视器4)
                {
                    b.Write(item11.Index.V);
                }
            },
            [typeof(HashMonitor<CharacterInfo>)] = delegate (BinaryWriter b, object o)
            {
                HashMonitor<CharacterInfo> 哈希监视器3 = (HashMonitor<CharacterInfo>)o;
                b.Write(哈希监视器3?.Count ?? 0);
                foreach (CharacterInfo item12 in 哈希监视器3)
                {
                    b.Write(item12.Index.V);
                }
            },
            [typeof(HashMonitor<MailInfo>)] = delegate (BinaryWriter b, object o)
            {
                HashMonitor<MailInfo> 哈希监视器2 = (HashMonitor<MailInfo>)o;
                b.Write(哈希监视器2?.Count ?? 0);
                foreach (MailInfo item13 in 哈希监视器2)
                {
                    b.Write(item13.Index.V);
                }
            },
            [typeof(DictionaryMonitor<byte, int>)] = delegate (BinaryWriter b, object o)
            {
                DictionaryMonitor<byte, int> 字典监视器20 = (DictionaryMonitor<byte, int>)o;
                b.Write(字典监视器20?.Count ?? 0);
                foreach (KeyValuePair<byte, int> item14 in 字典监视器20)
                {
                    b.Write(item14.Key);
                    b.Write(item14.Value);
                }
            },
            [typeof(DictionaryMonitor<int, int>)] = delegate (BinaryWriter b, object o)
            {
                DictionaryMonitor<int, int> 字典监视器19 = (DictionaryMonitor<int, int>)o;
                b.Write(字典监视器19?.Count ?? 0);
                foreach (KeyValuePair<int, int> item15 in 字典监视器19)
                {
                    b.Write(item15.Key);
                    b.Write(item15.Value);
                }
            },
            [typeof(DictionaryMonitor<int, DateTime>)] = delegate (BinaryWriter b, object o)
            {
                DictionaryMonitor<int, DateTime> 字典监视器18 = (DictionaryMonitor<int, DateTime>)o;
                b.Write(字典监视器18?.Count ?? 0);
                foreach (KeyValuePair<int, DateTime> item16 in 字典监视器18)
                {
                    b.Write(item16.Key);
                    b.Write(item16.Value.ToBinary());
                }
            },
            [typeof(DictionaryMonitor<byte, DateTime>)] = delegate (BinaryWriter b, object o)
            {
                DictionaryMonitor<byte, DateTime> 字典监视器17 = (DictionaryMonitor<byte, DateTime>)o;
                b.Write(字典监视器17?.Count ?? 0);
                foreach (KeyValuePair<byte, DateTime> item17 in 字典监视器17)
                {
                    b.Write(item17.Key);
                    b.Write(item17.Value.ToBinary());
                }
            },
            [typeof(DictionaryMonitor<string, DateTime>)] = delegate (BinaryWriter b, object o)
            {
                DictionaryMonitor<string, DateTime> 字典监视器16 = (DictionaryMonitor<string, DateTime>)o;
                b.Write(字典监视器16?.Count ?? 0);
                foreach (KeyValuePair<string, DateTime> item18 in 字典监视器16)
                {
                    b.Write(item18.Key);
                    b.Write(item18.Value.ToBinary());
                }
            },
            [typeof(DictionaryMonitor<byte, GameItem>)] = delegate (BinaryWriter b, object o)
            {
                DictionaryMonitor<byte, GameItem> 字典监视器15 = (DictionaryMonitor<byte, GameItem>)o;
                b.Write(字典监视器15?.Count ?? 0);
                foreach (KeyValuePair<byte, GameItem> item19 in 字典监视器15)
                {
                    b.Write(item19.Key);
                    b.Write(item19.Value.ID);
                }
            },
            [typeof(DictionaryMonitor<byte, InscriptionSkill>)] = delegate (BinaryWriter b, object o)
            {
                DictionaryMonitor<byte, InscriptionSkill> 字典监视器14 = (DictionaryMonitor<byte, InscriptionSkill>)o;
                b.Write(字典监视器14?.Count ?? 0);
                foreach (KeyValuePair<byte, InscriptionSkill> item20 in 字典监视器14)
                {
                    b.Write(item20.Key);
                    b.Write(item20.Value.Index);
                }
            },
            [typeof(DictionaryMonitor<ushort, BuffInfo>)] = delegate (BinaryWriter b, object o)
            {
                DictionaryMonitor<ushort, BuffInfo> 字典监视器13 = (DictionaryMonitor<ushort, BuffInfo>)o;
                b.Write(字典监视器13?.Count ?? 0);
                foreach (KeyValuePair<ushort, BuffInfo> item21 in 字典监视器13)
                {
                    b.Write(item21.Key);
                    b.Write(item21.Value.Index.V);
                }
            },
            [typeof(DictionaryMonitor<ushort, SkillInfo>)] = delegate (BinaryWriter b, object o)
            {
                DictionaryMonitor<ushort, SkillInfo> 字典监视器12 = (DictionaryMonitor<ushort, SkillInfo>)o;
                b.Write(字典监视器12?.Count ?? 0);
                foreach (KeyValuePair<ushort, SkillInfo> item22 in 字典监视器12)
                {
                    b.Write(item22.Key);
                    b.Write(item22.Value.Index.V);
                }
            },
            [typeof(DictionaryMonitor<byte, SkillInfo>)] = delegate (BinaryWriter b, object o)
            {
                DictionaryMonitor<byte, SkillInfo> 字典监视器11 = (DictionaryMonitor<byte, SkillInfo>)o;
                b.Write(字典监视器11?.Count ?? 0);
                foreach (KeyValuePair<byte, SkillInfo> item23 in 字典监视器11)
                {
                    b.Write(item23.Key);
                    b.Write(item23.Value.Index.V);
                }
            },
            [typeof(DictionaryMonitor<byte, EquipmentInfo>)] = delegate (BinaryWriter b, object o)
            {
                DictionaryMonitor<byte, EquipmentInfo> 字典监视器10 = (DictionaryMonitor<byte, EquipmentInfo>)o;
                b.Write(字典监视器10?.Count ?? 0);
                foreach (KeyValuePair<byte, EquipmentInfo> item24 in 字典监视器10)
                {
                    b.Write(item24.Key);
                    b.Write(item24.Value.Index.V);
                }
            },
            [typeof(DictionaryMonitor<byte, ItemInfo>)] = delegate (BinaryWriter b, object o)
            {
                DictionaryMonitor<byte, ItemInfo> 字典监视器9 = (DictionaryMonitor<byte, ItemInfo>)o;
                b.Write(字典监视器9?.Count ?? 0);
                foreach (KeyValuePair<byte, ItemInfo> item25 in 字典监视器9)
                {
                    b.Write(item25.Key);
                    b.Write(item25.Value is EquipmentInfo);
                    b.Write(item25.Value.Index.V);
                }
            },
            [typeof(DictionaryMonitor<int, CharacterInfo>)] = delegate (BinaryWriter b, object o)
            {
                DictionaryMonitor<int, CharacterInfo> 字典监视器8 = (DictionaryMonitor<int, CharacterInfo>)o;
                b.Write(字典监视器8?.Count ?? 0);
                foreach (KeyValuePair<int, CharacterInfo> item26 in 字典监视器8)
                {
                    b.Write(item26.Key);
                    b.Write(item26.Value.Index.V);
                }
            },
            [typeof(DictionaryMonitor<int, MailInfo>)] = delegate (BinaryWriter b, object o)
            {
                DictionaryMonitor<int, MailInfo> 字典监视器7 = (DictionaryMonitor<int, MailInfo>)o;
                b.Write(字典监视器7?.Count ?? 0);
                foreach (KeyValuePair<int, MailInfo> item27 in 字典监视器7)
                {
                    b.Write(item27.Key);
                    b.Write(item27.Value.Index.V);
                }
            },
            [typeof(DictionaryMonitor<CurrencyType, int>)] = delegate (BinaryWriter b, object o)
            {
                DictionaryMonitor<CurrencyType, int> 字典监视器6 = (DictionaryMonitor<CurrencyType, int>)o;
                b.Write(字典监视器6?.Count ?? 0);
                foreach (KeyValuePair<CurrencyType, int> item28 in 字典监视器6)
                {
                    b.Write((int)item28.Key);
                    b.Write(item28.Value);
                }
            },
            [typeof(DictionaryMonitor<GuildInfo, DateTime>)] = delegate (BinaryWriter b, object o)
            {
                DictionaryMonitor<GuildInfo, DateTime> 字典监视器5 = (DictionaryMonitor<GuildInfo, DateTime>)o;
                b.Write(字典监视器5?.Count ?? 0);
                foreach (KeyValuePair<GuildInfo, DateTime> item29 in 字典监视器5)
                {
                    b.Write(item29.Key.Index.V);
                    b.Write(item29.Value.ToBinary());
                }
            },
            [typeof(DictionaryMonitor<CharacterInfo, DateTime>)] = delegate (BinaryWriter b, object o)
            {
                DictionaryMonitor<CharacterInfo, DateTime> 字典监视器4 = (DictionaryMonitor<CharacterInfo, DateTime>)o;
                b.Write(字典监视器4?.Count ?? 0);
                foreach (KeyValuePair<CharacterInfo, DateTime> item30 in 字典监视器4)
                {
                    b.Write(item30.Key.Index.V);
                    b.Write(item30.Value.ToBinary());
                }
            },
            [typeof(DictionaryMonitor<CharacterInfo, GuildRank>)] = delegate (BinaryWriter b, object o)
            {
                DictionaryMonitor<CharacterInfo, GuildRank> 字典监视器3 = (DictionaryMonitor<CharacterInfo, GuildRank>)o;
                b.Write(字典监视器3?.Count ?? 0);
                foreach (KeyValuePair<CharacterInfo, GuildRank> item31 in 字典监视器3)
                {
                    b.Write(item31.Key.Index.V);
                    b.Write((int)item31.Value);
                }
            },
            [typeof(DictionaryMonitor<DateTime, GuildInfo>)] = delegate (BinaryWriter b, object o)
            {
                DictionaryMonitor<DateTime, GuildInfo> 字典监视器2 = (DictionaryMonitor<DateTime, GuildInfo>)o;
                b.Write(字典监视器2?.Count ?? 0);
                foreach (KeyValuePair<DateTime, GuildInfo> item32 in 字典监视器2)
                {
                    b.Write(item32.Key.ToBinary());
                    b.Write(item32.Value.Index.V);
                }
            }
        };
    }

    public override string ToString() => FieldName;

    public DBValue(BinaryReader reader, Type type)
    {
        FieldName = reader.ReadString();
        FieldType = Type.GetType(reader.ReadString());
        Field = type?.GetField(FieldName);
    }

    public DBValue(FieldInfo fieldInfo)
    {
        Field = fieldInfo;
        FieldName = fieldInfo.Name;
        FieldType = fieldInfo.FieldType;
    }

    public bool IsMatch(DBValue other)
    {
        return (string.Compare(FieldName, other.FieldName, StringComparison.Ordinal) == 0) && (FieldType == other.FieldType);
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(FieldName);
        writer.Write(FieldType.FullName);
    }

    public object ReadValue(BinaryReader reader, object value, DBValue field)
    {
        return TypeRead[FieldType](reader, (DBObject)value, field);
    }

    public void WriteValue(BinaryWriter writer, object value)
    {
        if (TypeWrite.ContainsKey(FieldType))
        {
            TypeWrite[FieldType](writer, value);
        }
    }
}
