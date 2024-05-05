namespace GameServer.Database;

public enum GuildLogType
{
    CreateGuild = 1,        // 创建公会
    JoinGuild = 2,          // 加入公会
    LeaveGuild = 3,         // 离开公会
    逐出公会 = 4,
    ChangeRank = 5,         // 变更职位
    建筑升级 = 6,
    会长传位 = 8,
    行会结盟 = 9,
    行会敌对 = 10,
    建造建筑 = 12,
    建筑被毁 = 13,
    建筑拆除 = 14,
    Boss刷新 = 15,
    战争获胜 = 16,
    战争失败 = 17,
    防守胜利 = 18,
    防守失败 = 19,
    取消结盟 = 21,
    取消敌对 = 22
}
