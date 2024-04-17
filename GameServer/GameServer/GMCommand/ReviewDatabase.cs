using System;
using System.Collections.Generic;
using GameServer.Database;

namespace GameServer;

public sealed class ReviewDatabase : GMCommand
{
    public override ExecuteCondition Priority => ExecuteCondition.Normal;

    public override void ExecuteCommand()
    {
        SMain.AddCommandLog("<= @" + GetType().Name + " The command has been executed, and the database details are as follows:");
        foreach (var table in Session.Tables)
        {
            SMain.AddCommandLog($"{table.Value.Type.Name}  Count: {table.Value.DataSheet.Count}");
        }
    }
}
