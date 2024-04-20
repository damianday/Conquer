using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using GameServer.Map;
using GameServer.Database;
using GameServer.Networking;

using GamePackets.Server;

namespace GameServer.Template;

public class SkillObject
{
    public GameSkill Template;
    public SkillInfo Skill;
    public MapObject Caster;

    public byte ActionID;
    public byte SegmentID;
    public Map.Map CurrentMap;
    public MapObject Target;
    public Point TargetLocation;
    public Point CastLocation;

    public DateTime ReleaseTime;

    public SkillObject ParentSkill;

    public bool TargetBorrow;

    public Dictionary<int, HitInfo> HitList;

    public int FlyTime;

    public int AttackSpeedDecrease;

    public bool CanGainExperience;

    public DateTime ProcessTime;
    public DateTime RunTime;

    private SortedDictionary<int, SkillTask> Nodes;
    private KeyValuePair<int, SkillTask> FirstNode;

    public int CasterID => Caster.ObjectID;
    public byte GroupID => Template.GroupID;
    public byte InscriptionID => Template.ID;
    public ushort SkillID => Template.OwnSkillID;

    public byte SkillLevel
    {
        get
        {
            if (Template.BindingLevelID != 0)
            {
                if (Caster is PlayerObject player && player.Skills.TryGetValue(Template.BindingLevelID, out var v))
                    return v.Level.V;

                if (Caster is TrapObject trap && trap.Caster is PlayerObject 玩家实例2 && 玩家实例2.Skills.TryGetValue(Template.BindingLevelID, out var v2))
                    return v2.Level.V;
                return 0;
            }
            return 0;
        }
    }

    public bool 检查计数 => Template.CheckSkillCount;

    public SkillObject(MapObject caster, GameSkill gskill, SkillInfo skill, byte actionId, Map.Map map, Point location, MapObject target, Point targetLocation, SkillObject parentSkill, Dictionary<int, HitInfo> hitters = null, bool targetBorrow = false)
    {
        Caster = caster;
        Template = gskill;
        Skill = skill;
        ActionID = actionId;
        CurrentMap = map;
        CastLocation = location;
        Target = target;
        TargetLocation = targetLocation;
        ParentSkill = parentSkill;
        ReleaseTime = SEngine.CurrentTime;
        TargetBorrow = targetBorrow;
        HitList = hitters ?? new Dictionary<int, HitInfo>();

        /*if (skill != null && skill.铭文模板.SwitchSkills.Count > 0 && GameSkill.DataSheet.TryGetValue(skill.铭文模板.SwitchSkills[0], out var switchSkill) && gskill != switchSkill)
        {
            var switchReleaseSkill = switchSkill.Nodes
                .Select(x => x.Value)
                .OfType<B_01_SkillReleaseNotification>()
                .FirstOrDefault();

            if (switchReleaseSkill != null)
            {
                IsSwitchedSkill = true;
                CasterObject.Coolings[SkillId | 16777216] = ReleaseTime.AddMilliseconds(switchReleaseSkill.ItSelfCooldown);
                CasterObject.Coolings[switchSkill.BindingLevelId | 16777216] = ReleaseTime.AddMilliseconds(switchReleaseSkill.ItSelfCooldown);
                if (CasterObject is PlayerObject playerObj)
                {
                    playerObj.Enqueue(new SyncSkillCountPacket
                    {
                        SkillId = switchSkill.BindingLevelId,
                        SkillCount = SkillData.RemainingTimeLeft.V,
                        技能冷却 = switchReleaseSkill.ItSelfCooldown
                    });
                }
            }
        }*/

        Nodes = new SortedDictionary<int, SkillTask>(gskill.Nodes);
        if (Nodes.Count > 0)
        {
            Caster.ActiveSkills.Add(this);
            RunTime = ReleaseTime.AddMilliseconds(FlyTime + Nodes.First().Key);
        }
    }

    public void Process()
    {
        if ((RunTime - ProcessTime).TotalMilliseconds > 5.0 && SEngine.CurrentTime < RunTime)
            return;

        FirstNode = Nodes.First();
        Nodes.Remove(FirstNode.Key);
        SkillTask task = FirstNode.Value;
        ProcessTime = RunTime;


        if (task != null)
        {
            switch (task)
            {
                case A_00_TriggerSubSkills skill:
                    ProcessA_00_TriggerSubSkills(skill);
                    break;
                case A_01_TriggerObjectBuff skill:
                    ProcessA_01_TriggerObjectBuff(skill);
                    break;
                case A_02_TriggerTrapSkills skill:
                    ProcessA_02_TriggerTrapSkills(skill);
                    break;

                case B_00_SkillSwitchNotification skill:
                    ProcessB_00_SkillSwitchNotification(skill);
                    break;
                case B_01_SkillReleaseNotification skill:
                    ProcessB_01_SkillReleaseNotification(skill);
                    break;
                case B_02_SkillHitNotification skill:
                    ProcessB_02_SkillHitNotification(skill);
                    break;
                case B_03_FrontSwingEndNotification skill:
                    ProcessB_03_FrontSwingEndNotification(skill);
                    break;
                case B_04_BackSwingEndNotification skill:
                    ProcessB_04_BackSwingEndNotification(skill);
                    break;

                case C_00_CalculateSkillAnchor skill:
                    ProcessC_00_CalculateSkillAnchor(skill);
                    break;
                case C_01_CalculateHitTarget skill:
                    ProcessC_01_CalculateHitTarget(skill);
                    break;
                case C_02_CalculateTargetDamage skill:
                    ProcessC_02_CalculateTargetDamage(skill);
                    break;
                case C_03_CalculateObjectDisplacement skill:
                    ProcessC_03_CalculateObjectDisplacement(skill);
                    break;
                case C_04_CalculateTargetTemptation skill:
                    ProcessC_04_CalculateTargetTemptation(skill);
                    break;
                case C_05_CalculateTargetReply skill:
                    ProcessC_05_CalculateTargetReply(skill);
                    break;
                case C_06_CalculatePetSummoning skill:
                    ProcessC_06_CalculatePetSummoning(skill);
                    break;
                case C_07_CalculateTargetTeleportation skill:
                    ProcessC_07_CalculateTargetTeleportation(skill);
                    break;
            }
        }

        if (Nodes.Count == 0)
        {
            Caster.ActiveSkills.Remove(this);
            return;
        }

        RunTime = ReleaseTime.AddMilliseconds(FlyTime + Nodes.First().Key);
        Process();
    }

