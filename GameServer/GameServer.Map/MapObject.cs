using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

using GameServer.Database;
using GameServer.Template;
using GameServer.Skill;

using GamePackets;
using GamePackets.Server;

namespace GameServer.Map;

public abstract class MapObject
{
    public bool SecondaryObject;
    public bool Activated;

    public HashSet<MapObject> ImportantNeighbors;
    public HashSet<MapObject> StealthNeighbors;
    public HashSet<MapObject> Neighbors;
    public HashSet<SkillObject> ActiveSkills;
    public HashSet<TrapObject> Traps;

    public Dictionary<object, Stats> BonusStats;

    public object 坐骑属性脱落;

    public DateTime RecoveryTime { get; set; }
    public DateTime HealTime { get; set; }
    public DateTime AttackStopTime { get; set; }
    public DateTime CurrentTime { get; set; }
    public DateTime ProcessTime { get; set; }
    public DateTime ReviveTime { get; set; }

    public virtual int ProcessInterval { get; }

    public int HealCount { get; set; }
    public int HealAmount { get; set; }

    public byte ActionID { get; set; }

    public bool CombatStance { get; set; }

    public abstract GameObjectType ObjectType { get; }

    public abstract ObjectSize Size { get; }

    public ushort WalkSpeed => (ushort)this[Stat.WalkSpeed];
    public ushort RunSpeed => (ushort)this[Stat.RunSpeed];

    public virtual int WalkInterval => WalkSpeed * 60;
    public virtual int RunInterval => RunSpeed * 60;

    public virtual int ObjectID { get; set; }

    public virtual int CurrentHP { get; set; }
    public virtual int CurrentMP { get; set; }
    public virtual byte CurrentLevel { get; set; }

    public virtual bool Dead { get; set; }

    public virtual bool Blocking { get; set; }

    public virtual bool CanBeHit => !Dead;

    public virtual string Name { get; set; }

    public virtual GameDirection CurrentDirection { get; set; }

    public virtual Map CurrentMap { get; set; }

    public virtual Point CurrentPosition { get; set; }

    public virtual ushort CurrentHeight => CurrentMap.GetTerrainHeight(CurrentPosition);

    public virtual DateTime BusyTime { get; set; }
    public virtual DateTime HardStunTime { get; set; }
    public virtual DateTime WalkTime { get; set; }
    public virtual DateTime RunTime { get; set; }

    public virtual int this[Stat stat]
    {
        get { return Stats[stat]; }
        set
        {
            Stats[stat] = value;
            switch (stat)
            {
                case Stat.MaxHP:
                    CurrentHP = Math.Min(CurrentHP, value);
                    break;
                case Stat.MaxMP:
                    CurrentMP = Math.Min(CurrentMP, value);
                    break;
            }
        }
    }

    public virtual Stats Stats { get; }

    public virtual DictionaryMonitor<int, DateTime> Cooldowns { get; }

    public virtual DictionaryMonitor<ushort, BuffInfo> Buffs { get; }

    public virtual bool CanAutoRun()
    {
        return !Dead && !(SEngine.CurrentTime <= BusyTime) && 
            !(SEngine.CurrentTime <= RunTime) && 
            !CheckStatus(GameObjectState.BusyGreen | GameObjectState.Disabled | GameObjectState.Immobilized | GameObjectState.Paralyzed | GameObjectState.Unconscious);
    }

    public override string ToString() => Name;

    public virtual void RefreshStats()
    {
        int num = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        foreach (var stat in Enum.GetValues<Stat>())
        {
            int num5 = 0;
            foreach (var kvp in BonusStats)
            {
                if (kvp.Value == null) continue;

                var value = kvp.Value[stat];
                if (value == 0) continue;

                if (kvp.Key is BuffInfo)
                {
                    switch (stat)
                    {
                        case Stat.WalkSpeed:
                            num2 = Math.Max(num2, value);
                            num = Math.Min(num, value);
                            break;
                        case Stat.RunSpeed:
                            num4 = Math.Max(num4, value);
                            num3 = Math.Min(num3, value);
                            break;
                        default:
                            num5 += value;
                            break;
                    }
                }
                else
                {
                    num5 += value;
                }
            }
            switch (stat)
            {
                case Stat.WalkSpeed:
                    this[stat] = Math.Max(1, num5 + num + num2);
                    continue;
                case Stat.RunSpeed:
                    this[stat] = Math.Max(1, num5 + num3 + num4);
                    continue;
                case Stat.Luck:
                    this[stat] = num5;
                    continue;
            }
            this[stat] = Math.Max(0, num5);

            if (this is PlayerObject player)
            {
                if (stat == Stat.物理击回)
                    player.Character.当前角色物理回击.V = num5;

                if (stat == Stat.魔法击回)
                    player.Character.当前角色魔法回击.V = num5;
            }
        }

        {
            if (this is not PlayerObject player)
                return;

            foreach (PetObject pet in player.Pets)
            {
                if (pet.MInfo.InheritedStats != null)
                {
                    Stats stats = new Stats();
                    foreach (var stat in pet.MInfo.InheritedStats)
                    {
                        stats[stat.ConvertStat] = (int)((float)this[stat.InheritedStat] * stat.Ratio);
                    }
                    pet.BonusStats[player.Character] = stats;
                    pet.RefreshStats();
                }
            }
        }
    }

    public virtual void Process()
    {
        CurrentTime = SEngine.CurrentTime;
        ProcessTime = SEngine.CurrentTime.AddMilliseconds(ProcessInterval);
    }

    public virtual void Die(MapObject attacker, bool skillDeath)
    {
        if (Buffs.TryGetValue(2555, out var v))
        {
            RemoveBuffEx(v.ID.V);
        }
        SendPacket(new ObjectDiePacket
        {
            ObjectID = ObjectID
        });
        ActiveSkills.Clear();
        Dead = true;
        Blocking = false;
        foreach (var obj in Neighbors)
            obj.OnDeath(this);
    }

    public MapObject()
    {
        CurrentTime = SEngine.CurrentTime;
        ActiveSkills = new HashSet<SkillObject>();
        Traps = new HashSet<TrapObject>();
        ImportantNeighbors = new HashSet<MapObject>();
        StealthNeighbors = new HashSet<MapObject>();
        Neighbors = new HashSet<MapObject>();
        Stats = new Stats();
        Cooldowns = new DictionaryMonitor<int, DateTime>(null);
        Buffs = new DictionaryMonitor<ushort, BuffInfo>(null);
        BonusStats = new Dictionary<object, Stats>();
        ProcessTime = SEngine.CurrentTime.AddMilliseconds(SEngine.Random.Next(ProcessInterval));
    }

    public void UnbindGrid()
    {
        var grid = Compute.CalculateGrid(CurrentPosition, CurrentDirection, Size);
        foreach (var point in grid)
            CurrentMap[point].Remove(this);
    }

    public void BindGrid()
    {
        var grid = Compute.CalculateGrid(CurrentPosition, CurrentDirection, Size);
        foreach (var point in grid)
            CurrentMap[point].Add(this);
    }

    public void Despawn()
    {
        RemoveAllNeighbors();
        UnbindGrid();
        SecondaryObject = false;
        MapManager.RemoveObject(this);
        Activated = false;
        MapManager.RemoveActiveObject(this);
    }

    public int GetDistance(Point location) => Compute.GetDistance(CurrentPosition, location);

    public int GetDistance(MapObject obj) => Compute.GetDistance(CurrentPosition, obj.CurrentPosition);

    public void SendPacket(GamePacket packet)
    {
        if (packet.Info.Broadcast)
            BroadcastPacket(packet);

        if (this is PlayerObject player)
            player.Enqueue(packet);
    }

    private void BroadcastPacket(GamePacket packet)
    {
        foreach (MapObject obj in Neighbors)
        {
            if (obj is not PlayerObject player) continue;

            if (!player.StealthNeighbors.Contains(this))
                player.Enqueue(packet);
        }
    }

    public bool CanBeSeenBy(MapObject obj)
    {
        if (Math.Abs(CurrentPosition.X - obj.CurrentPosition.X) <= 20 && Math.Abs(CurrentPosition.Y - obj.CurrentPosition.Y) <= 20)
        {
            return true;
        }
        return false;
    }

    public bool CanAttack(MapObject target)
    {
        if (target.Dead)
            return false;

        if (this is MonsterObject monster)
        {
            if (monster.ActiveAttackTarget && (target is PlayerObject || target is PetObject || (target is GuardObject 守卫实例2 && 守卫实例2.CanBeInjured)))
            {
                return true;
            }
        }
        else if (this is GuardObject guard)
        {
            if (!guard.ActiveAttackTarget)
                return false;

            if (target is MonsterObject 怪物实例3)
                return 怪物实例3.ActiveAttackTarget;

            if (target is PlayerObject 玩家实例2)
                return 玩家实例2.RedName;

            if (target is PetObject)
                return guard.GuardID == 6734;
        }
        else if (this is PetObject)
        {
            return (target as MonsterObject)?.ActiveAttackTarget ?? false;
        }
        return false;
    }

    public bool IsNeightbor(MapObject obj)
    {
        switch (ObjectType)
        {
            case GameObjectType.NPC:
                {
                    GameObjectType type = obj.ObjectType;
                    if ((uint)(type - 1) > 1u && type != GameObjectType.Monster && type != GameObjectType.Trap)
                    {
                        return false;
                    }
                    return true;
                }
            case GameObjectType.Player:
                return true;
            case GameObjectType.Pet:
            case GameObjectType.Monster:
                switch (obj.ObjectType)
                {
                    default:
                        return false;
                    case GameObjectType.Player:
                    case GameObjectType.Pet:
                    case GameObjectType.Monster:
                    case GameObjectType.NPC:
                    case GameObjectType.Trap:
                        return true;
                }
            
            case GameObjectType.Trap:
                {
                    GameObjectType type = obj.ObjectType;
                    if ((uint)(type - 1) > 1u && type != GameObjectType.Monster && type != GameObjectType.NPC)
                    {
                        return false;
                    }
                    return true;
                }
            case GameObjectType.Item:
                if (obj.ObjectType == GameObjectType.Player)
                {
                    return true;
                }
                return false;

            default:
                return false;
        }
    }

