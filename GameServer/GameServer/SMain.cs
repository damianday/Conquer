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
    private static Dictionary<GameMap, DataRow> MapDataRows;
    private static Dictionary<MonsterInfo, DataRow> MonsterDataRows;
    private static Dictionary<DataRow, MonsterInfo> 数据行怪物;
    private static Dictionary<string, DataRow> BlockedDataRows;
    private static Dictionary<DataGridViewRow, DateTime> 公告数据表;

    private static Dictionary<CharacterInfo, List<KeyValuePair<ushort, SkillInfo>>> CharacterSkillsList;
    private static Dictionary<CharacterInfo, List<KeyValuePair<byte, EquipmentInfo>>> CharacterEquipmentList;
    private static Dictionary<CharacterInfo, List<KeyValuePair<byte, ItemInfo>>> CharacterInventoryList;
    private static Dictionary<CharacterInfo, List<KeyValuePair<byte, ItemInfo>>> CharacterStorageList;
    private static Dictionary<MonsterInfo, List<KeyValuePair<GameItem, long>>> 怪物掉落表;

    #region LoadSystemData
    public static void LoadSystemData()
    {
        Main.allToolStripMenuItem.Visible = false;

        AddSystemLog("Connecting to 'System' database");
        DBAgent.X.InitDB(Settings.Default.GameDataPath + "\\System\\System.db");

        AddSystemLog("Loading system data...");
        MapDataTable = new System.Data.DataTable("地图数据表");
        MapDataRows = new Dictionary<GameMap, DataRow>();
        MapDataTable.Columns.Add("MapID", typeof(string));
        MapDataTable.Columns.Add("MapName", typeof(string));
        MapDataTable.Columns.Add("RequiredLevel", typeof(string));
        MapDataTable.Columns.Add("Number of players", typeof(string));
        MapDataTable.Columns.Add("Monster Cap", typeof(uint));
        MapDataTable.Columns.Add("Monsters Alive", typeof(uint));
        MapDataTable.Columns.Add("Monster resurrection times", typeof(uint));
        MapDataTable.Columns.Add("Monster Drops", typeof(long));
        MapDataTable.Columns.Add("Total gold dropped", typeof(long));
        Main?.MapBrowser.BeginInvoke(() =>
        {
            Main.MapBrowser.DataSource = MapDataTable;
        });
        MonsterDataTable = new System.Data.DataTable("怪物数据表");
        MonsterDataRows = new Dictionary<MonsterInfo, DataRow>();
        数据行怪物 = new Dictionary<DataRow, MonsterInfo>();
        MonsterDataTable.Columns.Add("Monster ID", typeof(string));
        MonsterDataTable.Columns.Add("Monster Name", typeof(string));
        MonsterDataTable.Columns.Add("Level", typeof(string));
        MonsterDataTable.Columns.Add("EXP", typeof(string));
        MonsterDataTable.Columns.Add("Grade", typeof(string));
        MonsterDataTable.Columns.Add("Move Time", typeof(string));
        MonsterDataTable.Columns.Add("Roaming interval", typeof(string));
        MonsterDataTable.Columns.Add("View Range", typeof(string));
        MonsterDataTable.Columns.Add("Agro Time", typeof(string));
        Main?.怪物浏览表.BeginInvoke(() =>
        {
            Main.怪物浏览表.DataSource = MonsterDataTable;
        });
        DropDataTable = new System.Data.DataTable("掉落数据表");
        怪物掉落表 = new Dictionary<MonsterInfo, List<KeyValuePair<GameItem, long>>>();
        DropDataTable.Columns.Add("Item Name", typeof(string));
        DropDataTable.Columns.Add("Drop Quantity", typeof(string));
        Main?.掉落浏览表.BeginInvoke(() =>
        {
            Main.掉落浏览表.DataSource = DropDataTable;
        });
        SystemDataGateway.LoadData();
        AddSystemLog("The system data load is complete");
    }
    #endregion

    #region LoadUserData
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

        RoleDataTable.Columns.Add("Name", typeof(string));
        RoleDataTable.Columns.Add("AccountName", typeof(string));
        RoleDataTable.Columns.Add("BlockDate", typeof(string));
        RoleDataTable.Columns.Add("AccountBlockDate", typeof(string));
        RoleDataTable.Columns.Add("FreezeDate", typeof(string));
        RoleDataTable.Columns.Add("DeletetionDate", typeof(string));
        RoleDataTable.Columns.Add("LoginDate", typeof(string));
        RoleDataTable.Columns.Add("DisconnectDate", typeof(string));
        RoleDataTable.Columns.Add("IPAddress", typeof(string));
        RoleDataTable.Columns.Add("MACAddress", typeof(string));
        RoleDataTable.Columns.Add("Class", typeof(string));
        RoleDataTable.Columns.Add("Gender", typeof(string));
        RoleDataTable.Columns.Add("Guild", typeof(string));
        RoleDataTable.Columns.Add("Ingots", typeof(string));
        RoleDataTable.Columns.Add("Consume gold", typeof(string));
        RoleDataTable.Columns.Add("Gold", typeof(string));
        RoleDataTable.Columns.Add("Traded Gold", typeof(string));
        RoleDataTable.Columns.Add("InventorySize", typeof(string));
        RoleDataTable.Columns.Add("StorageSize", typeof(string));
        RoleDataTable.Columns.Add("Reputation of the Master", typeof(string));
        RoleDataTable.Columns.Add("CurrentPrivilege", typeof(string));
        RoleDataTable.Columns.Add("Issue Date", typeof(string));
        RoleDataTable.Columns.Add("PreviousPrivilege", typeof(string));
        RoleDataTable.Columns.Add("IssueDate", typeof(string));
        RoleDataTable.Columns.Add("RemainingPrivileges", typeof(string));
        RoleDataTable.Columns.Add("Level", typeof(string));
        RoleDataTable.Columns.Add("EXP", typeof(string));
        RoleDataTable.Columns.Add("EXP Rate", typeof(string));
        RoleDataTable.Columns.Add("CombatPower", typeof(string));
        RoleDataTable.Columns.Add("Current Map", typeof(string));
        RoleDataTable.Columns.Add("Current Cords", typeof(string));
        RoleDataTable.Columns.Add("CurrentPKPoint", typeof(string));
        RoleDataTable.Columns.Add("Activation logo", typeof(string));

        Main?.BeginInvoke(() =>
        {
            Main.角色浏览表.DataSource = RoleDataTable;
            for (int i = 0; i < Main.角色浏览表.Columns.Count; i++)
            {
                Main.角色浏览表.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        });

        CharacterSkillsList = new Dictionary<CharacterInfo, List<KeyValuePair<ushort, SkillInfo>>>();
        SkillsDataTable.Columns.Add("Skill Name", typeof(string));
        SkillsDataTable.Columns.Add("Skill Number", typeof(string));
        SkillsDataTable.Columns.Add("Level", typeof(string));
        SkillsDataTable.Columns.Add("EXP", typeof(string));
        Main?.BeginInvoke(() =>
        {
            Main.技能浏览表.DataSource = SkillsDataTable;
        });

        CharacterEquipmentList = new Dictionary<CharacterInfo, List<KeyValuePair<byte, EquipmentInfo>>>();
        EquipmentDataTable.Columns.Add("Item Type", typeof(string));
        EquipmentDataTable.Columns.Add("Item Name", typeof(string));
        Main?.BeginInvoke(() =>
        {
            Main.装备浏览表.DataSource = EquipmentDataTable;
        });

        CharacterInventoryList = new Dictionary<CharacterInfo, List<KeyValuePair<byte, ItemInfo>>>();
        InventoryDataTable.Columns.Add("Bag Slot", typeof(string));
        InventoryDataTable.Columns.Add("Item Name", typeof(string));
        Main?.BeginInvoke(() =>
        {
            Main.背包浏览表.DataSource = InventoryDataTable;
        });

        CharacterStorageList = new Dictionary<CharacterInfo, List<KeyValuePair<byte, ItemInfo>>>();
        WarehouseDataTable.Columns.Add("Storage Slot", typeof(string));
        WarehouseDataTable.Columns.Add("Item Name", typeof(string));
        Main?.BeginInvoke(() =>
        {
            Main.仓库浏览表.DataSource = WarehouseDataTable;
        });

        BlockingDataTable = new System.Data.DataTable();
        BlockedDataRows = new Dictionary<string, DataRow>();
        BlockingDataTable.Columns.Add("IPAddress", typeof(string));
        BlockingDataTable.Columns.Add("MACAddress", typeof(string));
        BlockingDataTable.Columns.Add("ExpireDate", typeof(string));
        Main?.BeginInvoke(() =>
        {
            Main.封禁浏览表.DataSource = BlockingDataTable;
        });

        Session.Load();

        Main.allToolStripMenuItem.Visible = true;
        AddSystemLog("The user data is loaded");
    }
    #endregion

    #region On/Off Service Completed
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
    #endregion

    #region Add Logs
    public static void AddSystemLog(string message)
    {
        Main?.BeginInvoke(() =>
        {
            Main.SystemLogsTextBox.AppendText($"[{DateTime.Now}] {message}" + "\r\n");
            Main.SystemLogsTextBox.ScrollToCaret();
            Main.saveSystemLogsToolStripMenuItem.Enabled = true;
            Main.clearSystemLogsToolStripMenuItem.Enabled = true;
        });
    }

    public static void AddChatLog(string tag, string message)
    {
        Main?.BeginInvoke(() =>
        {
            Main.ChatLogsTextBox.AppendText($"[{DateTime.Now}] {tag + message}" + "\r\n");
            Main.ChatLogsTextBox.ScrollToCaret();
            Main.saveChatLogsToolStripMenuItem.Enabled = true;
            Main.clearChatLogsToolStripMenuItem.Enabled = true;
        });
    }

    public static void AddCommandLog(string message)
    {
        Main?.BeginInvoke(() =>
        {
            Main.CommandLogsTextBox.AppendText($"[{DateTime.Now}] {message}" + "\r\n");
            Main.CommandLogsTextBox.ScrollToCaret();
            Main.clearCommandsLogToolStripMenuItem.Enabled = true;
        });
    }
    #endregion

    #region UpdateStats
    public static void UpdateStats(SystemStatsState stats)
    {
        Main?.BeginInvoke(() =>
        {
            Main.PortStatusLabel.Text = Settings.Default.UserConnectionPort.ToString();
            Main.RunTimeStatusLabel.Text = $"RunTime: {stats.RunTime:dd\\:hh\\:mm\\:ss}";
            Main.ConnectionsStatusLabel.Text = $"Connections: {stats.ActiveConnections}/{stats.Connections}";
            Main.OnlineStatusLabel.Text = $"Online: {stats.ConnectionsOnline}/{stats.ConnectionsOnline1}/{stats.ConnectionsOnline2}";
            Main.ObjectsStatusLabel.Text = $"Objects: {stats.ActiveObjects}/{stats.SecondaryObjects}/{stats.Objects}";
            Main.CycleStatusLabel.Text = $"Cycles: {stats.CycleCount}";
            Main.DataSentStatusLabel.Text = $"Sent: {Compute.GetBytesReadable(stats.TotalSentBytes)}";
            Main.DataReceivedStatusLabel.Text = $"Received: {Compute.GetBytesReadable(stats.TotalReceivedBytes)}";

            string processName = "AccountServer"; 
            bool processFound = Process.GetProcessesByName(processName).Length > 0;
            Main.AccountServerCheck.Text = processFound ? "Account Server Online" : "Account Server Offline";
            Main.AccountServerCheck.BackColor = processFound ? Color.Green : Color.Red;
        });
    }
    #endregion

    #region Character Info
    public static void 添加数据显示(CharacterInfo character)
    {
        if (!RoleDataRows.ContainsKey(character))
        {
            RoleDataRows[character] = RoleDataTable.NewRow();
            RoleDataTable.Rows.Add(RoleDataRows[character]);
        }
    }

    public static void 修改数据显示(CharacterInfo character, string 表头文本, string 表格内容)
    {
        if (RoleDataRows.ContainsKey(character))
        {
            RoleDataRows[character][表头文本] = 表格内容;
        }
    }

    public static void AddCharacterInfo(CharacterInfo character)
    {
        Main?.BeginInvoke(() =>
        {
            if (!RoleDataRows.ContainsKey(character))
            {
                DataRow dataRow = RoleDataTable.NewRow();
                dataRow["Name"] = character;
                dataRow["AccountName"] = character.Account;
                dataRow["AccountBlockDate"] = ((character.Account.V.BlockDate.V != default(DateTime)) ? character.Account.V.BlockDate : null);
                dataRow["BlockDate"] = ((character.BlockDate.V != default(DateTime)) ? character.BlockDate : null);
                dataRow["FreezeDate"] = ((character.FrozenDate.V != default(DateTime)) ? character.FrozenDate : null);
                dataRow["DeletetionDate"] = ((character.DeletetionDate.V != default(DateTime)) ? character.DeletetionDate : null);
                dataRow["LoginDate"] = ((character.LoginDate.V != default(DateTime)) ? character.LoginDate : null);
                dataRow["DisconnectDate"] = (!character.Connected ? character.DisconnectDate : null);
                dataRow["IPAddress"] = character.IPAddress;
                dataRow["MACAddress"] = character.MACAddress;
                dataRow["Class"] = character.Job;
                dataRow["Gender"] = character.Gender;
                dataRow["Guild"] = character.Guild;
                dataRow["Ingots"] = character.Ingot;
                dataRow["Consume gold"] = character.消耗元宝;
                dataRow["Gold"] = character.Gold;
                dataRow["Traded Gold"] = character.TradeGold;
                dataRow["InventorySize"] = character.InventorySize;
                dataRow["StorageSize"] = character.StorageSize;
                dataRow["Reputation of the Master"] = character.师门声望;
                dataRow["CurrentPrivilege"] = character.CurrentPrivilege;
                dataRow["IssueDate"] = character.本期日期;
                dataRow["PreviousPrivilege"] = character.PreviousPrivilege;
                dataRow["IssueDate"] = character.上期日期;
                dataRow["RemainingPrivileges"] = character.RemainingPrivileges;
                dataRow["Level"] = character.Level;
                dataRow["EXP"] = character.Experience;
                dataRow["EXP Rate"] = character.ExperienceRate;
                dataRow["CombatPower"] = character.CombatPower;
                dataRow["Current Map"] = (GameMap.DataSheet.TryGetValue((byte)character.CurrentMap.V, out var value) ? ((object)value.MapName) : ((object)character.CurrentMap));
                dataRow["CurrentPKPoint"] = character.CurrentPKPoint;
                dataRow["Current Cords"] = $"{character.CurrentPosition.V.X}, {character.CurrentPosition.V.Y}";
                dataRow["Activation logo"] = character.激活标识;
                RoleDataRows[character] = dataRow;
                数据行角色[dataRow] = character;
                RoleDataTable.Rows.Add(dataRow);
            }
        });
    }

    public static void RemoveCharacterInfo(CharacterInfo character)
    {
        Main?.BeginInvoke(() =>
        {
            if (RoleDataRows.TryGetValue(character, out var value))
            {
                数据行角色.Remove(value);
                RoleDataTable.Rows.Remove(value);
                CharacterSkillsList.Remove(character);
                CharacterInventoryList.Remove(character);
                CharacterEquipmentList.Remove(character);
                CharacterStorageList.Remove(character);
            }
        });
    }

    #region Proccess Update UI
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
                if (CharacterSkillsList.TryGetValue(value, out var value2))
                {
                    foreach (KeyValuePair<ushort, SkillInfo> item in value2)
                    {
                        DataRow dataRow = SkillsDataTable.NewRow();
                        dataRow["Skill Name"] = item.Value.Inscription.SkillName;
                        dataRow["Skill Number"] = item.Value.ID;
                        dataRow["Level"] = item.Value.Level;
                        dataRow["EXP"] = item.Value.Experience;
                        SkillsDataTable.Rows.Add(dataRow);
                    }
                }
                if (CharacterEquipmentList.TryGetValue(value, out var value3))
                {
                    foreach (KeyValuePair<byte, EquipmentInfo> item2 in value3)
                    {
                        DataRow dataRow2 = EquipmentDataTable.NewRow();
                        dataRow2["Item Type"] = (EquipmentWearType)item2.Key;
                        dataRow2["Item Name"] = item2.Value;
                        EquipmentDataTable.Rows.Add(dataRow2);
                    }
                }
                if (CharacterInventoryList.TryGetValue(value, out var value4))
                {
                    foreach (KeyValuePair<byte, ItemInfo> item3 in value4)
                    {
                        DataRow dataRow3 = InventoryDataTable.NewRow();
                        dataRow3["Bag Slot"] = item3.Key;
                        dataRow3["Item Name"] = item3.Value;
                        InventoryDataTable.Rows.Add(dataRow3);
                    }
                }
                if (CharacterStorageList.TryGetValue(value, out var value5))
                {
                    foreach (KeyValuePair<byte, ItemInfo> item4 in value5)
                    {
                        DataRow dataRow4 = WarehouseDataTable.NewRow();
                        dataRow4["Storage Slot"] = item4.Key;
                        dataRow4["Item Name"] = item4.Value;
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
            dataRow5["Item Name"] = item5.Key.Name;
            dataRow5["Drop Quantity"] = item5.Value;
            DropDataTable.Rows.Add(dataRow5);
        }
    }
    #endregion

    #region Update Character
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

    public static void UpdateCharacterSkills(CharacterInfo character, List<KeyValuePair<ushort, SkillInfo>> skills)
    {
        Main?.BeginInvoke(() =>
        {
            CharacterSkillsList[character] = skills;
        });
    }

    public static void UpdateCharacterEquipment(CharacterInfo character, List<KeyValuePair<byte, EquipmentInfo>> equipment)
    {
        Main?.BeginInvoke(() =>
        {
            CharacterEquipmentList[character] = equipment;
        });
    }

    public static void UpdateCharacterInventory(CharacterInfo character, List<KeyValuePair<byte, ItemInfo>> inventory)
    {
        Main?.BeginInvoke(() =>
        {
            CharacterInventoryList[character] = inventory;
        });
    }

    public static void UpdateCharacterStorage(CharacterInfo character, List<KeyValuePair<byte, ItemInfo>> storage)
    {
        Main?.BeginInvoke(() =>
        {
            CharacterStorageList[character] = storage;
        });
    }
    #endregion
    #endregion

    #region Map Info
    public static void AddMapData(Map.Map map)
    {
        Main?.BeginInvoke(() =>
        {
            if (!MapDataRows.ContainsKey(map.MapInfo))
            {
                var row = MapDataTable.NewRow();
                row["MapID"] = map.MapID;
                row["MapName"] = map.MapInfo;
                row["RequiredLevel"] = map.MinLevel;
                row["Number of players"] = map.Players.Count;
                row["Monster Cap"] = map.TotalFixedMonsters;
                row["Monsters Alive"] = map.TotalSurvivingMonsters;
                row["Monster resurrection times"] = map.TotalAmountMonsterResurrected;
                row["Monster Drops"] = map.TotalAmountMonsterDrops;
                row["Total gold dropped"] = map.TotalAmountGoldDrops;
                MapDataRows[map.MapInfo] = row;
                MapDataTable.Rows.Add(row);
            }
        });
    }

    public static void UpdateMapData(Map.Map map, string key, object value)
    {
        Main?.BeginInvoke(() =>
        {
            if (MapDataRows.TryGetValue(map.MapInfo, out var row))
            {
                switch (key)
                {
                    default:
                        row[key] = value;
                        break;
                    case "Total gold dropped":
                    case "Monster Drops":
                        row[key] = Convert.ToInt64(row[key]) + (int)value;
                        break;
                    case "Monsters Alive":
                    case "Monster resurrection times":
                        row[key] = Convert.ToUInt32(row[key]) + (int)value;
                        break;
                }
            }
        });
    }
    #endregion

    #region Monster Info
    public static void AddMonsterData(MonsterInfo monster)
    {
        Main?.BeginInvoke(() =>
        {
            if (!MonsterDataRows.ContainsKey(monster))
            {
                var row = MonsterDataTable.NewRow();
                row["Monster ID"] = monster.ID;
                row["Monster Name"] = monster.MonsterName;
                row["Level"] = monster.Level;
                row["Grade"] = monster.Grade;
                row["EXP"] = monster.ProvideExperience;
                row["Move Time"] = monster.MoveInterval;
                row["View Range"] = monster.RangeHate;
                row["Agro Time"] = monster.HateTime;
                MonsterDataRows[monster] = row;
                数据行怪物[row] = monster;
                MonsterDataTable.Rows.Add(row);
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
    #endregion

    #region Bans
    public static void AddBlockData(string address, DateTime date, bool networkAddress = true)
    {
        Main?.BeginInvoke(() =>
        {
            if (!BlockedDataRows.ContainsKey(address))
            {
                var row = BlockingDataTable.NewRow();
                row["IPAddress"] = (networkAddress ? address : null);
                row["MACAddress"] = (networkAddress ? null : address);
                row["ExpireDate"] = date;
                BlockedDataRows[address] = row;
                BlockingDataTable.Rows.Add(row);
            }
        });
    }

    public static void UpdateBlockData(string address, DateTime date, bool networkAddress = true)
    {
        Main?.BeginInvoke(() =>
        {
            if (BlockedDataRows.TryGetValue(address, out var row))
            {
                row["IPAddress"] = (networkAddress ? address : null);
                row["MACAddress"] = (networkAddress ? null : address);
                row["ExpireDate"] = date;
            }
        });
    }

    public static void RemoveBlockData(string address)
    {
        Main?.BeginInvoke(() =>
        {
            if (BlockedDataRows.TryGetValue(address, out var value))
            {
                BlockedDataRows.Remove(address);
                BlockingDataTable.Rows.Remove(value);
            }
        });
    }
    #endregion
    public SMain()
    {
        int index;

        InitializeComponent();

        Settings.Default.Load();

        Control.CheckForIllegalCrossThreadCalls = false;
        Main = this;

        string 系统公告内容 = Settings.Default.系统公告内容;
        公告数据表 = new Dictionary<DataGridViewRow, DateTime>();
        string[] array = 系统公告内容.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < array.Length; i++)
        {
            string[] array2 = array[i].Split('\t');
            index = 公告浏览表.Rows.Add();
            公告浏览表.Rows[index].Cells["公告间隔"].Value = array2[0];
            公告浏览表.Rows[index].Cells["公告次数"].Value = array2[1];
            公告浏览表.Rows[index].Cells["公告内容"].Value = array2[2];
        }

        角色浏览表.ColumnHeadersDefaultCellStyle.Font = (MapBrowser.ColumnHeadersDefaultCellStyle.Font = (怪物浏览表.ColumnHeadersDefaultCellStyle.Font = (掉落浏览表.ColumnHeadersDefaultCellStyle.Font = (封禁浏览表.ColumnHeadersDefaultCellStyle.Font = (角色浏览表.DefaultCellStyle.Font = (MapBrowser.DefaultCellStyle.Font = (怪物浏览表.DefaultCellStyle.Font = (封禁浏览表.DefaultCellStyle.Font = (掉落浏览表.DefaultCellStyle.Font = new Font("宋体", 9f))))))))));
        S_GameDataPath.Text = Settings.Default.GameDataPath;
        S_DataBackupPath.Text = Settings.Default.DataBackupPath;
        S_UserConnectionPort.Value = Settings.Default.UserConnectionPort;
        S_TicketReceivePort.Value = Settings.Default.TicketReceivePort;
        S_PacketLimit.Value = Settings.Default.PacketLimit;
        S_AbnormalBlockTime.Value = Settings.Default.AbnormalBlockTime;
        S_DisconnectTime.Value = Settings.Default.DisconnectTime;
        S_MaxUserLevel.Value = Settings.Default.MaxUserLevel;
        S_NoobSupportLevel.Value = Settings.Default.NoobSupportLevel;
        S_SpecialRepairDiscount.Value = Settings.Default.SpecialRepairDiscount;
        S_ItemDropRate.Value = Settings.Default.ItemDropRate;
        S_MonsterExperienceMultiplier.Value = Settings.Default.MonsterExperienceMultiplier;
        S_减收益等级差.Value = Settings.Default.减收益等级差;
        S_收益减少比率.Value = Settings.Default.收益减少比率;
        S_怪物诱惑时长.Value = Settings.Default.怪物诱惑时长;
        S_物品归属时间.Value = Settings.Default.物品归属时间;
        S_ItemDisappearTime.Value = Settings.Default.ItemDisappearTime;
        S_自动保存时间.Value = Settings.Default.AutoSaveInterval;
        S_自动保存日志.Value = Settings.Default.自动保存日志;
        S_沃玛分解元宝.Value = Settings.Default.沃玛分解元宝;
        S_祖玛分解元宝.Value = Settings.Default.祖玛分解元宝;
        S_赤月分解元宝.Value = Settings.Default.赤月分解元宝;
        S_魔龙分解元宝.Value = Settings.Default.魔龙分解元宝;
        S_苍月分解元宝.Value = Settings.Default.苍月分解元宝;
        S_星王分解元宝.Value = Settings.Default.星王分解元宝;
        S_神秘分解元宝.Value = Settings.Default.神秘分解元宝;
        S_城主分解元宝.Value = Settings.Default.城主分解元宝;
        S_屠魔爆率开关.Value = Settings.Default.屠魔爆率开关;
        S_屠魔组队人数.Value = Settings.Default.屠魔组队人数;
        S_屠魔令回收经验.Value = Settings.Default.屠魔令回收经验;
        S_武斗场时间一.Value = Settings.Default.武斗场时间一;
        S_武斗场时间二.Value = Settings.Default.武斗场时间二;
        S_武斗场经验小.Value = Settings.Default.武斗场经验小;
        S_武斗场经验大.Value = Settings.Default.武斗场经验大;
        S_沙巴克开启.Value = Settings.Default.沙巴克开启;
        S_沙巴克结束.Value = Settings.Default.沙巴克结束;
        S_祝福油幸运1机率.Value = Settings.Default.祝福油幸运1机率;
        S_祝福油幸运2机率.Value = Settings.Default.祝福油幸运2机率;
        S_祝福油幸运3机率.Value = Settings.Default.祝福油幸运3机率;
        S_祝福油幸运4机率.Value = Settings.Default.祝福油幸运4机率;
        S_祝福油幸运5机率.Value = Settings.Default.祝福油幸运5机率;
        S_祝福油幸运6机率.Value = Settings.Default.祝福油幸运6机率;
        S_祝福油幸运7机率.Value = Settings.Default.祝福油幸运7机率;
        S_PKYellowNamePoint.Value = Settings.Default.PKYellowNamePoint;
        S_PKRedNamePoint.Value = Settings.Default.PKRedNamePoint;
        S_PKCrimsonNamePoint.Value = Settings.Default.PKCrimsonNamePoint;
        S_锻造成功倍数.Value = Settings.Default.锻造成功倍数;
        S_死亡掉落背包几率.Value = (decimal)Settings.Default.死亡掉落背包几率;
        S_死亡掉落身上几率.Value = (decimal)Settings.Default.死亡掉落身上几率;
        S_PK死亡幸运开关.Value = Settings.Default.PK死亡幸运开关;
        S_屠魔副本次数.Value = Settings.Default.屠魔副本次数;
        S_高级祝福油幸运机率.Value = Settings.Default.高级祝福油幸运机率;
        S_雕爷使用物品.Value = Settings.Default.雕爷使用物品;
        S_雕爷使用金币.Value = Settings.Default.雕爷使用金币;
        S_称号范围拾取判断.Value = Settings.Default.称号范围拾取判断;
        S_TitleRangePickUpDistance.Value = Settings.Default.TitleRangePickUpDistance;
        S_行会申请人数限制.Value = Settings.Default.行会申请人数限制;
        S_疗伤药HP.Value = Settings.Default.疗伤药HP;
        S_疗伤药MP.Value = Settings.Default.疗伤药MP;
        S_万年雪霜HP.Value = Settings.Default.万年雪霜HP;
        S_万年雪霜MP.Value = Settings.Default.万年雪霜MP;
        S_元宝金币回收设定.Value = Settings.Default.元宝金币回收设定;
        S_元宝金币传送设定.Value = Settings.Default.元宝金币传送设定;
        S_快捷传送一编号.Value = Settings.Default.快捷传送一编号;
        S_快捷传送一货币.Value = Settings.Default.快捷传送一货币;
        S_快捷传送一等级.Value = Settings.Default.快捷传送一等级;
        S_快捷传送二编号.Value = Settings.Default.快捷传送二编号;
        S_快捷传送二货币.Value = Settings.Default.快捷传送二货币;
        S_快捷传送二等级.Value = Settings.Default.快捷传送二等级;
        S_快捷传送三编号.Value = Settings.Default.快捷传送三编号;
        S_快捷传送三货币.Value = Settings.Default.快捷传送三货币;
        S_快捷传送三等级.Value = Settings.Default.快捷传送三等级;
        S_快捷传送四编号.Value = Settings.Default.快捷传送四编号;
        S_快捷传送四货币.Value = Settings.Default.快捷传送四货币;
        S_快捷传送四等级.Value = Settings.Default.快捷传送四等级;
        S_快捷传送五编号.Value = Settings.Default.快捷传送五编号;
        S_快捷传送五货币.Value = Settings.Default.快捷传送五货币;
        S_快捷传送五等级.Value = Settings.Default.快捷传送五等级;
        S_狂暴货币格式.Value = Settings.Default.狂暴货币格式;
        S_狂暴称号格式.Value = Settings.Default.狂暴称号格式;
        S_狂暴开启物品名称.Value = Settings.Default.狂暴开启物品名称;
        S_狂暴开启物品数量.Value = Settings.Default.狂暴开启物品数量;
        S_狂暴杀死物品数量.Value = Settings.Default.狂暴杀死物品数量;
        S_狂暴开启元宝数量.Value = Settings.Default.狂暴开启元宝数量;
        S_狂暴杀死元宝数量.Value = Settings.Default.狂暴杀死元宝数量;
        S_狂暴开启金币数量.Value = Settings.Default.狂暴开启金币数量;
        S_狂暴杀死金币数量.Value = Settings.Default.狂暴杀死金币数量;
        S_装备技能开关.Value = Settings.Default.装备技能开关;
        S_御兽属性开启.Value = Settings.Default.御兽属性开启;
        S_可摆摊地图编号.Value = Settings.Default.可摆摊地图编号;
        S_可摆摊地图坐标X.Value = Settings.Default.可摆摊地图坐标X;
        S_可摆摊地图坐标Y.Value = Settings.Default.可摆摊地图坐标Y;
        S_可摆摊地图范围.Value = Settings.Default.可摆摊地图范围;
        S_可摆摊货币选择.Value = Settings.Default.可摆摊货币选择;
        S_可摆摊等级.Value = Settings.Default.可摆摊等级;
        S_ReviveInterval.Value = Settings.Default.ReviveInterval;
        S_自定义麻痹几率.Value = (decimal)Settings.Default.自定义麻痹几率;


        index = (int)S_UpgradeXPLevel.Value;
        S_UpgradeXP.Value = Settings.Default.UserUpgradeXP[index - 1];

        index = (int)S_PetUpgradeXPLevel.Value;
        S_PetUpgradeXP.Value = Settings.Default.PetUpgradeXP[index - 1];

        S_下马击落机率.Value = Settings.Default.下马击落机率;
        S_AllowRaceWarrior.Value = Settings.Default.AllowRaceWarrior;
        S_AllowRaceWizard.Value = Settings.Default.AllowRaceWizard;
        S_AllowRaceTaoist.Value = Settings.Default.AllowRaceTaoist;
        S_AllowRaceArcher.Value = Settings.Default.AllowRaceArcher;
        S_AllowRaceAssassin.Value = Settings.Default.AllowRaceAssassin;
        S_AllowRaceDragonLance.Value = Settings.Default.AllowRaceDragonLance;
        S_泡点等级开关.Value = Settings.Default.泡点等级开关;
        S_泡点当前经验.Value = Settings.Default.泡点当前经验;
        S_泡点限制等级.Value = Settings.Default.泡点限制等级;
        S_杀人PK红名开关.Value = Settings.Default.杀人PK红名开关;
        S_泡点秒数控制.Value = Settings.Default.泡点秒数控制;
        S_自定义物品数量一.Value = Settings.Default.自定义物品数量一;
        S_自定义物品数量二.Value = Settings.Default.自定义物品数量二;
        S_自定义物品数量三.Value = Settings.Default.自定义物品数量三;
        S_自定义物品数量四.Value = Settings.Default.自定义物品数量四;
        S_自定义物品数量五.Value = Settings.Default.自定义物品数量五;
        S_自定义称号内容一.Value = Settings.Default.自定义称号内容一;
        S_自定义称号内容二.Value = Settings.Default.自定义称号内容二;
        S_自定义称号内容三.Value = Settings.Default.自定义称号内容三;
        S_自定义称号内容四.Value = Settings.Default.自定义称号内容四;
        S_自定义称号内容五.Value = Settings.Default.自定义称号内容五;
        S_元宝金币传送设定2.Value = Settings.Default.元宝金币传送设定2;
        S_快捷传送一编号2.Value = Settings.Default.快捷传送一编号2;
        S_快捷传送一货币2.Value = Settings.Default.快捷传送一货币2;
        S_快捷传送一等级2.Value = Settings.Default.快捷传送一等级2;
        S_快捷传送二编号2.Value = Settings.Default.快捷传送二编号2;
        S_快捷传送二货币2.Value = Settings.Default.快捷传送二货币2;
        S_快捷传送二等级2.Value = Settings.Default.快捷传送二等级2;
        S_快捷传送三编号2.Value = Settings.Default.快捷传送三编号2;
        S_快捷传送三货币2.Value = Settings.Default.快捷传送三货币2;
        S_快捷传送三等级2.Value = Settings.Default.快捷传送三等级2;
        S_快捷传送四编号2.Value = Settings.Default.快捷传送四编号2;
        S_快捷传送四货币2.Value = Settings.Default.快捷传送四货币2;
        S_快捷传送四等级2.Value = Settings.Default.快捷传送四等级2;
        S_快捷传送五编号2.Value = Settings.Default.快捷传送五编号2;
        S_快捷传送五货币2.Value = Settings.Default.快捷传送五货币2;
        S_快捷传送五等级2.Value = Settings.Default.快捷传送五等级2;
        S_快捷传送六编号2.Value = Settings.Default.快捷传送六编号2;
        S_快捷传送六货币2.Value = Settings.Default.快捷传送六货币2;
        S_快捷传送六等级2.Value = Settings.Default.快捷传送六等级2;
        S_武斗场次数限制.Value = Settings.Default.武斗场次数限制;
        S_AutoPickUpInventorySpace.Value = Settings.Default.AutoPickUpInventorySpace;
        S_BOSS刷新提示开关.Value = Settings.Default.BOSS刷新提示开关;
        S_自动整理背包计时.Value = Settings.Default.自动整理背包计时;
        S_自动整理背包开关.Value = Settings.Default.自动整理背包开关;
        S_称号叠加开关.Value = Settings.Default.称号叠加开关;
        S_称号叠加模块一.Value = Settings.Default.称号叠加模块一;
        S_称号叠加模块二.Value = Settings.Default.称号叠加模块二;
        S_称号叠加模块三.Value = Settings.Default.称号叠加模块三;
        S_称号叠加模块四.Value = Settings.Default.称号叠加模块四;
        S_称号叠加模块五.Value = Settings.Default.称号叠加模块五;
        S_称号叠加模块六.Value = Settings.Default.称号叠加模块六;
        S_称号叠加模块七.Value = Settings.Default.称号叠加模块七;
        S_称号叠加模块八.Value = Settings.Default.称号叠加模块八;
        S_沙城传送货币开关.Value = Settings.Default.沙城传送货币开关;
        S_沙城快捷货币一.Value = Settings.Default.沙城快捷货币一;
        S_沙城快捷等级一.Value = Settings.Default.沙城快捷等级一;
        S_沙城快捷货币二.Value = Settings.Default.沙城快捷货币二;
        S_沙城快捷等级二.Value = Settings.Default.沙城快捷等级二;
        S_沙城快捷货币三.Value = Settings.Default.沙城快捷货币三;
        S_沙城快捷等级三.Value = Settings.Default.沙城快捷等级三;
        S_沙城快捷货币四.Value = Settings.Default.沙城快捷货币四;
        S_沙城快捷等级四.Value = Settings.Default.沙城快捷等级四;
        S_未知暗点副本价格.Value = Settings.Default.未知暗点副本价格;
        S_未知暗点副本等级.Value = Settings.Default.未知暗点副本等级;
        S_未知暗点二层价格.Value = Settings.Default.未知暗点二层价格;
        S_未知暗点二层等级.Value = Settings.Default.未知暗点二层等级;
        S_幽冥海副本价格.Value = Settings.Default.幽冥海副本价格;
        S_幽冥海副本等级.Value = Settings.Default.幽冥海副本等级;
        S_猎魔暗使称号一.Value = Settings.Default.猎魔暗使称号一;
        S_猎魔暗使材料一.Value = Settings.Default.猎魔暗使材料一;
        S_猎魔暗使数量一.Value = Settings.Default.猎魔暗使数量一;
        S_猎魔暗使称号二.Value = Settings.Default.猎魔暗使称号二;
        S_猎魔暗使材料二.Value = Settings.Default.猎魔暗使材料二;
        S_猎魔暗使数量二.Value = Settings.Default.猎魔暗使数量二;
        S_猎魔暗使称号三.Value = Settings.Default.猎魔暗使称号三;
        S_猎魔暗使材料三.Value = Settings.Default.猎魔暗使材料三;
        S_猎魔暗使数量三.Value = Settings.Default.猎魔暗使数量三;
        S_猎魔暗使称号四.Value = Settings.Default.猎魔暗使称号四;
        S_猎魔暗使材料四.Value = Settings.Default.猎魔暗使材料四;
        S_猎魔暗使数量四.Value = Settings.Default.猎魔暗使数量四;
        S_猎魔暗使称号五.Value = Settings.Default.猎魔暗使称号五;
        S_猎魔暗使材料五.Value = Settings.Default.猎魔暗使材料五;
        S_猎魔暗使数量五.Value = Settings.Default.猎魔暗使数量五;
        S_猎魔暗使称号六.Value = Settings.Default.猎魔暗使称号六;
        S_猎魔暗使材料六.Value = Settings.Default.猎魔暗使材料六;
        S_猎魔暗使数量六.Value = Settings.Default.猎魔暗使数量六;
        S_怪物掉落广播开关.Value = Settings.Default.怪物掉落广播开关;
        S_怪物掉落窗口开关.Value = Settings.Default.怪物掉落窗口开关;
        S_珍宝阁提示开关.Value = Settings.Default.珍宝阁提示开关;
        S_城主分解物品一.Text = Settings.Default.城主分解物品一;
        S_城主分解物品二.Text = Settings.Default.城主分解物品二;
        S_城主分解物品三.Text = Settings.Default.城主分解物品三;
        S_城主分解物品四.Text = Settings.Default.城主分解物品四;
        S_城主分解几率一.Value = Settings.Default.城主分解几率一;
        S_城主分解几率二.Value = Settings.Default.城主分解几率二;
        S_城主分解几率三.Value = Settings.Default.城主分解几率三;
        S_城主分解几率四.Value = Settings.Default.城主分解几率四;
        S_城主分解数量一.Value = Settings.Default.城主分解数量一;
        S_城主分解数量二.Value = Settings.Default.城主分解数量二;
        S_城主分解数量三.Value = Settings.Default.城主分解数量三;
        S_城主分解数量四.Value = Settings.Default.城主分解数量四;
        S_城主分解开关.Value = Settings.Default.城主分解开关;
        S_星王分解物品一.Text = Settings.Default.星王分解物品一;
        S_星王分解物品二.Text = Settings.Default.星王分解物品二;
        S_星王分解物品三.Text = Settings.Default.星王分解物品三;
        S_星王分解物品四.Text = Settings.Default.星王分解物品四;
        S_星王分解几率一.Value = Settings.Default.星王分解几率一;
        S_星王分解几率二.Value = Settings.Default.星王分解几率二;
        S_星王分解几率三.Value = Settings.Default.星王分解几率三;
        S_星王分解几率四.Value = Settings.Default.星王分解几率四;
        S_星王分解数量一.Value = Settings.Default.星王分解数量一;
        S_星王分解数量二.Value = Settings.Default.星王分解数量二;
        S_星王分解数量三.Value = Settings.Default.星王分解数量三;
        S_星王分解数量四.Value = Settings.Default.星王分解数量四;
        S_星王分解开关.Value = Settings.Default.星王分解开关;
        S_苍月分解物品一.Text = Settings.Default.苍月分解物品一;
        S_苍月分解物品二.Text = Settings.Default.苍月分解物品二;
        S_苍月分解物品三.Text = Settings.Default.苍月分解物品三;
        S_苍月分解物品四.Text = Settings.Default.苍月分解物品四;
        S_苍月分解几率一.Value = Settings.Default.苍月分解几率一;
        S_苍月分解几率二.Value = Settings.Default.苍月分解几率二;
        S_苍月分解几率三.Value = Settings.Default.苍月分解几率三;
        S_苍月分解几率四.Value = Settings.Default.苍月分解几率四;
        S_苍月分解数量一.Value = Settings.Default.苍月分解数量一;
        S_苍月分解数量二.Value = Settings.Default.苍月分解数量二;
        S_苍月分解数量三.Value = Settings.Default.苍月分解数量三;
        S_苍月分解数量四.Value = Settings.Default.苍月分解数量四;
        S_苍月分解开关.Value = Settings.Default.苍月分解开关;
        S_魔龙分解物品一.Text = Settings.Default.魔龙分解物品一;
        S_魔龙分解物品二.Text = Settings.Default.魔龙分解物品二;
        S_魔龙分解物品三.Text = Settings.Default.魔龙分解物品三;
        S_魔龙分解物品四.Text = Settings.Default.魔龙分解物品四;
        S_魔龙分解几率一.Value = Settings.Default.魔龙分解几率一;
        S_魔龙分解几率二.Value = Settings.Default.魔龙分解几率二;
        S_魔龙分解几率三.Value = Settings.Default.魔龙分解几率三;
        S_魔龙分解几率四.Value = Settings.Default.魔龙分解几率四;
        S_魔龙分解数量一.Value = Settings.Default.魔龙分解数量一;
        S_魔龙分解数量二.Value = Settings.Default.魔龙分解数量二;
        S_魔龙分解数量三.Value = Settings.Default.魔龙分解数量三;
        S_魔龙分解数量四.Value = Settings.Default.魔龙分解数量四;
        S_魔龙分解开关.Value = Settings.Default.魔龙分解开关;
        S_赤月分解物品一.Text = Settings.Default.赤月分解物品一;
        S_赤月分解物品二.Text = Settings.Default.赤月分解物品二;
        S_赤月分解物品三.Text = Settings.Default.赤月分解物品三;
        S_赤月分解物品四.Text = Settings.Default.赤月分解物品四;
        S_赤月分解几率一.Value = Settings.Default.赤月分解几率一;
        S_赤月分解几率二.Value = Settings.Default.赤月分解几率二;
        S_赤月分解几率三.Value = Settings.Default.赤月分解几率三;
        S_赤月分解几率四.Value = Settings.Default.赤月分解几率四;
        S_赤月分解数量一.Value = Settings.Default.赤月分解数量一;
        S_赤月分解数量二.Value = Settings.Default.赤月分解数量二;
        S_赤月分解数量三.Value = Settings.Default.赤月分解数量三;
        S_赤月分解数量四.Value = Settings.Default.赤月分解数量四;
        S_赤月分解开关.Value = Settings.Default.赤月分解开关;
        S_祖玛分解物品一.Text = Settings.Default.祖玛分解物品一;
        S_祖玛分解物品二.Text = Settings.Default.祖玛分解物品二;
        S_祖玛分解物品三.Text = Settings.Default.祖玛分解物品三;
        S_祖玛分解物品四.Text = Settings.Default.祖玛分解物品四;
        S_祖玛分解几率一.Value = Settings.Default.祖玛分解几率一;
        S_祖玛分解几率二.Value = Settings.Default.祖玛分解几率二;
        S_祖玛分解几率三.Value = Settings.Default.祖玛分解几率三;
        S_祖玛分解几率四.Value = Settings.Default.祖玛分解几率四;
        S_祖玛分解数量一.Value = Settings.Default.祖玛分解数量一;
        S_祖玛分解数量二.Value = Settings.Default.祖玛分解数量二;
        S_祖玛分解数量三.Value = Settings.Default.祖玛分解数量三;
        S_祖玛分解数量四.Value = Settings.Default.祖玛分解数量四;
        S_祖玛分解开关.Value = Settings.Default.祖玛分解开关;
        S_BOSS卷轴怪物一.Text = Settings.Default.BOSS卷轴怪物一;
        S_BOSS卷轴怪物二.Text = Settings.Default.BOSS卷轴怪物二;
        S_BOSS卷轴怪物三.Text = Settings.Default.BOSS卷轴怪物三;
        S_BOSS卷轴怪物四.Text = Settings.Default.BOSS卷轴怪物四;
        S_BOSS卷轴怪物五.Text = Settings.Default.BOSS卷轴怪物五;
        S_BOSS卷轴怪物六.Text = Settings.Default.BOSS卷轴怪物六;
        S_BOSS卷轴怪物七.Text = Settings.Default.BOSS卷轴怪物七;
        S_BOSS卷轴怪物八.Text = Settings.Default.BOSS卷轴怪物八;
        S_BOSS卷轴怪物九.Text = Settings.Default.BOSS卷轴怪物九;
        S_BOSS卷轴怪物十.Text = Settings.Default.BOSS卷轴怪物十;
        S_BOSS卷轴怪物11.Text = Settings.Default.BOSS卷轴怪物11;
        S_BOSS卷轴怪物12.Text = Settings.Default.BOSS卷轴怪物12;
        S_BOSS卷轴怪物13.Text = Settings.Default.BOSS卷轴怪物13;
        S_BOSS卷轴怪物14.Text = Settings.Default.BOSS卷轴怪物14;
        S_BOSS卷轴怪物15.Text = Settings.Default.BOSS卷轴怪物15;
        S_BOSS卷轴怪物16.Text = Settings.Default.BOSS卷轴怪物16;
        S_BOSS卷轴地图编号.Value = Settings.Default.BOSS卷轴地图编号;
        S_BOSS卷轴地图开关.Value = Settings.Default.BOSS卷轴地图开关;
        S_沙巴克重置系统.Value = Settings.Default.沙巴克重置系统;
        S_资源包开关.Value = Settings.Default.资源包开关;
        S_StartingLevel.Value = Settings.Default.StartingLevel;
        S_MaxUserConnections.Value = Settings.Default.MaxUserConnections;
        S_掉落贵重物品颜色.Value = Settings.Default.掉落贵重物品颜色;
        S_掉落沃玛物品颜色.Value = Settings.Default.掉落沃玛物品颜色;
        S_掉落祖玛物品颜色.Value = Settings.Default.掉落祖玛物品颜色;
        S_掉落赤月物品颜色.Value = Settings.Default.掉落赤月物品颜色;
        S_掉落魔龙物品颜色.Value = Settings.Default.掉落魔龙物品颜色;
        S_掉落苍月物品颜色.Value = Settings.Default.掉落苍月物品颜色;
        S_掉落星王物品颜色.Value = Settings.Default.掉落星王物品颜色;
        S_掉落城主物品颜色.Value = Settings.Default.掉落城主物品颜色;
        S_掉落书籍物品颜色.Value = Settings.Default.掉落书籍物品颜色;
        S_DropPlayerNameColor.Value = Settings.Default.DropPlayerNameColor;
        S_狂暴击杀玩家颜色.Value = Settings.Default.狂暴击杀玩家颜色;
        S_狂暴被杀玩家颜色.Value = Settings.Default.狂暴被杀玩家颜色;
        S_祖玛战装备佩戴数量.Value = Settings.Default.祖玛战装备佩戴数量;
        S_祖玛法装备佩戴数量.Value = Settings.Default.祖玛法装备佩戴数量;
        S_祖玛道装备佩戴数量.Value = Settings.Default.祖玛道装备佩戴数量;
        S_祖玛刺装备佩戴数量.Value = Settings.Default.祖玛刺装备佩戴数量;
        S_祖玛枪装备佩戴数量.Value = Settings.Default.祖玛枪装备佩戴数量;
        S_祖玛弓装备佩戴数量.Value = Settings.Default.祖玛弓装备佩戴数量;
        S_赤月战装备佩戴数量.Value = Settings.Default.赤月战装备佩戴数量;
        S_赤月法装备佩戴数量.Value = Settings.Default.赤月法装备佩戴数量;
        S_赤月道装备佩戴数量.Value = Settings.Default.赤月道装备佩戴数量;
        S_赤月刺装备佩戴数量.Value = Settings.Default.赤月刺装备佩戴数量;
        S_赤月枪装备佩戴数量.Value = Settings.Default.赤月枪装备佩戴数量;
        S_赤月弓装备佩戴数量.Value = Settings.Default.赤月弓装备佩戴数量;
        S_魔龙战装备佩戴数量.Value = Settings.Default.魔龙战装备佩戴数量;
        S_魔龙法装备佩戴数量.Value = Settings.Default.魔龙法装备佩戴数量;
        S_魔龙道装备佩戴数量.Value = Settings.Default.魔龙道装备佩戴数量;
        S_魔龙刺装备佩戴数量.Value = Settings.Default.魔龙刺装备佩戴数量;
        S_魔龙枪装备佩戴数量.Value = Settings.Default.魔龙枪装备佩戴数量;
        S_魔龙弓装备佩戴数量.Value = Settings.Default.魔龙弓装备佩戴数量;
        S_苍月战装备佩戴数量.Value = Settings.Default.苍月战装备佩戴数量;
        S_苍月法装备佩戴数量.Value = Settings.Default.苍月法装备佩戴数量;
        S_苍月道装备佩戴数量.Value = Settings.Default.苍月道装备佩戴数量;
        S_苍月刺装备佩戴数量.Value = Settings.Default.苍月刺装备佩戴数量;
        S_苍月枪装备佩戴数量.Value = Settings.Default.苍月枪装备佩戴数量;
        S_苍月弓装备佩戴数量.Value = Settings.Default.苍月弓装备佩戴数量;
        S_星王战装备佩戴数量.Value = Settings.Default.星王战装备佩戴数量;
        S_星王法装备佩戴数量.Value = Settings.Default.星王法装备佩戴数量;
        S_星王道装备佩戴数量.Value = Settings.Default.星王道装备佩戴数量;
        S_星王刺装备佩戴数量.Value = Settings.Default.星王刺装备佩戴数量;
        S_星王枪装备佩戴数量.Value = Settings.Default.星王枪装备佩戴数量;
        S_星王弓装备佩戴数量.Value = Settings.Default.星王弓装备佩戴数量;
        S_特殊1战装备佩戴数量.Value = Settings.Default.特殊1战装备佩戴数量;
        S_特殊1法装备佩戴数量.Value = Settings.Default.特殊1法装备佩戴数量;
        S_特殊1道装备佩戴数量.Value = Settings.Default.特殊1道装备佩戴数量;
        S_特殊1刺装备佩戴数量.Value = Settings.Default.特殊1刺装备佩戴数量;
        S_特殊1枪装备佩戴数量.Value = Settings.Default.特殊1枪装备佩戴数量;
        S_特殊1弓装备佩戴数量.Value = Settings.Default.特殊1弓装备佩戴数量;
        S_特殊2战装备佩戴数量.Value = Settings.Default.特殊2战装备佩戴数量;
        S_特殊2法装备佩戴数量.Value = Settings.Default.特殊2法装备佩戴数量;
        S_特殊2道装备佩戴数量.Value = Settings.Default.特殊2道装备佩戴数量;
        S_特殊2刺装备佩戴数量.Value = Settings.Default.特殊2刺装备佩戴数量;
        S_特殊2枪装备佩戴数量.Value = Settings.Default.特殊2枪装备佩戴数量;
        S_特殊2弓装备佩戴数量.Value = Settings.Default.特殊2弓装备佩戴数量;
        S_特殊3战装备佩戴数量.Value = Settings.Default.特殊3战装备佩戴数量;
        S_特殊3法装备佩戴数量.Value = Settings.Default.特殊3法装备佩戴数量;
        S_特殊3道装备佩戴数量.Value = Settings.Default.特殊3道装备佩戴数量;
        S_特殊3刺装备佩戴数量.Value = Settings.Default.特殊3刺装备佩戴数量;
        S_特殊3枪装备佩戴数量.Value = Settings.Default.特殊3枪装备佩戴数量;
        S_特殊3弓装备佩戴数量.Value = Settings.Default.特殊3弓装备佩戴数量;
        S_每周特惠二物品5.Value = Settings.Default.每周特惠二物品5;
        S_每周特惠二物品4.Value = Settings.Default.每周特惠二物品4;
        S_每周特惠二物品3.Value = Settings.Default.每周特惠二物品3;
        S_每周特惠二物品2.Value = Settings.Default.每周特惠二物品2;
        S_每周特惠二物品1.Value = Settings.Default.每周特惠二物品1;
        S_每周特惠一物品1.Value = Settings.Default.每周特惠一物品1;
        S_每周特惠一物品2.Value = Settings.Default.每周特惠一物品2;
        S_每周特惠一物品3.Value = Settings.Default.每周特惠一物品3;
        S_每周特惠一物品4.Value = Settings.Default.每周特惠一物品4;
        S_每周特惠一物品5.Value = Settings.Default.每周特惠一物品5;
        S_新手出售货币值.Value = Settings.Default.新手出售货币值;
        S_挂机称号选项.Value = Settings.Default.挂机称号选项;
        S_分解称号选项.Value = Settings.Default.分解称号选项;
        S_法阵卡BUG清理.Value = Settings.Default.法阵卡BUG清理;
        S_随机宝箱一物品1.Value = Settings.Default.随机宝箱一物品1;
        S_随机宝箱一物品2.Value = Settings.Default.随机宝箱一物品2;
        S_随机宝箱一物品3.Value = Settings.Default.随机宝箱一物品3;
        S_随机宝箱一物品4.Value = Settings.Default.随机宝箱一物品4;
        S_随机宝箱一物品5.Value = Settings.Default.随机宝箱一物品5;
        S_随机宝箱一物品6.Value = Settings.Default.随机宝箱一物品6;
        S_随机宝箱一物品7.Value = Settings.Default.随机宝箱一物品7;
        S_随机宝箱一物品8.Value = Settings.Default.随机宝箱一物品8;
        S_随机宝箱一几率1.Value = Settings.Default.随机宝箱一几率1;
        S_随机宝箱一几率2.Value = Settings.Default.随机宝箱一几率2;
        S_随机宝箱一几率3.Value = Settings.Default.随机宝箱一几率3;
        S_随机宝箱一几率4.Value = Settings.Default.随机宝箱一几率4;
        S_随机宝箱一几率5.Value = Settings.Default.随机宝箱一几率5;
        S_随机宝箱一几率6.Value = Settings.Default.随机宝箱一几率6;
        S_随机宝箱一几率7.Value = Settings.Default.随机宝箱一几率7;
        S_随机宝箱一几率8.Value = Settings.Default.随机宝箱一几率8;
        S_随机宝箱二物品1.Value = Settings.Default.随机宝箱二物品1;
        S_随机宝箱二物品2.Value = Settings.Default.随机宝箱二物品2;
        S_随机宝箱二物品3.Value = Settings.Default.随机宝箱二物品3;
        S_随机宝箱二物品4.Value = Settings.Default.随机宝箱二物品4;
        S_随机宝箱二物品5.Value = Settings.Default.随机宝箱二物品5;
        S_随机宝箱二物品6.Value = Settings.Default.随机宝箱二物品6;
        S_随机宝箱二物品7.Value = Settings.Default.随机宝箱二物品7;
        S_随机宝箱二物品8.Value = Settings.Default.随机宝箱二物品8;
        S_随机宝箱二几率1.Value = Settings.Default.随机宝箱二几率1;
        S_随机宝箱二几率2.Value = Settings.Default.随机宝箱二几率2;
        S_随机宝箱二几率3.Value = Settings.Default.随机宝箱二几率3;
        S_随机宝箱二几率4.Value = Settings.Default.随机宝箱二几率4;
        S_随机宝箱二几率5.Value = Settings.Default.随机宝箱二几率5;
        S_随机宝箱二几率6.Value = Settings.Default.随机宝箱二几率6;
        S_随机宝箱二几率7.Value = Settings.Default.随机宝箱二几率7;
        S_随机宝箱二几率8.Value = Settings.Default.随机宝箱二几率8;
        S_随机宝箱三物品1.Value = Settings.Default.随机宝箱三物品1;
        S_随机宝箱三物品2.Value = Settings.Default.随机宝箱三物品2;
        S_随机宝箱三物品3.Value = Settings.Default.随机宝箱三物品3;
        S_随机宝箱三物品4.Value = Settings.Default.随机宝箱三物品4;
        S_随机宝箱三物品5.Value = Settings.Default.随机宝箱三物品5;
        S_随机宝箱三物品6.Value = Settings.Default.随机宝箱三物品6;
        S_随机宝箱三物品7.Value = Settings.Default.随机宝箱三物品7;
        S_随机宝箱三物品8.Value = Settings.Default.随机宝箱三物品8;
        S_随机宝箱三几率1.Value = Settings.Default.随机宝箱三几率1;
        S_随机宝箱三几率2.Value = Settings.Default.随机宝箱三几率2;
        S_随机宝箱三几率3.Value = Settings.Default.随机宝箱三几率3;
        S_随机宝箱三几率4.Value = Settings.Default.随机宝箱三几率4;
        S_随机宝箱三几率5.Value = Settings.Default.随机宝箱三几率5;
        S_随机宝箱三几率6.Value = Settings.Default.随机宝箱三几率6;
        S_随机宝箱三几率7.Value = Settings.Default.随机宝箱三几率7;
        S_随机宝箱三几率8.Value = Settings.Default.随机宝箱三几率8;
        S_随机宝箱一数量1.Value = Settings.Default.随机宝箱一数量1;
        S_随机宝箱一数量2.Value = Settings.Default.随机宝箱一数量2;
        S_随机宝箱一数量3.Value = Settings.Default.随机宝箱一数量3;
        S_随机宝箱一数量4.Value = Settings.Default.随机宝箱一数量4;
        S_随机宝箱一数量5.Value = Settings.Default.随机宝箱一数量5;
        S_随机宝箱一数量6.Value = Settings.Default.随机宝箱一数量6;
        S_随机宝箱一数量7.Value = Settings.Default.随机宝箱一数量7;
        S_随机宝箱一数量8.Value = Settings.Default.随机宝箱一数量8;
        S_随机宝箱二数量1.Value = Settings.Default.随机宝箱二数量1;
        S_随机宝箱二数量2.Value = Settings.Default.随机宝箱二数量2;
        S_随机宝箱二数量3.Value = Settings.Default.随机宝箱二数量3;
        S_随机宝箱二数量4.Value = Settings.Default.随机宝箱二数量4;
        S_随机宝箱二数量5.Value = Settings.Default.随机宝箱二数量5;
        S_随机宝箱二数量6.Value = Settings.Default.随机宝箱二数量6;
        S_随机宝箱二数量7.Value = Settings.Default.随机宝箱二数量7;
        S_随机宝箱二数量8.Value = Settings.Default.随机宝箱二数量8;
        S_随机宝箱三数量1.Value = Settings.Default.随机宝箱三数量1;
        S_随机宝箱三数量2.Value = Settings.Default.随机宝箱三数量2;
        S_随机宝箱三数量3.Value = Settings.Default.随机宝箱三数量3;
        S_随机宝箱三数量4.Value = Settings.Default.随机宝箱三数量4;
        S_随机宝箱三数量5.Value = Settings.Default.随机宝箱三数量5;
        S_随机宝箱三数量6.Value = Settings.Default.随机宝箱三数量6;
        S_随机宝箱三数量7.Value = Settings.Default.随机宝箱三数量7;
        S_随机宝箱三数量8.Value = Settings.Default.随机宝箱三数量8;
        S_沙城地图保护.Value = Settings.Default.沙城地图保护;
        S_NoobProtectionLevel.Value = Settings.Default.NoobProtectionLevel;
        S_新手地图保护1.Value = Settings.Default.新手地图保护1;
        S_新手地图保护2.Value = Settings.Default.新手地图保护2;
        S_新手地图保护3.Value = Settings.Default.新手地图保护3;
        S_新手地图保护4.Value = Settings.Default.新手地图保护4;
        S_新手地图保护5.Value = Settings.Default.新手地图保护5;
        S_新手地图保护6.Value = Settings.Default.新手地图保护6;
        S_新手地图保护7.Value = Settings.Default.新手地图保护7;
        S_新手地图保护8.Value = Settings.Default.新手地图保护8;
        S_新手地图保护9.Value = Settings.Default.新手地图保护9;
        S_新手地图保护10.Value = Settings.Default.新手地图保护10;
        S_沙巴克停止开关.Value = Settings.Default.沙巴克停止开关;
        S_沙巴克城主称号.Value = Settings.Default.沙巴克城主称号;
        S_沙巴克成员称号.Value = Settings.Default.沙巴克成员称号;
        S_沙巴克称号领取开关.Value = Settings.Default.沙巴克称号领取开关;
        S_通用1装备佩戴数量.Value = Settings.Default.通用1装备佩戴数量;
        S_通用2装备佩戴数量.Value = Settings.Default.通用2装备佩戴数量;
        S_通用3装备佩戴数量.Value = Settings.Default.通用3装备佩戴数量;
        S_通用4装备佩戴数量.Value = Settings.Default.通用4装备佩戴数量;
        S_通用5装备佩戴数量.Value = Settings.Default.通用5装备佩戴数量;
        S_通用6装备佩戴数量.Value = Settings.Default.通用6装备佩戴数量;
        S_重置屠魔副本时间.Value = Settings.Default.重置屠魔副本时间;
        S_屠魔令回收数量.Value = Settings.Default.重置屠魔副本时间;
        S_新手上线赠送开关.Value = Settings.Default.新手上线赠送开关;
        S_新手上线赠送物品1.Value = Settings.Default.新手上线赠送物品1;
        S_新手上线赠送物品2.Value = Settings.Default.新手上线赠送物品2;
        S_新手上线赠送物品3.Value = Settings.Default.新手上线赠送物品3;
        S_新手上线赠送物品4.Value = Settings.Default.新手上线赠送物品4;
        S_新手上线赠送物品5.Value = Settings.Default.新手上线赠送物品5;
        S_新手上线赠送物品6.Value = Settings.Default.新手上线赠送物品6;
        S_新手上线赠送称号1.Value = Settings.Default.新手上线赠送称号1;
        S_元宝袋新创数量1.Value = Settings.Default.元宝袋新创数量1;
        S_元宝袋新创数量2.Value = Settings.Default.元宝袋新创数量2;
        S_元宝袋新创数量3.Value = Settings.Default.元宝袋新创数量3;
        S_元宝袋新创数量4.Value = Settings.Default.元宝袋新创数量4;
        S_元宝袋新创数量5.Value = Settings.Default.元宝袋新创数量5;
        S_初级赞助礼包1.Value = Settings.Default.初级赞助礼包1;
        S_初级赞助礼包2.Value = Settings.Default.初级赞助礼包2;
        S_初级赞助礼包3.Value = Settings.Default.初级赞助礼包3;
        S_初级赞助礼包4.Value = Settings.Default.初级赞助礼包4;
        S_初级赞助礼包5.Value = Settings.Default.初级赞助礼包5;
        S_初级赞助礼包6.Value = Settings.Default.初级赞助礼包6;
        S_初级赞助礼包7.Value = Settings.Default.初级赞助礼包7;
        S_初级赞助礼包8.Value = Settings.Default.初级赞助礼包8;
        S_初级赞助称号1.Value = Settings.Default.初级赞助称号1;
        S_中级赞助礼包1.Value = Settings.Default.中级赞助礼包1;
        S_中级赞助礼包2.Value = Settings.Default.中级赞助礼包2;
        S_中级赞助礼包3.Value = Settings.Default.中级赞助礼包3;
        S_中级赞助礼包4.Value = Settings.Default.中级赞助礼包4;
        S_中级赞助礼包5.Value = Settings.Default.中级赞助礼包5;
        S_中级赞助礼包6.Value = Settings.Default.中级赞助礼包6;
        S_中级赞助礼包7.Value = Settings.Default.中级赞助礼包7;
        S_中级赞助礼包8.Value = Settings.Default.中级赞助礼包8;
        S_中级赞助称号1.Value = Settings.Default.中级赞助称号1;
        S_高级赞助礼包1.Value = Settings.Default.高级赞助礼包1;
        S_高级赞助礼包2.Value = Settings.Default.高级赞助礼包2;
        S_高级赞助礼包3.Value = Settings.Default.高级赞助礼包3;
        S_高级赞助礼包4.Value = Settings.Default.高级赞助礼包4;
        S_高级赞助礼包5.Value = Settings.Default.高级赞助礼包5;
        S_高级赞助礼包6.Value = Settings.Default.高级赞助礼包6;
        S_高级赞助礼包7.Value = Settings.Default.高级赞助礼包7;
        S_高级赞助礼包8.Value = Settings.Default.高级赞助礼包8;
        S_高级赞助称号1.Value = Settings.Default.高级赞助称号1;
        S_平台开关模式.Value = Settings.Default.平台开关模式;
        S_平台元宝充值模块.Value = Settings.Default.平台元宝充值模块;
        S_九层妖塔数量1.Value = Settings.Default.九层妖塔数量1;
        S_九层妖塔数量2.Value = Settings.Default.九层妖塔数量2;
        S_九层妖塔数量3.Value = Settings.Default.九层妖塔数量3;
        S_九层妖塔数量4.Value = Settings.Default.九层妖塔数量4;
        S_九层妖塔数量5.Value = Settings.Default.九层妖塔数量5;
        S_九层妖塔数量6.Value = Settings.Default.九层妖塔数量6;
        S_九层妖塔数量7.Value = Settings.Default.九层妖塔数量7;
        S_九层妖塔数量8.Value = Settings.Default.九层妖塔数量8;
        S_九层妖塔数量9.Value = Settings.Default.九层妖塔数量9;
        S_九层妖塔副本次数.Value = Settings.Default.九层妖塔副本次数;
        S_九层妖塔副本等级.Value = Settings.Default.九层妖塔副本等级;
        S_九层妖塔副本物品.Value = Settings.Default.九层妖塔副本物品;
        S_九层妖塔副本数量.Value = Settings.Default.九层妖塔副本数量;
        S_九层妖塔副本时间小.Value = Settings.Default.九层妖塔副本时间小;
        S_九层妖塔副本时间大.Value = Settings.Default.九层妖塔副本时间大;
        S_九层妖塔BOSS1.Text = Settings.Default.九层妖塔BOSS1;
        S_九层妖塔BOSS2.Text = Settings.Default.九层妖塔BOSS2;
        S_九层妖塔BOSS3.Text = Settings.Default.九层妖塔BOSS3;
        S_九层妖塔BOSS4.Text = Settings.Default.九层妖塔BOSS4;
        S_九层妖塔BOSS5.Text = Settings.Default.九层妖塔BOSS5;
        S_九层妖塔BOSS6.Text = Settings.Default.九层妖塔BOSS6;
        S_九层妖塔BOSS7.Text = Settings.Default.九层妖塔BOSS7;
        S_九层妖塔BOSS8.Text = Settings.Default.九层妖塔BOSS8;
        S_九层妖塔BOSS9.Text = Settings.Default.九层妖塔BOSS9;
        S_九层妖塔精英1.Text = Settings.Default.九层妖塔精英1;
        S_九层妖塔精英2.Text = Settings.Default.九层妖塔精英2;
        S_九层妖塔精英3.Text = Settings.Default.九层妖塔精英3;
        S_九层妖塔精英4.Text = Settings.Default.九层妖塔精英4;
        S_九层妖塔精英5.Text = Settings.Default.九层妖塔精英5;
        S_九层妖塔精英6.Text = Settings.Default.九层妖塔精英6;
        S_九层妖塔精英7.Text = Settings.Default.九层妖塔精英7;
        S_九层妖塔精英8.Text = Settings.Default.九层妖塔精英8;
        S_九层妖塔精英9.Text = Settings.Default.九层妖塔精英9;
        S_AutoBattleLevel.Value = Settings.Default.AutoBattleLevel;
        S_禁止背包铭文洗练.Value = Settings.Default.禁止背包铭文洗练;
        S_沙巴克禁止随机.Value = Settings.Default.沙巴克禁止随机;
        S_冥想丹自定义经验.Value = Settings.Default.冥想丹自定义经验;
        S_沙巴克爆装备开关.Value = Settings.Default.沙巴克爆装备开关;
        S_铭文战士1挡1次数.Value = Settings.Default.铭文战士1挡1次数;
        S_铭文战士1挡2次数.Value = Settings.Default.铭文战士1挡2次数;
        S_铭文战士1挡3次数.Value = Settings.Default.铭文战士1挡3次数;
        S_铭文战士1挡1概率.Value = Settings.Default.铭文战士1挡1概率;
        S_铭文战士1挡2概率.Value = Settings.Default.铭文战士1挡2概率;
        S_铭文战士1挡3概率.Value = Settings.Default.铭文战士1挡3概率;
        S_铭文战士1挡技能编号.Value = Settings.Default.铭文战士1挡技能编号;
        S_铭文战士1挡技能铭文.Value = Settings.Default.铭文战士1挡技能铭文;
        S_铭文战士2挡1次数.Value = Settings.Default.铭文战士2挡1次数;
        S_铭文战士2挡2次数.Value = Settings.Default.铭文战士2挡2次数;
        S_铭文战士2挡3次数.Value = Settings.Default.铭文战士2挡3次数;
        S_铭文战士2挡1概率.Value = Settings.Default.铭文战士2挡1概率;
        S_铭文战士2挡2概率.Value = Settings.Default.铭文战士2挡2概率;
        S_铭文战士2挡3概率.Value = Settings.Default.铭文战士2挡3概率;
        S_铭文战士2挡技能编号.Value = Settings.Default.铭文战士2挡技能编号;
        S_铭文战士2挡技能铭文.Value = Settings.Default.铭文战士2挡技能铭文;
        S_铭文战士3挡1次数.Value = Settings.Default.铭文战士3挡1次数;
        S_铭文战士3挡2次数.Value = Settings.Default.铭文战士3挡2次数;
        S_铭文战士3挡3次数.Value = Settings.Default.铭文战士3挡3次数;
        S_铭文战士3挡1概率.Value = Settings.Default.铭文战士3挡1概率;
        S_铭文战士3挡2概率.Value = Settings.Default.铭文战士3挡2概率;
        S_铭文战士3挡3概率.Value = Settings.Default.铭文战士3挡3概率;
        S_铭文战士3挡技能编号.Value = Settings.Default.铭文战士3挡技能编号;
        S_铭文战士3挡技能铭文.Value = Settings.Default.铭文战士3挡技能铭文;
        S_铭文法师1挡1次数.Value = Settings.Default.铭文法师1挡1次数;
        S_铭文法师1挡2次数.Value = Settings.Default.铭文法师1挡2次数;
        S_铭文法师1挡3次数.Value = Settings.Default.铭文法师1挡3次数;
        S_铭文法师1挡1概率.Value = Settings.Default.铭文法师1挡1概率;
        S_铭文法师1挡2概率.Value = Settings.Default.铭文法师1挡2概率;
        S_铭文法师1挡3概率.Value = Settings.Default.铭文法师1挡3概率;
        S_铭文法师1挡技能编号.Value = Settings.Default.铭文法师1挡技能编号;
        S_铭文法师1挡技能铭文.Value = Settings.Default.铭文法师1挡技能铭文;
        S_铭文法师2挡1次数.Value = Settings.Default.铭文法师2挡1次数;
        S_铭文法师2挡2次数.Value = Settings.Default.铭文法师2挡2次数;
        S_铭文法师2挡3次数.Value = Settings.Default.铭文法师2挡3次数;
        S_铭文法师2挡1概率.Value = Settings.Default.铭文法师2挡1概率;
        S_铭文法师2挡2概率.Value = Settings.Default.铭文法师2挡2概率;
        S_铭文法师2挡3概率.Value = Settings.Default.铭文法师2挡3概率;
        S_铭文法师2挡技能编号.Value = Settings.Default.铭文法师2挡技能编号;
        S_铭文法师2挡技能铭文.Value = Settings.Default.铭文法师2挡技能铭文;
        S_铭文法师3挡1次数.Value = Settings.Default.铭文法师3挡1次数;
        S_铭文法师3挡2次数.Value = Settings.Default.铭文法师3挡2次数;
        S_铭文法师3挡3次数.Value = Settings.Default.铭文法师3挡3次数;
        S_铭文法师3挡1概率.Value = Settings.Default.铭文法师3挡1概率;
        S_铭文法师3挡2概率.Value = Settings.Default.铭文法师3挡2概率;
        S_铭文法师3挡3概率.Value = Settings.Default.铭文法师3挡3概率;
        S_铭文法师3挡技能编号.Value = Settings.Default.铭文法师3挡技能编号;
        S_铭文法师3挡技能铭文.Value = Settings.Default.铭文法师3挡技能铭文;
        S_铭文道士1挡1次数.Value = Settings.Default.铭文道士1挡1次数;
        S_铭文道士1挡2次数.Value = Settings.Default.铭文道士1挡2次数;
        S_铭文道士1挡3次数.Value = Settings.Default.铭文道士1挡3次数;
        S_铭文道士1挡1概率.Value = Settings.Default.铭文道士1挡1概率;
        S_铭文道士1挡2概率.Value = Settings.Default.铭文道士1挡2概率;
        S_铭文道士1挡3概率.Value = Settings.Default.铭文道士1挡3概率;
        S_铭文道士1挡技能编号.Value = Settings.Default.铭文道士1挡技能编号;
        S_铭文道士1挡技能铭文.Value = Settings.Default.铭文道士1挡技能铭文;
        S_铭文道士2挡1次数.Value = Settings.Default.铭文道士2挡1次数;
        S_铭文道士2挡2次数.Value = Settings.Default.铭文道士2挡2次数;
        S_铭文道士2挡3次数.Value = Settings.Default.铭文道士2挡3次数;
        S_铭文道士2挡1概率.Value = Settings.Default.铭文道士2挡1概率;
        S_铭文道士2挡2概率.Value = Settings.Default.铭文道士2挡2概率;
        S_铭文道士2挡3概率.Value = Settings.Default.铭文道士2挡3概率;
        S_铭文道士2挡技能编号.Value = Settings.Default.铭文道士2挡技能编号;
        S_铭文道士2挡技能铭文.Value = Settings.Default.铭文道士2挡技能铭文;
        S_铭文道士3挡1次数.Value = Settings.Default.铭文道士3挡1次数;
        S_铭文道士3挡2次数.Value = Settings.Default.铭文道士3挡2次数;
        S_铭文道士3挡3次数.Value = Settings.Default.铭文道士3挡3次数;
        S_铭文道士3挡1概率.Value = Settings.Default.铭文道士3挡1概率;
        S_铭文道士3挡2概率.Value = Settings.Default.铭文道士3挡2概率;
        S_铭文道士3挡3概率.Value = Settings.Default.铭文道士3挡3概率;
        S_铭文道士3挡技能编号.Value = Settings.Default.铭文道士3挡技能编号;
        S_铭文道士3挡技能铭文.Value = Settings.Default.铭文道士3挡技能铭文;
        S_铭文刺客1挡1次数.Value = Settings.Default.铭文刺客1挡1次数;
        S_铭文刺客1挡2次数.Value = Settings.Default.铭文刺客1挡2次数;
        S_铭文刺客1挡3次数.Value = Settings.Default.铭文刺客1挡3次数;
        S_铭文刺客1挡1概率.Value = Settings.Default.铭文刺客1挡1概率;
        S_铭文刺客1挡2概率.Value = Settings.Default.铭文刺客1挡2概率;
        S_铭文刺客1挡3概率.Value = Settings.Default.铭文刺客1挡3概率;
        S_铭文刺客1挡技能编号.Value = Settings.Default.铭文刺客1挡技能编号;
        S_铭文刺客1挡技能铭文.Value = Settings.Default.铭文刺客1挡技能铭文;
        S_铭文刺客2挡1次数.Value = Settings.Default.铭文刺客2挡1次数;
        S_铭文刺客2挡2次数.Value = Settings.Default.铭文刺客2挡2次数;
        S_铭文刺客2挡3次数.Value = Settings.Default.铭文刺客2挡3次数;
        S_铭文刺客2挡1概率.Value = Settings.Default.铭文刺客2挡1概率;
        S_铭文刺客2挡2概率.Value = Settings.Default.铭文刺客2挡2概率;
        S_铭文刺客2挡3概率.Value = Settings.Default.铭文刺客2挡3概率;
        S_铭文刺客2挡技能编号.Value = Settings.Default.铭文刺客2挡技能编号;
        S_铭文刺客2挡技能铭文.Value = Settings.Default.铭文刺客2挡技能铭文;
        S_铭文刺客3挡1次数.Value = Settings.Default.铭文刺客3挡1次数;
        S_铭文刺客3挡2次数.Value = Settings.Default.铭文刺客3挡2次数;
        S_铭文刺客3挡3次数.Value = Settings.Default.铭文刺客3挡3次数;
        S_铭文刺客3挡1概率.Value = Settings.Default.铭文刺客3挡1概率;
        S_铭文刺客3挡2概率.Value = Settings.Default.铭文刺客3挡2概率;
        S_铭文刺客3挡3概率.Value = Settings.Default.铭文刺客3挡3概率;
        S_铭文刺客3挡技能编号.Value = Settings.Default.铭文刺客3挡技能编号;
        S_铭文刺客3挡技能铭文.Value = Settings.Default.铭文刺客3挡技能铭文;
        S_铭文弓手1挡1次数.Value = Settings.Default.铭文弓手1挡1次数;
        S_铭文弓手1挡2次数.Value = Settings.Default.铭文弓手1挡2次数;
        S_铭文弓手1挡3次数.Value = Settings.Default.铭文弓手1挡3次数;
        S_铭文弓手1挡1概率.Value = Settings.Default.铭文弓手1挡1概率;
        S_铭文弓手1挡2概率.Value = Settings.Default.铭文弓手1挡2概率;
        S_铭文弓手1挡3概率.Value = Settings.Default.铭文弓手1挡3概率;
        S_铭文弓手1挡技能编号.Value = Settings.Default.铭文弓手1挡技能编号;
        S_铭文弓手1挡技能铭文.Value = Settings.Default.铭文弓手1挡技能铭文;
        S_铭文弓手2挡1次数.Value = Settings.Default.铭文弓手2挡1次数;
        S_铭文弓手2挡2次数.Value = Settings.Default.铭文弓手2挡2次数;
        S_铭文弓手2挡3次数.Value = Settings.Default.铭文弓手2挡3次数;
        S_铭文弓手2挡1概率.Value = Settings.Default.铭文弓手2挡1概率;
        S_铭文弓手2挡2概率.Value = Settings.Default.铭文弓手2挡2概率;
        S_铭文弓手2挡3概率.Value = Settings.Default.铭文弓手2挡3概率;
        S_铭文弓手2挡技能编号.Value = Settings.Default.铭文弓手2挡技能编号;
        S_铭文弓手2挡技能铭文.Value = Settings.Default.铭文弓手2挡技能铭文;
        S_铭文弓手3挡1次数.Value = Settings.Default.铭文弓手3挡1次数;
        S_铭文弓手3挡2次数.Value = Settings.Default.铭文弓手3挡2次数;
        S_铭文弓手3挡3次数.Value = Settings.Default.铭文弓手3挡3次数;
        S_铭文弓手3挡1概率.Value = Settings.Default.铭文弓手3挡1概率;
        S_铭文弓手3挡2概率.Value = Settings.Default.铭文弓手3挡2概率;
        S_铭文弓手3挡3概率.Value = Settings.Default.铭文弓手3挡3概率;
        S_铭文弓手3挡技能编号.Value = Settings.Default.铭文弓手3挡技能编号;
        S_铭文弓手3挡技能铭文.Value = Settings.Default.铭文弓手3挡技能铭文;
        S_铭文龙枪1挡1次数.Value = Settings.Default.铭文龙枪1挡1次数;
        S_铭文龙枪1挡2次数.Value = Settings.Default.铭文龙枪1挡2次数;
        S_铭文龙枪1挡3次数.Value = Settings.Default.铭文龙枪1挡3次数;
        S_铭文龙枪1挡1概率.Value = Settings.Default.铭文龙枪1挡1概率;
        S_铭文龙枪1挡2概率.Value = Settings.Default.铭文龙枪1挡2概率;
        S_铭文龙枪1挡3概率.Value = Settings.Default.铭文龙枪1挡3概率;
        S_铭文龙枪1挡技能编号.Value = Settings.Default.铭文龙枪1挡技能编号;
        S_铭文龙枪1挡技能铭文.Value = Settings.Default.铭文龙枪1挡技能铭文;
        S_铭文龙枪2挡1次数.Value = Settings.Default.铭文龙枪2挡1次数;
        S_铭文龙枪2挡2次数.Value = Settings.Default.铭文龙枪2挡2次数;
        S_铭文龙枪2挡3次数.Value = Settings.Default.铭文龙枪2挡3次数;
        S_铭文龙枪2挡1概率.Value = Settings.Default.铭文龙枪2挡1概率;
        S_铭文龙枪2挡2概率.Value = Settings.Default.铭文龙枪2挡2概率;
        S_铭文龙枪2挡3概率.Value = Settings.Default.铭文龙枪2挡3概率;
        S_铭文龙枪2挡技能编号.Value = Settings.Default.铭文龙枪2挡技能编号;
        S_铭文龙枪2挡技能铭文.Value = Settings.Default.铭文龙枪2挡技能铭文;
        S_铭文龙枪3挡1次数.Value = Settings.Default.铭文龙枪3挡1次数;
        S_铭文龙枪3挡2次数.Value = Settings.Default.铭文龙枪3挡2次数;
        S_铭文龙枪3挡3次数.Value = Settings.Default.铭文龙枪3挡3次数;
        S_铭文龙枪3挡1概率.Value = Settings.Default.铭文龙枪3挡1概率;
        S_铭文龙枪3挡2概率.Value = Settings.Default.铭文龙枪3挡2概率;
        S_铭文龙枪3挡3概率.Value = Settings.Default.铭文龙枪3挡3概率;
        S_铭文龙枪3挡技能编号.Value = Settings.Default.铭文龙枪3挡技能编号;
        S_铭文龙枪3挡技能铭文.Value = Settings.Default.铭文龙枪3挡技能铭文;
        S_铭文道士保底开关.Value = Settings.Default.铭文道士保底开关;
        S_铭文战士保底开关.Value = Settings.Default.铭文战士保底开关;
        S_铭文法师保底开关.Value = Settings.Default.铭文法师保底开关;
        S_铭文刺客保底开关.Value = Settings.Default.铭文刺客保底开关;
        S_铭文弓手保底开关.Value = Settings.Default.铭文弓手保底开关;
        S_铭文龙枪保底开关.Value = Settings.Default.铭文龙枪保底开关;
        S_DropRateModifier.Value = Settings.Default.DropRateModifier;
        S_魔虫窟副本次数.Value = Settings.Default.魔虫窟副本次数;
        S_魔虫窟副本等级.Value = Settings.Default.魔虫窟副本等级;
        S_魔虫窟副本物品.Value = Settings.Default.魔虫窟副本物品;
        S_魔虫窟副本数量.Value = Settings.Default.魔虫窟副本数量;
        S_魔虫窟副本时间小.Value = Settings.Default.魔虫窟副本时间小;
        S_魔虫窟副本时间大.Value = Settings.Default.魔虫窟副本时间大;
        S_书店商贩物品.Text = Settings.Default.书店商贩物品;
        S_幸运洗练次数保底.Value = Settings.Default.幸运洗练次数保底;
        S_幸运洗练点数.Value = Settings.Default.幸运洗练点数;
        S_武器强化消耗货币值.Value = Settings.Default.武器强化消耗货币值;
        S_武器强化消耗货币开关.Value = Settings.Default.武器强化消耗货币开关;
        S_武器强化取回时间.Value = Settings.Default.武器强化取回时间;
        S_幸运额外1值.Value = Settings.Default.幸运额外1值;
        S_幸运额外2值.Value = Settings.Default.幸运额外2值;
        S_幸运额外3值.Value = Settings.Default.幸运额外3值;
        S_幸运额外4值.Value = Settings.Default.幸运额外4值;
        S_幸运额外5值.Value = Settings.Default.幸运额外5值;
        S_幸运额外1伤害.Value = (decimal)Settings.Default.幸运额外1伤害;
        S_幸运额外2伤害.Value = (decimal)Settings.Default.幸运额外2伤害;
        S_幸运额外3伤害.Value = (decimal)Settings.Default.幸运额外3伤害;
        S_幸运额外4伤害.Value = (decimal)Settings.Default.幸运额外4伤害;
        S_幸运额外5伤害.Value = (decimal)Settings.Default.幸运额外5伤害;
        S_暗之门地图1.Value = Settings.Default.暗之门地图1;
        S_暗之门地图2.Value = Settings.Default.暗之门地图2;
        S_暗之门地图3.Value = Settings.Default.暗之门地图3;
        S_暗之门地图4.Value = Settings.Default.暗之门地图4;
        S_暗之门全服提示.Value = Settings.Default.暗之门全服提示;
        S_暗之门杀怪触发.Value = Settings.Default.暗之门杀怪触发;
        S_暗之门时间.Value = Settings.Default.暗之门时间;
        S_暗之门地图1BOSS.Text = Settings.Default.暗之门地图1BOSS;
        S_暗之门地图2BOSS.Text = Settings.Default.暗之门地图2BOSS;
        S_暗之门地图3BOSS.Text = Settings.Default.暗之门地图3BOSS;
        S_暗之门地图4BOSS.Text = Settings.Default.暗之门地图4BOSS;
        S_暗之门地图1X.Value = Settings.Default.暗之门地图1X;
        S_暗之门地图1Y.Value = Settings.Default.暗之门地图1Y;
        S_暗之门地图2X.Value = Settings.Default.暗之门地图2X;
        S_暗之门地图2Y.Value = Settings.Default.暗之门地图2Y;
        S_暗之门地图3X.Value = Settings.Default.暗之门地图3X;
        S_暗之门地图3Y.Value = Settings.Default.暗之门地图3Y;
        S_暗之门地图4X.Value = Settings.Default.暗之门地图4X;
        S_暗之门地图4Y.Value = Settings.Default.暗之门地图4Y;
        S_暗之门开关.Value = Settings.Default.暗之门开关;
        S_监狱货币类型.Value = Settings.Default.监狱货币类型;
        S_监狱货币.Value = Settings.Default.监狱货币;
        S_魔虫窟分钟限制.Value = Settings.Default.魔虫窟分钟限制;
        S_自定义元宝兑换01.Value = Settings.Default.自定义元宝兑换01;
        S_自定义元宝兑换02.Value = Settings.Default.自定义元宝兑换02;
        S_自定义元宝兑换03.Value = Settings.Default.自定义元宝兑换03;
        S_自定义元宝兑换04.Value = Settings.Default.自定义元宝兑换04;
        S_自定义元宝兑换05.Value = Settings.Default.自定义元宝兑换05;
        S_直升等级1.Value = Settings.Default.直升等级1;
        S_直升等级2.Value = Settings.Default.直升等级2;
        S_直升等级3.Value = Settings.Default.直升等级3;
        S_直升等级4.Value = Settings.Default.直升等级4;
        S_直升等级5.Value = Settings.Default.直升等级5;
        S_直升等级6.Value = Settings.Default.直升等级6;
        S_直升等级7.Value = Settings.Default.直升等级7;
        S_直升等级8.Value = Settings.Default.直升等级8;
        S_直升等级9.Value = Settings.Default.直升等级9;
        S_直升经验1.Value = Settings.Default.直升经验1;
        S_直升经验2.Value = Settings.Default.直升经验2;
        S_直升经验3.Value = Settings.Default.直升经验3;
        S_直升经验4.Value = Settings.Default.直升经验4;
        S_直升经验5.Value = Settings.Default.直升经验5;
        S_直升经验6.Value = Settings.Default.直升经验6;
        S_直升经验7.Value = Settings.Default.直升经验7;
        S_直升经验8.Value = Settings.Default.直升经验8;
        S_直升经验9.Value = Settings.Default.直升经验9;
        S_直升物品1.Value = Settings.Default.直升物品1;
        S_直升物品2.Value = Settings.Default.直升物品2;
        S_直升物品3.Value = Settings.Default.直升物品3;
        S_直升物品4.Value = Settings.Default.直升物品4;
        S_直升物品5.Value = Settings.Default.直升物品5;
        S_直升物品6.Value = Settings.Default.直升物品6;
        S_直升物品7.Value = Settings.Default.直升物品7;
        S_直升物品8.Value = Settings.Default.直升物品8;
        S_直升物品9.Value = Settings.Default.直升物品9;
        S_RechargeSystemFormat.Value = Settings.Default.RechargeSystemFormat;
        DefaultSkillLevel.Value = Settings.Default.DefaultSkillLevel;
        S_沃玛分解物品一.Text = Settings.Default.沃玛分解物品一;
        S_沃玛分解物品二.Text = Settings.Default.沃玛分解物品二;
        S_沃玛分解物品三.Text = Settings.Default.沃玛分解物品三;
        S_沃玛分解物品四.Text = Settings.Default.沃玛分解物品四;
        S_沃玛分解几率一.Value = Settings.Default.沃玛分解几率一;
        S_沃玛分解几率二.Value = Settings.Default.沃玛分解几率二;
        S_沃玛分解几率三.Value = Settings.Default.沃玛分解几率三;
        S_沃玛分解几率四.Value = Settings.Default.沃玛分解几率四;
        S_沃玛分解数量一.Value = Settings.Default.沃玛分解数量一;
        S_沃玛分解数量二.Value = Settings.Default.沃玛分解数量二;
        S_沃玛分解数量三.Value = Settings.Default.沃玛分解数量三;
        S_沃玛分解数量四.Value = Settings.Default.沃玛分解数量四;
        S_沃玛分解开关.Value = Settings.Default.沃玛分解开关;
        S_其他分解物品一.Text = Settings.Default.其他分解物品一;
        S_其他分解物品二.Text = Settings.Default.其他分解物品二;
        S_其他分解物品三.Text = Settings.Default.其他分解物品三;
        S_其他分解物品四.Text = Settings.Default.其他分解物品四;
        S_其他分解几率一.Value = Settings.Default.其他分解几率一;
        S_其他分解几率二.Value = Settings.Default.其他分解几率二;
        S_其他分解几率三.Value = Settings.Default.其他分解几率三;
        S_其他分解几率四.Value = Settings.Default.其他分解几率四;
        S_其他分解数量一.Value = Settings.Default.其他分解数量一;
        S_其他分解数量二.Value = Settings.Default.其他分解数量二;
        S_其他分解数量三.Value = Settings.Default.其他分解数量三;
        S_其他分解数量四.Value = Settings.Default.其他分解数量四;
        S_其他分解开关.Value = Settings.Default.其他分解开关;
        拾取地图控制1.Value = Settings.Default.AutoPickUpMap1;
        拾取地图控制2.Value = Settings.Default.AutoPickUpMap2;
        拾取地图控制3.Value = Settings.Default.AutoPickUpMap3;
        拾取地图控制4.Value = Settings.Default.AutoPickUpMap4;
        拾取地图控制5.Value = Settings.Default.AutoPickUpMap5;
        拾取地图控制6.Value = Settings.Default.AutoPickUpMap6;
        拾取地图控制7.Value = Settings.Default.AutoPickUpMap7;
        拾取地图控制8.Value = Settings.Default.AutoPickUpMap8;
        沙城捐献货币类型.Value = Settings.Default.沙城捐献货币类型;
        沙城捐献支付数量.Value = Settings.Default.沙城捐献支付数量;
        沙城捐献获得物品1.Value = Settings.Default.沙城捐献获得物品1;
        沙城捐献获得物品2.Value = Settings.Default.沙城捐献获得物品2;
        沙城捐献获得物品3.Value = Settings.Default.沙城捐献获得物品3;
        沙城捐献物品数量1.Value = Settings.Default.沙城捐献物品数量1;
        沙城捐献物品数量2.Value = Settings.Default.沙城捐献物品数量2;
        沙城捐献物品数量3.Value = Settings.Default.沙城捐献物品数量3;
        沙城捐献赞助人数.Value = Settings.Default.沙城捐献赞助人数;
        沙城捐献赞助金额.Value = Settings.Default.沙城捐献赞助金额;
        雕爷激活灵符需求.Value = Settings.Default.雕爷激活灵符需求;
        雕爷1号位灵符.Value = Settings.Default.雕爷1号位灵符;
        雕爷1号位铭文石.Value = Settings.Default.雕爷1号位铭文石;
        雕爷2号位灵符.Value = Settings.Default.雕爷2号位灵符;
        雕爷2号位铭文石.Value = Settings.Default.雕爷2号位铭文石;
        雕爷3号位灵符.Value = Settings.Default.雕爷3号位灵符;
        雕爷3号位铭文石.Value = Settings.Default.雕爷3号位铭文石;
        S_称号范围拾取判断1.Value = Settings.Default.称号范围拾取判断;
        九层妖塔统计开关.Value = Settings.Default.九层妖塔统计开关;
        沙巴克每周攻沙时间.Value = Settings.Default.沙巴克每周攻沙时间;
        沙巴克皇宫传送等级.Value = Settings.Default.沙巴克皇宫传送等级;
        沙巴克皇宫传送物品.Value = Settings.Default.沙巴克皇宫传送物品;
        沙巴克皇宫传送数量.Value = Settings.Default.沙巴克皇宫传送数量;
        系统窗口发送.Value = Settings.Default.系统窗口发送;
        龙卫效果提示.Value = Settings.Default.龙卫效果提示;
        AllowRecharge.Value = Settings.Default.AllowRecharge;
        全服红包等级.Value = Settings.Default.全服红包等级;
        全服红包时间.Value = Settings.Default.全服红包时间;
        全服红包货币类型.Value = Settings.Default.GlobalBonusCurrencyType;
        全服红包货币数量.Value = Settings.Default.全服红包货币数量;
        龙卫蓝色词条概率.Value = Settings.Default.龙卫蓝色词条概率;
        龙卫紫色词条概率.Value = Settings.Default.龙卫紫色词条概率;
        龙卫橙色词条概率.Value = Settings.Default.龙卫橙色词条概率;
        自定义初始货币类型.Value = Settings.Default.自定义初始货币类型;
        自动回收设置.Checked = Settings.Default.自动回收设置;
        购买狂暴之力.Checked = Settings.Default.购买狂暴之力;
        会员满血设置.Checked = Settings.Default.会员满血设置;
        全屏拾取开关.Checked = Settings.Default.AutoPickUpAllVisible;
        打开随时仓库.Checked = Settings.Default.打开随时仓库;
        红包开关.Checked = Settings.Default.红包开关;
        龙卫焰焚烈火剑法.Value = Settings.Default.龙卫焰焚烈火剑法;
        会员物品对接.Value = Settings.Default.会员物品对接;
        变性等级.Value = Settings.Default.变性等级;
        变性货币类型.Value = Settings.Default.变性货币类型;
        变性货币值.Value = Settings.Default.变性货币值;
        变性物品ID.Value = Settings.Default.变性物品ID;
        变性物品数量.Value = Settings.Default.变性物品数量;
        称号叠加模块9.Value = Settings.Default.称号叠加模块9;
        称号叠加模块10.Value = Settings.Default.称号叠加模块10;
        称号叠加模块11.Value = Settings.Default.称号叠加模块11;
        称号叠加模块12.Value = Settings.Default.称号叠加模块12;
        称号叠加模块13.Value = Settings.Default.称号叠加模块13;
        称号叠加模块14.Value = Settings.Default.称号叠加模块14;
        称号叠加模块15.Value = Settings.Default.称号叠加模块15;
        称号叠加模块16.Value = Settings.Default.称号叠加模块16;
        幸运保底开关.Checked = Settings.Default.幸运保底开关;
        安全区收刀开关.Checked = Settings.Default.安全区收刀开关;
        屠魔殿等级限制.Value = Settings.Default.屠魔殿等级限制;
        职业等级.Value = Settings.Default.职业等级;
        RaceChangeCurrencyType.Value = Settings.Default.RaceChangeCurrencyType;
        RaceChangeCurrencyValue.Value = Settings.Default.RaceChangeCurrencyValue;
        RaceChangeItemID.Value = Settings.Default.RaceChangeItemID;
        RaceChangeItemQuantity.Value = Settings.Default.RaceChangeItemQuantity;
        武斗场杀人经验.Value = Settings.Default.武斗场杀人经验;
        武斗场杀人开关.Checked = Settings.Default.武斗场杀人开关;
        S_狂暴名称.Text = Settings.Default.狂暴名称;
        S_自定义物品内容一.Text = Settings.Default.自定义物品内容一;
        S_自定义物品内容二.Text = Settings.Default.自定义物品内容二;
        S_自定义物品内容三.Text = Settings.Default.自定义物品内容三;
        S_自定义物品内容四.Text = Settings.Default.自定义物品内容四;
        S_自定义物品内容五.Text = Settings.Default.自定义物品内容五;
        S_挂机权限选项.Text = Settings.Default.挂机权限选项;
        合成模块控件.Text = Settings.Default.合成模块控件;
        变性内容控件.Text = Settings.Default.变性内容控件;
        转职内容控件.Text = Settings.Default.转职内容控件;
        S_战将特权礼包.Text = Settings.Default.战将特权礼包;
        S_豪杰特权礼包.Text = Settings.Default.豪杰特权礼包;
        S_世界BOSS名字.Text = Settings.Default.WorldBossName;
        S_世界BOSS时间.Value = Settings.Default.WorldBossTimeHour;
        S_世界BOSS分钟.Value = Settings.Default.WorldBossTimeMinute;
        S_秘宝广场元宝.Value = Settings.Default.秘宝广场元宝;
        S_每周特惠礼包一元宝.Value = Settings.Default.每周特惠礼包一元宝;
        S_每周特惠礼包二元宝.Value = Settings.Default.每周特惠礼包二元宝;
        S_特权玛法名俊元宝.Value = Settings.Default.特权玛法名俊元宝;
        S_特权玛法豪杰元宝.Value = Settings.Default.特权玛法豪杰元宝;
        S_特权玛法战将元宝.Value = Settings.Default.特权玛法战将元宝;
        S_御兽切换开关.Value = Settings.Default.御兽切换开关;

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
            savaDatabaseToolStripMenuItem.BackColor = (savaDatabaseToolStripMenuItem.BackColor == Color.LightSteelBlue) ?
                Color.PaleVioletRed : Color.LightSteelBlue;
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
                S_GameDataPath.Text = Settings.Default.GameDataPath = path;
                Settings.Default.Save();
            }
            else if (sender == ButtonBrowseBackupDirectory)
            {
                var path = folderBrowserDialog.SelectedPath;
                S_DataBackupPath.Text = Settings.Default.DataBackupPath = path;
                Settings.Default.Save();
            }
            else if (sender == S_浏览平台目录)
            {
                var path = folderBrowserDialog.SelectedPath;
                S_平台接入目录.Text = Settings.Default.平台接入目录 = path;
                Settings.Default.Save();
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
            case nameof(S_收益减少比率): Settings.Default.收益减少比率 = control.Value; break;
            case nameof(S_DisconnectTime): Settings.Default.DisconnectTime = (ushort)control.Value; break;
            case nameof(S_MaxUserLevel): Settings.Default.MaxUserLevel = (byte)control.Value; break;
            case nameof(S_怪物诱惑时长): Settings.Default.怪物诱惑时长 = (ushort)control.Value; break;
            case nameof(S_MonsterExperienceMultiplier): Settings.Default.MonsterExperienceMultiplier = control.Value; break;
            case nameof(S_TicketReceivePort): Settings.Default.TicketReceivePort = (ushort)control.Value; break;
            case nameof(S_AbnormalBlockTime): Settings.Default.AbnormalBlockTime = (ushort)control.Value; break;
            case nameof(S_减收益等级差): Settings.Default.减收益等级差 = (ushort)control.Value; break;
            case nameof(S_ItemDropRate): Settings.Default.ItemDropRate = control.Value; break;
            case nameof(S_物品归属时间): Settings.Default.物品归属时间 = (ushort)control.Value; break;
            case nameof(S_NoobSupportLevel): Settings.Default.NoobSupportLevel = (byte)control.Value; break;
            case nameof(S_SpecialRepairDiscount): Settings.Default.SpecialRepairDiscount = control.Value; break;
            case nameof(S_ItemDisappearTime): Settings.Default.ItemDisappearTime = (ushort)control.Value; break;
            case nameof(S_PacketLimit): Settings.Default.PacketLimit = (ushort)control.Value; break;
            case nameof(S_UserConnectionPort): Settings.Default.UserConnectionPort = (ushort)control.Value; break;
            case nameof(S_自动保存时间): Settings.Default.AutoSaveInterval = (ushort)control.Value; break;
            case nameof(S_自动保存日志): Settings.Default.自动保存日志 = (ushort)control.Value; break;
            case nameof(S_沃玛分解元宝): Settings.Default.沃玛分解元宝 = (int)control.Value; break;
            case nameof(S_祖玛分解元宝): Settings.Default.祖玛分解元宝 = (int)control.Value; break;
            case nameof(S_赤月分解元宝): Settings.Default.赤月分解元宝 = (int)control.Value; break;
            case nameof(S_魔龙分解元宝): Settings.Default.魔龙分解元宝 = (int)control.Value; break;
            case nameof(S_星王分解元宝): Settings.Default.星王分解元宝 = (int)control.Value; break;
            case nameof(S_苍月分解元宝): Settings.Default.苍月分解元宝 = (int)control.Value; break;
            case nameof(S_城主分解元宝): Settings.Default.城主分解元宝 = (int)control.Value; break;
            case nameof(S_神秘分解元宝): Settings.Default.城主分解元宝 = (int)control.Value; break;
            case nameof(S_屠魔组队人数): Settings.Default.屠魔组队人数 = (int)control.Value; break;
            case nameof(S_屠魔令回收经验): Settings.Default.屠魔令回收经验 = (int)control.Value; break;
            case nameof(S_屠魔爆率开关): Settings.Default.屠魔爆率开关 = (int)control.Value; break;
            case nameof(S_武斗场时间一): Settings.Default.武斗场时间一 = (byte)control.Value; break;
            case nameof(S_武斗场时间二): Settings.Default.武斗场时间二 = (byte)control.Value; break;
            case nameof(S_武斗场经验小): Settings.Default.武斗场经验小 = (int)control.Value; break;
            case nameof(S_武斗场经验大): Settings.Default.武斗场经验大 = (int)control.Value; break;
            case nameof(S_沙巴克开启): Settings.Default.沙巴克开启 = (byte)control.Value; break;
            case nameof(S_沙巴克结束): Settings.Default.沙巴克结束 = (byte)control.Value; break;
            case nameof(S_祝福油幸运1机率): Settings.Default.祝福油幸运1机率 = (int)control.Value; break;
            case nameof(S_祝福油幸运2机率): Settings.Default.祝福油幸运2机率 = (int)control.Value; break;
            case nameof(S_祝福油幸运3机率): Settings.Default.祝福油幸运3机率 = (int)control.Value; break;
            case nameof(S_祝福油幸运4机率): Settings.Default.祝福油幸运4机率 = (int)control.Value; break;
            case nameof(S_祝福油幸运5机率): Settings.Default.祝福油幸运5机率 = (int)control.Value; break;
            case nameof(S_祝福油幸运6机率): Settings.Default.祝福油幸运6机率 = (int)control.Value; break;
            case nameof(S_祝福油幸运7机率): Settings.Default.祝福油幸运7机率 = (int)control.Value; break;
            case nameof(S_PKYellowNamePoint): Settings.Default.PKYellowNamePoint = (int)control.Value; break;
            case nameof(S_PKRedNamePoint): Settings.Default.PKRedNamePoint = (int)control.Value; break;
            case nameof(S_PKCrimsonNamePoint): Settings.Default.PKCrimsonNamePoint = (int)control.Value; break;
            case nameof(S_锻造成功倍数): Settings.Default.锻造成功倍数 = (int)control.Value; break;
            case nameof(S_死亡掉落背包几率): Settings.Default.死亡掉落背包几率 = (float)control.Value; break;
            case nameof(S_死亡掉落身上几率): Settings.Default.死亡掉落身上几率 = (float)control.Value; break;
            case nameof(S_PK死亡幸运开关): Settings.Default.PK死亡幸运开关 = (int)control.Value; break;
            case nameof(S_屠魔副本次数): Settings.Default.屠魔副本次数 = (int)control.Value; break;
            case nameof(S_高级祝福油幸运机率): Settings.Default.高级祝福油幸运机率 = (int)control.Value; break;
            case nameof(S_雕爷使用物品): Settings.Default.雕爷使用物品 = (int)control.Value; break;
            case nameof(S_雕爷使用金币): Settings.Default.雕爷使用金币 = (int)control.Value; break;
            case nameof(S_称号范围拾取判断): Settings.Default.称号范围拾取判断 = (int)control.Value; break;
            case nameof(S_TitleRangePickUpDistance): Settings.Default.TitleRangePickUpDistance = (int)control.Value; break;
            case nameof(S_行会申请人数限制): Settings.Default.行会申请人数限制 = (int)control.Value; break;
            case nameof(S_疗伤药HP): Settings.Default.疗伤药HP = (int)control.Value; break;
            case nameof(S_疗伤药MP): Settings.Default.疗伤药MP = (int)control.Value; break;
            case nameof(S_万年雪霜HP): Settings.Default.万年雪霜HP = (int)control.Value; break;
            case nameof(S_万年雪霜MP): Settings.Default.万年雪霜MP = (int)control.Value; break;
            case nameof(S_元宝金币回收设定): Settings.Default.元宝金币回收设定 = (int)control.Value; break;
            case nameof(S_元宝金币传送设定): Settings.Default.元宝金币传送设定 = (int)control.Value; break;
            case nameof(S_快捷传送一编号): Settings.Default.快捷传送一编号 = (int)control.Value; break;
            case nameof(S_快捷传送一货币): Settings.Default.快捷传送一货币 = (int)control.Value; break;
            case nameof(S_快捷传送一等级): Settings.Default.快捷传送一等级 = (int)control.Value; break;
            case nameof(S_快捷传送二编号): Settings.Default.快捷传送二编号 = (int)control.Value; break;
            case nameof(S_快捷传送二货币): Settings.Default.快捷传送二货币 = (int)control.Value; break;
            case nameof(S_快捷传送二等级): Settings.Default.快捷传送二等级 = (int)control.Value; break;
            case nameof(S_快捷传送三编号): Settings.Default.快捷传送三编号 = (int)control.Value; break;
            case nameof(S_快捷传送三货币): Settings.Default.快捷传送三货币 = (int)control.Value; break;
            case nameof(S_快捷传送三等级): Settings.Default.快捷传送三等级 = (int)control.Value; break;
            case nameof(S_快捷传送四编号): Settings.Default.快捷传送四编号 = (int)control.Value; break;
            case nameof(S_快捷传送四货币): Settings.Default.快捷传送四货币 = (int)control.Value; break;
            case nameof(S_快捷传送四等级): Settings.Default.快捷传送四等级 = (int)control.Value; break;
            case nameof(S_快捷传送五编号): Settings.Default.快捷传送五编号 = (int)control.Value; break;
            case nameof(S_快捷传送五货币): Settings.Default.快捷传送五货币 = (int)control.Value; break;
            case nameof(S_快捷传送五等级): Settings.Default.快捷传送五等级 = (int)control.Value; break;
            case nameof(S_狂暴货币格式): Settings.Default.狂暴货币格式 = (int)control.Value; break;
            case nameof(S_狂暴称号格式): Settings.Default.狂暴称号格式 = (byte)control.Value; break;
            case nameof(S_狂暴开启物品名称): Settings.Default.狂暴开启物品名称 = (int)control.Value; break;
            case nameof(S_狂暴开启物品数量): Settings.Default.狂暴开启物品数量 = (int)control.Value; break;
            case nameof(S_狂暴杀死物品数量): Settings.Default.狂暴杀死物品数量 = (int)control.Value; break;
            case nameof(S_狂暴开启元宝数量): Settings.Default.狂暴开启元宝数量 = (int)control.Value; break;
            case nameof(S_狂暴杀死元宝数量): Settings.Default.狂暴杀死元宝数量 = (int)control.Value; break;
            case nameof(S_狂暴开启金币数量): Settings.Default.狂暴开启金币数量 = (int)control.Value; break;
            case nameof(S_狂暴杀死金币数量): Settings.Default.狂暴杀死金币数量 = (int)control.Value; break;
            case nameof(S_装备技能开关): Settings.Default.装备技能开关 = (int)control.Value; break;
            case nameof(S_御兽属性开启): Settings.Default.御兽属性开启 = (int)control.Value; break;
            case nameof(S_可摆摊地图编号): Settings.Default.可摆摊地图编号 = (int)control.Value; break;
            case nameof(S_可摆摊地图坐标X): Settings.Default.可摆摊地图坐标X = (int)control.Value; break;
            case nameof(S_可摆摊地图坐标Y): Settings.Default.可摆摊地图坐标Y = (int)control.Value; break;
            case nameof(S_可摆摊地图范围): Settings.Default.可摆摊地图范围 = (int)control.Value; break;
            case nameof(S_可摆摊货币选择): Settings.Default.可摆摊货币选择 = (int)control.Value; break;
            case nameof(S_可摆摊等级): Settings.Default.可摆摊等级 = (int)control.Value; break;
            case nameof(S_ReviveInterval): Settings.Default.ReviveInterval = (int)control.Value; break;
            case nameof(S_自定义麻痹几率): Settings.Default.自定义麻痹几率 = (float)control.Value; break;

            case nameof(S_PetUpgradeXP):
                {
                    var index = (int)S_PetUpgradeXPLevel.Value;
                    Settings.Default.PetUpgradeXP[index - 1] = (ushort)control.Value;
                    break;
                }
            case nameof(S_PetUpgradeXPLevel):
                {
                    var index = (int)control.Value;
                    S_PetUpgradeXP.Value = Settings.Default.PetUpgradeXP[index - 1];
                    break;
                }

            case nameof(S_UpgradeXP):
                {
                    var index = (int)S_UpgradeXPLevel.Value;
                    Settings.Default.UserUpgradeXP[index - 1] = (int)control.Value;
                    break;
                }
            case nameof(S_UpgradeXPLevel):
                {
                    var index = (int)control.Value;
                    S_UpgradeXP.Value = Settings.Default.UserUpgradeXP[index - 1];
                    break;
                }

            case nameof(S_下马击落机率): Settings.Default.下马击落机率 = (int)control.Value; break;
            case nameof(S_AllowRaceWarrior): Settings.Default.AllowRaceWarrior = (int)control.Value; break;
            case nameof(S_AllowRaceWizard): Settings.Default.AllowRaceWizard = (int)control.Value; break;
            case nameof(S_AllowRaceTaoist): Settings.Default.AllowRaceTaoist = (int)control.Value; break;
            case nameof(S_AllowRaceArcher): Settings.Default.AllowRaceArcher = (int)control.Value; break;
            case nameof(S_AllowRaceAssassin): Settings.Default.AllowRaceAssassin = (int)control.Value; break;
            case nameof(S_AllowRaceDragonLance): Settings.Default.AllowRaceDragonLance = (int)control.Value; break;
            case nameof(S_泡点等级开关): Settings.Default.泡点等级开关 = (int)control.Value; break;
            case nameof(S_泡点当前经验): Settings.Default.泡点当前经验 = (int)control.Value; break;
            case nameof(S_泡点限制等级): Settings.Default.泡点限制等级 = (int)control.Value; break;
            case nameof(S_杀人PK红名开关): Settings.Default.杀人PK红名开关 = (int)control.Value; break;
            case nameof(S_泡点秒数控制): Settings.Default.泡点秒数控制 = (int)control.Value; break;
            case nameof(S_自定义物品数量一): Settings.Default.自定义物品数量一 = (int)control.Value; break;
            case nameof(S_自定义物品数量二): Settings.Default.自定义物品数量二 = (int)control.Value; break;
            case nameof(S_自定义物品数量三): Settings.Default.自定义物品数量三 = (int)control.Value; break;
            case nameof(S_自定义物品数量四): Settings.Default.自定义物品数量四 = (int)control.Value; break;
            case nameof(S_自定义物品数量五): Settings.Default.自定义物品数量五 = (int)control.Value; break;
            case nameof(S_自定义称号内容一): Settings.Default.自定义称号内容一 = (byte)control.Value; break;
            case nameof(S_自定义称号内容二): Settings.Default.自定义称号内容二 = (byte)control.Value; break;
            case nameof(S_自定义称号内容三): Settings.Default.自定义称号内容三 = (byte)control.Value; break;
            case nameof(S_自定义称号内容四): Settings.Default.自定义称号内容四 = (byte)control.Value; break;
            case nameof(S_自定义称号内容五): Settings.Default.自定义称号内容五 = (byte)control.Value; break;
            case nameof(S_元宝金币传送设定2): Settings.Default.元宝金币传送设定2 = (int)control.Value; break;
            case nameof(S_快捷传送一编号2): Settings.Default.快捷传送一编号2 = (int)control.Value; break;
            case nameof(S_快捷传送一货币2): Settings.Default.快捷传送一货币2 = (int)control.Value; break;
            case nameof(S_快捷传送一等级2): Settings.Default.快捷传送一等级2 = (int)control.Value; break;
            case nameof(S_快捷传送二编号2): Settings.Default.快捷传送二编号2 = (int)control.Value; break;
            case nameof(S_快捷传送二货币2): Settings.Default.快捷传送二货币2 = (int)control.Value; break;
            case nameof(S_快捷传送二等级2): Settings.Default.快捷传送二等级2 = (int)control.Value; break;
            case nameof(S_快捷传送三编号2): Settings.Default.快捷传送三编号2 = (int)control.Value; break;
            case nameof(S_快捷传送三货币2): Settings.Default.快捷传送三货币2 = (int)control.Value; break;
            case nameof(S_快捷传送三等级2): Settings.Default.快捷传送三等级2 = (int)control.Value; break;
            case nameof(S_快捷传送四编号2): Settings.Default.快捷传送四编号2 = (int)control.Value; break;
            case nameof(S_快捷传送四货币2): Settings.Default.快捷传送四货币2 = (int)control.Value; break;
            case nameof(S_快捷传送四等级2): Settings.Default.快捷传送四等级2 = (int)control.Value; break;
            case nameof(S_快捷传送五编号2): Settings.Default.快捷传送五编号2 = (int)control.Value; break;
            case nameof(S_快捷传送五货币2): Settings.Default.快捷传送五货币2 = (int)control.Value; break;
            case nameof(S_快捷传送五等级2): Settings.Default.快捷传送五等级2 = (int)control.Value; break;
            case nameof(S_快捷传送六编号2): Settings.Default.快捷传送六编号2 = (int)control.Value; break;
            case nameof(S_快捷传送六货币2): Settings.Default.快捷传送六货币2 = (int)control.Value; break;
            case nameof(S_快捷传送六等级2): Settings.Default.快捷传送六等级2 = (int)control.Value; break;
            case nameof(S_武斗场次数限制): Settings.Default.武斗场次数限制 = (int)control.Value; break;
            case nameof(S_AutoPickUpInventorySpace): Settings.Default.AutoPickUpInventorySpace = (int)control.Value; break;
            case nameof(S_BOSS刷新提示开关): Settings.Default.BOSS刷新提示开关 = (int)control.Value; break;
            case nameof(S_自动整理背包计时): Settings.Default.自动整理背包计时 = (int)control.Value; break;
            case nameof(S_自动整理背包开关): Settings.Default.自动整理背包开关 = (int)control.Value; break;
            case nameof(S_称号叠加开关): Settings.Default.称号叠加开关 = (int)control.Value; break;
            case nameof(S_称号叠加模块一): Settings.Default.称号叠加模块一 = (byte)control.Value; break;
            case nameof(S_称号叠加模块二): Settings.Default.称号叠加模块二 = (byte)control.Value; break;
            case nameof(S_称号叠加模块三): Settings.Default.称号叠加模块三 = (byte)control.Value; break;
            case nameof(S_称号叠加模块四): Settings.Default.称号叠加模块四 = (byte)control.Value; break;
            case nameof(S_称号叠加模块五): Settings.Default.称号叠加模块五 = (byte)control.Value; break;
            case nameof(S_称号叠加模块六): Settings.Default.称号叠加模块六 = (byte)control.Value; break;
            case nameof(S_称号叠加模块七): Settings.Default.称号叠加模块七 = (byte)control.Value; break;
            case nameof(S_称号叠加模块八): Settings.Default.称号叠加模块八 = (byte)control.Value; break;
            case nameof(S_沙城传送货币开关): Settings.Default.沙城传送货币开关 = (int)control.Value; break;
            case nameof(S_沙城快捷货币一): Settings.Default.沙城快捷货币一 = (int)control.Value; break;
            case nameof(S_沙城快捷货币二): Settings.Default.沙城快捷货币二 = (int)control.Value; break;
            case nameof(S_沙城快捷货币三): Settings.Default.沙城快捷货币三 = (int)control.Value; break;
            case nameof(S_沙城快捷货币四): Settings.Default.沙城快捷货币四 = (int)control.Value; break;
            case nameof(S_沙城快捷等级一): Settings.Default.沙城快捷等级一 = (int)control.Value; break;
            case nameof(S_沙城快捷等级二): Settings.Default.沙城快捷等级二 = (int)control.Value; break;
            case nameof(S_沙城快捷等级三): Settings.Default.沙城快捷等级三 = (int)control.Value; break;
            case nameof(S_沙城快捷等级四): Settings.Default.沙城快捷等级四 = (int)control.Value; break;
            case nameof(S_未知暗点副本价格): Settings.Default.未知暗点副本价格 = (int)control.Value; break;
            case nameof(S_未知暗点副本等级): Settings.Default.未知暗点副本等级 = (int)control.Value; break;
            case nameof(S_未知暗点二层价格): Settings.Default.未知暗点二层价格 = (int)control.Value; break;
            case nameof(S_未知暗点二层等级): Settings.Default.未知暗点二层等级 = (int)control.Value; break;
            case nameof(S_幽冥海副本价格): Settings.Default.幽冥海副本价格 = (int)control.Value; break;
            case nameof(S_幽冥海副本等级): Settings.Default.幽冥海副本等级 = (int)control.Value; break;
            case nameof(S_猎魔暗使称号六): Settings.Default.猎魔暗使称号六 = (byte)control.Value; break;
            case nameof(S_猎魔暗使材料六): Settings.Default.猎魔暗使材料六 = (int)control.Value; break;
            case nameof(S_猎魔暗使数量六): Settings.Default.猎魔暗使数量六 = (int)control.Value; break;
            case nameof(S_猎魔暗使称号五): Settings.Default.猎魔暗使称号五 = (byte)control.Value; break;
            case nameof(S_猎魔暗使材料五): Settings.Default.猎魔暗使材料五 = (int)control.Value; break;
            case nameof(S_猎魔暗使数量五): Settings.Default.猎魔暗使数量五 = (int)control.Value; break;
            case nameof(S_猎魔暗使称号四): Settings.Default.猎魔暗使称号四 = (byte)control.Value; break;
            case nameof(S_猎魔暗使材料四): Settings.Default.猎魔暗使材料四 = (int)control.Value; break;
            case nameof(S_猎魔暗使数量四): Settings.Default.猎魔暗使数量四 = (int)control.Value; break;
            case nameof(S_猎魔暗使称号三): Settings.Default.猎魔暗使称号三 = (byte)control.Value; break;
            case nameof(S_猎魔暗使材料三): Settings.Default.猎魔暗使材料三 = (int)control.Value; break;
            case nameof(S_猎魔暗使数量三): Settings.Default.猎魔暗使数量三 = (int)control.Value; break;
            case nameof(S_猎魔暗使称号二): Settings.Default.猎魔暗使称号二 = (byte)control.Value; break;
            case nameof(S_猎魔暗使材料二): Settings.Default.猎魔暗使材料二 = (int)control.Value; break;
            case nameof(S_猎魔暗使数量二): Settings.Default.猎魔暗使数量二 = (int)control.Value; break;
            case nameof(S_猎魔暗使称号一): Settings.Default.猎魔暗使称号一 = (byte)control.Value; break;
            case nameof(S_猎魔暗使材料一): Settings.Default.猎魔暗使材料一 = (int)control.Value; break;
            case nameof(S_猎魔暗使数量一): Settings.Default.猎魔暗使数量一 = (int)control.Value; break;
            case nameof(S_怪物掉落广播开关): Settings.Default.怪物掉落广播开关 = (int)control.Value; break;
            case nameof(S_怪物掉落窗口开关): Settings.Default.怪物掉落窗口开关 = (int)control.Value; break;
            case nameof(S_珍宝阁提示开关): Settings.Default.珍宝阁提示开关 = (int)control.Value; break;
            case nameof(S_祖玛分解几率一): Settings.Default.祖玛分解几率一 = (int)control.Value; break;
            case nameof(S_祖玛分解几率二): Settings.Default.祖玛分解几率二 = (int)control.Value; break;
            case nameof(S_祖玛分解几率三): Settings.Default.祖玛分解几率三 = (int)control.Value; break;
            case nameof(S_祖玛分解几率四): Settings.Default.祖玛分解几率四 = (int)control.Value; break;
            case nameof(S_祖玛分解数量一): Settings.Default.祖玛分解数量一 = (int)control.Value; break;
            case nameof(S_祖玛分解数量二): Settings.Default.祖玛分解数量二 = (int)control.Value; break;
            case nameof(S_祖玛分解数量三): Settings.Default.祖玛分解数量三 = (int)control.Value; break;
            case nameof(S_祖玛分解数量四): Settings.Default.祖玛分解数量四 = (int)control.Value; break;
            case nameof(S_祖玛分解开关): Settings.Default.祖玛分解开关 = (int)control.Value; break;
            case nameof(S_赤月分解几率一): Settings.Default.赤月分解几率一 = (int)control.Value; break;
            case nameof(S_赤月分解几率二): Settings.Default.赤月分解几率二 = (int)control.Value; break;
            case nameof(S_赤月分解几率三): Settings.Default.赤月分解几率三 = (int)control.Value; break;
            case nameof(S_赤月分解几率四): Settings.Default.赤月分解几率四 = (int)control.Value; break;
            case nameof(S_赤月分解数量一): Settings.Default.赤月分解数量一 = (int)control.Value; break;
            case nameof(S_赤月分解数量二): Settings.Default.赤月分解数量二 = (int)control.Value; break;
            case nameof(S_赤月分解数量三): Settings.Default.赤月分解数量三 = (int)control.Value; break;
            case nameof(S_赤月分解数量四): Settings.Default.赤月分解数量四 = (int)control.Value; break;
            case nameof(S_赤月分解开关): Settings.Default.赤月分解开关 = (int)control.Value; break;
            case nameof(S_魔龙分解几率一): Settings.Default.魔龙分解几率一 = (int)control.Value; break;
            case nameof(S_魔龙分解几率二): Settings.Default.魔龙分解几率二 = (int)control.Value; break;
            case nameof(S_魔龙分解几率三): Settings.Default.魔龙分解几率三 = (int)control.Value; break;
            case nameof(S_魔龙分解几率四): Settings.Default.魔龙分解几率四 = (int)control.Value; break;
            case nameof(S_魔龙分解数量一): Settings.Default.魔龙分解数量一 = (int)control.Value; break;
            case nameof(S_魔龙分解数量二): Settings.Default.魔龙分解数量二 = (int)control.Value; break;
            case nameof(S_魔龙分解数量三): Settings.Default.魔龙分解数量三 = (int)control.Value; break;
            case nameof(S_魔龙分解数量四): Settings.Default.魔龙分解数量四 = (int)control.Value; break;
            case nameof(S_魔龙分解开关): Settings.Default.魔龙分解开关 = (int)control.Value; break;
            case nameof(S_苍月分解几率一): Settings.Default.苍月分解几率一 = (int)control.Value; break;
            case nameof(S_苍月分解几率二): Settings.Default.苍月分解几率二 = (int)control.Value; break;
            case nameof(S_苍月分解几率三): Settings.Default.苍月分解几率三 = (int)control.Value; break;
            case nameof(S_苍月分解几率四): Settings.Default.苍月分解几率四 = (int)control.Value; break;
            case nameof(S_苍月分解数量一): Settings.Default.苍月分解数量一 = (int)control.Value; break;
            case nameof(S_苍月分解数量二): Settings.Default.苍月分解数量二 = (int)control.Value; break;
            case nameof(S_苍月分解数量三): Settings.Default.苍月分解数量三 = (int)control.Value; break;
            case nameof(S_苍月分解数量四): Settings.Default.苍月分解数量四 = (int)control.Value; break;
            case nameof(S_苍月分解开关): Settings.Default.苍月分解开关 = (int)control.Value; break;
            case nameof(S_星王分解几率一): Settings.Default.星王分解几率一 = (int)control.Value; break;
            case nameof(S_星王分解几率二): Settings.Default.星王分解几率二 = (int)control.Value; break;
            case nameof(S_星王分解几率三): Settings.Default.星王分解几率三 = (int)control.Value; break;
            case nameof(S_星王分解几率四): Settings.Default.星王分解几率四 = (int)control.Value; break;
            case nameof(S_星王分解数量一): Settings.Default.星王分解数量一 = (int)control.Value; break;
            case nameof(S_星王分解数量二): Settings.Default.星王分解数量二 = (int)control.Value; break;
            case nameof(S_星王分解数量三): Settings.Default.星王分解数量三 = (int)control.Value; break;
            case nameof(S_星王分解数量四): Settings.Default.星王分解数量四 = (int)control.Value; break;
            case nameof(S_星王分解开关): Settings.Default.星王分解开关 = (int)control.Value; break;
            case nameof(S_城主分解几率一): Settings.Default.城主分解几率一 = (int)control.Value; break;
            case nameof(S_城主分解几率二): Settings.Default.城主分解几率二 = (int)control.Value; break;
            case nameof(S_城主分解几率三): Settings.Default.城主分解几率三 = (int)control.Value; break;
            case nameof(S_城主分解几率四): Settings.Default.城主分解几率四 = (int)control.Value; break;
            case nameof(S_城主分解数量一): Settings.Default.城主分解数量一 = (int)control.Value; break;
            case nameof(S_城主分解数量二): Settings.Default.城主分解数量二 = (int)control.Value; break;
            case nameof(S_城主分解数量三): Settings.Default.城主分解数量三 = (int)control.Value; break;
            case nameof(S_城主分解数量四): Settings.Default.城主分解数量四 = (int)control.Value; break;
            case nameof(S_城主分解开关): Settings.Default.城主分解开关 = (int)control.Value; break;
            case nameof(S_世界BOSS时间): Settings.Default.WorldBossTimeHour = (byte)control.Value; break;
            case nameof(S_世界BOSS分钟): Settings.Default.WorldBossTimeMinute = (byte)control.Value; break;
            case nameof(S_秘宝广场元宝): Settings.Default.秘宝广场元宝 = (int)control.Value; break;
            case nameof(S_每周特惠礼包一元宝): Settings.Default.每周特惠礼包一元宝 = (int)control.Value; break;
            case nameof(S_每周特惠礼包二元宝): Settings.Default.每周特惠礼包二元宝 = (int)control.Value; break;
            case nameof(S_特权玛法名俊元宝): Settings.Default.特权玛法名俊元宝 = (int)control.Value; break;
            case nameof(S_特权玛法豪杰元宝): Settings.Default.特权玛法名俊元宝 = (int)control.Value; break;
            case nameof(S_特权玛法战将元宝): Settings.Default.特权玛法战将元宝 = (int)control.Value; break;
            case nameof(S_御兽切换开关): Settings.Default.御兽切换开关 = (int)control.Value; break;
            case nameof(S_BOSS卷轴地图编号): Settings.Default.BOSS卷轴地图编号 = (int)control.Value; break;
            case nameof(S_BOSS卷轴地图开关): Settings.Default.BOSS卷轴地图开关 = (int)control.Value; break;
            case nameof(S_沙巴克重置系统): Settings.Default.沙巴克重置系统 = (int)control.Value; break;
            case nameof(S_资源包开关): Settings.Default.资源包开关 = (int)control.Value; break;
            case nameof(S_StartingLevel): Settings.Default.StartingLevel = (byte)control.Value; break;
            case nameof(S_MaxUserConnections): Settings.Default.MaxUserConnections = (int)control.Value; break;
            case nameof(S_掉落贵重物品颜色): Settings.Default.掉落贵重物品颜色 = (int)control.Value; break;
            case nameof(S_掉落沃玛物品颜色): Settings.Default.掉落沃玛物品颜色 = (int)control.Value; break;
            case nameof(S_掉落祖玛物品颜色): Settings.Default.掉落祖玛物品颜色 = (int)control.Value; break;
            case nameof(S_掉落赤月物品颜色): Settings.Default.掉落赤月物品颜色 = (int)control.Value; break;
            case nameof(S_掉落魔龙物品颜色): Settings.Default.掉落魔龙物品颜色 = (int)control.Value; break;
            case nameof(S_掉落苍月物品颜色): Settings.Default.掉落苍月物品颜色 = (int)control.Value; break;
            case nameof(S_掉落星王物品颜色): Settings.Default.掉落星王物品颜色 = (int)control.Value; break;
            case nameof(S_掉落城主物品颜色): Settings.Default.掉落城主物品颜色 = (int)control.Value; break;
            case nameof(S_掉落书籍物品颜色): Settings.Default.掉落书籍物品颜色 = (int)control.Value; break;
            case nameof(S_DropPlayerNameColor): Settings.Default.DropPlayerNameColor = (int)control.Value; break;
            case nameof(S_狂暴击杀玩家颜色): Settings.Default.狂暴击杀玩家颜色 = (int)control.Value; break;
            case nameof(S_狂暴被杀玩家颜色): Settings.Default.狂暴被杀玩家颜色 = (int)control.Value; break;
            case nameof(S_祖玛战装备佩戴数量): Settings.Default.祖玛战装备佩戴数量 = (int)control.Value; break;
            case nameof(S_祖玛法装备佩戴数量): Settings.Default.祖玛法装备佩戴数量 = (int)control.Value; break;
            case nameof(S_祖玛道装备佩戴数量): Settings.Default.祖玛道装备佩戴数量 = (int)control.Value; break;
            case nameof(S_祖玛刺装备佩戴数量): Settings.Default.祖玛刺装备佩戴数量 = (int)control.Value; break;
            case nameof(S_祖玛弓装备佩戴数量): Settings.Default.祖玛弓装备佩戴数量 = (int)control.Value; break;
            case nameof(S_祖玛枪装备佩戴数量): Settings.Default.祖玛枪装备佩戴数量 = (int)control.Value; break;
            case nameof(S_赤月战装备佩戴数量): Settings.Default.赤月战装备佩戴数量 = (int)control.Value; break;
            case nameof(S_赤月法装备佩戴数量): Settings.Default.赤月法装备佩戴数量 = (int)control.Value; break;
            case nameof(S_赤月道装备佩戴数量): Settings.Default.赤月道装备佩戴数量 = (int)control.Value; break;
            case nameof(S_赤月刺装备佩戴数量): Settings.Default.赤月刺装备佩戴数量 = (int)control.Value; break;
            case nameof(S_赤月弓装备佩戴数量): Settings.Default.赤月弓装备佩戴数量 = (int)control.Value; break;
            case nameof(S_赤月枪装备佩戴数量): Settings.Default.赤月枪装备佩戴数量 = (int)control.Value; break;
            case nameof(S_魔龙战装备佩戴数量): Settings.Default.魔龙战装备佩戴数量 = (int)control.Value; break;
            case nameof(S_魔龙法装备佩戴数量): Settings.Default.魔龙法装备佩戴数量 = (int)control.Value; break;
            case nameof(S_魔龙道装备佩戴数量): Settings.Default.魔龙道装备佩戴数量 = (int)control.Value; break;
            case nameof(S_魔龙刺装备佩戴数量): Settings.Default.魔龙刺装备佩戴数量 = (int)control.Value; break;
            case nameof(S_魔龙弓装备佩戴数量): Settings.Default.魔龙弓装备佩戴数量 = (int)control.Value; break;
            case nameof(S_魔龙枪装备佩戴数量): Settings.Default.魔龙枪装备佩戴数量 = (int)control.Value; break;
            case nameof(S_苍月战装备佩戴数量): Settings.Default.苍月战装备佩戴数量 = (int)control.Value; break;
            case nameof(S_苍月法装备佩戴数量): Settings.Default.苍月法装备佩戴数量 = (int)control.Value; break;
            case nameof(S_苍月道装备佩戴数量): Settings.Default.苍月道装备佩戴数量 = (int)control.Value; break;
            case nameof(S_苍月刺装备佩戴数量): Settings.Default.苍月刺装备佩戴数量 = (int)control.Value; break;
            case nameof(S_苍月弓装备佩戴数量): Settings.Default.苍月弓装备佩戴数量 = (int)control.Value; break;
            case nameof(S_苍月枪装备佩戴数量): Settings.Default.苍月枪装备佩戴数量 = (int)control.Value; break;
            case nameof(S_星王战装备佩戴数量): Settings.Default.星王战装备佩戴数量 = (int)control.Value; break;
            case nameof(S_星王法装备佩戴数量): Settings.Default.星王法装备佩戴数量 = (int)control.Value; break;
            case nameof(S_星王道装备佩戴数量): Settings.Default.星王道装备佩戴数量 = (int)control.Value; break;
            case nameof(S_星王刺装备佩戴数量): Settings.Default.星王刺装备佩戴数量 = (int)control.Value; break;
            case nameof(S_星王弓装备佩戴数量): Settings.Default.星王弓装备佩戴数量 = (int)control.Value; break;
            case nameof(S_星王枪装备佩戴数量): Settings.Default.星王枪装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊1战装备佩戴数量): Settings.Default.特殊1战装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊1法装备佩戴数量): Settings.Default.特殊1法装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊1道装备佩戴数量): Settings.Default.特殊1道装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊1刺装备佩戴数量): Settings.Default.特殊1刺装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊1弓装备佩戴数量): Settings.Default.特殊1弓装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊1枪装备佩戴数量): Settings.Default.特殊1枪装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊2战装备佩戴数量): Settings.Default.特殊2战装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊2法装备佩戴数量): Settings.Default.特殊2法装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊2道装备佩戴数量): Settings.Default.特殊2道装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊2刺装备佩戴数量): Settings.Default.特殊2刺装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊2弓装备佩戴数量): Settings.Default.特殊2弓装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊2枪装备佩戴数量): Settings.Default.特殊2枪装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊3战装备佩戴数量): Settings.Default.特殊3战装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊3法装备佩戴数量): Settings.Default.特殊3法装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊3道装备佩戴数量): Settings.Default.特殊3道装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊3刺装备佩戴数量): Settings.Default.特殊3刺装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊3弓装备佩戴数量): Settings.Default.特殊3弓装备佩戴数量 = (int)control.Value; break;
            case nameof(S_特殊3枪装备佩戴数量): Settings.Default.特殊3枪装备佩戴数量 = (int)control.Value; break;
            case nameof(S_每周特惠一物品1): Settings.Default.每周特惠一物品1 = (int)control.Value; break;
            case nameof(S_每周特惠一物品2): Settings.Default.每周特惠一物品2 = (int)control.Value; break;
            case nameof(S_每周特惠一物品3): Settings.Default.每周特惠一物品3 = (int)control.Value; break;
            case nameof(S_每周特惠一物品4): Settings.Default.每周特惠一物品4 = (int)control.Value; break;
            case nameof(S_每周特惠一物品5): Settings.Default.每周特惠一物品5 = (int)control.Value; break;
            case nameof(S_每周特惠二物品1): Settings.Default.每周特惠二物品1 = (int)control.Value; break;
            case nameof(S_每周特惠二物品2): Settings.Default.每周特惠二物品2 = (int)control.Value; break;
            case nameof(S_每周特惠二物品3): Settings.Default.每周特惠二物品3 = (int)control.Value; break;
            case nameof(S_每周特惠二物品4): Settings.Default.每周特惠二物品4 = (int)control.Value; break;
            case nameof(S_每周特惠二物品5): Settings.Default.每周特惠二物品5 = (int)control.Value; break;
            case nameof(S_新手出售货币值): Settings.Default.新手出售货币值 = (int)control.Value; break;
            case nameof(S_挂机称号选项): Settings.Default.挂机称号选项 = (byte)control.Value; break;
            case nameof(S_分解称号选项): Settings.Default.分解称号选项 = (byte)control.Value; break;
            case nameof(S_法阵卡BUG清理): Settings.Default.法阵卡BUG清理 = (int)control.Value; break;
            case nameof(S_随机宝箱三物品1): Settings.Default.随机宝箱三物品1 = (int)control.Value; break;
            case nameof(S_随机宝箱三几率1): Settings.Default.随机宝箱三几率1 = (int)control.Value; break;
            case nameof(S_随机宝箱三物品2): Settings.Default.随机宝箱三物品2 = (int)control.Value; break;
            case nameof(S_随机宝箱三几率2): Settings.Default.随机宝箱三几率2 = (int)control.Value; break;
            case nameof(S_随机宝箱三物品3): Settings.Default.随机宝箱三物品3 = (int)control.Value; break;
            case nameof(S_随机宝箱三几率3): Settings.Default.随机宝箱三几率3 = (int)control.Value; break;
            case nameof(S_随机宝箱三物品4): Settings.Default.随机宝箱三物品4 = (int)control.Value; break;
            case nameof(S_随机宝箱三几率4): Settings.Default.随机宝箱三几率4 = (int)control.Value; break;
            case nameof(S_随机宝箱三物品5): Settings.Default.随机宝箱三物品5 = (int)control.Value; break;
            case nameof(S_随机宝箱三几率5): Settings.Default.随机宝箱三几率5 = (int)control.Value; break;
            case nameof(S_随机宝箱三物品6): Settings.Default.随机宝箱三物品6 = (int)control.Value; break;
            case nameof(S_随机宝箱三几率6): Settings.Default.随机宝箱三几率6 = (int)control.Value; break;
            case nameof(S_随机宝箱三物品7): Settings.Default.随机宝箱三物品7 = (int)control.Value; break;
            case nameof(S_随机宝箱三几率7): Settings.Default.随机宝箱三几率7 = (int)control.Value; break;
            case nameof(S_随机宝箱三物品8): Settings.Default.随机宝箱三物品8 = (int)control.Value; break;
            case nameof(S_随机宝箱三几率8): Settings.Default.随机宝箱三几率8 = (int)control.Value; break;
            case nameof(S_随机宝箱二物品1): Settings.Default.随机宝箱二物品1 = (int)control.Value; break;
            case nameof(S_随机宝箱二几率1): Settings.Default.随机宝箱二几率1 = (int)control.Value; break;
            case nameof(S_随机宝箱二物品2): Settings.Default.随机宝箱二物品2 = (int)control.Value; break;
            case nameof(S_随机宝箱二几率2): Settings.Default.随机宝箱二几率2 = (int)control.Value; break;
            case nameof(S_随机宝箱二物品3): Settings.Default.随机宝箱二物品3 = (int)control.Value; break;
            case nameof(S_随机宝箱二几率3): Settings.Default.随机宝箱二几率3 = (int)control.Value; break;
            case nameof(S_随机宝箱二物品4): Settings.Default.随机宝箱二物品4 = (int)control.Value; break;
            case nameof(S_随机宝箱二几率4): Settings.Default.随机宝箱二几率4 = (int)control.Value; break;
            case nameof(S_随机宝箱二物品5): Settings.Default.随机宝箱二物品5 = (int)control.Value; break;
            case nameof(S_随机宝箱二几率5): Settings.Default.随机宝箱二几率5 = (int)control.Value; break;
            case nameof(S_随机宝箱二物品6): Settings.Default.随机宝箱二物品6 = (int)control.Value; break;
            case nameof(S_随机宝箱二几率6): Settings.Default.随机宝箱二几率6 = (int)control.Value; break;
            case nameof(S_随机宝箱二物品7): Settings.Default.随机宝箱二物品7 = (int)control.Value; break;
            case nameof(S_随机宝箱二几率7): Settings.Default.随机宝箱二几率7 = (int)control.Value; break;
            case nameof(S_随机宝箱二物品8): Settings.Default.随机宝箱二物品8 = (int)control.Value; break;
            case nameof(S_随机宝箱二几率8): Settings.Default.随机宝箱二几率8 = (int)control.Value; break;
            case nameof(S_随机宝箱一物品1): Settings.Default.随机宝箱一物品1 = (int)control.Value; break;
            case nameof(S_随机宝箱一几率1): Settings.Default.随机宝箱一几率1 = (int)control.Value; break;
            case nameof(S_随机宝箱一物品2): Settings.Default.随机宝箱一物品2 = (int)control.Value; break;
            case nameof(S_随机宝箱一几率2): Settings.Default.随机宝箱一几率2 = (int)control.Value; break;
            case nameof(S_随机宝箱一物品3): Settings.Default.随机宝箱一物品3 = (int)control.Value; break;
            case nameof(S_随机宝箱一几率3): Settings.Default.随机宝箱一几率3 = (int)control.Value; break;
            case nameof(S_随机宝箱一物品4): Settings.Default.随机宝箱一物品4 = (int)control.Value; break;
            case nameof(S_随机宝箱一几率4): Settings.Default.随机宝箱一几率4 = (int)control.Value; break;
            case nameof(S_随机宝箱一物品5): Settings.Default.随机宝箱一物品5 = (int)control.Value; break;
            case nameof(S_随机宝箱一几率5): Settings.Default.随机宝箱一几率5 = (int)control.Value; break;
            case nameof(S_随机宝箱一物品6): Settings.Default.随机宝箱一物品6 = (int)control.Value; break;
            case nameof(S_随机宝箱一几率6): Settings.Default.随机宝箱一几率6 = (int)control.Value; break;
            case nameof(S_随机宝箱一物品7): Settings.Default.随机宝箱一物品7 = (int)control.Value; break;
            case nameof(S_随机宝箱一几率7): Settings.Default.随机宝箱一几率7 = (int)control.Value; break;
            case nameof(S_随机宝箱一物品8): Settings.Default.随机宝箱一物品8 = (int)control.Value; break;
            case nameof(S_随机宝箱一几率8): Settings.Default.随机宝箱一几率8 = (int)control.Value; break;
            case nameof(S_随机宝箱一数量1): Settings.Default.随机宝箱一数量1 = (int)control.Value; break;
            case nameof(S_随机宝箱一数量2): Settings.Default.随机宝箱一数量2 = (int)control.Value; break;
            case nameof(S_随机宝箱一数量3): Settings.Default.随机宝箱一数量3 = (int)control.Value; break;
            case nameof(S_随机宝箱一数量4): Settings.Default.随机宝箱一数量4 = (int)control.Value; break;
            case nameof(S_随机宝箱一数量5): Settings.Default.随机宝箱一数量5 = (int)control.Value; break;
            case nameof(S_随机宝箱一数量6): Settings.Default.随机宝箱一数量6 = (int)control.Value; break;
            case nameof(S_随机宝箱一数量7): Settings.Default.随机宝箱一数量7 = (int)control.Value; break;
            case nameof(S_随机宝箱一数量8): Settings.Default.随机宝箱一数量8 = (int)control.Value; break;
            case nameof(S_随机宝箱二数量1): Settings.Default.随机宝箱二数量1 = (int)control.Value; break;
            case nameof(S_随机宝箱二数量2): Settings.Default.随机宝箱二数量2 = (int)control.Value; break;
            case nameof(S_随机宝箱二数量3): Settings.Default.随机宝箱二数量3 = (int)control.Value; break;
            case nameof(S_随机宝箱二数量4): Settings.Default.随机宝箱二数量4 = (int)control.Value; break;
            case nameof(S_随机宝箱二数量5): Settings.Default.随机宝箱二数量5 = (int)control.Value; break;
            case nameof(S_随机宝箱二数量6): Settings.Default.随机宝箱二数量6 = (int)control.Value; break;
            case nameof(S_随机宝箱二数量7): Settings.Default.随机宝箱二数量7 = (int)control.Value; break;
            case nameof(S_随机宝箱二数量8): Settings.Default.随机宝箱二数量8 = (int)control.Value; break;
            case nameof(S_随机宝箱三数量1): Settings.Default.随机宝箱三数量1 = (int)control.Value; break;
            case nameof(S_随机宝箱三数量2): Settings.Default.随机宝箱三数量2 = (int)control.Value; break;
            case nameof(S_随机宝箱三数量3): Settings.Default.随机宝箱三数量3 = (int)control.Value; break;
            case nameof(S_随机宝箱三数量4): Settings.Default.随机宝箱三数量4 = (int)control.Value; break;
            case nameof(S_随机宝箱三数量5): Settings.Default.随机宝箱三数量5 = (int)control.Value; break;
            case nameof(S_随机宝箱三数量6): Settings.Default.随机宝箱三数量6 = (int)control.Value; break;
            case nameof(S_随机宝箱三数量7): Settings.Default.随机宝箱三数量7 = (int)control.Value; break;
            case nameof(S_随机宝箱三数量8): Settings.Default.随机宝箱三数量8 = (int)control.Value; break;
            case nameof(S_沙城地图保护): Settings.Default.沙城地图保护 = (int)control.Value; break;
            case nameof(S_NoobProtectionLevel): Settings.Default.NoobProtectionLevel = (int)control.Value; break;
            case nameof(S_新手地图保护1): Settings.Default.新手地图保护1 = (int)control.Value; break;
            case nameof(S_新手地图保护2): Settings.Default.新手地图保护2 = (int)control.Value; break;
            case nameof(S_新手地图保护3): Settings.Default.新手地图保护3 = (int)control.Value; break;
            case nameof(S_新手地图保护4): Settings.Default.新手地图保护4 = (int)control.Value; break;
            case nameof(S_新手地图保护5): Settings.Default.新手地图保护5 = (int)control.Value; break;
            case nameof(S_新手地图保护6): Settings.Default.新手地图保护6 = (int)control.Value; break;
            case nameof(S_新手地图保护7): Settings.Default.新手地图保护7 = (int)control.Value; break;
            case nameof(S_新手地图保护8): Settings.Default.新手地图保护8 = (int)control.Value; break;
            case nameof(S_新手地图保护9): Settings.Default.新手地图保护9 = (int)control.Value; break;
            case nameof(S_新手地图保护10): Settings.Default.新手地图保护10 = (int)control.Value; break;
            case nameof(S_沙巴克停止开关): Settings.Default.沙巴克停止开关 = (int)control.Value; break;
            case nameof(S_沙巴克城主称号): Settings.Default.沙巴克城主称号 = (byte)control.Value; break;
            case nameof(S_沙巴克成员称号): Settings.Default.沙巴克成员称号 = (byte)control.Value; break;
            case nameof(S_沙巴克称号领取开关): Settings.Default.沙巴克称号领取开关 = (int)control.Value; break;
            case nameof(S_通用1装备佩戴数量): Settings.Default.通用1装备佩戴数量 = (int)control.Value; break;
            case nameof(S_通用2装备佩戴数量): Settings.Default.通用2装备佩戴数量 = (int)control.Value; break;
            case nameof(S_通用3装备佩戴数量): Settings.Default.通用3装备佩戴数量 = (int)control.Value; break;
            case nameof(S_通用4装备佩戴数量): Settings.Default.通用4装备佩戴数量 = (int)control.Value; break;
            case nameof(S_通用5装备佩戴数量): Settings.Default.通用5装备佩戴数量 = (int)control.Value; break;
            case nameof(S_通用6装备佩戴数量): Settings.Default.通用6装备佩戴数量 = (int)control.Value; break;
            case nameof(S_重置屠魔副本时间): Settings.Default.重置屠魔副本时间 = (int)control.Value; break;
            case nameof(S_屠魔令回收数量): Settings.Default.屠魔令回收数量 = (int)control.Value; break;
            case nameof(S_新手上线赠送开关): Settings.Default.新手上线赠送开关 = (int)control.Value; break;
            case nameof(S_新手上线赠送物品1): Settings.Default.新手上线赠送物品1 = (int)control.Value; break;
            case nameof(S_新手上线赠送物品2): Settings.Default.新手上线赠送物品2 = (int)control.Value; break;
            case nameof(S_新手上线赠送物品3): Settings.Default.新手上线赠送物品3 = (int)control.Value; break;
            case nameof(S_新手上线赠送物品4): Settings.Default.新手上线赠送物品4 = (int)control.Value; break;
            case nameof(S_新手上线赠送物品5): Settings.Default.新手上线赠送物品5 = (int)control.Value; break;
            case nameof(S_新手上线赠送物品6): Settings.Default.新手上线赠送物品6 = (int)control.Value; break;
            case nameof(S_元宝袋新创数量1): Settings.Default.元宝袋新创数量1 = (int)control.Value; break;
            case nameof(S_元宝袋新创数量2): Settings.Default.元宝袋新创数量2 = (int)control.Value; break;
            case nameof(S_元宝袋新创数量3): Settings.Default.元宝袋新创数量3 = (int)control.Value; break;
            case nameof(S_元宝袋新创数量4): Settings.Default.元宝袋新创数量4 = (int)control.Value; break;
            case nameof(S_元宝袋新创数量5): Settings.Default.元宝袋新创数量5 = (int)control.Value; break;
            case nameof(S_高级赞助礼包1): Settings.Default.高级赞助礼包1 = (int)control.Value; break;
            case nameof(S_高级赞助礼包2): Settings.Default.高级赞助礼包2 = (int)control.Value; break;
            case nameof(S_高级赞助礼包3): Settings.Default.高级赞助礼包3 = (int)control.Value; break;
            case nameof(S_高级赞助礼包4): Settings.Default.高级赞助礼包4 = (int)control.Value; break;
            case nameof(S_高级赞助礼包5): Settings.Default.高级赞助礼包5 = (int)control.Value; break;
            case nameof(S_高级赞助礼包6): Settings.Default.高级赞助礼包6 = (int)control.Value; break;
            case nameof(S_高级赞助礼包7): Settings.Default.高级赞助礼包7 = (int)control.Value; break;
            case nameof(S_高级赞助礼包8): Settings.Default.高级赞助礼包8 = (int)control.Value; break;
            case nameof(S_高级赞助称号1): Settings.Default.高级赞助称号1 = (int)control.Value; break;
            case nameof(S_中级赞助礼包1): Settings.Default.中级赞助礼包1 = (int)control.Value; break;
            case nameof(S_中级赞助礼包2): Settings.Default.中级赞助礼包2 = (int)control.Value; break;
            case nameof(S_中级赞助礼包3): Settings.Default.中级赞助礼包3 = (int)control.Value; break;
            case nameof(S_中级赞助礼包4): Settings.Default.中级赞助礼包4 = (int)control.Value; break;
            case nameof(S_中级赞助礼包5): Settings.Default.中级赞助礼包5 = (int)control.Value; break;
            case nameof(S_中级赞助礼包6): Settings.Default.中级赞助礼包6 = (int)control.Value; break;
            case nameof(S_中级赞助礼包7): Settings.Default.中级赞助礼包7 = (int)control.Value; break;
            case nameof(S_中级赞助礼包8): Settings.Default.中级赞助礼包8 = (int)control.Value; break;
            case nameof(S_中级赞助称号1): Settings.Default.中级赞助称号1 = (int)control.Value; break;
            case nameof(S_初级赞助礼包1): Settings.Default.初级赞助礼包1 = (int)control.Value; break;
            case nameof(S_初级赞助礼包2): Settings.Default.初级赞助礼包2 = (int)control.Value; break;
            case nameof(S_初级赞助礼包3): Settings.Default.初级赞助礼包3 = (int)control.Value; break;
            case nameof(S_初级赞助礼包4): Settings.Default.初级赞助礼包4 = (int)control.Value; break;
            case nameof(S_初级赞助礼包5): Settings.Default.初级赞助礼包5 = (int)control.Value; break;
            case nameof(S_初级赞助礼包6): Settings.Default.初级赞助礼包6 = (int)control.Value; break;
            case nameof(S_初级赞助礼包7): Settings.Default.初级赞助礼包7 = (int)control.Value; break;
            case nameof(S_初级赞助礼包8): Settings.Default.初级赞助礼包8 = (int)control.Value; break;
            case nameof(S_初级赞助称号1): Settings.Default.初级赞助称号1 = (int)control.Value; break;
            case nameof(S_平台开关模式): Settings.Default.平台开关模式 = (int)control.Value; break;
            // TODO: Not used
            //case nameof(S_平台金币充值模块): Config.平台金币充值模块 = (int)control.Value; break;
            case nameof(S_平台元宝充值模块): Settings.Default.平台元宝充值模块 = (int)control.Value; break;
            case nameof(S_九层妖塔数量1): Settings.Default.九层妖塔数量1 = (int)control.Value; break;
            case nameof(S_九层妖塔数量2): Settings.Default.九层妖塔数量2 = (int)control.Value; break;
            case nameof(S_九层妖塔数量3): Settings.Default.九层妖塔数量3 = (int)control.Value; break;
            case nameof(S_九层妖塔数量4): Settings.Default.九层妖塔数量4 = (int)control.Value; break;
            case nameof(S_九层妖塔数量5): Settings.Default.九层妖塔数量5 = (int)control.Value; break;
            case nameof(S_九层妖塔数量6): Settings.Default.九层妖塔数量6 = (int)control.Value; break;
            case nameof(S_九层妖塔数量7): Settings.Default.九层妖塔数量7 = (int)control.Value; break;
            case nameof(S_九层妖塔数量8): Settings.Default.九层妖塔数量8 = (int)control.Value; break;
            case nameof(S_九层妖塔数量9): Settings.Default.九层妖塔数量9 = (int)control.Value; break;
            case nameof(S_九层妖塔副本次数): Settings.Default.九层妖塔副本次数 = (int)control.Value; break;
            case nameof(S_九层妖塔副本等级): Settings.Default.九层妖塔副本等级 = (int)control.Value; break;
            case nameof(S_九层妖塔副本物品): Settings.Default.九层妖塔副本物品 = (int)control.Value; break;
            case nameof(S_九层妖塔副本数量): Settings.Default.九层妖塔副本数量 = (int)control.Value; break;
            case nameof(S_九层妖塔副本时间小): Settings.Default.九层妖塔副本时间小 = (int)control.Value; break;
            case nameof(S_九层妖塔副本时间大): Settings.Default.九层妖塔副本时间大 = (int)control.Value; break;
            case nameof(S_AutoBattleLevel): Settings.Default.AutoBattleLevel = (byte)control.Value; break;
            case nameof(S_禁止背包铭文洗练): Settings.Default.禁止背包铭文洗练 = (byte)control.Value; break;
            case nameof(S_沙巴克禁止随机): Settings.Default.沙巴克禁止随机 = (byte)control.Value; break;
            case nameof(S_冥想丹自定义经验): Settings.Default.冥想丹自定义经验 = (int)control.Value; break;
            case nameof(S_沙巴克爆装备开关): Settings.Default.沙巴克爆装备开关 = (byte)control.Value; break;
            case nameof(S_铭文战士1挡1次数): Settings.Default.铭文战士1挡1次数 = (int)control.Value; break;
            case nameof(S_铭文战士1挡2次数): Settings.Default.铭文战士1挡2次数 = (int)control.Value; break;
            case nameof(S_铭文战士1挡3次数): Settings.Default.铭文战士1挡3次数 = (int)control.Value; break;
            case nameof(S_铭文战士2挡1次数): Settings.Default.铭文战士2挡1次数 = (int)control.Value; break;
            case nameof(S_铭文战士2挡2次数): Settings.Default.铭文战士2挡2次数 = (int)control.Value; break;
            case nameof(S_铭文战士2挡3次数): Settings.Default.铭文战士2挡3次数 = (int)control.Value; break;
            case nameof(S_铭文战士3挡1次数): Settings.Default.铭文战士3挡1次数 = (int)control.Value; break;
            case nameof(S_铭文战士3挡2次数): Settings.Default.铭文战士3挡2次数 = (int)control.Value; break;
            case nameof(S_铭文战士3挡3次数): Settings.Default.铭文战士3挡3次数 = (int)control.Value; break;
            case nameof(S_铭文战士1挡1概率): Settings.Default.铭文战士1挡1概率 = (int)control.Value; break;
            case nameof(S_铭文战士1挡2概率): Settings.Default.铭文战士1挡2概率 = (int)control.Value; break;
            case nameof(S_铭文战士1挡3概率): Settings.Default.铭文战士1挡3概率 = (int)control.Value; break;
            case nameof(S_铭文战士2挡1概率): Settings.Default.铭文战士2挡1概率 = (int)control.Value; break;
            case nameof(S_铭文战士2挡2概率): Settings.Default.铭文战士2挡2概率 = (int)control.Value; break;
            case nameof(S_铭文战士2挡3概率): Settings.Default.铭文战士2挡3概率 = (int)control.Value; break;
            case nameof(S_铭文战士3挡1概率): Settings.Default.铭文战士3挡1概率 = (int)control.Value; break;
            case nameof(S_铭文战士3挡2概率): Settings.Default.铭文战士3挡2概率 = (int)control.Value; break;
            case nameof(S_铭文战士3挡3概率): Settings.Default.铭文战士3挡3概率 = (int)control.Value; break;
            case nameof(S_铭文战士3挡技能编号): Settings.Default.铭文战士3挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文战士3挡技能铭文): Settings.Default.铭文战士3挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文战士2挡技能编号): Settings.Default.铭文战士2挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文战士2挡技能铭文): Settings.Default.铭文战士2挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文战士1挡技能编号): Settings.Default.铭文战士1挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文战士1挡技能铭文): Settings.Default.铭文战士1挡技能铭文 = (int)control.Value; break;
            case nameof(S_新手上线赠送称号1): Settings.Default.新手上线赠送称号1 = (int)control.Value; break;
            case nameof(S_铭文法师1挡1次数): Settings.Default.铭文法师1挡1次数 = (int)control.Value; break;
            case nameof(S_铭文法师1挡2次数): Settings.Default.铭文法师1挡2次数 = (int)control.Value; break;
            case nameof(S_铭文法师1挡3次数): Settings.Default.铭文法师1挡3次数 = (int)control.Value; break;
            case nameof(S_铭文法师2挡1次数): Settings.Default.铭文法师2挡1次数 = (int)control.Value; break;
            case nameof(S_铭文法师2挡2次数): Settings.Default.铭文法师2挡2次数 = (int)control.Value; break;
            case nameof(S_铭文法师2挡3次数): Settings.Default.铭文法师2挡3次数 = (int)control.Value; break;
            case nameof(S_铭文法师3挡1次数): Settings.Default.铭文法师3挡1次数 = (int)control.Value; break;
            case nameof(S_铭文法师3挡2次数): Settings.Default.铭文法师3挡2次数 = (int)control.Value; break;
            case nameof(S_铭文法师3挡3次数): Settings.Default.铭文法师3挡3次数 = (int)control.Value; break;
            case nameof(S_铭文法师1挡1概率): Settings.Default.铭文法师1挡1概率 = (int)control.Value; break;
            case nameof(S_铭文法师1挡2概率): Settings.Default.铭文法师1挡2概率 = (int)control.Value; break;
            case nameof(S_铭文法师1挡3概率): Settings.Default.铭文法师1挡3概率 = (int)control.Value; break;
            case nameof(S_铭文法师2挡1概率): Settings.Default.铭文法师2挡1概率 = (int)control.Value; break;
            case nameof(S_铭文法师2挡2概率): Settings.Default.铭文法师2挡2概率 = (int)control.Value; break;
            case nameof(S_铭文法师2挡3概率): Settings.Default.铭文法师2挡3概率 = (int)control.Value; break;
            case nameof(S_铭文法师3挡1概率): Settings.Default.铭文法师3挡1概率 = (int)control.Value; break;
            case nameof(S_铭文法师3挡2概率): Settings.Default.铭文法师3挡2概率 = (int)control.Value; break;
            case nameof(S_铭文法师3挡3概率): Settings.Default.铭文法师3挡3概率 = (int)control.Value; break;
            case nameof(S_铭文法师3挡技能编号): Settings.Default.铭文法师3挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文法师3挡技能铭文): Settings.Default.铭文法师3挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文法师2挡技能编号): Settings.Default.铭文法师2挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文法师2挡技能铭文): Settings.Default.铭文法师2挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文法师1挡技能编号): Settings.Default.铭文法师1挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文法师1挡技能铭文): Settings.Default.铭文法师1挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文道士1挡1次数): Settings.Default.铭文道士1挡1次数 = (int)control.Value; break;
            case nameof(S_铭文道士1挡2次数): Settings.Default.铭文道士1挡2次数 = (int)control.Value; break;
            case nameof(S_铭文道士1挡3次数): Settings.Default.铭文道士1挡3次数 = (int)control.Value; break;
            case nameof(S_铭文道士2挡1次数): Settings.Default.铭文道士2挡1次数 = (int)control.Value; break;
            case nameof(S_铭文道士2挡2次数): Settings.Default.铭文道士2挡2次数 = (int)control.Value; break;
            case nameof(S_铭文道士2挡3次数): Settings.Default.铭文道士2挡3次数 = (int)control.Value; break;
            case nameof(S_铭文道士3挡1次数): Settings.Default.铭文道士3挡1次数 = (int)control.Value; break;
            case nameof(S_铭文道士3挡2次数): Settings.Default.铭文道士3挡2次数 = (int)control.Value; break;
            case nameof(S_铭文道士3挡3次数): Settings.Default.铭文道士3挡3次数 = (int)control.Value; break;
            case nameof(S_铭文道士1挡1概率): Settings.Default.铭文道士1挡1概率 = (int)control.Value; break;
            case nameof(S_铭文道士1挡2概率): Settings.Default.铭文道士1挡2概率 = (int)control.Value; break;
            case nameof(S_铭文道士1挡3概率): Settings.Default.铭文道士1挡3概率 = (int)control.Value; break;
            case nameof(S_铭文道士2挡1概率): Settings.Default.铭文道士2挡1概率 = (int)control.Value; break;
            case nameof(S_铭文道士2挡2概率): Settings.Default.铭文道士2挡2概率 = (int)control.Value; break;
            case nameof(S_铭文道士2挡3概率): Settings.Default.铭文道士2挡3概率 = (int)control.Value; break;
            case nameof(S_铭文道士3挡1概率): Settings.Default.铭文道士3挡1概率 = (int)control.Value; break;
            case nameof(S_铭文道士3挡2概率): Settings.Default.铭文道士3挡2概率 = (int)control.Value; break;
            case nameof(S_铭文道士3挡3概率): Settings.Default.铭文道士3挡3概率 = (int)control.Value; break;
            case nameof(S_铭文道士3挡技能编号): Settings.Default.铭文道士3挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文道士3挡技能铭文): Settings.Default.铭文道士3挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文道士2挡技能编号): Settings.Default.铭文道士2挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文道士2挡技能铭文): Settings.Default.铭文道士2挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文道士1挡技能编号): Settings.Default.铭文道士1挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文道士1挡技能铭文): Settings.Default.铭文道士1挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文刺客1挡1次数): Settings.Default.铭文刺客1挡1次数 = (int)control.Value; break;
            case nameof(S_铭文刺客1挡2次数): Settings.Default.铭文刺客1挡2次数 = (int)control.Value; break;
            case nameof(S_铭文刺客1挡3次数): Settings.Default.铭文刺客1挡3次数 = (int)control.Value; break;
            case nameof(S_铭文刺客2挡1次数): Settings.Default.铭文刺客2挡1次数 = (int)control.Value; break;
            case nameof(S_铭文刺客2挡2次数): Settings.Default.铭文刺客2挡2次数 = (int)control.Value; break;
            case nameof(S_铭文刺客2挡3次数): Settings.Default.铭文刺客2挡3次数 = (int)control.Value; break;
            case nameof(S_铭文刺客3挡1次数): Settings.Default.铭文刺客3挡1次数 = (int)control.Value; break;
            case nameof(S_铭文刺客3挡2次数): Settings.Default.铭文刺客3挡2次数 = (int)control.Value; break;
            case nameof(S_铭文刺客3挡3次数): Settings.Default.铭文刺客3挡3次数 = (int)control.Value; break;
            case nameof(S_铭文刺客1挡1概率): Settings.Default.铭文刺客1挡1概率 = (int)control.Value; break;
            case nameof(S_铭文刺客1挡2概率): Settings.Default.铭文刺客1挡2概率 = (int)control.Value; break;
            case nameof(S_铭文刺客1挡3概率): Settings.Default.铭文刺客1挡3概率 = (int)control.Value; break;
            case nameof(S_铭文刺客2挡1概率): Settings.Default.铭文刺客2挡1概率 = (int)control.Value; break;
            case nameof(S_铭文刺客2挡2概率): Settings.Default.铭文刺客2挡2概率 = (int)control.Value; break;
            case nameof(S_铭文刺客2挡3概率): Settings.Default.铭文刺客2挡3概率 = (int)control.Value; break;
            case nameof(S_铭文刺客3挡1概率): Settings.Default.铭文刺客3挡1概率 = (int)control.Value; break;
            case nameof(S_铭文刺客3挡2概率): Settings.Default.铭文刺客3挡2概率 = (int)control.Value; break;
            case nameof(S_铭文刺客3挡3概率): Settings.Default.铭文刺客3挡3概率 = (int)control.Value; break;
            case nameof(S_铭文刺客3挡技能编号): Settings.Default.铭文刺客3挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文刺客3挡技能铭文): Settings.Default.铭文刺客3挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文刺客2挡技能编号): Settings.Default.铭文刺客2挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文刺客2挡技能铭文): Settings.Default.铭文刺客2挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文刺客1挡技能编号): Settings.Default.铭文刺客1挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文刺客1挡技能铭文): Settings.Default.铭文刺客1挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文弓手1挡1次数): Settings.Default.铭文弓手1挡1次数 = (int)control.Value; break;
            case nameof(S_铭文弓手1挡2次数): Settings.Default.铭文弓手1挡2次数 = (int)control.Value; break;
            case nameof(S_铭文弓手1挡3次数): Settings.Default.铭文弓手1挡3次数 = (int)control.Value; break;
            case nameof(S_铭文弓手2挡1次数): Settings.Default.铭文弓手2挡1次数 = (int)control.Value; break;
            case nameof(S_铭文弓手2挡2次数): Settings.Default.铭文弓手2挡2次数 = (int)control.Value; break;
            case nameof(S_铭文弓手2挡3次数): Settings.Default.铭文弓手2挡3次数 = (int)control.Value; break;
            case nameof(S_铭文弓手3挡1次数): Settings.Default.铭文弓手3挡1次数 = (int)control.Value; break;
            case nameof(S_铭文弓手3挡2次数): Settings.Default.铭文弓手3挡2次数 = (int)control.Value; break;
            case nameof(S_铭文弓手3挡3次数): Settings.Default.铭文弓手3挡3次数 = (int)control.Value; break;
            case nameof(S_铭文弓手1挡1概率): Settings.Default.铭文弓手1挡1概率 = (int)control.Value; break;
            case nameof(S_铭文弓手1挡2概率): Settings.Default.铭文弓手1挡2概率 = (int)control.Value; break;
            case nameof(S_铭文弓手1挡3概率): Settings.Default.铭文弓手1挡3概率 = (int)control.Value; break;
            case nameof(S_铭文弓手2挡1概率): Settings.Default.铭文弓手2挡1概率 = (int)control.Value; break;
            case nameof(S_铭文弓手2挡2概率): Settings.Default.铭文弓手2挡2概率 = (int)control.Value; break;
            case nameof(S_铭文弓手2挡3概率): Settings.Default.铭文弓手2挡3概率 = (int)control.Value; break;
            case nameof(S_铭文弓手3挡1概率): Settings.Default.铭文弓手3挡1概率 = (int)control.Value; break;
            case nameof(S_铭文弓手3挡2概率): Settings.Default.铭文弓手3挡2概率 = (int)control.Value; break;
            case nameof(S_铭文弓手3挡3概率): Settings.Default.铭文弓手3挡3概率 = (int)control.Value; break;
            case nameof(S_铭文弓手3挡技能编号): Settings.Default.铭文弓手3挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文弓手3挡技能铭文): Settings.Default.铭文弓手3挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文弓手2挡技能编号): Settings.Default.铭文弓手2挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文弓手2挡技能铭文): Settings.Default.铭文弓手2挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文弓手1挡技能编号): Settings.Default.铭文弓手1挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文弓手1挡技能铭文): Settings.Default.铭文弓手1挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文龙枪1挡1次数): Settings.Default.铭文龙枪1挡1次数 = (int)control.Value; break;
            case nameof(S_铭文龙枪1挡2次数): Settings.Default.铭文龙枪1挡2次数 = (int)control.Value; break;
            case nameof(S_铭文龙枪1挡3次数): Settings.Default.铭文龙枪1挡3次数 = (int)control.Value; break;
            case nameof(S_铭文龙枪2挡1次数): Settings.Default.铭文龙枪2挡1次数 = (int)control.Value; break;
            case nameof(S_铭文龙枪2挡2次数): Settings.Default.铭文龙枪2挡2次数 = (int)control.Value; break;
            case nameof(S_铭文龙枪2挡3次数): Settings.Default.铭文龙枪2挡3次数 = (int)control.Value; break;
            case nameof(S_铭文龙枪3挡1次数): Settings.Default.铭文龙枪3挡1次数 = (int)control.Value; break;
            case nameof(S_铭文龙枪3挡2次数): Settings.Default.铭文龙枪3挡2次数 = (int)control.Value; break;
            case nameof(S_铭文龙枪3挡3次数): Settings.Default.铭文龙枪3挡3次数 = (int)control.Value; break;
            case nameof(S_铭文龙枪1挡1概率): Settings.Default.铭文龙枪1挡1概率 = (int)control.Value; break;
            case nameof(S_铭文龙枪1挡2概率): Settings.Default.铭文龙枪1挡2概率 = (int)control.Value; break;
            case nameof(S_铭文龙枪1挡3概率): Settings.Default.铭文龙枪1挡3概率 = (int)control.Value; break;
            case nameof(S_铭文龙枪2挡1概率): Settings.Default.铭文龙枪2挡1概率 = (int)control.Value; break;
            case nameof(S_铭文龙枪2挡2概率): Settings.Default.铭文龙枪2挡2概率 = (int)control.Value; break;
            case nameof(S_铭文龙枪2挡3概率): Settings.Default.铭文龙枪2挡3概率 = (int)control.Value; break;
            case nameof(S_铭文龙枪3挡1概率): Settings.Default.铭文龙枪3挡1概率 = (int)control.Value; break;
            case nameof(S_铭文龙枪3挡2概率): Settings.Default.铭文龙枪3挡2概率 = (int)control.Value; break;
            case nameof(S_铭文龙枪3挡3概率): Settings.Default.铭文龙枪3挡3概率 = (int)control.Value; break;
            case nameof(S_铭文龙枪3挡技能编号): Settings.Default.铭文龙枪3挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文龙枪3挡技能铭文): Settings.Default.铭文龙枪3挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文龙枪2挡技能编号): Settings.Default.铭文龙枪2挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文龙枪2挡技能铭文): Settings.Default.铭文龙枪2挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文龙枪1挡技能编号): Settings.Default.铭文龙枪1挡技能编号 = (int)control.Value; break;
            case nameof(S_铭文龙枪1挡技能铭文): Settings.Default.铭文龙枪1挡技能铭文 = (int)control.Value; break;
            case nameof(S_铭文道士保底开关): Settings.Default.铭文道士保底开关 = (int)control.Value; break;
            case nameof(S_铭文龙枪保底开关): Settings.Default.铭文龙枪保底开关 = (int)control.Value; break;
            case nameof(S_铭文战士保底开关): Settings.Default.铭文战士保底开关 = (int)control.Value; break;
            case nameof(S_铭文法师保底开关): Settings.Default.铭文法师保底开关 = (int)control.Value; break;
            case nameof(S_铭文刺客保底开关): Settings.Default.铭文刺客保底开关 = (int)control.Value; break;
            case nameof(S_铭文弓手保底开关): Settings.Default.铭文弓手保底开关 = (int)control.Value; break;
            case nameof(S_DropRateModifier): Settings.Default.DropRateModifier = (int)control.Value; break;
            case nameof(S_魔虫窟副本次数): Settings.Default.魔虫窟副本次数 = (int)control.Value; break;
            case nameof(S_魔虫窟副本等级): Settings.Default.魔虫窟副本等级 = (int)control.Value; break;
            case nameof(S_魔虫窟副本物品): Settings.Default.魔虫窟副本物品 = (int)control.Value; break;
            case nameof(S_魔虫窟副本数量): Settings.Default.魔虫窟副本数量 = (int)control.Value; break;
            case nameof(S_魔虫窟副本时间小): Settings.Default.魔虫窟副本时间小 = (int)control.Value; break;
            case nameof(S_魔虫窟副本时间大): Settings.Default.魔虫窟副本时间大 = (int)control.Value; break;
            case nameof(S_幸运洗练次数保底): Settings.Default.幸运洗练次数保底 = (int)control.Value; break;
            case nameof(S_幸运洗练点数): Settings.Default.幸运洗练点数 = (int)control.Value; break;
            case nameof(S_武器强化消耗货币值): Settings.Default.武器强化消耗货币值 = (int)control.Value; break;
            case nameof(S_武器强化消耗货币开关): Settings.Default.武器强化消耗货币开关 = (int)control.Value; break;
            case nameof(S_武器强化取回时间): Settings.Default.武器强化取回时间 = (int)control.Value; break;
            case nameof(S_幸运额外1值): Settings.Default.幸运额外1值 = (int)control.Value; break;
            case nameof(S_幸运额外2值): Settings.Default.幸运额外2值 = (int)control.Value; break;
            case nameof(S_幸运额外3值): Settings.Default.幸运额外3值 = (int)control.Value; break;
            case nameof(S_幸运额外4值): Settings.Default.幸运额外4值 = (int)control.Value; break;
            case nameof(S_幸运额外5值): Settings.Default.幸运额外5值 = (int)control.Value; break;
            case nameof(S_幸运额外1伤害): Settings.Default.幸运额外1伤害 = (float)control.Value; break;
            case nameof(S_幸运额外2伤害): Settings.Default.幸运额外2伤害 = (float)control.Value; break;
            case nameof(S_幸运额外3伤害): Settings.Default.幸运额外3伤害 = (float)control.Value; break;
            case nameof(S_幸运额外4伤害): Settings.Default.幸运额外4伤害 = (float)control.Value; break;
            case nameof(S_幸运额外5伤害): Settings.Default.幸运额外5伤害 = (float)control.Value; break;
            case nameof(S_暗之门地图1): Settings.Default.暗之门地图1 = (int)control.Value; break;
            case nameof(S_暗之门地图2): Settings.Default.暗之门地图2 = (int)control.Value; break;
            case nameof(S_暗之门地图3): Settings.Default.暗之门地图3 = (int)control.Value; break;
            case nameof(S_暗之门地图4): Settings.Default.暗之门地图4 = (int)control.Value; break;
            case nameof(S_暗之门全服提示): Settings.Default.暗之门全服提示 = (int)control.Value; break;
            case nameof(S_暗之门杀怪触发): Settings.Default.暗之门杀怪触发 = (int)control.Value; break;
            case nameof(S_暗之门时间): Settings.Default.暗之门时间 = (int)control.Value; break;
            case nameof(S_暗之门地图1X): Settings.Default.暗之门地图1X = (int)control.Value; break;
            case nameof(S_暗之门地图1Y): Settings.Default.暗之门地图1Y = (int)control.Value; break;
            case nameof(S_暗之门地图2X): Settings.Default.暗之门地图2X = (int)control.Value; break;
            case nameof(S_暗之门地图2Y): Settings.Default.暗之门地图2Y = (int)control.Value; break;
            case nameof(S_暗之门地图3X): Settings.Default.暗之门地图3X = (int)control.Value; break;
            case nameof(S_暗之门地图3Y): Settings.Default.暗之门地图3Y = (int)control.Value; break;
            case nameof(S_暗之门地图4X): Settings.Default.暗之门地图4X = (int)control.Value; break;
            case nameof(S_暗之门地图4Y): Settings.Default.暗之门地图4Y = (int)control.Value; break;
            case nameof(S_暗之门开关): Settings.Default.暗之门开关 = (int)control.Value; break;
            case nameof(S_监狱货币): Settings.Default.监狱货币 = (int)control.Value; break;
            case nameof(S_监狱货币类型): Settings.Default.监狱货币类型 = (int)control.Value; break;
            case nameof(S_魔虫窟分钟限制): Settings.Default.魔虫窟分钟限制 = (int)control.Value; break;
            case nameof(S_自定义元宝兑换01): Settings.Default.自定义元宝兑换01 = (int)control.Value; break;
            case nameof(S_自定义元宝兑换02): Settings.Default.自定义元宝兑换02 = (int)control.Value; break;
            case nameof(S_自定义元宝兑换03): Settings.Default.自定义元宝兑换03 = (int)control.Value; break;
            case nameof(S_自定义元宝兑换04): Settings.Default.自定义元宝兑换04 = (int)control.Value; break;
            case nameof(S_自定义元宝兑换05): Settings.Default.自定义元宝兑换05 = (int)control.Value; break;
            case nameof(S_直升物品1): Settings.Default.直升物品1 = (int)control.Value; break;
            case nameof(S_直升物品2): Settings.Default.直升物品2 = (int)control.Value; break;
            case nameof(S_直升物品3): Settings.Default.直升物品3 = (int)control.Value; break;
            case nameof(S_直升物品4): Settings.Default.直升物品4 = (int)control.Value; break;
            case nameof(S_直升物品5): Settings.Default.直升物品5 = (int)control.Value; break;
            case nameof(S_直升物品6): Settings.Default.直升物品6 = (int)control.Value; break;
            case nameof(S_直升物品7): Settings.Default.直升物品7 = (int)control.Value; break;
            case nameof(S_直升物品8): Settings.Default.直升物品8 = (int)control.Value; break;
            case nameof(S_直升物品9): Settings.Default.直升物品9 = (int)control.Value; break;
            case nameof(S_直升等级1): Settings.Default.直升等级1 = (int)control.Value; break;
            case nameof(S_直升等级2): Settings.Default.直升等级2 = (int)control.Value; break;
            case nameof(S_直升等级3): Settings.Default.直升等级3 = (int)control.Value; break;
            case nameof(S_直升等级4): Settings.Default.直升等级4 = (int)control.Value; break;
            case nameof(S_直升等级5): Settings.Default.直升等级5 = (int)control.Value; break;
            case nameof(S_直升等级6): Settings.Default.直升等级6 = (int)control.Value; break;
            case nameof(S_直升等级7): Settings.Default.直升等级7 = (int)control.Value; break;
            case nameof(S_直升等级8): Settings.Default.直升等级8 = (int)control.Value; break;
            case nameof(S_直升等级9): Settings.Default.直升等级9 = (int)control.Value; break;
            case nameof(S_直升经验1): Settings.Default.直升经验1 = (int)control.Value; break;
            case nameof(S_直升经验2): Settings.Default.直升经验2 = (int)control.Value; break;
            case nameof(S_直升经验3): Settings.Default.直升经验3 = (int)control.Value; break;
            case nameof(S_直升经验4): Settings.Default.直升经验4 = (int)control.Value; break;
            case nameof(S_直升经验5): Settings.Default.直升经验5 = (int)control.Value; break;
            case nameof(S_直升经验6): Settings.Default.直升经验6 = (int)control.Value; break;
            case nameof(S_直升经验7): Settings.Default.直升经验7 = (int)control.Value; break;
            case nameof(S_直升经验8): Settings.Default.直升经验8 = (int)control.Value; break;
            case nameof(S_直升经验9): Settings.Default.直升经验9 = (int)control.Value; break;
            case nameof(S_RechargeSystemFormat): Settings.Default.RechargeSystemFormat = (int)control.Value; break;
            case nameof(DefaultSkillLevel): Settings.Default.DefaultSkillLevel = (int)control.Value; break;
            case nameof(S_其他分解几率一): Settings.Default.其他分解几率一 = (int)control.Value; break;
            case nameof(S_其他分解几率二): Settings.Default.其他分解几率二 = (int)control.Value; break;
            case nameof(S_其他分解几率三): Settings.Default.其他分解几率三 = (int)control.Value; break;
            case nameof(S_其他分解几率四): Settings.Default.其他分解几率四 = (int)control.Value; break;
            case nameof(S_其他分解数量一): Settings.Default.其他分解数量一 = (int)control.Value; break;
            case nameof(S_其他分解数量二): Settings.Default.其他分解数量二 = (int)control.Value; break;
            case nameof(S_其他分解数量三): Settings.Default.其他分解数量三 = (int)control.Value; break;
            case nameof(S_其他分解数量四): Settings.Default.其他分解数量四 = (int)control.Value; break;
            case nameof(S_其他分解开关): Settings.Default.其他分解开关 = (int)control.Value; break;
            case nameof(S_沃玛分解几率一): Settings.Default.沃玛分解几率一 = (int)control.Value; break;
            case nameof(S_沃玛分解几率二): Settings.Default.沃玛分解几率二 = (int)control.Value; break;
            case nameof(S_沃玛分解几率三): Settings.Default.沃玛分解几率三 = (int)control.Value; break;
            case nameof(S_沃玛分解几率四): Settings.Default.沃玛分解几率四 = (int)control.Value; break;
            case nameof(S_沃玛分解数量一): Settings.Default.沃玛分解数量一 = (int)control.Value; break;
            case nameof(S_沃玛分解数量二): Settings.Default.沃玛分解数量二 = (int)control.Value; break;
            case nameof(S_沃玛分解数量三): Settings.Default.沃玛分解数量三 = (int)control.Value; break;
            case nameof(S_沃玛分解数量四): Settings.Default.沃玛分解数量四 = (int)control.Value; break;
            case nameof(S_沃玛分解开关): Settings.Default.沃玛分解开关 = (int)control.Value; break;
            case nameof(拾取地图控制1): Settings.Default.AutoPickUpMap1 = (int)control.Value; break;
            case nameof(拾取地图控制2): Settings.Default.AutoPickUpMap2 = (int)control.Value; break;
            case nameof(拾取地图控制3): Settings.Default.AutoPickUpMap3 = (int)control.Value; break;
            case nameof(拾取地图控制4): Settings.Default.AutoPickUpMap4 = (int)control.Value; break;
            case nameof(拾取地图控制5): Settings.Default.AutoPickUpMap5 = (int)control.Value; break;
            case nameof(拾取地图控制6): Settings.Default.AutoPickUpMap6 = (int)control.Value; break;
            case nameof(拾取地图控制7): Settings.Default.AutoPickUpMap7 = (int)control.Value; break;
            case nameof(拾取地图控制8): Settings.Default.AutoPickUpMap8 = (int)control.Value; break;
            case nameof(沙城捐献货币类型): Settings.Default.沙城捐献货币类型 = (int)control.Value; break;
            case nameof(沙城捐献支付数量): Settings.Default.沙城捐献支付数量 = (int)control.Value; break;
            case nameof(沙城捐献获得物品1): Settings.Default.沙城捐献获得物品1 = (int)control.Value; break;
            case nameof(沙城捐献获得物品2): Settings.Default.沙城捐献获得物品2 = (int)control.Value; break;
            case nameof(沙城捐献获得物品3): Settings.Default.沙城捐献获得物品3 = (int)control.Value; break;
            case nameof(沙城捐献物品数量1): Settings.Default.沙城捐献物品数量1 = (int)control.Value; break;
            case nameof(沙城捐献物品数量2): Settings.Default.沙城捐献物品数量2 = (int)control.Value; break;
            case nameof(沙城捐献物品数量3): Settings.Default.沙城捐献物品数量3 = (int)control.Value; break;
            case nameof(沙城捐献赞助金额): Settings.Default.沙城捐献赞助金额 = (int)control.Value; break;
            case nameof(沙城捐献赞助人数): Settings.Default.沙城捐献赞助人数 = (int)control.Value; break;
            case nameof(雕爷激活灵符需求): Settings.Default.雕爷激活灵符需求 = (int)control.Value; break;
            case nameof(雕爷1号位灵符): Settings.Default.雕爷1号位灵符 = (int)control.Value; break;
            case nameof(雕爷2号位灵符): Settings.Default.雕爷2号位灵符 = (int)control.Value; break;
            case nameof(雕爷3号位灵符): Settings.Default.雕爷3号位灵符 = (int)control.Value; break;
            case nameof(雕爷1号位铭文石): Settings.Default.雕爷1号位铭文石 = (int)control.Value; break;
            case nameof(雕爷2号位铭文石): Settings.Default.雕爷2号位铭文石 = (int)control.Value; break;
            case nameof(雕爷3号位铭文石): Settings.Default.雕爷3号位铭文石 = (int)control.Value; break;
            case nameof(S_称号范围拾取判断1): Settings.Default.称号范围拾取判断1 = (int)control.Value; break;
            case nameof(九层妖塔统计开关): Settings.Default.九层妖塔统计开关 = (int)control.Value; break;
            case nameof(沙巴克每周攻沙时间): Settings.Default.沙巴克每周攻沙时间 = (int)control.Value; break;
            case nameof(沙巴克皇宫传送等级): Settings.Default.沙巴克皇宫传送等级 = (int)control.Value; break;
            case nameof(沙巴克皇宫传送物品): Settings.Default.沙巴克皇宫传送物品 = (int)control.Value; break;
            case nameof(沙巴克皇宫传送数量): Settings.Default.沙巴克皇宫传送数量 = (int)control.Value; break;
            case nameof(系统窗口发送): Settings.Default.系统窗口发送 = (int)control.Value; break;
            case nameof(龙卫效果提示): Settings.Default.龙卫效果提示 = (int)control.Value; break;
            case nameof(AllowRecharge): Settings.Default.AllowRecharge = (int)control.Value; break;
            case nameof(全服红包等级): Settings.Default.全服红包等级 = (int)control.Value; break;
            case nameof(全服红包时间): Settings.Default.全服红包时间 = (int)control.Value; break;
            case nameof(全服红包货币类型): Settings.Default.GlobalBonusCurrencyType = (int)control.Value; break;
            case nameof(全服红包货币数量): Settings.Default.全服红包货币数量 = (int)control.Value; break;
            case nameof(龙卫蓝色词条概率): Settings.Default.龙卫蓝色词条概率 = (int)control.Value; break;
            case nameof(龙卫紫色词条概率): Settings.Default.龙卫紫色词条概率 = (int)control.Value; break;
            case nameof(龙卫橙色词条概率): Settings.Default.龙卫橙色词条概率 = (int)control.Value; break;
            case nameof(自定义初始货币类型): Settings.Default.自定义初始货币类型 = (int)control.Value; break;
            case nameof(会员物品对接): Settings.Default.会员物品对接 = (int)control.Value; break;
            case nameof(称号叠加模块9): Settings.Default.称号叠加模块9 = (byte)control.Value; break;
            case nameof(称号叠加模块10): Settings.Default.称号叠加模块10 = (byte)control.Value; break;
            case nameof(称号叠加模块11): Settings.Default.称号叠加模块11 = (byte)control.Value; break;
            case nameof(称号叠加模块12): Settings.Default.称号叠加模块12 = (byte)control.Value; break;
            case nameof(称号叠加模块13): Settings.Default.称号叠加模块13 = (byte)control.Value; break;
            case nameof(称号叠加模块14): Settings.Default.称号叠加模块14 = (byte)control.Value; break;
            case nameof(称号叠加模块15): Settings.Default.称号叠加模块15 = (byte)control.Value; break;
            case nameof(称号叠加模块16): Settings.Default.称号叠加模块16 = (byte)control.Value; break;
            case nameof(变性等级): Settings.Default.变性等级 = (int)control.Value; break;
            case nameof(变性货币类型): Settings.Default.变性货币类型 = (int)control.Value; break;
            case nameof(变性货币值): Settings.Default.变性货币值 = (int)control.Value; break;
            case nameof(变性物品ID): Settings.Default.变性物品ID = (int)control.Value; break;
            case nameof(变性物品数量): Settings.Default.变性物品数量 = (int)control.Value; break;
            case nameof(龙卫焰焚烈火剑法): Settings.Default.龙卫焰焚烈火剑法 = (int)control.Value; break;
            case nameof(屠魔殿等级限制): Settings.Default.屠魔殿等级限制 = (int)control.Value; break;
            case nameof(职业等级): Settings.Default.职业等级 = (int)control.Value; break;
            case nameof(RaceChangeCurrencyType): Settings.Default.RaceChangeCurrencyType = (int)control.Value; break;
            case nameof(RaceChangeCurrencyValue): Settings.Default.RaceChangeCurrencyValue = (int)control.Value; break;
            case nameof(RaceChangeItemID): Settings.Default.RaceChangeItemID = (int)control.Value; break;
            case nameof(RaceChangeItemQuantity): Settings.Default.RaceChangeItemQuantity = (int)control.Value; break;

            default:
                MessageBox.Show("Unknown Control! " + control.Name);
                break;
        }
        Settings.Default.Save();
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
            case nameof(自动回收设置): Settings.Default.自动回收设置 = control.Checked; break;
            case nameof(购买狂暴之力): Settings.Default.购买狂暴之力 = control.Checked; break;
            case nameof(会员满血设置): Settings.Default.会员满血设置 = control.Checked; break;
            case nameof(全屏拾取开关): Settings.Default.AutoPickUpAllVisible = control.Checked; break;
            case nameof(打开随时仓库): Settings.Default.打开随时仓库 = control.Checked; break;
            case nameof(幸运保底开关): Settings.Default.幸运保底开关 = control.Checked; break;
            case nameof(红包开关): Settings.Default.红包开关 = control.Checked; break;
            case nameof(安全区收刀开关): Settings.Default.安全区收刀开关 = control.Checked; break;

            default:
                MessageBox.Show("Unknown Control! " + control.Name);
                break;
        }
        Settings.Default.Save();
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
            case nameof(S_狂暴名称): Settings.Default.狂暴名称 = control.Text; break;
            case nameof(S_自定义物品内容一): Settings.Default.自定义物品内容一 = control.Text; break;
            case nameof(S_自定义物品内容二): Settings.Default.自定义物品内容二 = control.Text; break;
            case nameof(S_自定义物品内容三): Settings.Default.自定义物品内容三 = control.Text; break;
            case nameof(S_自定义物品内容四): Settings.Default.自定义物品内容四 = control.Text; break;
            case nameof(S_自定义物品内容五): Settings.Default.自定义物品内容五 = control.Text; break;
            case nameof(S_战将特权礼包): Settings.Default.战将特权礼包 = control.Text; break;
            case nameof(S_豪杰特权礼包): Settings.Default.豪杰特权礼包 = control.Text; break;
            case nameof(S_世界BOSS名字): Settings.Default.WorldBossName = control.Text; break;
            case nameof(S_祖玛分解物品一): Settings.Default.祖玛分解物品一 = control.Text; break;
            case nameof(S_祖玛分解物品二): Settings.Default.祖玛分解物品二 = control.Text; break;
            case nameof(S_祖玛分解物品三): Settings.Default.祖玛分解物品三 = control.Text; break;
            case nameof(S_祖玛分解物品四): Settings.Default.祖玛分解物品四 = control.Text; break;
            case nameof(S_赤月分解物品一): Settings.Default.赤月分解物品一 = control.Text; break;
            case nameof(S_赤月分解物品二): Settings.Default.赤月分解物品二 = control.Text; break;
            case nameof(S_赤月分解物品三): Settings.Default.赤月分解物品三 = control.Text; break;
            case nameof(S_赤月分解物品四): Settings.Default.赤月分解物品四 = control.Text; break;
            case nameof(S_魔龙分解物品一): Settings.Default.魔龙分解物品一 = control.Text; break;
            case nameof(S_魔龙分解物品二): Settings.Default.魔龙分解物品二 = control.Text; break;
            case nameof(S_魔龙分解物品三): Settings.Default.魔龙分解物品三 = control.Text; break;
            case nameof(S_魔龙分解物品四): Settings.Default.魔龙分解物品四 = control.Text; break;
            case nameof(S_苍月分解物品一): Settings.Default.苍月分解物品一 = control.Text; break;
            case nameof(S_苍月分解物品二): Settings.Default.苍月分解物品二 = control.Text; break;
            case nameof(S_苍月分解物品三): Settings.Default.苍月分解物品三 = control.Text; break;
            case nameof(S_苍月分解物品四): Settings.Default.苍月分解物品四 = control.Text; break;
            case nameof(S_星王分解物品一): Settings.Default.星王分解物品一 = control.Text; break;
            case nameof(S_星王分解物品二): Settings.Default.星王分解物品二 = control.Text; break;
            case nameof(S_星王分解物品三): Settings.Default.星王分解物品三 = control.Text; break;
            case nameof(S_星王分解物品四): Settings.Default.星王分解物品四 = control.Text; break;
            case nameof(S_城主分解物品一): Settings.Default.城主分解物品一 = control.Text; break;
            case nameof(S_城主分解物品二): Settings.Default.城主分解物品二 = control.Text; break;
            case nameof(S_城主分解物品三): Settings.Default.城主分解物品三 = control.Text; break;
            case nameof(S_城主分解物品四): Settings.Default.城主分解物品四 = control.Text; break;
            case nameof(S_BOSS卷轴怪物一): Settings.Default.BOSS卷轴怪物一 = control.Text; break;
            case nameof(S_BOSS卷轴怪物二): Settings.Default.BOSS卷轴怪物二 = control.Text; break;
            case nameof(S_BOSS卷轴怪物三): Settings.Default.BOSS卷轴怪物三 = control.Text; break;
            case nameof(S_BOSS卷轴怪物四): Settings.Default.BOSS卷轴怪物四 = control.Text; break;
            case nameof(S_BOSS卷轴怪物五): Settings.Default.BOSS卷轴怪物五 = control.Text; break;
            case nameof(S_BOSS卷轴怪物六): Settings.Default.BOSS卷轴怪物六 = control.Text; break;
            case nameof(S_BOSS卷轴怪物七): Settings.Default.BOSS卷轴怪物七 = control.Text; break;
            case nameof(S_BOSS卷轴怪物八): Settings.Default.BOSS卷轴怪物八 = control.Text; break;
            case nameof(S_BOSS卷轴怪物九): Settings.Default.BOSS卷轴怪物九 = control.Text; break;
            case nameof(S_BOSS卷轴怪物十): Settings.Default.BOSS卷轴怪物十 = control.Text; break;
            case nameof(S_BOSS卷轴怪物11): Settings.Default.BOSS卷轴怪物11 = control.Text; break;
            case nameof(S_BOSS卷轴怪物12): Settings.Default.BOSS卷轴怪物12 = control.Text; break;
            case nameof(S_BOSS卷轴怪物13): Settings.Default.BOSS卷轴怪物13 = control.Text; break;
            case nameof(S_BOSS卷轴怪物14): Settings.Default.BOSS卷轴怪物14 = control.Text; break;
            case nameof(S_BOSS卷轴怪物15): Settings.Default.BOSS卷轴怪物15 = control.Text; break;
            case nameof(S_BOSS卷轴怪物16): Settings.Default.BOSS卷轴怪物16 = control.Text; break;
            case nameof(S_九层妖塔BOSS1): Settings.Default.九层妖塔BOSS1 = control.Text; break;
            case nameof(S_九层妖塔BOSS2): Settings.Default.九层妖塔BOSS2 = control.Text; break;
            case nameof(S_九层妖塔BOSS3): Settings.Default.九层妖塔BOSS3 = control.Text; break;
            case nameof(S_九层妖塔BOSS4): Settings.Default.九层妖塔BOSS4 = control.Text; break;
            case nameof(S_九层妖塔BOSS5): Settings.Default.九层妖塔BOSS5 = control.Text; break;
            case nameof(S_九层妖塔BOSS6): Settings.Default.九层妖塔BOSS6 = control.Text; break;
            case nameof(S_九层妖塔BOSS7): Settings.Default.九层妖塔BOSS7 = control.Text; break;
            case nameof(S_九层妖塔BOSS8): Settings.Default.九层妖塔BOSS8 = control.Text; break;
            case nameof(S_九层妖塔BOSS9): Settings.Default.九层妖塔BOSS9 = control.Text; break;
            case nameof(S_九层妖塔精英1): Settings.Default.九层妖塔精英1 = control.Text; break;
            case nameof(S_九层妖塔精英2): Settings.Default.九层妖塔精英2 = control.Text; break;
            case nameof(S_九层妖塔精英3): Settings.Default.九层妖塔精英3 = control.Text; break;
            case nameof(S_九层妖塔精英4): Settings.Default.九层妖塔精英4 = control.Text; break;
            case nameof(S_九层妖塔精英5): Settings.Default.九层妖塔精英5 = control.Text; break;
            case nameof(S_九层妖塔精英6): Settings.Default.九层妖塔精英6 = control.Text; break;
            case nameof(S_九层妖塔精英7): Settings.Default.九层妖塔精英7 = control.Text; break;
            case nameof(S_九层妖塔精英8): Settings.Default.九层妖塔精英8 = control.Text; break;
            case nameof(S_九层妖塔精英9): Settings.Default.九层妖塔精英9 = control.Text; break;
            case nameof(S_书店商贩物品): Settings.Default.书店商贩物品 = control.Text; break;
            case nameof(S_挂机权限选项): Settings.Default.挂机权限选项 = control.Text; break;
            case nameof(S_暗之门地图1BOSS): Settings.Default.暗之门地图1BOSS = control.Text; break;
            case nameof(S_暗之门地图2BOSS): Settings.Default.暗之门地图2BOSS = control.Text; break;
            case nameof(S_暗之门地图3BOSS): Settings.Default.暗之门地图3BOSS = control.Text; break;
            case nameof(S_暗之门地图4BOSS): Settings.Default.暗之门地图4BOSS = control.Text; break;
            case nameof(S_沃玛分解物品一): Settings.Default.沃玛分解物品一 = control.Text; break;
            case nameof(S_沃玛分解物品二): Settings.Default.沃玛分解物品二 = control.Text; break;
            case nameof(S_沃玛分解物品三): Settings.Default.沃玛分解物品三 = control.Text; break;
            case nameof(S_沃玛分解物品四): Settings.Default.沃玛分解物品四 = control.Text; break;
            case nameof(S_其他分解物品一): Settings.Default.其他分解物品一 = control.Text; break;
            case nameof(S_其他分解物品二): Settings.Default.其他分解物品二 = control.Text; break;
            case nameof(S_其他分解物品三): Settings.Default.其他分解物品三 = control.Text; break;
            case nameof(S_其他分解物品四): Settings.Default.其他分解物品四 = control.Text; break;
            case nameof(合成模块控件): Settings.Default.合成模块控件 = control.Text; break;
            case nameof(变性内容控件): Settings.Default.变性内容控件 = control.Text; break;
            case nameof(转职内容控件): Settings.Default.转职内容控件 = control.Text; break;

            default:
                MessageBox.Show("Unknown Control! " + control.Name);
                break;
        }
        Settings.Default.Save();
    }

    private void GMCommandTextBox_Press(object sender, KeyPressEventArgs e)
    {
        var str = GMCommandTextBox.Text;

        if (e.KeyChar != Convert.ToChar(13) || string.IsNullOrEmpty(str))
            return;

        主选项卡.SelectedIndex = 0;
        LoggingTab.SelectedIndex = 2;

        AddCommandLog("=> " + str);

        if (SEngine.AddGMCommand(str, UserDegree.Admin))
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
                Clipboard.SetDataObject(row["AccountName"]);
            }
            if (toolStripMenuItem.Name == "右键菜单_复制角色名字")
            {
                Clipboard.SetDataObject(row["Name"]);
            }
            if (toolStripMenuItem.Name == "右键菜单_复制网络地址")
            {
                Clipboard.SetDataObject(row["IPAddress"]);
            }
            if (toolStripMenuItem.Name == "右键菜单_复制物理地址")
            {
                Clipboard.SetDataObject(row["MACAddress"]);
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
    
    private void S_浏览平台目录_Click(object sender, EventArgs e)
    {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
        {
            Description = "请选择文件夹"
        };
        if (folderBrowserDialog.ShowDialog() == DialogResult.OK && sender == S_浏览平台目录)
        {
            var path = folderBrowserDialog.SelectedPath;
            S_平台接入目录.Text = Settings.Default.平台接入目录 = path;
            Settings.Default.Save();
        }
    }

    #region Start/Stop Server Buttons
    private void startServerToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SEngine.StartService();
        Settings.Default.Save();
        MapDataTable = new System.Data.DataTable("地图数据表");
        MapDataRows = new Dictionary<GameMap, DataRow>();
        MapDataTable.Columns.Add("MapID", typeof(string));
        MapDataTable.Columns.Add("MapName", typeof(string));
        MapDataTable.Columns.Add("RequiredLevel", typeof(string));
        MapDataTable.Columns.Add("Number of players", typeof(string));
        MapDataTable.Columns.Add("Monster Cap", typeof(string));
        MapDataTable.Columns.Add("Monsters Alive", typeof(string));
        MapDataTable.Columns.Add("Monster resurrection times", typeof(string));
        MapDataTable.Columns.Add("Monster Drops", typeof(string));
        MapDataTable.Columns.Add("Total gold dropped", typeof(string));
        Main.MapBrowser.DataSource = MapDataTable;
        MonsterDataTable = new System.Data.DataTable("怪物数据表");
        MonsterDataRows = new Dictionary<MonsterInfo, DataRow>();
        数据行怪物 = new Dictionary<DataRow, MonsterInfo>();
        MonsterDataTable.Columns.Add("Monster ID", typeof(string));
        MonsterDataTable.Columns.Add("Monster Name", typeof(string));
        MonsterDataTable.Columns.Add("Level", typeof(string));
        MonsterDataTable.Columns.Add("EXP", typeof(string));
        MonsterDataTable.Columns.Add("Grade", typeof(string));
        MonsterDataTable.Columns.Add("Move Time", typeof(string));
        MonsterDataTable.Columns.Add("Roaming interval", typeof(string));
        MonsterDataTable.Columns.Add("View Range", typeof(string));
        MonsterDataTable.Columns.Add("Agro Time", typeof(string));
        Main.怪物浏览表.DataSource = MonsterDataTable;
        DropDataTable = new System.Data.DataTable("掉落数据表");
        怪物掉落表 = new Dictionary<MonsterInfo, List<KeyValuePair<GameItem, long>>>();
        DropDataTable.Columns.Add("Item Name", typeof(string));
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
    #endregion

    #region Save/Clear Logs
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
    #endregion
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

    #region Menu Click Events
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
#endregion
}
