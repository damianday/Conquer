using System;
using System.Collections.Generic;

namespace GameServer.Map;

public sealed class HateObject
{
    public sealed class HateInfo
    {
        public int 仇恨数值;
        public DateTime HateTime;

        public HateInfo(DateTime 仇恨时间, int 仇恨数值)
        {
            this.仇恨数值 = 仇恨数值;
            this.HateTime = 仇恨时间;
        }
    }

    public MapObject Target;
    public DateTime 切换时间;
    public readonly Dictionary<MapObject, HateInfo> TargetList;

    public HateObject()
    {
        TargetList = new Dictionary<MapObject, HateInfo>();
    }

    public bool Remove(MapObject target)
    {
        if (Target == target)
            Target = null;
        return TargetList.Remove(target);
    }

    public void Add(MapObject target, DateTime 时间, int 仇恨数值)
    {
        if (!target.Dead)
        {
            if (TargetList.TryGetValue(target, out var value))
            {
                value.HateTime = ((value.HateTime < 时间) ? 时间 : value.HateTime);
                value.仇恨数值 += 仇恨数值;
            }
            else
            {
                TargetList[target] = new HateInfo(时间, 仇恨数值);
            }
        }
    }

    public bool 切换仇恨(MapObject master)
    {
        int num = int.MinValue;
        var targets = new List<MapObject>();
        foreach (var kvp in TargetList)
        {
            if (kvp.Value.仇恨数值 > num)
            {
                num = kvp.Value.仇恨数值;
                targets = new List<MapObject> { kvp.Key };
            }
            else if (kvp.Value.仇恨数值 == num)
            {
                targets.Add(kvp.Key);
            }
        }
        if (num == 0 && Target != null)
            return true;

        int dist = int.MaxValue;
        MapObject nearest = null;
        foreach (var obj in targets)
        {
            int d = master.GetDistance(obj);
            if (d < dist)
            {
                dist = d;
                nearest = obj;
            }
        }
        (nearest as PlayerObject)?.玩家获得仇恨(master);
        return (Target = nearest) != null;
    }

    public bool 最近仇恨(MapObject master)
    {
        int num = int.MaxValue;
        List<KeyValuePair<MapObject, HateInfo>> list = new List<KeyValuePair<MapObject, HateInfo>>();
        foreach (KeyValuePair<MapObject, HateInfo> item in TargetList)
        {
            int num2 = master.GetDistance(item.Key);
            if (num2 < num)
            {
                num = num2;
                list = new List<KeyValuePair<MapObject, HateInfo>> { item };
            }
            else if (num2 == num)
            {
                list.Add(item);
            }
        }
        int num3 = int.MinValue;
        MapObject 地图对象2 = null;
        foreach (KeyValuePair<MapObject, HateInfo> item2 in list)
        {
            if (item2.Value.仇恨数值 > num3)
            {
                num3 = item2.Value.仇恨数值;
                地图对象2 = item2.Key;
            }
        }
        (地图对象2 as PlayerObject)?.玩家获得仇恨(master);
        return (Target = 地图对象2) != null;
    }
}