    public GameObjectRelationship GetRelationship(MapObject target)
    {
        if (target is TrapObject targTrap)
            target = targTrap.Caster;

        if (this == target)
            return GameObjectRelationship.Myself;

        if (this is MonsterObject)
        {
            if (!(target is MonsterObject))
                return GameObjectRelationship.Hostile;

            return GameObjectRelationship.Friendly;
        }
        else if (this is GuardObject)
        {
            if (target is MonsterObject || target is PetObject || target is PlayerObject)
                return GameObjectRelationship.Hostile;
        }
        else if (this is PlayerObject myself)
        {
            if (target is MonsterObject)
                return GameObjectRelationship.Hostile;

            if (target is GuardObject)
            {
                if (myself.AttackMode == AttackMode.全体 && CurrentMap.MapID != 80)
                {
                    return GameObjectRelationship.Hostile;
                }
                return GameObjectRelationship.Friendly;
            }

            if (target is PlayerObject player)
            {
                if (myself.AttackMode == AttackMode.Peace)
                {
                    return GameObjectRelationship.Friendly;
                }
                if (myself.AttackMode == AttackMode.Guild)
                {
                    if (myself.Guild != null && player.Guild != null && (myself.Guild == player.Guild || myself.Guild.AllianceGuilds.ContainsKey(player.Guild)))
                    {
                        return GameObjectRelationship.Friendly;
                    }
                    return GameObjectRelationship.Hostile;
                }
                if (myself.AttackMode == AttackMode.Group)
                {
                    if (myself.Team != null && player.Team != null && myself.Team == player.Team)
                    {
                        return GameObjectRelationship.Friendly;
                    }
                    return GameObjectRelationship.Hostile;
                }
                if (myself.AttackMode == AttackMode.全体)
                {
                    return GameObjectRelationship.Hostile;
                }
                if (myself.AttackMode == AttackMode.GoodAndEvil)
                {
                    if (!player.RedName && !player.GreyName)
                    {
                        return GameObjectRelationship.Friendly;
                    }
                    return GameObjectRelationship.Hostile;
                }
                if (myself.AttackMode == AttackMode.Hostile)
                {
                    if (myself.Guild != null && player.Guild != null && myself.Guild.HostileGuilds.ContainsKey(player.Guild))
                    {
                        return GameObjectRelationship.Hostile;
                    }
                    return GameObjectRelationship.Friendly;
                }
            }
            else if (target is PetObject pet)
            {
                if (pet.Master == myself)
                {
                    if (myself.AttackMode != AttackMode.全体)
                    {
                        return GameObjectRelationship.Friendly;
                    }
                    return GameObjectRelationship.Friendly | GameObjectRelationship.Hostile;
                }
                if (myself.AttackMode == AttackMode.Peace)
                {
                    return GameObjectRelationship.Friendly;
                }
                if (myself.AttackMode == AttackMode.Guild)
                {
                    if (myself.Guild != null && pet.Master.Guild != null && (myself.Guild == pet.Master.Guild || myself.Guild.AllianceGuilds.ContainsKey(pet.Master.Guild)))
                    {
                        return GameObjectRelationship.Friendly;
                    }
                    return GameObjectRelationship.Hostile;
                }
                if (myself.AttackMode == AttackMode.Group)
                {
                    if (myself.Team != null && pet.Master.Team != null && myself.Team == pet.Master.Team)
                    {
                        return GameObjectRelationship.Friendly;
                    }
                    return GameObjectRelationship.Hostile;
                }
                if (myself.AttackMode == AttackMode.全体)
                {
                    return GameObjectRelationship.Hostile;
                }
                if (myself.AttackMode == AttackMode.GoodAndEvil)
                {
                    if (!pet.Master.RedName && !pet.Master.GreyName)
                    {
                        return GameObjectRelationship.Friendly;
                    }
                    return GameObjectRelationship.Hostile;
                }
                if (myself.AttackMode == AttackMode.Hostile)
                {
                    if (myself.Guild != null && pet.Master.Guild != null && myself.Guild.HostileGuilds.ContainsKey(pet.Master.Guild))
                    {
                        return GameObjectRelationship.Hostile;
                    }
                    return GameObjectRelationship.Friendly;
                }
            }
        }
        else if (this is PetObject pet)
        {
            if (pet.Master != target)
                return pet.Master.GetRelationship(target);
            return GameObjectRelationship.Friendly;
        }
        else if (this is TrapObject trap)
        {
            return trap.Caster.GetRelationship(target);
        }

        return GameObjectRelationship.Myself;
    }

    public bool IsValidTarget(MapObject attacker, SpecifyTargetType type)
    {
        bool IsValidBackstab()
        {
            GameDirection dir = Compute.DirectionFromPoint(attacker.CurrentPosition, CurrentPosition);
            switch (CurrentDirection)
            {
                case GameDirection.Up:
                    if (dir == GameDirection.UpLeft || dir == GameDirection.Up || dir == GameDirection.UpRight)
                        return true;
                    break;
                case GameDirection.UpLeft:
                    if (dir == GameDirection.Left || dir == GameDirection.UpLeft || dir == GameDirection.Up)
                        return true;
                    break;
                case GameDirection.Left:
                    if (dir == GameDirection.Left || dir == GameDirection.UpLeft || dir == GameDirection.DownLeft)
                        return true;
                    break;
                case GameDirection.Right:
                    if (dir == GameDirection.UpRight || dir == GameDirection.Right || dir == GameDirection.DownRight)
                        return true;
                    break;
                case GameDirection.UpRight:
                    if (dir == GameDirection.Up || dir == GameDirection.UpRight || dir == GameDirection.Right)
                        return true;
                    break;
                default:
                    if (dir == GameDirection.Right || dir == GameDirection.DownRight || dir == GameDirection.Down)
                        return true;
                    break;
                case GameDirection.DownLeft:
                    if (dir == GameDirection.Left || dir == GameDirection.Down || dir == GameDirection.DownLeft)
                        return true;
                    break;
                case GameDirection.Down:
                    if (dir == GameDirection.DownRight || dir == GameDirection.Down || dir == GameDirection.DownLeft)
                        return true;
                    break;
            }

            return false;
        }

        if (attacker is TrapObject trap)
            attacker = trap;

        if (this is MonsterObject monster)
        {
            if (type == SpecifyTargetType.None)
                return true;

            if ((type & SpecifyTargetType.LowLevelTarget) == SpecifyTargetType.LowLevelTarget && CurrentLevel < attacker.CurrentLevel)
                return true;

            if ((type & SpecifyTargetType.AllMonsters) == SpecifyTargetType.AllMonsters)
                return true;

            if ((type & SpecifyTargetType.LowLevelMonster) == SpecifyTargetType.LowLevelMonster && CurrentLevel < attacker.CurrentLevel)
                return true;
 
            if ((type & SpecifyTargetType.LowHealthMonster) == SpecifyTargetType.LowHealthMonster && (float)CurrentHP / (float)this[Stat.MaxHP] < 0.4f)
                return true;

            if ((type & SpecifyTargetType.Normal) == SpecifyTargetType.Normal && monster.Grade == MonsterGradeType.Normal)
                return true;

            if ((type & SpecifyTargetType.Undead) == SpecifyTargetType.Undead && monster.Race == MonsterRaceType.Undead)
                return true;

            if ((type & SpecifyTargetType.ZergCreature) == SpecifyTargetType.ZergCreature && monster.Race == MonsterRaceType.ZergCreature)
                return true;

            if ((type & SpecifyTargetType.WomaMonster) == SpecifyTargetType.WomaMonster && monster.Race == MonsterRaceType.WomaMonster)
                return true;

            if ((type & SpecifyTargetType.PigMonster) == SpecifyTargetType.PigMonster && monster.Race == MonsterRaceType.PigMonster)
                return true;

            if ((type & SpecifyTargetType.ZumaMonster) == SpecifyTargetType.ZumaMonster && monster.Race == MonsterRaceType.ZumaMonster)
                return true;

            if ((type & SpecifyTargetType.DragonMonster) == SpecifyTargetType.DragonMonster && monster.Race == MonsterRaceType.DragonMonster)
                return true;

            if ((type & SpecifyTargetType.EliteMonsters) == SpecifyTargetType.EliteMonsters && (monster.Grade == MonsterGradeType.Elite || monster.Grade == MonsterGradeType.Boss))
                return true;

            if ((type & SpecifyTargetType.Backstab) == SpecifyTargetType.Backstab)
            {
                if (IsValidBackstab())
                    return true;
            }
        }
        else if (this is GuardObject)
        {
            if (type == SpecifyTargetType.None)
                return true;

            if ((type & SpecifyTargetType.LowLevelTarget) == SpecifyTargetType.LowLevelTarget && CurrentLevel < attacker.CurrentLevel)
                return true;

            if ((type & SpecifyTargetType.Backstab) == SpecifyTargetType.Backstab)
            {
                if (IsValidBackstab())
                    return true;
            }
        }
        else if (this is PetObject pet)
        {
            if (type == SpecifyTargetType.None)
                return true;

            if ((type & SpecifyTargetType.LowLevelTarget) == SpecifyTargetType.LowLevelTarget && CurrentLevel < attacker.CurrentLevel)
                return true;

            if ((type & SpecifyTargetType.Undead) == SpecifyTargetType.Undead && pet.PetRace == MonsterRaceType.Undead)
                return true;

            if ((type & SpecifyTargetType.ZergCreature) == SpecifyTargetType.ZergCreature && pet.PetRace == MonsterRaceType.ZergCreature)
                return true;

            if ((type & SpecifyTargetType.AllPets) == SpecifyTargetType.AllPets)
                return true;

            if ((type & SpecifyTargetType.Backstab) == SpecifyTargetType.Backstab)
            {
                if (IsValidBackstab())
                    return true;
            }
        }
        else if (this is PlayerObject player)
        {
            if (type == SpecifyTargetType.None)
                return true;

            if ((type & SpecifyTargetType.LowLevelTarget) == SpecifyTargetType.LowLevelTarget && CurrentLevel < attacker.CurrentLevel)
                return true;

            if ((type & SpecifyTargetType.ShieldMage) == SpecifyTargetType.ShieldMage && player.Job == GameObjectRace.Wizard && player.Buffs.ContainsKey(25350))
                return true;

            if ((type & SpecifyTargetType.Backstab) == SpecifyTargetType.Backstab)
            {
                if (IsValidBackstab())
                    return true;
            }

            if ((type & SpecifyTargetType.AllPlayers) == SpecifyTargetType.AllPlayers)
                return true;
        }
        return false;
    }

    public virtual bool CanWalk()
    {
        if (Dead) return false;
        if (SEngine.CurrentTime < BusyTime) return false;
        if (SEngine.CurrentTime < WalkTime) return false;

        if (CheckStatus(GameObjectState.BusyGreen | GameObjectState.Immobilized | GameObjectState.Paralyzed | GameObjectState.Unconscious))
            return false;
        
        return true;
    }

    public virtual bool CanRun()
    {
        if (Dead) return false;
        if (SEngine.CurrentTime < BusyTime) return false;
        if (SEngine.CurrentTime < RunTime) return false;
 
        if (CheckStatus(GameObjectState.BusyGreen | GameObjectState.Disabled | GameObjectState.Immobilized | GameObjectState.Paralyzed | GameObjectState.Unconscious))
            return false;

        return true;
    }

