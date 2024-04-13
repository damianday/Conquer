using System.Windows.Forms;
using GameServer.Properties;

namespace GameServer;

public sealed class 设置经验 : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public decimal 经验倍率;

    public override ExecutionPriority Priority => ExecutionPriority.Background;

    public override void ExecuteCommand()
    {
        if (经验倍率 <= 0m)
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 经验倍率太低");
            return;
        }
        if (经验倍率 > 1000000m)
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 经验倍率太高");
            return;
        }
        Settings.Default.MonsterExperienceMultiplier = (Config.MonsterExperienceMultiplier = 经验倍率);
        Settings.Default.Save();
        SMain.Main.BeginInvoke((MethodInvoker)delegate
        {
            SMain.Main.S_怪物经验倍率.Value = 经验倍率;
        });
        SMain.AddCommandLog($"<= @{GetType().Name} 命令已经执行, 当前经验倍率:{Config.MonsterExperienceMultiplier}");
    }
}
