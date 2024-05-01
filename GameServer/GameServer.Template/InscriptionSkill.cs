using System.Collections.Generic;
using System.IO;

namespace GameServer.Template;

public sealed class InscriptionSkill
{
    public static Dictionary<ushort, InscriptionSkill> DataSheet;
    private static Dictionary<byte, List<InscriptionSkill>> m_ProbabilityTable;

    public string SkillName;
    public GameObjectRace Race;
    public ushort SkillID;
    public byte ID;
    public byte SkillCount;
    public ushort PeriodCount;
    public bool PassiveSkill;
    public byte Quality;
    public int Probability;
    public bool BroadcastNotification;
    public string Description;
    public byte[] MinPlayerLevel;
    public int[] MinSkillExp;
    public int[] SkillCombatBonus;
    public InscriptionStat[] StatsBonus;
    public List<ushort> ComesWithBuff;
    public List<ushort> PassiveSkills;
    public List<string> MainSkills;
    public List<string> SwitchSkills;
    public GameObjectState 角色所处状态;
    public bool RemoveOnDie;

    private Stats[] m_StatsBonus;
    public ushort Index => (ushort)(SkillID * 10 + ID);

    public Stats[] StatBonusTable
    {
        get
        {
            if (m_StatsBonus != null)
                return m_StatsBonus;

            m_StatsBonus =
            [
                new Stats(),
                new Stats(),
                new Stats(),
                new Stats()
            ];
            if (StatsBonus != null)
            {
                foreach (var stat in StatsBonus)
                {
                    m_StatsBonus[0][stat.Stat] = stat.Level0;
                    m_StatsBonus[1][stat.Stat] = stat.Level1;
                    m_StatsBonus[2][stat.Stat] = stat.Level2;
                    m_StatsBonus[3][stat.Stat] = stat.Level3;
                }
            }
            return m_StatsBonus;
        }
    }

    public static InscriptionSkill RandomRefinement(GameObjectRace race)
    {
        if (m_ProbabilityTable.TryGetValue((byte)race, out var value) && value.Count > 0)
            return value[SEngine.Random.Next(value.Count)];
        return null;
    }

    public static void LoadData()
    {
        DataSheet = new Dictionary<ushort, InscriptionSkill>();

        var path = Settings.Default.GameDataPath + "\\System\\Skills\\Inscriptions\\";
        if (Directory.Exists(path))
        {
            var array = Serializer.Deserialize<InscriptionSkill>(path);
            foreach (var obj in array)
                DataSheet.Add(obj.Index, obj);
        }
        m_ProbabilityTable = new Dictionary<byte, List<InscriptionSkill>>
        {
            [0] = new List<InscriptionSkill>(),
            [1] = new List<InscriptionSkill>(),
            [2] = new List<InscriptionSkill>(),
            [3] = new List<InscriptionSkill>(),
            [4] = new List<InscriptionSkill>(),
            [5] = new List<InscriptionSkill>()
        };
        foreach (var skill in DataSheet.Values)
        {
            if (skill.ID != 0)
            {
                for (var i = 0; i < skill.Probability; i++)
                    m_ProbabilityTable[(byte)skill.Race].Add(skill);
            }
        }
        foreach (List<InscriptionSkill> list in m_ProbabilityTable.Values)
        {
            for (int i = 0; i < list.Count; i++)
            {
                InscriptionSkill skill = list[i];
                int index = SEngine.Random.Next(list.Count);
                list[i] = list[index];
                list[index] = skill;
            }
        }
    }
}
