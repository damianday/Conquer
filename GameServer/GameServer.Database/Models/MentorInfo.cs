using System;
using System.Collections.Generic;
using System.IO;

using GameServer.Networking;

using GamePackets;
using GamePackets.Server;

namespace GameServer.Database;

public sealed class MentorInfo : DBObject
{
    public readonly DataMonitor<CharacterInfo> MasterInfo;
    public readonly HashMonitor<CharacterInfo> StudentsInfo;

    public readonly DictionaryMonitor<CharacterInfo, int> 徒弟经验;
    public readonly DictionaryMonitor<CharacterInfo, int> 徒弟金币;
    public readonly DictionaryMonitor<CharacterInfo, int> 师父经验;
    public readonly DictionaryMonitor<CharacterInfo, int> 师父金币;
    public readonly DictionaryMonitor<CharacterInfo, int> 师父声望;

    public Dictionary<int, DateTime> ApplicationsList;
    public Dictionary<int, DateTime> InvitationList;

    public int MasterID => Master.ID;
    public int StudentCount => StudentsInfo.Count;

    public CharacterInfo Master => MasterInfo.V;

    public MentorInfo()
    {
        ApplicationsList = new Dictionary<int, DateTime>();
        InvitationList = new Dictionary<int, DateTime>();
    }

    public MentorInfo(CharacterInfo 师父数据)
    {
        ApplicationsList = new Dictionary<int, DateTime>();
        InvitationList = new Dictionary<int, DateTime>();
        MasterInfo.V = 师父数据;
        Session.MentorInfoTable.Add(this, true);
    }

    public override string ToString()
    {
        return Master?.ToString();
    }

    public override void Remove()
    {
        Master.Mentor.V = null;
        foreach (var student in StudentsInfo)
        {
            student.Mentor.V = null;
        }
        base.Remove();
    }

    public int 徒弟提供经验(CharacterInfo character)
    {
        if (师父经验.TryGetValue(character, out var v))
            return v;
        return 0;
    }

    public int 徒弟提供金币(CharacterInfo character)
    {
        if (师父金币.TryGetValue(character, out var v))
            return v;
        return 0;
    }

    public int 徒弟提供声望(CharacterInfo character)
    {
        if (师父声望.TryGetValue(character, out var v))
            return v;
        return 0;
    }

    public int 徒弟出师经验(CharacterInfo character)
    {
        if (徒弟经验.TryGetValue(character, out var v))
            return v;
        return 0;
    }

    public int 徒弟出师金币(CharacterInfo character)
    {
        if (徒弟金币.TryGetValue(character, out var v))
            return v;
        return 0;
    }

    public void Broadcast(GamePacket p)
    {
        foreach (var student in StudentsInfo)
            student.Enqueue(p);
    }

    public void 添加徒弟(CharacterInfo character)
    {
        StudentsInfo.Add(character);
        徒弟经验.Add(character, 0);
        徒弟金币.Add(character, 0);
        师父经验.Add(character, 0);
        师父金币.Add(character, 0);
        师父声望.Add(character, 0);
        character.CurrentMentor = this;
        foreach (var student in StudentsInfo)
        {
            student?.Enqueue(new 同步师门成员
            {
                字节数据 = 成员数据()
            });
        }
    }

    public void 移除徒弟(CharacterInfo character)
    {
        StudentsInfo.Remove(character);
        徒弟经验.Remove(character);
        徒弟金币.Remove(character);
        师父经验.Remove(character);
        师父金币.Remove(character);
        师父声望.Remove(character);
        foreach (var student in StudentsInfo)
        {
            student?.Enqueue(new 同步师门成员
            {
                字节数据 = 成员数据()
            });
        }
    }

    public byte[] 奖励数据(CharacterInfo character)
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        if (character == Master)
        {
            writer.Seek(12, SeekOrigin.Begin);
            foreach (var student in StudentsInfo)
            {
                writer.Write(student.ID);
                writer.Write(徒弟提供经验(student));
                writer.Write(徒弟提供声望(student));
                writer.Write(徒弟提供金币(student));
            }
        }
        else
        {
            writer.Write(MasterID);
            writer.Write(徒弟出师经验(character));
            writer.Write(徒弟出师金币(character));
        }
        return ms.ToArray();
    }

    public byte[] 成员数据()
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        writer.Write(MasterID);
        writer.Write(Master.CurrentLevel);
        writer.Write((byte)StudentsInfo.Count);
        foreach (var student in StudentsInfo)
        {
            writer.Write(student.ID);
            writer.Write(student.CurrentLevel);
            writer.Write(student.CurrentLevel); // TODO: BUG? sending level twice
        }
        return ms.ToArray();
    }
}
