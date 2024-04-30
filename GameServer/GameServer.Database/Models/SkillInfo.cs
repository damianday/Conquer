using System;
using System.Collections.Generic;
using GameServer.Template;

namespace GameServer.Database;

public sealed class SkillInfo : DBObject
{
    public byte InscriptionID;
    public DateTime CooldownTime;

    public readonly DataMonitor<ushort> ID;
    public readonly DataMonitor<ushort> Experience;
    public readonly DataMonitor<byte> Level;
    public readonly DataMonitor<byte> Shortcut;
    public readonly DataMonitor<byte> RemainingCount;

    public int SkillIndex => ID.V * 100 + InscriptionID * 10 + Level.V;
    public InscriptionSkill Inscription => InscriptionSkill.DataSheet[InscriptionIndex];
    public bool IsPassive => Inscription.PassiveSkill;

    public byte RequiredLevel
    {
        get
        {
            if (Inscription.MinPlayerLevel != null && Inscription.MinPlayerLevel.Length > Level.V + 1)
            {
                if (Inscription.MinPlayerLevel[Level.V] == 0)
                    return byte.MaxValue;
                return Inscription.MinPlayerLevel[Level.V];
            }
            return byte.MaxValue;
        }
    }

    public byte SkillCount => Inscription.SkillCount;

    public ushort PeriodCount => Inscription.PeriodCount;

    public int 升级经验
    {
        get
        {
            if (Inscription.MinSkillExp != null && Inscription.MinSkillExp.Length > Level.V)
                return Inscription.MinSkillExp[Level.V];
            return 0;
        }
    }

    public ushort InscriptionIndex => (ushort)(ID.V * 10 + InscriptionID);
    public int CombatBonus => Inscription.SkillCombatBonus[Level.V];
    public List<ushort> SkillBuffs => Inscription.ComesWithBuff;
    public List<ushort> PassiveSkills => Inscription.PassiveSkills;

    public Stats BonusStats
    {
        get
        {
            if (Inscription.StatBonusTable != null && Inscription.StatBonusTable.Length > Level.V)
                return Inscription.StatBonusTable[Level.V];
            return null;
        }
    }

    public SkillInfo()
    {
    }

    public SkillInfo(ushort id)
    {
        Shortcut.V = 100;
        ID.V = id;
        RemainingCount.V = SkillCount;
        Session.SkillInfoTable.Add(this, true);
    }

    public override string ToString() => Inscription?.SkillName;
}
