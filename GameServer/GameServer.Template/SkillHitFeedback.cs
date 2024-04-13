using System;

namespace GameServer.Template;

[Flags]
public enum SkillHitFeedback
{
    Normal = 0,             // 正常
    DamageHealth = 1,       // 喷血
    Block = 2,              // 格挡
    Miss = 4,               // 闪避
    Parry = 8,              // 招架
    Lose = 0x10,            // 丢失
    Knockback = 0x20,       // 后仰
    Immune = 0x40,          // 免疫
    Death = 0x80,           // 死亡
    SpecialEffect = 0x100,  // 特效
    Shield = 0x200          // 护盾
}
