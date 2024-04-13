using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using GameServer.Database;
using GameServer.Template;
using GameServer.Networking;

using GamePackets.Server;

namespace GameServer.Map;

public sealed class GuardObject : MapObject
{
    public GuardInfo Info;

    public HateObject Target;

    public Point BirthPosition;

    public GameDirection BirthDirection;

    public Map BirthMap;

    public GameSkill BasicAttackSkill;

    public bool Disappeared { get; set; }

    public DateTime ResurrectionTime { get; set; }
    public DateTime DisappearTime { get; set; }
    public DateTime 转移计时 { get; set; }

    public bool AutoDisappear { get; set; }

    public DateTime 存在时间 { get; set; }

    public override int ProcessInterval => 10;

    public override DateTime BusyTime
    {
        get { return base.BusyTime; }
        set
        {
            if (base.BusyTime < value)
            {
                HardStunTime = value;
                base.BusyTime = value;
            }
        }
    }

    public override DateTime HardStunTime
    {
        get { return base.HardStunTime; }
        set
        {
            if (base.HardStunTime < value)
                base.HardStunTime = value;
        }
    }

    public override int CurrentHP
    {
        get { return base.CurrentHP; }
        set
        {
            value = Math.Clamp(value, 0, this[Stat.MaxHP]);
            if (base.CurrentHP != value)
            {
                base.CurrentHP = value;
                SendPacket(new SyncObjectHP
                {
                    ObjectID = ObjectID,
                    CurrentHP = CurrentHP,
                    MaxHP = this[Stat.MaxHP]
                });
            }
        }
    }

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

    public override GameDirection CurrentDirection
    {
        get { return base.CurrentDirection; }
        set
        {
            if (CurrentDirection != value)
            {
                base.CurrentDirection = value;
                SendPacket(new SyncObjectDirectionPacket
                {
                    ActionTime = 100,
                    ObjectID = ObjectID,
                    Direction = (ushort)value
                });
            }
        }
    }

    public override byte CurrentLevel => Info.Level;

    public override bool CanBeHit => CanBeInjured ? !Dead : false;

    public override string Name => Info.Name;

    public override GameObjectType ObjectType => GameObjectType.NPC;

    public override ObjectSize Size => ObjectSize.Single1x1;

    public override int this[Stat stat]
    {
        get { return base[stat]; }
        set { base[stat] = value; }
    }

    public int TargetRange => 10;
    public ushort GuardNumber => Info.GuardID;
    public int RevivalInterval => Info.RevivalInterval;
    public int StoreID => Info.StoreID;
    public string 界面代码 => Info.InterfaceCode;
    public bool CanBeInjured => Info.CanBeInjured;
    public bool ActiveAttackTarget => Info.ActiveAttack;

    public GuardObject(GuardInfo info, Map map, GameDirection dir, Point location)
    {
        Info = info;
        BirthMap = map;
        CurrentMap = map;
        BirthDirection = dir;
        BirthPosition = location;
        ObjectID = ++MapManager.ObjectID;
        BonusStats[this] = new Stats { [Stat.MaxHP] = 9999 };
        string text = Info.BasicAttackSkills;
        if (text != null && text.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(Info.BasicAttackSkills, out BasicAttackSkill);
        }
        MapManager.AddObject(this);
        守卫复活处理();
    }

