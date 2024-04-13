using System;
using GameServer.Database;

namespace GameServer;

public sealed class 找回角色 : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string 角色名字;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (Session.CharacterInfoTable.SearchTable.TryGetValue(角色名字, out var value) && value is CharacterInfo 角色数据)
        {
            if (!(角色数据.DeletetionDate.V == default(DateTime)) && 角色数据.Account.V.DeletedCharacters.Contains(角色数据))
            {
                if (角色数据.Account.V.Characters.Count >= 4)
                {
                    SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 角色列表已满");
                    return;
                }
                if (角色数据.Account.V.Connection != null)
                {
                    SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 账号必须下线");
                    return;
                }
                DataMonitor<DateTime> 删除日期 = 角色数据.DeletetionDate;
                DateTime dateTime2 = (角色数据.FrozenDate.V = default(DateTime));
                DateTime dateTime3 = dateTime2;
                dateTime2 = (删除日期.V = dateTime3);
                DateTime dateTime5 = dateTime2;
                DateTime dateTime6 = dateTime5;
                角色数据.Account.V.DeletedCharacters.Remove(角色数据);
                角色数据.Account.V.Characters.Add(角色数据);
                SMain.AddCommandLog("<= @" + GetType().Name + " 命令已经执行, 角色恢复成功");
            }
            else
            {
                SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 角色未被删除");
            }
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 角色不存在");
        }
    }
}
