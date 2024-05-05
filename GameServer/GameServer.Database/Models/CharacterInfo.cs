using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using GameServer.Map;
using GameServer.Template;
using GameServer.Networking;

using GamePackets;
using GamePackets.Server;

namespace GameServer.Database;

[SearchAttribute(SearchName = "UserName")]
public sealed class CharacterInfo : DBObject
{
    public readonly DataMonitor<string> UserName;
    public readonly DataMonitor<string> IPAddress;
    public readonly DataMonitor<string> MACAddress;

    public readonly DataMonitor<DateTime> CreatedDate;
    public readonly DataMonitor<DateTime> LoginDate;
    public readonly DataMonitor<DateTime> FrozenDate;
    public readonly DataMonitor<DateTime> DeletetionDate;
    public readonly DataMonitor<DateTime> DisconnectDate;
    public readonly DataMonitor<DateTime> PrisonTime;
    public readonly DataMonitor<DateTime> BlockDate;
    public readonly DataMonitor<TimeSpan> GreyTime;
    public readonly DataMonitor<TimeSpan> PKTime;
    public readonly DataMonitor<DateTime> 武斗日期;
    public readonly DataMonitor<DateTime> 攻沙日期;
    public readonly DataMonitor<DateTime> 领奖日期;
    public readonly DataMonitor<DateTime> 分解日期;

    public readonly DataMonitor<int> 分解经验;

    public readonly DataMonitor<GameObjectRace> Job;
    public readonly DataMonitor<GameObjectGender> Gender;
    public readonly DataMonitor<ObjectHairStyle> HairStyle;
    public readonly DataMonitor<ObjectHairColor> HairColor;
    public readonly DataMonitor<ObjectFaceStyle> FaceStyle;

    public readonly DataMonitor<int> CurrentHP;
    public readonly DataMonitor<int> CurrentMP;
    public readonly DataMonitor<byte> Level;

    public readonly DataMonitor<int> Experience;
    public readonly DataMonitor<int> ExperienceRate;

    public readonly DataMonitor<int> CombatPower;

    public readonly DataMonitor<int> CurrentPKPoint;

    public readonly DataMonitor<int> CurrentMap;
    public readonly DataMonitor<int> HomeMap;

    public readonly DataMonitor<Point> CurrentPosition;
    public readonly DataMonitor<GameDirection> CurrentDirection;

    public readonly DataMonitor<AttackMode> AttackMode;
    public readonly DataMonitor<PetMode> PetMode;

    public readonly HashMonitor<PetInfo> Pets;

    public readonly DataMonitor<byte> InventorySize;

    public readonly DataMonitor<byte> WarehouseSize;

    public readonly DataMonitor<byte> 资源背包大小;

    public readonly DataMonitor<long> 消耗元宝;

    public readonly DataMonitor<long> TradeGold;

    public readonly ListMonitor<uint> PlayerSettings;

    public readonly DataMonitor<EquipmentInfo> 升级装备;

    public readonly DataMonitor<DateTime> 取回时间;

    public readonly DataMonitor<bool> 升级成功;

    public readonly DataMonitor<byte> CurrentTitle;

    public readonly DataMonitor<ushort> MountID;

    public readonly DataMonitor<ushort> 装备套装编号;

    public readonly DictionaryMonitor<byte, int> PreviousRanking;
    public readonly DictionaryMonitor<byte, int> CurrentRanking;

    public readonly DictionaryMonitor<byte, DateTime> Titles;

    public readonly DictionaryMonitor<CurrencyType, int> Currencies;

    public readonly DictionaryMonitor<byte, ItemInfo> Inventory;
    public readonly DictionaryMonitor<byte, ItemInfo> Storage;
    public readonly DictionaryMonitor<byte, ItemInfo> 角色资源背包;
    public readonly DictionaryMonitor<byte, EquipmentInfo> Equipment;

    public readonly DictionaryMonitor<byte, SkillInfo> HotKeys;
    public readonly DictionaryMonitor<ushort, BuffInfo> Buff数据;
    public readonly DictionaryMonitor<ushort, SkillInfo> Skills;

    public readonly DictionaryMonitor<int, DateTime> Cooldowns;

    public readonly HashMonitor<MailInfo> Mail;
    public readonly HashMonitor<MailInfo> UnreadMail;

    public readonly DataMonitor<byte> 预定特权;
    public readonly DataMonitor<byte> CurrentPrivilege;
    public readonly DataMonitor<byte> PreviousPrivilege;
    public readonly DataMonitor<uint> 本期记录;
    public readonly DataMonitor<uint> 上期记录;

    public readonly DataMonitor<DateTime> 本期日期;
    public readonly DataMonitor<DateTime> 上期日期;
    public readonly DataMonitor<DateTime> 补给日期;
    public readonly DataMonitor<DateTime> 战备日期;

