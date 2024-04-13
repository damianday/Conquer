using GameServer.Database;
using GameServer.Template;
using GameServer.Networking;

using GamePackets.Server;

namespace GameServer;

public sealed class 添加称号 : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string 角色名字;

    [FieldDescription(0, Index = 1)]
    public byte 称号编号;

    [FieldDescription(0, Index = 2)]
    public int 持续时间;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (!Session.CharacterInfoTable.SearchTable.TryGetValue(角色名字, out var value))
        {
            return;
        }
        if (value is CharacterInfo character)
        {
            SConnection 网络连接 = character.Connection;
            if (GameTitle.DataSheet.TryGetValue(称号编号, out var value2))
            {
                character.Titles[称号编号] = SEngine.CurrentTime.AddMinutes(value2.EffectiveTime);
                网络连接?.SendPacket(new 玩家获得称号
                {
                    称号编号 = 称号编号,
                    剩余时间 = 持续时间
                });
            }
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令已经执行, 称号已经添加到角色");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 称号不存在");
        }
    }
}
