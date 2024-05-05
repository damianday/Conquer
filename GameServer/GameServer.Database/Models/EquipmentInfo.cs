using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameServer.Template;

namespace GameServer.Database;

public class EquipmentInfo : ItemInfo
{
    public readonly DataMonitor<byte> UpgradeCount;
    public readonly DataMonitor<byte> DCPower;
    public readonly DataMonitor<byte> MCPower;
    public readonly DataMonitor<byte> SCPower;
    public readonly DataMonitor<byte> NCPower;
    public readonly DataMonitor<byte> BCPower;
    public readonly DataMonitor<bool> SoulBinding;
    public readonly DataMonitor<byte> 祈祷次数;
    public readonly DataMonitor<sbyte> Luck;
    public readonly DataMonitor<int> 怪物伤害;
    public readonly DataMonitor<bool> 装备神佑;
    public readonly DataMonitor<byte> HolyDamage;
    public readonly DataMonitor<ushort> 圣石数量;
    public readonly DataMonitor<bool> 双铭文栏;
    public readonly DataMonitor<byte> 当前铭栏;
    public readonly DataMonitor<int> 洗练数一;
    public readonly DataMonitor<int> 洗练数二;
    public readonly DataMonitor<byte> Status;

    public readonly ListMonitor<RandomStats> RandomStats;
    public readonly ListMonitor<EquipSlotColor> SlotColor;

    public readonly DictionaryMonitor<byte, InscriptionSkill> InscriptionSkills;
    public readonly DictionaryMonitor<byte, GameItem> 镶嵌灵石;

    public EquipmentItem EquipInfo => base.Info as EquipmentItem;