    public virtual bool CanTurn()
    {
        if (Dead) return false;
        if (SEngine.CurrentTime < BusyTime) return false;
        if (SEngine.CurrentTime < WalkTime) return false;

        if (CheckStatus(GameObjectState.BusyGreen | GameObjectState.Paralyzed | GameObjectState.Unconscious))
            return false;

        return true;
    }

    public virtual bool CanPush(MapObject target)
    {
        if (this == target) return true;
        if (this is GuardObject) return false;
        if (CurrentLevel >= target.CurrentLevel) return false;
 
        if (this is MonsterObject monster && !monster.CanBePushedBySkill) return false;
        if (target.GetRelationship(this) != GameObjectRelationship.Hostile) return false;

        return true;
    }

    public virtual bool CanRush(MapObject target, Point location, int distance, int quantity, bool passWalls, out Point destination, out List<MapObject> targets)
    {
        destination = CurrentPosition;
        targets = null;
        if (!(CurrentPosition == location) && CanPush(target))
        {
            List<MapObject> list = new List<MapObject>();
            for (int i = 1; i <= distance; i++)
            {
                if (passWalls)
                {
                    Point point = Compute.GetFrontPosition(CurrentPosition, location, i);
                    if (CurrentMap.CanMove(point))
                    {
                        destination = point;
                    }
                    continue;
                }
                GameDirection dir = Compute.DirectionFromPoint(CurrentPosition, location);
                Point point2 = Compute.GetFrontPosition(CurrentPosition, location, i);
                if (!CurrentMap.ValidTerrain(point2))
                    break;

                bool flag = false;
                if (CurrentMap.IsBlocking(point2))
                {
                    foreach (MapObject obj in CurrentMap[point2].Where((MapObject O) => O.Blocking))
                    {
                        if (list.Count >= quantity)
                        {
                            flag = true;
                            break;
                        }
                        if (!obj.CanRush(target, Compute.GetNextPosition(obj.CurrentPosition, dir, 1), 1, quantity - list.Count - 1, passWalls: false, out var _, out var l))
                        {
                            flag = true;
                            break;
                        }
                        list.Add(obj);
                        list.AddRange(l);
                    }
                }
                if (flag)
                {
                    break;
                }
                destination = point2;
            }
            targets = list;
            return destination != CurrentPosition;
        }
        return false;
    }

    public virtual bool CheckStatus(GameObjectState status)
    {
        foreach (var value in Buffs.Values)
        {
            if ((value.BuffEffect & BuffEffectType.StatusFlag) != 0 && (value.Template.PlayerState & status) != 0)
            {
                return true;
            }
        }
        return false;
    }

    public void AddBuff(ushort id, MapObject target)
    {
        if (this is ItemObject || this is TrapObject || (this is GuardObject guard && !guard.CanBeInjured))
            return;

        if (target is TrapObject trap)
            target = trap.Caster;

        if (!GameBuff.DataSheet.TryGetValue(id, out var value))
            return;

        if ((value.Effect & BuffEffectType.StatusFlag) != 0)
        {
            if (((value.PlayerState & GameObjectState.Invisible) != 0 || (value.PlayerState & GameObjectState.Stealth) != 0) && CheckStatus(GameObjectState.Exposed))
            {
                return;
            }
            if ((value.PlayerState & GameObjectState.Exposed) != 0)
            {
                foreach (BuffInfo buff in Buffs.Values)
                {
                    if ((buff.Template.PlayerState & GameObjectState.Invisible) != 0 || (buff.Template.PlayerState & GameObjectState.Stealth) != 0)
                    {
                        RemoveBuffEx(buff.ID.V);
                    }
                }
            }
        }
        if ((value.Effect & BuffEffectType.DealDamage) != 0 && value.DamageType == SkillDamageType.Burn && Buffs.ContainsKey(25352))
        {
            return;
        }
        ushort 分组编号 = ((value.GroupID != 0) ? value.GroupID : value.ID);
        BuffInfo buff数据 = null;
        switch (value.StackingType)
        {
            case BuffStackType.Disabled:
                if (Buffs.Values.FirstOrDefault((BuffInfo O) => O.Buff分组 == 分组编号) == null)
                {
                    buff数据 = (Buffs[value.ID] = new BuffInfo(target, this, value.ID));
                }
                break;
            case BuffStackType.Substitute:
                {
                    foreach (BuffInfo item2 in Buffs.Values.Where((BuffInfo O) => O.Buff分组 == 分组编号).ToList())
                    {
                        RemoveBuffEx(item2.ID.V);
                    }
                    buff数据 = (Buffs[value.ID] = new BuffInfo(target, this, value.ID));
                    break;
                }
            case BuffStackType.StackStat:
                {
                    if (Buffs.TryGetValue(id, out var v2))
                    {
                        v2.当前层数.V = Math.Min((byte)(v2.当前层数.V + 1), v2.最大层数);
                        if (value.AllowsSynthesis && v2.当前层数.V >= value.BuffCraftingStacks && GameBuff.DataSheet.TryGetValue(value.BuffCraftingID, out var _))
                        {
                            RemoveBuffEx(v2.ID.V);
                            AddBuff(value.BuffCraftingID, target);
                            break;
                        }
                        v2.RemainingTime.V = v2.Duration.V;
                        if (v2.SyncClient)
                        {
                            SendPacket(new 对象状态变动
                            {
                                对象编号 = ObjectID,
                                Buff编号 = v2.ID.V,
                                Buff索引 = v2.ID.V,
                                当前层数 = v2.当前层数.V,
                                剩余时间 = (int)v2.RemainingTime.V.TotalMilliseconds,
                                持续时间 = (int)v2.Duration.V.TotalMilliseconds
                            });
                        }
                    }
                    else
                    {
                        buff数据 = (Buffs[value.ID] = new BuffInfo(target, this, value.ID));
                    }
                    break;
                }
            case BuffStackType.StackDuration:
                {
                    if (Buffs.TryGetValue(id, out var v))
                    {
                        v.RemainingTime.V += v.Duration.V;
                        if (v.SyncClient)
                        {
                            SendPacket(new 对象状态变动
                            {
                                对象编号 = ObjectID,
                                Buff编号 = v.ID.V,
                                Buff索引 = v.ID.V,
                                当前层数 = v.当前层数.V,
                                剩余时间 = (int)v.RemainingTime.V.TotalMilliseconds,
                                持续时间 = (int)v.Duration.V.TotalMilliseconds
                            });
                        }
                    }
                    else
                    {
                        buff数据 = (Buffs[value.ID] = new BuffInfo(target, this, value.ID));
                    }
                    break;
                }
        }
        if (buff数据 == null)
        {
            return;
        }
        if (buff数据.SyncClient)
        {
            SendPacket(new 对象添加状态
            {
                对象编号 = ObjectID,
                Buff来源 = target.ObjectID,
                Buff编号 = buff数据.ID.V,
                Buff索引 = buff数据.ID.V,
                Buff层数 = buff数据.当前层数.V,
                持续时间 = (int)buff数据.Duration.V.TotalMilliseconds
            });
        }
        if ((value.Effect & BuffEffectType.StatIncOrDec) != 0)
        {
            BonusStats.Add(buff数据, buff数据.BonusStats);
            RefreshStats();
        }
        if ((value.Effect & BuffEffectType.StatusFlag) != 0)
        {
            if ((value.PlayerState & GameObjectState.Invisible) != 0)
            {
                foreach (MapObject obj in Neighbors)
                {
                    obj.OnInvisible(this);
                }
            }
            if ((value.PlayerState & GameObjectState.Stealth) != 0)
            {
                foreach (MapObject obj in Neighbors)
                {
                    obj.OnSneaking(this);
                }
            }
        }
        if (this is PlayerObject player && value.ID == 2555 && GameMount.DataSheet.TryGetValue(player.CurrentMount, out var mount))
        {
            if (mount.BuffID != 0)
            {
                AddBuff(mount.BuffID, target);
            }
            坐骑属性脱落 = mount;
            BonusStats.Add(mount, mount.Stats);
            RefreshStats();
            
            foreach (MapObject obj in Neighbors)
            {
                if (obj is PlayerObject neighbour)
                {
                    neighbour.Enqueue(new SyncObjectMountPacket
                    {
                        ObjectID = player.ObjectID
                    });
                }
            }
        }
        if (value.AssociatedID != 0)
        {
            AddBuff(value.AssociatedID, target);
        }
    }

    public void RemoveBuffEx(ushort id)
    {
        if (!Buffs.TryGetValue(id, out var v))
            return;

        if (v.Template.FollowedByID != 0 && v.Caster != null && MapManager.Objects.TryGetValue(v.Caster.ObjectID, out var value) && value == v.Caster)
        {
            AddBuff(v.Template.FollowedByID, v.Caster);
        }

        if (v.RequiredBuffs != null)
        {
            foreach (ushort r in v.RequiredBuffs)
                RemoveBuff(r);
        }

        if (v.添加冷却 && v.绑定技能 != 0 && v.CooldownTime != 0 && this is PlayerObject 玩家实例2 && 玩家实例2.Skills.ContainsKey(v.绑定技能))
        {
            DateTime dateTime = SEngine.CurrentTime.AddMilliseconds((int)v.CooldownTime);
            DateTime dateTime2 = (Cooldowns.ContainsKey(v.绑定技能 | 0x1000000) ? Cooldowns[v.绑定技能 | 0x1000000] : default(DateTime));
            if (dateTime > dateTime2)
            {
                Cooldowns[v.绑定技能 | 0x1000000] = dateTime;
                SendPacket(new 添加技能冷却
                {
                    冷却编号 = (v.绑定技能 | 0x1000000),
                    冷却时间 = v.CooldownTime
                });
            }
        }
        Buffs.Remove(id);
        v.Remove();
        if (v.SyncClient)
        {
            SendPacket(new ObjectRemoveBuffPacket
            {
                ObjectID = ObjectID,
                BuffID = id
            });
        }
        if ((v.BuffEffect & BuffEffectType.StatIncOrDec) != 0)
        {
            BonusStats.Remove(v);
            RefreshStats();
        }
        if (v.Template.ID == 2555 && this is PlayerObject 玩家实例3 && GameMount.DataSheet.TryGetValue(玩家实例3.CurrentMount, out var mount))
        {
            坐骑属性脱落 = mount;
            BonusStats.Remove(坐骑属性脱落);
            RefreshStats();
            foreach (var obj in Neighbors)
            {
                if (obj is PlayerObject neighbour)
                {
                    neighbour.Enqueue(new SyncObjectMountPacket
                    {
                        ObjectID = 玩家实例3.ObjectID
                    });
                }
            }
        }

        if ((v.BuffEffect & BuffEffectType.StatusFlag) == 0)
            return;

        if ((v.Template.PlayerState & GameObjectState.Invisible) != 0)
        {
            foreach (var obj in Neighbors)
            {
                obj.ProcessStealthTarget(this);
            }
        }

        if ((v.Template.PlayerState & GameObjectState.Stealth) == 0)
            return;

        foreach (var obj in Neighbors)
            obj.ProcessVisibleTarget(this);
    }

