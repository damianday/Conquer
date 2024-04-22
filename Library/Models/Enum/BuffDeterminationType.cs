public enum BuffDeterminationType
{
    AllSpellDamage = 0,             // 所有技能伤害
    AllPhysicalDamage = 1,          // 所有物理伤害
    AllMagicDamage = 2,             // 所有魔法伤害
    AllSpecificDamage = 4,          // 所有特定伤害
    SourceSkillDamage = 8,          // 来源技能伤害
    SourcePhysicalDamage = 0x10,    // 来源物理伤害
    SourceMagicDamage = 0x20,       // 来源魔法伤害
    SourceSpecificDamage = 0x40,    // 来源特定伤害,
    DetectObjectBuff = 0x80         // 检测对象BUFF
}
