using System.Text;
using GameServer.Database;

namespace GameServer;

public sealed class CharacterRename : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string UserName;

    [FieldDescription(0, Index = 1)]
    public string NewUserName;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        var character = Session.GetCharacter(UserName);
        if (character != null)
        {
            if (character.Connection == null && character.Account.V.Connection == null)
            {
                if (Encoding.UTF8.GetBytes(NewUserName).Length > 24)
                {
                    SMain.AddCommandLog("<= @" + GetType().Name + " Command failed, role name too long");
                    return;
                }
                if (Session.CharacterInfoTable.SearchTable.ContainsKey(NewUserName))
                {
                    SMain.AddCommandLog("<= @" + GetType().Name + " Command failed. Name registered.");
                    return;
                }
                Session.CharacterInfoTable.SearchTable.Remove(character.UserName.V);
                character.UserName.V = NewUserName;
                Session.CharacterInfoTable.SearchTable.Add(character.UserName.V, character);
                SMain.AddCommandLog($"<= @{GetType().Name} Command has been executed, the characters current name: {character}");
            }
            else
            {
                SMain.AddCommandLog("<= @" + GetType().Name + " Command failed. Account must be taken offline.");
            }
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " Command failed, character does not exist");
        }
    }
}