    public void RemoveBuff(ushort id)
    {
        if (!Buffs.TryGetValue(id, out var v))
            return;
 
        if (v.RequiredBuffs != null)
        {
            foreach (ushort r in v.RequiredBuffs)
                RemoveBuff(r);
        }

        Buffs.Remove(id);
        v.Remove();
        if (v.SyncClient)
        {
            SendPacket(new ObjectRemoveBuffPacket
            {
                ObjectID = ObjectID,
                BuffID = id
            });
        }

        if ((v.BuffEffect & BuffEffectType.StatIncOrDec) != 0)
        {
            BonusStats.Remove(v);
            RefreshStats();
        }

        if ((v.BuffEffect & BuffEffectType.StatusFlag) == 0)
            return;

        if ((v.Template.PlayerState & GameObjectState.Invisible) != 0)
        {
            foreach (var obj in Neighbors)
                obj.ProcessStealthTarget(this);
        }

        if ((v.Template.PlayerState & GameObjectState.Stealth) == 0)
            return;

        foreach (MapObject obj in Neighbors)
            obj.ProcessVisibleTarget(this);
    }

    public void ProcessBuffs(BuffInfo buff)
    {
        if (buff.到期消失 && (buff.RemainingTime.V -= SEngine.CurrentTime - CurrentTime) < TimeSpan.Zero)
        {
            RemoveBuffEx(buff.ID.V);
        }
        else if ((buff.ProcessTime.V -= SEngine.CurrentTime - CurrentTime) < TimeSpan.Zero)
        {
            buff.ProcessTime.V += TimeSpan.FromMilliseconds(buff.ProcessInterval);
            if ((buff.BuffEffect & BuffEffectType.DealDamage) != 0)
            {
                ProcessBuffDamage(buff);
            }
            if ((buff.BuffEffect & BuffEffectType.HealthRecovery) != 0)
            {
                ProcessBuffHealthRecovery(buff);
            }
        }
    }

    public void 被技能命中处理(SkillObject skill, C_01_CalculateHitTarget 参数)
    {
        MapObject caster = ((skill.Caster is TrapObject trap) ? trap.Caster : skill.Caster);
        if (skill.HitList.ContainsKey(ObjectID) || !CanBeHit || (this != caster && !Neighbors.Contains(caster)) || skill.HitList.Count >= 参数.HitsLimit || (参数.LimitTargetRelationship & caster.GetRelationship(this)) == 0 || (参数.LimitTargetType & ObjectType) == 0 || !IsValidTarget(skill.Caster, 参数.LimitSpecificType) || ((参数.LimitTargetRelationship & GameObjectRelationship.Hostile) != 0 && (CheckStatus(GameObjectState.Invincible) || ((this is PlayerObject || this is PetObject) && (caster is PlayerObject || caster is PetObject) && (CurrentMap.IsSafeArea(CurrentPosition) || caster.CurrentMap.IsSafeArea(caster.CurrentPosition))) || (caster is MonsterObject && CurrentMap.IsSafeArea(CurrentPosition)))) || (this is MonsterObject 怪物实例2 && (怪物实例2.MonID == 8618 || 怪物实例2.MonID == 8621) && ((caster is PlayerObject 玩家实例2 && 玩家实例2.Guild != null && 玩家实例2.Guild == SystemInfo.Info.OccupyGuild.V) || (caster is PetObject 宠物实例2 && 宠物实例2.Master != null && 宠物实例2.Master.Guild != null && 宠物实例2.Master.Guild == SystemInfo.Info.OccupyGuild.V))) || (CurrentLevel <= Config.NoobProtectionLevel && ObjectType == GameObjectType.Player && (CurrentMap.MapID == Config.新手地图保护1 || CurrentMap.MapID == Config.新手地图保护2 || CurrentMap.MapID == Config.新手地图保护3 || CurrentMap.MapID == Config.新手地图保护4 || CurrentMap.MapID == Config.新手地图保护5 || CurrentMap.MapID == Config.新手地图保护6 || CurrentMap.MapID == Config.新手地图保护7 || CurrentMap.MapID == Config.新手地图保护8 || CurrentMap.MapID == Config.新手地图保护9 || CurrentMap.MapID == Config.新手地图保护10)))
        {
            return;
        }
        int accuracy = 0;
        float hit = 0f;
        int agility = 0;
        float evade = 0f;
        switch (参数.SkillEvasion)
        {
            case SkillEvasionType.SkillCannotBeEvaded:
                accuracy = 1;
                break;
            case SkillEvasionType.CanBePhsyicallyEvaded:
                agility = this[Stat.PhysicalAgility];
                accuracy = caster[Stat.PhysicalAccuracy];
                if (this is MonsterObject)
                {
                    hit += (float)caster[Stat.怪物命中] / 10000f;
                }
                if (caster is MonsterObject)
                {
                    evade += (float)this[Stat.怪物闪避] / 10000f;
                }
                break;
            case SkillEvasionType.CanBeMagicEvaded:
                evade = (float)this[Stat.MagicEvade] / 10000f;
                if (this is MonsterObject)
                {
                    hit += (float)caster[Stat.怪物命中] / 10000f;
                }
                if (caster is MonsterObject)
                {
                    evade += (float)this[Stat.怪物闪避] / 10000f;
                }
                break;
            case SkillEvasionType.CanBePoisonEvaded:
                evade = (float)this[Stat.PoisonEvade] / 10000f;
                break;
            case SkillEvasionType.NonMonstersCanEvade:
                if (this is MonsterObject)
                {
                    accuracy = 1;
                    break;
                }
                agility = this[Stat.PhysicalAgility];
                accuracy = caster[Stat.PhysicalAccuracy];
                break;
        }
        HitInfo value = new HitInfo(this)
        {
            SkillFeedback = (Compute.CalculateHit(accuracy, agility, hit, evade) ? 参数.SkillHitFeedback : SkillHitFeedback.Miss)
        };
        skill.HitList.Add(ObjectID, value);
        int num5 = SEngine.Random.Next(100);
        if (Buffs.TryGetValue(2555, out var v) && num5 <= Config.下马击落机率)
        {
            RemoveBuffEx(v.ID.V);
        }
    }