    public int CombatPower
    {
        get
        {
            if (EquipInfo.Type == ItemType.Weapon)
            {
                int num = (int)((long)(EquipInfo.BasePower * (Luck.V + 20)) * 1717986919L >> 32 >> 3);
                int num2 = HolyDamage.V * 3 + DCPower.V * 5 + MCPower.V * 5 + SCPower.V * 5 + NCPower.V * 5 + BCPower.V * 5;
                int num3 = RandomStats.Sum((RandomStats x) => x.CombatBonus);
                return num + num2 + num3;
            }
            int num4 = 0;
            switch (EquipInfo.EquipSet)
            {
                case GameItemSet.祖玛装备:
                    switch (EquipInfo.Type)
                    {
                        case ItemType.Belt:
                        case ItemType.Boots:
                        case ItemType.Helmet:
                            num4 = 2 * UpgradeCount.V;
                            break;
                        case ItemType.Armour:
                            num4 = 4 * UpgradeCount.V;
                            break;
                    }
                    break;
                case GameItemSet.赤月装备:
                    switch (EquipInfo.Type)
                    {
                        case ItemType.Belt:
                        case ItemType.Boots:
                        case ItemType.Helmet:
                            num4 = 4 * UpgradeCount.V;
                            break;
                        case ItemType.Armour:
                            num4 = 6 * UpgradeCount.V;
                            break;
                    }
                    break;
                case GameItemSet.魔龙装备:
                    switch (EquipInfo.Type)
                    {
                        case ItemType.Belt:
                        case ItemType.Boots:
                        case ItemType.Helmet:
                            num4 = 5 * UpgradeCount.V;
                            break;
                        case ItemType.Armour:
                            num4 = 8 * UpgradeCount.V;
                            break;
                    }
                    break;
                case GameItemSet.苍月装备:
                    switch (EquipInfo.Type)
                    {
                        case ItemType.Belt:
                        case ItemType.Boots:
                        case ItemType.Helmet:
                            num4 = 7 * UpgradeCount.V;
                            break;
                        case ItemType.Armour:
                            num4 = 11 * UpgradeCount.V;
                            break;
                    }
                    break;
                case GameItemSet.星王装备:
                    if (EquipInfo.Type == ItemType.Armour)
                    {
                        num4 = 13 * UpgradeCount.V;
                    }
                    break;
                case GameItemSet.神秘装备:
                case GameItemSet.城主装备:
                    switch (EquipInfo.Type)
                    {
                        case ItemType.Belt:
                        case ItemType.Boots:
                        case ItemType.Helmet:
                            num4 = 9 * UpgradeCount.V;
                            break;
                        case ItemType.Armour:
                            num4 = 13 * UpgradeCount.V;
                            break;
                    }
                    break;
            }
            int num5 = SlotColor.Count * 10;
            foreach (var item in 镶嵌灵石.Values)
            {
                switch (item.Name)
                {
                    case "驭朱灵石8级":
                    case "精绿灵石8级":
                    case "韧紫灵石8级":
                    case "抵御幻彩灵石8级":
                    case "进击幻彩灵石8级":
                    case "盈绿灵石8级":
                    case "狂热幻彩灵石8级":
                    case "透蓝灵石8级":
                    case "守阳灵石8级":
                    case "新阳灵石8级":
                    case "命朱灵石8级":
                    case "蔚蓝灵石8级":
                    case "赤褐灵石8级":
                    case "橙黄灵石8级":
                    case "纯紫灵石8级":
                    case "深灰灵石8级":
                        num5 += 80;
                        break;
                    case "精绿灵石5级":
                    case "新阳灵石5级":
                    case "命朱灵石5级":
                    case "蔚蓝灵石5级":
                    case "橙黄灵石5级":
                    case "进击幻彩灵石5级":
                    case "深灰灵石5级":
                    case "盈绿灵石5级":
                    case "透蓝灵石5级":
                    case "韧紫灵石5级":
                    case "抵御幻彩灵石5级":
                    case "驭朱灵石5级":
                    case "赤褐灵石5级":
                    case "守阳灵石5级":
                    case "狂热幻彩灵石5级":
                    case "纯紫灵石5级":
                        num5 += 50;
                        break;
                    case "精绿灵石2级":
                    case "蔚蓝灵石2级":
                    case "驭朱灵石2级":
                    case "橙黄灵石2级":
                    case "守阳灵石2级":
                    case "纯紫灵石2级":
                    case "透蓝灵石2级":
                    case "抵御幻彩灵石2级":
                    case "命朱灵石2级":
                    case "深灰灵石2级":
                    case "赤褐灵石2级":
                    case "新阳灵石2级":
                    case "进击幻彩灵石2级":
                    case "狂热幻彩灵石2级":
                    case "盈绿灵石2级":
                    case "韧紫灵石2级":
                        num5 += 20;
                        break;
                    case "抵御幻彩灵石7级":
                    case "命朱灵石7级":
                    case "赤褐灵石7级":
                    case "狂热幻彩灵石7级":
                    case "精绿灵石7级":
                    case "纯紫灵石7级":
                    case "韧紫灵石7级":
                    case "驭朱灵石7级":
                    case "深灰灵石7级":
                    case "盈绿灵石7级":
                    case "新阳灵石7级":
                    case "蔚蓝灵石7级":
                    case "橙黄灵石7级":
                    case "守阳灵石7级":
                    case "进击幻彩灵石7级":
                    case "透蓝灵石7级":
                        num5 += 70;
                        break;
                    case "精绿灵石9级":
                    case "驭朱灵石9级":
                    case "蔚蓝灵石9级":
                    case "橙黄灵石9级":
                    case "抵御幻彩灵石9级":
                    case "透蓝灵石9级":
                    case "纯紫灵石9级":
                    case "命朱灵石9级":
                    case "赤褐灵石9级":
                    case "深灰灵石9级":
                    case "守阳灵石9级":
                    case "新阳灵石9级":
                    case "盈绿灵石9级":
                    case "进击幻彩灵石9级":
                    case "狂热幻彩灵石9级":
                    case "韧紫灵石9级":
                        num5 += 90;
                        break;
                    case "驭朱灵石4级":
                    case "深灰灵石4级":
                    case "新阳灵石4级":
                    case "盈绿灵石4级":
                    case "蔚蓝灵石4级":
                    case "命朱灵石4级":
                    case "橙黄灵石4级":
                    case "进击幻彩灵石4级":
                    case "抵御幻彩灵石4级":
                    case "透蓝灵石4级":
                    case "守阳灵石4级":
                    case "精绿灵石4级":
                    case "赤褐灵石4级":
                    case "纯紫灵石4级":
                    case "韧紫灵石4级":
                    case "狂热幻彩灵石4级":
                        num5 += 40;
                        break;
                    case "透蓝灵石6级":
                    case "抵御幻彩灵石6级":
                    case "命朱灵石6级":
                    case "盈绿灵石6级":
                    case "深灰灵石6级":
                    case "蔚蓝灵石6级":
                    case "进击幻彩灵石6级":
                    case "橙黄灵石6级":
                    case "赤褐灵石6级":
                    case "驭朱灵石6级":
                    case "精绿灵石6级":
                    case "新阳灵石6级":
                    case "韧紫灵石6级":
                    case "守阳灵石6级":
                    case "纯紫灵石6级":
                    case "狂热幻彩灵石6级":
                        num5 += 60;
                        break;
                    case "透蓝灵石1级":
                    case "驭朱灵石1级":
                    case "韧紫灵石1级":
                    case "守阳灵石1级":
                    case "赤褐灵石1级":
                    case "纯紫灵石1级":
                    case "狂热幻彩灵石1级":
                    case "精绿灵石1级":
                    case "新阳灵石1级":
                    case "盈绿灵石1级":
                    case "蔚蓝灵石1级":
                    case "橙黄灵石1级":
                    case "深灰灵石1级":
                    case "命朱灵石1级":
                    case "进击幻彩灵石1级":
                    case "抵御幻彩灵石1级":
                        num5 += 10;
                        break;
                    case "蔚蓝灵石10级":
                    case "狂热幻彩灵石10级":
                    case "精绿灵石10级":
                    case "透蓝灵石10级":
                    case "橙黄灵石10级":
                    case "抵御幻彩灵石10级":
                    case "进击幻彩灵石10级":
                    case "命朱灵石10级":
                    case "守阳灵石10级":
                    case "赤褐灵石10级":
                    case "盈绿灵石10级":
                    case "深灰灵石10级":
                    case "韧紫灵石10级":
                    case "纯紫灵石10级":
                    case "新阳灵石10级":
                    case "驭朱灵石10级":
                        num5 += 100;
                        break;
                    case "驭朱灵石3级":
                    case "韧紫灵石3级":
                    case "精绿灵石3级":
                    case "新阳灵石3级":
                    case "守阳灵石3级":
                    case "盈绿灵石3级":
                    case "蔚蓝灵石3级":
                    case "命朱灵石3级":
                    case "橙黄灵石3级":
                    case "进击幻彩灵石3级":
                    case "抵御幻彩灵石3级":
                    case "透蓝灵石3级":
                    case "赤褐灵石3级":
                    case "深灰灵石3级":
                    case "狂热幻彩灵石3级":
                    case "纯紫灵石3级":
                        num5 += 30;
                        break;
                }
            }
            int num6 = RandomStats.Sum((RandomStats x) => x.CombatBonus);
            return EquipInfo.BasePower + num4 + num6 + num5;
        }
    }

