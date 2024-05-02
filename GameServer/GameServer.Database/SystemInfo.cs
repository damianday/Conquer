using System;
using System.Collections.Generic;
using System.IO;

namespace GameServer.Database;

public class SystemInfo : DBObject
{
    private sealed class GuildMemberComparer : IComparer<GuildInfo>
    {
        public int Compare(GuildInfo x, GuildInfo y)
        {
            return x.Members.Count - y.Members.Count;
        }
    }

    private sealed class LevelComparer : IComparer<CharacterInfo>
    {
        public int Compare(CharacterInfo x, CharacterInfo y)
        {
            if (x.CurrentLevel == y.CurrentLevel)
                return x.CurrentExperience - y.CurrentExperience;
            return x.CurrentLevel - y.CurrentLevel;
        }
    }

    private sealed class CombatPowerComparer : IComparer<CharacterInfo>
    {
        public int Compare(CharacterInfo x, CharacterInfo y)
        {
            return x.CurrentCombatPower - y.CurrentCombatPower;
        }
    }

    private sealed class PrestigeComparer : IComparer<CharacterInfo>
    {
        public int Compare(CharacterInfo x, CharacterInfo y)
        {
            return x.师门声望 - y.师门声望;
        }
    }

    private sealed class PKPointComparer : IComparer<CharacterInfo>
    {
        public int Compare(CharacterInfo x, CharacterInfo y)
        {
            return x.PKPoint - y.PKPoint;
        }
    }

    public readonly DictionaryMonitor<string, DateTime> IPBans;
    public readonly DictionaryMonitor<string, DateTime> NICBans;

    public readonly DataMonitor<DateTime> SabakOccupyTime;
    public readonly DataMonitor<GuildInfo> OccupyGuild;

    public readonly DictionaryMonitor<DateTime, GuildInfo> GuildApplications;

    public readonly ListMonitor<CharacterInfo> IndividualBattleRanking;
    public readonly ListMonitor<CharacterInfo> IndividualRanking;
    public readonly ListMonitor<CharacterInfo> IndividualPrestigeRanking;
    public readonly ListMonitor<CharacterInfo> IndividualPKRanking;

    public readonly ListMonitor<CharacterInfo> WarriorBattleRanking;
    public readonly ListMonitor<CharacterInfo> WizardBattleRanking;
    public readonly ListMonitor<CharacterInfo> TaoistBattleRanking;
    public readonly ListMonitor<CharacterInfo> AssassinBattleRanking;
    public readonly ListMonitor<CharacterInfo> ArcherBattleRanking;
    public readonly ListMonitor<CharacterInfo> DragonLanceBattleRanking;

    public readonly ListMonitor<CharacterInfo> WarriorRanking;
    public readonly ListMonitor<CharacterInfo> WizardRanking;
    public readonly ListMonitor<CharacterInfo> TaoistRanking;
    public readonly ListMonitor<CharacterInfo> AssassinRanking;
    public readonly ListMonitor<CharacterInfo> ArcherRanking;
    public readonly ListMonitor<CharacterInfo> DragonLanceRanking;

    public readonly ListMonitor<GuildInfo> GuildRanking;

    private static readonly CombatPowerComparer 战力计算器;
    private static readonly LevelComparer 等级计算器;
    private static readonly PrestigeComparer 声望计算器;
    private static readonly PKPointComparer PK值计算器;
    private static readonly GuildMemberComparer 行会计算器;

    public static SystemInfo Info => Session.SystemInfoTable.DataSheet[1] as SystemInfo;

    public SystemInfo()
    {
    }

    public SystemInfo(int index)
    {
        Index.V = index;
        Session.SystemInfoTable.Add(this);
    }

    public void UpdatePower(CharacterInfo character)
    {
        UpdateRanks(IndividualBattleRanking, 6, character, 战力计算器);
        switch (character.Job.V)
        {
            case GameObjectRace.Warrior:
                UpdateRanks(WarriorBattleRanking, 7, character, 战力计算器);
                break;
            case GameObjectRace.Wizard:
                UpdateRanks(WizardBattleRanking, 8, character, 战力计算器);
                break;
            case GameObjectRace.Assassin:
                UpdateRanks(AssassinBattleRanking, 10, character, 战力计算器);
                break;
            case GameObjectRace.Archer:
                UpdateRanks(ArcherBattleRanking, 11, character, 战力计算器);
                break;
            case GameObjectRace.Taoist:
                UpdateRanks(TaoistBattleRanking, 9, character, 战力计算器);
                break;
            case GameObjectRace.DragonLance:
                UpdateRanks(DragonLanceBattleRanking, 37, character, 战力计算器);
                break;
        }
    }

