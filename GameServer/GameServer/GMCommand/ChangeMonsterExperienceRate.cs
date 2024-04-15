using System.Windows.Forms;
using GameServer.Properties;

namespace GameServer;

public sealed class ChangeMonsterExperienceRate : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public decimal ExperienceRate;

    public override ExecutionPriority Priority => ExecutionPriority.Background;

    public override void ExecuteCommand()
    {
        if (ExperienceRate <= 0m)
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " Command execution failed, and the experience multiplier was too low");
            return;
        }
        if (ExperienceRate > 1000000m)
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " The command execution failed, and the experience multiplier was too high");
            return;
        }
        Settings.Default.MonsterExperienceMultiplier = (Config.MonsterExperienceMultiplier = ExperienceRate);
        Settings.Default.Save();
        SMain.Main.BeginInvoke((MethodInvoker)delegate
        {
            SMain.Main.S_MonsterExperienceMultiplier.Value = ExperienceRate;
        });
        SMain.AddCommandLog($"<= @{GetType().Name} The command has been executed, and the current experience multiplier:{Config.MonsterExperienceMultiplier}");
    }
}
