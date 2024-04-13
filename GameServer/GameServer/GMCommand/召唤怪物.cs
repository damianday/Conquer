using System;
using System.Drawing;
using GameServer.Map;
using GameServer.Template;

namespace GameServer;

public sealed class 召唤怪物 : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string 怪物名字;

    [FieldDescription(0, Index = 1)]
    public byte 地图编号;

    [FieldDescription(0, Index = 2)]
    public int 地图X;

    [FieldDescription(0, Index = 3)]
    public int 地图Y;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        GameMap value2;
        if (!MonsterInfo.DataSheet.TryGetValue(怪物名字, out var value))
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败，怪物 " + 怪物名字 + " 不存在");
        }
        else if (!GameMap.DataSheet.TryGetValue(地图编号, out value2))
        {
            SMain.AddCommandLog($"<= @{GetType().Name} 命令执行失败，地图 {地图编号} 不存在");
        }
        else
        {
            Map.Map 出生地图 = MapManager.GetMap(value2.MapID);
            new MonsterObject(value, 出生地图, int.MaxValue, new Point[1]
            {
                new Point(地图X, 地图Y)
            }, forbidResurrection: true, 立即刷新: true).SurvivalTime = DateTime.MaxValue;
        }
    }
}
