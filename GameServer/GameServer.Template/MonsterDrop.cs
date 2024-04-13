namespace GameServer.Template;

public class MonsterDrop
{
    public string Name;
    public string MonsterName;
    public int 爆率分组;
    public int Probability;
    public int MinAmount;
    public int MaxAmount;

    public override string ToString()
    {
        return $"{MonsterName} - {Name} - {爆率分组} - {Probability} - {MinAmount}/{MaxAmount}";
    }
}