    private void ProcessA_00_TriggerSubSkills(A_00_TriggerSubSkills task)
    {
        if (GameSkill.DataSheet.TryGetValue(task.触发技能名字, out var value2))
        {
            bool flag = true;
            if (task.CalculateTriggerProbability)
            {
                flag = ((!task.CalculateLuckyProbability) ? Compute.CalculateProbability(task.技能触发概率 + ((task.增加概率Buff == 0 || !Caster.Buffs.ContainsKey(task.增加概率Buff)) ? 0f : task.Buff增加系数)) : Compute.CalculateProbability(Compute.CalcLuck(Caster[Stat.Luck])));
            }
            if (flag && task.验证自身Buff)
            {
                if (!Caster.Buffs.ContainsKey(task.自身Buff编号))
                {
                    flag = false;
                }
                else if (task.触发成功移除)
                {
                    Caster.移除Buff时处理(task.自身Buff编号);
                }
            }
            if (flag && task.验证铭文技能 && Caster is PlayerObject 玩家实例)
            {
                int num = (int)task.所需铭文编号 / 10;
                int num2 = (int)task.所需铭文编号 % 10;
                flag = 玩家实例.Skills.TryGetValue((ushort)num, out var v) && (task.同组铭文无效 ? (num2 == v.InscriptionID) : (num2 == 0 || num2 == v.InscriptionID));
            }
            if (flag)
            {
                switch (task.技能触发方式)
                {
                    case SkillTriggerMethod.OriginAbsolutePosition:
                        new SkillObject(Caster, value2, Skill, ActionID, CurrentMap, CastLocation, Target, CastLocation, this);
                        break;
                    case SkillTriggerMethod.AnchorAbsolutePosition:
                        new SkillObject(Caster, value2, Skill, ActionID, CurrentMap, CastLocation, Target, TargetLocation, this);
                        break;
                    case SkillTriggerMethod.AssassinationAbsolutePosition:
                        new SkillObject(Caster, value2, Skill, ActionID, CurrentMap, CastLocation, Target, Compute.GetFrontPosition(CastLocation, TargetLocation, 2), this);
                        break;
                    case SkillTriggerMethod.TargetHitDefinitely:
                        foreach (KeyValuePair<int, HitInfo> item in HitList)
                        {
                            if ((item.Value.SkillFeedback & SkillHitFeedback.Miss) == 0 && (item.Value.SkillFeedback & SkillHitFeedback.Lose) == 0)
                            {
                                new SkillObject(Caster, value2, Skill, ActionID, CurrentMap, (ParentSkill == null) ? CastLocation : TargetLocation, item.Value.Target, item.Value.Target.CurrentPosition, this);
                            }
                        }
                        break;
                    case SkillTriggerMethod.MonsterDeathDefinitely:
                        foreach (KeyValuePair<int, HitInfo> item2 in HitList)
                        {
                            if (item2.Value.Target is MonsterObject && (item2.Value.SkillFeedback & SkillHitFeedback.Death) != 0)
                            {
                                new SkillObject(Caster, value2, Skill, ActionID, CurrentMap, CastLocation, null, item2.Value.Target.CurrentPosition, this);
                            }
                        }
                        break;
                    case SkillTriggerMethod.MonsterDeathTransposition:
                        foreach (KeyValuePair<int, HitInfo> item3 in HitList)
                        {
                            if (item3.Value.Target is MonsterObject && (item3.Value.SkillFeedback & SkillHitFeedback.Death) != 0)
                            {
                                new SkillObject(Caster, value2, null, item3.Value.Target.ActionID++, CurrentMap, item3.Value.Target.CurrentPosition, item3.Value.Target, item3.Value.Target.CurrentPosition, this, null, targetBorrow: true);
                            }
                        }
                        break;
                    case SkillTriggerMethod.MonsterHitDefinitely:
                        foreach (KeyValuePair<int, HitInfo> item4 in HitList)
                        {
                            if (item4.Value.Target is MonsterObject && (item4.Value.SkillFeedback & SkillHitFeedback.Lose) == 0)
                            {
                                new SkillObject(Caster, value2, Skill, ActionID, CurrentMap, (ParentSkill == null) ? CastLocation : TargetLocation, item4.Value.Target, item4.Value.Target.CurrentPosition, this);
                            }
                        }
                        break;
                    case SkillTriggerMethod.NoTargetPosition:
                        if (HitList.Count == 0 || HitList.Values.FirstOrDefault((HitInfo O) => O.SkillFeedback != SkillHitFeedback.Lose) == null)
                        {
                            new SkillObject(Caster, value2, Skill, ActionID, CurrentMap, CastLocation, null, TargetLocation, this);
                        }
                        break;
                    case SkillTriggerMethod.TargetPositionAbsolute:
                        foreach (KeyValuePair<int, HitInfo> item5 in HitList)
                        {
                            new SkillObject(Caster, value2, Skill, ActionID, CurrentMap, CastLocation, item5.Value.Target, item5.Value.Target.CurrentPosition, this);
                        }
                        break;
                    case SkillTriggerMethod.ForehandAndBackhandRandom:
                        {
                            if (Compute.CalculateProbability(0.5f) && GameSkill.DataSheet.TryGetValue(task.反手技能名字, out var value3))
                            {
                                new SkillObject(Caster, value3, Skill, ActionID, CurrentMap, CastLocation, null, TargetLocation, this);
                            }
                            else
                            {
                                new SkillObject(Caster, value2, Skill, ActionID, CurrentMap, CastLocation, null, TargetLocation, this);
                            }
                            break;
                        }
                    case SkillTriggerMethod.TargetDeathDefinitely:
                        foreach (KeyValuePair<int, HitInfo> item6 in HitList)
                        {
                            if ((item6.Value.SkillFeedback & SkillHitFeedback.Death) != 0)
                            {
                                new SkillObject(Caster, value2, Skill, ActionID, CurrentMap, CastLocation, null, item6.Value.Target.CurrentPosition, this);
                            }
                        }
                        break;
                    case SkillTriggerMethod.TargetMissDefinitely:
                        foreach (KeyValuePair<int, HitInfo> item7 in HitList)
                        {
                            if ((item7.Value.SkillFeedback & SkillHitFeedback.Miss) != 0)
                            {
                                new SkillObject(Caster, value2, Skill, ActionID, CurrentMap, CastLocation, null, item7.Value.Target.CurrentPosition, this);
                            }
                        }
                        break;
                }
            }
        }
    }

