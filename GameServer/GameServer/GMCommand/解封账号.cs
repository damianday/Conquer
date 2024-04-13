using System;
using GameServer.Database;

namespace GameServer;

public sealed class 解封账号 : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string 账号名字;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (Session.AccountInfoTable.SearchTable.TryGetValue(账号名字, out var value) && value is AccountInfo 账号数据)
        {
            账号数据.BlockDate.V = default(DateTime);
            SMain.AddCommandLog($"<= @{GetType().Name} 命令已经执行, 封禁到期时间: {账号数据.BlockDate}");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 账号不存在");
        }
    }
}
