using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using GameServer.Template;
using GameServer.Skill;

using GamePackets.Server;

namespace GameServer.Map;

public sealed class TrapObject : MapObject
{
    public byte TrapLevel;
    public ushort TrapID;
    public DateTime PlacementTime;
    public DateTime DisappearTime;
    public DateTime TriggerTime;

    public MapObject Caster;

    public SkillTrap TrapInfo;

    public HashSet<MapObject> PassiveTriggered;

    public byte TrapMoveCount;

    public GameSkill PassiveTriggerSkill;
    public GameSkill ActiveTriggerSkill;

    public ushort GroupID => TrapInfo.GroupID;
    public ushort ActiveTriggerInterval => TrapInfo.ActiveTriggerInterval;
    public ushort ActiveTriggerDelay => TrapInfo.ActiveTriggerDelay;
    public ushort RemainingTime => (ushort)Math.Ceiling((DisappearTime - SEngine.CurrentTime).TotalMilliseconds / 62.5);

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

    public override int ProcessInterval => 10;

    public override byte CurrentLevel
    {
        get { return Caster.CurrentLevel; }
        set { Caster.CurrentLevel = value; }
    }

    public override bool Blocking
    {
        get { return false; }
        set { base.Blocking = value; }
    }

    public override bool CanBeHit => false;
    public override string Name => TrapInfo.Name;
    public override GameObjectType ObjectType => GameObjectType.Trap;
    public override ObjectSize Size => TrapInfo.Size;
    public override Stats Stats => base.Stats;

    public TrapObject(MapObject caster, SkillTrap info, Map map, Point location)
    {
        Caster = caster;
        TrapInfo = info;
        CurrentMap = map;
        CurrentPosition = location;
        WalkTime = SEngine.CurrentTime;
        PlacementTime = SEngine.CurrentTime;
        TrapID = info.ID;
        CurrentDirection = Caster.CurrentDirection;
        PassiveTriggered = new HashSet<MapObject>();
        DisappearTime = PlacementTime + TimeSpan.FromMilliseconds(TrapInfo.Duration);
        TriggerTime = PlacementTime + TimeSpan.FromMilliseconds((int)TrapInfo.ActiveTriggerDelay);

        if (caster is PlayerObject player)
        {
            if (TrapInfo.BindingLevel != 0 && player.Skills.TryGetValue(TrapInfo.BindingLevel, out var v))
            {
                TrapLevel = v.Level.V;
            }
            if (TrapInfo.ExtendedDuration && TrapInfo.SkillLevelDelay)
            {
                DisappearTime += TimeSpan.FromMilliseconds(TrapLevel * TrapInfo.ExtendedTimePerLevel);
            }
            if (TrapInfo.ExtendedDuration && TrapInfo.PlayerStatDelay)
            {
                DisappearTime += TimeSpan.FromMilliseconds((float)player[TrapInfo.BoundPlayerStat] * TrapInfo.StatDelayFactor);
            }
            if (TrapInfo.ExtendedDuration && TrapInfo.HasSpecificInscriptionDelay && player.Skills.TryGetValue((ushort)(TrapInfo.SpecificInscriptionSkills / 10), out var v2) && v2.InscriptionID == TrapInfo.SpecificInscriptionSkills % 10)
            {
                DisappearTime += TimeSpan.FromMilliseconds(TrapInfo.InscriptionExtendedTime);
            }
        }
        ActiveTriggerSkill = ((TrapInfo.ActivelyTriggerSkills == null || !GameSkill.DataSheet.ContainsKey(TrapInfo.ActivelyTriggerSkills)) ? null : GameSkill.DataSheet[TrapInfo.ActivelyTriggerSkills]);
        PassiveTriggerSkill = ((TrapInfo.PassiveTriggerSkill == null || !GameSkill.DataSheet.ContainsKey(TrapInfo.PassiveTriggerSkill)) ? null : GameSkill.DataSheet[TrapInfo.PassiveTriggerSkill]);
        ObjectID = ++MapManager.TrapObjectID;
        BindGrid();
        UpdateAllNeighbours();
        MapManager.AddObject(this);
        Activated = true;
        MapManager.AddActiveObject(this);
    }

    public override void Process()
    {
        if (SEngine.CurrentTime < ProcessTime)
            return;
        if (SEngine.CurrentTime > DisappearTime)
        {
            Disappear();
            return;
        }

        foreach (var skill in ActiveSkills)
            skill.Process();

        if (ActiveTriggerSkill != null && SEngine.CurrentTime > TriggerTime)
        {
            ActivateTrigger();
        }

        if (TrapInfo.CanMove && TrapMoveCount < TrapInfo.LimitMoveSteps && SEngine.CurrentTime > WalkTime)
        {
            if (TrapInfo.MoveInCurrentDirection)
            {
                OnLocationChanged(Compute.GetNextPosition(CurrentPosition, CurrentDirection, 1));
                SendPacket(new SyncTrapPositionPacket
                {
                    TrapID = ObjectID,
                    Position = CurrentPosition,
                    Height = CurrentHeight,
                    MoveSpeed = TrapInfo.MoveSpeed
                });
            }

            if (PassiveTriggerSkill != null)
            {
                var grid = Compute.CalculateGrid(CurrentPosition, CurrentDirection, Size);
                foreach (var point in grid)
                {
                    foreach (var obj in CurrentMap[point].ToList())
                    {
                        ActivatePassive(obj);
                    }
                }
            }

            TrapMoveCount++;
            WalkTime = WalkTime.AddMilliseconds(TrapInfo.MoveSpeed * 60);
        }

        base.Process();
    }

    public void ActivatePassive(MapObject obj)
    {
        if (!(SEngine.CurrentTime > DisappearTime) && PassiveTriggerSkill != null && !obj.Dead && 
            (obj.ObjectType & TrapInfo.PassiveObjectType) != 0 && obj.IsValidTarget(Caster, TrapInfo.PassiveTargetType) && 
            (Caster.GetRelationship(obj) & TrapInfo.PassiveType) != 0 && (!TrapInfo.RetriggeringIsProhibited || PassiveTriggered.Add(obj)))
        {
            new SkillObject(this, PassiveTriggerSkill, null, 0, CurrentMap, CurrentPosition, obj, obj.CurrentPosition, null);
        }
    }

    public void ActivateTrigger()
    {
        if (SEngine.CurrentTime > DisappearTime) return;

        new SkillObject(this, ActiveTriggerSkill, null, 0, CurrentMap, CurrentPosition, null, CurrentPosition, null);
        TriggerTime += TimeSpan.FromMilliseconds(ActiveTriggerInterval);
    }

    public void Disappear()
    {
        Despawn();
    }
}
