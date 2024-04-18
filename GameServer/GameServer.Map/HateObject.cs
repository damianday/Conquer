using System;
using System.Collections.Generic;

namespace GameServer.Map;

public sealed class HateObject
{
    public sealed class HateInfo
    {
        public int Priority;
        public DateTime HateTime;

        public HateInfo(DateTime duration, int priority)
        {
            Priority = priority;
            HateTime = duration;
        }
    }

    public MapObject Target;
    public DateTime SelectTargetTime;
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

    public void Add(MapObject target, DateTime duration, int priority)
    {
        if (!target.Dead)
        {
            if (TargetList.TryGetValue(target, out var value))
            {
                value.HateTime = ((value.HateTime < duration) ? duration : value.HateTime);
                value.Priority += priority;
            }
            else
            {
                TargetList[target] = new HateInfo(duration, priority);
            }
        }
    }

    public bool SelectTarget(MapObject master)
    {
        var priority = int.MinValue;
        var targets = new List<MapObject>();
        foreach (var kvp in TargetList)
        {
            if (kvp.Value.Priority > priority)
            {
                priority = kvp.Value.Priority;
                targets = new List<MapObject> { kvp.Key };
            }
            else if (kvp.Value.Priority == priority)
                targets.Add(kvp.Key);
        }
        if (priority == 0 && Target != null)
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
        (nearest as PlayerObject)?.AddTarget(master);
        return (Target = nearest) != null;
    }

    public bool SelectBestTarget(MapObject master)
    {
        int dist = int.MaxValue;
        List<KeyValuePair<MapObject, HateInfo>> list = new List<KeyValuePair<MapObject, HateInfo>>();
        foreach (var kvp in TargetList)
        {
            int d = master.GetDistance(kvp.Key);
            if (d < dist)
            {
                dist = d;
                list = new List<KeyValuePair<MapObject, HateInfo>> { kvp };
            }
            else if (d == dist)
                list.Add(kvp);
        }

        int priority = int.MinValue;
        MapObject best = null;
        foreach (var kvp in list)
        {
            if (kvp.Value.Priority > priority)
            {
                priority = kvp.Value.Priority;
                best = kvp.Key;
            }
        }
        (best as PlayerObject)?.AddTarget(master);
        return (Target = best) != null;
    }
}
