using System;
using System.Collections.Generic;
using GameServer.Map;
using GameServer.Template;

namespace GameServer.Database;

public sealed class BuffInfo : DBObject
{
    public MapObject Caster;

    public readonly DataMonitor<ushort> ID;
    public readonly DataMonitor<TimeSpan> 持续时间;
    public readonly DataMonitor<TimeSpan> 剩余时间;
    public readonly DataMonitor<TimeSpan> 处理计时;
    public readonly DataMonitor<byte> 当前层数;
    public readonly DataMonitor<byte> BuffLevel;
    public readonly DataMonitor<int> 伤害基数;

    public BuffEffectType Buff效果 => Template.Effect;
    public SkillDamageType 伤害类型 => Template.DamageType;

    public GameBuff Template
    {
        get
        {
            if (GameBuff.DataSheet.TryGetValue(ID.V, out var value))
                return value;
            return null;
        }
    }

    public bool 增益Buff => Template.ActionType == BuffActionType.Gain;
    public bool Buff同步 => Template.SyncClient;
    public bool 到期消失 => Template?.RemoveOnExpire ?? false;
    public bool 下线消失 => Template.OnPlayerDisconnectRemove;
    public bool 死亡消失 => Template.OnPlayerDiesRemove;
    public bool 换图消失 => Template.OnChangeMapRemove;
    public bool 绑定武器 => Template.OnChangeWeaponRemove;
    public bool 添加冷却 => Template.RemoveAddCooling;
    public ushort 绑定技能 => Template.BindingSkillLevel;
    public ushort 冷却时间 => Template.SkillCooldown;
    public int 处理延迟 => Template.ProcessDelay;
    public int 处理间隔 => Template.ProcessInterval;
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

    public ushort[] 依存列表 => Template.RequireBuff;

    public Stats 属性加成
    {
        get
        {
            if ((Buff效果 & BuffEffectType.StatIncOrDec) != 0)
            {
                return Template.BaseStatsIncOrDec[BuffLevel.V];
            }
            return null;
        }
    }

    public BuffInfo()
    {
    }

    public BuffInfo(MapObject 来源, MapObject 目标, ushort id)
    {
        Caster = 来源;
        ID.V = id;
        当前层数.V = Template.InitialBuffStacks;
        持续时间.V = TimeSpan.FromMilliseconds(Template.Duration);
        处理计时.V = TimeSpan.FromMilliseconds(Template.ProcessDelay);
        if (来源 is PlayerObject 玩家实例)
        {
            if (Template.BindingSkillLevel != 0 && 玩家实例.Skills.TryGetValue(Template.BindingSkillLevel, out var v))
            {
                BuffLevel.V = v.Level.V;
            }
            if (Template.ExtendedDuration && Template.SkillLevelDelay)
            {
                持续时间.V += TimeSpan.FromMilliseconds(BuffLevel.V * Template.ExtendedTimePerLevel);
            }
            if (Template.ExtendedDuration && Template.PlayerStatDelay)
            {
                持续时间.V += TimeSpan.FromMilliseconds((float)玩家实例[Template.BoundPlayerStat] * Template.StatDelayFactor);
            }
            if (Template.ExtendedDuration && Template.HasSpecificInscriptionDelay && 玩家实例.Skills.TryGetValue((ushort)(Template.SpecificInscriptionSkills / 10), out var v2) && v2.InscriptionID == Template.SpecificInscriptionSkills % 10)
            {
                持续时间.V += TimeSpan.FromMilliseconds(Template.InscriptionExtendedTime);
            }
        }
        else if (来源 is PetObject 宠物实例)
        {
            if (Template.BindingSkillLevel != 0 && 宠物实例.Master.Skills.TryGetValue(Template.BindingSkillLevel, out var v3))
            {
                BuffLevel.V = v3.Level.V;
            }
            if (Template.ExtendedDuration && Template.SkillLevelDelay)
            {
                持续时间.V += TimeSpan.FromMilliseconds(BuffLevel.V * Template.ExtendedTimePerLevel);
            }
            if (Template.ExtendedDuration && Template.PlayerStatDelay)
            {
                持续时间.V += TimeSpan.FromMilliseconds((float)宠物实例.Master[Template.BoundPlayerStat] * Template.StatDelayFactor);
            }
            if (Template.ExtendedDuration && Template.HasSpecificInscriptionDelay && 宠物实例.Master.Skills.TryGetValue((ushort)(Template.SpecificInscriptionSkills / 10), out var v4) && v4.InscriptionID == Template.SpecificInscriptionSkills % 10)
            {
                持续时间.V += TimeSpan.FromMilliseconds(Template.InscriptionExtendedTime);
            }
        }
        剩余时间.V = 持续时间.V;
        if ((Buff效果 & BuffEffectType.DealDamage) != 0)
        {
            int num = ((Template.DamageBase?.Length > BuffLevel.V) ? Template.DamageBase[BuffLevel.V] : 0);
            float num2 = ((Template.DamageFactor?.Length > BuffLevel.V) ? Template.DamageFactor[BuffLevel.V] : 0f);
            if (来源 is PlayerObject 玩家实例2 && Template.StrengthenInscriptionID != 0 && 玩家实例2.Skills.TryGetValue((ushort)(Template.StrengthenInscriptionID / 10), out var v5) && v5.InscriptionID == Template.StrengthenInscriptionID % 10)
            {
                num += Template.StrengthenInscriptionBase;
                num2 += Template.InscriptionEnhancementFactor;
            }
            int num3 = 0;
            switch (伤害类型)
            {
                case SkillDamageType.Attack:
                    num3 = Compute.CalculateAttack(来源[Stat.MinDC], 来源[Stat.MaxDC], 来源[Stat.Luck]);
                    break;
                case SkillDamageType.Magic:
                    num3 = Compute.CalculateAttack(来源[Stat.MinMC], 来源[Stat.MaxMC], 来源[Stat.Luck]);
                    break;
                case SkillDamageType.Taoism:
                    num3 = Compute.CalculateAttack(来源[Stat.MinSC], 来源[Stat.MaxSC], 来源[Stat.Luck]);
                    break;
                case SkillDamageType.Piercing:
                    num3 = Compute.CalculateAttack(来源[Stat.MinNC], 来源[Stat.MaxNC], 来源[Stat.Luck]);
                    break;
                case SkillDamageType.Archery:
                    num3 = Compute.CalculateAttack(来源[Stat.MinBC], 来源[Stat.MaxBC], 来源[Stat.Luck]);
                    break;
                case SkillDamageType.Toxicity:
                    num3 = 来源[Stat.MaxSC];
                    break;
                case SkillDamageType.Sacred:
                    num3 = Compute.CalculateAttack(来源[Stat.MinHC], 来源[Stat.MaxHC], 0);
                    break;
            }
            伤害基数.V = num + (int)((float)num3 * num2);
        }
        if (目标 is PlayerObject)
        {
            Session.BuffInfoTable.Add(this, indexed: true);
        }
    }

    public override string ToString()
    {
        return Template?.Name;
    }
}
