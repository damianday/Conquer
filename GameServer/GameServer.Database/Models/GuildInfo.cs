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
    public readonly DataMonitor<string> 创建人名;
    public readonly DataMonitor<string> 行会宣言;
    public readonly DataMonitor<string> 行会公告;
    public readonly DataMonitor<byte> 行会等级;
    public readonly DataMonitor<int> 行会资金;
    public readonly DataMonitor<int> 粮食数量;
    public readonly DataMonitor<int> 木材数量;
    public readonly DataMonitor<int> 石材数量;
    public readonly DataMonitor<int> 铁矿数量;
    public readonly DataMonitor<int> 行会排名;
    public readonly ListMonitor<行会事记> 行会事记;
    public readonly DictionaryMonitor<CharacterInfo, GuildRank> Members;
    public readonly DictionaryMonitor<CharacterInfo, DateTime> 行会禁言;
    public readonly DictionaryMonitor<GuildInfo, DateTime> AllianceGuilds;
    public readonly DictionaryMonitor<GuildInfo, DateTime> HostileGuilds;

    public Dictionary<CharacterInfo, DateTime> 申请列表;
    public Dictionary<CharacterInfo, DateTime> 邀请列表;
    public Dictionary<GuildInfo, 外交申请> 结盟申请;
    public Dictionary<GuildInfo, DateTime> 解除申请;
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

    public DateTime 清理时间 { get; set; }

    public override string ToString()
    {
        return GuildName?.V;
    }

    public GuildInfo()
    {
        申请列表 = new Dictionary<CharacterInfo, DateTime>();
        邀请列表 = new Dictionary<CharacterInfo, DateTime>();
        结盟申请 = new Dictionary<GuildInfo, 外交申请>();
        解除申请 = new Dictionary<GuildInfo, DateTime>();
    }

    public GuildInfo(PlayerObject 创建玩家, string 行会名字, string 行会宣言)
    {
        申请列表 = new Dictionary<CharacterInfo, DateTime>();
        邀请列表 = new Dictionary<CharacterInfo, DateTime>();
        结盟申请 = new Dictionary<GuildInfo, 外交申请>();
        解除申请 = new Dictionary<GuildInfo, DateTime>();
        this.GuildName.V = 行会名字;
        this.行会宣言.V = 行会宣言;
        行会公告.V = "祝大家游戏愉快.";
        President.V = 创建玩家.Character;
        创建人名.V = 创建玩家.Name;
        Members.Add(创建玩家.Character, GuildRank.President);
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.创建公会,
            第一参数 = 创建玩家.ObjectID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.加入公会,
            第一参数 = 创建玩家.ObjectID,
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
        SystemInfo.Info.更新行会(this);
    }

    public void 清理数据()
    {
        if (!(SEngine.CurrentTime > 清理时间))
        {
            return;
        }
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
        foreach (KeyValuePair<CharacterInfo, DateTime> item3 in 申请列表.ToList())
        {
            if (SEngine.CurrentTime > item3.Value)
            {
                申请列表.Remove(item3.Key);
            }
        }
        foreach (KeyValuePair<CharacterInfo, DateTime> item4 in 邀请列表.ToList())
        {
            if (SEngine.CurrentTime > item4.Value)
            {
                邀请列表.Remove(item4.Key);
            }
        }
        foreach (KeyValuePair<GuildInfo, DateTime> item5 in 解除申请.ToList())
        {
            if (SEngine.CurrentTime > item5.Value)
            {
                解除申请.Remove(item5.Key);
            }
        }
        foreach (KeyValuePair<GuildInfo, 外交申请> item6 in 结盟申请.ToList())
        {
            if (SEngine.CurrentTime > item6.Value.申请时间)
            {
                结盟申请.Remove(item6.Key);
            }
        }
        清理时间 = SEngine.CurrentTime.AddSeconds(1.0);
    }

    public void 解散行会()
    {
        foreach (KeyValuePair<DateTime, GuildInfo> item in SystemInfo.Info.申请行会.ToList())
        {
            if (item.Value == this)
            {
                SystemInfo.Info.申请行会.Remove(item.Key);
            }
        }
        Broadcast(new 脱离行会应答
        {
            脱离方式 = 2
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
            SystemInfo.Info.行会人数排名.RemoveAt(行会排名.V - 1);
            for (int i = 行会排名.V - 1; i < SystemInfo.Info.行会人数排名.Count; i++)
            {
                SystemInfo.Info.行会人数排名[i].行会排名.V = i + 1;
            }
        }
        Members.Clear();
        行会禁言.Clear();
        Remove();
    }

    public void Broadcast(GamePacket p)
    {
        foreach (var member in Members.Keys)
        {
            member.Enqueue(p);
        }
    }

    public void 添加成员(CharacterInfo 成员, GuildRank 职位 = GuildRank.会员)
    {
        Members.Add(成员, 职位);
        成员.CurrentGuild = this;
        Broadcast(new 行会加入成员
        {
            对象编号 = 成员.ID,
            对象名字 = 成员.UserName.V,
            对象职位 = 7,
            对象等级 = 成员.CurrentLevel,
            对象职业 = (byte)成员.Job.V,
            当前地图 = (byte)成员.CurrentMap.V
        });
        if (成员.Connection == null)
        {
            Broadcast(new SyncMemberInfoPacket
            {
                ObjectID = 成员.ID,
                对象信息 = Compute.TimeSeconds(成员.DisconnectDate.V)
            });
        }
        成员.Enqueue(new 行会信息公告
        {
            Description = 行会信息描述()
        });
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.加入公会,
            第一参数 = 成员.ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        if (MapManager.Players.TryGetValue(成员.ID, out var value))
        {
            value.SendPacket(new 同步对象行会
            {
                对象编号 = 成员.ID,
                行会编号 = ID
            });
        }
        SystemInfo.Info.更新行会(this);
    }

    public void 退出行会(CharacterInfo 成员)
    {
        Members.Remove(成员);
        行会禁言.Remove(成员);
        成员.CurrentGuild = null;
        成员.Enqueue(new 脱离行会应答
        {
            脱离方式 = 1
        });
        Broadcast(new 脱离行会公告
        {
            对象编号 = 成员.ID
        });
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.离开公会,
            第一参数 = 成员.ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        if (MapManager.Players.TryGetValue(成员.ID, out var value))
        {
            value.SendPacket(new 同步对象行会
            {
                对象编号 = 成员.ID
            });
        }
        SystemInfo.Info.更新行会(this);
    }

    public void 逐出成员(CharacterInfo 主事, CharacterInfo 成员)
    {
        if (Members.Remove(成员))
        {
            行会禁言.Remove(成员);
            成员.CurrentGuild = null;
            成员.Enqueue(new 脱离行会应答
            {
                脱离方式 = 2
            });
            Broadcast(new 脱离行会公告
            {
                对象编号 = 成员.ID
            });
            添加事记(new 行会事记
            {
                事记类型 = 事记类型.逐出公会,
                第一参数 = 成员.ID,
                第二参数 = 主事.ID,
                事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
            });
            if (MapManager.Players.TryGetValue(成员.ID, out var value))
            {
                value.SendPacket(new 同步对象行会
                {
                    对象编号 = 成员.ID
                });
            }
            SystemInfo.Info.更新行会(this);
        }
    }

    public void 更改职位(CharacterInfo 主事, CharacterInfo 成员, GuildRank 职位)
    {
        GuildRank 行会职位3 = (Members[成员] = 职位);
        GuildRank 行会职位4 = 行会职位3;
        GuildRank 行会职位5 = 行会职位4;
        GuildRank 行会职位6 = 行会职位5;
        Members[成员] = 职位;
        Broadcast(new 变更职位公告
        {
            对象编号 = 成员.ID,
            对象职位 = (byte)职位
        });
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.变更职位,
            第一参数 = 主事.ID,
            第二参数 = 成员.ID,
            第三参数 = (byte)行会职位6,
            第四参数 = (byte)职位,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
    }

    public void 更改宣言(CharacterInfo 主事, string 宣言)
    {
        行会宣言.V = 宣言;
        主事.Enqueue(new SocialErrorPacket
        {
            ErrorCode = 6747
        });
    }

    public void 更改公告(string 公告)
    {
        行会公告.V = 公告;
        Broadcast(new 变更行会公告
        {
            字节数据 = Encoding.UTF8.GetBytes(公告 + "\0")
        });
    }

    public void 转移会长(CharacterInfo 会长, CharacterInfo 成员)
    {
        President.V = 成员;
        Members[会长] = GuildRank.会员;
        Members[成员] = GuildRank.President;
        Broadcast(new 会长传位公告
        {
            当前编号 = 会长.ID,
            传位编号 = 成员.ID
        });
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.会长传位,
            第一参数 = 会长.ID,
            第二参数 = 成员.ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
    }

    public void 成员禁言(CharacterInfo 主事, CharacterInfo 成员, byte 禁言状态)
    {
        if (禁言状态 == 2 && 行会禁言.Remove(成员))
        {
            Broadcast(new 行会禁言公告
            {
                对象编号 = 成员.ID,
                禁言状态 = 2
            });
        }
        else if (禁言状态 == 1)
        {
            行会禁言[成员] = SEngine.CurrentTime;
            Broadcast(new 行会禁言公告
            {
                对象编号 = 成员.ID,
                禁言状态 = 1
            });
        }
        else
        {
            主事.Enqueue(new SocialErrorPacket
            {
                ErrorCode = 6680
            });
        }
    }

    public void 申请结盟(CharacterInfo 主事, GuildInfo 行会, byte 时间参数)
    {
        主事.Enqueue(new 申请结盟应答
        {
            行会编号 = 行会.ID
        });
        if (!行会.结盟申请.ContainsKey(this))
        {
            行会.行会提醒(GuildRank.副长, 2);
        }
        行会.结盟申请[this] = new 外交申请
        {
            外交时间 = 时间参数,
            申请时间 = SEngine.CurrentTime.AddHours(10.0)
        };
    }

    public void 行会敌对(GuildInfo 行会, byte 时间参数)
    {
        DictionaryMonitor<GuildInfo, DateTime> 字典监视器2 = HostileGuilds;
        DictionaryMonitor<GuildInfo, DateTime> 字典监视器3 = 行会.HostileGuilds;
        DictionaryMonitor<GuildInfo, DateTime> 字典监视器4 = 字典监视器3;
        if (1 == 0)
        {
        }
        int num = 时间参数 switch
        {
            2 => 3,
            1 => 1,
            _ => 7,
        };
        if (1 == 0)
        {
        }
        DateTime dateTime2 = (字典监视器4[this] = SEngine.CurrentTime.AddDays(num));
        DateTime dateTime3 = dateTime2;
        DateTime dateTime5 = (字典监视器2[行会] = dateTime3);
        DateTime dateTime6 = dateTime5;
        DateTime dateTime7 = dateTime6;
        Broadcast(new 添加外交公告
        {
            外交类型 = 2,
            行会编号 = 行会.ID,
            行会名字 = 行会.GuildName.V,
            行会等级 = 行会.行会等级.V,
            行会人数 = (byte)行会.Members.Count,
            外交时间 = (int)(HostileGuilds[行会] - SEngine.CurrentTime).TotalSeconds
        });
        行会.Broadcast(new 添加外交公告
        {
            外交类型 = 2,
            行会编号 = ID,
            行会名字 = GuildName.V,
            行会等级 = 行会等级.V,
            行会人数 = (byte)Members.Count,
            外交时间 = (int)(行会.HostileGuilds[this] - SEngine.CurrentTime).TotalSeconds
        });
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.行会敌对,
            第一参数 = ID,
            第二参数 = 行会.ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        行会.添加事记(new 行会事记
        {
            事记类型 = 事记类型.行会敌对,
            第一参数 = 行会.ID,
            第二参数 = ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
    }

    public void 行会结盟(GuildInfo 行会)
    {
        DictionaryMonitor<GuildInfo, DateTime> 字典监视器2 = AllianceGuilds;
        DateTime dateTime2 = (行会.AllianceGuilds[this] = SEngine.CurrentTime.AddDays((结盟申请[行会].外交时间 == 1) ? 1 : ((结盟申请[行会].外交时间 == 2) ? 3 : 7)));
        DateTime dateTime3 = dateTime2;
        dateTime2 = (字典监视器2[行会] = dateTime3);
        DateTime dateTime5 = dateTime2;
        DateTime dateTime6 = dateTime5;
        Broadcast(new 添加外交公告
        {
            外交类型 = 1,
            行会名字 = 行会.GuildName.V,
            行会编号 = 行会.ID,
            行会等级 = 行会.行会等级.V,
            行会人数 = (byte)行会.Members.Count,
            外交时间 = (int)(AllianceGuilds[行会] - SEngine.CurrentTime).TotalSeconds
        });
        行会.Broadcast(new 添加外交公告
        {
            外交类型 = 1,
            行会名字 = GuildName.V,
            行会编号 = ID,
            行会等级 = 行会等级.V,
            行会人数 = (byte)Members.Count,
            外交时间 = (int)(行会.AllianceGuilds[this] - SEngine.CurrentTime).TotalSeconds
        });
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.行会结盟,
            第一参数 = ID,
            第二参数 = 行会.ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        行会.添加事记(new 行会事记
        {
            事记类型 = 事记类型.行会结盟,
            第一参数 = 行会.ID,
            第二参数 = ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
    }

    public void 解除结盟(CharacterInfo 主事, GuildInfo 行会)
    {
        AllianceGuilds.Remove(行会);
        行会.AllianceGuilds.Remove(this);
        Broadcast(new 删除外交公告
        {
            外交类型 = 1,
            行会编号 = 行会.ID
        });
        行会.Broadcast(new 删除外交公告
        {
            外交类型 = 1,
            行会编号 = ID
        });
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.取消结盟,
            第一参数 = ID,
            第二参数 = 行会.ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        行会.添加事记(new 行会事记
        {
            事记类型 = 事记类型.取消结盟,
            第一参数 = 行会.ID,
            第二参数 = ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        主事.Enqueue(new SocialErrorPacket
        {
            ErrorCode = 6812
        });
    }

    public void 申请解敌(CharacterInfo 主事, GuildInfo 敌对行会)
    {
        主事.Enqueue(new SocialErrorPacket
        {
            ErrorCode = 6829
        });
        foreach (KeyValuePair<CharacterInfo, GuildRank> item in 敌对行会.Members)
        {
            if (item.Value <= GuildRank.副长)
            {
                item.Key.Enqueue(new 解除敌对列表
                {
                    申请类型 = 1,
                    行会编号 = ID
                });
            }
        }
        敌对行会.解除申请[this] = SEngine.CurrentTime.AddHours(10.0);
    }

    public void 解除敌对(GuildInfo 行会)
    {
        HostileGuilds.Remove(行会);
        行会.HostileGuilds.Remove(this);
        Broadcast(new 解除敌对列表
        {
            申请类型 = 2,
            行会编号 = 行会.ID
        });
        Broadcast(new 删除外交公告
        {
            外交类型 = 2,
            行会编号 = 行会.ID
        });
        行会.Broadcast(new 删除外交公告
        {
            外交类型 = 2,
            行会编号 = ID
        });
        添加事记(new 行会事记
        {
            事记类型 = 事记类型.取消敌对,
            第一参数 = ID,
            第二参数 = 行会.ID,
            事记时间 = Compute.TimeSeconds(SEngine.CurrentTime)
        });
        行会.添加事记(new 行会事记
        {
            事记类型 = 事记类型.取消敌对,
            第一参数 = 行会.ID,
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

    public void 行会提醒(GuildRank 职位, byte 提醒类型)
    {
        foreach (KeyValuePair<CharacterInfo, GuildRank> item in Members)
        {
            if (item.Value <= 职位 && item.Key.CheckOnline(out var 网络))
            {
                网络.SendPacket(new 发送行会通知
                {
                    提醒类型 = 提醒类型
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
        Encoding.UTF8.GetBytes(创建人名.V).CopyTo(array, 0);
        binaryWriter.Write(array);
        binaryWriter.Write(创建时间);
        array = new byte[101];
        Encoding.UTF8.GetBytes(行会宣言.V).CopyTo(array, 0);
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
        binaryWriter.Write(Encoding.UTF8.GetBytes(创建人名.V));
        binaryWriter.Seek(107, SeekOrigin.Begin);
        binaryWriter.Write(Encoding.UTF8.GetBytes(行会公告.V));
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
            binaryWriter.Write((!item.Key.CheckOnline(out var _)) ? Compute.TimeSeconds(item.Key.DisconnectDate.V) : 0);
            binaryWriter.Write(0);
            binaryWriter.Write(行会禁言.ContainsKey(item.Key));
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
        binaryWriter.Write((ushort)申请列表.Count);
        foreach (CharacterInfo key in 申请列表.Keys)
        {
            binaryWriter.Write(key.ID);
            byte[] array = new byte[32];
            Encoding.UTF8.GetBytes(key.UserName.V).CopyTo(array, 0);
            binaryWriter.Write(array);
            binaryWriter.Write(key.CurrentLevel);
            binaryWriter.Write(key.CurrentLevel);
            binaryWriter.Write(key.CheckOnline(out var _) ? Compute.TimeSeconds(SEngine.CurrentTime) : Compute.TimeSeconds(key.DisconnectDate.V));
        }
        return memoryStream.ToArray();
    }

    public byte[] 结盟申请描述()
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
        binaryWriter.Write((byte)结盟申请.Count);
        foreach (KeyValuePair<GuildInfo, 外交申请> item in 结盟申请)
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
        foreach (KeyValuePair<GuildInfo, DateTime> item in 解除申请)
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
