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

    private static readonly CombatPowerComparer m_CombatPowerComparer;
    private static readonly LevelComparer m_LevelComparer;
    private static readonly PrestigeComparer m_PrestigeComparer;
    private static readonly PKPointComparer m_PKPointComparer;
    private static readonly GuildMemberComparer m_GuildMemberComparer;

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
        UpdateRanks(IndividualBattleRanking, 6, character, m_CombatPowerComparer);
        switch (character.Job.V)
        {
            case GameObjectRace.Warrior:
                UpdateRanks(WarriorBattleRanking, 7, character, m_CombatPowerComparer);
                break;
            case GameObjectRace.Wizard:
                UpdateRanks(WizardBattleRanking, 8, character, m_CombatPowerComparer);
                break;
            case GameObjectRace.Assassin:
                UpdateRanks(AssassinBattleRanking, 10, character, m_CombatPowerComparer);
                break;
            case GameObjectRace.Archer:
                UpdateRanks(ArcherBattleRanking, 11, character, m_CombatPowerComparer);
                break;
            case GameObjectRace.Taoist:
                UpdateRanks(TaoistBattleRanking, 9, character, m_CombatPowerComparer);
                break;
            case GameObjectRace.DragonLance:
                UpdateRanks(DragonLanceBattleRanking, 37, character, m_CombatPowerComparer);
                break;
        }
    }

    public void UpdateLevel(CharacterInfo character)
    {
        UpdateRanks(IndividualRanking, 0, character, m_LevelComparer);
        switch (character.Job.V)
        {
            case GameObjectRace.Warrior:
                UpdateRanks(WarriorRanking, 1, character, m_LevelComparer);
                break;
            case GameObjectRace.Wizard:
                UpdateRanks(WizardRanking, 2, character, m_LevelComparer);
                break;
            case GameObjectRace.Assassin:
                UpdateRanks(AssassinRanking, 4, character, m_LevelComparer);
                break;
            case GameObjectRace.Archer:
                UpdateRanks(ArcherRanking, 5, character, m_LevelComparer);
                break;
            case GameObjectRace.Taoist:
                UpdateRanks(TaoistRanking, 3, character, m_LevelComparer);
                break;
            case GameObjectRace.DragonLance:
                UpdateRanks(DragonLanceRanking, 36, character, m_LevelComparer);
                break;
        }
    }

    public void 更新声望(CharacterInfo character)
    {
        UpdateRanks(IndividualPrestigeRanking, 14, character, m_PrestigeComparer);
    }

    public void UpdatePKPoint(CharacterInfo character)
    {
        UpdateRanks(IndividualPKRanking, 15, character, m_PKPointComparer);
    }

    public void UpdateGuildRanks(GuildInfo guild)
    {
        int num = guild.GuildRanking.V - 1;
        if (GuildRanking.Count < 100)
        {
            if (num >= 0)
            {
                GuildRanking.RemoveAt(num);
                int num2 = BinarySearch(GuildRanking, guild, m_GuildMemberComparer, 0, GuildRanking.Count);
                GuildRanking.Insert(num2, guild);
                for (int i = Math.Min(num, num2); i <= Math.Max(num, num2); i++)
                {
                    GuildRanking[i].GuildRanking.V = i + 1;
                }
            }
            else
            {
                int num3 = BinarySearch(GuildRanking, guild, m_GuildMemberComparer, 0, GuildRanking.Count);
                GuildRanking.Insert(num3, guild);
                for (int j = num3; j < GuildRanking.Count; j++)
                {
                    GuildRanking[j].GuildRanking.V = j + 1;
                }
            }
        }
        else if (num >= 0)
        {
            GuildRanking.RemoveAt(num);
            int num4 = BinarySearch(GuildRanking, guild, m_GuildMemberComparer, 0, GuildRanking.Count);
            GuildRanking.Insert(num4, guild);
            for (int k = Math.Min(num, num4); k <= Math.Max(num, num4); k++)
            {
                GuildRanking[k].GuildRanking.V = k + 1;
            }
        }
        else if (m_GuildMemberComparer.Compare(guild, GuildRanking.Last) > 0)
        {
            int num5 = BinarySearch(GuildRanking, guild, m_GuildMemberComparer, 0, GuildRanking.Count);
            GuildRanking.Insert(num5, guild);
            for (int l = num5; l < GuildRanking.Count; l++)
            {
                GuildRanking[l].GuildRanking.V = l + 1;
            }
            GuildRanking[100].GuildRanking.V = 0;
            GuildRanking.RemoveAt(100);
        }
    }

    public void AddIPBan(string address, DateTime time)
    {
        if (IPBans.ContainsKey(address))
        {
            IPBans[address] = time;
            SMain.UpdateBlockData(address, time);
        }
        else
        {
            IPBans[address] = time;
            SMain.AddBlockData(address, time);
        }
    }

    public void AddNICBan(string address, DateTime time)
    {
        if (NICBans.ContainsKey(address))
        {
            NICBans[address] = time;
            SMain.UpdateBlockData(address, time, networkAddress: false);
        }
        else
        {
            NICBans[address] = time;
            SMain.AddBlockData(address, time, networkAddress: false);
        }
    }

    public void RemoveIPBan(string address)
    {
        if (IPBans.Remove(address))
        {
            SMain.RemoveBlockData(address);
        }
    }

    public void RemoveNICBan(string address)
    {
        if (NICBans.Remove(address))
        {
            SMain.RemoveBlockData(address);
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
            SMain.AddBlockData(item.Key, item.Value);
        }
        foreach (KeyValuePair<string, DateTime> item2 in NICBans)
        {
            SMain.AddBlockData(item2.Key, item2.Value, networkAddress: false);
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
                int index = BinarySearch(listing, character, m_CombatPowerComparer, 0, listing.Count);
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
            int index = BinarySearch(listing, character, m_CombatPowerComparer, 0, listing.Count);
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
        m_CombatPowerComparer = new CombatPowerComparer();
        m_LevelComparer = new LevelComparer();
        m_PrestigeComparer = new PrestigeComparer();
        m_PKPointComparer = new PKPointComparer();
        m_GuildMemberComparer = new GuildMemberComparer();
    }
}
