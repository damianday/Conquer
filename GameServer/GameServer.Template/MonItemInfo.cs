namespace GameServer.Template;

public class MonItemInfo
{
    public string Name;
    public string MonsterName;
    public int 爆率分组;
    public int SelPoint = 1;
    public int MaxPoint;
    public int MinAmount;
    public int MaxAmount;
    public int DropSet;

    public override string ToString()
    {
        return $"{MonsterName} - {Name} - {爆率分组} - {SelPoint}/{MaxPoint} - {MinAmount}/{MaxAmount}";
    }
}
