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
using GameServer.Map;
using GameServer.Database;
using GameServer.Template;
using GameServer.Networking;

namespace GameServer;

public partial class SMain : Form
{
    public static SMain Main;

    private static System.Data.DataTable RoleDataTable;
    private static System.Data.DataTable SkillsDataTable;
    private static System.Data.DataTable EquipmentDataTable;
    private static System.Data.DataTable InventoryDataTable;
    private static System.Data.DataTable WarehouseDataTable;
    private static System.Data.DataTable MapDataTable;
    private static System.Data.DataTable MonsterDataTable;
    private static System.Data.DataTable DropDataTable;
    private static System.Data.DataTable BlockingDataTable;

    private static Dictionary<CharacterInfo, DataRow> RoleDataRows;
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

        AddSystemLog("Connecting to 'System' database");
        DBAgent.X.InitDB(Config.GameDataPath + "\\System\\System.db");

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
        Main?.地图浏览表.BeginInvoke(() =>
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
        Main?.怪物浏览表.BeginInvoke(() =>
        {
            Main.怪物浏览表.DataSource = MonsterDataTable;
        });
        DropDataTable = new System.Data.DataTable("掉落数据表");
        怪物掉落表 = new Dictionary<MonsterInfo, List<KeyValuePair<GameItem, long>>>();
        DropDataTable.Columns.Add("物品名字", typeof(string));
        DropDataTable.Columns.Add("掉落数量", typeof(string));
        Main?.掉落浏览表.BeginInvoke(() =>
        {
            Main.掉落浏览表.DataSource = DropDataTable;
        });
        SystemDataGateway.LoadData();
        AddSystemLog("The system data load is complete");
    }

