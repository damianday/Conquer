using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using GameServer.Database;
using GameServer.Template;
using GameServer.Skill;

using GamePackets.Server;

namespace GameServer.Map;

public sealed class PetObject : MapObject
{
    public PlayerObject Master;

    public MonsterInfo MInfo;

    public HateObject Target;

    public GameSkill NormalAttackSkill;
    public List<GameSkill> RandomAttackSkill = new List<GameSkill>();
    public GameSkill EnterCombatSkill;
    public GameSkill ExitCombatSkill;
    public GameSkill DeathSkill;
    public GameSkill MoveSkill;
    public GameSkill BirthSkill;

    public PetInfo PInfo;

    public bool Disappeared { get; set; }

    public DateTime AttackTime { get; set; }
    public DateTime RoamingTime { get; set; }
    public DateTime ResurrectionTime { get; set; }
    public DateTime DisappearTime { get; set; }
    public DateTime SurvivalTime { get; set; }

    public int CurrentExp
    {
        get { return PInfo.CurrentExp.V; }
        set
        {
            if (PInfo.CurrentExp.V != value)
                PInfo.CurrentExp.V = value;
        }
    }

    public byte PetLevel
    {
        get { return PInfo.CurrentLevel.V; }
        set
        {
            if (PInfo.CurrentLevel.V != value)
                PInfo.CurrentLevel.V = value;
        }
    }

    public byte PetMaxLevel
    {
        get { return PInfo.MaxLevel.V; }
        set
        {
            if (PInfo.MaxLevel.V != value)
                PInfo.MaxLevel.V = value;
        }
    }

    public bool BoundWeapon
    {
        get { return PInfo.BoundWeapon.V; }
        set
        {
            if (PInfo.BoundWeapon.V != value)
                PInfo.BoundWeapon.V = value;
        }
    }

    public DateTime MutinyTime
    {
        get { return PInfo.MutinyTime.V; }
        set
        {
            if (PInfo.MutinyTime.V != value)
                PInfo.MutinyTime.V = value;
        }
    }

    public override int ProcessInterval => 10;

    public override DateTime BusyTime
    {
        get { return base.BusyTime; }
        set
        {
            if (base.BusyTime < value)
            {
                base.BusyTime = value;
                AttackTime = value;
            }
        }
    }

    public override DateTime HitTime
    {
        get { return base.HitTime; }
        set
        {
            if (base.HitTime < value)
                base.HitTime = value;
        }
    }