    public readonly DictionaryMonitor<byte, int> RemainingPrivileges;

    public readonly DataMonitor<AccountInfo> Account;
    public readonly DataMonitor<TeamInfo> Team;
    public readonly DataMonitor<GuildInfo> Guild;
    public readonly DataMonitor<MentorInfo> Mentor;

    public readonly HashMonitor<CharacterInfo> FriendList;
    public readonly HashMonitor<CharacterInfo> 偶像列表;
    public readonly HashMonitor<CharacterInfo> 粉丝列表;
    public readonly HashMonitor<CharacterInfo> 仇人列表;
    public readonly HashMonitor<CharacterInfo> 仇恨列表;
    public readonly HashMonitor<CharacterInfo> BlackList;

    public readonly DataMonitor<uint> CurrentMount;
    public readonly ListMonitor<uint> Mounts;
    public readonly ListMonitor<uint> 坐骑技能槽;
    public readonly DataMonitor<bool> AwakeningExpEnabled;
    public readonly DataMonitor<int> AwakeningExp;
    public readonly DataMonitor<DateTime> 屠魔大厅;
    public readonly DataMonitor<DateTime> 屠魔兑换;
    public readonly DataMonitor<int> 分解极品设置;
    public readonly DataMonitor<int> 分解套装设置;
    public readonly DataMonitor<int> 屠魔次数;
    public readonly DataMonitor<int> 九层妖塔次数;
    public readonly DataMonitor<int> 铭文洗练次数1;
    public readonly DataMonitor<int> 铭文洗练次数2;
    public readonly DataMonitor<int> 铭文洗练次数3;
    public readonly DataMonitor<int> 屠魔令回收数量;
    public readonly DataMonitor<int> 魔虫窟次数;
    public readonly DataMonitor<int> 祖玛套装件数;
    public readonly DataMonitor<int> 祖玛套装件头盔数;
    public readonly DataMonitor<int> 祖玛套装件项链数;
    public readonly DataMonitor<int> 祖玛套装件左戒指数;
    public readonly DataMonitor<int> 祖玛套装件右戒指数;
    public readonly DataMonitor<int> 祖玛套装件左手镯数;
    public readonly DataMonitor<int> 祖玛套装件右手镯数;
    public readonly DataMonitor<int> 祖玛套装件武器数;
    public readonly DataMonitor<int> 祖玛套装件衣服数;
    public readonly DataMonitor<int> 赤月套装件数;
    public readonly DataMonitor<int> 赤月套装件头盔数;
    public readonly DataMonitor<int> 赤月套装件项链数;
    public readonly DataMonitor<int> 赤月套装件左戒指数;
    public readonly DataMonitor<int> 赤月套装件右戒指数;
    public readonly DataMonitor<int> 赤月套装件左手镯数;
    public readonly DataMonitor<int> 赤月套装件右手镯数;
    public readonly DataMonitor<int> 赤月套装件武器数;
    public readonly DataMonitor<int> 赤月套装件衣服数;
    public readonly DataMonitor<int> 魔龙套装件数;
    public readonly DataMonitor<int> 魔龙套装件头盔数;
    public readonly DataMonitor<int> 魔龙套装件项链数;
    public readonly DataMonitor<int> 魔龙套装件左戒指数;
    public readonly DataMonitor<int> 魔龙套装件右戒指数;
    public readonly DataMonitor<int> 魔龙套装件左手镯数;
    public readonly DataMonitor<int> 魔龙套装件右手镯数;
    public readonly DataMonitor<int> 魔龙套装件武器数;
    public readonly DataMonitor<int> 魔龙套装件衣服数;
    public readonly DataMonitor<int> 苍月套装件数;
    public readonly DataMonitor<int> 苍月套装件头盔数;
    public readonly DataMonitor<int> 苍月套装件项链数;
    public readonly DataMonitor<int> 苍月套装件左戒指数;
    public readonly DataMonitor<int> 苍月套装件右戒指数;
    public readonly DataMonitor<int> 苍月套装件左手镯数;
    public readonly DataMonitor<int> 苍月套装件右手镯数;
    public readonly DataMonitor<int> 苍月套装件武器数;
    public readonly DataMonitor<int> 苍月套装件衣服数;
    public readonly DataMonitor<int> 星王套装件数;
    public readonly DataMonitor<int> 星王套装件头盔数;
    public readonly DataMonitor<int> 星王套装件项链数;
    public readonly DataMonitor<int> 星王套装件左戒指数;
    public readonly DataMonitor<int> 星王套装件右戒指数;
    public readonly DataMonitor<int> 星王套装件左手镯数;
    public readonly DataMonitor<int> 星王套装件右手镯数;
    public readonly DataMonitor<int> 星王套装件武器数;
    public readonly DataMonitor<int> 星王套装件衣服数;
    public readonly DataMonitor<int> 特殊1套装件数;
    public readonly DataMonitor<int> 特殊1套装件头盔数;
    public readonly DataMonitor<int> 特殊1套装件项链数;
    public readonly DataMonitor<int> 特殊1套装件左戒指数;
    public readonly DataMonitor<int> 特殊1套装件右戒指数;
    public readonly DataMonitor<int> 特殊1套装件左手镯数;
    public readonly DataMonitor<int> 特殊1套装件右手镯数;
    public readonly DataMonitor<int> 特殊1套装件武器数;
    public readonly DataMonitor<int> 特殊1套装件衣服数;
    public readonly DataMonitor<int> 特殊2套装件数;
    public readonly DataMonitor<int> 特殊2套装件头盔数;
    public readonly DataMonitor<int> 特殊2套装件项链数;
    public readonly DataMonitor<int> 特殊2套装件左戒指数;
    public readonly DataMonitor<int> 特殊2套装件右戒指数;
    public readonly DataMonitor<int> 特殊2套装件左手镯数;
    public readonly DataMonitor<int> 特殊2套装件右手镯数;
    public readonly DataMonitor<int> 特殊2套装件武器数;
    public readonly DataMonitor<int> 特殊2套装件衣服数;
    public readonly DataMonitor<int> 特殊3套装件数;
    public readonly DataMonitor<int> 特殊3套装件头盔数;
    public readonly DataMonitor<int> 特殊3套装件项链数;
    public readonly DataMonitor<int> 特殊3套装件左戒指数;
    public readonly DataMonitor<int> 特殊3套装件右戒指数;
    public readonly DataMonitor<int> 特殊3套装件左手镯数;
    public readonly DataMonitor<int> 特殊3套装件右手镯数;
    public readonly DataMonitor<int> 特殊3套装件武器数;
    public readonly DataMonitor<int> 特殊3套装件衣服数;
    public readonly DataMonitor<int> 通用套装1件数;
    public readonly DataMonitor<int> 通用套装1件头盔数;
    public readonly DataMonitor<int> 通用套装1件项链数;
    public readonly DataMonitor<int> 通用套装1件左戒指数;
    public readonly DataMonitor<int> 通用套装1件右戒指数;
    public readonly DataMonitor<int> 通用套装1件左手镯数;
    public readonly DataMonitor<int> 通用套装1件右手镯数;
    public readonly DataMonitor<int> 通用套装1件武器数;
    public readonly DataMonitor<int> 通用套装1件衣服数;
    public readonly DataMonitor<int> 通用套装2件数;
    public readonly DataMonitor<int> 通用套装2件头盔数;
    public readonly DataMonitor<int> 通用套装2件项链数;
    public readonly DataMonitor<int> 通用套装2件左戒指数;
    public readonly DataMonitor<int> 通用套装2件右戒指数;
    public readonly DataMonitor<int> 通用套装2件左手镯数;
    public readonly DataMonitor<int> 通用套装2件右手镯数;
    public readonly DataMonitor<int> 通用套装2件武器数;
    public readonly DataMonitor<int> 通用套装2件衣服数;
    public readonly DataMonitor<int> 通用套装3件数;
    public readonly DataMonitor<int> 通用套装3件头盔数;
    public readonly DataMonitor<int> 通用套装3件项链数;
    public readonly DataMonitor<int> 通用套装3件左戒指数;
    public readonly DataMonitor<int> 通用套装3件右戒指数;
    public readonly DataMonitor<int> 通用套装3件左手镯数;
    public readonly DataMonitor<int> 通用套装3件右手镯数;
    public readonly DataMonitor<int> 通用套装3件武器数;
    public readonly DataMonitor<int> 通用套装3件衣服数;
    public readonly DataMonitor<int> 通用套装4件数;
    public readonly DataMonitor<int> 通用套装4件头盔数;
    public readonly DataMonitor<int> 通用套装4件项链数;
    public readonly DataMonitor<int> 通用套装4件左戒指数;
    public readonly DataMonitor<int> 通用套装4件右戒指数;
    public readonly DataMonitor<int> 通用套装4件左手镯数;
    public readonly DataMonitor<int> 通用套装4件右手镯数;
    public readonly DataMonitor<int> 通用套装4件武器数;
    public readonly DataMonitor<int> 通用套装4件衣服数;
    public readonly DataMonitor<int> 通用套装5件数;
    public readonly DataMonitor<int> 通用套装5件头盔数;
    public readonly DataMonitor<int> 通用套装5件项链数;
    public readonly DataMonitor<int> 通用套装5件左戒指数;
    public readonly DataMonitor<int> 通用套装5件右戒指数;
    public readonly DataMonitor<int> 通用套装5件左手镯数;
    public readonly DataMonitor<int> 通用套装5件右手镯数;
    public readonly DataMonitor<int> 通用套装5件武器数;
    public readonly DataMonitor<int> 通用套装5件衣服数;
    public readonly DataMonitor<int> 通用套装6件数;
    public readonly DataMonitor<int> 通用套装6件头盔数;
    public readonly DataMonitor<int> 通用套装6件项链数;
    public readonly DataMonitor<int> 通用套装6件左戒指数;
    public readonly DataMonitor<int> 通用套装6件右戒指数;
    public readonly DataMonitor<int> 通用套装6件左手镯数;
    public readonly DataMonitor<int> 通用套装6件右手镯数;
    public readonly DataMonitor<int> 通用套装6件武器数;
    public readonly DataMonitor<int> 通用套装6件衣服数;
    public readonly DataMonitor<int> 幸运项链保底;
    public readonly DataMonitor<int> MonsterKillCount;
    public readonly DataMonitor<int> 升级直升变量;
    public readonly DataMonitor<int> VIPLevel;
    public readonly DataMonitor<int> 物理抗性变量;
    public readonly DataMonitor<int> 魔法抗性变量;
    public readonly DataMonitor<int> 物理伤害变量;
    public readonly DataMonitor<int> 魔法伤害变量;
    public readonly DataMonitor<int> 拉镖护送次数;
    public readonly DataMonitor<int> 百层塔层数;
    public readonly DataMonitor<int> 百层塔次数;
    public readonly DataMonitor<int> 保底参数1;
    public readonly DataMonitor<int> 保底参数2;
    public readonly DataMonitor<int> 保底参数3;
    public readonly DataMonitor<int> 保底参数4;
    public readonly DataMonitor<int> 保底参数5;
    public readonly DataMonitor<int> 保底参数6;
    public readonly DataMonitor<int> 保底参数7;
    public readonly DataMonitor<int> 保底参数8;
    public readonly DataMonitor<int> 保底参数9;
    public readonly DataMonitor<int> 保底参数10;
    public readonly DataMonitor<int> 激活标识;
    public readonly DataMonitor<int> VIPPoints;
    public readonly DataMonitor<int> 当前角色物理回击;
    public readonly DataMonitor<int> 当前角色魔法回击;
    public readonly DataMonitor<int> 地图分配线路;
    public readonly DataMonitor<bool> AutoPickUpAllVisible;

