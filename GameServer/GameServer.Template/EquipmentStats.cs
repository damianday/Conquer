using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameServer.Template;

public sealed class EquipmentStats
{
    public class StatsDetail
    {
        public int ID;              // 属性编号
        public int Probability;     // 属性概率
    }

    public static Dictionary<byte, EquipmentStats> DataSheet;
    public static Dictionary<byte, RandomStats[]> ProbabilityTable;

    public ItemType ItemType;
    public float MaxProbability;
    public int MinRate;
    public int MaxRate;
    public StatsDetail[] Stats;

    public static List<RandomStats> 生成属性(ItemType 部位, bool 重铸装备 = false)
    {
        if (DataSheet.TryGetValue((byte)部位, out var value) && ProbabilityTable.TryGetValue((byte)部位, out var value2) && value2.Length != 0 && (重铸装备 || Compute.CalculateProbability(value.MaxProbability)))
        {
            int num = SEngine.Random.Next(100);
            Dictionary<Stat, RandomStats> dictionary = new Dictionary<Stat, RandomStats>();
            int num2 = ((num < value.MinRate) ? 1 : ((num < value.MaxRate) ? 2 : 3));
            for (int i = 0; i < num2; i++)
            {
                RandomStats 随机属性2 = value2[SEngine.Random.Next(value2.Length)];
                if (!dictionary.ContainsKey(随机属性2.Stat) && value.ItemType == ItemType.Necklace && Settings.Default.幸运保底开关)
                {
                    dictionary[随机属性2.Stat] = 随机属性2;
                    continue;
                }
                if (!dictionary.ContainsKey(随机属性2.Stat) && !Settings.Default.幸运保底开关)
                {
                    dictionary[随机属性2.Stat] = 随机属性2;
                    continue;
                }
                RandomStats 随机属性3 = value2[SEngine.Random.Next(value2.Length)];
                if (!dictionary.ContainsKey(随机属性3.Stat) && 随机属性3.Stat != Stat.Luck)
                {
                    dictionary[随机属性3.Stat] = 随机属性3;
                    continue;
                }
                RandomStats 随机属性4 = value2[SEngine.Random.Next(value2.Length)];
                if (!dictionary.ContainsKey(随机属性4.Stat) && 随机属性4.Stat != Stat.Luck)
                {
                    dictionary[随机属性4.Stat] = 随机属性4;
                    continue;
                }
                RandomStats 随机属性5 = value2[SEngine.Random.Next(value2.Length)];
                if (!dictionary.ContainsKey(随机属性5.Stat) && 随机属性5.Stat != Stat.Luck)
                {
                    dictionary[随机属性5.Stat] = 随机属性5;
                    continue;
                }
                RandomStats 随机属性6 = value2[SEngine.Random.Next(value2.Length)];
                if (!dictionary.ContainsKey(随机属性6.Stat) && 随机属性6.Stat != Stat.Luck)
                {
                    dictionary[随机属性6.Stat] = 随机属性6;
                    continue;
                }
                RandomStats 随机属性7 = value2[SEngine.Random.Next(value2.Length)];
                if (!dictionary.ContainsKey(随机属性7.Stat) && 随机属性7.Stat != Stat.Luck)
                {
                    dictionary[随机属性7.Stat] = 随机属性7;
                    continue;
                }
                RandomStats 随机属性8 = value2[SEngine.Random.Next(value2.Length)];
                if (!dictionary.ContainsKey(随机属性8.Stat) && 随机属性8.Stat != Stat.Luck)
                {
                    dictionary[随机属性8.Stat] = 随机属性8;
                    continue;
                }
                RandomStats 随机属性9 = value2[SEngine.Random.Next(value2.Length)];
                if (!dictionary.ContainsKey(随机属性9.Stat) && 随机属性9.Stat != Stat.Luck)
                {
                    dictionary[随机属性9.Stat] = 随机属性9;
                    continue;
                }
                RandomStats 随机属性10 = value2[SEngine.Random.Next(value2.Length)];
                if (!dictionary.ContainsKey(随机属性10.Stat) && 随机属性10.Stat != Stat.Luck)
                {
                    dictionary[随机属性10.Stat] = 随机属性10;
                }
            }
            return dictionary.Values.ToList();
        }
        return new List<RandomStats>();
    }

    public static List<RandomStats> 生成属性1(ItemType 部位, bool 重铸装备 = false)
    {
        if (DataSheet.TryGetValue((byte)部位, out var value) && 
            ProbabilityTable.TryGetValue((byte)部位, out var value2) && 
            value2.Length != 0 && 
            (重铸装备 || Compute.CalculateProbability(value.MaxProbability)) && 
            value.ItemType == ItemType.Necklace && Settings.Default.幸运保底开关)
        {
            int num = SEngine.Random.Next(100);
            Dictionary<Stat, RandomStats> dictionary = new Dictionary<Stat, RandomStats>();
            int num2 = ((num < value.MinRate) ? 1 : ((num < value.MaxRate) ? 2 : 3));
            for (int i = 0; i < num2; i++)
            {
                RandomStats 随机属性2 = value2[SEngine.Random.Next(value2.Length)];
                if (!dictionary.ContainsKey(随机属性2.Stat))
                {
                    if (Settings.Default.幸运洗练点数 == 1)
                    {
                        随机属性2.Value = Settings.Default.幸运洗练点数;
                        随机属性2.Stat = Stat.Luck;
                        随机属性2.StatID = 32001;
                        随机属性2.StatDescription = "幸运等级+1";
                    }
                    if (Settings.Default.幸运洗练点数 == 2)
                    {
                        随机属性2.Value = Settings.Default.幸运洗练点数;
                        随机属性2.Stat = Stat.Luck;
                        随机属性2.StatID = 32041;
                        随机属性2.StatDescription = "幸运等级+2";
                    }
                    if (Settings.Default.幸运洗练点数 == 3)
                    {
                        随机属性2.Value = Settings.Default.幸运洗练点数;
                        随机属性2.Stat = Stat.Luck;
                        随机属性2.StatID = 32042;
                        随机属性2.StatDescription = "幸运等级+3";
                    }
                    dictionary[随机属性2.Stat] = 随机属性2;
                }
            }
            return dictionary.Values.ToList();
        }
        return new List<RandomStats>();
    }

    public static void LoadData()
    {
        DataSheet = new Dictionary<byte, EquipmentStats>();

        var path = Settings.Default.GameDataPath + "\\System\\Items\\EquipmentStats\\";
        if (Directory.Exists(path))
        {
            var array = Serializer.Deserialize<EquipmentStats>(path);
            foreach (var obj in array)
                DataSheet.Add((byte)obj.ItemType, obj);
        }

        ProbabilityTable = new Dictionary<byte, RandomStats[]>();
        foreach (KeyValuePair<byte, EquipmentStats> item in DataSheet)
        {
            List<RandomStats> list = new List<RandomStats>();
            StatsDetail[] array4 = item.Value.Stats;
            foreach (StatsDetail stat in array4)
            {
                if (RandomStats.DataSheet.TryGetValue(stat.ID, out var value))
                {
                    for (int k = 0; k < stat.Probability; k++)
                    {
                        list.Add(value);
                    }
                }
            }
            ProbabilityTable[item.Key] = list.ToArray();
        }
    }
}
