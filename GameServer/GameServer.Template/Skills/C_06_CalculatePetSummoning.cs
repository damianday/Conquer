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
}