    public int ID => Index.V;

    public int CurrentExperience
    {
        get { return Experience.V; }
        set { Experience.V = value; }
    }

    public byte CurrentLevel
    {
        get { return Level.V; }
        set
        {
            if (Level.V != value)
            {
                Level.V = value;
                SystemInfo.Info.UpdateLevel(this);
            }
        }
    }

    public int CurrentCombatPower
    {
        get { return CombatPower.V; }
        set
        {
            if (CombatPower.V != value)
            {
                CombatPower.V = value;
                SystemInfo.Info.UpdatePower(this);
            }
        }
    }

    public int PKPoint
    {
        get { return CurrentPKPoint.V; }
        set
        {
            if (CurrentPKPoint.V != value)
            {
                CurrentPKPoint.V = value;
                SystemInfo.Info.UpdatePKPoint(this);
            }
        }
    }

    public int MaxExperience => CurrentLevel < Settings.Default.UserUpgradeXP.Length ? Settings.Default.UserUpgradeXP[CurrentLevel - 1] : 0;

    public int Ingot
    {
        get
        {
            if (Currencies.TryGetValue(CurrencyType.Ingot, out var v))
                return v;
            return 0;
        }
        set
        {
            Currencies[CurrencyType.Ingot] = value;
            SMain.UpdateCharacter(this, "元宝数量", value);
        }
    }

