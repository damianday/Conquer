public enum SkillTriggerMethod
{
    OriginAbsolutePosition,             // 原点位置绝对触发
    AnchorAbsolutePosition,             // 锚点位置绝对触发
    AssassinationAbsolutePosition,      // 刺杀位置绝对触发
    TargetHitDefinitely,                // 目标命中绝对触发
    MonsterDeathDefinitely,             // 怪物死亡绝对触发
    MonsterDeathTransposition,          // 怪物死亡换位触发
    MonsterHitDefinitely,               // 怪物命中绝对触发
    MonsterHitProbability,              // 怪物命中概率触发
    NoTargetPosition,                   // 无目标锚点位触发
    TargetPositionAbsolute,             // 目标位置绝对触发
    ForehandAndBackhandRandom,          // 正手反手随机触发
    TargetDeathDefinitely,              // 目标死亡绝对触发
    TargetMissDefinitely                // 目标闪避绝对触发
}
