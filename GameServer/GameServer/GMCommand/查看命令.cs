using System.Collections.Generic;

namespace GameServer;

public sealed class 查看命令 : GMCommand
{
    public override ExecutionPriority Priority => ExecutionPriority.Immediate;

    public override void ExecuteCommand()
    {
        SMain.AddCommandLog("以下为所有支持的GM命令:");
        foreach (KeyValuePair<string, string> item in GMCommand.Commands)
        {
            SMain.AddCommandLog(item.Value);
        }
    }
}
