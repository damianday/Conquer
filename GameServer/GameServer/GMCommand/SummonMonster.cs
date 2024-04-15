using System;
using System.Drawing;
using GameServer.Map;
using GameServer.Template;

namespace GameServer;

public sealed class SummonMonster : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string Name;

    [FieldDescription(0, Index = 1)]
    public byte MapID;

    [FieldDescription(0, Index = 2)]
    public int MapX;

    [FieldDescription(0, Index = 3)]
    public int MapY;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        GameMap value2;
        if (!MonsterInfo.DataSheet.TryGetValue(Name, out var moni))
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " Command execution failed, monster " + Name + " does not exist");
        }
        else if (!GameMap.DataSheet.TryGetValue(MapID, out value2))
        {
            SMain.AddCommandLog($"<= @{GetType().Name} Command execution failed, map {MapID} does not exist");
        }
        else
        {
            var map = MapManager.GetMap(value2.MapID);
            new MonsterObject(moni, map, int.MaxValue, new Point[1]
            {
                new Point(MapX, MapY)
            }, forbidResurrection: true, true).SurvivalTime = DateTime.MaxValue;
        }
    }
}
