using System.Threading.Tasks;
using System.Windows.Forms;
using GameServer.Database;

namespace GameServer;

public sealed class 清理角色 : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public int 限制等级;

    [FieldDescription(0, Index = 1)]
    public int 限制天数;

    public override ExecutionPriority Priority => ExecutionPriority.Idle;

    public override void ExecuteCommand()
    {
        if (MessageBox.Show($"即将永久删除所有等级小于[{限制等级}]级且[{限制天数}]天内未登录的角色数据\r\n\r\n此操作不可逆, 请做好数据备份\r\n\r\n确定要执行吗?", "危险操作", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
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
            Session.清理角色(限制等级, 限制天数);
            SMain.Main.BeginInvoke((MethodInvoker)delegate
            {
                Panel 下方控件页 = SMain.Main.下方控件页;
                SMain.Main.SettingsPage.Enabled = true;
                下方控件页.Enabled = true;
            });
        });
    }
}
