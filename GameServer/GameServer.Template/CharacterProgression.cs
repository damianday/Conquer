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
            [1] = Settings.Default.UpgradeXPLevel1,
            [2] = Settings.Default.UpgradeXPLevel2,
            [3] = Settings.Default.UpgradeXPLevel3,
            [4] = Settings.Default.UpgradeXPLevel4,
            [5] = Settings.Default.UpgradeXPLevel5,
            [6] = Settings.Default.UpgradeXPLevel6,
            [7] = Settings.Default.UpgradeXPLevel7,
            [8] = Settings.Default.UpgradeXPLevel8,
            [9] = Settings.Default.UpgradeXPLevel9,
            [10] = Settings.Default.UpgradeXPLevel10,
            [11] = Settings.Default.UpgradeXPLevel11,
            [12] = Settings.Default.UpgradeXPLevel12,
            [13] = Settings.Default.UpgradeXPLevel13,
            [14] = Settings.Default.UpgradeXPLevel14,
            [15] = Settings.Default.UpgradeXPLevel15,
            [16] = Settings.Default.UpgradeXPLevel16,
            [17] = Settings.Default.UpgradeXPLevel17,
            [18] = Settings.Default.UpgradeXPLevel18,
            [19] = Settings.Default.UpgradeXPLevel19,
            [20] = Settings.Default.UpgradeXPLevel20,
            [21] = Settings.Default.UpgradeXPLevel21,
            [22] = Settings.Default.UpgradeXPLevel22,
            [23] = Settings.Default.UpgradeXPLevel23,
            [24] = Settings.Default.UpgradeXPLevel24,
            [25] = Settings.Default.UpgradeXPLevel25,
            [26] = Settings.Default.UpgradeXPLevel26,
            [27] = Settings.Default.UpgradeXPLevel27,
            [28] = Settings.Default.UpgradeXPLevel28,
            [29] = Settings.Default.UpgradeXPLevel29,
            [30] = Settings.Default.UpgradeXPLevel30,
            [31] = Settings.Default.UpgradeXPLevel31,
            [32] = Settings.Default.UpgradeXPLevel32,
            [33] = Settings.Default.UpgradeXPLevel33,
            [34] = Settings.Default.UpgradeXPLevel34,
            [35] = Settings.Default.UpgradeXPLevel35,
            [36] = Settings.Default.UpgradeXPLevel36,
            [37] = Settings.Default.UpgradeXPLevel37,
            [38] = Settings.Default.UpgradeXPLevel38,
            [39] = Settings.Default.UpgradeXPLevel39,
            [40] = Settings.Default.升级经验模块一,
            [41] = Settings.Default.升级经验模块二,
            [42] = Settings.Default.升级经验模块三,
            [43] = Settings.Default.升级经验模块四,
            [44] = Settings.Default.升级经验模块五,
            [45] = Settings.Default.升级经验模块六,
            [46] = Settings.Default.升级经验模块七,
            [47] = Settings.Default.升级经验模块八,
            [48] = Settings.Default.升级经验模块九,
            [49] = Settings.Default.升级经验模块十,
            [50] = Settings.Default.升级经验模块十一,
            [51] = Settings.Default.升级经验模块十二,
            [52] = Settings.Default.升级经验模块十三,
            [53] = Settings.Default.升级经验模块十四,
            [54] = Settings.Default.升级经验模块十五,
            [55] = Settings.Default.升级经验模块十六,
            [56] = Settings.Default.升级经验模块十七,
            [57] = Settings.Default.升级经验模块十八,
            [58] = Settings.Default.升级经验模块十九,
            [59] = Settings.Default.升级经验模块二十,
            [60] = Settings.Default.升级经验模块二十一,
            [61] = Settings.Default.升级经验模块二十二,
            [62] = Settings.Default.升级经验模块二十二,
            [63] = Settings.Default.升级经验模块二十二,
            [64] = Settings.Default.升级经验模块二十二,
            [65] = Settings.Default.升级经验模块二十二,
            [66] = Settings.Default.升级经验模块二十三,
            [67] = Settings.Default.升级经验模块二十三,
            [68] = Settings.Default.升级经验模块二十三,
            [69] = Settings.Default.升级经验模块二十三,
            [70] = Settings.Default.升级经验模块二十四,
            [71] = Settings.Default.升级经验模块二十四,
            [72] = Settings.Default.升级经验模块二十四,
            [73] = Settings.Default.升级经验模块二十四,
            [74] = Settings.Default.升级经验模块二十四,
            [75] = Settings.Default.升级经验模块二十五,
            [76] = Settings.Default.升级经验模块二十五,
            [77] = Settings.Default.升级经验模块二十五,
            [78] = Settings.Default.升级经验模块二十五,
            [79] = Settings.Default.升级经验模块二十六,
            [80] = Settings.Default.升级经验模块二十六,
            [81] = Settings.Default.升级经验模块二十六,
            [82] = Settings.Default.升级经验模块二十六,
            [83] = Settings.Default.升级经验模块二十六,
            [84] = Settings.Default.升级经验模块二十七,
            [85] = Settings.Default.升级经验模块二十七,
            [86] = Settings.Default.升级经验模块二十七,
            [87] = Settings.Default.升级经验模块二十七,
            [88] = Settings.Default.升级经验模块二十七,
            [89] = Settings.Default.升级经验模块二十八,
            [90] = Settings.Default.升级经验模块二十八,
            [91] = Settings.Default.升级经验模块二十八,
            [92] = Settings.Default.升级经验模块二十九,
            [93] = Settings.Default.升级经验模块二十九,
            [94] = Settings.Default.升级经验模块二十九,
            [95] = Settings.Default.升级经验模块二十九,
            [96] = Settings.Default.升级经验模块三十,
            [97] = Settings.Default.升级经验模块三十,
            [98] = Settings.Default.升级经验模块三十,
            [99] = Settings.Default.升级经验模块三十,
            [100] = Settings.Default.升级经验模块三十
        };

        PetMaxExpTable = new ushort[9]
        {
            Settings.Default.PetUpgradeXPLevel1,
            Settings.Default.PetUpgradeXPLevel2,
            Settings.Default.PetUpgradeXPLevel3,
            Settings.Default.PetUpgradeXPLevel4,
            Settings.Default.PetUpgradeXPLevel5,
            Settings.Default.PetUpgradeXPLevel6,
            Settings.Default.PetUpgradeXPLevel7,
            Settings.Default.PetUpgradeXPLevel8,
            Settings.Default.PetUpgradeXPLevel9
        };

        DataSheet = new Dictionary<int, Stats>();

        if (Settings.Default.DBMethod == 0)
        {
            var path = Settings.Default.GameDataPath + "\\System\\GrowthAttribute.csv";
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

        if (Settings.Default.DBMethod == 1)
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

        if (Settings.Default.DBMethod == 2)
        {
            // TODO: Buggy
            var path = Settings.Default.GameDataPath + "\\System\\GrowthAttribute.csv";
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
