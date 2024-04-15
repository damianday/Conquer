using System.Collections.Generic;

namespace GameServer;

public sealed class ReviewCommands : GMCommand
{
    public override ExecutionPriority Priority => ExecutionPriority.Immediate;

    public override void ExecuteCommand()
    {
        SMain.AddCommandLog("The following is a list of all supported GM commands:");
        foreach (var cmd in GMCommand.Commands)
        {
            SMain.AddCommandLog(cmd.Value);
        }
    }
}
