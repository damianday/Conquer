using System.Collections.Generic;

namespace GameServer;

public sealed class ViewCommands : GMCommand
{
    public override ExecuteCondition Priority => ExecuteCondition.Immediate;

    public override void ExecuteCommand()
    {
        SMain.AddCommandLog("The following is a list of all supported GM commands:");
        foreach (var cmd in GMCommand.Commands)
        {
            SMain.AddCommandLog(cmd.Value);
        }
    }
}
