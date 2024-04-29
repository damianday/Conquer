using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameServer.Template;

public struct GrowthStat
{
    public Stat Stat;       // 属性
    public int Level0;      // 零级
    public int Level1;      // 一级
    public int Level2;      // 二级
    public int Level3;      // 三级
    public int Level4;      // 四级
    public int Level5;      // 五级
    public int Level6;      // 六级
    public int Level7;      // 七级
}

public struct InheritStat
{
    public Stat InheritedStat;  // 继承属性
    public Stat ConvertStat;    // 转换属性
    public float Ratio;         // 继承比例
}

public sealed class MonsterInfo
{
    public static Dictionary<string, MonsterInfo> DataSheet;

    public string MonsterName;
    public ushort ID;
    public ushort GroupID;
    public ushort DropGroupID;
    public byte Level;
    public ObjectSize Size;
    public MonsterRaceType Race;
    public MonsterGradeType Grade;
    public bool ForbbidenMove;
    public bool OutWarAutomaticPetrochemical;
    public ushort PetrochemicalStatusID;
    public bool VisibleStealthTargets;
    public bool CanBeDrivenBySkills;
    public bool CanBeControlledBySkills;
    public bool CanBeSeducedBySkills;
    public bool CanBeSummonedBySkills;
    public bool CanBeHit;
    public float BaseTemptationProbability;
    public ushort MoveInterval;
    public ushort RoamInterval;
    public ushort CorpsePreservationDuration;
    public bool ActiveAttackTarget;
    public byte RangeHate;
    public ushort HateTime;
    public bool Blocking;
    public ushort ProvideExperience;
    public ushort DeathMasterBuff;
    public bool HatredLocksOnMaster;

    public string NormalAttackSkills;
    public string[] RandomTriggerSkills;
    public string EnterCombatSkills;
    public string ExitCombatSkills;
    public string MoveReleaseSkill;
    public string BirthReleaseSkill;
    public string DeathReleaseSkill;
    public string BerserkReleaseSkill;

    public Stats Stats = new Stats();
    public List<GrowthStat> Grows;
    public List<InheritStat> InheritedStats;
    
    public List<MonItemInfo> Drops = new List<MonItemInfo>();
    public Dictionary<GameItem, long> DropStats = new Dictionary<GameItem, long>();


    private Stats[] m_GrowStats;
    public Stats[] GrowStats
    {
        get
        {
            if (m_GrowStats != null)
                return m_GrowStats;

            m_GrowStats =
            [
                new Stats(),
                new Stats(),
                new Stats(),
                new Stats(),
                new Stats(),
                new Stats(),
                new Stats(),
                new Stats()
            ];
            if (Grows != null)
            {
                foreach (var grow in Grows)
                {
                    m_GrowStats[0][grow.Stat] = grow.Level0;
                    m_GrowStats[1][grow.Stat] = grow.Level1;
                    m_GrowStats[2][grow.Stat] = grow.Level2;
                    m_GrowStats[3][grow.Stat] = grow.Level3;
                    m_GrowStats[4][grow.Stat] = grow.Level4;
                    m_GrowStats[5][grow.Stat] = grow.Level5;
                    m_GrowStats[6][grow.Stat] = grow.Level6;
                    m_GrowStats[7][grow.Stat] = grow.Level7;
                }
            }
            return m_GrowStats;
        }
    }

    private static int LoadMonItems(ushort id, List<MonItemInfo> items)
    {
        var error = -1;

        try
        {
            var qstr = "SELECT * FROM MonItem WHERE GroupID=@GroupID";

            using var connection = DBAgent.X.DB.GetConnection();
            using var command = connection.CreateCommand();
            command.CommandText = qstr;
            command.Parameters.AddWithValue("@GroupID", id);

            using var reader = command.ExecuteReader();

            if (reader != null)
            {
                error = 0;
                while (reader.Read() == true)
                {
                    var mi = new MonItemInfo();
                    //mi.ItemID = reader.GetInt32("ItemID");
                    mi.Name = reader.GetString("ItemName");
                    mi.SelPoint = reader.GetInt32("SelPoint");
                    mi.MaxPoint = reader.GetInt32("MaxPoint");
                    mi.MinAmount = reader.GetInt32("MinAmount");
                    mi.MaxAmount = reader.GetInt32("MaxAmount");
                    mi.DropSet = reader.GetInt32("DropSet");

                    items.Add(mi);
                    error++;
                }
            }
        }
        catch (Exception err)
        {
            SMain.AddSystemLog(err.ToString());
            error = -2;
        }
        return error;
    }

