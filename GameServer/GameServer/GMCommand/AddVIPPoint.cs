using GameServer.Database;

namespace GameServer;

public sealed class AddVIPPoint : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string UserName;

    [FieldDescription(0, Index = 1)]
    public int Amount;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (Session.CharacterInfoTable.SearchTable.TryGetValue(UserName, out var value) && value is CharacterInfo character)
        {
            character.VIPPoints.V += Amount;
            SMain.AddCommandLog($"<= @{GetType().Name} The command has been executed, and the current VIP credits: {character.VIPPoints.V}");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " The command execution failed and the role does not exist");
        }
    }
}