    public void UpdateLevel(CharacterInfo character)
    {
        UpdateRanks(IndividualRanking, 0, character, 等级计算器);
        switch (character.Job.V)
        {
            case GameObjectRace.Warrior:
                UpdateRanks(WarriorRanking, 1, character, 等级计算器);
                break;
            case GameObjectRace.Wizard:
                UpdateRanks(WizardRanking, 2, character, 等级计算器);
                break;
            case GameObjectRace.Assassin:
                UpdateRanks(AssassinRanking, 4, character, 等级计算器);
                break;
            case GameObjectRace.Archer:
                UpdateRanks(ArcherRanking, 5, character, 等级计算器);
                break;
            case GameObjectRace.Taoist:
                UpdateRanks(TaoistRanking, 3, character, 等级计算器);
                break;
            case GameObjectRace.DragonLance:
                UpdateRanks(DragonLanceRanking, 36, character, 等级计算器);
                break;
        }
    }

    public void 更新声望(CharacterInfo character)
    {
        UpdateRanks(IndividualPrestigeRanking, 14, character, 声望计算器);
    }

    public void UpdatePKPoint(CharacterInfo character)
    {
        UpdateRanks(IndividualPKRanking, 15, character, PK值计算器);
    }

    public void UpdateGuildRanks(GuildInfo guild)
    {
        int num = guild.行会排名.V - 1;
        if (GuildRanking.Count < 100)
        {
            if (num >= 0)
            {
                GuildRanking.RemoveAt(num);
                int num2 = BinarySearch(GuildRanking, guild, 行会计算器, 0, GuildRanking.Count);
                GuildRanking.Insert(num2, guild);
                for (int i = Math.Min(num, num2); i <= Math.Max(num, num2); i++)
                {
                    GuildRanking[i].行会排名.V = i + 1;
                }
            }
            else
            {
                int num3 = BinarySearch(GuildRanking, guild, 行会计算器, 0, GuildRanking.Count);
                GuildRanking.Insert(num3, guild);
                for (int j = num3; j < GuildRanking.Count; j++)
                {
                    GuildRanking[j].行会排名.V = j + 1;
                }
            }
        }
        else if (num >= 0)
        {
            GuildRanking.RemoveAt(num);
            int num4 = BinarySearch(GuildRanking, guild, 行会计算器, 0, GuildRanking.Count);
            GuildRanking.Insert(num4, guild);
            for (int k = Math.Min(num, num4); k <= Math.Max(num, num4); k++)
            {
                GuildRanking[k].行会排名.V = k + 1;
            }
        }
        else if (行会计算器.Compare(guild, GuildRanking.Last) > 0)
        {
            int num5 = BinarySearch(GuildRanking, guild, 行会计算器, 0, GuildRanking.Count);
            GuildRanking.Insert(num5, guild);
            for (int l = num5; l < GuildRanking.Count; l++)
            {
                GuildRanking[l].行会排名.V = l + 1;
            }
            GuildRanking[100].行会排名.V = 0;
            GuildRanking.RemoveAt(100);
        }
    }

    public void AddIPBan(string address, DateTime time)
    {
        if (IPBans.ContainsKey(address))
        {
            IPBans[address] = time;
            SMain.更新封禁数据(address, time);
        }
        else
        {
            IPBans[address] = time;
            SMain.添加封禁数据(address, time);
        }
    }

    public void AddNICBan(string address, DateTime time)
    {
        if (NICBans.ContainsKey(address))
        {
            NICBans[address] = time;
            SMain.更新封禁数据(address, time, 网络地址: false);
        }
        else
        {
            NICBans[address] = time;
            SMain.添加封禁数据(address, time, 网络地址: false);
        }
    }

    public void RemoveIPBan(string address)
    {
        if (IPBans.Remove(address))
        {
            SMain.移除封禁数据(address);
        }
    }

    public void RemoveNICBan(string address)
    {
        if (NICBans.Remove(address))
        {
            SMain.移除封禁数据(address);
        }
    }

    public byte[] 沙城申请描述()
    {
        using var memoryStream = new MemoryStream();
        using var binaryWriter = new BinaryWriter(memoryStream);
        foreach (KeyValuePair<DateTime, GuildInfo> item in GuildApplications)
        {
            binaryWriter.Write(item.Value.ID);
            binaryWriter.Write(Compute.TimeSeconds(item.Key.AddDays(-1.0)));
        }
        return memoryStream.ToArray();
    }

