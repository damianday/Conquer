using System;

namespace GameServer.Database;

public sealed class PetInfo : DBObject
{
    public readonly DataMonitor<string> Name;
    public readonly DataMonitor<int> CurrentHP;
    public readonly DataMonitor<int> CurrentExp;
    public readonly DataMonitor<byte> CurrentLevel;
    public readonly DataMonitor<byte> MaxLevel;
    public readonly DataMonitor<bool> BoundWeapon;
    public readonly DataMonitor<DateTime> MutinyTime;

    public PetInfo()
    {
    }

    public PetInfo(string name, byte level, byte levelMax, bool bindWeapon, DateTime mutinyTime)
    {
        Name.V = name;
        CurrentLevel.V = level;
        MaxLevel.V = levelMax;
        BoundWeapon.V = bindWeapon;
        MutinyTime.V = mutinyTime;
        Session.PetInfoTable.Add(this, indexed: true);
    }
}
