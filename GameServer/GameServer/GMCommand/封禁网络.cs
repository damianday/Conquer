using System;
using System.Net;
using GameServer.Database;

namespace GameServer;

public sealed class 封禁网络 : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string 网络地址;

    [FieldDescription(0, Index = 1)]
    public float 封禁天数;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (IPAddress.TryParse(网络地址, out var _))
        {
            SystemInfo.Info.AddIPBan(网络地址, DateTime.Now.AddDays(封禁天数));
            SMain.AddCommandLog($"<= @{GetType().Name} 命令已经执行, 封禁到期时间: {DateTime.Now.AddDays(封禁天数)}");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 地址格式错误");
        }
    }
}
