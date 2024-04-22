[Flags]
public enum GameObjectState
{
    Normal = 0,             // 正常状态
    Stun = 1,               // 硬直状态
    BusyGreen = 2,          // 忙绿状态
    Poisoned = 4,           // 中毒状态
    Disabled = 8,           // 残废状态
    Immobilized = 0x10,     // 定身状态
    Paralyzed = 0x20,       // 麻痹状态
    Hegemony = 0x40,        // 霸体状态
    Invincible = 0x80,      // 无敌状态
    Invisible = 0x100,      // 隐身状态
    Stealth = 0x200,        // 潜行状态
    Unconscious = 0x400,    // 失神状态
    Exposed = 0x800,        // 暴露状态
    Mounted = 0x1000        // 坐骑状态
}