    private void ProcessA_01_TriggerObjectBuff(A_01_TriggerObjectBuff task)
    {
        bool flag2 = false;
        if (task.角色自身添加)
        {
            bool flag3 = true;
            float buff触发概率 = task.Buff触发概率;
            if (!Compute.CalculateProbability(buff触发概率))
            {
                flag3 = false;
            }
            if (flag3 && task.验证铭文技能 && Caster is PlayerObject 玩家实例2)
            {
                int num3 = (int)task.所需铭文编号 / 10;
                int num4 = (int)task.所需铭文编号 % 10;
                flag3 = 玩家实例2.Skills.TryGetValue((ushort)num3, out var v2) && (task.同组铭文无效 ? (num4 == v2.InscriptionID) : (num4 == 0 || num4 == v2.InscriptionID));
            }
            if (flag3 && task.验证自身Buff)
            {
                if (!Caster.Buffs.ContainsKey(task.自身Buff编号))
                {
                    flag3 = false;
                }
                else
                {
                    if (task.触发成功移除)
                    {
                        Caster.移除Buff时处理(task.自身Buff编号);
                    }
                    if (task.移除伴生Buff)
                    {
                        Caster.移除Buff时处理(task.移除伴生编号);
                    }
                }
            }
            if (flag3 && task.验证分组Buff && Caster.Buffs.Values.FirstOrDefault((BuffInfo O) => O.Buff分组 == task.BuffGroupID) == null)
            {
                flag3 = false;
            }
            if (flag3 && task.VerifyTargetBuff && HitList.Values.FirstOrDefault((HitInfo O) => (O.SkillFeedback & SkillHitFeedback.Miss) == 0 && (O.SkillFeedback & SkillHitFeedback.Lose) == 0 && O.Target.Buffs.TryGetValue(task.目标Buff编号, out var v10) && v10.当前层数.V >= task.所需Buff层数) == null)
            {
                flag3 = false;
            }
            if (flag3 && task.VerifyTargetType && HitList.Values.FirstOrDefault((HitInfo O) => (O.SkillFeedback & SkillHitFeedback.Miss) == 0 && (O.SkillFeedback & SkillHitFeedback.Lose) == 0 && O.Target.IsValidTarget(Caster, task.所需目标类型)) == null)
            {
                flag3 = false;
            }
            if (flag3)
            {
                Caster.AddBuff(task.触发Buff编号, Caster);
                if (task.伴生Buff编号 > 0)
                {
                    Caster.AddBuff(task.伴生Buff编号, Caster);
                }
                flag2 = true;
            }
        }
        else
        {
            bool flag4 = true;
            if (task.验证自身Buff)
            {
                if (!Caster.Buffs.ContainsKey(task.自身Buff编号))
                {
                    flag4 = false;
                }
                else
                {
                    if (task.触发成功移除)
                    {
                        Caster.移除Buff时处理(task.自身Buff编号);
                    }
                    if (task.移除伴生Buff)
                    {
                        Caster.移除Buff时处理(task.移除伴生编号);
                    }
                }
            }
            if (flag4 && task.验证分组Buff && Caster.Buffs.Values.FirstOrDefault((BuffInfo O) => O.Buff分组 == task.BuffGroupID) == null)
            {
                flag4 = false;
            }
            if (flag4 && task.验证铭文技能 && Caster is PlayerObject 玩家实例3)
            {
                int num5 = (int)task.所需铭文编号 / 10;
                int num6 = (int)task.所需铭文编号 % 10;
                flag4 = 玩家实例3.Skills.TryGetValue((ushort)num5, out var v3) && (task.同组铭文无效 ? (num6 == v3.InscriptionID) : (num6 == 0 || num6 == v3.InscriptionID));
            }
            if (flag4)
            {
                foreach (KeyValuePair<int, HitInfo> item8 in HitList)
                {
                    bool flag5 = true;
                    if ((item8.Value.SkillFeedback & (SkillHitFeedback.Miss | SkillHitFeedback.Lose)) != 0)
                    {
                        flag5 = false;
                    }
                    if (flag5 && !Compute.CalculateProbability(task.Buff触发概率))
                    {
                        flag5 = false;
                    }
                    if (flag5 && task.VerifyTargetType && !item8.Value.Target.IsValidTarget(Caster, task.所需目标类型))
                    {
                        flag5 = false;
                    }
                    if (flag5 && task.VerifyTargetBuff)
                    {
                        flag5 = item8.Value.Target.Buffs.TryGetValue(task.目标Buff编号, out var v4) && v4.当前层数.V >= task.所需Buff层数;
                    }
                    if (flag5)
                    {
                        item8.Value.Target.AddBuff(task.触发Buff编号, Caster);
                        if (task.伴生Buff编号 > 0)
                        {
                            item8.Value.Target.AddBuff(task.伴生Buff编号, Caster);
                        }
                        flag2 = true;
                    }
                }
            }
        }
        if (flag2 && task.GainSkillExp)
        {
            (Caster as PlayerObject)?.GainSkillExperience(task.ExpSkillID);
        }
    }

    private void ProcessA_02_TriggerTrapSkills(A_02_TriggerTrapSkills task)
    {
        if (SkillTrap.DataSheet.TryGetValue(task.TriggerTrapSkills, out var 陷阱模板))
        {
            int num7 = 0;
            Point[] array = Compute.CalculateGrid(TargetLocation, Compute.DirectionFromPoint(CastLocation, TargetLocation), task.NumberTrapsTriggered);
            foreach (Point 坐标 in array)
            {
                if (CurrentMap.ValidTerrain(坐标) && (陷阱模板.AllowStacking || CurrentMap[坐标].FirstOrDefault((MapObject O) => O is TrapObject 陷阱实例 && 陷阱实例.GroupID != 0 && 陷阱实例.GroupID == 陷阱模板.GroupID) == null))
                {
                    Caster.Traps.Add(new TrapObject(Caster, 陷阱模板, CurrentMap, 坐标));
                    num7++;
                }
            }
            if (num7 != 0 && task.ExpSkillID != 0)
            {
                (Caster as PlayerObject)?.GainSkillExperience(task.ExpSkillID);
            }
        }
    }

    private void ProcessB_00_SkillSwitchNotification(B_00_SkillSwitchNotification task)
    {
        if (Caster.Buffs.ContainsKey(task.SkillTagID))
        {
            if (task.TagRemovalAllowed)
            {
                Caster.移除Buff时处理(task.SkillTagID);
            }
        }
        else if (GameBuff.DataSheet.ContainsKey(task.SkillTagID))
        {
            Caster.AddBuff(task.SkillTagID, Caster);
        }
    }