    public int Silver
    {
        get
        {
            if (Currencies.TryGetValue(CurrencyType.Silver, out var v))
                return v;
            return 0;
        }
        set
        {
            Currencies[CurrencyType.Silver] = value;
            //SMain.UpdateCharacter(this, "Silver", value); // No column for silver :)
        }
    }

    public int Gold
    {
        get
        {
            if (Currencies.TryGetValue(CurrencyType.Gold, out var v))
                return v;
            return 0;
        }
        set
        {
            Currencies[CurrencyType.Gold] = value;
            SMain.UpdateCharacter(this, "金币数量", value);
        }
    }

    public int 师门声望
    {
        get
        {
            if (!Currencies.TryGetValue(CurrencyType.名师声望, out var v))
            {
                return 0;
            }
            return v;
        }
        set
        {
            Currencies[CurrencyType.名师声望] = value;
            SMain.UpdateCharacter(this, "师门声望", value);
        }
    }

    public byte 师门参数
    {
        get
        {
            if (CurrentMentor != null)
            {
                if (CurrentMentor.MasterID == ID)
                    return 2;
                return 1;
            }
            if (CurrentLevel < 30)
                return 0;
            return 2;
        }
    }

    public TeamInfo CurrentTeam
    {
        get { return Team.V; }
        set
        {
            if (Team.V != value)
                Team.V = value;
        }
    }

