using System.Drawing;
using System.Linq;
using GameServer.Map;
using GameServer.Database;
using GameServer.Template;

namespace GameServer;

public sealed class CharacterTeleport : GMCommand
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

    public override void ExecuteCommand()
    {
        var character = Session.GetCharacter(UserName);
        if (character == null)
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " Command execution failed, the character " + UserName + " does not exist");
            return;
        }
        if (!GameMap.DataSheet.TryGetValue(MapID, out var value2))
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

        var map = MapManager.GetMap(value2.MapID);
        var area = map.TeleportationArea ?? map.Areas.FirstOrDefault();
        var location = ((MapX != 0 && MapY != 0) ? new Point(MapX, MapY) : (area?.RandomCoords ?? Point.Empty));
        if (location.IsEmpty)
        {
            for (var x = 1; x < map.MapSize.X; x++)
            {
                for (var y = 1; y < map.MapSize.Y; y++)
                {
                    if (map.CanMove(new Point(x, y)))
                    {
                        location = new Point(x, y);
                        break;
                    }
                }
            }
        }
        player.Teleport(map, area?.RegionType ?? AreaType.Unknown, location);
    }
}
