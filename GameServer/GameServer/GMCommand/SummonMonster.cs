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

    public override ExecuteCondition Priority => ExecuteCondition.Normal;

    public override void ExecuteCommand()
    {
        if (!MonsterInfo.DataSheet.TryGetValue(Name, out var moni))
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " Command execution failed, monster " + Name + " does not exist");
            return;
        }

        var map = MapManager.GetMap(MapID);
        if (map == null)
        {
            SMain.AddCommandLog($"<= @{GetType().Name} Command execution failed, map {MapID} does not exist");
            return;
        }

        new MonsterObject(moni, map, int.MaxValue, new Point(MapX, MapY), 1,
            forbidResurrection: true, true).SurvivalTime = DateTime.MaxValue;
    }
}
