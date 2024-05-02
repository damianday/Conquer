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

    static CharacterProgression()
    {
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
