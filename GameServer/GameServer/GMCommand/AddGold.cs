using GameServer.Database;
using GameServer.Networking;

using GamePackets.Server;

namespace GameServer;

public sealed class AddGold : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string UserName;

    [FieldDescription(0, Index = 1)]
    public int Amount;

    public override ExecuteCondition Priority => ExecuteCondition.Normal;

    public override void ExecuteCommand()
    {
        if (Session.CharacterInfoTable.SearchTable.TryGetValue(UserName, out var value) && value is CharacterInfo character)
        {
            character.Gold += Amount;
            character.Enqueue(new SyncCurrencyPacket
            {
                Currency = 1,
                Amount = character.Gold
            });
            SMain.AddCommandLog($"<= @{GetType().Name} The command has been executed, the current amount of gold: {character.Gold}");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " The command execution failed, character does not exist");
        }
    }
}
