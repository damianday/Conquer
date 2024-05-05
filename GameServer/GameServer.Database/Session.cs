using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameServer.Database;

public static class Session
{
    private static bool m_Modified;
    private static byte[] m_Header;

    public static DBBindingList<SystemInfo> SystemInfoTable;
    public static DBBindingList<AccountInfo> AccountInfoTable;
    public static DBBindingList<CharacterInfo> CharacterInfoTable;
    public static DBBindingList<PetInfo> PetInfoTable;
    public static DBBindingList<ItemInfo> ItemInfoTable;
    public static DBBindingList<EquipmentInfo> EquipmentInfoTable;
    public static DBBindingList<SkillInfo> SkillInfoTable;
    public static DBBindingList<BuffInfo> BuffInfoTable;
    public static DBBindingList<TeamInfo> TeamInfoTable;
    public static DBBindingList<GuildInfo> GuildInfoTable;
    public static DBBindingList<MentorInfo> MentorInfoTable;
    public static DBBindingList<MailInfo> MailInfoTable;
    public static Dictionary<Type, DBCollection> Tables;

    public static bool Modified
    {
        get { return m_Modified; }
        set
        {
            m_Modified = value;
            if (m_Modified && !SEngine.Running && (SEngine.MainThread == null || !SEngine.MainThread.IsAlive))
            {
                SMain.Main.BeginInvoke(new Action(() =>
                {
                    SMain.Main.savaDatabaseToolStripMenuItem.Enabled = true;
                }));
            }
        }
    }

    public static string UserDataPath => Settings.Default.GameDataPath + "\\User";
    public static string BackupPath => Settings.Default.DataBackupPath;
    public static string UserDataFile => Settings.Default.GameDataPath + "\\User\\Data.db";
    public static string UserCacheFile => Settings.Default.GameDataPath + "\\User\\Temp.db";
    public static string BackupFile => $"{Settings.Default.DataBackupPath}\\User-{SEngine.CurrentTime:yyyy-MM-dd-HH-mm-ss-ffff}.db.gz";

    public static void Load()
    {
        Tables = new Dictionary<Type, DBCollection>();
        Type[] types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (Type type in types)
        {
            if (type.IsSubclassOf(typeof(DBObject)))
            {
                Tables[type] = (DBCollection)Activator.CreateInstance(typeof(DBBindingList<>).MakeGenericType(type));
            }
        }

        using (var ms = new MemoryStream())
        {
            using var writer = new BinaryWriter(ms);
            writer.Write(Tables.Count);
            foreach (KeyValuePair<Type, DBCollection> item in Tables)
            {
                item.Value.Mapping.Save(writer);
            }
            m_Header = ms.ToArray();
        }
        if (File.Exists(UserDataFile))
        {
            using var reader = new BinaryReader(File.OpenRead(UserDataFile));
            List<DBMapping> mappings = new List<DBMapping>();
            int count = reader.ReadInt32();
            for (int j = 0; j < count; j++)
            {
                mappings.Add(new DBMapping(reader));
            }
            List<Task> list2 = new List<Task>();
            foreach (DBMapping mapping in mappings)
            {
                byte[] buffer = reader.ReadBytes(reader.ReadInt32());
                if (!(mapping.Type == null) && Tables.TryGetValue(mapping.Type, out var table))
                {
                    list2.Add(Task.Run(delegate
                    {
                        table.Load(buffer, mapping);
                    }));
                }
            }
            if (list2.Count > 0)
            {
                Task.WaitAll(list2.ToArray());
            }
        }

        SystemInfoTable = Tables[typeof(SystemInfo)] as DBBindingList<SystemInfo>;
        AccountInfoTable = Tables[typeof(AccountInfo)] as DBBindingList<AccountInfo>;
        CharacterInfoTable = Tables[typeof(CharacterInfo)] as DBBindingList<CharacterInfo>;
        PetInfoTable = Tables[typeof(PetInfo)] as DBBindingList<PetInfo>;
        ItemInfoTable = Tables[typeof(ItemInfo)] as DBBindingList<ItemInfo>;
        EquipmentInfoTable = Tables[typeof(EquipmentInfo)] as DBBindingList<EquipmentInfo>;
        SkillInfoTable = Tables[typeof(SkillInfo)] as DBBindingList<SkillInfo>;
        BuffInfoTable = Tables[typeof(BuffInfo)] as DBBindingList<BuffInfo>;
        TeamInfoTable = Tables[typeof(TeamInfo)] as DBBindingList<TeamInfo>;
        GuildInfoTable = Tables[typeof(GuildInfo)] as DBBindingList<GuildInfo>;
        MentorInfoTable = Tables[typeof(MentorInfo)] as DBBindingList<MentorInfo>;
        MailInfoTable = Tables[typeof(MailInfo)] as DBBindingList<MailInfo>;

        if (SystemInfoTable.DataSheet.Count == 0)
            new SystemInfo(1);

        DataLinkTable.ProcessTasks();
        foreach (KeyValuePair<int, DBObject> data in CharacterInfoTable.DataSheet)
        {
            data.Value.OnLoaded();
        }
        SystemInfo.Info.OnLoaded();
    }

