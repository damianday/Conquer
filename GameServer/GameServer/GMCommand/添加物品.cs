using GameServer.Database;
using GameServer.Template;
using GameServer.Networking;

using GamePackets.Server;

namespace GameServer;

public sealed class 添加物品 : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string 角色名字;

    [FieldDescription(0, Index = 1)]
    public string 物品名字;

    [FieldDescription(0, Index = 2)]
    public int 物品数量;

    public override ExecutionPriority Priority => ExecutionPriority.ImmediateBackground;

    public override void ExecuteCommand()
    {
        if (Session.CharacterInfoTable.SearchTable.TryGetValue(角色名字, out var value) && value is CharacterInfo 角色数据)
        {
            if (!GameItem.DataSheetByName.TryGetValue(物品名字, out var value2))
            {
                SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 物品不存在");
                return;
            }
            if (角色数据.Inventory.Count >= 角色数据.InventorySize.V)
            {
                SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 角色背包已满");
                return;
            }
            if (value2.MaxDura == 0)
            {
                SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 不能添加物品");
                return;
            }
            byte b = byte.MaxValue;
            byte b2 = 0;
            while (b2 < 角色数据.InventorySize.V)
            {
                if (角色数据.Inventory.ContainsKey(b2))
                {
                    b2 = (byte)(b2 + 1);
                    continue;
                }
                b = b2;
                break;
            }
            if (value2 is EquipmentItem 模板)
            {
                角色数据.Inventory[b] = new EquipmentInfo(模板, 角色数据, 1, b, random: true);
            }
            else if (value2.PersistType == PersistentItemType.容器)
            {
                角色数据.Inventory[b] = new ItemInfo(value2, 角色数据, 1, b, 0);
            }
            else if (value2.PersistType == PersistentItemType.Stack)
            {
                角色数据.Inventory[b] = new ItemInfo(value2, 角色数据, 1, b, 1);
            }
            else
            {
                角色数据.Inventory[b] = new ItemInfo(value2, 角色数据, 1, b, value2.MaxDura);
            }
            if (物品数量 > 1)
            {
                角色数据.Inventory[b].Dura.V = 物品数量;
            }
            角色数据.Enqueue(new SyncItemPacket
            {
                Description = 角色数据.Inventory[b].ToArray()
            });
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令已经执行, 物品已经添加到角色背包");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " 命令执行失败, 角色不存在");
        }
    }
}
