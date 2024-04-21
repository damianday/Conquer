[Flags]
public enum SpecifyTargetType
{
    None = 0,               // 无
    LowLevelTarget = 1,     // 低级目标
    ShieldMage = 2,         // 带盾法师
    LowLevelMonster = 4,    // 低级怪物
    LowHealthMonster = 8,   // 低血怪物
    Normal = 0x10,          // 普通怪物
    AllMonsters = 0x20,     // 所有怪物
    Undead = 0x40,          // 不死生物
    ZergCreature = 0x80,    // 虫族生物
    WomaMonster = 0x100,    // 沃玛怪物
    PigMonster = 0x200,     // 猪类怪物
    ZumaMonster = 0x400,    // 祖玛怪物
    EliteMonsters = 0x800,  // 精英怪物
    AllPets = 0x1000,       // 所有宠物
    Backstab = 0x2000,      // 背刺目标
    DragonMonster = 0x4000, // 魔龙怪物
    AllPlayers = 0x8000     // 所有玩家
}
