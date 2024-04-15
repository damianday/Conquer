using GameServer.Database;

namespace GameServer;

public sealed class UnblockAddress : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string Address;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (SystemInfo.Info.IPBans.ContainsKey(Address))
        {
            SystemInfo.Info.RemoveIPBan(Address);
            SMain.AddCommandLog("<= @" + GetType().Name + " The command has been executed, and the address has been unblocked");
        }
        else if (SystemInfo.Info.NICBans.ContainsKey(Address))
        {
            SystemInfo.Info.RemoveNICBan(Address);
            SMain.AddCommandLog("<= @" + GetType().Name + " The command has been executed, and the address has been unblocked");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " The command failed to be executed, and the corresponding address is not in the blocked state");
        }
    }
}