    public override int CurrentHP
    {
        get { return PInfo.CurrentHP.V; }
        set
        {
            value = Compute.Clamp(0, value, this[Stat.MaxHP]);
            if (PInfo.CurrentHP.V != value)
            {
                PInfo.CurrentHP.V = value;
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
                    Direction = (ushort)CurrentDirection
                });
            }
        }
    }

    public override byte CurrentLevel => MInfo.Level;
    public override string Name => MInfo.MonsterName;
    public override GameObjectType ObjectType => GameObjectType.Pet;
    public override ObjectSize Size => MInfo.Size;

    public override int this[Stat stat]
    {
        get { return base[stat]; }
        set { base[stat] = value; }
    }

    public int HateRange => 4;
    public int HateDuration => 15_000;
    public int TargetSelecthInterval => 5000;
    public ushort PetID => MInfo.ID;

    public ushort MaxExperience
    {
        get { return PetLevel < Settings.Default.PetUpgradeXP.Length ? Settings.Default.PetUpgradeXP[PetLevel] : (ushort)10; }
    }

    public int MoveInterval => MInfo.MoveInterval;
    public int RoamInterval => MInfo.RoamInterval;
    public int CorpsePreservationDuration => MInfo.CorpsePreservationDuration;
    public bool CanBeSeducedBySkills => MInfo.CanBeSeducedBySkills;
    public float BaseTemptationProbability => MInfo.BaseTemptationProbability;
    public MonsterRaceType PetRace => MInfo.Race;
    public MonsterGradeType Grade => MInfo.Grade;

    public Stats BaseStats
    {
        get
        {
            if ((MInfo.GrowStats?.Length > PetLevel))
                return MInfo.GrowStats[PetLevel];
            return MInfo.Stats;
        }
    }

    public PetObject(PlayerObject master, PetInfo peti)
    {
        Master = master;
        PInfo = peti;
        MInfo = MonsterInfo.DataSheet[peti.Name.V];
        CurrentPosition = master.CurrentPosition;
        CurrentMap = master.CurrentMap;
        CurrentDirection = Compute.RandomDirection();
        BonusStats[this] = BaseStats;
        BonusStats[master.Character] = new Stats();
        if (MInfo.InheritedStats != null)
        {
            foreach (var stat in MInfo.InheritedStats)
            {
                BonusStats[master.Character][stat.ConvertStat] = (int)((float)master[stat.InheritedStat] * stat.Ratio);
            }
        }
        RefreshStats();

        Initialize();

        ObjectID = ++MapManager.ObjectID;
        MapManager.AddObject(this);
        Activated = true;
        MapManager.AddActiveObject(this);
        Blocking = true;
        PetRecall();
    }

    public PetObject(PlayerObject master, MonsterInfo moni, byte level, byte levelMax, bool bindWeapon)
    {
        Master = master;
        MInfo = moni;
        PInfo = new PetInfo(moni.MonsterName, level, levelMax, bindWeapon, DateTime.MaxValue);
        CurrentPosition = master.CurrentPosition;
        CurrentMap = master.CurrentMap;
        CurrentDirection = Compute.RandomDirection();
        ObjectID = ++MapManager.ObjectID;
        BonusStats[this] = BaseStats;
        BonusStats[master.Character] = new Stats();
        if (MInfo.InheritedStats != null)
        {
            foreach (var stat in MInfo.InheritedStats)
            {
                BonusStats[master.Character][stat.ConvertStat] = (int)((float)master[stat.InheritedStat] * stat.Ratio);
            }
        }
        RefreshStats();

        CurrentHP = this[Stat.MaxHP];

        Initialize();

        MapManager.AddObject(this);
        Activated = true;
        MapManager.AddActiveObject(this);
        Blocking = true;
        PetRecall();
    }

    public PetObject(PlayerObject master, MonsterObject moni, byte level, bool bindWeapon, int duration)
    {
        Master = master;
        MInfo = moni.Info;
        PInfo = new PetInfo(moni.Name, level, 7, bindWeapon, SEngine.CurrentTime.AddMinutes(duration));
        CurrentPosition = moni.CurrentPosition;
        CurrentMap = moni.CurrentMap;
        CurrentDirection = moni.CurrentDirection;
        BonusStats[this] = BaseStats;
        RefreshStats();

        CurrentHP = moni.CurrentHP;

        Initialize();
        
        moni.怪物诱惑处理();
        ObjectID = ++MapManager.ObjectID;
        Blocking = true;
        BindGrid();
        MapManager.AddObject(this);
        Activated = true;
        MapManager.AddActiveObject(this);
        UpdateAllNeighbours();
    }

    public PetObject(PlayerObject master, PetObject pet, byte level, bool bindWeapon, int duration)
    {
        Master = master;
        MInfo = pet.MInfo;
        PInfo = new PetInfo(pet.Name, level, 7, bindWeapon, SEngine.CurrentTime.AddMinutes(duration));
        CurrentPosition = pet.CurrentPosition;
        CurrentMap = pet.CurrentMap;
        CurrentDirection = pet.CurrentDirection;
        BonusStats[this] = BaseStats;
        RefreshStats();

        CurrentHP = pet.CurrentHP;

        Initialize();

        pet.Die(null, skillDeath: false);
        Blocking = true;
        BindGrid();
        ObjectID = ++MapManager.ObjectID;
        MapManager.AddObject(this);
        Activated = true;
        MapManager.AddActiveObject(this);
        UpdateAllNeighbours();
    }

    private void Initialize()
    {
        RecoveryTime = SEngine.CurrentTime.AddSeconds(5.0);
        AttackTime = SEngine.CurrentTime.AddSeconds(1.0);
        BusyTime = SEngine.CurrentTime.AddSeconds(1.0);
        RoamingTime = SEngine.CurrentTime.AddMilliseconds(SEngine.Random.Next(5000) + RoamInterval);
        SurvivalTime = DateTime.MaxValue;
        Target = new HateObject();

        if (!string.IsNullOrEmpty(MInfo.NormalAttackSkills))
            GameSkill.DataSheet.TryGetValue(MInfo.NormalAttackSkills, out NormalAttackSkill);

        RandomAttackSkill.Clear();
        foreach (var name in MInfo.RandomTriggerSkills)
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (GameSkill.DataSheet.TryGetValue(name, out var skill))
                    RandomAttackSkill.Add(skill);
            }
        }

        if (!string.IsNullOrEmpty(MInfo.EnterCombatSkills))
            GameSkill.DataSheet.TryGetValue(MInfo.EnterCombatSkills, out EnterCombatSkill);

        if (!string.IsNullOrEmpty(MInfo.ExitCombatSkills))
            GameSkill.DataSheet.TryGetValue(MInfo.ExitCombatSkills, out ExitCombatSkill);

        if (!string.IsNullOrEmpty(MInfo.BirthReleaseSkill))
            GameSkill.DataSheet.TryGetValue(MInfo.BirthReleaseSkill, out BirthSkill);

        if (!string.IsNullOrEmpty(MInfo.DeathReleaseSkill))
            GameSkill.DataSheet.TryGetValue(MInfo.DeathReleaseSkill, out DeathSkill);

        if (!string.IsNullOrEmpty(MInfo.MoveReleaseSkill))
            GameSkill.DataSheet.TryGetValue(MInfo.MoveReleaseSkill, out MoveSkill);
    }

    public override void Process()
    {
        if (SEngine.CurrentTime < ProcessTime)
            return;

        if (SEngine.CurrentTime >= SurvivalTime)
            Despawn();
        else if (Dead)
        {
            if (!Disappeared && SEngine.CurrentTime >= DisappearTime)
            {
                Despawn();
            }
        }
        else if (MutinyTime != default(DateTime) && SEngine.CurrentTime > MutinyTime)
        {
            new MonsterObject(this);
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
                {
                    CurrentHP += this[Stat.HealthRecovery];
                }
                RecoveryTime = SEngine.CurrentTime.AddSeconds(5.0);
            }

            if (SEngine.CurrentTime > HealTime && HealCount > 0)
            {
                HealCount--;
                HealTime = SEngine.CurrentTime.AddMilliseconds(500.0);
                CurrentHP += HealAmount;
            }

            if (EnterCombatSkill != null && !CombatStance && Target.TargetList.Count != 0)
            {
                new SkillObject(this, EnterCombatSkill, null, ActionID++, CurrentMap, CurrentPosition, null, CurrentPosition, null);
                CombatStance = true;
                AttackStopTime = SEngine.CurrentTime.AddSeconds(10.0);
            }
            else if (ExitCombatSkill != null && CombatStance && Target.TargetList.Count == 0 && SEngine.CurrentTime > AttackStopTime)
            {
                new SkillObject(this, ExitCombatSkill, null, ActionID++, CurrentMap, CurrentPosition, null, CurrentPosition, null);
                CombatStance = false;
            }
            else if (Master.PetMode == PetMode.Attack && SEngine.CurrentTime > BusyTime && SEngine.CurrentTime > AttackTime)
            {
                if (Target.Target == null && !Neighbors.Contains(Master))
                {
                    ProcessFollow();
                }
                else if (UpdateTarget())
                {
                    ProcessAttack();
                }
                else
                {
                    ProcessFollow();
                }
            }
        }

        base.Process();
    }

    public override void Die(MapObject attacker, bool skillDeath)
    {
        if (DeathSkill != null && attacker != null)
        {
            new SkillObject(this, DeathSkill, null, ActionID++, CurrentMap, CurrentPosition, null, CurrentPosition, null).Process();
        }

        base.Die(attacker, skillDeath);

        DisappearTime = SEngine.CurrentTime.AddMilliseconds(CorpsePreservationDuration);

        RemoveTargets();
        Master?.RemovePet(this);

        Buffs.Clear();
        PInfo?.Remove();
        SecondaryObject = true;
        MapManager.AddSecondaryObject(this);
        Activated = false;
        MapManager.RemoveActiveObject(this);
    }

    public void ProcessFollow()
    {
        if (Master == null || !CanWalk()) return;

        if (!Neighbors.Contains(Master))
        {
            PetRecall();
            return;
        }

        var point = Compute.GetNextPosition(Master.CurrentPosition, Compute.RotateDirection(Master.CurrentDirection, 4), 2);
        if (GetDistance(Master) <= 2 && GetDistance(point) <= 2)
        {
            if (SEngine.CurrentTime > RoamingTime)
            {
                RoamingTime = SEngine.CurrentTime.AddMilliseconds(RoamInterval + SEngine.Random.Next(5_000));

                Walk(Compute.RandomDirection());
            }
            return;
        }

        var dir = Compute.DirectionFromPoint(CurrentPosition, point);
        for (var i = 0; i < 8; i++)
        {
            var next = Compute.GetNextPosition(CurrentPosition, dir, 1);
            if (CurrentMap.CanMove(next))
                break;

            dir = Compute.RotateDirection(dir, (SEngine.Random.Next(2) != 0) ? 1 : -1);
        }

        Walk(dir);
    }

    public void ProcessAttack()
    {
        if (CheckStatus(GameObjectState.Paralyzed | GameObjectState.Unconscious))
            return;

        GameSkill skill = null;

        if (RandomAttackSkill.Count > 0)
        {
            var rskill = RandomAttackSkill[SEngine.Random.Next(RandomAttackSkill.Count)];

            if (rskill != null && 
                (!Cooldowns.ContainsKey(NormalAttackSkill.OwnSkillID | 0x1000000) || SEngine.CurrentTime > Cooldowns[NormalAttackSkill.OwnSkillID | 0x1000000]) &&
                Compute.CalculateProbability(rskill.CalculateLuckyProbability ? Compute.CalcLuck(this[Stat.Luck]) : rskill.CalculateTriggerProbability))
            {
                skill = rskill;
            }
        }
        else
        {
            if (NormalAttackSkill == null || (Cooldowns.ContainsKey(NormalAttackSkill.OwnSkillID | 0x1000000) && !(SEngine.CurrentTime > Cooldowns[NormalAttackSkill.OwnSkillID | 0x1000000])))
            {
                return;
            }
            skill = NormalAttackSkill;
        }

        if (skill == null) return;

        if (GetDistance(Target.Target) > skill.MaxDistance)
        {
            var dir = Compute.DirectionFromPoint(CurrentPosition, Target.Target.CurrentPosition);
            for (var i = 0; i < 8; i++)
            {
                var point = Compute.GetNextPosition(CurrentPosition, dir, 1);
                if (CurrentMap.CanMove(point))
                    break;

                dir = Compute.RotateDirection(dir, (SEngine.Random.Next(2) != 0) ? 1 : -1);
            }

            if (!Walk(dir))
                Target.Remove(Target.Target);
        }
        else if (SEngine.CurrentTime > AttackTime)
        {
            new SkillObject(this, skill, null, base.ActionID++, CurrentMap, CurrentPosition, Target.Target, Target.Target.CurrentPosition, null);
            AttackTime = SEngine.CurrentTime.AddMilliseconds(Compute.Clamp(0, 10 - this[Stat.AttackSpeed], 10) * 500);
        }
        else
        {
            var dir = Compute.DirectionFromPoint(CurrentPosition, Target.Target.CurrentPosition);
            Turn(dir);
        }
    }

    private bool Turn(GameDirection direction)
    {
        if (!CanTurn()) return false;

        CurrentDirection = direction;
        return true;
    }

    private bool Walk(GameDirection direction)
    {
        if (!CanWalk()) return false;

        var location = Compute.GetNextPosition(CurrentPosition, direction, 1);
        if (!CurrentMap.CanMove(location)) return false;

        BusyTime = SEngine.CurrentTime.AddMilliseconds(WalkInterval);
        WalkTime = SEngine.CurrentTime.AddMilliseconds(WalkInterval + MoveInterval);
        CurrentDirection = Compute.DirectionFromPoint(CurrentPosition, location);

        SendPacket(new ObjectWalkPacket { ObjectID = ObjectID, Position = location, Speed = WalkSpeed });
        OnLocationChanged(location);
        return true;
    }

    public void GainExperience()
    {
        if (PetLevel < PetMaxLevel && ++CurrentExp >= MaxExperience)
        {
            PetLevel++;
            CurrentExp = 0;
            BonusStats[this] = BaseStats;
            RefreshStats();
            CurrentHP = this[Stat.MaxHP];
            SendPacket(new 对象变换类型
            {
                改变类型 = 2,
                对象编号 = ObjectID
            });
            SendPacket(new SyncPetLevelPacket
            {
                ObjectID = ObjectID,
                PetLevel = PetLevel
            });
        }
    }

    public void PetRecall()
    {
        if (Master == null) return;

        var location = Master.CurrentPosition;
        for (var i = 1; i <= 120; i++)
        {
            Point next = Compute.GetPositionAround(location, i);
            if (Master.CurrentMap.CanMove(next))
            {
                location = next;
                break;
            }
        }

        RemoveTargets();
        RemoveAllNeighbors();
        UnbindGrid();
        CurrentPosition = location;
        CurrentMap = Master.CurrentMap;
        BindGrid();
        UpdateAllNeighbours();
    }

    public void Die()
    {
        ActiveSkills.Clear();
        Buffs.Clear();
        Dead = true;
        Despawn();
    }

    public bool UpdateTarget()
    {
        if (Target.TargetList.Count == 0)
        {
            return false;
        }
        if (Target.Target == null)
        {
            Target.SelectTargetTime = default(DateTime);
        }
        else if (!Target.Target.CanBeHit)
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
        else if (Master.GetRelationship(Target.Target) != GameObjectRelationship.Hostile)
        {
            Target.Remove(Target.Target);
        }
        else if (GetDistance(Target.Target) > HateRange && SEngine.CurrentTime > Target.TargetList[Target.Target].HateTime)
        {
            Target.Remove(Target.Target);
        }
        else if (GetDistance(Target.Target) <= HateRange)
        {
            Target.TargetList[Target.Target].HateTime = SEngine.CurrentTime.AddMilliseconds(HateDuration);
        }
        if (Target.SelectTargetTime < SEngine.CurrentTime && Target.SelectTarget(this))
        {
            Target.SelectTargetTime = SEngine.CurrentTime.AddMilliseconds(TargetSelecthInterval);
        }
        if (Target.Target == null)
        {
            return UpdateTarget();
        }
        return true;
    }

    public void RemoveTargets()
    {
        Target.Target = null;
        Target.TargetList.Clear();
    }
}
