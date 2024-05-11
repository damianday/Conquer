using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;

using GameServer.Map;
using GameServer.Database;
using GameServer.Template;

using GamePackets;
using GamePackets.Client;
using GamePackets.Server;

namespace GameServer.Networking;

public sealed class SConnection
{
    private DateTime DisconnectTime;
    private bool Sending;

    private byte[] _rawData = [];
    private byte[] _rawBytes = new byte[8 * 1024];

    private readonly EventHandler<Exception> ErrorEventHandler;

    private ConcurrentQueue<GamePacket> ReceivedPackets = new ConcurrentQueue<GamePacket>();
    private ConcurrentQueue<GamePacket> SendPackets = new ConcurrentQueue<GamePacket>();

    public bool Disconnecting;
    public readonly DateTime ConnectedTime;
    public readonly TcpClient Connection;
    public GameStage Stage;
    public AccountInfo Account;
    public PlayerObject Player;

    public string IPAddress;
    public string MACAddress;

    public int TotalSentBytes;
    public int TotalReceivedBytes;

    public SConnection(TcpClient client)
    {
        Connection = client;
        Connection.NoDelay = true;
        ConnectedTime = SEngine.CurrentTime;
        DisconnectTime = SEngine.CurrentTime.AddMinutes(Settings.Default.DisconnectTime);
        ErrorEventHandler = (EventHandler<Exception>)Delegate.Combine(ErrorEventHandler, new EventHandler<Exception>(NetworkManager.OnDisconnect));
        IPAddress = Connection.Client.RemoteEndPoint.ToString().Split(':')[0];
        BeginReceive();
    }

    public void Process()
    {
        try
        {
            if (!Disconnecting && !NetworkManager.Stopped)
            {
                if (!(SEngine.CurrentTime > DisconnectTime))
                {
                    ProcessReceivedPackets();
                    SendAllPackets();
                }
                else
                {
                    Close(new Exception("The connection is unresponsive for a long time, disconnecting."));
                }
                return;
            }

            if (Sending || !ReceivedPackets.IsEmpty || !SendPackets.IsEmpty)
            {
                ProcessReceivedPackets();
                SendAllPackets();
                return;
            }
        }
        catch (Exception ex)
        {
            File.WriteAllText($".\\Log\\Error\\{DateTime.Now:yyyy-MM-dd--HH-mm-ss}.txt", "TargetSite:\r\n" + ex.TargetSite?.ToString() + "\r\nSource:\r\n" + ex.Source + "\r\nMessage:\r\n" + ex.Message + "\r\nStackTrace:\r\n" + ex.StackTrace);

            string[] vals;

            if (Player != null)
            {
                vals = new string[10] { "An exception occurred while processing network data, and the corresponding connection was disconnected\r\nAccount:[", null, null, null, null, null, null, null, null, null };
                vals[1] = Account?.AccountName.V ?? "None";
                vals[2] = "]\r\nUserName:[";
                vals[3] = Player?.Name ?? "None";
                vals[4] = "]\r\nIPAddress:[";
                vals[5] = IPAddress;
                vals[6] = "]\r\nMACAddress:[";
                vals[7] = MACAddress;
                vals[8] = "]\r\nMessage:";
                vals[9] = ex.Message;
                SEngine.AddSystemLog(string.Concat(vals));
            }
        }

        Disconnect();
    }

    public void SendPacket(GamePacket packet)
    {
        if (!Disconnecting && !NetworkManager.Stopped && packet != null)
        {
            if (Settings.Default.SendPacketsAsync)
                SendPackets.Enqueue(packet);
            else
                Connection.Client.Send(packet.ReadPacket());
        }
    }

    public void SendRaw(ushort type, ushort length, byte[] data, bool encoded = true)
    {
        byte[] buffer;
        if (length == 0)
        {
            buffer = new byte[data.Length + 4];
            Array.Copy(BitConverter.GetBytes(type), 0, buffer, 0, 2);
            Array.Copy(BitConverter.GetBytes((ushort)buffer.Length), 0, buffer, 2, 2);
            Array.Copy(data, 0, buffer, 4, data.Length);
        }
        else
        {
            buffer = new byte[data.Length + 2];
            Array.Copy(BitConverter.GetBytes(type), 0, buffer, 0, 2);
            Array.Copy(data, 0, buffer, 2, data.Length);
        }
        if (encoded)
            GamePacket.EncodeDecode(ref buffer);
        Connection.Client.Send(buffer);
    }

    public void Close(Exception e)
    {
        if (!Disconnecting)
        {
            Disconnecting = true;
            ErrorEventHandler?.Invoke(this, e);
        }
    }