    public int RepairCost
    {
        get
        {
            int dura = MaxDura.V - Dura.V;
            decimal cost = ((EquipmentItem)Item.V).RepairCost;
            decimal num3 = (decimal)((EquipmentItem)Item.V).MaxDura * 1000m;
            return (int)(cost / num3 * (decimal)dura);
        }
    }

    public int SpecialRepairCost
    {
        get
        {
            int dura = MaxDura.V - Dura.V;
            decimal cost = ((EquipmentItem)Item.V).SpecialRepairCost;
            decimal num3 = (decimal)((EquipmentItem)Item.V).MaxDura * 1000m;
            return (int)(cost / num3 * dura * Settings.Default.SpecialRepairDiscount * 1.15m);
        }
    }

    public int NeedAttack => EquipInfo.NeedAttack;
    public int NeedMagic => EquipInfo.NeedMagic;
    public int NeedTaoism => EquipInfo.NeedTaoism;
    public int NeedPiercing => EquipInfo.NeedPiercing;
    public int NeedArchery => EquipInfo.NeedArchery;

    public string Name => base.Info.Name;

    public bool CanRemove => ((EquipmentItem)Item.V).CanRemove;
    public bool CanRepair => base.PersistType == PersistentItemType.装备;

    public int 传承材料
    {
        get
        {
            int num = base.ID;
            int result = num switch
            {
                99900022 => 21001,
                99900023 => 21002,
                99900024 => 21003,
                99900025 => 21001,
                99900026 => 21001,
                99900027 => 21003,
                99900028 => 21002,
                99900029 => 21002,
                99900030 => 21001,
                99900031 => 21003,
                99900032 => 21001,
                99900033 => 21002,
                99900037 => 21001,
                99900038 => 21003,
                99900039 => 21002,
                99900044 => 21003,
                99900045 => 21001,
                99900046 => 21002,
                99900047 => 21003,
                99900048 => 21001,
                99900049 => 21003,
                99900050 => 21002,
                99900055 => 21004,
                99900056 => 21004,
                99900057 => 21004,
                99900058 => 21004,
                99900059 => 21004,
                99900060 => 21004,
                99900061 => 21004,
                99900062 => 21004,
                99900063 => 21002,
                99900064 => 21003,
                99900074 => 21005,
                99900076 => 21005,
                99900077 => 21005,
                99900078 => 21005,
                99900079 => 21005,
                99900080 => 21005,
                99900081 => 21005,
                99900082 => 21005,
                99900104 => 21006,
                99900105 => 21006,
                99900106 => 21006,
                99900107 => 21006,
                99900108 => 21006,
                99900109 => 21006,
                99900110 => 21006,
                99900111 => 21006,
                _ => 0,
            };
            return result;
        }
    }

