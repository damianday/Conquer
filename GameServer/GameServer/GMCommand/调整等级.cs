using GameServer.Database;

using GamePackets.Server;

namespace GameServer;

public sealed class 调整等级 : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string UserName;

    [FieldDescription(0, Index = 1)]
    public int Level;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (Session.CharacterInfoTable.SearchTable.TryGetValue(UserName, out var value) && value is CharacterInfo character)
        {
            character.CurrentLevel = (byte)Level;
            character.Enqueue(new ObjectLevelUpPacket
            {
                ObjectID = character.Connection.Player.ObjectID,
                CurrentLevel = character.CurrentLevel
            });
            SMain.AddCommandLog($"<= @{GetType().Name} 命令已经执行, 当前角色等级: {character.CurrentLevel}");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 角色不存在");
        }
    }
}