    public void 被动受伤时处理(SkillObject skill, C_02_CalculateTargetDamage 参数, HitInfo 详情, float 伤害系数)
    {
        if (Config.CurrentVersion >= 1 && skill.Caster is PlayerObject 玩家实例2 && 玩家实例2.ParalysisRing && Compute.CalculateProbability(Config.自定义麻痹几率))
        {
            AddBuff(49160, 玩家实例2);
        }
        MapObject caster = ((skill.Caster is TrapObject trap) ? trap.Caster : skill.Caster);
        if (caster.Buffs.TryGetValue(2555, out var v))
        {
            caster.RemoveBuffEx(v.ID.V);
        }
        if (Dead)
        {
            详情.SkillFeedback = SkillHitFeedback.Lose;
        }
        else if (!Neighbors.Contains(caster))
        {
            详情.SkillFeedback = SkillHitFeedback.Lose;
        }
        else if ((caster.GetRelationship(this) & GameObjectRelationship.Hostile) == 0)
        {
            详情.SkillFeedback = SkillHitFeedback.Lose;
        }
        else if (this is MonsterObject 怪物实例2 && (怪物实例2.MonID == 8618 || 怪物实例2.MonID == 8621) && GetDistance(caster) >= 4)
        {
            详情.SkillFeedback = SkillHitFeedback.Lose;
        }
        if ((详情.SkillFeedback & SkillHitFeedback.Immune) != 0 || (详情.SkillFeedback & SkillHitFeedback.Lose) != 0)
        {
            return;
        }
        if ((详情.SkillFeedback & SkillHitFeedback.Miss) == 0)
        {
            if (参数.技能斩杀类型 != 0 && Compute.CalculateProbability(参数.技能斩杀概率) && IsValidTarget(caster, 参数.技能斩杀类型))
            {
                详情.SkillDamage = CurrentHP;
            }
            else
            {
                int num = ((参数.技能伤害基数?.Length > skill.SkillLevel) ? 参数.技能伤害基数[skill.SkillLevel] : 0);
                float num2 = ((参数.技能伤害系数?.Length > skill.SkillLevel) ? 参数.技能伤害系数[skill.SkillLevel] : 0f);
                if (this is MonsterObject)
                {
                    num += caster[Stat.怪物伤害];
                }
                int num3 = 0;
                float num4 = 0f;
                if (参数.技能增伤类型 != 0 && IsValidTarget(caster, 参数.技能增伤类型))
                {
                    num3 = 参数.技能增伤基数;
                    num4 = 参数.技能增伤系数;
                }
                int num5 = 0;
                float num6 = 0f;
                if (参数.技能破防概率 > 0f && Compute.CalculateProbability(参数.技能破防概率))
                {
                    num5 = 参数.技能破防基数;
                    num6 = 参数.技能破防系数;
                }
                int num7 = 0;
                int num8 = 0;
                switch (参数.技能伤害类型)
                {
                    case SkillDamageType.Attack:
                        num8 = Compute.CalculateDefence(this[Stat.MinDef], this[Stat.MaxDef]);
                        num7 = Compute.CalculateAttack(caster[Stat.MinDC], caster[Stat.MaxDC], caster[Stat.Luck]);
                        break;
                    case SkillDamageType.Magic:
                        num8 = Compute.CalculateDefence(this[Stat.MinMCDef], this[Stat.MaxMCDef]);
                        num7 = Compute.CalculateAttack(caster[Stat.MinMC], caster[Stat.MaxMC], caster[Stat.Luck]);
                        break;
                    case SkillDamageType.Taoism:
                        num8 = Compute.CalculateDefence(this[Stat.MinMCDef], this[Stat.MaxMCDef]);
                        num7 = Compute.CalculateAttack(caster[Stat.MinSC], caster[Stat.MaxSC], caster[Stat.Luck]);
                        break;
                    case SkillDamageType.Piercing:
                        num8 = Compute.CalculateDefence(this[Stat.MinDef], this[Stat.MaxDef]);
                        num7 = Compute.CalculateAttack(caster[Stat.MinNC], caster[Stat.MaxNC], caster[Stat.Luck]);
                        break;
                    case SkillDamageType.Archery:
                        num8 = Compute.CalculateDefence(this[Stat.MinDef], this[Stat.MaxDef]);
                        num7 = Compute.CalculateAttack(caster[Stat.MinBC], caster[Stat.MaxBC], caster[Stat.Luck]);
                        break;
                    case SkillDamageType.Toxicity:
                        num7 = caster[Stat.MaxSC];
                        break;
                    case SkillDamageType.Sacred:
                        num7 = Compute.CalculateAttack(caster[Stat.MinHC], caster[Stat.MaxHC], caster[Stat.Luck]);
                        break;
                }
                if (this is MonsterObject)
                {
                    num8 = Math.Max(0, num8 - (int)((float)(num8 * caster[Stat.怪物破防]) / 10000f));
                }
                int num9 = 0;
                float num10 = 0f;
                int num11 = int.MaxValue;
                foreach (BuffInfo item in caster.Buffs.Values.ToList())
                {
                    if ((item.BuffEffect & BuffEffectType.DamageIncOrDec) == 0 || (item.Template.HowJudgeEffect != 0 && item.Template.HowJudgeEffect != BuffDeterminationMethod.ActiveAttacksDecreaseDamage))
                    {
                        continue;
                    }
                    bool flag = false;
                    switch (参数.技能伤害类型)
                    {
                        case SkillDamageType.Magic:
                        case SkillDamageType.Taoism:
                            switch (item.Template.EffectJudgeType)
                            {
                                case BuffDeterminationType.AllSpellDamage:
                                case BuffDeterminationType.AllMagicDamage:
                                    flag = true;
                                    break;
                                case BuffDeterminationType.AllSpecificDamage:
                                    flag = item.Template.SpecificSkillID?.Contains(skill.SkillID) ?? false;
                                    break;
                            }
                            break;
                        case SkillDamageType.Attack:
                        case SkillDamageType.Piercing:
                        case SkillDamageType.Archery:
                            switch (item.Template.EffectJudgeType)
                            {
                                case BuffDeterminationType.AllSpecificDamage:
                                    flag = item.Template.SpecificSkillID?.Contains(skill.SkillID) ?? false;
                                    break;
                                case BuffDeterminationType.AllSpellDamage:
                                case BuffDeterminationType.AllPhysicalDamage:
                                    flag = true;
                                    break;
                            }
                            break;
                        case SkillDamageType.Toxicity:
                        case SkillDamageType.Sacred:
                        case SkillDamageType.Burn:
                        case SkillDamageType.Tear:
                            if (item.Template.EffectJudgeType == BuffDeterminationType.AllSpecificDamage)
                            {
                                flag = item.Template.SpecificSkillID?.Contains(skill.SkillID) ?? false;
                            }
                            break;
                    }
                    if (!flag)
                    {
                        continue;
                    }
                    int num12 = item.当前层数.V * ((item.Template.DamageIncOrDecBase?.Length > item.BuffLevel.V) ? item.Template.DamageIncOrDecBase[item.BuffLevel.V] : 0);
                    float num13 = (float)(int)item.当前层数.V * ((item.Template.DamageIncOrDecFactor?.Length > item.BuffLevel.V) ? item.Template.DamageIncOrDecFactor[item.BuffLevel.V] : 0f);
                    num9 += ((item.Template.HowJudgeEffect == BuffDeterminationMethod.ActiveAttacksIncreaseDamage) ? num12 : (-num12));
                    num10 += ((item.Template.HowJudgeEffect == BuffDeterminationMethod.ActiveAttacksIncreaseDamage) ? num13 : (0f - num13));
                    if (item.Template.EffectiveFollowedByID != 0 && item.Caster != null && MapManager.Objects.TryGetValue(item.Caster.ObjectID, out var value) && value == item.Caster)
                    {
                        if (item.Template.FollowedBySkillOwner)
                        {
                            caster.AddBuff(item.Template.EffectiveFollowedByID, item.Caster);
                        }
                        else
                        {
                            AddBuff(item.Template.EffectiveFollowedByID, item.Caster);
                        }
                    }
                    if (item.Template.EffectRemoved)
                    {
                        caster.RemoveBuffEx(item.ID.V);
                    }
                }
                foreach (BuffInfo item2 in Buffs.Values.ToList())
                {
                    if ((item2.BuffEffect & BuffEffectType.DamageIncOrDec) == 0 || (item2.Template.HowJudgeEffect != BuffDeterminationMethod.PassiveDamageIncrease && item2.Template.HowJudgeEffect != BuffDeterminationMethod.PassiveDecreaseDamage))
                    {
                        continue;
                    }
                    bool flag2 = false;
                    switch (参数.技能伤害类型)
                    {
                        case SkillDamageType.Magic:
                        case SkillDamageType.Taoism:
                            switch (item2.Template.EffectJudgeType)
                            {
                                case BuffDeterminationType.AllSpellDamage:
                                case BuffDeterminationType.AllMagicDamage:
                                    flag2 = true;
                                    break;
                                case BuffDeterminationType.AllSpecificDamage:
                                    flag2 = item2.Template.SpecificSkillID.Contains(skill.SkillID);
                                    break;
                                case BuffDeterminationType.SourceSpecificDamage:
                                    flag2 = caster == item2.Caster && (item2.Template.SpecificSkillID?.Contains(skill.SkillID) ?? false);
                                    break;
                                case BuffDeterminationType.SourceSkillDamage:
                                case BuffDeterminationType.SourceMagicDamage:
                                    flag2 = caster == item2.Caster;
                                    break;
                            }
                            break;
                        case SkillDamageType.Attack:
                        case SkillDamageType.Piercing:
                        case SkillDamageType.Archery:
                            switch (item2.Template.EffectJudgeType)
                            {
                                case BuffDeterminationType.AllSpecificDamage:
                                    flag2 = item2.Template.SpecificSkillID?.Contains(skill.SkillID) ?? false;
                                    break;
                                case BuffDeterminationType.AllSpellDamage:
                                case BuffDeterminationType.AllPhysicalDamage:
                                    flag2 = true;
                                    break;
                                case BuffDeterminationType.SourceSpecificDamage:
                                    flag2 = caster == item2.Caster && (item2.Template.SpecificSkillID?.Contains(skill.SkillID) ?? false);
                                    break;
                                case BuffDeterminationType.SourceSkillDamage:
                                case BuffDeterminationType.SourcePhysicalDamage:
                                    flag2 = caster == item2.Caster;
                                    break;
                            }
                            break;
                        case SkillDamageType.Toxicity:
                        case SkillDamageType.Sacred:
                        case SkillDamageType.Burn:
                        case SkillDamageType.Tear:
                            switch (item2.Template.EffectJudgeType)
                            {
                                case BuffDeterminationType.SourceSpecificDamage:
                                    flag2 = caster == item2.Caster && (item2.Template.SpecificSkillID?.Contains(skill.SkillID) ?? false);
                                    break;
                                case BuffDeterminationType.AllSpecificDamage:
                                    flag2 = item2.Template.SpecificSkillID?.Contains(skill.SkillID) ?? false;
                                    break;
                            }
                            break;
                    }
                    if (!flag2)
                    {
                        continue;
                    }
                    int num14 = item2.当前层数.V * ((item2.Template.DamageIncOrDecBase?.Length > item2.BuffLevel.V) ? item2.Template.DamageIncOrDecBase[item2.BuffLevel.V] : 0);
                    float num15 = (float)(int)item2.当前层数.V * ((item2.Template.DamageIncOrDecFactor?.Length > item2.BuffLevel.V) ? item2.Template.DamageIncOrDecFactor[item2.BuffLevel.V] : 0f);
                    num9 += ((item2.Template.HowJudgeEffect == BuffDeterminationMethod.PassiveDamageIncrease) ? num14 : (-num14));
                    num10 += ((item2.Template.HowJudgeEffect == BuffDeterminationMethod.PassiveDamageIncrease) ? num15 : (0f - num15));
                    if (item2.Template.EffectiveFollowedByID != 0 && item2.Caster != null && MapManager.Objects.TryGetValue(item2.Caster.ObjectID, out var value2) && value2 == item2.Caster)
                    {
                        if (item2.Template.FollowedBySkillOwner)
                        {
                            caster.AddBuff(item2.Template.EffectiveFollowedByID, item2.Caster);
                        }
                        else
                        {
                            AddBuff(item2.Template.EffectiveFollowedByID, item2.Caster);
                        }
                    }
                    if (item2.Template.HowJudgeEffect == BuffDeterminationMethod.PassiveDecreaseDamage && item2.Template.LimitedDamage)
                    {
                        num11 = Math.Min(num11, item2.Template.LimitedDamageValue);
                    }
                    if (item2.Template.EffectRemoved)
                    {
                        RemoveBuffEx(item2.ID.V);
                    }
                }
                float num16 = (num2 + num4) * (float)num7 + (float)num + (float)num3 + (float)num9;
                float val = (float)(num8 - num5) - (float)num8 * num6;
                float val2 = (num16 - Math.Max(0f, val)) * (1f + num10) * 伤害系数;
                int num17 = (详情.SkillDamage = (int)Math.Min(num11, Math.Max(0f, val2)));
            }
        }
        AttackStopTime = SEngine.CurrentTime.AddSeconds(10.0);
        caster.AttackStopTime = SEngine.CurrentTime.AddSeconds(10.0);
        if ((详情.SkillFeedback & SkillHitFeedback.Miss) == 0)
        {
            foreach (BuffInfo item3 in Buffs.Values.ToList())
            {
                if ((item3.BuffEffect & BuffEffectType.StatusFlag) != 0 && (item3.Template.PlayerState & GameObjectState.Unconscious) != 0)
                {
                    RemoveBuffEx(item3.ID.V);
                }
            }
        }
        if (this is MonsterObject 怪物实例3)
        {
            怪物实例3.HardStunTime = SEngine.CurrentTime.AddMilliseconds(参数.目标硬直时间);
            if (caster is PlayerObject || caster is PetObject)
            {
                怪物实例3.Target.Add(caster, SEngine.CurrentTime.AddMilliseconds(怪物实例3.HateTime), 详情.SkillDamage);
            }
        }
        else if (this is PlayerObject 玩家实例3)
        {
            if (详情.SkillDamage > 0)
            {
                玩家实例3.DamageAllEquipment(详情.SkillDamage);
            }
            if (详情.SkillDamage > 0)
            {
                玩家实例3.扣除护盾时间(详情.SkillDamage);
            }
            if (玩家实例3.GetRelationship(caster) == GameObjectRelationship.Hostile)
            {
                foreach (PetObject item4 in 玩家实例3.Pets.ToList())
                {
                    if (item4.Neighbors.Contains(caster) && !caster.CheckStatus(GameObjectState.Invisible | GameObjectState.Stealth))
                    {
                        item4.Target.Add(caster, SEngine.CurrentTime.AddMilliseconds(item4.HateDuration), 0);
                    }
                }
            }
            if (caster is PlayerObject 玩家实例4 && !CurrentMap.自由区内(CurrentPosition) && !玩家实例3.GreyName && !玩家实例3.RedName)
            {
                if (玩家实例4.RedName)
                {
                    玩家实例4.PKTime = TimeSpan.FromMinutes(1.0);
                }
                else
                {
                    玩家实例4.GreyTime = TimeSpan.FromMinutes(1.0);
                }
            }
            else if (caster is PetObject 宠物实例2 && !CurrentMap.自由区内(CurrentPosition) && !玩家实例3.GreyName && !玩家实例3.RedName)
            {
                if (宠物实例2.Master.RedName)
                {
                    宠物实例2.Master.PKTime = TimeSpan.FromMinutes(1.0);
                }
                else
                {
                    宠物实例2.Master.GreyTime = TimeSpan.FromMinutes(1.0);
                }
            }
        }
        else if (this is PetObject 宠物实例3)
        {
            if (caster != 宠物实例3.Master && 宠物实例3.GetRelationship(caster) == GameObjectRelationship.Hostile)
            {
                foreach (PetObject item5 in 宠物实例3.Master?.Pets.ToList())
                {
                    if (item5.Neighbors.Contains(caster) && !caster.CheckStatus(GameObjectState.Invisible | GameObjectState.Stealth))
                    {
                        item5.Target.Add(caster, SEngine.CurrentTime.AddMilliseconds(item5.HateDuration), 0);
                    }
                }
            }
            if (caster != 宠物实例3.Master && caster is PlayerObject 玩家实例5 && !CurrentMap.自由区内(CurrentPosition) && !宠物实例3.Master.GreyName && !宠物实例3.Master.RedName)
            {
                玩家实例5.GreyTime = TimeSpan.FromMinutes(1.0);
            }
        }
        else if (this is GuardObject 守卫实例2 && 守卫实例2.GetRelationship(caster) == GameObjectRelationship.Hostile)
        {
            守卫实例2.Target.Add(caster, default(DateTime), 0);
        }
        if (caster is PlayerObject 玩家实例6)
        {
            if (玩家实例6.GetRelationship(this) == GameObjectRelationship.Hostile && !CheckStatus(GameObjectState.Invisible | GameObjectState.Stealth))
            {
                foreach (PetObject item6 in 玩家实例6.Pets.ToList())
                {
                    if (item6.Neighbors.Contains(this))
                    {
                        item6.Target.Add(this, SEngine.CurrentTime.AddMilliseconds(item6.HateDuration), 参数.增加宠物仇恨 ? 详情.SkillDamage : 0);
                    }
                }
            }
            if (SEngine.CurrentTime > 玩家实例6.BattleEquipmentTime && !玩家实例6.Dead && 玩家实例6.CurrentHP < 玩家实例6[Stat.MaxHP] && 玩家实例6.Equipment.TryGetValue(15, out var v2) && v2.Dura.V > 0 && (v2.ID == 99999106 || v2.ID == 99999107))
            {
                玩家实例6.CurrentHP += ((this is MonsterObject) ? 20 : 10);
                玩家实例6.DamageCombatEquipment(1);
                玩家实例6.BattleEquipmentTime = SEngine.CurrentTime.AddMilliseconds(1000.0);
            }
        }
        if (this is PlayerObject 玩家实例7 && 玩家实例7.HasProtectionRing)
        {
            if (Config.CurrentVersion >= 1)
            {
                int num19 = (CurrentMP = Math.Max(0, CurrentMP - 详情.SkillDamage));
                if (num19 == 0 && (CurrentHP = Math.Max(0, CurrentHP - 详情.SkillDamage)) == 0)
                {
                    详情.SkillFeedback |= SkillHitFeedback.Death;
                    Die(caster, skillDeath: true);
                }
            }
            else if ((CurrentHP = Math.Max(0, CurrentHP - 详情.SkillDamage)) == 0)
            {
                详情.SkillFeedback |= SkillHitFeedback.Death;
                Die(caster, skillDeath: true);
            }
            return;
        }
        if ((CurrentHP = Math.Max(0, CurrentHP - 详情.SkillDamage)) == 0)
        {
            详情.SkillFeedback |= SkillHitFeedback.Death;
            Die(caster, skillDeath: true);
        }
        if (caster is PlayerObject 玩家实例8 && 玩家实例8.Character.当前角色魔法回击.V > 0 && SEngine.CurrentTime > 玩家实例8.回击计时 && 玩家实例8.Character.当前角色魔法回击.V > 0)
        {
            玩家实例8.CurrentHP += (int)((double)((float)玩家实例8[Stat.MaxHP] * (float)(玩家实例8.Character.当前角色魔法回击.V / 48)) * 2.2) / 100;
            玩家实例8.回击计时 = SEngine.CurrentTime.AddMilliseconds(1000.0);
        }
        if (caster is PlayerObject 玩家实例9 && 玩家实例9.Character.当前角色物理回击.V > 0 && SEngine.CurrentTime > 玩家实例9.回击计时 && 玩家实例9.Character.当前角色物理回击.V > 0)
        {
            玩家实例9.CurrentHP += (int)((double)((float)玩家实例9[Stat.MaxHP] * (float)(玩家实例9.Character.当前角色物理回击.V / 48)) * 2.2) / 100;
            玩家实例9.回击计时 = SEngine.CurrentTime.AddMilliseconds(1000.0);
        }
    }

