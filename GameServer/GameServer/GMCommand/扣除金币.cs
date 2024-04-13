using System;
using GameServer.Database;
using GameServer.Networking;

using GamePackets.Server;

namespace GameServer;

public sealed class 扣除金币 : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string UserName;

    [FieldDescription(0, Index = 1)]
    public int 金币数量;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (Session.CharacterInfoTable.SearchTable.TryGetValue(UserName, out var value) && value is CharacterInfo 角色数据)
        {
            角色数据.Gold = Math.Max(0, 角色数据.Gold - 金币数量);
            角色数据.Enqueue(new 货币数量变动
            {
                Currency = 1,
                Amount = 角色数据.Gold
            });
            SMain.AddCommandLog($"<= @{GetType().Name} 命令已经执行, 当前金币数量: {角色数据.Gold}");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 角色不存在");
        }
    }
}