    private void ProcessB_01_SkillReleaseNotification(B_01_SkillReleaseNotification task)
    {
        if (task.调整角色朝向)
        {
            GameDirection dir = Compute.DirectionFromPoint(CastLocation, TargetLocation);
            if (dir == Caster.CurrentDirection)
            {
                Caster.SendPacket(new SyncObjectDirectionPacket
                {
                    ObjectID = Caster.ObjectID,
                    Direction = (ushort)dir,
                    ActionTime = ((!(Caster is PlayerObject)) ? ((ushort)1) : ((ushort)0))
                });
            }
            else
            {
                Caster.CurrentDirection = dir;
            }
        }
        if (task.移除技能标记 && Template.SkillTagID != 0)
        {
            Caster.移除Buff时处理(Template.SkillTagID);
        }
        if (task.SelfCooldown != 0 || task.Buff增加冷却)
        {
            if (检查计数 && Caster is PlayerObject 玩家实例4)
            {
                if (--Skill.RemainingCount.V <= 0)
                {
                    Caster.Cooldowns[SkillID | 0x1000000] = ReleaseTime.AddMilliseconds((Skill.计数时间 - SEngine.CurrentTime).TotalMilliseconds);
                }
                玩家实例4.Enqueue(new 同步技能计数
                {
                    技能编号 = Skill.ID.V,
                    技能计数 = Skill.RemainingCount.V,
                    技能冷却 = (int)(Skill.计数时间 - SEngine.CurrentTime).TotalMilliseconds
                });
            }
            else if (task.SelfCooldown > 0 || task.Buff增加冷却)
            {
                int num8 = task.SelfCooldown;
                if (task.Buff增加冷却 && Caster.Buffs.ContainsKey(task.增加冷却Buff))
                {
                    num8 += task.冷却增加时间;
                }
                DateTime dateTime = ReleaseTime.AddMilliseconds(num8);
                DateTime dateTime2 = (Caster.Cooldowns.ContainsKey(SkillID | 0x1000000) ? Caster.Cooldowns[SkillID | 0x1000000] : default(DateTime));
                if (num8 > 0 && dateTime > dateTime2)
                {
                    Caster.Cooldowns[SkillID | 0x1000000] = dateTime;
                    Caster.SendPacket(new 添加技能冷却
                    {
                        冷却编号 = (SkillID | 0x1000000),
                        冷却时间 = num8
                    });
                }
            }
        }
        if (Caster is PlayerObject 玩家实例5 && task.分组冷却时间 != 0 && GroupID != 0)
        {
            DateTime dateTime3 = ReleaseTime.AddMilliseconds(task.分组冷却时间);
            DateTime dateTime4 = (玩家实例5.Cooldowns.ContainsKey(GroupID | 0) ? 玩家实例5.Cooldowns[GroupID | 0] : default(DateTime));
            if (dateTime3 > dateTime4)
            {
                玩家实例5.Cooldowns[GroupID | 0] = dateTime3;
            }
            Caster.SendPacket(new 添加技能冷却
            {
                冷却编号 = (GroupID | 0),
                冷却时间 = task.分组冷却时间
            });
        }
        if (task.角色忙绿时间 != 0)
        {
            Caster.BusyTime = ReleaseTime.AddMilliseconds(task.角色忙绿时间);
        }
        if (task.发送释放通知)
        {
            Caster.SendPacket(new 开始释放技能
            {
                ObjectID = ((!TargetBorrow || Target == null) ? Caster.ObjectID : Target.ObjectID),
                SkillID = SkillID,
                SkillLevel = SkillLevel,
                InscriptionID = InscriptionID,
                TargetLocation = TargetLocation,
                ActionID = ActionID,
                TargetID = (Target?.ObjectID ?? 0),
                TargetHeight = CurrentMap.GetTerrainHeight(TargetLocation)
            });
        }
    }

    private void ProcessB_02_SkillHitNotification(B_02_SkillHitNotification task)
    {
        if (task.命中扩展通知)
        {
            Caster.SendPacket(new 触发技能扩展
            {
                ObjectID = ((!TargetBorrow || Target == null) ? Caster.ObjectID : Target.ObjectID),
                SkillID = SkillID,
                SkillLevel = SkillLevel,
                InscriptionID = InscriptionID,
                ActionID = ActionID,
                HitDescription = HitInfo.命中描述(HitList, FlyTime)
            });
        }
        else
        {
            Caster.SendPacket(new 触发技能正常
            {
                ObjectID = ((!TargetBorrow || Target == null) ? Caster.ObjectID : Target.ObjectID),
                SkillID = SkillID,
                SkillLevel = SkillLevel,
                InscriptionID = InscriptionID,
                ActionID = ActionID,
                HitDescription = HitInfo.命中描述(HitList, FlyTime)
            });
        }
        if (task.计算飞行耗时)
        {
            FlyTime = Compute.GetDistance(CastLocation, TargetLocation) * task.单格飞行耗时;
        }
    }

    private void ProcessB_03_FrontSwingEndNotification(B_03_FrontSwingEndNotification task)
    {
        if (task.计算攻速缩减)
        {
            AttackSpeedDecrease = Compute.Clamp(Compute.CalcAttackSpeed(-5), AttackSpeedDecrease + Compute.CalcAttackSpeed(Caster[Stat.AttackSpeed]), Compute.CalcAttackSpeed(5));
            if (AttackSpeedDecrease != 0)
            {
                foreach (var node in Nodes)
                {
                    if (node.Value is B_04_BackSwingEndNotification)
                    {
                        int j;
                        for (j = node.Key - AttackSpeedDecrease; Nodes.ContainsKey(j); j++)
                        {
                        }
                        Nodes.Remove(node.Key);
                        Nodes.Add(j, node.Value);
                        break;
                    }
                }
            }
        }
        if (task.禁止行走时间 != 0)
        {
            Caster.WalkTime = ReleaseTime.AddMilliseconds(task.禁止行走时间);
        }
        if (task.禁止奔跑时间 != 0)
        {
            Caster.RunTime = ReleaseTime.AddMilliseconds(task.禁止奔跑时间);
        }
        if (task.角色硬直时间 != 0)
        {
            Caster.HardStunTime = ReleaseTime.AddMilliseconds(task.计算攻速缩减 ? (task.角色硬直时间 - AttackSpeedDecrease) : task.角色硬直时间);
        }
        if (task.发送结束通知)
        {
            Caster.SendPacket(new 触发技能正常
            {
                ObjectID = ((!TargetBorrow || Target == null) ? Caster.ObjectID : Target.ObjectID),
                SkillID = SkillID,
                SkillLevel = SkillLevel,
                InscriptionID = InscriptionID,
                ActionID = ActionID
            });
        }
        if (task.解除技能陷阱)
        {
            (Caster as TrapObject)?.Disappear();
        }
    }

