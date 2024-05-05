using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using GameServer.Map;
using GameServer.Networking;

using GamePackets;
using GamePackets.Server;

namespace GameServer.Database;

[SearchAttribute(SearchName = "GuildName")]
public sealed class GuildInfo : DBObject
{
    public readonly DataMonitor<CharacterInfo> President;
    public readonly DataMonitor<DateTime> CreatedDate;
    public readonly DataMonitor<string> GuildName;
    public readonly DataMonitor<string> CreatorName;
    public readonly DataMonitor<string> GuildDeclaration;
    public readonly DataMonitor<string> GuildNotice;
    public readonly DataMonitor<byte> GuildLevel;
    public readonly DataMonitor<int> GuildFunds;
    public readonly DataMonitor<int> FoodAmount;
    public readonly DataMonitor<int> WoodAmount;
    public readonly DataMonitor<int> StoneAmount;
    public readonly DataMonitor<int> OreAmount;
    public readonly DataMonitor<int> GuildRanking;
    public readonly ListMonitor<GuildLog> GuildLogs;
    public readonly DictionaryMonitor<CharacterInfo, GuildRank> Members;
    public readonly DictionaryMonitor<CharacterInfo, DateTime> BannedMembers;
    public readonly DictionaryMonitor<GuildInfo, DateTime> AllianceGuilds;
    public readonly DictionaryMonitor<GuildInfo, DateTime> HostileGuilds;

    public Dictionary<CharacterInfo, DateTime> Applications;
    public Dictionary<CharacterInfo, DateTime> Invitations;
    public Dictionary<GuildInfo, AllianceApplication> AllianceApplications;
    public Dictionary<GuildInfo, DateTime> HostileReleaseApplications;
    public int ID => Index.V;
    public int CreationTime => Compute.TimeSeconds(CreatedDate.V);
    public string PresidentName => President.V.UserName.V;

    public CharacterInfo PresidentInfo
    {
        get { return President.V; }
        set
        {
            if (President.V != value)
                President.V = value;
        }
    }

    public DateTime ProcessTime { get; set; }

    public override string ToString() => GuildName?.V;

    public GuildInfo()
    {
        Applications = new Dictionary<CharacterInfo, DateTime>();
        Invitations = new Dictionary<CharacterInfo, DateTime>();
        AllianceApplications = new Dictionary<GuildInfo, AllianceApplication>();
        HostileReleaseApplications = new Dictionary<GuildInfo, DateTime>();
    }