    public void Disconnect()
    {
        Player?.Disconnect();
        Account?.Disconnect();

        NetworkManager.RemoveConnection(this);
        Connection.Client?.Shutdown(SocketShutdown.Both);
        Connection?.Close();

        ReceivedPackets.Clear();
        SendPackets.Clear();

        Stage = GameStage.Login;
    }

    private void ProcessReceivedPackets()
    {
        GamePacket p;
        while (true)
        {
            if (ReceivedPackets.IsEmpty)
                return;

            if (ReceivedPackets.Count > Settings.Default.PacketLimit)
            {
                ReceivedPackets.Clear();
                NetworkManager.BlockIP(IPAddress);
                Close(new Exception("Too many packets, disconnect and restrict logins."));
                return;
            }

            if (ReceivedPackets.TryDequeue(out p))
            {
                if (!GamePacket.PacketProcessMethodTable.TryGetValue(p.PacketType, out var method))
                    break;

                method.Invoke(this, [p]);
            }
        }
        Close(new Exception("No packet handling found, disconnected. Packet type: " + p.PacketType.FullName));
    }

    private void SendAllPackets()
    {
        List<byte> data = new List<byte>();
        while (!SendPackets.IsEmpty)
        {
            if (SendPackets.TryDequeue(out var p))
            {
                data.AddRange(p.ReadPacket());
            }
        }
        if (data.Count > 0)
            BeginSend(data);
    }

    private void DelayDisconnectTime()
    {
        DisconnectTime = SEngine.CurrentTime.AddMinutes(Settings.Default.DisconnectTime);
    }

    private void BeginReceive()
    {
        if (Disconnecting || NetworkManager.Stopped) return;

        try
        {
            Connection.Client.BeginReceive(_rawBytes, 0, _rawBytes.Length, SocketFlags.None, ReceiveData, _rawBytes);
        }
        catch (Exception ex)
        {
            Close(new Exception("Asynchronous receive error: " + ex.Message));
        }
    }

    private void ReceiveData(IAsyncResult result)
    {
        try
        {
            if (Disconnecting || NetworkManager.Stopped || Connection.Client == null)
                return;

            int dataRead = Connection.Client?.EndReceive(result) ?? 0;
            if (dataRead == 0)
            {
                Close(new Exception("Exit the game."));
                return;
            }

            TotalReceivedBytes += dataRead;
            NetworkManager.TotalReceivedBytes += dataRead;
            byte[] src = result.AsyncState as byte[];
            byte[] dst = new byte[_rawData.Length + dataRead];
            Buffer.BlockCopy(_rawData, 0, dst, 0, _rawData.Length);
            Buffer.BlockCopy(src, 0, dst, _rawData.Length, dataRead);
            _rawData = dst;
            while (true)
            {
                try
                {
                    GamePacket packet = GamePacket.GetClientPacket(_rawData, out _rawData);
                    if (packet == null)
                        break;

                    ReceivedPackets.Enqueue(packet);
                    continue;
                }
                catch (Exception e)
                {
                    Close(e);
                }
                break;
            }

            DelayDisconnectTime();
            BeginReceive();
        }
        catch (Exception ex)
        {
            Close(new Exception("ReceiveData error :" + ex.Message));
        }
    }

    private void BeginSend(byte[] data)
    {
        try
        {
            Sending = true;
            Connection.Client.BeginSend(data, 0, data.Length, SocketFlags.None, SendComplete, null);
        }
        catch (Exception ex)
        {
            Sending = false;
            SendPackets.Clear();
            Close(new Exception("Asynchronous send error: " + ex.Message));
        }
    }

    private void BeginSend(List<byte> data) => BeginSend(data.ToArray());

    private void SendComplete(IAsyncResult result)
    {
        try
        {
            int dataSent = Connection.Client.EndSend(result);
            TotalSentBytes += dataSent;
            NetworkManager.TotalSentBytes += dataSent;
            if (dataSent == 0)
            {
                SendPackets.Clear();
                Close(new Exception("Sending Callback Error!"));
            }
            Sending = false;
        }
        catch (Exception ex)
        {
            Sending = false;
            SendPackets.Clear();
            Close(new Exception("Sending Callback Error: " + ex.Message));
        }
    }

    public void Process(UnknownC642 P)
    {
        if (Stage != GameStage.Loading && Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.Process:{P.GetType()},CurrentStage:{Stage}"));
            return;
        }
        Player.EnterScene();
        Stage = GameStage.Game;
    }

