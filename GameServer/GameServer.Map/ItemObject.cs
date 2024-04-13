using System;
using System.Collections.Generic;
using System.Drawing;
using GameServer.Database;
using GameServer.Template;

namespace GameServer.Map;

public sealed class ItemObject : MapObject
{
    public ItemInfo Item;
    public GameItem Info;
    public int Quantity;
    public bool IsBound;
    public DateTime DisappearTime;
    public DateTime 归属时间;
    public HashSet<CharacterInfo> ItemAttribution;

    public override Map CurrentMap
    {
        get { return base.CurrentMap; }
        set
        {
            if (CurrentMap != value)
            {
                base.CurrentMap?.RemoveObject(this);
                base.CurrentMap = value;
                base.CurrentMap.AddObject(this);
            }
        }
    }

    public override int ProcessInterval => 100;
    public override bool Dead => false;
    public override bool Blocking => false;
    public override bool CanBeHit => false;
    public override GameObjectType ObjectType => GameObjectType.Item;
    public override ObjectSize Size => ObjectSize.Single1x1;
    public PersistentItemType 持久类型 => Info.PersistType;
    public int MaxDura => Info.MaxDura;
    public int ItemID => Info?.ID ?? 0;

    public int Weight
    {
        get
        {
            if (Info.PersistType != PersistentItemType.Stack)
                return Info.Weight;
            return Info.Weight * Quantity;
        }
    }

    public bool CanStack => Info.PersistType == PersistentItemType.Stack;

    public ItemObject(GameItem info, ItemInfo item, Map map, Point location, HashSet<CharacterInfo> attribution, int quantity = 0, bool bind = false)
    {
        ItemAttribution = attribution;
        Info = info;
        Item = item;
        CurrentMap = map;
        Item = item;
        Quantity = quantity;
        IsBound = info.IsBound || bind;

        Point point = location;
        int max = int.MaxValue;
        for (int i = 0; i <= 120; i++)
        {
            int curr = 0;
            Point point2 = Compute.GetPositionAround(location, i);
            if (!map.ValidTerrain(point2))
                continue;

            foreach (var obj in map[point2])
            {
                if (!obj.Dead)
                {
                    switch (obj.ObjectType)
                    {
                        case GameObjectType.Item:
                            curr += 100;
                            break;
                        case GameObjectType.Player:
                            curr += 10000;
                            break;
                        case GameObjectType.Pet:
                        case GameObjectType.Monster:
                        case GameObjectType.NPC:
                            curr += 1000;
                            break;
                    }
                }
            }
            if (curr == 0)
            {
                point = point2;
                break;
            }
            if (curr < max)
            {
                point = point2;
                max = curr;
            }
        }
        CurrentPosition = point;
        DisappearTime = SEngine.CurrentTime.AddMinutes(Config.ItemDisappearTime);
        归属时间 = SEngine.CurrentTime.AddMinutes((int)Config.物品归属时间);
        ObjectID = ++MapManager.ItemObjectID;

        BindGrid();
        UpdateAllNeighbours();
        MapManager.AddObject(this);
        SecondaryObject = true;
        MapManager.AddSecondaryObject(this);
    }

    public override void Process()
    {
        if (SEngine.CurrentTime > DisappearTime)
            DestroyItem();
    }

    public void DestroyItem()
    {
        Item?.Remove();
        Despawn();
    }

    public void PickedUp()
    {
        Despawn();
    }
}
