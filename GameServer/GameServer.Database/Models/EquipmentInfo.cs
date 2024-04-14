using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameServer.Template;

namespace GameServer.Database;

public class EquipmentInfo : ItemInfo
{
    public readonly DataMonitor<byte> 升级次数;
    public readonly DataMonitor<byte> 升级攻击;
    public readonly DataMonitor<byte> 升级魔法;
    public readonly DataMonitor<byte> 升级道术;
    public readonly DataMonitor<byte> 升级刺术;
    public readonly DataMonitor<byte> 升级弓术;
    public readonly DataMonitor<bool> 灵魂绑定;
    public readonly DataMonitor<byte> 祈祷次数;
    public readonly DataMonitor<sbyte> Luck;
    public readonly DataMonitor<int> 怪物伤害;
    public readonly DataMonitor<bool> 装备神佑;
    public readonly DataMonitor<byte> 神圣伤害;
    public readonly DataMonitor<ushort> 圣石数量;
    public readonly DataMonitor<bool> 双铭文栏;
    public readonly DataMonitor<byte> 当前铭栏;
    public readonly DataMonitor<int> 洗练数一;
    public readonly DataMonitor<int> 洗练数二;
    public readonly DataMonitor<byte> 物品状态;

    public readonly ListMonitor<RandomStats> 随机属性;
    public readonly ListMonitor<装备孔洞颜色> 孔洞颜色;

    public readonly DictionaryMonitor<byte, InscriptionSkill> 铭文技能;
    public readonly DictionaryMonitor<byte, GameItem> 镶嵌灵石;

    public EquipmentItem 装备模板 => base.Info as EquipmentItem;

    public int 装备战力
    {
        get
        {
            if (装备模板.Type == ItemType.Weapon)
            {
                int num = (int)((long)(装备模板.BasePower * (Luck.V + 20)) * 1717986919L >> 32 >> 3);
                int num2 = 神圣伤害.V * 3 + 升级攻击.V * 5 + 升级魔法.V * 5 + 升级道术.V * 5 + 升级刺术.V * 5 + 升级弓术.V * 5;
                int num3 = 随机属性.Sum((RandomStats x) => x.CombatBonus);
                return num + num2 + num3;
            }
            int num4 = 0;
            switch (装备模板.EquipSet)
            {
                case GameItemSet.祖玛装备:
                    switch (装备模板.Type)
                    {
                        case ItemType.Belt:
                        case ItemType.Boots:
                        case ItemType.Helmet:
                            num4 = 2 * 升级次数.V;
                            break;
                        case ItemType.Armour:
                            num4 = 4 * 升级次数.V;
                            break;
                    }
                    break;
                case GameItemSet.赤月装备:
                    switch (装备模板.Type)
                    {
                        case ItemType.Belt:
                        case ItemType.Boots:
                        case ItemType.Helmet:
                            num4 = 4 * 升级次数.V;
                            break;
                        case ItemType.Armour:
                            num4 = 6 * 升级次数.V;
                            break;
                    }
                    break;
                case GameItemSet.魔龙装备:
                    switch (装备模板.Type)
                    {
                        case ItemType.Belt:
                        case ItemType.Boots:
                        case ItemType.Helmet:
                            num4 = 5 * 升级次数.V;
                            break;
                        case ItemType.Armour:
                            num4 = 8 * 升级次数.V;
                            break;
                    }
                    break;
                case GameItemSet.苍月装备:
                    switch (装备模板.Type)
                    {
                        case ItemType.Belt:
                        case ItemType.Boots:
                        case ItemType.Helmet:
                            num4 = 7 * 升级次数.V;
                            break;
                        case ItemType.Armour:
                            num4 = 11 * 升级次数.V;
                            break;
                    }
                    break;
                case GameItemSet.星王装备:
                    if (装备模板.Type == ItemType.Armour)
                    {
                        num4 = 13 * 升级次数.V;
                    }
                    break;
                case GameItemSet.神秘装备:
                case GameItemSet.城主装备:
                    switch (装备模板.Type)
                    {
                        case ItemType.Belt:
                        case ItemType.Boots:
                        case ItemType.Helmet:
                            num4 = 9 * 升级次数.V;
                            break;
                        case ItemType.Armour:
                            num4 = 13 * 升级次数.V;
                            break;
                    }
                    break;
            }
            int num5 = 孔洞颜色.Count * 10;
            using (IEnumerator<GameItem> enumerator = 镶嵌灵石.Values.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    switch (enumerator.Current.Name)
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
            }
            int num6 = 随机属性.Sum((RandomStats x) => x.CombatBonus);
            return 装备模板.BasePower + num4 + num6 + num5;
        }
    }