    public void Process(坐骑御兽拖动 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.Process:{P.GetType()},CurrentStage:{Stage}"));
        }
        else
        {
            Player.添加坐骑技能包(P.御兽栏位, (byte)P.坐骑编号);
        }
    }

    public void Process(打开坐骑面板 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.Process:{P.GetType()},CurrentStage:{Stage}"));
            return;
        }
        Player.RemoveBuffEx(2555);
        Player.UserSelectMount(P.MountID);
    }

    public void Process(上传游戏设置 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.ChangeSettings(P.Description);
        }
    }

    public void Process(客户碰触法阵 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(客户进入法阵 P)
    {
        /*if (Config.法阵卡BUG清理 == 1 && Stage != GameStage.Game)
        {
            Config.法阵卡BUG清理 = 0;
            Player.EnterScene();
            Stage = GameStage.Game;
        }
        if (Stage != GameStage.Game)
        {
            Disconnect(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.EnterTeleportGate(P.法阵编号);
        }*/

        if (Stage == GameStage.Loading || Stage == GameStage.Game)
        {
            Player.EnterTeleportGate(P.法阵编号);
        }
        else
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(点击Npcc对话 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(RequestObjectRolePacket P)
    {
        if (Stage == GameStage.Loading || Stage == GameStage.Game)
        {
            Player.RequestObjectAppearance(P.ObjectID, P.StatusID);
        }
        else
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(客户网速测试 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
            return;
        }
        SendPacket(new 网速测试应答
        {
            当前时间 = P.UserTime
        });
    }

    public void Process(测试网关网速 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
            return;
        }
        SendPacket(new 登陆查询应答
        {
            当前时间 = P.UserTime
        });
    }

    public void Process(客户请求复活 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.Resurrect();
        }
    }

    public void Process(UserChangeAttackMode P)
    {
        AttackMode result;
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else if (Enum.IsDefined(typeof(AttackMode), (int)P.攻击模式) && Enum.TryParse<AttackMode>(P.攻击模式.ToString(), out result))
        {
            Player.ChangeAttackMode(result);
        }
        else
        {
            Close(new Exception("更改攻击模式时提供错误的枚举参数.即将断开连接."));
        }
    }

    public void Process(UserChangePetMode P)
    {
        PetMode result;
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else if (Enum.IsDefined(typeof(PetMode), (int)P.宠物模式) && Enum.TryParse<PetMode>(P.宠物模式.ToString(), out result))
        {
            Player.ChangePetMode(result);
        }
        else
        {
            Close(new Exception($"更改宠物模式时提供错误的枚举参数.即将断开连接. 参数 - {P.宠物模式}"));
        }
    }

    public void Process(上传角色位置 P)
    {
        if (Stage == GameStage.Game)
        {
            Player.玩家同步位置();
        }
        else
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(UserRequestTurn P)
    {
        if (Stage == GameStage.Game)
        {
            if (Enum.IsDefined(typeof(GameDirection), (int)P.Direction) && Enum.TryParse<GameDirection>(P.Direction.ToString(), out var dir))
            {
                Player.TurnTo(dir);
            }
            else
            {
                Close(new Exception("玩家角色转动时提供错误的枚举参数.即将断开连接."));
            }
        }
        else
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(UserRequestWalk P)
    {
        if (Stage == GameStage.Game)
        {
            Player.WalkTo(P.Location);
        }
        else
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(UserRequestRun P)
    {
        if (Stage == GameStage.Game)
        {
            Player.RunTo(P.Location);
        }
        else
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(角色开关技能 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserSwitchSkill(P.技能编号);
        }
    }

    public void Process(角色装备技能 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else if (P.技能栏位 < 32)
        {
            Player.ChangeHotKey(P.技能栏位, P.技能编号);
        }
        else
        {
            Close(new Exception("玩家装配技能时提供错误的封包参数.即将断开连接."));
        }
    }

    public void Process(CharacterExecuteSkill P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserDoSkill(P.SkillID, P.ActionID, P.TargetID, P.Location);
        }
    }

    public void Process(战斗姿态切换 P)
    {
        if (Stage != GameStage.Loading && Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.SwitchStance(P.姿态编号, P.触发动作);
        }
    }

    public void Process(客户更换角色 P)
    {
        if (Stage == GameStage.Game)
        {
            Account.ChangeCharacter(this);
            Stage = GameStage.SelectPlayer;
        }
        else
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(场景加载完成 P)
    {
        if (Stage == GameStage.Loading || Stage == GameStage.Game)
        {
            Player.EnterScene();
            Stage = GameStage.Game;
        }
        else
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(退出当前副本 P)
    {
        if (Stage != GameStage.Loading && Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家退出副本();
        }
    }

    public void Process(玩家退出登录 P)
    {
        if (Stage == GameStage.Login)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Account.返回登录(this);
        }
    }

    public void Process(打开角色背包 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(角色拾取物品 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(角色丢弃物品 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.DropItem(P.Grid, P.Location, P.Quantity);
        }
    }

    public void Process(角色转移物品 P)
    {
        if (Settings.Default.法阵卡BUG清理 == 1 && Stage != GameStage.Game)
        {
            Settings.Default.法阵卡BUG清理 = 0;
            Player.UserMoveItem(1, 1, 1, 1);
            Player.EnterScene();
            Stage = GameStage.Game;
        }
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserMoveItem(P.当前背包, P.原有位置, P.目标背包, P.目标位置);
        }
    }

    public void Process(角色使用物品 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserUseItem(P.背包类型, P.物品位置);
        }
    }

    public void Process(玩家喝修复油 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserEatItem(P.背包类型, P.物品位置);
        }
    }

    public void Process(玩家扩展背包 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserExtendGrid(P.背包类型, P.扩展大小);
        }
    }

    public void Process(请求商店数据 P)
    {
        if (Stage == GameStage.Game)
        {
            Player.RequestStoreInfo(P.Version);
        }
        else
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(角色购买物品 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserBuyItem(P.StoreID, P.Location, P.Quantity);
        }
    }

    public void Process(角色卖出物品 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserSellItem(P.背包类型, P.物品位置, P.卖出数量);
        }
    }

    public void Process(查询回购列表 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserViewRepurchaseItemList();
        }
    }

    public void Process(角色回购物品 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else if (P.物品位置 < 100)
        {
            Player.UserRepurchaseItem(P.物品位置);
        }
        else
        {
            Close(new Exception("玩家回购物品时提供错误的位置参数.即将断开连接."));
        }
    }

    public void Process(商店修理单件 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserRepairItem(P.背包类型, P.物品位置);
        }
    }

    public void Process(商店修理全部 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserRepairAllItems();
        }
    }

    public void Process(商店特修单件 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.商店特修单件(P.物品容器, P.物品位置);
        }
    }

    public void Process(随身修理单件 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.随身修理单件(P.物品容器, P.物品位置, P.物品编号);
        }
    }

    public void Process(随身特修全部 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.随身修理全部();
        }
    }

    public void Process(角色整理背包 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家整理背包(P.背包类型);
        }
    }

    public void Process(角色拆分物品 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserSplitItem(P.当前背包, P.物品位置, P.拆分数量, P.目标背包, P.目标位置);
        }
    }

    public void Process(角色分解物品 P)
    {
        物品背包分类 result;
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else if (Enum.TryParse<物品背包分类>(P.背包类型.ToString(), out result) && Enum.IsDefined(typeof(物品背包分类), result))
        {
            Player.UserDisassembleItem(P.背包类型, P.物品位置, P.分解数量);
        }
        else
        {
            Close(new Exception("玩家分解物品时提供错误的枚举参数.即将断开连接."));
        }
    }

    public void Process(角色合成物品 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserCraftItem(P.物品编号);
        }
    }

    public void Process(玩家镶嵌灵石 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserAddSpiritStone(P.装备类型, P.装备位置, P.装备孔位, P.灵石类型, P.灵石位置);
        }
    }

    public void Process(玩家拆除灵石 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserRemoveSpiritStone(P.装备类型, P.装备位置, P.装备孔位);
        }
    }

    public void Process(普通铭文洗练 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.普通铭文洗练(P.装备类型, P.装备位置, P.物品编号);
        }
    }

    public void Process(高级铭文洗练 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.高级铭文洗练(P.装备类型, P.装备位置, P.物品编号);
        }
    }

    public void Process(替换铭文洗练 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.替换铭文洗练(P.装备类型, P.装备位置, P.物品编号);
        }
    }

    public void Process(替换高级铭文 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.高级洗练确认(P.装备类型, P.装备位置);
        }
    }

    public void Process(替换低级铭文 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.替换洗练确认(P.装备类型, P.装备位置);
        }
    }

    public void Process(放弃铭文替换 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.放弃替换铭文();
        }
    }

    public void Process(解锁双铭文位 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.解锁双铭文位(P.装备类型, P.装备位置, P.操作参数);
        }
    }

    public void Process(切换双铭文位 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.切换双铭文位(P.装备类型, P.装备位置, P.操作参数);
        }
    }

    public void Process(传承武器铭文 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.传承武器铭文(P.来源类型, P.来源位置, P.目标类型, P.目标位置);
        }
    }

    public void Process(升级武器普通 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.升级武器普通(P.首饰组, P.材料组);
        }
    }

    public void Process(角色选中目标 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.SelectObject(P.ObjectID);
        }
    }

    public void Process(开始Npcc对话 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserOpenNpcDialogue(P.ObjectID);
        }
    }

    public void Process(继续Npcc对话 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.继续Npcc对话(P.对话编号);
        }
    }

    public void Process(查看玩家装备 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestViewRoleEquipment(P.对象编号);
        }
    }

    public void Process(请求龙卫数据 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(请求魂石数据 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(龙卫修改备注 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(同步角色战力 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestRoleCombatPower(P.对象编号);
        }
    }

    public void Process(查询问卷调查 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(玩家申请交易 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家申请交易(P.对象编号);
        }
    }

    public void Process(玩家同意交易 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家同意交易(P.对象编号);
        }
    }

    public void Process(玩家结束交易 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家结束交易();
        }
    }

    public void Process(玩家放入金币 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家放入金币(P.金币数量);
        }
    }

    public void Process(玩家放入物品 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家放入物品(P.放入位置, P.放入物品, P.物品容器, P.物品位置);
        }
    }

    public void Process(玩家锁定交易 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家锁定交易();
        }
    }

    public void Process(玩家解锁交易 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家解锁交易();
        }
    }

    public void Process(玩家确认交易 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家确认交易();
        }
    }

    public void Process(玩家准备摆摊 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家准备摆摊();
        }
    }

    public void Process(玩家重整摊位 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家重整摊位();
        }
    }

    public void Process(玩家开始摆摊 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家开始摆摊();
        }
    }

    public void Process(玩家收起摊位 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家收起摊位();
        }
    }

    public void Process(放入摊位物品 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.放入摊位物品(P.放入位置, P.物品容器, P.物品位置, P.物品数量, P.物品价格);
        }
    }

    public void Process(取回摊位物品 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.取回摊位物品(P.取回位置);
        }
    }

    public void Process(更改摊位名字 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.更改摊位名字(P.摊位名字);
        }
    }

    public void Process(更改摊位外观 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.升级摊位外观(P.外观编号);
        }
    }

    public void Process(打开角色摊位 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家打开摊位(P.对象编号);
        }
    }

    public void Process(购买摊位物品 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.购买摊位物品(P.对象编号, P.物品位置, P.购买数量);
        }
    }

    public void Process(添加好友关注 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家添加关注(P.对象编号, P.对象名字);
        }
    }

    public void Process(取消好友关注 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家取消关注(P.对象编号);
        }
    }

    public void Process(新建好友分组 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(移动好友分组 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(发送好友聊天 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else if (P.Description.Length < 7)
        {
            Close(new Exception($"Data too short. disconnecting.  Process: {P.GetType()}, data length:{P.Description.Length}"));
        }
        else if (P.Description.Last() != 0)
        {
            Close(new Exception($"Data error, disconnecting.  Process: {P.GetType()}, no terminator."));
        }
        else
        {
            Player.ReceiveUserMessage(P.Channel, P.Description);
        }
    }

    public void Process(玩家添加仇人 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家添加仇人(P.对象编号);
        }
    }

    public void Process(玩家删除仇人 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家删除仇人(P.对象编号);
        }
    }

    public void Process(玩家屏蔽对象 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家屏蔽目标(P.对象编号);
        }
    }

    public void Process(玩家解除屏蔽 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家解除屏蔽(P.对象编号);
        }
    }

    public void Process(玩家比较成就 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(发送聊天信息 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else if (P.Description.Last() != 0)
        {
            Close(new Exception($"Data error, disconnecting.  Process: {P.GetType()},  no terminator."));
        }
        else
        {
            Player.UserSendBroadcastMessage(P.ChannelID, P.MsgType, P.Description);
        }
    }

    public void Process(发送社交消息 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else if (P.Description.Last() != 0)
        {
            Close(new Exception($"Data error, disconnecting.  Process: {P.GetType()},  no terminator."));
        }
        else
        {
            Player.UserSendMessage(P.Param1, P.Channel, P.Description);
        }
    }

    public void Process(FilterUserMessage P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
            return;
        }

        Player.UserFilterMessage(P.Param1, P.Description);
    }

    public void Process(请求角色数据 P)
    {
        if (Stage != GameStage.Loading && Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestRoleProfile(P.CharacterID);
        }
    }

    public void Process(上传社交信息 P)
    {
        if (Stage != GameStage.Loading && Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(查询附近队伍 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestTeamSearch();
        }
    }

    public void Process(查询队伍信息 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestTeamInformation(P.对象编号);
        }
    }

    public void Process(申请创建队伍 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestCreateTeam(P.对象编号, P.分配方式);
        }
    }

    public void Process(发送组队请求 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.SendTeamInvitationRequest(P.对象编号);
        }
    }

    public void Process(申请离开队伍 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestRemoveTeamMember(P.对象编号);
        }
    }

    public void Process(申请更改队伍 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestChangeTeamCaptain(P.队长编号);
        }
    }

    public void Process(回应组队请求 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.SendTeamInvitationResponse(P.对象编号, P.组队方式, P.回应方式);
        }
    }

    public void Process(玩家装配称号 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.ChangeTitle(P.TitleID);
        }
    }

    public void Process(玩家卸下称号 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RemoveCurrentTitle();
        }
    }

    public void Process(申请发送邮件 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserSendMailRequest(P.字节数据);
        }
    }

    public void Process(查询邮箱内容 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserRequestMailbox();
        }
    }

    public void Process(查看邮件内容 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserViewedMail(P.邮件编号);
        }
    }

    public void Process(删除指定邮件 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserDeletedMail(P.邮件编号);
        }
    }

    public void Process(提取邮件附件 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.UserExtractMailAttachment(P.邮件编号);
        }
    }

    public void Process(查询行会名字 P)
    {
        if (Stage != GameStage.Loading && Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestGuildInformation(P.行会编号);
        }
    }

    public void Process(更多行会信息 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.更多行会信息();
        }
    }

    public void Process(查看行会列表 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.查看行会列表(P.行会编号, P.查看方式);
        }
    }

    public void Process(查找对应行会 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.查找对应行会(P.行会编号, P.行会名字);
        }
    }

    public void Process(申请加入行会 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestJoinGuild(P.行会编号, P.行会名字);
        }
    }

    public void Process(查看申请列表 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestViewGuildApplications();
        }
    }

    public void Process(处理入会申请 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.处理入会申请(P.对象编号, P.处理类型);
        }
    }

    public void Process(处理入会邀请 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.处理入会邀请(P.对象编号, P.处理类型);
        }
    }

    public void Process(雕爷刻印激活 P)
    {
        ItemInfo 物品;
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else if (Player.FindItem(90226, out 物品))
        {
            Player.ConsumeItem(Settings.Default.雕爷激活灵符需求, 物品);
            if (P.位置 == 12)
            {
                P.位置 = 2;
            }
            else if (P.位置 == 13)
            {
                P.位置 = 3;
            }
            else if (P.位置 == 9)
            {
                P.位置 = 4;
            }
            else
            {
                P.位置 = 12;
            }
            if ((Player.激活标识 & (1 << (int)P.位置)) != 1 << (int)P.位置)
            {
                Player.激活标识 |= 1 << (int)P.位置;
            }
            Player.Enqueue(new 同步补充变量
            {
                变量类型 = 1,
                变量索引 = 50,
                ObjectID = Player.ObjectID,
                变量内容 = Player.激活标识
            });
        }
    }

    public void Process(装备铭文刻印 P)
    {
        ItemInfo 物品;
        ItemInfo 物品2;
        EquipmentInfo v;
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else if (Player.FindItem(90226, out 物品) && Player.FindItem(P.物品编号, out 物品2) && P.解锁参数 == 0 && Player.Equipment.TryGetValue(P.装备部位, out v))
        {
            Player.ConsumeItem(Settings.Default.雕爷1号位铭文石, 物品2);
            Player.ConsumeItem(Settings.Default.雕爷1号位灵符, 物品);
            v.FirstInscription = InscriptionSkill.DataSheet[(ushort)P.铭文索引];
            Player.UserChangeInscription(v.FirstInscription.SkillID, 0);
            Player.UserChangeInscription(v.FirstInscription.SkillID, v.FirstInscription.ID);
            SendPacket(new SyncItemPacket
            {
                Description = v.ToArray()
            });
        }
    }

    public void Process(邀请加入行会 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.邀请加入行会(P.对象名字);
        }
    }

    public void Process(申请创建行会 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestCreateGuild(P.字节数据);
        }
    }

    public void Process(申请解散行会 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestDeleteGuild();
        }
    }

    public void Process(捐献行会资金 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.DonateGuildFunds(P.金币数量);
        }
    }

    public void Process(发放行会福利 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.发放行会福利();
        }
    }

    public void Process(申请离开行会 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestLeaveGuild();
        }
    }

    public void Process(更改行会公告 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.ChangeGuildNotice(P.行会公告);
        }
    }

    public void Process(更改行会宣言 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.ChangeGuildDeclaration(P.行会宣言);
        }
    }

    public void Process(设置行会禁言 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.BanGuildMember(P.对象编号, P.禁言状态);
        }
    }

    public void Process(变更会员职位 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.ChangeGuildMemberRank(P.对象编号, P.对象职位);
        }
    }

    public void Process(逐出行会成员 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestRemoveGuildMember(P.对象编号);
        }
    }

    public void Process(转移会长职位 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestChangeGuildLeader(P.对象编号);
        }
    }

    public void Process(申请行会外交 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestGuildAlliance(P.外交类型, P.外交时间, P.行会名字);
        }
    }

    public void Process(申请行会敌对 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestGuildHostile(P.敌对时间, P.行会名字);
        }
    }

    public void Process(处理结盟申请 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.处理结盟申请(P.处理类型, P.行会编号);
        }
    }

    public void Process(申请解除结盟 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestReleaseAllianceGuild(P.行会编号);
        }
    }

    public void Process(申请解除敌对 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestReleaseHostileGuild(P.行会编号);
        }
    }

    public void Process(处理解敌申请 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.处理解除申请(P.行会编号, P.回应类型);
        }
    }

    public void Process(更改存储权限 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(查看结盟申请 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.RequestViewAllianceApplications();
        }
    }

    public void Process(更多行会事记 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.更多行会事记();
        }
    }

    public void Process(查询行会成就 P)
    {
        if (Stage != GameStage.Loading && Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(开始自动战斗 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Phase exception, disconnected.  Processing packet: {P.GetType()}, Current phase: {Stage}"));
        }
        else
        {
            Player.自动战斗开始(P.自动战斗, P.战斗范围, P.开启空闲使用道具, P.空闲时间, P.道具ID, P.Unk1, P.技能ID, P.开启自动拾取, P.拾取范围, P.开启预留背包, P.预留格数, P.优先战斗, P.不捡取他人装备, P.不抢怪, P.Unk2);
        }
    }

    public void Process(自动战斗取消 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Phase exception, disconnected.  Processing packet: {P.GetType()}, Current phase: {Stage}"));
        }
        else
        {
            Player.自动战斗关闭(P.自动战斗);
        }
    }

    public void Process(开启行会活动 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(发布通缉榜单 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(同步通缉榜单 P)
    {
        if (Stage != GameStage.Loading && Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(发起行会战争 P)
    {
        if (Stage != GameStage.Loading && Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(查询地图路线 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.查询地图路线();
        }
    }

    public void Process(切换地图路线 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.切换地图路线();
        }
    }

    public void Process(跳过剧情动画 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(更改收徒推送 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.更改收徒推送(P.收徒推送);
        }
    }

    public void Process(查询师门成员 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.查询师门成员();
        }
    }

    public void Process(查询师门奖励 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.查询师门奖励();
        }
    }

    public void Process(查询拜师名册 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.查询拜师名册();
        }
    }

    public void Process(查询收徒名册 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.查询收徒名册();
        }
    }

    public void Process(祝贺徒弟升级 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(玩家申请拜师 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家申请拜师(P.对象编号);
        }
    }

    public void Process(同意拜师申请 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.同意拜师申请(P.对象编号);
        }
    }

    public void Process(拒绝拜师申请 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.拒绝拜师申请(P.对象编号);
        }
    }

    public void Process(玩家申请收徒 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.玩家申请收徒(P.对象编号);
        }
    }

    public void Process(同意收徒申请 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.同意收徒申请(P.对象编号);
        }
    }

    public void Process(拒绝收徒申请 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.拒绝收徒申请(P.对象编号);
        }
    }

    public void Process(逐出师门申请 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.逐出师门申请(P.对象编号);
        }
    }

    public void Process(离开师门申请 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.离开师门申请();
        }
    }

    public void Process(提交出师申请 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.提交出师申请();
        }
    }

    public void Process(查询排名榜单 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.查询排名榜单(P.榜单类型, P.起始位置);
        }
    }

    public void Process(查看演武排名 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(刷新演武挑战 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(开始战场演武 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(进入演武战场 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(跨服武道排名 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(登录寄售平台 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
            return;
        }
        SendPacket(new SocialErrorPacket { ErrorCode = 12804 });
    }

    public void Process(查询平台商品 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
            return;
        }
        SendPacket(new SocialErrorPacket { ErrorCode = 12804 });
    }

    public void Process(查询指定商品 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
            return;
        }
        SendPacket(new SocialErrorPacket { ErrorCode = 12804 });
    }

    public void Process(上架平台商品 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
            return;
        }
        SendPacket(new SocialErrorPacket { ErrorCode = 12804 });
    }

    public void Process(请求珍宝数据 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.查询珍宝商店(P.数据版本);
        }
    }

    public void Process(查询出售信息 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.查询出售信息();
        }
    }

    public void Process(购买珍宝商品 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.购买珍宝商品(P.物品编号, P.购买数量);
        }
    }

    public void Process(购买每周特惠 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.购买每周特惠(P.礼包编号);
        }
    }

    public void Process(购买玛法特权 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.购买玛法特权(P.特权类型, P.购买数量);
        }
    }

    public void Process(预定玛法特权 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.预定玛法特权(P.特权类型);
        }
    }

    public void Process(领取特权礼包 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.领取特权礼包(P.特权类型, P.礼包位置);
        }
    }

    public void Process(玩家每日签到 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
    }

    public void Process(客户账号登录 P)
    {
        if (Stage == GameStage.Login)
        {
            if (SystemInfo.Info.NICBans.TryGetValue(P.MACAddress, out var v) && v > SEngine.CurrentTime)
            {
                Close(new Exception("NIC blocking, restricting logins"));
            }
            else if (NetworkManager.Tickets.TryGetValue(P.LoginTicket, out var ticket))
            {
                if (!(SEngine.CurrentTime > ticket.ValidTime))
                {
                    AccountInfo account = ((!Session.AccountInfoTable.SearchTable.TryGetValue(ticket.AccountName, out var data) || !(data is AccountInfo a)) ? new AccountInfo(ticket.AccountName) : a);
                    if (account.Connection == null)
                        account.AccountLogin(this, P.MACAddress);
                    else
                    {
                        account.Enqueue(new LoginErrorMessagePacket { ErrorCode = 260 });
                        account.Connection.Close(new Exception("Duplicate login of account, kicked offline."));

                        Close(new Exception("The account is already online and cannot be logged in."));
                    }
                }
                else
                {
                    Close(new Exception("The login ticket has expired."));
                }
            }
            else
            {
                Close(new Exception("The logged-in ticket does not exist."));
            }
        }
        else
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        NetworkManager.Tickets.Remove(P.LoginTicket);
    }

    public void Process(客户创建角色 P)
    {
        if (Stage != GameStage.SelectPlayer)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Account.NewCharacter(this, P);
        }
    }

    public void Process(客户删除角色 P)
    {
        if (Stage != GameStage.SelectPlayer)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Account.FreezeCharacter(this, P);
        }
    }

    public void Process(彻底删除角色 P)
    {
        if (Stage != GameStage.SelectPlayer)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Account.DeleteCharacter(this, P);
        }
    }

    public void Process(客户进入游戏 P)
    {
        if (Stage != GameStage.SelectPlayer)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Account.EnterGame(this, P);
        }
    }

    public void Process(客户找回角色 P)
    {
        if (Stage != GameStage.SelectPlayer)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Account.RestoreCharacter(this, P);
        }
    }

    public void Process(觉醒之力积累开关 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.切换觉醒栏经验封包(P.开关);
        }
    }

    public void Process(觉醒技能升级 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
        }
        else
        {
            Player.升级觉醒技能封包(P.技能编号);
        }
    }

    public void Process(玩家开始挖矿 P)
    {
        if (Stage != GameStage.Game)
        {
            Close(new Exception($"Abnormal stage, disconnecting.  Process: {P.GetType()},  CurrentStage:{Stage}"));
            return;
        }

        Player.UserTryDigMine(P.Position);
    }


    #region Missing Functions
    public void Process(传奇之力激活 P) { }

    public void Process(UnknownC644 P) { }

    public void Process(UnknownC155 P)
    {
        Player.Enqueue(new 成就完成通知
        {
            U1 = P.U1,
            U2 = 19225
        });
    }

    public void Process(UnknownC266 P) { }

    public void Process(UnknownC272 P) { }

    public void Process(内挂物品过滤 P) { }

    public void Process(请求悬赏剩余 P) { }

    public void Process(预留封包零一 P) { }

    public void Process(预留封包零二 P) { }

    public void Process(请求七天详情 P) { }

    public void Process(请求战功信息 P) { }

    public void Process(请求战功任务 P) { }
    #endregion
}
