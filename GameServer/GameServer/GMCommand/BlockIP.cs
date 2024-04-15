using System;
using System.Net;
using GameServer.Database;

namespace GameServer;

public sealed class BlockIP : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string IPAddress;

    [FieldDescription(0, Index = 1)]
    public float Days;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (System.Net.IPAddress.TryParse(IPAddress, out var _))
        {
            SystemInfo.Info.AddIPBan(IPAddress, DateTime.Now.AddDays(Days));
            SMain.AddCommandLog($"<= @{GetType().Name} The command has been executed, and the ban expires: {DateTime.Now.AddDays(Days)}");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " The command failed to be executed, and the address format was incorrect");
        }
    }
}
