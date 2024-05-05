using GameServer.Database;
using GameServer.Template;
using GameServer.Networking;

using GamePackets.Server;

namespace GameServer;

public sealed class AddItem : GMCommand
{
    [FieldDescription(0, Index = 0)]
    public string UserName;

    [FieldDescription(0, Index = 1)]
    public string ItemName;

    [FieldDescription(0, Index = 2)]
    public int Quantity;

    public override ExecuteCondition Priority => ExecuteCondition.Normal;
    public override UserDegree Degree => UserDegree.Admin;

    public override void ExecuteCommand()
    {
        var character = Session.GetCharacter(UserName);
        if (character != null)
        {
            if (!GameItem.DataSheetByName.TryGetValue(ItemName, out var item))
            {
                SMain.AddCommandLog("<= @" + GetType().Name + " Command failed. Item does not exist.");
                return;
            }
            if (character.Inventory.Count >= character.InventorySize.V)
            {
                SMain.AddCommandLog("<= @" + GetType().Name + " Command failed. Character inventory is full.");
                return;
            }
            if (item.MaxDura == 0)
            {
                SMain.AddCommandLog("<= @" + GetType().Name + " Failed to execute command, can't add item");
                return;
            }
            byte position = byte.MaxValue;
            for (byte i = 0; i < character.InventorySize.V; i++)
            {
                if (!character.Inventory.ContainsKey(i))
                {
                    position = i;
                    break;
                }
            }

            if (item is EquipmentItem equip)
            {
                character.Inventory[position] = new EquipmentInfo(equip, character, 1, position, random: true);
            }
            else if (item.PersistType == PersistentItemType.容器)
            {
                character.Inventory[position] = new ItemInfo(item, character, 1, position, 0);
            }
            else if (item.PersistType == PersistentItemType.Stack)
            {
                character.Inventory[position] = new ItemInfo(item, character, 1, position, 1);
            }
            else
            {
                character.Inventory[position] = new ItemInfo(item, character, 1, position, item.MaxDura);
            }
            if (Quantity > 1)
            {
                character.Inventory[position].Dura.V = Quantity;
            }
            character.Enqueue(new SyncItemPacket
            {
                Description = character.Inventory[position].ToArray()
            });
            SMain.AddCommandLog("<= @" + GetType().Name + " Command executed. Item added to character's inventory.");
        }
        else
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " Command failed, character does not exist");
        }
    }
}