    public void 被动回复时处理(SkillObject skill, C_05_CalculateTargetReply 参数)
    {
        if (!Dead && CurrentMap == skill.Caster.CurrentMap && (this == skill.Caster || Neighbors.Contains(skill.Caster)))
        {
            MapObject caster = ((skill.Caster is TrapObject trap) ? trap.Caster : skill.Caster);
            int num = ((参数.体力回复次数?.Length > skill.SkillLevel) ? 参数.体力回复次数[skill.SkillLevel] : 0);
            int num2 = ((参数.HealthRecoveryBase?.Length > skill.SkillLevel) ? 参数.HealthRecoveryBase[skill.SkillLevel] : 0);
            float num3 = ((参数.道术叠加次数?.Length > skill.SkillLevel) ? 参数.道术叠加次数[skill.SkillLevel] : 0f);
            float num4 = ((参数.道术叠加基数?.Length > skill.SkillLevel) ? 参数.道术叠加基数[skill.SkillLevel] : 0f);
            int num5 = ((参数.立即回复基数?.Length > skill.SkillLevel && caster == this) ? 参数.立即回复基数[skill.SkillLevel] : 0);
            float num6 = ((!(参数.立即回复系数?.Length > skill.SkillLevel) || caster != this) ? 0f : 参数.立即回复系数[skill.SkillLevel]);
            if (num3 > 0f)
            {
                num += (int)(num3 * (float)Compute.CalculateAttack(caster[Stat.MinSC], caster[Stat.MaxSC], caster[Stat.Luck]));
            }
            if (num4 > 0f)
            {
                num2 += (int)(num4 * (float)Compute.CalculateAttack(caster[Stat.MinSC], caster[Stat.MaxSC], caster[Stat.Luck]));
            }
            if (num5 > 0)
            {
                CurrentHP += num5;
            }
            if (num6 > 0f)
            {
                CurrentHP += (int)((float)this[Stat.MaxHP] * num6);
            }
            if (num > HealCount && num2 > 0)
            {
                HealCount = (byte)num;
                HealAmount = num2;
                HealTime = SEngine.CurrentTime.AddMilliseconds(500.0);
            }
        }
    }

    public void ProcessBuffDamage(BuffInfo buff)
    {
        int defence = 0;
        switch (buff.DamageType)
        {
            case SkillDamageType.Magic:
            case SkillDamageType.Taoism:
                defence = Compute.CalculateDefence(this[Stat.MinMCDef], this[Stat.MaxMCDef]);
                break;
            case SkillDamageType.Attack:
            case SkillDamageType.Piercing:
            case SkillDamageType.Archery:
                defence = Compute.CalculateDefence(this[Stat.MinDef], this[Stat.MaxDef]);
                break;
        }
        int damage = Math.Max(0, buff.DamageBase.V * buff.当前层数.V - defence);
        CurrentHP = Math.Max(0, CurrentHP - damage);

        SendPacket(new 触发状态效果
        {
            BuffID = buff.ID.V,
            CasterID = (buff.Caster?.ObjectID ?? 0),
            TargetID = ObjectID,
            HealthAmount = -damage
        });

        if (CurrentHP == 0)
        {
            Die(buff.Caster, false);
        }
    }

    public void ProcessBuffHealthRecovery(BuffInfo buff)
    {
        if (buff.Template.HealthRecoveryBase != null && buff.Template.HealthRecoveryBase.Length > buff.BuffLevel.V)
        {
            byte b = buff.Template.HealthRecoveryBase[buff.BuffLevel.V];
            CurrentHP += b;
            SendPacket(new 触发状态效果
            {
                BuffID = buff.ID.V,
                CasterID = (buff.Caster?.ObjectID ?? 0),
                TargetID = ObjectID,
                HealthAmount = b
            });
        }
    }

    public void OnLocationChanged(Point location)
    {
        if (this is PlayerObject player)
        {
            player.CurrentTrade?.BreakTrade();
            foreach (BuffInfo item in Buffs.Values.ToList())
            {
                if ((item.BuffEffect & BuffEffectType.CreateTrap) != 0 && SkillTrap.DataSheet.TryGetValue(item.Template.TriggerTrapSkills, out var 陷阱模板2))
                {
                    int num = 0;
                    while (true)
                    {
                        Point point = Compute.GetFrontPosition(CurrentPosition, location, num);
                        if (point == location)
                            break;
                        Point[] array = Compute.CalculateGrid(point, CurrentDirection, item.Template.NumberTrapsTriggered);
                        foreach (Point 坐标2 in array)
                        {
                            if (CurrentMap.ValidTerrain(坐标2) && CurrentMap[坐标2].FirstOrDefault((MapObject O) => O is TrapObject 陷阱实例3 && 陷阱实例3.GroupID != 0 && 陷阱实例3.GroupID == 陷阱模板2.GroupID) == null)
                            {
                                Traps.Add(new TrapObject(this, 陷阱模板2, CurrentMap, 坐标2));
                            }
                        }
                        num++;
                    }
                }
                if ((item.BuffEffect & BuffEffectType.StatusFlag) != 0 && (item.Template.PlayerState & GameObjectState.Invisible) != 0)
                {
                    RemoveBuffEx(item.ID.V);
                }
            }
        }
        else if (this is PetObject)
        {
            foreach (BuffInfo item2 in Buffs.Values.ToList())
            {
                if ((item2.BuffEffect & BuffEffectType.CreateTrap) != 0 && SkillTrap.DataSheet.TryGetValue(item2.Template.TriggerTrapSkills, out var 陷阱模板))
                {
                    var n = 0;
                    while (true)
                    {
                        var next = Compute.GetFrontPosition(CurrentPosition, location, n);
                        if (next == location)
                            break;
                        var grid = Compute.CalculateGrid(next, CurrentDirection, item2.Template.NumberTrapsTriggered);
                        foreach (var point in grid)
                        {
                            if (CurrentMap.ValidTerrain(point) && CurrentMap[point].FirstOrDefault((MapObject O) => O is TrapObject 陷阱实例2 && 陷阱实例2.GroupID != 0 && 陷阱实例2.GroupID == 陷阱模板.GroupID) == null)
                            {
                                Traps.Add(new TrapObject(this, 陷阱模板, CurrentMap, point));
                            }
                        }
                        n++;
                    }
                }
                if ((item2.BuffEffect & BuffEffectType.StatusFlag) != 0 && (item2.Template.PlayerState & GameObjectState.Invisible) != 0)
                {
                    RemoveBuffEx(item2.ID.V);
                }
            }
        }
        
        UnbindGrid();
        CurrentPosition = location;
        BindGrid();
        UpdateAllNeighbours();

        foreach (var obj in Neighbors)
            obj.ProcessOnMoved(this);
    }