    public int 修理费用
    {
        get
        {
            int num = MaxDura.V - Dura.V;
            decimal num2 = ((EquipmentItem)Item.V).RepairCost;
            decimal num3 = (decimal)((EquipmentItem)Item.V).MaxDura * 1000m;
            return (int)(num2 / num3 * (decimal)num);
        }
    }

    public int 特修费用
    {
        get
        {
            decimal num = (decimal)MaxDura.V - (decimal)Dura.V;
            decimal num2 = ((EquipmentItem)Item.V).SpecialRepairCost;
            decimal num3 = (decimal)((EquipmentItem)Item.V).MaxDura * 1000m;
            return (int)(num2 / num3 * num * Config.SpecialRepairDiscount * 1.15m);
        }
    }

    public int NeedAttack => ((EquipmentItem)base.Info).NeedAttack;

    public int NeedMagic => ((EquipmentItem)base.Info).NeedMagic;

    public int NeedTaoism => ((EquipmentItem)base.Info).NeedTaoism;

    public int NeedPiercing => ((EquipmentItem)base.Info).NeedPiercing;

    public int NeedArchery => ((EquipmentItem)base.Info).NeedArchery;

    public string Name => base.Info.Name;

    public bool 禁止卸下 => ((EquipmentItem)Item.V).CanRemove;

    public bool 能否修理 => base.PersistType == PersistentItemType.装备;

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

