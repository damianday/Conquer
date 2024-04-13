using System;

namespace GameServer.Template;

[Flags]
public enum BuffEffectType
{
    SkillSign = 0,              // 技能标志
    StatusFlag = 1,             // 状态标志
    DealDamage = 2,             // 造成伤害
    StatIncOrDec = 4,           // 属性增减
    DamageIncOrDec = 8,         // 伤害增减
    CreateTrap = 0x10,          // 创建陷阱
    HealthRecovery = 0x20,      // 生命回复
    TemptationBoost = 0x40,     // 诱惑提升
    AddedBuffs = 0x80,          // 添加BUFF
    UnleashSkills = 0x100,      // 释放技能
    Mounted = 0x200,            // 坐骑BUFF
    GiveReward = 0x400          // 获得奖励
}
