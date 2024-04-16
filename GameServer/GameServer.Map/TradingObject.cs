using System;
using System.Collections.Generic;
using System.Linq;

using GameServer.Database;
using GameServer.Networking;

using GamePackets;
using GamePackets.Server;

namespace GameServer.Map;

public sealed class TradingObject
{
    public PlayerObject Requester;
    public PlayerObject Recipient;

    public byte RequesterState;
    public byte RecipientState;

    public int RequesterGold;
    public int RecipientGold;

    public Dictionary<byte, ItemInfo> RequesterItems;
    public Dictionary<byte, ItemInfo> RecipientItems;

    public TradingObject(PlayerObject requester, PlayerObject recipient)
    {
        RequesterItems = new Dictionary<byte, ItemInfo>();
        RecipientItems = new Dictionary<byte, ItemInfo>();
        Requester = requester;
        Recipient = recipient;
        RequesterState = 1;
        RecipientState = 2;
        Enqueue(new 交易状态改变
        {
            对象编号 = Requester.ObjectID,
            交易状态 = RequesterState,
            对象等级 = Requester.CurrentLevel
        });
        Enqueue(new 交易状态改变
        {
            对象编号 = Recipient.ObjectID,
            交易状态 = RecipientState,
            对象等级 = Recipient.CurrentLevel
        });
    }

    public void BreakTrade()
    {
        Requester.Enqueue(new 交易状态改变
        {
            对象编号 = Requester.ObjectID,
            交易状态 = 0,
            对象等级 = Requester.CurrentLevel
        });
        Recipient.Enqueue(new 交易状态改变
        {
            对象编号 = Recipient.ObjectID,
            交易状态 = 0,
            对象等级 = Recipient.CurrentLevel
        });
        Requester.CurrentTrade = (Recipient.CurrentTrade = null);
    }

    public void ExchangeItems()
    {
        if (RecipientGold > 0)
        {
            Recipient.Gold -= (int)Math.Ceiling((float)RecipientGold * 1.04f);
            Recipient.Character.TradeGold.V += RecipientGold;
        }

        if (RequesterGold > 0)
        {
            Requester.Gold -= (int)Math.Ceiling((float)RequesterGold * 1.04f);
            Requester.Character.TradeGold.V += RequesterGold;
        }

        foreach (var item in RecipientItems.Values)
        {
            if (item.ID == 80207)
            {
                Recipient.Character.TradeGold.V += 1_000_000L;
            }
            else if (item.ID == 80209)
            {
                Recipient.Character.TradeGold.V += 5_000_000L;
            }
            Recipient.Inventory.Remove(item.Location.V);
            Recipient.Enqueue(new DeleteItemPacket
            {
                Grid = 1,
                Position = item.Location.V
            });
        }

        foreach (var item in RequesterItems.Values)
        {
            if (item.ID == 80207)
            {
                Requester.Character.TradeGold.V += 1_000_000L;
            }
            else if (item.ID == 80209)
            {
                Requester.Character.TradeGold.V += 5_000_000L;
            }
            Requester.Inventory.Remove(item.Location.V);
            Requester.Enqueue(new DeleteItemPacket
            {
                Grid = 1,
                Position = item.Location.V
            });
        }

        foreach (var item in RequesterItems.Values)
        {
            for (byte b = 0; b < Recipient.InventorySize; b = (byte)(b + 1))
            {
                if (!Recipient.Inventory.ContainsKey(b))
                {
                    Recipient.Inventory.Add(b, item);
                    item.Grid.V = 1;
                    item.Location.V = b;
                    Recipient.Enqueue(new SyncItemPacket
                    {
                        Description = item.ToArray()
                    });
                    break;
                }
            }
        }

        foreach (var item in RecipientItems.Values)
        {
            for (byte b2 = 0; b2 < Requester.InventorySize; b2 = (byte)(b2 + 1))
            {
                if (!Requester.Inventory.ContainsKey(b2))
                {
                    Requester.Inventory.Add(b2, item);
                    item.Grid.V = 1;
                    item.Location.V = b2;
                    Requester.Enqueue(new SyncItemPacket
                    {
                        Description = item.ToArray()
                    });
                    break;
                }
            }
        }

        if (RequesterGold > 0)
        {
            Recipient.Gold += RequesterGold;
        }

        if (RecipientGold > 0)
        {
            Requester.Gold += RecipientGold;
        }

        UpdateState(6);
        BreakTrade();
    }