    public string 属性描述
    {
        get
        {
            string text = "";
            Stats stats = new Stats();
            foreach (RandomStats item in 随机属性)
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

    public InscriptionSkill 第一铭文
    {
        get
        {
            if (当前铭栏.V == 0)
            {
                return 铭文技能[0];
            }
            return 铭文技能[2];
        }
        set
        {
            if (当前铭栏.V == 0)
            {
                铭文技能[0] = value;
            }
            else
            {
                铭文技能[2] = value;
            }
        }
    }

    public InscriptionSkill 第二铭文
    {
        get
        {
            if (当前铭栏.V == 0)
            {
                return 铭文技能[1];
            }
            return 铭文技能[3];
        }
        set
        {
            if (当前铭栏.V == 0)
            {
                铭文技能[1] = value;
            }
            else
            {
                铭文技能[3] = value;
            }
        }
    }

    public InscriptionSkill 最优铭文
    {
        get
        {
            if (当前铭栏.V == 0)
            {
                if (铭文技能[0].Quality < 铭文技能[1].Quality)
                {
                    return 铭文技能[1];
                }
                return 铭文技能[0];
            }
            if (铭文技能[2].Quality < 铭文技能[3].Quality)
            {
                return 铭文技能[3];
            }
            return 铭文技能[2];
        }
        set
        {
            if (当前铭栏.V == 0)
            {
                if (铭文技能[0].Quality >= 铭文技能[1].Quality)
                {
                    铭文技能[0] = value;
                }
                else
                {
                    铭文技能[1] = value;
                }
            }
            else if (铭文技能[2].Quality >= 铭文技能[3].Quality)
            {
                铭文技能[2] = value;
            }
            else
            {
                铭文技能[3] = value;
            }
        }
    }

    public InscriptionSkill 最差铭文
    {
        get
        {
            if (当前铭栏.V == 0)
            {
                if (铭文技能[0].Quality >= 铭文技能[1].Quality)
                {
                    return 铭文技能[1];
                }
                return 铭文技能[0];
            }
            if (铭文技能[2].Quality >= 铭文技能[3].Quality)
            {
                return 铭文技能[3];
            }
            return 铭文技能[2];
        }
        set
        {
            if (当前铭栏.V == 0)
            {
                if (铭文技能[0].Quality < 铭文技能[1].Quality)
                {
                    铭文技能[0] = value;
                }
                else
                {
                    铭文技能[1] = value;
                }
            }
            else if (铭文技能[2].Quality < 铭文技能[3].Quality)
            {
                铭文技能[2] = value;
            }
            else
            {
                铭文技能[3] = value;
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

    public Stats 装备属性
    {
        get
        {
            Stats dictionary = new Stats();
            if (装备模板.MinDC != 0)
            {
                dictionary[Stat.MinDC] = 装备模板.MinDC;
            }
            if (装备模板.MaxDC != 0)
            {
                dictionary[Stat.MaxDC] = 装备模板.MaxDC;
            }
            if (装备模板.MinMC != 0)
            {
                dictionary[Stat.MinMC] = 装备模板.MinMC;
            }
            if (装备模板.MaxMC != 0)
            {
                dictionary[Stat.MaxMC] = 装备模板.MaxMC;
            }
            if (装备模板.MinSC != 0)
            {
                dictionary[Stat.MinSC] = 装备模板.MinSC;
            }
            if (装备模板.MaxSC != 0)
            {
                dictionary[Stat.MaxSC] = 装备模板.MaxSC;
            }
            if (装备模板.MinNC != 0)
            {
                dictionary[Stat.MinNC] = 装备模板.MinNC;
            }
            if (装备模板.MaxNC != 0)
            {
                dictionary[Stat.MaxNC] = 装备模板.MaxNC;
            }
            if (装备模板.MinBC != 0)
            {
                dictionary[Stat.MinBC] = 装备模板.MinBC;
            }
            if (装备模板.MaxBC != 0)
            {
                dictionary[Stat.MaxBC] = 装备模板.MaxBC;
            }
            if (装备模板.MinDef != 0)
            {
                dictionary[Stat.MinDef] = 装备模板.MinDef;
            }
            if (装备模板.MaxDef != 0)
            {
                dictionary[Stat.MaxDef] = 装备模板.MaxDef;
            }
            if (装备模板.MinMCDef != 0)
            {
                dictionary[Stat.MinMCDef] = 装备模板.MinMCDef;
            }
            if (装备模板.MaxMCDef != 0)
            {
                dictionary[Stat.MaxMCDef] = 装备模板.MaxMCDef;
            }
            if (装备模板.MaxHP != 0)
            {
                dictionary[Stat.MaxHP] = 装备模板.MaxHP;
            }
            if (装备模板.MaxMP != 0)
            {
                dictionary[Stat.MaxMP] = 装备模板.MaxMP;
            }
            if (装备模板.AttackSpeed != 0)
            {
                dictionary[Stat.AttackSpeed] = 装备模板.AttackSpeed;
            }
            if (装备模板.MagicEvade != 0)
            {
                dictionary[Stat.MagicEvade] = 装备模板.MagicEvade;
            }
            if (装备模板.PhysicalAccuracy != 0)
            {
                dictionary[Stat.PhysicalAccuracy] = 装备模板.PhysicalAccuracy;
            }
            if (装备模板.PhysicalAgility != 0)
            {
                dictionary[Stat.PhysicalAgility] = 装备模板.PhysicalAgility;
            }
            if (装备模板.MinHC != 0)
            {
                dictionary[Stat.MinHC] = 装备模板.MinHC;
            }
            if (装备模板.MaxHC != 0)
            {
                dictionary[Stat.MaxHC] = 装备模板.MaxHC;
            }
            if (装备模板.Luck != 0)
            {
                dictionary[Stat.Luck] = 装备模板.Luck;
            }
            if (装备模板.怪物伤害 != 0)
            {
                dictionary[Stat.怪物伤害] = 装备模板.怪物伤害;
            }
            if (Luck.V != 0)
            {
                dictionary[Stat.Luck] = (dictionary.ContainsKey(Stat.Luck) ? (dictionary[Stat.Luck] + Luck.V) : Luck.V);
            }
            if (升级攻击.V != 0)
            {
                dictionary[Stat.MaxDC] = (dictionary.ContainsKey(Stat.MaxDC) ? (dictionary[Stat.MaxDC] + 升级攻击.V) : 升级攻击.V);
            }
            if (升级魔法.V != 0)
            {
                dictionary[Stat.MaxMC] = (dictionary.ContainsKey(Stat.MaxMC) ? (dictionary[Stat.MaxMC] + 升级魔法.V) : 升级魔法.V);
            }
            if (升级道术.V != 0)
            {
                dictionary[Stat.MaxSC] = (dictionary.ContainsKey(Stat.MaxSC) ? (dictionary[Stat.MaxSC] + 升级道术.V) : 升级道术.V);
            }
            if (升级刺术.V != 0)
            {
                dictionary[Stat.MaxNC] = (dictionary.ContainsKey(Stat.MaxNC) ? (dictionary[Stat.MaxNC] + 升级刺术.V) : 升级刺术.V);
            }
            if (升级弓术.V != 0)
            {
                dictionary[Stat.MaxBC] = (dictionary.ContainsKey(Stat.MaxBC) ? (dictionary[Stat.MaxBC] + 升级弓术.V) : 升级弓术.V);
            }
            foreach (RandomStats item in 随机属性.ToList())
            {
                dictionary[item.Stat] = (dictionary.ContainsKey(item.Stat) ? (dictionary[item.Stat] + item.Value) : item.Value);
            }
            using IEnumerator<GameItem> enumerator2 = 镶嵌灵石.Values.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                switch (enumerator2.Current.ID)
                {
                    case 10320:
                        dictionary[Stat.MaxMC] = ((!dictionary.ContainsKey(Stat.MaxMC)) ? 1 : (dictionary[Stat.MaxMC] + 1));
                        break;
                    case 10321:
                        dictionary[Stat.MaxMC] = (dictionary.ContainsKey(Stat.MaxMC) ? (dictionary[Stat.MaxMC] + 2) : 2);
                        break;
                    case 10322:
                        dictionary[Stat.MaxMC] = (dictionary.ContainsKey(Stat.MaxMC) ? (dictionary[Stat.MaxMC] + 3) : 3);
                        break;
                    case 10323:
                        dictionary[Stat.MaxMC] = (dictionary.ContainsKey(Stat.MaxMC) ? (dictionary[Stat.MaxMC] + 4) : 4);
                        break;
                    case 10324:
                        dictionary[Stat.MaxMC] = (dictionary.ContainsKey(Stat.MaxMC) ? (dictionary[Stat.MaxMC] + 5) : 5);
                        break;
                    case 10220:
                        dictionary[Stat.MaxDef] = ((!dictionary.ContainsKey(Stat.MaxDef)) ? 1 : (dictionary[Stat.MaxDef] + 1));
                        break;
                    case 10221:
                        dictionary[Stat.MaxDef] = (dictionary.ContainsKey(Stat.MaxDef) ? (dictionary[Stat.MaxDef] + 2) : 2);
                        break;
                    case 10222:
                        dictionary[Stat.MaxDef] = (dictionary.ContainsKey(Stat.MaxDef) ? (dictionary[Stat.MaxDef] + 3) : 3);
                        break;
                    case 10223:
                        dictionary[Stat.MaxDef] = (dictionary.ContainsKey(Stat.MaxDef) ? (dictionary[Stat.MaxDef] + 4) : 4);
                        break;
                    case 10224:
                        dictionary[Stat.MaxDef] = (dictionary.ContainsKey(Stat.MaxDef) ? (dictionary[Stat.MaxDef] + 5) : 5);
                        break;
                    case 10110:
                        dictionary[Stat.MaxSC] = ((!dictionary.ContainsKey(Stat.MaxSC)) ? 1 : (dictionary[Stat.MaxSC] + 1));
                        break;
                    case 10111:
                        dictionary[Stat.MaxSC] = (dictionary.ContainsKey(Stat.MaxSC) ? (dictionary[Stat.MaxSC] + 2) : 2);
                        break;
                    case 10112:
                        dictionary[Stat.MaxSC] = (dictionary.ContainsKey(Stat.MaxSC) ? (dictionary[Stat.MaxSC] + 3) : 3);
                        break;
                    case 10113:
                        dictionary[Stat.MaxSC] = (dictionary.ContainsKey(Stat.MaxSC) ? (dictionary[Stat.MaxSC] + 4) : 4);
                        break;
                    case 10114:
                        dictionary[Stat.MaxSC] = (dictionary.ContainsKey(Stat.MaxSC) ? (dictionary[Stat.MaxSC] + 5) : 5);
                        break;
                    case 10120:
                        dictionary[Stat.MaxHP] = (dictionary.ContainsKey(Stat.MaxHP) ? (dictionary[Stat.MaxHP] + 5) : 5);
                        break;
                    case 10121:
                        dictionary[Stat.MaxHP] = (dictionary.ContainsKey(Stat.MaxHP) ? (dictionary[Stat.MaxHP] + 10) : 10);
                        break;
                    case 10122:
                        dictionary[Stat.MaxHP] = (dictionary.ContainsKey(Stat.MaxHP) ? (dictionary[Stat.MaxHP] + 15) : 15);
                        break;
                    case 10123:
                        dictionary[Stat.MaxHP] = (dictionary.ContainsKey(Stat.MaxHP) ? (dictionary[Stat.MaxHP] + 20) : 20);
                        break;
                    case 10124:
                        dictionary[Stat.MaxHP] = (dictionary.ContainsKey(Stat.MaxHP) ? (dictionary[Stat.MaxHP] + 25) : 25);
                        break;
                    case 10520:
                        dictionary[Stat.MaxMCDef] = ((!dictionary.ContainsKey(Stat.MaxMCDef)) ? 1 : (dictionary[Stat.MaxMCDef] + 1));
                        break;
                    case 10521:
                        dictionary[Stat.MaxMCDef] = (dictionary.ContainsKey(Stat.MaxMCDef) ? (dictionary[Stat.MaxMCDef] + 2) : 2);
                        break;
                    case 10522:
                        dictionary[Stat.MaxMCDef] = (dictionary.ContainsKey(Stat.MaxMCDef) ? (dictionary[Stat.MaxMCDef] + 3) : 3);
                        break;
                    case 10523:
                        dictionary[Stat.MaxMCDef] = (dictionary.ContainsKey(Stat.MaxMCDef) ? (dictionary[Stat.MaxMCDef] + 4) : 4);
                        break;
                    case 10524:
                        dictionary[Stat.MaxMCDef] = (dictionary.ContainsKey(Stat.MaxMCDef) ? (dictionary[Stat.MaxMCDef] + 5) : 5);
                        break;
                    case 10420:
                        dictionary[Stat.MaxDC] = ((!dictionary.ContainsKey(Stat.MaxDC)) ? 1 : (dictionary[Stat.MaxDC] + 1));
                        break;
                    case 10421:
                        dictionary[Stat.MaxDC] = (dictionary.ContainsKey(Stat.MaxDC) ? (dictionary[Stat.MaxDC] + 2) : 2);
                        break;
                    case 10422:
                        dictionary[Stat.MaxDC] = (dictionary.ContainsKey(Stat.MaxDC) ? (dictionary[Stat.MaxDC] + 3) : 3);
                        break;
                    case 10423:
                        dictionary[Stat.MaxDC] = (dictionary.ContainsKey(Stat.MaxDC) ? (dictionary[Stat.MaxDC] + 4) : 4);
                        break;
                    case 10424:
                        dictionary[Stat.MaxDC] = (dictionary.ContainsKey(Stat.MaxDC) ? (dictionary[Stat.MaxDC] + 5) : 5);
                        break;
                    case 10720:
                        dictionary[Stat.MaxBC] = ((!dictionary.ContainsKey(Stat.MaxBC)) ? 1 : (dictionary[Stat.MaxBC] + 1));
                        break;
                    case 10721:
                        dictionary[Stat.MaxBC] = (dictionary.ContainsKey(Stat.MaxBC) ? (dictionary[Stat.MaxBC] + 2) : 2);
                        break;
                    case 10722:
                        dictionary[Stat.MaxBC] = (dictionary.ContainsKey(Stat.MaxBC) ? (dictionary[Stat.MaxBC] + 3) : 3);
                        break;
                    case 10723:
                        dictionary[Stat.MaxBC] = (dictionary.ContainsKey(Stat.MaxBC) ? (dictionary[Stat.MaxBC] + 4) : 4);
                        break;
                    case 10724:
                        dictionary[Stat.MaxBC] = (dictionary.ContainsKey(Stat.MaxBC) ? (dictionary[Stat.MaxBC] + 5) : 5);
                        break;
                    case 10620:
                        dictionary[Stat.MaxNC] = ((!dictionary.ContainsKey(Stat.MaxNC)) ? 1 : (dictionary[Stat.MaxNC] + 1));
                        break;
                    case 10621:
                        dictionary[Stat.MaxNC] = (dictionary.ContainsKey(Stat.MaxNC) ? (dictionary[Stat.MaxNC] + 2) : 2);
                        break;
                    case 10622:
                        dictionary[Stat.MaxNC] = (dictionary.ContainsKey(Stat.MaxNC) ? (dictionary[Stat.MaxNC] + 3) : 3);
                        break;
                    case 10623:
                        dictionary[Stat.MaxNC] = (dictionary.ContainsKey(Stat.MaxNC) ? (dictionary[Stat.MaxNC] + 4) : 4);
                        break;
                    case 10624:
                        dictionary[Stat.MaxNC] = (dictionary.ContainsKey(Stat.MaxNC) ? (dictionary[Stat.MaxNC] + 5) : 5);
                        break;
                }
            }
            return dictionary;
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
        物品状态.V = 1;
        MaxDura.V = ((item.PersistType == PersistentItemType.装备) ? (item.MaxDura * 1000) : item.MaxDura);
        Dura.V = ((!random || item.PersistType != PersistentItemType.装备) ? MaxDura.V : SEngine.Random.Next(0, MaxDura.V));
        if (random && item.PersistType == PersistentItemType.装备)
        {
            随机属性.SetValue(GameServer.Template.EquipmentStats.生成属性(base.Type));
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
        if (物品状态.V != 1)
        {
            num2 |= 1;
        }
        else if (随机属性.Count != 0)
        {
            num2 |= 1;
        }
        else if (神圣伤害.V != 0)
        {
            num2 |= 1;
        }
        if (随机属性.Count >= 1)
        {
            num2 |= 2;
        }
        if (随机属性.Count >= 2)
        {
            num2 |= 4;
        }
        if (随机属性.Count >= 3)
        {
            num2 |= 8;
        }
        if (随机属性.Count >= 4)
        {
            num2 |= 0x10;
        }
        if (Luck.V != 0)
        {
            num2 |= 0x800;
        }
        if (升级次数.V != 0)
        {
            num2 |= 0x1000;
        }
        if (孔洞颜色.Count != 0)
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
        if (神圣伤害.V != 0)
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
            writer.Write(物品状态.V);
        }
        if (((uint)num2 & 2u) != 0)
        {
            writer.Write((ushort)随机属性[0].StatID);
        }
        if (((uint)num2 & 4u) != 0)
        {
            writer.Write((ushort)随机属性[1].StatID);
        }
        if (((uint)num2 & 8u) != 0)
        {
            writer.Write((ushort)随机属性[2].StatID);
        }
        if (((uint)num2 & 0x10u) != 0)
        {
            writer.Write((ushort)随机属性[3].StatID);
        }
        if (((uint)num & 0x100u) != 0)
        {
            int num3 = 0;
            if (铭文技能[0] != null)
            {
                num3 |= 1;
            }
            if (铭文技能[1] != null)
            {
                num3 |= 2;
            }
            writer.Write((short)num3);
            writer.Write(洗练数一.V * 10000);
            if (((uint)num3 & (true ? 1u : 0u)) != 0)
            {
                writer.Write(铭文技能[0].Index);
            }
            if (((uint)num3 & 2u) != 0)
            {
                writer.Write(铭文技能[1].Index);
            }
        }
        if (((uint)num & 0x200u) != 0)
        {
            int num4 = 0;
            if (铭文技能[2] != null)
            {
                num4 |= 1;
            }
            if (铭文技能[3] != null)
            {
                num4 |= 2;
            }
            writer.Write((short)num4);
            writer.Write(洗练数二.V * 10000);
            if (((uint)num4 & (true ? 1u : 0u)) != 0)
            {
                writer.Write(铭文技能[2].Index);
            }
            if (((uint)num4 & 2u) != 0)
            {
                writer.Write(铭文技能[3].Index);
            }
        }
        if (((uint)num2 & 0x800u) != 0)
        {
            writer.Write(Luck.V);
        }
        if (((uint)num2 & 0x1000u) != 0)
        {
            writer.Write(升级次数.V);
            writer.Write((byte)0);
            writer.Write(升级攻击.V);
            writer.Write(升级魔法.V);
            writer.Write(升级道术.V);
            writer.Write(升级刺术.V);
            writer.Write(升级弓术.V);
            writer.Write(new byte[3]);
            writer.Write(灵魂绑定.V);
        }
        if (((uint)num2 & 0x2000u) != 0)
        {
            writer.Write(new byte[4]
            {
                (byte)孔洞颜色[0],
                (byte)孔洞颜色[1],
                (byte)孔洞颜色[2],
                (byte)孔洞颜色[3]
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
            writer.Write(神圣伤害.V);
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
