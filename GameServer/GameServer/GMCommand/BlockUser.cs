using System;
using GameServer.Database;

namespace GameServer;

public sealed class BlockUser : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string UserName;

    [FieldDescription(0, Index = 1)]
    public float Days;

    public override ExecuteCondition Priority => ExecuteCondition.Normal;
    public override UserDegree Degree => UserDegree.SysOp;

    public override void ExecuteCommand()
    {
        if (Session.CharacterInfoTable.SearchTable.TryGetValue(UserName, out var value) && value is CharacterInfo character)
        {
            character.BlockDate.V = DateTime.UtcNow.AddDays(Days);
            character.Connection?.Close(new Exception("The character is banned and forcibly taken offline"));
            SMain.AddCommandLog($"<= @{GetType().Name} The command has been executed, and the ban expires: {character.BlockDate}");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " The command execution failed and the character does not exist");
        }
    }
}