    public void UpdateState(byte state, PlayerObject player = null)
    {
        if (player == null)
        {
            RequesterState = (RecipientState = state);
            Enqueue(new 交易状态改变
            {
                对象编号 = Requester.ObjectID,
                交易状态 = RequesterState,
                对象等级 = Requester.CurrentLevel
            });
            Enqueue(new 交易状态改变
            {
                对象编号 = Recipient.ObjectID,
                交易状态 = RecipientState,
                对象等级 = Recipient.CurrentLevel
            });
        }
        else if (player == Requester)
        {
            RequesterState = state;
            Enqueue(new 交易状态改变
            {
                对象编号 = player.ObjectID,
                交易状态 = player.TradeState,
                对象等级 = player.CurrentLevel
            });
        }
        else if (player == Recipient)
        {
            RecipientState = state;
            Enqueue(new 交易状态改变
            {
                对象编号 = player.ObjectID,
                交易状态 = player.TradeState,
                对象等级 = player.CurrentLevel
            });
        }
        else
        {
            BreakTrade();
        }
    }

    public void AddGold(PlayerObject player, int quantity)
    {
        if (player == Requester)
        {
            RequesterGold = quantity;
            Enqueue(new TradeAddGoldPacket
            {
                对象编号 = player.ObjectID,
                金币数量 = quantity
            });
        }
        else if (player == Recipient)
        {
            RecipientGold = quantity;
            Enqueue(new TradeAddGoldPacket
            {
                对象编号 = player.ObjectID,
                金币数量 = quantity
            });
        }
        else
        {
            BreakTrade();
        }
    }

    public void AddItem(PlayerObject player, ItemInfo item, byte location)
    {
        if (player == Requester)
        {
            RequesterItems.Add(location, item);
            Enqueue(new TradeAddItemPacket
            {
                ObjectID = player.ObjectID,
                Location = location,
                放入物品 = 1,
                Description = item.ToArray()
            });
        }
        else if (player == Recipient)
        {
            RecipientItems.Add(location, item);
            Enqueue(new TradeAddItemPacket
            {
                ObjectID = player.ObjectID,
                Location = location,
                放入物品 = 1,
                Description = item.ToArray()
            });
        }
        else
        {
            BreakTrade();
        }
    }

    public bool 背包已满(out PlayerObject player)
    {
        player = null;
        if (Requester.RemainingInventorySpace < RecipientItems.Count)
        {
            player = Requester;
            return true;
        }
        if (Recipient.RemainingInventorySpace < RequesterItems.Count)
        {
            player = Recipient;
            return true;
        }
        return false;
    }

    public bool 金币重复(PlayerObject player)
    {
        if (player == Requester)
            return RequesterGold != 0;
        if (player == Recipient)
            return RecipientGold != 0;
        return true;
    }

    public bool 物品重复(PlayerObject player, ItemInfo item)
    {
        if (player == Requester)
        {
            return RequesterItems.Values.FirstOrDefault((ItemInfo O) => O == item) != null;
        }
        if (player == Recipient)
        {
            return RecipientItems.Values.FirstOrDefault((ItemInfo O) => O == item) != null;
        }
        return true;
    }

    public bool 物品重复(PlayerObject player, byte location)
    {
        if (player == Requester)
        {
            return RequesterItems.ContainsKey(location);
        }
        if (player == Recipient)
        {
            return RecipientItems.ContainsKey(location);
        }
        return true;
    }

    public byte OpponentState(PlayerObject player)
    {
        if (player == Recipient)
            return RequesterState;
        if (player == Requester)
            return RecipientState;
        return 0;
    }

    public void Enqueue(GamePacket packet)
    {
        Recipient.Enqueue(packet);
        Requester.Enqueue(packet);
    }

    public PlayerObject Opponent(PlayerObject player)
    {
        if (player == Recipient)
            return Requester;
        return Recipient;
    }

    public Dictionary<byte, ItemInfo> OpponentItems(PlayerObject player)
    {
        if (player == Recipient)
            return RequesterItems;
        if (player == Requester)
            return RecipientItems;
        return null;
    }
}
