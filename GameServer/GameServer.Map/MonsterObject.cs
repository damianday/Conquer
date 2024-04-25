using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using GameServer.Database;
using GameServer.Template;
using GameServer.Networking;
using GameServer.Skill;

using GamePackets.Server;

namespace GameServer.Map;

public sealed class MonsterObject : MapObject
{
    public byte PetLevel;

    public MonsterInfo Info;

    public int ResurrectionInterval;

    public HateObject Target;

    public Point[] BirthRange;
    public Map BirthMap;

    public GameSkill NormalAttackSkill;
    public List<GameSkill> RandomAttackSkill = new List<GameSkill>();
    public GameSkill EnterCombatSkill;
    public GameSkill ExitCombatSkill;
    public GameSkill BirthSkill;
    public GameSkill DeathSkill;
    public GameSkill MoveSkill;

    public bool ForbidResurrection { get; set; }
    public bool Disappeared { get; set; }

    public DateTime AttackTime { get; set; }
    public DateTime RoamTime { get; set; }
    public DateTime ResurrectionTime { get; set; }
    public DateTime DisappearTime { get; set; }
    public DateTime SurvivalTime { get; set; }

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
        get { return base.CurrentHP; }
        set
        {
            value = Compute.Clamp(0, value, this[Stat.MaxHP]);
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
                SendPacket(new SyncObjectDirectionPacket { ActionTime = 100, ObjectID = ObjectID, Direction = (ushort)value });
            }
        }
    }

    public override byte CurrentLevel => Info.Level;
    public override string Name => Info.MonsterName;
    public override GameObjectType ObjectType => GameObjectType.Monster;
    public override ObjectSize Size => Info.Size;

    public override int this[Stat stat]
    {
        get { return base[stat]; }
        set { base[stat] = value; }
    }

    public MonsterRaceType Race => Info.Race;
    public MonsterGradeType Grade => Info.Grade;

    public List<MonsterDrop> Drops => Info.Drops;
    public ushort MonID => Info.ID;
    public int Experience => Info.ProvideExperience;

    public int TargetRange
    {
        get
        {
            if (CurrentMap.MapID != 80)
                return Info.RangeHate;
            return 25;
        }
    }

    public int MoveInterval => Info.MoveInterval;
    public int TargetSelecthInterval => 5000;
    public int RoamInterval => Info.RoamInterval;
    public int HateTime => Info.HateTime;
    public int CorpsePreservationDuration => Info.CorpsePreservationDuration;
    public bool ForbbidenMove => Info.ForbbidenMove;
    public bool CanBePushedBySkill => Info.CanBeDrivenBySkills;
    public bool VisibleStealthTargets => Info.VisibleStealthTargets;
    public bool CanBeControlledBySkills => Info.CanBeControlledBySkills;
    public bool CanBeSeducedBySkills => Info.CanBeSeducedBySkills;
    public float BaseTemptationProbability => Info.BaseTemptationProbability;
    public bool ActiveAttackTarget => Info.ActiveAttackTarget;

    public MonsterObject(PetObject obj)
    {
        ObjectID = ++MapManager.ObjectID;
        Info = obj.MInfo;
        CurrentMap = obj.CurrentMap;
        CurrentPosition = obj.CurrentPosition;
        CurrentDirection = obj.CurrentDirection;
        PetLevel = obj.PetLevel;
        ForbidResurrection = true;

        Target = new HateObject();
        SurvivalTime = SEngine.CurrentTime.AddHours(2.0);
        RecoveryTime = SEngine.CurrentTime.AddSeconds(5.0);
        AttackTime = SEngine.CurrentTime.AddSeconds(1.0);
        RoamTime = SEngine.CurrentTime.AddMilliseconds(RoamInterval);

        BonusStats[this] = obj.BaseStats;
        RefreshStats();

        CurrentHP = Math.Min(obj.CurrentHP, this[Stat.MaxHP]);

        if (!string.IsNullOrEmpty(Info.NormalAttackSkills))
            GameSkill.DataSheet.TryGetValue(Info.NormalAttackSkills, out NormalAttackSkill);

        foreach (var name in Info.RandomTriggerSkills)
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (GameSkill.DataSheet.TryGetValue(name, out var skill))
                    RandomAttackSkill.Add(skill);
            }
        }

        if (!string.IsNullOrEmpty(Info.EnterCombatSkills))
            GameSkill.DataSheet.TryGetValue(Info.EnterCombatSkills, out EnterCombatSkill);

        if (!string.IsNullOrEmpty(Info.ExitCombatSkills))
            GameSkill.DataSheet.TryGetValue(Info.ExitCombatSkills, out ExitCombatSkill);

        if (!string.IsNullOrEmpty(Info.BirthReleaseSkill))
            GameSkill.DataSheet.TryGetValue(Info.BirthReleaseSkill, out BirthSkill);

        if (!string.IsNullOrEmpty(Info.DeathReleaseSkill))
            GameSkill.DataSheet.TryGetValue(Info.DeathReleaseSkill, out DeathSkill);

        if (!string.IsNullOrEmpty(Info.MoveReleaseSkill))
            GameSkill.DataSheet.TryGetValue(Info.MoveReleaseSkill, out MoveSkill);

        obj.Die(null, false);
        obj.Despawn();

        Dead = false;
        CombatStance = false;
        Blocking = true;
        BindGrid();
        UpdateAllNeighbours();
        MapManager.AddObject(this);
    }

    public MonsterObject(MonsterInfo info, Map map, int resInterval, Point[] locations, bool forbidResurrection, bool 立即刷新)
    {
        ObjectID = ++MapManager.ObjectID;
        Info = info;
        BirthMap = map;
        CurrentMap = map;
        ResurrectionInterval = resInterval;
        BirthRange = locations;
        ForbidResurrection = forbidResurrection;
        
        BonusStats[this] = info.Stats;
 
        if (!string.IsNullOrEmpty(Info.NormalAttackSkills))
            GameSkill.DataSheet.TryGetValue(Info.NormalAttackSkills, out NormalAttackSkill);

        RandomAttackSkill.Clear();
        foreach (var name in Info.RandomTriggerSkills)
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (GameSkill.DataSheet.TryGetValue(name, out var skill))
                    RandomAttackSkill.Add(skill);
            }
        }
 
        if (!string.IsNullOrEmpty(Info.EnterCombatSkills))
            GameSkill.DataSheet.TryGetValue(Info.EnterCombatSkills, out EnterCombatSkill);

        if (!string.IsNullOrEmpty(Info.ExitCombatSkills))
            GameSkill.DataSheet.TryGetValue(Info.ExitCombatSkills, out ExitCombatSkill);

        if (!string.IsNullOrEmpty(Info.BirthReleaseSkill))
            GameSkill.DataSheet.TryGetValue(Info.BirthReleaseSkill, out BirthSkill);

        if (!string.IsNullOrEmpty(Info.DeathReleaseSkill))
            GameSkill.DataSheet.TryGetValue(Info.DeathReleaseSkill, out DeathSkill);

        if (!string.IsNullOrEmpty(Info.MoveReleaseSkill))
            GameSkill.DataSheet.TryGetValue(Info.MoveReleaseSkill, out MoveSkill);

        MapManager.AddObject(this);
        if (!forbidResurrection)
        {
            CurrentMap.TotalFixedMonsters++;
            SMain.更新地图数据(CurrentMap, "固定怪物总数", CurrentMap.TotalFixedMonsters);
        }
        if (立即刷新)
        {
            Resurrect(false);
            return;
        }

        ResurrectionTime = SEngine.CurrentTime.AddMilliseconds(resInterval);
        Blocking = false;
        Disappeared = true;
        Dead = true;
        SecondaryObject = true;
        MapManager.AddSecondaryObject(this);
    }

    public override void Process()
    {
        if (SEngine.CurrentTime < ProcessTime)
            return;

        if (ForbidResurrection && SEngine.CurrentTime >= SurvivalTime)
            Despawn();
        else if (Dead)
        {
            if (!Disappeared && SEngine.CurrentTime >= DisappearTime)
            {
                if (ForbidResurrection)
                {
                    Despawn();
                }
                else
                {
                    Disappeared = true;
                    RemoveAllNeighbors();
                    UnbindGrid();
                }
            }
            if (!ForbidResurrection && SEngine.CurrentTime >= ResurrectionTime)
            {
                RemoveAllNeighbors();
                UnbindGrid();
                Resurrect(true);
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

            if (SEngine.CurrentTime > BusyTime && SEngine.CurrentTime > HardStunTime)
            {
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
                else if (Info.OutWarAutomaticPetrochemical && !CombatStance && Target.TargetList.Count != 0)
                {
                    CombatStance = true;
                    RemoveBuffEx(Info.PetrochemicalStatusID);
                    AttackStopTime = SEngine.CurrentTime.AddSeconds(10.0);
                }
                else if (Info.OutWarAutomaticPetrochemical && CombatStance && Target.TargetList.Count == 0 && SEngine.CurrentTime > AttackStopTime)
                {
                    CombatStance = false;
                    AddBuff(Info.PetrochemicalStatusID, this);
                }
                else if ((Grade == MonsterGradeType.Boss) ? UpdateBestTarget() : UpdateTarget())
                {
                    ProcessAttack();
                }
                else
                {
                    ProcessRoam();
                }
            }
        }
        base.Process();
    }

    public override void Die(MapObject attacker, bool skillDeath)
    {
        foreach (var skill in ActiveSkills)
            skill.Stop();

        base.Die(attacker, skillDeath);

        if (DeathSkill != null && attacker != null)
        {
            new SkillObject(this, DeathSkill, null, ActionID++, CurrentMap, CurrentPosition, null, CurrentPosition, null).Process();
        }

        if (CurrentMap.QuestMap || !ForbidResurrection)
            CurrentMap.TotalSurvivingMonsters--;
        if (CurrentMap.MapID == 227)
            CurrentMap.TotalSurvivingMonsters--;

        Disappeared = false;
        DisappearTime = SEngine.CurrentTime.AddMilliseconds(CorpsePreservationDuration);
        ResurrectionTime = SEngine.CurrentTime.AddMilliseconds(Math.Max(ResurrectionInterval, CorpsePreservationDuration + 5000));

        var guardKill = (attacker is GuardObject) ? true : false;

        (attacker as PetObject)?.GainExperience();
        if (Grade == MonsterGradeType.DarkGateMonster)
        {
            (attacker as PlayerObject)?.玩家杀怪增加();
        }
        
        if (GetKillOwner(out var hitter))
        {
            if (CurrentMap.MapID == 80 && Config.屠魔爆率开关 == 0)
            {
                int num = 0;
                if (GameItem.DataSheetByName.TryGetValue("强效金创药", out var value))
                {
                    int num2 = ((Grade != MonsterGradeType.Normal) ? 1 : 15);
                    int num3 = Math.Max(1, num2 - (int)Math.Round((decimal)num2 * Config.ItemDropRate));
                    if (SEngine.Random.Next(num3) == num3 / 2)
                    {
                        num++;
                        new ItemObject(value, null, CurrentMap, CurrentPosition, new HashSet<CharacterInfo>(), 1);
                    }
                }
                if (GameItem.DataSheetByName.TryGetValue("强效魔法药", out var value2))
                {
                    int num4 = ((Grade != MonsterGradeType.Normal) ? 1 : 20);
                    int num5 = Math.Max(1, num4 - (int)Math.Round((decimal)num4 * Config.ItemDropRate));
                    if (SEngine.Random.Next(num5) == num5 / 2)
                    {
                        num++;
                        new ItemObject(value2, null, CurrentMap, CurrentPosition, new HashSet<CharacterInfo>(), 1);
                    }
                }
                if (GameItem.DataSheetByName.TryGetValue("疗伤药", out var value3))
                {
                    int num6 = ((Grade != MonsterGradeType.Normal) ? 1 : 100);
                    int num7 = Math.Max(1, num6 - (int)Math.Round((decimal)num6 * Config.ItemDropRate));
                    if (SEngine.Random.Next(num7) == num7 / 2)
                    {
                        num++;
                        new ItemObject(value3, null, CurrentMap, CurrentPosition, new HashSet<CharacterInfo>(), 1);
                    }
                }
                if (GameItem.DataSheetByName.TryGetValue("祝福油", out var value4))
                {
                    int num8 = ((Grade == MonsterGradeType.Normal) ? 1000 : ((Grade == MonsterGradeType.Elite) ? 50 : 10));
                    int num9 = Math.Max(1, num8 - (int)Math.Round((decimal)num8 * Config.ItemDropRate));
                    if (SEngine.Random.Next(num9) == num9 / 2)
                    {
                        num++;
                        new ItemObject(value4, null, CurrentMap, CurrentPosition, new HashSet<CharacterInfo>(), 1);
                        NetworkManager.SendAnnouncement("[" + Name + "] 被 [" + hitter.Name + "] 击杀, 掉落了[祝福油]");
                    }
                }
                if (num > 0)
                {
                    SMain.更新地图数据(CurrentMap, "怪物掉落次数", num);
                }
                foreach (var player in CurrentMap.Players)
                {
                    player.GainExperience(this, (int)((float)Experience * 1.5f));
                }
            }
            else
            {
                if (!guardKill || Config.GuardKillWillDrop)
                {
                    DropItems(hitter);

                    if (hitter.Team == null)
                    {
                        hitter.GainExperience(this, Experience);
                    }
                    else
                    {
                        List<PlayerObject> nearby = new List<PlayerObject>();
                        nearby.Add(hitter);

                        foreach (var obj in ImportantNeighbors)
                        {
                            if (obj != hitter && obj is PlayerObject player && player.Team == hitter.Team)
                                nearby.Add(player);
                        }
                        float rate = (float)Experience * (1f + (float)(nearby.Count - 1) * 0.2f);
                        float sumlv = nearby.Sum(x => x.CurrentLevel);
                        foreach (var player in nearby)
                        {
                            player.GainExperience(this, (int)(rate * (float)(int)player.CurrentLevel / sumlv));
                        }
                    }
                }
            }
        }

        Buffs.Clear();
        SecondaryObject = true;
        MapManager.AddSecondaryObject(this);
        if (Activated)
        {
            Activated = false;
            MapManager.RemoveActiveObject(this);
        }
    }

    public void ProcessRoam()
    {
        if (ForbbidenMove || Dead) return;
        if (SEngine.CurrentTime < RoamTime) return;

        RoamTime = SEngine.CurrentTime.AddMilliseconds(RoamInterval + SEngine.Random.Next(5_000));

        switch (SEngine.Random.Next(3))
        {
            case 0:
                Turn(Compute.RandomDirection());
                break;
            default:
                Walk(CurrentDirection);
                break;
        }
    }

    public void ProcessAttack()
    {
        if (Dead) return;

        AttackStopTime = SEngine.CurrentTime.AddSeconds(10.0);

        if (CheckStatus(GameObjectState.BusyGreen | GameObjectState.Paralyzed | GameObjectState.Unconscious))
            return;

        GameSkill skill = null;

        if (RandomAttackSkill.Count > 0)
        {
            var rskill = RandomAttackSkill[SEngine.Random.Next(RandomAttackSkill.Count)];

            if (rskill != null && 
                (!Cooldowns.ContainsKey(rskill.OwnSkillID | 0x1000000) || SEngine.CurrentTime > Cooldowns[rskill.OwnSkillID | 0x1000000]) &&
                Compute.CalculateProbability(rskill.CalculateLuckyProbability ? Compute.CalcLuck(this[Stat.Luck]) : rskill.CalculateTriggerProbability))
                //Compute.CalculateProbability(rskill.CalculateTriggerProbability))
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

            Walk(dir);
        }
        else if (skill.NeedMoveForward && !Compute.直线方向(CurrentPosition, Target.Target.CurrentPosition))
        {
            var dir = Compute.正向方向(CurrentPosition, Target.Target.CurrentPosition);
            for (var i = 0; i < 8; i++)
            {
                var point = Compute.GetNextPosition(CurrentPosition, dir, 1);
                if (CurrentMap.CanMove(point))
                    break;

                dir = Compute.RotateDirection(dir, (SEngine.Random.Next(2) != 0) ? 1 : -1);
            }

            Walk(dir);
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
        if (ForbbidenMove || !CanTurn()) return false;

        CurrentDirection = direction;
        return true;
    }

    private bool Walk(GameDirection direction)
    {
        if (ForbbidenMove || !CanWalk()) return false;

        var location = Compute.GetNextPosition(CurrentPosition, direction, 1);
        if (!CurrentMap.CanMove(location)) return false;

        BusyTime = SEngine.CurrentTime.AddMilliseconds(WalkInterval);
        WalkTime = SEngine.CurrentTime.AddMilliseconds(WalkInterval + MoveInterval);
        CurrentDirection = Compute.DirectionFromPoint(CurrentPosition, location);

        SendPacket(new ObjectWalkPacket { ObjectID = ObjectID, Position = location, Speed = WalkSpeed });
        OnLocationChanged(location);
        return true;
    }

    private void DropItems(PlayerObject hitter)
    {
        HashSet<CharacterInfo> members;
        if (hitter.Team != null)
        {
            members = new HashSet<CharacterInfo>(hitter.Team.Members);
        }
        else
        {
            members = new HashSet<CharacterInfo>();
            members.Add(hitter.Character);
        }

        float chance = Compute.CalculateLevelRatio(hitter.CurrentLevel, CurrentLevel);
        int goldCount = 0;
        int itemCount = 0;
        if (chance < 1f && Config.DropRateModifier == 0)
        {
            foreach (MonsterDrop drop in Drops)
            {
                if (!GameItem.DataSheetByName.TryGetValue(drop.Name, out var item) || 
                    Compute.CalculateProbability(chance) || 
                    (hitter.CurrentDegree == 0 && Grade != MonsterGradeType.Boss && item.Type != ItemType.可用药剂 && Compute.CalculateProbability(0.5f)) || 
                    (hitter.CurrentDegree == 3 && Grade != MonsterGradeType.Boss && item.Type != ItemType.可用药剂 && Compute.CalculateProbability(0.25f)))
                {
                    continue;
                }

                int num13 = Math.Max(1, drop.Probability - (int)Math.Round((decimal)drop.Probability * Config.ItemDropRate));
                if (SEngine.Random.Next(num13) != num13 / 2)
                    continue;

                int amount = SEngine.Random.Next(drop.MinAmount, drop.MaxAmount + 1);
                if (amount == 0)
                    continue;

                if (item.MaxDura == 0)
                {
                    new ItemObject(item, null, CurrentMap, CurrentPosition, members, amount);
                    if (item.ID == 1)
                    {
                        CurrentMap.TotalAmountGoldDrops += amount;
                        goldCount = amount;
                    }
                    Info.DropStats[item] = (Info.DropStats.ContainsKey(item) ? Info.DropStats[item] : 0) + amount;
                    continue;
                }
                for (int i = 0; i < amount; i++)
                {
                    new ItemObject(item, null, CurrentMap, CurrentPosition, members, 1);
                }

                CurrentMap.TotalAmountMonsterDrops += amount;
                itemCount++;
                Info.DropStats[item] = (Info.DropStats.TryGetValue(item, out var value) ? value : 0) + amount;

                AnnounceItemDrop(hitter, item);
            }
        }

        if (chance < 1f && Config.DropRateModifier == 1 && Config.CurrentVersion >= 1)
        {
            int num15 = 0;
            int num16 = 0;
            int num17 = 0;
            int num18 = 0;
            int num19 = 0;
            int num20 = 0;
            int num21 = 0;
            int num22 = 0;
            int num23 = 0;
            int num24 = 0;
            int num25 = 0;
            int num26 = 0;
            int num27 = 0;
            int num28 = 0;
            int num29 = 0;
            int num30 = 0;
            int num31 = 0;
            int num32 = 0;
            int num33 = 0;
            int num34 = 0;
            int num35 = 0;

            foreach (MonsterDrop drop in Drops)
            {
                if (!GameItem.DataSheetByName.TryGetValue(drop.Name, out var item) || 
                    Compute.CalculateProbability(chance) || 
                    (hitter.CurrentDegree == 0 && Grade != MonsterGradeType.Boss && item.Type != ItemType.可用药剂 && Compute.CalculateProbability(0.5f)) || 
                    (hitter.CurrentDegree == 3 && Grade != MonsterGradeType.Boss && item.Type != ItemType.可用药剂 && Compute.CalculateProbability(0.25f)) || 
                    (num15 == 1000 && drop.爆率分组 == 1000) || (num16 == 1001 && drop.爆率分组 == 1001) || (num17 == 1002 && drop.爆率分组 == 1002) || (num18 == 1003 && drop.爆率分组 == 1003) || (num19 == 1004 && drop.爆率分组 == 1004) || (num20 == 1005 && drop.爆率分组 == 1005) || (num21 == 1006 && drop.爆率分组 == 1006) || (num22 == 1007 && drop.爆率分组 == 1007) || (num23 == 1008 && drop.爆率分组 == 1008) || (num24 == 1009 && drop.爆率分组 == 1009) || (num25 == 1010 && drop.爆率分组 == 1010) || (num26 == 1011 && drop.爆率分组 == 1011) || (num27 == 1012 && drop.爆率分组 == 1012) || (num28 == 1013 && drop.爆率分组 == 1013) || (num29 == 1014 && drop.爆率分组 == 1014) || (num30 == 1015 && drop.爆率分组 == 1015) || (num31 == 1016 && drop.爆率分组 == 1016) || (num32 == 1017 && drop.爆率分组 == 1017) || (num33 == 1018 && drop.爆率分组 == 1018) || (num34 == 1019 && drop.爆率分组 == 1019) || (num35 == 1020 && drop.爆率分组 == 1020))
                {
                    continue;
                }

                int num36 = Math.Max(1, drop.Probability - (int)Math.Round((decimal)drop.Probability * Config.ItemDropRate));
                if (SEngine.Random.Next(num36) != num36 / 2)
                    continue;

                int amount = SEngine.Random.Next(drop.MinAmount, drop.MaxAmount + 1);
                if (amount == 0)
                    continue;

                if (item.MaxDura == 0)
                {
                    if (drop.爆率分组 == 1000)
                    {
                        num15 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1001)
                    {
                        num16 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1002)
                    {
                        num17 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1003)
                    {
                        num18 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1004)
                    {
                        num19 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1005)
                    {
                        num20 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1006)
                    {
                        num21 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1007)
                    {
                        num22 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1008)
                    {
                        num23 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1009)
                    {
                        num24 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1010)
                    {
                        num25 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1011)
                    {
                        num26 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1012)
                    {
                        num27 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1013)
                    {
                        num28 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1014)
                    {
                        num29 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1015)
                    {
                        num30 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1016)
                    {
                        num31 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1017)
                    {
                        num32 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1018)
                    {
                        num33 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1019)
                    {
                        num34 = drop.爆率分组;
                    }
                    if (drop.爆率分组 == 1020)
                    {
                        num35 = drop.爆率分组;
                    }
                    new ItemObject(item, null, CurrentMap, CurrentPosition, members, amount);
                    if (item.ID == 1)
                    {
                        CurrentMap.TotalAmountGoldDrops += amount;
                        goldCount = amount;
                    }
                    Info.DropStats[item] = (Info.DropStats.TryGetValue(item, out var value) ? value : 0) + amount;
                    continue;
                }
                for (int j = 0; j < amount; j++)
                {
                    if ((num15 != 1000 || drop.爆率分组 != 1000) && (num16 != 1001 || drop.爆率分组 != 1001) && (num17 != 1002 || drop.爆率分组 != 1002) && (num18 != 1003 || drop.爆率分组 != 1003) && (num19 != 1004 || drop.爆率分组 != 1004) && (num20 != 1005 || drop.爆率分组 != 1005) && (num21 != 1006 || drop.爆率分组 != 1006) && (num22 != 1007 || drop.爆率分组 != 1007) && (num23 != 1008 || drop.爆率分组 != 1008) && (num24 != 1009 || drop.爆率分组 != 1009) && (num25 != 1010 || drop.爆率分组 != 1010) && (num26 != 1011 || drop.爆率分组 != 1011) && (num27 != 1012 || drop.爆率分组 != 1012) && (num28 != 1013 || drop.爆率分组 != 1013) && (num29 != 1014 || drop.爆率分组 != 1014) && (num30 != 1015 || drop.爆率分组 != 1015) && (num31 != 1016 || drop.爆率分组 != 1016) && (num32 != 1017 || drop.爆率分组 != 1017) && (num33 != 1018 || drop.爆率分组 != 1018) && (num34 != 1019 || drop.爆率分组 != 1019) && (num35 != 1020 || drop.爆率分组 != 1020))
                    {
                        if (drop.爆率分组 == 1000)
                        {
                            num15 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1001)
                        {
                            num16 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1002)
                        {
                            num17 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1003)
                        {
                            num18 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1004)
                        {
                            num19 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1005)
                        {
                            num20 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1006)
                        {
                            num21 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1007)
                        {
                            num22 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1008)
                        {
                            num23 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1009)
                        {
                            num24 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1010)
                        {
                            num25 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1011)
                        {
                            num26 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1012)
                        {
                            num27 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1013)
                        {
                            num28 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1014)
                        {
                            num29 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1015)
                        {
                            num30 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1016)
                        {
                            num31 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1017)
                        {
                            num32 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1018)
                        {
                            num33 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1019)
                        {
                            num34 = drop.爆率分组;
                        }
                        if (drop.爆率分组 == 1020)
                        {
                            num35 = drop.爆率分组;
                        }
                        new ItemObject(item, null, CurrentMap, CurrentPosition, members, 1);
                    }
                }
                CurrentMap.TotalAmountMonsterDrops += amount;
                itemCount++;
                Info.DropStats[item] = (Info.DropStats.ContainsKey(item) ? Info.DropStats[item] : 0) + amount;

                AnnounceItemDrop(hitter, item);
            }
        }

        if (goldCount > 0)
        {
            SMain.更新地图数据(CurrentMap, "金币掉落总数", goldCount);
        }
        if (itemCount > 0)
        {
            SMain.更新地图数据(CurrentMap, "怪物掉落次数", itemCount);
        }
        if (goldCount > 0 || itemCount > 0)
        {
            SMain.更新掉落统计(Info, Info.DropStats.ToList());
        }
    }

    private void AnnounceItemDrop(PlayerObject hitter, GameItem item)
    {
        static string GetDropUserNameColor()
        {
            return Config.DropPlayerNameColor switch
            {
                0 => "#f9e01d",
                1 => "#7dea00",
                2 => "#EA0000",
                3 => "#0003ea",
                4 => "#ff4aea",
                5 => "#ffad4a",
                6 => "#6bcbd2",
                7 => "#ff4abf",
                8 => "#fbf1f7",
                _ => string.Empty
            };
        }


        if (Config.CurrentVersion >= 1)
        {
            if (Config.怪物掉落广播开关 == 1 && item.ValuableObjects)
            {
                string color1 = null, color2 = null;

                if (Config.CurrentVersion >= 1 && item.装备套装提示 == GameItemSet.沃玛装备)
                {
                    color1 = GetDropUserNameColor();
                    
                    if (Config.掉落沃玛物品颜色 == 0)
                    {
                        color2 = "#f9e01d";
                    }
                    else if (Config.掉落沃玛物品颜色 == 1)
                    {
                        color2 = "#7dea00";
                    }
                    else if (Config.掉落沃玛物品颜色 == 2)
                    {
                        color2 = "#EA0000";
                    }
                    else if (Config.掉落沃玛物品颜色 == 3)
                    {
                        color2 = "#0003ea";
                    }
                    else if (Config.掉落沃玛物品颜色 == 4)
                    {
                        color2 = "#ff4aea";
                    }
                    else if (Config.掉落沃玛物品颜色 == 5)
                    {
                        color2 = "#ffad4a";
                    }
                    else if (Config.掉落沃玛物品颜色 == 6)
                    {
                        color2 = "#6bcbd2";
                    }
                    else if (Config.掉落沃玛物品颜色 == 7)
                    {
                        color2 = "#ff4abf";
                    }
                    else if (Config.掉落沃玛物品颜色 == 8)
                    {
                        color2 = "#fbf1f7";
                    }
                    if (item.装备套装提示 == GameItemSet.沃玛装备)
                    {
                        NetworkManager.SendAnnouncement("[" + Name + "] 被 [<font color = '" + color1 + "'>" + hitter.Name + "</font>]击杀, 掉落了[<font color = '" + color2 + "'>" + item.Name + "</font>]");
                    }
                }
                if (Config.CurrentVersion >= 1 && item.装备套装提示 == GameItemSet.祖玛装备)
                {
                    color1 = GetDropUserNameColor();

                    if (Config.掉落祖玛物品颜色 == 0)
                    {
                        color2 = "#f9e01d";
                    }
                    else if (Config.掉落祖玛物品颜色 == 1)
                    {
                        color2 = "#7dea00";
                    }
                    else if (Config.掉落祖玛物品颜色 == 2)
                    {
                        color2 = "#EA0000";
                    }
                    else if (Config.掉落祖玛物品颜色 == 3)
                    {
                        color2 = "#0003ea";
                    }
                    else if (Config.掉落祖玛物品颜色 == 4)
                    {
                        color2 = "#ff4aea";
                    }
                    else if (Config.掉落祖玛物品颜色 == 5)
                    {
                        color2 = "#ffad4a";
                    }
                    else if (Config.掉落祖玛物品颜色 == 6)
                    {
                        color2 = "#6bcbd2";
                    }
                    else if (Config.掉落祖玛物品颜色 == 7)
                    {
                        color2 = "#ff4abf";
                    }
                    else if (Config.掉落祖玛物品颜色 == 8)
                    {
                        color2 = "#fbf1f7";
                    }
                    if (item.装备套装提示 == GameItemSet.祖玛装备)
                    {
                        NetworkManager.SendAnnouncement("[" + Name + "] 被 [<font color = '" + color1 + "'>" + hitter.Name + "</font>]击杀, 掉落了[<font color = '" + color2 + "'>" + item.Name + "</font>]");
                    }
                }
                if (Config.CurrentVersion >= 1 && item.装备套装提示 == GameItemSet.赤月装备)
                {
                    color1 = GetDropUserNameColor();

                    if (Config.掉落赤月物品颜色 == 0)
                    {
                        color2 = "#f9e01d";
                    }
                    else if (Config.掉落赤月物品颜色 == 1)
                    {
                        color2 = "#7dea00";
                    }
                    else if (Config.掉落赤月物品颜色 == 2)
                    {
                        color2 = "#EA0000";
                    }
                    else if (Config.掉落赤月物品颜色 == 3)
                    {
                        color2 = "#0003ea";
                    }
                    else if (Config.掉落赤月物品颜色 == 4)
                    {
                        color2 = "#ff4aea";
                    }
                    else if (Config.掉落赤月物品颜色 == 5)
                    {
                        color2 = "#ffad4a";
                    }
                    else if (Config.掉落赤月物品颜色 == 6)
                    {
                        color2 = "#6bcbd2";
                    }
                    else if (Config.掉落赤月物品颜色 == 7)
                    {
                        color2 = "#ff4abf";
                    }
                    else if (Config.掉落赤月物品颜色 == 8)
                    {
                        color2 = "#fbf1f7";
                    }
                    if (item.装备套装提示 == GameItemSet.赤月装备)
                    {
                        NetworkManager.SendAnnouncement("[" + Name + "] 被 [<font color = '" + color1 + "'>" + hitter.Name + "</font>]击杀, 掉落了[<font color = '" + color2 + "'>" + item.Name + "</font>]");
                    }
                }
                if (Config.CurrentVersion >= 1 && item.装备套装提示 == GameItemSet.魔龙装备)
                {
                    color1 = GetDropUserNameColor();

                    if (Config.掉落魔龙物品颜色 == 0)
                    {
                        color2 = "#f9e01d";
                    }
                    else if (Config.掉落魔龙物品颜色 == 1)
                    {
                        color2 = "#7dea00";
                    }
                    else if (Config.掉落魔龙物品颜色 == 2)
                    {
                        color2 = "#EA0000";
                    }
                    else if (Config.掉落魔龙物品颜色 == 3)
                    {
                        color2 = "#0003ea";
                    }
                    else if (Config.掉落魔龙物品颜色 == 4)
                    {
                        color2 = "#ff4aea";
                    }
                    else if (Config.掉落魔龙物品颜色 == 5)
                    {
                        color2 = "#ffad4a";
                    }
                    else if (Config.掉落魔龙物品颜色 == 6)
                    {
                        color2 = "#6bcbd2";
                    }
                    else if (Config.掉落魔龙物品颜色 == 7)
                    {
                        color2 = "#ff4abf";
                    }
                    else if (Config.掉落魔龙物品颜色 == 8)
                    {
                        color2 = "#fbf1f7";
                    }
                    if (item.装备套装提示 == GameItemSet.魔龙装备)
                    {
                        NetworkManager.SendAnnouncement("[" + Name + "] 被 [<font color = '" + color1 + "'>" + hitter.Name + "</font>]击杀, 掉落了[<font color = '" + color2 + "'>" + item.Name + "</font>]");
                    }
                }
                if (Config.CurrentVersion >= 1 && item.装备套装提示 == GameItemSet.星王装备)
                {
                    color1 = GetDropUserNameColor();

                    if (Config.掉落星王物品颜色 == 0)
                    {
                        color2 = "#f9e01d";
                    }
                    else if (Config.掉落星王物品颜色 == 1)
                    {
                        color2 = "#7dea00";
                    }
                    else if (Config.掉落星王物品颜色 == 2)
                    {
                        color2 = "#EA0000";
                    }
                    else if (Config.掉落星王物品颜色 == 3)
                    {
                        color2 = "#0003ea";
                    }
                    else if (Config.掉落星王物品颜色 == 4)
                    {
                        color2 = "#ff4aea";
                    }
                    else if (Config.掉落星王物品颜色 == 5)
                    {
                        color2 = "#ffad4a";
                    }
                    else if (Config.掉落星王物品颜色 == 6)
                    {
                        color2 = "#6bcbd2";
                    }
                    else if (Config.掉落星王物品颜色 == 7)
                    {
                        color2 = "#ff4abf";
                    }
                    else if (Config.掉落星王物品颜色 == 8)
                    {
                        color2 = "#fbf1f7";
                    }
                    if (item.装备套装提示 == GameItemSet.星王装备)
                    {
                        NetworkManager.SendAnnouncement("[" + Name + "] 被 [<font color = '" + color1 + "'>" + hitter.Name + "</font>]击杀, 掉落了[<font color = '" + color2 + "'>" + item.Name + "</font>]");
                    }
                }
                if (Config.CurrentVersion >= 1 && item.装备套装提示 == GameItemSet.城主装备)
                {
                    color1 = GetDropUserNameColor();

                    if (Config.掉落城主物品颜色 == 0)
                    {
                        color2 = "#f9e01d";
                    }
                    else if (Config.掉落城主物品颜色 == 1)
                    {
                        color2 = "#7dea00";
                    }
                    else if (Config.掉落城主物品颜色 == 2)
                    {
                        color2 = "#EA0000";
                    }
                    else if (Config.掉落城主物品颜色 == 3)
                    {
                        color2 = "#0003ea";
                    }
                    else if (Config.掉落城主物品颜色 == 4)
                    {
                        color2 = "#ff4aea";
                    }
                    else if (Config.掉落城主物品颜色 == 5)
                    {
                        color2 = "#ffad4a";
                    }
                    else if (Config.掉落城主物品颜色 == 6)
                    {
                        color2 = "#6bcbd2";
                    }
                    else if (Config.掉落城主物品颜色 == 7)
                    {
                        color2 = "#ff4abf";
                    }
                    else if (Config.掉落城主物品颜色 == 8)
                    {
                        color2 = "#fbf1f7";
                    }
                    if (item.装备套装提示 == GameItemSet.城主装备)
                    {
                        NetworkManager.SendAnnouncement("[" + Name + "] 被 [<font color = '" + color1 + "'>" + hitter.Name + "</font>]击杀, 掉落了[<font color = '" + color2 + "'>" + item.Name + "</font>]");
                    }
                }
                if (Config.CurrentVersion >= 1 && item.ValuableObjects)
                {
                    color1 = GetDropUserNameColor();

                    if (Config.掉落贵重物品颜色 == 0)
                    {
                        color2 = "#f9e01d";
                    }
                    else if (Config.掉落贵重物品颜色 == 1)
                    {
                        color2 = "#7dea00";
                    }
                    else if (Config.掉落贵重物品颜色 == 2)
                    {
                        color2 = "#EA0000";
                    }
                    else if (Config.掉落贵重物品颜色 == 3)
                    {
                        color2 = "#0003ea";
                    }
                    else if (Config.掉落贵重物品颜色 == 4)
                    {
                        color2 = "#ff4aea";
                    }
                    else if (Config.掉落贵重物品颜色 == 5)
                    {
                        color2 = "#ffad4a";
                    }
                    else if (Config.掉落贵重物品颜色 == 6)
                    {
                        color2 = "#6bcbd2";
                    }
                    else if (Config.掉落贵重物品颜色 == 7)
                    {
                        color2 = "#ff4abf";
                    }
                    else if (Config.掉落贵重物品颜色 == 8)
                    {
                        color2 = "#fbf1f7";
                    }
                    if (item.ValuableObjects)
                    {
                        NetworkManager.SendAnnouncement("[" + Name + "] 被 [<font color = '" + color1 + "'>" + hitter.Name + "</font>]击杀, 掉落了[<font color = '" + color2 + "'>" + item.Name + "</font>]");
                    }
                }
                else
                {
                    NetworkManager.SendAnnouncement("[" + CurrentMap?.ToString() + "]的[" + Name + "] 被 [" + hitter.Name + "] 击杀, 掉落了[" + item.Name + "]");
                }
            }
            if (Config.怪物掉落窗口开关 == 1 && item.ValuableObjects)
            {
                string color1 = null;
                string color2 = null;
                if (Config.CurrentVersion >= 1 && item.装备套装提示 == GameItemSet.沃玛装备)
                {
                    color1 = GetDropUserNameColor();

                    if (Config.掉落沃玛物品颜色 == 0)
                    {
                        color2 = "#f9e01d";
                    }
                    else if (Config.掉落沃玛物品颜色 == 1)
                    {
                        color2 = "#7dea00";
                    }
                    else if (Config.掉落沃玛物品颜色 == 2)
                    {
                        color2 = "#EA0000";
                    }
                    else if (Config.掉落沃玛物品颜色 == 3)
                    {
                        color2 = "#0003ea";
                    }
                    else if (Config.掉落沃玛物品颜色 == 4)
                    {
                        color2 = "#ff4aea";
                    }
                    else if (Config.掉落沃玛物品颜色 == 5)
                    {
                        color2 = "#ffad4a";
                    }
                    else if (Config.掉落沃玛物品颜色 == 6)
                    {
                        color2 = "#6bcbd2";
                    }
                    else if (Config.掉落沃玛物品颜色 == 7)
                    {
                        color2 = "#ff4abf";
                    }
                    else if (Config.掉落沃玛物品颜色 == 8)
                    {
                        color2 = "#fbf1f7";
                    }
                    if (item.装备套装提示 == GameItemSet.沃玛装备)
                    {
                        NetworkManager.SendAnnouncement("[" + Name + "] 被 [<font color = '" + color1 + "'>" + hitter.Name + "</font>]击杀, 掉落了[<font color = '" + color2 + "'>" + item.Name + "</font>]", rolling: true);
                    }
                }
                if (Config.CurrentVersion >= 1 && item.装备套装提示 == GameItemSet.祖玛装备)
                {
                    color1 = GetDropUserNameColor();

                    if (Config.掉落祖玛物品颜色 == 0)
                    {
                        color2 = "#f9e01d";
                    }
                    else if (Config.掉落祖玛物品颜色 == 1)
                    {
                        color2 = "#7dea00";
                    }
                    else if (Config.掉落祖玛物品颜色 == 2)
                    {
                        color2 = "#EA0000";
                    }
                    else if (Config.掉落祖玛物品颜色 == 3)
                    {
                        color2 = "#0003ea";
                    }
                    else if (Config.掉落祖玛物品颜色 == 4)
                    {
                        color2 = "#ff4aea";
                    }
                    else if (Config.掉落祖玛物品颜色 == 5)
                    {
                        color2 = "#ffad4a";
                    }
                    else if (Config.掉落祖玛物品颜色 == 6)
                    {
                        color2 = "#6bcbd2";
                    }
                    else if (Config.掉落祖玛物品颜色 == 7)
                    {
                        color2 = "#ff4abf";
                    }
                    else if (Config.掉落祖玛物品颜色 == 8)
                    {
                        color2 = "#fbf1f7";
                    }
                    if (item.装备套装提示 == GameItemSet.祖玛装备)
                    {
                        NetworkManager.SendAnnouncement("[" + Name + "] 被 [<font color = '" + color1 + "'>" + hitter.Name + "</font>]击杀, 掉落了[<font color = '" + color2 + "'>" + item.Name + "</font>]", rolling: true);
                    }
                }
                if (Config.CurrentVersion >= 1 && item.装备套装提示 == GameItemSet.赤月装备)
                {
                    color1 = GetDropUserNameColor();

                    if (Config.掉落赤月物品颜色 == 0)
                    {
                        color2 = "#f9e01d";
                    }
                    else if (Config.掉落赤月物品颜色 == 1)
                    {
                        color2 = "#7dea00";
                    }
                    else if (Config.掉落赤月物品颜色 == 2)
                    {
                        color2 = "#EA0000";
                    }
                    else if (Config.掉落赤月物品颜色 == 3)
                    {
                        color2 = "#0003ea";
                    }
                    else if (Config.掉落赤月物品颜色 == 4)
                    {
                        color2 = "#ff4aea";
                    }
                    else if (Config.掉落赤月物品颜色 == 5)
                    {
                        color2 = "#ffad4a";
                    }
                    else if (Config.掉落赤月物品颜色 == 6)
                    {
                        color2 = "#6bcbd2";
                    }
                    else if (Config.掉落赤月物品颜色 == 7)
                    {
                        color2 = "#ff4abf";
                    }
                    else if (Config.掉落赤月物品颜色 == 8)
                    {
                        color2 = "#fbf1f7";
                    }
                    if (item.装备套装提示 == GameItemSet.赤月装备)
                    {
                        NetworkManager.SendAnnouncement("[" + Name + "] 被 [<font color = '" + color1 + "'>" + hitter.Name + "</font>]击杀, 掉落了[<font color = '" + color2 + "'>" + item.Name + "</font>]", rolling: true);
                    }
                }
                if (Config.CurrentVersion >= 1 && item.装备套装提示 == GameItemSet.魔龙装备)
                {
                    color1 = GetDropUserNameColor();

                    if (Config.掉落魔龙物品颜色 == 0)
                    {
                        color2 = "#f9e01d";
                    }
                    else if (Config.掉落魔龙物品颜色 == 1)
                    {
                        color2 = "#7dea00";
                    }
                    else if (Config.掉落魔龙物品颜色 == 2)
                    {
                        color2 = "#EA0000";
                    }
                    else if (Config.掉落魔龙物品颜色 == 3)
                    {
                        color2 = "#0003ea";
                    }
                    else if (Config.掉落魔龙物品颜色 == 4)
                    {
                        color2 = "#ff4aea";
                    }
                    else if (Config.掉落魔龙物品颜色 == 5)
                    {
                        color2 = "#ffad4a";
                    }
                    else if (Config.掉落魔龙物品颜色 == 6)
                    {
                        color2 = "#6bcbd2";
                    }
                    else if (Config.掉落魔龙物品颜色 == 7)
                    {
                        color2 = "#ff4abf";
                    }
                    else if (Config.掉落魔龙物品颜色 == 8)
                    {
                        color2 = "#fbf1f7";
                    }
                    if (item.装备套装提示 == GameItemSet.魔龙装备)
                    {
                        NetworkManager.SendAnnouncement("[" + Name + "] 被 [<font color = '" + color1 + "'>" + hitter.Name + "</font>]击杀, 掉落了[<font color = '" + color2 + "'>" + item.Name + "</font>]", rolling: true);
                    }
                }
                if (Config.CurrentVersion >= 1 && item.装备套装提示 == GameItemSet.星王装备)
                {
                    color1 = GetDropUserNameColor();

                    if (Config.掉落星王物品颜色 == 0)
                    {
                        color2 = "#f9e01d";
                    }
                    else if (Config.掉落星王物品颜色 == 1)
                    {
                        color2 = "#7dea00";
                    }
                    else if (Config.掉落星王物品颜色 == 2)
                    {
                        color2 = "#EA0000";
                    }
                    else if (Config.掉落星王物品颜色 == 3)
                    {
                        color2 = "#0003ea";
                    }
                    else if (Config.掉落星王物品颜色 == 4)
                    {
                        color2 = "#ff4aea";
                    }
                    else if (Config.掉落星王物品颜色 == 5)
                    {
                        color2 = "#ffad4a";
                    }
                    else if (Config.掉落星王物品颜色 == 6)
                    {
                        color2 = "#6bcbd2";
                    }
                    else if (Config.掉落星王物品颜色 == 7)
                    {
                        color2 = "#ff4abf";
                    }
                    else if (Config.掉落星王物品颜色 == 8)
                    {
                        color2 = "#fbf1f7";
                    }
                    if (item.装备套装提示 == GameItemSet.星王装备)
                    {
                        NetworkManager.SendAnnouncement("[" + Name + "] 被 [<font color = '" + color1 + "'>" + hitter.Name + "</font>]击杀, 掉落了[<font color = '" + color2 + "'>" + item.Name + "</font>]", rolling: true);
                    }
                }
                if (Config.CurrentVersion >= 1 && item.装备套装提示 == GameItemSet.城主装备)
                {
                    color1 = GetDropUserNameColor();

                    if (Config.掉落城主物品颜色 == 0)
                    {
                        color2 = "#f9e01d";
                    }
                    else if (Config.掉落城主物品颜色 == 1)
                    {
                        color2 = "#7dea00";
                    }
                    else if (Config.掉落城主物品颜色 == 2)
                    {
                        color2 = "#EA0000";
                    }
                    else if (Config.掉落城主物品颜色 == 3)
                    {
                        color2 = "#0003ea";
                    }
                    else if (Config.掉落城主物品颜色 == 4)
                    {
                        color2 = "#ff4aea";
                    }
                    else if (Config.掉落城主物品颜色 == 5)
                    {
                        color2 = "#ffad4a";
                    }
                    else if (Config.掉落城主物品颜色 == 6)
                    {
                        color2 = "#6bcbd2";
                    }
                    else if (Config.掉落城主物品颜色 == 7)
                    {
                        color2 = "#ff4abf";
                    }
                    else if (Config.掉落城主物品颜色 == 8)
                    {
                        color2 = "#fbf1f7";
                    }
                    if (item.装备套装提示 == GameItemSet.城主装备)
                    {
                        NetworkManager.SendAnnouncement("[" + Name + "] 被 [<font color = '" + color1 + "'>" + hitter.Name + "</font>]击杀, 掉落了[<font color = '" + color2 + "'>" + item.Name + "</font>]", rolling: true);
                    }
                }
                if (Config.CurrentVersion >= 1 && item.装备套装提示 == GameItemSet.None)
                {
                    color1 = GetDropUserNameColor();

                    if (Config.掉落贵重物品颜色 == 0)
                    {
                        color2 = "#f9e01d";
                    }
                    else if (Config.掉落贵重物品颜色 == 1)
                    {
                        color2 = "#7dea00";
                    }
                    else if (Config.掉落贵重物品颜色 == 2)
                    {
                        color2 = "#EA0000";
                    }
                    else if (Config.掉落贵重物品颜色 == 3)
                    {
                        color2 = "#0003ea";
                    }
                    else if (Config.掉落贵重物品颜色 == 4)
                    {
                        color2 = "#ff4aea";
                    }
                    else if (Config.掉落贵重物品颜色 == 5)
                    {
                        color2 = "#ffad4a";
                    }
                    else if (Config.掉落贵重物品颜色 == 6)
                    {
                        color2 = "#6bcbd2";
                    }
                    else if (Config.掉落贵重物品颜色 == 7)
                    {
                        color2 = "#ff4abf";
                    }
                    else if (Config.掉落贵重物品颜色 == 8)
                    {
                        color2 = "#fbf1f7";
                    }
                    if (item.ValuableObjects)
                    {
                        NetworkManager.SendAnnouncement("[" + Name + "] 被 [<font color = '" + color1 + "'>" + hitter.Name + "</font>]击杀, 掉落了[<font color = '" + color2 + "'>" + item.Name + "</font>]", rolling: true);
                    }
                }
                else
                {
                    NetworkManager.SendAnnouncement("[" + CurrentMap?.ToString() + "]的[" + Name + "] 被 [" + hitter.Name + "] 击杀, 掉落了[" + item.Name + "]", rolling: true);
                }
            }
        }
        if (Config.CurrentVersion == 0)
        {
            NetworkManager.SendAnnouncement("[" + Name + "] 被 [" + hitter.Name + "] 击杀, 掉落了[" + item.Name + "]");
        }
    }

    public void Resurrect(bool calculate)
    {
        if (CurrentMap.QuestMap || !ForbidResurrection)
        {
            CurrentMap.TotalSurvivingMonsters++;
            SMain.更新地图数据(CurrentMap, "存活怪物总数", 1);
            if (calculate)
            {
                CurrentMap.TotalAmountMonsterResurrected++;
                SMain.更新地图数据(CurrentMap, "怪物复活次数", 1);
            }
        }
        RefreshStats();
        CurrentMap = BirthMap;
        CurrentDirection = Compute.RandomDirection();
        CurrentHP = this[Stat.MaxHP];
        CurrentPosition = BirthRange[SEngine.Random.Next(0, BirthRange.Length)];

        for (var i = 0; i < 100; i++)
        {
            var point = Compute.GetPositionAround(CurrentPosition, i);
            if (!CurrentMap.IsBlocking(point))
            {
                CurrentPosition = point;
                break;
            }
        }

        AttackTime = SEngine.CurrentTime.AddSeconds(1.0);
        RecoveryTime = SEngine.CurrentTime.AddMilliseconds(SEngine.Random.Next(5000));
        RoamTime = SEngine.CurrentTime.AddMilliseconds(SEngine.Random.Next(5000) + RoamInterval);
        
        Target = new HateObject();
        SecondaryObject = false;
        Dead = false;
        CombatStance = false;
        Blocking = true;

        BindGrid();
        UpdateAllNeighbours();

        if (Grade == MonsterGradeType.Boss && Config.BOSS刷新提示开关 == 1)
        {
            NetworkManager.SendAnnouncement($"{Info.MonsterName}在{CurrentMap}刷新！", rolling: true);
        }

        if (!Activated)
        {
            if (Info.OutWarAutomaticPetrochemical)
            {
                AddBuff(Info.PetrochemicalStatusID, this);
            }
            if (ExitCombatSkill != null)
            {
                new SkillObject(this, ExitCombatSkill, null, base.ActionID++, CurrentMap, CurrentPosition, null, CurrentPosition, null).Process();
            }
        }
    }

    public void 怪物诱惑处理()
    {
        Buffs.Clear();
        Disappeared = true;
        Dead = true;
        Blocking = false;

        if (ForbidResurrection)
        {
            Despawn();
            return;
        }

        RemoveAllNeighbors();
        UnbindGrid();
        ResurrectionTime = SEngine.CurrentTime.AddMilliseconds(ResurrectionInterval);
        SecondaryObject = true;
        MapManager.AddSecondaryObject(this);
        Activated = false;
        MapManager.RemoveActiveObject(this);
    }

    public void Deactivate()
    {
        if (Activated)
        {
            Activated = false;
            ActiveSkills.Clear();
            MapManager.RemoveActiveObject(this);
        }
        if (ForbidResurrection && !SecondaryObject)
        {
            SecondaryObject = true;
            ActiveSkills.Clear();
            MapManager.AddSecondaryObject(this);
        }
    }

    public void Activate()
    {
        if (!Activated)
        {
            SecondaryObject = false;
            Activated = true;
            MapManager.AddActiveObject(this);

            int n = (int)Math.Max(0.0, (SEngine.CurrentTime - RecoveryTime).TotalSeconds / 5.0);
            CurrentHP = Math.Min(this[Stat.MaxHP], CurrentHP + n * this[Stat.HealthRecovery]);

            RecoveryTime = RecoveryTime.AddSeconds(5.0);
            AttackTime = SEngine.CurrentTime.AddSeconds(1.0);
            RoamTime = SEngine.CurrentTime.AddMilliseconds(SEngine.Random.Next(5000) + RoamInterval);
            BusyTime = SEngine.CurrentTime.AddMilliseconds(WalkInterval);
            WalkTime = SEngine.CurrentTime.AddMilliseconds(WalkInterval + MoveInterval);
        }
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
        else if (GetDistance(Target.Target) > TargetRange && SEngine.CurrentTime > Target.TargetList[Target.Target].HateTime)
        {
            Target.Remove(Target.Target);
        }
        else if (GetDistance(Target.Target) <= TargetRange)
        {
            Target.TargetList[Target.Target].HateTime = SEngine.CurrentTime.AddMilliseconds(HateTime);
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

    public bool UpdateBestTarget()
    {
        if (Target.TargetList.Count == 0)
        {
            return false;
        }
        if (Target.Target == null)
        {
            Target.SelectTargetTime = default(DateTime);
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
        else if (GetDistance(Target.Target) > TargetRange && SEngine.CurrentTime > Target.TargetList[Target.Target].HateTime)
        {
            Target.Remove(Target.Target);
        }
        else if (GetDistance(Target.Target) <= TargetRange)
        {
            Target.TargetList[Target.Target].HateTime = SEngine.CurrentTime.AddMilliseconds(HateTime);
        }
        if (Target.SelectTargetTime < SEngine.CurrentTime && Target.SelectBestTarget(this))
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

    public bool GetKillOwner(out PlayerObject hitter)
    {
        foreach (var kvp in Target.TargetList.ToList())
        {
            if (kvp.Key is PetObject pet)
            {
                if (kvp.Value.Priority > 0)
                {
                    Target.Add(pet.Master, kvp.Value.HateTime, kvp.Value.Priority);
                }
                Target.Remove(kvp.Key);
            }
            else if (kvp.Key is not PlayerObject)
            {
                Target.Remove(kvp.Key);
            }
        }

        var target = Target.TargetList.Keys
            .OrderBy(x => Target.TargetList[x].Priority)
            .FirstOrDefault();

        hitter = (target is PlayerObject player) ? player : null;
        return hitter != null;
    }
}
