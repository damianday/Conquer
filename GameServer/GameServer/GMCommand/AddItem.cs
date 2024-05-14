using System;
using GameServer.Database;
using GameServer.Template;

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
        if (character == null)
        {
            SMain.AddCommandLog("<= @" + GetType().Name + " Command failed, character does not exist");
            return;
        }

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

        int count = ((Quantity == 1 || item.PersistType != PersistentItemType.Stack) ? 1 : Math.Min(Quantity, item.MaxDura));

        byte position = character.FindEmptyInventoryIndex();

        if (item is EquipmentItem equipment)
        {
            character.Inventory[position] = new EquipmentInfo(equipment, character, 1, position);
        }
        else
        {
            int dura = 0;
            switch (item.PersistType)
            {
                case PersistentItemType.Stack:
                    dura = count;
                    break;
                case PersistentItemType.Container:
                    dura = 0;
                    break;
                case PersistentItemType.Consumeable:
                case PersistentItemType.Purity:
                    dura = item.MaxDura;
                    break;
            }
            character.Inventory[position] = new ItemInfo(item, character, 1, position, dura);
        }

        character.Enqueue(new SyncItemPacket
        {
            Description = character.Inventory[position].ToArray()
        });
        SMain.AddCommandLog("<= @" + GetType().Name + " Command executed. Item added to character's inventory.");
    }
}