    public static void Save()
    {
        Parallel.ForEach(Tables.Values, x => { x.Save(); });
    }

    public static void AutoSave()
    {
        Parallel.ForEach(Tables.Values, x => { x.Save(); });
    }

    public static void SaveSystem()
    {
        Parallel.ForEach(Tables.Values, x => { x.ForcedSave(); });
    }

    public static void 导出数据2()
    {
        if (!Directory.Exists(UserDataPath))
            Directory.CreateDirectory(UserDataPath);

        using var writer = new BinaryWriter(File.Create(UserDataFile));
        writer.Write(m_Header);
        foreach (var kvp in Tables)
        {
            var buffer = kvp.Value.ToArray();
            writer.Write(buffer.Length);
            writer.Write(buffer);
        }
    }

    public static void SaveUsers()
    {
        if (!Directory.Exists(UserDataPath))
            Directory.CreateDirectory(UserDataPath);

        using (var writer = new BinaryWriter(File.Create(UserCacheFile)))
        {
            writer.Write(m_Header);
            foreach (var kvp in Tables)
            {
                var buffer = kvp.Value.ToArray();
                writer.Write(buffer.Length);
                writer.Write(buffer);
            }
        }

        if (!Directory.Exists(Settings.Default.DataBackupPath))
            Directory.CreateDirectory(Settings.Default.DataBackupPath);
        if (File.Exists(UserDataFile))
        {
            using (var fs = File.OpenRead(UserDataFile))
            {
                using var stream = File.Create(BackupFile);
                using var destination = new GZipStream(stream, CompressionMode.Compress);
                fs.CopyTo(destination);
            }
            File.Delete(UserDataFile);
        }

        File.Move(UserCacheFile, UserDataFile);

        Modified = false;
    }

