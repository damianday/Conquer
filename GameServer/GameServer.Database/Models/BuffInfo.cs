using System;
using System.Collections.Generic;
using GameServer.Map;
using GameServer.Template;

namespace GameServer.Database;

public sealed class BuffInfo : DBObject
{
    public MapObject Caster;

    public readonly DataMonitor<ushort> ID;
    public readonly DataMonitor<TimeSpan> Duration;
    public readonly DataMonitor<TimeSpan> RemainingTime;
    public readonly DataMonitor<TimeSpan> ProcessTime;
    public readonly DataMonitor<byte> 当前层数;
    public readonly DataMonitor<byte> BuffLevel;
    public readonly DataMonitor<int> DamageBase;

    public BuffEffectType BuffEffect => Template.Effect;
    public SkillDamageType DamageType => Template.DamageType;

    public GameBuff Template
    {
        get
        {
            if (GameBuff.DataSheet.TryGetValue(ID.V, out var value))
                return value;
            return null;
        }
    }

    public bool GainBuff => Template.ActionType == BuffActionType.Gain;
    public bool SyncClient => Template.SyncClient;
    public bool 到期消失 => Template?.RemoveOnExpire ?? false;
    public bool 下线消失 => Template.OnPlayerDisconnectRemove;
    public bool 死亡消失 => Template.OnPlayerDiesRemove;
    public bool 换图消失 => Template.OnChangeMapRemove;
    public bool 绑定武器 => Template.OnChangeWeaponRemove;
    public bool 添加冷却 => Template.RemoveAddCooling;
    public ushort 绑定技能 => Template.BindingSkillLevel;
    public ushort CooldownTime => Template.SkillCooldown;
    public int ProcessDelay => Template.ProcessDelay;
    public int ProcessInterval => Template.ProcessInterval;
    public byte 最大层数 => Template.MaxBuffCount;

    public ushort Buff分组
    {
        get
        {
            if (Template.GroupID == 0)
                return ID.V;
            return Template.GroupID;
        }
    }

    public ushort[] RequiredBuffs => Template.RequireBuff;

    public Stats BonusStats
    {
        get
        {
            if ((BuffEffect & BuffEffectType.StatIncOrDec) != 0)
                return Template.BaseStatsIncOrDec[BuffLevel.V];

            return null;
        }
    }

    public BuffInfo()
    {
    }

    public BuffInfo(MapObject caster, MapObject target, ushort id)
    {
        Caster = caster;
        ID.V = id;
        当前层数.V = Template.InitialBuffStacks;
        Duration.V = TimeSpan.FromMilliseconds(Template.Duration);
        ProcessTime.V = TimeSpan.FromMilliseconds(Template.ProcessDelay);
        if (caster is PlayerObject player)
        {
            if (Template.BindingSkillLevel != 0 && player.Skills.TryGetValue(Template.BindingSkillLevel, out var v))
            {
                BuffLevel.V = v.Level.V;
            }
            if (Template.ExtendedDuration && Template.SkillLevelDelay)
            {
                Duration.V += TimeSpan.FromMilliseconds(BuffLevel.V * Template.ExtendedTimePerLevel);
            }
            if (Template.ExtendedDuration && Template.PlayerStatDelay)
            {
                Duration.V += TimeSpan.FromMilliseconds((float)player[Template.BoundPlayerStat] * Template.StatDelayFactor);
            }
            if (Template.ExtendedDuration && Template.HasSpecificInscriptionDelay && player.Skills.TryGetValue((ushort)(Template.SpecificInscriptionSkills / 10), out var v2) && v2.InscriptionID == Template.SpecificInscriptionSkills % 10)
            {
                Duration.V += TimeSpan.FromMilliseconds(Template.InscriptionExtendedTime);
            }
        }
        else if (caster is PetObject pet)
        {
            if (Template.BindingSkillLevel != 0 && pet.Master.Skills.TryGetValue(Template.BindingSkillLevel, out var v3))
            {
                BuffLevel.V = v3.Level.V;
            }
            if (Template.ExtendedDuration && Template.SkillLevelDelay)
            {
                Duration.V += TimeSpan.FromMilliseconds(BuffLevel.V * Template.ExtendedTimePerLevel);
            }
            if (Template.ExtendedDuration && Template.PlayerStatDelay)
            {
                Duration.V += TimeSpan.FromMilliseconds((float)pet.Master[Template.BoundPlayerStat] * Template.StatDelayFactor);
            }
            if (Template.ExtendedDuration && Template.HasSpecificInscriptionDelay && pet.Master.Skills.TryGetValue((ushort)(Template.SpecificInscriptionSkills / 10), out var v4) && v4.InscriptionID == Template.SpecificInscriptionSkills % 10)
            {
                Duration.V += TimeSpan.FromMilliseconds(Template.InscriptionExtendedTime);
            }
        }
        RemainingTime.V = Duration.V;
        if ((BuffEffect & BuffEffectType.DealDamage) != 0)
        {
            int num = ((Template.DamageBase?.Length > BuffLevel.V) ? Template.DamageBase[BuffLevel.V] : 0);
            float num2 = ((Template.DamageFactor?.Length > BuffLevel.V) ? Template.DamageFactor[BuffLevel.V] : 0f);
            if (caster is PlayerObject 玩家实例2 && Template.StrengthenInscriptionID != 0 && 玩家实例2.Skills.TryGetValue((ushort)(Template.StrengthenInscriptionID / 10), out var v5) && v5.InscriptionID == Template.StrengthenInscriptionID % 10)
            {
                num += Template.StrengthenInscriptionBase;
                num2 += Template.InscriptionEnhancementFactor;
            }
            int damage = 0;
            switch (DamageType)
            {
                case SkillDamageType.Attack:
                    damage = Compute.CalculateAttack(caster[Stat.MinDC], caster[Stat.MaxDC], caster[Stat.Luck]);
                    break;
                case SkillDamageType.Magic:
                    damage = Compute.CalculateAttack(caster[Stat.MinMC], caster[Stat.MaxMC], caster[Stat.Luck]);
                    break;
                case SkillDamageType.Taoism:
                    damage = Compute.CalculateAttack(caster[Stat.MinSC], caster[Stat.MaxSC], caster[Stat.Luck]);
                    break;
                case SkillDamageType.Piercing:
                    damage = Compute.CalculateAttack(caster[Stat.MinNC], caster[Stat.MaxNC], caster[Stat.Luck]);
                    break;
                case SkillDamageType.Archery:
                    damage = Compute.CalculateAttack(caster[Stat.MinBC], caster[Stat.MaxBC], caster[Stat.Luck]);
                    break;
                case SkillDamageType.Toxicity:
                    damage = caster[Stat.MaxSC];
                    break;
                case SkillDamageType.Holy:
                    damage = Compute.CalculateAttack(caster[Stat.MinHC], caster[Stat.MaxHC], 0);
                    break;
            }
            DamageBase.V = num + (int)((float)damage * num2);
        }
        if (target is PlayerObject)
        {
            Session.BuffInfoTable.Add(this, true);
        }
    }

    public override string ToString() => Template?.Name;
}