    private void ProcessB_04_BackSwingEndNotification(B_04_BackSwingEndNotification task)
    {
        Caster.SendPacket(new 技能释放完成
        {
            SkillID = SkillID,
            ActionID = ActionID
        });
        if (task.后摇结束死亡)
        {
            Caster.Die(null, false);
        }
    }

    private void ProcessC_00_CalculateSkillAnchor(C_00_CalculateSkillAnchor task)
    {
        if (task.计算当前位置)
        {
            Target = null;
            if (task.计算当前方向)
            {
                TargetLocation = Compute.GetNextPosition(Caster.CurrentPosition, Caster.CurrentDirection, task.技能最近距离);
            }
            else
            {
                TargetLocation = Compute.GetFrontPosition(Caster.CurrentPosition, TargetLocation, task.技能最近距离);
            }
        }
        else if (Compute.GetDistance(CastLocation, TargetLocation) > task.MaxDistance)
        {
            Target = null;
            TargetLocation = Compute.GetFrontPosition(CastLocation, TargetLocation, task.MaxDistance);
        }
        else if (Compute.GetDistance(CastLocation, TargetLocation) < task.技能最近距离)
        {
            Target = null;
            if (CastLocation == TargetLocation)
            {
                TargetLocation = Compute.GetNextPosition(CastLocation, Caster.CurrentDirection, task.技能最近距离);
            }
            else
            {
                TargetLocation = Compute.GetFrontPosition(CastLocation, TargetLocation, task.技能最近距离);
            }
        }
    }

    private void ProcessC_01_CalculateHitTarget(C_01_CalculateHitTarget task)
    {
        if (task.清空命中列表)
        {
            HitList = new Dictionary<int, HitInfo>();
        }
        if (task.技能能否穿墙 || !CurrentMap.IsTerrainBlocked(CastLocation, TargetLocation))
        {
            switch (task.技能锁定方式)
            {
                case SkillLockType.LockSelf:
                    Caster.被技能命中处理(this, task);
                    break;
                case SkillLockType.LockOnTarget:
                    Target?.被技能命中处理(this, task);
                    break;
                case SkillLockType.LockOnPosition:
                    {
                        var grid = Compute.CalculateGrid(Caster.CurrentPosition, Compute.DirectionFromPoint(CastLocation, TargetLocation), task.技能范围类型);
                        foreach (var point in grid)
                        {
                            foreach (var obj in CurrentMap[point])
                            {
                                obj.被技能命中处理(this, task);
                            }
                        }
                        break;
                    }
                case SkillLockType.LockOnTargetPosition:
                    {
                        var grid = Compute.CalculateGrid(Target?.CurrentPosition ?? TargetLocation, Compute.DirectionFromPoint(CastLocation, TargetLocation), task.技能范围类型);
                        foreach (var point in grid)
                        {
                            foreach (var obj in CurrentMap[point])
                            {
                                obj.被技能命中处理(this, task);
                            }
                        }
                        break;
                    }
                case SkillLockType.LockAnchorPosition:
                    {
                        var grid = Compute.CalculateGrid(TargetLocation, Compute.DirectionFromPoint(CastLocation, TargetLocation), task.技能范围类型);
                        foreach (var point in grid)
                        {
                            foreach (var obj in CurrentMap[point])
                            {
                                obj.被技能命中处理(this, task);
                            }
                        }
                        break;
                    }
                case SkillLockType.EmptyLockSelf:
                    {
                        var grid = Compute.CalculateGrid(TargetLocation, Compute.DirectionFromPoint(CastLocation, TargetLocation), task.技能范围类型);
                        foreach (var point in grid)
                        {
                            foreach (var obj in CurrentMap[point])
                            {
                                obj.被技能命中处理(this, task);
                            }
                        }
                        if (HitList.Count == 0)
                        {
                            Caster.被技能命中处理(this, task);
                        }
                        break;
                    }
            }
        }
        if (HitList.Count == 0 && task.放空结束技能)
        {
            if (task.发送中断通知)
            {
                Caster.SendPacket(new 技能释放中断
                {
                    ObjectID = Caster.ObjectID,
                    SkillID = SkillID,
                    SkillLevel = SkillLevel,
                    InscriptionID = InscriptionID,
                    ActionID = ActionID,
                    SegmentID = SegmentID
                });
            }
            Caster.ActiveSkills.Remove(this);
            return;
        }
        if (task.补发释放通知)
        {
            Caster.SendPacket(new 开始释放技能
            {
                ObjectID = ((!TargetBorrow || Target == null) ? Caster.ObjectID : Target.ObjectID),
                SkillID = SkillID,
                SkillLevel = SkillLevel,
                InscriptionID = InscriptionID,
                TargetID = (Target?.ObjectID ?? 0),
                TargetLocation = TargetLocation,
                TargetHeight = CurrentMap.GetTerrainHeight(TargetLocation),
                ActionID = ActionID
            });
        }
        if (HitList.Count != 0 && task.攻速提升类型 != 0 && HitList[0].Target.IsValidTarget(Caster, task.攻速提升类型))
        {
            AttackSpeedDecrease = Compute.Clamp(Compute.CalcAttackSpeed(-5), AttackSpeedDecrease + Compute.CalcAttackSpeed(task.攻速提升幅度), Compute.CalcAttackSpeed(5));
        }
        if (task.清除目标状态 && task.清除状态列表.Count != 0)
        {
            foreach (KeyValuePair<int, HitInfo> item14 in HitList)
            {
                if ((item14.Value.SkillFeedback & SkillHitFeedback.Miss) != 0 || (item14.Value.SkillFeedback & SkillHitFeedback.Lose) != 0)
                {
                    continue;
                }
                foreach (ushort item15 in task.清除状态列表.ToList())
                {
                    item14.Value.Target.移除Buff时处理(item15);
                }
            }
        }
        if (task.触发被动技能 && HitList.Count != 0 && Compute.CalculateProbability(task.触发被动概率))
        {
            Caster[Stat.SkillSign] = 1;
        }
        if (task.GainSkillExp && HitList.Count != 0)
        {
            (Caster as PlayerObject).GainSkillExperience(task.ExpSkillID);
        }
        if (task.计算飞行耗时 && task.单格飞行耗时 != 0)
        {
            FlyTime = Compute.GetDistance(CastLocation, TargetLocation) * task.单格飞行耗时;
        }
        if (task.技能命中通知)
        {
            Caster.SendPacket(new 触发技能正常
            {
                ObjectID = ((!TargetBorrow || Target == null) ? Caster.ObjectID : Target.ObjectID),
                SkillID = SkillID,
                SkillLevel = SkillLevel,
                InscriptionID = InscriptionID,
                ActionID = ActionID,
                HitDescription = HitInfo.命中描述(HitList, FlyTime)
            });
        }
        if (task.技能扩展通知)
        {
            Caster.SendPacket(new 触发技能扩展
            {
                ObjectID = ((!TargetBorrow || Target == null) ? Caster.ObjectID : Target.ObjectID),
                SkillID = SkillID,
                SkillLevel = SkillLevel,
                InscriptionID = InscriptionID,
                ActionID = ActionID,
                HitDescription = HitInfo.命中描述(HitList, FlyTime)
            });
        }
    }

