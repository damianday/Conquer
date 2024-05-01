using GameServer.Database;
using GameServer.Networking;

namespace GameServer;

public sealed class Superman : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string UserName;

    public override ExecuteCondition Priority => ExecuteCondition.Normal;

    public override void ExecuteCommand()
    {
        var character = Session.GetCharacter(UserName);
        if (character == null)
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " The command execution failed, character does not exist");
            return;
        }

        var player = character.Connection?.Player;
        if (player == null)
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " The command execution failed, the player is invalid");
            return;
        }

        //if (!IsGM && !Settings.Default.TestServer) return;

        player.GMNeverDie = !player.GMNeverDie;

        var msg = player.GMNeverDie ? "Invincible Mode." : "Normal Mode.";
        NetworkManager.SendMessage(player, msg);

        SMain.AddCommandLog($"<= @{GetType().Name} The command has been executed, the current superman state: {player.GMNeverDie}");
    }
}