    public string StatDescription
    {
        get
        {
            string text = "";
            Stats stats = new Stats();
            foreach (RandomStats item in RandomStats)
                stats[item.Stat] = item.Value;
            
            if (stats.ContainsKey(Stat.MinDC) || stats.ContainsKey(Stat.MaxDC))
            {
                text += $"\nDC{stats[Stat.MinDC]}-{stats[Stat.MaxDC]}";
            }
            if (stats.ContainsKey(Stat.MinMC) || stats.ContainsKey(Stat.MaxMC))
            {
                text += $"\nMC{stats[Stat.MinMC]}-{stats[Stat.MaxMC]}";
            }
            if (stats.ContainsKey(Stat.MinSC) || stats.ContainsKey(Stat.MaxSC))
            {
                text += $"\nSC{stats[Stat.MinSC]}-{stats[Stat.MaxSC]}";
            }
            if (stats.ContainsKey(Stat.MinNC) || stats.ContainsKey(Stat.MaxNC))
            {
                text += $"\nNC{stats[Stat.MinNC]}-{stats[Stat.MaxNC]}";
            }
            if (stats.ContainsKey(Stat.MinBC) || stats.ContainsKey(Stat.MaxBC))
            {
                text += $"\nBC{stats[Stat.MinBC]}-{stats[Stat.MaxBC]}";
            }
            if (stats.ContainsKey(Stat.MinDef) || stats.ContainsKey(Stat.MaxDef))
            {
                text += $"\nDef{stats[Stat.MinDef]}-{stats[Stat.MaxDef]}";
            }
            if (stats.ContainsKey(Stat.MinMCDef) || stats.ContainsKey(Stat.MaxMCDef))
            {
                text += $"\nMCDef{stats[Stat.MinMCDef]}-{stats[Stat.MaxMCDef]}";
            }
            if (stats.ContainsKey(Stat.PhysicalAccuracy))
            {
                text += $"\nPhysicalAccuracy{stats[Stat.PhysicalAccuracy]}";
            }
            if (stats.ContainsKey(Stat.PhysicalAgility))
            {
                text += $"\nPhysicalAgility{stats[Stat.PhysicalAgility]}";
            }
            if (stats.ContainsKey(Stat.MaxHP))
            {
                text += $"\nMaxHP{stats[Stat.MaxHP]}";
            }
            if (stats.ContainsKey(Stat.MaxMP))
            {
                text += $"\nMaxMP{stats[Stat.MaxMP]}";
            }
            if (stats.ContainsKey(Stat.MagicEvade))
            {
                text += $"\nMagicEvade{stats[Stat.MagicEvade] / 100}%";
            }
            if (stats.ContainsKey(Stat.PoisonEvade))
            {
                text += $"\nPoisonEvade{stats[Stat.PoisonEvade] / 100}%";
            }
            if (stats.ContainsKey(Stat.Luck))
            {
                text += $"\nLuck+{stats[Stat.Luck]}";
            }
            return text;
        }
    }

    public InscriptionSkill FirstInscription
    {
        get
        {
            if (当前铭栏.V == 0)
            {
                return InscriptionSkills[0];
            }
            return InscriptionSkills[2];
        }
        set
        {
            if (当前铭栏.V == 0)
            {
                InscriptionSkills[0] = value;
            }
            else
            {
                InscriptionSkills[2] = value;
            }
        }
    }

    public InscriptionSkill SecondInscription
    {
        get
        {
            if (当前铭栏.V == 0)
            {
                return InscriptionSkills[1];
            }
            return InscriptionSkills[3];
        }
        set
        {
            if (当前铭栏.V == 0)
            {
                InscriptionSkills[1] = value;
            }
            else
            {
                InscriptionSkills[3] = value;
            }
        }
    }

    public InscriptionSkill BestInscription
    {
        get
        {
            if (当前铭栏.V == 0)
            {
                if (InscriptionSkills[0].Quality < InscriptionSkills[1].Quality)
                {
                    return InscriptionSkills[1];
                }
                return InscriptionSkills[0];
            }
            if (InscriptionSkills[2].Quality < InscriptionSkills[3].Quality)
            {
                return InscriptionSkills[3];
            }
            return InscriptionSkills[2];
        }
        set
        {
            if (当前铭栏.V == 0)
            {
                if (InscriptionSkills[0].Quality >= InscriptionSkills[1].Quality)
                {
                    InscriptionSkills[0] = value;
                }
                else
                {
                    InscriptionSkills[1] = value;
                }
            }
            else if (InscriptionSkills[2].Quality >= InscriptionSkills[3].Quality)
            {
                InscriptionSkills[2] = value;
            }
            else
            {
                InscriptionSkills[3] = value;
            }
        }
    }

    public InscriptionSkill WorstInscription
    {
        get
        {
            if (当前铭栏.V == 0)
            {
                if (InscriptionSkills[0].Quality >= InscriptionSkills[1].Quality)
                {
                    return InscriptionSkills[1];
                }
                return InscriptionSkills[0];
            }
            if (InscriptionSkills[2].Quality >= InscriptionSkills[3].Quality)
            {
                return InscriptionSkills[3];
            }
            return InscriptionSkills[2];
        }
        set
        {
            if (当前铭栏.V == 0)
            {
                if (InscriptionSkills[0].Quality < InscriptionSkills[1].Quality)
                {
                    InscriptionSkills[0] = value;
                }
                else
                {
                    InscriptionSkills[1] = value;
                }
            }
            else if (InscriptionSkills[2].Quality < InscriptionSkills[3].Quality)
            {
                InscriptionSkills[2] = value;
            }
            else
            {
                InscriptionSkills[3] = value;
            }
        }
    }

    public int 双铭文点
    {
        get
        {
            if (当前铭栏.V == 0)
            {
                return 洗练数一.V;
            }
            return 洗练数二.V;
        }
        set
        {
            if (当前铭栏.V == 0)
            {
                洗练数一.V = value;
            }
            else
            {
                洗练数二.V = value;
            }
        }
    }

