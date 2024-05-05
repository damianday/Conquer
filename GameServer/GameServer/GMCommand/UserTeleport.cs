using System.Drawing;
using GameServer.Map;
using GameServer.Database;

namespace GameServer;

public sealed class UserTeleport : GMCommand
{
    [FieldDescription(0)]
    public string UserName;

    [FieldDescription(1)]
    public byte MapID;

    [FieldDescription(2)]
    public int MapX;

    [FieldDescription(3)]
    public int MapY;

    public override ExecuteCondition Priority => ExecuteCondition.Normal;
    public override UserDegree Degree => UserDegree.Admin;

    public override void ExecuteCommand()
    {
        var character = Session.GetCharacter(UserName);
        if (character == null)
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " Command execution failed, the character " + UserName + " does not exist");
            return;
        }

        var map = MapManager.GetMap(MapID);
        if (map == null)
        {
            SMain.AddCommandLog($"<= @{GetType().Name} Command execution failed, map {MapID} does not exist");
            return;
        }

        var player = character.Connection?.Player;
        if (player == null)
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " Command execution failed, the character " + UserName + " is offline");
            return;
        }

        var location = (MapX == 0 && MapY == 0) ? map.GetRandomPosition(AreaType.Random) : new Point(MapX, MapY);
        var found = true;
        if (location.IsEmpty)
        {
            found = false;
            for (var i = 0; i < 100; i++)
            {
                var point = Compute.GetPositionAround(location, i);
                if (!map.CanMove(point))
                {
                    location = point;
                    found = true;
                    break;
                }
            }
        }

        if (!found)
        {
            SMain.AddCommandLog($"<= @{GetType().Name} Command execution failed, cannot teleport to {location}");
            return;
        }

        player.Teleport(map, AreaType.Unknown, location);
    }
}