    public static void Save(bool commit)
    {
        int count = 0;
        foreach (KeyValuePair<Type, DBCollection> item in Tables)
        {
            int num2 = 0;
            if (item.Value.Type == typeof(GuildInfo))
            {
                num2 = 1610612736;
            }
            if (item.Value.Type == typeof(TeamInfo))
            {
                num2 = 1879048192;
            }
            List<DBObject> list = item.Value.DataSheet.Values.OrderBy((DBObject O) => O.Index.V).ToList();
            int num3 = 0;
            for (int i = 0; i < list.Count; i++)
            {
                int num4 = num2 + i + 1;
                DBObject data = list[i];
                if (data.Index.V == num4)
                {
                    continue;
                }
                if (data is CharacterInfo)
                {
                    foreach (var kvp in GuildInfoTable.DataSheet)
                    {
                        foreach (行会事记 item3 in ((GuildInfo)kvp.Value).行会事记)
                        {
                            switch (item3.事记类型)
                            {
                                case 事记类型.CreateGuild:
                                case 事记类型.JoinGuild:
                                case 事记类型.LeaveGuild:
                                    if (item3.第一参数 == data.Index.V)
                                    {
                                        item3.第一参数 = num4;
                                    }
                                    break;
                                case 事记类型.逐出公会:
                                case 事记类型.ChangeRank:
                                case 事记类型.会长传位:
                                    if (item3.第一参数 == data.Index.V)
                                    {
                                        item3.第一参数 = num4;
                                    }
                                    if (item3.第二参数 == data.Index.V)
                                    {
                                        item3.第二参数 = num4;
                                    }
                                    break;
                            }
                        }
                    }
                }
                else if (data is GuildInfo)
                {
                    foreach (var kvp in GuildInfoTable.DataSheet)
                    {
                        foreach (行会事记 item5 in ((GuildInfo)kvp.Value).行会事记)
                        {
                            事记类型 事记类型2 = item5.事记类型;
                            if ((uint)(事记类型2 - 9) <= 1u || (uint)(事记类型2 - 21) <= 1u)
                            {
                                if (item5.第一参数 == data.Index.V)
                                {
                                    item5.第一参数 = num4;
                                }
                                if (item5.第二参数 == data.Index.V)
                                {
                                    item5.第二参数 = num4;
                                }
                            }
                        }
                    }
                }
                data.Index.V = num4;
                num3++;
            }
            item.Value.Index = list.Count + num2;
            count += num3;
            item.Value.DataSheet = item.Value.DataSheet.ToDictionary((KeyValuePair<int, DBObject> x) => x.Value.Index.V, (KeyValuePair<int, DBObject> x) => x.Value);
            SMain.AddCommandLog($"{item.Key.Name}已经整理完毕, 整理数量:{num3}");
        }

        SMain.AddCommandLog($"User data has been collated, total collated:{count}");

        if (count > 0 && commit)
        {
            SMain.AddCommandLog("Please wait for a while as we are re-saving the organized user data.");
            SaveSystem();
            SaveUsers();
            SMain.AddCommandLog("Data has been saved to disk");
            MessageBox.Show("User data has been collated, the application needs to be restarted.");
            Environment.Exit(0);
        }
    }

    public static void 清理角色(int 限制等级, int 限制天数)
    {
        SMain.AddCommandLog("Starting to clean up character data...");
        DateTime dateTime = DateTime.UtcNow.AddDays(-限制天数);

        int count = 0;
        foreach (DBObject item in CharacterInfoTable.DataSheet.Values.ToList())
        {
            if (item is CharacterInfo character && character.Level.V < 限制等级 && !(character.DisconnectDate.V > dateTime))
            {
                if (character.CurrentRanking.Count > 0)
                {
                    SMain.AddCommandLog($"[{character}]({character.Level}/{(int)(DateTime.UtcNow - character.DisconnectDate.V).TotalDays}) 在排行榜单上, 已跳过清理");
                    continue;
                }
                if (character.Ingot > 0)
                {
                    SMain.AddCommandLog($"[{character}]({character.Level}/{(int)(DateTime.UtcNow - character.DisconnectDate.V).TotalDays}) 有未消费元宝, 已跳过清理");
                    continue;
                }
                if (character.CurrentGuild?.PresidentInfo == character)
                {
                    SMain.AddCommandLog($"[{character}]({character.Level}/{(int)(DateTime.UtcNow - character.DisconnectDate.V).TotalDays}) 是行会的会长, 已跳过清理");
                    continue;
                }
                SMain.AddCommandLog($"开始清理[{character}]({character.Level}/{(int)(DateTime.UtcNow - character.DisconnectDate.V).TotalDays})...");
                character.Remove();
                count++;
                SMain.RemoveCharacterInfo(character);
            }
        }

        SMain.AddCommandLog($"角色数据已经清理完成, 清理总数:{count}");

        if (count > 0)
        {
            SMain.AddCommandLog("The cleaned user data is being re-saved, it may take a long time, please wait...");
            Save();
            SaveUsers();
            Load();
            SMain.AddCommandLog("Data has been saved to disk");
        }
    }

