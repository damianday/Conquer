using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameServer.Template;

public sealed class MonsterInfo
{
    public static Dictionary<string, MonsterInfo> DataSheet;

    public string MonsterName;
    public ushort ID;
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
    public GrowthStat[] Grows;
    public InheritStat[] InheritsStats;
    
    public List<MonsterDrop> Drops = new List<MonsterDrop>();
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
                            monster.BaseTemptationProbability = reader.GetFloat("BaseTemptationProbability");
                            monster.MoveInterval = reader.GetUInt16("MoveInterval", 350);
                            monster.RoamInterval = reader.GetUInt16("RoamInterval");
                            monster.CorpsePreservationDuration = reader.GetUInt16("CorpsePreservationDuration");
                            monster.ActiveAttackTarget = reader.GetBoolean("ActiveAttackTarget");
                            monster.RangeHate = reader.GetByte("RangeHate");
                            monster.HateTime = reader.GetUInt16("HateTime");
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

                            DataSheet.Add(monster.MonsterName, monster);
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
