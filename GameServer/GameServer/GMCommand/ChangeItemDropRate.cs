using System.Windows.Forms;

namespace GameServer;

public sealed class ChangeItemDropRate : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public decimal DropRate;

    public override ExecuteCondition Priority => ExecuteCondition.Background;
    public override UserDegree Degree => UserDegree.SysOp;

    public override void ExecuteCommand()
    {
        if (DropRate < 0m)
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " Failed to execute command. Rate multiplier is too low.");
            return;
        }
        if (DropRate >= 1m)
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " Failed to execute command. Rate multiplier is too high.");
            return;
        }
        Settings.Default.ItemDropRate = DropRate;
        Settings.Default.Save();
        SMain.Main.BeginInvoke(() =>
        {
            SMain.Main.S_ItemDropRate.Value = DropRate;
        });
        SMain.AddCommandLog($"<= @{GetType().Name} The command has been executed, the current drop rate:{Settings.Default.ItemDropRate}");
    }
}
