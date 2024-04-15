using GameServer.Database;
using GameServer.Template;
using GameServer.Networking;

using GamePackets.Server;

namespace GameServer;

public sealed class AddTitle : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string UserName;

    [FieldDescription(0, Index = 1)]
    public byte TitleID;

    [FieldDescription(0, Index = 2)]
    public int Duration;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (!Session.CharacterInfoTable.SearchTable.TryGetValue(UserName, out var value))
            return;

        if (value is CharacterInfo character)
        {
            if (GameTitle.DataSheet.TryGetValue(TitleID, out var value2))
            {
                character.Titles[TitleID] = SEngine.CurrentTime.AddMinutes(value2.EffectiveTime);
                character.Enqueue(new AddTitlePacket
                {
                    TitleID = TitleID,
                    Duration = Duration
                });
            }
            SMain.AddCommandLog("<= @" + GetType().Name + " The command has been executed and the title has been added to the role");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " The command execution failed and the title does not exist");
        }
    }
}
