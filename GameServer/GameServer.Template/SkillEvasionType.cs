namespace GameServer.Template;

public enum SkillEvasionType
{
    SkillCannotBeEvaded,        // 技能无法闪避
    CanBePhsyicallyEvaded,      // 可被物理闪避
    CanBeMagicEvaded,           // 可被魔法闪避
    CanBePoisonEvaded,          // 可被中毒闪避
    NonMonstersCanEvade         // 非怪物可闪避
}
