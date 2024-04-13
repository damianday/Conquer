using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Properties;

using AccountServer.Networking;

namespace AccountServer;

public partial class SMain : Form
{
    public sealed class GameServerInfo
    {
        public string ServerName { get; set; }
        public IPEndPoint InternalAddress { get; set; }
        public IPEndPoint PublicAddress { get; set; }
    }

    public static uint CreatedAccounts;
	public static uint TotalTickets;
	public static long TotalBytesReceived;
	public static long TotalBytesSent;

	public static SMain Instance;

	public static string AccountDirectory = ".\\Accounts";
	public static string PatchDirectory = ".\\Patchs";
    public static string ServerConfigFile = ".\\!ServerInfo.txt";
    public static string PatchConfigFile = ".\\patch";

	public static Dictionary<string, AccountInfo> Accounts;
	public static Dictionary<string, AccountInfo> AccountRefferalCodes;
	public static Dictionary<string, GameServerInfo> ServerTable = new Dictionary<string, GameServerInfo>();

	public static string 游戏更新 = "";

	public static Dictionary<string, ulong> 更新校验;
	public static Dictionary<ulong, byte[]> 更新数据;

	

	public static byte[] PatchData;

	public static ulong PatchChecksum;

	public static int PatchChunks;

	public static byte[] 启动数据;

	public static ulong 启动校验;

	private Button 加载更新按钮;
	private Button 打开更新按钮;
	private Button ViewPatchButton;

	public static int 启动数量;

    public static string PublicServerInfo => string.Join("\n", ServerTable.Values.Select(x => $"{x.PublicAddress.Address}:{x.PublicAddress.Port}/{x.ServerName}"));

    private static char[] RandomChars = new char[36]
	{
		'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
		'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
		'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
		'u', 'v', 'w', 'x', 'y', 'z'
	};

	public static char[] RandomNumberChars = new char[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

	public static Dictionary<string, DateTime> 验证码时间 = new Dictionary<string, DateTime>();
	public static Dictionary<string, string> 验证码值 = new Dictionary<string, string>();

	public SMain()
	{
		InitializeComponent();
		Instance = this;
		LocalListeningPortEdit.Value = Settings.Default.LocalListeningPort;
		TicketSendingPortEdit.Value = Settings.Default.TicketSendingPort;

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

	private void StartServiceButton_Click(object sender, EventArgs e)
	{
        WritePatchFile();

        if (ServerTable.Count == 0)
			LoadConfigurationButton_Click(sender, e);

		if (ServerTable.Count == 0)
		{
			AddLogMessage("The server configuration is empty and the startup fails");
			return;
		}
		if (Accounts == null || Accounts.Count == 0)
		{
			LoadAccountButton_Click(sender, e);
		}

        Settings.Default.LocalListeningPort = (ushort)LocalListeningPortEdit.Value;
        Settings.Default.TicketSendingPort = (ushort)TicketSendingPortEdit.Value;
        Settings.Default.Save();

        if (SEngine.StartService())
		{
			StopServiceButton.Enabled = true;
			LoadAccountButton.Enabled = false;
            LoadConfigurationButton.Enabled = false;
			StartServiceButton.Enabled = false;
			LocalListeningPortEdit.Enabled = false;
			TicketSendingPortEdit.Enabled = false;
		}
	}

	private void StopServiceButton_Click(object sender, EventArgs e)
	{
        SEngine.StopService();
		StopServiceButton.Enabled = false;
		LoadAccountButton.Enabled = true;
        LoadConfigurationButton.Enabled = true;
		TicketSendingPortEdit.Enabled = true;
        LocalListeningPortEdit.Enabled = true;
        StartServiceButton.Enabled = true;
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

	private void 恢复窗口_Click(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			base.Visible = true;
			TrayIcon.Visible = false;
		}
	}

	private void 恢复窗口_Click(object sender, EventArgs e)
	{
		base.Visible = true;
		TrayIcon.Visible = false;
	}

	private void 结束进程_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show("Are you sure to shut down the server?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
		{
            SEngine.StopService();
			TrayIcon.Visible = false;
			Environment.Exit(0);
		}
	}

	private void OpenConfigurationButton_Click(object sender, EventArgs e)
	{
		if (!File.Exists(ServerConfigFile))
		{
			AddLogMessage("The configuration file does not exist and has been created automatically");
			File.WriteAllBytes(ServerConfigFile, Array.Empty<byte>());
		}
		Process.Start("notepad.exe", ServerConfigFile);
	}

	private void LoadConfigurationButton_Click(object sender, EventArgs e)
	{
		if (!File.Exists(ServerConfigFile))
			return;

		ServerTable.Clear();

		var text = File.ReadAllText(ServerConfigFile, Encoding.Unicode).Trim('\r', '\n', ' ');
		string[] lines = text.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

		foreach (string line in lines)
		{
			var arr = line.Split(new char[] { ':', ',', '/' }, StringSplitOptions.RemoveEmptyEntries);
			if (arr.Length != 4)
			{
				MessageBox.Show("Server configuration error, parsing failed. Row: " + line);
				Environment.Exit(0);
			}

            var ip = arr[0];
            var port = -1;
            var tport = -1;
            var name = arr[3];

            if (!int.TryParse(arr[1], out port) || !int.TryParse(arr[2], out tport) ||
                string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Server configuration error, parsing failed. Row: " + line);
                Environment.Exit(0);
            }

            ServerTable.Add(name, new GameServerInfo
            {
                InternalAddress = new IPEndPoint(IPAddress.Loopback, tport),
                //InternalAddress = new IPEndPoint(IPAddress.Parse(ip), port),
                PublicAddress = new IPEndPoint(IPAddress.Parse(ip), port),
                ServerName = name
            });
        }

		//AddLogMessage("The network configuration is loaded, and the current configuration order\r\n" + 游戏区服);
	}

	private void ViewAccountButton_Click(object sender, EventArgs e)
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

	private void LoadAccountButton_Click(object sender, EventArgs e)
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

    private void 打开更新按钮_Click(object sender, EventArgs e)
	{
		if (!File.Exists(PatchConfigFile))
		{
			AddLogMessage("The configuration file does not exist and has been created automatically");
			File.WriteAllBytes(PatchConfigFile, Array.Empty<byte>());
		}
		Process.Start("notepad.exe", PatchConfigFile);
	}

	private void 加载更新按钮_Click(object sender, EventArgs e)
	{
        WritePatchFile();
	}

	private void ViewPatchButton_Click(object sender, EventArgs e)
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

    private void WritePatchFile()
    {
        if (File.Exists(".\\GameLogin.exe"))
        {
            var buffer = File.ReadAllBytes(".\\GameLogin.exe");
            using var ms = new MemoryStream();
            using var writer = new DeflaterOutputStream(ms);
            writer.Write(buffer, 0, buffer.Length);
            writer.Close();
            PatchData = ms.ToArray();
            PatchChecksum = CalcFileChecksum(File.ReadAllBytes(".\\GameLogin.exe"));
            PatchChunks = (int)Math.Ceiling((float)PatchData.Length / 40960f);

            AddLogMessage($"{PatchData.Length} {PatchChecksum}");
        }
    }
}
