using GameServer.Database;

namespace GameServer;

public sealed class 解封地址 : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string 对应地址;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (SystemInfo.Info.IPBans.ContainsKey(对应地址))
        {
            SystemInfo.Info.RemoveIPBan(对应地址);
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令已经执行, 地址已经解除封禁");
        }
        else if (SystemInfo.Info.NICBans.ContainsKey(对应地址))
        {
            SystemInfo.Info.RemoveNICBan(对应地址);
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令已经执行, 地址已经解除封禁");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 对应地址未处于封禁状态");
        }
    }
}
