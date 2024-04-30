using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

using Ionic.Zlib;
using Newtonsoft.Json;

using AccountServer.Networking;

namespace AccountServer;

public partial class SMain : Form
{
    public sealed class GameServerInfo
    {
        public string ServerName { get; set; }

        public string TicketAddressIP { get; set; }
        public ushort TicketAddressPort { get; set; }
        public string PublicAddressIP { get; set; }
        public ushort PublicAddressPort { get; set; }

        [JsonIgnore]
        public IPEndPoint TicketAddress;

        [JsonIgnore]
        public IPEndPoint PublicAddress;
    }

    public static uint CreatedAccounts;
    public static uint TotalTickets;
    public static long TotalBytesReceived;
    public static long TotalBytesSent;

    public static SMain Instance;

    public static string AccountDirectory = ".\\Accounts";
    public static string PatchDirectory = ".\\Patches";
    public static string ServerConfigFile = ".\\!ServerInfo.txt";
    public static string PatchConfigFile = ".\\!Patch.txt";

    public static Dictionary<string, AccountInfo> Accounts;
    public static Dictionary<string, AccountInfo> AccountRefferalCodes;
    public static Dictionary<string, GameServerInfo> ServerTable = new Dictionary<string, GameServerInfo>();

    public static string PatchFile = ".\\GameLogin.exe";
    public static byte[] PatchData;
    public static ulong PatchChecksum;
    public static int PatchChunks;

    public static string PublicServerInfo => string.Join("\n", ServerTable.Values.Select(x => $"{x.PublicAddressIP}:{x.PublicAddressPort}/{x.ServerName}"));

