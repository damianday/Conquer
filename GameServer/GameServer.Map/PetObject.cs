using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using GameServer.Database;
using GameServer.Template;
using GameServer.Networking;

using GamePackets.Server;

namespace GameServer.Map;

public sealed class PetObject : MapObject
{
    public PlayerObject Master;

    public MonsterInfo MInfo;

    public HateObject Target;

    public GameSkill 普通攻击技能;
    public GameSkill 概率触发技能;
    public GameSkill 进入战斗技能;
    public GameSkill 退出战斗技能;
    public GameSkill 死亡释放技能;
    public GameSkill 移动释放技能;
    public GameSkill 出生释放技能;

    public PetInfo PInfo;

    public bool 尸体消失 { get; set; }

    public DateTime AttackTime { get; set; }
    public DateTime RoamingTime { get; set; }
    public DateTime 复活时间 { get; set; }
    public DateTime VanishingTime { get; set; }

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
                HardStunTime = value;
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
    public int 仇恨时长 => 15_000;
    public int 切换间隔 => 5000;
    public ushort PetID => MInfo.ID;

    public ushort MaxExp
    {
        get
        {
            if (CharacterProgression.PetMaxExpTable?.Length > PetLevel)
                return CharacterProgression.PetMaxExpTable[PetLevel];
            return 10;
        }
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
            return MInfo.BasicStats;
        }
    }

    public PetObject(PlayerObject master, PetInfo data)
    {
        this.Master = master;
        PInfo = data;
        MInfo = MonsterInfo.DataSheet[data.Name.V];
        CurrentPosition = master.CurrentPosition;
        CurrentMap = master.CurrentMap;
        CurrentDirection = Compute.RandomDirection();
        BonusStats[this] = BaseStats;
        BonusStats[master.Character] = new Stats();
        if (MInfo.InheritsStats != null)
        {
            InheritStat[] 继承属性 = MInfo.InheritsStats;
            for (int i = 0; i < 继承属性.Length; i++)
            {
                InheritStat 属性继承 = 继承属性[i];
                BonusStats[master.Character][属性继承.ConvertStat] = (int)((float)master[属性继承.InheritsStats] * 属性继承.Ratio);
            }
        }
        RefreshStats();
        base.RecoveryTime = SEngine.CurrentTime.AddSeconds(5.0);
        AttackTime = SEngine.CurrentTime.AddSeconds(1.0);
        RoamingTime = SEngine.CurrentTime.AddMilliseconds(SEngine.Random.Next(5000) + RoamInterval);
        Target = new HateObject();
        string text = MInfo.NormalAttackSkills;
        if (text != null && text.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.NormalAttackSkills, out 普通攻击技能);
        }
        string text2 = MInfo.ProbabilityTriggerSkills;
        if (text2 != null && text2.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.ProbabilityTriggerSkills, out 概率触发技能);
        }
        string text3 = MInfo.EnterCombatSkills;
        if (text3 != null && text3.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.EnterCombatSkills, out 进入战斗技能);
        }
        string text4 = MInfo.ExitCombatSkills;
        if (text4 != null && text4.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.ExitCombatSkills, out 退出战斗技能);
        }
        string text5 = MInfo.DeathReleaseSkill;
        if (text5 != null && text5.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.DeathReleaseSkill, out 死亡释放技能);
        }
        string text6 = MInfo.MoveReleaseSkill;
        if (text6 != null && text6.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.MoveReleaseSkill, out 移动释放技能);
        }
        string text7 = MInfo.BirthReleaseSkill;
        if (text7 != null && text7.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.BirthReleaseSkill, out 出生释放技能);
        }
        ObjectID = ++MapManager.ObjectID;
        MapManager.AddObject(this);
        Activated = true;
        MapManager.AddActiveObject(this);
        Blocking = true;
        PetRecall();
    }

    public PetObject(PlayerObject master, MonsterInfo data, byte level, byte levelMax, bool bindWeapon)
    {
        this.Master = master;
        MInfo = data;
        PInfo = new PetInfo(data.MonsterName, level, levelMax, bindWeapon, DateTime.MaxValue);
        CurrentPosition = master.CurrentPosition;
        CurrentMap = master.CurrentMap;
        CurrentDirection = Compute.RandomDirection();
        ObjectID = ++MapManager.ObjectID;
        BonusStats[this] = BaseStats;
        BonusStats[master.Character] = new Stats();
        if (MInfo.InheritsStats != null)
        {
            InheritStat[] 继承属性 = MInfo.InheritsStats;
            for (int i = 0; i < 继承属性.Length; i++)
            {
                InheritStat 属性继承 = 继承属性[i];
                BonusStats[master.Character][属性继承.ConvertStat] = (int)((float)master[属性继承.InheritsStats] * 属性继承.Ratio);
            }
        }
        RefreshStats();
        CurrentHP = this[Stat.MaxHP];
        base.RecoveryTime = SEngine.CurrentTime.AddSeconds(5.0);
        AttackTime = SEngine.CurrentTime.AddSeconds(1.0);
        RoamingTime = SEngine.CurrentTime.AddMilliseconds(SEngine.Random.Next(5000) + RoamInterval);
        Target = new HateObject();
        string text = MInfo.NormalAttackSkills;
        if (text != null && text.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.NormalAttackSkills, out 普通攻击技能);
        }
        string text2 = MInfo.ProbabilityTriggerSkills;
        if (text2 != null && text2.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.ProbabilityTriggerSkills, out 概率触发技能);
        }
        string text3 = MInfo.EnterCombatSkills;
        if (text3 != null && text3.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.EnterCombatSkills, out 进入战斗技能);
        }
        string text4 = MInfo.ExitCombatSkills;
        if (text4 != null && text4.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.ExitCombatSkills, out 退出战斗技能);
        }
        string text5 = MInfo.DeathReleaseSkill;
        if (text5 != null && text5.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.DeathReleaseSkill, out 死亡释放技能);
        }
        string text6 = MInfo.MoveReleaseSkill;
        if (text6 != null && text6.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.MoveReleaseSkill, out 移动释放技能);
        }
        string text7 = MInfo.BirthReleaseSkill;
        if (text7 != null && text7.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.BirthReleaseSkill, out 出生释放技能);
        }
        MapManager.AddObject(this);
        Activated = true;
        MapManager.AddActiveObject(this);
        Blocking = true;
        PetRecall();
    }

    public PetObject(PlayerObject master, MonsterObject 诱惑怪物, byte 初始等级, bool 绑定武器, int 宠物时长)
    {
        this.Master = master;
        MInfo = 诱惑怪物.Info;
        PInfo = new PetInfo(诱惑怪物.Name, 初始等级, 7, 绑定武器, SEngine.CurrentTime.AddMinutes(宠物时长));
        CurrentPosition = 诱惑怪物.CurrentPosition;
        CurrentMap = 诱惑怪物.CurrentMap;
        CurrentDirection = 诱惑怪物.CurrentDirection;
        BonusStats[this] = BaseStats;
        RefreshStats();
        CurrentHP = Math.Min(诱惑怪物.CurrentHP, this[Stat.MaxHP]);
        base.RecoveryTime = SEngine.CurrentTime.AddSeconds(5.0);
        AttackTime = SEngine.CurrentTime.AddSeconds(1.0);
        BusyTime = SEngine.CurrentTime.AddSeconds(1.0);
        RoamingTime = SEngine.CurrentTime.AddMilliseconds(RoamInterval);
        Target = new HateObject();
        string text = MInfo.NormalAttackSkills;
        if (text != null && text.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.NormalAttackSkills, out 普通攻击技能);
        }
        string text2 = MInfo.ProbabilityTriggerSkills;
        if (text2 != null && text2.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.ProbabilityTriggerSkills, out 概率触发技能);
        }
        string text3 = MInfo.EnterCombatSkills;
        if (text3 != null && text3.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.EnterCombatSkills, out 进入战斗技能);
        }
        string text4 = MInfo.ExitCombatSkills;
        if (text4 != null && text4.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.ExitCombatSkills, out 退出战斗技能);
        }
        string text5 = MInfo.DeathReleaseSkill;
        if (text5 != null && text5.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.DeathReleaseSkill, out 死亡释放技能);
        }
        string text6 = MInfo.MoveReleaseSkill;
        if (text6 != null && text6.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.MoveReleaseSkill, out 移动释放技能);
        }
        string text7 = MInfo.BirthReleaseSkill;
        if (text7 != null && text7.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.BirthReleaseSkill, out 出生释放技能);
        }
        诱惑怪物.怪物诱惑处理();
        ObjectID = ++MapManager.ObjectID;
        Blocking = true;
        BindGrid();
        MapManager.AddObject(this);
        Activated = true;
        MapManager.AddActiveObject(this);
        UpdateAllNeighbours();
    }

    public PetObject(PlayerObject master, PetObject 诱惑宠物, byte 初始等级, bool 绑定武器, int 宠物时长)
    {
        this.Master = master;
        MInfo = 诱惑宠物.MInfo;
        PInfo = new PetInfo(诱惑宠物.Name, 初始等级, 7, 绑定武器, SEngine.CurrentTime.AddMinutes(宠物时长));
        CurrentPosition = 诱惑宠物.CurrentPosition;
        CurrentMap = 诱惑宠物.CurrentMap;
        CurrentDirection = 诱惑宠物.CurrentDirection;
        BonusStats[this] = BaseStats;
        RefreshStats();
        CurrentHP = Math.Min(诱惑宠物.CurrentHP, this[Stat.MaxHP]);
        base.RecoveryTime = SEngine.CurrentTime.AddSeconds(5.0);
        AttackTime = SEngine.CurrentTime.AddSeconds(1.0);
        BusyTime = SEngine.CurrentTime.AddSeconds(1.0);
        RoamingTime = SEngine.CurrentTime.AddMilliseconds(RoamInterval);
        Target = new HateObject();
        string text = MInfo.NormalAttackSkills;
        if (text != null && text.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.NormalAttackSkills, out 普通攻击技能);
        }
        string text2 = MInfo.ProbabilityTriggerSkills;
        if (text2 != null && text2.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.ProbabilityTriggerSkills, out 概率触发技能);
        }
        string text3 = MInfo.EnterCombatSkills;
        if (text3 != null && text3.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.EnterCombatSkills, out 进入战斗技能);
        }
        string text4 = MInfo.ExitCombatSkills;
        if (text4 != null && text4.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.ExitCombatSkills, out 退出战斗技能);
        }
        string text5 = MInfo.DeathReleaseSkill;
        if (text5 != null && text5.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.DeathReleaseSkill, out 死亡释放技能);
        }
        string text6 = MInfo.MoveReleaseSkill;
        if (text6 != null && text6.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.MoveReleaseSkill, out 移动释放技能);
        }
        string text7 = MInfo.BirthReleaseSkill;
        if (text7 != null && text7.Length > 0)
        {
            GameSkill.DataSheet.TryGetValue(MInfo.BirthReleaseSkill, out 出生释放技能);
        }
        诱惑宠物.Die(null, 技能击杀: false);
        Blocking = true;
        BindGrid();
        ObjectID = ++MapManager.ObjectID;
        MapManager.AddObject(this);
        Activated = true;
        MapManager.AddActiveObject(this);
        UpdateAllNeighbours();
    }

    public override void Process()
    {
        if (SEngine.CurrentTime < base.ProcessTime)
        {
            return;
        }
        if (Dead)
        {
            if (!尸体消失 && SEngine.CurrentTime >= VanishingTime)
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
                    CurrentHP += this[Stat.HealthRecovery];
                }
                base.RecoveryTime = SEngine.CurrentTime.AddSeconds(5.0);
            }
            if (SEngine.CurrentTime > base.HealTime && base.治疗次数 > 0)
            {
                base.治疗次数--;
                base.HealTime = SEngine.CurrentTime.AddMilliseconds(500.0);
                CurrentHP += base.治疗基数;
            }
            if (进入战斗技能 != null && !base.CombatStance && Target.TargetList.Count != 0)
            {
                new SkillObject(this, 进入战斗技能, null, base.ActionID++, CurrentMap, CurrentPosition, null, CurrentPosition, null);
                base.CombatStance = true;
                base.AttackStopTime = SEngine.CurrentTime.AddSeconds(10.0);
            }
            else if (退出战斗技能 != null && base.CombatStance && Target.TargetList.Count == 0 && SEngine.CurrentTime > base.AttackStopTime)
            {
                new SkillObject(this, 退出战斗技能, null, base.ActionID++, CurrentMap, CurrentPosition, null, CurrentPosition, null);
                base.CombatStance = false;
            }
            else if (Master.PetMode == PetMode.Attack && SEngine.CurrentTime > BusyTime && SEngine.CurrentTime > HardStunTime)
            {
                if (Target.Target == null && !Neighbors.Contains(Master))
                {
                    宠物智能跟随();
                }
                else if (UpdateTargets())
                {
                    宠物智能攻击();
                }
                else
                {
                    宠物智能跟随();
                }
            }
        }
        base.Process();
    }

    public override void Die(MapObject 对象, bool 技能击杀)
    {
        if (死亡释放技能 != null && 对象 != null)
        {
            new SkillObject(this, 死亡释放技能, null, base.ActionID++, CurrentMap, CurrentPosition, null, CurrentPosition, null).Process();
        }
        base.Die(对象, 技能击杀);
        VanishingTime = SEngine.CurrentTime.AddMilliseconds(CorpsePreservationDuration);
        RemoveTargets();
        Master?.宠物死亡处理(this);
        Master?.PetInfo.Remove(PInfo);
        Master?.Pets.Remove(this);
        int? num = Master?.PetCount;
        if ((num.GetValueOrDefault() == 0) & num.HasValue)
        {
            Master?.Enqueue(new GameErrorMessagePacket
            {
                ErrorCode = 9473
            });
        }
        Buffs.Clear();
        PInfo?.Remove();
        SecondaryObject = true;
        MapManager.AddSecondaryObject(this);
        Activated = false;
        MapManager.RemoveActiveObject(this);
    }

    public void 宠物智能跟随()
    {
        if (!CanWalk())
        {
            return;
        }
        if (Neighbors.Contains(Master))
        {
            Point point = Compute.GetNextPosition(Master.CurrentPosition, Compute.RotateDirection(Master.CurrentDirection, 4), 2);
            if (GetDistance(Master) <= 2 && GetDistance(point) <= 2)
            {
                if (SEngine.CurrentTime > RoamingTime)
                {
                    RoamingTime = SEngine.CurrentTime.AddMilliseconds(RoamInterval + SEngine.Random.Next(5000));
                    Point point2 = Compute.GetNextPosition(CurrentPosition, Compute.RandomDirection(), 1);
                    if (CurrentMap.CanMove(point2))
                    {
                        BusyTime = SEngine.CurrentTime.AddMilliseconds(WalkInterval);
                        WalkTime = SEngine.CurrentTime.AddMilliseconds(WalkInterval + MoveInterval);
                        CurrentDirection = Compute.DirectionFromPoint(CurrentPosition, point2);
                        OnMoved(point2);
                        SendPacket(new ObjectWalkPacket
                        {
                            ObjectID = ObjectID,
                            Position = CurrentPosition,
                            MovementSpeed = base.WalkSpeed
                        });
                    }
                }
                return;
            }
            GameDirection 方向 = Compute.DirectionFromPoint(CurrentPosition, point);
            int num = 0;
            Point point3;
            while (true)
            {
                if (num < 8)
                {
                    point3 = Compute.GetNextPosition(CurrentPosition, 方向, 1);
                    if (CurrentMap.CanMove(point3))
                    {
                        break;
                    }
                    方向 = Compute.RotateDirection(方向, (SEngine.Random.Next(2) != 0) ? 1 : (-1));
                    num++;
                    continue;
                }
                return;
            }
            BusyTime = SEngine.CurrentTime.AddMilliseconds(WalkInterval);
            WalkTime = SEngine.CurrentTime.AddMilliseconds(WalkInterval + MoveInterval);
            CurrentDirection = Compute.DirectionFromPoint(CurrentPosition, point3);
            OnMoved(point3);
            SendPacket(new ObjectWalkPacket
            {
                ObjectID = ObjectID,
                Position = CurrentPosition,
                MovementSpeed = base.WalkSpeed
            });
        }
        else
        {
            PetRecall();
        }
    }

    public void 宠物智能攻击()
    {
        if (CheckStatus(GameObjectState.Paralyzed | GameObjectState.Unconscious))
        {
            return;
        }
        GameSkill 游戏技能;
        if (概率触发技能 != null && (!Cooldowns.ContainsKey(普通攻击技能.OwnSkillID | 0x1000000) || SEngine.CurrentTime > Cooldowns[普通攻击技能.OwnSkillID | 0x1000000]) && Compute.CalculateProbability(概率触发技能.CalculateLuckyProbability ? Compute.CalcLuck(this[Stat.Luck]) : 概率触发技能.CalculateTriggerProbability))
        {
            游戏技能 = 概率触发技能;
        }
        else
        {
            if (普通攻击技能 == null || (Cooldowns.ContainsKey(普通攻击技能.OwnSkillID | 0x1000000) && !(SEngine.CurrentTime > Cooldowns[普通攻击技能.OwnSkillID | 0x1000000])))
            {
                return;
            }
            游戏技能 = 普通攻击技能;
        }
        if (GetDistance(Target.Target) > 游戏技能.MaxDistance)
        {
            if (!CanWalk())
            {
                return;
            }
            GameDirection 方向 = Compute.DirectionFromPoint(CurrentPosition, Target.Target.CurrentPosition);
            bool flag = false;
            for (int i = 0; i < 10; i++)
            {
                Point point = Compute.GetNextPosition(CurrentPosition, 方向, 1);
                if (!CurrentMap.CanMove(point))
                {
                    方向 = Compute.RotateDirection(方向, (SEngine.Random.Next(2) != 0) ? 1 : (-1));
                    continue;
                }
                BusyTime = SEngine.CurrentTime.AddMilliseconds(WalkInterval);
                WalkTime = SEngine.CurrentTime.AddMilliseconds(WalkInterval + MoveInterval);
                CurrentDirection = Compute.DirectionFromPoint(CurrentPosition, point);
                OnMoved(point);
                SendPacket(new ObjectWalkPacket
                {
                    ObjectID = ObjectID,
                    Position = point,
                    MovementSpeed = base.WalkSpeed
                });
                flag = true;
                break;
            }
            if (!flag)
            {
                Target.Remove(Target.Target);
            }
        }
        else if (SEngine.CurrentTime > AttackTime)
        {
            new SkillObject(this, 游戏技能, null, base.ActionID++, CurrentMap, CurrentPosition, Target.Target, Target.Target.CurrentPosition, null);
            AttackTime = SEngine.CurrentTime.AddMilliseconds(Compute.Clamp(0, 10 - this[Stat.AttackSpeed], 10) * 500);
        }
        else if (CanTurn())
        {
            CurrentDirection = Compute.DirectionFromPoint(CurrentPosition, Target.Target.CurrentPosition);
        }
    }

    public void GainExperience()
    {
        if (PetLevel < PetMaxLevel && ++CurrentExp >= MaxExp)
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
            SendPacket(new 同步宠物等级
            {
                ObjectID = ObjectID,
                PetLevel = PetLevel
            });
        }
    }

    public void PetRecall()
    {
        Point location = Master.CurrentPosition;
        for (int i = 1; i <= 120; i++)
        {
            Point point = Compute.GetPositionAround(location, i);
            if (Master.CurrentMap.CanMove(point))
            {
                location = point;
                break;
            }
        }
        RemoveTargets();
        RemoveAllNeighbors();
        UnbindGrid();
        CurrentPosition = location;
        CurrentMap = Master?.CurrentMap;
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

    public bool UpdateTargets()
    {
        if (Target.TargetList.Count == 0)
        {
            return false;
        }
        if (Target.Target == null)
        {
            Target.切换时间 = default(DateTime);
        }
        else if (Target.Target.Dead)
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
            Target.TargetList[Target.Target].HateTime = SEngine.CurrentTime.AddMilliseconds(仇恨时长);
        }
        if (Target.切换时间 < SEngine.CurrentTime && Target.切换仇恨(this))
        {
            Target.切换时间 = SEngine.CurrentTime.AddMilliseconds(切换间隔);
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
