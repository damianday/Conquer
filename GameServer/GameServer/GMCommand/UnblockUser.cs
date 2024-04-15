using System;
using GameServer.Database;

namespace GameServer;

public sealed class UnblockUser : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string UserName;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (Session.CharacterInfoTable.SearchTable.TryGetValue(UserName, out var value) && value is CharacterInfo character)
        {
            character.BlockDate.V = default(DateTime);
            SMain.AddCommandLog($"<= @{GetType().Name} The command has been executed, and the ban expires: {character.BlockDate}");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " The command execution failed and the role does not exist");
        }
    }
}
