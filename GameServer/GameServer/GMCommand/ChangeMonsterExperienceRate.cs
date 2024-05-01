using System.Windows.Forms;

namespace GameServer;

public sealed class ChangeMonsterExperienceRate : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public decimal ExperienceRate;

    public override ExecuteCondition Priority => ExecuteCondition.Background;

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
        Settings.MonsterExperienceMultiplier = ExperienceRate;
        Settings.Save();
        SMain.Main.BeginInvoke(() =>
        {
            SMain.Main.S_MonsterExperienceMultiplier.Value = ExperienceRate;
        });
        SMain.AddCommandLog($"<= @{GetType().Name} The command has been executed, and the current experience multiplier:{Settings.MonsterExperienceMultiplier}");
    }
}
