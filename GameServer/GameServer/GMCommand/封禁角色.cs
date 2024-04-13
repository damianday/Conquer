using System;
using GameServer.Database;

namespace GameServer;

public sealed class 封禁角色 : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string 角色名字;

    [FieldDescription(0, Index = 1)]
    public float 封禁天数;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (Session.CharacterInfoTable.SearchTable.TryGetValue(角色名字, out var value) && value is CharacterInfo 角色数据)
        {
            角色数据.BlockDate.V = DateTime.Now.AddDays(封禁天数);
            角色数据.Connection?.Disconnect(new Exception("角色被封禁, 强制下线"));
            SMain.AddCommandLog($"<= @{GetType().Name} 命令已经执行, 封禁到期时间: {角色数据.BlockDate}");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 角色不存在");
        }
    }
}
