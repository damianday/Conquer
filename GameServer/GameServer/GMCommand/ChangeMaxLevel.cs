using System.Windows.Forms;
using GameServer.Properties;

namespace GameServer;

public sealed class ChangeMaxLevel : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public byte MaxLevel;

    public override ExecuteCondition Priority => ExecuteCondition.Background;

    public override void ExecuteCommand()
    {
        if (MaxLevel <= Config.MaxUserLevel)
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " The command execution failed, and the level is lower than the current max level");
            return;
        }
        Config.MaxUserLevel = MaxLevel;
        Config.Save();
        SMain.Main.BeginInvoke((MethodInvoker)delegate
        {
            SMain.Main.S_MaxUserLevel.Value = MaxLevel;
        });
        SMain.AddCommandLog($"<= @{GetType().Name} The command has been executed, and the current max level is open: {Config.MaxUserLevel}");
    }
}
