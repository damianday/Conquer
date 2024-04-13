using System.Windows.Forms;
using GameServer.Properties;

namespace GameServer;

public sealed class ChangeNoobSupportLevel : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public byte Level;

    public override ExecutionPriority Priority => ExecutionPriority.Background;

    public override void ExecuteCommand()
    {
        Settings.Default.NoobSupportLevel = (Config.NoobSupportLevel = Level);
        Settings.Default.Save();
        SMain.Main.BeginInvoke((MethodInvoker)delegate
        {
            SMain.Main.S_NoobSupportLevel.Value = Level;
        });
        SMain.AddCommandLog($"<= @{GetType().Name} The command has been executed, the current support level:{Config.NoobSupportLevel}");
    }
}
