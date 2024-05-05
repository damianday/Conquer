using System;
using GameServer.Database;

namespace GameServer;

public sealed class UnblockAccount : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string AccountName;

    public override ExecuteCondition Priority => ExecuteCondition.Normal;
    public override UserDegree Degree => UserDegree.SysOp;

    public override void ExecuteCommand()
    {
        if (Session.AccountInfoTable.SearchTable.TryGetValue(AccountName, out var value) && value is AccountInfo account)
        {
            account.BlockDate.V = default(DateTime);
            SMain.AddCommandLog($"<= @{GetType().Name} The command has been executed, and the ban expires: {account.BlockDate}");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " The command fails to be executed, and the account does not exist");
        }
    }
}
