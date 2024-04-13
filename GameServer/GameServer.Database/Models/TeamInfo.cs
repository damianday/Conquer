using System;
using System.Collections.Generic;
using System.IO;

using GameServer.Networking;

using GamePackets;

namespace GameServer.Database;

public sealed class TeamInfo : DBObject
{
    public readonly DataMonitor<byte> Distribution;
    public readonly DataMonitor<CharacterInfo> TeamCaptain;
    public readonly HashMonitor<CharacterInfo> Members;

    public Dictionary<CharacterInfo, DateTime> Applications = new Dictionary<CharacterInfo, DateTime>();
    public Dictionary<CharacterInfo, DateTime> Invitations = new Dictionary<CharacterInfo, DateTime>();

    public int TeamID => Index.V;
    public int CaptainObjectID => TeamCaptain.V.Index.V;
    public int MemberCount => Members.Count;
    public byte PickUpMethod => Distribution.V;
    public string CaptainName => Captain.UserName.V;

    public CharacterInfo Captain
    {
        get { return TeamCaptain.V; }
        set
        {
            if (TeamCaptain.V.Index.V != value.Index.V)
                TeamCaptain.V = value;
        }
    }

    public override string ToString() => Captain?.UserName?.V;

    public TeamInfo()
    {
    }

    public TeamInfo(CharacterInfo captain, byte method)
    {
        TeamCaptain.V = captain;
        Distribution.V = method;
        
        Members.Add(captain);
        Session.TeamInfoTable.Add(this, true);
    }

    public override void Remove()
    {
        foreach (var member in Members)
            member.CurrentTeam = null;
        base.Remove();
    }

    public void Broadcast(GamePacket p)
    {
        foreach (var member in Members)
            member.Enqueue(p);
    }

    public byte[] 队伍描述()
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        writer.Write(Index.V);
        writer.Write(Captain.NameDescription());
        writer.Seek(36, SeekOrigin.Begin);
        writer.Write(PickUpMethod);
        writer.Write(CaptainObjectID);
        writer.Write(11);
        writer.Write((ushort)Members.Count);
        writer.Write(0);
        foreach (var member in Members)
        {
            writer.Write(队友描述(member));
        }
        return ms.ToArray();
    }

    public byte[] 队友描述(CharacterInfo member)
    {
        using var ms = new MemoryStream(new byte[39]);
        using var writer = new BinaryWriter(ms);
        writer.Write(member.Index.V);
        writer.Write(member.NameDescription());
        writer.Seek(36, SeekOrigin.Begin);
        writer.Write((byte)member.Gender.V);
        writer.Write((byte)member.Job.V);
        writer.Write((byte)((member.Connection == null) ? 3u : 0u));
        return ms.ToArray();
    }
}
