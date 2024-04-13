using System.Collections.Generic;
using System.IO;

namespace GameServer.Template;

public sealed class GuardInfo
{
    public static Dictionary<ushort, GuardInfo> DataSheet;

    public string Name;
    public ushort GuardID;
    public byte Level;
    public bool Nothingness;
    public bool CanBeInjured;
    public int CorpsePreservation;
    public int RevivalInterval;
    public bool ActiveAttack;
    public byte RangeHate;
    public string BasicAttackSkills;
    public int StoreID;
    public string InterfaceCode;

    public static void LoadData()
    {
        DataSheet = new Dictionary<ushort, GuardInfo>();

        var path = Config.GameDataPath + "\\System\\Npc\\Guards\\";
        if (!Directory.Exists(path))
            return;

        var array = Serializer.Deserialize<GuardInfo>(path);
        foreach (var obj in array)
            DataSheet.Add(obj.GuardID, obj);
    }
}