    public override void Process()
    {
        if (SEngine.CurrentTime < base.ProcessTime)
            return;

        if (AutoDisappear && SEngine.CurrentTime > 存在时间)
        {
            Despawn();
            return;
        }
        if (Dead)
        {
            if (!Disappeared && SEngine.CurrentTime >= DisappearTime)
            {
                RemoveAllNeighbors();
                UnbindGrid();
                Disappeared = true;
            }
            if (SEngine.CurrentTime >= ResurrectionTime)
            {
                RemoveAllNeighbors();
                UnbindGrid();
                守卫复活处理();
            }
        }
        else
        {
            foreach (KeyValuePair<ushort, BuffInfo> item in Buffs.ToList())
            {
                轮询Buff时处理(item.Value);
            }
            foreach (SkillObject item2 in ActiveSkills.ToList())
            {
                item2.Process();
            }
            if (SEngine.CurrentTime > base.RecoveryTime)
            {
                if (!CheckStatus(GameObjectState.Poisoned))
                {
                    CurrentHP += 5;
                }
                base.RecoveryTime = SEngine.CurrentTime.AddSeconds(5.0);
            }
            if (ActiveAttackTarget && SEngine.CurrentTime > BusyTime && SEngine.CurrentTime > HardStunTime)
            {
                if (更新对象仇恨())
                {
                    守卫智能攻击();
                }
                else if (Target.TargetList.Count == 0 && CanTurn())
                {
                    CurrentDirection = BirthDirection;
                }
            }
            if (GuardNumber == 6121 && CurrentMap.MapID == 183 && SEngine.CurrentTime > 转移计时)
            {
                RemoveAllNeighbors();
                UnbindGrid();
                CurrentPosition = CurrentMap.传送区域.RandomCoords;
                BindGrid();
                UpdateAllNeighbours();
                转移计时 = SEngine.CurrentTime.AddMinutes(2.5);
            }
        }
        base.Process();
    }

    public override void Die(MapObject 对象, bool 技能击杀)
    {
        base.Die(对象, 技能击杀);
        DisappearTime = SEngine.CurrentTime.AddMilliseconds(10000.0);
        ResurrectionTime = SEngine.CurrentTime.AddMilliseconds((CurrentMap.MapID == 80) ? int.MaxValue : 60000);
        Buffs.Clear();
        SecondaryObject = true;
        MapManager.AddSecondaryObject(this);
        if (Activated)
        {
            Activated = false;
            MapManager.RemoveActiveObject(this);
        }
    }

    public void Deactivate()
    {
        if (Activated)
        {
            Activated = false;
            ActiveSkills.Clear();
            MapManager.RemoveActiveObject(this);
        }
    }

    public void Activate()
    {
        if (!Activated)
        {
            Activated = true;
            MapManager.AddActiveObject(this);
            int num = (int)Math.Max(0.0, (SEngine.CurrentTime - base.RecoveryTime).TotalSeconds / 5.0);
            base.CurrentHP = Math.Min(this[Stat.MaxHP], CurrentHP + num * this[Stat.HealthRecovery]);
            base.RecoveryTime = base.RecoveryTime.AddSeconds(5.0);
        }
    }

    public void 守卫智能攻击()
    {
        if (!CheckStatus(GameObjectState.Paralyzed | GameObjectState.Unconscious) && BasicAttackSkill != null)
        {
            if (GetDistance(Target.Target) > BasicAttackSkill.MaxDistance)
            {
                Target.Remove(Target.Target);
            }
            else
            {
                new SkillObject(this, BasicAttackSkill, null, base.ActionID++, CurrentMap, CurrentPosition, Target.Target, Target.Target.CurrentPosition, null);
            }
        }
    }

    public void 守卫复活处理()
    {
        RefreshStats();
        SecondaryObject = false;
        Dead = false;
        Blocking = !Info.Nothingness;
        CurrentMap = BirthMap;
        CurrentDirection = BirthDirection;
        CurrentPosition = BirthPosition;
        CurrentHP = this[Stat.MaxHP];
        base.RecoveryTime = SEngine.CurrentTime.AddMilliseconds(SEngine.Random.Next(5000));
        Target = new HateObject();
        BindGrid();
        UpdateAllNeighbours();
    }

    public bool 更新对象仇恨()
    {
        if (Target.TargetList.Count == 0)
        {
            return false;
        }
        if (Target.Target == null)
        {
            return Target.切换仇恨(this);
        }
        if (Target.Target.Dead)
        {
            Target.Remove(Target.Target);
        }
        else if (!Neighbors.Contains(Target.Target))
        {
            Target.Remove(Target.Target);
        }
        else if (!Target.TargetList.ContainsKey(Target.Target))
        {
            Target.Remove(Target.Target);
        }
        else if (GetDistance(Target.Target) > TargetRange)
        {
            Target.Remove(Target.Target);
        }
        if (Target.Target == null)
        {
            return 更新对象仇恨();
        }
        return true;
    }

    public void 清空守卫仇恨()
    {
        Target.Target = null;
        Target.TargetList.Clear();
    }
}
