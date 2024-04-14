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
    public PlayerObject 交易申请方;
    public PlayerObject 交易接收方;

    public byte 申请方状态;
    public byte 接收方状态;

    public int 申请方金币;
    public int 接收方金币;

    public Dictionary<byte, ItemInfo> 申请方物品;
    public Dictionary<byte, ItemInfo> 接收方物品;

    public TradingObject(PlayerObject 申请方, PlayerObject 接收方)
    {
        申请方物品 = new Dictionary<byte, ItemInfo>();
        接收方物品 = new Dictionary<byte, ItemInfo>();
        交易申请方 = 申请方;
        交易接收方 = 接收方;
        申请方状态 = 1;
        接收方状态 = 2;
        发送封包(new 交易状态改变
        {
            对象编号 = 交易申请方.ObjectID,
            交易状态 = 申请方状态,
            对象等级 = 交易申请方.CurrentLevel
        });
        发送封包(new 交易状态改变
        {
            对象编号 = 交易接收方.ObjectID,
            交易状态 = 接收方状态,
            对象等级 = 交易接收方.CurrentLevel
        });
    }

    public void BreakTrade()
    {
        交易申请方.Enqueue(new 交易状态改变
        {
            对象编号 = 交易申请方.ObjectID,
            交易状态 = 0,
            对象等级 = 交易申请方.CurrentLevel
        });
        交易接收方.Enqueue(new 交易状态改变
        {
            对象编号 = 交易接收方.ObjectID,
            交易状态 = 0,
            对象等级 = 交易接收方.CurrentLevel
        });
        交易申请方.CurrentTrade = (交易接收方.CurrentTrade = null);
    }

    public void 交换物品()
    {
        if (接收方金币 > 0)
        {
            交易接收方.Gold -= (int)Math.Ceiling((float)接收方金币 * 1.04f);
            交易接收方.Character.转出金币.V += 接收方金币;
        }
        if (申请方金币 > 0)
        {
            交易申请方.Gold -= (int)Math.Ceiling((float)申请方金币 * 1.04f);
            交易申请方.Character.转出金币.V += 申请方金币;
        }
        foreach (ItemInfo value in 接收方物品.Values)
        {
            if (value.ID == 80207)
            {
                交易接收方.Character.转出金币.V += 1000000L;
            }
            else if (value.ID == 80209)
            {
                交易接收方.Character.转出金币.V += 5000000L;
            }
            交易接收方.Inventory.Remove(value.Location.V);
            交易接收方.Enqueue(new DeleteItemPacket
            {
                Grid = 1,
                Position = value.Location.V
            });
        }
        foreach (ItemInfo value2 in 申请方物品.Values)
        {
            if (value2.ID == 80207)
            {
                交易申请方.Character.转出金币.V += 1000000L;
            }
            else if (value2.ID == 80209)
            {
                交易申请方.Character.转出金币.V += 5000000L;
            }
            交易申请方.Inventory.Remove(value2.Location.V);
            交易申请方.Enqueue(new DeleteItemPacket
            {
                Grid = 1,
                Position = value2.Location.V
            });
        }
        foreach (ItemInfo value3 in 申请方物品.Values)
        {
            for (byte b = 0; b < 交易接收方.InventorySize; b = (byte)(b + 1))
            {
                if (!交易接收方.Inventory.ContainsKey(b))
                {
                    交易接收方.Inventory.Add(b, value3);
                    value3.Grid.V = 1;
                    value3.Location.V = b;
                    交易接收方.Enqueue(new SyncItemPacket
                    {
                        Description = value3.ToArray()
                    });
                    break;
                }
            }
        }
        foreach (ItemInfo value4 in 接收方物品.Values)
        {
            for (byte b2 = 0; b2 < 交易申请方.InventorySize; b2 = (byte)(b2 + 1))
            {
                if (!交易申请方.Inventory.ContainsKey(b2))
                {
                    交易申请方.Inventory.Add(b2, value4);
                    value4.Grid.V = 1;
                    value4.Location.V = b2;
                    交易申请方.Enqueue(new SyncItemPacket
                    {
                        Description = value4.ToArray()
                    });
                    break;
                }
            }
        }
        if (申请方金币 > 0)
        {
            交易接收方.Gold += 申请方金币;
        }
        if (接收方金币 > 0)
        {
            交易申请方.Gold += 接收方金币;
        }
        更改状态(6);
        BreakTrade();
    }

    public void 更改状态(byte 状态, PlayerObject 玩家 = null)
    {
        if (玩家 == null)
        {
            申请方状态 = (接收方状态 = 状态);
            发送封包(new 交易状态改变
            {
                对象编号 = 交易申请方.ObjectID,
                交易状态 = 申请方状态,
                对象等级 = 交易申请方.CurrentLevel
            });
            发送封包(new 交易状态改变
            {
                对象编号 = 交易接收方.ObjectID,
                交易状态 = 接收方状态,
                对象等级 = 交易接收方.CurrentLevel
            });
        }
        else if (玩家 == 交易申请方)
        {
            申请方状态 = 状态;
            发送封包(new 交易状态改变
            {
                对象编号 = 玩家.ObjectID,
                交易状态 = 玩家.TradeState,
                对象等级 = 玩家.CurrentLevel
            });
        }
        else if (玩家 == 交易接收方)
        {
            接收方状态 = 状态;
            发送封包(new 交易状态改变
            {
                对象编号 = 玩家.ObjectID,
                交易状态 = 玩家.TradeState,
                对象等级 = 玩家.CurrentLevel
            });
        }
        else
        {
            BreakTrade();
        }
    }

    public void 放入金币(PlayerObject 玩家, int 数量)
    {
        if (玩家 == 交易申请方)
        {
            申请方金币 = 数量;
            发送封包(new 放入交易金币
            {
                对象编号 = 玩家.ObjectID,
                金币数量 = 数量
            });
        }
        else if (玩家 == 交易接收方)
        {
            接收方金币 = 数量;
            发送封包(new 放入交易金币
            {
                对象编号 = 玩家.ObjectID,
                金币数量 = 数量
            });
        }
        else
        {
            BreakTrade();
        }
    }

    public void 放入物品(PlayerObject 玩家, ItemInfo 物品, byte 位置)
    {
        if (玩家 == 交易申请方)
        {
            申请方物品.Add(位置, 物品);
            发送封包(new 放入交易物品
            {
                对象编号 = 玩家.ObjectID,
                放入位置 = 位置,
                放入物品 = 1,
                物品描述 = 物品.ToArray()
            });
        }
        else if (玩家 == 交易接收方)
        {
            接收方物品.Add(位置, 物品);
            发送封包(new 放入交易物品
            {
                对象编号 = 玩家.ObjectID,
                放入位置 = 位置,
                放入物品 = 1,
                物品描述 = 物品.ToArray()
            });
        }
        else
        {
            BreakTrade();
        }
    }

    public bool 背包已满(out PlayerObject 玩家)
    {
        玩家 = null;
        if (交易申请方.RemainingInventorySpace < 接收方物品.Count)
        {
            玩家 = 交易申请方;
            return true;
        }
        if (交易接收方.RemainingInventorySpace < 申请方物品.Count)
        {
            玩家 = 交易接收方;
            return true;
        }
        return false;
    }

    public bool 金币重复(PlayerObject 玩家)
    {
        if (玩家 == 交易申请方)
        {
            return 申请方金币 != 0;
        }
        if (玩家 == 交易接收方)
        {
            return 接收方金币 != 0;
        }
        return true;
    }

    public bool 物品重复(PlayerObject 玩家, ItemInfo 物品)
    {
        if (玩家 == 交易申请方)
        {
            return 申请方物品.Values.FirstOrDefault((ItemInfo O) => O == 物品) != null;
        }
        if (玩家 == 交易接收方)
        {
            return 接收方物品.Values.FirstOrDefault((ItemInfo O) => O == 物品) != null;
        }
        return true;
    }

    public bool 物品重复(PlayerObject 玩家, byte 位置)
    {
        if (玩家 == 交易申请方)
        {
            return 申请方物品.ContainsKey(位置);
        }
        if (玩家 == 交易接收方)
        {
            return 接收方物品.ContainsKey(位置);
        }
        return true;
    }

    public byte 对方状态(PlayerObject 玩家)
    {
        if (玩家 == 交易接收方)
        {
            return 申请方状态;
        }
        if (玩家 == 交易申请方)
        {
            return 接收方状态;
        }
        return 0;
    }

    public void 发送封包(GamePacket 封包)
    {
        交易接收方.Enqueue(封包);
        交易申请方.Enqueue(封包);
    }

    public PlayerObject 对方玩家(PlayerObject 玩家)
    {
        if (玩家 == 交易接收方)
        {
            return 交易申请方;
        }
        return 交易接收方;
    }

    public Dictionary<byte, ItemInfo> 对方物品(PlayerObject 玩家)
    {
        if (玩家 == 交易接收方)
        {
            return 申请方物品;
        }
        if (玩家 == 交易申请方)
        {
            return 接收方物品;
        }
        return null;
    }
}
