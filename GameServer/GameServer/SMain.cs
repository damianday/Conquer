using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameServer.Properties;
using GameServer.Map;
using GameServer.Database;
using GameServer.Template;
using GameServer.Networking;

namespace GameServer;

public partial class SMain : Form
{
    public static SMain Main;

    private static System.Data.DataTable 角色数据表;
    private static System.Data.DataTable 技能数据表;
    private static System.Data.DataTable 装备数据表;
    private static System.Data.DataTable 背包数据表;
    private static System.Data.DataTable 仓库数据表;
    private static System.Data.DataTable MapDataTable;
    private static System.Data.DataTable MonsterDataTable;
    private static System.Data.DataTable 掉落数据表;
    private static System.Data.DataTable 封禁数据表;

    private static Dictionary<CharacterInfo, DataRow> 角色数据行;
    private static Dictionary<DataRow, CharacterInfo> 数据行角色;
    private static Dictionary<GameMap, DataRow> 地图数据行;
    private static Dictionary<MonsterInfo, DataRow> 怪物数据行;
    private static Dictionary<DataRow, MonsterInfo> 数据行怪物;
    private static Dictionary<string, DataRow> 封禁数据行;
    private static Dictionary<DataGridViewRow, DateTime> 公告数据表;

    private static Dictionary<CharacterInfo, List<KeyValuePair<ushort, SkillInfo>>> 角色技能表;
    private static Dictionary<CharacterInfo, List<KeyValuePair<byte, EquipmentInfo>>> 角色装备表;
    private static Dictionary<CharacterInfo, List<KeyValuePair<byte, ItemInfo>>> 角色背包表;
    private static Dictionary<CharacterInfo, List<KeyValuePair<byte, ItemInfo>>> 角色仓库表;
    private static Dictionary<MonsterInfo, List<KeyValuePair<GameItem, long>>> 怪物掉落表;

    public static void LoadSystemData()
    {
        Main.allToolStripMenuItem.Visible = false;
        AddSystemLog("Loading system data...");
        MapDataTable = new System.Data.DataTable("地图数据表");
        地图数据行 = new Dictionary<GameMap, DataRow>();
        MapDataTable.Columns.Add("地图编号", typeof(string));
        MapDataTable.Columns.Add("地图名字", typeof(string));
        MapDataTable.Columns.Add("限制等级", typeof(string));
        MapDataTable.Columns.Add("玩家数量", typeof(string));
        MapDataTable.Columns.Add("固定怪物总数", typeof(uint));
        MapDataTable.Columns.Add("存活怪物总数", typeof(uint));
        MapDataTable.Columns.Add("怪物复活次数", typeof(uint));
        MapDataTable.Columns.Add("怪物掉落次数", typeof(long));
        MapDataTable.Columns.Add("金币掉落总数", typeof(long));
        Main?.地图浏览表.BeginInvoke((MethodInvoker)delegate
        {
            Main.地图浏览表.DataSource = MapDataTable;
        });
        MonsterDataTable = new System.Data.DataTable("怪物数据表");
        怪物数据行 = new Dictionary<MonsterInfo, DataRow>();
        数据行怪物 = new Dictionary<DataRow, MonsterInfo>();
        MonsterDataTable.Columns.Add("模板编号", typeof(string));
        MonsterDataTable.Columns.Add("怪物名字", typeof(string));
        MonsterDataTable.Columns.Add("怪物等级", typeof(string));
        MonsterDataTable.Columns.Add("怪物经验", typeof(string));
        MonsterDataTable.Columns.Add("怪物级别", typeof(string));
        MonsterDataTable.Columns.Add("移动间隔", typeof(string));
        MonsterDataTable.Columns.Add("漫游间隔", typeof(string));
        MonsterDataTable.Columns.Add("仇恨范围", typeof(string));
        MonsterDataTable.Columns.Add("仇恨时长", typeof(string));
        Main?.怪物浏览表.BeginInvoke((MethodInvoker)delegate
        {
            Main.怪物浏览表.DataSource = MonsterDataTable;
        });
        掉落数据表 = new System.Data.DataTable("掉落数据表");
        怪物掉落表 = new Dictionary<MonsterInfo, List<KeyValuePair<GameItem, long>>>();
        掉落数据表.Columns.Add("物品名字", typeof(string));
        掉落数据表.Columns.Add("掉落数量", typeof(string));
        Main?.掉落浏览表.BeginInvoke((MethodInvoker)delegate
        {
            Main.掉落浏览表.DataSource = 掉落数据表;
        });
        SystemDataGateway.LoadData();
        AddSystemLog("The system data load is complete");
    }

    public static void LoadUserData()
    {
        AddSystemLog("Loading user data...");
        角色数据表 = new System.Data.DataTable("角色数据表");
        技能数据表 = new System.Data.DataTable("技能数据表");
        装备数据表 = new System.Data.DataTable("装备数据表");
        背包数据表 = new System.Data.DataTable("装备数据表");
        仓库数据表 = new System.Data.DataTable("装备数据表");
        角色数据表 = new System.Data.DataTable("角色数据表");
        角色数据行 = new Dictionary<CharacterInfo, DataRow>();
        数据行角色 = new Dictionary<DataRow, CharacterInfo>();
        角色数据表.Columns.Add("角色名字", typeof(string));
        角色数据表.Columns.Add("角色封禁", typeof(string));
        角色数据表.Columns.Add("所属账号", typeof(string));
        角色数据表.Columns.Add("账号封禁", typeof(string));
        角色数据表.Columns.Add("冻结日期", typeof(string));
        角色数据表.Columns.Add("删除日期", typeof(string));
        角色数据表.Columns.Add("登录日期", typeof(string));
        角色数据表.Columns.Add("离线日期", typeof(string));
        角色数据表.Columns.Add("网络地址", typeof(string));
        角色数据表.Columns.Add("物理地址", typeof(string));
        角色数据表.Columns.Add("角色职业", typeof(string));
        角色数据表.Columns.Add("角色性别", typeof(string));
        角色数据表.Columns.Add("所属行会", typeof(string));
        角色数据表.Columns.Add("元宝数量", typeof(string));
        角色数据表.Columns.Add("消耗元宝", typeof(string));
        角色数据表.Columns.Add("金币数量", typeof(string));
        角色数据表.Columns.Add("转出金币", typeof(string));
        角色数据表.Columns.Add("背包大小", typeof(string));
        角色数据表.Columns.Add("仓库大小", typeof(string));
        角色数据表.Columns.Add("师门声望", typeof(string));
        角色数据表.Columns.Add("本期特权", typeof(string));
        角色数据表.Columns.Add("本期日期", typeof(string));
        角色数据表.Columns.Add("上期特权", typeof(string));
        角色数据表.Columns.Add("上期日期", typeof(string));
        角色数据表.Columns.Add("剩余特权", typeof(string));
        角色数据表.Columns.Add("当前等级", typeof(string));
        角色数据表.Columns.Add("当前经验", typeof(string));
        角色数据表.Columns.Add("双倍经验", typeof(string));
        角色数据表.Columns.Add("当前战力", typeof(string));
        角色数据表.Columns.Add("当前地图", typeof(string));
        角色数据表.Columns.Add("当前坐标", typeof(string));
        角色数据表.Columns.Add("当前PK值", typeof(string));
        角色数据表.Columns.Add("激活标识", typeof(string));
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            Main.角色浏览表.DataSource = 角色数据表;
            for (int i = 0; i < Main.角色浏览表.Columns.Count; i++)
            {
                Main.角色浏览表.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        });
        角色技能表 = new Dictionary<CharacterInfo, List<KeyValuePair<ushort, SkillInfo>>>();
        技能数据表.Columns.Add("技能名字", typeof(string));
        技能数据表.Columns.Add("技能编号", typeof(string));
        技能数据表.Columns.Add("当前等级", typeof(string));
        技能数据表.Columns.Add("当前经验", typeof(string));
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            Main.技能浏览表.DataSource = 技能数据表;
        });
        角色装备表 = new Dictionary<CharacterInfo, List<KeyValuePair<byte, EquipmentInfo>>>();
        装备数据表.Columns.Add("穿戴部位", typeof(string));
        装备数据表.Columns.Add("穿戴装备", typeof(string));
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            Main.装备浏览表.DataSource = 装备数据表;
        });
        角色背包表 = new Dictionary<CharacterInfo, List<KeyValuePair<byte, ItemInfo>>>();
        背包数据表.Columns.Add("背包位置", typeof(string));
        背包数据表.Columns.Add("背包物品", typeof(string));
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            Main.背包浏览表.DataSource = 背包数据表;
        });
        角色仓库表 = new Dictionary<CharacterInfo, List<KeyValuePair<byte, ItemInfo>>>();
        仓库数据表.Columns.Add("仓库位置", typeof(string));
        仓库数据表.Columns.Add("仓库物品", typeof(string));
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            Main.仓库浏览表.DataSource = 仓库数据表;
        });
        封禁数据表 = new System.Data.DataTable();
        封禁数据行 = new Dictionary<string, DataRow>();
        封禁数据表.Columns.Add("网络地址", typeof(string));
        封禁数据表.Columns.Add("物理地址", typeof(string));
        封禁数据表.Columns.Add("到期时间", typeof(string));
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            Main.封禁浏览表.DataSource = 封禁数据表;
        });
        Session.Load();
        Main.allToolStripMenuItem.Visible = true;
        AddSystemLog("The user data is loaded");
    }

    public static void OnStartServiceCompleted()
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            Main.定时发送公告.Enabled = true;
            Main.UIUpdateTimer.Enabled = true;
            Main.stopServerToolStripMenuItem.Enabled = true;
            Main.SettingsPage.Enabled = false;
            Main.startServerToolStripMenuItem.Enabled = false;
        });
    }

    public static void OnStopServiceCompleted()
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            Main.定时发送公告.Enabled = true;
            Main.SettingsPage.Enabled = true;
            Main.startServerToolStripMenuItem.Enabled = true;
            Main.UIUpdateTimer.Enabled = false;
            Main.stopServerToolStripMenuItem.Enabled = false;
            foreach (KeyValuePair<DataGridViewRow, DateTime> item in 公告数据表)
            {
                item.Key.ReadOnly = false;
                item.Key.Cells["公告状态"].Value = "";
                item.Key.Cells["公告计时"].Value = "";
                item.Key.Cells["剩余次数"].Value = 0;
            }
            if (Main.公告浏览表.SelectedRows.Count != 0)
            {
                Main.开始公告按钮.Enabled = true;
                Main.停止公告按钮.Enabled = false;
            }
            公告数据表.Clear();
        });
    }

    public static void AddSystemLog(string message)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            Main.SystemLogsTextBox.AppendText($"[{DateTime.Now:F}]: {message}" + "\r\n");
            Main.SystemLogsTextBox.ScrollToCaret();
            Main.saveSystemLogsToolStripMenuItem.Enabled = true;
            Main.clearSystemLogsToolStripMenuItem.Enabled = true;
        });
    }

    public static void AddChatLog(string tag, byte[] message)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            Main.ChatLogsTextBox.AppendText($"[{DateTime.Now:F}]: {tag + Encoding.UTF8.GetString(message).Trim(default(char))}" + "\r\n");
            Main.ChatLogsTextBox.ScrollToCaret();
            Main.saveChatLogsToolStripMenuItem.Enabled = true;
            Main.clearChatLogsToolStripMenuItem.Enabled = true;
        });
    }

    public static void AddCommandLog(string message)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            Main.CommandLogsTextBox.AppendText($"[{DateTime.Now:F}]: {message}" + "\r\n");
            Main.CommandLogsTextBox.ScrollToCaret();
            Main.clearCommandsLogToolStripMenuItem.Enabled = true;
        });
    }

    public static void UpdateStats()
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            Main.StatTotalConnections.Text = $"Total Connections: {NetworkManager.Connections.Count}";
            Main.StatLoggedInConnections.Text = $"Already Logged in: {NetworkManager.ActiveConnections}";
            Main.StatLiveConnections.Text = $"Now Online: {NetworkManager.已上线连接数}/{NetworkManager.已上线连接数1}/{NetworkManager.已上线连接数2}";
            Main.CycleCountLabel.Text = $"Cycle Count: {SEngine.CycleCount}";
            Main.StatSent.Text = $"Sent: {NetworkManager.TotalSentBytes}";
            Main.StatReceived.Text = $"Received: {NetworkManager.TotalReceivedBytes}";
            Main.StatObjectsStatistics.Text = $"Objects Statistics: {MapManager.ActiveObjects.Count} / {MapManager.SecondaryObjects.Count} / {MapManager.Objects.Count}";
        });
    }

    public static void 更新连接总数(uint count)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            Main.StatTotalConnections.Text = $"Total Connections: {count}";
        });
    }

    public static void 更新已经登录(uint count)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            Main.StatLoggedInConnections.Text = $"Already Logged in: {count}";
        });
    }

    public static void 更新已经上线(uint 内容, uint 内容1, uint 内容2)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            Main.StatLiveConnections.Text = $"Now Online: {内容}/{内容1}/{内容2}";
        });
    }

    public static void UpdateCycleCount(uint count)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            Main.CycleCountLabel.Text = $"Cycle Count: {count}";
        });
    }

    public static void 更新接收字节(long 内容)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            Main.StatReceived.Text = $"Received: {内容}";
        });
    }

    public static void 更新发送字节(long 内容)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            Main.StatSent.Text = $"Sent: {内容}";
        });
    }

    public static void 更新对象统计(int 激活对象, int 次要对象, int 对象总数)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            Main.StatObjectsStatistics.Text = $"Objects Statistics: {激活对象} / {次要对象} / {对象总数}";
        });
    }

    public static void 添加数据显示(CharacterInfo 数据)
    {
        if (!角色数据行.ContainsKey(数据))
        {
            角色数据行[数据] = 角色数据表.NewRow();
            角色数据表.Rows.Add(角色数据行[数据]);
        }
    }

    public static void 修改数据显示(CharacterInfo 数据, string 表头文本, string 表格内容)
    {
        if (角色数据行.ContainsKey(数据))
        {
            角色数据行[数据][表头文本] = 表格内容;
        }
    }

    public static void 添加角色数据(CharacterInfo 角色)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            if (!角色数据行.ContainsKey(角色))
            {
                DataRow dataRow = 角色数据表.NewRow();
                dataRow["角色名字"] = 角色;
                dataRow["所属账号"] = 角色.Account;
                dataRow["账号封禁"] = ((角色.Account.V.BlockDate.V != default(DateTime)) ? 角色.Account.V.BlockDate : null);
                dataRow["角色封禁"] = ((角色.BlockDate.V != default(DateTime)) ? 角色.BlockDate : null);
                dataRow["冻结日期"] = ((角色.FrozenDate.V != default(DateTime)) ? 角色.FrozenDate : null);
                dataRow["删除日期"] = ((角色.DeletetionDate.V != default(DateTime)) ? 角色.DeletetionDate : null);
                dataRow["登录日期"] = ((角色.LoginDate.V != default(DateTime)) ? 角色.LoginDate : null);
                dataRow["离线日期"] = ((角色.Connection == null) ? 角色.DisconnectDate : null);
                dataRow["网络地址"] = 角色.IPAddress;
                dataRow["物理地址"] = 角色.MACAddress;
                dataRow["角色职业"] = 角色.Job;
                dataRow["角色性别"] = 角色.Gender;
                dataRow["所属行会"] = 角色.Guild;
                dataRow["元宝数量"] = 角色.Ingot;
                dataRow["消耗元宝"] = 角色.消耗元宝;
                dataRow["金币数量"] = 角色.Gold;
                dataRow["转出金币"] = 角色.转出金币;
                dataRow["背包大小"] = 角色.InventorySize;
                dataRow["仓库大小"] = 角色.WarehouseSize;
                dataRow["师门声望"] = 角色.师门声望;
                dataRow["本期特权"] = 角色.本期特权;
                dataRow["本期日期"] = 角色.本期日期;
                dataRow["上期特权"] = 角色.上期特权;
                dataRow["上期日期"] = 角色.上期日期;
                dataRow["剩余特权"] = 角色.剩余特权;
                dataRow["当前等级"] = 角色.Level;
                dataRow["当前经验"] = 角色.Experience;
                dataRow["双倍经验"] = 角色.ExperienceRate;
                dataRow["当前战力"] = 角色.CombatPower;
                dataRow["当前地图"] = (GameServer.Template.GameMap.DataSheet.TryGetValue((byte)角色.CurrentMap.V, out var value) ? ((object)value.MapName) : ((object)角色.CurrentMap));
                dataRow["当前PK值"] = 角色.CurrentPKPoint;
                dataRow["当前坐标"] = $"{角色.CurrentPosition.V.X}, {角色.CurrentPosition.V.Y}";
                dataRow["激活标识"] = 角色.激活标识;
                角色数据行[角色] = dataRow;
                数据行角色[dataRow] = 角色;
                角色数据表.Rows.Add(dataRow);
            }
        });
    }

    public static void 移除角色数据(CharacterInfo 角色)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            if (角色数据行.TryGetValue(角色, out var value))
            {
                数据行角色.Remove(value);
                角色数据表.Rows.Remove(value);
                角色技能表.Remove(角色);
                角色背包表.Remove(角色);
                角色装备表.Remove(角色);
                角色仓库表.Remove(角色);
            }
        });
    }

    public static void ProcessUpdateUI(object sender, EventArgs e)
    {
        技能数据表.Rows.Clear();
        装备数据表.Rows.Clear();
        背包数据表.Rows.Clear();
        仓库数据表.Rows.Clear();
        掉落数据表.Rows.Clear();
        if (Main == null)
        {
            return;
        }
        if (Main.角色浏览表.Rows.Count > 0 && Main.角色浏览表.SelectedRows.Count > 0)
        {
            DataRow row = (Main.角色浏览表.Rows[Main.角色浏览表.SelectedRows[0].Index].DataBoundItem as DataRowView).Row;
            if (数据行角色.TryGetValue(row, out var value))
            {
                if (角色技能表.TryGetValue(value, out var value2))
                {
                    foreach (KeyValuePair<ushort, SkillInfo> item in value2)
                    {
                        DataRow dataRow = 技能数据表.NewRow();
                        dataRow["技能名字"] = item.Value.Inscription.SkillName;
                        dataRow["技能编号"] = item.Value.ID;
                        dataRow["当前等级"] = item.Value.Level;
                        dataRow["当前经验"] = item.Value.Experience;
                        技能数据表.Rows.Add(dataRow);
                    }
                }
                if (角色装备表.TryGetValue(value, out var value3))
                {
                    foreach (KeyValuePair<byte, EquipmentInfo> item2 in value3)
                    {
                        DataRow dataRow2 = 装备数据表.NewRow();
                        dataRow2["穿戴部位"] = (装备穿戴部位)item2.Key;
                        dataRow2["穿戴装备"] = item2.Value;
                        装备数据表.Rows.Add(dataRow2);
                    }
                }
                if (角色背包表.TryGetValue(value, out var value4))
                {
                    foreach (KeyValuePair<byte, ItemInfo> item3 in value4)
                    {
                        DataRow dataRow3 = 背包数据表.NewRow();
                        dataRow3["背包位置"] = item3.Key;
                        dataRow3["背包物品"] = item3.Value;
                        背包数据表.Rows.Add(dataRow3);
                    }
                }
                if (角色仓库表.TryGetValue(value, out var value5))
                {
                    foreach (KeyValuePair<byte, ItemInfo> item4 in value5)
                    {
                        DataRow dataRow4 = 仓库数据表.NewRow();
                        dataRow4["仓库位置"] = item4.Key;
                        dataRow4["仓库物品"] = item4.Value;
                        仓库数据表.Rows.Add(dataRow4);
                    }
                }
            }
        }
        if (Main.怪物浏览表.Rows.Count <= 0 || Main.怪物浏览表.SelectedRows.Count <= 0)
        {
            return;
        }
        DataRow row2 = (Main.怪物浏览表.Rows[Main.怪物浏览表.SelectedRows[0].Index].DataBoundItem as DataRowView).Row;
        if (!数据行怪物.TryGetValue(row2, out var value6) || !怪物掉落表.TryGetValue(value6, out var value7))
        {
            return;
        }
        foreach (KeyValuePair<GameItem, long> item5 in value7)
        {
            DataRow dataRow5 = 掉落数据表.NewRow();
            dataRow5["物品名字"] = item5.Key.Name;
            dataRow5["掉落数量"] = item5.Value;
            掉落数据表.Rows.Add(dataRow5);
        }
    }

    public static void UpdateCharacter(CharacterInfo 角色, string 表头, object 内容)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            if (角色数据行.TryGetValue(角色, out var value))
            {
                value[表头] = 内容;
            }
        });
    }

    public static void 更新角色技能(CharacterInfo 角色, List<KeyValuePair<ushort, SkillInfo>> 技能)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            角色技能表[角色] = 技能;
        });
    }

    public static void 更新角色装备(CharacterInfo 角色, List<KeyValuePair<byte, EquipmentInfo>> 装备)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            角色装备表[角色] = 装备;
        });
    }

    public static void 更新角色背包(CharacterInfo 角色, List<KeyValuePair<byte, ItemInfo>> 物品)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            角色背包表[角色] = 物品;
        });
    }

    public static void 更新角色仓库(CharacterInfo 角色, List<KeyValuePair<byte, ItemInfo>> 物品)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            角色仓库表[角色] = 物品;
        });
    }

    public static void 添加地图数据(Map.Map 地图)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            if (!地图数据行.ContainsKey(地图.MapInfo))
            {
                DataRow dataRow = MapDataTable.NewRow();
                dataRow["地图编号"] = 地图.MapID;
                dataRow["地图名字"] = 地图.MapInfo;
                dataRow["限制等级"] = 地图.MinLevel;
                dataRow["玩家数量"] = 地图.Players.Count;
                dataRow["固定怪物总数"] = 地图.固定怪物总数;
                dataRow["存活怪物总数"] = 地图.TotalSurvivingMonsters;
                dataRow["怪物复活次数"] = 地图.TotalAmountMonsterResurrected;
                dataRow["怪物掉落次数"] = 地图.TotalAmountMonsterDrops;
                dataRow["金币掉落总数"] = 地图.TotalAmountGoldDrops;
                地图数据行[地图.MapInfo] = dataRow;
                MapDataTable.Rows.Add(dataRow);
            }
        });
    }

    public static void 更新地图数据(Map.Map 地图, string 表头, object 内容)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            if (地图数据行.TryGetValue(地图.MapInfo, out var value))
            {
                switch (表头)
                {
                    default:
                        value[表头] = 内容;
                        break;
                    case "金币掉落总数":
                    case "怪物掉落次数":
                        value[表头] = Convert.ToInt64(value[表头]) + (int)内容;
                        break;
                    case "存活怪物总数":
                    case "怪物复活次数":
                        value[表头] = Convert.ToUInt32(value[表头]) + (int)内容;
                        break;
                }
            }
        });
    }

    public static void 添加怪物数据(MonsterInfo 怪物)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            if (!怪物数据行.ContainsKey(怪物))
            {
                DataRow dataRow = MonsterDataTable.NewRow();
                dataRow["模板编号"] = 怪物.ID;
                dataRow["怪物名字"] = 怪物.MonsterName;
                dataRow["怪物等级"] = 怪物.Level;
                dataRow["怪物级别"] = 怪物.Grade;
                dataRow["怪物经验"] = 怪物.ProvideExperience;
                dataRow["移动间隔"] = 怪物.MoveInterval;
                dataRow["仇恨范围"] = 怪物.RangeHate;
                dataRow["仇恨时长"] = 怪物.HateTime;
                怪物数据行[怪物] = dataRow;
                数据行怪物[dataRow] = 怪物;
                MonsterDataTable.Rows.Add(dataRow);
            }
        });
    }

    public static void 更新掉落统计(MonsterInfo 怪物, List<KeyValuePair<GameItem, long>> 物品)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            怪物掉落表[怪物] = 物品;
        });
    }

    public static void 添加封禁数据(string 地址, object 时间, bool 网络地址 = true)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            if (!封禁数据行.ContainsKey(地址))
            {
                DataRow dataRow = 封禁数据表.NewRow();
                dataRow["网络地址"] = (网络地址 ? 地址 : null);
                dataRow["物理地址"] = (网络地址 ? null : 地址);
                dataRow["到期时间"] = 时间;
                封禁数据行[地址] = dataRow;
                封禁数据表.Rows.Add(dataRow);
            }
        });
    }

    public static void 更新封禁数据(string 地址, object 时间, bool 网络地址 = true)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            if (封禁数据行.TryGetValue(地址, out var value))
            {
                if (网络地址)
                {
                    value["网络地址"] = 时间;
                }
                else
                {
                    value["物理地址"] = 时间;
                }
            }
        });
    }

    public static void 移除封禁数据(string 地址)
    {
        Main?.BeginInvoke((MethodInvoker)delegate
        {
            if (封禁数据行.TryGetValue(地址, out var value))
            {
                封禁数据行.Remove(地址);
                封禁数据表.Rows.Remove(value);
            }
        });
    }

    public SMain()
    {
        InitializeComponent();

        Control.CheckForIllegalCrossThreadCalls = false;
        Main = this;

        string 系统公告内容 = Settings.Default.系统公告内容;
        公告数据表 = new Dictionary<DataGridViewRow, DateTime>();
        string[] array = 系统公告内容.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < array.Length; i++)
        {
            string[] array2 = array[i].Split('\t');
            int index = 公告浏览表.Rows.Add();
            公告浏览表.Rows[index].Cells["公告间隔"].Value = array2[0];
            公告浏览表.Rows[index].Cells["公告次数"].Value = array2[1];
            公告浏览表.Rows[index].Cells["公告内容"].Value = array2[2];
        }

        角色浏览表.ColumnHeadersDefaultCellStyle.Font = (地图浏览表.ColumnHeadersDefaultCellStyle.Font = (怪物浏览表.ColumnHeadersDefaultCellStyle.Font = (掉落浏览表.ColumnHeadersDefaultCellStyle.Font = (封禁浏览表.ColumnHeadersDefaultCellStyle.Font = (角色浏览表.DefaultCellStyle.Font = (地图浏览表.DefaultCellStyle.Font = (怪物浏览表.DefaultCellStyle.Font = (封禁浏览表.DefaultCellStyle.Font = (掉落浏览表.DefaultCellStyle.Font = new Font("宋体", 9f))))))))));
        S_GameDataPath.Text = (Config.GameDataPath = Settings.Default.GameDataPath);
        S_DataBackupPath.Text = (Config.DataBackupPath = Settings.Default.DataBackupPath);
        S_UserConnectionPort.Value = (Config.UserConnectionPort = Settings.Default.UserConnectionPort);
        S_TicketReceivePort.Value = (Config.TicketReceivePort = Settings.Default.TicketReceivePort);
        S_PacketLimit.Value = (Config.PacketLimit = Settings.Default.PacketLimit);
        S_AbnormalBlockTime.Value = (Config.AbnormalBlockTime = Settings.Default.AbnormalBlockTime);
        S_DisconnectTime.Value = (Config.DisconnectTime = Settings.Default.DisconnectTime);
        S_MaxUserLevel.Value = (Config.MaxUserLevel = Settings.Default.MaxUserLevel);
        S_NoobSupportLevel.Value = (Config.NoobSupportLevel = Settings.Default.NoobSupportLevel);
        S_SpecialRepairDiscount.Value = (Config.SpecialRepairDiscount = Settings.Default.SpecialRepairDiscount);
        S_怪物额外爆率.Value = (Config.怪物额外爆率 = Settings.Default.怪物额外爆率);
        S_怪物经验倍率.Value = (Config.MonsterExperienceMultiplier = Settings.Default.MonsterExperienceMultiplier);
        S_减收益等级差.Value = (Config.减收益等级差 = Settings.Default.减收益等级差);
        S_收益减少比率.Value = (Config.收益减少比率 = Settings.Default.收益减少比率);
        S_怪物诱惑时长.Value = (Config.怪物诱惑时长 = Settings.Default.怪物诱惑时长);
        S_物品归属时间.Value = (Config.物品归属时间 = Settings.Default.物品归属时间);
        S_ItemDisappearTime.Value = (Config.ItemDisappearTime = Settings.Default.ItemDisappearTime);
        S_自动保存时间.Value = (Config.AutoSaveInterval = Settings.Default.AutoSaveInterval);
        S_自动保存日志.Value = (Config.自动保存日志 = Settings.Default.自动保存日志);
        S_沃玛分解元宝.Value = (Config.沃玛分解元宝 = Settings.Default.沃玛分解元宝);
        S_祖玛分解元宝.Value = (Config.祖玛分解元宝 = Settings.Default.祖玛分解元宝);
        S_赤月分解元宝.Value = (Config.赤月分解元宝 = Settings.Default.赤月分解元宝);
        S_魔龙分解元宝.Value = (Config.魔龙分解元宝 = Settings.Default.魔龙分解元宝);
        S_苍月分解元宝.Value = (Config.苍月分解元宝 = Settings.Default.苍月分解元宝);
        S_星王分解元宝.Value = (Config.星王分解元宝 = Settings.Default.星王分解元宝);
        S_神秘分解元宝.Value = (Config.神秘分解元宝 = Settings.Default.神秘分解元宝);
        S_城主分解元宝.Value = (Config.城主分解元宝 = Settings.Default.城主分解元宝);
        S_屠魔爆率开关.Value = (Config.屠魔爆率开关 = Settings.Default.屠魔爆率开关);
        S_屠魔组队人数.Value = (Config.屠魔组队人数 = Settings.Default.屠魔组队人数);
        S_屠魔令回收经验.Value = (Config.屠魔令回收经验 = Settings.Default.屠魔令回收经验);
        S_武斗场时间一.Value = (Config.武斗场时间一 = Settings.Default.武斗场时间一);
        S_武斗场时间二.Value = (Config.武斗场时间二 = Settings.Default.武斗场时间二);
        S_武斗场经验小.Value = (Config.武斗场经验小 = Settings.Default.武斗场经验小);
        S_武斗场经验大.Value = (Config.武斗场经验大 = Settings.Default.武斗场经验大);
        S_沙巴克开启.Value = (Config.沙巴克开启 = Settings.Default.沙巴克开启);
        S_沙巴克结束.Value = (Config.沙巴克结束 = Settings.Default.沙巴克结束);
        S_祝福油幸运1机率.Value = (Config.祝福油幸运1机率 = Settings.Default.祝福油幸运1机率);
        S_祝福油幸运2机率.Value = (Config.祝福油幸运2机率 = Settings.Default.祝福油幸运2机率);
        S_祝福油幸运3机率.Value = (Config.祝福油幸运3机率 = Settings.Default.祝福油幸运3机率);
        S_祝福油幸运4机率.Value = (Config.祝福油幸运4机率 = Settings.Default.祝福油幸运4机率);
        S_祝福油幸运5机率.Value = (Config.祝福油幸运5机率 = Settings.Default.祝福油幸运5机率);
        S_祝福油幸运6机率.Value = (Config.祝福油幸运6机率 = Settings.Default.祝福油幸运6机率);
        S_祝福油幸运7机率.Value = (Config.祝福油幸运7机率 = Settings.Default.祝福油幸运7机率);
        S_PKYellowNamePoint.Value = (Config.PKYellowNamePoint = Settings.Default.PKYellowNamePoint);
        S_PKRedNamePoint.Value = (Config.PKRedNamePoint = Settings.Default.PKRedNamePoint);
        S_PKCrimsonNamePoint.Value = (Config.PKCrimsonNamePoint = Settings.Default.PKCrimsonNamePoint);
        S_锻造成功倍数.Value = (Config.锻造成功倍数 = Settings.Default.锻造成功倍数);
        S_死亡掉落背包几率.Value = (decimal)(Config.死亡掉落背包几率 = Settings.Default.死亡掉落背包几率);
        S_死亡掉落身上几率.Value = (decimal)(Config.死亡掉落身上几率 = Settings.Default.死亡掉落身上几率);
        S_PK死亡幸运开关.Value = (Config.PK死亡幸运开关 = Settings.Default.PK死亡幸运开关);
        S_屠魔副本次数.Value = (Config.屠魔副本次数 = Settings.Default.屠魔副本次数);
        S_升级经验模块一.Value = (Config.升级经验模块一 = Settings.Default.升级经验模块一);
        S_升级经验模块二.Value = (Config.升级经验模块二 = Settings.Default.升级经验模块二);
        S_升级经验模块三.Value = (Config.升级经验模块三 = Settings.Default.升级经验模块三);
        S_升级经验模块四.Value = (Config.升级经验模块四 = Settings.Default.升级经验模块四);
        S_升级经验模块五.Value = (Config.升级经验模块五 = Settings.Default.升级经验模块五);
        S_升级经验模块六.Value = (Config.升级经验模块六 = Settings.Default.升级经验模块六);
        S_升级经验模块七.Value = (Config.升级经验模块七 = Settings.Default.升级经验模块七);
        S_升级经验模块八.Value = (Config.升级经验模块八 = Settings.Default.升级经验模块八);
        S_升级经验模块九.Value = (Config.升级经验模块九 = Settings.Default.升级经验模块九);
        S_升级经验模块十.Value = (Config.升级经验模块十 = Settings.Default.升级经验模块十);
        S_升级经验模块十一.Value = (Config.升级经验模块十一 = Settings.Default.升级经验模块十一);
        S_升级经验模块十二.Value = (Config.升级经验模块十二 = Settings.Default.升级经验模块十二);
        S_升级经验模块十三.Value = (Config.升级经验模块十三 = Settings.Default.升级经验模块十三);
        S_升级经验模块十四.Value = (Config.升级经验模块十四 = Settings.Default.升级经验模块十四);
        S_升级经验模块十五.Value = (Config.升级经验模块十五 = Settings.Default.升级经验模块十五);
        S_升级经验模块十六.Value = (Config.升级经验模块十六 = Settings.Default.升级经验模块十六);
        S_升级经验模块十七.Value = (Config.升级经验模块十七 = Settings.Default.升级经验模块十七);
        S_升级经验模块十八.Value = (Config.升级经验模块十八 = Settings.Default.升级经验模块十八);
        S_升级经验模块十九.Value = (Config.升级经验模块十九 = Settings.Default.升级经验模块十九);
        S_升级经验模块二十.Value = (Config.升级经验模块二十 = Settings.Default.升级经验模块二十);
        S_升级经验模块二十一.Value = (Config.升级经验模块二十一 = Settings.Default.升级经验模块二十一);
        S_升级经验模块二十二.Value = (Config.升级经验模块二十二 = Settings.Default.升级经验模块二十二);
        S_升级经验模块二十三.Value = (Config.升级经验模块二十三 = Settings.Default.升级经验模块二十三);
        S_升级经验模块二十四.Value = (Config.升级经验模块二十四 = Settings.Default.升级经验模块二十四);
        S_升级经验模块二十五.Value = (Config.升级经验模块二十五 = Settings.Default.升级经验模块二十五);
        S_升级经验模块二十六.Value = (Config.升级经验模块二十六 = Settings.Default.升级经验模块二十六);
        S_升级经验模块二十七.Value = (Config.升级经验模块二十七 = Settings.Default.升级经验模块二十七);
        S_升级经验模块二十八.Value = (Config.升级经验模块二十八 = Settings.Default.升级经验模块二十八);
        S_升级经验模块二十九.Value = (Config.升级经验模块二十九 = Settings.Default.升级经验模块二十九);
        S_升级经验模块三十.Value = (Config.升级经验模块三十 = Settings.Default.升级经验模块三十);
        S_高级祝福油幸运机率.Value = (Config.高级祝福油幸运机率 = Settings.Default.高级祝福油幸运机率);
        S_雕爷使用物品.Value = (Config.雕爷使用物品 = Settings.Default.雕爷使用物品);
        S_雕爷使用金币.Value = (Config.雕爷使用金币 = Settings.Default.雕爷使用金币);
        S_称号范围拾取判断.Value = (Config.称号范围拾取判断 = Settings.Default.称号范围拾取判断);
        S_TitleRangePickUpDistance.Value = (Config.TitleRangePickUpDistance = Settings.Default.TitleRangePickUpDistance);
        S_行会申请人数限制.Value = (Config.行会申请人数限制 = Settings.Default.行会申请人数限制);
        S_疗伤药HP.Value = (Config.疗伤药HP = Settings.Default.疗伤药HP);
        S_疗伤药MP.Value = (Config.疗伤药MP = Settings.Default.疗伤药MP);
        S_万年雪霜HP.Value = (Config.万年雪霜HP = Settings.Default.万年雪霜HP);
        S_万年雪霜MP.Value = (Config.万年雪霜MP = Settings.Default.万年雪霜MP);
        S_元宝金币回收设定.Value = (Config.元宝金币回收设定 = Settings.Default.元宝金币回收设定);
        S_元宝金币传送设定.Value = (Config.元宝金币传送设定 = Settings.Default.元宝金币传送设定);
        S_快捷传送一编号.Value = (Config.快捷传送一编号 = Settings.Default.快捷传送一编号);
        S_快捷传送一货币.Value = (Config.快捷传送一货币 = Settings.Default.快捷传送一货币);
        S_快捷传送一等级.Value = (Config.快捷传送一等级 = Settings.Default.快捷传送一等级);
        S_快捷传送二编号.Value = (Config.快捷传送二编号 = Settings.Default.快捷传送二编号);
        S_快捷传送二货币.Value = (Config.快捷传送二货币 = Settings.Default.快捷传送二货币);
        S_快捷传送二等级.Value = (Config.快捷传送二等级 = Settings.Default.快捷传送二等级);
        S_快捷传送三编号.Value = (Config.快捷传送三编号 = Settings.Default.快捷传送三编号);
        S_快捷传送三货币.Value = (Config.快捷传送三货币 = Settings.Default.快捷传送三货币);
        S_快捷传送三等级.Value = (Config.快捷传送三等级 = Settings.Default.快捷传送三等级);
        S_快捷传送四编号.Value = (Config.快捷传送四编号 = Settings.Default.快捷传送四编号);
        S_快捷传送四货币.Value = (Config.快捷传送四货币 = Settings.Default.快捷传送四货币);
        S_快捷传送四等级.Value = (Config.快捷传送四等级 = Settings.Default.快捷传送四等级);
        S_快捷传送五编号.Value = (Config.快捷传送五编号 = Settings.Default.快捷传送五编号);
        S_快捷传送五货币.Value = (Config.快捷传送五货币 = Settings.Default.快捷传送五货币);
        S_快捷传送五等级.Value = (Config.快捷传送五等级 = Settings.Default.快捷传送五等级);
        S_狂暴货币格式.Value = (Config.狂暴货币格式 = Settings.Default.狂暴货币格式);
        S_狂暴称号格式.Value = (Config.狂暴称号格式 = Settings.Default.狂暴称号格式);
        S_狂暴开启物品名称.Value = (Config.狂暴开启物品名称 = Settings.Default.狂暴开启物品名称);
        S_狂暴开启物品数量.Value = (Config.狂暴开启物品数量 = Settings.Default.狂暴开启物品数量);
        S_狂暴杀死物品数量.Value = (Config.狂暴杀死物品数量 = Settings.Default.狂暴杀死物品数量);
        S_狂暴开启元宝数量.Value = (Config.狂暴开启元宝数量 = Settings.Default.狂暴开启元宝数量);
        S_狂暴杀死元宝数量.Value = (Config.狂暴杀死元宝数量 = Settings.Default.狂暴杀死元宝数量);
        S_狂暴开启金币数量.Value = (Config.狂暴开启金币数量 = Settings.Default.狂暴开启金币数量);
        S_狂暴杀死金币数量.Value = (Config.狂暴杀死金币数量 = Settings.Default.狂暴杀死金币数量);
        S_装备技能开关.Value = (Config.装备技能开关 = Settings.Default.装备技能开关);
        S_御兽属性开启.Value = (Config.御兽属性开启 = Settings.Default.御兽属性开启);
        S_可摆摊地图编号.Value = (Config.可摆摊地图编号 = Settings.Default.可摆摊地图编号);
        S_可摆摊地图坐标X.Value = (Config.可摆摊地图坐标X = Settings.Default.可摆摊地图坐标X);
        S_可摆摊地图坐标Y.Value = (Config.可摆摊地图坐标Y = Settings.Default.可摆摊地图坐标Y);
        S_可摆摊地图范围.Value = (Config.可摆摊地图范围 = Settings.Default.可摆摊地图范围);
        S_可摆摊货币选择.Value = (Config.可摆摊货币选择 = Settings.Default.可摆摊货币选择);
        S_可摆摊等级.Value = (Config.可摆摊等级 = Settings.Default.可摆摊等级);
        S_ReviveInterval.Value = (Config.ReviveInterval = Settings.Default.ReviveInterval);
        S_自定义麻痹几率.Value = (decimal)(Config.自定义麻痹几率 = Settings.Default.自定义麻痹几率);
        S_PetUpgradeXPLevel1.Value = (Config.PetUpgradeXPLevel1 = Settings.Default.PetUpgradeXPLevel1);
        S_PetUpgradeXPLevel2.Value = (Config.PetUpgradeXPLevel2 = Settings.Default.PetUpgradeXPLevel2);
        S_PetUpgradeXPLevel3.Value = (Config.PetUpgradeXPLevel3 = Settings.Default.PetUpgradeXPLevel3);
        S_PetUpgradeXPLevel4.Value = (Config.PetUpgradeXPLevel4 = Settings.Default.PetUpgradeXPLevel4);
        S_PetUpgradeXPLevel5.Value = (Config.PetUpgradeXPLevel5 = Settings.Default.PetUpgradeXPLevel5);
        S_PetUpgradeXPLevel6.Value = (Config.PetUpgradeXPLevel6 = Settings.Default.PetUpgradeXPLevel6);
        S_PetUpgradeXPLevel7.Value = (Config.PetUpgradeXPLevel7 = Settings.Default.PetUpgradeXPLevel7);
        S_PetUpgradeXPLevel8.Value = (Config.PetUpgradeXPLevel8 = Settings.Default.PetUpgradeXPLevel8);
        S_PetUpgradeXPLevel9.Value = (Config.PetUpgradeXPLevel9 = Settings.Default.PetUpgradeXPLevel9);
        S_下马击落机率.Value = (Config.下马击落机率 = Settings.Default.下马击落机率);
        S_AllowRaceWarrior.Value = (Config.AllowRaceWarrior = Settings.Default.AllowRaceWarrior);
        S_AllowRaceWizard.Value = (Config.AllowRaceWizard = Settings.Default.AllowRaceWizard);
        S_AllowRaceTaoist.Value = (Config.AllowRaceTaoist = Settings.Default.AllowRaceTaoist);
        S_AllowRaceArcher.Value = (Config.AllowRaceArcher = Settings.Default.AllowRaceArcher);
        S_AllowRaceAssassin.Value = (Config.AllowRaceAssassin = Settings.Default.AllowRaceAssassin);
        S_AllowRaceDragonLance.Value = (Config.AllowRaceDragonLance = Settings.Default.AllowRaceDragonLance);
        S_泡点等级开关.Value = (Config.泡点等级开关 = Settings.Default.泡点等级开关);
        S_泡点当前经验.Value = (Config.泡点当前经验 = Settings.Default.泡点当前经验);
        S_泡点限制等级.Value = (Config.泡点限制等级 = Settings.Default.泡点限制等级);
        S_杀人PK红名开关.Value = (Config.杀人PK红名开关 = Settings.Default.杀人PK红名开关);
        S_泡点秒数控制.Value = (Config.泡点秒数控制 = Settings.Default.泡点秒数控制);
        S_自定义物品数量一.Value = (Config.自定义物品数量一 = Settings.Default.自定义物品数量一);
        S_自定义物品数量二.Value = (Config.自定义物品数量二 = Settings.Default.自定义物品数量二);
        S_自定义物品数量三.Value = (Config.自定义物品数量三 = Settings.Default.自定义物品数量三);
        S_自定义物品数量四.Value = (Config.自定义物品数量四 = Settings.Default.自定义物品数量四);
        S_自定义物品数量五.Value = (Config.自定义物品数量五 = Settings.Default.自定义物品数量五);
        S_自定义称号内容一.Value = (Config.自定义称号内容一 = Settings.Default.自定义称号内容一);
        S_自定义称号内容二.Value = (Config.自定义称号内容二 = Settings.Default.自定义称号内容二);
        S_自定义称号内容三.Value = (Config.自定义称号内容三 = Settings.Default.自定义称号内容三);
        S_自定义称号内容四.Value = (Config.自定义称号内容四 = Settings.Default.自定义称号内容四);
        S_自定义称号内容五.Value = (Config.自定义称号内容五 = Settings.Default.自定义称号内容五);
        S_元宝金币传送设定2.Value = (Config.元宝金币传送设定2 = Settings.Default.元宝金币传送设定2);
        S_快捷传送一编号2.Value = (Config.快捷传送一编号2 = Settings.Default.快捷传送一编号2);
        S_快捷传送一货币2.Value = (Config.快捷传送一货币2 = Settings.Default.快捷传送一货币2);
        S_快捷传送一等级2.Value = (Config.快捷传送一等级2 = Settings.Default.快捷传送一等级2);
        S_快捷传送二编号2.Value = (Config.快捷传送二编号2 = Settings.Default.快捷传送二编号2);
        S_快捷传送二货币2.Value = (Config.快捷传送二货币2 = Settings.Default.快捷传送二货币2);
        S_快捷传送二等级2.Value = (Config.快捷传送二等级2 = Settings.Default.快捷传送二等级2);
        S_快捷传送三编号2.Value = (Config.快捷传送三编号2 = Settings.Default.快捷传送三编号2);
        S_快捷传送三货币2.Value = (Config.快捷传送三货币2 = Settings.Default.快捷传送三货币2);
        S_快捷传送三等级2.Value = (Config.快捷传送三等级2 = Settings.Default.快捷传送三等级2);
        S_快捷传送四编号2.Value = (Config.快捷传送四编号2 = Settings.Default.快捷传送四编号2);
        S_快捷传送四货币2.Value = (Config.快捷传送四货币2 = Settings.Default.快捷传送四货币2);
        S_快捷传送四等级2.Value = (Config.快捷传送四等级2 = Settings.Default.快捷传送四等级2);
        S_快捷传送五编号2.Value = (Config.快捷传送五编号2 = Settings.Default.快捷传送五编号2);
        S_快捷传送五货币2.Value = (Config.快捷传送五货币2 = Settings.Default.快捷传送五货币2);
        S_快捷传送五等级2.Value = (Config.快捷传送五等级2 = Settings.Default.快捷传送五等级2);
        S_快捷传送六编号2.Value = (Config.快捷传送六编号2 = Settings.Default.快捷传送六编号2);
        S_快捷传送六货币2.Value = (Config.快捷传送六货币2 = Settings.Default.快捷传送六货币2);
        S_快捷传送六等级2.Value = (Config.快捷传送六等级2 = Settings.Default.快捷传送六等级2);
        S_BOSS一时间.Value = (Config.BOSS一时间 = Settings.Default.BOSS一时间);
        S_BOSS一分钟.Value = (Config.BOSS一分钟 = Settings.Default.BOSS一分钟);
        S_BOSS一地图编号.Value = (Config.BOSS一地图编号 = Settings.Default.BOSS一地图编号);
        S_BOSS一坐标X.Value = (Config.BOSS一坐标X = Settings.Default.BOSS一坐标X);
        S_BOSS一坐标Y.Value = (Config.BOSS一坐标Y = Settings.Default.BOSS一坐标Y);
        S_武斗场次数限制.Value = (Config.武斗场次数限制 = Settings.Default.武斗场次数限制);
        S_AutoPickUpInventorySpace.Value = (Config.AutoPickUpInventorySpace = Settings.Default.AutoPickUpInventorySpace);
        S_BOSS刷新提示开关.Value = (Config.BOSS刷新提示开关 = Settings.Default.BOSS刷新提示开关);
        S_自动整理背包计时.Value = (Config.自动整理背包计时 = Settings.Default.自动整理背包计时);
        S_自动整理背包开关.Value = (Config.自动整理背包开关 = Settings.Default.自动整理背包开关);
        S_称号叠加开关.Value = (Config.称号叠加开关 = Settings.Default.称号叠加开关);
        S_称号叠加模块一.Value = (Config.称号叠加模块一 = Settings.Default.称号叠加模块一);
        S_称号叠加模块二.Value = (Config.称号叠加模块二 = Settings.Default.称号叠加模块二);
        S_称号叠加模块三.Value = (Config.称号叠加模块三 = Settings.Default.称号叠加模块三);
        S_称号叠加模块四.Value = (Config.称号叠加模块四 = Settings.Default.称号叠加模块四);
        S_称号叠加模块五.Value = (Config.称号叠加模块五 = Settings.Default.称号叠加模块五);
        S_称号叠加模块六.Value = (Config.称号叠加模块六 = Settings.Default.称号叠加模块六);
        S_称号叠加模块七.Value = (Config.称号叠加模块七 = Settings.Default.称号叠加模块七);
        S_称号叠加模块八.Value = (Config.称号叠加模块八 = Settings.Default.称号叠加模块八);
        S_沙城传送货币开关.Value = (Config.沙城传送货币开关 = Settings.Default.沙城传送货币开关);
        S_沙城快捷货币一.Value = (Config.沙城快捷货币一 = Settings.Default.沙城快捷货币一);
        S_沙城快捷等级一.Value = (Config.沙城快捷等级一 = Settings.Default.沙城快捷等级一);
        S_沙城快捷货币二.Value = (Config.沙城快捷货币二 = Settings.Default.沙城快捷货币二);
        S_沙城快捷等级二.Value = (Config.沙城快捷等级二 = Settings.Default.沙城快捷等级二);
        S_沙城快捷货币三.Value = (Config.沙城快捷货币三 = Settings.Default.沙城快捷货币三);
        S_沙城快捷等级三.Value = (Config.沙城快捷等级三 = Settings.Default.沙城快捷等级三);
        S_沙城快捷货币四.Value = (Config.沙城快捷货币四 = Settings.Default.沙城快捷货币四);
        S_沙城快捷等级四.Value = (Config.沙城快捷等级四 = Settings.Default.沙城快捷等级四);
        S_BOSS二时间.Value = (Config.BOSS二时间 = Settings.Default.BOSS二时间);
        S_BOSS二分钟.Value = (Config.BOSS二分钟 = Settings.Default.BOSS二分钟);
        S_BOSS二地图编号.Value = (Config.BOSS二地图编号 = Settings.Default.BOSS二地图编号);
        S_BOSS二坐标X.Value = (Config.BOSS二坐标X = Settings.Default.BOSS二坐标X);
        S_BOSS二坐标Y.Value = (Config.BOSS二坐标Y = Settings.Default.BOSS二坐标Y);
        S_BOSS三时间.Value = (Config.BOSS三时间 = Settings.Default.BOSS三时间);
        S_BOSS三分钟.Value = (Config.BOSS三分钟 = Settings.Default.BOSS三分钟);
        S_BOSS三地图编号.Value = (Config.BOSS三地图编号 = Settings.Default.BOSS三地图编号);
        S_BOSS三坐标X.Value = (Config.BOSS三坐标X = Settings.Default.BOSS三坐标X);
        S_BOSS三坐标Y.Value = (Config.BOSS三坐标Y = Settings.Default.BOSS三坐标Y);
        S_BOSS四时间.Value = (Config.BOSS四时间 = Settings.Default.BOSS四时间);
        S_BOSS四分钟.Value = (Config.BOSS四分钟 = Settings.Default.BOSS四分钟);
        S_BOSS四地图编号.Value = (Config.BOSS四地图编号 = Settings.Default.BOSS四地图编号);
        S_BOSS四坐标X.Value = (Config.BOSS四坐标X = Settings.Default.BOSS四坐标X);
        S_BOSS四坐标Y.Value = (Config.BOSS四坐标Y = Settings.Default.BOSS四坐标Y);
        S_BOSS五时间.Value = (Config.BOSS五时间 = Settings.Default.BOSS五时间);
        S_BOSS五分钟.Value = (Config.BOSS五分钟 = Settings.Default.BOSS五分钟);
        S_BOSS五地图编号.Value = (Config.BOSS五地图编号 = Settings.Default.BOSS五地图编号);
        S_BOSS五坐标X.Value = (Config.BOSS五坐标X = Settings.Default.BOSS五坐标X);
        S_BOSS五坐标Y.Value = (Config.BOSS五坐标Y = Settings.Default.BOSS五坐标Y);
        S_BOSS名字二.Text = (Config.BOSS名字二 = Settings.Default.BOSS名字二);
        S_BOSS二地图名字.Text = (Config.BOSS二地图名字 = Settings.Default.BOSS二地图名字);
        S_BOSS名字三.Text = (Config.BOSS名字三 = Settings.Default.BOSS名字三);
        S_BOSS三地图名字.Text = (Config.BOSS三地图名字 = Settings.Default.BOSS三地图名字);
        S_BOSS名字四.Text = (Config.BOSS名字四 = Settings.Default.BOSS名字四);
        S_BOSS四地图名字.Text = (Config.BOSS四地图名字 = Settings.Default.BOSS四地图名字);
        S_BOSS名字五.Text = (Config.BOSS名字五 = Settings.Default.BOSS名字五);
        S_BOSS五地图名字.Text = (Config.BOSS五地图名字 = Settings.Default.BOSS五地图名字);
        S_未知暗点副本价格.Value = (Config.未知暗点副本价格 = Settings.Default.未知暗点副本价格);
        S_未知暗点副本等级.Value = (Config.未知暗点副本等级 = Settings.Default.未知暗点副本等级);
        S_未知暗点二层价格.Value = (Config.未知暗点二层价格 = Settings.Default.未知暗点二层价格);
        S_未知暗点二层等级.Value = (Config.未知暗点二层等级 = Settings.Default.未知暗点二层等级);
        S_幽冥海副本价格.Value = (Config.幽冥海副本价格 = Settings.Default.幽冥海副本价格);
        S_幽冥海副本等级.Value = (Config.幽冥海副本等级 = Settings.Default.幽冥海副本等级);
        S_猎魔暗使称号一.Value = (Config.猎魔暗使称号一 = Settings.Default.猎魔暗使称号一);
        S_猎魔暗使材料一.Value = (Config.猎魔暗使材料一 = Settings.Default.猎魔暗使材料一);
        S_猎魔暗使数量一.Value = (Config.猎魔暗使数量一 = Settings.Default.猎魔暗使数量一);
        S_猎魔暗使称号二.Value = (Config.猎魔暗使称号二 = Settings.Default.猎魔暗使称号二);
        S_猎魔暗使材料二.Value = (Config.猎魔暗使材料二 = Settings.Default.猎魔暗使材料二);
        S_猎魔暗使数量二.Value = (Config.猎魔暗使数量二 = Settings.Default.猎魔暗使数量二);
        S_猎魔暗使称号三.Value = (Config.猎魔暗使称号三 = Settings.Default.猎魔暗使称号三);
        S_猎魔暗使材料三.Value = (Config.猎魔暗使材料三 = Settings.Default.猎魔暗使材料三);
        S_猎魔暗使数量三.Value = (Config.猎魔暗使数量三 = Settings.Default.猎魔暗使数量三);
        S_猎魔暗使称号四.Value = (Config.猎魔暗使称号四 = Settings.Default.猎魔暗使称号四);
        S_猎魔暗使材料四.Value = (Config.猎魔暗使材料四 = Settings.Default.猎魔暗使材料四);
        S_猎魔暗使数量四.Value = (Config.猎魔暗使数量四 = Settings.Default.猎魔暗使数量四);
        S_猎魔暗使称号五.Value = (Config.猎魔暗使称号五 = Settings.Default.猎魔暗使称号五);
        S_猎魔暗使材料五.Value = (Config.猎魔暗使材料五 = Settings.Default.猎魔暗使材料五);
        S_猎魔暗使数量五.Value = (Config.猎魔暗使数量五 = Settings.Default.猎魔暗使数量五);
        S_猎魔暗使称号六.Value = (Config.猎魔暗使称号六 = Settings.Default.猎魔暗使称号六);
        S_猎魔暗使材料六.Value = (Config.猎魔暗使材料六 = Settings.Default.猎魔暗使材料六);
        S_猎魔暗使数量六.Value = (Config.猎魔暗使数量六 = Settings.Default.猎魔暗使数量六);
        S_怪物掉落广播开关.Value = (Config.怪物掉落广播开关 = Settings.Default.怪物掉落广播开关);
        S_怪物掉落窗口开关.Value = (Config.怪物掉落窗口开关 = Settings.Default.怪物掉落窗口开关);
        S_珍宝阁提示开关.Value = (Config.珍宝阁提示开关 = Settings.Default.珍宝阁提示开关);
        S_城主分解物品一.Text = (Config.城主分解物品一 = Settings.Default.城主分解物品一);
        S_城主分解物品二.Text = (Config.城主分解物品二 = Settings.Default.城主分解物品二);
        S_城主分解物品三.Text = (Config.城主分解物品三 = Settings.Default.城主分解物品三);
        S_城主分解物品四.Text = (Config.城主分解物品四 = Settings.Default.城主分解物品四);
        S_城主分解几率一.Value = (Config.城主分解几率一 = Settings.Default.城主分解几率一);
        S_城主分解几率二.Value = (Config.城主分解几率二 = Settings.Default.城主分解几率二);
        S_城主分解几率三.Value = (Config.城主分解几率三 = Settings.Default.城主分解几率三);
        S_城主分解几率四.Value = (Config.城主分解几率四 = Settings.Default.城主分解几率四);
        S_城主分解数量一.Value = (Config.城主分解数量一 = Settings.Default.城主分解数量一);
        S_城主分解数量二.Value = (Config.城主分解数量二 = Settings.Default.城主分解数量二);
        S_城主分解数量三.Value = (Config.城主分解数量三 = Settings.Default.城主分解数量三);
        S_城主分解数量四.Value = (Config.城主分解数量四 = Settings.Default.城主分解数量四);
        S_城主分解开关.Value = (Config.城主分解开关 = Settings.Default.城主分解开关);
        S_星王分解物品一.Text = (Config.星王分解物品一 = Settings.Default.星王分解物品一);
        S_星王分解物品二.Text = (Config.星王分解物品二 = Settings.Default.星王分解物品二);
        S_星王分解物品三.Text = (Config.星王分解物品三 = Settings.Default.星王分解物品三);
        S_星王分解物品四.Text = (Config.星王分解物品四 = Settings.Default.星王分解物品四);
        S_星王分解几率一.Value = (Config.星王分解几率一 = Settings.Default.星王分解几率一);
        S_星王分解几率二.Value = (Config.星王分解几率二 = Settings.Default.星王分解几率二);
        S_星王分解几率三.Value = (Config.星王分解几率三 = Settings.Default.星王分解几率三);
        S_星王分解几率四.Value = (Config.星王分解几率四 = Settings.Default.星王分解几率四);
        S_星王分解数量一.Value = (Config.星王分解数量一 = Settings.Default.星王分解数量一);
        S_星王分解数量二.Value = (Config.星王分解数量二 = Settings.Default.星王分解数量二);
        S_星王分解数量三.Value = (Config.星王分解数量三 = Settings.Default.星王分解数量三);
        S_星王分解数量四.Value = (Config.星王分解数量四 = Settings.Default.星王分解数量四);
        S_星王分解开关.Value = (Config.星王分解开关 = Settings.Default.星王分解开关);
        S_苍月分解物品一.Text = (Config.苍月分解物品一 = Settings.Default.苍月分解物品一);
        S_苍月分解物品二.Text = (Config.苍月分解物品二 = Settings.Default.苍月分解物品二);
        S_苍月分解物品三.Text = (Config.苍月分解物品三 = Settings.Default.苍月分解物品三);
        S_苍月分解物品四.Text = (Config.苍月分解物品四 = Settings.Default.苍月分解物品四);
        S_苍月分解几率一.Value = (Config.苍月分解几率一 = Settings.Default.苍月分解几率一);
        S_苍月分解几率二.Value = (Config.苍月分解几率二 = Settings.Default.苍月分解几率二);
        S_苍月分解几率三.Value = (Config.苍月分解几率三 = Settings.Default.苍月分解几率三);
        S_苍月分解几率四.Value = (Config.苍月分解几率四 = Settings.Default.苍月分解几率四);
        S_苍月分解数量一.Value = (Config.苍月分解数量一 = Settings.Default.苍月分解数量一);
        S_苍月分解数量二.Value = (Config.苍月分解数量二 = Settings.Default.苍月分解数量二);
        S_苍月分解数量三.Value = (Config.苍月分解数量三 = Settings.Default.苍月分解数量三);
        S_苍月分解数量四.Value = (Config.苍月分解数量四 = Settings.Default.苍月分解数量四);
        S_苍月分解开关.Value = (Config.苍月分解开关 = Settings.Default.苍月分解开关);
        S_魔龙分解物品一.Text = (Config.魔龙分解物品一 = Settings.Default.魔龙分解物品一);
        S_魔龙分解物品二.Text = (Config.魔龙分解物品二 = Settings.Default.魔龙分解物品二);
        S_魔龙分解物品三.Text = (Config.魔龙分解物品三 = Settings.Default.魔龙分解物品三);
        S_魔龙分解物品四.Text = (Config.魔龙分解物品四 = Settings.Default.魔龙分解物品四);
        S_魔龙分解几率一.Value = (Config.魔龙分解几率一 = Settings.Default.魔龙分解几率一);
        S_魔龙分解几率二.Value = (Config.魔龙分解几率二 = Settings.Default.魔龙分解几率二);
        S_魔龙分解几率三.Value = (Config.魔龙分解几率三 = Settings.Default.魔龙分解几率三);
        S_魔龙分解几率四.Value = (Config.魔龙分解几率四 = Settings.Default.魔龙分解几率四);
        S_魔龙分解数量一.Value = (Config.魔龙分解数量一 = Settings.Default.魔龙分解数量一);
        S_魔龙分解数量二.Value = (Config.魔龙分解数量二 = Settings.Default.魔龙分解数量二);
        S_魔龙分解数量三.Value = (Config.魔龙分解数量三 = Settings.Default.魔龙分解数量三);
        S_魔龙分解数量四.Value = (Config.魔龙分解数量四 = Settings.Default.魔龙分解数量四);
        S_魔龙分解开关.Value = (Config.魔龙分解开关 = Settings.Default.魔龙分解开关);
        S_赤月分解物品一.Text = (Config.赤月分解物品一 = Settings.Default.赤月分解物品一);
        S_赤月分解物品二.Text = (Config.赤月分解物品二 = Settings.Default.赤月分解物品二);
        S_赤月分解物品三.Text = (Config.赤月分解物品三 = Settings.Default.赤月分解物品三);
        S_赤月分解物品四.Text = (Config.赤月分解物品四 = Settings.Default.赤月分解物品四);
        S_赤月分解几率一.Value = (Config.赤月分解几率一 = Settings.Default.赤月分解几率一);
        S_赤月分解几率二.Value = (Config.赤月分解几率二 = Settings.Default.赤月分解几率二);
        S_赤月分解几率三.Value = (Config.赤月分解几率三 = Settings.Default.赤月分解几率三);
        S_赤月分解几率四.Value = (Config.赤月分解几率四 = Settings.Default.赤月分解几率四);
        S_赤月分解数量一.Value = (Config.赤月分解数量一 = Settings.Default.赤月分解数量一);
        S_赤月分解数量二.Value = (Config.赤月分解数量二 = Settings.Default.赤月分解数量二);
        S_赤月分解数量三.Value = (Config.赤月分解数量三 = Settings.Default.赤月分解数量三);
        S_赤月分解数量四.Value = (Config.赤月分解数量四 = Settings.Default.赤月分解数量四);
        S_赤月分解开关.Value = (Config.赤月分解开关 = Settings.Default.赤月分解开关);
        S_祖玛分解物品一.Text = (Config.祖玛分解物品一 = Settings.Default.祖玛分解物品一);
        S_祖玛分解物品二.Text = (Config.祖玛分解物品二 = Settings.Default.祖玛分解物品二);
        S_祖玛分解物品三.Text = (Config.祖玛分解物品三 = Settings.Default.祖玛分解物品三);
        S_祖玛分解物品四.Text = (Config.祖玛分解物品四 = Settings.Default.祖玛分解物品四);
        S_祖玛分解几率一.Value = (Config.祖玛分解几率一 = Settings.Default.祖玛分解几率一);
        S_祖玛分解几率二.Value = (Config.祖玛分解几率二 = Settings.Default.祖玛分解几率二);
        S_祖玛分解几率三.Value = (Config.祖玛分解几率三 = Settings.Default.祖玛分解几率三);
        S_祖玛分解几率四.Value = (Config.祖玛分解几率四 = Settings.Default.祖玛分解几率四);
        S_祖玛分解数量一.Value = (Config.祖玛分解数量一 = Settings.Default.祖玛分解数量一);
        S_祖玛分解数量二.Value = (Config.祖玛分解数量二 = Settings.Default.祖玛分解数量二);
        S_祖玛分解数量三.Value = (Config.祖玛分解数量三 = Settings.Default.祖玛分解数量三);
        S_祖玛分解数量四.Value = (Config.祖玛分解数量四 = Settings.Default.祖玛分解数量四);
        S_祖玛分解开关.Value = (Config.祖玛分解开关 = Settings.Default.祖玛分解开关);
        S_BOSS卷轴怪物一.Text = (Config.BOSS卷轴怪物一 = Settings.Default.BOSS卷轴怪物一);
        S_BOSS卷轴怪物二.Text = (Config.BOSS卷轴怪物二 = Settings.Default.BOSS卷轴怪物二);
        S_BOSS卷轴怪物三.Text = (Config.BOSS卷轴怪物三 = Settings.Default.BOSS卷轴怪物三);
        S_BOSS卷轴怪物四.Text = (Config.BOSS卷轴怪物四 = Settings.Default.BOSS卷轴怪物四);
        S_BOSS卷轴怪物五.Text = (Config.BOSS卷轴怪物五 = Settings.Default.BOSS卷轴怪物五);
        S_BOSS卷轴怪物六.Text = (Config.BOSS卷轴怪物六 = Settings.Default.BOSS卷轴怪物六);
        S_BOSS卷轴怪物七.Text = (Config.BOSS卷轴怪物七 = Settings.Default.BOSS卷轴怪物七);
        S_BOSS卷轴怪物八.Text = (Config.BOSS卷轴怪物八 = Settings.Default.BOSS卷轴怪物八);
        S_BOSS卷轴怪物九.Text = (Config.BOSS卷轴怪物九 = Settings.Default.BOSS卷轴怪物九);
        S_BOSS卷轴怪物十.Text = (Config.BOSS卷轴怪物十 = Settings.Default.BOSS卷轴怪物十);
        S_BOSS卷轴怪物11.Text = (Config.BOSS卷轴怪物11 = Settings.Default.BOSS卷轴怪物11);
        S_BOSS卷轴怪物12.Text = (Config.BOSS卷轴怪物12 = Settings.Default.BOSS卷轴怪物12);
        S_BOSS卷轴怪物13.Text = (Config.BOSS卷轴怪物13 = Settings.Default.BOSS卷轴怪物13);
        S_BOSS卷轴怪物14.Text = (Config.BOSS卷轴怪物14 = Settings.Default.BOSS卷轴怪物14);
        S_BOSS卷轴怪物15.Text = (Config.BOSS卷轴怪物15 = Settings.Default.BOSS卷轴怪物15);
        S_BOSS卷轴怪物16.Text = (Config.BOSS卷轴怪物16 = Settings.Default.BOSS卷轴怪物16);
        S_BOSS卷轴地图编号.Value = (Config.BOSS卷轴地图编号 = Settings.Default.BOSS卷轴地图编号);
        S_BOSS卷轴地图开关.Value = (Config.BOSS卷轴地图开关 = Settings.Default.BOSS卷轴地图开关);
        S_沙巴克重置系统.Value = (Config.沙巴克重置系统 = Settings.Default.沙巴克重置系统);
        S_资源包开关.Value = (Config.资源包开关 = Settings.Default.资源包开关);
        S_StartingLevel.Value = (Config.StartingLevel = Settings.Default.StartingLevel);
        S_MaxUserConnections.Value = (Config.MaxUserConnections = Settings.Default.MaxUserConnections);
        S_掉落贵重物品颜色.Value = (Config.掉落贵重物品颜色 = Settings.Default.掉落贵重物品颜色);
        S_掉落沃玛物品颜色.Value = (Config.掉落沃玛物品颜色 = Settings.Default.掉落沃玛物品颜色);
        S_掉落祖玛物品颜色.Value = (Config.掉落祖玛物品颜色 = Settings.Default.掉落祖玛物品颜色);
        S_掉落赤月物品颜色.Value = (Config.掉落赤月物品颜色 = Settings.Default.掉落赤月物品颜色);
        S_掉落魔龙物品颜色.Value = (Config.掉落魔龙物品颜色 = Settings.Default.掉落魔龙物品颜色);
        S_掉落苍月物品颜色.Value = (Config.掉落苍月物品颜色 = Settings.Default.掉落苍月物品颜色);
        S_掉落星王物品颜色.Value = (Config.掉落星王物品颜色 = Settings.Default.掉落星王物品颜色);
        S_掉落城主物品颜色.Value = (Config.掉落城主物品颜色 = Settings.Default.掉落城主物品颜色);
        S_掉落书籍物品颜色.Value = (Config.掉落书籍物品颜色 = Settings.Default.掉落书籍物品颜色);
        S_DropPlayerNameColor.Value = (Config.DropPlayerNameColor = Settings.Default.DropPlayerNameColor);
        S_狂暴击杀玩家颜色.Value = (Config.狂暴击杀玩家颜色 = Settings.Default.狂暴击杀玩家颜色);
        S_狂暴被杀玩家颜色.Value = (Config.狂暴被杀玩家颜色 = Settings.Default.狂暴被杀玩家颜色);
        S_祖玛战装备佩戴数量.Value = (Config.祖玛战装备佩戴数量 = Settings.Default.祖玛战装备佩戴数量);
        S_祖玛法装备佩戴数量.Value = (Config.祖玛法装备佩戴数量 = Settings.Default.祖玛法装备佩戴数量);
        S_祖玛道装备佩戴数量.Value = (Config.祖玛道装备佩戴数量 = Settings.Default.祖玛道装备佩戴数量);
        S_祖玛刺装备佩戴数量.Value = (Config.祖玛刺装备佩戴数量 = Settings.Default.祖玛刺装备佩戴数量);
        S_祖玛枪装备佩戴数量.Value = (Config.祖玛枪装备佩戴数量 = Settings.Default.祖玛枪装备佩戴数量);
        S_祖玛弓装备佩戴数量.Value = (Config.祖玛弓装备佩戴数量 = Settings.Default.祖玛弓装备佩戴数量);
        S_赤月战装备佩戴数量.Value = (Config.赤月战装备佩戴数量 = Settings.Default.赤月战装备佩戴数量);
        S_赤月法装备佩戴数量.Value = (Config.赤月法装备佩戴数量 = Settings.Default.赤月法装备佩戴数量);
        S_赤月道装备佩戴数量.Value = (Config.赤月道装备佩戴数量 = Settings.Default.赤月道装备佩戴数量);
        S_赤月刺装备佩戴数量.Value = (Config.赤月刺装备佩戴数量 = Settings.Default.赤月刺装备佩戴数量);
        S_赤月枪装备佩戴数量.Value = (Config.赤月枪装备佩戴数量 = Settings.Default.赤月枪装备佩戴数量);
        S_赤月弓装备佩戴数量.Value = (Config.赤月弓装备佩戴数量 = Settings.Default.赤月弓装备佩戴数量);
        S_魔龙战装备佩戴数量.Value = (Config.魔龙战装备佩戴数量 = Settings.Default.魔龙战装备佩戴数量);
        S_魔龙法装备佩戴数量.Value = (Config.魔龙法装备佩戴数量 = Settings.Default.魔龙法装备佩戴数量);
        S_魔龙道装备佩戴数量.Value = (Config.魔龙道装备佩戴数量 = Settings.Default.魔龙道装备佩戴数量);
        S_魔龙刺装备佩戴数量.Value = (Config.魔龙刺装备佩戴数量 = Settings.Default.魔龙刺装备佩戴数量);
        S_魔龙枪装备佩戴数量.Value = (Config.魔龙枪装备佩戴数量 = Settings.Default.魔龙枪装备佩戴数量);
        S_魔龙弓装备佩戴数量.Value = (Config.魔龙弓装备佩戴数量 = Settings.Default.魔龙弓装备佩戴数量);
        S_苍月战装备佩戴数量.Value = (Config.苍月战装备佩戴数量 = Settings.Default.苍月战装备佩戴数量);
        S_苍月法装备佩戴数量.Value = (Config.苍月法装备佩戴数量 = Settings.Default.苍月法装备佩戴数量);
        S_苍月道装备佩戴数量.Value = (Config.苍月道装备佩戴数量 = Settings.Default.苍月道装备佩戴数量);
        S_苍月刺装备佩戴数量.Value = (Config.苍月刺装备佩戴数量 = Settings.Default.苍月刺装备佩戴数量);
        S_苍月枪装备佩戴数量.Value = (Config.苍月枪装备佩戴数量 = Settings.Default.苍月枪装备佩戴数量);
        S_苍月弓装备佩戴数量.Value = (Config.苍月弓装备佩戴数量 = Settings.Default.苍月弓装备佩戴数量);
        S_星王战装备佩戴数量.Value = (Config.星王战装备佩戴数量 = Settings.Default.星王战装备佩戴数量);
        S_星王法装备佩戴数量.Value = (Config.星王法装备佩戴数量 = Settings.Default.星王法装备佩戴数量);
        S_星王道装备佩戴数量.Value = (Config.星王道装备佩戴数量 = Settings.Default.星王道装备佩戴数量);
        S_星王刺装备佩戴数量.Value = (Config.星王刺装备佩戴数量 = Settings.Default.星王刺装备佩戴数量);
        S_星王枪装备佩戴数量.Value = (Config.星王枪装备佩戴数量 = Settings.Default.星王枪装备佩戴数量);
        S_星王弓装备佩戴数量.Value = (Config.星王弓装备佩戴数量 = Settings.Default.星王弓装备佩戴数量);
        S_特殊1战装备佩戴数量.Value = (Config.特殊1战装备佩戴数量 = Settings.Default.特殊1战装备佩戴数量);
        S_特殊1法装备佩戴数量.Value = (Config.特殊1法装备佩戴数量 = Settings.Default.特殊1法装备佩戴数量);
        S_特殊1道装备佩戴数量.Value = (Config.特殊1道装备佩戴数量 = Settings.Default.特殊1道装备佩戴数量);
        S_特殊1刺装备佩戴数量.Value = (Config.特殊1刺装备佩戴数量 = Settings.Default.特殊1刺装备佩戴数量);
        S_特殊1枪装备佩戴数量.Value = (Config.特殊1枪装备佩戴数量 = Settings.Default.特殊1枪装备佩戴数量);
        S_特殊1弓装备佩戴数量.Value = (Config.特殊1弓装备佩戴数量 = Settings.Default.特殊1弓装备佩戴数量);
        S_特殊2战装备佩戴数量.Value = (Config.特殊2战装备佩戴数量 = Settings.Default.特殊2战装备佩戴数量);
        S_特殊2法装备佩戴数量.Value = (Config.特殊2法装备佩戴数量 = Settings.Default.特殊2法装备佩戴数量);
        S_特殊2道装备佩戴数量.Value = (Config.特殊2道装备佩戴数量 = Settings.Default.特殊2道装备佩戴数量);
        S_特殊2刺装备佩戴数量.Value = (Config.特殊2刺装备佩戴数量 = Settings.Default.特殊2刺装备佩戴数量);
        S_特殊2枪装备佩戴数量.Value = (Config.特殊2枪装备佩戴数量 = Settings.Default.特殊2枪装备佩戴数量);
        S_特殊2弓装备佩戴数量.Value = (Config.特殊2弓装备佩戴数量 = Settings.Default.特殊2弓装备佩戴数量);
        S_特殊3战装备佩戴数量.Value = (Config.特殊3战装备佩戴数量 = Settings.Default.特殊3战装备佩戴数量);
        S_特殊3法装备佩戴数量.Value = (Config.特殊3法装备佩戴数量 = Settings.Default.特殊3法装备佩戴数量);
        S_特殊3道装备佩戴数量.Value = (Config.特殊3道装备佩戴数量 = Settings.Default.特殊3道装备佩戴数量);
        S_特殊3刺装备佩戴数量.Value = (Config.特殊3刺装备佩戴数量 = Settings.Default.特殊3刺装备佩戴数量);
        S_特殊3枪装备佩戴数量.Value = (Config.特殊3枪装备佩戴数量 = Settings.Default.特殊3枪装备佩戴数量);
        S_特殊3弓装备佩戴数量.Value = (Config.特殊3弓装备佩戴数量 = Settings.Default.特殊3弓装备佩戴数量);
        S_每周特惠二物品5.Value = (Config.每周特惠二物品5 = Settings.Default.每周特惠二物品5);
        S_每周特惠二物品4.Value = (Config.每周特惠二物品4 = Settings.Default.每周特惠二物品4);
        S_每周特惠二物品3.Value = (Config.每周特惠二物品3 = Settings.Default.每周特惠二物品3);
        S_每周特惠二物品2.Value = (Config.每周特惠二物品2 = Settings.Default.每周特惠二物品2);
        S_每周特惠二物品1.Value = (Config.每周特惠二物品1 = Settings.Default.每周特惠二物品1);
        S_每周特惠一物品1.Value = (Config.每周特惠一物品1 = Settings.Default.每周特惠一物品1);
        S_每周特惠一物品2.Value = (Config.每周特惠一物品2 = Settings.Default.每周特惠一物品2);
        S_每周特惠一物品3.Value = (Config.每周特惠一物品3 = Settings.Default.每周特惠一物品3);
        S_每周特惠一物品4.Value = (Config.每周特惠一物品4 = Settings.Default.每周特惠一物品4);
        S_每周特惠一物品5.Value = (Config.每周特惠一物品5 = Settings.Default.每周特惠一物品5);
        S_新手出售货币值.Value = (Config.新手出售货币值 = Settings.Default.新手出售货币值);
        S_挂机称号选项.Value = (Config.挂机称号选项 = Settings.Default.挂机称号选项);
        S_分解称号选项.Value = (Config.分解称号选项 = Settings.Default.分解称号选项);
        S_法阵卡BUG清理.Value = (Config.法阵卡BUG清理 = Settings.Default.法阵卡BUG清理);
        S_随机宝箱一物品1.Value = (Config.随机宝箱一物品1 = Settings.Default.随机宝箱一物品1);
        S_随机宝箱一物品2.Value = (Config.随机宝箱一物品2 = Settings.Default.随机宝箱一物品2);
        S_随机宝箱一物品3.Value = (Config.随机宝箱一物品3 = Settings.Default.随机宝箱一物品3);
        S_随机宝箱一物品4.Value = (Config.随机宝箱一物品4 = Settings.Default.随机宝箱一物品4);
        S_随机宝箱一物品5.Value = (Config.随机宝箱一物品5 = Settings.Default.随机宝箱一物品5);
        S_随机宝箱一物品6.Value = (Config.随机宝箱一物品6 = Settings.Default.随机宝箱一物品6);
        S_随机宝箱一物品7.Value = (Config.随机宝箱一物品7 = Settings.Default.随机宝箱一物品7);
        S_随机宝箱一物品8.Value = (Config.随机宝箱一物品8 = Settings.Default.随机宝箱一物品8);
        S_随机宝箱一几率1.Value = (Config.随机宝箱一几率1 = Settings.Default.随机宝箱一几率1);
        S_随机宝箱一几率2.Value = (Config.随机宝箱一几率2 = Settings.Default.随机宝箱一几率2);
        S_随机宝箱一几率3.Value = (Config.随机宝箱一几率3 = Settings.Default.随机宝箱一几率3);
        S_随机宝箱一几率4.Value = (Config.随机宝箱一几率4 = Settings.Default.随机宝箱一几率4);
        S_随机宝箱一几率5.Value = (Config.随机宝箱一几率5 = Settings.Default.随机宝箱一几率5);
        S_随机宝箱一几率6.Value = (Config.随机宝箱一几率6 = Settings.Default.随机宝箱一几率6);
        S_随机宝箱一几率7.Value = (Config.随机宝箱一几率7 = Settings.Default.随机宝箱一几率7);
        S_随机宝箱一几率8.Value = (Config.随机宝箱一几率8 = Settings.Default.随机宝箱一几率8);
        S_随机宝箱二物品1.Value = (Config.随机宝箱二物品1 = Settings.Default.随机宝箱二物品1);
        S_随机宝箱二物品2.Value = (Config.随机宝箱二物品2 = Settings.Default.随机宝箱二物品2);
        S_随机宝箱二物品3.Value = (Config.随机宝箱二物品3 = Settings.Default.随机宝箱二物品3);
        S_随机宝箱二物品4.Value = (Config.随机宝箱二物品4 = Settings.Default.随机宝箱二物品4);
        S_随机宝箱二物品5.Value = (Config.随机宝箱二物品5 = Settings.Default.随机宝箱二物品5);
        S_随机宝箱二物品6.Value = (Config.随机宝箱二物品6 = Settings.Default.随机宝箱二物品6);
        S_随机宝箱二物品7.Value = (Config.随机宝箱二物品7 = Settings.Default.随机宝箱二物品7);
        S_随机宝箱二物品8.Value = (Config.随机宝箱二物品8 = Settings.Default.随机宝箱二物品8);
        S_随机宝箱二几率1.Value = (Config.随机宝箱二几率1 = Settings.Default.随机宝箱二几率1);
        S_随机宝箱二几率2.Value = (Config.随机宝箱二几率2 = Settings.Default.随机宝箱二几率2);
        S_随机宝箱二几率3.Value = (Config.随机宝箱二几率3 = Settings.Default.随机宝箱二几率3);
        S_随机宝箱二几率4.Value = (Config.随机宝箱二几率4 = Settings.Default.随机宝箱二几率4);
        S_随机宝箱二几率5.Value = (Config.随机宝箱二几率5 = Settings.Default.随机宝箱二几率5);
        S_随机宝箱二几率6.Value = (Config.随机宝箱二几率6 = Settings.Default.随机宝箱二几率6);
        S_随机宝箱二几率7.Value = (Config.随机宝箱二几率7 = Settings.Default.随机宝箱二几率7);
        S_随机宝箱二几率8.Value = (Config.随机宝箱二几率8 = Settings.Default.随机宝箱二几率8);
        S_随机宝箱三物品1.Value = (Config.随机宝箱三物品1 = Settings.Default.随机宝箱三物品1);
        S_随机宝箱三物品2.Value = (Config.随机宝箱三物品2 = Settings.Default.随机宝箱三物品2);
        S_随机宝箱三物品3.Value = (Config.随机宝箱三物品3 = Settings.Default.随机宝箱三物品3);
        S_随机宝箱三物品4.Value = (Config.随机宝箱三物品4 = Settings.Default.随机宝箱三物品4);
        S_随机宝箱三物品5.Value = (Config.随机宝箱三物品5 = Settings.Default.随机宝箱三物品5);
        S_随机宝箱三物品6.Value = (Config.随机宝箱三物品6 = Settings.Default.随机宝箱三物品6);
        S_随机宝箱三物品7.Value = (Config.随机宝箱三物品7 = Settings.Default.随机宝箱三物品7);
        S_随机宝箱三物品8.Value = (Config.随机宝箱三物品8 = Settings.Default.随机宝箱三物品8);
        S_随机宝箱三几率1.Value = (Config.随机宝箱三几率1 = Settings.Default.随机宝箱三几率1);
        S_随机宝箱三几率2.Value = (Config.随机宝箱三几率2 = Settings.Default.随机宝箱三几率2);
        S_随机宝箱三几率3.Value = (Config.随机宝箱三几率3 = Settings.Default.随机宝箱三几率3);
        S_随机宝箱三几率4.Value = (Config.随机宝箱三几率4 = Settings.Default.随机宝箱三几率4);
        S_随机宝箱三几率5.Value = (Config.随机宝箱三几率5 = Settings.Default.随机宝箱三几率5);
        S_随机宝箱三几率6.Value = (Config.随机宝箱三几率6 = Settings.Default.随机宝箱三几率6);
        S_随机宝箱三几率7.Value = (Config.随机宝箱三几率7 = Settings.Default.随机宝箱三几率7);
        S_随机宝箱三几率8.Value = (Config.随机宝箱三几率8 = Settings.Default.随机宝箱三几率8);
        S_随机宝箱一数量1.Value = (Config.随机宝箱一数量1 = Settings.Default.随机宝箱一数量1);
        S_随机宝箱一数量2.Value = (Config.随机宝箱一数量2 = Settings.Default.随机宝箱一数量2);
        S_随机宝箱一数量3.Value = (Config.随机宝箱一数量3 = Settings.Default.随机宝箱一数量3);
        S_随机宝箱一数量4.Value = (Config.随机宝箱一数量4 = Settings.Default.随机宝箱一数量4);
        S_随机宝箱一数量5.Value = (Config.随机宝箱一数量5 = Settings.Default.随机宝箱一数量5);
        S_随机宝箱一数量6.Value = (Config.随机宝箱一数量6 = Settings.Default.随机宝箱一数量6);
        S_随机宝箱一数量7.Value = (Config.随机宝箱一数量7 = Settings.Default.随机宝箱一数量7);
        S_随机宝箱一数量8.Value = (Config.随机宝箱一数量8 = Settings.Default.随机宝箱一数量8);
        S_随机宝箱二数量1.Value = (Config.随机宝箱二数量1 = Settings.Default.随机宝箱二数量1);
        S_随机宝箱二数量2.Value = (Config.随机宝箱二数量2 = Settings.Default.随机宝箱二数量2);
        S_随机宝箱二数量3.Value = (Config.随机宝箱二数量3 = Settings.Default.随机宝箱二数量3);
        S_随机宝箱二数量4.Value = (Config.随机宝箱二数量4 = Settings.Default.随机宝箱二数量4);
        S_随机宝箱二数量5.Value = (Config.随机宝箱二数量5 = Settings.Default.随机宝箱二数量5);
        S_随机宝箱二数量6.Value = (Config.随机宝箱二数量6 = Settings.Default.随机宝箱二数量6);
        S_随机宝箱二数量7.Value = (Config.随机宝箱二数量7 = Settings.Default.随机宝箱二数量7);
        S_随机宝箱二数量8.Value = (Config.随机宝箱二数量8 = Settings.Default.随机宝箱二数量8);
        S_随机宝箱三数量1.Value = (Config.随机宝箱三数量1 = Settings.Default.随机宝箱三数量1);
        S_随机宝箱三数量2.Value = (Config.随机宝箱三数量2 = Settings.Default.随机宝箱三数量2);
        S_随机宝箱三数量3.Value = (Config.随机宝箱三数量3 = Settings.Default.随机宝箱三数量3);
        S_随机宝箱三数量4.Value = (Config.随机宝箱三数量4 = Settings.Default.随机宝箱三数量4);
        S_随机宝箱三数量5.Value = (Config.随机宝箱三数量5 = Settings.Default.随机宝箱三数量5);
        S_随机宝箱三数量6.Value = (Config.随机宝箱三数量6 = Settings.Default.随机宝箱三数量6);
        S_随机宝箱三数量7.Value = (Config.随机宝箱三数量7 = Settings.Default.随机宝箱三数量7);
        S_随机宝箱三数量8.Value = (Config.随机宝箱三数量8 = Settings.Default.随机宝箱三数量8);
        S_沙城地图保护.Value = (Config.沙城地图保护 = Settings.Default.沙城地图保护);
        S_NoobProtectionLevel.Value = (Config.NoobProtectionLevel = Settings.Default.NoobProtectionLevel);
        S_新手地图保护1.Value = (Config.新手地图保护1 = Settings.Default.新手地图保护1);
        S_新手地图保护2.Value = (Config.新手地图保护2 = Settings.Default.新手地图保护2);
        S_新手地图保护3.Value = (Config.新手地图保护3 = Settings.Default.新手地图保护3);
        S_新手地图保护4.Value = (Config.新手地图保护4 = Settings.Default.新手地图保护4);
        S_新手地图保护5.Value = (Config.新手地图保护5 = Settings.Default.新手地图保护5);
        S_新手地图保护6.Value = (Config.新手地图保护6 = Settings.Default.新手地图保护6);
        S_新手地图保护7.Value = (Config.新手地图保护7 = Settings.Default.新手地图保护7);
        S_新手地图保护8.Value = (Config.新手地图保护8 = Settings.Default.新手地图保护8);
        S_新手地图保护9.Value = (Config.新手地图保护9 = Settings.Default.新手地图保护9);
        S_新手地图保护10.Value = (Config.新手地图保护10 = Settings.Default.新手地图保护10);
        S_沙巴克停止开关.Value = (Config.沙巴克停止开关 = Settings.Default.沙巴克停止开关);
        S_沙巴克城主称号.Value = (Config.沙巴克城主称号 = Settings.Default.沙巴克城主称号);
        S_沙巴克成员称号.Value = (Config.沙巴克成员称号 = Settings.Default.沙巴克成员称号);
        S_沙巴克称号领取开关.Value = (Config.沙巴克称号领取开关 = Settings.Default.沙巴克称号领取开关);
        S_通用1装备佩戴数量.Value = (Config.通用1装备佩戴数量 = Settings.Default.通用1装备佩戴数量);
        S_通用2装备佩戴数量.Value = (Config.通用2装备佩戴数量 = Settings.Default.通用2装备佩戴数量);
        S_通用3装备佩戴数量.Value = (Config.通用3装备佩戴数量 = Settings.Default.通用3装备佩戴数量);
        S_通用4装备佩戴数量.Value = (Config.通用4装备佩戴数量 = Settings.Default.通用4装备佩戴数量);
        S_通用5装备佩戴数量.Value = (Config.通用5装备佩戴数量 = Settings.Default.通用5装备佩戴数量);
        S_通用6装备佩戴数量.Value = (Config.通用6装备佩戴数量 = Settings.Default.通用6装备佩戴数量);
        S_重置屠魔副本时间.Value = (Config.重置屠魔副本时间 = Settings.Default.重置屠魔副本时间);
        S_屠魔令回收数量.Value = (Config.重置屠魔副本时间 = Settings.Default.重置屠魔副本时间);
        S_新手上线赠送开关.Value = (Config.新手上线赠送开关 = Settings.Default.新手上线赠送开关);
        S_新手上线赠送物品1.Value = (Config.新手上线赠送物品1 = Settings.Default.新手上线赠送物品1);
        S_新手上线赠送物品2.Value = (Config.新手上线赠送物品2 = Settings.Default.新手上线赠送物品2);
        S_新手上线赠送物品3.Value = (Config.新手上线赠送物品3 = Settings.Default.新手上线赠送物品3);
        S_新手上线赠送物品4.Value = (Config.新手上线赠送物品4 = Settings.Default.新手上线赠送物品4);
        S_新手上线赠送物品5.Value = (Config.新手上线赠送物品5 = Settings.Default.新手上线赠送物品5);
        S_新手上线赠送物品6.Value = (Config.新手上线赠送物品6 = Settings.Default.新手上线赠送物品6);
        S_新手上线赠送称号1.Value = (Config.新手上线赠送称号1 = Settings.Default.新手上线赠送称号1);
        S_元宝袋新创数量1.Value = (Config.元宝袋新创数量1 = Settings.Default.元宝袋新创数量1);
        S_元宝袋新创数量2.Value = (Config.元宝袋新创数量2 = Settings.Default.元宝袋新创数量2);
        S_元宝袋新创数量3.Value = (Config.元宝袋新创数量3 = Settings.Default.元宝袋新创数量3);
        S_元宝袋新创数量4.Value = (Config.元宝袋新创数量4 = Settings.Default.元宝袋新创数量4);
        S_元宝袋新创数量5.Value = (Config.元宝袋新创数量5 = Settings.Default.元宝袋新创数量5);
        S_初级赞助礼包1.Value = (Config.初级赞助礼包1 = Settings.Default.初级赞助礼包1);
        S_初级赞助礼包2.Value = (Config.初级赞助礼包2 = Settings.Default.初级赞助礼包2);
        S_初级赞助礼包3.Value = (Config.初级赞助礼包3 = Settings.Default.初级赞助礼包3);
        S_初级赞助礼包4.Value = (Config.初级赞助礼包4 = Settings.Default.初级赞助礼包4);
        S_初级赞助礼包5.Value = (Config.初级赞助礼包5 = Settings.Default.初级赞助礼包5);
        S_初级赞助礼包6.Value = (Config.初级赞助礼包6 = Settings.Default.初级赞助礼包6);
        S_初级赞助礼包7.Value = (Config.初级赞助礼包7 = Settings.Default.初级赞助礼包7);
        S_初级赞助礼包8.Value = (Config.初级赞助礼包8 = Settings.Default.初级赞助礼包8);
        S_初级赞助称号1.Value = (Config.初级赞助称号1 = Settings.Default.初级赞助称号1);
        S_中级赞助礼包1.Value = (Config.中级赞助礼包1 = Settings.Default.中级赞助礼包1);
        S_中级赞助礼包2.Value = (Config.中级赞助礼包2 = Settings.Default.中级赞助礼包2);
        S_中级赞助礼包3.Value = (Config.中级赞助礼包3 = Settings.Default.中级赞助礼包3);
        S_中级赞助礼包4.Value = (Config.中级赞助礼包4 = Settings.Default.中级赞助礼包4);
        S_中级赞助礼包5.Value = (Config.中级赞助礼包5 = Settings.Default.中级赞助礼包5);
        S_中级赞助礼包6.Value = (Config.中级赞助礼包6 = Settings.Default.中级赞助礼包6);
        S_中级赞助礼包7.Value = (Config.中级赞助礼包7 = Settings.Default.中级赞助礼包7);
        S_中级赞助礼包8.Value = (Config.中级赞助礼包8 = Settings.Default.中级赞助礼包8);
        S_中级赞助称号1.Value = (Config.中级赞助称号1 = Settings.Default.中级赞助称号1);
        S_高级赞助礼包1.Value = (Config.高级赞助礼包1 = Settings.Default.高级赞助礼包1);
        S_高级赞助礼包2.Value = (Config.高级赞助礼包2 = Settings.Default.高级赞助礼包2);
        S_高级赞助礼包3.Value = (Config.高级赞助礼包3 = Settings.Default.高级赞助礼包3);
        S_高级赞助礼包4.Value = (Config.高级赞助礼包4 = Settings.Default.高级赞助礼包4);
        S_高级赞助礼包5.Value = (Config.高级赞助礼包5 = Settings.Default.高级赞助礼包5);
        S_高级赞助礼包6.Value = (Config.高级赞助礼包6 = Settings.Default.高级赞助礼包6);
        S_高级赞助礼包7.Value = (Config.高级赞助礼包7 = Settings.Default.高级赞助礼包7);
        S_高级赞助礼包8.Value = (Config.高级赞助礼包8 = Settings.Default.高级赞助礼包8);
        S_高级赞助称号1.Value = (Config.高级赞助称号1 = Settings.Default.高级赞助称号1);
        S_自动BOSS1界面1开关.Value = (Config.自动BOSS1界面1开关 = Settings.Default.自动BOSS1界面1开关);
        S_自动BOSS1界面2开关.Value = (Config.自动BOSS1界面2开关 = Settings.Default.自动BOSS1界面2开关);
        S_自动BOSS1界面3开关.Value = (Config.自动BOSS1界面3开关 = Settings.Default.自动BOSS1界面3开关);
        S_自动BOSS1界面4开关.Value = (Config.自动BOSS1界面4开关 = Settings.Default.自动BOSS1界面4开关);
        S_自动BOSS1界面5开关.Value = (Config.自动BOSS1界面5开关 = Settings.Default.自动BOSS1界面5开关);
        S_平台开关模式.Value = (Config.平台开关模式 = Settings.Default.平台开关模式);
        S_平台元宝充值模块.Value = (Config.平台元宝充值模块 = Settings.Default.平台元宝充值模块);
        S_九层妖塔数量1.Value = (Config.九层妖塔数量1 = Settings.Default.九层妖塔数量1);
        S_九层妖塔数量2.Value = (Config.九层妖塔数量2 = Settings.Default.九层妖塔数量2);
        S_九层妖塔数量3.Value = (Config.九层妖塔数量3 = Settings.Default.九层妖塔数量3);
        S_九层妖塔数量4.Value = (Config.九层妖塔数量4 = Settings.Default.九层妖塔数量4);
        S_九层妖塔数量5.Value = (Config.九层妖塔数量5 = Settings.Default.九层妖塔数量5);
        S_九层妖塔数量6.Value = (Config.九层妖塔数量6 = Settings.Default.九层妖塔数量6);
        S_九层妖塔数量7.Value = (Config.九层妖塔数量7 = Settings.Default.九层妖塔数量7);
        S_九层妖塔数量8.Value = (Config.九层妖塔数量8 = Settings.Default.九层妖塔数量8);
        S_九层妖塔数量9.Value = (Config.九层妖塔数量9 = Settings.Default.九层妖塔数量9);
        S_九层妖塔副本次数.Value = (Config.九层妖塔副本次数 = Settings.Default.九层妖塔副本次数);
        S_九层妖塔副本等级.Value = (Config.九层妖塔副本等级 = Settings.Default.九层妖塔副本等级);
        S_九层妖塔副本物品.Value = (Config.九层妖塔副本物品 = Settings.Default.九层妖塔副本物品);
        S_九层妖塔副本数量.Value = (Config.九层妖塔副本数量 = Settings.Default.九层妖塔副本数量);
        S_九层妖塔副本时间小.Value = (Config.九层妖塔副本时间小 = Settings.Default.九层妖塔副本时间小);
        S_九层妖塔副本时间大.Value = (Config.九层妖塔副本时间大 = Settings.Default.九层妖塔副本时间大);
        S_九层妖塔BOSS1.Text = (Config.九层妖塔BOSS1 = Settings.Default.九层妖塔BOSS1);
        S_九层妖塔BOSS2.Text = (Config.九层妖塔BOSS2 = Settings.Default.九层妖塔BOSS2);
        S_九层妖塔BOSS3.Text = (Config.九层妖塔BOSS3 = Settings.Default.九层妖塔BOSS3);
        S_九层妖塔BOSS4.Text = (Config.九层妖塔BOSS4 = Settings.Default.九层妖塔BOSS4);
        S_九层妖塔BOSS5.Text = (Config.九层妖塔BOSS5 = Settings.Default.九层妖塔BOSS5);
        S_九层妖塔BOSS6.Text = (Config.九层妖塔BOSS6 = Settings.Default.九层妖塔BOSS6);
        S_九层妖塔BOSS7.Text = (Config.九层妖塔BOSS7 = Settings.Default.九层妖塔BOSS7);
        S_九层妖塔BOSS8.Text = (Config.九层妖塔BOSS8 = Settings.Default.九层妖塔BOSS8);
        S_九层妖塔BOSS9.Text = (Config.九层妖塔BOSS9 = Settings.Default.九层妖塔BOSS9);
        S_九层妖塔精英1.Text = (Config.九层妖塔精英1 = Settings.Default.九层妖塔精英1);
        S_九层妖塔精英2.Text = (Config.九层妖塔精英2 = Settings.Default.九层妖塔精英2);
        S_九层妖塔精英3.Text = (Config.九层妖塔精英3 = Settings.Default.九层妖塔精英3);
        S_九层妖塔精英4.Text = (Config.九层妖塔精英4 = Settings.Default.九层妖塔精英4);
        S_九层妖塔精英5.Text = (Config.九层妖塔精英5 = Settings.Default.九层妖塔精英5);
        S_九层妖塔精英6.Text = (Config.九层妖塔精英6 = Settings.Default.九层妖塔精英6);
        S_九层妖塔精英7.Text = (Config.九层妖塔精英7 = Settings.Default.九层妖塔精英7);
        S_九层妖塔精英8.Text = (Config.九层妖塔精英8 = Settings.Default.九层妖塔精英8);
        S_九层妖塔精英9.Text = (Config.九层妖塔精英9 = Settings.Default.九层妖塔精英9);
        S_AutoBattleLevel.Value = (Config.AutoBattleLevel = Settings.Default.AutoBattleLevel);
        S_禁止背包铭文洗练.Value = (Config.禁止背包铭文洗练 = Settings.Default.禁止背包铭文洗练);
        S_沙巴克禁止随机.Value = (Config.沙巴克禁止随机 = Settings.Default.沙巴克禁止随机);
        S_冥想丹自定义经验.Value = (Config.冥想丹自定义经验 = Settings.Default.冥想丹自定义经验);
        S_沙巴克爆装备开关.Value = (Config.沙巴克爆装备开关 = Settings.Default.沙巴克爆装备开关);
        S_铭文战士1挡1次数.Value = (Config.铭文战士1挡1次数 = Settings.Default.铭文战士1挡1次数);
        S_铭文战士1挡2次数.Value = (Config.铭文战士1挡2次数 = Settings.Default.铭文战士1挡2次数);
        S_铭文战士1挡3次数.Value = (Config.铭文战士1挡3次数 = Settings.Default.铭文战士1挡3次数);
        S_铭文战士1挡1概率.Value = (Config.铭文战士1挡1概率 = Settings.Default.铭文战士1挡1概率);
        S_铭文战士1挡2概率.Value = (Config.铭文战士1挡2概率 = Settings.Default.铭文战士1挡2概率);
        S_铭文战士1挡3概率.Value = (Config.铭文战士1挡3概率 = Settings.Default.铭文战士1挡3概率);
        S_铭文战士1挡技能编号.Value = (Config.铭文战士1挡技能编号 = Settings.Default.铭文战士1挡技能编号);
        S_铭文战士1挡技能铭文.Value = (Config.铭文战士1挡技能铭文 = Settings.Default.铭文战士1挡技能铭文);
        S_铭文战士2挡1次数.Value = (Config.铭文战士2挡1次数 = Settings.Default.铭文战士2挡1次数);
        S_铭文战士2挡2次数.Value = (Config.铭文战士2挡2次数 = Settings.Default.铭文战士2挡2次数);
        S_铭文战士2挡3次数.Value = (Config.铭文战士2挡3次数 = Settings.Default.铭文战士2挡3次数);
        S_铭文战士2挡1概率.Value = (Config.铭文战士2挡1概率 = Settings.Default.铭文战士2挡1概率);
        S_铭文战士2挡2概率.Value = (Config.铭文战士2挡2概率 = Settings.Default.铭文战士2挡2概率);
        S_铭文战士2挡3概率.Value = (Config.铭文战士2挡3概率 = Settings.Default.铭文战士2挡3概率);
        S_铭文战士2挡技能编号.Value = (Config.铭文战士2挡技能编号 = Settings.Default.铭文战士2挡技能编号);
        S_铭文战士2挡技能铭文.Value = (Config.铭文战士2挡技能铭文 = Settings.Default.铭文战士2挡技能铭文);
        S_铭文战士3挡1次数.Value = (Config.铭文战士3挡1次数 = Settings.Default.铭文战士3挡1次数);
        S_铭文战士3挡2次数.Value = (Config.铭文战士3挡2次数 = Settings.Default.铭文战士3挡2次数);
        S_铭文战士3挡3次数.Value = (Config.铭文战士3挡3次数 = Settings.Default.铭文战士3挡3次数);
        S_铭文战士3挡1概率.Value = (Config.铭文战士3挡1概率 = Settings.Default.铭文战士3挡1概率);
        S_铭文战士3挡2概率.Value = (Config.铭文战士3挡2概率 = Settings.Default.铭文战士3挡2概率);
        S_铭文战士3挡3概率.Value = (Config.铭文战士3挡3概率 = Settings.Default.铭文战士3挡3概率);
        S_铭文战士3挡技能编号.Value = (Config.铭文战士3挡技能编号 = Settings.Default.铭文战士3挡技能编号);
        S_铭文战士3挡技能铭文.Value = (Config.铭文战士3挡技能铭文 = Settings.Default.铭文战士3挡技能铭文);
        S_铭文法师1挡1次数.Value = (Config.铭文法师1挡1次数 = Settings.Default.铭文法师1挡1次数);
        S_铭文法师1挡2次数.Value = (Config.铭文法师1挡2次数 = Settings.Default.铭文法师1挡2次数);
        S_铭文法师1挡3次数.Value = (Config.铭文法师1挡3次数 = Settings.Default.铭文法师1挡3次数);
        S_铭文法师1挡1概率.Value = (Config.铭文法师1挡1概率 = Settings.Default.铭文法师1挡1概率);
        S_铭文法师1挡2概率.Value = (Config.铭文法师1挡2概率 = Settings.Default.铭文法师1挡2概率);
        S_铭文法师1挡3概率.Value = (Config.铭文法师1挡3概率 = Settings.Default.铭文法师1挡3概率);
        S_铭文法师1挡技能编号.Value = (Config.铭文法师1挡技能编号 = Settings.Default.铭文法师1挡技能编号);
        S_铭文法师1挡技能铭文.Value = (Config.铭文法师1挡技能铭文 = Settings.Default.铭文法师1挡技能铭文);
        S_铭文法师2挡1次数.Value = (Config.铭文法师2挡1次数 = Settings.Default.铭文法师2挡1次数);
        S_铭文法师2挡2次数.Value = (Config.铭文法师2挡2次数 = Settings.Default.铭文法师2挡2次数);
        S_铭文法师2挡3次数.Value = (Config.铭文法师2挡3次数 = Settings.Default.铭文法师2挡3次数);
        S_铭文法师2挡1概率.Value = (Config.铭文法师2挡1概率 = Settings.Default.铭文法师2挡1概率);
        S_铭文法师2挡2概率.Value = (Config.铭文法师2挡2概率 = Settings.Default.铭文法师2挡2概率);
        S_铭文法师2挡3概率.Value = (Config.铭文法师2挡3概率 = Settings.Default.铭文法师2挡3概率);
        S_铭文法师2挡技能编号.Value = (Config.铭文法师2挡技能编号 = Settings.Default.铭文法师2挡技能编号);
        S_铭文法师2挡技能铭文.Value = (Config.铭文法师2挡技能铭文 = Settings.Default.铭文法师2挡技能铭文);
        S_铭文法师3挡1次数.Value = (Config.铭文法师3挡1次数 = Settings.Default.铭文法师3挡1次数);
        S_铭文法师3挡2次数.Value = (Config.铭文法师3挡2次数 = Settings.Default.铭文法师3挡2次数);
        S_铭文法师3挡3次数.Value = (Config.铭文法师3挡3次数 = Settings.Default.铭文法师3挡3次数);
        S_铭文法师3挡1概率.Value = (Config.铭文法师3挡1概率 = Settings.Default.铭文法师3挡1概率);
        S_铭文法师3挡2概率.Value = (Config.铭文法师3挡2概率 = Settings.Default.铭文法师3挡2概率);
        S_铭文法师3挡3概率.Value = (Config.铭文法师3挡3概率 = Settings.Default.铭文法师3挡3概率);
        S_铭文法师3挡技能编号.Value = (Config.铭文法师3挡技能编号 = Settings.Default.铭文法师3挡技能编号);
        S_铭文法师3挡技能铭文.Value = (Config.铭文法师3挡技能铭文 = Settings.Default.铭文法师3挡技能铭文);
        S_铭文道士1挡1次数.Value = (Config.铭文道士1挡1次数 = Settings.Default.铭文道士1挡1次数);
        S_铭文道士1挡2次数.Value = (Config.铭文道士1挡2次数 = Settings.Default.铭文道士1挡2次数);
        S_铭文道士1挡3次数.Value = (Config.铭文道士1挡3次数 = Settings.Default.铭文道士1挡3次数);
        S_铭文道士1挡1概率.Value = (Config.铭文道士1挡1概率 = Settings.Default.铭文道士1挡1概率);
        S_铭文道士1挡2概率.Value = (Config.铭文道士1挡2概率 = Settings.Default.铭文道士1挡2概率);
        S_铭文道士1挡3概率.Value = (Config.铭文道士1挡3概率 = Settings.Default.铭文道士1挡3概率);
        S_铭文道士1挡技能编号.Value = (Config.铭文道士1挡技能编号 = Settings.Default.铭文道士1挡技能编号);
        S_铭文道士1挡技能铭文.Value = (Config.铭文道士1挡技能铭文 = Settings.Default.铭文道士1挡技能铭文);
        S_铭文道士2挡1次数.Value = (Config.铭文道士2挡1次数 = Settings.Default.铭文道士2挡1次数);
        S_铭文道士2挡2次数.Value = (Config.铭文道士2挡2次数 = Settings.Default.铭文道士2挡2次数);
        S_铭文道士2挡3次数.Value = (Config.铭文道士2挡3次数 = Settings.Default.铭文道士2挡3次数);
        S_铭文道士2挡1概率.Value = (Config.铭文道士2挡1概率 = Settings.Default.铭文道士2挡1概率);
        S_铭文道士2挡2概率.Value = (Config.铭文道士2挡2概率 = Settings.Default.铭文道士2挡2概率);
        S_铭文道士2挡3概率.Value = (Config.铭文道士2挡3概率 = Settings.Default.铭文道士2挡3概率);
        S_铭文道士2挡技能编号.Value = (Config.铭文道士2挡技能编号 = Settings.Default.铭文道士2挡技能编号);
        S_铭文道士2挡技能铭文.Value = (Config.铭文道士2挡技能铭文 = Settings.Default.铭文道士2挡技能铭文);
        S_铭文道士3挡1次数.Value = (Config.铭文道士3挡1次数 = Settings.Default.铭文道士3挡1次数);
        S_铭文道士3挡2次数.Value = (Config.铭文道士3挡2次数 = Settings.Default.铭文道士3挡2次数);
        S_铭文道士3挡3次数.Value = (Config.铭文道士3挡3次数 = Settings.Default.铭文道士3挡3次数);
        S_铭文道士3挡1概率.Value = (Config.铭文道士3挡1概率 = Settings.Default.铭文道士3挡1概率);
        S_铭文道士3挡2概率.Value = (Config.铭文道士3挡2概率 = Settings.Default.铭文道士3挡2概率);
        S_铭文道士3挡3概率.Value = (Config.铭文道士3挡3概率 = Settings.Default.铭文道士3挡3概率);
        S_铭文道士3挡技能编号.Value = (Config.铭文道士3挡技能编号 = Settings.Default.铭文道士3挡技能编号);
        S_铭文道士3挡技能铭文.Value = (Config.铭文道士3挡技能铭文 = Settings.Default.铭文道士3挡技能铭文);
        S_铭文刺客1挡1次数.Value = (Config.铭文刺客1挡1次数 = Settings.Default.铭文刺客1挡1次数);
        S_铭文刺客1挡2次数.Value = (Config.铭文刺客1挡2次数 = Settings.Default.铭文刺客1挡2次数);
        S_铭文刺客1挡3次数.Value = (Config.铭文刺客1挡3次数 = Settings.Default.铭文刺客1挡3次数);
        S_铭文刺客1挡1概率.Value = (Config.铭文刺客1挡1概率 = Settings.Default.铭文刺客1挡1概率);
        S_铭文刺客1挡2概率.Value = (Config.铭文刺客1挡2概率 = Settings.Default.铭文刺客1挡2概率);
        S_铭文刺客1挡3概率.Value = (Config.铭文刺客1挡3概率 = Settings.Default.铭文刺客1挡3概率);
        S_铭文刺客1挡技能编号.Value = (Config.铭文刺客1挡技能编号 = Settings.Default.铭文刺客1挡技能编号);
        S_铭文刺客1挡技能铭文.Value = (Config.铭文刺客1挡技能铭文 = Settings.Default.铭文刺客1挡技能铭文);
        S_铭文刺客2挡1次数.Value = (Config.铭文刺客2挡1次数 = Settings.Default.铭文刺客2挡1次数);
        S_铭文刺客2挡2次数.Value = (Config.铭文刺客2挡2次数 = Settings.Default.铭文刺客2挡2次数);
        S_铭文刺客2挡3次数.Value = (Config.铭文刺客2挡3次数 = Settings.Default.铭文刺客2挡3次数);
        S_铭文刺客2挡1概率.Value = (Config.铭文刺客2挡1概率 = Settings.Default.铭文刺客2挡1概率);
        S_铭文刺客2挡2概率.Value = (Config.铭文刺客2挡2概率 = Settings.Default.铭文刺客2挡2概率);
        S_铭文刺客2挡3概率.Value = (Config.铭文刺客2挡3概率 = Settings.Default.铭文刺客2挡3概率);
        S_铭文刺客2挡技能编号.Value = (Config.铭文刺客2挡技能编号 = Settings.Default.铭文刺客2挡技能编号);
        S_铭文刺客2挡技能铭文.Value = (Config.铭文刺客2挡技能铭文 = Settings.Default.铭文刺客2挡技能铭文);
        S_铭文刺客3挡1次数.Value = (Config.铭文刺客3挡1次数 = Settings.Default.铭文刺客3挡1次数);
        S_铭文刺客3挡2次数.Value = (Config.铭文刺客3挡2次数 = Settings.Default.铭文刺客3挡2次数);
        S_铭文刺客3挡3次数.Value = (Config.铭文刺客3挡3次数 = Settings.Default.铭文刺客3挡3次数);
        S_铭文刺客3挡1概率.Value = (Config.铭文刺客3挡1概率 = Settings.Default.铭文刺客3挡1概率);
        S_铭文刺客3挡2概率.Value = (Config.铭文刺客3挡2概率 = Settings.Default.铭文刺客3挡2概率);
        S_铭文刺客3挡3概率.Value = (Config.铭文刺客3挡3概率 = Settings.Default.铭文刺客3挡3概率);
        S_铭文刺客3挡技能编号.Value = (Config.铭文刺客3挡技能编号 = Settings.Default.铭文刺客3挡技能编号);
        S_铭文刺客3挡技能铭文.Value = (Config.铭文刺客3挡技能铭文 = Settings.Default.铭文刺客3挡技能铭文);
        S_铭文弓手1挡1次数.Value = (Config.铭文弓手1挡1次数 = Settings.Default.铭文弓手1挡1次数);
        S_铭文弓手1挡2次数.Value = (Config.铭文弓手1挡2次数 = Settings.Default.铭文弓手1挡2次数);
        S_铭文弓手1挡3次数.Value = (Config.铭文弓手1挡3次数 = Settings.Default.铭文弓手1挡3次数);
        S_铭文弓手1挡1概率.Value = (Config.铭文弓手1挡1概率 = Settings.Default.铭文弓手1挡1概率);
        S_铭文弓手1挡2概率.Value = (Config.铭文弓手1挡2概率 = Settings.Default.铭文弓手1挡2概率);
        S_铭文弓手1挡3概率.Value = (Config.铭文弓手1挡3概率 = Settings.Default.铭文弓手1挡3概率);
        S_铭文弓手1挡技能编号.Value = (Config.铭文弓手1挡技能编号 = Settings.Default.铭文弓手1挡技能编号);
        S_铭文弓手1挡技能铭文.Value = (Config.铭文弓手1挡技能铭文 = Settings.Default.铭文弓手1挡技能铭文);
        S_铭文弓手2挡1次数.Value = (Config.铭文弓手2挡1次数 = Settings.Default.铭文弓手2挡1次数);
        S_铭文弓手2挡2次数.Value = (Config.铭文弓手2挡2次数 = Settings.Default.铭文弓手2挡2次数);
        S_铭文弓手2挡3次数.Value = (Config.铭文弓手2挡3次数 = Settings.Default.铭文弓手2挡3次数);
        S_铭文弓手2挡1概率.Value = (Config.铭文弓手2挡1概率 = Settings.Default.铭文弓手2挡1概率);
        S_铭文弓手2挡2概率.Value = (Config.铭文弓手2挡2概率 = Settings.Default.铭文弓手2挡2概率);
        S_铭文弓手2挡3概率.Value = (Config.铭文弓手2挡3概率 = Settings.Default.铭文弓手2挡3概率);
        S_铭文弓手2挡技能编号.Value = (Config.铭文弓手2挡技能编号 = Settings.Default.铭文弓手2挡技能编号);
        S_铭文弓手2挡技能铭文.Value = (Config.铭文弓手2挡技能铭文 = Settings.Default.铭文弓手2挡技能铭文);
        S_铭文弓手3挡1次数.Value = (Config.铭文弓手3挡1次数 = Settings.Default.铭文弓手3挡1次数);
        S_铭文弓手3挡2次数.Value = (Config.铭文弓手3挡2次数 = Settings.Default.铭文弓手3挡2次数);
        S_铭文弓手3挡3次数.Value = (Config.铭文弓手3挡3次数 = Settings.Default.铭文弓手3挡3次数);
        S_铭文弓手3挡1概率.Value = (Config.铭文弓手3挡1概率 = Settings.Default.铭文弓手3挡1概率);
        S_铭文弓手3挡2概率.Value = (Config.铭文弓手3挡2概率 = Settings.Default.铭文弓手3挡2概率);
        S_铭文弓手3挡3概率.Value = (Config.铭文弓手3挡3概率 = Settings.Default.铭文弓手3挡3概率);
        S_铭文弓手3挡技能编号.Value = (Config.铭文弓手3挡技能编号 = Settings.Default.铭文弓手3挡技能编号);
        S_铭文弓手3挡技能铭文.Value = (Config.铭文弓手3挡技能铭文 = Settings.Default.铭文弓手3挡技能铭文);
        S_铭文龙枪1挡1次数.Value = (Config.铭文龙枪1挡1次数 = Settings.Default.铭文龙枪1挡1次数);
        S_铭文龙枪1挡2次数.Value = (Config.铭文龙枪1挡2次数 = Settings.Default.铭文龙枪1挡2次数);
        S_铭文龙枪1挡3次数.Value = (Config.铭文龙枪1挡3次数 = Settings.Default.铭文龙枪1挡3次数);
        S_铭文龙枪1挡1概率.Value = (Config.铭文龙枪1挡1概率 = Settings.Default.铭文龙枪1挡1概率);
        S_铭文龙枪1挡2概率.Value = (Config.铭文龙枪1挡2概率 = Settings.Default.铭文龙枪1挡2概率);
        S_铭文龙枪1挡3概率.Value = (Config.铭文龙枪1挡3概率 = Settings.Default.铭文龙枪1挡3概率);
        S_铭文龙枪1挡技能编号.Value = (Config.铭文龙枪1挡技能编号 = Settings.Default.铭文龙枪1挡技能编号);
        S_铭文龙枪1挡技能铭文.Value = (Config.铭文龙枪1挡技能铭文 = Settings.Default.铭文龙枪1挡技能铭文);
        S_铭文龙枪2挡1次数.Value = (Config.铭文龙枪2挡1次数 = Settings.Default.铭文龙枪2挡1次数);
        S_铭文龙枪2挡2次数.Value = (Config.铭文龙枪2挡2次数 = Settings.Default.铭文龙枪2挡2次数);
        S_铭文龙枪2挡3次数.Value = (Config.铭文龙枪2挡3次数 = Settings.Default.铭文龙枪2挡3次数);
        S_铭文龙枪2挡1概率.Value = (Config.铭文龙枪2挡1概率 = Settings.Default.铭文龙枪2挡1概率);
        S_铭文龙枪2挡2概率.Value = (Config.铭文龙枪2挡2概率 = Settings.Default.铭文龙枪2挡2概率);
        S_铭文龙枪2挡3概率.Value = (Config.铭文龙枪2挡3概率 = Settings.Default.铭文龙枪2挡3概率);
        S_铭文龙枪2挡技能编号.Value = (Config.铭文龙枪2挡技能编号 = Settings.Default.铭文龙枪2挡技能编号);
        S_铭文龙枪2挡技能铭文.Value = (Config.铭文龙枪2挡技能铭文 = Settings.Default.铭文龙枪2挡技能铭文);
        S_铭文龙枪3挡1次数.Value = (Config.铭文龙枪3挡1次数 = Settings.Default.铭文龙枪3挡1次数);
        S_铭文龙枪3挡2次数.Value = (Config.铭文龙枪3挡2次数 = Settings.Default.铭文龙枪3挡2次数);
        S_铭文龙枪3挡3次数.Value = (Config.铭文龙枪3挡3次数 = Settings.Default.铭文龙枪3挡3次数);
        S_铭文龙枪3挡1概率.Value = (Config.铭文龙枪3挡1概率 = Settings.Default.铭文龙枪3挡1概率);
        S_铭文龙枪3挡2概率.Value = (Config.铭文龙枪3挡2概率 = Settings.Default.铭文龙枪3挡2概率);
        S_铭文龙枪3挡3概率.Value = (Config.铭文龙枪3挡3概率 = Settings.Default.铭文龙枪3挡3概率);
        S_铭文龙枪3挡技能编号.Value = (Config.铭文龙枪3挡技能编号 = Settings.Default.铭文龙枪3挡技能编号);
        S_铭文龙枪3挡技能铭文.Value = (Config.铭文龙枪3挡技能铭文 = Settings.Default.铭文龙枪3挡技能铭文);
        S_铭文道士保底开关.Value = (Config.铭文道士保底开关 = Settings.Default.铭文道士保底开关);
        S_铭文战士保底开关.Value = (Config.铭文战士保底开关 = Settings.Default.铭文战士保底开关);
        S_铭文法师保底开关.Value = (Config.铭文法师保底开关 = Settings.Default.铭文法师保底开关);
        S_铭文刺客保底开关.Value = (Config.铭文刺客保底开关 = Settings.Default.铭文刺客保底开关);
        S_铭文弓手保底开关.Value = (Config.铭文弓手保底开关 = Settings.Default.铭文弓手保底开关);
        S_铭文龙枪保底开关.Value = (Config.铭文龙枪保底开关 = Settings.Default.铭文龙枪保底开关);
        S_DropRateModifier.Value = (Config.DropRateModifier = Settings.Default.DropRateModifier);
        S_魔虫窟副本次数.Value = (Config.魔虫窟副本次数 = Settings.Default.魔虫窟副本次数);
        S_魔虫窟副本等级.Value = (Config.魔虫窟副本等级 = Settings.Default.魔虫窟副本等级);
        S_魔虫窟副本物品.Value = (Config.魔虫窟副本物品 = Settings.Default.魔虫窟副本物品);
        S_魔虫窟副本数量.Value = (Config.魔虫窟副本数量 = Settings.Default.魔虫窟副本数量);
        S_魔虫窟副本时间小.Value = (Config.魔虫窟副本时间小 = Settings.Default.魔虫窟副本时间小);
        S_魔虫窟副本时间大.Value = (Config.魔虫窟副本时间大 = Settings.Default.魔虫窟副本时间大);
        S_书店商贩物品.Text = (Config.书店商贩物品 = Settings.Default.书店商贩物品);
        S_幸运洗练次数保底.Value = (Config.幸运洗练次数保底 = Settings.Default.幸运洗练次数保底);
        S_幸运洗练点数.Value = (Config.幸运洗练点数 = Settings.Default.幸运洗练点数);
        S_武器强化消耗货币值.Value = (Config.武器强化消耗货币值 = Settings.Default.武器强化消耗货币值);
        S_武器强化消耗货币开关.Value = (Config.武器强化消耗货币开关 = Settings.Default.武器强化消耗货币开关);
        S_武器强化取回时间.Value = (Config.武器强化取回时间 = Settings.Default.武器强化取回时间);
        S_幸运额外1值.Value = (Config.幸运额外1值 = Settings.Default.幸运额外1值);
        S_幸运额外2值.Value = (Config.幸运额外2值 = Settings.Default.幸运额外2值);
        S_幸运额外3值.Value = (Config.幸运额外3值 = Settings.Default.幸运额外3值);
        S_幸运额外4值.Value = (Config.幸运额外4值 = Settings.Default.幸运额外4值);
        S_幸运额外5值.Value = (Config.幸运额外5值 = Settings.Default.幸运额外5值);
        S_幸运额外1伤害.Value = (decimal)(Config.幸运额外1伤害 = Settings.Default.幸运额外1伤害);
        S_幸运额外2伤害.Value = (decimal)(Config.幸运额外2伤害 = Settings.Default.幸运额外2伤害);
        S_幸运额外3伤害.Value = (decimal)(Config.幸运额外3伤害 = Settings.Default.幸运额外3伤害);
        S_幸运额外4伤害.Value = (decimal)(Config.幸运额外4伤害 = Settings.Default.幸运额外4伤害);
        S_幸运额外5伤害.Value = (decimal)(Config.幸运额外5伤害 = Settings.Default.幸运额外5伤害);
        S_暗之门地图1.Value = (Config.暗之门地图1 = Settings.Default.暗之门地图1);
        S_暗之门地图2.Value = (Config.暗之门地图2 = Settings.Default.暗之门地图2);
        S_暗之门地图3.Value = (Config.暗之门地图3 = Settings.Default.暗之门地图3);
        S_暗之门地图4.Value = (Config.暗之门地图4 = Settings.Default.暗之门地图4);
        S_暗之门全服提示.Value = (Config.暗之门全服提示 = Settings.Default.暗之门全服提示);
        S_暗之门杀怪触发.Value = (Config.暗之门杀怪触发 = Settings.Default.暗之门杀怪触发);
        S_暗之门时间.Value = (Config.暗之门时间 = Settings.Default.暗之门时间);
        S_暗之门地图1BOSS.Text = (Config.暗之门地图1BOSS = Settings.Default.暗之门地图1BOSS);
        S_暗之门地图2BOSS.Text = (Config.暗之门地图2BOSS = Settings.Default.暗之门地图2BOSS);
        S_暗之门地图3BOSS.Text = (Config.暗之门地图3BOSS = Settings.Default.暗之门地图3BOSS);
        S_暗之门地图4BOSS.Text = (Config.暗之门地图4BOSS = Settings.Default.暗之门地图4BOSS);
        S_暗之门地图1X.Value = (Config.暗之门地图1X = Settings.Default.暗之门地图1X);
        S_暗之门地图1Y.Value = (Config.暗之门地图1Y = Settings.Default.暗之门地图1Y);
        S_暗之门地图2X.Value = (Config.暗之门地图2X = Settings.Default.暗之门地图2X);
        S_暗之门地图2Y.Value = (Config.暗之门地图2Y = Settings.Default.暗之门地图2Y);
        S_暗之门地图3X.Value = (Config.暗之门地图3X = Settings.Default.暗之门地图3X);
        S_暗之门地图3Y.Value = (Config.暗之门地图3Y = Settings.Default.暗之门地图3Y);
        S_暗之门地图4X.Value = (Config.暗之门地图4X = Settings.Default.暗之门地图4X);
        S_暗之门地图4Y.Value = (Config.暗之门地图4Y = Settings.Default.暗之门地图4Y);
        S_暗之门开关.Value = (Config.暗之门开关 = Settings.Default.暗之门开关);
        S_监狱货币类型.Value = (Config.监狱货币类型 = Settings.Default.监狱货币类型);
        S_监狱货币.Value = (Config.监狱货币 = Settings.Default.监狱货币);
        S_魔虫窟分钟限制.Value = (Config.魔虫窟分钟限制 = Settings.Default.魔虫窟分钟限制);
        S_自定义元宝兑换01.Value = (Config.自定义元宝兑换01 = Settings.Default.自定义元宝兑换01);
        S_自定义元宝兑换02.Value = (Config.自定义元宝兑换02 = Settings.Default.自定义元宝兑换02);
        S_自定义元宝兑换03.Value = (Config.自定义元宝兑换03 = Settings.Default.自定义元宝兑换03);
        S_自定义元宝兑换04.Value = (Config.自定义元宝兑换04 = Settings.Default.自定义元宝兑换04);
        S_自定义元宝兑换05.Value = (Config.自定义元宝兑换05 = Settings.Default.自定义元宝兑换05);
        S_直升等级1.Value = (Config.直升等级1 = Settings.Default.直升等级1);
        S_直升等级2.Value = (Config.直升等级2 = Settings.Default.直升等级2);
        S_直升等级3.Value = (Config.直升等级3 = Settings.Default.直升等级3);
        S_直升等级4.Value = (Config.直升等级4 = Settings.Default.直升等级4);
        S_直升等级5.Value = (Config.直升等级5 = Settings.Default.直升等级5);
        S_直升等级6.Value = (Config.直升等级6 = Settings.Default.直升等级6);
        S_直升等级7.Value = (Config.直升等级7 = Settings.Default.直升等级7);
        S_直升等级8.Value = (Config.直升等级8 = Settings.Default.直升等级8);
        S_直升等级9.Value = (Config.直升等级9 = Settings.Default.直升等级9);
        S_直升经验1.Value = (Config.直升经验1 = Settings.Default.直升经验1);
        S_直升经验2.Value = (Config.直升经验2 = Settings.Default.直升经验2);
        S_直升经验3.Value = (Config.直升经验3 = Settings.Default.直升经验3);
        S_直升经验4.Value = (Config.直升经验4 = Settings.Default.直升经验4);
        S_直升经验5.Value = (Config.直升经验5 = Settings.Default.直升经验5);
        S_直升经验6.Value = (Config.直升经验6 = Settings.Default.直升经验6);
        S_直升经验7.Value = (Config.直升经验7 = Settings.Default.直升经验7);
        S_直升经验8.Value = (Config.直升经验8 = Settings.Default.直升经验8);
        S_直升经验9.Value = (Config.直升经验9 = Settings.Default.直升经验9);
        S_直升物品1.Value = (Config.直升物品1 = Settings.Default.直升物品1);
        S_直升物品2.Value = (Config.直升物品2 = Settings.Default.直升物品2);
        S_直升物品3.Value = (Config.直升物品3 = Settings.Default.直升物品3);
        S_直升物品4.Value = (Config.直升物品4 = Settings.Default.直升物品4);
        S_直升物品5.Value = (Config.直升物品5 = Settings.Default.直升物品5);
        S_直升物品6.Value = (Config.直升物品6 = Settings.Default.直升物品6);
        S_直升物品7.Value = (Config.直升物品7 = Settings.Default.直升物品7);
        S_直升物品8.Value = (Config.直升物品8 = Settings.Default.直升物品8);
        S_直升物品9.Value = (Config.直升物品9 = Settings.Default.直升物品9);
        S_充值模块格式.Value = (Config.充值模块格式 = Settings.Default.充值模块格式);
        UpgradeXPLevel1.Value = (Config.UpgradeXPLevel1 = Settings.Default.UpgradeXPLevel1);
        UpgradeXPLevel2.Value = (Config.UpgradeXPLevel2 = Settings.Default.UpgradeXPLevel2);
        UpgradeXPLevel3.Value = (Config.UpgradeXPLevel3 = Settings.Default.UpgradeXPLevel3);
        UpgradeXPLevel4.Value = (Config.UpgradeXPLevel4 = Settings.Default.UpgradeXPLevel4);
        UpgradeXPLevel5.Value = (Config.UpgradeXPLevel5 = Settings.Default.UpgradeXPLevel5);
        UpgradeXPLevel6.Value = (Config.UpgradeXPLevel6 = Settings.Default.UpgradeXPLevel6);
        UpgradeXPLevel7.Value = (Config.UpgradeXPLevel7 = Settings.Default.UpgradeXPLevel7);
        UpgradeXPLevel8.Value = (Config.UpgradeXPLevel8 = Settings.Default.UpgradeXPLevel8);
        UpgradeXPLevel9.Value = (Config.UpgradeXPLevel9 = Settings.Default.UpgradeXPLevel9);
        UpgradeXPLevel10.Value = (Config.UpgradeXPLevel10 = Settings.Default.UpgradeXPLevel10);
        UpgradeXPLevel11.Value = (Config.UpgradeXPLevel11 = Settings.Default.UpgradeXPLevel11);
        UpgradeXPLevel12.Value = (Config.UpgradeXPLevel12 = Settings.Default.UpgradeXPLevel12);
        UpgradeXPLevel13.Value = (Config.UpgradeXPLevel13 = Settings.Default.UpgradeXPLevel13);
        UpgradeXPLevel14.Value = (Config.UpgradeXPLevel14 = Settings.Default.UpgradeXPLevel14);
        UpgradeXPLevel15.Value = (Config.UpgradeXPLevel15 = Settings.Default.UpgradeXPLevel15);
        UpgradeXPLevel16.Value = (Config.UpgradeXPLevel16 = Settings.Default.UpgradeXPLevel16);
        UpgradeXPLevel17.Value = (Config.UpgradeXPLevel17 = Settings.Default.UpgradeXPLevel17);
        UpgradeXPLevel18.Value = (Config.UpgradeXPLevel18 = Settings.Default.UpgradeXPLevel18);
        UpgradeXPLevel19.Value = (Config.UpgradeXPLevel19 = Settings.Default.UpgradeXPLevel19);
        UpgradeXPLevel20.Value = (Config.UpgradeXPLevel20 = Settings.Default.UpgradeXPLevel20);
        UpgradeXPLevel21.Value = (Config.UpgradeXPLevel21 = Settings.Default.UpgradeXPLevel21);
        UpgradeXPLevel22.Value = (Config.UpgradeXPLevel22 = Settings.Default.UpgradeXPLevel22);
        UpgradeXPLevel23.Value = (Config.UpgradeXPLevel23 = Settings.Default.UpgradeXPLevel23);
        UpgradeXPLevel24.Value = (Config.UpgradeXPLevel24 = Settings.Default.UpgradeXPLevel24);
        UpgradeXPLevel25.Value = (Config.UpgradeXPLevel25 = Settings.Default.UpgradeXPLevel25);
        UpgradeXPLevel26.Value = (Config.UpgradeXPLevel26 = Settings.Default.UpgradeXPLevel26);
        UpgradeXPLevel27.Value = (Config.UpgradeXPLevel27 = Settings.Default.UpgradeXPLevel27);
        UpgradeXPLevel28.Value = (Config.UpgradeXPLevel28 = Settings.Default.UpgradeXPLevel28);
        UpgradeXPLevel29.Value = (Config.UpgradeXPLevel29 = Settings.Default.UpgradeXPLevel29);
        UpgradeXPLevel30.Value = (Config.UpgradeXPLevel30 = Settings.Default.UpgradeXPLevel30);
        UpgradeXPLevel31.Value = (Config.UpgradeXPLevel31 = Settings.Default.UpgradeXPLevel31);
        UpgradeXPLevel32.Value = (Config.UpgradeXPLevel32 = Settings.Default.UpgradeXPLevel32);
        UpgradeXPLevel33.Value = (Config.UpgradeXPLevel33 = Settings.Default.UpgradeXPLevel33);
        UpgradeXPLevel34.Value = (Config.UpgradeXPLevel34 = Settings.Default.UpgradeXPLevel34);
        UpgradeXPLevel35.Value = (Config.UpgradeXPLevel35 = Settings.Default.UpgradeXPLevel35);
        UpgradeXPLevel36.Value = (Config.UpgradeXPLevel36 = Settings.Default.UpgradeXPLevel36);
        UpgradeXPLevel37.Value = (Config.UpgradeXPLevel37 = Settings.Default.UpgradeXPLevel37);
        UpgradeXPLevel38.Value = (Config.UpgradeXPLevel38 = Settings.Default.UpgradeXPLevel38);
        UpgradeXPLevel39.Value = (Config.UpgradeXPLevel39 = Settings.Default.UpgradeXPLevel39);
        DefaultSkillLevel.Value = (Config.DefaultSkillLevel = Settings.Default.DefaultSkillLevel);
        S_沃玛分解物品一.Text = (Config.沃玛分解物品一 = Settings.Default.沃玛分解物品一);
        S_沃玛分解物品二.Text = (Config.沃玛分解物品二 = Settings.Default.沃玛分解物品二);
        S_沃玛分解物品三.Text = (Config.沃玛分解物品三 = Settings.Default.沃玛分解物品三);
        S_沃玛分解物品四.Text = (Config.沃玛分解物品四 = Settings.Default.沃玛分解物品四);
        S_沃玛分解几率一.Value = (Config.沃玛分解几率一 = Settings.Default.沃玛分解几率一);
        S_沃玛分解几率二.Value = (Config.沃玛分解几率二 = Settings.Default.沃玛分解几率二);
        S_沃玛分解几率三.Value = (Config.沃玛分解几率三 = Settings.Default.沃玛分解几率三);
        S_沃玛分解几率四.Value = (Config.沃玛分解几率四 = Settings.Default.沃玛分解几率四);
        S_沃玛分解数量一.Value = (Config.沃玛分解数量一 = Settings.Default.沃玛分解数量一);
        S_沃玛分解数量二.Value = (Config.沃玛分解数量二 = Settings.Default.沃玛分解数量二);
        S_沃玛分解数量三.Value = (Config.沃玛分解数量三 = Settings.Default.沃玛分解数量三);
        S_沃玛分解数量四.Value = (Config.沃玛分解数量四 = Settings.Default.沃玛分解数量四);
        S_沃玛分解开关.Value = (Config.沃玛分解开关 = Settings.Default.沃玛分解开关);
        S_其他分解物品一.Text = (Config.其他分解物品一 = Settings.Default.其他分解物品一);
        S_其他分解物品二.Text = (Config.其他分解物品二 = Settings.Default.其他分解物品二);
        S_其他分解物品三.Text = (Config.其他分解物品三 = Settings.Default.其他分解物品三);
        S_其他分解物品四.Text = (Config.其他分解物品四 = Settings.Default.其他分解物品四);
        S_其他分解几率一.Value = (Config.其他分解几率一 = Settings.Default.其他分解几率一);
        S_其他分解几率二.Value = (Config.其他分解几率二 = Settings.Default.其他分解几率二);
        S_其他分解几率三.Value = (Config.其他分解几率三 = Settings.Default.其他分解几率三);
        S_其他分解几率四.Value = (Config.其他分解几率四 = Settings.Default.其他分解几率四);
        S_其他分解数量一.Value = (Config.其他分解数量一 = Settings.Default.其他分解数量一);
        S_其他分解数量二.Value = (Config.其他分解数量二 = Settings.Default.其他分解数量二);
        S_其他分解数量三.Value = (Config.其他分解数量三 = Settings.Default.其他分解数量三);
        S_其他分解数量四.Value = (Config.其他分解数量四 = Settings.Default.其他分解数量四);
        S_其他分解开关.Value = (Config.其他分解开关 = Settings.Default.其他分解开关);
        拾取地图控制1.Value = (Config.拾取地图控制1 = Settings.Default.拾取地图控制1);
        拾取地图控制2.Value = (Config.拾取地图控制2 = Settings.Default.拾取地图控制2);
        拾取地图控制3.Value = (Config.拾取地图控制3 = Settings.Default.拾取地图控制3);
        拾取地图控制4.Value = (Config.拾取地图控制4 = Settings.Default.拾取地图控制4);
        拾取地图控制5.Value = (Config.拾取地图控制5 = Settings.Default.拾取地图控制5);
        拾取地图控制6.Value = (Config.拾取地图控制6 = Settings.Default.拾取地图控制6);
        拾取地图控制7.Value = (Config.拾取地图控制7 = Settings.Default.拾取地图控制7);
        拾取地图控制8.Value = (Config.拾取地图控制8 = Settings.Default.拾取地图控制8);
        沙城捐献货币类型.Value = (Config.沙城捐献货币类型 = Settings.Default.沙城捐献货币类型);
        沙城捐献支付数量.Value = (Config.沙城捐献支付数量 = Settings.Default.沙城捐献支付数量);
        沙城捐献获得物品1.Value = (Config.沙城捐献获得物品1 = Settings.Default.沙城捐献获得物品1);
        沙城捐献获得物品2.Value = (Config.沙城捐献获得物品2 = Settings.Default.沙城捐献获得物品2);
        沙城捐献获得物品3.Value = (Config.沙城捐献获得物品3 = Settings.Default.沙城捐献获得物品3);
        沙城捐献物品数量1.Value = (Config.沙城捐献物品数量1 = Settings.Default.沙城捐献物品数量1);
        沙城捐献物品数量2.Value = (Config.沙城捐献物品数量2 = Settings.Default.沙城捐献物品数量2);
        沙城捐献物品数量3.Value = (Config.沙城捐献物品数量3 = Settings.Default.沙城捐献物品数量3);
        沙城捐献赞助人数.Value = (Config.沙城捐献赞助人数 = Settings.Default.沙城捐献赞助人数);
        沙城捐献赞助金额.Value = (Config.沙城捐献赞助金额 = Settings.Default.沙城捐献赞助金额);
        雕爷激活灵符需求.Value = (Config.雕爷激活灵符需求 = Settings.Default.雕爷激活灵符需求);
        雕爷1号位灵符.Value = (Config.雕爷1号位灵符 = Settings.Default.雕爷1号位灵符);
        雕爷1号位铭文石.Value = (Config.雕爷1号位铭文石 = Settings.Default.雕爷1号位铭文石);
        雕爷2号位灵符.Value = (Config.雕爷2号位灵符 = Settings.Default.雕爷2号位灵符);
        雕爷2号位铭文石.Value = (Config.雕爷2号位铭文石 = Settings.Default.雕爷2号位铭文石);
        雕爷3号位灵符.Value = (Config.雕爷3号位灵符 = Settings.Default.雕爷3号位灵符);
        雕爷3号位铭文石.Value = (Config.雕爷3号位铭文石 = Settings.Default.雕爷3号位铭文石);
        S_称号范围拾取判断1.Value = (Config.称号范围拾取判断 = Settings.Default.称号范围拾取判断);
        九层妖塔统计开关.Value = (Config.九层妖塔统计开关 = Settings.Default.九层妖塔统计开关);
        沙巴克每周攻沙时间.Value = (Config.沙巴克每周攻沙时间 = Settings.Default.沙巴克每周攻沙时间);
        沙巴克皇宫传送等级.Value = (Config.沙巴克皇宫传送等级 = Settings.Default.沙巴克皇宫传送等级);
        沙巴克皇宫传送物品.Value = (Config.沙巴克皇宫传送物品 = Settings.Default.沙巴克皇宫传送物品);
        沙巴克皇宫传送数量.Value = (Config.沙巴克皇宫传送数量 = Settings.Default.沙巴克皇宫传送数量);
        系统窗口发送.Value = (Config.系统窗口发送 = Settings.Default.系统窗口发送);
        龙卫效果提示.Value = (Config.龙卫效果提示 = Settings.Default.龙卫效果提示);
        充值平台切换.Value = (Config.充值平台切换 = Settings.Default.充值平台切换);
        坐骑骑乘切换.Value = (Config.坐骑骑乘切换 = Settings.Default.坐骑骑乘切换);
        坐骑属性切换.Value = (Config.坐骑属性切换 = Settings.Default.坐骑属性切换);
        珍宝模块切换.Value = (Config.珍宝模块切换 = Settings.Default.珍宝模块切换);
        称号属性切换.Value = (Config.称号属性切换 = Settings.Default.称号属性切换);
        全服红包等级.Value = (Config.全服红包等级 = Settings.Default.全服红包等级);
        全服红包时间.Value = (Config.全服红包时间 = Settings.Default.全服红包时间);
        全服红包货币类型.Value = (Config.全服红包货币类型 = Settings.Default.全服红包货币类型);
        全服红包货币数量.Value = (Config.全服红包货币数量 = Settings.Default.全服红包货币数量);
        龙卫蓝色词条概率.Value = (Config.龙卫蓝色词条概率 = Settings.Default.龙卫蓝色词条概率);
        龙卫紫色词条概率.Value = (Config.龙卫紫色词条概率 = Settings.Default.龙卫紫色词条概率);
        龙卫橙色词条概率.Value = (Config.龙卫橙色词条概率 = Settings.Default.龙卫橙色词条概率);
        自定义初始货币类型.Value = (Config.自定义初始货币类型 = Settings.Default.自定义初始货币类型);
        Config.自动回收设置 = Settings.Default.自动回收设置;
        自动回收设置.Checked = Config.自动回收设置 == 1;
        Config.购买狂暴之力 = Settings.Default.购买狂暴之力;
        购买狂暴之力.Checked = Config.购买狂暴之力 == 1;
        Config.会员满血设置 = Settings.Default.会员满血设置;
        会员满血设置.Checked = Config.会员满血设置 == 1;
        Config.全屏拾取开关 = Settings.Default.全屏拾取开关;
        全屏拾取开关.Checked = Config.全屏拾取开关 == 1;
        Config.打开随时仓库 = Settings.Default.打开随时仓库;
        打开随时仓库.Checked = Config.打开随时仓库 == 1;
        Config.红包开关 = Settings.Default.红包开关;
        红包开关.Checked = Config.红包开关 == 1;
        龙卫焰焚烈火剑法.Value = (Config.龙卫焰焚烈火剑法 = Settings.Default.龙卫焰焚烈火剑法);
        会员物品对接.Value = (Config.会员物品对接 = Settings.Default.会员物品对接);
        变性等级.Value = (Config.变性等级 = Settings.Default.变性等级);
        变性货币类型.Value = (Config.变性货币类型 = Settings.Default.变性货币类型);
        变性货币值.Value = (Config.变性货币值 = Settings.Default.变性货币值);
        变性物品ID.Value = (Config.变性物品ID = Settings.Default.变性物品ID);
        变性物品数量.Value = (Config.变性物品数量 = Settings.Default.变性物品数量);
        称号叠加模块9.Value = (Config.称号叠加模块9 = Settings.Default.称号叠加模块9);
        称号叠加模块10.Value = (Config.称号叠加模块10 = Settings.Default.称号叠加模块10);
        称号叠加模块11.Value = (Config.称号叠加模块11 = Settings.Default.称号叠加模块11);
        称号叠加模块12.Value = (Config.称号叠加模块12 = Settings.Default.称号叠加模块12);
        称号叠加模块13.Value = (Config.称号叠加模块13 = Settings.Default.称号叠加模块13);
        称号叠加模块14.Value = (Config.称号叠加模块14 = Settings.Default.称号叠加模块14);
        称号叠加模块15.Value = (Config.称号叠加模块15 = Settings.Default.称号叠加模块15);
        称号叠加模块16.Value = (Config.称号叠加模块16 = Settings.Default.称号叠加模块16);
        Config.幸运保底开关 = Settings.Default.幸运保底开关;
        幸运保底开关.Checked = Config.幸运保底开关 == 1;
        Config.安全区收刀开关 = Settings.Default.安全区收刀开关;
        安全区收刀开关.Checked = Config.安全区收刀开关 == 1;
        屠魔殿等级限制.Value = (Config.屠魔殿等级限制 = Settings.Default.屠魔殿等级限制);
        职业等级.Value = (Config.职业等级 = Settings.Default.职业等级);
        职业货币类型.Value = (Config.职业货币类型 = Settings.Default.职业货币类型);
        职业货币值.Value = (Config.职业货币值 = Settings.Default.职业货币值);
        职业物品ID.Value = (Config.职业物品ID = Settings.Default.职业物品ID);
        职业物品数量.Value = (Config.职业物品数量 = Settings.Default.职业物品数量);
        武斗场杀人经验.Value = (Config.武斗场杀人经验 = Settings.Default.武斗场杀人经验);
        Config.武斗场杀人开关 = Settings.Default.武斗场杀人开关;
        武斗场杀人开关.Checked = Config.武斗场杀人开关 == 1;
        S_狂暴名称.Text = (Config.狂暴名称 = Settings.Default.狂暴名称);
        S_自定义物品内容一.Text = (Config.自定义物品内容一 = Settings.Default.自定义物品内容一);
        S_自定义物品内容二.Text = (Config.自定义物品内容二 = Settings.Default.自定义物品内容二);
        S_自定义物品内容三.Text = (Config.自定义物品内容三 = Settings.Default.自定义物品内容三);
        S_自定义物品内容四.Text = (Config.自定义物品内容四 = Settings.Default.自定义物品内容四);
        S_自定义物品内容五.Text = (Config.自定义物品内容五 = Settings.Default.自定义物品内容五);
        S_挂机权限选项.Text = (Config.挂机权限选项 = Settings.Default.挂机权限选项);
        合成模块控件.Text = (Config.合成模块控件 = Settings.Default.合成模块控件);
        变性内容控件.Text = (Config.变性内容控件 = Settings.Default.变性内容控件);
        转职内容控件.Text = (Config.转职内容控件 = Settings.Default.转职内容控件);
        S_战将特权礼包.Text = (Config.战将特权礼包 = Settings.Default.战将特权礼包);
        S_豪杰特权礼包.Text = (Config.豪杰特权礼包 = Settings.Default.豪杰特权礼包);
        S_世界BOSS名字.Text = (Config.世界BOSS名字 = Settings.Default.世界BOSS名字);
        S_BOSS名字一.Text = (Config.BOSS名字一 = Settings.Default.BOSS名字一);
        S_BOSS一地图名字.Text = (Config.BOSS一地图名字 = Settings.Default.BOSS一地图名字);
        S_世界BOSS时间.Value = (Config.世界BOSS时间 = Settings.Default.世界BOSS时间);
        S_世界BOSS分钟.Value = (Config.世界BOSS分钟 = Settings.Default.世界BOSS分钟);
        S_秘宝广场元宝.Value = (Config.秘宝广场元宝 = Settings.Default.秘宝广场元宝);
        S_每周特惠礼包一元宝.Value = (Config.每周特惠礼包一元宝 = Settings.Default.每周特惠礼包一元宝);
        S_每周特惠礼包二元宝.Value = (Config.每周特惠礼包二元宝 = Settings.Default.每周特惠礼包二元宝);
        S_特权玛法名俊元宝.Value = (Config.特权玛法名俊元宝 = Settings.Default.特权玛法名俊元宝);
        S_特权玛法豪杰元宝.Value = (Config.特权玛法豪杰元宝 = Settings.Default.特权玛法豪杰元宝);
        S_特权玛法战将元宝.Value = (Config.特权玛法战将元宝 = Settings.Default.特权玛法战将元宝);
        S_御兽切换开关.Value = (Config.御兽切换开关 = Settings.Default.御兽切换开关);
        Config.GuardKillWillDrop = Settings.Default.GuardKillWillDrop == 1;

        Task.Run(delegate
        {
            Thread.Sleep(100);
            BeginInvoke((MethodInvoker)delegate
            {
                SettingsPage.Enabled = false;
                下方控件页.Enabled = false;
            });
            LoadSystemData();
            LoadUserData();
            BeginInvoke((MethodInvoker)delegate
            {
                UIUpdateTimer.Tick += ProcessUpdateUI;
                角色浏览表.SelectionChanged += ProcessUpdateUI;
                怪物浏览表.SelectionChanged += ProcessUpdateUI;
                SettingsPage.Enabled = true;
                下方控件页.Enabled = true;
            });
        });
    }

    private void MainForm_Closing(object sender, FormClosingEventArgs e)
    {
        if (MessageBox.Show("Are you sure to shut down the server?", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        {
            while (true)
            {
                var thread = SEngine.MainThread;
                if (thread == null || !thread.IsAlive)
                    break;
                SEngine.StopService();
                Thread.Sleep(1);
            }
            if (Session.Modified && MessageBox.Show("User data has been modified but has not been saved, do you want to save the data?", "Save the data", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                Session.Save();
                Session.SaveUsers();
            }
        }
        else
        {
            e.Cancel = true;
        }
    }

    private void 保存数据提醒_Tick(object sender, EventArgs e)
    {
        if (savaDatabaseToolStripMenuItem.Enabled && Session.Modified)
        {
            if (savaDatabaseToolStripMenuItem.BackColor == Color.LightSteelBlue)
            {
                savaDatabaseToolStripMenuItem.BackColor = Color.PaleVioletRed;
            }
            else
            {
                savaDatabaseToolStripMenuItem.BackColor = Color.LightSteelBlue;
            }
        }
    }

    private void 重载系统数据_Click(object sender, EventArgs e)
    {
        SettingsPage.Enabled = false;
        下方控件页.Enabled = false;
        Task.Run(delegate
        {
            LoadSystemData();
            BeginInvoke((MethodInvoker)delegate
            {
                SettingsPage.Enabled = true;
                下方控件页.Enabled = true;
            });
        });
    }

    private void 重载客户数据_Click(object sender, EventArgs e)
    {
        SettingsPage.Enabled = false;
        下方控件页.Enabled = false;
        Task.Run(delegate
        {
            LoadUserData();
            BeginInvoke((MethodInvoker)delegate
            {
                SettingsPage.Enabled = true;
                下方控件页.Enabled = true;
            });
        });
    }

    private void ButtonBrowseDataCatalog_Click(object sender, EventArgs e)
    {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
        {
            Description = "Please select a folder"
        };
        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
        {
            if (sender == ButtonBrowseDataCatalog)
            {
                Settings @default = Settings.Default;
                string text = (S_GameDataPath.Text = folderBrowserDialog.SelectedPath);
                string text2 = text;
                string text3 = text2;
                text = (@default.GameDataPath = text3);
                text2 = text;
                string text5 = (Config.GameDataPath = text2);
                Settings.Default.Save();
            }
            else if (sender == ButtonBrowseBackupDirectory)
            {
                Settings default2 = Settings.Default;
                string text = (S_DataBackupPath.Text = folderBrowserDialog.SelectedPath);
                string text6 = text;
                string text7 = text6;
                text = (default2.DataBackupPath = text7);
                text6 = text;
                string text9 = (Config.DataBackupPath = text6);
                Settings.Default.Save();
            }
            else if (sender == S_浏览平台目录)
            {
                Settings default3 = Settings.Default;
                string text = (S_平台接入目录.Text = folderBrowserDialog.SelectedPath);
                string text10 = text;
                string text11 = text10;
                text = (default3.平台接入目录 = text11);
                text10 = text;
                string text13 = (Config.平台接入目录 = text10);
                Settings.Default.Save();
            }
        }
    }

    private void UpdateSettingsValue_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown control)
        {
            switch (control.Name)
            {
                case nameof(S_收益减少比率):
                    {
                        Config.收益减少比率 = Settings.Default.收益减少比率 = control.Value;
                        break;
                    }
                case nameof(S_DisconnectTime):
                    {
                        Config.DisconnectTime = Settings.Default.DisconnectTime = (ushort)control.Value;
                        break;
                    }
                case nameof(S_MaxUserLevel):
                    {
                        Config.MaxUserLevel = Settings.Default.MaxUserLevel = (byte)control.Value;
                        break;
                    }
                case nameof(S_怪物诱惑时长):
                    {
                        Config.怪物诱惑时长 = Settings.Default.怪物诱惑时长 = (ushort)control.Value;
                        break;
                    }
                case nameof(S_怪物经验倍率):
                    {
                        Config.MonsterExperienceMultiplier = Settings.Default.MonsterExperienceMultiplier = control.Value;
                        break;
                    }
                case nameof(S_TicketReceivePort):
                    {
                        Config.TicketReceivePort = Settings.Default.TicketReceivePort = (ushort)control.Value;
                        break;
                    }
                case nameof(S_AbnormalBlockTime):
                    {
                        Config.AbnormalBlockTime = Settings.Default.AbnormalBlockTime = (ushort)control.Value;
                        break;
                    }
                case nameof(S_减收益等级差):
                    {
                        Config.减收益等级差 = Settings.Default.减收益等级差 = (byte)control.Value;
                        break;
                    }
                case nameof(S_怪物额外爆率):
                    {
                        Config.怪物额外爆率 = Settings.Default.怪物额外爆率 = control.Value;
                        break;
                    }
                case nameof(S_物品归属时间):
                    {
                        Config.物品归属时间 = Settings.Default.物品归属时间 = (byte)control.Value;
                        break;
                    }
                case nameof(S_NoobSupportLevel):
                    {
                        Config.NoobSupportLevel = Settings.Default.NoobSupportLevel = (byte)control.Value;
                        break;
                    }
                case nameof(S_SpecialRepairDiscount):
                    {
                        Config.SpecialRepairDiscount = Settings.Default.SpecialRepairDiscount = control.Value;
                        break;
                    }
                case nameof(S_ItemDisappearTime):
                    {
                        Config.ItemDisappearTime = Settings.Default.ItemDisappearTime = (byte)control.Value;
                        break;
                    }
                case nameof(S_PacketLimit):
                    {
                        Config.PacketLimit = Settings.Default.PacketLimit = (ushort)control.Value;
                        break;
                    }
                case nameof(S_UserConnectionPort):
                    {
                        Config.UserConnectionPort = Settings.Default.UserConnectionPort = (ushort)control.Value;
                        break;
                    }
                case nameof(S_自动保存时间):
                    {
                        Config.AutoSaveInterval = Settings.Default.AutoSaveInterval = (ushort)control.Value;
                        break;
                    }
                case nameof(S_自动保存日志):
                    {
                        Config.自动保存日志 = Settings.Default.自动保存日志 = (ushort)control.Value;
                        break;
                    }
                case nameof(S_沃玛分解元宝):
                    {
                        Config.沃玛分解元宝 = Settings.Default.沃玛分解元宝 = (int)control.Value;
                        break;
                    }
                case nameof(S_祖玛分解元宝):
                    {
                        Config.祖玛分解元宝 = Settings.Default.祖玛分解元宝 = (int)control.Value;
                        break;
                    }
                case nameof(S_赤月分解元宝):
                    {
                        Config.赤月分解元宝 = Settings.Default.赤月分解元宝 = (int)control.Value;
                        break;
                    }
                case nameof(S_魔龙分解元宝):
                    {
                        Config.魔龙分解元宝 = Settings.Default.魔龙分解元宝 = (int)control.Value;
                        break;
                    }
                case nameof(S_星王分解元宝):
                    {
                        Config.星王分解元宝 = Settings.Default.星王分解元宝 = (int)control.Value;
                        break;
                    }
                case nameof(S_苍月分解元宝):
                    {
                        int num2 = (Settings.Default.苍月分解元宝 = (int)control.Value);
                        int 苍月分解元宝 = num2;
                        int num1664 = (Config.苍月分解元宝 = 苍月分解元宝);
                        break;
                    }
                case nameof(S_城主分解元宝):
                    {
                        int num2 = (Settings.Default.城主分解元宝 = (int)control.Value);
                        int 城主分解元宝2 = num2;
                        int num1662 = (Config.城主分解元宝 = 城主分解元宝2);
                        break;
                    }
                case nameof(S_神秘分解元宝):
                    {
                        int num2 = (Settings.Default.神秘分解元宝 = (int)control.Value);
                        int 城主分解元宝 = num2;
                        int num1660 = (Config.城主分解元宝 = 城主分解元宝);
                        break;
                    }
                case nameof(S_屠魔组队人数):
                    {
                        int num2 = (Settings.Default.屠魔组队人数 = (int)control.Value);
                        int 屠魔组队人数 = num2;
                        int num1658 = (Config.屠魔组队人数 = 屠魔组队人数);
                        break;
                    }
                case nameof(S_屠魔令回收经验):
                    {
                        int num2 = (Settings.Default.屠魔令回收经验 = (int)control.Value);
                        int 屠魔令回收经验 = num2;
                        int num1656 = (Config.屠魔令回收经验 = 屠魔令回收经验);
                        break;
                    }
                case nameof(S_屠魔爆率开关):
                    {
                        int num2 = (Settings.Default.屠魔爆率开关 = (int)control.Value);
                        int 屠魔爆率开关 = num2;
                        int num1654 = (Config.屠魔爆率开关 = 屠魔爆率开关);
                        break;
                    }
                case nameof(S_武斗场时间一):
                    {
                        byte b2 = (Settings.Default.武斗场时间一 = (byte)control.Value);
                        byte 武斗场时间一 = b2;
                        byte b91 = (Config.武斗场时间一 = 武斗场时间一);
                        break;
                    }
                case nameof(S_武斗场时间二):
                    {
                        byte b2 = (Settings.Default.武斗场时间二 = (byte)control.Value);
                        byte 武斗场时间二 = b2;
                        byte b89 = (Config.武斗场时间二 = 武斗场时间二);
                        break;
                    }
                case nameof(S_武斗场经验小):
                    {
                        int num2 = (Settings.Default.武斗场经验小 = (int)control.Value);
                        int 武斗场经验小 = num2;
                        int num1652 = (Config.武斗场经验小 = 武斗场经验小);
                        break;
                    }
                case nameof(S_武斗场经验大):
                    {
                        int num2 = (Settings.Default.武斗场经验大 = (int)control.Value);
                        int 武斗场经验大 = num2;
                        int num1650 = (Config.武斗场经验大 = 武斗场经验大);
                        break;
                    }
                case nameof(S_沙巴克开启):
                    {
                        byte b2 = (Settings.Default.沙巴克开启 = (byte)control.Value);
                        byte 沙巴克开启 = b2;
                        byte b87 = (Config.沙巴克开启 = 沙巴克开启);
                        break;
                    }
                case nameof(S_沙巴克结束):
                    {
                        byte b2 = (Settings.Default.沙巴克结束 = (byte)control.Value);
                        byte 沙巴克结束 = b2;
                        byte b85 = (Config.沙巴克结束 = 沙巴克结束);
                        break;
                    }
                case nameof(S_祝福油幸运1机率):
                    {
                        int num2 = (Settings.Default.祝福油幸运1机率 = (int)control.Value);
                        int 祝福油幸运1机率 = num2;
                        int num1648 = (Config.祝福油幸运1机率 = 祝福油幸运1机率);
                        break;
                    }
                case nameof(S_祝福油幸运2机率):
                    {
                        int num2 = (Settings.Default.祝福油幸运2机率 = (int)control.Value);
                        int 祝福油幸运2机率 = num2;
                        int num1646 = (Config.祝福油幸运2机率 = 祝福油幸运2机率);
                        break;
                    }
                case nameof(S_祝福油幸运3机率):
                    {
                        int num2 = (Settings.Default.祝福油幸运3机率 = (int)control.Value);
                        int 祝福油幸运3机率 = num2;
                        int num1644 = (Config.祝福油幸运3机率 = 祝福油幸运3机率);
                        break;
                    }
                case nameof(S_祝福油幸运4机率):
                    {
                        int num2 = (Settings.Default.祝福油幸运4机率 = (int)control.Value);
                        int 祝福油幸运4机率 = num2;
                        int num1642 = (Config.祝福油幸运4机率 = 祝福油幸运4机率);
                        break;
                    }
                case nameof(S_祝福油幸运5机率):
                    {
                        int num2 = (Settings.Default.祝福油幸运5机率 = (int)control.Value);
                        int 祝福油幸运5机率 = num2;
                        int num1640 = (Config.祝福油幸运5机率 = 祝福油幸运5机率);
                        break;
                    }
                case nameof(S_祝福油幸运6机率):
                    {
                        int num2 = (Settings.Default.祝福油幸运6机率 = (int)control.Value);
                        int 祝福油幸运6机率 = num2;
                        int num1638 = (Config.祝福油幸运6机率 = 祝福油幸运6机率);
                        break;
                    }
                case nameof(S_祝福油幸运7机率):
                    {
                        int num2 = (Settings.Default.祝福油幸运7机率 = (int)control.Value);
                        int 祝福油幸运7机率 = num2;
                        int num1636 = (Config.祝福油幸运7机率 = 祝福油幸运7机率);
                        break;
                    }
                case nameof(S_PKYellowNamePoint):
                    {
                        int num2 = (Settings.Default.PKYellowNamePoint = (int)control.Value);
                        int pK黄名设定 = num2;
                        int num1634 = (Config.PKYellowNamePoint = pK黄名设定);
                        break;
                    }
                case nameof(S_PKRedNamePoint):
                    {
                        int num2 = (Settings.Default.PKRedNamePoint = (int)control.Value);
                        int pK红名设定 = num2;
                        int num1632 = (Config.PKRedNamePoint = pK红名设定);
                        break;
                    }
                case nameof(S_PKCrimsonNamePoint):
                    {
                        int num2 = (Settings.Default.PKCrimsonNamePoint = (int)control.Value);
                        int pK深红设定 = num2;
                        int num1630 = (Config.PKCrimsonNamePoint = pK深红设定);
                        break;
                    }
                case nameof(S_锻造成功倍数):
                    {
                        int num2 = (Settings.Default.锻造成功倍数 = (int)control.Value);
                        int 锻造成功倍数 = num2;
                        int num1628 = (Config.锻造成功倍数 = 锻造成功倍数);
                        break;
                    }
                case nameof(S_死亡掉落背包几率):
                    {
                        float num340 = (Settings.Default.死亡掉落背包几率 = (float)control.Value);
                        float 死亡掉落背包几率 = num340;
                        float num1626 = (Config.死亡掉落背包几率 = 死亡掉落背包几率);
                        break;
                    }
                case nameof(S_死亡掉落身上几率):
                    {
                        float num340 = (Settings.Default.死亡掉落身上几率 = (float)control.Value);
                        float 死亡掉落身上几率 = num340;
                        float num1624 = (Config.死亡掉落身上几率 = 死亡掉落身上几率);
                        break;
                    }
                case nameof(S_PK死亡幸运开关):
                    {
                        int num2 = (Settings.Default.PK死亡幸运开关 = (int)control.Value);
                        int pK死亡幸运开关 = num2;
                        int num1622 = (Config.PK死亡幸运开关 = pK死亡幸运开关);
                        break;
                    }
                case nameof(S_屠魔副本次数):
                    {
                        int num2 = (Settings.Default.屠魔副本次数 = (int)control.Value);
                        int 屠魔副本次数 = num2;
                        int num1620 = (Config.屠魔副本次数 = 屠魔副本次数);
                        break;
                    }
                case nameof(S_升级经验模块一):
                    {
                        int num2 = (Settings.Default.升级经验模块一 = (int)control.Value);
                        int 升级经验模块一 = num2;
                        int num1618 = (Config.升级经验模块一 = 升级经验模块一);
                        break;
                    }
                case nameof(S_升级经验模块二):
                    {
                        int num2 = (Settings.Default.升级经验模块二 = (int)control.Value);
                        int 升级经验模块二 = num2;
                        int num1616 = (Config.升级经验模块二 = 升级经验模块二);
                        break;
                    }
                case nameof(S_升级经验模块三):
                    {
                        int num2 = (Settings.Default.升级经验模块三 = (int)control.Value);
                        int 升级经验模块三 = num2;
                        int num1614 = (Config.升级经验模块三 = 升级经验模块三);
                        break;
                    }
                case nameof(S_升级经验模块四):
                    {
                        int num2 = (Settings.Default.升级经验模块四 = (int)control.Value);
                        int 升级经验模块四 = num2;
                        int num1612 = (Config.升级经验模块四 = 升级经验模块四);
                        break;
                    }
                case nameof(S_升级经验模块五):
                    {
                        int num2 = (Settings.Default.升级经验模块五 = (int)control.Value);
                        int 升级经验模块五 = num2;
                        int num1610 = (Config.升级经验模块五 = 升级经验模块五);
                        break;
                    }
                case nameof(S_升级经验模块六):
                    {
                        int num2 = (Settings.Default.升级经验模块六 = (int)control.Value);
                        int 升级经验模块六 = num2;
                        int num1608 = (Config.升级经验模块六 = 升级经验模块六);
                        break;
                    }
                case nameof(S_升级经验模块七):
                    {
                        int num2 = (Settings.Default.升级经验模块七 = (int)control.Value);
                        int 升级经验模块七 = num2;
                        int num1606 = (Config.升级经验模块七 = 升级经验模块七);
                        break;
                    }
                case nameof(S_升级经验模块八):
                    {
                        int num2 = (Settings.Default.升级经验模块八 = (int)control.Value);
                        int 升级经验模块八 = num2;
                        int num1604 = (Config.升级经验模块八 = 升级经验模块八);
                        break;
                    }
                case nameof(S_升级经验模块九):
                    {
                        int num2 = (Settings.Default.升级经验模块九 = (int)control.Value);
                        int 升级经验模块九 = num2;
                        int num1602 = (Config.升级经验模块九 = 升级经验模块九);
                        break;
                    }
                case nameof(S_升级经验模块十):
                    {
                        int num2 = (Settings.Default.升级经验模块十 = (int)control.Value);
                        int 升级经验模块十 = num2;
                        int num1600 = (Config.升级经验模块十 = 升级经验模块十);
                        break;
                    }
                case nameof(S_升级经验模块十一):
                    {
                        int num2 = (Settings.Default.升级经验模块十一 = (int)control.Value);
                        int 升级经验模块十一 = num2;
                        int num1598 = (Config.升级经验模块十一 = 升级经验模块十一);
                        break;
                    }
                case nameof(S_升级经验模块十二):
                    {
                        int num2 = (Settings.Default.升级经验模块十二 = (int)control.Value);
                        int 升级经验模块十二 = num2;
                        int num1596 = (Config.升级经验模块十二 = 升级经验模块十二);
                        break;
                    }
                case nameof(S_升级经验模块十三):
                    {
                        int num2 = (Settings.Default.升级经验模块十三 = (int)control.Value);
                        int 升级经验模块十三 = num2;
                        int num1594 = (Config.升级经验模块十三 = 升级经验模块十三);
                        break;
                    }
                case nameof(S_升级经验模块十四):
                    {
                        int num2 = (Settings.Default.升级经验模块十四 = (int)control.Value);
                        int 升级经验模块十四 = num2;
                        int num1592 = (Config.升级经验模块十四 = 升级经验模块十四);
                        break;
                    }
                case nameof(S_升级经验模块十五):
                    {
                        int num2 = (Settings.Default.升级经验模块十五 = (int)control.Value);
                        int 升级经验模块十五 = num2;
                        int num1590 = (Config.升级经验模块十五 = 升级经验模块十五);
                        break;
                    }
                case nameof(S_升级经验模块十六):
                    {
                        int num2 = (Settings.Default.升级经验模块十六 = (int)control.Value);
                        int 升级经验模块十六 = num2;
                        int num1588 = (Config.升级经验模块十六 = 升级经验模块十六);
                        break;
                    }
                case nameof(S_升级经验模块十七):
                    {
                        int num2 = (Settings.Default.升级经验模块十七 = (int)control.Value);
                        int 升级经验模块十七 = num2;
                        int num1586 = (Config.升级经验模块十七 = 升级经验模块十七);
                        break;
                    }
                case nameof(S_升级经验模块十八):
                    {
                        int num2 = (Settings.Default.升级经验模块十八 = (int)control.Value);
                        int 升级经验模块十八 = num2;
                        int num1584 = (Config.升级经验模块十八 = 升级经验模块十八);
                        break;
                    }
                case nameof(S_升级经验模块十九):
                    {
                        int num2 = (Settings.Default.升级经验模块十九 = (int)control.Value);
                        int 升级经验模块十九 = num2;
                        int num1582 = (Config.升级经验模块十九 = 升级经验模块十九);
                        break;
                    }
                case nameof(S_升级经验模块二十):
                    {
                        int num2 = (Settings.Default.升级经验模块二十 = (int)control.Value);
                        int 升级经验模块二十 = num2;
                        int num1580 = (Config.升级经验模块二十 = 升级经验模块二十);
                        break;
                    }
                case nameof(S_升级经验模块二十一):
                    {
                        int num2 = (Settings.Default.升级经验模块二十一 = (int)control.Value);
                        int 升级经验模块二十一 = num2;
                        int num1578 = (Config.升级经验模块二十一 = 升级经验模块二十一);
                        break;
                    }
                case nameof(S_升级经验模块二十二):
                    {
                        int num2 = (Settings.Default.升级经验模块二十二 = (int)control.Value);
                        int 升级经验模块二十二 = num2;
                        int num1576 = (Config.升级经验模块二十二 = 升级经验模块二十二);
                        break;
                    }
                case nameof(S_升级经验模块二十三):
                    {
                        int num2 = (Settings.Default.升级经验模块二十三 = (int)control.Value);
                        int 升级经验模块二十三 = num2;
                        int num1574 = (Config.升级经验模块二十三 = 升级经验模块二十三);
                        break;
                    }
                case nameof(S_升级经验模块二十四):
                    {
                        int num2 = (Settings.Default.升级经验模块二十四 = (int)control.Value);
                        int 升级经验模块二十四 = num2;
                        int num1572 = (Config.升级经验模块二十四 = 升级经验模块二十四);
                        break;
                    }
                case nameof(S_升级经验模块二十五):
                    {
                        int num2 = (Settings.Default.升级经验模块二十五 = (int)control.Value);
                        int 升级经验模块二十五 = num2;
                        int num1570 = (Config.升级经验模块二十五 = 升级经验模块二十五);
                        break;
                    }
                case nameof(S_升级经验模块二十六):
                    {
                        int num2 = (Settings.Default.升级经验模块二十六 = (int)control.Value);
                        int 升级经验模块二十六 = num2;
                        int num1568 = (Config.升级经验模块二十六 = 升级经验模块二十六);
                        break;
                    }
                case nameof(S_升级经验模块二十七):
                    {
                        int num2 = (Settings.Default.升级经验模块二十七 = (int)control.Value);
                        int 升级经验模块二十七 = num2;
                        int num1566 = (Config.升级经验模块二十七 = 升级经验模块二十七);
                        break;
                    }
                case nameof(S_升级经验模块二十八):
                    {
                        int num2 = (Settings.Default.升级经验模块二十八 = (int)control.Value);
                        int 升级经验模块二十八 = num2;
                        int num1564 = (Config.升级经验模块二十八 = 升级经验模块二十八);
                        break;
                    }
                case nameof(S_升级经验模块二十九):
                    {
                        int num2 = (Settings.Default.升级经验模块二十九 = (int)control.Value);
                        int 升级经验模块二十九 = num2;
                        int num1562 = (Config.升级经验模块二十九 = 升级经验模块二十九);
                        break;
                    }
                case nameof(S_升级经验模块三十):
                    {
                        int num2 = (Settings.Default.升级经验模块三十 = (int)control.Value);
                        int 升级经验模块三十 = num2;
                        int num1560 = (Config.升级经验模块三十 = 升级经验模块三十);
                        break;
                    }
                case nameof(S_高级祝福油幸运机率):
                    {
                        int num2 = (Settings.Default.高级祝福油幸运机率 = (int)control.Value);
                        int 高级祝福油幸运机率 = num2;
                        int num1558 = (Config.高级祝福油幸运机率 = 高级祝福油幸运机率);
                        break;
                    }
                case nameof(S_雕爷使用物品):
                    {
                        int num2 = (Settings.Default.雕爷使用物品 = (int)control.Value);
                        int 雕爷使用物品 = num2;
                        int num1556 = (Config.雕爷使用物品 = 雕爷使用物品);
                        break;
                    }
                case nameof(S_雕爷使用金币):
                    {
                        int num2 = (Settings.Default.雕爷使用金币 = (int)control.Value);
                        int 雕爷使用金币 = num2;
                        int num1554 = (Config.雕爷使用金币 = 雕爷使用金币);
                        break;
                    }
                case nameof(S_称号范围拾取判断):
                    {
                        int num2 = (Settings.Default.称号范围拾取判断 = (int)control.Value);
                        int 称号范围拾取判断2 = num2;
                        int num1552 = (Config.称号范围拾取判断 = 称号范围拾取判断2);
                        break;
                    }
                case nameof(S_TitleRangePickUpDistance):
                    {
                        int num2 = (Settings.Default.TitleRangePickUpDistance = (int)control.Value);
                        int 称号范围拾取距离 = num2;
                        int num1550 = (Config.TitleRangePickUpDistance = 称号范围拾取距离);
                        break;
                    }
                case nameof(S_行会申请人数限制):
                    {
                        int num2 = (Settings.Default.行会申请人数限制 = (int)control.Value);
                        int 行会申请人数限制 = num2;
                        int num1548 = (Config.行会申请人数限制 = 行会申请人数限制);
                        break;
                    }
                case nameof(S_疗伤药HP):
                    {
                        int num2 = (Settings.Default.疗伤药HP = (int)control.Value);
                        int 疗伤药HP = num2;
                        int num1546 = (Config.疗伤药HP = 疗伤药HP);
                        break;
                    }
                case nameof(S_疗伤药MP):
                    {
                        int num2 = (Settings.Default.疗伤药MP = (int)control.Value);
                        int 疗伤药MP = num2;
                        int num1544 = (Config.疗伤药MP = 疗伤药MP);
                        break;
                    }
                case nameof(S_万年雪霜HP):
                    {
                        int num2 = (Settings.Default.万年雪霜HP = (int)control.Value);
                        int 万年雪霜HP = num2;
                        int num1542 = (Config.万年雪霜HP = 万年雪霜HP);
                        break;
                    }
                case nameof(S_万年雪霜MP):
                    {
                        int num2 = (Settings.Default.万年雪霜MP = (int)control.Value);
                        int 万年雪霜MP = num2;
                        int num1540 = (Config.万年雪霜MP = 万年雪霜MP);
                        break;
                    }
                case nameof(S_元宝金币回收设定):
                    {
                        int num2 = (Settings.Default.元宝金币回收设定 = (int)control.Value);
                        int 元宝金币回收设定 = num2;
                        int num1538 = (Config.元宝金币回收设定 = 元宝金币回收设定);
                        break;
                    }
                case nameof(S_元宝金币传送设定):
                    {
                        int num2 = (Settings.Default.元宝金币传送设定 = (int)control.Value);
                        int 元宝金币传送设定2 = num2;
                        int num1536 = (Config.元宝金币传送设定 = 元宝金币传送设定2);
                        break;
                    }
                case nameof(S_快捷传送一编号):
                    {
                        int num2 = (Settings.Default.快捷传送一编号 = (int)control.Value);
                        int 快捷传送一编号2 = num2;
                        int num1534 = (Config.快捷传送一编号 = 快捷传送一编号2);
                        break;
                    }
                case nameof(S_快捷传送一货币):
                    {
                        int num2 = (Settings.Default.快捷传送一货币 = (int)control.Value);
                        int 快捷传送一货币2 = num2;
                        int num1532 = (Config.快捷传送一货币 = 快捷传送一货币2);
                        break;
                    }
                case nameof(S_快捷传送一等级):
                    {
                        int num2 = (Settings.Default.快捷传送一等级 = (int)control.Value);
                        int 快捷传送一等级2 = num2;
                        int num1530 = (Config.快捷传送一等级 = 快捷传送一等级2);
                        break;
                    }
                case nameof(S_快捷传送二编号):
                    {
                        int num2 = (Settings.Default.快捷传送二编号 = (int)control.Value);
                        int 快捷传送二编号2 = num2;
                        int num1528 = (Config.快捷传送二编号 = 快捷传送二编号2);
                        break;
                    }
                case nameof(S_快捷传送二货币):
                    {
                        int num2 = (Settings.Default.快捷传送二货币 = (int)control.Value);
                        int 快捷传送二货币2 = num2;
                        int num1526 = (Config.快捷传送二货币 = 快捷传送二货币2);
                        break;
                    }
                case nameof(S_快捷传送二等级):
                    {
                        int num2 = (Settings.Default.快捷传送二等级 = (int)control.Value);
                        int 快捷传送二等级2 = num2;
                        int num1524 = (Config.快捷传送二等级 = 快捷传送二等级2);
                        break;
                    }
                case nameof(S_快捷传送三编号):
                    {
                        int num2 = (Settings.Default.快捷传送三编号 = (int)control.Value);
                        int 快捷传送三编号2 = num2;
                        int num1522 = (Config.快捷传送三编号 = 快捷传送三编号2);
                        break;
                    }
                case nameof(S_快捷传送三货币):
                    {
                        int num2 = (Settings.Default.快捷传送三货币 = (int)control.Value);
                        int 快捷传送三货币2 = num2;
                        int num1520 = (Config.快捷传送三货币 = 快捷传送三货币2);
                        break;
                    }
                case nameof(S_快捷传送三等级):
                    {
                        int num2 = (Settings.Default.快捷传送三等级 = (int)control.Value);
                        int 快捷传送三等级2 = num2;
                        int num1518 = (Config.快捷传送三等级 = 快捷传送三等级2);
                        break;
                    }
                case nameof(S_快捷传送四编号):
                    {
                        int num2 = (Settings.Default.快捷传送四编号 = (int)control.Value);
                        int 快捷传送四编号2 = num2;
                        int num1516 = (Config.快捷传送四编号 = 快捷传送四编号2);
                        break;
                    }
                case nameof(S_快捷传送四货币):
                    {
                        int num2 = (Settings.Default.快捷传送四货币 = (int)control.Value);
                        int 快捷传送四货币2 = num2;
                        int num1514 = (Config.快捷传送四货币 = 快捷传送四货币2);
                        break;
                    }
                case nameof(S_快捷传送四等级):
                    {
                        int num2 = (Settings.Default.快捷传送四等级 = (int)control.Value);
                        int 快捷传送四等级2 = num2;
                        int num1512 = (Config.快捷传送四等级 = 快捷传送四等级2);
                        break;
                    }
                case nameof(S_快捷传送五编号):
                    {
                        int num2 = (Settings.Default.快捷传送五编号 = (int)control.Value);
                        int 快捷传送五编号2 = num2;
                        int num1510 = (Config.快捷传送五编号 = 快捷传送五编号2);
                        break;
                    }
                case nameof(S_快捷传送五货币):
                    {
                        int num2 = (Settings.Default.快捷传送五货币 = (int)control.Value);
                        int 快捷传送五货币2 = num2;
                        int num1508 = (Config.快捷传送五货币 = 快捷传送五货币2);
                        break;
                    }
                case nameof(S_快捷传送五等级):
                    {
                        int num2 = (Settings.Default.快捷传送五等级 = (int)control.Value);
                        int 快捷传送五等级2 = num2;
                        int num1506 = (Config.快捷传送五等级 = 快捷传送五等级2);
                        break;
                    }
                case nameof(S_狂暴货币格式):
                    {
                        int num2 = (Settings.Default.狂暴货币格式 = (int)control.Value);
                        int 狂暴货币格式 = num2;
                        int num1504 = (Config.狂暴货币格式 = 狂暴货币格式);
                        break;
                    }
                case nameof(S_狂暴称号格式):
                    {
                        byte b2 = (Settings.Default.狂暴称号格式 = (byte)control.Value);
                        byte 狂暴称号格式 = b2;
                        byte b83 = (Config.狂暴称号格式 = 狂暴称号格式);
                        break;
                    }
                case nameof(S_狂暴开启物品名称):
                    {
                        int num2 = (Settings.Default.狂暴开启物品名称 = (int)control.Value);
                        int 狂暴开启物品名称 = num2;
                        int num1502 = (Config.狂暴开启物品名称 = 狂暴开启物品名称);
                        break;
                    }
                case nameof(S_狂暴开启物品数量):
                    {
                        int num2 = (Settings.Default.狂暴开启物品数量 = (int)control.Value);
                        int 狂暴开启物品数量 = num2;
                        int num1500 = (Config.狂暴开启物品数量 = 狂暴开启物品数量);
                        break;
                    }
                case nameof(S_狂暴杀死物品数量):
                    {
                        int num2 = (Settings.Default.狂暴杀死物品数量 = (int)control.Value);
                        int 狂暴杀死物品数量 = num2;
                        int num1498 = (Config.狂暴杀死物品数量 = 狂暴杀死物品数量);
                        break;
                    }
                case nameof(S_狂暴开启元宝数量):
                    {
                        int num2 = (Settings.Default.狂暴开启元宝数量 = (int)control.Value);
                        int 狂暴开启元宝数量 = num2;
                        int num1496 = (Config.狂暴开启元宝数量 = 狂暴开启元宝数量);
                        break;
                    }
                case nameof(S_狂暴杀死元宝数量):
                    {
                        int num2 = (Settings.Default.狂暴杀死元宝数量 = (int)control.Value);
                        int 狂暴杀死元宝数量3 = num2;
                        int num1494 = (Config.狂暴杀死元宝数量 = 狂暴杀死元宝数量3);
                        break;
                    }
                case nameof(S_狂暴开启金币数量):
                    {
                        int num2 = (Settings.Default.狂暴开启金币数量 = (int)control.Value);
                        int 狂暴杀死元宝数量2 = num2;
                        int num1492 = (Config.狂暴杀死元宝数量 = 狂暴杀死元宝数量2);
                        break;
                    }
                case nameof(S_狂暴杀死金币数量):
                    {
                        int num2 = (Settings.Default.狂暴杀死金币数量 = (int)control.Value);
                        int 狂暴杀死元宝数量 = num2;
                        int num1490 = (Config.狂暴杀死元宝数量 = 狂暴杀死元宝数量);
                        break;
                    }
                case nameof(S_装备技能开关):
                    {
                        int num2 = (Settings.Default.装备技能开关 = (int)control.Value);
                        int 装备技能开关 = num2;
                        int num1488 = (Config.装备技能开关 = 装备技能开关);
                        break;
                    }
                case nameof(S_御兽属性开启):
                    {
                        int num2 = (Settings.Default.御兽属性开启 = (int)control.Value);
                        int 御兽属性开启 = num2;
                        int num1486 = (Config.御兽属性开启 = 御兽属性开启);
                        break;
                    }
                case nameof(S_可摆摊地图编号):
                    {
                        int num2 = (Settings.Default.可摆摊地图编号 = (int)control.Value);
                        int 可摆摊地图编号 = num2;
                        int num1484 = (Config.可摆摊地图编号 = 可摆摊地图编号);
                        break;
                    }
                case nameof(S_可摆摊地图坐标X):
                    {
                        int num2 = (Settings.Default.可摆摊地图坐标X = (int)control.Value);
                        int 可摆摊地图坐标X = num2;
                        int num1482 = (Config.可摆摊地图坐标X = 可摆摊地图坐标X);
                        break;
                    }
                case nameof(S_可摆摊地图坐标Y):
                    {
                        int num2 = (Settings.Default.可摆摊地图坐标Y = (int)control.Value);
                        int 可摆摊地图坐标Y = num2;
                        int num1480 = (Config.可摆摊地图坐标Y = 可摆摊地图坐标Y);
                        break;
                    }
                case nameof(S_可摆摊地图范围):
                    {
                        int num2 = (Settings.Default.可摆摊地图范围 = (int)control.Value);
                        int 可摆摊地图范围 = num2;
                        int num1478 = (Config.可摆摊地图范围 = 可摆摊地图范围);
                        break;
                    }
                case nameof(S_可摆摊货币选择):
                    {
                        int num2 = (Settings.Default.可摆摊货币选择 = (int)control.Value);
                        int 可摆摊货币选择 = num2;
                        int num1476 = (Config.可摆摊货币选择 = 可摆摊货币选择);
                        break;
                    }
                case nameof(S_可摆摊等级):
                    {
                        int num2 = (Settings.Default.可摆摊等级 = (int)control.Value);
                        int 可摆摊等级 = num2;
                        int num1474 = (Config.可摆摊等级 = 可摆摊等级);
                        break;
                    }
                case nameof(S_ReviveInterval):
                    {
                        int num2 = (Settings.Default.ReviveInterval = (int)control.Value);
                        int 自定义复活间隔 = num2;
                        int num1472 = (Config.ReviveInterval = 自定义复活间隔);
                        break;
                    }
                case nameof(S_自定义麻痹几率):
                    {
                        float num340 = (Settings.Default.自定义麻痹几率 = (float)control.Value);
                        float 自定义麻痹几率 = num340;
                        float num1470 = (Config.自定义麻痹几率 = 自定义麻痹几率);
                        break;
                    }
                case nameof(S_PetUpgradeXPLevel1):
                    {
                        ushort num1451 = (Settings.Default.PetUpgradeXPLevel1 = (ushort)control.Value);
                        ushort 宠物升级1级经验 = num1451;
                        ushort num1468 = (Config.PetUpgradeXPLevel1 = 宠物升级1级经验);
                        break;
                    }
                case nameof(S_PetUpgradeXPLevel2):
                    {
                        ushort num1451 = (Settings.Default.PetUpgradeXPLevel2 = (ushort)control.Value);
                        ushort 宠物升级2级经验 = num1451;
                        ushort num1466 = (Config.PetUpgradeXPLevel2 = 宠物升级2级经验);
                        break;
                    }
                case nameof(S_PetUpgradeXPLevel3):
                    {
                        ushort num1451 = (Settings.Default.PetUpgradeXPLevel3 = (ushort)control.Value);
                        ushort 宠物升级3级经验 = num1451;
                        ushort num1464 = (Config.PetUpgradeXPLevel3 = 宠物升级3级经验);
                        break;
                    }
                case nameof(S_PetUpgradeXPLevel4):
                    {
                        ushort num1451 = (Settings.Default.PetUpgradeXPLevel4 = (ushort)control.Value);
                        ushort 宠物升级4级经验 = num1451;
                        ushort num1462 = (Config.PetUpgradeXPLevel4 = 宠物升级4级经验);
                        break;
                    }
                case nameof(S_PetUpgradeXPLevel5):
                    {
                        ushort num1451 = (Settings.Default.PetUpgradeXPLevel5 = (ushort)control.Value);
                        ushort 宠物升级5级经验 = num1451;
                        ushort num1460 = (Config.PetUpgradeXPLevel5 = 宠物升级5级经验);
                        break;
                    }
                case nameof(S_PetUpgradeXPLevel6):
                    {
                        ushort num1451 = (Settings.Default.PetUpgradeXPLevel6 = (ushort)control.Value);
                        ushort 宠物升级6级经验 = num1451;
                        ushort num1458 = (Config.PetUpgradeXPLevel6 = 宠物升级6级经验);
                        break;
                    }
                case nameof(S_PetUpgradeXPLevel7):
                    {
                        ushort num1451 = (Settings.Default.PetUpgradeXPLevel7 = (ushort)control.Value);
                        ushort 宠物升级7级经验 = num1451;
                        ushort num1456 = (Config.PetUpgradeXPLevel7 = 宠物升级7级经验);
                        break;
                    }
                case nameof(S_PetUpgradeXPLevel8):
                    {
                        ushort num1451 = (Settings.Default.PetUpgradeXPLevel8 = (ushort)control.Value);
                        ushort 宠物升级8级经验 = num1451;
                        ushort num1454 = (Config.PetUpgradeXPLevel8 = 宠物升级8级经验);
                        break;
                    }
                case nameof(S_PetUpgradeXPLevel9):
                    {
                        ushort num1451 = (Settings.Default.PetUpgradeXPLevel9 = (ushort)control.Value);
                        ushort 宠物升级9级经验 = num1451;
                        ushort num1452 = (Config.PetUpgradeXPLevel9 = 宠物升级9级经验);
                        break;
                    }
                case nameof(S_下马击落机率):
                    {
                        int num2 = (Settings.Default.下马击落机率 = (int)control.Value);
                        int 下马击落机率 = num2;
                        int num1449 = (Config.下马击落机率 = 下马击落机率);
                        break;
                    }
                case nameof(S_AllowRaceWarrior):
                    {
                        int num2 = (Settings.Default.AllowRaceWarrior = (int)control.Value);
                        int 六职业关闭战士 = num2;
                        int num1447 = (Config.AllowRaceWarrior = 六职业关闭战士);
                        break;
                    }
                case nameof(S_AllowRaceWizard):
                    {
                        int num2 = (Settings.Default.AllowRaceWizard = (int)control.Value);
                        int 六职业关闭法师 = num2;
                        int num1445 = (Config.AllowRaceWizard = 六职业关闭法师);
                        break;
                    }
                case nameof(S_AllowRaceTaoist):
                    {
                        int num2 = (Settings.Default.AllowRaceTaoist = (int)control.Value);
                        int 六职业关闭道士 = num2;
                        int num1443 = (Config.AllowRaceTaoist = 六职业关闭道士);
                        break;
                    }
                case nameof(S_AllowRaceArcher):
                    {
                        int num2 = (Settings.Default.AllowRaceArcher = (int)control.Value);
                        int 六职业关闭弓手 = num2;
                        int num1441 = (Config.AllowRaceArcher = 六职业关闭弓手);
                        break;
                    }
                case nameof(S_AllowRaceAssassin):
                    {
                        int num2 = (Settings.Default.AllowRaceAssassin = (int)control.Value);
                        int 六职业关闭刺客 = num2;
                        int num1439 = (Config.AllowRaceAssassin = 六职业关闭刺客);
                        break;
                    }
                case nameof(S_AllowRaceDragonLance):
                    {
                        int num2 = (Settings.Default.AllowRaceDragonLance = (int)control.Value);
                        int 六职业关闭龙枪 = num2;
                        int num1437 = (Config.AllowRaceDragonLance = 六职业关闭龙枪);
                        break;
                    }
                case nameof(S_泡点等级开关):
                    {
                        int num2 = (Settings.Default.泡点等级开关 = (int)control.Value);
                        int 泡点等级开关 = num2;
                        int num1435 = (Config.泡点等级开关 = 泡点等级开关);
                        break;
                    }
                case nameof(S_泡点当前经验):
                    {
                        int num2 = (Settings.Default.泡点当前经验 = (int)control.Value);
                        int 泡点当前经验 = num2;
                        int num1433 = (Config.泡点当前经验 = 泡点当前经验);
                        break;
                    }
                case nameof(S_泡点限制等级):
                    {
                        int num2 = (Settings.Default.泡点限制等级 = (int)control.Value);
                        int 泡点限制等级 = num2;
                        int num1431 = (Config.泡点限制等级 = 泡点限制等级);
                        break;
                    }
                case nameof(S_杀人PK红名开关):
                    {
                        int num2 = (Settings.Default.杀人PK红名开关 = (int)control.Value);
                        int 杀人PK红名开关 = num2;
                        int num1429 = (Config.杀人PK红名开关 = 杀人PK红名开关);
                        break;
                    }
                case nameof(S_泡点秒数控制):
                    {
                        int num2 = (Settings.Default.泡点秒数控制 = (int)control.Value);
                        int 泡点秒数控制 = num2;
                        int num1427 = (Config.泡点秒数控制 = 泡点秒数控制);
                        break;
                    }
                case nameof(S_自定义物品数量一):
                    {
                        int num2 = (Settings.Default.自定义物品数量一 = (int)control.Value);
                        int 自定义物品数量一 = num2;
                        int num1425 = (Config.自定义物品数量一 = 自定义物品数量一);
                        break;
                    }
                case nameof(S_自定义物品数量二):
                    {
                        int num2 = (Settings.Default.自定义物品数量二 = (int)control.Value);
                        int 自定义物品数量二 = num2;
                        int num1423 = (Config.自定义物品数量二 = 自定义物品数量二);
                        break;
                    }
                case nameof(S_自定义物品数量三):
                    {
                        int num2 = (Settings.Default.自定义物品数量三 = (int)control.Value);
                        int 自定义物品数量三 = num2;
                        int num1421 = (Config.自定义物品数量三 = 自定义物品数量三);
                        break;
                    }
                case nameof(S_自定义物品数量四):
                    {
                        int num2 = (Settings.Default.自定义物品数量四 = (int)control.Value);
                        int 自定义物品数量四 = num2;
                        int num1419 = (Config.自定义物品数量四 = 自定义物品数量四);
                        break;
                    }
                case nameof(S_自定义物品数量五):
                    {
                        int num2 = (Settings.Default.自定义物品数量五 = (int)control.Value);
                        int 自定义物品数量五 = num2;
                        int num1417 = (Config.自定义物品数量五 = 自定义物品数量五);
                        break;
                    }
                case nameof(S_自定义称号内容一):
                    {
                        byte b2 = (Settings.Default.自定义称号内容一 = (byte)control.Value);
                        byte 自定义称号内容一 = b2;
                        byte b81 = (Config.自定义称号内容一 = 自定义称号内容一);
                        break;
                    }
                case nameof(S_自定义称号内容二):
                    {
                        byte b2 = (Settings.Default.自定义称号内容二 = (byte)control.Value);
                        byte 自定义称号内容二 = b2;
                        byte b79 = (Config.自定义称号内容二 = 自定义称号内容二);
                        break;
                    }
                case nameof(S_自定义称号内容三):
                    {
                        byte b2 = (Settings.Default.自定义称号内容三 = (byte)control.Value);
                        byte 自定义称号内容三 = b2;
                        byte b77 = (Config.自定义称号内容三 = 自定义称号内容三);
                        break;
                    }
                case nameof(S_自定义称号内容四):
                    {
                        byte b2 = (Settings.Default.自定义称号内容四 = (byte)control.Value);
                        byte 自定义称号内容四 = b2;
                        byte b75 = (Config.自定义称号内容四 = 自定义称号内容四);
                        break;
                    }
                case nameof(S_自定义称号内容五):
                    {
                        byte b2 = (Settings.Default.自定义称号内容五 = (byte)control.Value);
                        byte 自定义称号内容五 = b2;
                        byte b73 = (Config.自定义称号内容五 = 自定义称号内容五);
                        break;
                    }
                case nameof(S_元宝金币传送设定2):
                    {
                        int num2 = (Settings.Default.元宝金币传送设定2 = (int)control.Value);
                        int 元宝金币传送设定 = num2;
                        int num1415 = (Config.元宝金币传送设定2 = 元宝金币传送设定);
                        break;
                    }
                case nameof(S_快捷传送一编号2):
                    {
                        int num2 = (Settings.Default.快捷传送一编号2 = (int)control.Value);
                        int 快捷传送一编号 = num2;
                        int num1413 = (Config.快捷传送一编号2 = 快捷传送一编号);
                        break;
                    }
                case nameof(S_快捷传送一货币2):
                    {
                        int num2 = (Settings.Default.快捷传送一货币2 = (int)control.Value);
                        int 快捷传送一货币 = num2;
                        int num1411 = (Config.快捷传送一货币2 = 快捷传送一货币);
                        break;
                    }
                case nameof(S_快捷传送一等级2):
                    {
                        int num2 = (Settings.Default.快捷传送一等级2 = (int)control.Value);
                        int 快捷传送一等级 = num2;
                        int num1409 = (Config.快捷传送一等级2 = 快捷传送一等级);
                        break;
                    }
                case nameof(S_快捷传送二编号2):
                    {
                        int num2 = (Settings.Default.快捷传送二编号2 = (int)control.Value);
                        int 快捷传送二编号 = num2;
                        int num1407 = (Config.快捷传送二编号2 = 快捷传送二编号);
                        break;
                    }
                case nameof(S_快捷传送二货币2):
                    {
                        int num2 = (Settings.Default.快捷传送二货币2 = (int)control.Value);
                        int 快捷传送二货币 = num2;
                        int num1405 = (Config.快捷传送二货币2 = 快捷传送二货币);
                        break;
                    }
                case nameof(S_快捷传送二等级2):
                    {
                        int num2 = (Settings.Default.快捷传送二等级2 = (int)control.Value);
                        int 快捷传送二等级 = num2;
                        int num1403 = (Config.快捷传送二等级2 = 快捷传送二等级);
                        break;
                    }
                case nameof(S_快捷传送三编号2):
                    {
                        int num2 = (Settings.Default.快捷传送三编号2 = (int)control.Value);
                        int 快捷传送三编号 = num2;
                        int num1401 = (Config.快捷传送三编号2 = 快捷传送三编号);
                        break;
                    }
                case nameof(S_快捷传送三货币2):
                    {
                        int num2 = (Settings.Default.快捷传送三货币2 = (int)control.Value);
                        int 快捷传送三货币 = num2;
                        int num1399 = (Config.快捷传送三货币2 = 快捷传送三货币);
                        break;
                    }
                case nameof(S_快捷传送三等级2):
                    {
                        int num2 = (Settings.Default.快捷传送三等级2 = (int)control.Value);
                        int 快捷传送三等级 = num2;
                        int num1397 = (Config.快捷传送三等级2 = 快捷传送三等级);
                        break;
                    }
                case nameof(S_快捷传送四编号2):
                    {
                        int num2 = (Settings.Default.快捷传送四编号2 = (int)control.Value);
                        int 快捷传送四编号 = num2;
                        int num1395 = (Config.快捷传送四编号2 = 快捷传送四编号);
                        break;
                    }
                case nameof(S_快捷传送四货币2):
                    {
                        int num2 = (Settings.Default.快捷传送四货币2 = (int)control.Value);
                        int 快捷传送四货币 = num2;
                        int num1393 = (Config.快捷传送四货币2 = 快捷传送四货币);
                        break;
                    }
                case nameof(S_快捷传送四等级2):
                    {
                        int num2 = (Settings.Default.快捷传送四等级2 = (int)control.Value);
                        int 快捷传送四等级 = num2;
                        int num1391 = (Config.快捷传送四等级2 = 快捷传送四等级);
                        break;
                    }
                case nameof(S_快捷传送五编号2):
                    {
                        int num2 = (Settings.Default.快捷传送五编号2 = (int)control.Value);
                        int 快捷传送五编号 = num2;
                        int num1389 = (Config.快捷传送五编号2 = 快捷传送五编号);
                        break;
                    }
                case nameof(S_快捷传送五货币2):
                    {
                        int num2 = (Settings.Default.快捷传送五货币2 = (int)control.Value);
                        int 快捷传送五货币 = num2;
                        int num1387 = (Config.快捷传送五货币2 = 快捷传送五货币);
                        break;
                    }
                case nameof(S_快捷传送五等级2):
                    {
                        int num2 = (Settings.Default.快捷传送五等级2 = (int)control.Value);
                        int 快捷传送五等级 = num2;
                        int num1385 = (Config.快捷传送五等级2 = 快捷传送五等级);
                        break;
                    }
                case nameof(S_快捷传送六编号2):
                    {
                        int num2 = (Settings.Default.快捷传送六编号2 = (int)control.Value);
                        int 快捷传送六编号 = num2;
                        int num1383 = (Config.快捷传送六编号2 = 快捷传送六编号);
                        break;
                    }
                case nameof(S_快捷传送六货币2):
                    {
                        int num2 = (Settings.Default.快捷传送六货币2 = (int)control.Value);
                        int 快捷传送六货币 = num2;
                        int num1381 = (Config.快捷传送六货币2 = 快捷传送六货币);
                        break;
                    }
                case nameof(S_快捷传送六等级2):
                    {
                        int num2 = (Settings.Default.快捷传送六等级2 = (int)control.Value);
                        int 快捷传送六等级 = num2;
                        int num1379 = (Config.快捷传送六等级2 = 快捷传送六等级);
                        break;
                    }
                case nameof(S_BOSS一时间):
                    {
                        byte b2 = (Settings.Default.BOSS一时间 = (byte)control.Value);
                        byte bOSS一时间 = b2;
                        byte b71 = (Config.BOSS一时间 = bOSS一时间);
                        break;
                    }
                case nameof(S_BOSS一分钟):
                    {
                        byte b2 = (Settings.Default.BOSS一分钟 = (byte)control.Value);
                        byte bOSS一分钟 = b2;
                        byte b69 = (Config.BOSS一分钟 = bOSS一分钟);
                        break;
                    }
                case nameof(S_BOSS一地图编号):
                    {
                        int num2 = (Settings.Default.BOSS一地图编号 = (int)control.Value);
                        int bOSS一地图编号 = num2;
                        int num1377 = (Config.BOSS一地图编号 = bOSS一地图编号);
                        break;
                    }
                case nameof(S_BOSS一坐标X):
                    {
                        int num2 = (Settings.Default.BOSS一坐标X = (int)control.Value);
                        int bOSS一坐标X = num2;
                        int num1375 = (Config.BOSS一坐标X = bOSS一坐标X);
                        break;
                    }
                case nameof(S_BOSS一坐标Y):
                    {
                        int num2 = (Settings.Default.BOSS一坐标Y = (int)control.Value);
                        int bOSS一坐标Y = num2;
                        int num1373 = (Config.BOSS一坐标Y = bOSS一坐标Y);
                        break;
                    }
                case nameof(S_武斗场次数限制):
                    {
                        int num2 = (Settings.Default.武斗场次数限制 = (int)control.Value);
                        int 武斗场次数限制 = num2;
                        int num1371 = (Config.武斗场次数限制 = 武斗场次数限制);
                        break;
                    }
                case nameof(S_AutoPickUpInventorySpace):
                    {
                        int num2 = (Settings.Default.AutoPickUpInventorySpace = (int)control.Value);
                        int 自动拾取背包空格 = num2;
                        int num1369 = (Config.AutoPickUpInventorySpace = 自动拾取背包空格);
                        break;
                    }
                case nameof(S_BOSS刷新提示开关):
                    {
                        int num2 = (Settings.Default.BOSS刷新提示开关 = (int)control.Value);
                        int bOSS刷新提示开关 = num2;
                        int num1367 = (Config.BOSS刷新提示开关 = bOSS刷新提示开关);
                        break;
                    }
                case nameof(S_自动整理背包计时):
                    {
                        int num2 = (Settings.Default.自动整理背包计时 = (int)control.Value);
                        int 自动整理背包计时 = num2;
                        int num1365 = (Config.自动整理背包计时 = 自动整理背包计时);
                        break;
                    }
                case nameof(S_自动整理背包开关):
                    {
                        int num2 = (Settings.Default.自动整理背包开关 = (int)control.Value);
                        int 自动整理背包开关 = num2;
                        int num1363 = (Config.自动整理背包开关 = 自动整理背包开关);
                        break;
                    }
                case nameof(S_称号叠加开关):
                    {
                        int num2 = (Settings.Default.称号叠加开关 = (int)control.Value);
                        int 称号叠加开关 = num2;
                        int num1361 = (Config.称号叠加开关 = 称号叠加开关);
                        break;
                    }
                case nameof(S_称号叠加模块一):
                    {
                        byte b2 = (Settings.Default.称号叠加模块一 = (byte)control.Value);
                        byte 称号叠加模块一 = b2;
                        byte b67 = (Config.称号叠加模块一 = 称号叠加模块一);
                        break;
                    }
                case nameof(S_称号叠加模块二):
                    {
                        byte b2 = (Settings.Default.称号叠加模块二 = (byte)control.Value);
                        byte 称号叠加模块二 = b2;
                        byte b65 = (Config.称号叠加模块二 = 称号叠加模块二);
                        break;
                    }
                case nameof(S_称号叠加模块三):
                    {
                        byte b2 = (Settings.Default.称号叠加模块三 = (byte)control.Value);
                        byte 称号叠加模块三 = b2;
                        byte b63 = (Config.称号叠加模块三 = 称号叠加模块三);
                        break;
                    }
                case nameof(S_称号叠加模块四):
                    {
                        byte b2 = (Settings.Default.称号叠加模块四 = (byte)control.Value);
                        byte 称号叠加模块四 = b2;
                        byte b61 = (Config.称号叠加模块四 = 称号叠加模块四);
                        break;
                    }
                case nameof(S_称号叠加模块五):
                    {
                        byte b2 = (Settings.Default.称号叠加模块五 = (byte)control.Value);
                        byte 称号叠加模块五 = b2;
                        byte b59 = (Config.称号叠加模块五 = 称号叠加模块五);
                        break;
                    }
                case nameof(S_称号叠加模块六):
                    {
                        byte b2 = (Settings.Default.称号叠加模块六 = (byte)control.Value);
                        byte 称号叠加模块六 = b2;
                        byte b57 = (Config.称号叠加模块六 = 称号叠加模块六);
                        break;
                    }
                case nameof(S_称号叠加模块七):
                    {
                        byte b2 = (Settings.Default.称号叠加模块七 = (byte)control.Value);
                        byte 称号叠加模块七 = b2;
                        byte b55 = (Config.称号叠加模块七 = 称号叠加模块七);
                        break;
                    }
                case nameof(S_称号叠加模块八):
                    {
                        byte b2 = (Settings.Default.称号叠加模块八 = (byte)control.Value);
                        byte 称号叠加模块八 = b2;
                        byte b53 = (Config.称号叠加模块八 = 称号叠加模块八);
                        break;
                    }
                case nameof(S_沙城传送货币开关):
                    {
                        int num2 = (Settings.Default.沙城传送货币开关 = (int)control.Value);
                        int 沙城传送货币开关 = num2;
                        int num1359 = (Config.沙城传送货币开关 = 沙城传送货币开关);
                        break;
                    }
                case nameof(S_沙城快捷货币一):
                    {
                        int num2 = (Settings.Default.沙城快捷货币一 = (int)control.Value);
                        int 沙城快捷货币一 = num2;
                        int num1357 = (Config.沙城快捷货币一 = 沙城快捷货币一);
                        break;
                    }
                case nameof(S_沙城快捷货币二):
                    {
                        int num2 = (Settings.Default.沙城快捷货币二 = (int)control.Value);
                        int 沙城快捷货币二 = num2;
                        int num1355 = (Config.沙城快捷货币二 = 沙城快捷货币二);
                        break;
                    }
                case nameof(S_沙城快捷货币三):
                    {
                        int num2 = (Settings.Default.沙城快捷货币三 = (int)control.Value);
                        int 沙城快捷货币三 = num2;
                        int num1353 = (Config.沙城快捷货币三 = 沙城快捷货币三);
                        break;
                    }
                case nameof(S_沙城快捷货币四):
                    {
                        int num2 = (Settings.Default.沙城快捷货币四 = (int)control.Value);
                        int 沙城快捷货币四 = num2;
                        int num1351 = (Config.沙城快捷货币四 = 沙城快捷货币四);
                        break;
                    }
                case nameof(S_沙城快捷等级一):
                    {
                        int num2 = (Settings.Default.沙城快捷等级一 = (int)control.Value);
                        int 沙城快捷等级一 = num2;
                        int num1349 = (Config.沙城快捷等级一 = 沙城快捷等级一);
                        break;
                    }
                case nameof(S_沙城快捷等级二):
                    {
                        int num2 = (Settings.Default.沙城快捷等级二 = (int)control.Value);
                        int 沙城快捷等级二 = num2;
                        int num1347 = (Config.沙城快捷等级二 = 沙城快捷等级二);
                        break;
                    }
                case nameof(S_沙城快捷等级三):
                    {
                        int num2 = (Settings.Default.沙城快捷等级三 = (int)control.Value);
                        int 沙城快捷等级三 = num2;
                        int num1345 = (Config.沙城快捷等级三 = 沙城快捷等级三);
                        break;
                    }
                case nameof(S_沙城快捷等级四):
                    {
                        int num2 = (Settings.Default.沙城快捷等级四 = (int)control.Value);
                        int 沙城快捷等级四 = num2;
                        int num1343 = (Config.沙城快捷等级四 = 沙城快捷等级四);
                        break;
                    }
                case nameof(S_BOSS二时间):
                    {
                        byte b2 = (Settings.Default.BOSS二时间 = (byte)control.Value);
                        byte bOSS二时间 = b2;
                        byte b51 = (Config.BOSS二时间 = bOSS二时间);
                        break;
                    }
                case nameof(S_BOSS二分钟):
                    {
                        byte b2 = (Settings.Default.BOSS二分钟 = (byte)control.Value);
                        byte bOSS二分钟 = b2;
                        byte b49 = (Config.BOSS二分钟 = bOSS二分钟);
                        break;
                    }
                case nameof(S_BOSS二地图编号):
                    {
                        int num2 = (Settings.Default.BOSS二地图编号 = (int)control.Value);
                        int bOSS二地图编号 = num2;
                        int num1341 = (Config.BOSS二地图编号 = bOSS二地图编号);
                        break;
                    }
                case nameof(S_BOSS二坐标X):
                    {
                        int num2 = (Settings.Default.BOSS二坐标X = (int)control.Value);
                        int bOSS二坐标X = num2;
                        int num1339 = (Config.BOSS二坐标X = bOSS二坐标X);
                        break;
                    }
                case nameof(S_BOSS二坐标Y):
                    {
                        int num2 = (Settings.Default.BOSS二坐标Y = (int)control.Value);
                        int bOSS二坐标Y = num2;
                        int num1337 = (Config.BOSS二坐标Y = bOSS二坐标Y);
                        break;
                    }
                case nameof(S_BOSS三时间):
                    {
                        byte b2 = (Settings.Default.BOSS三时间 = (byte)control.Value);
                        byte bOSS三时间 = b2;
                        byte b47 = (Config.BOSS三时间 = bOSS三时间);
                        break;
                    }
                case nameof(S_BOSS三分钟):
                    {
                        byte b2 = (Settings.Default.BOSS三分钟 = (byte)control.Value);
                        byte bOSS三分钟 = b2;
                        byte b45 = (Config.BOSS三分钟 = bOSS三分钟);
                        break;
                    }
                case nameof(S_BOSS三地图编号):
                    {
                        int num2 = (Settings.Default.BOSS三地图编号 = (int)control.Value);
                        int bOSS三地图编号 = num2;
                        int num1335 = (Config.BOSS三地图编号 = bOSS三地图编号);
                        break;
                    }
                case nameof(S_BOSS三坐标X):
                    {
                        int num2 = (Settings.Default.BOSS三坐标X = (int)control.Value);
                        int bOSS三坐标X = num2;
                        int num1333 = (Config.BOSS三坐标X = bOSS三坐标X);
                        break;
                    }
                case nameof(S_BOSS三坐标Y):
                    {
                        int num2 = (Settings.Default.BOSS三坐标Y = (int)control.Value);
                        int bOSS三坐标Y = num2;
                        int num1331 = (Config.BOSS三坐标Y = bOSS三坐标Y);
                        break;
                    }
                case nameof(S_BOSS四时间):
                    {
                        byte b2 = (Settings.Default.BOSS四时间 = (byte)control.Value);
                        byte bOSS四时间 = b2;
                        byte b43 = (Config.BOSS四时间 = bOSS四时间);
                        break;
                    }
                case nameof(S_BOSS四分钟):
                    {
                        byte b2 = (Settings.Default.BOSS四分钟 = (byte)control.Value);
                        byte bOSS四分钟 = b2;
                        byte b41 = (Config.BOSS四分钟 = bOSS四分钟);
                        break;
                    }
                case nameof(S_BOSS四地图编号):
                    {
                        int num2 = (Settings.Default.BOSS四地图编号 = (int)control.Value);
                        int bOSS四地图编号 = num2;
                        int num1329 = (Config.BOSS四地图编号 = bOSS四地图编号);
                        break;
                    }
                case nameof(S_BOSS四坐标X):
                    {
                        int num2 = (Settings.Default.BOSS四坐标X = (int)control.Value);
                        int bOSS四坐标X = num2;
                        int num1327 = (Config.BOSS四坐标X = bOSS四坐标X);
                        break;
                    }
                case nameof(S_BOSS四坐标Y):
                    {
                        int num2 = (Settings.Default.BOSS四坐标Y = (int)control.Value);
                        int bOSS四坐标Y = num2;
                        int num1325 = (Config.BOSS四坐标Y = bOSS四坐标Y);
                        break;
                    }
                case nameof(S_BOSS五时间):
                    {
                        byte b2 = (Settings.Default.BOSS五时间 = (byte)control.Value);
                        byte bOSS五时间 = b2;
                        byte b39 = (Config.BOSS五时间 = bOSS五时间);
                        break;
                    }
                case nameof(S_BOSS五分钟):
                    {
                        byte b2 = (Settings.Default.BOSS五分钟 = (byte)control.Value);
                        byte bOSS五分钟 = b2;
                        byte b37 = (Config.BOSS五分钟 = bOSS五分钟);
                        break;
                    }
                case nameof(S_BOSS五地图编号):
                    {
                        int num2 = (Settings.Default.BOSS五地图编号 = (int)control.Value);
                        int bOSS五地图编号 = num2;
                        int num1323 = (Config.BOSS五地图编号 = bOSS五地图编号);
                        break;
                    }
                case nameof(S_BOSS五坐标X):
                    {
                        int num2 = (Settings.Default.BOSS五坐标X = (int)control.Value);
                        int bOSS五坐标X = num2;
                        int num1321 = (Config.BOSS五坐标X = bOSS五坐标X);
                        break;
                    }
                case nameof(S_BOSS五坐标Y):
                    {
                        int num2 = (Settings.Default.BOSS五坐标Y = (int)control.Value);
                        int bOSS五坐标Y = num2;
                        int num1319 = (Config.BOSS五坐标Y = bOSS五坐标Y);
                        break;
                    }
                case nameof(S_未知暗点副本价格):
                    {
                        int num2 = (Settings.Default.未知暗点副本价格 = (int)control.Value);
                        int 未知暗点副本价格 = num2;
                        int num1317 = (Config.未知暗点副本价格 = 未知暗点副本价格);
                        break;
                    }
                case nameof(S_未知暗点副本等级):
                    {
                        int num2 = (Settings.Default.未知暗点副本等级 = (int)control.Value);
                        int 未知暗点副本等级 = num2;
                        int num1315 = (Config.未知暗点副本等级 = 未知暗点副本等级);
                        break;
                    }
                case nameof(S_未知暗点二层价格):
                    {
                        int num2 = (Settings.Default.未知暗点二层价格 = (int)control.Value);
                        int 未知暗点二层价格 = num2;
                        int num1313 = (Config.未知暗点二层价格 = 未知暗点二层价格);
                        break;
                    }
                case nameof(S_未知暗点二层等级):
                    {
                        int num2 = (Settings.Default.未知暗点二层等级 = (int)control.Value);
                        int 未知暗点二层等级 = num2;
                        int num1311 = (Config.未知暗点二层等级 = 未知暗点二层等级);
                        break;
                    }
                case nameof(S_幽冥海副本价格):
                    {
                        int num2 = (Settings.Default.幽冥海副本价格 = (int)control.Value);
                        int 幽冥海副本价格 = num2;
                        int num1309 = (Config.幽冥海副本价格 = 幽冥海副本价格);
                        break;
                    }
                case nameof(S_幽冥海副本等级):
                    {
                        int num2 = (Settings.Default.幽冥海副本等级 = (int)control.Value);
                        int 幽冥海副本等级 = num2;
                        int num1307 = (Config.幽冥海副本等级 = 幽冥海副本等级);
                        break;
                    }
                case nameof(S_猎魔暗使称号六):
                    {
                        byte b2 = (Settings.Default.猎魔暗使称号六 = (byte)control.Value);
                        byte 猎魔暗使称号六 = b2;
                        byte b35 = (Config.猎魔暗使称号六 = 猎魔暗使称号六);
                        break;
                    }
                case nameof(S_猎魔暗使材料六):
                    {
                        int num2 = (Settings.Default.猎魔暗使材料六 = (int)control.Value);
                        int 猎魔暗使材料六 = num2;
                        int num1305 = (Config.猎魔暗使材料六 = 猎魔暗使材料六);
                        break;
                    }
                case nameof(S_猎魔暗使数量六):
                    {
                        int num2 = (Settings.Default.猎魔暗使数量六 = (int)control.Value);
                        int 猎魔暗使数量六 = num2;
                        int num1303 = (Config.猎魔暗使数量六 = 猎魔暗使数量六);
                        break;
                    }
                case nameof(S_猎魔暗使称号五):
                    {
                        byte b2 = (Settings.Default.猎魔暗使称号五 = (byte)control.Value);
                        byte 猎魔暗使称号五 = b2;
                        byte b33 = (Config.猎魔暗使称号五 = 猎魔暗使称号五);
                        break;
                    }
                case nameof(S_猎魔暗使材料五):
                    {
                        int num2 = (Settings.Default.猎魔暗使材料五 = (int)control.Value);
                        int 猎魔暗使材料五 = num2;
                        int num1301 = (Config.猎魔暗使材料五 = 猎魔暗使材料五);
                        break;
                    }
                case nameof(S_猎魔暗使数量五):
                    {
                        int num2 = (Settings.Default.猎魔暗使数量五 = (int)control.Value);
                        int 猎魔暗使数量五 = num2;
                        int num1299 = (Config.猎魔暗使数量五 = 猎魔暗使数量五);
                        break;
                    }
                case nameof(S_猎魔暗使称号四):
                    {
                        byte b2 = (Settings.Default.猎魔暗使称号四 = (byte)control.Value);
                        byte 猎魔暗使称号四 = b2;
                        byte b31 = (Config.猎魔暗使称号四 = 猎魔暗使称号四);
                        break;
                    }
                case nameof(S_猎魔暗使材料四):
                    {
                        int num2 = (Settings.Default.猎魔暗使材料四 = (int)control.Value);
                        int 猎魔暗使材料四 = num2;
                        int num1297 = (Config.猎魔暗使材料四 = 猎魔暗使材料四);
                        break;
                    }
                case nameof(S_猎魔暗使数量四):
                    {
                        int num2 = (Settings.Default.猎魔暗使数量四 = (int)control.Value);
                        int 猎魔暗使数量四 = num2;
                        int num1295 = (Config.猎魔暗使数量四 = 猎魔暗使数量四);
                        break;
                    }
                case nameof(S_猎魔暗使称号三):
                    {
                        byte b2 = (Settings.Default.猎魔暗使称号三 = (byte)control.Value);
                        byte 猎魔暗使称号三 = b2;
                        byte b29 = (Config.猎魔暗使称号三 = 猎魔暗使称号三);
                        break;
                    }
                case nameof(S_猎魔暗使材料三):
                    {
                        int num2 = (Settings.Default.猎魔暗使材料三 = (int)control.Value);
                        int 猎魔暗使材料三 = num2;
                        int num1293 = (Config.猎魔暗使材料三 = 猎魔暗使材料三);
                        break;
                    }
                case nameof(S_猎魔暗使数量三):
                    {
                        int num2 = (Settings.Default.猎魔暗使数量三 = (int)control.Value);
                        int 猎魔暗使数量三 = num2;
                        int num1291 = (Config.猎魔暗使数量三 = 猎魔暗使数量三);
                        break;
                    }
                case nameof(S_猎魔暗使称号二):
                    {
                        byte b2 = (Settings.Default.猎魔暗使称号二 = (byte)control.Value);
                        byte 猎魔暗使称号二 = b2;
                        byte b27 = (Config.猎魔暗使称号二 = 猎魔暗使称号二);
                        break;
                    }
                case nameof(S_猎魔暗使材料二):
                    {
                        int num2 = (Settings.Default.猎魔暗使材料二 = (int)control.Value);
                        int 猎魔暗使材料二 = num2;
                        int num1289 = (Config.猎魔暗使材料二 = 猎魔暗使材料二);
                        break;
                    }
                case nameof(S_猎魔暗使数量二):
                    {
                        int num2 = (Settings.Default.猎魔暗使数量二 = (int)control.Value);
                        int 猎魔暗使数量二 = num2;
                        int num1287 = (Config.猎魔暗使数量二 = 猎魔暗使数量二);
                        break;
                    }
                case nameof(S_猎魔暗使称号一):
                    {
                        byte b2 = (Settings.Default.猎魔暗使称号一 = (byte)control.Value);
                        byte 猎魔暗使称号一 = b2;
                        byte b25 = (Config.猎魔暗使称号一 = 猎魔暗使称号一);
                        break;
                    }
                case nameof(S_猎魔暗使材料一):
                    {
                        int num2 = (Settings.Default.猎魔暗使材料一 = (int)control.Value);
                        int 猎魔暗使材料一 = num2;
                        int num1285 = (Config.猎魔暗使材料一 = 猎魔暗使材料一);
                        break;
                    }
                case nameof(S_猎魔暗使数量一):
                    {
                        int num2 = (Settings.Default.猎魔暗使数量一 = (int)control.Value);
                        int 猎魔暗使数量一 = num2;
                        int num1283 = (Config.猎魔暗使数量一 = 猎魔暗使数量一);
                        break;
                    }
                case nameof(S_怪物掉落广播开关):
                    {
                        int num2 = (Settings.Default.怪物掉落广播开关 = (int)control.Value);
                        int 怪物掉落广播开关 = num2;
                        int num1281 = (Config.怪物掉落广播开关 = 怪物掉落广播开关);
                        break;
                    }
                case nameof(S_怪物掉落窗口开关):
                    {
                        int num2 = (Settings.Default.怪物掉落窗口开关 = (int)control.Value);
                        int 怪物掉落窗口开关 = num2;
                        int num1279 = (Config.怪物掉落窗口开关 = 怪物掉落窗口开关);
                        break;
                    }
                case nameof(S_珍宝阁提示开关):
                    {
                        int num2 = (Settings.Default.珍宝阁提示开关 = (int)control.Value);
                        int 珍宝阁提示开关 = num2;
                        int num1277 = (Config.珍宝阁提示开关 = 珍宝阁提示开关);
                        break;
                    }
                case nameof(S_祖玛分解几率一):
                    {
                        int num2 = (Settings.Default.祖玛分解几率一 = (int)control.Value);
                        int 祖玛分解几率一 = num2;
                        int num1275 = (Config.祖玛分解几率一 = 祖玛分解几率一);
                        break;
                    }
                case nameof(S_祖玛分解几率二):
                    {
                        int num2 = (Settings.Default.祖玛分解几率二 = (int)control.Value);
                        int 祖玛分解几率二 = num2;
                        int num1273 = (Config.祖玛分解几率二 = 祖玛分解几率二);
                        break;
                    }
                case nameof(S_祖玛分解几率三):
                    {
                        int num2 = (Settings.Default.祖玛分解几率三 = (int)control.Value);
                        int 祖玛分解几率三 = num2;
                        int num1271 = (Config.祖玛分解几率三 = 祖玛分解几率三);
                        break;
                    }
                case nameof(S_祖玛分解几率四):
                    {
                        int num2 = (Settings.Default.祖玛分解几率四 = (int)control.Value);
                        int 祖玛分解几率四 = num2;
                        int num1269 = (Config.祖玛分解几率四 = 祖玛分解几率四);
                        break;
                    }
                case nameof(S_祖玛分解数量一):
                    {
                        int num2 = (Settings.Default.祖玛分解数量一 = (int)control.Value);
                        int 祖玛分解数量一 = num2;
                        int num1267 = (Config.祖玛分解数量一 = 祖玛分解数量一);
                        break;
                    }
                case nameof(S_祖玛分解数量二):
                    {
                        int num2 = (Settings.Default.祖玛分解数量二 = (int)control.Value);
                        int 祖玛分解数量二 = num2;
                        int num1265 = (Config.祖玛分解数量二 = 祖玛分解数量二);
                        break;
                    }
                case nameof(S_祖玛分解数量三):
                    {
                        int num2 = (Settings.Default.祖玛分解数量三 = (int)control.Value);
                        int 祖玛分解数量三 = num2;
                        int num1263 = (Config.祖玛分解数量三 = 祖玛分解数量三);
                        break;
                    }
                case nameof(S_祖玛分解数量四):
                    {
                        int num2 = (Settings.Default.祖玛分解数量四 = (int)control.Value);
                        int 祖玛分解数量四 = num2;
                        int num1261 = (Config.祖玛分解数量四 = 祖玛分解数量四);
                        break;
                    }
                case nameof(S_祖玛分解开关):
                    {
                        int num2 = (Settings.Default.祖玛分解开关 = (int)control.Value);
                        int 祖玛分解开关 = num2;
                        int num1259 = (Config.祖玛分解开关 = 祖玛分解开关);
                        break;
                    }
                case nameof(S_赤月分解几率一):
                    {
                        int num2 = (Settings.Default.赤月分解几率一 = (int)control.Value);
                        int 赤月分解几率一 = num2;
                        int num1257 = (Config.赤月分解几率一 = 赤月分解几率一);
                        break;
                    }
                case nameof(S_赤月分解几率二):
                    {
                        int num2 = (Settings.Default.赤月分解几率二 = (int)control.Value);
                        int 赤月分解几率二 = num2;
                        int num1255 = (Config.赤月分解几率二 = 赤月分解几率二);
                        break;
                    }
                case nameof(S_赤月分解几率三):
                    {
                        int num2 = (Settings.Default.赤月分解几率三 = (int)control.Value);
                        int 赤月分解几率三 = num2;
                        int num1253 = (Config.赤月分解几率三 = 赤月分解几率三);
                        break;
                    }
                case nameof(S_赤月分解几率四):
                    {
                        int num2 = (Settings.Default.赤月分解几率四 = (int)control.Value);
                        int 赤月分解几率四 = num2;
                        int num1251 = (Config.赤月分解几率四 = 赤月分解几率四);
                        break;
                    }
                case nameof(S_赤月分解数量一):
                    {
                        int num2 = (Settings.Default.赤月分解数量一 = (int)control.Value);
                        int 赤月分解数量一 = num2;
                        int num1249 = (Config.赤月分解数量一 = 赤月分解数量一);
                        break;
                    }
                case nameof(S_赤月分解数量二):
                    {
                        int num2 = (Settings.Default.赤月分解数量二 = (int)control.Value);
                        int 赤月分解数量二 = num2;
                        int num1247 = (Config.赤月分解数量二 = 赤月分解数量二);
                        break;
                    }
                case nameof(S_赤月分解数量三):
                    {
                        int num2 = (Settings.Default.赤月分解数量三 = (int)control.Value);
                        int 赤月分解数量三 = num2;
                        int num1245 = (Config.赤月分解数量三 = 赤月分解数量三);
                        break;
                    }
                case nameof(S_赤月分解数量四):
                    {
                        int num2 = (Settings.Default.赤月分解数量四 = (int)control.Value);
                        int 赤月分解数量四 = num2;
                        int num1243 = (Config.赤月分解数量四 = 赤月分解数量四);
                        break;
                    }
                case nameof(S_赤月分解开关):
                    {
                        int num2 = (Settings.Default.赤月分解开关 = (int)control.Value);
                        int 赤月分解开关 = num2;
                        int num1241 = (Config.赤月分解开关 = 赤月分解开关);
                        break;
                    }
                case nameof(S_魔龙分解几率一):
                    {
                        int num2 = (Settings.Default.魔龙分解几率一 = (int)control.Value);
                        int 魔龙分解几率一 = num2;
                        int num1239 = (Config.魔龙分解几率一 = 魔龙分解几率一);
                        break;
                    }
                case nameof(S_魔龙分解几率二):
                    {
                        int num2 = (Settings.Default.魔龙分解几率二 = (int)control.Value);
                        int 魔龙分解几率二 = num2;
                        int num1237 = (Config.魔龙分解几率二 = 魔龙分解几率二);
                        break;
                    }
                case nameof(S_魔龙分解几率三):
                    {
                        int num2 = (Settings.Default.魔龙分解几率三 = (int)control.Value);
                        int 魔龙分解几率三 = num2;
                        int num1235 = (Config.魔龙分解几率三 = 魔龙分解几率三);
                        break;
                    }
                case nameof(S_魔龙分解几率四):
                    {
                        int num2 = (Settings.Default.魔龙分解几率四 = (int)control.Value);
                        int 魔龙分解几率四 = num2;
                        int num1233 = (Config.魔龙分解几率四 = 魔龙分解几率四);
                        break;
                    }
                case nameof(S_魔龙分解数量一):
                    {
                        int num2 = (Settings.Default.魔龙分解数量一 = (int)control.Value);
                        int 魔龙分解数量一 = num2;
                        int num1231 = (Config.魔龙分解数量一 = 魔龙分解数量一);
                        break;
                    }
                case nameof(S_魔龙分解数量二):
                    {
                        int num2 = (Settings.Default.魔龙分解数量二 = (int)control.Value);
                        int 魔龙分解数量二 = num2;
                        int num1229 = (Config.魔龙分解数量二 = 魔龙分解数量二);
                        break;
                    }
                case nameof(S_魔龙分解数量三):
                    {
                        int num2 = (Settings.Default.魔龙分解数量三 = (int)control.Value);
                        int 魔龙分解数量三 = num2;
                        int num1227 = (Config.魔龙分解数量三 = 魔龙分解数量三);
                        break;
                    }
                case nameof(S_魔龙分解数量四):
                    {
                        int num2 = (Settings.Default.魔龙分解数量四 = (int)control.Value);
                        int 魔龙分解数量四 = num2;
                        int num1225 = (Config.魔龙分解数量四 = 魔龙分解数量四);
                        break;
                    }
                case nameof(S_魔龙分解开关):
                    {
                        int num2 = (Settings.Default.魔龙分解开关 = (int)control.Value);
                        int 魔龙分解开关 = num2;
                        int num1223 = (Config.魔龙分解开关 = 魔龙分解开关);
                        break;
                    }
                case nameof(S_苍月分解几率一):
                    {
                        int num2 = (Settings.Default.苍月分解几率一 = (int)control.Value);
                        int 苍月分解几率一 = num2;
                        int num1221 = (Config.苍月分解几率一 = 苍月分解几率一);
                        break;
                    }
                case nameof(S_苍月分解几率二):
                    {
                        int num2 = (Settings.Default.苍月分解几率二 = (int)control.Value);
                        int 苍月分解几率二 = num2;
                        int num1219 = (Config.苍月分解几率二 = 苍月分解几率二);
                        break;
                    }
                case nameof(S_苍月分解几率三):
                    {
                        int num2 = (Settings.Default.苍月分解几率三 = (int)control.Value);
                        int 苍月分解几率三 = num2;
                        int num1217 = (Config.苍月分解几率三 = 苍月分解几率三);
                        break;
                    }
                case nameof(S_苍月分解几率四):
                    {
                        int num2 = (Settings.Default.苍月分解几率四 = (int)control.Value);
                        int 苍月分解几率四 = num2;
                        int num1215 = (Config.苍月分解几率四 = 苍月分解几率四);
                        break;
                    }
                case nameof(S_苍月分解数量一):
                    {
                        int num2 = (Settings.Default.苍月分解数量一 = (int)control.Value);
                        int 苍月分解数量一 = num2;
                        int num1213 = (Config.苍月分解数量一 = 苍月分解数量一);
                        break;
                    }
                case nameof(S_苍月分解数量二):
                    {
                        int num2 = (Settings.Default.苍月分解数量二 = (int)control.Value);
                        int 苍月分解数量二 = num2;
                        int num1211 = (Config.苍月分解数量二 = 苍月分解数量二);
                        break;
                    }
                case nameof(S_苍月分解数量三):
                    {
                        int num2 = (Settings.Default.苍月分解数量三 = (int)control.Value);
                        int 苍月分解数量三 = num2;
                        int num1209 = (Config.苍月分解数量三 = 苍月分解数量三);
                        break;
                    }
                case nameof(S_苍月分解数量四):
                    {
                        int num2 = (Settings.Default.苍月分解数量四 = (int)control.Value);
                        int 苍月分解数量四 = num2;
                        int num1207 = (Config.苍月分解数量四 = 苍月分解数量四);
                        break;
                    }
                case nameof(S_苍月分解开关):
                    {
                        int num2 = (Settings.Default.苍月分解开关 = (int)control.Value);
                        int 苍月分解开关 = num2;
                        int num1205 = (Config.苍月分解开关 = 苍月分解开关);
                        break;
                    }
                case nameof(S_星王分解几率一):
                    {
                        int num2 = (Settings.Default.星王分解几率一 = (int)control.Value);
                        int 星王分解几率一 = num2;
                        int num1203 = (Config.星王分解几率一 = 星王分解几率一);
                        break;
                    }
                case nameof(S_星王分解几率二):
                    {
                        int num2 = (Settings.Default.星王分解几率二 = (int)control.Value);
                        int 星王分解几率二 = num2;
                        int num1201 = (Config.星王分解几率二 = 星王分解几率二);
                        break;
                    }
                case nameof(S_星王分解几率三):
                    {
                        int num2 = (Settings.Default.星王分解几率三 = (int)control.Value);
                        int 星王分解几率三 = num2;
                        int num1199 = (Config.星王分解几率三 = 星王分解几率三);
                        break;
                    }
                case nameof(S_星王分解几率四):
                    {
                        int num2 = (Settings.Default.星王分解几率四 = (int)control.Value);
                        int 星王分解几率四 = num2;
                        int num1197 = (Config.星王分解几率四 = 星王分解几率四);
                        break;
                    }
                case nameof(S_星王分解数量一):
                    {
                        int num2 = (Settings.Default.星王分解数量一 = (int)control.Value);
                        int 星王分解数量一 = num2;
                        int num1195 = (Config.星王分解数量一 = 星王分解数量一);
                        break;
                    }
                case nameof(S_星王分解数量二):
                    {
                        int num2 = (Settings.Default.星王分解数量二 = (int)control.Value);
                        int 星王分解数量二 = num2;
                        int num1193 = (Config.星王分解数量二 = 星王分解数量二);
                        break;
                    }
                case nameof(S_星王分解数量三):
                    {
                        int num2 = (Settings.Default.星王分解数量三 = (int)control.Value);
                        int 星王分解数量三 = num2;
                        int num1191 = (Config.星王分解数量三 = 星王分解数量三);
                        break;
                    }
                case nameof(S_星王分解数量四):
                    {
                        int num2 = (Settings.Default.星王分解数量四 = (int)control.Value);
                        int 星王分解数量四 = num2;
                        int num1189 = (Config.星王分解数量四 = 星王分解数量四);
                        break;
                    }
                case nameof(S_星王分解开关):
                    {
                        int num2 = (Settings.Default.星王分解开关 = (int)control.Value);
                        int 星王分解开关 = num2;
                        int num1187 = (Config.星王分解开关 = 星王分解开关);
                        break;
                    }
                case nameof(S_城主分解几率一):
                    {
                        int num2 = (Settings.Default.城主分解几率一 = (int)control.Value);
                        int 城主分解几率一 = num2;
                        int num1185 = (Config.城主分解几率一 = 城主分解几率一);
                        break;
                    }
                case nameof(S_城主分解几率二):
                    {
                        int num2 = (Settings.Default.城主分解几率二 = (int)control.Value);
                        int 城主分解几率二 = num2;
                        int num1183 = (Config.城主分解几率二 = 城主分解几率二);
                        break;
                    }
                case nameof(S_城主分解几率三):
                    {
                        int num2 = (Settings.Default.城主分解几率三 = (int)control.Value);
                        int 城主分解几率三 = num2;
                        int num1181 = (Config.城主分解几率三 = 城主分解几率三);
                        break;
                    }
                case nameof(S_城主分解几率四):
                    {
                        int num2 = (Settings.Default.城主分解几率四 = (int)control.Value);
                        int 城主分解几率四 = num2;
                        int num1179 = (Config.城主分解几率四 = 城主分解几率四);
                        break;
                    }
                case nameof(S_城主分解数量一):
                    {
                        int num2 = (Settings.Default.城主分解数量一 = (int)control.Value);
                        int 城主分解数量一 = num2;
                        int num1177 = (Config.城主分解数量一 = 城主分解数量一);
                        break;
                    }
                case nameof(S_城主分解数量二):
                    {
                        int num2 = (Settings.Default.城主分解数量二 = (int)control.Value);
                        int 城主分解数量二 = num2;
                        int num1175 = (Config.城主分解数量二 = 城主分解数量二);
                        break;
                    }
                case nameof(S_城主分解数量三):
                    {
                        int num2 = (Settings.Default.城主分解数量三 = (int)control.Value);
                        int 城主分解数量三 = num2;
                        int num1173 = (Config.城主分解数量三 = 城主分解数量三);
                        break;
                    }
                case nameof(S_城主分解数量四):
                    {
                        int num2 = (Settings.Default.城主分解数量四 = (int)control.Value);
                        int 城主分解数量四 = num2;
                        int num1171 = (Config.城主分解数量四 = 城主分解数量四);
                        break;
                    }
                case nameof(S_城主分解开关):
                    {
                        int num2 = (Settings.Default.城主分解开关 = (int)control.Value);
                        int 城主分解开关 = num2;
                        int num1169 = (Config.城主分解开关 = 城主分解开关);
                        break;
                    }
                case nameof(S_世界BOSS时间):
                    {
                        byte b2 = (Settings.Default.世界BOSS时间 = (byte)control.Value);
                        byte 世界BOSS时间 = b2;
                        byte b23 = (Config.世界BOSS时间 = 世界BOSS时间);
                        break;
                    }
                case nameof(S_世界BOSS分钟):
                    {
                        byte b2 = (Settings.Default.世界BOSS分钟 = (byte)control.Value);
                        byte 世界BOSS分钟 = b2;
                        byte b21 = (Config.世界BOSS分钟 = 世界BOSS分钟);
                        break;
                    }
                case nameof(S_秘宝广场元宝):
                    {
                        int num2 = (Settings.Default.秘宝广场元宝 = (int)control.Value);
                        int 秘宝广场元宝 = num2;
                        int num1167 = (Config.秘宝广场元宝 = 秘宝广场元宝);
                        break;
                    }
                case nameof(S_每周特惠礼包一元宝):
                    {
                        int num2 = (Settings.Default.每周特惠礼包一元宝 = (int)control.Value);
                        int 每周特惠礼包一元宝 = num2;
                        int num1165 = (Config.每周特惠礼包一元宝 = 每周特惠礼包一元宝);
                        break;
                    }
                case nameof(S_每周特惠礼包二元宝):
                    {
                        int num2 = (Settings.Default.每周特惠礼包二元宝 = (int)control.Value);
                        int 每周特惠礼包二元宝 = num2;
                        int num1163 = (Config.每周特惠礼包二元宝 = 每周特惠礼包二元宝);
                        break;
                    }
                case nameof(S_特权玛法名俊元宝):
                    {
                        int num2 = (Settings.Default.特权玛法名俊元宝 = (int)control.Value);
                        int 特权玛法名俊元宝2 = num2;
                        int num1161 = (Config.特权玛法名俊元宝 = 特权玛法名俊元宝2);
                        break;
                    }
                case nameof(S_特权玛法豪杰元宝):
                    {
                        int num2 = (Settings.Default.特权玛法豪杰元宝 = (int)control.Value);
                        int 特权玛法名俊元宝 = num2;
                        int num1159 = (Config.特权玛法名俊元宝 = 特权玛法名俊元宝);
                        break;
                    }
                case nameof(S_特权玛法战将元宝):
                    {
                        int num2 = (Settings.Default.特权玛法战将元宝 = (int)control.Value);
                        int 特权玛法战将元宝 = num2;
                        int num1157 = (Config.特权玛法战将元宝 = 特权玛法战将元宝);
                        break;
                    }
                case nameof(S_御兽切换开关):
                    {
                        int num2 = (Settings.Default.御兽切换开关 = (int)control.Value);
                        int 御兽切换开关 = num2;
                        int num1155 = (Config.御兽切换开关 = 御兽切换开关);
                        break;
                    }
                case nameof(S_BOSS卷轴地图编号):
                    {
                        int num2 = (Settings.Default.BOSS卷轴地图编号 = (int)control.Value);
                        int bOSS卷轴地图编号 = num2;
                        int num1153 = (Config.BOSS卷轴地图编号 = bOSS卷轴地图编号);
                        break;
                    }
                case nameof(S_BOSS卷轴地图开关):
                    {
                        int num2 = (Settings.Default.BOSS卷轴地图开关 = (int)control.Value);
                        int bOSS卷轴地图开关 = num2;
                        int num1151 = (Config.BOSS卷轴地图开关 = bOSS卷轴地图开关);
                        break;
                    }
                case nameof(S_沙巴克重置系统):
                    {
                        int num2 = (Settings.Default.沙巴克重置系统 = (int)control.Value);
                        int 沙巴克重置系统 = num2;
                        int num1149 = (Config.沙巴克重置系统 = 沙巴克重置系统);
                        break;
                    }
                case nameof(S_资源包开关):
                    {
                        int num2 = (Settings.Default.资源包开关 = (int)control.Value);
                        int 资源包开关 = num2;
                        int num1147 = (Config.资源包开关 = 资源包开关);
                        break;
                    }
                case nameof(S_StartingLevel):
                    {
                        byte b2 = (Settings.Default.StartingLevel = (byte)control.Value);
                        byte 新手出生等级 = b2;
                        byte b19 = (Config.StartingLevel = 新手出生等级);
                        break;
                    }
                case nameof(S_MaxUserConnections):
                    {
                        int num2 = (Settings.Default.MaxUserConnections = (int)control.Value);
                        int 在线人数控制 = num2;
                        int num1145 = (Config.MaxUserConnections = 在线人数控制);
                        break;
                    }
                case nameof(S_掉落贵重物品颜色):
                    {
                        int num2 = (Settings.Default.掉落贵重物品颜色 = (int)control.Value);
                        int 掉落贵重物品颜色 = num2;
                        int num1143 = (Config.掉落贵重物品颜色 = 掉落贵重物品颜色);
                        break;
                    }
                case nameof(S_掉落沃玛物品颜色):
                    {
                        int num2 = (Settings.Default.掉落沃玛物品颜色 = (int)control.Value);
                        int 掉落沃玛物品颜色 = num2;
                        int num1141 = (Config.掉落沃玛物品颜色 = 掉落沃玛物品颜色);
                        break;
                    }
                case nameof(S_掉落祖玛物品颜色):
                    {
                        int num2 = (Settings.Default.掉落祖玛物品颜色 = (int)control.Value);
                        int 掉落祖玛物品颜色 = num2;
                        int num1139 = (Config.掉落祖玛物品颜色 = 掉落祖玛物品颜色);
                        break;
                    }
                case nameof(S_掉落赤月物品颜色):
                    {
                        int num2 = (Settings.Default.掉落赤月物品颜色 = (int)control.Value);
                        int 掉落赤月物品颜色 = num2;
                        int num1137 = (Config.掉落赤月物品颜色 = 掉落赤月物品颜色);
                        break;
                    }
                case nameof(S_掉落魔龙物品颜色):
                    {
                        int num2 = (Settings.Default.掉落魔龙物品颜色 = (int)control.Value);
                        int 掉落魔龙物品颜色 = num2;
                        int num1135 = (Config.掉落魔龙物品颜色 = 掉落魔龙物品颜色);
                        break;
                    }
                case nameof(S_掉落苍月物品颜色):
                    {
                        int num2 = (Settings.Default.掉落苍月物品颜色 = (int)control.Value);
                        int 掉落苍月物品颜色 = num2;
                        int num1133 = (Config.掉落苍月物品颜色 = 掉落苍月物品颜色);
                        break;
                    }
                case nameof(S_掉落星王物品颜色):
                    {
                        int num2 = (Settings.Default.掉落星王物品颜色 = (int)control.Value);
                        int 掉落星王物品颜色 = num2;
                        int num1131 = (Config.掉落星王物品颜色 = 掉落星王物品颜色);
                        break;
                    }
                case nameof(S_掉落城主物品颜色):
                    {
                        int num2 = (Settings.Default.掉落城主物品颜色 = (int)control.Value);
                        int 掉落城主物品颜色 = num2;
                        int num1129 = (Config.掉落城主物品颜色 = 掉落城主物品颜色);
                        break;
                    }
                case nameof(S_掉落书籍物品颜色):
                    {
                        int num2 = (Settings.Default.掉落书籍物品颜色 = (int)control.Value);
                        int 掉落书籍物品颜色 = num2;
                        int num1127 = (Config.掉落书籍物品颜色 = 掉落书籍物品颜色);
                        break;
                    }
                case nameof(S_DropPlayerNameColor):
                    {
                        Config.DropPlayerNameColor = Settings.Default.DropPlayerNameColor = (int)control.Value;
                        break;
                    }
                case nameof(S_狂暴击杀玩家颜色):
                    {
                        int num2 = (Settings.Default.狂暴击杀玩家颜色 = (int)control.Value);
                        int 狂暴击杀玩家颜色 = num2;
                        int num1123 = (Config.狂暴击杀玩家颜色 = 狂暴击杀玩家颜色);
                        break;
                    }
                case nameof(S_狂暴被杀玩家颜色):
                    {
                        int num2 = (Settings.Default.狂暴被杀玩家颜色 = (int)control.Value);
                        int 狂暴被杀玩家颜色 = num2;
                        int num1121 = (Config.狂暴被杀玩家颜色 = 狂暴被杀玩家颜色);
                        break;
                    }
                case nameof(S_祖玛战装备佩戴数量):
                    {
                        int num2 = (Settings.Default.祖玛战装备佩戴数量 = (int)control.Value);
                        int 祖玛战装备佩戴数量 = num2;
                        int num1119 = (Config.祖玛战装备佩戴数量 = 祖玛战装备佩戴数量);
                        break;
                    }
                case nameof(S_祖玛法装备佩戴数量):
                    {
                        int num2 = (Settings.Default.祖玛法装备佩戴数量 = (int)control.Value);
                        int 祖玛法装备佩戴数量 = num2;
                        int num1117 = (Config.祖玛法装备佩戴数量 = 祖玛法装备佩戴数量);
                        break;
                    }
                case nameof(S_祖玛道装备佩戴数量):
                    {
                        int num2 = (Settings.Default.祖玛道装备佩戴数量 = (int)control.Value);
                        int 祖玛道装备佩戴数量 = num2;
                        int num1115 = (Config.祖玛道装备佩戴数量 = 祖玛道装备佩戴数量);
                        break;
                    }
                case nameof(S_祖玛刺装备佩戴数量):
                    {
                        int num2 = (Settings.Default.祖玛刺装备佩戴数量 = (int)control.Value);
                        int 祖玛刺装备佩戴数量 = num2;
                        int num1113 = (Config.祖玛刺装备佩戴数量 = 祖玛刺装备佩戴数量);
                        break;
                    }
                case nameof(S_祖玛弓装备佩戴数量):
                    {
                        int num2 = (Settings.Default.祖玛弓装备佩戴数量 = (int)control.Value);
                        int 祖玛弓装备佩戴数量 = num2;
                        int num1111 = (Config.祖玛弓装备佩戴数量 = 祖玛弓装备佩戴数量);
                        break;
                    }
                case nameof(S_祖玛枪装备佩戴数量):
                    {
                        int num2 = (Settings.Default.祖玛枪装备佩戴数量 = (int)control.Value);
                        int 祖玛枪装备佩戴数量 = num2;
                        int num1109 = (Config.祖玛枪装备佩戴数量 = 祖玛枪装备佩戴数量);
                        break;
                    }
                case nameof(S_赤月战装备佩戴数量):
                    {
                        int num2 = (Settings.Default.赤月战装备佩戴数量 = (int)control.Value);
                        int 赤月战装备佩戴数量 = num2;
                        int num1107 = (Config.赤月战装备佩戴数量 = 赤月战装备佩戴数量);
                        break;
                    }
                case nameof(S_赤月法装备佩戴数量):
                    {
                        int num2 = (Settings.Default.赤月法装备佩戴数量 = (int)control.Value);
                        int 赤月法装备佩戴数量 = num2;
                        int num1105 = (Config.赤月法装备佩戴数量 = 赤月法装备佩戴数量);
                        break;
                    }
                case nameof(S_赤月道装备佩戴数量):
                    {
                        int num2 = (Settings.Default.赤月道装备佩戴数量 = (int)control.Value);
                        int 赤月道装备佩戴数量 = num2;
                        int num1103 = (Config.赤月道装备佩戴数量 = 赤月道装备佩戴数量);
                        break;
                    }
                case nameof(S_赤月刺装备佩戴数量):
                    {
                        int num2 = (Settings.Default.赤月刺装备佩戴数量 = (int)control.Value);
                        int 赤月刺装备佩戴数量 = num2;
                        int num1101 = (Config.赤月刺装备佩戴数量 = 赤月刺装备佩戴数量);
                        break;
                    }
                case nameof(S_赤月弓装备佩戴数量):
                    {
                        int num2 = (Settings.Default.赤月弓装备佩戴数量 = (int)control.Value);
                        int 赤月弓装备佩戴数量 = num2;
                        int num1099 = (Config.赤月弓装备佩戴数量 = 赤月弓装备佩戴数量);
                        break;
                    }
                case nameof(S_赤月枪装备佩戴数量):
                    {
                        int num2 = (Settings.Default.赤月枪装备佩戴数量 = (int)control.Value);
                        int 赤月枪装备佩戴数量 = num2;
                        int num1097 = (Config.赤月枪装备佩戴数量 = 赤月枪装备佩戴数量);
                        break;
                    }
                case nameof(S_魔龙战装备佩戴数量):
                    {
                        int num2 = (Settings.Default.魔龙战装备佩戴数量 = (int)control.Value);
                        int 魔龙战装备佩戴数量 = num2;
                        int num1095 = (Config.魔龙战装备佩戴数量 = 魔龙战装备佩戴数量);
                        break;
                    }
                case nameof(S_魔龙法装备佩戴数量):
                    {
                        int num2 = (Settings.Default.魔龙法装备佩戴数量 = (int)control.Value);
                        int 魔龙法装备佩戴数量 = num2;
                        int num1093 = (Config.魔龙法装备佩戴数量 = 魔龙法装备佩戴数量);
                        break;
                    }
                case nameof(S_魔龙道装备佩戴数量):
                    {
                        int num2 = (Settings.Default.魔龙道装备佩戴数量 = (int)control.Value);
                        int 魔龙道装备佩戴数量 = num2;
                        int num1091 = (Config.魔龙道装备佩戴数量 = 魔龙道装备佩戴数量);
                        break;
                    }
                case nameof(S_魔龙刺装备佩戴数量):
                    {
                        int num2 = (Settings.Default.魔龙刺装备佩戴数量 = (int)control.Value);
                        int 魔龙刺装备佩戴数量 = num2;
                        int num1089 = (Config.魔龙刺装备佩戴数量 = 魔龙刺装备佩戴数量);
                        break;
                    }
                case nameof(S_魔龙弓装备佩戴数量):
                    {
                        int num2 = (Settings.Default.魔龙弓装备佩戴数量 = (int)control.Value);
                        int 魔龙弓装备佩戴数量 = num2;
                        int num1087 = (Config.魔龙弓装备佩戴数量 = 魔龙弓装备佩戴数量);
                        break;
                    }
                case nameof(S_魔龙枪装备佩戴数量):
                    {
                        int num2 = (Settings.Default.魔龙枪装备佩戴数量 = (int)control.Value);
                        int 魔龙枪装备佩戴数量 = num2;
                        int num1085 = (Config.魔龙枪装备佩戴数量 = 魔龙枪装备佩戴数量);
                        break;
                    }
                case nameof(S_苍月战装备佩戴数量):
                    {
                        int num2 = (Settings.Default.苍月战装备佩戴数量 = (int)control.Value);
                        int 苍月战装备佩戴数量 = num2;
                        int num1083 = (Config.苍月战装备佩戴数量 = 苍月战装备佩戴数量);
                        break;
                    }
                case nameof(S_苍月法装备佩戴数量):
                    {
                        int num2 = (Settings.Default.苍月法装备佩戴数量 = (int)control.Value);
                        int 苍月法装备佩戴数量 = num2;
                        int num1081 = (Config.苍月法装备佩戴数量 = 苍月法装备佩戴数量);
                        break;
                    }
                case nameof(S_苍月道装备佩戴数量):
                    {
                        int num2 = (Settings.Default.苍月道装备佩戴数量 = (int)control.Value);
                        int 苍月道装备佩戴数量 = num2;
                        int num1079 = (Config.苍月道装备佩戴数量 = 苍月道装备佩戴数量);
                        break;
                    }
                case nameof(S_苍月刺装备佩戴数量):
                    {
                        int num2 = (Settings.Default.苍月刺装备佩戴数量 = (int)control.Value);
                        int 苍月刺装备佩戴数量 = num2;
                        int num1077 = (Config.苍月刺装备佩戴数量 = 苍月刺装备佩戴数量);
                        break;
                    }
                case nameof(S_苍月弓装备佩戴数量):
                    {
                        int num2 = (Settings.Default.苍月弓装备佩戴数量 = (int)control.Value);
                        int 苍月弓装备佩戴数量 = num2;
                        int num1075 = (Config.苍月弓装备佩戴数量 = 苍月弓装备佩戴数量);
                        break;
                    }
                case nameof(S_苍月枪装备佩戴数量):
                    {
                        int num2 = (Settings.Default.苍月枪装备佩戴数量 = (int)control.Value);
                        int 苍月枪装备佩戴数量 = num2;
                        int num1073 = (Config.苍月枪装备佩戴数量 = 苍月枪装备佩戴数量);
                        break;
                    }
                case nameof(S_星王战装备佩戴数量):
                    {
                        int num2 = (Settings.Default.星王战装备佩戴数量 = (int)control.Value);
                        int 星王战装备佩戴数量 = num2;
                        int num1071 = (Config.星王战装备佩戴数量 = 星王战装备佩戴数量);
                        break;
                    }
                case nameof(S_星王法装备佩戴数量):
                    {
                        int num2 = (Settings.Default.星王法装备佩戴数量 = (int)control.Value);
                        int 星王法装备佩戴数量 = num2;
                        int num1069 = (Config.星王法装备佩戴数量 = 星王法装备佩戴数量);
                        break;
                    }
                case nameof(S_星王道装备佩戴数量):
                    {
                        int num2 = (Settings.Default.星王道装备佩戴数量 = (int)control.Value);
                        int 星王道装备佩戴数量 = num2;
                        int num1067 = (Config.星王道装备佩戴数量 = 星王道装备佩戴数量);
                        break;
                    }
                case nameof(S_星王刺装备佩戴数量):
                    {
                        int num2 = (Settings.Default.星王刺装备佩戴数量 = (int)control.Value);
                        int 星王刺装备佩戴数量 = num2;
                        int num1065 = (Config.星王刺装备佩戴数量 = 星王刺装备佩戴数量);
                        break;
                    }
                case nameof(S_星王弓装备佩戴数量):
                    {
                        int num2 = (Settings.Default.星王弓装备佩戴数量 = (int)control.Value);
                        int 星王弓装备佩戴数量 = num2;
                        int num1063 = (Config.星王弓装备佩戴数量 = 星王弓装备佩戴数量);
                        break;
                    }
                case nameof(S_星王枪装备佩戴数量):
                    {
                        int num2 = (Settings.Default.星王枪装备佩戴数量 = (int)control.Value);
                        int 星王枪装备佩戴数量 = num2;
                        int num1061 = (Config.星王枪装备佩戴数量 = 星王枪装备佩戴数量);
                        break;
                    }
                case nameof(S_特殊1战装备佩戴数量):
                    {
                        int num2 = (Settings.Default.特殊1战装备佩戴数量 = (int)control.Value);
                        int 特殊1战装备佩戴数量 = num2;
                        int num1059 = (Config.特殊1战装备佩戴数量 = 特殊1战装备佩戴数量);
                        break;
                    }
                case nameof(S_特殊1法装备佩戴数量):
                    {
                        int num2 = (Settings.Default.特殊1法装备佩戴数量 = (int)control.Value);
                        int 特殊1法装备佩戴数量 = num2;
                        int num1057 = (Config.特殊1法装备佩戴数量 = 特殊1法装备佩戴数量);
                        break;
                    }
                case nameof(S_特殊1道装备佩戴数量):
                    {
                        int num2 = (Settings.Default.特殊1道装备佩戴数量 = (int)control.Value);
                        int 特殊1道装备佩戴数量 = num2;
                        int num1055 = (Config.特殊1道装备佩戴数量 = 特殊1道装备佩戴数量);
                        break;
                    }
                case nameof(S_特殊1刺装备佩戴数量):
                    {
                        int num2 = (Settings.Default.特殊1刺装备佩戴数量 = (int)control.Value);
                        int 特殊1刺装备佩戴数量 = num2;
                        int num1053 = (Config.特殊1刺装备佩戴数量 = 特殊1刺装备佩戴数量);
                        break;
                    }
                case nameof(S_特殊1弓装备佩戴数量):
                    {
                        int num2 = (Settings.Default.特殊1弓装备佩戴数量 = (int)control.Value);
                        int 特殊1弓装备佩戴数量 = num2;
                        int num1051 = (Config.特殊1弓装备佩戴数量 = 特殊1弓装备佩戴数量);
                        break;
                    }
                case nameof(S_特殊1枪装备佩戴数量):
                    {
                        int num2 = (Settings.Default.特殊1枪装备佩戴数量 = (int)control.Value);
                        int 特殊1枪装备佩戴数量 = num2;
                        int num1049 = (Config.特殊1枪装备佩戴数量 = 特殊1枪装备佩戴数量);
                        break;
                    }
                case nameof(S_特殊2战装备佩戴数量):
                    {
                        int num2 = (Settings.Default.特殊2战装备佩戴数量 = (int)control.Value);
                        int 特殊2战装备佩戴数量 = num2;
                        int num1047 = (Config.特殊2战装备佩戴数量 = 特殊2战装备佩戴数量);
                        break;
                    }
                case nameof(S_特殊2法装备佩戴数量):
                    {
                        int num2 = (Settings.Default.特殊2法装备佩戴数量 = (int)control.Value);
                        int 特殊2法装备佩戴数量 = num2;
                        int num1045 = (Config.特殊2法装备佩戴数量 = 特殊2法装备佩戴数量);
                        break;
                    }
                case nameof(S_特殊2道装备佩戴数量):
                    {
                        int num2 = (Settings.Default.特殊2道装备佩戴数量 = (int)control.Value);
                        int 特殊2道装备佩戴数量 = num2;
                        int num1043 = (Config.特殊2道装备佩戴数量 = 特殊2道装备佩戴数量);
                        break;
                    }
                case nameof(S_特殊2刺装备佩戴数量):
                    {
                        int num2 = (Settings.Default.特殊2刺装备佩戴数量 = (int)control.Value);
                        int 特殊2刺装备佩戴数量 = num2;
                        int num1041 = (Config.特殊2刺装备佩戴数量 = 特殊2刺装备佩戴数量);
                        break;
                    }
                case nameof(S_特殊2弓装备佩戴数量):
                    {
                        int num2 = (Settings.Default.特殊2弓装备佩戴数量 = (int)control.Value);
                        int 特殊2弓装备佩戴数量 = num2;
                        int num1039 = (Config.特殊2弓装备佩戴数量 = 特殊2弓装备佩戴数量);
                        break;
                    }
                case nameof(S_特殊2枪装备佩戴数量):
                    {
                        int num2 = (Settings.Default.特殊2枪装备佩戴数量 = (int)control.Value);
                        int 特殊2枪装备佩戴数量 = num2;
                        int num1037 = (Config.特殊2枪装备佩戴数量 = 特殊2枪装备佩戴数量);
                        break;
                    }
                case nameof(S_特殊3战装备佩戴数量):
                    {
                        int num2 = (Settings.Default.特殊3战装备佩戴数量 = (int)control.Value);
                        int 特殊3战装备佩戴数量 = num2;
                        int num1035 = (Config.特殊3战装备佩戴数量 = 特殊3战装备佩戴数量);
                        break;
                    }
                case nameof(S_特殊3法装备佩戴数量):
                    {
                        int num2 = (Settings.Default.特殊3法装备佩戴数量 = (int)control.Value);
                        int 特殊3法装备佩戴数量 = num2;
                        int num1033 = (Config.特殊3法装备佩戴数量 = 特殊3法装备佩戴数量);
                        break;
                    }
                case nameof(S_特殊3道装备佩戴数量):
                    {
                        int num2 = (Settings.Default.特殊3道装备佩戴数量 = (int)control.Value);
                        int 特殊3道装备佩戴数量 = num2;
                        int num1031 = (Config.特殊3道装备佩戴数量 = 特殊3道装备佩戴数量);
                        break;
                    }
                case nameof(S_特殊3刺装备佩戴数量):
                    {
                        int num2 = (Settings.Default.特殊3刺装备佩戴数量 = (int)control.Value);
                        int 特殊3刺装备佩戴数量 = num2;
                        int num1029 = (Config.特殊3刺装备佩戴数量 = 特殊3刺装备佩戴数量);
                        break;
                    }
                case nameof(S_特殊3弓装备佩戴数量):
                    {
                        int num2 = (Settings.Default.特殊3弓装备佩戴数量 = (int)control.Value);
                        int 特殊3弓装备佩戴数量 = num2;
                        int num1027 = (Config.特殊3弓装备佩戴数量 = 特殊3弓装备佩戴数量);
                        break;
                    }
                case nameof(S_特殊3枪装备佩戴数量):
                    {
                        int num2 = (Settings.Default.特殊3枪装备佩戴数量 = (int)control.Value);
                        int 特殊3枪装备佩戴数量 = num2;
                        int num1025 = (Config.特殊3枪装备佩戴数量 = 特殊3枪装备佩戴数量);
                        break;
                    }
                case nameof(S_每周特惠一物品1):
                    {
                        int num2 = (Settings.Default.每周特惠一物品1 = (int)control.Value);
                        int 每周特惠一物品5 = num2;
                        int num1023 = (Config.每周特惠一物品1 = 每周特惠一物品5);
                        break;
                    }
                case nameof(S_每周特惠一物品2):
                    {
                        int num2 = (Settings.Default.每周特惠一物品2 = (int)control.Value);
                        int 每周特惠一物品4 = num2;
                        int num1021 = (Config.每周特惠一物品2 = 每周特惠一物品4);
                        break;
                    }
                case nameof(S_每周特惠一物品3):
                    {
                        int num2 = (Settings.Default.每周特惠一物品3 = (int)control.Value);
                        int 每周特惠一物品3 = num2;
                        int num1019 = (Config.每周特惠一物品3 = 每周特惠一物品3);
                        break;
                    }
                case nameof(S_每周特惠一物品4):
                    {
                        int num2 = (Settings.Default.每周特惠一物品4 = (int)control.Value);
                        int 每周特惠一物品2 = num2;
                        int num1017 = (Config.每周特惠一物品4 = 每周特惠一物品2);
                        break;
                    }
                case nameof(S_每周特惠一物品5):
                    {
                        int num2 = (Settings.Default.每周特惠一物品5 = (int)control.Value);
                        int 每周特惠一物品 = num2;
                        int num1015 = (Config.每周特惠一物品5 = 每周特惠一物品);
                        break;
                    }
                case nameof(S_每周特惠二物品1):
                    {
                        int num2 = (Settings.Default.每周特惠二物品1 = (int)control.Value);
                        int 每周特惠二物品5 = num2;
                        int num1013 = (Config.每周特惠二物品1 = 每周特惠二物品5);
                        break;
                    }
                case nameof(S_每周特惠二物品2):
                    {
                        int num2 = (Settings.Default.每周特惠二物品2 = (int)control.Value);
                        int 每周特惠二物品4 = num2;
                        int num1011 = (Config.每周特惠二物品2 = 每周特惠二物品4);
                        break;
                    }
                case nameof(S_每周特惠二物品3):
                    {
                        int num2 = (Settings.Default.每周特惠二物品3 = (int)control.Value);
                        int 每周特惠二物品3 = num2;
                        int num1009 = (Config.每周特惠二物品3 = 每周特惠二物品3);
                        break;
                    }
                case nameof(S_每周特惠二物品4):
                    {
                        int num2 = (Settings.Default.每周特惠二物品4 = (int)control.Value);
                        int 每周特惠二物品2 = num2;
                        int num1007 = (Config.每周特惠二物品4 = 每周特惠二物品2);
                        break;
                    }
                case nameof(S_每周特惠二物品5):
                    {
                        int num2 = (Settings.Default.每周特惠二物品5 = (int)control.Value);
                        int 每周特惠二物品 = num2;
                        int num1005 = (Config.每周特惠二物品5 = 每周特惠二物品);
                        break;
                    }
                case nameof(S_新手出售货币值):
                    {
                        int num2 = (Settings.Default.新手出售货币值 = (int)control.Value);
                        int 新手出售货币值 = num2;
                        int num1003 = (Config.新手出售货币值 = 新手出售货币值);
                        break;
                    }
                case nameof(S_挂机称号选项):
                    {
                        byte b2 = (Settings.Default.挂机称号选项 = (byte)control.Value);
                        byte 挂机称号选项 = b2;
                        byte b17 = (Config.挂机称号选项 = 挂机称号选项);
                        break;
                    }
                case nameof(S_分解称号选项):
                    {
                        byte b2 = (Settings.Default.分解称号选项 = (byte)control.Value);
                        byte 分解称号选项 = b2;
                        byte b15 = (Config.分解称号选项 = 分解称号选项);
                        break;
                    }
                case nameof(S_法阵卡BUG清理):
                    {
                        int num2 = (Settings.Default.法阵卡BUG清理 = (byte)control.Value);
                        int 法阵卡BUG清理 = num2;
                        int num1001 = (Config.法阵卡BUG清理 = 法阵卡BUG清理);
                        break;
                    }
                case nameof(S_随机宝箱三物品1):
                    {
                        int num2 = (Settings.Default.随机宝箱三物品1 = (int)control.Value);
                        int 随机宝箱三物品8 = num2;
                        int num999 = (Config.随机宝箱三物品1 = 随机宝箱三物品8);
                        break;
                    }
                case nameof(S_随机宝箱三几率1):
                    {
                        int num2 = (Settings.Default.随机宝箱三几率1 = (int)control.Value);
                        int 随机宝箱三几率8 = num2;
                        int num997 = (Config.随机宝箱三几率1 = 随机宝箱三几率8);
                        break;
                    }
                case nameof(S_随机宝箱三物品2):
                    {
                        int num2 = (Settings.Default.随机宝箱三物品2 = (int)control.Value);
                        int 随机宝箱三物品7 = num2;
                        int num995 = (Config.随机宝箱三物品2 = 随机宝箱三物品7);
                        break;
                    }
                case nameof(S_随机宝箱三几率2):
                    {
                        int num2 = (Settings.Default.随机宝箱三几率2 = (int)control.Value);
                        int 随机宝箱三几率7 = num2;
                        int num993 = (Config.随机宝箱三几率2 = 随机宝箱三几率7);
                        break;
                    }
                case nameof(S_随机宝箱三物品3):
                    {
                        int num2 = (Settings.Default.随机宝箱三物品3 = (int)control.Value);
                        int 随机宝箱三物品6 = num2;
                        int num991 = (Config.随机宝箱三物品3 = 随机宝箱三物品6);
                        break;
                    }
                case nameof(S_随机宝箱三几率3):
                    {
                        int num2 = (Settings.Default.随机宝箱三几率3 = (int)control.Value);
                        int 随机宝箱三几率6 = num2;
                        int num989 = (Config.随机宝箱三几率3 = 随机宝箱三几率6);
                        break;
                    }
                case nameof(S_随机宝箱三物品4):
                    {
                        int num2 = (Settings.Default.随机宝箱三物品4 = (int)control.Value);
                        int 随机宝箱三物品5 = num2;
                        int num987 = (Config.随机宝箱三物品4 = 随机宝箱三物品5);
                        break;
                    }
                case nameof(S_随机宝箱三几率4):
                    {
                        int num2 = (Settings.Default.随机宝箱三几率4 = (int)control.Value);
                        int 随机宝箱三几率5 = num2;
                        int num985 = (Config.随机宝箱三几率4 = 随机宝箱三几率5);
                        break;
                    }
                case nameof(S_随机宝箱三物品5):
                    {
                        int num2 = (Settings.Default.随机宝箱三物品5 = (int)control.Value);
                        int 随机宝箱三物品4 = num2;
                        int num983 = (Config.随机宝箱三物品5 = 随机宝箱三物品4);
                        break;
                    }
                case nameof(S_随机宝箱三几率5):
                    {
                        int num2 = (Settings.Default.随机宝箱三几率5 = (int)control.Value);
                        int 随机宝箱三几率4 = num2;
                        int num981 = (Config.随机宝箱三几率5 = 随机宝箱三几率4);
                        break;
                    }
                case nameof(S_随机宝箱三物品6):
                    {
                        int num2 = (Settings.Default.随机宝箱三物品6 = (int)control.Value);
                        int 随机宝箱三物品3 = num2;
                        int num979 = (Config.随机宝箱三物品6 = 随机宝箱三物品3);
                        break;
                    }
                case nameof(S_随机宝箱三几率6):
                    {
                        int num2 = (Settings.Default.随机宝箱三几率6 = (int)control.Value);
                        int 随机宝箱三几率3 = num2;
                        int num977 = (Config.随机宝箱三几率6 = 随机宝箱三几率3);
                        break;
                    }
                case nameof(S_随机宝箱三物品7):
                    {
                        int num2 = (Settings.Default.随机宝箱三物品7 = (int)control.Value);
                        int 随机宝箱三物品2 = num2;
                        int num975 = (Config.随机宝箱三物品7 = 随机宝箱三物品2);
                        break;
                    }
                case nameof(S_随机宝箱三几率7):
                    {
                        int num2 = (Settings.Default.随机宝箱三几率7 = (int)control.Value);
                        int 随机宝箱三几率2 = num2;
                        int num973 = (Config.随机宝箱三几率7 = 随机宝箱三几率2);
                        break;
                    }
                case nameof(S_随机宝箱三物品8):
                    {
                        int num2 = (Settings.Default.随机宝箱三物品8 = (int)control.Value);
                        int 随机宝箱三物品 = num2;
                        int num971 = (Config.随机宝箱三物品8 = 随机宝箱三物品);
                        break;
                    }
                case nameof(S_随机宝箱三几率8):
                    {
                        int num2 = (Settings.Default.随机宝箱三几率8 = (int)control.Value);
                        int 随机宝箱三几率 = num2;
                        int num969 = (Config.随机宝箱三几率8 = 随机宝箱三几率);
                        break;
                    }
                case nameof(S_随机宝箱二物品1):
                    {
                        int num2 = (Settings.Default.随机宝箱二物品1 = (int)control.Value);
                        int 随机宝箱二物品8 = num2;
                        int num967 = (Config.随机宝箱二物品1 = 随机宝箱二物品8);
                        break;
                    }
                case nameof(S_随机宝箱二几率1):
                    {
                        int num2 = (Settings.Default.随机宝箱二几率1 = (int)control.Value);
                        int 随机宝箱二几率8 = num2;
                        int num965 = (Config.随机宝箱二几率1 = 随机宝箱二几率8);
                        break;
                    }
                case nameof(S_随机宝箱二物品2):
                    {
                        int num2 = (Settings.Default.随机宝箱二物品2 = (int)control.Value);
                        int 随机宝箱二物品7 = num2;
                        int num963 = (Config.随机宝箱二物品2 = 随机宝箱二物品7);
                        break;
                    }
                case nameof(S_随机宝箱二几率2):
                    {
                        int num2 = (Settings.Default.随机宝箱二几率2 = (int)control.Value);
                        int 随机宝箱二几率7 = num2;
                        int num961 = (Config.随机宝箱二几率2 = 随机宝箱二几率7);
                        break;
                    }
                case nameof(S_随机宝箱二物品3):
                    {
                        int num2 = (Settings.Default.随机宝箱二物品3 = (int)control.Value);
                        int 随机宝箱二物品6 = num2;
                        int num959 = (Config.随机宝箱二物品3 = 随机宝箱二物品6);
                        break;
                    }
                case nameof(S_随机宝箱二几率3):
                    {
                        int num2 = (Settings.Default.随机宝箱二几率3 = (int)control.Value);
                        int 随机宝箱二几率6 = num2;
                        int num957 = (Config.随机宝箱二几率3 = 随机宝箱二几率6);
                        break;
                    }
                case nameof(S_随机宝箱二物品4):
                    {
                        int num2 = (Settings.Default.随机宝箱二物品4 = (int)control.Value);
                        int 随机宝箱二物品5 = num2;
                        int num955 = (Config.随机宝箱二物品4 = 随机宝箱二物品5);
                        break;
                    }
                case nameof(S_随机宝箱二几率4):
                    {
                        int num2 = (Settings.Default.随机宝箱二几率4 = (int)control.Value);
                        int 随机宝箱二几率5 = num2;
                        int num953 = (Config.随机宝箱二几率4 = 随机宝箱二几率5);
                        break;
                    }
                case nameof(S_随机宝箱二物品5):
                    {
                        int num2 = (Settings.Default.随机宝箱二物品5 = (int)control.Value);
                        int 随机宝箱二物品4 = num2;
                        int num951 = (Config.随机宝箱二物品5 = 随机宝箱二物品4);
                        break;
                    }
                case nameof(S_随机宝箱二几率5):
                    {
                        int num2 = (Settings.Default.随机宝箱二几率5 = (int)control.Value);
                        int 随机宝箱二几率4 = num2;
                        int num949 = (Config.随机宝箱二几率5 = 随机宝箱二几率4);
                        break;
                    }
                case nameof(S_随机宝箱二物品6):
                    {
                        int num2 = (Settings.Default.随机宝箱二物品6 = (int)control.Value);
                        int 随机宝箱二物品3 = num2;
                        int num947 = (Config.随机宝箱二物品6 = 随机宝箱二物品3);
                        break;
                    }
                case nameof(S_随机宝箱二几率6):
                    {
                        int num2 = (Settings.Default.随机宝箱二几率6 = (int)control.Value);
                        int 随机宝箱二几率3 = num2;
                        int num945 = (Config.随机宝箱二几率6 = 随机宝箱二几率3);
                        break;
                    }
                case nameof(S_随机宝箱二物品7):
                    {
                        int num2 = (Settings.Default.随机宝箱二物品7 = (int)control.Value);
                        int 随机宝箱二物品2 = num2;
                        int num943 = (Config.随机宝箱二物品7 = 随机宝箱二物品2);
                        break;
                    }
                case nameof(S_随机宝箱二几率7):
                    {
                        int num2 = (Settings.Default.随机宝箱二几率7 = (int)control.Value);
                        int 随机宝箱二几率2 = num2;
                        int num941 = (Config.随机宝箱二几率7 = 随机宝箱二几率2);
                        break;
                    }
                case nameof(S_随机宝箱二物品8):
                    {
                        int num2 = (Settings.Default.随机宝箱二物品8 = (int)control.Value);
                        int 随机宝箱二物品 = num2;
                        int num939 = (Config.随机宝箱二物品8 = 随机宝箱二物品);
                        break;
                    }
                case nameof(S_随机宝箱二几率8):
                    {
                        int num2 = (Settings.Default.随机宝箱二几率8 = (int)control.Value);
                        int 随机宝箱二几率 = num2;
                        int num937 = (Config.随机宝箱二几率8 = 随机宝箱二几率);
                        break;
                    }
                case nameof(S_随机宝箱一物品1):
                    {
                        int num2 = (Settings.Default.随机宝箱一物品1 = (int)control.Value);
                        int 随机宝箱一物品8 = num2;
                        int num935 = (Config.随机宝箱一物品1 = 随机宝箱一物品8);
                        break;
                    }
                case nameof(S_随机宝箱一几率1):
                    {
                        int num2 = (Settings.Default.随机宝箱一几率1 = (int)control.Value);
                        int 随机宝箱一几率8 = num2;
                        int num933 = (Config.随机宝箱一几率1 = 随机宝箱一几率8);
                        break;
                    }
                case nameof(S_随机宝箱一物品2):
                    {
                        int num2 = (Settings.Default.随机宝箱一物品2 = (int)control.Value);
                        int 随机宝箱一物品7 = num2;
                        int num931 = (Config.随机宝箱一物品2 = 随机宝箱一物品7);
                        break;
                    }
                case nameof(S_随机宝箱一几率2):
                    {
                        int num2 = (Settings.Default.随机宝箱一几率2 = (int)control.Value);
                        int 随机宝箱一几率7 = num2;
                        int num929 = (Config.随机宝箱一几率2 = 随机宝箱一几率7);
                        break;
                    }
                case nameof(S_随机宝箱一物品3):
                    {
                        int num2 = (Settings.Default.随机宝箱一物品3 = (int)control.Value);
                        int 随机宝箱一物品6 = num2;
                        int num927 = (Config.随机宝箱一物品3 = 随机宝箱一物品6);
                        break;
                    }
                case nameof(S_随机宝箱一几率3):
                    {
                        int num2 = (Settings.Default.随机宝箱一几率3 = (int)control.Value);
                        int 随机宝箱一几率6 = num2;
                        int num925 = (Config.随机宝箱一几率3 = 随机宝箱一几率6);
                        break;
                    }
                case nameof(S_随机宝箱一物品4):
                    {
                        int num2 = (Settings.Default.随机宝箱一物品4 = (int)control.Value);
                        int 随机宝箱一物品5 = num2;
                        int num923 = (Config.随机宝箱一物品4 = 随机宝箱一物品5);
                        break;
                    }
                case nameof(S_随机宝箱一几率4):
                    {
                        int num2 = (Settings.Default.随机宝箱一几率4 = (int)control.Value);
                        int 随机宝箱一几率5 = num2;
                        int num921 = (Config.随机宝箱一几率4 = 随机宝箱一几率5);
                        break;
                    }
                case nameof(S_随机宝箱一物品5):
                    {
                        int num2 = (Settings.Default.随机宝箱一物品5 = (int)control.Value);
                        int 随机宝箱一物品4 = num2;
                        int num919 = (Config.随机宝箱一物品5 = 随机宝箱一物品4);
                        break;
                    }
                case nameof(S_随机宝箱一几率5):
                    {
                        int num2 = (Settings.Default.随机宝箱一几率5 = (int)control.Value);
                        int 随机宝箱一几率4 = num2;
                        int num917 = (Config.随机宝箱一几率5 = 随机宝箱一几率4);
                        break;
                    }
                case nameof(S_随机宝箱一物品6):
                    {
                        int num2 = (Settings.Default.随机宝箱一物品6 = (int)control.Value);
                        int 随机宝箱一物品3 = num2;
                        int num915 = (Config.随机宝箱一物品6 = 随机宝箱一物品3);
                        break;
                    }
                case nameof(S_随机宝箱一几率6):
                    {
                        int num2 = (Settings.Default.随机宝箱一几率6 = (int)control.Value);
                        int 随机宝箱一几率3 = num2;
                        int num913 = (Config.随机宝箱一几率6 = 随机宝箱一几率3);
                        break;
                    }
                case nameof(S_随机宝箱一物品7):
                    {
                        int num2 = (Settings.Default.随机宝箱一物品7 = (int)control.Value);
                        int 随机宝箱一物品2 = num2;
                        int num911 = (Config.随机宝箱一物品7 = 随机宝箱一物品2);
                        break;
                    }
                case nameof(S_随机宝箱一几率7):
                    {
                        int num2 = (Settings.Default.随机宝箱一几率7 = (int)control.Value);
                        int 随机宝箱一几率2 = num2;
                        int num909 = (Config.随机宝箱一几率7 = 随机宝箱一几率2);
                        break;
                    }
                case nameof(S_随机宝箱一物品8):
                    {
                        int num2 = (Settings.Default.随机宝箱一物品8 = (int)control.Value);
                        int 随机宝箱一物品 = num2;
                        int num907 = (Config.随机宝箱一物品8 = 随机宝箱一物品);
                        break;
                    }
                case nameof(S_随机宝箱一几率8):
                    {
                        int num2 = (Settings.Default.随机宝箱一几率8 = (int)control.Value);
                        int 随机宝箱一几率 = num2;
                        int num905 = (Config.随机宝箱一几率8 = 随机宝箱一几率);
                        break;
                    }
                case nameof(S_随机宝箱一数量1):
                    {
                        int num2 = (Settings.Default.随机宝箱一数量1 = (int)control.Value);
                        int 随机宝箱一数量8 = num2;
                        int num903 = (Config.随机宝箱一数量1 = 随机宝箱一数量8);
                        break;
                    }
                case nameof(S_随机宝箱一数量2):
                    {
                        int num2 = (Settings.Default.随机宝箱一数量2 = (int)control.Value);
                        int 随机宝箱一数量7 = num2;
                        int num901 = (Config.随机宝箱一数量2 = 随机宝箱一数量7);
                        break;
                    }
                case nameof(S_随机宝箱一数量3):
                    {
                        int num2 = (Settings.Default.随机宝箱一数量3 = (int)control.Value);
                        int 随机宝箱一数量6 = num2;
                        int num899 = (Config.随机宝箱一数量3 = 随机宝箱一数量6);
                        break;
                    }
                case nameof(S_随机宝箱一数量4):
                    {
                        int num2 = (Settings.Default.随机宝箱一数量4 = (int)control.Value);
                        int 随机宝箱一数量5 = num2;
                        int num897 = (Config.随机宝箱一数量4 = 随机宝箱一数量5);
                        break;
                    }
                case nameof(S_随机宝箱一数量5):
                    {
                        int num2 = (Settings.Default.随机宝箱一数量5 = (int)control.Value);
                        int 随机宝箱一数量4 = num2;
                        int num895 = (Config.随机宝箱一数量5 = 随机宝箱一数量4);
                        break;
                    }
                case nameof(S_随机宝箱一数量6):
                    {
                        int num2 = (Settings.Default.随机宝箱一数量6 = (int)control.Value);
                        int 随机宝箱一数量3 = num2;
                        int num893 = (Config.随机宝箱一数量6 = 随机宝箱一数量3);
                        break;
                    }
                case nameof(S_随机宝箱一数量7):
                    {
                        int num2 = (Settings.Default.随机宝箱一数量7 = (int)control.Value);
                        int 随机宝箱一数量2 = num2;
                        int num891 = (Config.随机宝箱一数量7 = 随机宝箱一数量2);
                        break;
                    }
                case nameof(S_随机宝箱一数量8):
                    {
                        int num2 = (Settings.Default.随机宝箱一数量8 = (int)control.Value);
                        int 随机宝箱一数量 = num2;
                        int num889 = (Config.随机宝箱一数量8 = 随机宝箱一数量);
                        break;
                    }
                case nameof(S_随机宝箱二数量1):
                    {
                        int num2 = (Settings.Default.随机宝箱二数量1 = (int)control.Value);
                        int 随机宝箱二数量8 = num2;
                        int num887 = (Config.随机宝箱二数量1 = 随机宝箱二数量8);
                        break;
                    }
                case nameof(S_随机宝箱二数量2):
                    {
                        int num2 = (Settings.Default.随机宝箱二数量2 = (int)control.Value);
                        int 随机宝箱二数量7 = num2;
                        int num885 = (Config.随机宝箱二数量2 = 随机宝箱二数量7);
                        break;
                    }
                case nameof(S_随机宝箱二数量3):
                    {
                        int num2 = (Settings.Default.随机宝箱二数量3 = (int)control.Value);
                        int 随机宝箱二数量6 = num2;
                        int num883 = (Config.随机宝箱二数量3 = 随机宝箱二数量6);
                        break;
                    }
                case nameof(S_随机宝箱二数量4):
                    {
                        int num2 = (Settings.Default.随机宝箱二数量4 = (int)control.Value);
                        int 随机宝箱二数量5 = num2;
                        int num881 = (Config.随机宝箱二数量4 = 随机宝箱二数量5);
                        break;
                    }
                case nameof(S_随机宝箱二数量5):
                    {
                        int num2 = (Settings.Default.随机宝箱二数量5 = (int)control.Value);
                        int 随机宝箱二数量4 = num2;
                        int num879 = (Config.随机宝箱二数量5 = 随机宝箱二数量4);
                        break;
                    }
                case nameof(S_随机宝箱二数量6):
                    {
                        int num2 = (Settings.Default.随机宝箱二数量6 = (int)control.Value);
                        int 随机宝箱二数量3 = num2;
                        int num877 = (Config.随机宝箱二数量6 = 随机宝箱二数量3);
                        break;
                    }
                case nameof(S_随机宝箱二数量7):
                    {
                        int num2 = (Settings.Default.随机宝箱二数量7 = (int)control.Value);
                        int 随机宝箱二数量2 = num2;
                        int num875 = (Config.随机宝箱二数量7 = 随机宝箱二数量2);
                        break;
                    }
                case nameof(S_随机宝箱二数量8):
                    {
                        int num2 = (Settings.Default.随机宝箱二数量8 = (int)control.Value);
                        int 随机宝箱二数量 = num2;
                        int num873 = (Config.随机宝箱二数量8 = 随机宝箱二数量);
                        break;
                    }
                case nameof(S_随机宝箱三数量1):
                    {
                        int num2 = (Settings.Default.随机宝箱三数量1 = (int)control.Value);
                        int 随机宝箱三数量8 = num2;
                        int num871 = (Config.随机宝箱三数量1 = 随机宝箱三数量8);
                        break;
                    }
                case nameof(S_随机宝箱三数量2):
                    {
                        int num2 = (Settings.Default.随机宝箱三数量2 = (int)control.Value);
                        int 随机宝箱三数量7 = num2;
                        int num869 = (Config.随机宝箱三数量2 = 随机宝箱三数量7);
                        break;
                    }
                case nameof(S_随机宝箱三数量3):
                    {
                        int num2 = (Settings.Default.随机宝箱三数量3 = (int)control.Value);
                        int 随机宝箱三数量6 = num2;
                        int num867 = (Config.随机宝箱三数量3 = 随机宝箱三数量6);
                        break;
                    }
                case nameof(S_随机宝箱三数量4):
                    {
                        int num2 = (Settings.Default.随机宝箱三数量4 = (int)control.Value);
                        int 随机宝箱三数量5 = num2;
                        int num865 = (Config.随机宝箱三数量4 = 随机宝箱三数量5);
                        break;
                    }
                case nameof(S_随机宝箱三数量5):
                    {
                        int num2 = (Settings.Default.随机宝箱三数量5 = (int)control.Value);
                        int 随机宝箱三数量4 = num2;
                        int num863 = (Config.随机宝箱三数量5 = 随机宝箱三数量4);
                        break;
                    }
                case nameof(S_随机宝箱三数量6):
                    {
                        int num2 = (Settings.Default.随机宝箱三数量6 = (int)control.Value);
                        int 随机宝箱三数量3 = num2;
                        int num861 = (Config.随机宝箱三数量6 = 随机宝箱三数量3);
                        break;
                    }
                case nameof(S_随机宝箱三数量7):
                    {
                        int num2 = (Settings.Default.随机宝箱三数量7 = (int)control.Value);
                        int 随机宝箱三数量2 = num2;
                        int num859 = (Config.随机宝箱三数量7 = 随机宝箱三数量2);
                        break;
                    }
                case nameof(S_随机宝箱三数量8):
                    {
                        int num2 = (Settings.Default.随机宝箱三数量8 = (int)control.Value);
                        int 随机宝箱三数量 = num2;
                        int num857 = (Config.随机宝箱三数量8 = 随机宝箱三数量);
                        break;
                    }
                case nameof(S_沙城地图保护):
                    {
                        int num2 = (Settings.Default.沙城地图保护 = (int)control.Value);
                        int 沙城地图保护 = num2;
                        int num855 = (Config.沙城地图保护 = 沙城地图保护);
                        break;
                    }
                case nameof(S_NoobProtectionLevel):
                    {
                        int num2 = (Settings.Default.NoobProtectionLevel = (int)control.Value);
                        int 新手等级保护 = num2;
                        int num853 = (Config.NoobProtectionLevel = 新手等级保护);
                        break;
                    }
                case nameof(S_新手地图保护1):
                    {
                        int num2 = (Settings.Default.新手地图保护1 = (int)control.Value);
                        int 新手地图保护10 = num2;
                        int num851 = (Config.新手地图保护1 = 新手地图保护10);
                        break;
                    }
                case nameof(S_新手地图保护2):
                    {
                        int num2 = (Settings.Default.新手地图保护2 = (int)control.Value);
                        int 新手地图保护9 = num2;
                        int num849 = (Config.新手地图保护2 = 新手地图保护9);
                        break;
                    }
                case nameof(S_新手地图保护3):
                    {
                        int num2 = (Settings.Default.新手地图保护3 = (int)control.Value);
                        int 新手地图保护8 = num2;
                        int num847 = (Config.新手地图保护3 = 新手地图保护8);
                        break;
                    }
                case nameof(S_新手地图保护4):
                    {
                        int num2 = (Settings.Default.新手地图保护4 = (int)control.Value);
                        int 新手地图保护7 = num2;
                        int num845 = (Config.新手地图保护4 = 新手地图保护7);
                        break;
                    }
                case nameof(S_新手地图保护5):
                    {
                        int num2 = (Settings.Default.新手地图保护5 = (int)control.Value);
                        int 新手地图保护6 = num2;
                        int num843 = (Config.新手地图保护5 = 新手地图保护6);
                        break;
                    }
                case nameof(S_新手地图保护6):
                    {
                        int num2 = (Settings.Default.新手地图保护6 = (int)control.Value);
                        int 新手地图保护5 = num2;
                        int num841 = (Config.新手地图保护6 = 新手地图保护5);
                        break;
                    }
                case nameof(S_新手地图保护7):
                    {
                        int num2 = (Settings.Default.新手地图保护7 = (int)control.Value);
                        int 新手地图保护4 = num2;
                        int num839 = (Config.新手地图保护7 = 新手地图保护4);
                        break;
                    }
                case nameof(S_新手地图保护8):
                    {
                        int num2 = (Settings.Default.新手地图保护8 = (int)control.Value);
                        int 新手地图保护3 = num2;
                        int num837 = (Config.新手地图保护8 = 新手地图保护3);
                        break;
                    }
                case nameof(S_新手地图保护9):
                    {
                        int num2 = (Settings.Default.新手地图保护9 = (int)control.Value);
                        int 新手地图保护2 = num2;
                        int num835 = (Config.新手地图保护9 = 新手地图保护2);
                        break;
                    }
                case nameof(S_新手地图保护10):
                    {
                        int num2 = (Settings.Default.新手地图保护10 = (int)control.Value);
                        int 新手地图保护 = num2;
                        int num833 = (Config.新手地图保护10 = 新手地图保护);
                        break;
                    }
                case nameof(S_沙巴克停止开关):
                    {
                        int num2 = (Settings.Default.沙巴克停止开关 = (int)control.Value);
                        int 沙巴克停止开关 = num2;
                        int num831 = (Config.沙巴克停止开关 = 沙巴克停止开关);
                        break;
                    }
                case nameof(S_沙巴克城主称号):
                    {
                        byte b2 = (Settings.Default.沙巴克城主称号 = (byte)control.Value);
                        byte 沙巴克城主称号 = b2;
                        byte b13 = (Config.沙巴克城主称号 = 沙巴克城主称号);
                        break;
                    }
                case nameof(S_沙巴克成员称号):
                    {
                        byte b2 = (Settings.Default.沙巴克成员称号 = (byte)control.Value);
                        byte 沙巴克成员称号 = b2;
                        byte b11 = (Config.沙巴克成员称号 = 沙巴克成员称号);
                        break;
                    }
                case nameof(S_沙巴克称号领取开关):
                    {
                        int num2 = (Settings.Default.沙巴克称号领取开关 = (int)control.Value);
                        int 沙巴克称号领取开关 = num2;
                        int num829 = (Config.沙巴克称号领取开关 = 沙巴克称号领取开关);
                        break;
                    }
                case nameof(S_通用1装备佩戴数量):
                    {
                        int num2 = (Settings.Default.通用1装备佩戴数量 = (int)control.Value);
                        int 通用1装备佩戴数量 = num2;
                        int num827 = (Config.通用1装备佩戴数量 = 通用1装备佩戴数量);
                        break;
                    }
                case nameof(S_通用2装备佩戴数量):
                    {
                        int num2 = (Settings.Default.通用2装备佩戴数量 = (int)control.Value);
                        int 通用2装备佩戴数量 = num2;
                        int num825 = (Config.通用2装备佩戴数量 = 通用2装备佩戴数量);
                        break;
                    }
                case nameof(S_通用3装备佩戴数量):
                    {
                        int num2 = (Settings.Default.通用3装备佩戴数量 = (int)control.Value);
                        int 通用3装备佩戴数量 = num2;
                        int num823 = (Config.通用3装备佩戴数量 = 通用3装备佩戴数量);
                        break;
                    }
                case nameof(S_通用4装备佩戴数量):
                    {
                        int num2 = (Settings.Default.通用4装备佩戴数量 = (int)control.Value);
                        int 通用4装备佩戴数量 = num2;
                        int num821 = (Config.通用4装备佩戴数量 = 通用4装备佩戴数量);
                        break;
                    }
                case nameof(S_通用5装备佩戴数量):
                    {
                        int num2 = (Settings.Default.通用5装备佩戴数量 = (int)control.Value);
                        int 通用5装备佩戴数量 = num2;
                        int num819 = (Config.通用5装备佩戴数量 = 通用5装备佩戴数量);
                        break;
                    }
                case nameof(S_通用6装备佩戴数量):
                    {
                        int num2 = (Settings.Default.通用6装备佩戴数量 = (int)control.Value);
                        int 通用6装备佩戴数量 = num2;
                        int num817 = (Config.通用6装备佩戴数量 = 通用6装备佩戴数量);
                        break;
                    }
                case nameof(S_重置屠魔副本时间):
                    {
                        int num2 = (Settings.Default.重置屠魔副本时间 = (int)control.Value);
                        int 重置屠魔副本时间 = num2;
                        int num815 = (Config.重置屠魔副本时间 = 重置屠魔副本时间);
                        break;
                    }
                case nameof(S_屠魔令回收数量):
                    {
                        int num2 = (Settings.Default.屠魔令回收数量 = (int)control.Value);
                        int 屠魔令回收数量 = num2;
                        int num813 = (Config.屠魔令回收数量 = 屠魔令回收数量);
                        break;
                    }
                case nameof(S_新手上线赠送开关):
                    {
                        int num2 = (Settings.Default.新手上线赠送开关 = (int)control.Value);
                        int 新手上线赠送开关 = num2;
                        int num811 = (Config.新手上线赠送开关 = 新手上线赠送开关);
                        break;
                    }
                case nameof(S_新手上线赠送物品1):
                    {
                        int num2 = (Settings.Default.新手上线赠送物品1 = (int)control.Value);
                        int 新手上线赠送物品6 = num2;
                        int num809 = (Config.新手上线赠送物品1 = 新手上线赠送物品6);
                        break;
                    }
                case nameof(S_新手上线赠送物品2):
                    {
                        int num2 = (Settings.Default.新手上线赠送物品2 = (int)control.Value);
                        int 新手上线赠送物品5 = num2;
                        int num807 = (Config.新手上线赠送物品2 = 新手上线赠送物品5);
                        break;
                    }
                case nameof(S_新手上线赠送物品3):
                    {
                        int num2 = (Settings.Default.新手上线赠送物品3 = (int)control.Value);
                        int 新手上线赠送物品4 = num2;
                        int num805 = (Config.新手上线赠送物品3 = 新手上线赠送物品4);
                        break;
                    }
                case nameof(S_新手上线赠送物品4):
                    {
                        int num2 = (Settings.Default.新手上线赠送物品4 = (int)control.Value);
                        int 新手上线赠送物品3 = num2;
                        int num803 = (Config.新手上线赠送物品4 = 新手上线赠送物品3);
                        break;
                    }
                case nameof(S_新手上线赠送物品5):
                    {
                        int num2 = (Settings.Default.新手上线赠送物品5 = (int)control.Value);
                        int 新手上线赠送物品2 = num2;
                        int num801 = (Config.新手上线赠送物品5 = 新手上线赠送物品2);
                        break;
                    }
                case nameof(S_新手上线赠送物品6):
                    {
                        int num2 = (Settings.Default.新手上线赠送物品6 = (int)control.Value);
                        int 新手上线赠送物品 = num2;
                        int num799 = (Config.新手上线赠送物品6 = 新手上线赠送物品);
                        break;
                    }
                case nameof(S_元宝袋新创数量1):
                    {
                        int num2 = (Settings.Default.元宝袋新创数量1 = (int)control.Value);
                        int 元宝袋新创数量5 = num2;
                        int num797 = (Config.元宝袋新创数量1 = 元宝袋新创数量5);
                        break;
                    }
                case nameof(S_元宝袋新创数量2):
                    {
                        int num2 = (Settings.Default.元宝袋新创数量2 = (int)control.Value);
                        int 元宝袋新创数量4 = num2;
                        int num795 = (Config.元宝袋新创数量2 = 元宝袋新创数量4);
                        break;
                    }
                case nameof(S_元宝袋新创数量3):
                    {
                        int num2 = (Settings.Default.元宝袋新创数量3 = (int)control.Value);
                        int 元宝袋新创数量3 = num2;
                        int num793 = (Config.元宝袋新创数量3 = 元宝袋新创数量3);
                        break;
                    }
                case nameof(S_元宝袋新创数量4):
                    {
                        int num2 = (Settings.Default.元宝袋新创数量4 = (int)control.Value);
                        int 元宝袋新创数量2 = num2;
                        int num791 = (Config.元宝袋新创数量4 = 元宝袋新创数量2);
                        break;
                    }
                case nameof(S_元宝袋新创数量5):
                    {
                        int num2 = (Settings.Default.元宝袋新创数量5 = (int)control.Value);
                        int 元宝袋新创数量 = num2;
                        int num789 = (Config.元宝袋新创数量5 = 元宝袋新创数量);
                        break;
                    }
                case nameof(S_高级赞助礼包1):
                    {
                        int num2 = (Settings.Default.高级赞助礼包1 = (int)control.Value);
                        int 高级赞助礼包8 = num2;
                        int num787 = (Config.高级赞助礼包1 = 高级赞助礼包8);
                        break;
                    }
                case nameof(S_高级赞助礼包2):
                    {
                        int num2 = (Settings.Default.高级赞助礼包2 = (int)control.Value);
                        int 高级赞助礼包7 = num2;
                        int num785 = (Config.高级赞助礼包2 = 高级赞助礼包7);
                        break;
                    }
                case nameof(S_高级赞助礼包3):
                    {
                        int num2 = (Settings.Default.高级赞助礼包3 = (int)control.Value);
                        int 高级赞助礼包6 = num2;
                        int num783 = (Config.高级赞助礼包3 = 高级赞助礼包6);
                        break;
                    }
                case nameof(S_高级赞助礼包4):
                    {
                        int num2 = (Settings.Default.高级赞助礼包4 = (int)control.Value);
                        int 高级赞助礼包5 = num2;
                        int num781 = (Config.高级赞助礼包4 = 高级赞助礼包5);
                        break;
                    }
                case nameof(S_高级赞助礼包5):
                    {
                        int num2 = (Settings.Default.高级赞助礼包5 = (int)control.Value);
                        int 高级赞助礼包4 = num2;
                        int num779 = (Config.高级赞助礼包5 = 高级赞助礼包4);
                        break;
                    }
                case nameof(S_高级赞助礼包6):
                    {
                        int num2 = (Settings.Default.高级赞助礼包6 = (int)control.Value);
                        int 高级赞助礼包3 = num2;
                        int num777 = (Config.高级赞助礼包6 = 高级赞助礼包3);
                        break;
                    }
                case nameof(S_高级赞助礼包7):
                    {
                        int num2 = (Settings.Default.高级赞助礼包7 = (int)control.Value);
                        int 高级赞助礼包2 = num2;
                        int num775 = (Config.高级赞助礼包7 = 高级赞助礼包2);
                        break;
                    }
                case nameof(S_高级赞助礼包8):
                    {
                        int num2 = (Settings.Default.高级赞助礼包8 = (int)control.Value);
                        int 高级赞助礼包 = num2;
                        int num773 = (Config.高级赞助礼包8 = 高级赞助礼包);
                        break;
                    }
                case nameof(S_高级赞助称号1):
                    {
                        int num2 = (Settings.Default.高级赞助称号1 = (int)control.Value);
                        int 高级赞助称号 = num2;
                        int num771 = (Config.高级赞助称号1 = 高级赞助称号);
                        break;
                    }
                case nameof(S_中级赞助礼包1):
                    {
                        int num2 = (Settings.Default.中级赞助礼包1 = (int)control.Value);
                        int 中级赞助礼包8 = num2;
                        int num769 = (Config.中级赞助礼包1 = 中级赞助礼包8);
                        break;
                    }
                case nameof(S_中级赞助礼包2):
                    {
                        int num2 = (Settings.Default.中级赞助礼包2 = (int)control.Value);
                        int 中级赞助礼包7 = num2;
                        int num767 = (Config.中级赞助礼包2 = 中级赞助礼包7);
                        break;
                    }
                case nameof(S_中级赞助礼包3):
                    {
                        int num2 = (Settings.Default.中级赞助礼包3 = (int)control.Value);
                        int 中级赞助礼包6 = num2;
                        int num765 = (Config.中级赞助礼包3 = 中级赞助礼包6);
                        break;
                    }
                case nameof(S_中级赞助礼包4):
                    {
                        int num2 = (Settings.Default.中级赞助礼包4 = (int)control.Value);
                        int 中级赞助礼包5 = num2;
                        int num763 = (Config.中级赞助礼包4 = 中级赞助礼包5);
                        break;
                    }
                case nameof(S_中级赞助礼包5):
                    {
                        int num2 = (Settings.Default.中级赞助礼包5 = (int)control.Value);
                        int 中级赞助礼包4 = num2;
                        int num761 = (Config.中级赞助礼包5 = 中级赞助礼包4);
                        break;
                    }
                case nameof(S_中级赞助礼包6):
                    {
                        int num2 = (Settings.Default.中级赞助礼包6 = (int)control.Value);
                        int 中级赞助礼包3 = num2;
                        int num759 = (Config.中级赞助礼包6 = 中级赞助礼包3);
                        break;
                    }
                case nameof(S_中级赞助礼包7):
                    {
                        int num2 = (Settings.Default.中级赞助礼包7 = (int)control.Value);
                        int 中级赞助礼包2 = num2;
                        int num757 = (Config.中级赞助礼包7 = 中级赞助礼包2);
                        break;
                    }
                case nameof(S_中级赞助礼包8):
                    {
                        int num2 = (Settings.Default.中级赞助礼包8 = (int)control.Value);
                        int 中级赞助礼包 = num2;
                        int num755 = (Config.中级赞助礼包8 = 中级赞助礼包);
                        break;
                    }
                case nameof(S_中级赞助称号1):
                    {
                        int num2 = (Settings.Default.中级赞助称号1 = (int)control.Value);
                        int 中级赞助称号 = num2;
                        int num753 = (Config.中级赞助称号1 = 中级赞助称号);
                        break;
                    }
                case nameof(S_初级赞助礼包1):
                    {
                        int num2 = (Settings.Default.初级赞助礼包1 = (int)control.Value);
                        int 初级赞助礼包8 = num2;
                        int num751 = (Config.初级赞助礼包1 = 初级赞助礼包8);
                        break;
                    }
                case nameof(S_初级赞助礼包2):
                    {
                        int num2 = (Settings.Default.初级赞助礼包2 = (int)control.Value);
                        int 初级赞助礼包7 = num2;
                        int num749 = (Config.初级赞助礼包2 = 初级赞助礼包7);
                        break;
                    }
                case nameof(S_初级赞助礼包3):
                    {
                        int num2 = (Settings.Default.初级赞助礼包3 = (int)control.Value);
                        int 初级赞助礼包6 = num2;
                        int num747 = (Config.初级赞助礼包3 = 初级赞助礼包6);
                        break;
                    }
                case nameof(S_初级赞助礼包4):
                    {
                        int num2 = (Settings.Default.初级赞助礼包4 = (int)control.Value);
                        int 初级赞助礼包5 = num2;
                        int num745 = (Config.初级赞助礼包4 = 初级赞助礼包5);
                        break;
                    }
                case nameof(S_初级赞助礼包5):
                    {
                        int num2 = (Settings.Default.初级赞助礼包5 = (int)control.Value);
                        int 初级赞助礼包4 = num2;
                        int num743 = (Config.初级赞助礼包5 = 初级赞助礼包4);
                        break;
                    }
                case nameof(S_初级赞助礼包6):
                    {
                        int num2 = (Settings.Default.初级赞助礼包6 = (int)control.Value);
                        int 初级赞助礼包3 = num2;
                        int num741 = (Config.初级赞助礼包6 = 初级赞助礼包3);
                        break;
                    }
                case nameof(S_初级赞助礼包7):
                    {
                        int num2 = (Settings.Default.初级赞助礼包7 = (int)control.Value);
                        int 初级赞助礼包2 = num2;
                        int num739 = (Config.初级赞助礼包7 = 初级赞助礼包2);
                        break;
                    }
                case nameof(S_初级赞助礼包8):
                    {
                        int num2 = (Settings.Default.初级赞助礼包8 = (int)control.Value);
                        int 初级赞助礼包 = num2;
                        int num737 = (Config.初级赞助礼包8 = 初级赞助礼包);
                        break;
                    }
                case nameof(S_初级赞助称号1):
                    {
                        int num2 = (Settings.Default.初级赞助称号1 = (int)control.Value);
                        int 初级赞助称号 = num2;
                        int num735 = (Config.初级赞助称号1 = 初级赞助称号);
                        break;
                    }
                case nameof(S_自动BOSS1界面1开关):
                    {
                        int num2 = (Settings.Default.自动BOSS1界面1开关 = (int)control.Value);
                        int 自动BOSS1界面1开关 = num2;
                        int num733 = (Config.自动BOSS1界面1开关 = 自动BOSS1界面1开关);
                        break;
                    }
                case nameof(S_自动BOSS1界面2开关):
                    {
                        int num2 = (Settings.Default.自动BOSS1界面2开关 = (int)control.Value);
                        int 自动BOSS1界面2开关 = num2;
                        int num731 = (Config.自动BOSS1界面2开关 = 自动BOSS1界面2开关);
                        break;
                    }
                case nameof(S_自动BOSS1界面3开关):
                    {
                        int num2 = (Settings.Default.自动BOSS1界面3开关 = (int)control.Value);
                        int 自动BOSS1界面3开关 = num2;
                        int num729 = (Config.自动BOSS1界面3开关 = 自动BOSS1界面3开关);
                        break;
                    }
                case nameof(S_自动BOSS1界面4开关):
                    {
                        int num2 = (Settings.Default.自动BOSS1界面4开关 = (int)control.Value);
                        int 自动BOSS1界面4开关 = num2;
                        int num727 = (Config.自动BOSS1界面4开关 = 自动BOSS1界面4开关);
                        break;
                    }
                case nameof(S_自动BOSS1界面5开关):
                    {
                        int num2 = (Settings.Default.自动BOSS1界面5开关 = (int)control.Value);
                        int 自动BOSS1界面5开关 = num2;
                        int num725 = (Config.自动BOSS1界面5开关 = 自动BOSS1界面5开关);
                        break;
                    }
                case nameof(S_平台开关模式):
                    {
                        int num2 = (Settings.Default.平台开关模式 = (int)control.Value);
                        int 平台开关模式 = num2;
                        int num723 = (Config.平台开关模式 = 平台开关模式);
                        break;
                    }
                 // TODO: Not used
                /*case nameof(S_平台金币充值模块):
                    {
                        int num2 = (Settings.Default.平台金币充值模块 = (int)numericUpDown.Value);
                        int 平台金币充值模块 = num2;
                        int num721 = (Config.平台金币充值模块 = 平台金币充值模块);
                        break;
                    }*/
                case nameof(S_平台元宝充值模块):
                    {
                        int num2 = (Settings.Default.平台元宝充值模块 = (int)control.Value);
                        int 平台元宝充值模块 = num2;
                        int num719 = (Config.平台元宝充值模块 = 平台元宝充值模块);
                        break;
                    }
                case nameof(S_九层妖塔数量1):
                    {
                        int num2 = (Settings.Default.九层妖塔数量1 = (int)control.Value);
                        int 九层妖塔数量9 = num2;
                        int num717 = (Config.九层妖塔数量1 = 九层妖塔数量9);
                        break;
                    }
                case nameof(S_九层妖塔数量2):
                    {
                        int num2 = (Settings.Default.九层妖塔数量2 = (int)control.Value);
                        int 九层妖塔数量8 = num2;
                        int num715 = (Config.九层妖塔数量2 = 九层妖塔数量8);
                        break;
                    }
                case nameof(S_九层妖塔数量3):
                    {
                        int num2 = (Settings.Default.九层妖塔数量3 = (int)control.Value);
                        int 九层妖塔数量7 = num2;
                        int num713 = (Config.九层妖塔数量3 = 九层妖塔数量7);
                        break;
                    }
                case nameof(S_九层妖塔数量4):
                    {
                        int num2 = (Settings.Default.九层妖塔数量4 = (int)control.Value);
                        int 九层妖塔数量6 = num2;
                        int num711 = (Config.九层妖塔数量4 = 九层妖塔数量6);
                        break;
                    }
                case nameof(S_九层妖塔数量5):
                    {
                        int num2 = (Settings.Default.九层妖塔数量5 = (int)control.Value);
                        int 九层妖塔数量5 = num2;
                        int num709 = (Config.九层妖塔数量5 = 九层妖塔数量5);
                        break;
                    }
                case nameof(S_九层妖塔数量6):
                    {
                        int num2 = (Settings.Default.九层妖塔数量6 = (int)control.Value);
                        int 九层妖塔数量4 = num2;
                        int num707 = (Config.九层妖塔数量6 = 九层妖塔数量4);
                        break;
                    }
                case nameof(S_九层妖塔数量7):
                    {
                        int num2 = (Settings.Default.九层妖塔数量7 = (int)control.Value);
                        int 九层妖塔数量3 = num2;
                        int num705 = (Config.九层妖塔数量7 = 九层妖塔数量3);
                        break;
                    }
                case nameof(S_九层妖塔数量8):
                    {
                        int num2 = (Settings.Default.九层妖塔数量8 = (int)control.Value);
                        int 九层妖塔数量2 = num2;
                        int num703 = (Config.九层妖塔数量8 = 九层妖塔数量2);
                        break;
                    }
                case nameof(S_九层妖塔数量9):
                    {
                        int num2 = (Settings.Default.九层妖塔数量9 = (int)control.Value);
                        int 九层妖塔数量 = num2;
                        int num701 = (Config.九层妖塔数量9 = 九层妖塔数量);
                        break;
                    }
                case nameof(S_九层妖塔副本次数):
                    {
                        int num2 = (Settings.Default.九层妖塔副本次数 = (int)control.Value);
                        int 九层妖塔副本次数 = num2;
                        int num699 = (Config.九层妖塔副本次数 = 九层妖塔副本次数);
                        break;
                    }
                case nameof(S_九层妖塔副本等级):
                    {
                        int num2 = (Settings.Default.九层妖塔副本等级 = (int)control.Value);
                        int 九层妖塔副本等级 = num2;
                        int num697 = (Config.九层妖塔副本等级 = 九层妖塔副本等级);
                        break;
                    }
                case nameof(S_九层妖塔副本物品):
                    {
                        int num2 = (Settings.Default.九层妖塔副本物品 = (int)control.Value);
                        int 九层妖塔副本物品 = num2;
                        int num695 = (Config.九层妖塔副本物品 = 九层妖塔副本物品);
                        break;
                    }
                case nameof(S_九层妖塔副本数量):
                    {
                        int num2 = (Settings.Default.九层妖塔副本数量 = (int)control.Value);
                        int 九层妖塔副本数量 = num2;
                        int num693 = (Config.九层妖塔副本数量 = 九层妖塔副本数量);
                        break;
                    }
                case nameof(S_九层妖塔副本时间小):
                    {
                        int num2 = (Settings.Default.九层妖塔副本时间小 = (int)control.Value);
                        int 九层妖塔副本时间小 = num2;
                        int num691 = (Config.九层妖塔副本时间小 = 九层妖塔副本时间小);
                        break;
                    }
                case nameof(S_九层妖塔副本时间大):
                    {
                        int num2 = (Settings.Default.九层妖塔副本时间大 = (int)control.Value);
                        int 九层妖塔副本时间大 = num2;
                        int num689 = (Config.九层妖塔副本时间大 = 九层妖塔副本时间大);
                        break;
                    }
                case nameof(S_AutoBattleLevel):
                    {
                        byte b2 = (Settings.Default.AutoBattleLevel = (byte)control.Value);
                        byte 挂机等级快捷 = b2;
                        byte b9 = (Config.AutoBattleLevel = 挂机等级快捷);
                        break;
                    }
                case nameof(S_禁止背包铭文洗练):
                    {
                        byte b2 = (Settings.Default.禁止背包铭文洗练 = (byte)control.Value);
                        byte 禁止背包铭文洗练 = b2;
                        byte b7 = (Config.禁止背包铭文洗练 = 禁止背包铭文洗练);
                        break;
                    }
                case nameof(S_沙巴克禁止随机):
                    {
                        byte b2 = (Settings.Default.沙巴克禁止随机 = (byte)control.Value);
                        byte 沙巴克禁止随机 = b2;
                        byte b5 = (Config.沙巴克禁止随机 = 沙巴克禁止随机);
                        break;
                    }
                case nameof(S_冥想丹自定义经验):
                    {
                        int num2 = (Settings.Default.冥想丹自定义经验 = (int)control.Value);
                        int 冥想丹自定义经验 = num2;
                        int num687 = (Config.冥想丹自定义经验 = 冥想丹自定义经验);
                        break;
                    }
                case nameof(S_沙巴克爆装备开关):
                    {
                        byte b2 = (Settings.Default.沙巴克爆装备开关 = (byte)control.Value);
                        byte 沙巴克爆装备开关 = b2;
                        byte b3 = (Config.沙巴克爆装备开关 = 沙巴克爆装备开关);
                        break;
                    }
                case nameof(S_铭文战士1挡1次数):
                    {
                        int num2 = (Settings.Default.铭文战士1挡1次数 = (int)control.Value);
                        int 铭文战士1挡1次数 = num2;
                        int num685 = (Config.铭文战士1挡1次数 = 铭文战士1挡1次数);
                        break;
                    }
                case nameof(S_铭文战士1挡2次数):
                    {
                        int num2 = (Settings.Default.铭文战士1挡2次数 = (int)control.Value);
                        int 铭文战士1挡2次数 = num2;
                        int num683 = (Config.铭文战士1挡2次数 = 铭文战士1挡2次数);
                        break;
                    }
                case nameof(S_铭文战士1挡3次数):
                    {
                        int num2 = (Settings.Default.铭文战士1挡3次数 = (int)control.Value);
                        int 铭文战士1挡3次数 = num2;
                        int num681 = (Config.铭文战士1挡3次数 = 铭文战士1挡3次数);
                        break;
                    }
                case nameof(S_铭文战士2挡1次数):
                    {
                        int num2 = (Settings.Default.铭文战士2挡1次数 = (int)control.Value);
                        int 铭文战士2挡1次数 = num2;
                        int num679 = (Config.铭文战士2挡1次数 = 铭文战士2挡1次数);
                        break;
                    }
                case nameof(S_铭文战士2挡2次数):
                    {
                        int num2 = (Settings.Default.铭文战士2挡2次数 = (int)control.Value);
                        int 铭文战士2挡2次数 = num2;
                        int num677 = (Config.铭文战士2挡2次数 = 铭文战士2挡2次数);
                        break;
                    }
                case nameof(S_铭文战士2挡3次数):
                    {
                        int num2 = (Settings.Default.铭文战士2挡3次数 = (int)control.Value);
                        int 铭文战士2挡3次数 = num2;
                        int num675 = (Config.铭文战士2挡3次数 = 铭文战士2挡3次数);
                        break;
                    }
                case nameof(S_铭文战士3挡1次数):
                    {
                        int num2 = (Settings.Default.铭文战士3挡1次数 = (int)control.Value);
                        int 铭文战士3挡1次数 = num2;
                        int num673 = (Config.铭文战士3挡1次数 = 铭文战士3挡1次数);
                        break;
                    }
                case nameof(S_铭文战士3挡2次数):
                    {
                        int num2 = (Settings.Default.铭文战士3挡2次数 = (int)control.Value);
                        int 铭文战士3挡2次数 = num2;
                        int num671 = (Config.铭文战士3挡2次数 = 铭文战士3挡2次数);
                        break;
                    }
                case nameof(S_铭文战士3挡3次数):
                    {
                        int num2 = (Settings.Default.铭文战士3挡3次数 = (int)control.Value);
                        int 铭文战士3挡3次数 = num2;
                        int num669 = (Config.铭文战士3挡3次数 = 铭文战士3挡3次数);
                        break;
                    }
                case nameof(S_铭文战士1挡1概率):
                    {
                        int num2 = (Settings.Default.铭文战士1挡1概率 = (int)control.Value);
                        int 铭文战士1挡1概率 = num2;
                        int num667 = (Config.铭文战士1挡1概率 = 铭文战士1挡1概率);
                        break;
                    }
                case nameof(S_铭文战士1挡2概率):
                    {
                        int num2 = (Settings.Default.铭文战士1挡2概率 = (int)control.Value);
                        int 铭文战士1挡2概率 = num2;
                        int num665 = (Config.铭文战士1挡2概率 = 铭文战士1挡2概率);
                        break;
                    }
                case nameof(S_铭文战士1挡3概率):
                    {
                        int num2 = (Settings.Default.铭文战士1挡3概率 = (int)control.Value);
                        int 铭文战士1挡3概率 = num2;
                        int num663 = (Config.铭文战士1挡3概率 = 铭文战士1挡3概率);
                        break;
                    }
                case nameof(S_铭文战士2挡1概率):
                    {
                        int num2 = (Settings.Default.铭文战士2挡1概率 = (int)control.Value);
                        int 铭文战士2挡1概率 = num2;
                        int num661 = (Config.铭文战士2挡1概率 = 铭文战士2挡1概率);
                        break;
                    }
                case nameof(S_铭文战士2挡2概率):
                    {
                        int num2 = (Settings.Default.铭文战士2挡2概率 = (int)control.Value);
                        int 铭文战士2挡2概率 = num2;
                        int num659 = (Config.铭文战士2挡2概率 = 铭文战士2挡2概率);
                        break;
                    }
                case nameof(S_铭文战士2挡3概率):
                    {
                        int num2 = (Settings.Default.铭文战士2挡3概率 = (int)control.Value);
                        int 铭文战士2挡3概率 = num2;
                        int num657 = (Config.铭文战士2挡3概率 = 铭文战士2挡3概率);
                        break;
                    }
                case nameof(S_铭文战士3挡1概率):
                    {
                        int num2 = (Settings.Default.铭文战士3挡1概率 = (int)control.Value);
                        int 铭文战士3挡1概率 = num2;
                        int num655 = (Config.铭文战士3挡1概率 = 铭文战士3挡1概率);
                        break;
                    }
                case nameof(S_铭文战士3挡2概率):
                    {
                        int num2 = (Settings.Default.铭文战士3挡2概率 = (int)control.Value);
                        int 铭文战士3挡2概率 = num2;
                        int num653 = (Config.铭文战士3挡2概率 = 铭文战士3挡2概率);
                        break;
                    }
                case nameof(S_铭文战士3挡3概率):
                    {
                        int num2 = (Settings.Default.铭文战士3挡3概率 = (int)control.Value);
                        int 铭文战士3挡3概率 = num2;
                        int num651 = (Config.铭文战士3挡3概率 = 铭文战士3挡3概率);
                        break;
                    }
                case nameof(S_铭文战士3挡技能编号):
                    {
                        int num2 = (Settings.Default.铭文战士3挡技能编号 = (int)control.Value);
                        int 铭文战士3挡技能编号 = num2;
                        int num649 = (Config.铭文战士3挡技能编号 = 铭文战士3挡技能编号);
                        break;
                    }
                case nameof(S_铭文战士3挡技能铭文):
                    {
                        int num2 = (Settings.Default.铭文战士3挡技能铭文 = (int)control.Value);
                        int 铭文战士3挡技能铭文 = num2;
                        int num647 = (Config.铭文战士3挡技能铭文 = 铭文战士3挡技能铭文);
                        break;
                    }
                case nameof(S_铭文战士2挡技能编号):
                    {
                        int num2 = (Settings.Default.铭文战士2挡技能编号 = (int)control.Value);
                        int 铭文战士2挡技能编号 = num2;
                        int num645 = (Config.铭文战士2挡技能编号 = 铭文战士2挡技能编号);
                        break;
                    }
                case nameof(S_铭文战士2挡技能铭文):
                    {
                        int num2 = (Settings.Default.铭文战士2挡技能铭文 = (int)control.Value);
                        int 铭文战士2挡技能铭文 = num2;
                        int num643 = (Config.铭文战士2挡技能铭文 = 铭文战士2挡技能铭文);
                        break;
                    }
                case nameof(S_铭文战士1挡技能编号):
                    {
                        int num2 = (Settings.Default.铭文战士1挡技能编号 = (int)control.Value);
                        int 铭文战士1挡技能编号 = num2;
                        int num641 = (Config.铭文战士1挡技能编号 = 铭文战士1挡技能编号);
                        break;
                    }
                case nameof(S_铭文战士1挡技能铭文):
                    {
                        int num2 = (Settings.Default.铭文战士1挡技能铭文 = (int)control.Value);
                        int 铭文战士1挡技能铭文 = num2;
                        int num639 = (Config.铭文战士1挡技能铭文 = 铭文战士1挡技能铭文);
                        break;
                    }
                case nameof(S_新手上线赠送称号1):
                    {
                        int num2 = (Settings.Default.新手上线赠送称号1 = (int)control.Value);
                        int 新手上线赠送称号 = num2;
                        int num637 = (Config.新手上线赠送称号1 = 新手上线赠送称号);
                        break;
                    }
                case nameof(S_铭文法师1挡1次数):
                    {
                        int num2 = (Settings.Default.铭文法师1挡1次数 = (int)control.Value);
                        int 铭文法师1挡1次数 = num2;
                        int num635 = (Config.铭文法师1挡1次数 = 铭文法师1挡1次数);
                        break;
                    }
                case nameof(S_铭文法师1挡2次数):
                    {
                        int num2 = (Settings.Default.铭文法师1挡2次数 = (int)control.Value);
                        int 铭文法师1挡2次数 = num2;
                        int num633 = (Config.铭文法师1挡2次数 = 铭文法师1挡2次数);
                        break;
                    }
                case nameof(S_铭文法师1挡3次数):
                    {
                        int num2 = (Settings.Default.铭文法师1挡3次数 = (int)control.Value);
                        int 铭文法师1挡3次数 = num2;
                        int num631 = (Config.铭文法师1挡3次数 = 铭文法师1挡3次数);
                        break;
                    }
                case nameof(S_铭文法师2挡1次数):
                    {
                        int num2 = (Settings.Default.铭文法师2挡1次数 = (int)control.Value);
                        int 铭文法师2挡1次数 = num2;
                        int num629 = (Config.铭文法师2挡1次数 = 铭文法师2挡1次数);
                        break;
                    }
                case nameof(S_铭文法师2挡2次数):
                    {
                        int num2 = (Settings.Default.铭文法师2挡2次数 = (int)control.Value);
                        int 铭文法师2挡2次数 = num2;
                        int num627 = (Config.铭文法师2挡2次数 = 铭文法师2挡2次数);
                        break;
                    }
                case nameof(S_铭文法师2挡3次数):
                    {
                        int num2 = (Settings.Default.铭文法师2挡3次数 = (int)control.Value);
                        int 铭文法师2挡3次数 = num2;
                        int num625 = (Config.铭文法师2挡3次数 = 铭文法师2挡3次数);
                        break;
                    }
                case nameof(S_铭文法师3挡1次数):
                    {
                        int num2 = (Settings.Default.铭文法师3挡1次数 = (int)control.Value);
                        int 铭文法师3挡1次数 = num2;
                        int num623 = (Config.铭文法师3挡1次数 = 铭文法师3挡1次数);
                        break;
                    }
                case nameof(S_铭文法师3挡2次数):
                    {
                        int num2 = (Settings.Default.铭文法师3挡2次数 = (int)control.Value);
                        int 铭文法师3挡2次数 = num2;
                        int num621 = (Config.铭文法师3挡2次数 = 铭文法师3挡2次数);
                        break;
                    }
                case nameof(S_铭文法师3挡3次数):
                    {
                        int num2 = (Settings.Default.铭文法师3挡3次数 = (int)control.Value);
                        int 铭文法师3挡3次数 = num2;
                        int num619 = (Config.铭文法师3挡3次数 = 铭文法师3挡3次数);
                        break;
                    }
                case nameof(S_铭文法师1挡1概率):
                    {
                        int num2 = (Settings.Default.铭文法师1挡1概率 = (int)control.Value);
                        int 铭文法师1挡1概率 = num2;
                        int num617 = (Config.铭文法师1挡1概率 = 铭文法师1挡1概率);
                        break;
                    }
                case nameof(S_铭文法师1挡2概率):
                    {
                        int num2 = (Settings.Default.铭文法师1挡2概率 = (int)control.Value);
                        int 铭文法师1挡2概率 = num2;
                        int num615 = (Config.铭文法师1挡2概率 = 铭文法师1挡2概率);
                        break;
                    }
                case nameof(S_铭文法师1挡3概率):
                    {
                        int num2 = (Settings.Default.铭文法师1挡3概率 = (int)control.Value);
                        int 铭文法师1挡3概率 = num2;
                        int num613 = (Config.铭文法师1挡3概率 = 铭文法师1挡3概率);
                        break;
                    }
                case nameof(S_铭文法师2挡1概率):
                    {
                        int num2 = (Settings.Default.铭文法师2挡1概率 = (int)control.Value);
                        int 铭文法师2挡1概率 = num2;
                        int num611 = (Config.铭文法师2挡1概率 = 铭文法师2挡1概率);
                        break;
                    }
                case nameof(S_铭文法师2挡2概率):
                    {
                        int num2 = (Settings.Default.铭文法师2挡2概率 = (int)control.Value);
                        int 铭文法师2挡2概率 = num2;
                        int num609 = (Config.铭文法师2挡2概率 = 铭文法师2挡2概率);
                        break;
                    }
                case nameof(S_铭文法师2挡3概率):
                    {
                        int num2 = (Settings.Default.铭文法师2挡3概率 = (int)control.Value);
                        int 铭文法师2挡3概率 = num2;
                        int num607 = (Config.铭文法师2挡3概率 = 铭文法师2挡3概率);
                        break;
                    }
                case nameof(S_铭文法师3挡1概率):
                    {
                        int num2 = (Settings.Default.铭文法师3挡1概率 = (int)control.Value);
                        int 铭文法师3挡1概率 = num2;
                        int num605 = (Config.铭文法师3挡1概率 = 铭文法师3挡1概率);
                        break;
                    }
                case nameof(S_铭文法师3挡2概率):
                    {
                        int num2 = (Settings.Default.铭文法师3挡2概率 = (int)control.Value);
                        int 铭文法师3挡2概率 = num2;
                        int num603 = (Config.铭文法师3挡2概率 = 铭文法师3挡2概率);
                        break;
                    }
                case nameof(S_铭文法师3挡3概率):
                    {
                        int num2 = (Settings.Default.铭文法师3挡3概率 = (int)control.Value);
                        int 铭文法师3挡3概率 = num2;
                        int num601 = (Config.铭文法师3挡3概率 = 铭文法师3挡3概率);
                        break;
                    }
                case nameof(S_铭文法师3挡技能编号):
                    {
                        int num2 = (Settings.Default.铭文法师3挡技能编号 = (int)control.Value);
                        int 铭文法师3挡技能编号 = num2;
                        int num599 = (Config.铭文法师3挡技能编号 = 铭文法师3挡技能编号);
                        break;
                    }
                case nameof(S_铭文法师3挡技能铭文):
                    {
                        int num2 = (Settings.Default.铭文法师3挡技能铭文 = (int)control.Value);
                        int 铭文法师3挡技能铭文 = num2;
                        int num597 = (Config.铭文法师3挡技能铭文 = 铭文法师3挡技能铭文);
                        break;
                    }
                case nameof(S_铭文法师2挡技能编号):
                    {
                        int num2 = (Settings.Default.铭文法师2挡技能编号 = (int)control.Value);
                        int 铭文法师2挡技能编号 = num2;
                        int num595 = (Config.铭文法师2挡技能编号 = 铭文法师2挡技能编号);
                        break;
                    }
                case nameof(S_铭文法师2挡技能铭文):
                    {
                        int num2 = (Settings.Default.铭文法师2挡技能铭文 = (int)control.Value);
                        int 铭文法师2挡技能铭文 = num2;
                        int num593 = (Config.铭文法师2挡技能铭文 = 铭文法师2挡技能铭文);
                        break;
                    }
                case nameof(S_铭文法师1挡技能编号):
                    {
                        int num2 = (Settings.Default.铭文法师1挡技能编号 = (int)control.Value);
                        int 铭文法师1挡技能编号 = num2;
                        int num591 = (Config.铭文法师1挡技能编号 = 铭文法师1挡技能编号);
                        break;
                    }
                case nameof(S_铭文法师1挡技能铭文):
                    {
                        int num2 = (Settings.Default.铭文法师1挡技能铭文 = (int)control.Value);
                        int 铭文法师1挡技能铭文 = num2;
                        int num589 = (Config.铭文法师1挡技能铭文 = 铭文法师1挡技能铭文);
                        break;
                    }
                case nameof(S_铭文道士1挡1次数):
                    {
                        int num2 = (Settings.Default.铭文道士1挡1次数 = (int)control.Value);
                        int 铭文道士1挡1次数 = num2;
                        int num587 = (Config.铭文道士1挡1次数 = 铭文道士1挡1次数);
                        break;
                    }
                case nameof(S_铭文道士1挡2次数):
                    {
                        int num2 = (Settings.Default.铭文道士1挡2次数 = (int)control.Value);
                        int 铭文道士1挡2次数 = num2;
                        int num585 = (Config.铭文道士1挡2次数 = 铭文道士1挡2次数);
                        break;
                    }
                case nameof(S_铭文道士1挡3次数):
                    {
                        int num2 = (Settings.Default.铭文道士1挡3次数 = (int)control.Value);
                        int 铭文道士1挡3次数 = num2;
                        int num583 = (Config.铭文道士1挡3次数 = 铭文道士1挡3次数);
                        break;
                    }
                case nameof(S_铭文道士2挡1次数):
                    {
                        int num2 = (Settings.Default.铭文道士2挡1次数 = (int)control.Value);
                        int 铭文道士2挡1次数 = num2;
                        int num581 = (Config.铭文道士2挡1次数 = 铭文道士2挡1次数);
                        break;
                    }
                case nameof(S_铭文道士2挡2次数):
                    {
                        int num2 = (Settings.Default.铭文道士2挡2次数 = (int)control.Value);
                        int 铭文道士2挡2次数 = num2;
                        int num579 = (Config.铭文道士2挡2次数 = 铭文道士2挡2次数);
                        break;
                    }
                case nameof(S_铭文道士2挡3次数):
                    {
                        int num2 = (Settings.Default.铭文道士2挡3次数 = (int)control.Value);
                        int 铭文道士2挡3次数 = num2;
                        int num577 = (Config.铭文道士2挡3次数 = 铭文道士2挡3次数);
                        break;
                    }
                case nameof(S_铭文道士3挡1次数):
                    {
                        int num2 = (Settings.Default.铭文道士3挡1次数 = (int)control.Value);
                        int 铭文道士3挡1次数 = num2;
                        int num575 = (Config.铭文道士3挡1次数 = 铭文道士3挡1次数);
                        break;
                    }
                case nameof(S_铭文道士3挡2次数):
                    {
                        int num2 = (Settings.Default.铭文道士3挡2次数 = (int)control.Value);
                        int 铭文道士3挡2次数 = num2;
                        int num573 = (Config.铭文道士3挡2次数 = 铭文道士3挡2次数);
                        break;
                    }
                case nameof(S_铭文道士3挡3次数):
                    {
                        int num2 = (Settings.Default.铭文道士3挡3次数 = (int)control.Value);
                        int 铭文道士3挡3次数 = num2;
                        int num571 = (Config.铭文道士3挡3次数 = 铭文道士3挡3次数);
                        break;
                    }
                case nameof(S_铭文道士1挡1概率):
                    {
                        int num2 = (Settings.Default.铭文道士1挡1概率 = (int)control.Value);
                        int 铭文道士1挡1概率 = num2;
                        int num569 = (Config.铭文道士1挡1概率 = 铭文道士1挡1概率);
                        break;
                    }
                case nameof(S_铭文道士1挡2概率):
                    {
                        int num2 = (Settings.Default.铭文道士1挡2概率 = (int)control.Value);
                        int 铭文道士1挡2概率 = num2;
                        int num567 = (Config.铭文道士1挡2概率 = 铭文道士1挡2概率);
                        break;
                    }
                case nameof(S_铭文道士1挡3概率):
                    {
                        int num2 = (Settings.Default.铭文道士1挡3概率 = (int)control.Value);
                        int 铭文道士1挡3概率 = num2;
                        int num565 = (Config.铭文道士1挡3概率 = 铭文道士1挡3概率);
                        break;
                    }
                case nameof(S_铭文道士2挡1概率):
                    {
                        int num2 = (Settings.Default.铭文道士2挡1概率 = (int)control.Value);
                        int 铭文道士2挡1概率 = num2;
                        int num563 = (Config.铭文道士2挡1概率 = 铭文道士2挡1概率);
                        break;
                    }
                case nameof(S_铭文道士2挡2概率):
                    {
                        int num2 = (Settings.Default.铭文道士2挡2概率 = (int)control.Value);
                        int 铭文道士2挡2概率 = num2;
                        int num561 = (Config.铭文道士2挡2概率 = 铭文道士2挡2概率);
                        break;
                    }
                case nameof(S_铭文道士2挡3概率):
                    {
                        int num2 = (Settings.Default.铭文道士2挡3概率 = (int)control.Value);
                        int 铭文道士2挡3概率 = num2;
                        int num559 = (Config.铭文道士2挡3概率 = 铭文道士2挡3概率);
                        break;
                    }
                case nameof(S_铭文道士3挡1概率):
                    {
                        int num2 = (Settings.Default.铭文道士3挡1概率 = (int)control.Value);
                        int 铭文道士3挡1概率 = num2;
                        int num557 = (Config.铭文道士3挡1概率 = 铭文道士3挡1概率);
                        break;
                    }
                case nameof(S_铭文道士3挡2概率):
                    {
                        int num2 = (Settings.Default.铭文道士3挡2概率 = (int)control.Value);
                        int 铭文道士3挡2概率 = num2;
                        int num555 = (Config.铭文道士3挡2概率 = 铭文道士3挡2概率);
                        break;
                    }
                case nameof(S_铭文道士3挡3概率):
                    {
                        int num2 = (Settings.Default.铭文道士3挡3概率 = (int)control.Value);
                        int 铭文道士3挡3概率 = num2;
                        int num553 = (Config.铭文道士3挡3概率 = 铭文道士3挡3概率);
                        break;
                    }
                case nameof(S_铭文道士3挡技能编号):
                    {
                        int num2 = (Settings.Default.铭文道士3挡技能编号 = (int)control.Value);
                        int 铭文道士3挡技能编号 = num2;
                        int num551 = (Config.铭文道士3挡技能编号 = 铭文道士3挡技能编号);
                        break;
                    }
                case nameof(S_铭文道士3挡技能铭文):
                    {
                        int num2 = (Settings.Default.铭文道士3挡技能铭文 = (int)control.Value);
                        int 铭文道士3挡技能铭文 = num2;
                        int num549 = (Config.铭文道士3挡技能铭文 = 铭文道士3挡技能铭文);
                        break;
                    }
                case nameof(S_铭文道士2挡技能编号):
                    {
                        int num2 = (Settings.Default.铭文道士2挡技能编号 = (int)control.Value);
                        int 铭文道士2挡技能编号 = num2;
                        int num547 = (Config.铭文道士2挡技能编号 = 铭文道士2挡技能编号);
                        break;
                    }
                case nameof(S_铭文道士2挡技能铭文):
                    {
                        int num2 = (Settings.Default.铭文道士2挡技能铭文 = (int)control.Value);
                        int 铭文道士2挡技能铭文 = num2;
                        int num545 = (Config.铭文道士2挡技能铭文 = 铭文道士2挡技能铭文);
                        break;
                    }
                case nameof(S_铭文道士1挡技能编号):
                    {
                        int num2 = (Settings.Default.铭文道士1挡技能编号 = (int)control.Value);
                        int 铭文道士1挡技能编号 = num2;
                        int num543 = (Config.铭文道士1挡技能编号 = 铭文道士1挡技能编号);
                        break;
                    }
                case nameof(S_铭文道士1挡技能铭文):
                    {
                        int num2 = (Settings.Default.铭文道士1挡技能铭文 = (int)control.Value);
                        int 铭文道士1挡技能铭文 = num2;
                        int num541 = (Config.铭文道士1挡技能铭文 = 铭文道士1挡技能铭文);
                        break;
                    }
                case nameof(S_铭文刺客1挡1次数):
                    {
                        int num2 = (Settings.Default.铭文刺客1挡1次数 = (int)control.Value);
                        int 铭文刺客1挡1次数 = num2;
                        int num539 = (Config.铭文刺客1挡1次数 = 铭文刺客1挡1次数);
                        break;
                    }
                case nameof(S_铭文刺客1挡2次数):
                    {
                        int num2 = (Settings.Default.铭文刺客1挡2次数 = (int)control.Value);
                        int 铭文刺客1挡2次数 = num2;
                        int num537 = (Config.铭文刺客1挡2次数 = 铭文刺客1挡2次数);
                        break;
                    }
                case nameof(S_铭文刺客1挡3次数):
                    {
                        int num2 = (Settings.Default.铭文刺客1挡3次数 = (int)control.Value);
                        int 铭文刺客1挡3次数 = num2;
                        int num535 = (Config.铭文刺客1挡3次数 = 铭文刺客1挡3次数);
                        break;
                    }
                case nameof(S_铭文刺客2挡1次数):
                    {
                        int num2 = (Settings.Default.铭文刺客2挡1次数 = (int)control.Value);
                        int 铭文刺客2挡1次数 = num2;
                        int num533 = (Config.铭文刺客2挡1次数 = 铭文刺客2挡1次数);
                        break;
                    }
                case nameof(S_铭文刺客2挡2次数):
                    {
                        int num2 = (Settings.Default.铭文刺客2挡2次数 = (int)control.Value);
                        int 铭文刺客2挡2次数 = num2;
                        int num531 = (Config.铭文刺客2挡2次数 = 铭文刺客2挡2次数);
                        break;
                    }
                case nameof(S_铭文刺客2挡3次数):
                    {
                        int num2 = (Settings.Default.铭文刺客2挡3次数 = (int)control.Value);
                        int 铭文刺客2挡3次数 = num2;
                        int num529 = (Config.铭文刺客2挡3次数 = 铭文刺客2挡3次数);
                        break;
                    }
                case nameof(S_铭文刺客3挡1次数):
                    {
                        int num2 = (Settings.Default.铭文刺客3挡1次数 = (int)control.Value);
                        int 铭文刺客3挡1次数 = num2;
                        int num527 = (Config.铭文刺客3挡1次数 = 铭文刺客3挡1次数);
                        break;
                    }
                case nameof(S_铭文刺客3挡2次数):
                    {
                        int num2 = (Settings.Default.铭文刺客3挡2次数 = (int)control.Value);
                        int 铭文刺客3挡2次数 = num2;
                        int num525 = (Config.铭文刺客3挡2次数 = 铭文刺客3挡2次数);
                        break;
                    }
                case nameof(S_铭文刺客3挡3次数):
                    {
                        int num2 = (Settings.Default.铭文刺客3挡3次数 = (int)control.Value);
                        int 铭文刺客3挡3次数 = num2;
                        int num523 = (Config.铭文刺客3挡3次数 = 铭文刺客3挡3次数);
                        break;
                    }
                case nameof(S_铭文刺客1挡1概率):
                    {
                        int num2 = (Settings.Default.铭文刺客1挡1概率 = (int)control.Value);
                        int 铭文刺客1挡1概率 = num2;
                        int num521 = (Config.铭文刺客1挡1概率 = 铭文刺客1挡1概率);
                        break;
                    }
                case nameof(S_铭文刺客1挡2概率):
                    {
                        int num2 = (Settings.Default.铭文刺客1挡2概率 = (int)control.Value);
                        int 铭文刺客1挡2概率 = num2;
                        int num519 = (Config.铭文刺客1挡2概率 = 铭文刺客1挡2概率);
                        break;
                    }
                case nameof(S_铭文刺客1挡3概率):
                    {
                        int num2 = (Settings.Default.铭文刺客1挡3概率 = (int)control.Value);
                        int 铭文刺客1挡3概率 = num2;
                        int num517 = (Config.铭文刺客1挡3概率 = 铭文刺客1挡3概率);
                        break;
                    }
                case nameof(S_铭文刺客2挡1概率):
                    {
                        int num2 = (Settings.Default.铭文刺客2挡1概率 = (int)control.Value);
                        int 铭文刺客2挡1概率 = num2;
                        int num515 = (Config.铭文刺客2挡1概率 = 铭文刺客2挡1概率);
                        break;
                    }
                case nameof(S_铭文刺客2挡2概率):
                    {
                        int num2 = (Settings.Default.铭文刺客2挡2概率 = (int)control.Value);
                        int 铭文刺客2挡2概率 = num2;
                        int num513 = (Config.铭文刺客2挡2概率 = 铭文刺客2挡2概率);
                        break;
                    }
                case nameof(S_铭文刺客2挡3概率):
                    {
                        int num2 = (Settings.Default.铭文刺客2挡3概率 = (int)control.Value);
                        int 铭文刺客2挡3概率 = num2;
                        int num511 = (Config.铭文刺客2挡3概率 = 铭文刺客2挡3概率);
                        break;
                    }
                case nameof(S_铭文刺客3挡1概率):
                    {
                        int num2 = (Settings.Default.铭文刺客3挡1概率 = (int)control.Value);
                        int 铭文刺客3挡1概率 = num2;
                        int num509 = (Config.铭文刺客3挡1概率 = 铭文刺客3挡1概率);
                        break;
                    }
                case nameof(S_铭文刺客3挡2概率):
                    {
                        int num2 = (Settings.Default.铭文刺客3挡2概率 = (int)control.Value);
                        int 铭文刺客3挡2概率 = num2;
                        int num507 = (Config.铭文刺客3挡2概率 = 铭文刺客3挡2概率);
                        break;
                    }
                case nameof(S_铭文刺客3挡3概率):
                    {
                        int num2 = (Settings.Default.铭文刺客3挡3概率 = (int)control.Value);
                        int 铭文刺客3挡3概率 = num2;
                        int num505 = (Config.铭文刺客3挡3概率 = 铭文刺客3挡3概率);
                        break;
                    }
                case nameof(S_铭文刺客3挡技能编号):
                    {
                        int num2 = (Settings.Default.铭文刺客3挡技能编号 = (int)control.Value);
                        int 铭文刺客3挡技能编号 = num2;
                        int num503 = (Config.铭文刺客3挡技能编号 = 铭文刺客3挡技能编号);
                        break;
                    }
                case nameof(S_铭文刺客3挡技能铭文):
                    {
                        int num2 = (Settings.Default.铭文刺客3挡技能铭文 = (int)control.Value);
                        int 铭文刺客3挡技能铭文 = num2;
                        int num501 = (Config.铭文刺客3挡技能铭文 = 铭文刺客3挡技能铭文);
                        break;
                    }
                case nameof(S_铭文刺客2挡技能编号):
                    {
                        int num2 = (Settings.Default.铭文刺客2挡技能编号 = (int)control.Value);
                        int 铭文刺客2挡技能编号 = num2;
                        int num499 = (Config.铭文刺客2挡技能编号 = 铭文刺客2挡技能编号);
                        break;
                    }
                case nameof(S_铭文刺客2挡技能铭文):
                    {
                        int num2 = (Settings.Default.铭文刺客2挡技能铭文 = (int)control.Value);
                        int 铭文刺客2挡技能铭文 = num2;
                        int num497 = (Config.铭文刺客2挡技能铭文 = 铭文刺客2挡技能铭文);
                        break;
                    }
                case nameof(S_铭文刺客1挡技能编号):
                    {
                        int num2 = (Settings.Default.铭文刺客1挡技能编号 = (int)control.Value);
                        int 铭文刺客1挡技能编号 = num2;
                        int num495 = (Config.铭文刺客1挡技能编号 = 铭文刺客1挡技能编号);
                        break;
                    }
                case nameof(S_铭文刺客1挡技能铭文):
                    {
                        int num2 = (Settings.Default.铭文刺客1挡技能铭文 = (int)control.Value);
                        int 铭文刺客1挡技能铭文 = num2;
                        int num493 = (Config.铭文刺客1挡技能铭文 = 铭文刺客1挡技能铭文);
                        break;
                    }
                case nameof(S_铭文弓手1挡1次数):
                    {
                        int num2 = (Settings.Default.铭文弓手1挡1次数 = (int)control.Value);
                        int 铭文弓手1挡1次数 = num2;
                        int num491 = (Config.铭文弓手1挡1次数 = 铭文弓手1挡1次数);
                        break;
                    }
                case nameof(S_铭文弓手1挡2次数):
                    {
                        int num2 = (Settings.Default.铭文弓手1挡2次数 = (int)control.Value);
                        int 铭文弓手1挡2次数 = num2;
                        int num489 = (Config.铭文弓手1挡2次数 = 铭文弓手1挡2次数);
                        break;
                    }
                case nameof(S_铭文弓手1挡3次数):
                    {
                        int num2 = (Settings.Default.铭文弓手1挡3次数 = (int)control.Value);
                        int 铭文弓手1挡3次数 = num2;
                        int num487 = (Config.铭文弓手1挡3次数 = 铭文弓手1挡3次数);
                        break;
                    }
                case nameof(S_铭文弓手2挡1次数):
                    {
                        int num2 = (Settings.Default.铭文弓手2挡1次数 = (int)control.Value);
                        int 铭文弓手2挡1次数 = num2;
                        int num485 = (Config.铭文弓手2挡1次数 = 铭文弓手2挡1次数);
                        break;
                    }
                case nameof(S_铭文弓手2挡2次数):
                    {
                        int num2 = (Settings.Default.铭文弓手2挡2次数 = (int)control.Value);
                        int 铭文弓手2挡2次数 = num2;
                        int num483 = (Config.铭文弓手2挡2次数 = 铭文弓手2挡2次数);
                        break;
                    }
                case nameof(S_铭文弓手2挡3次数):
                    {
                        int num2 = (Settings.Default.铭文弓手2挡3次数 = (int)control.Value);
                        int 铭文弓手2挡3次数 = num2;
                        int num481 = (Config.铭文弓手2挡3次数 = 铭文弓手2挡3次数);
                        break;
                    }
                case nameof(S_铭文弓手3挡1次数):
                    {
                        int num2 = (Settings.Default.铭文弓手3挡1次数 = (int)control.Value);
                        int 铭文弓手3挡1次数 = num2;
                        int num479 = (Config.铭文弓手3挡1次数 = 铭文弓手3挡1次数);
                        break;
                    }
                case nameof(S_铭文弓手3挡2次数):
                    {
                        int num2 = (Settings.Default.铭文弓手3挡2次数 = (int)control.Value);
                        int 铭文弓手3挡2次数 = num2;
                        int num477 = (Config.铭文弓手3挡2次数 = 铭文弓手3挡2次数);
                        break;
                    }
                case nameof(S_铭文弓手3挡3次数):
                    {
                        int num2 = (Settings.Default.铭文弓手3挡3次数 = (int)control.Value);
                        int 铭文弓手3挡3次数 = num2;
                        int num475 = (Config.铭文弓手3挡3次数 = 铭文弓手3挡3次数);
                        break;
                    }
                case nameof(S_铭文弓手1挡1概率):
                    {
                        int num2 = (Settings.Default.铭文弓手1挡1概率 = (int)control.Value);
                        int 铭文弓手1挡1概率 = num2;
                        int num473 = (Config.铭文弓手1挡1概率 = 铭文弓手1挡1概率);
                        break;
                    }
                case nameof(S_铭文弓手1挡2概率):
                    {
                        int num2 = (Settings.Default.铭文弓手1挡2概率 = (int)control.Value);
                        int 铭文弓手1挡2概率 = num2;
                        int num471 = (Config.铭文弓手1挡2概率 = 铭文弓手1挡2概率);
                        break;
                    }
                case nameof(S_铭文弓手1挡3概率):
                    {
                        int num2 = (Settings.Default.铭文弓手1挡3概率 = (int)control.Value);
                        int 铭文弓手1挡3概率 = num2;
                        int num469 = (Config.铭文弓手1挡3概率 = 铭文弓手1挡3概率);
                        break;
                    }
                case nameof(S_铭文弓手2挡1概率):
                    {
                        int num2 = (Settings.Default.铭文弓手2挡1概率 = (int)control.Value);
                        int 铭文弓手2挡1概率 = num2;
                        int num467 = (Config.铭文弓手2挡1概率 = 铭文弓手2挡1概率);
                        break;
                    }
                case nameof(S_铭文弓手2挡2概率):
                    {
                        int num2 = (Settings.Default.铭文弓手2挡2概率 = (int)control.Value);
                        int 铭文弓手2挡2概率 = num2;
                        int num465 = (Config.铭文弓手2挡2概率 = 铭文弓手2挡2概率);
                        break;
                    }
                case nameof(S_铭文弓手2挡3概率):
                    {
                        int num2 = (Settings.Default.铭文弓手2挡3概率 = (int)control.Value);
                        int 铭文弓手2挡3概率 = num2;
                        int num463 = (Config.铭文弓手2挡3概率 = 铭文弓手2挡3概率);
                        break;
                    }
                case nameof(S_铭文弓手3挡1概率):
                    {
                        int num2 = (Settings.Default.铭文弓手3挡1概率 = (int)control.Value);
                        int 铭文弓手3挡1概率 = num2;
                        int num461 = (Config.铭文弓手3挡1概率 = 铭文弓手3挡1概率);
                        break;
                    }
                case nameof(S_铭文弓手3挡2概率):
                    {
                        int num2 = (Settings.Default.铭文弓手3挡2概率 = (int)control.Value);
                        int 铭文弓手3挡2概率 = num2;
                        int num459 = (Config.铭文弓手3挡2概率 = 铭文弓手3挡2概率);
                        break;
                    }
                case nameof(S_铭文弓手3挡3概率):
                    {
                        int num2 = (Settings.Default.铭文弓手3挡3概率 = (int)control.Value);
                        int 铭文弓手3挡3概率 = num2;
                        int num457 = (Config.铭文弓手3挡3概率 = 铭文弓手3挡3概率);
                        break;
                    }
                case nameof(S_铭文弓手3挡技能编号):
                    {
                        int num2 = (Settings.Default.铭文弓手3挡技能编号 = (int)control.Value);
                        int 铭文弓手3挡技能编号 = num2;
                        int num455 = (Config.铭文弓手3挡技能编号 = 铭文弓手3挡技能编号);
                        break;
                    }
                case nameof(S_铭文弓手3挡技能铭文):
                    {
                        int num2 = (Settings.Default.铭文弓手3挡技能铭文 = (int)control.Value);
                        int 铭文弓手3挡技能铭文 = num2;
                        int num453 = (Config.铭文弓手3挡技能铭文 = 铭文弓手3挡技能铭文);
                        break;
                    }
                case nameof(S_铭文弓手2挡技能编号):
                    {
                        int num2 = (Settings.Default.铭文弓手2挡技能编号 = (int)control.Value);
                        int 铭文弓手2挡技能编号 = num2;
                        int num451 = (Config.铭文弓手2挡技能编号 = 铭文弓手2挡技能编号);
                        break;
                    }
                case nameof(S_铭文弓手2挡技能铭文):
                    {
                        int num2 = (Settings.Default.铭文弓手2挡技能铭文 = (int)control.Value);
                        int 铭文弓手2挡技能铭文 = num2;
                        int num449 = (Config.铭文弓手2挡技能铭文 = 铭文弓手2挡技能铭文);
                        break;
                    }
                case nameof(S_铭文弓手1挡技能编号):
                    {
                        int num2 = (Settings.Default.铭文弓手1挡技能编号 = (int)control.Value);
                        int 铭文弓手1挡技能编号 = num2;
                        int num447 = (Config.铭文弓手1挡技能编号 = 铭文弓手1挡技能编号);
                        break;
                    }
                case nameof(S_铭文弓手1挡技能铭文):
                    {
                        int num2 = (Settings.Default.铭文弓手1挡技能铭文 = (int)control.Value);
                        int 铭文弓手1挡技能铭文 = num2;
                        int num445 = (Config.铭文弓手1挡技能铭文 = 铭文弓手1挡技能铭文);
                        break;
                    }
                case nameof(S_铭文龙枪1挡1次数):
                    {
                        int num2 = (Settings.Default.铭文龙枪1挡1次数 = (int)control.Value);
                        int 铭文龙枪1挡1次数 = num2;
                        int num443 = (Config.铭文龙枪1挡1次数 = 铭文龙枪1挡1次数);
                        break;
                    }
                case nameof(S_铭文龙枪1挡2次数):
                    {
                        int num2 = (Settings.Default.铭文龙枪1挡2次数 = (int)control.Value);
                        int 铭文龙枪1挡2次数 = num2;
                        int num441 = (Config.铭文龙枪1挡2次数 = 铭文龙枪1挡2次数);
                        break;
                    }
                case nameof(S_铭文龙枪1挡3次数):
                    {
                        int num2 = (Settings.Default.铭文龙枪1挡3次数 = (int)control.Value);
                        int 铭文龙枪1挡3次数 = num2;
                        int num439 = (Config.铭文龙枪1挡3次数 = 铭文龙枪1挡3次数);
                        break;
                    }
                case nameof(S_铭文龙枪2挡1次数):
                    {
                        int num2 = (Settings.Default.铭文龙枪2挡1次数 = (int)control.Value);
                        int 铭文龙枪2挡1次数 = num2;
                        int num437 = (Config.铭文龙枪2挡1次数 = 铭文龙枪2挡1次数);
                        break;
                    }
                case nameof(S_铭文龙枪2挡2次数):
                    {
                        int num2 = (Settings.Default.铭文龙枪2挡2次数 = (int)control.Value);
                        int 铭文龙枪2挡2次数 = num2;
                        int num435 = (Config.铭文龙枪2挡2次数 = 铭文龙枪2挡2次数);
                        break;
                    }
                case nameof(S_铭文龙枪2挡3次数):
                    {
                        int num2 = (Settings.Default.铭文龙枪2挡3次数 = (int)control.Value);
                        int 铭文龙枪2挡3次数 = num2;
                        int num433 = (Config.铭文龙枪2挡3次数 = 铭文龙枪2挡3次数);
                        break;
                    }
                case nameof(S_铭文龙枪3挡1次数):
                    {
                        int num2 = (Settings.Default.铭文龙枪3挡1次数 = (int)control.Value);
                        int 铭文龙枪3挡1次数 = num2;
                        int num431 = (Config.铭文龙枪3挡1次数 = 铭文龙枪3挡1次数);
                        break;
                    }
                case nameof(S_铭文龙枪3挡2次数):
                    {
                        int num2 = (Settings.Default.铭文龙枪3挡2次数 = (int)control.Value);
                        int 铭文龙枪3挡2次数 = num2;
                        int num429 = (Config.铭文龙枪3挡2次数 = 铭文龙枪3挡2次数);
                        break;
                    }
                case nameof(S_铭文龙枪3挡3次数):
                    {
                        int num2 = (Settings.Default.铭文龙枪3挡3次数 = (int)control.Value);
                        int 铭文龙枪3挡3次数 = num2;
                        int num427 = (Config.铭文龙枪3挡3次数 = 铭文龙枪3挡3次数);
                        break;
                    }
                case nameof(S_铭文龙枪1挡1概率):
                    {
                        int num2 = (Settings.Default.铭文龙枪1挡1概率 = (int)control.Value);
                        int 铭文龙枪1挡1概率 = num2;
                        int num425 = (Config.铭文龙枪1挡1概率 = 铭文龙枪1挡1概率);
                        break;
                    }
                case nameof(S_铭文龙枪1挡2概率):
                    {
                        int num2 = (Settings.Default.铭文龙枪1挡2概率 = (int)control.Value);
                        int 铭文龙枪1挡2概率 = num2;
                        int num423 = (Config.铭文龙枪1挡2概率 = 铭文龙枪1挡2概率);
                        break;
                    }
                case nameof(S_铭文龙枪1挡3概率):
                    {
                        int num2 = (Settings.Default.铭文龙枪1挡3概率 = (int)control.Value);
                        int 铭文龙枪1挡3概率 = num2;
                        int num421 = (Config.铭文龙枪1挡3概率 = 铭文龙枪1挡3概率);
                        break;
                    }
                case nameof(S_铭文龙枪2挡1概率):
                    {
                        int num2 = (Settings.Default.铭文龙枪2挡1概率 = (int)control.Value);
                        int 铭文龙枪2挡1概率 = num2;
                        int num419 = (Config.铭文龙枪2挡1概率 = 铭文龙枪2挡1概率);
                        break;
                    }
                case nameof(S_铭文龙枪2挡2概率):
                    {
                        int num2 = (Settings.Default.铭文龙枪2挡2概率 = (int)control.Value);
                        int 铭文龙枪2挡2概率 = num2;
                        int num417 = (Config.铭文龙枪2挡2概率 = 铭文龙枪2挡2概率);
                        break;
                    }
                case nameof(S_铭文龙枪2挡3概率):
                    {
                        int num2 = (Settings.Default.铭文龙枪2挡3概率 = (int)control.Value);
                        int 铭文龙枪2挡3概率 = num2;
                        int num415 = (Config.铭文龙枪2挡3概率 = 铭文龙枪2挡3概率);
                        break;
                    }
                case nameof(S_铭文龙枪3挡1概率):
                    {
                        int num2 = (Settings.Default.铭文龙枪3挡1概率 = (int)control.Value);
                        int 铭文龙枪3挡1概率 = num2;
                        int num413 = (Config.铭文龙枪3挡1概率 = 铭文龙枪3挡1概率);
                        break;
                    }
                case nameof(S_铭文龙枪3挡2概率):
                    {
                        int num2 = (Settings.Default.铭文龙枪3挡2概率 = (int)control.Value);
                        int 铭文龙枪3挡2概率 = num2;
                        int num411 = (Config.铭文龙枪3挡2概率 = 铭文龙枪3挡2概率);
                        break;
                    }
                case nameof(S_铭文龙枪3挡3概率):
                    {
                        int num2 = (Settings.Default.铭文龙枪3挡3概率 = (int)control.Value);
                        int 铭文龙枪3挡3概率 = num2;
                        int num409 = (Config.铭文龙枪3挡3概率 = 铭文龙枪3挡3概率);
                        break;
                    }
                case nameof(S_铭文龙枪3挡技能编号):
                    {
                        int num2 = (Settings.Default.铭文龙枪3挡技能编号 = (int)control.Value);
                        int 铭文龙枪3挡技能编号 = num2;
                        int num407 = (Config.铭文龙枪3挡技能编号 = 铭文龙枪3挡技能编号);
                        break;
                    }
                case nameof(S_铭文龙枪3挡技能铭文):
                    {
                        int num2 = (Settings.Default.铭文龙枪3挡技能铭文 = (int)control.Value);
                        int 铭文龙枪3挡技能铭文 = num2;
                        int num405 = (Config.铭文龙枪3挡技能铭文 = 铭文龙枪3挡技能铭文);
                        break;
                    }
                case nameof(S_铭文龙枪2挡技能编号):
                    {
                        int num2 = (Settings.Default.铭文龙枪2挡技能编号 = (int)control.Value);
                        int 铭文龙枪2挡技能编号 = num2;
                        int num403 = (Config.铭文龙枪2挡技能编号 = 铭文龙枪2挡技能编号);
                        break;
                    }
                case nameof(S_铭文龙枪2挡技能铭文):
                    {
                        int num2 = (Settings.Default.铭文龙枪2挡技能铭文 = (int)control.Value);
                        int 铭文龙枪2挡技能铭文 = num2;
                        int num401 = (Config.铭文龙枪2挡技能铭文 = 铭文龙枪2挡技能铭文);
                        break;
                    }
                case nameof(S_铭文龙枪1挡技能编号):
                    {
                        int num2 = (Settings.Default.铭文龙枪1挡技能编号 = (int)control.Value);
                        int 铭文龙枪1挡技能编号 = num2;
                        int num399 = (Config.铭文龙枪1挡技能编号 = 铭文龙枪1挡技能编号);
                        break;
                    }
                case nameof(S_铭文龙枪1挡技能铭文):
                    {
                        int num2 = (Settings.Default.铭文龙枪1挡技能铭文 = (int)control.Value);
                        int 铭文龙枪1挡技能铭文 = num2;
                        int num397 = (Config.铭文龙枪1挡技能铭文 = 铭文龙枪1挡技能铭文);
                        break;
                    }
                case nameof(S_铭文道士保底开关):
                    {
                        int num2 = (Settings.Default.铭文道士保底开关 = (int)control.Value);
                        int 铭文道士保底开关 = num2;
                        int num395 = (Config.铭文道士保底开关 = 铭文道士保底开关);
                        break;
                    }
                case nameof(S_铭文龙枪保底开关):
                    {
                        int num2 = (Settings.Default.铭文龙枪保底开关 = (int)control.Value);
                        int 铭文龙枪保底开关 = num2;
                        int num393 = (Config.铭文龙枪保底开关 = 铭文龙枪保底开关);
                        break;
                    }
                case nameof(S_铭文战士保底开关):
                    {
                        Config.铭文战士保底开关 = Settings.Default.铭文战士保底开关 = (int)control.Value;
                        break;
                    }
                case nameof(S_铭文法师保底开关):
                    {
                        Config.铭文法师保底开关 = Settings.Default.铭文法师保底开关 = (int)control.Value;
                        break;
                    }
                case nameof(S_铭文刺客保底开关):
                    {
                        Config.铭文刺客保底开关 = Settings.Default.铭文刺客保底开关 = (int)control.Value;
                        break;
                    }
                case nameof(S_铭文弓手保底开关):
                    {
                        Config.铭文弓手保底开关 = Settings.Default.铭文弓手保底开关 = (int)control.Value;
                        break;
                    }
                case nameof(S_DropRateModifier):
                    {
                        Config.DropRateModifier = Settings.Default.DropRateModifier = (int)control.Value;
                        break;
                    }
                case nameof(S_魔虫窟副本次数):
                    {
                        int num2 = (Settings.Default.魔虫窟副本次数 = (int)control.Value);
                        int 魔虫窟副本次数 = num2;
                        int num381 = (Config.魔虫窟副本次数 = 魔虫窟副本次数);
                        break;
                    }
                case nameof(S_魔虫窟副本等级):
                    {
                        int num2 = (Settings.Default.魔虫窟副本等级 = (int)control.Value);
                        int 魔虫窟副本等级 = num2;
                        int num379 = (Config.魔虫窟副本等级 = 魔虫窟副本等级);
                        break;
                    }
                case nameof(S_魔虫窟副本物品):
                    {
                        int num2 = (Settings.Default.魔虫窟副本物品 = (int)control.Value);
                        int 魔虫窟副本物品 = num2;
                        int num377 = (Config.魔虫窟副本物品 = 魔虫窟副本物品);
                        break;
                    }
                case nameof(S_魔虫窟副本数量):
                    {
                        int num2 = (Settings.Default.魔虫窟副本数量 = (int)control.Value);
                        int 魔虫窟副本数量 = num2;
                        int num375 = (Config.魔虫窟副本数量 = 魔虫窟副本数量);
                        break;
                    }
                case nameof(S_魔虫窟副本时间小):
                    {
                        int num2 = (Settings.Default.魔虫窟副本时间小 = (int)control.Value);
                        int 魔虫窟副本时间小 = num2;
                        int num373 = (Config.魔虫窟副本时间小 = 魔虫窟副本时间小);
                        break;
                    }
                case nameof(S_魔虫窟副本时间大):
                    {
                        int num2 = (Settings.Default.魔虫窟副本时间大 = (int)control.Value);
                        int 魔虫窟副本时间大 = num2;
                        int num371 = (Config.魔虫窟副本时间大 = 魔虫窟副本时间大);
                        break;
                    }
                case nameof(S_幸运洗练次数保底):
                    {
                        int num2 = (Settings.Default.幸运洗练次数保底 = (int)control.Value);
                        int 幸运洗练次数保底 = num2;
                        int num369 = (Config.幸运洗练次数保底 = 幸运洗练次数保底);
                        break;
                    }
                case nameof(S_幸运洗练点数):
                    {
                        int num2 = (Settings.Default.幸运洗练点数 = (int)control.Value);
                        int 幸运洗练点数 = num2;
                        int num367 = (Config.幸运洗练点数 = 幸运洗练点数);
                        break;
                    }
                case nameof(S_武器强化消耗货币值):
                    {
                        int num2 = (Settings.Default.武器强化消耗货币值 = (int)control.Value);
                        int 武器强化消耗货币值 = num2;
                        int num365 = (Config.武器强化消耗货币值 = 武器强化消耗货币值);
                        break;
                    }
                case nameof(S_武器强化消耗货币开关):
                    {
                        int num2 = (Settings.Default.武器强化消耗货币开关 = (int)control.Value);
                        int 武器强化消耗货币开关 = num2;
                        int num363 = (Config.武器强化消耗货币开关 = 武器强化消耗货币开关);
                        break;
                    }
                case nameof(S_武器强化取回时间):
                    {
                        int num2 = (Settings.Default.武器强化取回时间 = (int)control.Value);
                        int 武器强化取回时间 = num2;
                        int num361 = (Config.武器强化取回时间 = 武器强化取回时间);
                        break;
                    }
                case nameof(S_幸运额外1值):
                    {
                        int num2 = (Settings.Default.幸运额外1值 = (int)control.Value);
                        int 幸运额外1值 = num2;
                        int num359 = (Config.幸运额外1值 = 幸运额外1值);
                        break;
                    }
                case nameof(S_幸运额外2值):
                    {
                        int num2 = (Settings.Default.幸运额外2值 = (int)control.Value);
                        int 幸运额外2值 = num2;
                        int num357 = (Config.幸运额外2值 = 幸运额外2值);
                        break;
                    }
                case nameof(S_幸运额外3值):
                    {
                        int num2 = (Settings.Default.幸运额外3值 = (int)control.Value);
                        int 幸运额外3值 = num2;
                        int num355 = (Config.幸运额外3值 = 幸运额外3值);
                        break;
                    }
                case nameof(S_幸运额外4值):
                    {
                        int num2 = (Settings.Default.幸运额外4值 = (int)control.Value);
                        int 幸运额外4值 = num2;
                        int num353 = (Config.幸运额外4值 = 幸运额外4值);
                        break;
                    }
                case nameof(S_幸运额外5值):
                    {
                        int num2 = (Settings.Default.幸运额外5值 = (int)control.Value);
                        int 幸运额外5值 = num2;
                        int num351 = (Config.幸运额外5值 = 幸运额外5值);
                        break;
                    }
                case nameof(S_幸运额外1伤害):
                    {
                        float num340 = (Settings.Default.幸运额外1伤害 = (float)control.Value);
                        float 幸运额外1伤害 = num340;
                        float num349 = (Config.幸运额外1伤害 = 幸运额外1伤害);
                        break;
                    }
                case nameof(S_幸运额外2伤害):
                    {
                        float num340 = (Settings.Default.幸运额外2伤害 = (float)control.Value);
                        float 幸运额外2伤害 = num340;
                        float num347 = (Config.幸运额外2伤害 = 幸运额外2伤害);
                        break;
                    }
                case nameof(S_幸运额外3伤害):
                    {
                        float num340 = (Settings.Default.幸运额外3伤害 = (float)control.Value);
                        float 幸运额外3伤害 = num340;
                        float num345 = (Config.幸运额外3伤害 = 幸运额外3伤害);
                        break;
                    }
                case nameof(S_幸运额外4伤害):
                    {
                        float num340 = (Settings.Default.幸运额外4伤害 = (float)control.Value);
                        float 幸运额外4伤害 = num340;
                        float num343 = (Config.幸运额外4伤害 = 幸运额外4伤害);
                        break;
                    }
                case nameof(S_幸运额外5伤害):
                    {
                        float num340 = (Settings.Default.幸运额外5伤害 = (float)control.Value);
                        float 幸运额外5伤害 = num340;
                        float num341 = (Config.幸运额外5伤害 = 幸运额外5伤害);
                        break;
                    }
                case nameof(S_暗之门地图1):
                    {
                        int num2 = (Settings.Default.暗之门地图1 = (int)control.Value);
                        int 暗之门地图4 = num2;
                        int num338 = (Config.暗之门地图1 = 暗之门地图4);
                        break;
                    }
                case nameof(S_暗之门地图2):
                    {
                        int num2 = (Settings.Default.暗之门地图2 = (int)control.Value);
                        int 暗之门地图3 = num2;
                        int num336 = (Config.暗之门地图2 = 暗之门地图3);
                        break;
                    }
                case nameof(S_暗之门地图3):
                    {
                        int num2 = (Settings.Default.暗之门地图3 = (int)control.Value);
                        int 暗之门地图2 = num2;
                        int num334 = (Config.暗之门地图3 = 暗之门地图2);
                        break;
                    }
                case nameof(S_暗之门地图4):
                    {
                        int num2 = (Settings.Default.暗之门地图4 = (int)control.Value);
                        int 暗之门地图 = num2;
                        int num332 = (Config.暗之门地图4 = 暗之门地图);
                        break;
                    }
                case nameof(S_暗之门全服提示):
                    {
                        int num2 = (Settings.Default.暗之门全服提示 = (int)control.Value);
                        int 暗之门全服提示 = num2;
                        int num330 = (Config.暗之门全服提示 = 暗之门全服提示);
                        break;
                    }
                case nameof(S_暗之门杀怪触发):
                    {
                        int num2 = (Settings.Default.暗之门杀怪触发 = (int)control.Value);
                        int 暗之门杀怪触发 = num2;
                        int num328 = (Config.暗之门杀怪触发 = 暗之门杀怪触发);
                        break;
                    }
                case nameof(S_暗之门时间):
                    {
                        int num2 = (Settings.Default.暗之门时间 = (int)control.Value);
                        int 暗之门时间 = num2;
                        int num326 = (Config.暗之门时间 = 暗之门时间);
                        break;
                    }
                case nameof(S_暗之门地图1X):
                    {
                        int num2 = (Settings.Default.暗之门地图1X = (int)control.Value);
                        int 暗之门地图1X = num2;
                        int num324 = (Config.暗之门地图1X = 暗之门地图1X);
                        break;
                    }
                case nameof(S_暗之门地图1Y):
                    {
                        int num2 = (Settings.Default.暗之门地图1Y = (int)control.Value);
                        int 暗之门地图1Y = num2;
                        int num322 = (Config.暗之门地图1Y = 暗之门地图1Y);
                        break;
                    }
                case nameof(S_暗之门地图2X):
                    {
                        int num2 = (Settings.Default.暗之门地图2X = (int)control.Value);
                        int 暗之门地图2X = num2;
                        int num320 = (Config.暗之门地图2X = 暗之门地图2X);
                        break;
                    }
                case nameof(S_暗之门地图2Y):
                    {
                        int num2 = (Settings.Default.暗之门地图2Y = (int)control.Value);
                        int 暗之门地图2Y = num2;
                        int num318 = (Config.暗之门地图2Y = 暗之门地图2Y);
                        break;
                    }
                case nameof(S_暗之门地图3X):
                    {
                        int num2 = (Settings.Default.暗之门地图3X = (int)control.Value);
                        int 暗之门地图3X = num2;
                        int num316 = (Config.暗之门地图3X = 暗之门地图3X);
                        break;
                    }
                case nameof(S_暗之门地图3Y):
                    {
                        int num2 = (Settings.Default.暗之门地图3Y = (int)control.Value);
                        int 暗之门地图3Y = num2;
                        int num314 = (Config.暗之门地图3Y = 暗之门地图3Y);
                        break;
                    }
                case nameof(S_暗之门地图4X):
                    {
                        int num2 = (Settings.Default.暗之门地图4X = (int)control.Value);
                        int 暗之门地图4X = num2;
                        int num312 = (Config.暗之门地图4X = 暗之门地图4X);
                        break;
                    }
                case nameof(S_暗之门地图4Y):
                    {
                        int num2 = (Settings.Default.暗之门地图4Y = (int)control.Value);
                        int 暗之门地图4Y = num2;
                        int num310 = (Config.暗之门地图4Y = 暗之门地图4Y);
                        break;
                    }
                case nameof(S_暗之门开关):
                    {
                        int num2 = (Settings.Default.暗之门开关 = (int)control.Value);
                        int 暗之门开关 = num2;
                        int num308 = (Config.暗之门开关 = 暗之门开关);
                        break;
                    }
                case nameof(S_监狱货币):
                    {
                        int num2 = (Settings.Default.监狱货币 = (int)control.Value);
                        int 监狱货币 = num2;
                        int num306 = (Config.监狱货币 = 监狱货币);
                        break;
                    }
                case nameof(S_监狱货币类型):
                    {
                        int num2 = (Settings.Default.监狱货币类型 = (int)control.Value);
                        int 监狱货币类型 = num2;
                        int num304 = (Config.监狱货币类型 = 监狱货币类型);
                        break;
                    }
                case nameof(S_魔虫窟分钟限制):
                    {
                        int num2 = (Settings.Default.魔虫窟分钟限制 = (int)control.Value);
                        int 魔虫窟分钟限制 = num2;
                        int num302 = (Config.魔虫窟分钟限制 = 魔虫窟分钟限制);
                        break;
                    }
                case nameof(S_自定义元宝兑换01):
                    {
                        int num2 = (Settings.Default.自定义元宝兑换01 = (int)control.Value);
                        int 自定义元宝兑换5 = num2;
                        int num300 = (Config.自定义元宝兑换01 = 自定义元宝兑换5);
                        break;
                    }
                case nameof(S_自定义元宝兑换02):
                    {
                        int num2 = (Settings.Default.自定义元宝兑换02 = (int)control.Value);
                        int 自定义元宝兑换4 = num2;
                        int num298 = (Config.自定义元宝兑换02 = 自定义元宝兑换4);
                        break;
                    }
                case nameof(S_自定义元宝兑换03):
                    {
                        int num2 = (Settings.Default.自定义元宝兑换03 = (int)control.Value);
                        int 自定义元宝兑换3 = num2;
                        int num296 = (Config.自定义元宝兑换03 = 自定义元宝兑换3);
                        break;
                    }
                case nameof(S_自定义元宝兑换04):
                    {
                        int num2 = (Settings.Default.自定义元宝兑换04 = (int)control.Value);
                        int 自定义元宝兑换2 = num2;
                        int num294 = (Config.自定义元宝兑换04 = 自定义元宝兑换2);
                        break;
                    }
                case nameof(S_自定义元宝兑换05):
                    {
                        int num2 = (Settings.Default.自定义元宝兑换05 = (int)control.Value);
                        int 自定义元宝兑换 = num2;
                        int num292 = (Config.自定义元宝兑换05 = 自定义元宝兑换);
                        break;
                    }
                case nameof(S_直升物品1):
                    {
                        int num2 = (Settings.Default.直升物品1 = (int)control.Value);
                        int 直升物品9 = num2;
                        int num290 = (Config.直升物品1 = 直升物品9);
                        break;
                    }
                case nameof(S_直升物品2):
                    {
                        int num2 = (Settings.Default.直升物品2 = (int)control.Value);
                        int 直升物品8 = num2;
                        int num288 = (Config.直升物品2 = 直升物品8);
                        break;
                    }
                case nameof(S_直升物品3):
                    {
                        int num2 = (Settings.Default.直升物品3 = (int)control.Value);
                        int 直升物品7 = num2;
                        int num286 = (Config.直升物品3 = 直升物品7);
                        break;
                    }
                case nameof(S_直升物品4):
                    {
                        int num2 = (Settings.Default.直升物品4 = (int)control.Value);
                        int 直升物品6 = num2;
                        int num284 = (Config.直升物品4 = 直升物品6);
                        break;
                    }
                case nameof(S_直升物品5):
                    {
                        int num2 = (Settings.Default.直升物品5 = (int)control.Value);
                        int 直升物品5 = num2;
                        int num282 = (Config.直升物品5 = 直升物品5);
                        break;
                    }
                case nameof(S_直升物品6):
                    {
                        int num2 = (Settings.Default.直升物品6 = (int)control.Value);
                        int 直升物品4 = num2;
                        int num280 = (Config.直升物品6 = 直升物品4);
                        break;
                    }
                case nameof(S_直升物品7):
                    {
                        int num2 = (Settings.Default.直升物品7 = (int)control.Value);
                        int 直升物品3 = num2;
                        int num278 = (Config.直升物品7 = 直升物品3);
                        break;
                    }
                case nameof(S_直升物品8):
                    {
                        int num2 = (Settings.Default.直升物品8 = (int)control.Value);
                        int 直升物品2 = num2;
                        int num276 = (Config.直升物品8 = 直升物品2);
                        break;
                    }
                case nameof(S_直升物品9):
                    {
                        int num2 = (Settings.Default.直升物品9 = (int)control.Value);
                        int 直升物品 = num2;
                        int num274 = (Config.直升物品9 = 直升物品);
                        break;
                    }
                case nameof(S_直升等级1):
                    {
                        int num2 = (Settings.Default.直升等级1 = (int)control.Value);
                        int 直升等级9 = num2;
                        int num272 = (Config.直升等级1 = 直升等级9);
                        break;
                    }
                case nameof(S_直升等级2):
                    {
                        int num2 = (Settings.Default.直升等级2 = (int)control.Value);
                        int 直升等级8 = num2;
                        int num270 = (Config.直升等级2 = 直升等级8);
                        break;
                    }
                case nameof(S_直升等级3):
                    {
                        int num2 = (Settings.Default.直升等级3 = (int)control.Value);
                        int 直升等级7 = num2;
                        int num268 = (Config.直升等级3 = 直升等级7);
                        break;
                    }
                case nameof(S_直升等级4):
                    {
                        int num2 = (Settings.Default.直升等级4 = (int)control.Value);
                        int 直升等级6 = num2;
                        int num266 = (Config.直升等级4 = 直升等级6);
                        break;
                    }
                case nameof(S_直升等级5):
                    {
                        int num2 = (Settings.Default.直升等级5 = (int)control.Value);
                        int 直升等级5 = num2;
                        int num264 = (Config.直升等级5 = 直升等级5);
                        break;
                    }
                case nameof(S_直升等级6):
                    {
                        int num2 = (Settings.Default.直升等级6 = (int)control.Value);
                        int 直升等级4 = num2;
                        int num262 = (Config.直升等级6 = 直升等级4);
                        break;
                    }
                case nameof(S_直升等级7):
                    {
                        int num2 = (Settings.Default.直升等级7 = (int)control.Value);
                        int 直升等级3 = num2;
                        int num260 = (Config.直升等级7 = 直升等级3);
                        break;
                    }
                case nameof(S_直升等级8):
                    {
                        int num2 = (Settings.Default.直升等级8 = (int)control.Value);
                        int 直升等级2 = num2;
                        int num258 = (Config.直升等级8 = 直升等级2);
                        break;
                    }
                case nameof(S_直升等级9):
                    {
                        int num2 = (Settings.Default.直升等级9 = (int)control.Value);
                        int 直升等级 = num2;
                        int num256 = (Config.直升等级9 = 直升等级);
                        break;
                    }
                case nameof(S_直升经验1):
                    {
                        int num2 = (Settings.Default.直升经验1 = (int)control.Value);
                        int 直升经验9 = num2;
                        int num254 = (Config.直升经验1 = 直升经验9);
                        break;
                    }
                case nameof(S_直升经验2):
                    {
                        int num2 = (Settings.Default.直升经验2 = (int)control.Value);
                        int 直升经验8 = num2;
                        int num252 = (Config.直升经验2 = 直升经验8);
                        break;
                    }
                case nameof(S_直升经验3):
                    {
                        int num2 = (Settings.Default.直升经验3 = (int)control.Value);
                        int 直升经验7 = num2;
                        int num250 = (Config.直升经验3 = 直升经验7);
                        break;
                    }
                case nameof(S_直升经验4):
                    {
                        int num2 = (Settings.Default.直升经验4 = (int)control.Value);
                        int 直升经验6 = num2;
                        int num248 = (Config.直升经验4 = 直升经验6);
                        break;
                    }
                case nameof(S_直升经验5):
                    {
                        int num2 = (Settings.Default.直升经验5 = (int)control.Value);
                        int 直升经验5 = num2;
                        int num246 = (Config.直升经验5 = 直升经验5);
                        break;
                    }
                case nameof(S_直升经验6):
                    {
                        int num2 = (Settings.Default.直升经验6 = (int)control.Value);
                        int 直升经验4 = num2;
                        int num244 = (Config.直升经验6 = 直升经验4);
                        break;
                    }
                case nameof(S_直升经验7):
                    {
                        int num2 = (Settings.Default.直升经验7 = (int)control.Value);
                        int 直升经验3 = num2;
                        int num242 = (Config.直升经验7 = 直升经验3);
                        break;
                    }
                case nameof(S_直升经验8):
                    {
                        int num2 = (Settings.Default.直升经验8 = (int)control.Value);
                        int 直升经验2 = num2;
                        int num240 = (Config.直升经验8 = 直升经验2);
                        break;
                    }
                case nameof(S_直升经验9):
                    {
                        int num2 = (Settings.Default.直升经验9 = (int)control.Value);
                        int 直升经验 = num2;
                        int num238 = (Config.直升经验9 = 直升经验);
                        break;
                    }
                case nameof(S_充值模块格式):
                    {
                        int num2 = (Settings.Default.充值模块格式 = (int)control.Value);
                        int 充值模块格式 = num2;
                        int num236 = (Config.充值模块格式 = 充值模块格式);
                        break;
                    }
                case nameof(UpgradeXPLevel1):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel1 = (int)control.Value);
                        int num233 = num2;
                        int num234 = (Config.UpgradeXPLevel1 = num233);
                        break;
                    }
                case nameof(UpgradeXPLevel2):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel2 = (int)control.Value);
                        int num230 = num2;
                        int num231 = (Config.UpgradeXPLevel2 = num230);
                        break;
                    }
                case nameof(UpgradeXPLevel3):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel3 = (int)control.Value);
                        int num227 = num2;
                        int num228 = (Config.UpgradeXPLevel3 = num227);
                        break;
                    }
                case nameof(UpgradeXPLevel4):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel4 = (int)control.Value);
                        int num224 = num2;
                        int num225 = (Config.UpgradeXPLevel4 = num224);
                        break;
                    }
                case nameof(UpgradeXPLevel5):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel5 = (int)control.Value);
                        int num221 = num2;
                        int num222 = (Config.UpgradeXPLevel5 = num221);
                        break;
                    }
                case nameof(UpgradeXPLevel6):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel6 = (int)control.Value);
                        int num218 = num2;
                        int num219 = (Config.UpgradeXPLevel6 = num218);
                        break;
                    }
                case nameof(UpgradeXPLevel7):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel7 = (int)control.Value);
                        int num215 = num2;
                        int num216 = (Config.UpgradeXPLevel7 = num215);
                        break;
                    }
                case nameof(UpgradeXPLevel8):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel8 = (int)control.Value);
                        int num212 = num2;
                        int num213 = (Config.UpgradeXPLevel8 = num212);
                        break;
                    }
                case nameof(UpgradeXPLevel9):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel9 = (int)control.Value);
                        int num209 = num2;
                        int num210 = (Config.UpgradeXPLevel9 = num209);
                        break;
                    }
                case nameof(UpgradeXPLevel10):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel10 = (int)control.Value);
                        int num206 = num2;
                        int num207 = (Config.UpgradeXPLevel10 = num206);
                        break;
                    }
                case nameof(UpgradeXPLevel11):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel11 = (int)control.Value);
                        int num203 = num2;
                        int num204 = (Config.UpgradeXPLevel11 = num203);
                        break;
                    }
                case nameof(UpgradeXPLevel12):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel12 = (int)control.Value);
                        int num200 = num2;
                        int num201 = (Config.UpgradeXPLevel12 = num200);
                        break;
                    }
                case nameof(UpgradeXPLevel13):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel13 = (int)control.Value);
                        int num197 = num2;
                        int num198 = (Config.UpgradeXPLevel13 = num197);
                        break;
                    }
                case nameof(UpgradeXPLevel14):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel14 = (int)control.Value);
                        int num194 = num2;
                        int num195 = (Config.UpgradeXPLevel14 = num194);
                        break;
                    }
                case nameof(UpgradeXPLevel15):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel15 = (int)control.Value);
                        int num191 = num2;
                        int num192 = (Config.UpgradeXPLevel15 = num191);
                        break;
                    }
                case nameof(UpgradeXPLevel16):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel16 = (int)control.Value);
                        int num188 = num2;
                        int num189 = (Config.UpgradeXPLevel16 = num188);
                        break;
                    }
                case nameof(UpgradeXPLevel17):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel17 = (int)control.Value);
                        int num185 = num2;
                        int num186 = (Config.UpgradeXPLevel17 = num185);
                        break;
                    }
                case nameof(UpgradeXPLevel18):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel18 = (int)control.Value);
                        int num182 = num2;
                        int num183 = (Config.UpgradeXPLevel18 = num182);
                        break;
                    }
                case nameof(UpgradeXPLevel19):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel19 = (int)control.Value);
                        int num179 = num2;
                        int num180 = (Config.UpgradeXPLevel19 = num179);
                        break;
                    }
                case nameof(UpgradeXPLevel20):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel20 = (int)control.Value);
                        int num176 = num2;
                        int num177 = (Config.UpgradeXPLevel20 = num176);
                        break;
                    }
                case nameof(UpgradeXPLevel21):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel21 = (int)control.Value);
                        int num173 = num2;
                        int num174 = (Config.UpgradeXPLevel21 = num173);
                        break;
                    }
                case nameof(UpgradeXPLevel22):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel22 = (int)control.Value);
                        int num170 = num2;
                        int num171 = (Config.UpgradeXPLevel22 = num170);
                        break;
                    }
                case nameof(UpgradeXPLevel23):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel23 = (int)control.Value);
                        int num167 = num2;
                        int num168 = (Config.UpgradeXPLevel23 = num167);
                        break;
                    }
                case nameof(UpgradeXPLevel24):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel24 = (int)control.Value);
                        int num164 = num2;
                        int num165 = (Config.UpgradeXPLevel24 = num164);
                        break;
                    }
                case nameof(UpgradeXPLevel25):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel25 = (int)control.Value);
                        int num161 = num2;
                        int num162 = (Config.UpgradeXPLevel25 = num161);
                        break;
                    }
                case nameof(UpgradeXPLevel26):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel26 = (int)control.Value);
                        int num158 = num2;
                        int num159 = (Config.UpgradeXPLevel26 = num158);
                        break;
                    }
                case nameof(UpgradeXPLevel27):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel27 = (int)control.Value);
                        int num155 = num2;
                        int num156 = (Config.UpgradeXPLevel27 = num155);
                        break;
                    }
                case nameof(UpgradeXPLevel28):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel28 = (int)control.Value);
                        int num152 = num2;
                        int num153 = (Config.UpgradeXPLevel28 = num152);
                        break;
                    }
                case nameof(UpgradeXPLevel29):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel29 = (int)control.Value);
                        int num149 = num2;
                        int num150 = (Config.UpgradeXPLevel29 = num149);
                        break;
                    }
                case nameof(UpgradeXPLevel30):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel30 = (int)control.Value);
                        int num146 = num2;
                        int num147 = (Config.UpgradeXPLevel30 = num146);
                        break;
                    }
                case nameof(UpgradeXPLevel31):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel31 = (int)control.Value);
                        int num143 = num2;
                        int num144 = (Config.UpgradeXPLevel31 = num143);
                        break;
                    }
                case nameof(UpgradeXPLevel32):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel32 = (int)control.Value);
                        int num140 = num2;
                        int num141 = (Config.UpgradeXPLevel32 = num140);
                        break;
                    }
                case nameof(UpgradeXPLevel33):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel33 = (int)control.Value);
                        int num137 = num2;
                        int num138 = (Config.UpgradeXPLevel33 = num137);
                        break;
                    }
                case nameof(UpgradeXPLevel34):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel34 = (int)control.Value);
                        int num134 = num2;
                        int num135 = (Config.UpgradeXPLevel34 = num134);
                        break;
                    }
                case nameof(UpgradeXPLevel35):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel35 = (int)control.Value);
                        int num131 = num2;
                        int num132 = (Config.UpgradeXPLevel35 = num131);
                        break;
                    }
                case nameof(UpgradeXPLevel36):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel36 = (int)control.Value);
                        int num128 = num2;
                        int num129 = (Config.UpgradeXPLevel36 = num128);
                        break;
                    }
                case nameof(UpgradeXPLevel37):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel37 = (int)control.Value);
                        int num125 = num2;
                        int num126 = (Config.UpgradeXPLevel37 = num125);
                        break;
                    }
                case nameof(UpgradeXPLevel38):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel38 = (int)control.Value);
                        int num122 = num2;
                        int num123 = (Config.UpgradeXPLevel38 = num122);
                        break;
                    }
                case nameof(UpgradeXPLevel39):
                    {
                        int num2 = (Settings.Default.UpgradeXPLevel39 = (int)control.Value);
                        int num119 = num2;
                        int num120 = (Config.UpgradeXPLevel39 = num119);
                        break;
                    }
                case nameof(DefaultSkillLevel):
                    {
                        int num2 = (Settings.Default.DefaultSkillLevel = (int)control.Value);
                        int num116 = num2;
                        int num117 = (Config.DefaultSkillLevel = num116);
                        break;
                    }
                case nameof(S_其他分解几率一):
                    {
                        int num2 = (Settings.Default.其他分解几率一 = (int)control.Value);
                        int 其他分解几率一 = num2;
                        int num114 = (Config.其他分解几率一 = 其他分解几率一);
                        break;
                    }
                case nameof(S_其他分解几率二):
                    {
                        int num2 = (Settings.Default.其他分解几率二 = (int)control.Value);
                        int 其他分解几率二 = num2;
                        int num112 = (Config.其他分解几率二 = 其他分解几率二);
                        break;
                    }
                case nameof(S_其他分解几率三):
                    {
                        Config.其他分解几率三 = Settings.Default.其他分解几率三 = (int)control.Value;
                        break;
                    }
                case nameof(S_其他分解几率四):
                    {
                        Config.其他分解几率四 = Settings.Default.其他分解几率四 = (int)control.Value;
                        break;
                    }
                case nameof(S_其他分解数量一):
                    {
                        Config.其他分解数量一 = Settings.Default.其他分解数量一 = (int)control.Value;
                        break;
                    }
                case nameof(S_其他分解数量二):
                    {
                        Config.其他分解数量二 = Settings.Default.其他分解数量二 = (int)control.Value;
                        break;
                    }
                case nameof(S_其他分解数量三):
                    {
                        Config.其他分解数量三 = Settings.Default.其他分解数量三 = (int)control.Value;
                        break;
                    }
                case nameof(S_其他分解数量四):
                    {
                        Config.其他分解数量四 = Settings.Default.其他分解数量四 = (int)control.Value;
                        break;
                    }
                case nameof(S_其他分解开关):
                    {
                        int num2 = (Settings.Default.其他分解开关 = (int)control.Value);
                        int 其他分解开关 = num2;
                        int num98 = (Config.其他分解开关 = 其他分解开关);
                        break;
                    }
                case nameof(S_沃玛分解几率一):
                    {
                        int num2 = (Settings.Default.沃玛分解几率一 = (int)control.Value);
                        int 沃玛分解几率一 = num2;
                        int num96 = (Config.沃玛分解几率一 = 沃玛分解几率一);
                        break;
                    }
                case nameof(S_沃玛分解几率二):
                    {
                        int num2 = (Settings.Default.沃玛分解几率二 = (int)control.Value);
                        int 沃玛分解几率二 = num2;
                        int num94 = (Config.沃玛分解几率二 = 沃玛分解几率二);
                        break;
                    }
                case nameof(S_沃玛分解几率三):
                    {
                        int num2 = (Settings.Default.沃玛分解几率三 = (int)control.Value);
                        int 沃玛分解几率三 = num2;
                        int num92 = (Config.沃玛分解几率三 = 沃玛分解几率三);
                        break;
                    }
                case nameof(S_沃玛分解几率四):
                    {
                        int num2 = (Settings.Default.沃玛分解几率四 = (int)control.Value);
                        int 沃玛分解几率四 = num2;
                        int num90 = (Config.沃玛分解几率四 = 沃玛分解几率四);
                        break;
                    }
                case nameof(S_沃玛分解数量一):
                    {
                        int num2 = (Settings.Default.沃玛分解数量一 = (int)control.Value);
                        int 沃玛分解数量一 = num2;
                        int num88 = (Config.沃玛分解数量一 = 沃玛分解数量一);
                        break;
                    }
                case nameof(S_沃玛分解数量二):
                    {
                        int num2 = (Settings.Default.沃玛分解数量二 = (int)control.Value);
                        int 沃玛分解数量二 = num2;
                        int num86 = (Config.沃玛分解数量二 = 沃玛分解数量二);
                        break;
                    }
                case nameof(S_沃玛分解数量三):
                    {
                        int num2 = (Settings.Default.沃玛分解数量三 = (int)control.Value);
                        int 沃玛分解数量三 = num2;
                        int num84 = (Config.沃玛分解数量三 = 沃玛分解数量三);
                        break;
                    }
                case nameof(S_沃玛分解数量四):
                    {
                        int num2 = (Settings.Default.沃玛分解数量四 = (int)control.Value);
                        int 沃玛分解数量四 = num2;
                        int num82 = (Config.沃玛分解数量四 = 沃玛分解数量四);
                        break;
                    }
                case nameof(S_沃玛分解开关):
                    {
                        int num2 = (Settings.Default.沃玛分解开关 = (int)control.Value);
                        int 沃玛分解开关 = num2;
                        int num80 = (Config.沃玛分解开关 = 沃玛分解开关);
                        break;
                    }
                case nameof(拾取地图控制1):
                    {
                        int num2 = (Settings.Default.拾取地图控制1 = (int)control.Value);
                        int num77 = num2;
                        int num78 = (Config.拾取地图控制1 = num77);
                        break;
                    }
                case nameof(拾取地图控制2):
                    {
                        int num2 = (Settings.Default.拾取地图控制2 = (int)control.Value);
                        int num74 = num2;
                        int num75 = (Config.拾取地图控制2 = num74);
                        break;
                    }
                case nameof(拾取地图控制3):
                    {
                        int num2 = (Settings.Default.拾取地图控制3 = (int)control.Value);
                        int num71 = num2;
                        int num72 = (Config.拾取地图控制3 = num71);
                        break;
                    }
                case nameof(拾取地图控制4):
                    {
                        int num2 = (Settings.Default.拾取地图控制4 = (int)control.Value);
                        int num68 = num2;
                        int num69 = (Config.拾取地图控制4 = num68);
                        break;
                    }
                case nameof(拾取地图控制5):
                    {
                        int num2 = (Settings.Default.拾取地图控制5 = (int)control.Value);
                        int num65 = num2;
                        int num66 = (Config.拾取地图控制5 = num65);
                        break;
                    }
                case nameof(拾取地图控制6):
                    {
                        int num2 = (Settings.Default.拾取地图控制6 = (int)control.Value);
                        int num62 = num2;
                        int num63 = (Config.拾取地图控制6 = num62);
                        break;
                    }
                case nameof(拾取地图控制7):
                    {
                        int num2 = (Settings.Default.拾取地图控制7 = (int)control.Value);
                        int num59 = num2;
                        int num60 = (Config.拾取地图控制7 = num59);
                        break;
                    }
                case nameof(拾取地图控制8):
                    {
                        int num2 = (Settings.Default.拾取地图控制8 = (int)control.Value);
                        int num56 = num2;
                        int num57 = (Config.拾取地图控制8 = num56);
                        break;
                    }
                case nameof(沙城捐献货币类型):
                    {
                        int num2 = (Settings.Default.沙城捐献货币类型 = (int)control.Value);
                        int num53 = num2;
                        int num54 = (Config.沙城捐献货币类型 = num53);
                        break;
                    }
                case nameof(沙城捐献支付数量):
                    {
                        int num2 = (Settings.Default.沙城捐献支付数量 = (int)control.Value);
                        int num50 = num2;
                        int num51 = (Config.沙城捐献支付数量 = num50);
                        break;
                    }
                case nameof(沙城捐献获得物品1):
                    {
                        int num2 = (Settings.Default.沙城捐献获得物品1 = (int)control.Value);
                        int num47 = num2;
                        int num48 = (Config.沙城捐献获得物品1 = num47);
                        break;
                    }
                case nameof(沙城捐献获得物品2):
                    {
                        int num2 = (Settings.Default.沙城捐献获得物品2 = (int)control.Value);
                        int num44 = num2;
                        int num45 = (Config.沙城捐献获得物品2 = num44);
                        break;
                    }
                case nameof(沙城捐献获得物品3):
                    {
                        int num2 = (Settings.Default.沙城捐献获得物品3 = (int)control.Value);
                        int num41 = num2;
                        int num42 = (Config.沙城捐献获得物品3 = num41);
                        break;
                    }
                case nameof(沙城捐献物品数量1):
                    {
                        int num2 = (Settings.Default.沙城捐献物品数量1 = (int)control.Value);
                        int num38 = num2;
                        int num39 = (Config.沙城捐献物品数量1 = num38);
                        break;
                    }
                case nameof(沙城捐献物品数量2):
                    {
                        int num2 = (Settings.Default.沙城捐献物品数量2 = (int)control.Value);
                        int num35 = num2;
                        int num36 = (Config.沙城捐献物品数量2 = num35);
                        break;
                    }
                case nameof(沙城捐献物品数量3):
                    {
                        int num2 = (Settings.Default.沙城捐献物品数量3 = (int)control.Value);
                        int num32 = num2;
                        int num33 = (Config.沙城捐献物品数量3 = num32);
                        break;
                    }
                case nameof(沙城捐献赞助金额):
                    {
                        int num2 = (Settings.Default.沙城捐献赞助金额 = (int)control.Value);
                        int num29 = num2;
                        int num30 = (Config.沙城捐献赞助金额 = num29);
                        break;
                    }
                case nameof(沙城捐献赞助人数):
                    {
                        int num2 = (Settings.Default.沙城捐献赞助人数 = (int)control.Value);
                        int num26 = num2;
                        int num27 = (Config.沙城捐献赞助人数 = num26);
                        break;
                    }
                case nameof(雕爷激活灵符需求):
                    {
                        Config.雕爷激活灵符需求 = Settings.Default.雕爷激活灵符需求 = (int)control.Value;
                        break;
                    }
                case nameof(雕爷1号位灵符):
                    {
                        Config.雕爷1号位灵符 = Settings.Default.雕爷1号位灵符 = (int)control.Value;
                        break;
                    }
                case nameof(雕爷2号位灵符):
                    {
                        Config.雕爷2号位灵符 = Settings.Default.雕爷2号位灵符 = (int)control.Value;
                        break;
                    }
                case nameof(雕爷3号位灵符):
                    {
                        Config.雕爷3号位灵符 = Settings.Default.雕爷3号位灵符 = (int)control.Value;
                        break;
                    }
                case nameof(雕爷1号位铭文石):
                    {
                        Config.雕爷1号位铭文石 = Settings.Default.雕爷1号位铭文石 = (int)control.Value;
                        break;
                    }
                case nameof(雕爷2号位铭文石):
                    {
                        Config.雕爷2号位铭文石 = Settings.Default.雕爷2号位铭文石 = (int)control.Value;
                        break;
                    }
                case nameof(雕爷3号位铭文石):
                    {
                        Config.雕爷3号位铭文石 = Settings.Default.雕爷3号位铭文石 = (int)control.Value;
                        break;
                    }
                case nameof(S_称号范围拾取判断1):
                    {
                        Config.称号范围拾取判断1 = Settings.Default.称号范围拾取判断1 = (int)control.Value;
                        break;
                    }
                default:
                    MessageBox.Show("Unknown Control! " + control.Name);
                    break;
            }
            Settings.Default.Save();
        }
    }


    private void 执行GM命令行_Press(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar != Convert.ToChar(13) || GM命令文本.Text.Length <= 0)
        {
            return;
        }
        主选项卡.SelectedIndex = 0;
        日志选项卡.SelectedIndex = 2;
        AddCommandLog("=> " + GM命令文本.Text);
        GMCommand cmd;
        if (GM命令文本.Text[0] != '@')
        {
            AddCommandLog("<= 命令解析错误, GM命令必须以 '@' 开头. 输入 '@查看命令' 获取所有受支持的命令格式");
        }
        else if (GM命令文本.Text.Trim('@', ' ').Length == 0)
        {
            AddCommandLog("<= 命令解析错误, GM命令不能为空. 输入 '@查看命令' 获取所有受支持的命令格式");
        }
        else if (GMCommand.ParseCommand(GM命令文本.Text, out cmd))
        {
            if (cmd.Priority == ExecutionPriority.Immediate)
            {
                cmd.ExecuteCommand();
            }
            else if (cmd.Priority == ExecutionPriority.ImmediateBackground)
            {
                if (SEngine.Running)
                {
                    SEngine.ExternalCommands.Enqueue(cmd);
                }
                else
                {
                    cmd.ExecuteCommand();
                }
            }
            else if (cmd.Priority == ExecutionPriority.Background)
            {
                if (SEngine.Running)
                {
                    SEngine.ExternalCommands.Enqueue(cmd);
                }
                else
                {
                    AddCommandLog("<= 命令执行失败, 当前命令只能在服务器运行时执行, 请先启动服务器");
                }
            }
            else if (cmd.Priority == ExecutionPriority.Idle)
            {
                if (!SEngine.Running && (SEngine.MainThread == null || !SEngine.MainThread.IsAlive))
                {
                    cmd.ExecuteCommand();
                }
                else
                {
                    AddCommandLog("<= 命令执行失败, 当前命令只能在服务器未运行时执行, 请先关闭服务器");
                }
            }
            e.Handled = true;
        }
        GM命令文本.Clear();
    }

    private void 角色右键菜单_Click(object sender, EventArgs e)
    {
        if (sender is ToolStripMenuItem toolStripMenuItem && Main.角色浏览表.Rows.Count > 0 && Main.角色浏览表.SelectedRows.Count > 0)
        {
            DataRow row = (Main.角色浏览表.Rows[Main.角色浏览表.SelectedRows[0].Index].DataBoundItem as DataRowView).Row;
            if (toolStripMenuItem.Name == "右键菜单_复制账号名字")
            {
                Clipboard.SetDataObject(row["所属账号"]);
            }
            if (toolStripMenuItem.Name == "右键菜单_复制角色名字")
            {
                Clipboard.SetDataObject(row["角色名字"]);
            }
            if (toolStripMenuItem.Name == "右键菜单_复制网络地址")
            {
                Clipboard.SetDataObject(row["网络地址"]);
            }
            if (toolStripMenuItem.Name == "右键菜单_复制物理地址")
            {
                Clipboard.SetDataObject(row["物理地址"]);
            }
        }
    }

    private void 添加公告按钮_Click(object sender, EventArgs e)
    {
        int index = 公告浏览表.Rows.Add();
        公告浏览表.Rows[index].Cells["公告间隔"].Value = 5;
        公告浏览表.Rows[index].Cells["公告次数"].Value = 1;
        公告浏览表.Rows[index].Cells["公告内容"].Value = "请输入公告内容";
        string text = null;
        string text2;
        string text3;
        object obj3;
        string text4;
        for (int i = 0; i < 公告浏览表.Rows.Count; text4 = (string)obj3, text = text + text2 + "\t" + text3 + "\t" + text4 + "\r\n", i++)
        {
            object value = 公告浏览表.Rows[i].Cells["公告间隔"].Value;
            object obj;
            if (value == null)
            {
                obj = null;
            }
            else
            {
                obj = value.ToString();
                if (obj != null)
                {
                    goto IL_00fc;
                }
            }
            obj = "";
            goto IL_00fc;
        IL_00fc:
            text2 = (string)obj;
            object value2 = 公告浏览表.Rows[i].Cells["公告次数"].Value;
            object obj2;
            if (value2 == null)
            {
                obj2 = null;
            }
            else
            {
                obj2 = value2.ToString();
                if (obj2 != null)
                {
                    goto IL_0161;
                }
            }
            obj2 = "";
            goto IL_0161;
        IL_0161:
            text3 = (string)obj2;
            object value3 = 公告浏览表.Rows[i].Cells["公告内容"].Value;
            if (value3 == null)
            {
                obj3 = null;
            }
            else
            {
                obj3 = value3.ToString();
                if (obj3 != null)
                {
                    continue;
                }
            }
            obj3 = "";
        }
        Settings.Default.系统公告内容 = text;
        Settings.Default.Save();
    }

    private void 删除公告按钮_Click(object sender, EventArgs e)
    {
        if (公告浏览表.Rows.Count == 0 || 公告浏览表.SelectedRows.Count == 0)
        {
            return;
        }
        DataGridViewRow key = 公告浏览表.Rows[公告浏览表.SelectedRows[0].Index];
        公告数据表.Remove(key);
        公告浏览表.Rows.RemoveAt(公告浏览表.SelectedRows[0].Index);
        string text = null;
        string text2;
        string text3;
        object obj3;
        string text4;
        for (int i = 0; i < 公告浏览表.Rows.Count; text4 = (string)obj3, text = text + text2 + "\t" + text3 + "\t" + text4 + "\r\n", i++)
        {
            object value = 公告浏览表.Rows[i].Cells["公告间隔"].Value;
            object obj;
            if (value == null)
            {
                obj = null;
            }
            else
            {
                obj = value.ToString();
                if (obj != null)
                {
                    goto IL_00f6;
                }
            }
            obj = "";
            goto IL_00f6;
        IL_00f6:
            text2 = (string)obj;
            object value2 = 公告浏览表.Rows[i].Cells["公告次数"].Value;
            object obj2;
            if (value2 == null)
            {
                obj2 = null;
            }
            else
            {
                obj2 = value2.ToString();
                if (obj2 != null)
                {
                    goto IL_015b;
                }
            }
            obj2 = "";
            goto IL_015b;
        IL_015b:
            text3 = (string)obj2;
            object value3 = 公告浏览表.Rows[i].Cells["公告内容"].Value;
            if (value3 == null)
            {
                obj3 = null;
            }
            else
            {
                obj3 = value3.ToString();
                if (obj3 != null)
                {
                    continue;
                }
            }
            obj3 = "";
        }
        Settings.Default.系统公告内容 = text;
        Settings.Default.Save();
    }

    private void 开始公告按钮_Click(object sender, EventArgs e)
    {
        if (SEngine.Running && stopServerToolStripMenuItem.Enabled)
        {
            if (公告浏览表.Rows.Count == 0 || 公告浏览表.SelectedRows.Count == 0)
            {
                return;
            }
            DataGridViewRow dataGridViewRow = 公告浏览表.Rows[公告浏览表.SelectedRows[0].Index];
            if (int.TryParse(dataGridViewRow.Cells["公告间隔"].Value.ToString(), out var result) && result > 0)
            {
                if (int.TryParse(dataGridViewRow.Cells["公告次数"].Value.ToString(), out var result2) && result2 > 0)
                {
                    if (dataGridViewRow.Cells["公告内容"].Value != null && dataGridViewRow.Cells["公告内容"].Value.ToString().Length > 0)
                    {
                        dataGridViewRow.ReadOnly = true;
                        dataGridViewRow.Cells["公告状态"].Value = "√";
                        dataGridViewRow.Cells["剩余次数"].Value = dataGridViewRow.Cells["公告次数"].Value;
                        公告数据表.Add(dataGridViewRow, DateTime.Now);
                        开始公告按钮.Enabled = false;
                        停止公告按钮.Enabled = true;
                    }
                    else
                    {
                        Task.Run(delegate
                        {
                            MessageBox.Show("系统公告未能开启, 公告内容不能为空");
                        });
                    }
                }
                else
                {
                    Task.Run(delegate
                    {
                        MessageBox.Show("系统公告未能开启, 公告次数必须为大于0的整数");
                    });
                }
            }
            else
            {
                Task.Run(delegate
                {
                    MessageBox.Show("系统公告未能开启, 公告间隔必须为大于0的整数");
                });
            }
        }
        else
        {
            Task.Run(delegate
            {
                MessageBox.Show("服务器未启动, 请先开启服务器");
            });
        }
    }

    private void 停止公告按钮_Click(object sender, EventArgs e)
    {
        if (公告浏览表.Rows.Count != 0 && 公告浏览表.SelectedRows.Count != 0)
        {
            DataGridViewRow dataGridViewRow = 公告浏览表.Rows[公告浏览表.SelectedRows[0].Index];
            公告数据表.Remove(dataGridViewRow);
            dataGridViewRow.ReadOnly = false;
            dataGridViewRow.Cells["公告状态"].Value = "";
            dataGridViewRow.Cells["公告计时"].Value = "";
            dataGridViewRow.Cells["剩余次数"].Value = 0;
            开始公告按钮.Enabled = true;
            停止公告按钮.Enabled = false;
        }
    }

    private void 定时发送公告_Tick(object sender, EventArgs e)
    {
        if (!SEngine.Running || 公告数据表.Count == 0)
        {
            return;
        }
        DateTime now = DateTime.Now;
        foreach (KeyValuePair<DataGridViewRow, DateTime> item in 公告数据表.ToList())
        {
            item.Key.Cells["公告计时"].Value = (item.Value - now).ToString("hh\\:mm\\:ss");
            if (!(now > item.Value))
            {
                continue;
            }
            NetworkManager.SendAnnouncement(item.Key.Cells["公告内容"].Value.ToString(), rolling: true);
            公告数据表[item.Key] = now.AddMinutes(Convert.ToInt32(item.Key.Cells["公告间隔"].Value));
            int num = Convert.ToInt32(item.Key.Cells["剩余次数"].Value) - 1;
            item.Key.Cells["剩余次数"].Value = num;
            if (num <= 0)
            {
                公告数据表.Remove(item.Key);
                item.Key.ReadOnly = false;
                item.Key.Cells["公告状态"].Value = "";
                if (item.Key.Selected)
                {
                    开始公告按钮.Enabled = true;
                    停止公告按钮.Enabled = false;
                }
            }
        }
    }

    private void 公告浏览表_SelectionChanged(object sender, EventArgs e)
    {
        if (公告浏览表.Rows.Count != 0 && 公告浏览表.SelectedRows.Count != 0)
        {
            DataGridViewRow key = 公告浏览表.Rows[公告浏览表.SelectedRows[0].Index];
            if (公告数据表.ContainsKey(key))
            {
                开始公告按钮.Enabled = false;
                停止公告按钮.Enabled = true;
            }
            else
            {
                开始公告按钮.Enabled = true;
                停止公告按钮.Enabled = false;
            }
        }
        else
        {
            停止公告按钮.Enabled = false;
            开始公告按钮.Enabled = false;
        }
    }

    private void 公告浏览表_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
        string text = null;
        string text2;
        string text3;
        object obj3;
        string text4;
        for (int i = 0; i < 公告浏览表.Rows.Count; text4 = (string)obj3, text = text + text2 + "\t" + text3 + "\t" + text4 + "\r\n", i++)
        {
            object value = 公告浏览表.Rows[i].Cells["公告间隔"].Value;
            object obj;
            if (value == null)
            {
                obj = null;
            }
            else
            {
                obj = value.ToString();
                if (obj != null)
                {
                    goto IL_0068;
                }
            }
            obj = "";
            goto IL_0068;
        IL_0068:
            text2 = (string)obj;
            object value2 = 公告浏览表.Rows[i].Cells["公告次数"].Value;
            object obj2;
            if (value2 == null)
            {
                obj2 = null;
            }
            else
            {
                obj2 = value2.ToString();
                if (obj2 != null)
                {
                    goto IL_00cd;
                }
            }
            obj2 = "";
            goto IL_00cd;
        IL_00cd:
            text3 = (string)obj2;
            object value3 = 公告浏览表.Rows[i].Cells["公告内容"].Value;
            if (value3 == null)
            {
                obj3 = null;
            }
            else
            {
                obj3 = value3.ToString();
                if (obj3 != null)
                {
                    continue;
                }
            }
            obj3 = "";
        }
        Settings.Default.系统公告内容 = text;
        Settings.Default.Save();
    }

    private void button8_Click(object sender, EventArgs e)
    {
        Process.Start("IEXPLORE.EXE", "https://jq.qq.com/?_wv=1027&k=WS2L3pIf");
    }

    private void S_狂暴名称_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_狂暴名称")
            {
                string text3 = (Settings.Default.狂暴名称 = textBox.Text);
                string 狂暴名称 = text3;
                string text4 = (Config.狂暴名称 = 狂暴名称);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_自定义物品内容一_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_自定义物品内容一")
            {
                string text3 = (Settings.Default.自定义物品内容一 = textBox.Text);
                string 自定义物品内容一 = text3;
                string text4 = (Config.自定义物品内容一 = 自定义物品内容一);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_自定义物品内容二_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_自定义物品内容二")
            {
                string text3 = (Settings.Default.自定义物品内容二 = textBox.Text);
                string 自定义物品内容二 = text3;
                string text4 = (Config.自定义物品内容二 = 自定义物品内容二);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_自定义物品内容三_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_自定义物品内容三")
            {
                string text3 = (Settings.Default.自定义物品内容三 = textBox.Text);
                string 自定义物品内容三 = text3;
                string text4 = (Config.自定义物品内容三 = 自定义物品内容三);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_自定义物品内容四_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_自定义物品内容四")
            {
                string text3 = (Settings.Default.自定义物品内容四 = textBox.Text);
                string 自定义物品内容四 = text3;
                string text4 = (Config.自定义物品内容四 = 自定义物品内容四);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_自定义物品内容五_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_自定义物品内容五")
            {
                string text3 = (Settings.Default.自定义物品内容五 = textBox.Text);
                string 自定义物品内容五 = text3;
                string text4 = (Config.自定义物品内容五 = 自定义物品内容五);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_战将特权礼包_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_战将特权礼包")
            {
                string text3 = (Settings.Default.战将特权礼包 = textBox.Text);
                string 战将特权礼包 = text3;
                string text4 = (Config.战将特权礼包 = 战将特权礼包);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_豪杰特权礼包_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_豪杰特权礼包")
            {
                string text3 = (Settings.Default.豪杰特权礼包 = textBox.Text);
                string 豪杰特权礼包 = text3;
                string text4 = (Config.豪杰特权礼包 = 豪杰特权礼包);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_世界BOSS名字_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_世界BOSS名字")
            {
                string text3 = (Settings.Default.世界BOSS名字 = textBox.Text);
                string 世界BOSS名字 = text3;
                string text4 = (Config.世界BOSS名字 = 世界BOSS名字);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS名字一_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS名字一")
            {
                string text3 = (Settings.Default.BOSS名字一 = textBox.Text);
                string bOSS名字一 = text3;
                string text4 = (Config.BOSS名字一 = bOSS名字一);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS一地图名字_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS一地图名字")
            {
                string text3 = (Settings.Default.BOSS一地图名字 = textBox.Text);
                string bOSS一地图名字 = text3;
                string text4 = (Config.BOSS一地图名字 = bOSS一地图名字);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void ButtonMonsterInfo_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadMonsters();
    }

    private void ButtonGuard_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadGuards();
    }

    private void 对话数据_Click(object sender, EventArgs e)
    {
        SystemDataGateway.对话数据();
    }

    private void 游戏地图_Click(object sender, EventArgs e)
    {
        SystemDataGateway.游戏地图();
    }

    private void ButtonTerrain_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadTerrains();
    }

    private void 地图区域_Click(object sender, EventArgs e)
    {
        SystemDataGateway.地图区域();
    }

    private void 传送法阵_Click(object sender, EventArgs e)
    {
        SystemDataGateway.传送法阵();
    }

    private void 怪物刷新_Click(object sender, EventArgs e)
    {
        SystemDataGateway.怪物刷新();
    }

    private void 守卫刷新_Click(object sender, EventArgs e)
    {
        SystemDataGateway.守卫刷新();
    }

    private void 游戏物品_Click(object sender, EventArgs e)
    {
        SystemDataGateway.游戏物品();
    }

    private void 随机属性_Click(object sender, EventArgs e)
    {
        SystemDataGateway.随机属性();
    }

    private void 装备属性_Click(object sender, EventArgs e)
    {
        SystemDataGateway.装备属性();
    }

    private void 游戏商店_Click(object sender, EventArgs e)
    {
        SystemDataGateway.游戏商店();
    }

    private void 珍宝商品_Click(object sender, EventArgs e)
    {
        SystemDataGateway.珍宝商品();
    }

    private void 游戏称号_Click(object sender, EventArgs e)
    {
        SystemDataGateway.游戏称号();
    }

    private void 铭文技能_Click(object sender, EventArgs e)
    {
        SystemDataGateway.铭文技能();
    }

    private void 游戏技能_Click(object sender, EventArgs e)
    {
        SystemDataGateway.游戏技能();
    }

    private void 技能陷阱_Click(object sender, EventArgs e)
    {
        SystemDataGateway.技能陷阱();
    }

    private void 游戏Buff_Click(object sender, EventArgs e)
    {
        SystemDataGateway.游戏Buff();
    }

    private void 游戏坐骑_Click(object sender, EventArgs e)
    {
        SystemDataGateway.游戏坐骑();
    }

    private void 合成数据_Click(object sender, EventArgs e)
    {
        SystemDataGateway.合成数据();
    }

    private void VIP数据_Click(object sender, EventArgs e)
    {
        SystemDataGateway.VIP数据();
    }

    private void 宝箱数据_Click(object sender, EventArgs e)
    {
        SystemDataGateway.宝箱数据();
    }

    private void S_BOSS二地图名字_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS二地图名字")
            {
                string text3 = (Settings.Default.BOSS二地图名字 = textBox.Text);
                string bOSS二地图名字 = text3;
                string text4 = (Config.BOSS二地图名字 = bOSS二地图名字);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS三地图名字_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS三地图名字")
            {
                string text3 = (Settings.Default.BOSS三地图名字 = textBox.Text);
                string bOSS三地图名字 = text3;
                string text4 = (Config.BOSS三地图名字 = bOSS三地图名字);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS四地图名字_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS四地图名字")
            {
                string text3 = (Settings.Default.BOSS四地图名字 = textBox.Text);
                string bOSS四地图名字 = text3;
                string text4 = (Config.BOSS四地图名字 = bOSS四地图名字);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS五地图名字_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS五地图名字")
            {
                string text3 = (Settings.Default.BOSS五地图名字 = textBox.Text);
                string bOSS五地图名字 = text3;
                string text4 = (Config.BOSS五地图名字 = bOSS五地图名字);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS名字二_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS名字二")
            {
                string text3 = (Settings.Default.BOSS名字二 = textBox.Text);
                string bOSS名字二 = text3;
                string text4 = (Config.BOSS名字二 = bOSS名字二);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS名字三_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS名字三")
            {
                string text3 = (Settings.Default.BOSS名字三 = textBox.Text);
                string bOSS名字三 = text3;
                string text4 = (Config.BOSS名字三 = bOSS名字三);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS名字四_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS名字四")
            {
                string text3 = (Settings.Default.BOSS名字四 = textBox.Text);
                string bOSS名字四 = text3;
                string text4 = (Config.BOSS名字四 = bOSS名字四);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS名字五_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS名字五")
            {
                string text3 = (Settings.Default.BOSS名字五 = textBox.Text);
                string bOSS名字五 = text3;
                string text4 = (Config.BOSS名字五 = bOSS名字五);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_祖玛分解物品一_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_祖玛分解物品一")
            {
                string text3 = (Settings.Default.祖玛分解物品一 = textBox.Text);
                string 祖玛分解物品一 = text3;
                string text4 = (Config.祖玛分解物品一 = 祖玛分解物品一);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_祖玛分解物品二_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_祖玛分解物品二")
            {
                string text3 = (Settings.Default.祖玛分解物品二 = textBox.Text);
                string 祖玛分解物品二 = text3;
                string text4 = (Config.祖玛分解物品二 = 祖玛分解物品二);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_祖玛分解物品三_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_祖玛分解物品三")
            {
                string text3 = (Settings.Default.祖玛分解物品三 = textBox.Text);
                string 祖玛分解物品三 = text3;
                string text4 = (Config.祖玛分解物品三 = 祖玛分解物品三);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_祖玛分解物品四_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_祖玛分解物品四")
            {
                string text3 = (Settings.Default.祖玛分解物品四 = textBox.Text);
                string 祖玛分解物品四 = text3;
                string text4 = (Config.祖玛分解物品四 = 祖玛分解物品四);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_赤月分解物品一_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_赤月分解物品一")
            {
                string text3 = (Settings.Default.赤月分解物品一 = textBox.Text);
                string 赤月分解物品一 = text3;
                string text4 = (Config.赤月分解物品一 = 赤月分解物品一);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_赤月分解物品二_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_赤月分解物品二")
            {
                string text3 = (Settings.Default.赤月分解物品二 = textBox.Text);
                string 赤月分解物品二 = text3;
                string text4 = (Config.赤月分解物品二 = 赤月分解物品二);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_赤月分解物品三_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_赤月分解物品三")
            {
                string text3 = (Settings.Default.赤月分解物品三 = textBox.Text);
                string 赤月分解物品三 = text3;
                string text4 = (Config.赤月分解物品三 = 赤月分解物品三);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_赤月分解物品四_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_赤月分解物品四")
            {
                string text3 = (Settings.Default.赤月分解物品四 = textBox.Text);
                string 赤月分解物品四 = text3;
                string text4 = (Config.赤月分解物品四 = 赤月分解物品四);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_魔龙分解物品一_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_魔龙分解物品一")
            {
                string text3 = (Settings.Default.魔龙分解物品一 = textBox.Text);
                string 魔龙分解物品一 = text3;
                string text4 = (Config.魔龙分解物品一 = 魔龙分解物品一);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_魔龙分解物品二_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_魔龙分解物品二")
            {
                string text3 = (Settings.Default.魔龙分解物品二 = textBox.Text);
                string 魔龙分解物品二 = text3;
                string text4 = (Config.魔龙分解物品二 = 魔龙分解物品二);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_魔龙分解物品三_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_魔龙分解物品三")
            {
                string text3 = (Settings.Default.魔龙分解物品三 = textBox.Text);
                string 魔龙分解物品三 = text3;
                string text4 = (Config.魔龙分解物品三 = 魔龙分解物品三);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_魔龙分解物品四_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_魔龙分解物品四")
            {
                string text3 = (Settings.Default.魔龙分解物品四 = textBox.Text);
                string 魔龙分解物品四 = text3;
                string text4 = (Config.魔龙分解物品四 = 魔龙分解物品四);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_苍月分解物品一_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_苍月分解物品一")
            {
                string text3 = (Settings.Default.苍月分解物品一 = textBox.Text);
                string 苍月分解物品一 = text3;
                string text4 = (Config.苍月分解物品一 = 苍月分解物品一);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_苍月分解物品二_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_苍月分解物品二")
            {
                string text3 = (Settings.Default.苍月分解物品二 = textBox.Text);
                string 苍月分解物品二 = text3;
                string text4 = (Config.苍月分解物品二 = 苍月分解物品二);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_苍月分解物品三_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_苍月分解物品三")
            {
                string text3 = (Settings.Default.苍月分解物品三 = textBox.Text);
                string 苍月分解物品三 = text3;
                string text4 = (Config.苍月分解物品三 = 苍月分解物品三);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_苍月分解物品四_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_苍月分解物品四")
            {
                string text3 = (Settings.Default.苍月分解物品四 = textBox.Text);
                string 苍月分解物品四 = text3;
                string text4 = (Config.苍月分解物品四 = 苍月分解物品四);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_星王分解物品一_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_星王分解物品一")
            {
                string text3 = (Settings.Default.星王分解物品一 = textBox.Text);
                string 星王分解物品一 = text3;
                string text4 = (Config.星王分解物品一 = 星王分解物品一);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_星王分解物品二_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_星王分解物品二")
            {
                string text3 = (Settings.Default.星王分解物品二 = textBox.Text);
                string 星王分解物品二 = text3;
                string text4 = (Config.星王分解物品二 = 星王分解物品二);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_星王分解物品三_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_星王分解物品三")
            {
                string text3 = (Settings.Default.星王分解物品三 = textBox.Text);
                string 星王分解物品三 = text3;
                string text4 = (Config.星王分解物品三 = 星王分解物品三);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_星王分解物品四_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_星王分解物品四")
            {
                string text3 = (Settings.Default.星王分解物品四 = textBox.Text);
                string 星王分解物品四 = text3;
                string text4 = (Config.星王分解物品四 = 星王分解物品四);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_城主分解物品一_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_城主分解物品一")
            {
                string text3 = (Settings.Default.城主分解物品一 = textBox.Text);
                string 城主分解物品一 = text3;
                string text4 = (Config.城主分解物品一 = 城主分解物品一);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_城主分解物品二_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_城主分解物品二")
            {
                string text3 = (Settings.Default.城主分解物品二 = textBox.Text);
                string 城主分解物品二 = text3;
                string text4 = (Config.城主分解物品二 = 城主分解物品二);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_城主分解物品三_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_城主分解物品三")
            {
                string text3 = (Settings.Default.城主分解物品三 = textBox.Text);
                string 城主分解物品三 = text3;
                string text4 = (Config.城主分解物品三 = 城主分解物品三);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_城主分解物品四_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_城主分解物品四")
            {
                string text3 = (Settings.Default.城主分解物品四 = textBox.Text);
                string 城主分解物品四 = text3;
                string text4 = (Config.城主分解物品四 = 城主分解物品四);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS卷轴怪物一_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS卷轴怪物一")
            {
                string text3 = (Settings.Default.BOSS卷轴怪物一 = textBox.Text);
                string bOSS卷轴怪物一 = text3;
                string text4 = (Config.BOSS卷轴怪物一 = bOSS卷轴怪物一);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS卷轴怪物二_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS卷轴怪物二")
            {
                string text3 = (Settings.Default.BOSS卷轴怪物二 = textBox.Text);
                string bOSS卷轴怪物二 = text3;
                string text4 = (Config.BOSS卷轴怪物二 = bOSS卷轴怪物二);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS卷轴怪物三_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS卷轴怪物三")
            {
                string text3 = (Settings.Default.BOSS卷轴怪物三 = textBox.Text);
                string bOSS卷轴怪物三 = text3;
                string text4 = (Config.BOSS卷轴怪物三 = bOSS卷轴怪物三);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS卷轴怪物四_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS卷轴怪物四")
            {
                string text3 = (Settings.Default.BOSS卷轴怪物四 = textBox.Text);
                string bOSS卷轴怪物四 = text3;
                string text4 = (Config.BOSS卷轴怪物四 = bOSS卷轴怪物四);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS卷轴怪物五_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS卷轴怪物五")
            {
                string text3 = (Settings.Default.BOSS卷轴怪物五 = textBox.Text);
                string bOSS卷轴怪物五 = text3;
                string text4 = (Config.BOSS卷轴怪物五 = bOSS卷轴怪物五);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS卷轴怪物六_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS卷轴怪物六")
            {
                string text3 = (Settings.Default.BOSS卷轴怪物六 = textBox.Text);
                string bOSS卷轴怪物六 = text3;
                string text4 = (Config.BOSS卷轴怪物六 = bOSS卷轴怪物六);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS卷轴怪物七_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS卷轴怪物七")
            {
                string text3 = (Settings.Default.BOSS卷轴怪物七 = textBox.Text);
                string bOSS卷轴怪物七 = text3;
                string text4 = (Config.BOSS卷轴怪物七 = bOSS卷轴怪物七);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS卷轴怪物八_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS卷轴怪物八")
            {
                string text3 = (Settings.Default.BOSS卷轴怪物八 = textBox.Text);
                string bOSS卷轴怪物八 = text3;
                string text4 = (Config.BOSS卷轴怪物八 = bOSS卷轴怪物八);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS卷轴怪物九_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS卷轴怪物九")
            {
                string text3 = (Settings.Default.BOSS卷轴怪物九 = textBox.Text);
                string bOSS卷轴怪物九 = text3;
                string text4 = (Config.BOSS卷轴怪物九 = bOSS卷轴怪物九);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS卷轴怪物十_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS卷轴怪物十")
            {
                string text3 = (Settings.Default.BOSS卷轴怪物十 = textBox.Text);
                string bOSS卷轴怪物十 = text3;
                string text4 = (Config.BOSS卷轴怪物十 = bOSS卷轴怪物十);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS卷轴怪物11_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS卷轴怪物11")
            {
                string text3 = (Settings.Default.BOSS卷轴怪物11 = textBox.Text);
                string bOSS卷轴怪物 = text3;
                string text4 = (Config.BOSS卷轴怪物11 = bOSS卷轴怪物);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS卷轴怪物12_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS卷轴怪物12")
            {
                string text3 = (Settings.Default.BOSS卷轴怪物12 = textBox.Text);
                string bOSS卷轴怪物 = text3;
                string text4 = (Config.BOSS卷轴怪物12 = bOSS卷轴怪物);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS卷轴怪物13_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS卷轴怪物13")
            {
                string text3 = (Settings.Default.BOSS卷轴怪物13 = textBox.Text);
                string bOSS卷轴怪物 = text3;
                string text4 = (Config.BOSS卷轴怪物13 = bOSS卷轴怪物);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS卷轴怪物14_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS卷轴怪物14")
            {
                string text3 = (Settings.Default.BOSS卷轴怪物14 = textBox.Text);
                string bOSS卷轴怪物 = text3;
                string text4 = (Config.BOSS卷轴怪物14 = bOSS卷轴怪物);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS卷轴怪物15_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS卷轴怪物15")
            {
                string text3 = (Settings.Default.BOSS卷轴怪物15 = textBox.Text);
                string bOSS卷轴怪物 = text3;
                string text4 = (Config.BOSS卷轴怪物15 = bOSS卷轴怪物);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_BOSS卷轴怪物16_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_BOSS卷轴怪物16")
            {
                string text3 = (Settings.Default.BOSS卷轴怪物16 = textBox.Text);
                string bOSS卷轴怪物 = text3;
                string text4 = (Config.BOSS卷轴怪物16 = bOSS卷轴怪物);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 坐骑御兽_Click(object sender, EventArgs e)
    {
        SystemDataGateway.坐骑御兽();
    }

    private void 装备套装_Click(object sender, EventArgs e)
    {
        SystemDataGateway.套装数据();
    }

    private void S_浏览平台目录_Click(object sender, EventArgs e)
    {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
        {
            Description = "请选择文件夹"
        };
        if (folderBrowserDialog.ShowDialog() == DialogResult.OK && sender == S_浏览平台目录)
        {
            Settings @default = Settings.Default;
            string text = (S_平台接入目录.Text = folderBrowserDialog.SelectedPath);
            string text2 = text;
            string text3 = text2;
            text = (@default.平台接入目录 = text3);
            text2 = text;
            string text5 = (Config.平台接入目录 = text2);
            Settings.Default.Save();
        }
    }

    private void S_九层妖塔BOSS1_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_九层妖塔BOSS1")
            {
                string text3 = (Settings.Default.九层妖塔BOSS1 = textBox.Text);
                string 九层妖塔BOSS = text3;
                string text4 = (Config.九层妖塔BOSS1 = 九层妖塔BOSS);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_九层妖塔BOSS2_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_九层妖塔BOSS2")
            {
                string text3 = (Settings.Default.九层妖塔BOSS2 = textBox.Text);
                string 九层妖塔BOSS = text3;
                string text4 = (Config.九层妖塔BOSS2 = 九层妖塔BOSS);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_九层妖塔BOSS3_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_九层妖塔BOSS3")
            {
                string text3 = (Settings.Default.九层妖塔BOSS3 = textBox.Text);
                string 九层妖塔BOSS = text3;
                string text4 = (Config.九层妖塔BOSS3 = 九层妖塔BOSS);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_九层妖塔BOSS4_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_九层妖塔BOSS4")
            {
                string text3 = (Settings.Default.九层妖塔BOSS4 = textBox.Text);
                string 九层妖塔BOSS = text3;
                string text4 = (Config.九层妖塔BOSS4 = 九层妖塔BOSS);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_九层妖塔BOSS5_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_九层妖塔BOSS5")
            {
                string text3 = (Settings.Default.九层妖塔BOSS5 = textBox.Text);
                string 九层妖塔BOSS = text3;
                string text4 = (Config.九层妖塔BOSS5 = 九层妖塔BOSS);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_九层妖塔BOSS6_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_九层妖塔BOSS6")
            {
                string text3 = (Settings.Default.九层妖塔BOSS6 = textBox.Text);
                string 九层妖塔BOSS = text3;
                string text4 = (Config.九层妖塔BOSS6 = 九层妖塔BOSS);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_九层妖塔BOSS7_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_九层妖塔BOSS7")
            {
                string text3 = (Settings.Default.九层妖塔BOSS7 = textBox.Text);
                string 九层妖塔BOSS = text3;
                string text4 = (Config.九层妖塔BOSS7 = 九层妖塔BOSS);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_九层妖塔BOSS8_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_九层妖塔BOSS8")
            {
                string text3 = (Settings.Default.九层妖塔BOSS8 = textBox.Text);
                string 九层妖塔BOSS = text3;
                string text4 = (Config.九层妖塔BOSS8 = 九层妖塔BOSS);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_九层妖塔BOSS9_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_九层妖塔BOSS9")
            {
                string text3 = (Settings.Default.九层妖塔BOSS9 = textBox.Text);
                string 九层妖塔BOSS = text3;
                string text4 = (Config.九层妖塔BOSS9 = 九层妖塔BOSS);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_九层妖塔精英1_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_九层妖塔精英1")
            {
                string text3 = (Settings.Default.九层妖塔精英1 = textBox.Text);
                string 九层妖塔精英 = text3;
                string text4 = (Config.九层妖塔精英1 = 九层妖塔精英);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_九层妖塔精英2_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_九层妖塔精英2")
            {
                string text3 = (Settings.Default.九层妖塔精英2 = textBox.Text);
                string 九层妖塔精英 = text3;
                string text4 = (Config.九层妖塔精英2 = 九层妖塔精英);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_九层妖塔精英3_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_九层妖塔精英3")
            {
                string text3 = (Settings.Default.九层妖塔精英3 = textBox.Text);
                string 九层妖塔精英 = text3;
                string text4 = (Config.九层妖塔精英3 = 九层妖塔精英);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_九层妖塔精英4_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_九层妖塔精英4")
            {
                string text3 = (Settings.Default.九层妖塔精英4 = textBox.Text);
                string 九层妖塔精英 = text3;
                string text4 = (Config.九层妖塔精英4 = 九层妖塔精英);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_九层妖塔精英5_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_九层妖塔精英5")
            {
                string text3 = (Settings.Default.九层妖塔精英5 = textBox.Text);
                string 九层妖塔精英 = text3;
                string text4 = (Config.九层妖塔精英5 = 九层妖塔精英);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_九层妖塔精英6_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_九层妖塔精英6")
            {
                string text3 = (Settings.Default.九层妖塔精英6 = textBox.Text);
                string 九层妖塔精英 = text3;
                string text4 = (Config.九层妖塔精英6 = 九层妖塔精英);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_九层妖塔精英7_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_九层妖塔精英7")
            {
                string text3 = (Settings.Default.九层妖塔精英7 = textBox.Text);
                string 九层妖塔精英 = text3;
                string text4 = (Config.九层妖塔精英7 = 九层妖塔精英);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_九层妖塔精英8_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_九层妖塔精英8")
            {
                string text3 = (Settings.Default.九层妖塔精英8 = textBox.Text);
                string 九层妖塔精英 = text3;
                string text4 = (Config.九层妖塔精英8 = 九层妖塔精英);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_九层妖塔精英9_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_九层妖塔精英9")
            {
                string text3 = (Settings.Default.九层妖塔精英9 = textBox.Text);
                string 九层妖塔精英 = text3;
                string text4 = (Config.九层妖塔精英9 = 九层妖塔精英);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_书店商贩物品_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_书店商贩物品")
            {
                string text3 = (Settings.Default.书店商贩物品 = textBox.Text);
                string 书店商贩物品 = text3;
                string text4 = (Config.书店商贩物品 = 书店商贩物品);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_挂机权限选项_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_挂机权限选项")
            {
                string text3 = (Settings.Default.挂机权限选项 = textBox.Text);
                string 挂机权限选项 = text3;
                string text4 = (Config.挂机权限选项 = 挂机权限选项);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_暗之门地图1BOSS_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_暗之门地图1BOSS")
            {
                string text3 = (Settings.Default.暗之门地图1BOSS = textBox.Text);
                string 暗之门地图1BOSS = text3;
                string text4 = (Config.暗之门地图1BOSS = 暗之门地图1BOSS);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_暗之门地图2BOSS_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_暗之门地图2BOSS")
            {
                string text3 = (Settings.Default.暗之门地图2BOSS = textBox.Text);
                string 暗之门地图2BOSS = text3;
                string text4 = (Config.暗之门地图2BOSS = 暗之门地图2BOSS);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_暗之门地图3BOSS_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_暗之门地图3BOSS")
            {
                string text3 = (Settings.Default.暗之门地图3BOSS = textBox.Text);
                string 暗之门地图3BOSS = text3;
                string text4 = (Config.暗之门地图3BOSS = 暗之门地图3BOSS);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_暗之门地图4BOSS_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_暗之门地图4BOSS")
            {
                string text3 = (Settings.Default.暗之门地图4BOSS = textBox.Text);
                string 暗之门地图4BOSS = text3;
                string text4 = (Config.暗之门地图4BOSS = 暗之门地图4BOSS);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_沃玛分解物品一_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_沃玛分解物品一")
            {
                string text3 = (Settings.Default.沃玛分解物品一 = textBox.Text);
                string 沃玛分解物品一 = text3;
                string text4 = (Config.沃玛分解物品一 = 沃玛分解物品一);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_沃玛分解物品二_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_沃玛分解物品二")
            {
                string text3 = (Settings.Default.沃玛分解物品二 = textBox.Text);
                string 沃玛分解物品二 = text3;
                string text4 = (Config.沃玛分解物品二 = 沃玛分解物品二);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_沃玛分解物品三_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_沃玛分解物品三")
            {
                string text3 = (Settings.Default.沃玛分解物品三 = textBox.Text);
                string 沃玛分解物品三 = text3;
                string text4 = (Config.沃玛分解物品三 = 沃玛分解物品三);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_沃玛分解物品四_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_沃玛分解物品四")
            {
                string text3 = (Settings.Default.沃玛分解物品四 = textBox.Text);
                string 沃玛分解物品四 = text3;
                string text4 = (Config.沃玛分解物品四 = 沃玛分解物品四);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_其他分解物品一_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_其他分解物品一")
            {
                string text3 = (Settings.Default.其他分解物品一 = textBox.Text);
                string 其他分解物品一 = text3;
                string text4 = (Config.其他分解物品一 = 其他分解物品一);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_其他分解物品二_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_其他分解物品二")
            {
                string text3 = (Settings.Default.其他分解物品二 = textBox.Text);
                string 其他分解物品二 = text3;
                string text4 = (Config.其他分解物品二 = 其他分解物品二);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_其他分解物品三_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_其他分解物品三")
            {
                string text3 = (Settings.Default.其他分解物品三 = textBox.Text);
                string 其他分解物品三 = text3;
                string text4 = (Config.其他分解物品三 = 其他分解物品三);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void S_其他分解物品四_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "S_其他分解物品四")
            {
                string text3 = (Settings.Default.其他分解物品四 = textBox.Text);
                string 其他分解物品四 = text3;
                string text4 = (Config.其他分解物品四 = 其他分解物品四);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 九层妖塔统计开关_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "九层妖塔统计开关")
            {
                int num2 = (Settings.Default.九层妖塔统计开关 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.九层妖塔统计开关 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 沙巴克每周攻沙时间_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "沙巴克每周攻沙时间")
            {
                int num2 = (Settings.Default.沙巴克每周攻沙时间 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.沙巴克每周攻沙时间 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 沙巴克皇宫传送等级_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "沙巴克皇宫传送等级")
            {
                int num2 = (Settings.Default.沙巴克皇宫传送等级 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.沙巴克皇宫传送等级 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 沙巴克皇宫传送物品_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "沙巴克皇宫传送物品")
            {
                int num2 = (Settings.Default.沙巴克皇宫传送物品 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.沙巴克皇宫传送物品 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 沙巴克皇宫传送数量_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "沙巴克皇宫传送数量")
            {
                int num2 = (Settings.Default.沙巴克皇宫传送数量 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.沙巴克皇宫传送数量 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 合成模块控件_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "合成模块控件")
            {
                string text3 = (Settings.Default.合成模块控件 = textBox.Text);
                string text4 = text3;
                string text5 = (Config.合成模块控件 = text4);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 系统窗口发送_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "系统窗口发送")
            {
                int num2 = (Settings.Default.系统窗口发送 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.系统窗口发送 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 龙卫效果提示_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "龙卫效果提示")
            {
                int num2 = (Settings.Default.龙卫效果提示 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.龙卫效果提示 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 充值平台切换_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "充值平台切换")
            {
                int num2 = (Settings.Default.充值平台切换 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.充值平台切换 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 坐骑骑乘切换_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "坐骑骑乘切换")
            {
                int num2 = (Settings.Default.坐骑骑乘切换 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.坐骑骑乘切换 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 坐骑属性切换_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "坐骑属性切换")
            {
                int num2 = (Settings.Default.坐骑属性切换 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.坐骑属性切换 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 称号属性切换_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "称号属性切换")
            {
                int num2 = (Settings.Default.称号属性切换 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.称号属性切换 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 珍宝模块切换_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "珍宝模块切换")
            {
                int num2 = (Settings.Default.珍宝模块切换 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.珍宝模块切换 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 全服红包等级_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "全服红包等级")
            {
                int num2 = (Settings.Default.全服红包等级 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.全服红包等级 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 全服红包时间_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "全服红包时间")
            {
                int num2 = (Settings.Default.全服红包时间 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.全服红包时间 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 全服红包货币类型_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "全服红包货币类型")
            {
                int num2 = (Settings.Default.全服红包货币类型 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.全服红包货币类型 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 全服红包货币数量_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "全服红包货币数量")
            {
                int num2 = (Settings.Default.全服红包货币数量 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.全服红包货币数量 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 龙卫蓝色词条概率_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "龙卫蓝色词条概率")
            {
                int num2 = (Settings.Default.龙卫蓝色词条概率 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.龙卫蓝色词条概率 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 龙卫紫色词条概率_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "龙卫紫色词条概率")
            {
                int num2 = (Settings.Default.龙卫紫色词条概率 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.龙卫紫色词条概率 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 龙卫橙色词条概率_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "龙卫橙色词条概率")
            {
                int num2 = (Settings.Default.龙卫橙色词条概率 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.龙卫橙色词条概率 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 自定义初始货币类型_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "自定义初始货币类型")
            {
                int num2 = (Settings.Default.自定义初始货币类型 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.自定义初始货币类型 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 自动回收设置_CheckedChanged(object sender, EventArgs e)
    {
        if (!(sender is CheckBox checkBox))
        {
            return;
        }
        string name = checkBox.Name;
        string text = name;
        if (text == "自动回收设置")
        {
            if (自动回收设置.Checked)
            {
                Settings.Default.自动回收设置 = 1;
                Config.自动回收设置 = 1;
            }
            else
            {
                Settings.Default.自动回收设置 = 0;
                Config.自动回收设置 = 0;
            }
        }
        else
        {
            MessageBox.Show("未知变量! " + checkBox.Name);
        }
        Settings.Default.Save();
    }

    private void 购买狂暴之力_CheckedChanged(object sender, EventArgs e)
    {
        if (!(sender is CheckBox checkBox))
        {
            return;
        }
        string name = checkBox.Name;
        string text = name;
        if (text == "购买狂暴之力")
        {
            if (购买狂暴之力.Checked)
            {
                Settings.Default.购买狂暴之力 = 1;
                Config.购买狂暴之力 = 1;
            }
            else
            {
                Settings.Default.购买狂暴之力 = 0;
                Config.购买狂暴之力 = 0;
            }
        }
        else
        {
            MessageBox.Show("未知变量! " + checkBox.Name);
        }
        Settings.Default.Save();
    }

    private void 会员满血设置_CheckedChanged(object sender, EventArgs e)
    {
        if (!(sender is CheckBox checkBox))
        {
            return;
        }
        string name = checkBox.Name;
        string text = name;
        if (text == "会员满血设置")
        {
            if (会员满血设置.Checked)
            {
                Settings.Default.会员满血设置 = 1;
                Config.会员满血设置 = 1;
            }
            else
            {
                Settings.Default.会员满血设置 = 0;
                Config.会员满血设置 = 0;
            }
        }
        else
        {
            MessageBox.Show("未知变量! " + checkBox.Name);
        }
        Settings.Default.Save();
    }

    private void 全屏拾取开关_CheckedChanged(object sender, EventArgs e)
    {
        if (!(sender is CheckBox checkBox))
        {
            return;
        }
        string name = checkBox.Name;
        string text = name;
        if (text == "全屏拾取开关")
        {
            if (全屏拾取开关.Checked)
            {
                Settings.Default.全屏拾取开关 = 1;
                Config.全屏拾取开关 = 1;
            }
            else
            {
                Settings.Default.全屏拾取开关 = 0;
                Config.全屏拾取开关 = 0;
            }
        }
        else
        {
            MessageBox.Show("未知变量! " + checkBox.Name);
        }
        Settings.Default.Save();
    }

    private void 打开随时仓库_CheckedChanged(object sender, EventArgs e)
    {
        if (!(sender is CheckBox checkBox))
        {
            return;
        }
        string name = checkBox.Name;
        string text = name;
        if (text == "打开随时仓库")
        {
            if (打开随时仓库.Checked)
            {
                Settings.Default.打开随时仓库 = 1;
                Config.打开随时仓库 = 1;
            }
            else
            {
                Settings.Default.打开随时仓库 = 0;
                Config.打开随时仓库 = 0;
            }
        }
        else
        {
            MessageBox.Show("未知变量! " + checkBox.Name);
        }
        Settings.Default.Save();
    }

    private void 会员物品对接_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "会员物品对接")
            {
                int num2 = (Settings.Default.会员物品对接 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.会员物品对接 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 称号叠加模块9_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "称号叠加模块9")
            {
                byte b2 = (Settings.Default.称号叠加模块9 = (byte)numericUpDown.Value);
                byte b3 = b2;
                byte b4 = (Config.称号叠加模块9 = b3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 称号叠加模块10_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "称号叠加模块10")
            {
                byte b2 = (Settings.Default.称号叠加模块10 = (byte)numericUpDown.Value);
                byte b3 = b2;
                byte b4 = (Config.称号叠加模块10 = b3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 称号叠加模块11_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "称号叠加模块11")
            {
                byte b2 = (Settings.Default.称号叠加模块11 = (byte)numericUpDown.Value);
                byte b3 = b2;
                byte b4 = (Config.称号叠加模块11 = b3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 称号叠加模块12_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "称号叠加模块12")
            {
                byte b2 = (Settings.Default.称号叠加模块12 = (byte)numericUpDown.Value);
                byte b3 = b2;
                byte b4 = (Config.称号叠加模块12 = b3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 称号叠加模块13_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "称号叠加模块13")
            {
                byte b2 = (Settings.Default.称号叠加模块13 = (byte)numericUpDown.Value);
                byte b3 = b2;
                byte b4 = (Config.称号叠加模块13 = b3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 称号叠加模块14_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "称号叠加模块14")
            {
                byte b2 = (Settings.Default.称号叠加模块14 = (byte)numericUpDown.Value);
                byte b3 = b2;
                byte b4 = (Config.称号叠加模块14 = b3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 称号叠加模块15_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "称号叠加模块15")
            {
                byte b2 = (Settings.Default.称号叠加模块15 = (byte)numericUpDown.Value);
                byte b3 = b2;
                byte b4 = (Config.称号叠加模块15 = b3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 称号叠加模块16_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "称号叠加模块16")
            {
                byte b2 = (Settings.Default.称号叠加模块16 = (byte)numericUpDown.Value);
                byte b3 = b2;
                byte b4 = (Config.称号叠加模块16 = b3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 变性等级_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "变性等级")
            {
                int num2 = (Settings.Default.变性等级 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.变性等级 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 变性货币类型_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "变性货币类型")
            {
                int num2 = (Settings.Default.变性货币类型 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.变性货币类型 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 变性货币值_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "变性货币值")
            {
                int num2 = (Settings.Default.变性货币值 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.变性货币值 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 变性物品ID_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "变性物品ID")
            {
                int num2 = (Settings.Default.变性物品ID = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.变性物品ID = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 变性物品数量_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "变性物品数量")
            {
                int num2 = (Settings.Default.变性物品数量 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.变性物品数量 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 变性内容控件_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "变性内容控件")
            {
                string text3 = (Settings.Default.变性内容控件 = textBox.Text);
                string text4 = text3;
                string text5 = (Config.变性内容控件 = text4);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 幸运保底开关_CheckedChanged(object sender, EventArgs e)
    {
        if (!(sender is CheckBox checkBox))
        {
            return;
        }
        string name = checkBox.Name;
        string text = name;
        if (text == "幸运保底开关")
        {
            if (幸运保底开关.Checked)
            {
                Settings.Default.幸运保底开关 = 1;
                Config.幸运保底开关 = 1;
            }
            else
            {
                Settings.Default.幸运保底开关 = 0;
                Config.幸运保底开关 = 0;
            }
        }
        else
        {
            MessageBox.Show("未知变量! " + checkBox.Name);
        }
        Settings.Default.Save();
    }

    private void 龙卫焰焚烈火剑法_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "龙卫焰焚烈火剑法")
            {
                int num2 = (Settings.Default.龙卫焰焚烈火剑法 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.龙卫焰焚烈火剑法 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 红包开关_CheckedChanged(object sender, EventArgs e)
    {
        if (!(sender is CheckBox checkBox))
        {
            return;
        }
        string name = checkBox.Name;
        string text = name;
        if (text == "红包开关")
        {
            if (红包开关.Checked)
            {
                Settings.Default.红包开关 = 1;
                Config.红包开关 = 1;
            }
            else
            {
                Settings.Default.红包开关 = 0;
                Config.红包开关 = 0;
            }
        }
        else
        {
            MessageBox.Show("未知变量! " + checkBox.Name);
        }
        Settings.Default.Save();
    }

    private void 安全区收刀开关_CheckedChanged(object sender, EventArgs e)
    {
        if (!(sender is CheckBox checkBox))
        {
            return;
        }
        string name = checkBox.Name;
        string text = name;
        if (text == "安全区收刀开关")
        {
            if (安全区收刀开关.Checked)
            {
                Settings.Default.安全区收刀开关 = 1;
                Config.安全区收刀开关 = 1;
            }
            else
            {
                Settings.Default.安全区收刀开关 = 0;
                Config.安全区收刀开关 = 0;
            }
        }
        else
        {
            MessageBox.Show("未知变量! " + checkBox.Name);
        }
        Settings.Default.Save();
    }

    private void 屠魔殿等级限制_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "屠魔殿等级限制")
            {
                int num2 = (Settings.Default.屠魔殿等级限制 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.屠魔殿等级限制 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 职业等级_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "职业等级")
            {
                int num2 = (Settings.Default.职业等级 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.职业等级 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 职业货币类型_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "职业货币类型")
            {
                int num2 = (Settings.Default.职业货币类型 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.职业货币类型 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 职业货币值_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "职业货币值")
            {
                int num2 = (Settings.Default.职业货币值 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.职业货币值 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 职业物品ID_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "职业物品ID")
            {
                int num2 = (Settings.Default.职业物品ID = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.职业物品ID = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 职业物品数量_ValueChanged(object sender, EventArgs e)
    {
        if (sender is NumericUpDown numericUpDown)
        {
            string name = numericUpDown.Name;
            string text = name;
            if (text == "职业物品数量")
            {
                int num2 = (Settings.Default.职业物品数量 = (int)numericUpDown.Value);
                int num3 = num2;
                int num4 = (Config.职业物品数量 = num3);
            }
            else
            {
                MessageBox.Show("未知变量! " + numericUpDown.Name);
            }
            Settings.Default.Save();
        }
    }

    private void 转职内容控件_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            string name = textBox.Name;
            string text = name;
            if (text == "转职内容控件")
            {
                string text3 = (Settings.Default.转职内容控件 = textBox.Text);
                string text4 = text3;
                string text5 = (Config.转职内容控件 = text4);
            }
            else
            {
                MessageBox.Show("未知变量! " + textBox.Name);
            }
            Settings.Default.Save();
        }
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
    }

    private void startServerToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SEngine.StartService();
        Settings.Default.Save();
        MapDataTable = new System.Data.DataTable("地图数据表");
        地图数据行 = new Dictionary<GameMap, DataRow>();
        MapDataTable.Columns.Add("地图编号", typeof(string));
        MapDataTable.Columns.Add("地图名字", typeof(string));
        MapDataTable.Columns.Add("限制等级", typeof(string));
        MapDataTable.Columns.Add("玩家数量", typeof(string));
        MapDataTable.Columns.Add("固定怪物总数", typeof(string));
        MapDataTable.Columns.Add("存活怪物总数", typeof(string));
        MapDataTable.Columns.Add("怪物复活次数", typeof(string));
        MapDataTable.Columns.Add("怪物掉落次数", typeof(string));
        MapDataTable.Columns.Add("金币掉落总数", typeof(string));
        Main.地图浏览表.DataSource = MapDataTable;
        MonsterDataTable = new System.Data.DataTable("怪物数据表");
        怪物数据行 = new Dictionary<MonsterInfo, DataRow>();
        数据行怪物 = new Dictionary<DataRow, MonsterInfo>();
        MonsterDataTable.Columns.Add("模板编号", typeof(string));
        MonsterDataTable.Columns.Add("怪物名字", typeof(string));
        MonsterDataTable.Columns.Add("怪物等级", typeof(string));
        MonsterDataTable.Columns.Add("怪物经验", typeof(string));
        MonsterDataTable.Columns.Add("怪物级别", typeof(string));
        MonsterDataTable.Columns.Add("移动间隔", typeof(string));
        MonsterDataTable.Columns.Add("漫游间隔", typeof(string));
        MonsterDataTable.Columns.Add("仇恨范围", typeof(string));
        MonsterDataTable.Columns.Add("仇恨时长", typeof(string));
        Main.怪物浏览表.DataSource = MonsterDataTable;
        掉落数据表 = new System.Data.DataTable("掉落数据表");
        怪物掉落表 = new Dictionary<MonsterInfo, List<KeyValuePair<GameItem, long>>>();
        掉落数据表.Columns.Add("物品名字", typeof(string));
        掉落数据表.Columns.Add("掉落数量", typeof(string));
        Main.掉落浏览表.DataSource = 掉落数据表;
        主选项卡.SelectedIndex = 0;
        SettingsPage.Enabled = false;
        stopServerToolStripMenuItem.Enabled = false;
        startServerToolStripMenuItem.Enabled = false;
    }

    private void stopServerToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show("Are you sure to stop the server?", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        {
            SEngine.StopService();
            stopServerToolStripMenuItem.Enabled = false;
        }
    }

    private void saveSystemLogsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (SystemLogsTextBox.Text != null && !(SystemLogsTextBox.Text == ""))
        {
            if (!Directory.Exists(".\\Log\\Sys"))
            {
                Directory.CreateDirectory(".\\Log\\Sys");
            }
            File.WriteAllText($".\\Log\\Sys\\{DateTime.Now:yyyy-MM-dd--HH-mm-ss}.txt", SystemLogsTextBox.Text.Replace("\n", "\r\n"));
            AddSystemLog("The system log has been successfully saved");
        }
    }

    private void saveChatLogsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (ChatLogsTextBox.Text != null && !(ChatLogsTextBox.Text == ""))
        {
            if (!Directory.Exists(".\\Log\\Chat"))
            {
                Directory.CreateDirectory(".\\Log\\Chat");
            }
            File.WriteAllText($".\\Log\\Chat\\{DateTime.Now:yyyy-MM-dd--HH-mm-ss}.txt", SystemLogsTextBox.Text);
            AddSystemLog("The game chat log has been successfully saved");
        }
    }

    private void clearSystemLogsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemLogsTextBox.Clear();
        saveSystemLogsToolStripMenuItem.Enabled = false;
        clearSystemLogsToolStripMenuItem.Enabled = false;
    }

    private void clearChatLogsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ChatLogsTextBox.Clear();
        saveChatLogsToolStripMenuItem.Enabled = false;
        clearChatLogsToolStripMenuItem.Enabled = false;
    }

    private void clearCommandsLogToolStripMenuItem_Click(object sender, EventArgs e)
    {
        CommandLogsTextBox.Clear();
        clearCommandsLogToolStripMenuItem.Enabled = false;
    }

    private void savaDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Task.Run(delegate
        {
            AddSystemLog("User data is being saved...");
            Session.Save();
            Session.SaveUsers();
            AddSystemLog("User data save completed");
        });
    }

    private void allToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.ReloadData();
    }
}
