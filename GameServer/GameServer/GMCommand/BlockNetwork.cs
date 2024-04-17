using System;
using System.Text.RegularExpressions;
using GameServer.Database;

namespace GameServer;

public sealed class BlockNetwork : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string MACAddress;

    [FieldDescription(0, Index = 1)]
    public float Days;

    public override ExecuteCondition Priority => ExecuteCondition.Normal;

    public override void ExecuteCommand()
    {
        if (Regex.IsMatch(MACAddress, "^([0-9a-fA-F]{2}(?:[:-]?[0-9a-fA-F]{2}){5})$"))
        {
            SystemInfo.Info.AddNICBan(MACAddress, DateTime.Now.AddDays(Days));
            SMain.AddCommandLog($"<= @{GetType().Name} The command has been executed, and the ban expires: {DateTime.Now.AddDays(Days)}");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " The command failed to be executed, and the address format was incorrect");
        }
    }
}
