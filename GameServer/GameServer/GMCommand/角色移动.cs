using System.Drawing;
using System.Linq;
using GameServer.Map;
using GameServer.Database;
using GameServer.Template;

namespace GameServer;

public sealed class 角色移动 : GMCommand
{
    [FieldDescription(0)]
    public string 角色名字;

    [FieldDescription(1)]
    public byte 地图编号;

    [FieldDescription(2)]
    public int 地图X;

    [FieldDescription(3)]
    public int 地图Y;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (!Session.CharacterInfoTable.SearchTable.TryGetValue(角色名字, out var value))
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败，角色 " + 角色名字 + " 不存在");
            return;
        }
        if (!GameMap.DataSheet.TryGetValue(地图编号, out var value2))
        {
            SMain.AddCommandLog($"<= @{GetType().Name} 命令执行失败，地图 {地图编号} 不存在");
            return;
        }
        PlayerObject 玩家实例 = ((!(value is CharacterInfo 角色数据)) ? null : 角色数据.Connection?.Player);
        if (玩家实例 == null)
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败，角色 " + 角色名字 + " 不在线");
            return;
        }
        Map.Map 地图实例 = MapManager.GetMap(value2.MapID);
        MapArea 地图区域 = 地图实例.传送区域 ?? 地图实例.Areas.FirstOrDefault();
        Point 坐标 = ((地图X != 0 && 地图Y != 0) ? new Point(地图X, 地图Y) : (地图区域?.RandomCoords ?? Point.Empty));
        if (坐标.IsEmpty)
        {
            for (int i = 1; i < 地图实例.MapSize.X; i++)
            {
                for (int j = 1; j < 地图实例.MapSize.Y; j++)
                {
                    if (地图实例.CanMove(new Point(i, j)))
                    {
                        坐标 = new Point(i, j);
                        break;
                    }
                }
            }
        }
        玩家实例.Teleport(地图实例, 地图区域?.RegionType ?? AreaType.Unknown, 坐标);
    }
}