    public MentorInfo CurrentMentor
    {
        get { return Mentor.V; }
        set
        {
            if (Mentor.V != value)
                Mentor.V = value;
        }
    }

    public GuildInfo CurrentGuild
    {
        get { return Guild.V; }
        set
        {
            if (Guild.V != value)
                Guild.V = value;
        }
    }

    public SConnection Connection { get; set; }

    public bool Connected => Connection != null;

    public void Enqueue(GamePacket packet)
    {
        if (Connection == null) return;
        Connection.SendPacket(packet);
    }

    public void GainExperience(int exp)
    {
        if ((CurrentLevel < Settings.Default.MaxUserLevel || CurrentExperience < MaxExperience) && (CurrentExperience += exp) > MaxExperience && CurrentLevel < Settings.Default.MaxUserLevel)
        {
            while (CurrentExperience >= MaxExperience)
            {
                CurrentExperience -= MaxExperience;
                CurrentLevel++;
            }
        }
    }

    public void Disconnect()
    {
        if (地图分配线路.V == 1 && NetworkManager.ConnectionsOnline > 0)
            NetworkManager.ConnectionsOnline--;
        else if (地图分配线路.V == 2 && NetworkManager.ConnectionsOnline1 > 0)
            NetworkManager.ConnectionsOnline1--;
        else if (地图分配线路.V == 3 && NetworkManager.ConnectionsOnline2 > 0)
            NetworkManager.ConnectionsOnline2--;
        else
        {
            if (NetworkManager.ConnectionsOnline > 0)
                NetworkManager.ConnectionsOnline--;
        }

        Connection.Player = null;
        Connection = null;
        DisconnectDate.V = SEngine.CurrentTime;
        SMain.UpdateCharacter(this, "DisconnectDate", DisconnectDate);
    }

    public void Connect(SConnection conn)
    {
        int r = SEngine.Random.Next(1, 3);
        地图分配线路.V = r;

        if (地图分配线路.V == 1)
            NetworkManager.ConnectionsOnline++;
        else if (地图分配线路.V == 2)
            NetworkManager.ConnectionsOnline1++;
        else if (地图分配线路.V == 3)
            NetworkManager.ConnectionsOnline2++;
        else
            NetworkManager.ConnectionsOnline++;

        Connection = conn;
        MACAddress.V = conn.MACAddress;
        IPAddress.V = conn.IPAddress;
        SMain.UpdateCharacter(this, "DisconnectDate", null);
        SMain.AddSystemLog($"Player [{UserName}][Lvl.{Level}] entered game (Thread: {地图分配线路.V})");
    }

    public void SendMail(MailInfo mail)
    {
        mail.Recipient.V = this;
        Mail.Add(mail);
        UnreadMail.Add(mail);
        Enqueue(new SyncNewMailPacket
        {
            MessageCount = UnreadMail.Count
        });
    }

    public CharacterInfo()
    {
    }

