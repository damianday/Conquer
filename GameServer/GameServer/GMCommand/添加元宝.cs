using GameServer.Database;
using GameServer.Networking;

using GamePackets.Server;

namespace GameServer;

public sealed class 添加元宝 : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string UserName;

    [FieldDescription(0, Index = 1)]
    public int 元宝数量;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (Session.CharacterInfoTable.SearchTable.TryGetValue(UserName, out var value) && value is CharacterInfo character)
        {
            character.Ingot += 元宝数量;
            character.Enqueue(new 同步元宝数量
            {
                元宝数量 = character.Ingot
            });
            SMain.AddCommandLog($"<= @{GetType().Name} 命令已经执行, 当前元宝数量: {character.Ingot}");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 角色不存在");
        }
    }
}
