using System;
using System.Collections.Generic;
using GameServer.Database;

namespace GameServer;

public sealed class 查看数据 : GMCommand
{
    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        SMain.AddCommandLog("<= @" + GetType().Name + " 命令已经执行, 数据库详情如下:");
        foreach (KeyValuePair<Type, DBCollection> item in Session.Tables)
        {
            SMain.AddCommandLog($"{item.Value.Type.Name}  数量: {item.Value.DataSheet.Count}");
        }
    }
}
