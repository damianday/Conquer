using System;
using GameServer.Database;

namespace GameServer;

public sealed class CharacterTransfer : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string UserName;

    [FieldDescription(0, Index = 1)]
    public string NewAccountName;

    public override ExecuteCondition Priority => ExecuteCondition.Normal;

    public override void ExecuteCommand()
    {
        var character = Session.GetCharacter(UserName);
        if (character != null)
        {
            if (!character.Account.V.Characters.Contains(character))
            {
                SMain.AddCommandLog("<= @" + GetType().Name + " Failed to execute the command, character is deleted");
            }
            else if (character.BlockDate.V > DateTime.Now)
            {
                SMain.AddCommandLog("<= @" + GetType().Name + " Failed to execute the command, character is blocked.");
            }
            else if (character.Account.V.BlockDate.V > DateTime.Now)
            {
                SMain.AddCommandLog("<= @" + GetType().Name + " Failed to execute the command, the original account is banned.");
            }
            else
            {
                var account = Session.GetAccount(NewAccountName);
                if (account != null)
                {
                    if (account.BlockDate.V > DateTime.Now)
                    {
                        SMain.AddCommandLog("<= @" + GetType().Name + " Failed to execute the command, the transfer account is in the banned state");
                    }
                    else if (account.Characters.Count >= 4)
                    {
                        SMain.AddCommandLog("<= @" + GetType().Name + " Failed to execute the command, the number of transferred accounts has reached the maximum number of character.");
                    }
                    else if (character.Account.V.Connection == null && account.Connection == null)
                    {
                        character.Account.V.Characters.Remove(character);
                        character.Account.V = account;
                        account.Characters.Add(character);
                        SMain.AddCommandLog($"<= @{GetType().Name} Command has been executed, the characters current account:{character.Account}");
                    }
                    else
                    {
                        SMain.AddCommandLog("<= @" + GetType().Name + " Command failed. Both accounts must be offline.");
                    }
                }
                else
                {
                    SMain.AddCommandLog("<= @" + GetType().Name + " Command failed, transfer account does not exist or never logged in");
                }
            }
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " Command failed, character does not exist");
        }
    }
}