    public static void Merge(string fileName)
    {
        byte[] data = null;
        DBCollection 存表实例 = null;
        SMain.Main?.BeginInvoke(new Action(() =>
        {
            SMain.Main.下方控件页.Enabled = false;
            SMain.Main.SettingsPage.Enabled = false;
            SMain.Main.主选项卡.SelectedIndex = 0;
            SMain.Main.LoggingTab.SelectedIndex = 2;
            SMain.AddCommandLog("Start to merging current user data...");
            Save(commit: false);
            Dictionary<Type, DBCollection> dictionary = Tables;
            SMain.AddCommandLog("Start to load the specified user data...");

            Tables = new Dictionary<Type, DBCollection>();
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in types)
            {
                if (type.IsSubclassOf(typeof(DBObject)))
                {
                    Tables[type] = (DBCollection)Activator.CreateInstance(typeof(DBBindingList<>).MakeGenericType(type));
                }
            }

            using (var ms = new MemoryStream())
            {
                using var writer = new BinaryWriter(ms);
                writer.Write(Tables.Count);
                foreach (var item in Tables)
                {
                    item.Value.Mapping.Save(writer);
                }
                m_Header = ms.ToArray();
            }

            if (File.Exists(fileName))
            {
                using var reader = new BinaryReader(File.OpenRead(fileName));
                List<DBMapping> mappings = new List<DBMapping>();
                int count = reader.ReadInt32();
                for (int j = 0; j < count; j++)
                {
                    mappings.Add(new DBMapping(reader));
                }
                List<Task> list2 = new List<Task>();
                foreach (var mapping in mappings)
                {
                    data = reader.ReadBytes(reader.ReadInt32());
                    if (!(mapping.Type == null) && Tables.TryGetValue(mapping.Type, out 存表实例))
                    {
                        list2.Add(Task.Run(delegate
                        {
                            存表实例.Load(data, mapping);
                        }));
                    }
                }
                if (list2.Count > 0)
                {
                    Task.WaitAll(list2.ToArray());
                }
            }

            SMain.AddCommandLog("开始整理指定客户数据...");
            DataLinkTable.ProcessTasks();
            Save(commit: false);
            Dictionary<Type, DBCollection> dictionary2 = Tables;
            foreach (KeyValuePair<Type, DBCollection> item2 in dictionary)
            {
                if (dictionary2.ContainsKey(item2.Key))
                {
                    if (item2.Key == typeof(AccountInfo))
                    {
                        DBBindingList<AccountInfo> 数据表实例2 = dictionary[item2.Key] as DBBindingList<AccountInfo>;
                        foreach (KeyValuePair<int, DBObject> item3 in (dictionary2[item2.Key] as DBBindingList<AccountInfo>).DataSheet)
                        {
                            AccountInfo account = item3.Value as AccountInfo;
                            if (数据表实例2.SearchTable.TryGetValue(account.AccountName.V, out var value) && value is AccountInfo 账号数据3)
                            {
                                foreach (var character in account.Characters)
                                {
                                    账号数据3.Characters.Add(character);
                                    character.Account.V = 账号数据3;
                                }
                                foreach (var character in account.FrozenCharacters)
                                {
                                    账号数据3.FrozenCharacters.Add(character);
                                    character.Account.V = 账号数据3;
                                }
                                foreach (var character in account.DeletedCharacters)
                                {
                                    账号数据3.DeletedCharacters.Add(character);
                                    character.Account.V = 账号数据3;
                                }
                                账号数据3.BlockDate.V = ((账号数据3.BlockDate.V <= account.BlockDate.V) ? 账号数据3.BlockDate.V : account.BlockDate.V);
                                账号数据3.DeletetionDate.V = default(DateTime);
                            }
                            else
                            {
                                item2.Value.Add(account, index: true);
                            }
                        }
                    }
                    else if (item2.Key == typeof(CharacterInfo))
                    {
                        DBBindingList<CharacterInfo> 数据表实例3 = dictionary[item2.Key] as DBBindingList<CharacterInfo>;
                        foreach (KeyValuePair<int, DBObject> item7 in (dictionary2[item2.Key] as DBBindingList<CharacterInfo>).DataSheet)
                        {
                            CharacterInfo 角色数据2 = item7.Value as CharacterInfo;
                            if (数据表实例3.SearchTable.TryGetValue(角色数据2.UserName.V, out var value2) && value2 is CharacterInfo 角色数据3)
                            {
                                if (角色数据3.CreatedDate.V > 角色数据2.CreatedDate.V)
                                {
                                    角色数据3.UserName.V += "_";
                                }
                                else
                                {
                                    角色数据2.UserName.V += "_";
                                }
                            }
                            item2.Value.Add(角色数据2, index: true);
                        }
                    }
                    else if (item2.Key == typeof(GuildInfo))
                    {
                        DBBindingList<GuildInfo> 数据表实例4 = dictionary[item2.Key] as DBBindingList<GuildInfo>;
                        foreach (KeyValuePair<int, DBObject> item8 in (dictionary2[item2.Key] as DBBindingList<GuildInfo>).DataSheet)
                        {
                            GuildInfo 行会数据2 = item8.Value as GuildInfo;
                            if (数据表实例4.SearchTable.TryGetValue(行会数据2.GuildName.V, out var value3) && value3 is GuildInfo 行会数据3)
                            {
                                if (行会数据3.CreatedDate.V > 行会数据2.CreatedDate.V)
                                {
                                    行会数据3.GuildName.V += "_";
                                }
                                else
                                {
                                    行会数据2.GuildName.V += "_";
                                }
                            }
                            item2.Value.Add(行会数据2, index: true);
                        }
                    }
                    else
                    {
                        foreach (KeyValuePair<int, DBObject> item9 in dictionary2[item2.Key].DataSheet)
                        {
                            item2.Value.Add(item9.Value, index: true);
                        }
                    }
                }
            }
            dictionary2[typeof(SystemInfo)].DataSheet.Clear();
            dictionary[typeof(SystemInfo)].DataSheet.Clear();
            dictionary[typeof(SystemInfo)].DataSheet[1] = new SystemInfo(1);
            foreach (KeyValuePair<int, DBObject> item10 in dictionary[typeof(GuildInfo)].DataSheet)
            {
                ((GuildInfo)item10.Value).行会排名.V = 0;
            }
            foreach (KeyValuePair<int, DBObject> item11 in dictionary[typeof(CharacterInfo)].DataSheet)
            {
                ((CharacterInfo)item11.Value).PreviousRanking.Clear();
                ((CharacterInfo)item11.Value).CurrentRanking.Clear();
            }

            Tables = dictionary;
            SaveSystem();
            SaveUsers();
            SMain.AddCommandLog("Consolidation of user data has been completed");
            MessageBox.Show("User data has been merged, the application needs to be restarted.");
            Environment.Exit(0);
        }));
    }

    public static AccountInfo GetAccount(string accountName)
    {
        if (AccountInfoTable.SearchTable.TryGetValue(accountName, out var value) && value is AccountInfo account)
            return account;
        return null;
    }

    public static AccountInfo GetAccount(int id)
    {
        if (AccountInfoTable.DataSheet.TryGetValue(id, out var value) && value is AccountInfo account)
            return account;
        return null;
    }

    public static CharacterInfo GetCharacter(string uname)
    {
        if (CharacterInfoTable.SearchTable.TryGetValue(uname, out var value) && value is CharacterInfo character)
            return character;
        return null;
    }

    public static CharacterInfo GetCharacter(int id)
    {
        if (CharacterInfoTable.DataSheet.TryGetValue(id, out var value) && value is CharacterInfo character)
            return character;
        return null;
    }
}