    public static void LoadUserData()
    {
        AddSystemLog("Loading user data...");
        RoleDataTable = new System.Data.DataTable("角色数据表");
        SkillsDataTable = new System.Data.DataTable("技能数据表");
        EquipmentDataTable = new System.Data.DataTable("装备数据表");
        InventoryDataTable = new System.Data.DataTable("装备数据表");
        WarehouseDataTable = new System.Data.DataTable("装备数据表");
        RoleDataTable = new System.Data.DataTable("角色数据表");
        RoleDataRows = new Dictionary<CharacterInfo, DataRow>();
        数据行角色 = new Dictionary<DataRow, CharacterInfo>();
        RoleDataTable.Columns.Add("角色名字", typeof(string));
        RoleDataTable.Columns.Add("角色封禁", typeof(string));
        RoleDataTable.Columns.Add("所属账号", typeof(string));
        RoleDataTable.Columns.Add("账号封禁", typeof(string));
        RoleDataTable.Columns.Add("冻结日期", typeof(string));
        RoleDataTable.Columns.Add("删除日期", typeof(string));
        RoleDataTable.Columns.Add("登录日期", typeof(string));
        RoleDataTable.Columns.Add("离线日期", typeof(string));
        RoleDataTable.Columns.Add("网络地址", typeof(string));
        RoleDataTable.Columns.Add("物理地址", typeof(string));
        RoleDataTable.Columns.Add("角色职业", typeof(string));
        RoleDataTable.Columns.Add("角色性别", typeof(string));
        RoleDataTable.Columns.Add("所属行会", typeof(string));
        RoleDataTable.Columns.Add("元宝数量", typeof(string));
        RoleDataTable.Columns.Add("消耗元宝", typeof(string));
        RoleDataTable.Columns.Add("金币数量", typeof(string));
        RoleDataTable.Columns.Add("转出金币", typeof(string));
        RoleDataTable.Columns.Add("背包大小", typeof(string));
        RoleDataTable.Columns.Add("仓库大小", typeof(string));
        RoleDataTable.Columns.Add("师门声望", typeof(string));
        RoleDataTable.Columns.Add("本期特权", typeof(string));
        RoleDataTable.Columns.Add("本期日期", typeof(string));
        RoleDataTable.Columns.Add("上期特权", typeof(string));
        RoleDataTable.Columns.Add("上期日期", typeof(string));
        RoleDataTable.Columns.Add("剩余特权", typeof(string));
        RoleDataTable.Columns.Add("当前等级", typeof(string));
        RoleDataTable.Columns.Add("当前经验", typeof(string));
        RoleDataTable.Columns.Add("双倍经验", typeof(string));
        RoleDataTable.Columns.Add("当前战力", typeof(string));
        RoleDataTable.Columns.Add("当前地图", typeof(string));
        RoleDataTable.Columns.Add("当前坐标", typeof(string));
        RoleDataTable.Columns.Add("当前PK值", typeof(string));
        RoleDataTable.Columns.Add("激活标识", typeof(string));
        Main?.BeginInvoke(() =>
        {
            Main.角色浏览表.DataSource = RoleDataTable;
            for (int i = 0; i < Main.角色浏览表.Columns.Count; i++)
            {
                Main.角色浏览表.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        });
        角色技能表 = new Dictionary<CharacterInfo, List<KeyValuePair<ushort, SkillInfo>>>();
        SkillsDataTable.Columns.Add("技能名字", typeof(string));
        SkillsDataTable.Columns.Add("技能编号", typeof(string));
        SkillsDataTable.Columns.Add("当前等级", typeof(string));
        SkillsDataTable.Columns.Add("当前经验", typeof(string));
        Main?.BeginInvoke(() =>
        {
            Main.技能浏览表.DataSource = SkillsDataTable;
        });
        角色装备表 = new Dictionary<CharacterInfo, List<KeyValuePair<byte, EquipmentInfo>>>();
        EquipmentDataTable.Columns.Add("穿戴部位", typeof(string));
        EquipmentDataTable.Columns.Add("穿戴装备", typeof(string));
        Main?.BeginInvoke(() =>
        {
            Main.装备浏览表.DataSource = EquipmentDataTable;
        });
        角色背包表 = new Dictionary<CharacterInfo, List<KeyValuePair<byte, ItemInfo>>>();
        InventoryDataTable.Columns.Add("背包位置", typeof(string));
        InventoryDataTable.Columns.Add("背包物品", typeof(string));
        Main?.BeginInvoke(() =>
        {
            Main.背包浏览表.DataSource = InventoryDataTable;
        });
        角色仓库表 = new Dictionary<CharacterInfo, List<KeyValuePair<byte, ItemInfo>>>();
        WarehouseDataTable.Columns.Add("仓库位置", typeof(string));
        WarehouseDataTable.Columns.Add("仓库物品", typeof(string));
        Main?.BeginInvoke(() =>
        {
            Main.仓库浏览表.DataSource = WarehouseDataTable;
        });
        BlockingDataTable = new System.Data.DataTable();
        封禁数据行 = new Dictionary<string, DataRow>();
        BlockingDataTable.Columns.Add("网络地址", typeof(string));
        BlockingDataTable.Columns.Add("物理地址", typeof(string));
        BlockingDataTable.Columns.Add("到期时间", typeof(string));
        Main?.BeginInvoke(() =>
        {
            Main.封禁浏览表.DataSource = BlockingDataTable;
        });
        Session.Load();
        Main.allToolStripMenuItem.Visible = true;
        AddSystemLog("The user data is loaded");
    }

    public static void OnStartServiceCompleted()
    {
        Main?.BeginInvoke(() =>
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
        Main?.BeginInvoke(() =>
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
        Main?.BeginInvoke(() =>
        {
            Main.SystemLogsTextBox.AppendText($"[{DateTime.Now:F}]: {message}" + "\r\n");
            Main.SystemLogsTextBox.ScrollToCaret();
            Main.saveSystemLogsToolStripMenuItem.Enabled = true;
            Main.clearSystemLogsToolStripMenuItem.Enabled = true;
        });
    }

    public static void AddChatLog(string tag, byte[] message)
    {
        Main?.BeginInvoke(() =>
        {
            Main.ChatLogsTextBox.AppendText($"[{DateTime.Now:F}]: {tag + Encoding.UTF8.GetString(message).Trim(default(char))}" + "\r\n");
            Main.ChatLogsTextBox.ScrollToCaret();
            Main.saveChatLogsToolStripMenuItem.Enabled = true;
            Main.clearChatLogsToolStripMenuItem.Enabled = true;
        });
    }

    public static void AddCommandLog(string message)
    {
        Main?.BeginInvoke(() =>
        {
            Main.CommandLogsTextBox.AppendText($"[{DateTime.Now:F}]: {message}" + "\r\n");
            Main.CommandLogsTextBox.ScrollToCaret();
            Main.clearCommandsLogToolStripMenuItem.Enabled = true;
        });
    }

    public static void UpdateStats(SEngineStats stats)
    {
        Main?.BeginInvoke(() =>
        {
            Main.PortStatusLabel.Text = Config.UserConnectionPort.ToString();
            Main.ConnectionsStatusLabel.Text = $"Connections: {stats.ActiveConnections}/{stats.Connections}";
            Main.OnlineStatusLabel.Text = $"Online: {stats.ConnectionsOnline}/{stats.ConnectionsOnline1}/{stats.ConnectionsOnline2}";
            Main.ObjectsStatusLabel.Text = $"Objects: {stats.ActiveObjects}/{stats.SecondaryObjects}/{stats.Objects}";
            Main.CycleStatusLabel.Text = $"Cycles: {stats.CycleCount}";
            Main.DataSentStatusLabel.Text = $"Sent: {Compute.GetBytesReadable(stats.TotalSentBytes)}";
            Main.DataReceivedStatusLabel.Text = $"Received: {Compute.GetBytesReadable(stats.TotalReceivedBytes)}";
        });
    }

    public static void 添加数据显示(CharacterInfo 数据)
    {
        if (!RoleDataRows.ContainsKey(数据))
        {
            RoleDataRows[数据] = RoleDataTable.NewRow();
            RoleDataTable.Rows.Add(RoleDataRows[数据]);
        }
    }

    public static void 修改数据显示(CharacterInfo 数据, string 表头文本, string 表格内容)
    {
        if (RoleDataRows.ContainsKey(数据))
        {
            RoleDataRows[数据][表头文本] = 表格内容;
        }
    }

    public static void 添加角色数据(CharacterInfo character)
    {
        Main?.BeginInvoke(() =>
        {
            if (!RoleDataRows.ContainsKey(character))
            {
                DataRow dataRow = RoleDataTable.NewRow();
                dataRow["角色名字"] = character;
                dataRow["所属账号"] = character.Account;
                dataRow["账号封禁"] = ((character.Account.V.BlockDate.V != default(DateTime)) ? character.Account.V.BlockDate : null);
                dataRow["角色封禁"] = ((character.BlockDate.V != default(DateTime)) ? character.BlockDate : null);
                dataRow["冻结日期"] = ((character.FrozenDate.V != default(DateTime)) ? character.FrozenDate : null);
                dataRow["删除日期"] = ((character.DeletetionDate.V != default(DateTime)) ? character.DeletetionDate : null);
                dataRow["登录日期"] = ((character.LoginDate.V != default(DateTime)) ? character.LoginDate : null);
                dataRow["离线日期"] = ((character.Connection == null) ? character.DisconnectDate : null);
                dataRow["网络地址"] = character.IPAddress;
                dataRow["物理地址"] = character.MACAddress;
                dataRow["角色职业"] = character.Job;
                dataRow["角色性别"] = character.Gender;
                dataRow["所属行会"] = character.Guild;
                dataRow["元宝数量"] = character.Ingot;
                dataRow["消耗元宝"] = character.消耗元宝;
                dataRow["金币数量"] = character.Gold;
                dataRow["转出金币"] = character.TradeGold;
                dataRow["背包大小"] = character.InventorySize;
                dataRow["仓库大小"] = character.WarehouseSize;
                dataRow["师门声望"] = character.师门声望;
                dataRow["本期特权"] = character.本期特权;
                dataRow["本期日期"] = character.本期日期;
                dataRow["上期特权"] = character.上期特权;
                dataRow["上期日期"] = character.上期日期;
                dataRow["剩余特权"] = character.剩余特权;
                dataRow["当前等级"] = character.Level;
                dataRow["当前经验"] = character.Experience;
                dataRow["双倍经验"] = character.ExperienceRate;
                dataRow["当前战力"] = character.CombatPower;
                dataRow["当前地图"] = (GameMap.DataSheet.TryGetValue((byte)character.CurrentMap.V, out var value) ? ((object)value.MapName) : ((object)character.CurrentMap));
                dataRow["当前PK值"] = character.CurrentPKPoint;
                dataRow["当前坐标"] = $"{character.CurrentPosition.V.X}, {character.CurrentPosition.V.Y}";
                dataRow["激活标识"] = character.激活标识;
                RoleDataRows[character] = dataRow;
                数据行角色[dataRow] = character;
                RoleDataTable.Rows.Add(dataRow);
            }
        });
    }

    public static void 移除角色数据(CharacterInfo 角色)
    {
        Main?.BeginInvoke(() =>
        {
            if (RoleDataRows.TryGetValue(角色, out var value))
            {
                数据行角色.Remove(value);
                RoleDataTable.Rows.Remove(value);
                角色技能表.Remove(角色);
                角色背包表.Remove(角色);
                角色装备表.Remove(角色);
                角色仓库表.Remove(角色);
            }
        });
    }

    public static void ProcessUpdateUI(object sender, EventArgs e)
    {
        SkillsDataTable.Rows.Clear();
        EquipmentDataTable.Rows.Clear();
        InventoryDataTable.Rows.Clear();
        WarehouseDataTable.Rows.Clear();
        DropDataTable.Rows.Clear();
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
                        DataRow dataRow = SkillsDataTable.NewRow();
                        dataRow["技能名字"] = item.Value.Inscription.SkillName;
                        dataRow["技能编号"] = item.Value.ID;
                        dataRow["当前等级"] = item.Value.Level;
                        dataRow["当前经验"] = item.Value.Experience;
                        SkillsDataTable.Rows.Add(dataRow);
                    }
                }
                if (角色装备表.TryGetValue(value, out var value3))
                {
                    foreach (KeyValuePair<byte, EquipmentInfo> item2 in value3)
                    {
                        DataRow dataRow2 = EquipmentDataTable.NewRow();
                        dataRow2["穿戴部位"] = (装备穿戴部位)item2.Key;
                        dataRow2["穿戴装备"] = item2.Value;
                        EquipmentDataTable.Rows.Add(dataRow2);
                    }
                }
                if (角色背包表.TryGetValue(value, out var value4))
                {
                    foreach (KeyValuePair<byte, ItemInfo> item3 in value4)
                    {
                        DataRow dataRow3 = InventoryDataTable.NewRow();
                        dataRow3["背包位置"] = item3.Key;
                        dataRow3["背包物品"] = item3.Value;
                        InventoryDataTable.Rows.Add(dataRow3);
                    }
                }
                if (角色仓库表.TryGetValue(value, out var value5))
                {
                    foreach (KeyValuePair<byte, ItemInfo> item4 in value5)
                    {
                        DataRow dataRow4 = WarehouseDataTable.NewRow();
                        dataRow4["仓库位置"] = item4.Key;
                        dataRow4["仓库物品"] = item4.Value;
                        WarehouseDataTable.Rows.Add(dataRow4);
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
            DataRow dataRow5 = DropDataTable.NewRow();
            dataRow5["物品名字"] = item5.Key.Name;
            dataRow5["掉落数量"] = item5.Value;
            DropDataTable.Rows.Add(dataRow5);
        }
    }

    public static void UpdateCharacter(CharacterInfo character, string key, object value)
    {
        Main?.BeginInvoke(() =>
        {
            if (RoleDataRows.TryGetValue(character, out var row))
            {
                row[key] = value;
            }
        });
    }

    public static void 更新角色技能(CharacterInfo 角色, List<KeyValuePair<ushort, SkillInfo>> 技能)
    {
        Main?.BeginInvoke(() =>
        {
            角色技能表[角色] = 技能;
        });
    }

    public static void 更新角色装备(CharacterInfo 角色, List<KeyValuePair<byte, EquipmentInfo>> 装备)
    {
        Main?.BeginInvoke(() =>
        {
            角色装备表[角色] = 装备;
        });
    }

    public static void 更新角色背包(CharacterInfo 角色, List<KeyValuePair<byte, ItemInfo>> 物品)
    {
        Main?.BeginInvoke(() =>
        {
            角色背包表[角色] = 物品;
        });
    }

    public static void 更新角色仓库(CharacterInfo 角色, List<KeyValuePair<byte, ItemInfo>> 物品)
    {
        Main?.BeginInvoke(() =>
        {
            角色仓库表[角色] = 物品;
        });
    }

    public static void 添加地图数据(Map.Map 地图)
    {
        Main?.BeginInvoke(() =>
        {
            if (!地图数据行.ContainsKey(地图.MapInfo))
            {
                DataRow dataRow = MapDataTable.NewRow();
                dataRow["地图编号"] = 地图.MapID;
                dataRow["地图名字"] = 地图.MapInfo;
                dataRow["限制等级"] = 地图.MinLevel;
                dataRow["玩家数量"] = 地图.Players.Count;
                dataRow["固定怪物总数"] = 地图.TotalFixedMonsters;
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
        Main?.BeginInvoke(() =>
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
        Main?.BeginInvoke(() =>
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
        Main?.BeginInvoke(() =>
        {
            怪物掉落表[怪物] = 物品;
        });
    }

    public static void 添加封禁数据(string 地址, object 时间, bool 网络地址 = true)
    {
        Main?.BeginInvoke(() =>
        {
            if (!封禁数据行.ContainsKey(地址))
            {
                DataRow dataRow = BlockingDataTable.NewRow();
                dataRow["网络地址"] = (网络地址 ? 地址 : null);
                dataRow["物理地址"] = (网络地址 ? null : 地址);
                dataRow["到期时间"] = 时间;
                封禁数据行[地址] = dataRow;
                BlockingDataTable.Rows.Add(dataRow);
            }
        });
    }

    public static void 更新封禁数据(string 地址, object 时间, bool 网络地址 = true)
    {
        Main?.BeginInvoke(() =>
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
        Main?.BeginInvoke(() =>
        {
            if (封禁数据行.TryGetValue(地址, out var value))
            {
                封禁数据行.Remove(地址);
                BlockingDataTable.Rows.Remove(value);
            }
        });
    }

    public SMain()
    {
        InitializeComponent();

        Config.Load();

        Control.CheckForIllegalCrossThreadCalls = false;
        Main = this;

        string 系统公告内容 = Config.系统公告内容;
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
        S_GameDataPath.Text = Config.GameDataPath;
        S_DataBackupPath.Text = Config.DataBackupPath;
        S_UserConnectionPort.Value = Config.UserConnectionPort;
        S_TicketReceivePort.Value = Config.TicketReceivePort;
        S_PacketLimit.Value = Config.PacketLimit;
        S_AbnormalBlockTime.Value = Config.AbnormalBlockTime;
        S_DisconnectTime.Value = Config.DisconnectTime;
        S_MaxUserLevel.Value = Config.MaxUserLevel;
        S_NoobSupportLevel.Value = Config.NoobSupportLevel;
        S_SpecialRepairDiscount.Value = Config.SpecialRepairDiscount;
        S_ItemDropRate.Value = Config.ItemDropRate;
        S_MonsterExperienceMultiplier.Value = Config.MonsterExperienceMultiplier;
        S_减收益等级差.Value = Config.减收益等级差;
        S_收益减少比率.Value = Config.收益减少比率;
        S_怪物诱惑时长.Value = Config.怪物诱惑时长;
        S_物品归属时间.Value = Config.物品归属时间;
        S_ItemDisappearTime.Value = Config.ItemDisappearTime;
        S_自动保存时间.Value = Config.AutoSaveInterval;
        S_自动保存日志.Value = Config.自动保存日志;
        S_沃玛分解元宝.Value = Config.沃玛分解元宝;
        S_祖玛分解元宝.Value = Config.祖玛分解元宝;
        S_赤月分解元宝.Value = Config.赤月分解元宝;
        S_魔龙分解元宝.Value = Config.魔龙分解元宝;
        S_苍月分解元宝.Value = Config.苍月分解元宝;
        S_星王分解元宝.Value = Config.星王分解元宝;
        S_神秘分解元宝.Value = Config.神秘分解元宝;
        S_城主分解元宝.Value = Config.城主分解元宝;
        S_屠魔爆率开关.Value = Config.屠魔爆率开关;
        S_屠魔组队人数.Value = Config.屠魔组队人数;
        S_屠魔令回收经验.Value = Config.屠魔令回收经验;
        S_武斗场时间一.Value = Config.武斗场时间一;
        S_武斗场时间二.Value = Config.武斗场时间二;
        S_武斗场经验小.Value = Config.武斗场经验小;
        S_武斗场经验大.Value = Config.武斗场经验大;
        S_沙巴克开启.Value = Config.沙巴克开启;
        S_沙巴克结束.Value = Config.沙巴克结束;
        S_祝福油幸运1机率.Value = Config.祝福油幸运1机率;
        S_祝福油幸运2机率.Value = Config.祝福油幸运2机率;
        S_祝福油幸运3机率.Value = Config.祝福油幸运3机率;
        S_祝福油幸运4机率.Value = Config.祝福油幸运4机率;
        S_祝福油幸运5机率.Value = Config.祝福油幸运5机率;
        S_祝福油幸运6机率.Value = Config.祝福油幸运6机率;
        S_祝福油幸运7机率.Value = Config.祝福油幸运7机率;
        S_PKYellowNamePoint.Value = Config.PKYellowNamePoint;
        S_PKRedNamePoint.Value = Config.PKRedNamePoint;
        S_PKCrimsonNamePoint.Value = Config.PKCrimsonNamePoint;
        S_锻造成功倍数.Value = Config.锻造成功倍数;
        S_死亡掉落背包几率.Value = (decimal)Config.死亡掉落背包几率;
        S_死亡掉落身上几率.Value = (decimal)Config.死亡掉落身上几率;
        S_PK死亡幸运开关.Value = Config.PK死亡幸运开关;
        S_屠魔副本次数.Value = Config.屠魔副本次数;
        S_升级经验模块一.Value = Config.升级经验模块一;
        S_升级经验模块二.Value = Config.升级经验模块二;
        S_升级经验模块三.Value = Config.升级经验模块三;
        S_升级经验模块四.Value = Config.升级经验模块四;
        S_升级经验模块五.Value = Config.升级经验模块五;
        S_升级经验模块六.Value = Config.升级经验模块六;
        S_升级经验模块七.Value = Config.升级经验模块七;
        S_升级经验模块八.Value = Config.升级经验模块八;
        S_升级经验模块九.Value = Config.升级经验模块九;
        S_升级经验模块十.Value = Config.升级经验模块十;
        S_升级经验模块十一.Value = Config.升级经验模块十一;
        S_升级经验模块十二.Value = Config.升级经验模块十二;
        S_升级经验模块十三.Value = Config.升级经验模块十三;
        S_升级经验模块十四.Value = Config.升级经验模块十四;
        S_升级经验模块十五.Value = Config.升级经验模块十五;
        S_升级经验模块十六.Value = Config.升级经验模块十六;
        S_升级经验模块十七.Value = Config.升级经验模块十七;
        S_升级经验模块十八.Value = Config.升级经验模块十八;
        S_升级经验模块十九.Value = Config.升级经验模块十九;
        S_升级经验模块二十.Value = Config.升级经验模块二十;
        S_升级经验模块二十一.Value = Config.升级经验模块二十一;
        S_升级经验模块二十二.Value = Config.升级经验模块二十二;
        S_升级经验模块二十三.Value = Config.升级经验模块二十三;
        S_升级经验模块二十四.Value = Config.升级经验模块二十四;
        S_升级经验模块二十五.Value = Config.升级经验模块二十五;
        S_升级经验模块二十六.Value = Config.升级经验模块二十六;
        S_升级经验模块二十七.Value = Config.升级经验模块二十七;
        S_升级经验模块二十八.Value = Config.升级经验模块二十八;
        S_升级经验模块二十九.Value = Config.升级经验模块二十九;
        S_升级经验模块三十.Value = Config.升级经验模块三十;
        S_高级祝福油幸运机率.Value = Config.高级祝福油幸运机率;
        S_雕爷使用物品.Value = Config.雕爷使用物品;
        S_雕爷使用金币.Value = Config.雕爷使用金币;
        S_称号范围拾取判断.Value = Config.称号范围拾取判断;
        S_TitleRangePickUpDistance.Value = Config.TitleRangePickUpDistance;
        S_行会申请人数限制.Value = Config.行会申请人数限制;
        S_疗伤药HP.Value = Config.疗伤药HP;
        S_疗伤药MP.Value = Config.疗伤药MP;
        S_万年雪霜HP.Value = Config.万年雪霜HP;
        S_万年雪霜MP.Value = Config.万年雪霜MP;
        S_元宝金币回收设定.Value = Config.元宝金币回收设定;
        S_元宝金币传送设定.Value = Config.元宝金币传送设定;
        S_快捷传送一编号.Value = Config.快捷传送一编号;
        S_快捷传送一货币.Value = Config.快捷传送一货币;
        S_快捷传送一等级.Value = Config.快捷传送一等级;
        S_快捷传送二编号.Value = Config.快捷传送二编号;
        S_快捷传送二货币.Value = Config.快捷传送二货币;
        S_快捷传送二等级.Value = Config.快捷传送二等级;
        S_快捷传送三编号.Value = Config.快捷传送三编号;
        S_快捷传送三货币.Value = Config.快捷传送三货币;
        S_快捷传送三等级.Value = Config.快捷传送三等级;
        S_快捷传送四编号.Value = Config.快捷传送四编号;
        S_快捷传送四货币.Value = Config.快捷传送四货币;
        S_快捷传送四等级.Value = Config.快捷传送四等级;
        S_快捷传送五编号.Value = Config.快捷传送五编号;
        S_快捷传送五货币.Value = Config.快捷传送五货币;
        S_快捷传送五等级.Value = Config.快捷传送五等级;
        S_狂暴货币格式.Value = Config.狂暴货币格式;
        S_狂暴称号格式.Value = Config.狂暴称号格式;
        S_狂暴开启物品名称.Value = Config.狂暴开启物品名称;
        S_狂暴开启物品数量.Value = Config.狂暴开启物品数量;
        S_狂暴杀死物品数量.Value = Config.狂暴杀死物品数量;
        S_狂暴开启元宝数量.Value = Config.狂暴开启元宝数量;
        S_狂暴杀死元宝数量.Value = Config.狂暴杀死元宝数量;
        S_狂暴开启金币数量.Value = Config.狂暴开启金币数量;
        S_狂暴杀死金币数量.Value = Config.狂暴杀死金币数量;
        S_装备技能开关.Value = Config.装备技能开关;
        S_御兽属性开启.Value = Config.御兽属性开启;
        S_可摆摊地图编号.Value = Config.可摆摊地图编号;
        S_可摆摊地图坐标X.Value = Config.可摆摊地图坐标X;
        S_可摆摊地图坐标Y.Value = Config.可摆摊地图坐标Y;
        S_可摆摊地图范围.Value = Config.可摆摊地图范围;
        S_可摆摊货币选择.Value = Config.可摆摊货币选择;
        S_可摆摊等级.Value = Config.可摆摊等级;
        S_ReviveInterval.Value = Config.ReviveInterval;
        S_自定义麻痹几率.Value = (decimal)Config.自定义麻痹几率;
        S_PetUpgradeXPLevel1.Value = Config.PetUpgradeXPLevel1;
        S_PetUpgradeXPLevel2.Value = Config.PetUpgradeXPLevel2;
        S_PetUpgradeXPLevel3.Value = Config.PetUpgradeXPLevel3;
        S_PetUpgradeXPLevel4.Value = Config.PetUpgradeXPLevel4;
        S_PetUpgradeXPLevel5.Value = Config.PetUpgradeXPLevel5;
        S_PetUpgradeXPLevel6.Value = Config.PetUpgradeXPLevel6;
        S_PetUpgradeXPLevel7.Value = Config.PetUpgradeXPLevel7;
        S_PetUpgradeXPLevel8.Value = Config.PetUpgradeXPLevel8;
        S_PetUpgradeXPLevel9.Value = Config.PetUpgradeXPLevel9;
        S_下马击落机率.Value = Config.下马击落机率;
        S_AllowRaceWarrior.Value = Config.AllowRaceWarrior;
        S_AllowRaceWizard.Value = Config.AllowRaceWizard;
        S_AllowRaceTaoist.Value = Config.AllowRaceTaoist;
        S_AllowRaceArcher.Value = Config.AllowRaceArcher;
        S_AllowRaceAssassin.Value = Config.AllowRaceAssassin;
        S_AllowRaceDragonLance.Value = Config.AllowRaceDragonLance;
        S_泡点等级开关.Value = Config.泡点等级开关;
        S_泡点当前经验.Value = Config.泡点当前经验;
        S_泡点限制等级.Value = Config.泡点限制等级;
        S_杀人PK红名开关.Value = Config.杀人PK红名开关;
        S_泡点秒数控制.Value = Config.泡点秒数控制;
        S_自定义物品数量一.Value = Config.自定义物品数量一;
        S_自定义物品数量二.Value = Config.自定义物品数量二;
        S_自定义物品数量三.Value = Config.自定义物品数量三;
        S_自定义物品数量四.Value = Config.自定义物品数量四;
        S_自定义物品数量五.Value = Config.自定义物品数量五;
        S_自定义称号内容一.Value = Config.自定义称号内容一;
        S_自定义称号内容二.Value = Config.自定义称号内容二;
        S_自定义称号内容三.Value = Config.自定义称号内容三;
        S_自定义称号内容四.Value = Config.自定义称号内容四;
        S_自定义称号内容五.Value = Config.自定义称号内容五;
        S_元宝金币传送设定2.Value = Config.元宝金币传送设定2;
        S_快捷传送一编号2.Value = Config.快捷传送一编号2;
        S_快捷传送一货币2.Value = Config.快捷传送一货币2;
        S_快捷传送一等级2.Value = Config.快捷传送一等级2;
        S_快捷传送二编号2.Value = Config.快捷传送二编号2;
        S_快捷传送二货币2.Value = Config.快捷传送二货币2;
        S_快捷传送二等级2.Value = Config.快捷传送二等级2;
        S_快捷传送三编号2.Value = Config.快捷传送三编号2;
        S_快捷传送三货币2.Value = Config.快捷传送三货币2;
        S_快捷传送三等级2.Value = Config.快捷传送三等级2;
        S_快捷传送四编号2.Value = Config.快捷传送四编号2;
        S_快捷传送四货币2.Value = Config.快捷传送四货币2;
        S_快捷传送四等级2.Value = Config.快捷传送四等级2;
        S_快捷传送五编号2.Value = Config.快捷传送五编号2;
        S_快捷传送五货币2.Value = Config.快捷传送五货币2;
        S_快捷传送五等级2.Value = Config.快捷传送五等级2;
        S_快捷传送六编号2.Value = Config.快捷传送六编号2;
        S_快捷传送六货币2.Value = Config.快捷传送六货币2;
        S_快捷传送六等级2.Value = Config.快捷传送六等级2;
        S_武斗场次数限制.Value = Config.武斗场次数限制;
        S_AutoPickUpInventorySpace.Value = Config.AutoPickUpInventorySpace;
        S_BOSS刷新提示开关.Value = Config.BOSS刷新提示开关;
        S_自动整理背包计时.Value = Config.自动整理背包计时;
        S_自动整理背包开关.Value = Config.自动整理背包开关;
        S_称号叠加开关.Value = Config.称号叠加开关;
        S_称号叠加模块一.Value = Config.称号叠加模块一;
        S_称号叠加模块二.Value = Config.称号叠加模块二;
        S_称号叠加模块三.Value = Config.称号叠加模块三;
        S_称号叠加模块四.Value = Config.称号叠加模块四;
        S_称号叠加模块五.Value = Config.称号叠加模块五;
        S_称号叠加模块六.Value = Config.称号叠加模块六;
        S_称号叠加模块七.Value = Config.称号叠加模块七;
        S_称号叠加模块八.Value = Config.称号叠加模块八;
        S_沙城传送货币开关.Value = Config.沙城传送货币开关;
        S_沙城快捷货币一.Value = Config.沙城快捷货币一;
        S_沙城快捷等级一.Value = Config.沙城快捷等级一;
        S_沙城快捷货币二.Value = Config.沙城快捷货币二;
        S_沙城快捷等级二.Value = Config.沙城快捷等级二;
        S_沙城快捷货币三.Value = Config.沙城快捷货币三;
        S_沙城快捷等级三.Value = Config.沙城快捷等级三;
        S_沙城快捷货币四.Value = Config.沙城快捷货币四;
        S_沙城快捷等级四.Value = Config.沙城快捷等级四;
        S_未知暗点副本价格.Value = Config.未知暗点副本价格;
        S_未知暗点副本等级.Value = Config.未知暗点副本等级;
        S_未知暗点二层价格.Value = Config.未知暗点二层价格;
        S_未知暗点二层等级.Value = Config.未知暗点二层等级;
        S_幽冥海副本价格.Value = Config.幽冥海副本价格;
        S_幽冥海副本等级.Value = Config.幽冥海副本等级;
        S_猎魔暗使称号一.Value = Config.猎魔暗使称号一;
        S_猎魔暗使材料一.Value = Config.猎魔暗使材料一;
        S_猎魔暗使数量一.Value = Config.猎魔暗使数量一;
        S_猎魔暗使称号二.Value = Config.猎魔暗使称号二;
        S_猎魔暗使材料二.Value = Config.猎魔暗使材料二;
        S_猎魔暗使数量二.Value = Config.猎魔暗使数量二;
        S_猎魔暗使称号三.Value = Config.猎魔暗使称号三;
        S_猎魔暗使材料三.Value = Config.猎魔暗使材料三;
        S_猎魔暗使数量三.Value = Config.猎魔暗使数量三;
        S_猎魔暗使称号四.Value = Config.猎魔暗使称号四;
        S_猎魔暗使材料四.Value = Config.猎魔暗使材料四;
        S_猎魔暗使数量四.Value = Config.猎魔暗使数量四;
        S_猎魔暗使称号五.Value = Config.猎魔暗使称号五;
        S_猎魔暗使材料五.Value = Config.猎魔暗使材料五;
        S_猎魔暗使数量五.Value = Config.猎魔暗使数量五;
        S_猎魔暗使称号六.Value = Config.猎魔暗使称号六;
        S_猎魔暗使材料六.Value = Config.猎魔暗使材料六;
        S_猎魔暗使数量六.Value = Config.猎魔暗使数量六;
        S_怪物掉落广播开关.Value = Config.怪物掉落广播开关;
        S_怪物掉落窗口开关.Value = Config.怪物掉落窗口开关;
        S_珍宝阁提示开关.Value = Config.珍宝阁提示开关;
        S_城主分解物品一.Text = Config.城主分解物品一;
        S_城主分解物品二.Text = Config.城主分解物品二;
        S_城主分解物品三.Text = Config.城主分解物品三;
        S_城主分解物品四.Text = Config.城主分解物品四;
        S_城主分解几率一.Value = Config.城主分解几率一;
        S_城主分解几率二.Value = Config.城主分解几率二;
        S_城主分解几率三.Value = Config.城主分解几率三;
        S_城主分解几率四.Value = Config.城主分解几率四;
        S_城主分解数量一.Value = Config.城主分解数量一;
        S_城主分解数量二.Value = Config.城主分解数量二;
        S_城主分解数量三.Value = Config.城主分解数量三;
        S_城主分解数量四.Value = Config.城主分解数量四;
        S_城主分解开关.Value = Config.城主分解开关;
        S_星王分解物品一.Text = Config.星王分解物品一;
        S_星王分解物品二.Text = Config.星王分解物品二;
        S_星王分解物品三.Text = Config.星王分解物品三;
        S_星王分解物品四.Text = Config.星王分解物品四;
        S_星王分解几率一.Value = Config.星王分解几率一;
        S_星王分解几率二.Value = Config.星王分解几率二;
        S_星王分解几率三.Value = Config.星王分解几率三;
        S_星王分解几率四.Value = Config.星王分解几率四;
        S_星王分解数量一.Value = Config.星王分解数量一;
        S_星王分解数量二.Value = Config.星王分解数量二;
        S_星王分解数量三.Value = Config.星王分解数量三;
        S_星王分解数量四.Value = Config.星王分解数量四;
        S_星王分解开关.Value = Config.星王分解开关;
        S_苍月分解物品一.Text = Config.苍月分解物品一;
        S_苍月分解物品二.Text = Config.苍月分解物品二;
        S_苍月分解物品三.Text = Config.苍月分解物品三;
        S_苍月分解物品四.Text = Config.苍月分解物品四;
        S_苍月分解几率一.Value = Config.苍月分解几率一;
        S_苍月分解几率二.Value = Config.苍月分解几率二;
        S_苍月分解几率三.Value = Config.苍月分解几率三;
        S_苍月分解几率四.Value = Config.苍月分解几率四;
        S_苍月分解数量一.Value = Config.苍月分解数量一;
        S_苍月分解数量二.Value = Config.苍月分解数量二;
        S_苍月分解数量三.Value = Config.苍月分解数量三;
        S_苍月分解数量四.Value = Config.苍月分解数量四;
        S_苍月分解开关.Value = Config.苍月分解开关;
        S_魔龙分解物品一.Text = Config.魔龙分解物品一;
        S_魔龙分解物品二.Text = Config.魔龙分解物品二;
        S_魔龙分解物品三.Text = Config.魔龙分解物品三;
        S_魔龙分解物品四.Text = Config.魔龙分解物品四;
        S_魔龙分解几率一.Value = Config.魔龙分解几率一;
        S_魔龙分解几率二.Value = Config.魔龙分解几率二;
        S_魔龙分解几率三.Value = Config.魔龙分解几率三;
        S_魔龙分解几率四.Value = Config.魔龙分解几率四;
        S_魔龙分解数量一.Value = Config.魔龙分解数量一;
        S_魔龙分解数量二.Value = Config.魔龙分解数量二;
        S_魔龙分解数量三.Value = Config.魔龙分解数量三;
        S_魔龙分解数量四.Value = Config.魔龙分解数量四;
        S_魔龙分解开关.Value = Config.魔龙分解开关;
        S_赤月分解物品一.Text = Config.赤月分解物品一;
        S_赤月分解物品二.Text = Config.赤月分解物品二;
        S_赤月分解物品三.Text = Config.赤月分解物品三;
        S_赤月分解物品四.Text = Config.赤月分解物品四;
        S_赤月分解几率一.Value = Config.赤月分解几率一;
        S_赤月分解几率二.Value = Config.赤月分解几率二;
        S_赤月分解几率三.Value = Config.赤月分解几率三;
        S_赤月分解几率四.Value = Config.赤月分解几率四;
        S_赤月分解数量一.Value = Config.赤月分解数量一;
        S_赤月分解数量二.Value = Config.赤月分解数量二;
        S_赤月分解数量三.Value = Config.赤月分解数量三;
        S_赤月分解数量四.Value = Config.赤月分解数量四;
        S_赤月分解开关.Value = Config.赤月分解开关;
        S_祖玛分解物品一.Text = Config.祖玛分解物品一;
        S_祖玛分解物品二.Text = Config.祖玛分解物品二;
        S_祖玛分解物品三.Text = Config.祖玛分解物品三;
        S_祖玛分解物品四.Text = Config.祖玛分解物品四;
        S_祖玛分解几率一.Value = Config.祖玛分解几率一;
        S_祖玛分解几率二.Value = Config.祖玛分解几率二;
        S_祖玛分解几率三.Value = Config.祖玛分解几率三;
        S_祖玛分解几率四.Value = Config.祖玛分解几率四;
        S_祖玛分解数量一.Value = Config.祖玛分解数量一;
        S_祖玛分解数量二.Value = Config.祖玛分解数量二;
        S_祖玛分解数量三.Value = Config.祖玛分解数量三;
        S_祖玛分解数量四.Value = Config.祖玛分解数量四;
        S_祖玛分解开关.Value = Config.祖玛分解开关;
        S_BOSS卷轴怪物一.Text = Config.BOSS卷轴怪物一;
        S_BOSS卷轴怪物二.Text = Config.BOSS卷轴怪物二;
        S_BOSS卷轴怪物三.Text = Config.BOSS卷轴怪物三;
        S_BOSS卷轴怪物四.Text = Config.BOSS卷轴怪物四;
        S_BOSS卷轴怪物五.Text = Config.BOSS卷轴怪物五;
        S_BOSS卷轴怪物六.Text = Config.BOSS卷轴怪物六;
        S_BOSS卷轴怪物七.Text = Config.BOSS卷轴怪物七;
        S_BOSS卷轴怪物八.Text = Config.BOSS卷轴怪物八;
        S_BOSS卷轴怪物九.Text = Config.BOSS卷轴怪物九;
        S_BOSS卷轴怪物十.Text = Config.BOSS卷轴怪物十;
        S_BOSS卷轴怪物11.Text = Config.BOSS卷轴怪物11;
        S_BOSS卷轴怪物12.Text = Config.BOSS卷轴怪物12;
        S_BOSS卷轴怪物13.Text = Config.BOSS卷轴怪物13;
        S_BOSS卷轴怪物14.Text = Config.BOSS卷轴怪物14;
        S_BOSS卷轴怪物15.Text = Config.BOSS卷轴怪物15;
        S_BOSS卷轴怪物16.Text = Config.BOSS卷轴怪物16;
        S_BOSS卷轴地图编号.Value = Config.BOSS卷轴地图编号;
        S_BOSS卷轴地图开关.Value = Config.BOSS卷轴地图开关;
        S_沙巴克重置系统.Value = Config.沙巴克重置系统;
        S_资源包开关.Value = Config.资源包开关;
        S_StartingLevel.Value = Config.StartingLevel;
        S_MaxUserConnections.Value = Config.MaxUserConnections;
        S_掉落贵重物品颜色.Value = Config.掉落贵重物品颜色;
        S_掉落沃玛物品颜色.Value = Config.掉落沃玛物品颜色;
        S_掉落祖玛物品颜色.Value = Config.掉落祖玛物品颜色;
        S_掉落赤月物品颜色.Value = Config.掉落赤月物品颜色;
        S_掉落魔龙物品颜色.Value = Config.掉落魔龙物品颜色;
        S_掉落苍月物品颜色.Value = Config.掉落苍月物品颜色;
        S_掉落星王物品颜色.Value = Config.掉落星王物品颜色;
        S_掉落城主物品颜色.Value = Config.掉落城主物品颜色;
        S_掉落书籍物品颜色.Value = Config.掉落书籍物品颜色;
        S_DropPlayerNameColor.Value = Config.DropPlayerNameColor;
        S_狂暴击杀玩家颜色.Value = Config.狂暴击杀玩家颜色;
        S_狂暴被杀玩家颜色.Value = Config.狂暴被杀玩家颜色;
        S_祖玛战装备佩戴数量.Value = Config.祖玛战装备佩戴数量;
        S_祖玛法装备佩戴数量.Value = Config.祖玛法装备佩戴数量;
        S_祖玛道装备佩戴数量.Value = Config.祖玛道装备佩戴数量;
        S_祖玛刺装备佩戴数量.Value = Config.祖玛刺装备佩戴数量;
        S_祖玛枪装备佩戴数量.Value = Config.祖玛枪装备佩戴数量;
        S_祖玛弓装备佩戴数量.Value = Config.祖玛弓装备佩戴数量;
        S_赤月战装备佩戴数量.Value = Config.赤月战装备佩戴数量;
        S_赤月法装备佩戴数量.Value = Config.赤月法装备佩戴数量;
        S_赤月道装备佩戴数量.Value = Config.赤月道装备佩戴数量;
        S_赤月刺装备佩戴数量.Value = Config.赤月刺装备佩戴数量;
        S_赤月枪装备佩戴数量.Value = Config.赤月枪装备佩戴数量;
        S_赤月弓装备佩戴数量.Value = Config.赤月弓装备佩戴数量;
        S_魔龙战装备佩戴数量.Value = Config.魔龙战装备佩戴数量;
        S_魔龙法装备佩戴数量.Value = Config.魔龙法装备佩戴数量;
        S_魔龙道装备佩戴数量.Value = Config.魔龙道装备佩戴数量;
        S_魔龙刺装备佩戴数量.Value = Config.魔龙刺装备佩戴数量;
        S_魔龙枪装备佩戴数量.Value = Config.魔龙枪装备佩戴数量;
        S_魔龙弓装备佩戴数量.Value = Config.魔龙弓装备佩戴数量;
        S_苍月战装备佩戴数量.Value = Config.苍月战装备佩戴数量;
        S_苍月法装备佩戴数量.Value = Config.苍月法装备佩戴数量;
        S_苍月道装备佩戴数量.Value = Config.苍月道装备佩戴数量;
        S_苍月刺装备佩戴数量.Value = Config.苍月刺装备佩戴数量;
        S_苍月枪装备佩戴数量.Value = Config.苍月枪装备佩戴数量;
        S_苍月弓装备佩戴数量.Value = Config.苍月弓装备佩戴数量;
        S_星王战装备佩戴数量.Value = Config.星王战装备佩戴数量;
        S_星王法装备佩戴数量.Value = Config.星王法装备佩戴数量;
        S_星王道装备佩戴数量.Value = Config.星王道装备佩戴数量;
        S_星王刺装备佩戴数量.Value = Config.星王刺装备佩戴数量;
        S_星王枪装备佩戴数量.Value = Config.星王枪装备佩戴数量;
        S_星王弓装备佩戴数量.Value = Config.星王弓装备佩戴数量;
        S_特殊1战装备佩戴数量.Value = Config.特殊1战装备佩戴数量;
        S_特殊1法装备佩戴数量.Value = Config.特殊1法装备佩戴数量;
        S_特殊1道装备佩戴数量.Value = Config.特殊1道装备佩戴数量;
        S_特殊1刺装备佩戴数量.Value = Config.特殊1刺装备佩戴数量;
        S_特殊1枪装备佩戴数量.Value = Config.特殊1枪装备佩戴数量;
        S_特殊1弓装备佩戴数量.Value = Config.特殊1弓装备佩戴数量;
        S_特殊2战装备佩戴数量.Value = Config.特殊2战装备佩戴数量;
        S_特殊2法装备佩戴数量.Value = Config.特殊2法装备佩戴数量;
        S_特殊2道装备佩戴数量.Value = Config.特殊2道装备佩戴数量;
        S_特殊2刺装备佩戴数量.Value = Config.特殊2刺装备佩戴数量;
        S_特殊2枪装备佩戴数量.Value = Config.特殊2枪装备佩戴数量;
        S_特殊2弓装备佩戴数量.Value = Config.特殊2弓装备佩戴数量;
        S_特殊3战装备佩戴数量.Value = Config.特殊3战装备佩戴数量;
        S_特殊3法装备佩戴数量.Value = Config.特殊3法装备佩戴数量;
        S_特殊3道装备佩戴数量.Value = Config.特殊3道装备佩戴数量;
        S_特殊3刺装备佩戴数量.Value = Config.特殊3刺装备佩戴数量;
        S_特殊3枪装备佩戴数量.Value = Config.特殊3枪装备佩戴数量;
        S_特殊3弓装备佩戴数量.Value = Config.特殊3弓装备佩戴数量;
        S_每周特惠二物品5.Value = Config.每周特惠二物品5;
        S_每周特惠二物品4.Value = Config.每周特惠二物品4;
        S_每周特惠二物品3.Value = Config.每周特惠二物品3;
        S_每周特惠二物品2.Value = Config.每周特惠二物品2;
        S_每周特惠二物品1.Value = Config.每周特惠二物品1;
        S_每周特惠一物品1.Value = Config.每周特惠一物品1;
        S_每周特惠一物品2.Value = Config.每周特惠一物品2;
        S_每周特惠一物品3.Value = Config.每周特惠一物品3;
        S_每周特惠一物品4.Value = Config.每周特惠一物品4;
        S_每周特惠一物品5.Value = Config.每周特惠一物品5;
        S_新手出售货币值.Value = Config.新手出售货币值;
        S_挂机称号选项.Value = Config.挂机称号选项;
        S_分解称号选项.Value = Config.分解称号选项;
        S_法阵卡BUG清理.Value = Config.法阵卡BUG清理;
        S_随机宝箱一物品1.Value = Config.随机宝箱一物品1;
        S_随机宝箱一物品2.Value = Config.随机宝箱一物品2;
        S_随机宝箱一物品3.Value = Config.随机宝箱一物品3;
        S_随机宝箱一物品4.Value = Config.随机宝箱一物品4;
        S_随机宝箱一物品5.Value = Config.随机宝箱一物品5;
        S_随机宝箱一物品6.Value = Config.随机宝箱一物品6;
        S_随机宝箱一物品7.Value = Config.随机宝箱一物品7;
        S_随机宝箱一物品8.Value = Config.随机宝箱一物品8;
        S_随机宝箱一几率1.Value = Config.随机宝箱一几率1;
        S_随机宝箱一几率2.Value = Config.随机宝箱一几率2;
        S_随机宝箱一几率3.Value = Config.随机宝箱一几率3;
        S_随机宝箱一几率4.Value = Config.随机宝箱一几率4;
        S_随机宝箱一几率5.Value = Config.随机宝箱一几率5;
        S_随机宝箱一几率6.Value = Config.随机宝箱一几率6;
        S_随机宝箱一几率7.Value = Config.随机宝箱一几率7;
        S_随机宝箱一几率8.Value = Config.随机宝箱一几率8;
        S_随机宝箱二物品1.Value = Config.随机宝箱二物品1;
        S_随机宝箱二物品2.Value = Config.随机宝箱二物品2;
        S_随机宝箱二物品3.Value = Config.随机宝箱二物品3;
        S_随机宝箱二物品4.Value = Config.随机宝箱二物品4;
        S_随机宝箱二物品5.Value = Config.随机宝箱二物品5;
        S_随机宝箱二物品6.Value = Config.随机宝箱二物品6;
        S_随机宝箱二物品7.Value = Config.随机宝箱二物品7;
        S_随机宝箱二物品8.Value = Config.随机宝箱二物品8;
        S_随机宝箱二几率1.Value = Config.随机宝箱二几率1;
        S_随机宝箱二几率2.Value = Config.随机宝箱二几率2;
        S_随机宝箱二几率3.Value = Config.随机宝箱二几率3;
        S_随机宝箱二几率4.Value = Config.随机宝箱二几率4;
        S_随机宝箱二几率5.Value = Config.随机宝箱二几率5;
        S_随机宝箱二几率6.Value = Config.随机宝箱二几率6;
        S_随机宝箱二几率7.Value = Config.随机宝箱二几率7;
        S_随机宝箱二几率8.Value = Config.随机宝箱二几率8;
        S_随机宝箱三物品1.Value = Config.随机宝箱三物品1;
        S_随机宝箱三物品2.Value = Config.随机宝箱三物品2;
        S_随机宝箱三物品3.Value = Config.随机宝箱三物品3;
        S_随机宝箱三物品4.Value = Config.随机宝箱三物品4;
        S_随机宝箱三物品5.Value = Config.随机宝箱三物品5;
        S_随机宝箱三物品6.Value = Config.随机宝箱三物品6;
        S_随机宝箱三物品7.Value = Config.随机宝箱三物品7;
        S_随机宝箱三物品8.Value = Config.随机宝箱三物品8;
        S_随机宝箱三几率1.Value = Config.随机宝箱三几率1;
        S_随机宝箱三几率2.Value = Config.随机宝箱三几率2;
        S_随机宝箱三几率3.Value = Config.随机宝箱三几率3;
        S_随机宝箱三几率4.Value = Config.随机宝箱三几率4;
        S_随机宝箱三几率5.Value = Config.随机宝箱三几率5;
        S_随机宝箱三几率6.Value = Config.随机宝箱三几率6;
        S_随机宝箱三几率7.Value = Config.随机宝箱三几率7;
        S_随机宝箱三几率8.Value = Config.随机宝箱三几率8;
        S_随机宝箱一数量1.Value = Config.随机宝箱一数量1;
        S_随机宝箱一数量2.Value = Config.随机宝箱一数量2;
        S_随机宝箱一数量3.Value = Config.随机宝箱一数量3;
        S_随机宝箱一数量4.Value = Config.随机宝箱一数量4;
        S_随机宝箱一数量5.Value = Config.随机宝箱一数量5;
        S_随机宝箱一数量6.Value = Config.随机宝箱一数量6;
        S_随机宝箱一数量7.Value = Config.随机宝箱一数量7;
        S_随机宝箱一数量8.Value = Config.随机宝箱一数量8;
        S_随机宝箱二数量1.Value = Config.随机宝箱二数量1;
        S_随机宝箱二数量2.Value = Config.随机宝箱二数量2;
        S_随机宝箱二数量3.Value = Config.随机宝箱二数量3;
        S_随机宝箱二数量4.Value = Config.随机宝箱二数量4;
        S_随机宝箱二数量5.Value = Config.随机宝箱二数量5;
        S_随机宝箱二数量6.Value = Config.随机宝箱二数量6;
        S_随机宝箱二数量7.Value = Config.随机宝箱二数量7;
        S_随机宝箱二数量8.Value = Config.随机宝箱二数量8;
        S_随机宝箱三数量1.Value = Config.随机宝箱三数量1;
        S_随机宝箱三数量2.Value = Config.随机宝箱三数量2;
        S_随机宝箱三数量3.Value = Config.随机宝箱三数量3;
        S_随机宝箱三数量4.Value = Config.随机宝箱三数量4;
        S_随机宝箱三数量5.Value = Config.随机宝箱三数量5;
        S_随机宝箱三数量6.Value = Config.随机宝箱三数量6;
        S_随机宝箱三数量7.Value = Config.随机宝箱三数量7;
        S_随机宝箱三数量8.Value = Config.随机宝箱三数量8;
        S_沙城地图保护.Value = Config.沙城地图保护;
        S_NoobProtectionLevel.Value = Config.NoobProtectionLevel;
        S_新手地图保护1.Value = Config.新手地图保护1;
        S_新手地图保护2.Value = Config.新手地图保护2;
        S_新手地图保护3.Value = Config.新手地图保护3;
        S_新手地图保护4.Value = Config.新手地图保护4;
        S_新手地图保护5.Value = Config.新手地图保护5;
        S_新手地图保护6.Value = Config.新手地图保护6;
        S_新手地图保护7.Value = Config.新手地图保护7;
        S_新手地图保护8.Value = Config.新手地图保护8;
        S_新手地图保护9.Value = Config.新手地图保护9;
        S_新手地图保护10.Value = Config.新手地图保护10;
        S_沙巴克停止开关.Value = Config.沙巴克停止开关;
        S_沙巴克城主称号.Value = Config.沙巴克城主称号;
        S_沙巴克成员称号.Value = Config.沙巴克成员称号;
        S_沙巴克称号领取开关.Value = Config.沙巴克称号领取开关;
        S_通用1装备佩戴数量.Value = Config.通用1装备佩戴数量;
        S_通用2装备佩戴数量.Value = Config.通用2装备佩戴数量;
        S_通用3装备佩戴数量.Value = Config.通用3装备佩戴数量;
        S_通用4装备佩戴数量.Value = Config.通用4装备佩戴数量;
        S_通用5装备佩戴数量.Value = Config.通用5装备佩戴数量;
        S_通用6装备佩戴数量.Value = Config.通用6装备佩戴数量;
        S_重置屠魔副本时间.Value = Config.重置屠魔副本时间;
        S_屠魔令回收数量.Value = Config.重置屠魔副本时间;
        S_新手上线赠送开关.Value = Config.新手上线赠送开关;
        S_新手上线赠送物品1.Value = Config.新手上线赠送物品1;
        S_新手上线赠送物品2.Value = Config.新手上线赠送物品2;
        S_新手上线赠送物品3.Value = Config.新手上线赠送物品3;
        S_新手上线赠送物品4.Value = Config.新手上线赠送物品4;
        S_新手上线赠送物品5.Value = Config.新手上线赠送物品5;
        S_新手上线赠送物品6.Value = Config.新手上线赠送物品6;
        S_新手上线赠送称号1.Value = Config.新手上线赠送称号1;
        S_元宝袋新创数量1.Value = Config.元宝袋新创数量1;
        S_元宝袋新创数量2.Value = Config.元宝袋新创数量2;
        S_元宝袋新创数量3.Value = Config.元宝袋新创数量3;
        S_元宝袋新创数量4.Value = Config.元宝袋新创数量4;
        S_元宝袋新创数量5.Value = Config.元宝袋新创数量5;
        S_初级赞助礼包1.Value = Config.初级赞助礼包1;
        S_初级赞助礼包2.Value = Config.初级赞助礼包2;
        S_初级赞助礼包3.Value = Config.初级赞助礼包3;
        S_初级赞助礼包4.Value = Config.初级赞助礼包4;
        S_初级赞助礼包5.Value = Config.初级赞助礼包5;
        S_初级赞助礼包6.Value = Config.初级赞助礼包6;
        S_初级赞助礼包7.Value = Config.初级赞助礼包7;
        S_初级赞助礼包8.Value = Config.初级赞助礼包8;
        S_初级赞助称号1.Value = Config.初级赞助称号1;
        S_中级赞助礼包1.Value = Config.中级赞助礼包1;
        S_中级赞助礼包2.Value = Config.中级赞助礼包2;
        S_中级赞助礼包3.Value = Config.中级赞助礼包3;
        S_中级赞助礼包4.Value = Config.中级赞助礼包4;
        S_中级赞助礼包5.Value = Config.中级赞助礼包5;
        S_中级赞助礼包6.Value = Config.中级赞助礼包6;
        S_中级赞助礼包7.Value = Config.中级赞助礼包7;
        S_中级赞助礼包8.Value = Config.中级赞助礼包8;
        S_中级赞助称号1.Value = Config.中级赞助称号1;
        S_高级赞助礼包1.Value = Config.高级赞助礼包1;
        S_高级赞助礼包2.Value = Config.高级赞助礼包2;
        S_高级赞助礼包3.Value = Config.高级赞助礼包3;
        S_高级赞助礼包4.Value = Config.高级赞助礼包4;
        S_高级赞助礼包5.Value = Config.高级赞助礼包5;
        S_高级赞助礼包6.Value = Config.高级赞助礼包6;
        S_高级赞助礼包7.Value = Config.高级赞助礼包7;
        S_高级赞助礼包8.Value = Config.高级赞助礼包8;
        S_高级赞助称号1.Value = Config.高级赞助称号1;
        S_平台开关模式.Value = Config.平台开关模式;
        S_平台元宝充值模块.Value = Config.平台元宝充值模块;
        S_九层妖塔数量1.Value = Config.九层妖塔数量1;
        S_九层妖塔数量2.Value = Config.九层妖塔数量2;
        S_九层妖塔数量3.Value = Config.九层妖塔数量3;
        S_九层妖塔数量4.Value = Config.九层妖塔数量4;
        S_九层妖塔数量5.Value = Config.九层妖塔数量5;
        S_九层妖塔数量6.Value = Config.九层妖塔数量6;
        S_九层妖塔数量7.Value = Config.九层妖塔数量7;
        S_九层妖塔数量8.Value = Config.九层妖塔数量8;
        S_九层妖塔数量9.Value = Config.九层妖塔数量9;
        S_九层妖塔副本次数.Value = Config.九层妖塔副本次数;
        S_九层妖塔副本等级.Value = Config.九层妖塔副本等级;
        S_九层妖塔副本物品.Value = Config.九层妖塔副本物品;
        S_九层妖塔副本数量.Value = Config.九层妖塔副本数量;
        S_九层妖塔副本时间小.Value = Config.九层妖塔副本时间小;
        S_九层妖塔副本时间大.Value = Config.九层妖塔副本时间大;
        S_九层妖塔BOSS1.Text = Config.九层妖塔BOSS1;
        S_九层妖塔BOSS2.Text = Config.九层妖塔BOSS2;
        S_九层妖塔BOSS3.Text = Config.九层妖塔BOSS3;
        S_九层妖塔BOSS4.Text = Config.九层妖塔BOSS4;
        S_九层妖塔BOSS5.Text = Config.九层妖塔BOSS5;
        S_九层妖塔BOSS6.Text = Config.九层妖塔BOSS6;
        S_九层妖塔BOSS7.Text = Config.九层妖塔BOSS7;
        S_九层妖塔BOSS8.Text = Config.九层妖塔BOSS8;
        S_九层妖塔BOSS9.Text = Config.九层妖塔BOSS9;
        S_九层妖塔精英1.Text = Config.九层妖塔精英1;
        S_九层妖塔精英2.Text = Config.九层妖塔精英2;
        S_九层妖塔精英3.Text = Config.九层妖塔精英3;
        S_九层妖塔精英4.Text = Config.九层妖塔精英4;
        S_九层妖塔精英5.Text = Config.九层妖塔精英5;
        S_九层妖塔精英6.Text = Config.九层妖塔精英6;
        S_九层妖塔精英7.Text = Config.九层妖塔精英7;
        S_九层妖塔精英8.Text = Config.九层妖塔精英8;
        S_九层妖塔精英9.Text = Config.九层妖塔精英9;
        S_AutoBattleLevel.Value = Config.AutoBattleLevel;
        S_禁止背包铭文洗练.Value = Config.禁止背包铭文洗练;
        S_沙巴克禁止随机.Value = Config.沙巴克禁止随机;
        S_冥想丹自定义经验.Value = Config.冥想丹自定义经验;
        S_沙巴克爆装备开关.Value = Config.沙巴克爆装备开关;
        S_铭文战士1挡1次数.Value = Config.铭文战士1挡1次数;
        S_铭文战士1挡2次数.Value = Config.铭文战士1挡2次数;
        S_铭文战士1挡3次数.Value = Config.铭文战士1挡3次数;
        S_铭文战士1挡1概率.Value = Config.铭文战士1挡1概率;
        S_铭文战士1挡2概率.Value = Config.铭文战士1挡2概率;
        S_铭文战士1挡3概率.Value = Config.铭文战士1挡3概率;
        S_铭文战士1挡技能编号.Value = Config.铭文战士1挡技能编号;
        S_铭文战士1挡技能铭文.Value = Config.铭文战士1挡技能铭文;
        S_铭文战士2挡1次数.Value = Config.铭文战士2挡1次数;
        S_铭文战士2挡2次数.Value = Config.铭文战士2挡2次数;
        S_铭文战士2挡3次数.Value = Config.铭文战士2挡3次数;
        S_铭文战士2挡1概率.Value = Config.铭文战士2挡1概率;
        S_铭文战士2挡2概率.Value = Config.铭文战士2挡2概率;
        S_铭文战士2挡3概率.Value = Config.铭文战士2挡3概率;
        S_铭文战士2挡技能编号.Value = Config.铭文战士2挡技能编号;
        S_铭文战士2挡技能铭文.Value = Config.铭文战士2挡技能铭文;
        S_铭文战士3挡1次数.Value = Config.铭文战士3挡1次数;
        S_铭文战士3挡2次数.Value = Config.铭文战士3挡2次数;
        S_铭文战士3挡3次数.Value = Config.铭文战士3挡3次数;
        S_铭文战士3挡1概率.Value = Config.铭文战士3挡1概率;
        S_铭文战士3挡2概率.Value = Config.铭文战士3挡2概率;
        S_铭文战士3挡3概率.Value = Config.铭文战士3挡3概率;
        S_铭文战士3挡技能编号.Value = Config.铭文战士3挡技能编号;
        S_铭文战士3挡技能铭文.Value = Config.铭文战士3挡技能铭文;
        S_铭文法师1挡1次数.Value = Config.铭文法师1挡1次数;
        S_铭文法师1挡2次数.Value = Config.铭文法师1挡2次数;
        S_铭文法师1挡3次数.Value = Config.铭文法师1挡3次数;
        S_铭文法师1挡1概率.Value = Config.铭文法师1挡1概率;
        S_铭文法师1挡2概率.Value = Config.铭文法师1挡2概率;
        S_铭文法师1挡3概率.Value = Config.铭文法师1挡3概率;
        S_铭文法师1挡技能编号.Value = Config.铭文法师1挡技能编号;
        S_铭文法师1挡技能铭文.Value = Config.铭文法师1挡技能铭文;
        S_铭文法师2挡1次数.Value = Config.铭文法师2挡1次数;
        S_铭文法师2挡2次数.Value = Config.铭文法师2挡2次数;
        S_铭文法师2挡3次数.Value = Config.铭文法师2挡3次数;
        S_铭文法师2挡1概率.Value = Config.铭文法师2挡1概率;
        S_铭文法师2挡2概率.Value = Config.铭文法师2挡2概率;
        S_铭文法师2挡3概率.Value = Config.铭文法师2挡3概率;
        S_铭文法师2挡技能编号.Value = Config.铭文法师2挡技能编号;
        S_铭文法师2挡技能铭文.Value = Config.铭文法师2挡技能铭文;
        S_铭文法师3挡1次数.Value = Config.铭文法师3挡1次数;
        S_铭文法师3挡2次数.Value = Config.铭文法师3挡2次数;
        S_铭文法师3挡3次数.Value = Config.铭文法师3挡3次数;
        S_铭文法师3挡1概率.Value = Config.铭文法师3挡1概率;
        S_铭文法师3挡2概率.Value = Config.铭文法师3挡2概率;
        S_铭文法师3挡3概率.Value = Config.铭文法师3挡3概率;
        S_铭文法师3挡技能编号.Value = Config.铭文法师3挡技能编号;
        S_铭文法师3挡技能铭文.Value = Config.铭文法师3挡技能铭文;
        S_铭文道士1挡1次数.Value = Config.铭文道士1挡1次数;
        S_铭文道士1挡2次数.Value = Config.铭文道士1挡2次数;
        S_铭文道士1挡3次数.Value = Config.铭文道士1挡3次数;
        S_铭文道士1挡1概率.Value = Config.铭文道士1挡1概率;
        S_铭文道士1挡2概率.Value = Config.铭文道士1挡2概率;
        S_铭文道士1挡3概率.Value = Config.铭文道士1挡3概率;
        S_铭文道士1挡技能编号.Value = Config.铭文道士1挡技能编号;
        S_铭文道士1挡技能铭文.Value = Config.铭文道士1挡技能铭文;
        S_铭文道士2挡1次数.Value = Config.铭文道士2挡1次数;
        S_铭文道士2挡2次数.Value = Config.铭文道士2挡2次数;
        S_铭文道士2挡3次数.Value = Config.铭文道士2挡3次数;
        S_铭文道士2挡1概率.Value = Config.铭文道士2挡1概率;
        S_铭文道士2挡2概率.Value = Config.铭文道士2挡2概率;
        S_铭文道士2挡3概率.Value = Config.铭文道士2挡3概率;
        S_铭文道士2挡技能编号.Value = Config.铭文道士2挡技能编号;
        S_铭文道士2挡技能铭文.Value = Config.铭文道士2挡技能铭文;
        S_铭文道士3挡1次数.Value = Config.铭文道士3挡1次数;
        S_铭文道士3挡2次数.Value = Config.铭文道士3挡2次数;
        S_铭文道士3挡3次数.Value = Config.铭文道士3挡3次数;
        S_铭文道士3挡1概率.Value = Config.铭文道士3挡1概率;
        S_铭文道士3挡2概率.Value = Config.铭文道士3挡2概率;
        S_铭文道士3挡3概率.Value = Config.铭文道士3挡3概率;
        S_铭文道士3挡技能编号.Value = Config.铭文道士3挡技能编号;
        S_铭文道士3挡技能铭文.Value = Config.铭文道士3挡技能铭文;
        S_铭文刺客1挡1次数.Value = Config.铭文刺客1挡1次数;
        S_铭文刺客1挡2次数.Value = Config.铭文刺客1挡2次数;
        S_铭文刺客1挡3次数.Value = Config.铭文刺客1挡3次数;
        S_铭文刺客1挡1概率.Value = Config.铭文刺客1挡1概率;
        S_铭文刺客1挡2概率.Value = Config.铭文刺客1挡2概率;
        S_铭文刺客1挡3概率.Value = Config.铭文刺客1挡3概率;
        S_铭文刺客1挡技能编号.Value = Config.铭文刺客1挡技能编号;
        S_铭文刺客1挡技能铭文.Value = Config.铭文刺客1挡技能铭文;
        S_铭文刺客2挡1次数.Value = Config.铭文刺客2挡1次数;
        S_铭文刺客2挡2次数.Value = Config.铭文刺客2挡2次数;
        S_铭文刺客2挡3次数.Value = Config.铭文刺客2挡3次数;
        S_铭文刺客2挡1概率.Value = Config.铭文刺客2挡1概率;
        S_铭文刺客2挡2概率.Value = Config.铭文刺客2挡2概率;
        S_铭文刺客2挡3概率.Value = Config.铭文刺客2挡3概率;
        S_铭文刺客2挡技能编号.Value = Config.铭文刺客2挡技能编号;
        S_铭文刺客2挡技能铭文.Value = Config.铭文刺客2挡技能铭文;
        S_铭文刺客3挡1次数.Value = Config.铭文刺客3挡1次数;
        S_铭文刺客3挡2次数.Value = Config.铭文刺客3挡2次数;
        S_铭文刺客3挡3次数.Value = Config.铭文刺客3挡3次数;
        S_铭文刺客3挡1概率.Value = Config.铭文刺客3挡1概率;
        S_铭文刺客3挡2概率.Value = Config.铭文刺客3挡2概率;
        S_铭文刺客3挡3概率.Value = Config.铭文刺客3挡3概率;
        S_铭文刺客3挡技能编号.Value = Config.铭文刺客3挡技能编号;
        S_铭文刺客3挡技能铭文.Value = Config.铭文刺客3挡技能铭文;
        S_铭文弓手1挡1次数.Value = Config.铭文弓手1挡1次数;
        S_铭文弓手1挡2次数.Value = Config.铭文弓手1挡2次数;
        S_铭文弓手1挡3次数.Value = Config.铭文弓手1挡3次数;
        S_铭文弓手1挡1概率.Value = Config.铭文弓手1挡1概率;
        S_铭文弓手1挡2概率.Value = Config.铭文弓手1挡2概率;
        S_铭文弓手1挡3概率.Value = Config.铭文弓手1挡3概率;
        S_铭文弓手1挡技能编号.Value = Config.铭文弓手1挡技能编号;
        S_铭文弓手1挡技能铭文.Value = Config.铭文弓手1挡技能铭文;
        S_铭文弓手2挡1次数.Value = Config.铭文弓手2挡1次数;
        S_铭文弓手2挡2次数.Value = Config.铭文弓手2挡2次数;
        S_铭文弓手2挡3次数.Value = Config.铭文弓手2挡3次数;
        S_铭文弓手2挡1概率.Value = Config.铭文弓手2挡1概率;
        S_铭文弓手2挡2概率.Value = Config.铭文弓手2挡2概率;
        S_铭文弓手2挡3概率.Value = Config.铭文弓手2挡3概率;
        S_铭文弓手2挡技能编号.Value = Config.铭文弓手2挡技能编号;
        S_铭文弓手2挡技能铭文.Value = Config.铭文弓手2挡技能铭文;
        S_铭文弓手3挡1次数.Value = Config.铭文弓手3挡1次数;
        S_铭文弓手3挡2次数.Value = Config.铭文弓手3挡2次数;
        S_铭文弓手3挡3次数.Value = Config.铭文弓手3挡3次数;
        S_铭文弓手3挡1概率.Value = Config.铭文弓手3挡1概率;
        S_铭文弓手3挡2概率.Value = Config.铭文弓手3挡2概率;
        S_铭文弓手3挡3概率.Value = Config.铭文弓手3挡3概率;
        S_铭文弓手3挡技能编号.Value = Config.铭文弓手3挡技能编号;
        S_铭文弓手3挡技能铭文.Value = Config.铭文弓手3挡技能铭文;
        S_铭文龙枪1挡1次数.Value = Config.铭文龙枪1挡1次数;
        S_铭文龙枪1挡2次数.Value = Config.铭文龙枪1挡2次数;
        S_铭文龙枪1挡3次数.Value = Config.铭文龙枪1挡3次数;
        S_铭文龙枪1挡1概率.Value = Config.铭文龙枪1挡1概率;
        S_铭文龙枪1挡2概率.Value = Config.铭文龙枪1挡2概率;
        S_铭文龙枪1挡3概率.Value = Config.铭文龙枪1挡3概率;
        S_铭文龙枪1挡技能编号.Value = Config.铭文龙枪1挡技能编号;
        S_铭文龙枪1挡技能铭文.Value = Config.铭文龙枪1挡技能铭文;
        S_铭文龙枪2挡1次数.Value = Config.铭文龙枪2挡1次数;
        S_铭文龙枪2挡2次数.Value = Config.铭文龙枪2挡2次数;
        S_铭文龙枪2挡3次数.Value = Config.铭文龙枪2挡3次数;
        S_铭文龙枪2挡1概率.Value = Config.铭文龙枪2挡1概率;
        S_铭文龙枪2挡2概率.Value = Config.铭文龙枪2挡2概率;
        S_铭文龙枪2挡3概率.Value = Config.铭文龙枪2挡3概率;
        S_铭文龙枪2挡技能编号.Value = Config.铭文龙枪2挡技能编号;
        S_铭文龙枪2挡技能铭文.Value = Config.铭文龙枪2挡技能铭文;
        S_铭文龙枪3挡1次数.Value = Config.铭文龙枪3挡1次数;
        S_铭文龙枪3挡2次数.Value = Config.铭文龙枪3挡2次数;
        S_铭文龙枪3挡3次数.Value = Config.铭文龙枪3挡3次数;
        S_铭文龙枪3挡1概率.Value = Config.铭文龙枪3挡1概率;
        S_铭文龙枪3挡2概率.Value = Config.铭文龙枪3挡2概率;
        S_铭文龙枪3挡3概率.Value = Config.铭文龙枪3挡3概率;
        S_铭文龙枪3挡技能编号.Value = Config.铭文龙枪3挡技能编号;
        S_铭文龙枪3挡技能铭文.Value = Config.铭文龙枪3挡技能铭文;
        S_铭文道士保底开关.Value = Config.铭文道士保底开关;
        S_铭文战士保底开关.Value = Config.铭文战士保底开关;
        S_铭文法师保底开关.Value = Config.铭文法师保底开关;
        S_铭文刺客保底开关.Value = Config.铭文刺客保底开关;
        S_铭文弓手保底开关.Value = Config.铭文弓手保底开关;
        S_铭文龙枪保底开关.Value = Config.铭文龙枪保底开关;
        S_DropRateModifier.Value = Config.DropRateModifier;
        S_魔虫窟副本次数.Value = Config.魔虫窟副本次数;
        S_魔虫窟副本等级.Value = Config.魔虫窟副本等级;
        S_魔虫窟副本物品.Value = Config.魔虫窟副本物品;
        S_魔虫窟副本数量.Value = Config.魔虫窟副本数量;
        S_魔虫窟副本时间小.Value = Config.魔虫窟副本时间小;
        S_魔虫窟副本时间大.Value = Config.魔虫窟副本时间大;
        S_书店商贩物品.Text = Config.书店商贩物品;
        S_幸运洗练次数保底.Value = Config.幸运洗练次数保底;
        S_幸运洗练点数.Value = Config.幸运洗练点数;
        S_武器强化消耗货币值.Value = Config.武器强化消耗货币值;
        S_武器强化消耗货币开关.Value = Config.武器强化消耗货币开关;
        S_武器强化取回时间.Value = Config.武器强化取回时间;
        S_幸运额外1值.Value = Config.幸运额外1值;
        S_幸运额外2值.Value = Config.幸运额外2值;
        S_幸运额外3值.Value = Config.幸运额外3值;
        S_幸运额外4值.Value = Config.幸运额外4值;
        S_幸运额外5值.Value = Config.幸运额外5值;
        S_幸运额外1伤害.Value = (decimal)Config.幸运额外1伤害;
        S_幸运额外2伤害.Value = (decimal)Config.幸运额外2伤害;
        S_幸运额外3伤害.Value = (decimal)Config.幸运额外3伤害;
        S_幸运额外4伤害.Value = (decimal)Config.幸运额外4伤害;
        S_幸运额外5伤害.Value = (decimal)Config.幸运额外5伤害;
        S_暗之门地图1.Value = Config.暗之门地图1;
        S_暗之门地图2.Value = Config.暗之门地图2;
        S_暗之门地图3.Value = Config.暗之门地图3;
        S_暗之门地图4.Value = Config.暗之门地图4;
        S_暗之门全服提示.Value = Config.暗之门全服提示;
        S_暗之门杀怪触发.Value = Config.暗之门杀怪触发;
        S_暗之门时间.Value = Config.暗之门时间;
        S_暗之门地图1BOSS.Text = Config.暗之门地图1BOSS;
        S_暗之门地图2BOSS.Text = Config.暗之门地图2BOSS;
        S_暗之门地图3BOSS.Text = Config.暗之门地图3BOSS;
        S_暗之门地图4BOSS.Text = Config.暗之门地图4BOSS;
        S_暗之门地图1X.Value = Config.暗之门地图1X;
        S_暗之门地图1Y.Value = Config.暗之门地图1Y;
        S_暗之门地图2X.Value = Config.暗之门地图2X;
        S_暗之门地图2Y.Value = Config.暗之门地图2Y;
        S_暗之门地图3X.Value = Config.暗之门地图3X;
        S_暗之门地图3Y.Value = Config.暗之门地图3Y;
        S_暗之门地图4X.Value = Config.暗之门地图4X;
        S_暗之门地图4Y.Value = Config.暗之门地图4Y;
        S_暗之门开关.Value = Config.暗之门开关;
        S_监狱货币类型.Value = Config.监狱货币类型;
        S_监狱货币.Value = Config.监狱货币;
        S_魔虫窟分钟限制.Value = Config.魔虫窟分钟限制;
        S_自定义元宝兑换01.Value = Config.自定义元宝兑换01;
        S_自定义元宝兑换02.Value = Config.自定义元宝兑换02;
        S_自定义元宝兑换03.Value = Config.自定义元宝兑换03;
        S_自定义元宝兑换04.Value = Config.自定义元宝兑换04;
        S_自定义元宝兑换05.Value = Config.自定义元宝兑换05;
        S_直升等级1.Value = Config.直升等级1;
        S_直升等级2.Value = Config.直升等级2;
        S_直升等级3.Value = Config.直升等级3;
        S_直升等级4.Value = Config.直升等级4;
        S_直升等级5.Value = Config.直升等级5;
        S_直升等级6.Value = Config.直升等级6;
        S_直升等级7.Value = Config.直升等级7;
        S_直升等级8.Value = Config.直升等级8;
        S_直升等级9.Value = Config.直升等级9;
        S_直升经验1.Value = Config.直升经验1;
        S_直升经验2.Value = Config.直升经验2;
        S_直升经验3.Value = Config.直升经验3;
        S_直升经验4.Value = Config.直升经验4;
        S_直升经验5.Value = Config.直升经验5;
        S_直升经验6.Value = Config.直升经验6;
        S_直升经验7.Value = Config.直升经验7;
        S_直升经验8.Value = Config.直升经验8;
        S_直升经验9.Value = Config.直升经验9;
        S_直升物品1.Value = Config.直升物品1;
        S_直升物品2.Value = Config.直升物品2;
        S_直升物品3.Value = Config.直升物品3;
        S_直升物品4.Value = Config.直升物品4;
        S_直升物品5.Value = Config.直升物品5;
        S_直升物品6.Value = Config.直升物品6;
        S_直升物品7.Value = Config.直升物品7;
        S_直升物品8.Value = Config.直升物品8;
        S_直升物品9.Value = Config.直升物品9;
        S_充值模块格式.Value = Config.充值模块格式;
        UpgradeXPLevel1.Value = Config.UpgradeXPLevel1;
        UpgradeXPLevel2.Value = Config.UpgradeXPLevel2;
        UpgradeXPLevel3.Value = Config.UpgradeXPLevel3;
        UpgradeXPLevel4.Value = Config.UpgradeXPLevel4;
        UpgradeXPLevel5.Value = Config.UpgradeXPLevel5;
        UpgradeXPLevel6.Value = Config.UpgradeXPLevel6;
        UpgradeXPLevel7.Value = Config.UpgradeXPLevel7;
        UpgradeXPLevel8.Value = Config.UpgradeXPLevel8;
        UpgradeXPLevel9.Value = Config.UpgradeXPLevel9;
        UpgradeXPLevel10.Value = Config.UpgradeXPLevel10;
        UpgradeXPLevel11.Value = Config.UpgradeXPLevel11;
        UpgradeXPLevel12.Value = Config.UpgradeXPLevel12;
        UpgradeXPLevel13.Value = Config.UpgradeXPLevel13;
        UpgradeXPLevel14.Value = Config.UpgradeXPLevel14;
        UpgradeXPLevel15.Value = Config.UpgradeXPLevel15;
        UpgradeXPLevel16.Value = Config.UpgradeXPLevel16;
        UpgradeXPLevel17.Value = Config.UpgradeXPLevel17;
        UpgradeXPLevel18.Value = Config.UpgradeXPLevel18;
        UpgradeXPLevel19.Value = Config.UpgradeXPLevel19;
        UpgradeXPLevel20.Value = Config.UpgradeXPLevel20;
        UpgradeXPLevel21.Value = Config.UpgradeXPLevel21;
        UpgradeXPLevel22.Value = Config.UpgradeXPLevel22;
        UpgradeXPLevel23.Value = Config.UpgradeXPLevel23;
        UpgradeXPLevel24.Value = Config.UpgradeXPLevel24;
        UpgradeXPLevel25.Value = Config.UpgradeXPLevel25;
        UpgradeXPLevel26.Value = Config.UpgradeXPLevel26;
        UpgradeXPLevel27.Value = Config.UpgradeXPLevel27;
        UpgradeXPLevel28.Value = Config.UpgradeXPLevel28;
        UpgradeXPLevel29.Value = Config.UpgradeXPLevel29;
        UpgradeXPLevel30.Value = Config.UpgradeXPLevel30;
        UpgradeXPLevel31.Value = Config.UpgradeXPLevel31;
        UpgradeXPLevel32.Value = Config.UpgradeXPLevel32;
        UpgradeXPLevel33.Value = Config.UpgradeXPLevel33;
        UpgradeXPLevel34.Value = Config.UpgradeXPLevel34;
        UpgradeXPLevel35.Value = Config.UpgradeXPLevel35;
        UpgradeXPLevel36.Value = Config.UpgradeXPLevel36;
        UpgradeXPLevel37.Value = Config.UpgradeXPLevel37;
        UpgradeXPLevel38.Value = Config.UpgradeXPLevel38;
        UpgradeXPLevel39.Value = Config.UpgradeXPLevel39;
        DefaultSkillLevel.Value = Config.DefaultSkillLevel;
        S_沃玛分解物品一.Text = Config.沃玛分解物品一;
        S_沃玛分解物品二.Text = Config.沃玛分解物品二;
        S_沃玛分解物品三.Text = Config.沃玛分解物品三;
        S_沃玛分解物品四.Text = Config.沃玛分解物品四;
        S_沃玛分解几率一.Value = Config.沃玛分解几率一;
        S_沃玛分解几率二.Value = Config.沃玛分解几率二;
        S_沃玛分解几率三.Value = Config.沃玛分解几率三;
        S_沃玛分解几率四.Value = Config.沃玛分解几率四;
        S_沃玛分解数量一.Value = Config.沃玛分解数量一;
        S_沃玛分解数量二.Value = Config.沃玛分解数量二;
        S_沃玛分解数量三.Value = Config.沃玛分解数量三;
        S_沃玛分解数量四.Value = Config.沃玛分解数量四;
        S_沃玛分解开关.Value = Config.沃玛分解开关;
        S_其他分解物品一.Text = Config.其他分解物品一;
        S_其他分解物品二.Text = Config.其他分解物品二;
        S_其他分解物品三.Text = Config.其他分解物品三;
        S_其他分解物品四.Text = Config.其他分解物品四;
        S_其他分解几率一.Value = Config.其他分解几率一;
        S_其他分解几率二.Value = Config.其他分解几率二;
        S_其他分解几率三.Value = Config.其他分解几率三;
        S_其他分解几率四.Value = Config.其他分解几率四;
        S_其他分解数量一.Value = Config.其他分解数量一;
        S_其他分解数量二.Value = Config.其他分解数量二;
        S_其他分解数量三.Value = Config.其他分解数量三;
        S_其他分解数量四.Value = Config.其他分解数量四;
        S_其他分解开关.Value = Config.其他分解开关;
        拾取地图控制1.Value = Config.AutoPickUpMap1;
        拾取地图控制2.Value = Config.AutoPickUpMap2;
        拾取地图控制3.Value = Config.AutoPickUpMap3;
        拾取地图控制4.Value = Config.AutoPickUpMap4;
        拾取地图控制5.Value = Config.AutoPickUpMap5;
        拾取地图控制6.Value = Config.AutoPickUpMap6;
        拾取地图控制7.Value = Config.AutoPickUpMap7;
        拾取地图控制8.Value = Config.AutoPickUpMap8;
        沙城捐献货币类型.Value = Config.沙城捐献货币类型;
        沙城捐献支付数量.Value = Config.沙城捐献支付数量;
        沙城捐献获得物品1.Value = Config.沙城捐献获得物品1;
        沙城捐献获得物品2.Value = Config.沙城捐献获得物品2;
        沙城捐献获得物品3.Value = Config.沙城捐献获得物品3;
        沙城捐献物品数量1.Value = Config.沙城捐献物品数量1;
        沙城捐献物品数量2.Value = Config.沙城捐献物品数量2;
        沙城捐献物品数量3.Value = Config.沙城捐献物品数量3;
        沙城捐献赞助人数.Value = Config.沙城捐献赞助人数;
        沙城捐献赞助金额.Value = Config.沙城捐献赞助金额;
        雕爷激活灵符需求.Value = Config.雕爷激活灵符需求;
        雕爷1号位灵符.Value = Config.雕爷1号位灵符;
        雕爷1号位铭文石.Value = Config.雕爷1号位铭文石;
        雕爷2号位灵符.Value = Config.雕爷2号位灵符;
        雕爷2号位铭文石.Value = Config.雕爷2号位铭文石;
        雕爷3号位灵符.Value = Config.雕爷3号位灵符;
        雕爷3号位铭文石.Value = Config.雕爷3号位铭文石;
        S_称号范围拾取判断1.Value = Config.称号范围拾取判断;
        九层妖塔统计开关.Value = Config.九层妖塔统计开关;
        沙巴克每周攻沙时间.Value = Config.沙巴克每周攻沙时间;
        沙巴克皇宫传送等级.Value = Config.沙巴克皇宫传送等级;
        沙巴克皇宫传送物品.Value = Config.沙巴克皇宫传送物品;
        沙巴克皇宫传送数量.Value = Config.沙巴克皇宫传送数量;
        系统窗口发送.Value = Config.系统窗口发送;
        龙卫效果提示.Value = Config.龙卫效果提示;
        充值平台切换.Value = Config.充值平台切换;
        全服红包等级.Value = Config.全服红包等级;
        全服红包时间.Value = Config.全服红包时间;
        全服红包货币类型.Value = Config.GlobalBonusCurrencyType;
        全服红包货币数量.Value = Config.全服红包货币数量;
        龙卫蓝色词条概率.Value = Config.龙卫蓝色词条概率;
        龙卫紫色词条概率.Value = Config.龙卫紫色词条概率;
        龙卫橙色词条概率.Value = Config.龙卫橙色词条概率;
        自定义初始货币类型.Value = Config.自定义初始货币类型;
        自动回收设置.Checked = Config.自动回收设置;
        购买狂暴之力.Checked = Config.购买狂暴之力;
        会员满血设置.Checked = Config.会员满血设置;
        全屏拾取开关.Checked = Config.AutoPickUpAllVisible;
        打开随时仓库.Checked = Config.打开随时仓库;
        红包开关.Checked = Config.红包开关;
        龙卫焰焚烈火剑法.Value = Config.龙卫焰焚烈火剑法;
        会员物品对接.Value = Config.会员物品对接;
        变性等级.Value = Config.变性等级;
        变性货币类型.Value = Config.变性货币类型;
        变性货币值.Value = Config.变性货币值;
        变性物品ID.Value = Config.变性物品ID;
        变性物品数量.Value = Config.变性物品数量;
        称号叠加模块9.Value = Config.称号叠加模块9;
        称号叠加模块10.Value = Config.称号叠加模块10;
        称号叠加模块11.Value = Config.称号叠加模块11;
        称号叠加模块12.Value = Config.称号叠加模块12;
        称号叠加模块13.Value = Config.称号叠加模块13;
        称号叠加模块14.Value = Config.称号叠加模块14;
        称号叠加模块15.Value = Config.称号叠加模块15;
        称号叠加模块16.Value = Config.称号叠加模块16;
        幸运保底开关.Checked = Config.幸运保底开关;
        安全区收刀开关.Checked = Config.安全区收刀开关;
        屠魔殿等级限制.Value = Config.屠魔殿等级限制;
        职业等级.Value = Config.职业等级;
        职业货币类型.Value = Config.职业货币类型;
        职业货币值.Value = Config.职业货币值;
        职业物品ID.Value = Config.职业物品ID;
        职业物品数量.Value = Config.职业物品数量;
        武斗场杀人经验.Value = Config.武斗场杀人经验;
        武斗场杀人开关.Checked = Config.武斗场杀人开关;
        S_狂暴名称.Text = Config.狂暴名称;
        S_自定义物品内容一.Text = Config.自定义物品内容一;
        S_自定义物品内容二.Text = Config.自定义物品内容二;
        S_自定义物品内容三.Text = Config.自定义物品内容三;
        S_自定义物品内容四.Text = Config.自定义物品内容四;
        S_自定义物品内容五.Text = Config.自定义物品内容五;
        S_挂机权限选项.Text = Config.挂机权限选项;
        合成模块控件.Text = Config.合成模块控件;
        变性内容控件.Text = Config.变性内容控件;
        转职内容控件.Text = Config.转职内容控件;
        S_战将特权礼包.Text = Config.战将特权礼包;
        S_豪杰特权礼包.Text = Config.豪杰特权礼包;
        S_世界BOSS名字.Text = Config.WorldBossName;
        S_世界BOSS时间.Value = Config.WorldBossTimeHour;
        S_世界BOSS分钟.Value = Config.WorldBossTimeMinute;
        S_秘宝广场元宝.Value = Config.秘宝广场元宝;
        S_每周特惠礼包一元宝.Value = Config.每周特惠礼包一元宝;
        S_每周特惠礼包二元宝.Value = Config.每周特惠礼包二元宝;
        S_特权玛法名俊元宝.Value = Config.特权玛法名俊元宝;
        S_特权玛法豪杰元宝.Value = Config.特权玛法豪杰元宝;
        S_特权玛法战将元宝.Value = Config.特权玛法战将元宝;
        S_御兽切换开关.Value = Config.御兽切换开关;

        Task.Run(delegate
        {
            Thread.Sleep(100);
            BeginInvoke(() =>
            {
                SettingsPage.Enabled = false;
                下方控件页.Enabled = false;
            });
            LoadSystemData();
            LoadUserData();
            BeginInvoke(() =>
            {
                UIUpdateTimer.Tick += ProcessUpdateUI;
                角色浏览表.SelectionChanged += ProcessUpdateUI;
                怪物浏览表.SelectionChanged += ProcessUpdateUI;
                SettingsPage.Enabled = true;
                下方控件页.Enabled = true;
            });
        });
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
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
            BeginInvoke(() =>
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
            BeginInvoke(() =>
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
                var path = folderBrowserDialog.SelectedPath;
                S_GameDataPath.Text = Config.GameDataPath = path;
                Config.Save();
            }
            else if (sender == ButtonBrowseBackupDirectory)
            {
                var path = folderBrowserDialog.SelectedPath;
                S_DataBackupPath.Text = Config.DataBackupPath = path;
                Config.Save();
            }
            else if (sender == S_浏览平台目录)
            {
                var path = folderBrowserDialog.SelectedPath;
                S_平台接入目录.Text = Config.平台接入目录 = path;
                Config.Save();
            }
        }
    }

    private void UpdateNumericSettingsValue_ValueChanged(object sender, EventArgs e)
    {
        if (sender is not NumericUpDown control)
        {
            MessageBox.Show($"ValueChanged raised on invalid control {sender.GetType()}");
            return;
        }

        if (!control.Visible) return;

        if (control.Tag is decimal oldValue)
        {
            if (oldValue == control.Value)
                return;
            control.Tag = oldValue;
        }

        switch (control.Name)
        {
            case nameof(S_收益减少比率): Config.收益减少比率 = control.Value; break;
            case nameof(S_DisconnectTime): Config.DisconnectTime = (ushort)control.Value; break;
            case nameof(S_MaxUserLevel): Config.MaxUserLevel = (byte)control.Value; break;
            case nameof(S_怪物诱惑时长): Config.怪物诱惑时长 = (ushort)control.Value; break;
            case nameof(S_MonsterExperienceMultiplier): Config.MonsterExperienceMultiplier = control.Value; break;
            case nameof(S_TicketReceivePort): Config.TicketReceivePort = (ushort)control.Value; break;
            case nameof(S_AbnormalBlockTime): Config.AbnormalBlockTime = (ushort)control.Value; break;
            case nameof(S_减收益等级差): Config.减收益等级差 = (ushort)control.Value; break;
            case nameof(S_ItemDropRate): Config.ItemDropRate = control.Value; break;
            case nameof(S_物品归属时间): Config.物品归属时间 = (ushort)control.Value; break;
            case nameof(S_NoobSupportLevel): Config.NoobSupportLevel = (byte)control.Value; break;
            case nameof(S_SpecialRepairDiscount): Config.SpecialRepairDiscount = control.Value; break;
            case nameof(S_ItemDisappearTime): Config.ItemDisappearTime = (ushort)control.Value; break;
            case nameof(S_PacketLimit): Config.PacketLimit = (ushort)control.Value; break;
            case nameof(S_UserConnectionPort): Config.UserConnectionPort = (ushort)control.Value; break;
            case nameof(S_自动保存时间): Config.AutoSaveInterval = (ushort)control.Value; break;
            case nameof(S_自动保存日志): Config.自动保存日志 = (ushort)control.Value; break;
            case nameof(S_沃玛分解元宝): Config.沃玛分解元宝 = (int)control.Value; break;
            case nameof(S_祖玛分解元宝): Config.祖玛分解元宝 = (int)control.Value; break;
            case nameof(S_赤月分解元宝): Config.赤月分解元宝 = (int)control.Value; break;
            case nameof(S_魔龙分解元宝): Config.魔龙分解元宝 = (int)control.Value; break;
            case nameof(S_星王分解元宝): Config.星王分解元宝 = (int)control.Value; break;
            case nameof(S_苍月分解元宝): Config.苍月分解元宝 = (int)control.Value; break;
            case nameof(S_城主分解元宝): Config.城主分解元宝 = (int)control.Value; break;
            case nameof(S_神秘分解元宝): Config.城主分解元宝 = (int)control.Value; break;
            case nameof(S_屠魔组队人数): Config.屠魔组队人数 = (int)control.Value; break;
            case nameof(S_屠魔令回收经验): Config.屠魔令回收经验 = (int)control.Value; break;
            case nameof(S_屠魔爆率开关): Config.屠魔爆率开关 = (int)control.Value; break;
            case nameof(S_武斗场时间一): Config.武斗场时间一 = (byte)control.Value; break;
            case nameof(S_武斗场时间二): Config.武斗场时间二 = (byte)control.Value; break;
            case nameof(S_武斗场经验小): Config.武斗场经验小 = (int)control.Value; break;
            case nameof(S_武斗场经验大): Config.武斗场经验大 = (int)control.Value; break;
            case nameof(S_沙巴克开启): Config.沙巴克开启 = (byte)control.Value; break;
            case nameof(S_沙巴克结束): Config.沙巴克结束 = (byte)control.Value; break;
            case nameof(S_祝福油幸运1机率): Config.祝福油幸运1机率 = (int)control.Value; break;
            case nameof(S_祝福油幸运2机率): Config.祝福油幸运2机率 = (int)control.Value; break;
            case nameof(S_祝福油幸运3机率): Config.祝福油幸运3机率 = (int)control.Value; break;
            case nameof(S_祝福油幸运4机率): Config.祝福油幸运4机率 = (int)control.Value; break;
            case nameof(S_祝福油幸运5机率): Config.祝福油幸运5机率 = (int)control.Value; break;
            case nameof(S_祝福油幸运6机率): Config.祝福油幸运6机率 = (int)control.Value; break;
            case nameof(S_祝福油幸运7机率): Config.祝福油幸运7机率 = (int)control.Value; break;
            case nameof(S_PKYellowNamePoint): Config.PKYellowNamePoint = (int)control.Value; break;
            case nameof(S_PKRedNamePoint): Config.PKRedNamePoint = (int)control.Value; break;
            case nameof(S_PKCrimsonNamePoint): Config.PKCrimsonNamePoint = (int)control.Value; break;
            case nameof(S_锻造成功倍数): Config.锻造成功倍数 = (int)control.Value; break;
            case nameof(S_死亡掉落背包几率): Config.死亡掉落背包几率 = (float)control.Value; break;
            case nameof(S_死亡掉落身上几率): Config.死亡掉落身上几率 = (float)control.Value; break;
            case nameof(S_PK死亡幸运开关): Config.PK死亡幸运开关 = (int)control.Value; break;
            case nameof(S_屠魔副本次数): Config.屠魔副本次数 = (int)control.Value; break;
            case nameof(S_升级经验模块一): Config.升级经验模块一 = (int)control.Value; break;
            case nameof(S_升级经验模块二): Config.升级经验模块二 = (int)control.Value; break;
            case nameof(S_升级经验模块三): Config.升级经验模块三 = (int)control.Value; break;
            case nameof(S_升级经验模块四): Config.升级经验模块四 = (int)control.Value; break;
            case nameof(S_升级经验模块五): Config.升级经验模块五 = (int)control.Value; break;
            case nameof(S_升级经验模块六): Config.升级经验模块六 = (int)control.Value; break;
            case nameof(S_升级经验模块七): Config.升级经验模块七 = (int)control.Value; break;
            case nameof(S_升级经验模块八): Config.升级经验模块八 = (int)control.Value; break;
            case nameof(S_升级经验模块九): Config.升级经验模块九 = (int)control.Value; break;
            case nameof(S_升级经验模块十): Config.升级经验模块十 = (int)control.Value; break;
            case nameof(S_升级经验模块十一): Config.升级经验模块十一 = (int)control.Value; break;
            case nameof(S_升级经验模块十二): Config.升级经验模块十二 = (int)control.Value; break;
            case nameof(S_升级经验模块十三): Config.升级经验模块十三 = (int)control.Value; break;
            case nameof(S_升级经验模块十四): Config.升级经验模块十四 = (int)control.Value; break;
            case nameof(S_升级经验模块十五): Config.升级经验模块十五 = (int)control.Value; break;
            case nameof(S_升级经验模块十六): Config.升级经验模块十六 = (int)control.Value; break;
            case nameof(S_升级经验模块十七): Config.升级经验模块十七 = (int)control.Value; break;
            case nameof(S_升级经验模块十八): Config.升级经验模块十八 = (int)control.Value; break;
            case nameof(S_升级经验模块十九): Config.升级经验模块十九 = (int)control.Value; break;
            case nameof(S_升级经验模块二十): Config.升级经验模块二十 = (int)control.Value; break;
            case nameof(S_升级经验模块二十一): Config.升级经验模块二十一 = (int)control.Value; break;
            case nameof(S_升级经验模块二十二): Config.升级经验模块二十二 = (int)control.Value; break;
            case nameof(S_升级经验模块二十三): Config.升级经验模块二十三 = (int)control.Value; break;
            case nameof(S_升级经验模块二十四): Config.升级经验模块二十四 = (int)control.Value; break;
            case nameof(S_升级经验模块二十五): Config.升级经验模块二十五 = (int)control.Value; break;
            case nameof(S_升级经验模块二十六): Config.升级经验模块二十六 = (int)control.Value; break;
            case nameof(S_升级经验模块二十七): Config.升级经验模块二十七 = (int)control.Value; break;
            case nameof(S_升级经验模块二十八): Config.升级经验模块二十八 = (int)control.Value; break;
            case nameof(S_升级经验模块二十九): Config.升级经验模块二十九 = (int)control.Value; break;
            case nameof(S_升级经验模块三十): Config.升级经验模块三十 = (int)control.Value; break;
            case nameof(S_高级祝福油幸运机率): Config.高级祝福油幸运机率 = (int)control.Value; break;
            case nameof(S_雕爷使用物品): Config.雕爷使用物品 = (int)control.Value; break;
            case nameof(S_雕爷使用金币): Config.雕爷使用金币 = (int)control.Value; break;
            case nameof(S_称号范围拾取判断): Config.称号范围拾取判断 = (int)control.Value; break;
            case nameof(S_TitleRangePickUpDistance): Config.TitleRangePickUpDistance = (int)control.Value; break;
            case nameof(S_行会申请人数限制): Config.行会申请人数限制 = (int)control.Value; break;
            case nameof(S_疗伤药HP): Config.疗伤药HP = (int)control.Value; break;
            case nameof(S_疗伤药MP): Config.疗伤药MP = (int)control.Value; break;
            case nameof(S_万年雪霜HP): Config.万年雪霜HP = (int)control.Value; break;
            case nameof(S_万年雪霜MP): Config.万年雪霜MP = (int)control.Value; break;
            case nameof(S_元宝金币回收设定): Config.元宝金币回收设定 = (int)control.Value; break;
            case nameof(S_元宝金币传送设定): Config.元宝金币传送设定 = (int)control.Value; break;
            case nameof(S_快捷传送一编号): Config.快捷传送一编号 = (int)control.Value; break;
            case nameof(S_快捷传送一货币): Config.快捷传送一货币 = (int)control.Value; break;
            case nameof(S_快捷传送一等级): Config.快捷传送一等级 = (int)control.Value; break;
            case nameof(S_快捷传送二编号): Config.快捷传送二编号 = (int)control.Value; break;
            case nameof(S_快捷传送二货币): Config.快捷传送二货币 = (int)control.Value; break;
            case nameof(S_快捷传送二等级): Config.快捷传送二等级 = (int)control.Value; break;
            case nameof(S_快捷传送三编号): Config.快捷传送三编号 = (int)control.Value; break;
            case nameof(S_快捷传送三货币): Config.快捷传送三货币 = (int)control.Value; break;
            case nameof(S_快捷传送三等级): Config.快捷传送三等级 = (int)control.Value; break;
            case nameof(S_快捷传送四编号): Config.快捷传送四编号 = (int)control.Value; break;
            case nameof(S_快捷传送四货币): Config.快捷传送四货币 = (int)control.Value; break;
            case nameof(S_快捷传送四等级): Config.快捷传送四等级 = (int)control.Value; break;
            case nameof(S_快捷传送五编号): Config.快捷传送五编号 = (int)control.Value; break;
            case nameof(S_快捷传送五货币): Config.快捷传送五货币 = (int)control.Value; break;
            case nameof(S_快捷传送五等级): Config.快捷传送五等级 = (int)control.Value; break;
            case nameof(S_狂暴货币格式): Config.狂暴货币格式 = (int)control.Value; break;
            case nameof(S_狂暴称号格式): Config.狂暴称号格式 = (byte)control.Value; break;
            case nameof(S_狂暴开启物品名称): Config.狂暴开启物品名称 = (int)control.Value; break;
            case nameof(S_狂暴开启物品数量): Config.狂暴开启物品数量 = (int)control.Value; break;
            case nameof(S_狂暴杀死物品数量): Config.狂暴杀死物品数量 = (int)control.Value; break;
            case nameof(S_狂暴开启元宝数量): Config.狂暴开启元宝数量 = (int)control.Value; break;
            case nameof(S_狂暴杀死元宝数量): Config.狂暴杀死元宝数量 = (int)control.Value; break;
            case nameof(S_狂暴开启金币数量): Config.狂暴开启金币数量 = (int)control.Value; break;
            case nameof(S_狂暴杀死金币数量): Config.狂暴杀死金币数量 = (int)control.Value; break;
            case nameof(S_装备技能开关): Config.装备技能开关 = (int)control.Value; break;
            case nameof(S_御兽属性开启): Config.御兽属性开启 = (int)control.Value; break;
            case nameof(S_可摆摊地图编号): Config.可摆摊地图编号 = (int)control.Value; break;
            case nameof(S_可摆摊地图坐标X): Config.可摆摊地图坐标X = (int)control.Value; break;
            case nameof(S_可摆摊地图坐标Y): Config.可摆摊地图坐标Y = (int)control.Value; break;
            case nameof(S_可摆摊地图范围): Config.可摆摊地图范围 = (int)control.Value; break;
            case nameof(S_可摆摊货币选择): Config.可摆摊货币选择 = (int)control.Value; break;
            case nameof(S_可摆摊等级): Config.可摆摊等级 = (int)control.Value; break;
            case nameof(S_ReviveInterval): Config.ReviveInterval = (int)control.Value; break;
            case nameof(S_自定义麻痹几率): Config.自定义麻痹几率 = (float)control.Value; break;
            case nameof(S_PetUpgradeXPLevel1): Config.PetUpgradeXPLevel1 = (ushort)control.Value; break;
            case nameof(S_PetUpgradeXPLevel2): Config.PetUpgradeXPLevel2 = (ushort)control.Value; break;
            case nameof(S_PetUpgradeXPLevel3): Config.PetUpgradeXPLevel3 = (ushort)control.Value; break;
            case nameof(S_PetUpgradeXPLevel4): Config.PetUpgradeXPLevel4 = (ushort)control.Value; break;
            case nameof(S_PetUpgradeXPLevel5): Config.PetUpgradeXPLevel5 = (ushort)control.Value; break;
            case nameof(S_PetUpgradeXPLevel6): Config.PetUpgradeXPLevel6 = (ushort)control.Value; break;
            case nameof(S_PetUpgradeXPLevel7): Config.PetUpgradeXPLevel7 = (ushort)control.Value; break;
            case nameof(S_PetUpgradeXPLevel8): Config.PetUpgradeXPLevel8 = (ushort)control.Value; break;
            case nameof(S_PetUpgradeXPLevel9): Config.PetUpgradeXPLevel9 = (ushort)control.Value; break;
            case nameof(S_下马击落机率): Config.下马击落机率 = (int)control.Value; break;
            case nameof(S_AllowRaceWarrior): Config.AllowRaceWarrior = (int)control.Value; break;
            case nameof(S_AllowRaceWizard): Config.AllowRaceWizard = (int)control.Value; break;
            case nameof(S_AllowRaceTaoist): Config.AllowRaceTaoist = (int)control.Value; break;
            case nameof(S_AllowRaceArcher): Config.AllowRaceArcher = (int)control.Value; break;
            case nameof(S_AllowRaceAssassin): Config.AllowRaceAssassin = (int)control.Value; break;
            case nameof(S_AllowRaceDragonLance): Config.AllowRaceDragonLance = (int)control.Value; break;
            case nameof(S_泡点等级开关): Config.泡点等级开关 = (int)control.Value; break;
            case nameof(S_泡点当前经验): Config.泡点当前经验 = (int)control.Value; break;
            case nameof(S_泡点限制等级): Config.泡点限制等级 = (int)control.Value; break;
            case nameof(S_杀人PK红名开关): Config.杀人PK红名开关 = (int)control.Value; break;
            case nameof(S_泡点秒数控制): Config.泡点秒数控制 = (int)control.Value; break;
            case nameof(S_自定义物品数量一): Config.自定义物品数量一 = (int)control.Value; break;
            case nameof(S_自定义物品数量二): Config.自定义物品数量二 = (int)control.Value; break;
            case nameof(S_自定义物品数量三): Config.自定义物品数量三 = (int)control.Value; break;
            case nameof(S_自定义物品数量四): Config.自定义物品数量四 = (int)control.Value; break;
            case nameof(S_自定义物品数量五): Config.自定义物品数量五 = (int)control.Value; break;
            case nameof(S_自定义称号内容一): Config.自定义称号内容一 = (byte)control.Value; break;
            case nameof(S_自定义称号内容二): Config.自定义称号内容二 = (byte)control.Value; break;
            case nameof(S_自定义称号内容三): Config.自定义称号内容三 = (byte)control.Value; break;
            case nameof(S_自定义称号内容四): Config.自定义称号内容四 = (byte)control.Value; break;
            case nameof(S_自定义称号内容五): Config.自定义称号内容五 = (byte)control.Value; break;
            case nameof(S_元宝金币传送设定2): Config.元宝金币传送设定2 = (int)control.Value; break;
            case nameof(S_快捷传送一编号2): Config.快捷传送一编号2 = (int)control.Value; break;
            case nameof(S_快捷传送一货币2): Config.快捷传送一货币2 = (int)control.Value; break;
            case nameof(S_快捷传送一等级2): Config.快捷传送一等级2 = (int)control.Value; break;
            case nameof(S_快捷传送二编号2): Config.快捷传送二编号2 = (int)control.Value; break;
            case nameof(S_快捷传送二货币2): Config.快捷传送二货币2 = (int)control.Value; break;
            case nameof(S_快捷传送二等级2): Config.快捷传送二等级2 = (int)control.Value; break;
            case nameof(S_快捷传送三编号2): Config.快捷传送三编号2 = (int)control.Value; break;
            case nameof(S_快捷传送三货币2): Config.快捷传送三货币2 = (int)control.Value; break;
            case nameof(S_快捷传送三等级2): Config.快捷传送三等级2 = (int)control.Value; break;
            case nameof(S_快捷传送四编号2): Config.快捷传送四编号2 = (int)control.Value; break;
            case nameof(S_快捷传送四货币2): Config.快捷传送四货币2 = (int)control.Value; break;
            case nameof(S_快捷传送四等级2): Config.快捷传送四等级2 = (int)control.Value; break;
            case nameof(S_快捷传送五编号2): Config.快捷传送五编号2 = (int)control.Value; break;
            case nameof(S_快捷传送五货币2): Config.快捷传送五货币2 = (int)control.Value; break;
            case nameof(S_快捷传送五等级2): Config.快捷传送五等级2 = (int)control.Value; break;
            case nameof(S_快捷传送六编号2): Config.快捷传送六编号2 = (int)control.Value; break;
            case nameof(S_快捷传送六货币2): Config.快捷传送六货币2 = (int)control.Value; break;
            case nameof(S_快捷传送六等级2): Config.快捷传送六等级2 = (int)control.Value; break;
            case nameof(S_武斗场次数限制): Config.武斗场次数限制 = (int)control.Value; break;
            case nameof(S_AutoPickUpInventorySpace): Config.AutoPickUpInventorySpace = (int)control.Value; break;
            case nameof(S_BOSS刷新提示开关): Config.BOSS刷新提示开关 = (int)control.Value; break;
            case nameof(S_自动整理背包计时): Config.自动整理背包计时 = (int)control.Value; break;
            case nameof(S_自动整理背包开关): Config.自动整理背包开关 = (int)control.Value; break;
            case nameof(S_称号叠加开关): Config.称号叠加开关 = (int)control.Value; break;
            case nameof(S_称号叠加模块一): Config.称号叠加模块一 = (byte)control.Value; break;
            case nameof(S_称号叠加模块二): Config.称号叠加模块二 = (byte)control.Value; break;
            case nameof(S_称号叠加模块三): Config.称号叠加模块三 = (byte)control.Value; break;
            case nameof(S_称号叠加模块四): Config.称号叠加模块四 = (byte)control.Value; break;
            case nameof(S_称号叠加模块五): Config.称号叠加模块五 = (byte)control.Value; break;
            case nameof(S_称号叠加模块六): Config.称号叠加模块六 = (byte)control.Value; break;
            case nameof(S_称号叠加模块七): Config.称号叠加模块七 = (byte)control.Value; break;
            case nameof(S_称号叠加模块八): Config.称号叠加模块八 = (byte)control.Value; break;
            case nameof(S_沙城传送货币开关): Config.沙城传送货币开关 = (int)control.Value; break;
            case nameof(S_沙城快捷货币一): Config.沙城快捷货币一 = (int)control.Value; break;
            case nameof(S_沙城快捷货币二): Config.沙城快捷货币二 = (int)control.Value; break;
            case nameof(S_沙城快捷货币三): Config.沙城快捷货币三 = (int)control.Value; break;
            case nameof(S_沙城快捷货币四): Config.沙城快捷货币四 = (int)control.Value; break;
            case nameof(S_沙城快捷等级一): Config.沙城快捷等级一 = (int)control.Value; break;
            case nameof(S_沙城快捷等级二): Config.沙城快捷等级二 = (int)control.Value; break;
            case nameof(S_沙城快捷等级三): Config.沙城快捷等级三 = (int)control.Value; break;
            case nameof(S_沙城快捷等级四): Config.沙城快捷等级四 = (int)control.Value; break;
            case nameof(S_未知暗点副本价格): Config.未知暗点副本价格 = (int)control.Value; break;
            case nameof(S_未知暗点副本等级): Config.未知暗点副本等级 = (int)control.Value; break;
            case nameof(S_未知暗点二层价格): Config.未知暗点二层价格 = (int)control.Value; break;
            case nameof(S_未知暗点二层等级): Config.未知暗点二层等级 = (int)control.Value; break;
            case nameof(S_幽冥海副本价格): Config.幽冥海副本价格 = (int)control.Value; break;
            case nameof(S_幽冥海副本等级): Config.幽冥海副本等级 = (int)control.Value; break;
            case nameof(S_猎魔暗使称号六): Config.猎魔暗使称号六 = (byte)control.Value; break;
            case nameof(S_猎魔暗使材料六): Config.猎魔暗使材料六 = (int)control.Value; break;
            case nameof(S_猎魔暗使数量六): Config.猎魔暗使数量六 = (int)control.Value; break;
            case nameof(S_猎魔暗使称号五): Config.猎魔暗使称号五 = (byte)control.Value; break;
            case nameof(S_猎魔暗使材料五): Config.猎魔暗使材料五 = (int)control.Value; break;
            case nameof(S_猎魔暗使数量五): Config.猎魔暗使数量五 = (int)control.Value; break;
            case nameof(S_猎魔暗使称号四): Config.猎魔暗使称号四 = (byte)control.Value; break;
            case nameof(S_猎魔暗使材料四): Config.猎魔暗使材料四 = (int)control.Value; break;
            case nameof(S_猎魔暗使数量四): Config.猎魔暗使数量四 = (int)control.Value; break;
            case nameof(S_猎魔暗使称号三): Config.猎魔暗使称号三 = (byte)control.Value; break;
            case nameof(S_猎魔暗使材料三): Config.猎魔暗使材料三 = (int)control.Value; break;
            case nameof(S_猎魔暗使数量三): Config.猎魔暗使数量三 = (int)control.Value; break;
            case nameof(S_猎魔暗使称号二): Config.猎魔暗使称号二 = (byte)control.Value; break;
            case nameof(S_猎魔暗使材料二): Config.猎魔暗使材料二 = (int)control.Value; break;
            case nameof(S_猎魔暗使数量二): Config.猎魔暗使数量二 = (int)control.Value; break;
            case nameof(S_猎魔暗使称号一): Config.猎魔暗使称号一 = (byte)control.Value; break;
            case nameof(S_猎魔暗使材料一): Config.猎魔暗使材料一 = (int)control.Value; break;
            case nameof(S_猎魔暗使数量一): Config.猎魔暗使数量一 = (int)control.Value; break;
            case nameof(S_怪物掉落广播开关): Config.怪物掉落广播开关 = (int)control.Value; break;
            case nameof(S_怪物掉落窗口开关): Config.怪物掉落窗口开关 = (int)control.Value; break;
            case nameof(S_珍宝阁提示开关): Config.珍宝阁提示开关 = (int)control.Value; break;
            case nameof(S_祖玛分解几率一): Config.祖玛分解几率一 = (int)control.Value; break;
            case nameof(S_祖玛分解几率二): Config.祖玛分解几率二 = (int)control.Value; break;
            case nameof(S_祖玛分解几率三): Config.祖玛分解几率三 = (int)control.Value; break;
            case nameof(S_祖玛分解几率四): Config.祖玛分解几率四 = (int)control.Value; break;
            case nameof(S_祖玛分解数量一): Config.祖玛分解数量一 = (int)control.Value; break;
            case nameof(S_祖玛分解数量二): Config.祖玛分解数量二 = (int)control.Value; break;
            case nameof(S_祖玛分解数量三): Config.祖玛分解数量三 = (int)control.Value; break;
            case nameof(S_祖玛分解数量四): Config.祖玛分解数量四 = (int)control.Value; break;
            case nameof(S_祖玛分解开关): Config.祖玛分解开关 = (int)control.Value; break;
            case nameof(S_赤月分解几率一): Config.赤月分解几率一 = (int)control.Value; break;
            case nameof(S_赤月分解几率二): Config.赤月分解几率二 = (int)control.Value; break;
            case nameof(S_赤月分解几率三): Config.赤月分解几率三 = (int)control.Value; break;
            case nameof(S_赤月分解几率四): Config.赤月分解几率四 = (int)control.Value; break;
            case nameof(S_赤月分解数量一): Config.赤月分解数量一 = (int)control.Value; break;
            case nameof(S_赤月分解数量二): Config.赤月分解数量二 = (int)control.Value; break;
            case nameof(S_赤月分解数量三): Config.赤月分解数量三 = (int)control.Value; break;
            case nameof(S_赤月分解数量四): Config.赤月分解数量四 = (int)control.Value; break;
            case nameof(S_赤月分解开关): Config.赤月分解开关 = (int)control.Value; break;
            case nameof(S_魔龙分解几率一): Config.魔龙分解几率一 = (int)control.Value; break;
            case nameof(S_魔龙分解几率二): Config.魔龙分解几率二 = (int)control.Value; break;
            case nameof(S_魔龙分解几率三): Config.魔龙分解几率三 = (int)control.Value; break;
            case nameof(S_魔龙分解几率四): Config.魔龙分解几率四 = (int)control.Value; break;
            case nameof(S_魔龙分解数量一): Config.魔龙分解数量一 = (int)control.Value; break;
            case nameof(S_魔龙分解数量二): Config.魔龙分解数量二 = (int)control.Value; break;
            case nameof(S_魔龙分解数量三): Config.魔龙分解数量三 = (int)control.Value; break;
            case nameof(S_魔龙分解数量四): Config.魔龙分解数量四 = (int)control.Value; break;
            case nameof(S_魔龙分解开关): Config.魔龙分解开关 = (int)control.Value; break;
            case nameof(S_苍月分解几率一): Config.苍月分解几率一 = (int)control.Value; break;
            case nameof(S_苍月分解几率二): Config.苍月分解几率二 = (int)control.Value; break;
            case nameof(S_苍月分解几率三): Config.苍月分解几率三 = (int)control.Value; break;
            case nameof(S_苍月分解几率四): Config.苍月分解几率四 = (int)control.Value; break;
            case nameof(S_苍月分解数量一): Config.苍月分解数量一 = (int)control.Value; break;
            case nameof(S_苍月分解数量二): Config.苍月分解数量二 = (int)control.Value; break;
            case nameof(S_苍月分解数量三): Config.苍月分解数量三 = (int)control.Value; break;
            case nameof(S_苍月分解数量四): Config.苍月分解数量四 = (int)control.Value; break;
            case nameof(S_苍月分解开关): Config.苍月分解开关 = (int)control.Value; break;
            case nameof(S_星王分解几率一): Config.星王分解几率一 = (int)control.Value; break;
            case nameof(S_星王分解几率二): Config.星王分解几率二 = (int)control.Value; break;
            case nameof(S_星王分解几率三): Config.星王分解几率三 = (int)control.Value; break;
            case nameof(S_星王分解几率四): Config.星王分解几率四 = (int)control.Value; break;
            case nameof(S_星王分解数量一): Config.星王分解数量一 = (int)control.Value; break;
            case nameof(S_星王分解数量二): Config.星王分解数量二 = (int)control.Value; break;
            case nameof(S_星王分解数量三): Config.星王分解数量三 = (int)control.Value; break;
            case nameof(S_星王分解数量四): Config.星王分解数量四 = (int)control.Value; break;
            case nameof(S_星王分解开关): Config.星王分解开关 = (int)control.Value; break;
            case nameof(S_城主分解几率一): Config.城主分解几率一 = (int)control.Value; break;
            case nameof(S_城主分解几率二): Config.城主分解几率二 = (int)control.Value; break;
            case nameof(S_城主分解几率三): Config.城主分解几率三 = (int)control.Value; break;
            case nameof(S_城主分解几率四): Config.城主分解几率四 = (int)control.Value; break;
            case nameof(S_城主分解数量一): Config.城主分解数量一 = (int)control.Value; break;
            case nameof(S_城主分解数量二): Config.城主分解数量二 = (int)control.Value; break;
            case nameof(S_城主分解数量三): Config.城主分解数量三 = (int)control.Value; break;
            case nameof(S_城主分解数量四): Config.城主分解数量四 = (int)control.Value; break;
            case nameof(S_城主分解开关): Config.城主分解开关 = (int)control.Value; break;
            case nameof(S_世界BOSS时间): Config.WorldBossTimeHour = (byte)control.Value; break;
            case nameof(S_世界BOSS分钟): Config.WorldBossTimeMinute = (byte)control.Value; break;
            case nameof(S_秘宝广场元宝): Config.秘宝广场元宝 = (int)control.Value; break;
            case nameof(S_每周特惠礼包一元宝): Config.每周特惠礼包一元宝 = (int)control.Value; break;
            case nameof(S_每周特惠礼包二元宝): Config.每周特惠礼包二元宝 = (int)control.Value; break;
            case nameof(S_特权玛法名俊元宝): Config.特权玛法名俊元宝 = (int)control.Value; break;
            case nameof(S_特权玛法豪杰元宝): Config.特权玛法名俊元宝 = (int)control.Value; break;
            case nameof(S_特权玛法战将元宝): Config.特权玛法战将元宝 = (int)control.Value; break;
            case nameof(S_御兽切换开关): Config.御兽切换开关 = (int)control.Value; break;
            case nameof(S_BOSS卷轴地图编号): Config.BOSS卷轴地图编号 = (int)control.Value; break;
            case nameof(S_BOSS卷轴地图开关): Config.BOSS卷轴地图开关 = (int)control.Value; break;
            case nameof(S_沙巴克重置系统): Config.沙巴克重置系统 = (int)control.Value; break;
            case nameof(S_资源包开关): Config.资源包开关 = (int)control.Value; break;
            case nameof(S_StartingLevel): Config.StartingLevel = (byte)control.Value; break;
            case nameof(S_MaxUserConnections): Config.MaxUserConnections = (int)control.Value; break;
            case nameof(S_掉落贵重物品颜色): Config.掉落贵重物品颜色 = (int)control.Value; break;
            case nameof(S_掉落沃玛物品颜色): Config.掉落沃玛物品颜色 = (int)control.Value; break;
            case nameof(S_掉落祖玛物品颜色): Config.掉落祖玛物品颜色 = (int)control.Value; break;
            case nameof(S_掉落赤月物品颜色): Config.掉落赤月物品颜色 = (int)control.Value; break;
            case nameof(S_掉落魔龙物品颜色): Config.掉落魔龙物品颜色 = (int)control.Value; break;
            case nameof(S_掉落苍月物品颜色): Config.掉落苍月物品颜色 = (int)control.Value; break;
            case nameof(S_掉落星王物品颜色): Config.掉落星王物品颜色 = (int)control.Value; break;
            case nameof(S_掉落城主物品颜色): Config.掉落城主物品颜色 = (int)control.Value; break;
            case nameof(S_掉落书籍物品颜色): Config.掉落书籍物品颜色 = (int)control.Value; break;
            case nameof(S_DropPlayerNameColor): Config.DropPlayerNameColor = (int)control.Value; break;
            case nameof(S_狂暴击杀玩家颜色): Config.狂暴击杀玩家颜色 = (int)control.Value; break;
            case nameof(S_狂暴被杀玩家颜色): Config.狂暴被杀玩家颜色 = (int)control.Value; break;
            case nameof(S_祖玛战装备佩戴数量): Config.祖玛战装备佩戴数量 = (int)control.Value; break;
            case nameof(S_祖玛法装备佩戴数量): Config.祖玛法装备佩戴数量 = (int)control.Value; break;
            case nameof(S_祖玛道装备佩戴数量): Config.祖玛道装备佩戴数量 = (int)control.Value; break;
            case nameof(S_祖玛刺装备佩戴数量): Config.祖玛刺装备佩戴数量 = (int)control.Value; break;
            case nameof(S_祖玛弓装备佩戴数量): Config.祖玛弓装备佩戴数量 = (int)control.Value; break;
            case nameof(S_祖玛枪装备佩戴数量): Config.祖玛枪装备佩戴数量 = (int)control.Value; break;
            case nameof(S_赤月战装备佩戴数量): Config.赤月战装备佩戴数量 = (int)control.Value; break;
            case nameof(S_赤月法装备佩戴数量): Config.赤月法装备佩戴数量 = (int)control.Value; break;
            case nameof(S_赤月道装备佩戴数量): Config.赤月道装备佩戴数量 = (int)control.Value; break;
            case nameof(S_赤月刺装备佩戴数量): Config.赤月刺装备佩戴数量 = (int)control.Value; break;
            case nameof(S_赤月弓装备佩戴数量): Config.赤月弓装备佩戴数量 = (int)control.Value; break;
            case nameof(S_赤月枪装备佩戴数量): Config.赤月枪装备佩戴数量 = (int)control.Value; break;
            case nameof(S_魔龙战装备佩戴数量): Config.魔龙战装备佩戴数量 = (int)control.Value; break;
            case nameof(S_魔龙法装备佩戴数量): Config.魔龙法装备佩戴数量 = (int)control.Value; break;
            case nameof(S_魔龙道装备佩戴数量): Config.魔龙道装备佩戴数量 = (int)control.Value; break;
            case nameof(S_魔龙刺装备佩戴数量): Config.魔龙刺装备佩戴数量 = (int)control.Value; break;
            case nameof(S_魔龙弓装备佩戴数量): Config.魔龙弓装备佩戴数量 = (int)control.Value; break;
            case nameof(S_魔龙枪装备佩戴数量): Config.魔龙枪装备佩戴数量 = (int)control.Value; break;
            case nameof(S_苍月战装备佩戴数量): Config.苍月战装备佩戴数量 = (int)control.Value; break;
            case nameof(S_苍月法装备佩戴数量): Config.苍月法装备佩戴数量 = (int)control.Value; break;
            case nameof(S_苍月道装备佩戴数量): Config.苍月道装备佩戴数量 = (int)control.Value; break;
            case nameof(S_苍月刺装备佩戴数量): Config.苍月刺装备佩戴数量 = (int)control.Value; break;
            case nameof(S_苍月弓装备佩戴数量): Config.苍月弓装备佩戴数量 = (int)control.Value; break;
            case nameof(S_苍月枪装备佩戴数量): Config.苍月枪装备佩戴数量 = (int)control.Value; break;
            case nameof(S_星王战装备佩戴数量): Config.星王战装备佩戴数量 = (int)control.Value; break;
            case nameof(S_星王法装备佩戴数量): Config.星王法装备佩戴数量 = (int)control.Value; break;
            case nameof(S_星王道装备佩戴数量): Config.星王道装备佩戴数量 = (int)control.Value; break;
            case nameof(S_星王刺装备佩戴数量): Config.星王刺装备佩戴数量 = (int)control.Value; break;
            case nameof(S_星王弓装备佩戴数量): Config.星王弓装备佩戴数量 = (int)control.Value; break;
            case nameof(S_星王枪装备佩戴数量): Config.星王枪装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊1战装备佩戴数量): Config.特殊1战装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊1法装备佩戴数量): Config.特殊1法装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊1道装备佩戴数量): Config.特殊1道装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊1刺装备佩戴数量): Config.特殊1刺装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊1弓装备佩戴数量): Config.特殊1弓装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊1枪装备佩戴数量): Config.特殊1枪装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊2战装备佩戴数量): Config.特殊2战装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊2法装备佩戴数量): Config.特殊2法装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊2道装备佩戴数量): Config.特殊2道装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊2刺装备佩戴数量): Config.特殊2刺装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊2弓装备佩戴数量): Config.特殊2弓装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊2枪装备佩戴数量): Config.特殊2枪装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊3战装备佩戴数量): Config.特殊3战装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊3法装备佩戴数量): Config.特殊3法装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊3道装备佩戴数量): Config.特殊3道装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊3刺装备佩戴数量): Config.特殊3刺装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊3弓装备佩戴数量): Config.特殊3弓装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊3枪装备佩戴数量): Config.特殊3枪装备佩戴数量 = (int)control.Value; break;
            case nameof(S_每周特惠一物品1): Config.每周特惠一物品1 = (int)control.Value; break;
            case nameof(S_每周特惠一物品2): Config.每周特惠一物品2 = (int)control.Value; break;
            case nameof(S_每周特惠一物品3): Config.每周特惠一物品3 = (int)control.Value; break;
            case nameof(S_每周特惠一物品4): Config.每周特惠一物品4 = (int)control.Value; break;
            case nameof(S_每周特惠一物品5): Config.每周特惠一物品5 = (int)control.Value; break;
            case nameof(S_每周特惠二物品1): Config.每周特惠二物品1 = (int)control.Value; break;
            case nameof(S_每周特惠二物品2): Config.每周特惠二物品2 = (int)control.Value; break;
            case nameof(S_每周特惠二物品3): Config.每周特惠二物品3 = (int)control.Value; break;
            case nameof(S_每周特惠二物品4): Config.每周特惠二物品4 = (int)control.Value; break;
            case nameof(S_每周特惠二物品5): Config.每周特惠二物品5 = (int)control.Value; break;
            case nameof(S_新手出售货币值): Config.新手出售货币值 = (int)control.Value; break;
            case nameof(S_挂机称号选项): Config.挂机称号选项 = (byte)control.Value; break;
            case nameof(S_分解称号选项): Config.分解称号选项 = (byte)control.Value; break;
            case nameof(S_法阵卡BUG清理): Config.法阵卡BUG清理 = (int)control.Value; break;
            case nameof(S_随机宝箱三物品1): Config.随机宝箱三物品1 = (int)control.Value; break;
            case nameof(S_随机宝箱三几率1): Config.随机宝箱三几率1 = (int)control.Value; break;
            case nameof(S_随机宝箱三物品2): Config.随机宝箱三物品2 = (int)control.Value; break;
            case nameof(S_随机宝箱三几率2): Config.随机宝箱三几率2 = (int)control.Value; break;
            case nameof(S_随机宝箱三物品3): Config.随机宝箱三物品3 = (int)control.Value; break;
            case nameof(S_随机宝箱三几率3): Config.随机宝箱三几率3 = (int)control.Value; break;
            case nameof(S_随机宝箱三物品4): Config.随机宝箱三物品4 = (int)control.Value; break;
            case nameof(S_随机宝箱三几率4): Config.随机宝箱三几率4 = (int)control.Value; break;
            case nameof(S_随机宝箱三物品5): Config.随机宝箱三物品5 = (int)control.Value; break;
            case nameof(S_随机宝箱三几率5): Config.随机宝箱三几率5 = (int)control.Value; break;
            case nameof(S_随机宝箱三物品6): Config.随机宝箱三物品6 = (int)control.Value; break;
            case nameof(S_随机宝箱三几率6): Config.随机宝箱三几率6 = (int)control.Value; break;
            case nameof(S_随机宝箱三物品7): Config.随机宝箱三物品7 = (int)control.Value; break;
            case nameof(S_随机宝箱三几率7): Config.随机宝箱三几率7 = (int)control.Value; break;
            case nameof(S_随机宝箱三物品8): Config.随机宝箱三物品8 = (int)control.Value; break;
            case nameof(S_随机宝箱三几率8): Config.随机宝箱三几率8 = (int)control.Value; break;
            case nameof(S_随机宝箱二物品1): Config.随机宝箱二物品1 = (int)control.Value; break;
            case nameof(S_随机宝箱二几率1): Config.随机宝箱二几率1 = (int)control.Value; break;
            case nameof(S_随机宝箱二物品2): Config.随机宝箱二物品2 = (int)control.Value; break;
            case nameof(S_随机宝箱二几率2): Config.随机宝箱二几率2 = (int)control.Value; break;
            case nameof(S_随机宝箱二物品3): Config.随机宝箱二物品3 = (int)control.Value; break;
            case nameof(S_随机宝箱二几率3): Config.随机宝箱二几率3 = (int)control.Value; break;
            case nameof(S_随机宝箱二物品4): Config.随机宝箱二物品4 = (int)control.Value; break;
            case nameof(S_随机宝箱二几率4): Config.随机宝箱二几率4 = (int)control.Value; break;
            case nameof(S_随机宝箱二物品5): Config.随机宝箱二物品5 = (int)control.Value; break;
            case nameof(S_随机宝箱二几率5): Config.随机宝箱二几率5 = (int)control.Value; break;
            case nameof(S_随机宝箱二物品6): Config.随机宝箱二物品6 = (int)control.Value; break;
            case nameof(S_随机宝箱二几率6): Config.随机宝箱二几率6 = (int)control.Value; break;
            case nameof(S_随机宝箱二物品7): Config.随机宝箱二物品7 = (int)control.Value; break;
            case nameof(S_随机宝箱二几率7): Config.随机宝箱二几率7 = (int)control.Value; break;
            case nameof(S_随机宝箱二物品8): Config.随机宝箱二物品8 = (int)control.Value; break;
            case nameof(S_随机宝箱二几率8): Config.随机宝箱二几率8 = (int)control.Value; break;
            case nameof(S_随机宝箱一物品1): Config.随机宝箱一物品1 = (int)control.Value; break;
            case nameof(S_随机宝箱一几率1): Config.随机宝箱一几率1 = (int)control.Value; break;
            case nameof(S_随机宝箱一物品2): Config.随机宝箱一物品2 = (int)control.Value; break;
            case nameof(S_随机宝箱一几率2): Config.随机宝箱一几率2 = (int)control.Value; break;
            case nameof(S_随机宝箱一物品3): Config.随机宝箱一物品3 = (int)control.Value; break;
            case nameof(S_随机宝箱一几率3): Config.随机宝箱一几率3 = (int)control.Value; break;
            case nameof(S_随机宝箱一物品4): Config.随机宝箱一物品4 = (int)control.Value; break;
            case nameof(S_随机宝箱一几率4): Config.随机宝箱一几率4 = (int)control.Value; break;
            case nameof(S_随机宝箱一物品5): Config.随机宝箱一物品5 = (int)control.Value; break;
            case nameof(S_随机宝箱一几率5): Config.随机宝箱一几率5 = (int)control.Value; break;
            case nameof(S_随机宝箱一物品6): Config.随机宝箱一物品6 = (int)control.Value; break;
            case nameof(S_随机宝箱一几率6): Config.随机宝箱一几率6 = (int)control.Value; break;
            case nameof(S_随机宝箱一物品7): Config.随机宝箱一物品7 = (int)control.Value; break;
            case nameof(S_随机宝箱一几率7): Config.随机宝箱一几率7 = (int)control.Value; break;
            case nameof(S_随机宝箱一物品8): Config.随机宝箱一物品8 = (int)control.Value; break;
            case nameof(S_随机宝箱一几率8): Config.随机宝箱一几率8 = (int)control.Value; break;
            case nameof(S_随机宝箱一数量1): Config.随机宝箱一数量1 = (int)control.Value; break;
            case nameof(S_随机宝箱一数量2): Config.随机宝箱一数量2 = (int)control.Value; break;
            case nameof(S_随机宝箱一数量3): Config.随机宝箱一数量3 = (int)control.Value; break;
            case nameof(S_随机宝箱一数量4): Config.随机宝箱一数量4 = (int)control.Value; break;
            case nameof(S_随机宝箱一数量5): Config.随机宝箱一数量5 = (int)control.Value; break;
            case nameof(S_随机宝箱一数量6): Config.随机宝箱一数量6 = (int)control.Value; break;
            case nameof(S_随机宝箱一数量7): Config.随机宝箱一数量7 = (int)control.Value; break;
            case nameof(S_随机宝箱一数量8): Config.随机宝箱一数量8 = (int)control.Value; break;
            case nameof(S_随机宝箱二数量1): Config.随机宝箱二数量1 = (int)control.Value; break;
            case nameof(S_随机宝箱二数量2): Config.随机宝箱二数量2 = (int)control.Value; break;
            case nameof(S_随机宝箱二数量3): Config.随机宝箱二数量3 = (int)control.Value; break;
            case nameof(S_随机宝箱二数量4): Config.随机宝箱二数量4 = (int)control.Value; break;
            case nameof(S_随机宝箱二数量5): Config.随机宝箱二数量5 = (int)control.Value; break;
            case nameof(S_随机宝箱二数量6): Config.随机宝箱二数量6 = (int)control.Value; break;
            case nameof(S_随机宝箱二数量7): Config.随机宝箱二数量7 = (int)control.Value; break;
            case nameof(S_随机宝箱二数量8): Config.随机宝箱二数量8 = (int)control.Value; break;
            case nameof(S_随机宝箱三数量1): Config.随机宝箱三数量1 = (int)control.Value; break;
            case nameof(S_随机宝箱三数量2): Config.随机宝箱三数量2 = (int)control.Value; break;
            case nameof(S_随机宝箱三数量3): Config.随机宝箱三数量3 = (int)control.Value; break;
            case nameof(S_随机宝箱三数量4): Config.随机宝箱三数量4 = (int)control.Value; break;
            case nameof(S_随机宝箱三数量5): Config.随机宝箱三数量5 = (int)control.Value; break;
            case nameof(S_随机宝箱三数量6): Config.随机宝箱三数量6 = (int)control.Value; break;
            case nameof(S_随机宝箱三数量7): Config.随机宝箱三数量7 = (int)control.Value; break;
            case nameof(S_随机宝箱三数量8): Config.随机宝箱三数量8 = (int)control.Value; break;
            case nameof(S_沙城地图保护): Config.沙城地图保护 = (int)control.Value; break;
            case nameof(S_NoobProtectionLevel): Config.NoobProtectionLevel = (int)control.Value; break;
            case nameof(S_新手地图保护1): Config.新手地图保护1 = (int)control.Value; break;
            case nameof(S_新手地图保护2): Config.新手地图保护2 = (int)control.Value; break;
            case nameof(S_新手地图保护3): Config.新手地图保护3 = (int)control.Value; break;
            case nameof(S_新手地图保护4): Config.新手地图保护4 = (int)control.Value; break;
            case nameof(S_新手地图保护5): Config.新手地图保护5 = (int)control.Value; break;
            case nameof(S_新手地图保护6): Config.新手地图保护6 = (int)control.Value; break;
            case nameof(S_新手地图保护7): Config.新手地图保护7 = (int)control.Value; break;
            case nameof(S_新手地图保护8): Config.新手地图保护8 = (int)control.Value; break;
            case nameof(S_新手地图保护9): Config.新手地图保护9 = (int)control.Value; break;
            case nameof(S_新手地图保护10): Config.新手地图保护10 = (int)control.Value; break;
            case nameof(S_沙巴克停止开关): Config.沙巴克停止开关 = (int)control.Value; break;
            case nameof(S_沙巴克城主称号): Config.沙巴克城主称号 = (byte)control.Value; break;
            case nameof(S_沙巴克成员称号): Config.沙巴克成员称号 = (byte)control.Value; break;
            case nameof(S_沙巴克称号领取开关): Config.沙巴克称号领取开关 = (int)control.Value; break;
            case nameof(S_通用1装备佩戴数量): Config.通用1装备佩戴数量 = (int)control.Value; break;
            case nameof(S_通用2装备佩戴数量): Config.通用2装备佩戴数量 = (int)control.Value; break;
            case nameof(S_通用3装备佩戴数量): Config.通用3装备佩戴数量 = (int)control.Value; break;
            case nameof(S_通用4装备佩戴数量): Config.通用4装备佩戴数量 = (int)control.Value; break;
            case nameof(S_通用5装备佩戴数量): Config.通用5装备佩戴数量 = (int)control.Value; break;
            case nameof(S_通用6装备佩戴数量): Config.通用6装备佩戴数量 = (int)control.Value; break;
            case nameof(S_重置屠魔副本时间): Config.重置屠魔副本时间 = (int)control.Value; break;
            case nameof(S_屠魔令回收数量): Config.屠魔令回收数量 = (int)control.Value; break;
            case nameof(S_新手上线赠送开关): Config.新手上线赠送开关 = (int)control.Value; break;
            case nameof(S_新手上线赠送物品1): Config.新手上线赠送物品1 = (int)control.Value; break;
            case nameof(S_新手上线赠送物品2): Config.新手上线赠送物品2 = (int)control.Value; break;
            case nameof(S_新手上线赠送物品3): Config.新手上线赠送物品3 = (int)control.Value; break;
            case nameof(S_新手上线赠送物品4): Config.新手上线赠送物品4 = (int)control.Value; break;
            case nameof(S_新手上线赠送物品5): Config.新手上线赠送物品5 = (int)control.Value; break;
            case nameof(S_新手上线赠送物品6): Config.新手上线赠送物品6 = (int)control.Value; break;
            case nameof(S_元宝袋新创数量1): Config.元宝袋新创数量1 = (int)control.Value; break;
            case nameof(S_元宝袋新创数量2): Config.元宝袋新创数量2 = (int)control.Value; break;
            case nameof(S_元宝袋新创数量3): Config.元宝袋新创数量3 = (int)control.Value; break;
            case nameof(S_元宝袋新创数量4): Config.元宝袋新创数量4 = (int)control.Value; break;
            case nameof(S_元宝袋新创数量5): Config.元宝袋新创数量5 = (int)control.Value; break;
            case nameof(S_高级赞助礼包1): Config.高级赞助礼包1 = (int)control.Value; break;
            case nameof(S_高级赞助礼包2): Config.高级赞助礼包2 = (int)control.Value; break;
            case nameof(S_高级赞助礼包3): Config.高级赞助礼包3 = (int)control.Value; break;
            case nameof(S_高级赞助礼包4): Config.高级赞助礼包4 = (int)control.Value; break;
            case nameof(S_高级赞助礼包5): Config.高级赞助礼包5 = (int)control.Value; break;
            case nameof(S_高级赞助礼包6): Config.高级赞助礼包6 = (int)control.Value; break;
            case nameof(S_高级赞助礼包7): Config.高级赞助礼包7 = (int)control.Value; break;
            case nameof(S_高级赞助礼包8): Config.高级赞助礼包8 = (int)control.Value; break;
            case nameof(S_高级赞助称号1): Config.高级赞助称号1 = (int)control.Value; break;
            case nameof(S_中级赞助礼包1): Config.中级赞助礼包1 = (int)control.Value; break;
            case nameof(S_中级赞助礼包2): Config.中级赞助礼包2 = (int)control.Value; break;
            case nameof(S_中级赞助礼包3): Config.中级赞助礼包3 = (int)control.Value; break;
            case nameof(S_中级赞助礼包4): Config.中级赞助礼包4 = (int)control.Value; break;
            case nameof(S_中级赞助礼包5): Config.中级赞助礼包5 = (int)control.Value; break;
            case nameof(S_中级赞助礼包6): Config.中级赞助礼包6 = (int)control.Value; break;
            case nameof(S_中级赞助礼包7): Config.中级赞助礼包7 = (int)control.Value; break;
            case nameof(S_中级赞助礼包8): Config.中级赞助礼包8 = (int)control.Value; break;
            case nameof(S_中级赞助称号1): Config.中级赞助称号1 = (int)control.Value; break;
            case nameof(S_初级赞助礼包1): Config.初级赞助礼包1 = (int)control.Value; break;
            case nameof(S_初级赞助礼包2): Config.初级赞助礼包2 = (int)control.Value; break;
            case nameof(S_初级赞助礼包3): Config.初级赞助礼包3 = (int)control.Value; break;
            case nameof(S_初级赞助礼包4): Config.初级赞助礼包4 = (int)control.Value; break;
            case nameof(S_初级赞助礼包5): Config.初级赞助礼包5 = (int)control.Value; break;
            case nameof(S_初级赞助礼包6): Config.初级赞助礼包6 = (int)control.Value; break;
            case nameof(S_初级赞助礼包7): Config.初级赞助礼包7 = (int)control.Value; break;
            case nameof(S_初级赞助礼包8): Config.初级赞助礼包8 = (int)control.Value; break;
            case nameof(S_初级赞助称号1): Config.初级赞助称号1 = (int)control.Value; break;
            case nameof(S_平台开关模式): Config.平台开关模式 = (int)control.Value; break;
            // TODO: Not used
            //case nameof(S_平台金币充值模块): Config.平台金币充值模块 = (int)control.Value; break;
            case nameof(S_平台元宝充值模块): Config.平台元宝充值模块 = (int)control.Value; break;
            case nameof(S_九层妖塔数量1): Config.九层妖塔数量1 = (int)control.Value; break;
            case nameof(S_九层妖塔数量2): Config.九层妖塔数量2 = (int)control.Value; break;
            case nameof(S_九层妖塔数量3): Config.九层妖塔数量3 = (int)control.Value; break;
            case nameof(S_九层妖塔数量4): Config.九层妖塔数量4 = (int)control.Value; break;
            case nameof(S_九层妖塔数量5): Config.九层妖塔数量5 = (int)control.Value; break;
            case nameof(S_九层妖塔数量6): Config.九层妖塔数量6 = (int)control.Value; break;
            case nameof(S_九层妖塔数量7): Config.九层妖塔数量7 = (int)control.Value; break;
            case nameof(S_九层妖塔数量8): Config.九层妖塔数量8 = (int)control.Value; break;
            case nameof(S_九层妖塔数量9): Config.九层妖塔数量9 = (int)control.Value; break;
            case nameof(S_九层妖塔副本次数): Config.九层妖塔副本次数 = (int)control.Value; break;
            case nameof(S_九层妖塔副本等级): Config.九层妖塔副本等级 = (int)control.Value; break;
            case nameof(S_九层妖塔副本物品): Config.九层妖塔副本物品 = (int)control.Value; break;
            case nameof(S_九层妖塔副本数量): Config.九层妖塔副本数量 = (int)control.Value; break;
            case nameof(S_九层妖塔副本时间小): Config.九层妖塔副本时间小 = (int)control.Value; break;
            case nameof(S_九层妖塔副本时间大): Config.九层妖塔副本时间大 = (int)control.Value; break;
            case nameof(S_AutoBattleLevel): Config.AutoBattleLevel = (byte)control.Value; break;
            case nameof(S_禁止背包铭文洗练): Config.禁止背包铭文洗练 = (byte)control.Value; break;
            case nameof(S_沙巴克禁止随机): Config.沙巴克禁止随机 = (byte)control.Value; break;
            case nameof(S_冥想丹自定义经验): Config.冥想丹自定义经验 = (int)control.Value; break;
            case nameof(S_沙巴克爆装备开关): Config.沙巴克爆装备开关 = (byte)control.Value; break;
            case nameof(S_铭文战士1挡1次数): Config.铭文战士1挡1次数 = (int)control.Value; break;
            case nameof(S_铭文战士1挡2次数): Config.铭文战士1挡2次数 = (int)control.Value; break;
            case nameof(S_铭文战士1挡3次数): Config.铭文战士1挡3次数 = (int)control.Value; break;
            case nameof(S_铭文战士2挡1次数): Config.铭文战士2挡1次数 = (int)control.Value; break;
            case nameof(S_铭文战士2挡2次数): Config.铭文战士2挡2次数 = (int)control.Value; break;
            case nameof(S_铭文战士2挡3次数): Config.铭文战士2挡3次数 = (int)control.Value; break;
            case nameof(S_铭文战士3挡1次数): Config.铭文战士3挡1次数 = (int)control.Value; break;
            case nameof(S_铭文战士3挡2次数): Config.铭文战士3挡2次数 = (int)control.Value; break;
            case nameof(S_铭文战士3挡3次数): Config.铭文战士3挡3次数 = (int)control.Value; break;
            case nameof(S_铭文战士1挡1概率): Config.铭文战士1挡1概率 = (int)control.Value; break;
            case nameof(S_铭文战士1挡2概率): Config.铭文战士1挡2概率 = (int)control.Value; break;
            case nameof(S_铭文战士1挡3概率): Config.铭文战士1挡3概率 = (int)control.Value; break;
            case nameof(S_铭文战士2挡1概率): Config.铭文战士2挡1概率 = (int)control.Value; break;
            case nameof(S_铭文战士2挡2概率): Config.铭文战士2挡2概率 = (int)control.Value; break;
            case nameof(S_铭文战士2挡3概率): Config.铭文战士2挡3概率 = (int)control.Value; break;
            case nameof(S_铭文战士3挡1概率): Config.铭文战士3挡1概率 = (int)control.Value; break;
            case nameof(S_铭文战士3挡2概率): Config.铭文战士3挡2概率 = (int)control.Value; break;
            case nameof(S_铭文战士3挡3概率): Config.铭文战士3挡3概率 = (int)control.Value; break;
            case nameof(S_铭文战士3挡技能编号): Config.铭文战士3挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文战士3挡技能铭文): Config.铭文战士3挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文战士2挡技能编号): Config.铭文战士2挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文战士2挡技能铭文): Config.铭文战士2挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文战士1挡技能编号): Config.铭文战士1挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文战士1挡技能铭文): Config.铭文战士1挡技能铭文 = (int)control.Value; break;
            case nameof(S_新手上线赠送称号1): Config.新手上线赠送称号1 = (int)control.Value; break;
            case nameof(S_铭文法师1挡1次数): Config.铭文法师1挡1次数 = (int)control.Value; break;
            case nameof(S_铭文法师1挡2次数): Config.铭文法师1挡2次数 = (int)control.Value; break;
            case nameof(S_铭文法师1挡3次数): Config.铭文法师1挡3次数 = (int)control.Value; break;
            case nameof(S_铭文法师2挡1次数): Config.铭文法师2挡1次数 = (int)control.Value; break;
            case nameof(S_铭文法师2挡2次数): Config.铭文法师2挡2次数 = (int)control.Value; break;
            case nameof(S_铭文法师2挡3次数): Config.铭文法师2挡3次数 = (int)control.Value; break;
            case nameof(S_铭文法师3挡1次数): Config.铭文法师3挡1次数 = (int)control.Value; break;
            case nameof(S_铭文法师3挡2次数): Config.铭文法师3挡2次数 = (int)control.Value; break;
            case nameof(S_铭文法师3挡3次数): Config.铭文法师3挡3次数 = (int)control.Value; break;
            case nameof(S_铭文法师1挡1概率): Config.铭文法师1挡1概率 = (int)control.Value; break;
            case nameof(S_铭文法师1挡2概率): Config.铭文法师1挡2概率 = (int)control.Value; break;
            case nameof(S_铭文法师1挡3概率): Config.铭文法师1挡3概率 = (int)control.Value; break;
            case nameof(S_铭文法师2挡1概率): Config.铭文法师2挡1概率 = (int)control.Value; break;
            case nameof(S_铭文法师2挡2概率): Config.铭文法师2挡2概率 = (int)control.Value; break;
            case nameof(S_铭文法师2挡3概率): Config.铭文法师2挡3概率 = (int)control.Value; break;
            case nameof(S_铭文法师3挡1概率): Config.铭文法师3挡1概率 = (int)control.Value; break;
            case nameof(S_铭文法师3挡2概率): Config.铭文法师3挡2概率 = (int)control.Value; break;
            case nameof(S_铭文法师3挡3概率): Config.铭文法师3挡3概率 = (int)control.Value; break;
            case nameof(S_铭文法师3挡技能编号): Config.铭文法师3挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文法师3挡技能铭文): Config.铭文法师3挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文法师2挡技能编号): Config.铭文法师2挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文法师2挡技能铭文): Config.铭文法师2挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文法师1挡技能编号): Config.铭文法师1挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文法师1挡技能铭文): Config.铭文法师1挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文道士1挡1次数): Config.铭文道士1挡1次数 = (int)control.Value; break;
            case nameof(S_铭文道士1挡2次数): Config.铭文道士1挡2次数 = (int)control.Value; break;
            case nameof(S_铭文道士1挡3次数): Config.铭文道士1挡3次数 = (int)control.Value; break;
            case nameof(S_铭文道士2挡1次数): Config.铭文道士2挡1次数 = (int)control.Value; break;
            case nameof(S_铭文道士2挡2次数): Config.铭文道士2挡2次数 = (int)control.Value; break;
            case nameof(S_铭文道士2挡3次数): Config.铭文道士2挡3次数 = (int)control.Value; break;
            case nameof(S_铭文道士3挡1次数): Config.铭文道士3挡1次数 = (int)control.Value; break;
            case nameof(S_铭文道士3挡2次数): Config.铭文道士3挡2次数 = (int)control.Value; break;
            case nameof(S_铭文道士3挡3次数): Config.铭文道士3挡3次数 = (int)control.Value; break;
            case nameof(S_铭文道士1挡1概率): Config.铭文道士1挡1概率 = (int)control.Value; break;
            case nameof(S_铭文道士1挡2概率): Config.铭文道士1挡2概率 = (int)control.Value; break;
            case nameof(S_铭文道士1挡3概率): Config.铭文道士1挡3概率 = (int)control.Value; break;
            case nameof(S_铭文道士2挡1概率): Config.铭文道士2挡1概率 = (int)control.Value; break;
            case nameof(S_铭文道士2挡2概率): Config.铭文道士2挡2概率 = (int)control.Value; break;
            case nameof(S_铭文道士2挡3概率): Config.铭文道士2挡3概率 = (int)control.Value; break;
            case nameof(S_铭文道士3挡1概率): Config.铭文道士3挡1概率 = (int)control.Value; break;
            case nameof(S_铭文道士3挡2概率): Config.铭文道士3挡2概率 = (int)control.Value; break;
            case nameof(S_铭文道士3挡3概率): Config.铭文道士3挡3概率 = (int)control.Value; break;
            case nameof(S_铭文道士3挡技能编号): Config.铭文道士3挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文道士3挡技能铭文): Config.铭文道士3挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文道士2挡技能编号): Config.铭文道士2挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文道士2挡技能铭文): Config.铭文道士2挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文道士1挡技能编号): Config.铭文道士1挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文道士1挡技能铭文): Config.铭文道士1挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文刺客1挡1次数): Config.铭文刺客1挡1次数 = (int)control.Value; break;
            case nameof(S_铭文刺客1挡2次数): Config.铭文刺客1挡2次数 = (int)control.Value; break;
            case nameof(S_铭文刺客1挡3次数): Config.铭文刺客1挡3次数 = (int)control.Value; break;
            case nameof(S_铭文刺客2挡1次数): Config.铭文刺客2挡1次数 = (int)control.Value; break;
            case nameof(S_铭文刺客2挡2次数): Config.铭文刺客2挡2次数 = (int)control.Value; break;
            case nameof(S_铭文刺客2挡3次数): Config.铭文刺客2挡3次数 = (int)control.Value; break;
            case nameof(S_铭文刺客3挡1次数): Config.铭文刺客3挡1次数 = (int)control.Value; break;
            case nameof(S_铭文刺客3挡2次数): Config.铭文刺客3挡2次数 = (int)control.Value; break;
            case nameof(S_铭文刺客3挡3次数): Config.铭文刺客3挡3次数 = (int)control.Value; break;
            case nameof(S_铭文刺客1挡1概率): Config.铭文刺客1挡1概率 = (int)control.Value; break;
            case nameof(S_铭文刺客1挡2概率): Config.铭文刺客1挡2概率 = (int)control.Value; break;
            case nameof(S_铭文刺客1挡3概率): Config.铭文刺客1挡3概率 = (int)control.Value; break;
            case nameof(S_铭文刺客2挡1概率): Config.铭文刺客2挡1概率 = (int)control.Value; break;
            case nameof(S_铭文刺客2挡2概率): Config.铭文刺客2挡2概率 = (int)control.Value; break;
            case nameof(S_铭文刺客2挡3概率): Config.铭文刺客2挡3概率 = (int)control.Value; break;
            case nameof(S_铭文刺客3挡1概率): Config.铭文刺客3挡1概率 = (int)control.Value; break;
            case nameof(S_铭文刺客3挡2概率): Config.铭文刺客3挡2概率 = (int)control.Value; break;
            case nameof(S_铭文刺客3挡3概率): Config.铭文刺客3挡3概率 = (int)control.Value; break;
            case nameof(S_铭文刺客3挡技能编号): Config.铭文刺客3挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文刺客3挡技能铭文): Config.铭文刺客3挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文刺客2挡技能编号): Config.铭文刺客2挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文刺客2挡技能铭文): Config.铭文刺客2挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文刺客1挡技能编号): Config.铭文刺客1挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文刺客1挡技能铭文): Config.铭文刺客1挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文弓手1挡1次数): Config.铭文弓手1挡1次数 = (int)control.Value; break;
            case nameof(S_铭文弓手1挡2次数): Config.铭文弓手1挡2次数 = (int)control.Value; break;
            case nameof(S_铭文弓手1挡3次数): Config.铭文弓手1挡3次数 = (int)control.Value; break;
            case nameof(S_铭文弓手2挡1次数): Config.铭文弓手2挡1次数 = (int)control.Value; break;
            case nameof(S_铭文弓手2挡2次数): Config.铭文弓手2挡2次数 = (int)control.Value; break;
            case nameof(S_铭文弓手2挡3次数): Config.铭文弓手2挡3次数 = (int)control.Value; break;
            case nameof(S_铭文弓手3挡1次数): Config.铭文弓手3挡1次数 = (int)control.Value; break;
            case nameof(S_铭文弓手3挡2次数): Config.铭文弓手3挡2次数 = (int)control.Value; break;
            case nameof(S_铭文弓手3挡3次数): Config.铭文弓手3挡3次数 = (int)control.Value; break;
            case nameof(S_铭文弓手1挡1概率): Config.铭文弓手1挡1概率 = (int)control.Value; break;
            case nameof(S_铭文弓手1挡2概率): Config.铭文弓手1挡2概率 = (int)control.Value; break;
            case nameof(S_铭文弓手1挡3概率): Config.铭文弓手1挡3概率 = (int)control.Value; break;
            case nameof(S_铭文弓手2挡1概率): Config.铭文弓手2挡1概率 = (int)control.Value; break;
            case nameof(S_铭文弓手2挡2概率): Config.铭文弓手2挡2概率 = (int)control.Value; break;
            case nameof(S_铭文弓手2挡3概率): Config.铭文弓手2挡3概率 = (int)control.Value; break;
            case nameof(S_铭文弓手3挡1概率): Config.铭文弓手3挡1概率 = (int)control.Value; break;
            case nameof(S_铭文弓手3挡2概率): Config.铭文弓手3挡2概率 = (int)control.Value; break;
            case nameof(S_铭文弓手3挡3概率): Config.铭文弓手3挡3概率 = (int)control.Value; break;
            case nameof(S_铭文弓手3挡技能编号): Config.铭文弓手3挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文弓手3挡技能铭文): Config.铭文弓手3挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文弓手2挡技能编号): Config.铭文弓手2挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文弓手2挡技能铭文): Config.铭文弓手2挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文弓手1挡技能编号): Config.铭文弓手1挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文弓手1挡技能铭文): Config.铭文弓手1挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文龙枪1挡1次数): Config.铭文龙枪1挡1次数 = (int)control.Value; break;
            case nameof(S_铭文龙枪1挡2次数): Config.铭文龙枪1挡2次数 = (int)control.Value; break;
            case nameof(S_铭文龙枪1挡3次数): Config.铭文龙枪1挡3次数 = (int)control.Value; break;
            case nameof(S_铭文龙枪2挡1次数): Config.铭文龙枪2挡1次数 = (int)control.Value; break;
            case nameof(S_铭文龙枪2挡2次数): Config.铭文龙枪2挡2次数 = (int)control.Value; break;
            case nameof(S_铭文龙枪2挡3次数): Config.铭文龙枪2挡3次数 = (int)control.Value; break;
            case nameof(S_铭文龙枪3挡1次数): Config.铭文龙枪3挡1次数 = (int)control.Value; break;
            case nameof(S_铭文龙枪3挡2次数): Config.铭文龙枪3挡2次数 = (int)control.Value; break;
            case nameof(S_铭文龙枪3挡3次数): Config.铭文龙枪3挡3次数 = (int)control.Value; break;
            case nameof(S_铭文龙枪1挡1概率): Config.铭文龙枪1挡1概率 = (int)control.Value; break;
            case nameof(S_铭文龙枪1挡2概率): Config.铭文龙枪1挡2概率 = (int)control.Value; break;
            case nameof(S_铭文龙枪1挡3概率): Config.铭文龙枪1挡3概率 = (int)control.Value; break;
            case nameof(S_铭文龙枪2挡1概率): Config.铭文龙枪2挡1概率 = (int)control.Value; break;
            case nameof(S_铭文龙枪2挡2概率): Config.铭文龙枪2挡2概率 = (int)control.Value; break;
            case nameof(S_铭文龙枪2挡3概率): Config.铭文龙枪2挡3概率 = (int)control.Value; break;
            case nameof(S_铭文龙枪3挡1概率): Config.铭文龙枪3挡1概率 = (int)control.Value; break;
            case nameof(S_铭文龙枪3挡2概率): Config.铭文龙枪3挡2概率 = (int)control.Value; break;
            case nameof(S_铭文龙枪3挡3概率): Config.铭文龙枪3挡3概率 = (int)control.Value; break;
            case nameof(S_铭文龙枪3挡技能编号): Config.铭文龙枪3挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文龙枪3挡技能铭文): Config.铭文龙枪3挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文龙枪2挡技能编号): Config.铭文龙枪2挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文龙枪2挡技能铭文): Config.铭文龙枪2挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文龙枪1挡技能编号): Config.铭文龙枪1挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文龙枪1挡技能铭文): Config.铭文龙枪1挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文道士保底开关): Config.铭文道士保底开关 = (int)control.Value; break;
            case nameof(S_铭文龙枪保底开关): Config.铭文龙枪保底开关 = (int)control.Value; break;
            case nameof(S_铭文战士保底开关): Config.铭文战士保底开关 = (int)control.Value; break;
            case nameof(S_铭文法师保底开关): Config.铭文法师保底开关 = (int)control.Value; break;
            case nameof(S_铭文刺客保底开关): Config.铭文刺客保底开关 = (int)control.Value; break;
            case nameof(S_铭文弓手保底开关): Config.铭文弓手保底开关 = (int)control.Value; break;
            case nameof(S_DropRateModifier): Config.DropRateModifier = (int)control.Value; break;
            case nameof(S_魔虫窟副本次数): Config.魔虫窟副本次数 = (int)control.Value; break;
            case nameof(S_魔虫窟副本等级): Config.魔虫窟副本等级 = (int)control.Value; break;
            case nameof(S_魔虫窟副本物品): Config.魔虫窟副本物品 = (int)control.Value; break;
            case nameof(S_魔虫窟副本数量): Config.魔虫窟副本数量 = (int)control.Value; break;
            case nameof(S_魔虫窟副本时间小): Config.魔虫窟副本时间小 = (int)control.Value; break;
            case nameof(S_魔虫窟副本时间大): Config.魔虫窟副本时间大 = (int)control.Value; break;
            case nameof(S_幸运洗练次数保底): Config.幸运洗练次数保底 = (int)control.Value; break;
            case nameof(S_幸运洗练点数): Config.幸运洗练点数 = (int)control.Value; break;
            case nameof(S_武器强化消耗货币值): Config.武器强化消耗货币值 = (int)control.Value; break;
            case nameof(S_武器强化消耗货币开关): Config.武器强化消耗货币开关 = (int)control.Value; break;
            case nameof(S_武器强化取回时间): Config.武器强化取回时间 = (int)control.Value; break;
            case nameof(S_幸运额外1值): Config.幸运额外1值 = (int)control.Value; break;
            case nameof(S_幸运额外2值): Config.幸运额外2值 = (int)control.Value; break;
            case nameof(S_幸运额外3值): Config.幸运额外3值 = (int)control.Value; break;
            case nameof(S_幸运额外4值): Config.幸运额外4值 = (int)control.Value; break;
            case nameof(S_幸运额外5值): Config.幸运额外5值 = (int)control.Value; break;
            case nameof(S_幸运额外1伤害): Config.幸运额外1伤害 = (float)control.Value; break;
            case nameof(S_幸运额外2伤害): Config.幸运额外2伤害 = (float)control.Value; break;
            case nameof(S_幸运额外3伤害): Config.幸运额外3伤害 = (float)control.Value; break;
            case nameof(S_幸运额外4伤害): Config.幸运额外4伤害 = (float)control.Value; break;
            case nameof(S_幸运额外5伤害): Config.幸运额外5伤害 = (float)control.Value; break;
            case nameof(S_暗之门地图1): Config.暗之门地图1 = (int)control.Value; break;
            case nameof(S_暗之门地图2): Config.暗之门地图2 = (int)control.Value; break;
            case nameof(S_暗之门地图3): Config.暗之门地图3 = (int)control.Value; break;
            case nameof(S_暗之门地图4): Config.暗之门地图4 = (int)control.Value; break;
            case nameof(S_暗之门全服提示): Config.暗之门全服提示 = (int)control.Value; break;
            case nameof(S_暗之门杀怪触发): Config.暗之门杀怪触发 = (int)control.Value; break;
            case nameof(S_暗之门时间): Config.暗之门时间 = (int)control.Value; break;
            case nameof(S_暗之门地图1X): Config.暗之门地图1X = (int)control.Value; break;
            case nameof(S_暗之门地图1Y): Config.暗之门地图1Y = (int)control.Value; break;
            case nameof(S_暗之门地图2X): Config.暗之门地图2X = (int)control.Value; break;
            case nameof(S_暗之门地图2Y): Config.暗之门地图2Y = (int)control.Value; break;
            case nameof(S_暗之门地图3X): Config.暗之门地图3X = (int)control.Value; break;
            case nameof(S_暗之门地图3Y): Config.暗之门地图3Y = (int)control.Value; break;
            case nameof(S_暗之门地图4X): Config.暗之门地图4X = (int)control.Value; break;
            case nameof(S_暗之门地图4Y): Config.暗之门地图4Y = (int)control.Value; break;
            case nameof(S_暗之门开关): Config.暗之门开关 = (int)control.Value; break;
            case nameof(S_监狱货币): Config.监狱货币 = (int)control.Value; break;
            case nameof(S_监狱货币类型): Config.监狱货币类型 = (int)control.Value; break;
            case nameof(S_魔虫窟分钟限制): Config.魔虫窟分钟限制 = (int)control.Value; break;
            case nameof(S_自定义元宝兑换01): Config.自定义元宝兑换01 = (int)control.Value; break;
            case nameof(S_自定义元宝兑换02): Config.自定义元宝兑换02 = (int)control.Value; break;
            case nameof(S_自定义元宝兑换03): Config.自定义元宝兑换03 = (int)control.Value; break;
            case nameof(S_自定义元宝兑换04): Config.自定义元宝兑换04 = (int)control.Value; break;
            case nameof(S_自定义元宝兑换05): Config.自定义元宝兑换05 = (int)control.Value; break;
            case nameof(S_直升物品1): Config.直升物品1 = (int)control.Value; break;
            case nameof(S_直升物品2): Config.直升物品2 = (int)control.Value; break;
            case nameof(S_直升物品3): Config.直升物品3 = (int)control.Value; break;
            case nameof(S_直升物品4): Config.直升物品4 = (int)control.Value; break;
            case nameof(S_直升物品5): Config.直升物品5 = (int)control.Value; break;
            case nameof(S_直升物品6): Config.直升物品6 = (int)control.Value; break;
            case nameof(S_直升物品7): Config.直升物品7 = (int)control.Value; break;
            case nameof(S_直升物品8): Config.直升物品8 = (int)control.Value; break;
            case nameof(S_直升物品9): Config.直升物品9 = (int)control.Value; break;
            case nameof(S_直升等级1): Config.直升等级1 = (int)control.Value; break;
            case nameof(S_直升等级2): Config.直升等级2 = (int)control.Value; break;
            case nameof(S_直升等级3): Config.直升等级3 = (int)control.Value; break;
            case nameof(S_直升等级4): Config.直升等级4 = (int)control.Value; break;
            case nameof(S_直升等级5): Config.直升等级5 = (int)control.Value; break;
            case nameof(S_直升等级6): Config.直升等级6 = (int)control.Value; break;
            case nameof(S_直升等级7): Config.直升等级7 = (int)control.Value; break;
            case nameof(S_直升等级8): Config.直升等级8 = (int)control.Value; break;
            case nameof(S_直升等级9): Config.直升等级9 = (int)control.Value; break;
            case nameof(S_直升经验1): Config.直升经验1 = (int)control.Value; break;
            case nameof(S_直升经验2): Config.直升经验2 = (int)control.Value; break;
            case nameof(S_直升经验3): Config.直升经验3 = (int)control.Value; break;
            case nameof(S_直升经验4): Config.直升经验4 = (int)control.Value; break;
            case nameof(S_直升经验5): Config.直升经验5 = (int)control.Value; break;
            case nameof(S_直升经验6): Config.直升经验6 = (int)control.Value; break;
            case nameof(S_直升经验7): Config.直升经验7 = (int)control.Value; break;
            case nameof(S_直升经验8): Config.直升经验8 = (int)control.Value; break;
            case nameof(S_直升经验9): Config.直升经验9 = (int)control.Value; break;
            case nameof(S_充值模块格式): Config.充值模块格式 = (int)control.Value; break;
            case nameof(UpgradeXPLevel1): Config.UpgradeXPLevel1 = (int)control.Value; break;
            case nameof(UpgradeXPLevel2): Config.UpgradeXPLevel2 = (int)control.Value; break;
            case nameof(UpgradeXPLevel3): Config.UpgradeXPLevel3 = (int)control.Value; break;
            case nameof(UpgradeXPLevel4): Config.UpgradeXPLevel4 = (int)control.Value; break;
            case nameof(UpgradeXPLevel5): Config.UpgradeXPLevel5 = (int)control.Value; break;
            case nameof(UpgradeXPLevel6): Config.UpgradeXPLevel6 = (int)control.Value; break;
            case nameof(UpgradeXPLevel7): Config.UpgradeXPLevel7 = (int)control.Value; break;
            case nameof(UpgradeXPLevel8): Config.UpgradeXPLevel8 = (int)control.Value; break;
            case nameof(UpgradeXPLevel9): Config.UpgradeXPLevel9 = (int)control.Value; break;
            case nameof(UpgradeXPLevel10): Config.UpgradeXPLevel10 = (int)control.Value; break;
            case nameof(UpgradeXPLevel11): Config.UpgradeXPLevel11 = (int)control.Value; break;
            case nameof(UpgradeXPLevel12): Config.UpgradeXPLevel12 = (int)control.Value; break;
            case nameof(UpgradeXPLevel13): Config.UpgradeXPLevel13 = (int)control.Value; break;
            case nameof(UpgradeXPLevel14): Config.UpgradeXPLevel14 = (int)control.Value; break;
            case nameof(UpgradeXPLevel15): Config.UpgradeXPLevel15 = (int)control.Value; break;
            case nameof(UpgradeXPLevel16): Config.UpgradeXPLevel16 = (int)control.Value; break;
            case nameof(UpgradeXPLevel17): Config.UpgradeXPLevel17 = (int)control.Value; break;
            case nameof(UpgradeXPLevel18): Config.UpgradeXPLevel18 = (int)control.Value; break;
            case nameof(UpgradeXPLevel19): Config.UpgradeXPLevel19 = (int)control.Value; break;
            case nameof(UpgradeXPLevel20): Config.UpgradeXPLevel20 = (int)control.Value; break;
            case nameof(UpgradeXPLevel21): Config.UpgradeXPLevel21 = (int)control.Value; break;
            case nameof(UpgradeXPLevel22): Config.UpgradeXPLevel22 = (int)control.Value; break;
            case nameof(UpgradeXPLevel23): Config.UpgradeXPLevel23 = (int)control.Value; break;
            case nameof(UpgradeXPLevel24): Config.UpgradeXPLevel24 = (int)control.Value; break;
            case nameof(UpgradeXPLevel25): Config.UpgradeXPLevel25 = (int)control.Value; break;
            case nameof(UpgradeXPLevel26): Config.UpgradeXPLevel26 = (int)control.Value; break;
            case nameof(UpgradeXPLevel27): Config.UpgradeXPLevel27 = (int)control.Value; break;
            case nameof(UpgradeXPLevel28): Config.UpgradeXPLevel28 = (int)control.Value; break;
            case nameof(UpgradeXPLevel29): Config.UpgradeXPLevel29 = (int)control.Value; break;
            case nameof(UpgradeXPLevel30): Config.UpgradeXPLevel30 = (int)control.Value; break;
            case nameof(UpgradeXPLevel31): Config.UpgradeXPLevel31 = (int)control.Value; break;
            case nameof(UpgradeXPLevel32): Config.UpgradeXPLevel32 = (int)control.Value; break;
            case nameof(UpgradeXPLevel33): Config.UpgradeXPLevel33 = (int)control.Value; break;
            case nameof(UpgradeXPLevel34): Config.UpgradeXPLevel34 = (int)control.Value; break;
            case nameof(UpgradeXPLevel35): Config.UpgradeXPLevel35 = (int)control.Value; break;
            case nameof(UpgradeXPLevel36): Config.UpgradeXPLevel36 = (int)control.Value; break;
            case nameof(UpgradeXPLevel37): Config.UpgradeXPLevel37 = (int)control.Value; break;
            case nameof(UpgradeXPLevel38): Config.UpgradeXPLevel38 = (int)control.Value; break;
            case nameof(UpgradeXPLevel39): Config.UpgradeXPLevel39 = (int)control.Value; break;
            case nameof(DefaultSkillLevel): Config.DefaultSkillLevel = (int)control.Value; break;
            case nameof(S_其他分解几率一): Config.其他分解几率一 = (int)control.Value; break;
            case nameof(S_其他分解几率二): Config.其他分解几率二 = (int)control.Value; break;
            case nameof(S_其他分解几率三): Config.其他分解几率三 = (int)control.Value; break;
            case nameof(S_其他分解几率四): Config.其他分解几率四 = (int)control.Value; break;
            case nameof(S_其他分解数量一): Config.其他分解数量一 = (int)control.Value; break;
            case nameof(S_其他分解数量二): Config.其他分解数量二 = (int)control.Value; break;
            case nameof(S_其他分解数量三): Config.其他分解数量三 = (int)control.Value; break;
            case nameof(S_其他分解数量四): Config.其他分解数量四 = (int)control.Value; break;
            case nameof(S_其他分解开关): Config.其他分解开关 = (int)control.Value; break;
            case nameof(S_沃玛分解几率一): Config.沃玛分解几率一 = (int)control.Value; break;
            case nameof(S_沃玛分解几率二): Config.沃玛分解几率二 = (int)control.Value; break;
            case nameof(S_沃玛分解几率三): Config.沃玛分解几率三 = (int)control.Value; break;
            case nameof(S_沃玛分解几率四): Config.沃玛分解几率四 = (int)control.Value; break;
            case nameof(S_沃玛分解数量一): Config.沃玛分解数量一 = (int)control.Value; break;
            case nameof(S_沃玛分解数量二): Config.沃玛分解数量二 = (int)control.Value; break;
            case nameof(S_沃玛分解数量三): Config.沃玛分解数量三 = (int)control.Value; break;
            case nameof(S_沃玛分解数量四): Config.沃玛分解数量四 = (int)control.Value; break;
            case nameof(S_沃玛分解开关): Config.沃玛分解开关 = (int)control.Value; break;
            case nameof(拾取地图控制1): Config.AutoPickUpMap1 = (int)control.Value; break;
            case nameof(拾取地图控制2): Config.AutoPickUpMap2 = (int)control.Value; break;
            case nameof(拾取地图控制3): Config.AutoPickUpMap3 = (int)control.Value; break;
            case nameof(拾取地图控制4): Config.AutoPickUpMap4 = (int)control.Value; break;
            case nameof(拾取地图控制5): Config.AutoPickUpMap5 = (int)control.Value; break;
            case nameof(拾取地图控制6): Config.AutoPickUpMap6 = (int)control.Value; break;
            case nameof(拾取地图控制7): Config.AutoPickUpMap7 = (int)control.Value; break;
            case nameof(拾取地图控制8): Config.AutoPickUpMap8 = (int)control.Value; break;
            case nameof(沙城捐献货币类型): Config.沙城捐献货币类型 = (int)control.Value; break;
            case nameof(沙城捐献支付数量): Config.沙城捐献支付数量 = (int)control.Value; break;
            case nameof(沙城捐献获得物品1): Config.沙城捐献获得物品1 = (int)control.Value; break;
            case nameof(沙城捐献获得物品2): Config.沙城捐献获得物品2 = (int)control.Value; break;
            case nameof(沙城捐献获得物品3): Config.沙城捐献获得物品3 = (int)control.Value; break;
            case nameof(沙城捐献物品数量1): Config.沙城捐献物品数量1 = (int)control.Value; break;
            case nameof(沙城捐献物品数量2): Config.沙城捐献物品数量2 = (int)control.Value; break;
            case nameof(沙城捐献物品数量3): Config.沙城捐献物品数量3 = (int)control.Value; break;
            case nameof(沙城捐献赞助金额): Config.沙城捐献赞助金额 = (int)control.Value; break;
            case nameof(沙城捐献赞助人数): Config.沙城捐献赞助人数 = (int)control.Value; break;
            case nameof(雕爷激活灵符需求): Config.雕爷激活灵符需求 = (int)control.Value; break;
            case nameof(雕爷1号位灵符): Config.雕爷1号位灵符 = (int)control.Value; break;
            case nameof(雕爷2号位灵符): Config.雕爷2号位灵符 = (int)control.Value; break;
            case nameof(雕爷3号位灵符): Config.雕爷3号位灵符 = (int)control.Value; break;
            case nameof(雕爷1号位铭文石): Config.雕爷1号位铭文石 = (int)control.Value; break;
            case nameof(雕爷2号位铭文石): Config.雕爷2号位铭文石 = (int)control.Value; break;
            case nameof(雕爷3号位铭文石): Config.雕爷3号位铭文石 = (int)control.Value; break;
            case nameof(S_称号范围拾取判断1): Config.称号范围拾取判断1 = (int)control.Value; break;
            case nameof(九层妖塔统计开关): Config.九层妖塔统计开关 = (int)control.Value; break;
            case nameof(沙巴克每周攻沙时间): Config.沙巴克每周攻沙时间 = (int)control.Value; break;
            case nameof(沙巴克皇宫传送等级): Config.沙巴克皇宫传送等级 = (int)control.Value; break;
            case nameof(沙巴克皇宫传送物品): Config.沙巴克皇宫传送物品 = (int)control.Value; break;
            case nameof(沙巴克皇宫传送数量): Config.沙巴克皇宫传送数量 = (int)control.Value; break;
            case nameof(系统窗口发送): Config.系统窗口发送 = (int)control.Value; break;
            case nameof(龙卫效果提示): Config.龙卫效果提示 = (int)control.Value; break;
            case nameof(充值平台切换): Config.充值平台切换 = (int)control.Value; break;
            case nameof(全服红包等级): Config.全服红包等级 = (int)control.Value; break;
            case nameof(全服红包时间): Config.全服红包时间 = (int)control.Value; break;
            case nameof(全服红包货币类型): Config.GlobalBonusCurrencyType = (int)control.Value; break;
            case nameof(全服红包货币数量): Config.全服红包货币数量 = (int)control.Value; break;
            case nameof(龙卫蓝色词条概率): Config.龙卫蓝色词条概率 = (int)control.Value; break;
            case nameof(龙卫紫色词条概率): Config.龙卫紫色词条概率 = (int)control.Value; break;
            case nameof(龙卫橙色词条概率): Config.龙卫橙色词条概率 = (int)control.Value; break;
            case nameof(自定义初始货币类型): Config.自定义初始货币类型 = (int)control.Value; break;
            case nameof(会员物品对接): Config.会员物品对接 = (int)control.Value; break;
            case nameof(称号叠加模块9): Config.称号叠加模块9 = (byte)control.Value; break;
            case nameof(称号叠加模块10): Config.称号叠加模块10 = (byte)control.Value; break;
            case nameof(称号叠加模块11): Config.称号叠加模块11 = (byte)control.Value; break;
            case nameof(称号叠加模块12): Config.称号叠加模块12 = (byte)control.Value; break;
            case nameof(称号叠加模块13): Config.称号叠加模块13 = (byte)control.Value; break;
            case nameof(称号叠加模块14): Config.称号叠加模块14 = (byte)control.Value; break;
            case nameof(称号叠加模块15): Config.称号叠加模块15 = (byte)control.Value; break;
            case nameof(称号叠加模块16): Config.称号叠加模块16 = (byte)control.Value; break;
            case nameof(变性等级): Config.变性等级 = (int)control.Value; break;
            case nameof(变性货币类型): Config.变性货币类型 = (int)control.Value; break;
            case nameof(变性货币值): Config.变性货币值 = (int)control.Value; break;
            case nameof(变性物品ID): Config.变性物品ID = (int)control.Value; break;
            case nameof(变性物品数量): Config.变性物品数量 = (int)control.Value; break;
            case nameof(龙卫焰焚烈火剑法): Config.龙卫焰焚烈火剑法 = (int)control.Value; break;
            case nameof(屠魔殿等级限制): Config.屠魔殿等级限制 = (int)control.Value; break;
            case nameof(职业等级): Config.职业等级 = (int)control.Value; break;
            case nameof(职业货币类型): Config.职业货币类型 = (int)control.Value; break;
            case nameof(职业货币值): Config.职业货币值 = (int)control.Value; break;
            case nameof(职业物品ID): Config.职业物品ID = (int)control.Value; break;
            case nameof(职业物品数量): Config.职业物品数量 = (int)control.Value; break;

            default:
                MessageBox.Show("Unknown Control! " + control.Name);
                break;
        }
        Config.Save();
    }

    private void UpdateBooleanSettingsValue_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not CheckBox control)
        {
            MessageBox.Show($"CheckedChanged raised on invalid control {sender.GetType()}");
            return;
        }

        if (!control.Visible) return;

        if (control.Tag is bool oldValue)
        {
            if (oldValue == control.Checked)
                return;
            control.Tag = oldValue;
        }

        switch (control.Name)
        {
            case nameof(自动回收设置): Config.自动回收设置 = control.Checked; break;
            case nameof(购买狂暴之力): Config.购买狂暴之力 = control.Checked; break;
            case nameof(会员满血设置): Config.会员满血设置 = control.Checked; break;
            case nameof(全屏拾取开关): Config.AutoPickUpAllVisible = control.Checked; break;
            case nameof(打开随时仓库): Config.打开随时仓库 = control.Checked; break;
            case nameof(幸运保底开关): Config.幸运保底开关 = control.Checked; break;
            case nameof(红包开关): Config.红包开关 = control.Checked; break;
            case nameof(安全区收刀开关): Config.安全区收刀开关 = control.Checked; break;

            default:
                MessageBox.Show("Unknown Control! " + control.Name);
                break;
        }
        Config.Save();
    }

    private void UpdateTextSettingsValue_TextChanged(object sender, EventArgs e)
    {
        if (sender is not TextBox control)
        {
            MessageBox.Show($"TextChanged raised on invalid control {sender.GetType()}");
            return;
        }

        if (!control.Visible) return;

        if (control.Tag is string oldValue)
        {
            if (oldValue == control.Text)
                return;
            control.Tag = oldValue;
        }

        switch (control.Name)
        {
            case nameof(S_狂暴名称): Config.狂暴名称 = control.Text; break;
            case nameof(S_自定义物品内容一): Config.自定义物品内容一 = control.Text; break;
            case nameof(S_自定义物品内容二): Config.自定义物品内容二 = control.Text; break;
            case nameof(S_自定义物品内容三): Config.自定义物品内容三 = control.Text; break;
            case nameof(S_自定义物品内容四): Config.自定义物品内容四 = control.Text; break;
            case nameof(S_自定义物品内容五): Config.自定义物品内容五 = control.Text; break;
            case nameof(S_战将特权礼包): Config.战将特权礼包 = control.Text; break;
            case nameof(S_豪杰特权礼包): Config.豪杰特权礼包 = control.Text; break;
            case nameof(S_世界BOSS名字): Config.WorldBossName = control.Text; break;
            case nameof(S_祖玛分解物品一): Config.祖玛分解物品一 = control.Text; break;
            case nameof(S_祖玛分解物品二): Config.祖玛分解物品二 = control.Text; break;
            case nameof(S_祖玛分解物品三): Config.祖玛分解物品三 = control.Text; break;
            case nameof(S_祖玛分解物品四): Config.祖玛分解物品四 = control.Text; break;
            case nameof(S_赤月分解物品一): Config.赤月分解物品一 = control.Text; break;
            case nameof(S_赤月分解物品二): Config.赤月分解物品二 = control.Text; break;
            case nameof(S_赤月分解物品三): Config.赤月分解物品三 = control.Text; break;
            case nameof(S_赤月分解物品四): Config.赤月分解物品四 = control.Text; break;
            case nameof(S_魔龙分解物品一): Config.魔龙分解物品一 = control.Text; break;
            case nameof(S_魔龙分解物品二): Config.魔龙分解物品二 = control.Text; break;
            case nameof(S_魔龙分解物品三): Config.魔龙分解物品三 = control.Text; break;
            case nameof(S_魔龙分解物品四): Config.魔龙分解物品四 = control.Text; break;
            case nameof(S_苍月分解物品一): Config.苍月分解物品一 = control.Text; break;
            case nameof(S_苍月分解物品二): Config.苍月分解物品二 = control.Text; break;
            case nameof(S_苍月分解物品三): Config.苍月分解物品三 = control.Text; break;
            case nameof(S_苍月分解物品四): Config.苍月分解物品四 = control.Text; break;
            case nameof(S_星王分解物品一): Config.星王分解物品一 = control.Text; break;
            case nameof(S_星王分解物品二): Config.星王分解物品二 = control.Text; break;
            case nameof(S_星王分解物品三): Config.星王分解物品三 = control.Text; break;
            case nameof(S_星王分解物品四): Config.星王分解物品四 = control.Text; break;
            case nameof(S_城主分解物品一): Config.城主分解物品一 = control.Text; break;
            case nameof(S_城主分解物品二): Config.城主分解物品二 = control.Text; break;
            case nameof(S_城主分解物品三): Config.城主分解物品三 = control.Text; break;
            case nameof(S_城主分解物品四): Config.城主分解物品四 = control.Text; break;
            case nameof(S_BOSS卷轴怪物一): Config.BOSS卷轴怪物一 = control.Text; break;
            case nameof(S_BOSS卷轴怪物二): Config.BOSS卷轴怪物二 = control.Text; break;
            case nameof(S_BOSS卷轴怪物三): Config.BOSS卷轴怪物三 = control.Text; break;
            case nameof(S_BOSS卷轴怪物四): Config.BOSS卷轴怪物四 = control.Text; break;
            case nameof(S_BOSS卷轴怪物五): Config.BOSS卷轴怪物五 = control.Text; break;
            case nameof(S_BOSS卷轴怪物六): Config.BOSS卷轴怪物六 = control.Text; break;
            case nameof(S_BOSS卷轴怪物七): Config.BOSS卷轴怪物七 = control.Text; break;
            case nameof(S_BOSS卷轴怪物八): Config.BOSS卷轴怪物八 = control.Text; break;
            case nameof(S_BOSS卷轴怪物九): Config.BOSS卷轴怪物九 = control.Text; break;
            case nameof(S_BOSS卷轴怪物十): Config.BOSS卷轴怪物十 = control.Text; break;
            case nameof(S_BOSS卷轴怪物11): Config.BOSS卷轴怪物11 = control.Text; break;
            case nameof(S_BOSS卷轴怪物12): Config.BOSS卷轴怪物12 = control.Text; break;
            case nameof(S_BOSS卷轴怪物13): Config.BOSS卷轴怪物13 = control.Text; break;
            case nameof(S_BOSS卷轴怪物14): Config.BOSS卷轴怪物14 = control.Text; break;
            case nameof(S_BOSS卷轴怪物15): Config.BOSS卷轴怪物15 = control.Text; break;
            case nameof(S_BOSS卷轴怪物16): Config.BOSS卷轴怪物16 = control.Text; break;
            case nameof(S_九层妖塔BOSS1): Config.九层妖塔BOSS1 = control.Text; break;
            case nameof(S_九层妖塔BOSS2): Config.九层妖塔BOSS2 = control.Text; break;
            case nameof(S_九层妖塔BOSS3): Config.九层妖塔BOSS3 = control.Text; break;
            case nameof(S_九层妖塔BOSS4): Config.九层妖塔BOSS4 = control.Text; break;
            case nameof(S_九层妖塔BOSS5): Config.九层妖塔BOSS5 = control.Text; break;
            case nameof(S_九层妖塔BOSS6): Config.九层妖塔BOSS6 = control.Text; break;
            case nameof(S_九层妖塔BOSS7): Config.九层妖塔BOSS7 = control.Text; break;
            case nameof(S_九层妖塔BOSS8): Config.九层妖塔BOSS8 = control.Text; break;
            case nameof(S_九层妖塔BOSS9): Config.九层妖塔BOSS9 = control.Text; break;
            case nameof(S_九层妖塔精英1): Config.九层妖塔精英1 = control.Text; break;
            case nameof(S_九层妖塔精英2): Config.九层妖塔精英2 = control.Text; break;
            case nameof(S_九层妖塔精英3): Config.九层妖塔精英3 = control.Text; break;
            case nameof(S_九层妖塔精英4): Config.九层妖塔精英4 = control.Text; break;
            case nameof(S_九层妖塔精英5): Config.九层妖塔精英5 = control.Text; break;
            case nameof(S_九层妖塔精英6): Config.九层妖塔精英6 = control.Text; break;
            case nameof(S_九层妖塔精英7): Config.九层妖塔精英7 = control.Text; break;
            case nameof(S_九层妖塔精英8): Config.九层妖塔精英8 = control.Text; break;
            case nameof(S_九层妖塔精英9): Config.九层妖塔精英9 = control.Text; break;
            case nameof(S_书店商贩物品): Config.书店商贩物品 = control.Text; break;
            case nameof(S_挂机权限选项): Config.挂机权限选项 = control.Text; break;
            case nameof(S_暗之门地图1BOSS): Config.暗之门地图1BOSS = control.Text; break;
            case nameof(S_暗之门地图2BOSS): Config.暗之门地图2BOSS = control.Text; break;
            case nameof(S_暗之门地图3BOSS): Config.暗之门地图3BOSS = control.Text; break;
            case nameof(S_暗之门地图4BOSS): Config.暗之门地图4BOSS = control.Text; break;
            case nameof(S_沃玛分解物品一): Config.沃玛分解物品一 = control.Text; break;
            case nameof(S_沃玛分解物品二): Config.沃玛分解物品二 = control.Text; break;
            case nameof(S_沃玛分解物品三): Config.沃玛分解物品三 = control.Text; break;
            case nameof(S_沃玛分解物品四): Config.沃玛分解物品四 = control.Text; break;
            case nameof(S_其他分解物品一): Config.其他分解物品一 = control.Text; break;
            case nameof(S_其他分解物品二): Config.其他分解物品二 = control.Text; break;
            case nameof(S_其他分解物品三): Config.其他分解物品三 = control.Text; break;
            case nameof(S_其他分解物品四): Config.其他分解物品四 = control.Text; break;
            case nameof(合成模块控件): Config.合成模块控件 = control.Text; break;
            case nameof(变性内容控件): Config.变性内容控件 = control.Text; break;
            case nameof(转职内容控件): Config.转职内容控件 = control.Text; break;

            default:
                MessageBox.Show("Unknown Control! " + control.Name);
                break;
        }
        Config.Save();
    }

    private void GMCommandTextBox_Press(object sender, KeyPressEventArgs e)
    {
        var str = GMCommandTextBox.Text;

        if (e.KeyChar != Convert.ToChar(13) || string.IsNullOrEmpty(str))
            return;

        主选项卡.SelectedIndex = 0;
        LoggingTab.SelectedIndex = 2;

        AddCommandLog("=> " + str);

        if (SEngine.AddGMCommand(str))
            e.Handled = true;
        GMCommandTextBox.Clear();
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
        Config.系统公告内容 = text;
        Config.Save();
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
        Config.系统公告内容 = text;
        Config.Save();
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
        Config.系统公告内容 = text;
        Config.Save();
    }

    private void button8_Click(object sender, EventArgs e)
    {
        Process.Start("IEXPLORE.EXE", "https://jq.qq.com/?_wv=1027&k=WS2L3pIf");
    }
    
    private void S_浏览平台目录_Click(object sender, EventArgs e)
    {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
        {
            Description = "请选择文件夹"
        };
        if (folderBrowserDialog.ShowDialog() == DialogResult.OK && sender == S_浏览平台目录)
        {
            var path = folderBrowserDialog.SelectedPath;
            S_平台接入目录.Text = Config.平台接入目录 = path;
            Config.Save();
        }
    }
    
    private void startServerToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SEngine.StartService();
        Config.Save();
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
        DropDataTable = new System.Data.DataTable("掉落数据表");
        怪物掉落表 = new Dictionary<MonsterInfo, List<KeyValuePair<GameItem, long>>>();
        DropDataTable.Columns.Add("物品名字", typeof(string));
        DropDataTable.Columns.Add("掉落数量", typeof(string));
        Main.掉落浏览表.DataSource = DropDataTable;
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

    private void gameMonsterToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadMonsterInfos();
    }

    private void mapGuardToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadGuardInfos();
    }

    private void dialogDataToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadNpcDialogs();
    }

    private void gameMapToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadGameMaps();
    }

    private void terrainDataToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadTerrains();
    }

    private void mapAreaToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadMapAreas();
    }

    private void teleportCircleToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadTeleportGates();
    }

    private void monsterRefreshToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadMonsterSpawns();
    }

    private void guardRefreshToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadMapGuards();
    }

    private void gameItemsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadGameItems();
    }

    private void equipmentSetToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadItemSetInfos();
    }

    private void syntheticDataToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadItemCraftings();
    }

    private void chestDataToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadTreasureChestInfos();
    }

    private void randomAttributeToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadRandomStats();
    }

    private void itemAttributeToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadEquipmentStats();
    }

    private void gameShopToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadGameStores();
    }

    private void treasuresToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadRareTreasureItems();
    }

    private void gameTitleToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadGameTitles();
    }

    private void inscriptionSkillsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadInscriptionSkills();
    }

    private void gameSkillsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadGameSkills();
    }

    private void skillTrapToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadSkillTraps();
    }

    private void gameBuffToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadGameBuffs();
    }

    private void gameMountToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadGameMounts();
    }

    private void mountRoyalBeastToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadMountBeasts();
    }

    private void vIPDataToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SystemDataGateway.LoadVIPSystem();
    }
}