    private static char[] RandomChars = new char[36]
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
        'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
        'u', 'v', 'w', 'x', 'y', 'z'
    };

    public static char[] RandomNumberChars = new char[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

    public static Dictionary<string, DateTime> PhoneCaptchaTime = new Dictionary<string, DateTime>();
    public static Dictionary<string, string> PhoneCaptcha = new Dictionary<string, string>();

    public SMain()
    {
        InitializeComponent();
        Instance = this;
        LocalListeningPortEdit.Value = Settings.LocalListeningPort;
        TicketSendingPortEdit.Value = Settings.TicketSendingPort;

        if (!File.Exists(ServerConfigFile))
        {
            LogTextBox.AppendText("The server profile was not found, note the configuration\r\n");
        }
        if (!Directory.Exists(AccountDirectory))
        {
            LogTextBox.AppendText("The account configuration folder could not be found, please note the guide\r\n");
        }
        if (!File.Exists(".\\00000.pak"))
        {
            LogTextBox.AppendText("The game patch update file was not found, please check the import\r\n");
        }
        if (!File.Exists(PatchConfigFile))
        {
            LogTextBox.AppendText("The update profile was not found, please check configuration\r\n");
        }
    }

    public static void UpdateServerStats()
    {
        Instance?.BeginInvoke((MethodInvoker)delegate
        {
            Instance.ExistingAccountsLabel.Text = $"Accounts: {Accounts.Count}";
            Instance.NewAccountsLabel.Text = $"New Accounts: {CreatedAccounts}";
            Instance.TicketsGeneratedLabel.Text = $"Tickets: {TotalTickets}";
            Instance.BytesReceivedLabel.Text = $"Bytes Received: {TotalBytesReceived}";
            Instance.BytesSentLabel.Text = $"Bytes Sent: {TotalBytesSent}";
        });
    }

    public static void AddLogMessage(string message)
    {
        Instance?.BeginInvoke((MethodInvoker)delegate
        {
            Instance.LogTextBox.AppendText(message + "\r\n");
            Instance.LogTextBox.ScrollToCaret();
        });
    }

    public static void AddAccount(AccountInfo account)
    {
        if (!Accounts.ContainsKey(account.AccountName))
        {
            account.PromoCode = CreatePromoCode();

            AccountRefferalCodes[account.PromoCode] = account;
            Accounts[account.AccountName] = account;
            SaveAccount(account);
        }
    }

    public static string CreatePromoCode()
    {
        string code;
        do
        {
            code = "";
            for (int i = 0; i < 4; i++)
            {
                code += RandomChars[Random.Shared.Next(RandomChars.Length)];
            }
        }
        while (AccountRefferalCodes.ContainsKey(code));
        return code;
    }

    public static string CreateVerificationCode()
    {
        string text = "";
        for (int i = 0; i < 4; i++)
        {
            text += RandomNumberChars[Random.Shared.Next(RandomNumberChars.Length)];
        }
        return text;
    }

    public static void SaveAccount(AccountInfo account)
    {
        File.WriteAllText(AccountDirectory + "\\" + account.AccountName + ".txt", Serializer.Serialize(account));
    }

    public static ulong CalcFileChecksum(byte[] buffer)
    {
        ulong csum = 0uL;
        foreach (var b in buffer)
            csum += b;
        return csum;
    }

    private void FormClosing_Click(object sender, FormClosingEventArgs e)
    {
        if (MessageBox.Show("Are you sure to shut down the server?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        {
            TrayIcon.Visible = false;
            Environment.Exit(0);
            return;
        }
        TrayIcon.Visible = true;
        Hide();
        if (e != null)
            e.Cancel = true;
        TrayIcon.ShowBalloonTip(1000, "", "The server has been turned to run in the background.", ToolTipIcon.Info);
    }

    private void RestoreWindow_Click(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            base.Visible = true;
            TrayIcon.Visible = false;
        }
    }

    private void RestoreWindowMenuItem_Click(object sender, EventArgs e)
    {
        base.Visible = true;
        TrayIcon.Visible = false;
    }

    private void EndProcessMenuItem_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show("Are you sure to shut down the server?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        {
            SEngine.StopService();
            TrayIcon.Visible = false;
            Environment.Exit(0);
        }
    }

    private void ReadPatchFile()
    {
        if (!File.Exists(PatchFile)) return;

        var buffer = File.ReadAllBytes(PatchFile);
        using var ms = new MemoryStream();
        using var writer = new DeflateStream(ms, CompressionMode.Decompress);
        writer.Write(buffer, 0, buffer.Length);
        writer.Close();

        PatchData = ms.ToArray();
        PatchChecksum = CalcFileChecksum(File.ReadAllBytes(PatchFile));
        PatchChunks = (int)Math.Ceiling((float)PatchData.Length / 40960f);

        AddLogMessage($"{PatchData.Length} {PatchChecksum}");
    }

    private void startServiceToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ReadPatchFile();

        if (ServerTable.Count == 0)
            loadConfigurationToolStripMenuItem_Click(sender, e);

        if (ServerTable.Count == 0)
        {
            AddLogMessage("The server configuration is empty and the startup fails");
            return;
        }
        if (Accounts == null || Accounts.Count == 0)
        {
            loadAccountsToolStripMenuItem_Click(sender, e);
        }

        Settings.LocalListeningPort = (ushort)LocalListeningPortEdit.Value;
        Settings.TicketSendingPort = (ushort)TicketSendingPortEdit.Value;
        Settings.Save();

        if (SEngine.StartService())
        {
            stopServiceToolStripMenuItem.Enabled = true;
            loadAccountsToolStripMenuItem.Enabled = false;
            loadConfigurationToolStripMenuItem.Enabled = false;
            startServiceToolStripMenuItem.Enabled = false;
            LocalListeningPortEdit.Enabled = false;
            TicketSendingPortEdit.Enabled = false;
        }

        LoadAccountsView();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void stopServiceToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SEngine.StopService();
        stopServiceToolStripMenuItem.Enabled = false;
        loadAccountsToolStripMenuItem.Enabled = true;
        loadConfigurationToolStripMenuItem.Enabled = true;
        TicketSendingPortEdit.Enabled = true;
        LocalListeningPortEdit.Enabled = true;
        startServiceToolStripMenuItem.Enabled = true;
    }

    private void openServerConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (!File.Exists(ServerConfigFile))
        {
            AddLogMessage("The configuration file does not exist and has been created automatically");
            File.WriteAllBytes(ServerConfigFile, Array.Empty<byte>());
        }
        Process.Start("notepad.exe", ServerConfigFile);
    }

    private void loadConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (!File.Exists(ServerConfigFile))
            return;

        ServerTable.Clear();

        try
        {
            var json = File.ReadAllText(ServerConfigFile, Encoding.UTF8);
            var settings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };

            ServerTable = JsonConvert.DeserializeObject<Dictionary<string, GameServerInfo>>(json, settings);
            foreach (var kvp in ServerTable)
            {
                var server = kvp.Value;
                server.TicketAddress = new IPEndPoint(IPAddress.Loopback, server.TicketAddressPort);
                server.PublicAddress = new IPEndPoint(IPAddress.Parse(server.PublicAddressIP), server.PublicAddressPort);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Server configuration error, parsing failed. \n\n" + ex.ToString());
            Environment.Exit(0);
        }
        
        AddLogMessage($"The network configuration loaded {ServerTable.Count}.");
    }

    private void openAccountDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (!Directory.Exists(AccountDirectory))
        {
            AddLogMessage("The account directory does not exist and is automatically created");
            Directory.CreateDirectory(AccountDirectory);
        }
        else
        {
            Process.Start("explorer.exe", AccountDirectory);
        }
    }

    private void loadAccountsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Accounts = new Dictionary<string, AccountInfo>();
        AccountRefferalCodes = new Dictionary<string, AccountInfo>();
        if (!Directory.Exists(AccountDirectory))
        {
            AddLogMessage("The account directory does not exist and is automatically created");
            Directory.CreateDirectory(AccountDirectory);
            return;
        }

        var array = Serializer.Deserialize<AccountInfo>(AccountDirectory);
        foreach (var account in array)
        {
            if (account.PromoCode == null || account.PromoCode == string.Empty)
            {
                account.PromoCode = CreatePromoCode();
                SaveAccount(account);
            }
            Accounts[account.AccountName] = account;
            AccountRefferalCodes[account.PromoCode] = account;
        }

        AddLogMessage($"Account data loaded, the current number of accounts: {Accounts.Count}");
        ExistingAccountsLabel.Text = $"Accounts: {Accounts.Count}";
    }

    private void OpenUpdateConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (!File.Exists(PatchConfigFile))
        {
            AddLogMessage("The configuration file does not exist and has been created automatically");
            File.WriteAllBytes(PatchConfigFile, Array.Empty<byte>());
        }
        Process.Start("notepad.exe", PatchConfigFile);
    }

    private void openPatchDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (!Directory.Exists(PatchDirectory))
        {
            AddLogMessage("The patch directory does not exist and is automatically created");
            Directory.CreateDirectory(PatchDirectory);
        }
        else
        {
            Process.Start("explorer.exe", PatchDirectory);
        }
    }

    private void LoadUpdateConfiguration(object sender, EventArgs e)
    {
        ReadPatchFile();
    }

    private void LoadAccountsView()
    {
        string[] accountFiles = Directory.GetFiles(AccountDirectory, "*.txt");
        foreach (string file in accountFiles)
        {
            string jsonContent = File.ReadAllText(file);

            AccountInfo accountInfo = JsonConvert.DeserializeObject<AccountInfo>(jsonContent);

            ListViewItem item = new ListViewItem(accountInfo.AccountName);
            item.SubItems.Add(accountInfo.Password);
            item.SubItems.Add(accountInfo.SecurityQuestion);
            item.SubItems.Add(accountInfo.SecurityAnswer);
            item.SubItems.Add(accountInfo.CreationDate.ToString("yyyy-MM-dd"));
            item.SubItems.Add(accountInfo.PromoCode);
            item.SubItems.Add(accountInfo.ReferrerCode);
            AccountsListView.Items.Add(item);
        }
    }
}
