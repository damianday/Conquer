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
    public DateTime ExistenceTime { get; set; }

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
    public ushort GuardID => Info.GuardID;
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

        if (!string.IsNullOrEmpty(Info.BasicAttackSkills))
            GameSkill.DataSheet.TryGetValue(Info.BasicAttackSkills, out BasicAttackSkill);

        MapManager.AddObject(this);
        Resurrect();
    }

    public override void Process()
    {
        if (SEngine.CurrentTime < ProcessTime)
            return;

        if (AutoDisappear && SEngine.CurrentTime > ExistenceTime)
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
                Resurrect();
            }
        }
        else
        {
            foreach (var buff in Buffs)
                ProcessBuffs(buff.Value);

            foreach (var skill in ActiveSkills)
                skill.Process();

            if (SEngine.CurrentTime > RecoveryTime)
            {
                if (!CheckStatus(GameObjectState.Poisoned))
                    CurrentHP += 5;

                RecoveryTime = SEngine.CurrentTime.AddSeconds(5.0);
            }
            if (ActiveAttackTarget && SEngine.CurrentTime > BusyTime && SEngine.CurrentTime > HardStunTime)
            {
                if (UpdateTargets())
                {
                    ProcessAttack();
                }
                else if (Target.TargetList.Count == 0 && CanTurn())
                {
                    CurrentDirection = BirthDirection;
                }
            }
            if (GuardID == 6121 && CurrentMap.MapID == 183 && SEngine.CurrentTime > 转移计时)
            {
                RemoveAllNeighbors();
                UnbindGrid();
                CurrentPosition = CurrentMap.TeleportationArea.RandomCoords;
                BindGrid();
                UpdateAllNeighbours();
                转移计时 = SEngine.CurrentTime.AddMinutes(2.5);
            }
        }

        base.Process();
    }

    public override void Die(MapObject attacker, bool skillDeath)
    {
        base.Die(attacker, skillDeath);

        DisappearTime = SEngine.CurrentTime.AddSeconds(10.0);
        ResurrectionTime = SEngine.CurrentTime.AddMilliseconds((CurrentMap.MapID == 80) ? int.MaxValue : 60_000);
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

            int num = (int)Math.Max(0.0, (SEngine.CurrentTime - RecoveryTime).TotalSeconds / 5.0);
            base.CurrentHP = Math.Min(this[Stat.MaxHP], CurrentHP + num * this[Stat.HealthRecovery]);
            RecoveryTime = RecoveryTime.AddSeconds(5.0);
        }
    }

    public void ProcessAttack()
    {
        if (CheckStatus(GameObjectState.Paralyzed | GameObjectState.Unconscious)) return;
        if (BasicAttackSkill == null) return;

        if (GetDistance(Target.Target) > BasicAttackSkill.MaxDistance)
            Target.Remove(Target.Target);
        else
        {
            new SkillObject(this, BasicAttackSkill, null, ActionID++, CurrentMap, CurrentPosition, Target.Target, Target.Target.CurrentPosition, null);
        }
    }

    public void Resurrect()
    {
        RefreshStats();

        SecondaryObject = false;
        Dead = false;
        Blocking = !Info.Nothingness;
        CurrentMap = BirthMap;
        CurrentDirection = BirthDirection;
        CurrentPosition = BirthPosition;
        CurrentHP = this[Stat.MaxHP];
        RecoveryTime = SEngine.CurrentTime.AddMilliseconds(SEngine.Random.Next(5_000));
        Target = new HateObject();

        BindGrid();
        UpdateAllNeighbours();
    }

    public bool UpdateTargets()
    {
        if (Target.TargetList.Count == 0)
        {
            return false;
        }
        if (Target.Target == null)
        {
            return Target.SelectTarget(this);
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
            return UpdateTargets();
        }
        return true;
    }

    public void RemoveTargets()
    {
        Target.Target = null;
        Target.TargetList.Clear();
    }
}