    public void RemoveAllNeighbors()
    {
        foreach (var obj in Neighbors)
            obj.Disappear(this);
        Neighbors.Clear();
        ImportantNeighbors.Clear();
        StealthNeighbors.Clear();
    }

    public void UpdateAllNeighbours()
    {
        foreach (var obj in Neighbors)
        {
            if (CurrentMap != obj.CurrentMap || !CanBeSeenBy(obj))
            {
                obj.Disappear(this);
                Disappear(obj);
            }
        }
        for (var x = -20; x <= 20; x++)
        {
            for (var y = -20; y <= 20; y++)
            {
                var objs = CurrentMap[new Point(CurrentPosition.X + x, CurrentPosition.Y + y)].ToList();

                foreach (var obj in objs)
                {
                    if (obj != this)
                    {
                        if (!Neighbors.Contains(obj) && IsNeightbor(obj))
                            Appear(obj);
                        if (!obj.Neighbors.Contains(this) && obj.IsNeightbor(this))
                            obj.Appear(this);
                    }
                }
            }
        }
    }

    public void ProcessOnMoved(MapObject obj)
    {
        if (this is ItemObject)
            return;
        
        if (this is PetObject pet)
        {
            HateObject.HateInfo value;
            if (pet.CanAttack(obj) && GetDistance(obj) <= pet.HateRange && !obj.CheckStatus(GameObjectState.Invisible | GameObjectState.Stealth))
            {
                pet.Target.Add(obj, default(DateTime), 0);
            }
            else if (GetDistance(obj) > pet.HateRange && pet.Target.TargetList.TryGetValue(obj, out value) && value.HateTime < SEngine.CurrentTime)
            {
                pet.Target.Remove(obj);
            }
        }
        else if (this is MonsterObject monster)
        {
            HateObject.HateInfo value2;
            if (GetDistance(obj) <= monster.TargetRange && monster.CanAttack(obj) && (monster.VisibleStealthTargets || !obj.CheckStatus(GameObjectState.Invisible | GameObjectState.Stealth)))
            {
                monster.Target.Add(obj, default(DateTime), 0);
            }
            else if (GetDistance(obj) > monster.TargetRange && monster.Target.TargetList.TryGetValue(obj, out value2) && value2.HateTime < SEngine.CurrentTime)
            {
                monster.Target.Remove(obj);
            }
        }
        else if (this is TrapObject trap)
        {
            if (Compute.CalculateGrid(trap.CurrentPosition, trap.CurrentDirection, trap.Size).Contains(obj.CurrentPosition))
            {
                trap.ActivatePassive(obj);
            }
        }
        else if (this is GuardObject guard)
        {
            if (guard.CanAttack(obj) && GetDistance(obj) <= guard.TargetRange)
            {
                guard.Target.Add(obj, default(DateTime), 0);
            }
            else if (GetDistance(obj) > guard.TargetRange)
            {
                guard.Target.Remove(obj);
            }
        }

        if (obj is PetObject pet2)
        {
            HateObject.HateInfo value3;
            if (pet2.GetDistance(this) <= pet2.HateRange && pet2.CanAttack(this) && !CheckStatus(GameObjectState.Invisible | GameObjectState.Stealth))
            {
                pet2.Target.Add(this, default(DateTime), 0);
            }
            else if (pet2.GetDistance(this) > pet2.HateRange && pet2.Target.TargetList.TryGetValue(this, out value3) && value3.HateTime < SEngine.CurrentTime)
            {
                pet2.Target.Remove(this);
            }
        }
        else if (obj is MonsterObject monster2)
        {
            HateObject.HateInfo value4;
            if (monster2.GetDistance(this) <= monster2.TargetRange && monster2.CanAttack(this) && (monster2.VisibleStealthTargets || !CheckStatus(GameObjectState.Invisible | GameObjectState.Stealth)))
            {
                monster2.Target.Add(this, default(DateTime), 0);
            }
            else if (monster2.GetDistance(this) > monster2.TargetRange && monster2.Target.TargetList.TryGetValue(this, out value4) && value4.HateTime < SEngine.CurrentTime)
            {
                monster2.Target.Remove(this);
            }
        }
        else if (obj is TrapObject trap2)
        {
            if (Compute.CalculateGrid(trap2.CurrentPosition, trap2.CurrentDirection, trap2.Size).Contains(CurrentPosition))
            {
                trap2.ActivatePassive(this);
            }
        }
        else if (obj is GuardObject guard2)
        {
            if (guard2.CanAttack(this) && guard2.GetDistance(this) <= guard2.TargetRange)
            {
                guard2.Target.Add(this, default(DateTime), 0);
            }
            else if (guard2.GetDistance(this) > guard2.TargetRange)
            {
                guard2.Target.Remove(this);
            }
        }
    }

