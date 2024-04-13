using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Launcher.Properties;

using GamePackets.Client;
using GamePackets;

namespace Launcher
{
    public partial class MainForm : Form
    {
        public sealed class GameServerInfo
        {
            public string ServerName { get; set; }
            public IPEndPoint PublicAddress { get; set; }
        }

        public static string LoginAccount, RegisterAccount, ModifyAccount;
        public static string LoginPassword, RegisterPassword, ModifyPassword;
        public static bool LoggedIn;
        public static Process GameProgress;
        public static MainForm CurrentForm;
        public static Dictionary<string, GameServerInfo> ServerTable = new Dictionary<string, GameServerInfo>();

        public bool Is64Bit => uiCheckBox2.Checked;
        public bool Is32Bit => uiCheckBox1.Checked;

        public MainForm()
        {
            InitializeComponent();

            CurrentForm = this;

            PreLaunchChecks();
            LoadConfig();

            if (Environment.Is64BitOperatingSystem)
            {
                uiCheckBox1.Enabled = true;
                uiCheckBox1.Checked = false;
                uiCheckBox2.Enabled = true;
                uiCheckBox2.Checked = true;
            }
            else
            {
                uiCheckBox2.Enabled = false;
                uiCheckBox2.Checked = false;
                uiCheckBox1.Enabled = false;
                uiCheckBox1.Checked = true;
            }

            start_selected_zone.Text = Settings.Default.SaveArea;
            AccountTextBox.Text = Settings.Default.SaveAccountName;

            ConnectionStatusLabel.Text = "Attempting to connect to the server.";
            Network.Instance.Connect();
        }
        private void LoadConfig()
        {
            /*bool ServerCfgFound = File.Exists("./ServerCfg.txt");
            if (!ServerCfgFound)
            {
                MessageBox.Show("ServerCfg.txt Cannot Be Found!\r\nPlease Read The README.txt");
                Environment.Exit(0);
            }
            string[] strArray = File.ReadAllText("./ServerCfg.txt").Trim('\r', '\n', '\t', ' ').Split(':');
            if (strArray.Length != 2)
            {
                MessageBox.Show("ServerCfg.txt Configuration Error!\r\nPlease Read The README.txt");
                Environment.Exit(0);
            }
            Network.ASAddress = new IPEndPoint(IPAddress.Parse(strArray[0]), Convert.ToInt32(strArray[1]));*/

            Network.Instance.ASAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7_000);
        }
        private void PreLaunchChecks()
        {
            bool ClientFound32Bit = File.Exists(".\\Binaries\\Win32\\MMOGame-Win32-Shipping.exe");
            bool ClientFound64Bit = File.Exists(".\\Binaries\\Win64\\MMOGame-Win64-Shipping.exe");

            if (!ClientFound32Bit && !ClientFound64Bit)
            {
                MessageBox.Show("Client Cannot Be Found!\r\nPlease Read The README.txt");
                Environment.Exit(0);
            }
        }
        public void UILock()
        {
            MainTab.Enabled = false;
            LoginErrorLabel.Visible = false;
            RegistrationErrorLabel.Visible = false;
            Modify_ErrorLabel.Visible = false;
        }

        public void PacketProcess(object sender, EventArgs e)
        {
            Network.Instance.Process();

            if (Network.Instance.Connected)
                ConnectionStatusLabel.Text = $"Connected";
            else
                ConnectionStatusLabel.Text = $"Attempting to connect to the server. Attempt: {Network.Instance.ConnectAttempt}";
        }

