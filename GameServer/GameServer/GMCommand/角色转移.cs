using System;
using GameServer.Database;

namespace GameServer;

public sealed class 角色转移 : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string 角色名字;

    [FieldDescription(0, Index = 1)]
    public string 新账号名;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (Session.CharacterInfoTable.SearchTable.TryGetValue(角色名字, out var value) && value is CharacterInfo 角色数据)
        {
            DBObject value2;
            if (!角色数据.Account.V.Characters.Contains(角色数据))
            {
                SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 角色处于删除状态");
            }
            else if (角色数据.BlockDate.V > DateTime.Now)
            {
                SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 角色处于封禁状态");
            }
            else if (角色数据.Account.V.BlockDate.V > DateTime.Now)
            {
                SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 原账号处于封禁状态");
            }
            else if (Session.AccountInfoTable.SearchTable.TryGetValue(新账号名, out value2) && value2 is AccountInfo 账号数据)
            {
                if (账号数据.BlockDate.V > DateTime.Now)
                {
                    SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 转移账号处于封禁状态");
                }
                else if (账号数据.Characters.Count >= 4)
                {
                    SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 转移的账号角色数量已达上限");
                }
                else if (角色数据.Account.V.Connection == null && 账号数据.Connection == null)
                {
                    角色数据.Account.V.Characters.Remove(角色数据);
                    角色数据.Account.V = 账号数据;
                    账号数据.Characters.Add(角色数据);
                    SMain.AddCommandLog($"<= @{GetType().Name} 命令已经执行, 角色当前账号:{角色数据.Account}");
                }
                else
                {
                    SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 双方账号必须下线");
                }
            }
            else
            {
                SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 转移账号不存在或从未登录");
            }
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 角色不存在");
        }
    }
}
