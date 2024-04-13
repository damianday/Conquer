using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using CsvHelper;
using CsvHelper.Configuration;

namespace GameServer.Template;

public sealed class CharacterProgression
{
    public static Dictionary<int, Stats> DataSheet;

    public static readonly Dictionary<byte, int> MaxExpTable;
    public static readonly ushort[] PetMaxExpTable;

    static CharacterProgression()
    {
        MaxExpTable = new Dictionary<byte, int>
        {
            [1] = Config.UpgradeXPLevel1,
            [2] = Config.UpgradeXPLevel2,
            [3] = Config.UpgradeXPLevel3,
            [4] = Config.UpgradeXPLevel4,
            [5] = Config.UpgradeXPLevel5,
            [6] = Config.UpgradeXPLevel6,
            [7] = Config.UpgradeXPLevel7,
            [8] = Config.UpgradeXPLevel8,
            [9] = Config.UpgradeXPLevel9,
            [10] = Config.UpgradeXPLevel10,
            [11] = Config.UpgradeXPLevel11,
            [12] = Config.UpgradeXPLevel12,
            [13] = Config.UpgradeXPLevel13,
            [14] = Config.UpgradeXPLevel14,
            [15] = Config.UpgradeXPLevel15,
            [16] = Config.UpgradeXPLevel16,
            [17] = Config.UpgradeXPLevel17,
            [18] = Config.UpgradeXPLevel18,
            [19] = Config.UpgradeXPLevel19,
            [20] = Config.UpgradeXPLevel20,
            [21] = Config.UpgradeXPLevel21,
            [22] = Config.UpgradeXPLevel22,
            [23] = Config.UpgradeXPLevel23,
            [24] = Config.UpgradeXPLevel24,
            [25] = Config.UpgradeXPLevel25,
            [26] = Config.UpgradeXPLevel26,
            [27] = Config.UpgradeXPLevel27,
            [28] = Config.UpgradeXPLevel28,
            [29] = Config.UpgradeXPLevel29,
            [30] = Config.UpgradeXPLevel30,
            [31] = Config.UpgradeXPLevel31,
            [32] = Config.UpgradeXPLevel32,
            [33] = Config.UpgradeXPLevel33,
            [34] = Config.UpgradeXPLevel34,
            [35] = Config.UpgradeXPLevel35,
            [36] = Config.UpgradeXPLevel36,
            [37] = Config.UpgradeXPLevel37,
            [38] = Config.UpgradeXPLevel38,
            [39] = Config.UpgradeXPLevel39,
            [40] = Config.升级经验模块一,
            [41] = Config.升级经验模块二,
            [42] = Config.升级经验模块三,
            [43] = Config.升级经验模块四,
            [44] = Config.升级经验模块五,
            [45] = Config.升级经验模块六,
            [46] = Config.升级经验模块七,
            [47] = Config.升级经验模块八,
            [48] = Config.升级经验模块九,
            [49] = Config.升级经验模块十,
            [50] = Config.升级经验模块十一,
            [51] = Config.升级经验模块十二,
            [52] = Config.升级经验模块十三,
            [53] = Config.升级经验模块十四,
            [54] = Config.升级经验模块十五,
            [55] = Config.升级经验模块十六,
            [56] = Config.升级经验模块十七,
            [57] = Config.升级经验模块十八,
            [58] = Config.升级经验模块十九,
            [59] = Config.升级经验模块二十,
            [60] = Config.升级经验模块二十一,
            [61] = Config.升级经验模块二十二,
            [62] = Config.升级经验模块二十二,
            [63] = Config.升级经验模块二十二,
            [64] = Config.升级经验模块二十二,
            [65] = Config.升级经验模块二十二,
            [66] = Config.升级经验模块二十三,
            [67] = Config.升级经验模块二十三,
            [68] = Config.升级经验模块二十三,
            [69] = Config.升级经验模块二十三,
            [70] = Config.升级经验模块二十四,
            [71] = Config.升级经验模块二十四,
            [72] = Config.升级经验模块二十四,
            [73] = Config.升级经验模块二十四,
            [74] = Config.升级经验模块二十四,
            [75] = Config.升级经验模块二十五,
            [76] = Config.升级经验模块二十五,
            [77] = Config.升级经验模块二十五,
            [78] = Config.升级经验模块二十五,
            [79] = Config.升级经验模块二十六,
            [80] = Config.升级经验模块二十六,
            [81] = Config.升级经验模块二十六,
            [82] = Config.升级经验模块二十六,
            [83] = Config.升级经验模块二十六,
            [84] = Config.升级经验模块二十七,
            [85] = Config.升级经验模块二十七,
            [86] = Config.升级经验模块二十七,
            [87] = Config.升级经验模块二十七,
            [88] = Config.升级经验模块二十七,
            [89] = Config.升级经验模块二十八,
            [90] = Config.升级经验模块二十八,
            [91] = Config.升级经验模块二十八,
            [92] = Config.升级经验模块二十九,
            [93] = Config.升级经验模块二十九,
            [94] = Config.升级经验模块二十九,
            [95] = Config.升级经验模块二十九,
            [96] = Config.升级经验模块三十,
            [97] = Config.升级经验模块三十,
            [98] = Config.升级经验模块三十,
            [99] = Config.升级经验模块三十,
            [100] = Config.升级经验模块三十
        };
        PetMaxExpTable = new ushort[9]
        {
            Config.PetUpgradeXPLevel1,
            Config.PetUpgradeXPLevel2,
            Config.PetUpgradeXPLevel3,
            Config.PetUpgradeXPLevel4,
            Config.PetUpgradeXPLevel5,
            Config.PetUpgradeXPLevel6,
            Config.PetUpgradeXPLevel7,
            Config.PetUpgradeXPLevel8,
            Config.PetUpgradeXPLevel9
        };

        DataSheet = new Dictionary<int, Stats>();

        var path = Config.GameDataPath + "\\System\\GrowthAttribute.csv";
        if (!File.Exists(path))
            return;

        /*var config = new CsvConfiguration(CultureInfo.InvariantCulture);
        config.HasHeaderRecord = true;
        config.Delimiter = "\t";
        using var reader = new StreamReader(path, Encoding.GetEncoding("GB18030"));
        using var csvReader = new CsvReader(reader, config);
        
        csvReader.Read();
        csvReader.ReadHeader();
        try
        {
            while (csvReader.Read())
            {
                var stats = new Stats();

                GameObjectRace race = (GameObjectRace)Enum.Parse(typeof(GameObjectRace), csvReader.GetField<string>(0));
                int level = csvReader.GetField<int>(1);
                int key = (int)race * 256 + level;
                for (int j = 2; j < csvReader.ColumnCount; j++)
                {
                    var statName = csvReader.HeaderRecord[j];

                    if (Enum.TryParse<Stat>(statName, out var stat) && Enum.IsDefined(typeof(Stat), stat))
                        stats[stat] = csvReader.GetField<int>(j);
                    else
                        throw new FormatException($"Unknown stat found {statName}.");
                }

                DataSheet.Add(key, stats);
            }
        }
        catch (Exception ex)
        {
            SEngine.AddSystemLog(ex.Message);
        }*/

        var lines = Regex.Split(File.ReadAllText(path).Trim('\r', '\n', '\r'), "\r\n", RegexOptions.IgnoreCase);
        var header = lines[0].Split('\t');
        Dictionary<string, int> dictionary = header.ToDictionary((string K) => K, (string V) => Array.IndexOf(header, V));
        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split('\t');
            if (fields.Length <= 1)
                continue;

            Stats stats = new Stats();
            GameObjectRace race = (GameObjectRace)Enum.Parse(typeof(GameObjectRace), fields[0]);
            int level = Convert.ToInt32(fields[1]);
            int key = (int)race * 256 + level;
            for (int j = 2; j < header.Length; j++)
            {
                if (Enum.TryParse<Stat>((string)((object[])header)[j], out var stat) && Enum.IsDefined(typeof(Stat), stat))
                {
                    stats[stat] = Convert.ToInt32(fields[dictionary[stat.ToString()]]);
                }
            }
            DataSheet.Add(key, stats);
        }
    }

    public static Stats GetData(GameObjectRace job, byte level)
    {
        return DataSheet[(byte)job * 256 + level];
    }
}
