using System;
using GameServer.Database;
using GameServer.Networking;

using GamePackets.Server;

namespace GameServer;

public sealed class RemoveIngot : GMCommand
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
            character.Ingot = Math.Max(0, character.Ingot - Amount);
            character.Enqueue(new SyncIngotsPacket
            {
                Amount = character.Ingot
            });
            SMain.AddCommandLog($"<= @{GetType().Name} The command has been executed, the current number of ingots: {character.Ingot}");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " The command execution failed and the character does not exist");
        }
    }
}