    public void Appear(MapObject obj)
    {
        if (StealthNeighbors.Remove(obj))
        {
            if (this is ItemObject)
                return;

            if (this is PlayerObject player)
            {
                switch (obj.ObjectType)
                {
                    case GameObjectType.Pet:
                        {
                            player.Enqueue(new ObjectStopPacket
                            {
                                ObjectID = obj.ObjectID,
                                Position = obj.CurrentPosition,
                                Height = obj.CurrentHeight
                            });
                            player.Enqueue(new ObjectAppearPacket
                            {
                                Effect = 1,
                                ObjectID = obj.ObjectID,
                                Position = obj.CurrentPosition,
                                Height = obj.CurrentHeight,
                                Direction = (ushort)obj.CurrentDirection,
                                现身姿态 = (byte)(!obj.Dead ? 1u : 13u),
                                HealthPercent = (byte)(obj.CurrentHP * 100 / obj[Stat.MaxHP])
                            });
                            player.Enqueue(new SyncObjectHP
                            {
                                ObjectID = obj.ObjectID,
                                CurrentHP = obj.CurrentHP,
                                MaxHP = obj[Stat.MaxHP]
                            });
                            player.Enqueue(new 对象变换类型
                            {
                                改变类型 = 2,
                                对象编号 = obj.ObjectID
                            });
                            break;
                        }
                    case GameObjectType.Player:
                    case GameObjectType.Monster:
                    case GameObjectType.NPC:
                        {
                            player.Enqueue(new ObjectStopPacket
                            {
                                ObjectID = obj.ObjectID,
                                Position = obj.CurrentPosition,
                                Height = obj.CurrentHeight
                            });

                            var target = obj as PlayerObject;
                            var ncolor = (byte)((target != null && target.GreyName) ? 2u : 0u);

                            player.Enqueue(new ObjectAppearPacket
                            {
                                Effect = 1,
                                ObjectID = obj.ObjectID,
                                Position = obj.CurrentPosition,
                                Height = obj.CurrentHeight,
                                Direction = (ushort)obj.CurrentDirection,
                                现身姿态 = (byte)((!obj.Dead) ? 1u : 13u),
                                补充参数 = ncolor,
                                HealthPercent = (byte)(obj.CurrentHP * 100 / obj[Stat.MaxHP])
                            });
                            
                            player.Enqueue(new SyncObjectHP
                            {
                                ObjectID = obj.ObjectID,
                                CurrentHP = obj.CurrentHP,
                                MaxHP = obj[Stat.MaxHP]
                            });
                            break;
                        }
                    case GameObjectType.Trap:
                        {
                            if (obj is TrapObject trap)
                            {
                                player.Enqueue(new 陷阱进入视野
                                {
                                    ObjectID = trap.ObjectID,
                                    Position = trap.CurrentPosition,
                                    Height = trap.CurrentHeight,
                                    CasterID = trap.Caster.ObjectID,
                                    TrapID = trap.TrapID,
                                    Duration = trap.RemainingTime
                                });
                            }
                            break;
                        }
                    case GameObjectType.Item:
                        {
                            if (obj is ItemObject item)
                            {
                                player.Enqueue(new 对象掉落物品
                                {
                                    ObjectID = item.ObjectID,
                                    MapID = item.ObjectID,
                                    Position = item.CurrentPosition,
                                    Height = item.CurrentHeight,
                                    ItemID = item.ItemID,
                                    Quantity = item.Quantity
                                });
                            }
                            break;
                        }
                }
                if (obj.Buffs.Count > 0)
                {
                    player.Enqueue(new 同步对象Buff
                    {
                        Description = obj.对象Buff简述()
                    });
                }
            }
            else if (this is TrapObject trap)
            {
                if (Compute.CalculateGrid(trap.CurrentPosition, trap.CurrentDirection, trap.Size).Contains(obj.CurrentPosition))
                {
                    trap.ActivatePassive(obj);
                }
            }
            else if (this is PetObject pet)
            {
                if (GetDistance(obj) <= pet.HateRange && pet.CanAttack(obj) && !obj.CheckStatus(GameObjectState.Invisible | GameObjectState.Stealth))
                {
                    pet.Target.Add(obj, default(DateTime), 0);
                }
                else if (GetDistance(obj) > pet.HateRange && pet.Target.TargetList.TryGetValue(obj, out var hater) && hater.HateTime < SEngine.CurrentTime)
                {
                    pet.Target.Remove(obj);
                }
            }
            else if (this is MonsterObject monster)
            {
                if (GetDistance(obj) <= monster.TargetRange && monster.CanAttack(obj) && (monster.VisibleStealthTargets || !obj.CheckStatus(GameObjectState.Invisible | GameObjectState.Stealth)))
                {
                    monster.Target.Add(obj, default(DateTime), 0);
                }
                else if (GetDistance(obj) > monster.TargetRange && monster.Target.TargetList.TryGetValue(obj, out var hater) && hater.HateTime < SEngine.CurrentTime)
                {
                    monster.Target.Remove(obj);
                }
            }
        }
        else
        {
            if (!Neighbors.Add(obj))
                return;

            if (obj is PlayerObject || obj is PetObject)
                ImportantNeighbors.Add(obj);
            
            if (this is ItemObject)
                return;

            if (this is PlayerObject player)
            {
                switch (obj.ObjectType)
                {
                    case GameObjectType.Pet:
                        player.Enqueue(new ObjectStopPacket
                        {
                            ObjectID = obj.ObjectID,
                            Position = obj.CurrentPosition,
                            Height = obj.CurrentHeight
                        });
                        player.Enqueue(new ObjectAppearPacket
                        {
                            Effect = 1,
                            ObjectID = obj.ObjectID,
                            Position = obj.CurrentPosition,
                            Height = obj.CurrentHeight,
                            Direction = (ushort)obj.CurrentDirection,
                            现身姿态 = (byte)(!obj.Dead ? 1u : 13u),
                            HealthPercent = (byte)(obj.CurrentHP * 100 / obj[Stat.MaxHP])
                        });
                        player.Enqueue(new SyncObjectHP
                        {
                            ObjectID = obj.ObjectID,
                            CurrentHP = obj.CurrentHP,
                            MaxHP = obj[Stat.MaxHP]
                        });
                        player.Enqueue(new 对象变换类型
                        {
                            改变类型 = 2,
                            对象编号 = obj.ObjectID
                        });
                        break;
                    case GameObjectType.Player:
                    case GameObjectType.Monster:
                    case GameObjectType.NPC:
                        {
                            player.Enqueue(new ObjectStopPacket
                            {
                                ObjectID = obj.ObjectID,
                                Position = obj.CurrentPosition,
                                Height = obj.CurrentHeight
                            });

                            var target = obj as PlayerObject;
                            var ncolor = (byte)((target != null && target.GreyName) ? 2u : 0u);

                            player.Enqueue(new ObjectAppearPacket
                            {
                                Effect = 1,
                                ObjectID = obj.ObjectID,
                                Position = obj.CurrentPosition,
                                Height = obj.CurrentHeight,
                                Direction = (ushort)obj.CurrentDirection,
                                现身姿态 = (byte)(!obj.Dead ? 1u : 13u),
                                补充参数 = ncolor,
                                HealthPercent = (byte)(obj.CurrentHP * 100 / obj[Stat.MaxHP])
                            });
                            
                            player.Enqueue(new SyncObjectHP
                            {
                                ObjectID = obj.ObjectID,
                                CurrentHP = obj.CurrentHP,
                                MaxHP = obj[Stat.MaxHP]
                            });
                            break;
                        }
                    case GameObjectType.Trap:
                        if (obj is TrapObject trap)
                        {
                            player.Enqueue(new 陷阱进入视野
                            {
                                ObjectID = trap.ObjectID,
                                Position = trap.CurrentPosition,
                                Height = trap.CurrentHeight,
                                CasterID = trap.Caster.ObjectID,
                                TrapID = trap.TrapID,
                                Duration = trap.RemainingTime
                            });
                        }
                        break;
                    case GameObjectType.Item:
                        if (obj is ItemObject item)
                        {
                            player.Enqueue(new 对象掉落物品
                            {
                                ObjectID = item.ObjectID,
                                MapID = item.ObjectID,
                                Position = item.CurrentPosition,
                                Height = item.CurrentHeight,
                                ItemID = item.ItemID,
                                Quantity = item.Quantity
                            });
                        }
                        break;
                }
                if (obj.Buffs.Count > 0)
                {
                    player.Enqueue(new 同步对象Buff
                    {
                        Description = obj.对象Buff简述()
                    });
                }
            }
            else if (this is TrapObject trap)
            {
                if (Compute.CalculateGrid(trap.CurrentPosition, trap.CurrentDirection, trap.Size).Contains(obj.CurrentPosition))
                {
                    trap.ActivatePassive(obj);
                }
            }
            else if (this is PetObject pet && !Dead)
            {
                HateObject.HateInfo value3;
                if (GetDistance(obj) <= pet.HateRange && pet.CanAttack(obj) && !obj.CheckStatus(GameObjectState.Invisible | GameObjectState.Stealth))
                {
                    pet.Target.Add(obj, default(DateTime), 0);
                }
                else if (GetDistance(obj) > pet.HateRange && pet.Target.TargetList.TryGetValue(obj, out value3) && value3.HateTime < SEngine.CurrentTime)
                {
                    pet.Target.Remove(obj);
                }
            }
            else if (this is MonsterObject monster && !Dead)
            {
                if (GetDistance(obj) <= monster.TargetRange && monster.CanAttack(obj) && (monster.VisibleStealthTargets || !obj.CheckStatus(GameObjectState.Invisible | GameObjectState.Stealth)))
                {
                    monster.Target.Add(obj, default(DateTime), 0);
                }
                else if (GetDistance(obj) > monster.TargetRange && monster.Target.TargetList.TryGetValue(obj, out var hater) && hater.HateTime < SEngine.CurrentTime)
                {
                    monster.Target.Remove(obj);
                }
                if (ImportantNeighbors.Count != 0)
                {
                    monster.Activate();
                }
            }
            else if (this is GuardObject guard && !Dead)
            {
                if (guard.CanAttack(obj) && GetDistance(obj) <= guard.TargetRange)
                {
                    guard.Target.Add(obj, default(DateTime), 0);
                }
                else if (GetDistance(obj) > guard.TargetRange)
                {
                    guard.Target.Remove(obj);
                }
                if (ImportantNeighbors.Count != 0)
                {
                    guard.Activate();
                }
            }
        }
    }

    public void Disappear(MapObject obj)
    {
        if (!Neighbors.Remove(obj))
        {
            return;
        }
        StealthNeighbors.Remove(obj);
        ImportantNeighbors.Remove(obj);
        if (this is ItemObject)
        {
            return;
        }
        if (this is PlayerObject player)
        {
            player.Enqueue(new ObjectDisappearPacket
            {
                ObjectID = obj.ObjectID
            });
        }
        else if (this is PetObject pet)
        {
            pet.Target.Remove(obj);
        }
        else if (this is MonsterObject monster)
        {
            if (!Dead)
            {
                monster.Target.Remove(obj);
                if (monster.ImportantNeighbors.Count == 0)
                {
                    monster.Deactivate();
                }
            }
        }
        else if (this is GuardObject guard && !Dead)
        {
            guard.Target.Remove(obj);
            if (guard.ImportantNeighbors.Count == 0)
            {
                guard.Deactivate();
            }
        }
    }

    public void OnDeath(MapObject obj)
    {
        if (this is MonsterObject monster)
            monster.Target.Remove(obj);
        else if (this is PetObject pet)
            pet.Target.Remove(obj);
        else if (this is GuardObject guard)
            guard.Target.Remove(obj);
    }

    public void OnInvisible(MapObject obj)
    {
        if (this is PetObject pet && pet.Target.TargetList.ContainsKey(obj))
        {
            pet.Target.Remove(obj);
        }
        if (this is MonsterObject monster && monster.Target.TargetList.ContainsKey(obj) && !monster.VisibleStealthTargets)
        {
            monster.Target.Remove(obj);
        }
    }

    public void OnSneaking(MapObject obj)
    {
        if (this is PetObject pet)
        {
            if (pet.Target.TargetList.ContainsKey(obj))
                pet.Target.Remove(obj);
            StealthNeighbors.Add(obj);
        }
        if (this is MonsterObject monster && !monster.VisibleStealthTargets)
        {
            if (monster.Target.TargetList.ContainsKey(obj))
                monster.Target.Remove(obj);
            StealthNeighbors.Add(obj);
        }
        if (this is PlayerObject player && (GetRelationship(obj) == GameObjectRelationship.Hostile || obj.GetRelationship(this) == GameObjectRelationship.Hostile))
        {
            StealthNeighbors.Add(obj);
            player.Enqueue(new ObjectDisappearPacket
            {
                ObjectID = obj.ObjectID
            });
        }
    }

    public void ProcessStealthTarget(MapObject obj)
    {
        if (this is PetObject pet)
        {
            HateObject.HateInfo value;
            if (GetDistance(obj) <= pet.HateRange && pet.CanAttack(obj) && !obj.CheckStatus(GameObjectState.Invisible | GameObjectState.Stealth))
            {
                pet.Target.Add(obj, default(DateTime), 0);
            }
            else if (GetDistance(obj) > pet.HateRange && pet.Target.TargetList.TryGetValue(obj, out value) && value.HateTime < SEngine.CurrentTime)
            {
                pet.Target.Remove(obj);
            }
        }
        if (this is MonsterObject monster)
        {
            HateObject.HateInfo value2;
            if (GetDistance(obj) <= monster.TargetRange && monster.CanAttack(obj) && (monster.VisibleStealthTargets || !obj.CheckStatus(GameObjectState.Invisible | GameObjectState.Stealth)))
            {
                monster.Target.Add(obj, default(DateTime), 0);
            }
            else if (GetDistance(obj) > monster.TargetRange && monster.Target.TargetList.TryGetValue(obj, out value2) && value2.HateTime < SEngine.CurrentTime)
            {
                monster.Target.Remove(obj);
            }
        }
    }

    public void ProcessVisibleTarget(MapObject obj)
    {
        if (StealthNeighbors.Contains(obj))
        {
            Appear(obj);
        }
    }

    public byte[] 对象Buff详述()
    {
        using var ms = new MemoryStream(34);
        using var writer = new BinaryWriter(ms);
        writer.Write((byte)Buffs.Count);
        foreach (var kvp in Buffs)
        {
            writer.Write(kvp.Value.ID.V);
            writer.Write((int)kvp.Value.ID.V);
            writer.Write(kvp.Value.当前层数.V);
            writer.Write((int)kvp.Value.RemainingTime.V.TotalMilliseconds);
            writer.Write((int)kvp.Value.Duration.V.TotalMilliseconds);
        }
        return ms.ToArray();
    }

    public byte[] 对象Buff简述()
    {
        using var ms = new MemoryStream(34);
        using var writer = new BinaryWriter(ms);
        writer.Write(ObjectID);
        int num = 0;
        foreach (var kvp in Buffs)
        {
            writer.Write(kvp.Value.ID.V);
            writer.Write((int)kvp.Value.ID.V);
            if (++num >= 5)
                break;
        }
        return ms.ToArray();
    }
}