        public void AccountRegisterSuccessUpdate()
        {
            BeginInvoke((MethodInvoker)delegate
            {
                AccountTextBox.Text = RegisterAccount;
                AccountPasswordTextBox.Text = RegisterPassword;
                UIUnlock(null, null);
                MainTab.SelectedIndex = 0;
                MessageBox.Show("Account Created Successfully");
            });
        }
        public void AccountRegisterFailUpdate(string message)
        {
            RegisterAccount = string.Empty;
            RegisterPassword = string.Empty;

            BeginInvoke((MethodInvoker)delegate
            {
                UIUnlock(null, null);
                RegistrationErrorLabel.Text = message;
                RegistrationErrorLabel.Visible = true;
                RegistrationErrorLabel.ForeColor = Color.Red;
            });
        }
        public void AccountChangePasswordSuccessUpdate()
        {
            BeginInvoke((MethodInvoker)delegate
            {
                AccountTextBox.Text = ModifyAccount;
                AccountPasswordTextBox.Text = ModifyPassword;
                UIUnlock(null, null);
                MainTab.SelectedIndex = 0;
                MessageBox.Show("Password Change Completed");
            });
        }
        public void AccountChangePasswordFailUpdate(string message)
        {
            ModifyAccount = string.Empty;
            ModifyPassword = string.Empty;

            BeginInvoke((MethodInvoker)delegate
            {
                UIUnlock(null, null);
                Modify_ErrorLabel.Text = message;
                Modify_ErrorLabel.Visible = true;
            });
        }
        public void AccountLogInSuccessUpdate(string data)
        {
            LoggedIn = true;

            BeginInvoke((MethodInvoker)delegate
            {
                UIUnlock(null, null);

                ServerTable.Clear();
                GameServerList.Items.Clear();
                var lines = data.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var arr = line.Split(new char[2] { ':', '/' }, StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Length != 3)
                    {
                        MessageBox.Show("Server Data Parsing Failed");
                        Environment.Exit(0);
                    }

                    var ip = arr[0];
                    var port = -1;
                    var name = arr[2];

                    if (!int.TryParse(arr[1], out port) ||  string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(name))
                    {
                        MessageBox.Show("Server configuration error, parsing failed. Row: " + line);
                        Environment.Exit(0);
                    }

                    ServerTable.Add(name, new GameServerInfo
                    {
                        PublicAddress = new IPEndPoint(IPAddress.Parse(ip), port),
                        ServerName = name
                    });

                    GameServerList.Items.Add(name);
                }
                MainTab.SelectedIndex = 3;
                Settings.Default.SaveAccountName = LoginAccount;
                Settings.Default.Save();
            });
        }
        public void AccountLogInFailUpdate(string message)
        {
            LoggedIn = false;
            LoginAccount = string.Empty;
            LoginPassword = string.Empty;

            BeginInvoke((MethodInvoker)delegate
            {
                UIUnlock(null, null);
                LoginErrorLabel.Text = message;
                LoginErrorLabel.ForeColor = Color.Red;
                LoginErrorLabel.Visible = true;
            });
        }
        public void AccountLogOutSuccessUpdate(string message)
        {
            LoggedIn = false;
            LoginAccount = string.Empty;
            LoginPassword = string.Empty;

            BeginInvoke((MethodInvoker)delegate
            {
                UIUnlock(null, null);
                LoginErrorLabel.Text = message;
                LoginErrorLabel.ForeColor = Color.Red;
                LoginErrorLabel.Visible = true;

                MainTab.SelectedIndex = 0;
            });
        }
        public void AccountStartGameSuccessUpdate(string ticket)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                if (ServerTable.TryGetValue(start_selected_zone.Text, out var value))
                {
                    string arguments = "-wegame=" + $"1,1,{value.PublicAddress.Address},{value.PublicAddress.Port}," + 
                        $"1,1,{value.PublicAddress.Address},{value.PublicAddress.Port}," + start_selected_zone.Text + "  " + 
                        $"/ip:1,1,{value.PublicAddress.Address} " + $"/port:{value.PublicAddress.Port} " + 
                        "/ticket:" + ticket + 
                        " /AreaName:" + start_selected_zone.Text;

                    Settings.Default.SaveArea = start_selected_zone.Text;
                    Settings.Default.Save();
                    GameProgress = new Process();

                    if (Is32Bit && Is64Bit || !Is32Bit && !Is64Bit)
                    {
                        MessageBox.Show("Error Getting OS Version");
                        Environment.Exit(0);
                    }
                    else if (Is32Bit)
                        GameProgress.StartInfo.FileName = ".\\Binaries\\Win32\\MMOGame-Win32-Shipping.exe";
                    else if (Is64Bit)
                        GameProgress.StartInfo.FileName = ".\\Binaries\\Win64\\MMOGame-Win64-Shipping.exe";

                    GameProgress.StartInfo.Arguments = arguments;
                    GameProgress.Start();
                    GameProcessTimer.Enabled = true;
                    MainForm_FormClosing(null, null);
                    UILock();
                    InterfaceUpdateTimer.Enabled = false;
                    minimizeToTray.ShowBalloonTip(1000, "", "Game Loading Please Wait. . .", ToolTipIcon.Info);
                }
            });
        }
        public void AccountStartGameFailUpdate(string message)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                UIUnlock(null, null);
                MessageBox.Show("Failed To Start The Game\r\n" + message);
            });
        }
        public void UIUnlock(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                MainTab.Enabled = true;
                InterfaceUpdateTimer.Enabled = false;
            });
        }
        public void GameProgressCheck(object sender, EventArgs e)
        {
            if (GameProgress == null || !MainForm.GameProgress.HasExited)
                return;

            if (LoggedIn)
            {
                LoggedIn = false;
                Network.Instance.SendPacket(new AccountLogOutPacket { });
            }

            UIUnlock(null, null);
            TrayRestoreFromTaskBar(null, null);
            GameProcessTimer.Enabled = false;
        }
        private void LoginAccountLabel_Click(object sender, EventArgs e)
        {
            if (!Network.Instance.Connected)
            {
                LoginErrorLabel.Text = "Not connected to server..";
                LoginErrorLabel.Visible = true;
                return;
            }

            if (AccountTextBox.Text.Length <= 0)
            {
                LoginErrorLabel.Text = "Account Name Cannot Be Empty";
                LoginErrorLabel.Visible = true;
            }
            else if (AccountTextBox.Text.IndexOf(' ') >= 0)
            {
                LoginErrorLabel.Text = "Account Name Cannot Contain Spaces";
                LoginErrorLabel.Visible = true;
            }
            else if (AccountPasswordTextBox.Text.Length <= 0)
            {
                LoginErrorLabel.Text = "Password Cannot Be Blank";
                LoginErrorLabel.Visible = true;
            }
            else if (AccountTextBox.Text.IndexOf(' ') >= 0)
            {
                LoginErrorLabel.Text = "Password Cannot Contain Spaces";
                LoginErrorLabel.Visible = true;
            }
            else
            {
                LoginAccount = AccountTextBox.Text;
                LoginPassword = AccountPasswordTextBox.Text;

                var str = AccountTextBox.Text + '/' + AccountPasswordTextBox.Text;
                Network.Instance.SendPacket(new AccountLogInPacket
                {
                    LoginInformation = Encoding.UTF8.GetBytes(str)
                });
                UILock();

                AccountPasswordTextBox.Text = "";
                InterfaceUpdateTimer.Enabled = true;
            }
        }
        private void Login_ForgotPassword_Click(object sender, EventArgs e) => MainTab.SelectedIndex = 2;
        private void Login_Registertab_Click(object sender, EventArgs e) => MainTab.SelectedIndex = 1;
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            minimizeToTray.Visible = true;
            Hide();
            if (e == null)
                return;
            e.Cancel = true;
        }
        private void TrayRestoreFromTaskBar(object sender, MouseEventArgs e)
        {
            if (e != null && e.Button != MouseButtons.Left)
                return;
            Visible = true;
            minimizeToTray.Visible = false;
        }
        private void Tray_Restore(object sender, EventArgs e)
        {
            Visible = true;
            minimizeToTray.Visible = false;
        }
        private void TrayCloseLauncher(object sender, EventArgs e)
        {
            minimizeToTray.Visible = false;

            if (LoggedIn)
            {
                LoggedIn = false;
                Network.Instance.SendPacket(new AccountLogOutPacket { });
            }
            
            Environment.Exit(Environment.ExitCode);
        }
        private void RegisterAccount_Click(object sender, EventArgs e)
        {
            if (!Network.Instance.Connected)
            {
                RegistrationErrorLabel.Text = "Not connected to server..";
                RegistrationErrorLabel.Visible = true;
                return;
            }

            if (Register_AccountNameTextBox.Text.Length <= 0)
            {
                RegistrationErrorLabel.Text = "Account Name Cannot Be Empty";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_AccountNameTextBox.Text.IndexOf(' ') >= 0)
            {
                RegistrationErrorLabel.Text = "Account Name Cannot Contain Spaces";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_AccountNameTextBox.Text.Length <= 5 || Register_AccountNameTextBox.Text.Length > 12)
            {
                RegistrationErrorLabel.Text = "Account Name Must Be 6 to 12 Characters Long";
                RegistrationErrorLabel.Visible = true;
            }
            else if (!Regex.IsMatch(Register_AccountNameTextBox.Text, "^[a-zA-Z]+.*$"))
            {
                RegistrationErrorLabel.Text = "Account Name Must Start With A Letter";
                RegistrationErrorLabel.Visible = true;
            }
            else if (!Regex.IsMatch(Register_AccountNameTextBox.Text, "^[a-zA-Z_][A-Za-z0-9_]*$"))
            {
                RegistrationErrorLabel.Text = "Account Name Can Only Contain Alphanumeric and Underscores";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_PasswordTextBox.Text.Length <= 0)
            {
                RegistrationErrorLabel.Text = "Password Cannot Be Blank";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_PasswordTextBox.Text.IndexOf(' ') >= 0)
            {
                RegistrationErrorLabel.Text = "Password Cannot Contain Spaces";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_PasswordTextBox.Text.Length <= 5 || Register_PasswordTextBox.Text.Length > 18)
            {
                RegistrationErrorLabel.Text = "Password Must Be 6 to 18 Characters Long";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_QuestionTextBox.Text.Length <= 0)
            {
                RegistrationErrorLabel.Text = "Security Question Cannot Be Empty";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_QuestionTextBox.Text.IndexOf(' ') >= 0)
            {
                RegistrationErrorLabel.Text = "Security Question Cannot Contain Spaces";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_QuestionTextBox.Text.Length <= 1 || Register_QuestionTextBox.Text.Length > 18)
            {
                RegistrationErrorLabel.Text = "Security Question Must Be 2 to 18 Characters Long";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_SecretAnswerTextBox.Text.Length <= 0)
            {
                RegistrationErrorLabel.Text = "Security Answer Cannot Be Empty";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_SecretAnswerTextBox.Text.IndexOf(' ') >= 0)
            {
                RegistrationErrorLabel.Text = "Security Answer Cannot Contain Spaces";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_SecretAnswerTextBox.Text.Length <= 1 || Register_SecretAnswerTextBox.Text.Length > 18)
            {
                RegistrationErrorLabel.Text = "Security Answer Must Be 2 to 18 Characters Long";
                RegistrationErrorLabel.Visible = true;
            }
            else
            {
                RegisterAccount = Register_AccountNameTextBox.Text;
                RegisterPassword = Register_PasswordTextBox.Text;

                var str = Register_AccountNameTextBox.Text + '/' + Register_PasswordTextBox.Text + '/' +
                    Register_QuestionTextBox.Text + '/' + Register_SecretAnswerTextBox.Text + '/' +
                    string.Empty; // ReferralCode // TODO..

                Network.Instance.SendPacket(new AccountRegisterPacket
                {
                    RegistrationInformation = Encoding.UTF8.GetBytes(str)
                });

                /*var p = new AccountRegisterPacket
                {
                    RegistrationInformation = Encoding.UTF8.GetBytes(str)
                };
                var buffer = p.ReadPacket();

                AccountRegisterPacket packet = GamePacket.GetPacket(buffer, out _) as AccountRegisterPacket;
                var buffer2 = packet.ReadPacket();

                string[] array = Encoding.UTF8.GetString(packet.RegistrationInformation).Split('/');*/

                UILock();
                Register_PasswordTextBox.Text = Register_SecretAnswerTextBox.Text = "";
                InterfaceUpdateTimer.Enabled = true;
            }
        }
        private void RegisterBackToLogin_Click(object sender, EventArgs e) => MainTab.SelectedIndex = 0;
        private void Modify_ChangePassword_Click(object sender, EventArgs e)
        {
            if (!Network.Instance.Connected)
            {
                Modify_ErrorLabel.Text = "Not connected to server..";
                Modify_ErrorLabel.Visible = true;
                return;
            }

            if (Modify_AccountNameTextBox.Text.Length <= 0)
            {
                Modify_ErrorLabel.Text = "Account Name Cannot Be Empty";
                Modify_ErrorLabel.Visible = true;
            }
            else if (Modify_AccountNameTextBox.Text.IndexOf(' ') >= 0)
            {
                Modify_ErrorLabel.Text = "Account Name Cannot Contain Spaces";
                Modify_ErrorLabel.Visible = true;
            }
            else if (Modify_PasswordTextBox.Text.Length <= 0)
            {
                Modify_ErrorLabel.Text = "Password Cannot Be Empty";
                Modify_ErrorLabel.Visible = true;
            }
            else if (Modify_PasswordTextBox.Text.IndexOf(' ') >= 0)
            {
                Modify_ErrorLabel.Text = "Password Cannot Contain Spaces";
                Modify_ErrorLabel.Visible = true;
            }
            else if (Modify_PasswordTextBox.Text.Length <= 5 || Modify_PasswordTextBox.Text.Length > 18)
            {
                Modify_ErrorLabel.Text = "Password Must Be 6 to 18 Characters Long";
                Modify_ErrorLabel.Visible = true;
            }
            else if (Modify_QuestionTextBox.Text.Length <= 0)
            {
                Modify_ErrorLabel.Text = "Security Question Cannot Be Empty";
                Modify_ErrorLabel.Visible = true;
            }
            else if (Modify_QuestionTextBox.Text.IndexOf(' ') >= 0)
            {
                Modify_ErrorLabel.Text = "Security Question Cannot Contain Spaces";
                Modify_ErrorLabel.Visible = true;
            }
            else if (Modify_AnswerTextBox.Text.Length <= 0)
            {
                Modify_ErrorLabel.Text = "Security Answer Cannot Be Empty";
                Modify_ErrorLabel.Visible = true;
            }
            else if (Modify_AnswerTextBox.Text.IndexOf(' ') >= 0)
            {
                Modify_ErrorLabel.Text = "Security Answer Cannot Contain Spaces";
                Modify_ErrorLabel.Visible = true;
            }
            else
            {
                ModifyAccount = Modify_AccountNameTextBox.Text;
                ModifyPassword = Modify_PasswordTextBox.Text;

                var str = Modify_AccountNameTextBox.Text + '/' + Modify_PasswordTextBox.Text + '/' +
                    Modify_QuestionTextBox.Text + '/' + Modify_AnswerTextBox.Text;
                Network.Instance.SendPacket(new AccountChangePasswordPacket
                {
                    AccountInformation = Encoding.UTF8.GetBytes(str)
                });
                UILock();
                Modify_PasswordTextBox.Text = Modify_AnswerTextBox.Text = "";
                InterfaceUpdateTimer.Enabled = true;
            }
        }
        private void Modify_BackToLogin_Click(object sender, EventArgs e) => MainTab.SelectedIndex = 0;
        private void Launch_EnterGame_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(LoginAccount) || !LoggedIn)
                MainTab.SelectedIndex = 0;
            else if (string.IsNullOrEmpty(start_selected_zone.Text))
            {
                MessageBox.Show("You must select a server");
            }
            else if (!ServerTable.ContainsKey(start_selected_zone.Text))
            {
                MessageBox.Show("Server selection error");
            }
            else
            {
                var str = LoginAccount + '/' + start_selected_zone.Text;
                Network.Instance.SendPacket(new AccountStartGamePacket
                {
                    LoginInformation = Encoding.UTF8.GetBytes(str)
                });
                UILock();
                InterfaceUpdateTimer.Enabled = true;
            }
        }
        private void LogoutTab_Click(object sender, EventArgs e)
        {
            LoginAccount = null;
            LoginPassword = null;
            MainTab.SelectedIndex = 0;
        }
        private void StartupChoosegameServer_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            StringFormat format = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            e.Graphics.DrawString(this.GameServerList.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), (RectangleF)e.Bounds, format);
        }
        private void StartupChooseGS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GameServerList.SelectedIndex < 0)
                start_selected_zone.Text = "";
            else
                start_selected_zone.Text = GameServerList.Items[GameServerList.SelectedIndex].ToString();
        }
        private void Login_PasswordKeyPress(object sender, KeyPressEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AccountPasswordTextBox.Text)) return;
            if (e.KeyChar != (char)13) return;
            LoginAccountLabel_Click(sender, null);
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        private void pictureBox3_MouseClick(object sender, MouseEventArgs e)
        {
            Environment.Exit(0);
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        private void uiCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            uiCheckBox2.Checked = !uiCheckBox1.Checked;
        }
        private void uiCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            uiCheckBox1.Checked = !uiCheckBox2.Checked;
        }
    }
}
