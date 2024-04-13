namespace GameServer.Template;

public class 药品恢复
{
    public string 物品名字;

    public string 物品规格;

    public int 药品血量;

    public int 药品魔量;

    public int 使用间隔;

    public int 药品模式;

    public override string ToString()
    {
        return $"{物品名字} - {物品规格} - {药品血量} - {药品魔量} - {使用间隔} - {药品模式}";
    }
}
