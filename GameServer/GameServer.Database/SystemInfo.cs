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

    private sealed class 声望比较器 : IComparer<CharacterInfo>
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

    public readonly DataMonitor<DateTime> 占领时间;

    public readonly DataMonitor<GuildInfo> OccupyGuild;

    public readonly DictionaryMonitor<DateTime, GuildInfo> 申请行会;

    public readonly ListMonitor<CharacterInfo> 个人战力排名;

    public readonly ListMonitor<CharacterInfo> 个人等级排名;

    public readonly ListMonitor<CharacterInfo> 个人声望排名;

    public readonly ListMonitor<CharacterInfo> 个人PK值排名;

    public readonly ListMonitor<CharacterInfo> 战士战力排名;

    public readonly ListMonitor<CharacterInfo> 法师战力排名;

    public readonly ListMonitor<CharacterInfo> 道士战力排名;

    public readonly ListMonitor<CharacterInfo> 刺客战力排名;

    public readonly ListMonitor<CharacterInfo> 弓手战力排名;

    public readonly ListMonitor<CharacterInfo> 龙枪战力排名;

    public readonly ListMonitor<CharacterInfo> 战士等级排名;

    public readonly ListMonitor<CharacterInfo> 法师等级排名;

    public readonly ListMonitor<CharacterInfo> 道士等级排名;

    public readonly ListMonitor<CharacterInfo> 刺客等级排名;

    public readonly ListMonitor<CharacterInfo> 弓手等级排名;

    public readonly ListMonitor<CharacterInfo> 龙枪等级排名;

    public readonly ListMonitor<GuildInfo> 行会人数排名;

    private static readonly CombatPowerComparer 战力计算器;

    private static readonly LevelComparer 等级计算器;

    private static readonly 声望比较器 声望计算器;

    private static readonly PKPointComparer PK值计算器;

    private static readonly GuildMemberComparer 行会计算器;

    public static SystemInfo Info => Session.SystemInfoTable.DataSheet[1] as SystemInfo;

    public SystemInfo()
    {
    }

    public SystemInfo(int 索引)
    {
        Index.V = 索引;
        Session.SystemInfoTable.Add(this);
    }

    public void UpdatePower(CharacterInfo character)
    {
        更新榜单(个人战力排名, 6, character, 战力计算器);
        switch (character.Job.V)
        {
            case GameObjectRace.Warrior:
                更新榜单(战士战力排名, 7, character, 战力计算器);
                break;
            case GameObjectRace.Wizard:
                更新榜单(法师战力排名, 8, character, 战力计算器);
                break;
            case GameObjectRace.Assassin:
                更新榜单(刺客战力排名, 10, character, 战力计算器);
                break;
            case GameObjectRace.Archer:
                更新榜单(弓手战力排名, 11, character, 战力计算器);
                break;
            case GameObjectRace.Taoist:
                更新榜单(道士战力排名, 9, character, 战力计算器);
                break;
            case GameObjectRace.DragonLance:
                更新榜单(龙枪战力排名, 37, character, 战力计算器);
                break;
        }
    }

    public void UpdateLevel(CharacterInfo character)
    {
        更新榜单(个人等级排名, 0, character, 等级计算器);
        switch (character.Job.V)
        {
            case GameObjectRace.Warrior:
                更新榜单(战士等级排名, 1, character, 等级计算器);
                break;
            case GameObjectRace.Wizard:
                更新榜单(法师等级排名, 2, character, 等级计算器);
                break;
            case GameObjectRace.Assassin:
                更新榜单(刺客等级排名, 4, character, 等级计算器);
                break;
            case GameObjectRace.Archer:
                更新榜单(弓手等级排名, 5, character, 等级计算器);
                break;
            case GameObjectRace.Taoist:
                更新榜单(道士等级排名, 3, character, 等级计算器);
                break;
            case GameObjectRace.DragonLance:
                更新榜单(龙枪等级排名, 36, character, 等级计算器);
                break;
        }
    }

    public void 更新声望(CharacterInfo character)
    {
        更新榜单(个人声望排名, 14, character, 声望计算器);
    }

    public void UpdatePKPoint(CharacterInfo character)
    {
        更新榜单(个人PK值排名, 15, character, PK值计算器);
    }

    public void 更新行会(GuildInfo 行会)
    {
        int num = 行会.行会排名.V - 1;
        if (行会人数排名.Count < 100)
        {
            if (num >= 0)
            {
                行会人数排名.RemoveAt(num);
                int num2 = 二分查找(行会人数排名, 行会, 行会计算器, 0, 行会人数排名.Count);
                行会人数排名.Insert(num2, 行会);
                for (int i = Math.Min(num, num2); i <= Math.Max(num, num2); i++)
                {
                    行会人数排名[i].行会排名.V = i + 1;
                }
            }
            else
            {
                int num3 = 二分查找(行会人数排名, 行会, 行会计算器, 0, 行会人数排名.Count);
                行会人数排名.Insert(num3, 行会);
                for (int j = num3; j < 行会人数排名.Count; j++)
                {
                    行会人数排名[j].行会排名.V = j + 1;
                }
            }
        }
        else if (num >= 0)
        {
            行会人数排名.RemoveAt(num);
            int num4 = 二分查找(行会人数排名, 行会, 行会计算器, 0, 行会人数排名.Count);
            行会人数排名.Insert(num4, 行会);
            for (int k = Math.Min(num, num4); k <= Math.Max(num, num4); k++)
            {
                行会人数排名[k].行会排名.V = k + 1;
            }
        }
        else if (行会计算器.Compare(行会, 行会人数排名.Last) > 0)
        {
            int num5 = 二分查找(行会人数排名, 行会, 行会计算器, 0, 行会人数排名.Count);
            行会人数排名.Insert(num5, 行会);
            for (int l = num5; l < 行会人数排名.Count; l++)
            {
                行会人数排名[l].行会排名.V = l + 1;
            }
            行会人数排名[100].行会排名.V = 0;
            行会人数排名.RemoveAt(100);
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
        foreach (KeyValuePair<DateTime, GuildInfo> item in 申请行会)
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

    private static void 更新榜单(ListMonitor<CharacterInfo> 当前榜单, byte 当前类型, object 角色, IComparer<CharacterInfo> 比较方法)
    {
        int num = ((CharacterInfo)角色).当前排名[当前类型] - 1;
        if (当前榜单.Count < 300)
        {
            if (num >= 0)
            {
                当前榜单.RemoveAt(num);
                int num2 = 二分查找(当前榜单, 角色, 比较方法, 0, 当前榜单.Count);
                当前榜单.Insert(num2, (CharacterInfo)角色);
                for (int i = Math.Min(num, num2); i <= Math.Max(num, num2); i++)
                {
                    当前榜单[i].历史排名[当前类型] = 当前榜单[i].当前排名[当前类型];
                    当前榜单[i].当前排名[当前类型] = i + 1;
                }
            }
            else
            {
                int num3 = 二分查找(当前榜单, 角色, 战力计算器, 0, 当前榜单.Count);
                当前榜单.Insert(num3, (CharacterInfo)角色);
                for (int j = num3; j < 当前榜单.Count; j++)
                {
                    当前榜单[j].历史排名[当前类型] = 当前榜单[j].当前排名[当前类型];
                    当前榜单[j].当前排名[当前类型] = j + 1;
                }
            }
        }
        else if (num >= 0)
        {
            当前榜单.RemoveAt(num);
            int num4 = 二分查找(当前榜单, 角色, 比较方法, 0, 当前榜单.Count);
            当前榜单.Insert(num4, (CharacterInfo)角色);
            for (int k = Math.Min(num, num4); k <= Math.Max(num, num4); k++)
            {
                当前榜单[k].历史排名[当前类型] = 当前榜单[k].当前排名[当前类型];
                当前榜单[k].当前排名[当前类型] = k + 1;
            }
        }
        else if (比较方法.Compare((CharacterInfo)角色, 当前榜单.Last) > 0)
        {
            int num5 = 二分查找(当前榜单, 角色, 战力计算器, 0, 当前榜单.Count);
            当前榜单.Insert(num5, (CharacterInfo)角色);
            for (int l = num5; l < 当前榜单.Count; l++)
            {
                当前榜单[l].历史排名[当前类型] = 当前榜单[l].当前排名[当前类型];
                当前榜单[l].当前排名[当前类型] = l + 1;
            }
            当前榜单[300].当前排名.Remove(当前类型);
            当前榜单.RemoveAt(300);
        }
    }

    private static int 二分查找(ListMonitor<CharacterInfo> 列表, object 元素, IComparer<CharacterInfo> 比较器, int 起始位置, int 结束位置)
    {
        if (结束位置 >= 0 && 列表.Count != 0)
        {
            if (起始位置 >= 列表.Count)
            {
                return 列表.Count;
            }
            int num = (起始位置 + 结束位置) / 2;
            int num2 = 比较器.Compare(列表[num], (CharacterInfo)元素);
            if (num2 == 0)
            {
                return num;
            }
            if (num2 > 0)
            {
                if (num + 1 >= 列表.Count)
                {
                    return 列表.Count;
                }
                if (比较器.Compare(列表[num + 1], (CharacterInfo)元素) <= 0)
                {
                    return num + 1;
                }
                return 二分查找(列表, 元素, 比较器, num + 1, 结束位置);
            }
            if (num - 1 < 0)
            {
                return 0;
            }
            if (比较器.Compare(列表[num - 1], (CharacterInfo)元素) >= 0)
            {
                return num;
            }
            return 二分查找(列表, 元素, 比较器, 起始位置, num - 1);
        }
        return 0;
    }

    private static int 二分查找(ListMonitor<GuildInfo> 列表, object 元素, IComparer<GuildInfo> 比较器, int 起始位置, int 结束位置)
    {
        if (结束位置 < 0)
        {
            return 0;
        }
        if (起始位置 >= 列表.Count)
        {
            return 列表.Count;
        }
        int num = (起始位置 + 结束位置) / 2;
        int num2 = 比较器.Compare(列表[num], (GuildInfo)元素);
        if (num2 == 0)
        {
            return num;
        }
        if (num2 > 0)
        {
            if (num + 1 >= 列表.Count)
            {
                return 列表.Count;
            }
            if (比较器.Compare(列表[num + 1], (GuildInfo)元素) <= 0)
            {
                return num + 1;
            }
            return 二分查找(列表, 元素, 比较器, num + 1, 结束位置);
        }
        if (num - 1 < 0)
        {
            return 0;
        }
        if (比较器.Compare(列表[num - 1], (GuildInfo)元素) >= 0)
        {
            return num;
        }
        return 二分查找(列表, 元素, 比较器, 起始位置, num - 1);
    }

    static SystemInfo()
    {
        战力计算器 = new CombatPowerComparer();
        等级计算器 = new LevelComparer();
        声望计算器 = new 声望比较器();
        PK值计算器 = new PKPointComparer();
        行会计算器 = new GuildMemberComparer();
    }
}