    public CharacterInfo(AccountInfo account, string uname, GameObjectRace job, GameObjectGender gender, ObjectHairStyle hairStyle, ObjectHairColor hairColor, ObjectFaceStyle faceStyle)
    {
        if ((job == GameObjectRace.Warrior && Settings.Default.AllowRaceWarrior == 0) || 
            (job == GameObjectRace.Wizard && Settings.Default.AllowRaceWizard == 0) || 
            (job == GameObjectRace.Assassin && Settings.Default.AllowRaceAssassin == 0) || 
            (job == GameObjectRace.Archer && Settings.Default.AllowRaceArcher == 0) || 
            (job == GameObjectRace.Taoist && Settings.Default.AllowRaceTaoist == 0) || 
            (job == GameObjectRace.DragonLance && Settings.Default.AllowRaceDragonLance == 0))
        {
            return;
        }

        Level.V = Settings.Default.StartingLevel;
        InventorySize.V = 32;
        WarehouseSize.V = 16;
        资源背包大小.V = 0;
        Account.V = account;
        UserName.V = uname;
        Job.V = job;
        Gender.V = gender;
        HairStyle.V = hairStyle;
        HairColor.V = hairColor;
        FaceStyle.V = faceStyle;
        CreatedDate.V = SEngine.CurrentTime;
        CurrentHP.V = CharacterProgression.GetData(job, 1)[Stat.MaxHP];
        CurrentMP.V = CharacterProgression.GetData(job, 1)[Stat.MaxMP];
        CurrentDirection.V = Compute.RandomDirection();
        CurrentMap.V = 142;
        HomeMap.V = 142;
        CurrentPosition.V = MapManager.GetMap(142).GetRandomPosition(AreaType.Resurrection);

        if (Settings.Default.CurrentVersion == 0)
        {
            for (int i = 0; i <= 19; i++)
            {
                Currencies[(CurrencyType)i] = 0;
            }
            PlayerSettings.SetValue(new uint[128].ToList());
            if (GameItem.DataSheetByName.TryGetValue(Settings.Default.战将特权礼包, out var value))
            {
                Inventory[0] = new ItemInfo(value, this, 1, 0, 1);
            }
            if (GameItem.DataSheetByName.TryGetValue(Settings.Default.豪杰特权礼包, out var value2))
            {
                Inventory[1] = new ItemInfo(value2, this, 1, 1, 1);
            }
        }

        if (Settings.Default.CurrentVersion >= 1)
        {
            for (int j = 0; j <= 19; j++)
            {
                Currencies[(CurrencyType)Settings.Default.自定义初始货币类型] = Settings.Default.新手出售货币值;
            }
            PlayerSettings.SetValue(new uint[128].ToList());
            if (GameItem.DataSheetByName.TryGetValue(Settings.Default.战将特权礼包, out var value3))
            {
                Inventory[0] = new ItemInfo(value3, this, 1, 0, 1);
            }
            if (GameItem.DataSheetByName.TryGetValue(Settings.Default.豪杰特权礼包, out var value4))
            {
                Inventory[1] = new ItemInfo(value4, this, 1, 1, 1);
            }
            if (Settings.Default.新手上线赠送开关 == 1 && Settings.Default.新手领取选项 == 1)
            {
                if (GameItem.DataSheet.TryGetValue(Settings.Default.新手上线赠送物品1, out var value5))
                {
                    Inventory[2] = new ItemInfo(value5, this, 1, 2, 1);
                }
                if (GameItem.DataSheet.TryGetValue(Settings.Default.新手上线赠送物品2, out var value6))
                {
                    Inventory[3] = new ItemInfo(value6, this, 1, 3, 1);
                }
                if (GameItem.DataSheet.TryGetValue(Settings.Default.新手上线赠送物品3, out var value7))
                {
                    Inventory[4] = new ItemInfo(value7, this, 1, 4, 1);
                }
                if (GameItem.DataSheet.TryGetValue(Settings.Default.新手上线赠送物品4, out var value8))
                {
                    Inventory[5] = new ItemInfo(value8, this, 1, 5, 1);
                }
                if (GameItem.DataSheet.TryGetValue(Settings.Default.新手上线赠送物品5, out var value9))
                {
                    Inventory[6] = new ItemInfo(value9, this, 1, 6, 1);
                }
                if (GameItem.DataSheet.TryGetValue(Settings.Default.新手上线赠送物品6, out var value10))
                {
                    Inventory[7] = new ItemInfo(value10, this, 1, 7, 1);
                }
            }
        }

        foreach (var item in Settings.Default.StarterItems)
        {
            if (string.IsNullOrEmpty(item.ItemName)) continue;
            if (item.RequiredGender != GameObjectGender.Any && item.RequiredGender != gender) continue;
            if (item.RequiredRace != GameObjectRace.Any && item.RequiredRace != job) continue;

            if (item.BlockedGender != GameObjectGender.Any && item.BlockedGender == gender) continue;
            if (item.BlockedRace != GameObjectRace.Any && item.BlockedRace == job) continue;

            var index = FindEmptyInventoryIndex();
            if (index == byte.MaxValue) break; // No more space to store items

            if (GameItem.DataSheetByName.TryGetValue(item.ItemName, out var val))
            {
                if (val is EquipmentItem equipment)
                    Inventory[index] = new EquipmentInfo(equipment, this, 1, index);
                else
                    Inventory[index] = new ItemInfo(val, this, 1, index);
            }
        }

        ushort n = job switch
        {
            GameObjectRace.Archer => 20400,
            GameObjectRace.Assassin => 15300,
            GameObjectRace.Taoist => 30000,
            GameObjectRace.Wizard => 25300,
            GameObjectRace.Warrior => 10300,
            _ => 12000,
        };

        if (InscriptionSkill.DataSheet.TryGetValue(n, out var inscription))
        {
            var skill = new SkillInfo(inscription.SkillID);
            Skills.Add(skill.ID.V, skill);
            HotKeys[0] = skill;
            skill.Shortcut.V = 0;
        }
        Session.CharacterInfoTable.Add(this, true);
        account.Characters.Add(this);
        OnLoaded();
    }

    public override string ToString() => UserName?.V;

