using System;
using GameServer.Database;

namespace GameServer;

public sealed class BlockAccount : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string AccountName;

    [FieldDescription(0, Index = 1)]
    public float BanDays;

    public override ExecuteCondition Priority => ExecuteCondition.Normal;

    public override void ExecuteCommand()
    {
        if (Session.AccountInfoTable.SearchTable.TryGetValue(AccountName, out var data) && data is AccountInfo account)
        {
            account.BlockDate.V = DateTime.Now.AddDays(BanDays);
            account.Connection?.Disconnect(new Exception("The account was banned and forcibly taken offline"));
            SMain.AddCommandLog($"<= @{GetType().Name} command has been executed, BlockDate: {account.BlockDate}");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " command execution failed, account does not exist");
        }
    }
}