    private void ProcessC_02_CalculateTargetDamage(C_02_CalculateTargetDamage task)
    {
        float num9 = 1f;
        foreach (var hitter in HitList)
        {
            var flag = false;
            if (task.点爆标记编号 != null)
            {
                foreach (var value in task.点爆标记编号)
                {
                    if (task.点爆命中目标 && hitter.Value.Target.Buffs.ContainsKey(value))
                    {
                        hitter.Value.Target.移除Buff时处理(value);
                    }
                    else if (task.点爆命中目标 && task.失败添加层数)
                    {
                        hitter.Value.Target.AddBuff(value, Caster);
                        flag = true;
                    }
                }
            }

            if (flag) continue;

            hitter.Value.Target.被动受伤时处理(this, task, hitter.Value, num9);
            if ((hitter.Value.SkillFeedback & SkillHitFeedback.Lose) == 0)
            {
                if (task.数量衰减伤害)
                {
                    num9 = Math.Max(task.伤害衰减下限, num9 - task.伤害衰减系数);
                }
                Caster.SendPacket(new 触发命中特效
                {
                    ObjectID = ((!TargetBorrow || Target == null) ? Caster.ObjectID : Target.ObjectID),
                    SkillID = SkillID,
                    SkillLevel = SkillLevel,
                    InscriptionID = InscriptionID,
                    ActionID = ActionID,
                    TargetID = hitter.Value.Target.ObjectID,
                    SkillFeedback = (ushort)hitter.Value.SkillFeedback,
                    SkillDamage = -hitter.Value.SkillDamage,
                    ParryDamage = hitter.Value.ParryDamage
                });
            }
        }
        if (task.目标死亡回复)
        {
            foreach (var hitter in HitList)
            {
                if ((hitter.Value.SkillFeedback & SkillHitFeedback.Death) != 0 && hitter.Value.Target.IsValidTarget(Caster, task.回复限定类型))
                {
                    int amount = task.HealthRecoveryBase;
                    if (task.等级差减回复)
                    {
                        int 数值 = Caster.CurrentLevel - hitter.Value.Target.CurrentLevel - task.减回复等级差;
                        int num11 = task.零回复等级差 - task.减回复等级差;
                        float num12 = (float)Compute.Clamp(0, 数值, num11) / (float)num11;
                        amount = (int)((float)amount - (float)amount * num12);
                    }
                    if (amount > 0)
                    {
                        Caster.CurrentHP += amount;
                        Caster.SendPacket(new IncreaseHealthIndicatorPacket
                        {
                            HealthAmount = amount,
                            ObjectID = Caster.ObjectID
                        });
                    }
                }
            }
        }
        if (task.击杀减少冷却)
        {
            int num13 = 0;
            foreach (KeyValuePair<int, HitInfo> item18 in HitList)
            {
                if ((item18.Value.SkillFeedback & SkillHitFeedback.Death) != 0 && item18.Value.Target.IsValidTarget(Caster, task.冷却减少类型))
                {
                    num13 += task.冷却减少时间;
                }
            }
            if (num13 > 0)
            {
                if (Caster.Cooldowns.TryGetValue(task.冷却减少技能 | 0x1000000, out var v5))
                {
                    v5 -= TimeSpan.FromMilliseconds(num13);
                    Caster.Cooldowns[task.冷却减少技能 | 0x1000000] = v5;
                    Caster.SendPacket(new 添加技能冷却
                    {
                        冷却编号 = (task.冷却减少技能 | 0x1000000),
                        冷却时间 = Math.Max(0, (int)(v5 - SEngine.CurrentTime).TotalMilliseconds)
                    });
                }
                if (task.冷却减少分组 != 0 && Caster is PlayerObject 玩家实例6 && 玩家实例6.Cooldowns.TryGetValue(task.冷却减少分组 | 0, out var v6))
                {
                    v6 -= TimeSpan.FromMilliseconds(num13);
                    玩家实例6.Cooldowns[task.冷却减少分组 | 0] = v6;
                    Caster.SendPacket(new 添加技能冷却
                    {
                        冷却编号 = (task.冷却减少分组 | 0),
                        冷却时间 = Math.Max(0, (int)(v6 - SEngine.CurrentTime).TotalMilliseconds)
                    });
                }
            }
        }
        if (task.命中减少冷却)
        {
            int num14 = 0;
            foreach (KeyValuePair<int, HitInfo> item19 in HitList)
            {
                if ((item19.Value.SkillFeedback & SkillHitFeedback.Miss) == 0 && (item19.Value.SkillFeedback & SkillHitFeedback.Lose) == 0 && item19.Value.Target.IsValidTarget(Caster, task.冷却减少类型))
                {
                    num14 += task.冷却减少时间;
                }
            }
            if (num14 > 0)
            {
                if (Caster.Cooldowns.TryGetValue(task.冷却减少技能 | 0x1000000, out var v7))
                {
                    v7 -= TimeSpan.FromMilliseconds(num14);
                    Caster.Cooldowns[task.冷却减少技能 | 0x1000000] = v7;
                    Caster.SendPacket(new 添加技能冷却
                    {
                        冷却编号 = (task.冷却减少技能 | 0x1000000),
                        冷却时间 = Math.Max(0, (int)(v7 - SEngine.CurrentTime).TotalMilliseconds)
                    });
                }
                if (task.冷却减少分组 != 0 && Caster is PlayerObject 玩家实例7 && 玩家实例7.Cooldowns.TryGetValue(task.冷却减少分组 | 0, out var v8))
                {
                    v8 -= TimeSpan.FromMilliseconds(num14);
                    玩家实例7.Cooldowns[task.冷却减少分组 | 0] = v8;
                    Caster.SendPacket(new 添加技能冷却
                    {
                        冷却编号 = (task.冷却减少分组 | 0),
                        冷却时间 = Math.Max(0, (int)(v8 - SEngine.CurrentTime).TotalMilliseconds)
                    });
                }
            }
        }
        if (task.目标硬直时间 > 0)
        {
            foreach (KeyValuePair<int, HitInfo> item20 in HitList)
            {
                if ((item20.Value.SkillFeedback & SkillHitFeedback.Miss) == 0 && (item20.Value.SkillFeedback & SkillHitFeedback.Lose) == 0 && item20.Value.Target is MonsterObject 怪物实例 && 怪物实例.Grade != MonsterGradeType.Boss)
                {
                    item20.Value.Target.HardStunTime = SEngine.CurrentTime.AddMilliseconds(task.目标硬直时间);
                }
            }
        }
        if (task.清除目标状态 && task.清除状态列表.Count != 0)
        {
            foreach (KeyValuePair<int, HitInfo> item21 in HitList)
            {
                if ((item21.Value.SkillFeedback & SkillHitFeedback.Miss) != 0 || (item21.Value.SkillFeedback & SkillHitFeedback.Lose) != 0)
                {
                    continue;
                }
                foreach (ushort item22 in task.清除状态列表)
                {
                    item21.Value.Target.移除Buff时处理(item22);
                }
            }
        }
        if (task.GainSkillExp && HitList.Count != 0)
        {
            (Caster as PlayerObject).GainSkillExperience(task.ExpSkillID);
        }
        if (task.扣除武器持久 && HitList.Count != 0)
        {
            if (Caster is PlayerObject player)
            {
                player.DamageWeapon(SEngine.Random.Next(1, 6));
            }
        }
    }

