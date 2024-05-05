using GameServer.Database;

namespace GameServer;

public sealed class ChangeLevel : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string UserName;

    [FieldDescription(0, Index = 1)]
    public int Level;

    public override ExecuteCondition Priority => ExecuteCondition.Normal;
    public override UserDegree Degree => UserDegree.SysOp;

    public override void ExecuteCommand()
    {
        var character = Session.GetCharacter(UserName);
        if (character == null)
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " The command execution failed, character does not exist");
            return;
        }

        character.CurrentLevel = (byte)Level;
        character.CurrentExperience = 0;
        character.Connection?.Player?.LevelChanged();

        SMain.AddCommandLog($"<= @{GetType().Name} The command has been executed, the current character level: {character.CurrentLevel}");
    }
}