    private void SubscribeToEvents()
    {
        Account.Changed += delegate (AccountInfo O)
        {
            SMain.UpdateCharacter(this, "AccountName", O);
            SMain.UpdateCharacter(this, "AccountBlockDate", (O.BlockDate.V != default(DateTime)) ? O.BlockDate : null);
        };
        Account.V.BlockDate.Changed += delegate (DateTime O)
        {
            SMain.UpdateCharacter(this, "AccountBlockDate", (O != default(DateTime)) ? O : null);
        };
        UserName.Changed += delegate (string O)
        {
            SMain.UpdateCharacter(this, "Name", O);
        };
        BlockDate.Changed += delegate (DateTime O)
        {
            SMain.UpdateCharacter(this, "BlockDate", (O != default(DateTime)) ? O : null);
        };
        FrozenDate.Changed += delegate (DateTime O)
        {
            SMain.UpdateCharacter(this, "FreezeDate", (O != default(DateTime)) ? O : null);
        };
        DeletetionDate.Changed += delegate (DateTime O)
        {
            SMain.UpdateCharacter(this, "DeletetionDate", (O != default(DateTime)) ? O : null);
        };
        LoginDate.Changed += delegate (DateTime O)
        {
            SMain.UpdateCharacter(this, "LoginDate", (O != default(DateTime)) ? O : null);
        };
        DisconnectDate.Changed += delegate (DateTime O)
        {
            SMain.UpdateCharacter(this, "DisconnectDate", !Connected ? O : null);
        };
        IPAddress.Changed += delegate (string O)
        {
            SMain.UpdateCharacter(this, "IPAddress", O);
        };
        MACAddress.Changed += delegate (string O)
        {
            SMain.UpdateCharacter(this, "MACAddress", O);
        };
        Job.Changed += delegate (GameObjectRace O)
        {
            SMain.UpdateCharacter(this, "角色职业", O);
        };
        Gender.Changed += delegate (GameObjectGender O)
        {
            SMain.UpdateCharacter(this, "角色性别", O);
        };
        Guild.Changed += delegate (GuildInfo O)
        {
            SMain.UpdateCharacter(this, "所属行会", O);
        };
        消耗元宝.Changed += delegate (long O)
        {
            SMain.UpdateCharacter(this, "消耗元宝", O);
        };
        TradeGold.Changed += delegate (long O)
        {
            SMain.UpdateCharacter(this, "转出金币", O);
        };
        InventorySize.Changed += delegate (byte O)
        {
            SMain.UpdateCharacter(this, "背包大小", O);
        };
        WarehouseSize.Changed += delegate (byte O)
        {
            SMain.UpdateCharacter(this, "仓库大小", O);
        };
        CurrentPrivilege.Changed += delegate (byte O)
        {
            SMain.UpdateCharacter(this, "本期特权", O);
        };
        本期日期.Changed += delegate (DateTime O)
        {
            SMain.UpdateCharacter(this, "本期日期", O);
        };
        PreviousPrivilege.Changed += delegate (byte O)
        {
            SMain.UpdateCharacter(this, "上期特权", O);
        };
        上期日期.Changed += delegate (DateTime O)
        {
            SMain.UpdateCharacter(this, "上期日期", O);
        };
        RemainingPrivileges.Changed += delegate (List<KeyValuePair<byte, int>> O)
        {
            SMain.UpdateCharacter(this, "剩余特权", O.Sum((KeyValuePair<byte, int> X) => X.Value));
        };
        Level.Changed += delegate (byte O)
        {
            SMain.UpdateCharacter(this, "当前等级", O);
        };
        Experience.Changed += delegate (int O)
        {
            SMain.UpdateCharacter(this, "当前经验", O);
        };
        ExperienceRate.Changed += delegate (int O)
        {
            SMain.UpdateCharacter(this, "双倍经验", O);
        };
        CombatPower.Changed += delegate (int O)
        {
            SMain.UpdateCharacter(this, "当前战力", O);
        };
        CurrentMap.Changed += delegate (int O)
        {
            SMain.UpdateCharacter(this, "当前地图", GameMap.DataSheet.TryGetValue((byte)O, out var value) ? value : ((object)O));
        };
        CurrentPosition.Changed += delegate (Point O)
        {
            SMain.UpdateCharacter(this, "当前坐标", $"{O.X}, {O.Y}");
        };
        CurrentPKPoint.Changed += delegate (int O)
        {
            SMain.UpdateCharacter(this, "当前PK值", O);
        };
        Skills.Changed += delegate (List<KeyValuePair<ushort, SkillInfo>> O)
        {
            SMain.UpdateCharacterSkills(this, O);
        };
        Equipment.Changed += delegate (List<KeyValuePair<byte, EquipmentInfo>> O)
        {
            SMain.UpdateCharacterEquipment(this, O);
        };
        Inventory.Changed += delegate (List<KeyValuePair<byte, ItemInfo>> O)
        {
            SMain.UpdateCharacterInventory(this, O);
        };
        Storage.Changed += delegate (List<KeyValuePair<byte, ItemInfo>> O)
        {
            SMain.UpdateCharacterStorage(this, O);
        };
    }