    private void ProcessC_03_CalculateObjectDisplacement(C_03_CalculateObjectDisplacement task)
    {
        byte[] 自身位移次数 = task.自身位移次数;
        byte b = (byte)((((自身位移次数 != null) ? 自身位移次数.Length : 0) > SkillLevel) ? task.自身位移次数[SkillLevel] : 0);
        if (task.角色自身位移 && (CurrentMap != Caster.CurrentMap || SegmentID >= b))
        {
            Caster.SendPacket(new 技能释放中断
            {
                ObjectID = ((!TargetBorrow || Target == null) ? Caster.ObjectID : Target.ObjectID),
                SkillID = SkillID,
                SkillLevel = SkillLevel,
                InscriptionID = InscriptionID,
                ActionID = ActionID,
                SegmentID = SegmentID
            });
            Caster.SendPacket(new 技能释放完成
            {
                SkillID = SkillID,
                ActionID = ActionID
            });
        }
        else if (task.角色自身位移)
        {
            int 数量 = (task.推动目标位移 ? task.连续推动数量 : 0);
            byte[] 自身位移距离 = task.自身位移距离;
            int num15 = ((((自身位移距离 != null) ? 自身位移距离.Length : 0) > SkillLevel) ? task.自身位移距离[SkillLevel] : 0);
            int distance = ((task.允许超出锚点 || task.锚点反向位移) ? num15 : Math.Min(num15, Compute.GetDistance(CastLocation, TargetLocation)));
            Point location = (task.锚点反向位移 ? Compute.GetNextPosition(Caster.CurrentPosition, Compute.DirectionFromPoint(TargetLocation, Caster.CurrentPosition), distance) : TargetLocation);
            if (Caster.CanRush(Caster, location, distance, 数量, task.能否穿越障碍, out var 终点, out var targets))
            {
                foreach (MapObject obj in targets)
                {
                    if (task.目标位移编号 != 0 && Compute.CalculateProbability(task.位移Buff概率))
                    {
                        obj.AddBuff(task.目标位移编号, Caster);
                    }
                    if (task.目标附加编号 != 0 && Compute.CalculateProbability(task.附加Buff概率) && obj.IsValidTarget(Caster, task.限定附加类型))
                    {
                        obj.AddBuff(task.目标附加编号, Caster);
                    }
                    obj.CurrentDirection = Compute.DirectionFromPoint(obj.CurrentPosition, Caster.CurrentPosition);
                    Point point = Compute.GetNextPosition(obj.CurrentPosition, Compute.DirectionFromPoint(Caster.CurrentPosition, obj.CurrentPosition), 1);
                    obj.BusyTime = SEngine.CurrentTime.AddMilliseconds(task.目标位移耗时 * 60);
                    obj.HardStunTime = SEngine.CurrentTime.AddMilliseconds(task.目标位移耗时 * 60 + task.目标硬直时间);
                    obj.SendPacket(new 对象被动位移
                    {
                        Position = point,
                        ObjectID = obj.ObjectID,
                        位移朝向 = (ushort)obj.CurrentDirection,
                        位移速度 = task.目标位移耗时
                    });
                    obj.OnLocationChanged(point);
                    if (task.推动增加经验 && !CanGainExperience)
                    {
                        (Caster as PlayerObject)?.GainSkillExperience(SkillID);
                        CanGainExperience = true;
                    }
                }
                if (task.成功Buff编号 != 0 && Compute.CalculateProbability(task.成功Buff概率))
                {
                    Caster.AddBuff(task.成功Buff编号, Caster);
                }
                Caster.CurrentDirection = Compute.DirectionFromPoint(Caster.CurrentPosition, 终点);
                int num18 = task.自身位移耗时 * Caster.GetDistance(终点);
                Caster.BusyTime = SEngine.CurrentTime.AddMilliseconds(num18 * 60);
                Caster.SendPacket(new 对象被动位移
                {
                    Position = 终点,
                    ObjectID = Caster.ObjectID,
                    位移朝向 = (ushort)Caster.CurrentDirection,
                    位移速度 = (ushort)num18
                });
                Caster.OnLocationChanged(终点);
                if (Caster is PlayerObject player && task.DisplacementIncreaseExp && !CanGainExperience)
                {
                    player.GainSkillExperience(SkillID);
                    CanGainExperience = true;
                }
                if (task.多段位移通知)
                {
                    Caster.SendPacket(new 触发技能正常
                    {
                        ObjectID = ((!TargetBorrow || Target == null) ? Caster.ObjectID : Target.ObjectID),
                        SkillID = SkillID,
                        SkillLevel = SkillLevel,
                        InscriptionID = InscriptionID,
                        ActionID = ActionID,
                        SegmentID = SegmentID
                    });
                }
                if (b > 1)
                {
                    TargetLocation = Compute.GetNextPosition(Caster.CurrentPosition, Caster.CurrentDirection, distance);
                }
                SegmentID++;
            }
            else
            {
                if (Compute.CalculateProbability(task.失败Buff概率))
                {
                    Caster.AddBuff(task.失败Buff编号, Caster);
                }
                Caster.HardStunTime = SEngine.CurrentTime.AddMilliseconds((int)task.自身硬直时间);
                SegmentID = b;
            }
            if (b > 1)
            {
                int num19;
                for (num19 = FirstNode.Key + task.自身位移耗时 * 60; Nodes.ContainsKey(num19); num19++)
                {
                }
                Nodes.Add(num19, FirstNode.Value);
            }
        }
        else if (task.推动目标位移)
        {
            foreach (var hitter in HitList)
            {
                if ((hitter.Value.SkillFeedback & SkillHitFeedback.Miss) != 0 ||
                    (hitter.Value.SkillFeedback & SkillHitFeedback.Lose) != 0 || 
                    (hitter.Value.SkillFeedback & SkillHitFeedback.Death) != 0 || 
                    !Compute.CalculateProbability(task.推动目标概率) || 
                    !hitter.Value.Target.IsValidTarget(Caster, task.推动目标类型))
                {
                    continue;
                }
                byte[] 目标位移距离 = task.目标位移距离;
                int val = ((((目标位移距离 != null) ? 目标位移距离.Length : 0) > SkillLevel) ? task.目标位移距离[SkillLevel] : 0);
                int num20 = Compute.GetDistance(Caster.CurrentPosition, hitter.Value.Target.CurrentPosition);
                int distance = Math.Max(0, Math.Min(8 - num20, val));
                if (distance == 0)
                {
                    continue;
                }
                GameDirection dir = Compute.DirectionFromPoint(Caster.CurrentPosition, hitter.Value.Target.CurrentPosition);
                Point location = Compute.GetNextPosition(hitter.Value.Target.CurrentPosition, dir, distance);
                if (hitter.Value.Target.CanRush(Caster, location, distance, 0, false, out var 终点2, out var _))
                {
                    if (Compute.CalculateProbability(task.位移Buff概率))
                    {
                        hitter.Value.Target.AddBuff(task.目标位移编号, Caster);
                    }
                    if (Compute.CalculateProbability(task.附加Buff概率) && hitter.Value.Target.IsValidTarget(Caster, task.限定附加类型))
                    {
                        hitter.Value.Target.AddBuff(task.目标附加编号, Caster);
                    }
                    hitter.Value.Target.CurrentDirection = Compute.DirectionFromPoint(hitter.Value.Target.CurrentPosition, Caster.CurrentPosition);
                    ushort num22 = (ushort)(Compute.GetDistance(hitter.Value.Target.CurrentPosition, 终点2) * task.目标位移耗时);
                    hitter.Value.Target.BusyTime = SEngine.CurrentTime.AddMilliseconds(num22 * 60);
                    hitter.Value.Target.HardStunTime = SEngine.CurrentTime.AddMilliseconds(num22 * 60 + task.目标硬直时间);
                    hitter.Value.Target.SendPacket(new 对象被动位移
                    {
                        Position = 终点2,
                        位移速度 = num22,
                        ObjectID = hitter.Value.Target.ObjectID,
                        位移朝向 = (ushort)hitter.Value.Target.CurrentDirection
                    });
                    hitter.Value.Target.OnLocationChanged(终点2);
                    if (task.推动增加经验 && !CanGainExperience)
                    {
                        (Caster as PlayerObject)?.GainSkillExperience(SkillID);
                        CanGainExperience = true;
                    }
                }
            }
        }
    }

