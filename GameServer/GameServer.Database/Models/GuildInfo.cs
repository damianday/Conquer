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
    public readonly DataMonitor<byte> 行会等级;
    public readonly DataMonitor<int> 行会资金;
    public readonly DataMonitor<int> 粮食数量;
    public readonly DataMonitor<int> 木材数量;
    public readonly DataMonitor<int> 石材数量;
    public readonly DataMonitor<int> 铁矿数量;
    public readonly DataMonitor<int> 行会排名;
    public readonly ListMonitor<行会事记> 行会事记;
    public readonly DictionaryMonitor<CharacterInfo, GuildRank> Members;
    public readonly DictionaryMonitor<CharacterInfo, DateTime> BannedMembers;
    public readonly DictionaryMonitor<GuildInfo, DateTime> AllianceGuilds;
    public readonly DictionaryMonitor<GuildInfo, DateTime> HostileGuilds;

    public Dictionary<CharacterInfo, DateTime> Applications;
    public Dictionary<CharacterInfo, DateTime> Invitations;
    public Dictionary<GuildInfo, 外交申请> AllianceApplications;
    public Dictionary<GuildInfo, DateTime> HostileReleaseApplications;
    public int ID => Index.V;
    public int 创建时间 => Compute.TimeSeconds(CreatedDate.V);
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

    public override string ToString()
    {
        return GuildName?.V;
    }

    public GuildInfo()
    {
        Applications = new Dictionary<CharacterInfo, DateTime>();
        Invitations = new Dictionary<CharacterInfo, DateTime>();
        AllianceApplications = new Dictionary<GuildInfo, 外交申请>();
        HostileReleaseApplications = new Dictionary<GuildInfo, DateTime>();
    }

    public GuildInfo(PlayerObject player, string guildName, string declaration)
    {
        Applications = new Dictionary<CharacterInfo, DateTime>();
        Invitations = new Dictionary<CharacterInfo, DateTime>();
        AllianceApplications = new Dictionary<GuildInfo, 外交申请>();
        HostileReleaseApplications = new Dictionary<GuildInfo, DateTime>();
        this.GuildName.V = guildName;
        this.GuildDeclaration.V = declaration;
        GuildNotice.V = "Have a great game.";
        President.V = player.Character;
        CreatorName.V = player.Name;
        Members.Add(player.Character, GuildRank.President);
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.CreateGuild,
            第一参数 = player.ObjectID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.JoinGuild,
            第一参数 = player.ObjectID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        行会等级.V = 1;
        行会资金.V = 1000000;
        粮食数量.V = 1000000;
        木材数量.V = 1000000;
        石材数量.V = 1000000;
        铁矿数量.V = 1000000;
        CreatedDate.V = SEngine.CurrentTime;
        Session.GuildInfoTable.Add(this, indexed: true);
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
        foreach (KeyValuePair<GuildInfo, 外交申请> item6 in AllianceApplications.ToList())
        {
            if (SEngine.CurrentTime > item6.Value.申请时间)
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
        if (行会排名.V > 0)
        {
            SystemInfo.Info.GuildRanking.RemoveAt(行会排名.V - 1);
            for (int i = 行会排名.V - 1; i < SystemInfo.Info.GuildRanking.Count; i++)
            {
                SystemInfo.Info.GuildRanking[i].行会排名.V = i + 1;
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
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.JoinGuild,
            第一参数 = member.ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
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
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.LeaveGuild,
            第一参数 = member.ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
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
            添加事记(new 行会事记
            {
                事记类型 = 事记类型.逐出公会,
                第一参数 = member.ID,
                第二参数 = principal.ID,
                事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
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
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.ChangeRank,
            第一参数 = principal.ID,
            第二参数 = member.ID,
            第三参数 = (byte)行会职位3,
            第四参数 = (byte)rank,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
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
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.会长传位,
            第一参数 = leader.ID,
            第二参数 = member.ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
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

    public void AddAllianceRequest(CharacterInfo principal, GuildInfo guild, byte 时间参数)
    {
        principal.Enqueue(new 申请结盟应答
        {
            行会编号 = guild.ID
        });
        if (!guild.AllianceApplications.ContainsKey(this))
        {
            guild.GuildAlert(GuildRank.副长, 2);
        }
        guild.AllianceApplications[this] = new 外交申请
        {
            外交时间 = 时间参数,
            申请时间 = SEngine.CurrentTime.AddHours(10.0)
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
            行会等级 = guild.行会等级.V,
            行会人数 = (byte)guild.Members.Count,
            外交时间 = (int)(HostileGuilds[guild] - SEngine.CurrentTime).TotalSeconds
        });
        guild.Broadcast(new 添加外交公告
        {
            外交类型 = 2,
            行会编号 = ID,
            行会名字 = GuildName.V,
            行会等级 = 行会等级.V,
            行会人数 = (byte)Members.Count,
            外交时间 = (int)(guild.HostileGuilds[this] - SEngine.CurrentTime).TotalSeconds
        });
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.行会敌对,
            第一参数 = ID,
            第二参数 = guild.ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        guild.添加事记(new 行会事记
        {
            事记类型 = 事记类型.行会敌对,
            第一参数 = guild.ID,
            第二参数 = ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
    }

    public void AddAllianceGuild(GuildInfo guild)
    {
        var days = AllianceApplications[guild].外交时间 switch
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
            行会等级 = guild.行会等级.V,
            行会人数 = (byte)guild.Members.Count,
            外交时间 = (int)(AllianceGuilds[guild] - SEngine.CurrentTime).TotalSeconds
        });
        guild.Broadcast(new 添加外交公告
        {
            外交类型 = 1,
            行会名字 = GuildName.V,
            行会编号 = ID,
            行会等级 = 行会等级.V,
            行会人数 = (byte)Members.Count,
            外交时间 = (int)(guild.AllianceGuilds[this] - SEngine.CurrentTime).TotalSeconds
        });
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.行会结盟,
            第一参数 = ID,
            第二参数 = guild.ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        guild.添加事记(new 行会事记
        {
            事记类型 = 事记类型.行会结盟,
            第一参数 = guild.ID,
            第二参数 = ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
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
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.取消结盟,
            第一参数 = ID,
            第二参数 = guild.ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        guild.添加事记(new 行会事记
        {
            事记类型 = 事记类型.取消结盟,
            第一参数 = guild.ID,
            第二参数 = ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
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
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.取消敌对,
            第一参数 = ID,
            第二参数 = guild.ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        guild.添加事记(new 行会事记
        {
            事记类型 = 事记类型.取消敌对,
            第一参数 = guild.ID,
            第二参数 = ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
    }

    public void 发送邮件(GuildRank 职位, string 标题, string 内容)
    {
        foreach (KeyValuePair<CharacterInfo, GuildRank> item in Members)
        {
            if (item.Value <= 职位)
            {
                item.Key.SendMail(new MailInfo(null, 标题, 内容, null));
            }
        }
    }

    public void 添加事记(行会事记 事记)
    {
        行会事记.Insert(0, 事记);
        Broadcast(new 添加公会事记
        {
            事记类型 = (byte)事记.事记类型,
            第一参数 = 事记.第一参数,
            第二参数 = 事记.第二参数,
            第三参数 = 事记.第三参数,
            第四参数 = 事记.第四参数,
            事记时间 = 事记.事记时间
        });
        while (行会事记.Count > 10)
        {
            行会事记.RemoveAt(行会事记.Count - 1);
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
        binaryWriter.Write(行会等级.V);
        binaryWriter.Write((byte)Members.Count);
        binaryWriter.Write(0);
        array = new byte[32];
        Encoding.UTF8.GetBytes(PresidentName).CopyTo(array, 0);
        binaryWriter.Write(array);
        array = new byte[32];
        Encoding.UTF8.GetBytes(CreatorName.V).CopyTo(array, 0);
        binaryWriter.Write(array);
        binaryWriter.Write(创建时间);
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
        binaryWriter.Write(行会等级.V);
        binaryWriter.Write((byte)Members.Count);
        binaryWriter.Write(行会资金.V);
        binaryWriter.Write(创建时间);
        binaryWriter.Seek(43, SeekOrigin.Begin);
        binaryWriter.Write(Encoding.UTF8.GetBytes(PresidentName));
        binaryWriter.Seek(75, SeekOrigin.Begin);
        binaryWriter.Write(Encoding.UTF8.GetBytes(CreatorName.V));
        binaryWriter.Seek(107, SeekOrigin.Begin);
        binaryWriter.Write(Encoding.UTF8.GetBytes(GuildNotice.V));
        binaryWriter.Seek(4258, SeekOrigin.Begin);
        binaryWriter.Write(粮食数量.V);
        binaryWriter.Write(木材数量.V);
        binaryWriter.Write(石材数量.V);
        binaryWriter.Write(铁矿数量.V);
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
        binaryWriter.Write((byte)Math.Min(10, 行会事记.Count));
        for (int i = 0; i < 10 && i < 行会事记.Count; i++)
        {
            binaryWriter.Write((byte)行会事记[i].事记类型);
            binaryWriter.Write(行会事记[i].第一参数);
            binaryWriter.Write(行会事记[i].第二参数);
            binaryWriter.Write(行会事记[i].第三参数);
            binaryWriter.Write(行会事记[i].第四参数);
            binaryWriter.Write(行会事记[i].事记时间);
        }
        binaryWriter.Seek(1592, SeekOrigin.Begin);
        binaryWriter.Write((byte)AllianceGuilds.Count);
        foreach (KeyValuePair<GuildInfo, DateTime> item2 in AllianceGuilds)
        {
            binaryWriter.Write((byte)1);
            binaryWriter.Write(item2.Key.ID);
            binaryWriter.Write(Compute.TimeSeconds(item2.Value));
            binaryWriter.Write(item2.Key.行会等级.V);
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
            binaryWriter.Write(item3.Key.行会等级.V);
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
        foreach (KeyValuePair<GuildInfo, 外交申请> item in AllianceApplications)
        {
            binaryWriter.Write(item.Key.ID);
            byte[] array = new byte[25];
            Encoding.UTF8.GetBytes(item.Key.GuildName.V).CopyTo(array, 0);
            binaryWriter.Write(array);
            binaryWriter.Write(item.Key.行会等级.V);
            binaryWriter.Write((byte)item.Key.Members.Count);
            array = new byte[32];
            Encoding.UTF8.GetBytes(item.Key.PresidentName).CopyTo(array, 0);
            binaryWriter.Write(array);
            binaryWriter.Write(Compute.TimeSeconds(item.Value.申请时间));
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
        binaryWriter.Write((ushort)Math.Max(0, 行会事记.Count - 10));
        for (int i = 10; i < 行会事记.Count; i++)
        {
            binaryWriter.Write((byte)行会事记[i].事记类型);
            binaryWriter.Write(行会事记[i].第一参数);
            binaryWriter.Write(行会事记[i].第二参数);
            binaryWriter.Write(行会事记[i].第三参数);
            binaryWriter.Write(行会事记[i].第四参数);
            binaryWriter.Write(行会事记[i].事记时间);
        }
        return memoryStream.ToArray();
    }
}