    public GuildInfo(PlayerObject player, string guildName, string declaration)
    {
        Applications = new Dictionary<CharacterInfo, DateTime>();
        Invitations = new Dictionary<CharacterInfo, DateTime>();
        AllianceApplications = new Dictionary<GuildInfo, AllianceApplication>();
        HostileReleaseApplications = new Dictionary<GuildInfo, DateTime>();

        GuildName.V = guildName;
        GuildDeclaration.V = declaration;
        GuildNotice.V = "Have a great game.";
        President.V = player.Character;
        CreatorName.V = player.Name;
        Members.Add(player.Character, GuildRank.President);

        AddLog(new GuildLog
        {
            LogType = GuildLogType.CreateGuild,
            Param1 = player.ObjectID,
            LogTime = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        AddLog(new GuildLog
        {
            LogType = GuildLogType.JoinGuild,
            Param1 = player.ObjectID,
            LogTime = Compute.TimeSeconds(SEngine.CurrentTime)
        });

        GuildLevel.V = 1;
        GuildFunds.V = 1_000_000;
        FoodAmount.V = 1_000_000;
        WoodAmount.V = 1_000_000;
        StoneAmount.V = 1_000_000;
        OreAmount.V = 1_000_000;
        CreatedDate.V = SEngine.CurrentTime;

        Session.GuildInfoTable.Add(this, true);
        SystemInfo.Info.UpdateGuildRanks(this);
    }

    public void Process()
    {
        if (SEngine.CurrentTime <= ProcessTime) return;
        
        foreach (KeyValuePair<GuildInfo, DateTime> item in AllianceGuilds.ToList())
        {
            if (SEngine.CurrentTime > item.Value)
            {
                AllianceGuilds.Remove(item.Key);
                item.Key.AllianceGuilds.Remove(this);
                Broadcast(new 删除外交公告
                {
                    外交类型 = 1,
                    行会编号 = item.Key.ID
                });
                item.Key.Broadcast(new 删除外交公告
                {
                    外交类型 = 1,
                    行会编号 = ID
                });
                NetworkManager.SendAnnouncement($"[{this}]和[{item.Key}]的行会盟约已经到期自动解除");
            }
        }
        foreach (KeyValuePair<GuildInfo, DateTime> item2 in HostileGuilds.ToList())
        {
            if (SEngine.CurrentTime > item2.Value)
            {
                HostileGuilds.Remove(item2.Key);
                item2.Key.HostileGuilds.Remove(this);
                Broadcast(new 删除外交公告
                {
                    外交类型 = 2,
                    行会编号 = item2.Key.ID
                });
                item2.Key.Broadcast(new 删除外交公告
                {
                    外交类型 = 2,
                    行会编号 = ID
                });
                NetworkManager.SendAnnouncement($"[{this}]和[{item2.Key}]的行会敌对已经到期自动解除");
            }
        }
        foreach (KeyValuePair<CharacterInfo, DateTime> item3 in Applications.ToList())
        {
            if (SEngine.CurrentTime > item3.Value)
            {
                Applications.Remove(item3.Key);
            }
        }
        foreach (KeyValuePair<CharacterInfo, DateTime> item4 in Invitations.ToList())
        {
            if (SEngine.CurrentTime > item4.Value)
            {
                Invitations.Remove(item4.Key);
            }
        }
        foreach (KeyValuePair<GuildInfo, DateTime> item5 in HostileReleaseApplications.ToList())
        {
            if (SEngine.CurrentTime > item5.Value)
            {
                HostileReleaseApplications.Remove(item5.Key);
            }
        }
        foreach (KeyValuePair<GuildInfo, AllianceApplication> item6 in AllianceApplications.ToList())
        {
            if (SEngine.CurrentTime > item6.Value.ValidTime)
            {
                AllianceApplications.Remove(item6.Key);
            }
        }
        ProcessTime = SEngine.CurrentTime.AddSeconds(1.0);
    }

    public void BreakGuild()
    {
        foreach (KeyValuePair<DateTime, GuildInfo> item in SystemInfo.Info.GuildApplications.ToList())
        {
            if (item.Value == this)
            {
                SystemInfo.Info.GuildApplications.Remove(item.Key);
            }
        }
        Broadcast(new 脱离行会应答
        {
            WithdrawMode = 2
        });
        foreach (CharacterInfo key in Members.Keys)
        {
            key.CurrentGuild = null;
            key.Enqueue(new 同步对象行会
            {
                对象编号 = key.ID
            });
        }
        if (GuildRanking.V > 0)
        {
            SystemInfo.Info.GuildRanking.RemoveAt(GuildRanking.V - 1);
            for (int i = GuildRanking.V - 1; i < SystemInfo.Info.GuildRanking.Count; i++)
            {
                SystemInfo.Info.GuildRanking[i].GuildRanking.V = i + 1;
            }
        }
        Members.Clear();
        BannedMembers.Clear();
        Remove();
    }

    public void Broadcast(GamePacket p)
    {
        foreach (var member in Members.Keys)
        {
            member.Enqueue(p);
        }
    }

    public void AddMember(CharacterInfo member, GuildRank rank = GuildRank.Member)
    {
        Members.Add(member, rank);
        member.CurrentGuild = this;
        Broadcast(new 行会加入成员
        {
            对象编号 = member.ID,
            对象名字 = member.UserName.V,
            对象职位 = 7,
            对象等级 = member.CurrentLevel,
            对象职业 = (byte)member.Job.V,
            当前地图 = (byte)member.CurrentMap.V
        });
        if (!member.Connected)
        {
            Broadcast(new SyncMemberInfoPacket
            {
                ObjectID = member.ID,
                对象信息 = Compute.TimeSeconds(member.DisconnectDate.V)
            });
        }
        member.Enqueue(new 行会信息公告
        {
            Description = 行会信息描述()
        });
        AddLog(new GuildLog
        {
            LogType = GuildLogType.JoinGuild,
            Param1 = member.ID,
            LogTime = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        if (MapManager.Players.TryGetValue(member.ID, out var value))
        {
            value.SendPacket(new 同步对象行会
            {
                对象编号 = member.ID,
                行会编号 = ID
            });
        }
        SystemInfo.Info.UpdateGuildRanks(this);
    }

    public void RemoveMember(CharacterInfo member)
    {
        Members.Remove(member);
        BannedMembers.Remove(member);
        member.CurrentGuild = null;
        member.Enqueue(new 脱离行会应答
        {
            WithdrawMode = 1
        });
        Broadcast(new 脱离行会公告
        {
            对象编号 = member.ID
        });
        AddLog(new GuildLog
        {
            LogType = GuildLogType.LeaveGuild,
            Param1 = member.ID,
            LogTime = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        if (MapManager.Players.TryGetValue(member.ID, out var player))
        {
            player.SendPacket(new 同步对象行会
            {
                对象编号 = member.ID
            });
        }
        SystemInfo.Info.UpdateGuildRanks(this);
    }

    public void KickMember(CharacterInfo principal, CharacterInfo member)
    {
        if (Members.Remove(member))
        {
            BannedMembers.Remove(member);
            member.CurrentGuild = null;
            member.Enqueue(new 脱离行会应答
            {
                WithdrawMode = 2
            });
            Broadcast(new 脱离行会公告
            {
                对象编号 = member.ID
            });
            AddLog(new GuildLog
            {
                LogType = GuildLogType.逐出公会,
                Param1 = member.ID,
                Param2 = principal.ID,
                LogTime = Compute.TimeSeconds(SEngine.CurrentTime)
            });
            if (MapManager.Players.TryGetValue(member.ID, out var player))
            {
                player.SendPacket(new 同步对象行会
                {
                    对象编号 = member.ID
                });
            }
            SystemInfo.Info.UpdateGuildRanks(this);
        }
    }

    public void ChangeRank(CharacterInfo principal, CharacterInfo member, GuildRank rank)
    {
        GuildRank 行会职位3 = (Members[member] = rank);
 
        Members[member] = rank;
        Broadcast(new 变更职位公告
        {
            对象编号 = member.ID,
            对象职位 = (byte)rank
        });
        AddLog(new GuildLog
        {
            LogType = GuildLogType.ChangeRank,
            Param1 = principal.ID,
            Param2 = member.ID,
            Param3 = (byte)行会职位3,
            Param4 = (byte)rank,
            LogTime = Compute.TimeSeconds(SEngine.CurrentTime)
        });
    }

    public void UpdateDeclaration(CharacterInfo principal, string declaration)
    {
        GuildDeclaration.V = declaration;
        principal.Enqueue(new SocialErrorPacket
        {
            ErrorCode = 6747
        });
    }

    public void UpdateNotice(string notice)
    {
        GuildNotice.V = notice;
        Broadcast(new 变更行会公告
        {
            字节数据 = Encoding.UTF8.GetBytes(notice + "\0")
        });
    }

    public void ChangeLeader(CharacterInfo leader, CharacterInfo member)
    {
        President.V = member;
        Members[leader] = GuildRank.Member;
        Members[member] = GuildRank.President;

        Broadcast(new 会长传位公告
        {
            当前编号 = leader.ID,
            传位编号 = member.ID
        });
        AddLog(new GuildLog
        {
            LogType = GuildLogType.会长传位,
            Param1 = leader.ID,
            Param2 = member.ID,
            LogTime = Compute.TimeSeconds(SEngine.CurrentTime)
        });
    }

    public void BanMember(CharacterInfo principal, CharacterInfo member, byte state)
    {
        if (state == 2 && BannedMembers.Remove(member))
        {
            Broadcast(new 行会禁言公告
            {
                对象编号 = member.ID,
                禁言状态 = 2
            });
        }
        else if (state == 1)
        {
            BannedMembers[member] = SEngine.CurrentTime;
            Broadcast(new 行会禁言公告
            {
                对象编号 = member.ID,
                禁言状态 = 1
            });
        }
        else
        {
            principal.Enqueue(new SocialErrorPacket
            {
                ErrorCode = 6680
            });
        }
    }

    public void AddAllianceRequest(CharacterInfo principal, GuildInfo guild, byte time)
    {
        principal.Enqueue(new 申请结盟应答
        {
            行会编号 = guild.ID
        });
        if (!guild.AllianceApplications.ContainsKey(this))
        {
            guild.GuildAlert(GuildRank.副长, 2);
        }
        guild.AllianceApplications[this] = new AllianceApplication
        {
            DiplomaticHours = time,
            ValidTime = SEngine.CurrentTime.AddHours(10.0)
        };
    }

    public void AddHostileGuild(GuildInfo guild, byte duration)
    {
        var days = duration switch
        {
            2 => 3,
            1 => 1,
            _ => 7,
        };

        var date = SEngine.CurrentTime.AddDays(days);
        guild.HostileGuilds[this] = date;
        HostileGuilds[guild] = date;

        Broadcast(new 添加外交公告
        {
            外交类型 = 2,
            行会编号 = guild.ID,
            行会名字 = guild.GuildName.V,
            行会等级 = guild.GuildLevel.V,
            行会人数 = (byte)guild.Members.Count,
            外交时间 = (int)(HostileGuilds[guild] - SEngine.CurrentTime).TotalSeconds
        });
        guild.Broadcast(new 添加外交公告
        {
            外交类型 = 2,
            行会编号 = ID,
            行会名字 = GuildName.V,
            行会等级 = GuildLevel.V,
            行会人数 = (byte)Members.Count,
            外交时间 = (int)(guild.HostileGuilds[this] - SEngine.CurrentTime).TotalSeconds
        });
        AddLog(new GuildLog
        {
            LogType = GuildLogType.行会敌对,
            Param1 = ID,
            Param2 = guild.ID,
            LogTime = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        guild.AddLog(new GuildLog
        {
            LogType = GuildLogType.行会敌对,
            Param1 = guild.ID,
            Param2 = ID,
            LogTime = Compute.TimeSeconds(SEngine.CurrentTime)
        });
    }

    public void AddAllianceGuild(GuildInfo guild)
    {
        var days = AllianceApplications[guild].DiplomaticHours switch
        {
            2 => 3,
            1 => 1,
            _ => 7,
        };

        var date = SEngine.CurrentTime.AddDays(days);
        guild.AllianceGuilds[this] = date;
        AllianceGuilds[guild] = date;

        Broadcast(new 添加外交公告
        {
            外交类型 = 1,
            行会名字 = guild.GuildName.V,
            行会编号 = guild.ID,
            行会等级 = guild.GuildLevel.V,
            行会人数 = (byte)guild.Members.Count,
            外交时间 = (int)(AllianceGuilds[guild] - SEngine.CurrentTime).TotalSeconds
        });
        guild.Broadcast(new 添加外交公告
        {
            外交类型 = 1,
            行会名字 = GuildName.V,
            行会编号 = ID,
            行会等级 = GuildLevel.V,
            行会人数 = (byte)Members.Count,
            外交时间 = (int)(guild.AllianceGuilds[this] - SEngine.CurrentTime).TotalSeconds
        });
        AddLog(new GuildLog
        {
            LogType = GuildLogType.行会结盟,
            Param1 = ID,
            Param2 = guild.ID,
            LogTime = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        guild.AddLog(new GuildLog
        {
            LogType = GuildLogType.行会结盟,
            Param1 = guild.ID,
            Param2 = ID,
            LogTime = Compute.TimeSeconds(SEngine.CurrentTime)
        });
    }

    public void RemoveAllyGuild(CharacterInfo principal, GuildInfo guild)
    {
        AllianceGuilds.Remove(guild);
        guild.AllianceGuilds.Remove(this);
        Broadcast(new 删除外交公告
        {
            外交类型 = 1,
            行会编号 = guild.ID
        });
        guild.Broadcast(new 删除外交公告
        {
            外交类型 = 1,
            行会编号 = ID
        });
        AddLog(new GuildLog
        {
            LogType = GuildLogType.取消结盟,
            Param1 = ID,
            Param2 = guild.ID,
            LogTime = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        guild.AddLog(new GuildLog
        {
            LogType = GuildLogType.取消结盟,
            Param1 = guild.ID,
            Param2 = ID,
            LogTime = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        principal.Enqueue(new SocialErrorPacket
        {
            ErrorCode = 6812
        });
    }

    public void RequestReleaseHostileGuild(CharacterInfo principal, GuildInfo guild)
    {
        principal.Enqueue(new SocialErrorPacket { ErrorCode = 6829 });

        foreach (var member in guild.Members)
        {
            if (member.Value <= GuildRank.副长)
            {
                member.Key.Enqueue(new 解除敌对列表
                {
                    申请类型 = 1,
                    行会编号 = ID
                });
            }
        }
        guild.HostileReleaseApplications[this] = SEngine.CurrentTime.AddHours(10.0);
    }

    public void RemoveHostileGuild(GuildInfo guild)
    {
        HostileGuilds.Remove(guild);
        guild.HostileGuilds.Remove(this);

        Broadcast(new 解除敌对列表
        {
            申请类型 = 2,
            行会编号 = guild.ID
        });
        Broadcast(new 删除外交公告
        {
            外交类型 = 2,
            行会编号 = guild.ID
        });
        guild.Broadcast(new 删除外交公告
        {
            外交类型 = 2,
            行会编号 = ID
        });
        AddLog(new GuildLog
        {
            LogType = GuildLogType.取消敌对,
            Param1 = ID,
            Param2 = guild.ID,
            LogTime = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        guild.AddLog(new GuildLog
        {
            LogType = GuildLogType.取消敌对,
            Param1 = guild.ID,
            Param2 = ID,
            LogTime = Compute.TimeSeconds(SEngine.CurrentTime)
        });
    }

    public void SendMail(GuildRank rank, string subject, string msg)
    {
        foreach (KeyValuePair<CharacterInfo, GuildRank> item in Members)
        {
            if (item.Value <= rank)
            {
                item.Key.SendMail(new MailInfo(null, subject, msg, null));
            }
        }
    }

    public void AddLog(GuildLog log)
    {
        GuildLogs.Insert(0, log);
        Broadcast(new AddGuildLogPacket
        {
            LogType = (byte)log.LogType,
            Param1 = log.Param1,
            Param2 = log.Param2,
            Param3 = log.Param3,
            Param4 = log.Param4,
            LogTime = log.LogTime
        });
        while (GuildLogs.Count > 10)
        {
            GuildLogs.RemoveAt(GuildLogs.Count - 1);
        }
    }

    public void GuildAlert(GuildRank rank, byte reminder)
    {
        foreach (var member in Members)
        {
            if (member.Value <= rank && member.Key.Connected)
            {
                member.Key.Enqueue(new 发送行会通知
                {
                    提醒类型 = reminder
                });
            }
        }
    }

    public byte[] 行会检索描述()
    {
        using MemoryStream memoryStream = new MemoryStream(new byte[229]);
        using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
        binaryWriter.Write(ID);
        byte[] array = new byte[25];
        Encoding.UTF8.GetBytes(GuildName.V).CopyTo(array, 0);
        binaryWriter.Write(array);
        binaryWriter.Write(GuildLevel.V);
        binaryWriter.Write((byte)Members.Count);
        binaryWriter.Write(0);
        array = new byte[32];
        Encoding.UTF8.GetBytes(PresidentName).CopyTo(array, 0);
        binaryWriter.Write(array);
        array = new byte[32];
        Encoding.UTF8.GetBytes(CreatorName.V).CopyTo(array, 0);
        binaryWriter.Write(array);
        binaryWriter.Write(CreationTime);
        array = new byte[101];
        Encoding.UTF8.GetBytes(GuildDeclaration.V).CopyTo(array, 0);
        binaryWriter.Write(array);
        binaryWriter.Write(new byte[17]);
        binaryWriter.Write(new byte[8]);
        return memoryStream.ToArray();
    }

    public byte[] 行会信息描述()
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
        binaryWriter.Write(ID);
        binaryWriter.Write(Encoding.UTF8.GetBytes(GuildName.V));
        binaryWriter.Seek(29, SeekOrigin.Begin);
        binaryWriter.Write(GuildLevel.V);
        binaryWriter.Write((byte)Members.Count);
        binaryWriter.Write(GuildFunds.V);
        binaryWriter.Write(CreationTime);
        binaryWriter.Seek(43, SeekOrigin.Begin);
        binaryWriter.Write(Encoding.UTF8.GetBytes(PresidentName));
        binaryWriter.Seek(75, SeekOrigin.Begin);
        binaryWriter.Write(Encoding.UTF8.GetBytes(CreatorName.V));
        binaryWriter.Seek(107, SeekOrigin.Begin);
        binaryWriter.Write(Encoding.UTF8.GetBytes(GuildNotice.V));
        binaryWriter.Seek(4258, SeekOrigin.Begin);
        binaryWriter.Write(FoodAmount.V);
        binaryWriter.Write(WoodAmount.V);
        binaryWriter.Write(StoneAmount.V);
        binaryWriter.Write(OreAmount.V);
        binaryWriter.Write(402);
        binaryWriter.Seek(7960, SeekOrigin.Begin);
        foreach (KeyValuePair<CharacterInfo, GuildRank> item in Members)
        {
            binaryWriter.Write(item.Key.ID);
            byte[] array = new byte[32];
            Encoding.UTF8.GetBytes(item.Key.UserName.V).CopyTo(array, 0);
            binaryWriter.Write(array);
            binaryWriter.Write((byte)item.Value);
            binaryWriter.Write(item.Key.CurrentLevel);
            binaryWriter.Write((byte)item.Key.Job.V);
            binaryWriter.Write(item.Key.CurrentMap.V);
            binaryWriter.Write(!item.Key.Connected ? Compute.TimeSeconds(item.Key.DisconnectDate.V) : 0);
            binaryWriter.Write(0);
            binaryWriter.Write(BannedMembers.ContainsKey(item.Key));
        }
        binaryWriter.Seek(330, SeekOrigin.Begin);
        binaryWriter.Write((byte)Math.Min(10, GuildLogs.Count));
        for (int i = 0; i < 10 && i < GuildLogs.Count; i++)
        {
            binaryWriter.Write((byte)GuildLogs[i].LogType);
            binaryWriter.Write(GuildLogs[i].Param1);
            binaryWriter.Write(GuildLogs[i].Param2);
            binaryWriter.Write(GuildLogs[i].Param3);
            binaryWriter.Write(GuildLogs[i].Param4);
            binaryWriter.Write(GuildLogs[i].LogTime);
        }
        binaryWriter.Seek(1592, SeekOrigin.Begin);
        binaryWriter.Write((byte)AllianceGuilds.Count);
        foreach (KeyValuePair<GuildInfo, DateTime> item2 in AllianceGuilds)
        {
            binaryWriter.Write((byte)1);
            binaryWriter.Write(item2.Key.ID);
            binaryWriter.Write(Compute.TimeSeconds(item2.Value));
            binaryWriter.Write(item2.Key.GuildLevel.V);
            binaryWriter.Write((byte)item2.Key.Members.Count);
            byte[] array2 = new byte[25];
            Encoding.UTF8.GetBytes(item2.Key.GuildName.V).CopyTo(array2, 0);
            binaryWriter.Write(array2);
        }
        binaryWriter.Seek(1953, SeekOrigin.Begin);
        binaryWriter.Write((byte)HostileGuilds.Count);
        foreach (KeyValuePair<GuildInfo, DateTime> item3 in HostileGuilds)
        {
            binaryWriter.Write((byte)2);
            binaryWriter.Write(item3.Key.ID);
            binaryWriter.Write(Compute.TimeSeconds(item3.Value));
            binaryWriter.Write(item3.Key.GuildLevel.V);
            binaryWriter.Write((byte)item3.Key.Members.Count);
            byte[] array3 = new byte[25];
            Encoding.UTF8.GetBytes(item3.Key.GuildName.V).CopyTo(array3, 0);
            binaryWriter.Write(array3);
        }
        return memoryStream.ToArray();
    }

    public byte[] 入会申请描述()
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
        binaryWriter.Write((ushort)Applications.Count);
        foreach (CharacterInfo key in Applications.Keys)
        {
            binaryWriter.Write(key.ID);
            byte[] array = new byte[32];
            Encoding.UTF8.GetBytes(key.UserName.V).CopyTo(array, 0);
            binaryWriter.Write(array);
            binaryWriter.Write(key.CurrentLevel);
            binaryWriter.Write(key.CurrentLevel);
            binaryWriter.Write(key.Connected ? Compute.TimeSeconds(SEngine.CurrentTime) : Compute.TimeSeconds(key.DisconnectDate.V));
        }
        return memoryStream.ToArray();
    }

    public byte[] 结盟申请描述()
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
        binaryWriter.Write((byte)AllianceApplications.Count);
        foreach (KeyValuePair<GuildInfo, AllianceApplication> item in AllianceApplications)
        {
            binaryWriter.Write(item.Key.ID);
            byte[] array = new byte[25];
            Encoding.UTF8.GetBytes(item.Key.GuildName.V).CopyTo(array, 0);
            binaryWriter.Write(array);
            binaryWriter.Write(item.Key.GuildLevel.V);
            binaryWriter.Write((byte)item.Key.Members.Count);
            array = new byte[32];
            Encoding.UTF8.GetBytes(item.Key.PresidentName).CopyTo(array, 0);
            binaryWriter.Write(array);
            binaryWriter.Write(Compute.TimeSeconds(item.Value.ValidTime));
        }
        return memoryStream.ToArray();
    }

    public byte[] 解除申请描述()
    {
        using MemoryStream memoryStream = new MemoryStream(new byte[256]);
        using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
        foreach (KeyValuePair<GuildInfo, DateTime> item in HostileReleaseApplications)
        {
            binaryWriter.Write(item.Key.ID);
        }
        return memoryStream.ToArray();
    }

    public byte[] 更多事记描述()
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
        binaryWriter.Write((ushort)Math.Max(0, GuildLogs.Count - 10));
        for (int i = 10; i < GuildLogs.Count; i++)
        {
            binaryWriter.Write((byte)GuildLogs[i].LogType);
            binaryWriter.Write(GuildLogs[i].Param1);
            binaryWriter.Write(GuildLogs[i].Param2);
            binaryWriter.Write(GuildLogs[i].Param3);
            binaryWriter.Write(GuildLogs[i].Param4);
            binaryWriter.Write(GuildLogs[i].LogTime);
        }
        return memoryStream.ToArray();
    }
}