    public Stats Stats
    {
        get
        {
            Stats stats = new Stats();
            if (EquipInfo.MinDC != 0)
            {
                stats[Stat.MinDC] = EquipInfo.MinDC;
            }
            if (EquipInfo.MaxDC != 0)
            {
                stats[Stat.MaxDC] = EquipInfo.MaxDC;
            }
            if (EquipInfo.MinMC != 0)
            {
                stats[Stat.MinMC] = EquipInfo.MinMC;
            }
            if (EquipInfo.MaxMC != 0)
            {
                stats[Stat.MaxMC] = EquipInfo.MaxMC;
            }
            if (EquipInfo.MinSC != 0)
            {
                stats[Stat.MinSC] = EquipInfo.MinSC;
            }
            if (EquipInfo.MaxSC != 0)
            {
                stats[Stat.MaxSC] = EquipInfo.MaxSC;
            }
            if (EquipInfo.MinNC != 0)
            {
                stats[Stat.MinNC] = EquipInfo.MinNC;
            }
            if (EquipInfo.MaxNC != 0)
            {
                stats[Stat.MaxNC] = EquipInfo.MaxNC;
            }
            if (EquipInfo.MinBC != 0)
            {
                stats[Stat.MinBC] = EquipInfo.MinBC;
            }
            if (EquipInfo.MaxBC != 0)
            {
                stats[Stat.MaxBC] = EquipInfo.MaxBC;
            }
            if (EquipInfo.MinDef != 0)
            {
                stats[Stat.MinDef] = EquipInfo.MinDef;
            }
            if (EquipInfo.MaxDef != 0)
            {
                stats[Stat.MaxDef] = EquipInfo.MaxDef;
            }
            if (EquipInfo.MinMCDef != 0)
            {
                stats[Stat.MinMCDef] = EquipInfo.MinMCDef;
            }
            if (EquipInfo.MaxMCDef != 0)
            {
                stats[Stat.MaxMCDef] = EquipInfo.MaxMCDef;
            }
            if (EquipInfo.MaxHP != 0)
            {
                stats[Stat.MaxHP] = EquipInfo.MaxHP;
            }
            if (EquipInfo.MaxMP != 0)
            {
                stats[Stat.MaxMP] = EquipInfo.MaxMP;
            }
            if (EquipInfo.AttackSpeed != 0)
            {
                stats[Stat.AttackSpeed] = EquipInfo.AttackSpeed;
            }
            if (EquipInfo.MagicEvade != 0)
            {
                stats[Stat.MagicEvade] = EquipInfo.MagicEvade;
            }
            if (EquipInfo.PhysicalAccuracy != 0)
            {
                stats[Stat.PhysicalAccuracy] = EquipInfo.PhysicalAccuracy;
            }
            if (EquipInfo.PhysicalAgility != 0)
            {
                stats[Stat.PhysicalAgility] = EquipInfo.PhysicalAgility;
            }
            if (EquipInfo.MinHC != 0)
            {
                stats[Stat.MinHC] = EquipInfo.MinHC;
            }
            if (EquipInfo.MaxHC != 0)
            {
                stats[Stat.MaxHC] = EquipInfo.MaxHC;
            }
            if (EquipInfo.Luck != 0)
            {
                stats[Stat.Luck] = EquipInfo.Luck;
            }
            if (EquipInfo.怪物伤害 != 0)
            {
                stats[Stat.MonsterDamage] = EquipInfo.怪物伤害;
            }
            if (Luck.V != 0)
            {
                stats[Stat.Luck] = (stats.ContainsKey(Stat.Luck) ? (stats[Stat.Luck] + Luck.V) : Luck.V);
            }
            if (DCPower.V != 0)
            {
                stats[Stat.MaxDC] = (stats.ContainsKey(Stat.MaxDC) ? (stats[Stat.MaxDC] + DCPower.V) : DCPower.V);
            }
            if (MCPower.V != 0)
            {
                stats[Stat.MaxMC] = (stats.ContainsKey(Stat.MaxMC) ? (stats[Stat.MaxMC] + MCPower.V) : MCPower.V);
            }
            if (SCPower.V != 0)
            {
                stats[Stat.MaxSC] = (stats.ContainsKey(Stat.MaxSC) ? (stats[Stat.MaxSC] + SCPower.V) : SCPower.V);
            }
            if (NCPower.V != 0)
            {
                stats[Stat.MaxNC] = (stats.ContainsKey(Stat.MaxNC) ? (stats[Stat.MaxNC] + NCPower.V) : NCPower.V);
            }
            if (BCPower.V != 0)
            {
                stats[Stat.MaxBC] = (stats.ContainsKey(Stat.MaxBC) ? (stats[Stat.MaxBC] + BCPower.V) : BCPower.V);
            }
            foreach (RandomStats item in RandomStats.ToList())
            {
                stats[item.Stat] = (stats.ContainsKey(item.Stat) ? (stats[item.Stat] + item.Value) : item.Value);
            }
            using IEnumerator<GameItem> enumerator2 = 镶嵌灵石.Values.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                switch (enumerator2.Current.ID)
                {
                    case 10320:
                        stats[Stat.MaxMC] = ((!stats.ContainsKey(Stat.MaxMC)) ? 1 : (stats[Stat.MaxMC] + 1));
                        break;
                    case 10321:
                        stats[Stat.MaxMC] = (stats.ContainsKey(Stat.MaxMC) ? (stats[Stat.MaxMC] + 2) : 2);
                        break;
                    case 10322:
                        stats[Stat.MaxMC] = (stats.ContainsKey(Stat.MaxMC) ? (stats[Stat.MaxMC] + 3) : 3);
                        break;
                    case 10323:
                        stats[Stat.MaxMC] = (stats.ContainsKey(Stat.MaxMC) ? (stats[Stat.MaxMC] + 4) : 4);
                        break;
                    case 10324:
                        stats[Stat.MaxMC] = (stats.ContainsKey(Stat.MaxMC) ? (stats[Stat.MaxMC] + 5) : 5);
                        break;
                    case 10220:
                        stats[Stat.MaxDef] = ((!stats.ContainsKey(Stat.MaxDef)) ? 1 : (stats[Stat.MaxDef] + 1));
                        break;
                    case 10221:
                        stats[Stat.MaxDef] = (stats.ContainsKey(Stat.MaxDef) ? (stats[Stat.MaxDef] + 2) : 2);
                        break;
                    case 10222:
                        stats[Stat.MaxDef] = (stats.ContainsKey(Stat.MaxDef) ? (stats[Stat.MaxDef] + 3) : 3);
                        break;
                    case 10223:
                        stats[Stat.MaxDef] = (stats.ContainsKey(Stat.MaxDef) ? (stats[Stat.MaxDef] + 4) : 4);
                        break;
                    case 10224:
                        stats[Stat.MaxDef] = (stats.ContainsKey(Stat.MaxDef) ? (stats[Stat.MaxDef] + 5) : 5);
                        break;
                    case 10110:
                        stats[Stat.MaxSC] = ((!stats.ContainsKey(Stat.MaxSC)) ? 1 : (stats[Stat.MaxSC] + 1));
                        break;
                    case 10111:
                        stats[Stat.MaxSC] = (stats.ContainsKey(Stat.MaxSC) ? (stats[Stat.MaxSC] + 2) : 2);
                        break;
                    case 10112:
                        stats[Stat.MaxSC] = (stats.ContainsKey(Stat.MaxSC) ? (stats[Stat.MaxSC] + 3) : 3);
                        break;
                    case 10113:
                        stats[Stat.MaxSC] = (stats.ContainsKey(Stat.MaxSC) ? (stats[Stat.MaxSC] + 4) : 4);
                        break;
                    case 10114:
                        stats[Stat.MaxSC] = (stats.ContainsKey(Stat.MaxSC) ? (stats[Stat.MaxSC] + 5) : 5);
                        break;
                    case 10120:
                        stats[Stat.MaxHP] = (stats.ContainsKey(Stat.MaxHP) ? (stats[Stat.MaxHP] + 5) : 5);
                        break;
                    case 10121:
                        stats[Stat.MaxHP] = (stats.ContainsKey(Stat.MaxHP) ? (stats[Stat.MaxHP] + 10) : 10);
                        break;
                    case 10122:
                        stats[Stat.MaxHP] = (stats.ContainsKey(Stat.MaxHP) ? (stats[Stat.MaxHP] + 15) : 15);
                        break;
                    case 10123:
                        stats[Stat.MaxHP] = (stats.ContainsKey(Stat.MaxHP) ? (stats[Stat.MaxHP] + 20) : 20);
                        break;
                    case 10124:
                        stats[Stat.MaxHP] = (stats.ContainsKey(Stat.MaxHP) ? (stats[Stat.MaxHP] + 25) : 25);
                        break;
                    case 10520:
                        stats[Stat.MaxMCDef] = ((!stats.ContainsKey(Stat.MaxMCDef)) ? 1 : (stats[Stat.MaxMCDef] + 1));
                        break;
                    case 10521:
                        stats[Stat.MaxMCDef] = (stats.ContainsKey(Stat.MaxMCDef) ? (stats[Stat.MaxMCDef] + 2) : 2);
                        break;
                    case 10522:
                        stats[Stat.MaxMCDef] = (stats.ContainsKey(Stat.MaxMCDef) ? (stats[Stat.MaxMCDef] + 3) : 3);
                        break;
                    case 10523:
                        stats[Stat.MaxMCDef] = (stats.ContainsKey(Stat.MaxMCDef) ? (stats[Stat.MaxMCDef] + 4) : 4);
                        break;
                    case 10524:
                        stats[Stat.MaxMCDef] = (stats.ContainsKey(Stat.MaxMCDef) ? (stats[Stat.MaxMCDef] + 5) : 5);
                        break;
                    case 10420:
                        stats[Stat.MaxDC] = ((!stats.ContainsKey(Stat.MaxDC)) ? 1 : (stats[Stat.MaxDC] + 1));
                        break;
                    case 10421:
                        stats[Stat.MaxDC] = (stats.ContainsKey(Stat.MaxDC) ? (stats[Stat.MaxDC] + 2) : 2);
                        break;
                    case 10422:
                        stats[Stat.MaxDC] = (stats.ContainsKey(Stat.MaxDC) ? (stats[Stat.MaxDC] + 3) : 3);
                        break;
                    case 10423:
                        stats[Stat.MaxDC] = (stats.ContainsKey(Stat.MaxDC) ? (stats[Stat.MaxDC] + 4) : 4);
                        break;
                    case 10424:
                        stats[Stat.MaxDC] = (stats.ContainsKey(Stat.MaxDC) ? (stats[Stat.MaxDC] + 5) : 5);
                        break;
                    case 10720:
                        stats[Stat.MaxBC] = ((!stats.ContainsKey(Stat.MaxBC)) ? 1 : (stats[Stat.MaxBC] + 1));
                        break;
                    case 10721:
                        stats[Stat.MaxBC] = (stats.ContainsKey(Stat.MaxBC) ? (stats[Stat.MaxBC] + 2) : 2);
                        break;
                    case 10722:
                        stats[Stat.MaxBC] = (stats.ContainsKey(Stat.MaxBC) ? (stats[Stat.MaxBC] + 3) : 3);
                        break;
                    case 10723:
                        stats[Stat.MaxBC] = (stats.ContainsKey(Stat.MaxBC) ? (stats[Stat.MaxBC] + 4) : 4);
                        break;
                    case 10724:
                        stats[Stat.MaxBC] = (stats.ContainsKey(Stat.MaxBC) ? (stats[Stat.MaxBC] + 5) : 5);
                        break;
                    case 10620:
                        stats[Stat.MaxNC] = ((!stats.ContainsKey(Stat.MaxNC)) ? 1 : (stats[Stat.MaxNC] + 1));
                        break;
                    case 10621:
                        stats[Stat.MaxNC] = (stats.ContainsKey(Stat.MaxNC) ? (stats[Stat.MaxNC] + 2) : 2);
                        break;
                    case 10622:
                        stats[Stat.MaxNC] = (stats.ContainsKey(Stat.MaxNC) ? (stats[Stat.MaxNC] + 3) : 3);
                        break;
                    case 10623:
                        stats[Stat.MaxNC] = (stats.ContainsKey(Stat.MaxNC) ? (stats[Stat.MaxNC] + 4) : 4);
                        break;
                    case 10624:
                        stats[Stat.MaxNC] = (stats.ContainsKey(Stat.MaxNC) ? (stats[Stat.MaxNC] + 5) : 5);
                        break;
                }
            }
            return stats;
        }
    }

    public int 重铸所需灵气
    {
        get
        {
            switch (base.Type)
            {
                default:
                    return 0;
                case ItemType.Weapon:
                    return 112001;
                case ItemType.Armour:
                case ItemType.Cloak:
                case ItemType.Belt:
                case ItemType.Boots:
                case ItemType.ShoulderPad:
                case ItemType.护腕:
                case ItemType.Helmet:
                    return 112003;
                case ItemType.Necklace:
                case ItemType.Ring:
                case ItemType.Bracelet:
                case ItemType.Medal:
                case ItemType.玉佩:
                    return 112002;
            }
        }
    }

    public EquipmentInfo()
    {
    }

    public EquipmentInfo(EquipmentItem item, CharacterInfo character, byte grid, byte location, bool random = false)
    {
        Item.V = item;
        Owner.V = character;
        Grid.V = grid;
        Location.V = location;
        CreatedDate.V = SEngine.CurrentTime;
        Status.V = 1;
        MaxDura.V = ((item.PersistType == PersistentItemType.装备) ? (item.MaxDura * 1000) : item.MaxDura);
        Dura.V = ((!random || item.PersistType != PersistentItemType.装备) ? MaxDura.V : SEngine.Random.Next(0, MaxDura.V));
        if (random && item.PersistType == PersistentItemType.装备)
        {
            RandomStats.SetValue(GameServer.Template.EquipmentStats.生成属性(base.Type));
        }
        Session.EquipmentInfoTable.Add(this, true);
    }

    public override byte[] ToArray()
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        writer.Write(Version);
        writer.Write(Owner.V?.Index.V ?? 0);
        writer.Write(Compute.TimeSeconds(CreatedDate.V));
        writer.Write(Item.V.ID);
        writer.Write(Grid.V);
        writer.Write(Location.V);
        writer.Write(Dura.V);
        writer.Write(MaxDura.V);
        writer.Write((byte)(IsBound ? 10u : 0u));
        int num = 256;
        num = 0x100 | 当前铭栏.V;
        if (双铭文栏.V)
        {
            num |= 0x200;
        }
        writer.Write((short)num);
        int num2 = 0;
        if (Status.V != 1)
        {
            num2 |= 1;
        }
        else if (RandomStats.Count != 0)
        {
            num2 |= 1;
        }
        else if (HolyDamage.V != 0)
        {
            num2 |= 1;
        }
        if (RandomStats.Count >= 1)
        {
            num2 |= 2;
        }
        if (RandomStats.Count >= 2)
        {
            num2 |= 4;
        }
        if (RandomStats.Count >= 3)
        {
            num2 |= 8;
        }
        if (RandomStats.Count >= 4)
        {
            num2 |= 0x10;
        }
        if (Luck.V != 0)
        {
            num2 |= 0x800;
        }
        if (UpgradeCount.V != 0)
        {
            num2 |= 0x1000;
        }
        if (SlotColor.Count != 0)
        {
            num2 |= 0x2000;
        }
        if (镶嵌灵石[0] != null)
        {
            num2 |= 0x4000;
        }
        if (镶嵌灵石[1] != null)
        {
            num2 |= 0x8000;
        }
        if (镶嵌灵石[2] != null)
        {
            num2 |= 0x10000;
        }
        if (镶嵌灵石[3] != null)
        {
            num2 |= 0x20000;
        }
        if (HolyDamage.V != 0)
        {
            num2 |= 0x400000;
        }
        else if (圣石数量.V != 0)
        {
            num2 |= 0x400000;
        }
        if (祈祷次数.V != 0)
        {
            num2 |= 0x800000;
        }
        if (装备神佑.V)
        {
            num2 |= 0x2000000;
        }
        writer.Write(num2);
        if (((uint)num2 & (true ? 1u : 0u)) != 0)
        {
            writer.Write(Status.V);
        }
        if (((uint)num2 & 2u) != 0)
        {
            writer.Write((ushort)RandomStats[0].StatID);
        }
        if (((uint)num2 & 4u) != 0)
        {
            writer.Write((ushort)RandomStats[1].StatID);
        }
        if (((uint)num2 & 8u) != 0)
        {
            writer.Write((ushort)RandomStats[2].StatID);
        }
        if (((uint)num2 & 0x10u) != 0)
        {
            writer.Write((ushort)RandomStats[3].StatID);
        }
        if (((uint)num & 0x100u) != 0)
        {
            int num3 = 0;
            if (InscriptionSkills[0] != null)
            {
                num3 |= 1;
            }
            if (InscriptionSkills[1] != null)
            {
                num3 |= 2;
            }
            writer.Write((short)num3);
            writer.Write(洗练数一.V * 10000);
            if (((uint)num3 & (true ? 1u : 0u)) != 0)
            {
                writer.Write(InscriptionSkills[0].Index);
            }
            if (((uint)num3 & 2u) != 0)
            {
                writer.Write(InscriptionSkills[1].Index);
            }
        }
        if (((uint)num & 0x200u) != 0)
        {
            int num4 = 0;
            if (InscriptionSkills[2] != null)
            {
                num4 |= 1;
            }
            if (InscriptionSkills[3] != null)
            {
                num4 |= 2;
            }
            writer.Write((short)num4);
            writer.Write(洗练数二.V * 10000);
            if (((uint)num4 & (true ? 1u : 0u)) != 0)
            {
                writer.Write(InscriptionSkills[2].Index);
            }
            if (((uint)num4 & 2u) != 0)
            {
                writer.Write(InscriptionSkills[3].Index);
            }
        }
        if (((uint)num2 & 0x800u) != 0)
        {
            writer.Write(Luck.V);
        }
        if (((uint)num2 & 0x1000u) != 0)
        {
            writer.Write(UpgradeCount.V);
            writer.Write((byte)0);
            writer.Write(DCPower.V);
            writer.Write(MCPower.V);
            writer.Write(SCPower.V);
            writer.Write(NCPower.V);
            writer.Write(BCPower.V);
            writer.Write(new byte[3]);
            writer.Write(SoulBinding.V);
        }
        if (((uint)num2 & 0x2000u) != 0)
        {
            writer.Write(new byte[4]
            {
                (byte)SlotColor[0],
                (byte)SlotColor[1],
                (byte)SlotColor[2],
                (byte)SlotColor[3]
            });
        }
        if (((uint)num2 & 0x4000u) != 0)
        {
            writer.Write(镶嵌灵石[0].ID);
        }
        if (((uint)num2 & 0x8000u) != 0)
        {
            writer.Write(镶嵌灵石[1].ID);
        }
        if (((uint)num2 & 0x10000u) != 0)
        {
            writer.Write(镶嵌灵石[2].ID);
        }
        if (((uint)num2 & 0x20000u) != 0)
        {
            writer.Write(镶嵌灵石[3].ID);
        }
        if (((uint)num2 & 0x80000u) != 0)
        {
            writer.Write(0);
        }
        if (((uint)num2 & 0x100000u) != 0)
        {
            writer.Write(0);
        }
        if (((uint)num2 & 0x200000u) != 0)
        {
            writer.Write(0);
        }
        if (((uint)num2 & 0x400000u) != 0)
        {
            writer.Write(HolyDamage.V);
            writer.Write(圣石数量.V);
        }
        if (((uint)num2 & 0x800000u) != 0)
        {
            writer.Write((int)祈祷次数.V);
        }
        if (((uint)num2 & 0x2000000u) != 0)
        {
            writer.Write(装备神佑.V);
        }
        if (((uint)num2 & 0x4000000u) != 0)
        {
            writer.Write(0);
        }
        writer.Write(0); // Unknown
        return ms.ToArray();
    }
}