    public static void LoadData()
    {
        DataSheet = new Dictionary<string, MonsterInfo>();

        if (Config.DBMethod == 0)
        {
            var path = Config.GameDataPath + "\\System\\Npc\\Monsters\\";
            if (!Directory.Exists(path))
                return;

            var array = Serializer.Deserialize<MonsterInfo>(path);
            foreach (var obj in array)
                DataSheet.Add(obj.MonsterName, obj);
        }

        if (Config.DBMethod == 1)
        {
            if (!DBAgent.X.Connected)
                return;

            try
            {
                var qstr = "SELECT * FROM Monsters";
                using (var connection = DBAgent.X.DB.GetConnection())
                {
                    using var command = DBAgent.X.DB.GetCommand(connection, qstr);

                    using var reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read() == true)
                        {
                            var monster = new MonsterInfo();
                            monster.MonsterName = reader.GetString("MonsterName");
                            monster.ID = reader.GetUInt16("ID");
                            monster.GroupID = reader.GetUInt16("GroupID");
                            monster.DropGroupID = reader.GetUInt16("DropGroupID");
                            monster.Level = reader.GetByte("Level");
                            monster.Size = (ObjectSize)reader.GetInt32("Size");
                            monster.Race = (MonsterRaceType)reader.GetInt32("Race");
                            monster.Grade = (MonsterGradeType)reader.GetInt32("Grade");
                            monster.ForbbidenMove = reader.GetBoolean("ForbbidenMove");
                            monster.OutWarAutomaticPetrochemical = reader.GetBoolean("OutWarAutomaticPetrochemical");
                            monster.PetrochemicalStatusID = reader.GetUInt16("PetrochemicalStatusID");
                            monster.VisibleStealthTargets = reader.GetBoolean("VisibleStealthTargets");
                            monster.CanBeDrivenBySkills = reader.GetBoolean("CanBeDrivenBySkills");
                            monster.CanBeControlledBySkills = reader.GetBoolean("CanBeControlledBySkills");
                            monster.CanBeSeducedBySkills = reader.GetBoolean("CanBeSeducedBySkills");
                            monster.CanBeSummonedBySkills = reader.GetBoolean("CanBeSummonedBySkills");
                            monster.CanBeHit = reader.GetBoolean("CanBeHit");
                            monster.BaseTemptationProbability = reader.GetFloat("BaseTemptationProbability", 0F);
                            monster.MoveInterval = reader.GetUInt16("MoveInterval", 350);
                            monster.RoamInterval = reader.GetUInt16("RoamInterval", 120);
                            monster.CorpsePreservationDuration = reader.GetUInt16("CorpsePreservationDuration", 15_000);
                            monster.ActiveAttackTarget = reader.GetBoolean("ActiveAttackTarget", monster.Grade >= MonsterGradeType.Normal);
                            monster.RangeHate = reader.GetByte("RangeHate", 5);
                            monster.HateTime = reader.GetUInt16("HateTime", 15_000);
                            monster.Blocking = reader.GetBoolean("Blocking");
                            monster.ProvideExperience = reader.GetUInt16("ProvideExperience");
                            monster.DeathMasterBuff = reader.GetUInt16("DeathMasterBuff");
                            monster.HatredLocksOnMaster = reader.GetBoolean("HatredLocksOnMaster");

                            monster.NormalAttackSkills = reader.GetString("NormalAttackSkills");
                            monster.RandomTriggerSkills = reader.GetString("RandomTriggerSkills")?.Split(',').Select(x => x.Trim((char)39)).ToArray();
                            monster.EnterCombatSkills = reader.GetString("EnterCombatSkills");
                            monster.ExitCombatSkills = reader.GetString("ExitCombatSkills");
                            monster.MoveReleaseSkill = reader.GetString("MoveReleaseSkill");
                            monster.BirthReleaseSkill = reader.GetString("BirthReleaseSkill");
                            monster.DeathReleaseSkill = reader.GetString("DeathReleaseSkill");
                            monster.BerserkReleaseSkill = reader.GetString("BerserkReleaseSkill");

                            monster.Stats[Stat.MinDef] = reader.GetInt32("MinDef");
                            monster.Stats[Stat.MaxDef] = reader.GetInt32("MaxDef");
                            monster.Stats[Stat.MinMCDef] = reader.GetInt32("MinMCDef");
                            monster.Stats[Stat.MaxMCDef] = reader.GetInt32("MaxMCDef");
                            monster.Stats[Stat.MinDC] = reader.GetInt32("MinDC");
                            monster.Stats[Stat.MaxDC] = reader.GetInt32("MaxDC");
                            monster.Stats[Stat.MinMC] = reader.GetInt32("MinMC");
                            monster.Stats[Stat.MaxMC] = reader.GetInt32("MaxMC");
                            monster.Stats[Stat.MaxHP] = reader.GetInt32("MaxHP");
                            monster.Stats[Stat.WalkSpeed] = reader.GetInt32("WalkSpeed");
                            monster.Stats[Stat.AttackSpeed] = reader.GetInt32("AttackSpeed");
                            monster.Stats[Stat.HealthRecovery] = reader.GetInt32("HealthRecovery");
                            monster.Stats[Stat.PhysicalAccuracy] = reader.GetInt32("PhysicalAccuracy");
                            monster.Stats[Stat.PhysicalAgility] = reader.GetInt32("PhysicalAgility");
                            monster.Stats[Stat.MagicEvade] = reader.GetInt32("MagicEvade");

                            LoadMonItems(monster.DropGroupID, monster.Drops);

                            DataSheet.Add(monster.MonsterName, monster);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                SMain.AddSystemLog(err.ToString());
                return;
            }


            foreach (var monster in DataSheet)
            {
                try
                {
                    var qstr = "SELECT * FROM MonsterGrowthAttribute WHERE MonsterID = @MonsterID";
                    using (var connection = DBAgent.X.DB.GetConnection())
                    {
                        using var command = DBAgent.X.DB.GetCommand(connection, qstr);
                        command.Parameters.AddWithValue("@MonsterID", monster.Value.ID);

                        using var reader = command.ExecuteReader();
                        if (reader != null)
                        {
                            monster.Value.Grows = new List<GrowthStat>();
                            while (reader.Read() == true)
                            {
                                var grow = new GrowthStat();
                                grow.Stat = (Stat)reader.GetUInt16("Stat");
                                grow.Level0 = reader.GetInt32("Level0");
                                grow.Level1 = reader.GetInt32("Level1");
                                grow.Level2 = reader.GetInt32("Level2");
                                grow.Level3 = reader.GetInt32("Level3");
                                grow.Level4 = reader.GetInt32("Level4");
                                grow.Level5 = reader.GetInt32("Level5");
                                grow.Level6 = reader.GetInt32("Level6");
                                grow.Level7 = reader.GetInt32("Level7");

                                monster.Value.Grows.Add(grow);
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    SMain.AddSystemLog(err.ToString());
                }

                try
                {
                    var qstr = "SELECT * FROM MonsterInheritedAttribute WHERE MonsterID = @MonsterID";
                    using (var connection = DBAgent.X.DB.GetConnection())
                    {
                        using var command = DBAgent.X.DB.GetCommand(connection, qstr);
                        command.Parameters.AddWithValue("@MonsterID", monster.Value.ID);

                        using var reader = command.ExecuteReader();
                        if (reader != null)
                        {
                            monster.Value.InheritedStats = new List<InheritStat>(); 
                            while (reader.Read() == true)
                            {
                                var stat = new InheritStat();
                                stat.InheritedStat = (Stat)reader.GetUInt16("InheritedStat");
                                stat.ConvertStat = (Stat)reader.GetUInt16("ConvertStat");
                                stat.Ratio = reader.GetFloat("Ratio");

                                monster.Value.InheritedStats.Add(stat);
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    SMain.AddSystemLog(err.ToString());
                }
            }
        }
    }
}
