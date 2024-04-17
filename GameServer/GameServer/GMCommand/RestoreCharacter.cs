using System;
using GameServer.Database;

namespace GameServer;

public sealed class RestoreCharacter : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string UserName;

    public override ExecuteCondition Priority => ExecuteCondition.Normal;

    public override void ExecuteCommand()
    {
        if (Session.CharacterInfoTable.SearchTable.TryGetValue(UserName, out var value) && value is CharacterInfo character)
        {
            if (!(character.DeletetionDate.V == default(DateTime)) && character.Account.V.DeletedCharacters.Contains(character))
            {
                if (character.Account.V.Characters.Count >= 4)
                {
                    SMain.AddCommandLog("<= @" + GetType().Name + " The command execution failed and the list of roles is full");
                    return;
                }
                if (character.Account.V.Connection != null)
                {
                    SMain.AddCommandLog("<= @" + GetType().Name + " If the command fails to be executed, the account must be offline");
                    return;
                }
                
                character.DeletetionDate.V = character.FrozenDate.V = default(DateTime);
                character.Account.V.DeletedCharacters.Remove(character);
                character.Account.V.Characters.Add(character);
                SMain.AddCommandLog("<= @" + GetType().Name + " The command has been executed and the role has been restored successfully");
            }
            else
            {
                SMain.AddCommandLog("<= @" + GetType().Name + " The command execution failed, and the role was not deleted");
            }
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " The command execution failed and the role does not exist");
        }
    }
}
