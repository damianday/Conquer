using System.Collections.Generic;
using System.IO;

namespace GameServer.Template;

public struct BasicStat
{
    public Stat Stat;       // 属性
    public int Value;       // 数值
}

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
    public float BaseTemptationProbability;
    public ushort MoveInterval;
    public ushort RoamInterval;
    public ushort CorpsePreservationDuration;
    public bool ActiveAttackTarget;
    public byte RangeHate;
    public ushort HateTime;
    public string NormalAttackSkills;
    public string[] RandomTriggerSkills;
    public string EnterCombatSkills;
    public string ExitCombatSkills;
    public string MoveReleaseSkill;
    public string BirthReleaseSkill;
    public string DeathReleaseSkill;
    public BasicStat[] Stats;
    public GrowthStat[] Grows;
    public InheritStat[] InheritsStats;
    public ushort ProvideExperience;
    public List<MonsterDrop> Drops;
    public Dictionary<GameItem, long> DropStats = new Dictionary<GameItem, long>();

    private Stats m_BasicStats;
    public Stats BasicStats
    {
        get
        {
            if (m_BasicStats != null)
                return m_BasicStats;

            m_BasicStats = new Stats();
            if (Stats != null)
            {
                foreach (var stat in Stats)
                {
                    m_BasicStats[stat.Stat] = stat.Value;
                }
            }
            return m_BasicStats;
        }
    }

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

        var path = Config.GameDataPath + "\\System\\Npc\\Monsters\\";
        if (!Directory.Exists(path))
            return;

        var array = Serializer.Deserialize<MonsterInfo>(path);
        foreach (var obj in array)
            DataSheet.Add(obj.MonsterName, obj);
    }
}
