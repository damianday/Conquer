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
            [1] = Settings.UpgradeXPLevel1,
            [2] = Settings.UpgradeXPLevel2,
            [3] = Settings.UpgradeXPLevel3,
            [4] = Settings.UpgradeXPLevel4,
            [5] = Settings.UpgradeXPLevel5,
            [6] = Settings.UpgradeXPLevel6,
            [7] = Settings.UpgradeXPLevel7,
            [8] = Settings.UpgradeXPLevel8,
            [9] = Settings.UpgradeXPLevel9,
            [10] = Settings.UpgradeXPLevel10,
            [11] = Settings.UpgradeXPLevel11,
            [12] = Settings.UpgradeXPLevel12,
            [13] = Settings.UpgradeXPLevel13,
            [14] = Settings.UpgradeXPLevel14,
            [15] = Settings.UpgradeXPLevel15,
            [16] = Settings.UpgradeXPLevel16,
            [17] = Settings.UpgradeXPLevel17,
            [18] = Settings.UpgradeXPLevel18,
            [19] = Settings.UpgradeXPLevel19,
            [20] = Settings.UpgradeXPLevel20,
            [21] = Settings.UpgradeXPLevel21,
            [22] = Settings.UpgradeXPLevel22,
            [23] = Settings.UpgradeXPLevel23,
            [24] = Settings.UpgradeXPLevel24,
            [25] = Settings.UpgradeXPLevel25,
            [26] = Settings.UpgradeXPLevel26,
            [27] = Settings.UpgradeXPLevel27,
            [28] = Settings.UpgradeXPLevel28,
            [29] = Settings.UpgradeXPLevel29,
            [30] = Settings.UpgradeXPLevel30,
            [31] = Settings.UpgradeXPLevel31,
            [32] = Settings.UpgradeXPLevel32,
            [33] = Settings.UpgradeXPLevel33,
            [34] = Settings.UpgradeXPLevel34,
            [35] = Settings.UpgradeXPLevel35,
            [36] = Settings.UpgradeXPLevel36,
            [37] = Settings.UpgradeXPLevel37,
            [38] = Settings.UpgradeXPLevel38,
            [39] = Settings.UpgradeXPLevel39,
            [40] = Settings.升级经验模块一,
            [41] = Settings.升级经验模块二,
            [42] = Settings.升级经验模块三,
            [43] = Settings.升级经验模块四,
            [44] = Settings.升级经验模块五,
            [45] = Settings.升级经验模块六,
            [46] = Settings.升级经验模块七,
            [47] = Settings.升级经验模块八,
            [48] = Settings.升级经验模块九,
            [49] = Settings.升级经验模块十,
            [50] = Settings.升级经验模块十一,
            [51] = Settings.升级经验模块十二,
            [52] = Settings.升级经验模块十三,
            [53] = Settings.升级经验模块十四,
            [54] = Settings.升级经验模块十五,
            [55] = Settings.升级经验模块十六,
            [56] = Settings.升级经验模块十七,
            [57] = Settings.升级经验模块十八,
            [58] = Settings.升级经验模块十九,
            [59] = Settings.升级经验模块二十,
            [60] = Settings.升级经验模块二十一,
            [61] = Settings.升级经验模块二十二,
            [62] = Settings.升级经验模块二十二,
            [63] = Settings.升级经验模块二十二,
            [64] = Settings.升级经验模块二十二,
            [65] = Settings.升级经验模块二十二,
            [66] = Settings.升级经验模块二十三,
            [67] = Settings.升级经验模块二十三,
            [68] = Settings.升级经验模块二十三,
            [69] = Settings.升级经验模块二十三,
            [70] = Settings.升级经验模块二十四,
            [71] = Settings.升级经验模块二十四,
            [72] = Settings.升级经验模块二十四,
            [73] = Settings.升级经验模块二十四,
            [74] = Settings.升级经验模块二十四,
            [75] = Settings.升级经验模块二十五,
            [76] = Settings.升级经验模块二十五,
            [77] = Settings.升级经验模块二十五,
            [78] = Settings.升级经验模块二十五,
            [79] = Settings.升级经验模块二十六,
            [80] = Settings.升级经验模块二十六,
            [81] = Settings.升级经验模块二十六,
            [82] = Settings.升级经验模块二十六,
            [83] = Settings.升级经验模块二十六,
            [84] = Settings.升级经验模块二十七,
            [85] = Settings.升级经验模块二十七,
            [86] = Settings.升级经验模块二十七,
            [87] = Settings.升级经验模块二十七,
            [88] = Settings.升级经验模块二十七,
            [89] = Settings.升级经验模块二十八,
            [90] = Settings.升级经验模块二十八,
            [91] = Settings.升级经验模块二十八,
            [92] = Settings.升级经验模块二十九,
            [93] = Settings.升级经验模块二十九,
            [94] = Settings.升级经验模块二十九,
            [95] = Settings.升级经验模块二十九,
            [96] = Settings.升级经验模块三十,
            [97] = Settings.升级经验模块三十,
            [98] = Settings.升级经验模块三十,
            [99] = Settings.升级经验模块三十,
            [100] = Settings.升级经验模块三十
        };

        PetMaxExpTable = new ushort[9]
        {
            Settings.PetUpgradeXPLevel1,
            Settings.PetUpgradeXPLevel2,
            Settings.PetUpgradeXPLevel3,
            Settings.PetUpgradeXPLevel4,
            Settings.PetUpgradeXPLevel5,
            Settings.PetUpgradeXPLevel6,
            Settings.PetUpgradeXPLevel7,
            Settings.PetUpgradeXPLevel8,
            Settings.PetUpgradeXPLevel9
        };

        DataSheet = new Dictionary<int, Stats>();

        if (Settings.DBMethod == 0)
        {
            var path = Settings.GameDataPath + "\\System\\GrowthAttribute.csv";
            if (!File.Exists(path))
                return;

            var lines = Regex.Split(File.ReadAllText(path).Trim('\r', '\n', '\r'), "\r\n", RegexOptions.IgnoreCase);
            var header = lines[0].Split('\t');
            Dictionary<string, int> dictionary = header.ToDictionary((string K) => K, (string V) => Array.IndexOf(header, V));
            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split('\t');
                if (fields.Length <= 1)
                    continue;

                Stats stats = new Stats();
                GameObjectRace race = Enum.Parse<GameObjectRace>(fields[0]);
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

        if (Settings.DBMethod == 1)
        {
            if (!DBAgent.X.Connected)
                return;

            try
            {
                var qstr = "SELECT * FROM GrowthAttribute";
                using (var connection = DBAgent.X.DB.GetConnection())
                {
                    using var command = DBAgent.X.DB.GetCommand(connection, qstr);

                    using var reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read() == true)
                        {
                            var stats = new Stats();

                            var str = reader.GetString("Race");
                            GameObjectRace race = Enum.Parse<GameObjectRace>(str);

                            int level = reader.GetInt32("Level");
                            int key = (int)race * 256 + level;

                            stats[Stat.MinDef] = reader.GetInt32("MinDef");
                            stats[Stat.MaxDef] = reader.GetInt32("MaxDef");
                            stats[Stat.MinMCDef] = reader.GetInt32("MinMCDef");
                            stats[Stat.MaxMCDef] = reader.GetInt32("MaxMCDef");
                            stats[Stat.MinDC] = reader.GetInt32("MinDC");
                            stats[Stat.MaxDC] = reader.GetInt32("MaxDC");
                            stats[Stat.MinMC] = reader.GetInt32("MinMC");
                            stats[Stat.MaxMC] = reader.GetInt32("MaxMC");
                            stats[Stat.MinSC] = reader.GetInt32("MinSC");
                            stats[Stat.MaxSC] = reader.GetInt32("MaxSC");
                            stats[Stat.MinNC] = reader.GetInt32("MinNC");
                            stats[Stat.MaxNC] = reader.GetInt32("MaxNC");
                            stats[Stat.MinBC] = reader.GetInt32("MinBC");
                            stats[Stat.MaxBC] = reader.GetInt32("MaxBC");
                            stats[Stat.MaxHP] = reader.GetInt32("MaxHP");
                            stats[Stat.MaxMP] = reader.GetInt32("MaxMP");
                            stats[Stat.WalkSpeed] = reader.GetInt32("WalkSpeed");
                            stats[Stat.RunSpeed] = reader.GetInt32("RunSpeed");
                            stats[Stat.PhysicalAccuracy] = reader.GetInt32("PhysicalAccuracy");
                            stats[Stat.PhysicalAgility] = reader.GetInt32("PhysicalAgility");
                            stats[Stat.MagicEvade] = reader.GetInt32("MagicEvade");
                            stats[Stat.CriticalHitRate] = reader.GetInt32("CriticalHitRate");
                            stats[Stat.CriticalDamage] = reader.GetInt32("CriticalDamage");
                            stats[Stat.HPRatePercent] = reader.GetInt32("HPRatePercent");
                            stats[Stat.MPRatePercent] = reader.GetInt32("MPRatePercent");
                            stats[Stat.Luck] = reader.GetInt32("Luck");
                            stats[Stat.AttackSpeed] = reader.GetInt32("AttackSpeed");
                            stats[Stat.HealthRecovery] = reader.GetInt32("HealthRecovery");
                            stats[Stat.ManaRecovery] = reader.GetInt32("ManaRecovery");
                            stats[Stat.PoisonEvade] = reader.GetInt32("PoisonEvade");
                            stats[Stat.最大腕力] = reader.GetInt32("最大腕力");
                            stats[Stat.最大穿戴] = reader.GetInt32("最大穿戴");
                            stats[Stat.最大负重] = reader.GetInt32("最大负重");
                            stats[Stat.MinHC] = reader.GetInt32("MinHC");
                            stats[Stat.MaxHC] = reader.GetInt32("MaxHC");

                            DataSheet.Add(key, stats);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                SMain.AddSystemLog(err.ToString());
            }
        }

        if (Settings.DBMethod == 2)
        {
            // TODO: Buggy
            var path = Settings.GameDataPath + "\\System\\GrowthAttribute.csv";
            if (!File.Exists(path))
                return;

            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
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

                    GameObjectRace race = Enum.Parse<GameObjectRace>(csvReader.GetField<string>(0));
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
            }
        }
    }

    public static Stats GetData(GameObjectRace job, byte level)
    {
        var key = (byte)job * 256 + level;
        return DataSheet.TryGetValue(key, out var value) ? value : null;
    }
}
