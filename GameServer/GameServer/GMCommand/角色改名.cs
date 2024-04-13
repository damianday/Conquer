using System.Text;
using GameServer.Database;

namespace GameServer;

public sealed class 角色改名 : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string 角色名字;

    [FieldDescription(0, Index = 1)]
    public string 新角色名;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (Session.CharacterInfoTable.SearchTable.TryGetValue(角色名字, out var value) && value is CharacterInfo 角色数据)
        {
            if (角色数据.Connection == null && 角色数据.Account.V.Connection == null)
            {
                if (Encoding.UTF8.GetBytes(新角色名).Length > 24)
                {
                    SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 角色名字太长");
                    return;
                }
                if (Session.CharacterInfoTable.SearchTable.ContainsKey(新角色名))
                {
                    SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 名字已被注册");
                    return;
                }
                Session.CharacterInfoTable.SearchTable.Remove(角色数据.UserName.V);
                角色数据.UserName.V = 新角色名;
                Session.CharacterInfoTable.SearchTable.Add(角色数据.UserName.V, 角色数据);
                SMain.AddCommandLog($"<= @{GetType().Name} 命令已经执行, 角色当前名字: {角色数据}");
            }
            else
            {
                SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 账号必须下线");
            }
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 角色不存在");
        }
    }
}
