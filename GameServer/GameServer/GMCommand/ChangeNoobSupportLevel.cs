using System.Windows.Forms;

namespace GameServer;

public sealed class ChangeNoobSupportLevel : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public byte Level;

    public override ExecuteCondition Priority => ExecuteCondition.Background;
    public override UserDegree Degree => UserDegree.SysOp;

    public override void ExecuteCommand()
    {
        Settings.Default.NoobSupportLevel = Level;
        Settings.Default.Save();
        SMain.Main.BeginInvoke(() =>
        {
            SMain.Main.S_NoobSupportLevel.Value = Level;
        });
        SMain.AddCommandLog($"<= @{GetType().Name} The command has been executed, the current support level:{Settings.Default.NoobSupportLevel}");
    }
}
