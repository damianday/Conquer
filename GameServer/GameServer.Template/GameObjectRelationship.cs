using System;

namespace GameServer.Template;

[Flags]
public enum GameObjectRelationship
{
    Myself = 1,     // 自身
    Friendly = 2,   // 友方
    Hostile = 4     // 敌对
}
