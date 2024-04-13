using System.Windows.Forms;
using GameServer.Properties;

namespace GameServer;

public sealed class 设置爆率 : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public decimal 额外爆率;

    public override ExecutionPriority Priority => ExecutionPriority.Background;

    public override void ExecuteCommand()
    {
        if (额外爆率 < 0m)
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 额外爆率太低");
            return;
        }
        if (额外爆率 >= 1m)
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 额外爆率太高");
            return;
        }
        Settings.Default.怪物额外爆率 = (Config.怪物额外爆率 = 额外爆率);
        Settings.Default.Save();
        SMain.Main.BeginInvoke((MethodInvoker)delegate
        {
            SMain.Main.S_怪物额外爆率.Value = 额外爆率;
        });
        SMain.AddCommandLog($"<= @{GetType().Name} 命令已经执行, 当前额外爆率:{Config.怪物额外爆率}");
    }
}
