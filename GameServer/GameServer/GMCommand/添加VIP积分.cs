using GameServer.Database;

namespace GameServer;

public sealed class 添加VIP积分 : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string 角色名字;

    [FieldDescription(0, Index = 1)]
    public int VIP积分数量;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (Session.CharacterInfoTable.SearchTable.TryGetValue(角色名字, out var value) && value is CharacterInfo 角色数据)
        {
            角色数据.VIPPoints.V += VIP积分数量;
            SMain.AddCommandLog($"<= @{GetType().Name} 命令已经执行, 当前VIP积分: {角色数据.VIPPoints.V}");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 角色不存在");
        }
    }
}
