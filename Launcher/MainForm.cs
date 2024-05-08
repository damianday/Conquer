using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using GamePackets.Client;

namespace Launcher
{
    public partial class MainForm : Form
    {
        private Point offset; // Used to store the offset between the mouse cursor and the form's location
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
            Settings.Default.Load();

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

            start_selected_zone.Text = Settings.Default.ServerName;
            AccountTextBox.Text = Settings.Default.AccountName;

            ConnectionStatusLabel.Text = "Attempting to connect to the server.";
            Network.Instance.Connect();
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
            BeginInvoke(() =>
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

            BeginInvoke(() =>
            {
                UIUnlock(null, null);
                RegistrationErrorLabel.Text = message;
                RegistrationErrorLabel.Visible = true;
                RegistrationErrorLabel.ForeColor = Color.Red;
            });
        }
        public void AccountChangePasswordSuccessUpdate()
        {
            BeginInvoke(() =>
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

            BeginInvoke(() =>
            {
                UIUnlock(null, null);
                Modify_ErrorLabel.Text = message;
                Modify_ErrorLabel.Visible = true;
            });
        }
        public void AccountLogInSuccessUpdate(string data)
        {
            LoggedIn = true;

            BeginInvoke(() =>
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

                    if (!int.TryParse(arr[1], out port) || string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(name))
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
                Settings.Default.AccountName = LoginAccount;
                Settings.Default.Save();
            });
        }
        public void AccountLogInFailUpdate(string message)
        {
            LoggedIn = false;
            LoginAccount = string.Empty;
            LoginPassword = string.Empty;

            BeginInvoke(() =>
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

            BeginInvoke(() =>
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
            BeginInvoke(() =>
            {
                if (ServerTable.TryGetValue(start_selected_zone.Text, out var value))
                {
                    string arguments = "-wegame=" + $"1,1,{value.PublicAddress.Address},{value.PublicAddress.Port}," +
                        $"1,1,{value.PublicAddress.Address},{value.PublicAddress.Port}," + start_selected_zone.Text + "  " +
                        $"/ip:1,1,{value.PublicAddress.Address} " + $"/port:{value.PublicAddress.Port} " +
                        "/ticket:" + ticket +
                        " /AreaName:" + start_selected_zone.Text;

                    Settings.Default.ServerName = start_selected_zone.Text;
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
            BeginInvoke(() =>
            {
                UIUnlock(null, null);
                MessageBox.Show("Failed To Start The Game\r\n" + message);
            });
        }
        public void UIUnlock(object sender, EventArgs e)
        {
            BeginInvoke(() =>
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
                RegistrationErrorLabel.Text = "Account name cannot be empty";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_AccountNameTextBox.Text.IndexOf(' ') >= 0)
            {
                RegistrationErrorLabel.Text = "Account name cannot contain spaces";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_AccountNameTextBox.Text.Length <= 5 || Register_AccountNameTextBox.Text.Length > 12)
            {
                RegistrationErrorLabel.Text = "Account name must be 6 to 12 characters long";
                RegistrationErrorLabel.Visible = true;
            }
            else if (!Regex.IsMatch(Register_AccountNameTextBox.Text, "^[a-zA-Z]+.*$"))
            {
                RegistrationErrorLabel.Text = "Account name must start with a letter";
                RegistrationErrorLabel.Visible = true;
            }
            else if (!Regex.IsMatch(Register_AccountNameTextBox.Text, "^[a-zA-Z_][A-Za-z0-9_]*$"))
            {
                RegistrationErrorLabel.Text = "Account name can only contain alphanumeric and underscores";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_PasswordTextBox.Text.Length <= 0)
            {
                RegistrationErrorLabel.Text = "Password cannot be empty";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_PasswordTextBox.Text.IndexOf(' ') >= 0)
            {
                RegistrationErrorLabel.Text = "Password cannot contain spaces";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_PasswordTextBox.Text.Length <= 5 || Register_PasswordTextBox.Text.Length > 18)
            {
                RegistrationErrorLabel.Text = "Password must be 6 to 18 characters long";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_QuestionTextBox.Text.Length <= 0)
            {
                RegistrationErrorLabel.Text = "Security question cannot be empty";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_QuestionTextBox.Text.IndexOf(' ') >= 0)
            {
                RegistrationErrorLabel.Text = "Security question cannot contain spaces";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_QuestionTextBox.Text.Length <= 1 || Register_QuestionTextBox.Text.Length > 18)
            {
                RegistrationErrorLabel.Text = "Security question must me 2 to 18 characters long";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_SecretAnswerTextBox.Text.Length <= 0)
            {
                RegistrationErrorLabel.Text = "Security answer cannot be empty";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_SecretAnswerTextBox.Text.IndexOf(' ') >= 0)
            {
                RegistrationErrorLabel.Text = "Security answer cannot contain spaces";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_SecretAnswerTextBox.Text.Length <= 1 || Register_SecretAnswerTextBox.Text.Length > 18)
            {
                RegistrationErrorLabel.Text = "Security answer must be 2 to 18 characters long";
                RegistrationErrorLabel.Visible = true;
            }
            else if (Register_ReferralCodeTextBox.Text.Length > 0 && Register_ReferralCodeTextBox.Text.Length != 4)
            {
                RegistrationErrorLabel.Text = "Referral code must me 4 characters long";
                RegistrationErrorLabel.Visible = true;
            }
            else
            {
                RegisterAccount = Register_AccountNameTextBox.Text;
                RegisterPassword = Register_PasswordTextBox.Text;

                var str = Register_AccountNameTextBox.Text + '/' + Register_PasswordTextBox.Text + '/' +
                    Register_QuestionTextBox.Text + '/' + Register_SecretAnswerTextBox.Text + '/' +
                    Register_ReferralCodeTextBox.Text;

                Network.Instance.SendPacket(new AccountRegisterPacket
                {
                    RegistrationInformation = Encoding.UTF8.GetBytes(str)
                });

                UILock();
                Register_AccountNameTextBox.Text = string.Empty;
                Register_PasswordTextBox.Text = string.Empty;
                Register_QuestionTextBox.Text = string.Empty;
                Register_SecretAnswerTextBox.Text = string.Empty;
                Register_ReferralCodeTextBox.Text = string.Empty;
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

        private void ConfigButton_Click(object sender, EventArgs e)
        {
            ConfigForm configForm = new ConfigForm();
            configForm.Show();
        }

        private void AccountLoginTab_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Capture the offset between the mouse cursor and the form's location
                offset = new Point(e.X, e.Y);
            }
        }

        private void AccountLoginTab_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the new location of the form based on the offset
                Point newLocation = this.PointToScreen(new Point(e.X, e.Y));
                newLocation.Offset(-offset.X, -offset.Y);

                // Set the new location of the form
                this.Location = newLocation;
            }
        }

        private void AccountLoginTab_MouseUp(object sender, MouseEventArgs e)
        {
            // Reset the offset when the mouse button is released
            offset = Point.Empty;
        }

        private void MainTab_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Capture the offset between the mouse cursor and the form's location
                offset = new Point(e.X, e.Y);
            }
        }

        private void MainTab_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the new location of the form based on the offset
                Point newLocation = this.PointToScreen(new Point(e.X, e.Y));
                newLocation.Offset(-offset.X, -offset.Y);

                // Set the new location of the form
                this.Location = newLocation;
            }
        }

        private void MainTab_MouseUp(object sender, MouseEventArgs e)
        {
            // Reset the offset when the mouse button is released
            offset = Point.Empty;
        }
    }
}
