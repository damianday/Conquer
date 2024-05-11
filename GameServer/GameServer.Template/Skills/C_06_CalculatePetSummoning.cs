using System.Collections.Generic;

namespace GameServer.Template;

public sealed class C_06_CalculatePetSummoning : SkillTask
{
    public string PetName;
    public bool Companion;
    public byte[] SpawnCount;
    public byte[] LevelCap;
    public bool GainSkillExp;
    public ushort ExpSkillID;
    public bool PetBoundWeapons;
    public bool CheckSkillInscriptions;

    public ushort PetBindingBuffID; // 宠物绑定BUFF
    //public 范围召唤怪物;
    public List<string> IgnorePetList = new List<string>();
    public int PetSurvivalTime;
    public bool 启用范围召唤;
    public bool 死亡同伴消失;
    public ushort 同伴添加BUFF;
}