    public override void OnLoaded()
    {
        SubscribeToEvents();
        SMain.AddCharacterInfo(this);
        SMain.UpdateCharacterSkills(this, Skills.ToList());
        SMain.UpdateCharacterEquipment(this, Equipment.ToList());
        SMain.UpdateCharacterInventory(this, Inventory.ToList());
        SMain.UpdateCharacterStorage(this, Storage.ToList());
    }

    public override void Remove()
    {
        Account.V.Characters.Remove(this);
        Account.V.FrozenCharacters.Remove(this);
        Account.V.DeletedCharacters.Remove(this);
        升级装备.V?.Remove();
        foreach (PetInfo item in Pets)
        {
            item.Remove();
        }
        foreach (MailInfo item2 in Mail)
        {
            item2.Remove();
        }
        foreach (KeyValuePair<byte, ItemInfo> item3 in Inventory)
        {
            item3.Value.Remove();
        }
        foreach (KeyValuePair<byte, EquipmentInfo> item4 in Equipment)
        {
            item4.Value.Remove();
        }
        foreach (KeyValuePair<byte, ItemInfo> item5 in Storage)
        {
            item5.Value.Remove();
        }
        foreach (KeyValuePair<ushort, SkillInfo> item6 in Skills)
        {
            item6.Value.Remove();
        }
        foreach (KeyValuePair<ushort, BuffInfo> item7 in Buff数据)
        {
            item7.Value.Remove();
        }
        if (Team.V != null)
        {
            if (this == Team.V.Captain)
            {
                Team.V.Remove();
            }
            else
            {
                Team.V.Members.Remove(this);
            }
        }
        if (Mentor.V != null)
        {
            if (this == Mentor.V.Master)
                Mentor.V.Remove();
            else
                Mentor.V.移除徒弟(this);
        }
        if (Guild.V != null)
        {
            Guild.V.Members.Remove(this);
            Guild.V.BannedMembers.Remove(this);
        }
        foreach (CharacterInfo character in FriendList)
        {
            character.FriendList.Remove(this);
        }
        foreach (CharacterInfo character in 粉丝列表)
        {
            character.偶像列表.Remove(this);
        }
        foreach (CharacterInfo character in 仇恨列表)
        {
            character.仇人列表.Remove(this);
        }
        base.Remove();
    }

    public bool FindItem(int id, out ItemInfo item)
    {
        for (byte i = 0; i < InventorySize.V; i++)
        {
            if (Inventory.TryGetValue(i, out item) && item.ID == id)
                return true;
        }

        item = null;
        return false;
    }

    public byte FindEmptyInventoryIndex()
    {
        for (byte i = 0; i < InventorySize.V; i++)
        {
            if (!Inventory.ContainsKey(i))
                return i;
        }

        return byte.MaxValue;
    }

    #region Descriptions
    public byte[] RoleDescription()
    {
        using var ms = new MemoryStream(new byte[94]);
        using var writer = new BinaryWriter(ms);

        writer.Write(Index.V);
        writer.Write(NameDescription());
        writer.Seek(61, SeekOrigin.Begin);
        writer.Write((byte)Job.V);
        writer.Write((byte)Gender.V);
        writer.Write((byte)HairStyle.V);
        writer.Write((byte)HairColor.V);
        writer.Write((byte)FaceStyle.V);
        writer.Write((byte)0);
        writer.Write(CurrentLevel);
        writer.Write(CurrentMap.V);
        writer.Write(Equipment[0]?.UpgradeCount.V ?? 0);
        writer.Write((Equipment[0]?.Item.V?.ID).GetValueOrDefault());
        writer.Write((Equipment[1]?.Item.V?.ID).GetValueOrDefault());
        writer.Write((Equipment[2]?.Item.V?.ID).GetValueOrDefault());
        writer.Write(Compute.TimeSeconds(DisconnectDate.V));
        writer.Write((!FrozenDate.V.Equals(default(DateTime))) ? Compute.TimeSeconds(FrozenDate.V) : 0);

        return ms.ToArray();
    }

    public byte[] NameDescription()
    {
        return Encoding.UTF8.GetBytes(UserName.V);
    }

    public byte[] 角色设置()
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        foreach (uint item in PlayerSettings)
        {
            writer.Write(item);
        }
        return ms.ToArray();
    }

    public byte[] 坐骑列表描述()
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        foreach (uint item in Mounts)
        {
            byte value = (byte)item;
            writer.Write((ushort)value);
        }
        return ms.ToArray();
    }

    public byte[] MailBoxDescription()
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);

        writer.Write((ushort)Mail.Count);
        foreach (var mail in Mail)
        {
            writer.Write(mail.MailMessageDescription());
        }
        return ms.ToArray();
    } 
    #endregion
}