    public override void OnLoaded()
    {
        foreach (KeyValuePair<string, DateTime> item in IPBans)
        {
            SMain.添加封禁数据(item.Key, item.Value);
        }
        foreach (KeyValuePair<string, DateTime> item2 in NICBans)
        {
            SMain.添加封禁数据(item2.Key, item2.Value, 网络地址: false);
        }
    }

    private static void UpdateRanks(ListMonitor<CharacterInfo> listing, byte currentType, CharacterInfo character, IComparer<CharacterInfo> comparer)
    {
        int num = (character).CurrentRanking[currentType] - 1;
        if (listing.Count < 300)
        {
            if (num >= 0)
            {
                listing.RemoveAt(num);
                int index = BinarySearch(listing, character, comparer, 0, listing.Count);
                listing.Insert(index, character);
                for (int i = Math.Min(num, index); i <= Math.Max(num, index); i++)
                {
                    listing[i].PreviousRanking[currentType] = listing[i].CurrentRanking[currentType];
                    listing[i].CurrentRanking[currentType] = i + 1;
                }
            }
            else
            {
                int index = BinarySearch(listing, character, 战力计算器, 0, listing.Count);
                listing.Insert(index, character);
                for (int j = index; j < listing.Count; j++)
                {
                    listing[j].PreviousRanking[currentType] = listing[j].CurrentRanking[currentType];
                    listing[j].CurrentRanking[currentType] = j + 1;
                }
            }
        }
        else if (num >= 0)
        {
            listing.RemoveAt(num);
            int index = BinarySearch(listing, character, comparer, 0, listing.Count);
            listing.Insert(index, character);
            for (int k = Math.Min(num, index); k <= Math.Max(num, index); k++)
            {
                listing[k].PreviousRanking[currentType] = listing[k].CurrentRanking[currentType];
                listing[k].CurrentRanking[currentType] = k + 1;
            }
        }
        else if (comparer.Compare(character, listing.Last) > 0)
        {
            int index = BinarySearch(listing, character, 战力计算器, 0, listing.Count);
            listing.Insert(index, character);
            for (int l = index; l < listing.Count; l++)
            {
                listing[l].PreviousRanking[currentType] = listing[l].CurrentRanking[currentType];
                listing[l].CurrentRanking[currentType] = l + 1;
            }
            listing[300].CurrentRanking.Remove(currentType);
            listing.RemoveAt(300);
        }
    }

    private static int BinarySearch(ListMonitor<CharacterInfo> list, CharacterInfo value, IComparer<CharacterInfo> comparer, int index, int length)
    {
        if (length < 0) return 0;
        if (list.Count == 0) return 0;
        if (index >= list.Count) return list.Count;

        int n = (index + length) / 2;
        int cmp = comparer.Compare(list[n], (CharacterInfo)value);
        if (cmp == 0)
        {
            return n;
        }
        if (cmp > 0)
        {
            if (n + 1 >= list.Count)
            {
                return list.Count;
            }
            if (comparer.Compare(list[n + 1], value) <= 0)
            {
                return n + 1;
            }
            return BinarySearch(list, value, comparer, n + 1, length);
        }
        if (n - 1 < 0)
        {
            return 0;
        }
        if (comparer.Compare(list[n - 1], value) >= 0)
        {
            return n;
        }
        return BinarySearch(list, value, comparer, index, n - 1);
    }

    private static int BinarySearch(ListMonitor<GuildInfo> list, GuildInfo value, IComparer<GuildInfo> comparer, int index, int length)
    {
        if (length < 0) return 0;
        if (list.Count == 0) return 0;
        if (index >= list.Count) return list.Count;

        int n = (index + length) / 2;
        int cmp = comparer.Compare(list[n], value);
        if (cmp == 0)
        {
            return n;
        }
        if (cmp > 0)
        {
            if (n + 1 >= list.Count)
            {
                return list.Count;
            }
            if (comparer.Compare(list[n + 1], value) <= 0)
            {
                return n + 1;
            }
            return BinarySearch(list, value, comparer, n + 1, length);
        }
        if (n - 1 < 0)
        {
            return 0;
        }
        if (comparer.Compare(list[n - 1], value) >= 0)
        {
            return n;
        }
        return BinarySearch(list, value, comparer, index, n - 1);
    }

    static SystemInfo()
    {
        战力计算器 = new CombatPowerComparer();
        等级计算器 = new LevelComparer();
        声望计算器 = new PrestigeComparer();
        PK值计算器 = new PKPointComparer();
        行会计算器 = new GuildMemberComparer();
    }
}
