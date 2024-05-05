using System.Threading.Tasks;
using System.Windows.Forms;
using GameServer.Database;

namespace GameServer;

public sealed class PurgeUsers : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public int RestrictionLevel;

    [FieldDescription(0, Index = 1)]
    public int Days;

    public override ExecuteCondition Priority => ExecuteCondition.Inactive;
    public override UserDegree Degree => UserDegree.SysOp;

    public override void ExecuteCommand()
    {
        var msg = $"We will be permanently deleting the data of all characters whose level is less than [{RestrictionLevel}] and who have not logged in for [{Days}] days.\r\n\r\n" +
                   "This operation is irreversible, please make a good backup of your data.\r\n\r\n" +
                   "Are you sure you want to carry it out?";
        if (MessageBox.Show(msg, "Dangerous operation", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
            return;

        SMain.AddCommandLog("<= @" + GetType().Name + " Start executing the command, do not close the window during the process.");
        SMain.Main.BeginInvoke(() =>
        {
            SMain.Main.SettingsPage.Enabled = false;
            SMain.Main.下方控件页.Enabled = false;
        });
        Task.Run(delegate
        {
            Session.PurgeUsers(RestrictionLevel, Days);
            SMain.Main.BeginInvoke(() =>
            {
                SMain.Main.SettingsPage.Enabled = true;
                SMain.Main.下方控件页.Enabled = true;
            });
        });
    }
}