    private void ProcessC_04_CalculateTargetTemptation(C_04_CalculateTargetTemptation task)
    {
        foreach (var kvp in HitList)
        {
            (Caster as PlayerObject).玩家诱惑目标(this, task, kvp.Value.Target);
        }
    }

    private void ProcessC_05_CalculateTargetReply(C_05_CalculateTargetReply task)
    {
        foreach (var kvp in HitList)
        {
            kvp.Value.Target.被动回复时处理(this, task);
        }
        if (task.GainSkillExp && HitList.Count != 0)
        {
            (Caster as PlayerObject).GainSkillExperience(task.ExpSkillID);
        }
    }

    private void ProcessC_06_CalculatePetSummoning(C_06_CalculatePetSummoning task)
    {
        if (task.Companion)
        {
            if (string.IsNullOrEmpty(task.PetName))
                return;

            if (MonsterInfo.DataSheet.TryGetValue(task.PetName, out var moni))
            {
                new MonsterObject(moni, CurrentMap, int.MaxValue, new Point[1] { CastLocation }, true, true).SurvivalTime = SEngine.CurrentTime.AddMinutes(1.0);
            }
        }
        else if (Caster is PlayerObject player)
        {
            if ((task.CheckSkillInscriptions && (!player.Skills.TryGetValue(SkillID, out var v9) || v9.InscriptionID != InscriptionID)) || string.IsNullOrEmpty(task.PetName))
                return;

            int max = ((task.SpawnCount?.Length > SkillLevel) ? task.SpawnCount[SkillLevel] : 0);
            if (player.Pets.Count < max && MonsterInfo.DataSheet.TryGetValue(task.PetName, out var moni))
            {
                byte levelMax = (byte)((task.LevelCap?.Length > SkillLevel) ? task.LevelCap[SkillLevel] : 0);
                PetObject pet = new PetObject(player, moni, SkillLevel, levelMax, task.PetBoundWeapons);
                player.Enqueue(new SyncPetLevelPacket
                {
                    ObjectID = pet.ObjectID,
                    PetLevel = pet.PetLevel
                });
                player.Enqueue(new GameErrorMessagePacket
                {
                    ErrorCode = 9473,
                    Param1 = (int)player.PetMode
                });
                player.PetInfo.Add(pet.PInfo);
                player.Pets.Add(pet);
                if (task.GainSkillExp)
                {
                    player.GainSkillExperience(task.ExpSkillID);
                }
            }
        }
    }

    private void ProcessC_07_CalculateTargetTeleportation(C_07_CalculateTargetTeleportation task)
    {
        (Caster as PlayerObject).玩家瞬间移动(this, task);
    }

    public void Stop()
    {
        Nodes.Clear();

        Caster?.SendPacket(new 技能释放中断
        {
            ObjectID = ((!TargetBorrow || Target == null) ? Caster.ObjectID : Target.ObjectID),
            SkillID = SkillID,
            SkillLevel = SkillLevel,
            InscriptionID = InscriptionID,
            ActionID = ActionID,
            SegmentID = SegmentID
        });
    }
}
