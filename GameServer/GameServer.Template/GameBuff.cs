using System.Collections.Generic;
using System.IO;

namespace GameServer.Template;

public sealed class GameBuff
{
    public static Dictionary<ushort, GameBuff> DataSheet = new Dictionary<ushort, GameBuff>();

    public string Name;
    public ushort ID;
    public ushort GroupID;
    public BuffActionType ActionType;
    public BuffStackType StackingType;
    public BuffEffectType Effect;
    public bool SyncClient;
    public bool RemoveOnExpire;
    public bool OnChangeMapRemove;
    public bool OnChangeWeaponRemove;
    public bool OnPlayerDiesRemove;
    public bool OnPlayerDisconnectRemove;
    public ushort BindingSkillLevel;
    public bool OnReleaseSkillRemove;
    public bool RemoveAddCooling;
    public ushort SkillCooldown;
    public byte InitialBuffStacks;
    public byte MaxBuffCount;
    public bool AllowsSynthesis;
    public byte BuffCraftingStacks;
    public ushort BuffCraftingID;
    public int ProcessInterval;
    public int ProcessDelay;
    public int Duration;
    public bool ExtendedDuration;
    public ushort FollowedByID;
    public ushort AssociatedID;
    public ushort[] RequireBuff;
    public bool SkillLevelDelay;
    public int ExtendedTimePerLevel;
    public bool PlayerStatDelay;
    public Stat BoundPlayerStat;
    public float StatDelayFactor;
    public bool HasSpecificInscriptionDelay;
    public int SpecificInscriptionSkills;
    public int InscriptionExtendedTime;
    public GameObjectState PlayerState;
    public InscriptionStat[] StatsIncOrDec;
    private Stats[] m_BaseStatsIncOrDec;
    public SkillDamageType DamageType;
    public int[] DamageBase;
    public float[] DamageFactor;
    public int StrengthenInscriptionID;
    public int StrengthenInscriptionBase;
    public float InscriptionEnhancementFactor;
    public bool EffectRemoved;
    public ushort EffectiveFollowedByID;
    public bool FollowedBySkillOwner;
    public BuffDeterminationMethod HowJudgeEffect;
    public bool LimitedDamage;
    public int LimitedDamageValue;
    public BuffDeterminationType EffectJudgeType;
    public HashSet<ushort> SpecificSkillID;
    public int[] DamageIncOrDecBase;
    public float[] DamageIncOrDecFactor;
    public string TriggerTrapSkills;
    public ObjectSize NumberTrapsTriggered;
    public byte[] HealthRecoveryBase;
    public int TemptationDurationIncreased;
    public float IncreasedTemptationChance;
    public byte TemptationLevelIncreased;

    // TODO: New in DB
    /*public ushort[] SpecificBuffID;
    public int[] 每层触发Buff;
    public float[] HealthRegenerationFactor;
    public byte[] ManaRecoveryBase;
    public float[] ManaRegenerationFactor;
    public string BuffNote;
    public GameObjectType LimitedTargetType;
    public bool DoesNotCountBuffStacks;
    public string[] AttackTriggerSkill;
    public bool ReverseJudgeEffect;
    public bool AttackAnimationDisappears;
    public ushort TriggerSpellDamage;
    public bool SacredDamage;*/

    public Stats[] BaseStatsIncOrDec
    {
        get
        {
            if (m_BaseStatsIncOrDec != null)
                return m_BaseStatsIncOrDec;

            m_BaseStatsIncOrDec = new Stats[4]
            {
                new Stats(),
                new Stats(),
                new Stats(),
                new Stats()
            };
            if (StatsIncOrDec != null)
            {
                foreach (var stat in StatsIncOrDec)
                {
                    m_BaseStatsIncOrDec[0][stat.Stat] = stat.Level0;
                    m_BaseStatsIncOrDec[1][stat.Stat] = stat.Level1;
                    m_BaseStatsIncOrDec[2][stat.Stat] = stat.Level2;
                    m_BaseStatsIncOrDec[3][stat.Stat] = stat.Level3;
                }
            }
            return m_BaseStatsIncOrDec;
        }
    }

    public static void LoadData()
    {
        DataSheet.Clear();

        var path = Settings.Default.GameDataPath + "\\System\\Skills\\Buffs\\";
        if (!Directory.Exists(path))
            return;

        var array = Serializer.Deserialize<GameBuff>(path);
        foreach (var obj in array)
            DataSheet.Add(obj.ID, obj);
    }
}
