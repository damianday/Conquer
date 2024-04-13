using System.Threading.Tasks;
using System.Windows.Forms;
using GameServer.Database;

namespace GameServer;

public sealed class 整理数据 : GMCommand
{
    public override ExecutionPriority Priority => ExecutionPriority.Idle;

    public override void ExecuteCommand()
    {
        if (MessageBox.Show("整理数据需要重新排序所有客户数据以便节省ID资源\r\n\r\n此操作不可逆, 请做好数据备份\r\n\r\n确定要执行吗?", "危险操作", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
        {
            return;
        }
        SMain.AddCommandLog("<= @" + GetType().Name + " 开始执行命令, 过程中请勿关闭窗口");
        SMain.Main.BeginInvoke((MethodInvoker)delegate
        {
            Panel 下方控件页2 = SMain.Main.下方控件页;
            SMain.Main.SettingsPage.Enabled = false;
            下方控件页2.Enabled = false;
        });
        Task.Run(delegate
        {
            Session.Save(commit: true);
            SMain.Main.BeginInvoke((MethodInvoker)delegate
            {
                Panel 下方控件页 = SMain.Main.下方控件页;
                SMain.Main.SettingsPage.Enabled = true;
                下方控件页.Enabled = true;
            });
        });
    }
}
