using System;
using GameServer.Database;
using GameServer.Networking;

using GamePackets.Server;

namespace GameServer;

public sealed class 扣除元宝 : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string UserName;

    [FieldDescription(0, Index = 1)]
    public int 元宝数量;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (Session.CharacterInfoTable.SearchTable.TryGetValue(UserName, out var value) && value is CharacterInfo 角色数据)
        {
            角色数据.Ingot = Math.Max(0, 角色数据.Ingot - 元宝数量);
            角色数据.Enqueue(new 同步元宝数量
            {
                元宝数量 = 角色数据.Ingot
            });
            SMain.AddCommandLog($"<= @{GetType().Name} 命令已经执行, 当前元宝数量: {角色数据.Ingot}");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 角色不存在");
        }
    }
}
