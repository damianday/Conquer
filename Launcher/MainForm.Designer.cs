using Sunny.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Launcher
{
    public partial class MainForm : global::System.Windows.Forms.Form
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
            MainTab = new UITabControl();
            AccountLoginTab = new TabPage();
            ConnectionStatusLabel = new UILabel();
            pictureBox2 = new PictureBox();
            pictureBox1 = new PictureBox();
            LoginAccountLabel = new UISymbolButton();
            ForgotPasswordLabel = new UILinkLabel();
            AccountPasswordTextBox = new UITextBox();
            RegisterAccountLabel = new UISymbolButton();
            LoginErrorLabel = new UILabel();
            AccountTextBox = new UITextBox();
            RegistrationTab = new TabPage();
            pictureBox3 = new PictureBox();
            Register_Back_To_LoginBtn = new UISymbolButton();
            RegistrationErrorLabel = new UILabel();
            Register_AccountBtn = new UISymbolButton();
            Register_ReferralCodeTextBox = new UITextBox();
            Register_SecretAnswerTextBox = new UITextBox();
            Register_PasswordTextBox = new UITextBox();
            Register_QuestionTextBox = new UITextBox();
            Register_AccountNameTextBox = new UITextBox();
            ChangePasswordTab = new TabPage();
            pictureBox4 = new PictureBox();
            Modify_Back_To_LoginBtn = new UISymbolButton();
            Modify_ErrorLabel = new UILabel();
            Modify_PasswordBtn = new UISymbolButton();
            Modify_AnswerTextBox = new UITextBox();
            Modify_PasswordTextBox = new UITextBox();
            Modify_QuestionTextBox = new UITextBox();
            Modify_AccountNameTextBox = new UITextBox();
            StartGameTab = new TabPage();
            uiCheckBox2 = new UICheckBox();
            uiCheckBox1 = new UICheckBox();
            label1 = new Label();
            pictureBox5 = new PictureBox();
            activate_account = new UISymbolButton();
            start_selected_zone = new UILinkLabel();
            GameServerList = new ListBox();
            Launcher_enterGameBtn = new UIButton();
            InterfaceUpdateTimer = new Timer(components);
            DataProcessTimer = new Timer(components);
            minimizeToTray = new NotifyIcon(components);
            TrayRightClickMenu = new ContextMenuStrip(components);
            OpenToolStripMenuItem = new ToolStripMenuItem();
            QuitToolStripMenuItem = new ToolStripMenuItem();
            GameProcessTimer = new Timer(components);
            MainTab.SuspendLayout();
            AccountLoginTab.SuspendLayout();
            ((ISupportInitialize)pictureBox2).BeginInit();
            ((ISupportInitialize)pictureBox1).BeginInit();
            RegistrationTab.SuspendLayout();
            ((ISupportInitialize)pictureBox3).BeginInit();
            ChangePasswordTab.SuspendLayout();
            ((ISupportInitialize)pictureBox4).BeginInit();
            StartGameTab.SuspendLayout();
            ((ISupportInitialize)pictureBox5).BeginInit();
            TrayRightClickMenu.SuspendLayout();
            SuspendLayout();
            // 
            // MainTab
            // 
            MainTab.Controls.Add(AccountLoginTab);
            MainTab.Controls.Add(RegistrationTab);
            MainTab.Controls.Add(ChangePasswordTab);
            MainTab.Controls.Add(StartGameTab);
            MainTab.DrawMode = TabDrawMode.OwnerDrawFixed;
            MainTab.FillColor = Color.Transparent;
            MainTab.Font = new Font("Arial", 12F);
            MainTab.ItemSize = new Size(140, 15);
            MainTab.Location = new Point(0, 0);
            MainTab.MainPage = "";
            MainTab.Margin = new Padding(0);
            MainTab.MenuStyle = UIMenuStyle.Custom;
            MainTab.Name = "MainTab";
            MainTab.Padding = new Point(0, 0);
            MainTab.SelectedIndex = 0;
            MainTab.Size = new Size(600, 430);
            MainTab.SizeMode = TabSizeMode.Fixed;
            MainTab.Style = UIStyle.Custom;
            MainTab.StyleCustomMode = true;
            MainTab.TabBackColor = Color.Transparent;
            MainTab.TabIndex = 9;
            MainTab.TabSelectedColor = Color.Transparent;
            MainTab.TabSelectedForeColor = Color.Transparent;
            MainTab.TabSelectedHighColor = Color.Transparent;
            MainTab.TabSelectedHighColorSize = 0;
            MainTab.TabStop = false;
            MainTab.TabUnSelectedForeColor = Color.Transparent;
            MainTab.TipsFont = new Font("Arial", 9F);
            MainTab.TipsForeColor = Color.Transparent;
            MainTab.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            // 
            // AccountLoginTab
            // 
            AccountLoginTab.BackColor = Color.Transparent;
            AccountLoginTab.BackgroundImage = (Image)resources.GetObject("AccountLoginTab.BackgroundImage");
            AccountLoginTab.BackgroundImageLayout = ImageLayout.None;
            AccountLoginTab.Controls.Add(ConnectionStatusLabel);
            AccountLoginTab.Controls.Add(pictureBox2);
            AccountLoginTab.Controls.Add(pictureBox1);
            AccountLoginTab.Controls.Add(LoginAccountLabel);
            AccountLoginTab.Controls.Add(ForgotPasswordLabel);
            AccountLoginTab.Controls.Add(AccountPasswordTextBox);
            AccountLoginTab.Controls.Add(RegisterAccountLabel);
            AccountLoginTab.Controls.Add(LoginErrorLabel);
            AccountLoginTab.Controls.Add(AccountTextBox);
            AccountLoginTab.Location = new Point(0, 15);
            AccountLoginTab.Margin = new Padding(0);
            AccountLoginTab.Name = "AccountLoginTab";
            AccountLoginTab.Size = new Size(600, 415);
            AccountLoginTab.TabIndex = 0;
            AccountLoginTab.Text = "Login";
            // 
            // ConnectionStatusLabel
            // 
            ConnectionStatusLabel.AutoSize = true;
            ConnectionStatusLabel.Font = new Font("Arial", 9F);
            ConnectionStatusLabel.ForeColor = Color.DodgerBlue;
            ConnectionStatusLabel.Location = new Point(42, 352);
            ConnectionStatusLabel.Margin = new Padding(4, 0, 4, 0);
            ConnectionStatusLabel.Name = "ConnectionStatusLabel";
            ConnectionStatusLabel.Size = new Size(28, 15);
            ConnectionStatusLabel.Style = UIStyle.Custom;
            ConnectionStatusLabel.TabIndex = 19;
            ConnectionStatusLabel.Text = "???";
            ConnectionStatusLabel.TextAlign = ContentAlignment.MiddleCenter;
            ConnectionStatusLabel.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            // 
            // pictureBox2
            // 
            pictureBox2.Location = new Point(553, 23);
            pictureBox2.Margin = new Padding(0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(22, 22);
            pictureBox2.TabIndex = 18;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(127, 23);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(384, 127);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 17;
            pictureBox1.TabStop = false;
            // 
            // LoginAccountLabel
            // 
            LoginAccountLabel.BackgroundImage = (Image)resources.GetObject("LoginAccountLabel.BackgroundImage");
            LoginAccountLabel.BackgroundImageLayout = ImageLayout.Stretch;
            LoginAccountLabel.Cursor = Cursors.Hand;
            LoginAccountLabel.FillColor = Color.Transparent;
            LoginAccountLabel.FillColor2 = Color.Transparent;
            LoginAccountLabel.FillDisableColor = Color.Transparent;
            LoginAccountLabel.FillHoverColor = Color.Transparent;
            LoginAccountLabel.FillPressColor = Color.Transparent;
            LoginAccountLabel.FillSelectedColor = Color.Transparent;
            LoginAccountLabel.Font = new Font("Arial", 12F);
            LoginAccountLabel.ForeColor = Color.LightSkyBlue;
            LoginAccountLabel.Location = new Point(364, 291);
            LoginAccountLabel.Margin = new Padding(4, 2, 4, 2);
            LoginAccountLabel.MinimumSize = new Size(1, 1);
            LoginAccountLabel.Name = "LoginAccountLabel";
            LoginAccountLabel.RectColor = Color.Transparent;
            LoginAccountLabel.RectHoverColor = Color.LightSkyBlue;
            LoginAccountLabel.RectPressColor = Color.DodgerBlue;
            LoginAccountLabel.RectSelectedColor = Color.FromArgb(184, 64, 64);
            LoginAccountLabel.Size = new Size(195, 36);
            LoginAccountLabel.Style = UIStyle.Custom;
            LoginAccountLabel.Symbol = 0;
            LoginAccountLabel.SymbolHoverColor = Color.DodgerBlue;
            LoginAccountLabel.TabIndex = 2;
            LoginAccountLabel.TabStop = false;
            LoginAccountLabel.Text = "Login";
            LoginAccountLabel.TipsFont = new Font("Arial", 9F);
            LoginAccountLabel.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            LoginAccountLabel.Click += LoginAccountLabel_Click;
            // 
            // ForgotPasswordLabel
            // 
            ForgotPasswordLabel.ActiveLinkColor = Color.DodgerBlue;
            ForgotPasswordLabel.Font = new Font("Arial", 9F);
            ForgotPasswordLabel.LinkBehavior = LinkBehavior.AlwaysUnderline;
            ForgotPasswordLabel.LinkColor = Color.LightSkyBlue;
            ForgotPasswordLabel.Location = new Point(250, 220);
            ForgotPasswordLabel.Margin = new Padding(4, 0, 4, 0);
            ForgotPasswordLabel.Name = "ForgotPasswordLabel";
            ForgotPasswordLabel.Size = new Size(134, 23);
            ForgotPasswordLabel.Style = UIStyle.Custom;
            ForgotPasswordLabel.TabIndex = 16;
            ForgotPasswordLabel.TabStop = true;
            ForgotPasswordLabel.Text = "Forgot Password?";
            ForgotPasswordLabel.TextAlign = ContentAlignment.MiddleCenter;
            ForgotPasswordLabel.VisitedLinkColor = Color.FromArgb(230, 80, 80);
            ForgotPasswordLabel.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            ForgotPasswordLabel.Click += Login_ForgotPassword_Click;
            // 
            // AccountPasswordTextBox
            // 
            AccountPasswordTextBox.ButtonFillColor = Color.FromArgb(230, 80, 80);
            AccountPasswordTextBox.ButtonFillHoverColor = Color.FromArgb(235, 115, 115);
            AccountPasswordTextBox.ButtonFillPressColor = Color.FromArgb(184, 64, 64);
            AccountPasswordTextBox.ButtonRectColor = Color.FromArgb(230, 80, 80);
            AccountPasswordTextBox.ButtonRectHoverColor = Color.FromArgb(235, 115, 115);
            AccountPasswordTextBox.ButtonRectPressColor = Color.FromArgb(184, 64, 64);
            AccountPasswordTextBox.Cursor = Cursors.IBeam;
            AccountPasswordTextBox.FillColor = Color.FromArgb(64, 64, 64);
            AccountPasswordTextBox.FillColor2 = Color.Transparent;
            AccountPasswordTextBox.Font = new Font("Arial", 12F);
            AccountPasswordTextBox.ForeColor = Color.DodgerBlue;
            AccountPasswordTextBox.Location = new Point(321, 159);
            AccountPasswordTextBox.Margin = new Padding(4, 5, 4, 5);
            AccountPasswordTextBox.MinimumSize = new Size(1, 14);
            AccountPasswordTextBox.Name = "AccountPasswordTextBox";
            AccountPasswordTextBox.PasswordChar = '*';
            AccountPasswordTextBox.RectColor = Color.LightSkyBlue;
            AccountPasswordTextBox.ScrollBarBackColor = Color.Transparent;
            AccountPasswordTextBox.ScrollBarColor = Color.Transparent;
            AccountPasswordTextBox.ShowText = false;
            AccountPasswordTextBox.Size = new Size(190, 36);
            AccountPasswordTextBox.Style = UIStyle.Custom;
            AccountPasswordTextBox.Symbol = 61475;
            AccountPasswordTextBox.SymbolColor = Color.LightSkyBlue;
            AccountPasswordTextBox.SymbolSize = 22;
            AccountPasswordTextBox.TabIndex = 1;
            AccountPasswordTextBox.TextAlignment = ContentAlignment.MiddleLeft;
            AccountPasswordTextBox.Watermark = "Password";
            AccountPasswordTextBox.WatermarkActiveColor = Color.DodgerBlue;
            AccountPasswordTextBox.WatermarkColor = Color.LightSkyBlue;
            AccountPasswordTextBox.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            AccountPasswordTextBox.KeyPress += Login_PasswordKeyPress;
            // 
            // RegisterAccountLabel
            // 
            RegisterAccountLabel.BackgroundImage = (Image)resources.GetObject("RegisterAccountLabel.BackgroundImage");
            RegisterAccountLabel.BackgroundImageLayout = ImageLayout.Stretch;
            RegisterAccountLabel.Cursor = Cursors.Hand;
            RegisterAccountLabel.FillColor = Color.Transparent;
            RegisterAccountLabel.FillColor2 = Color.Transparent;
            RegisterAccountLabel.FillHoverColor = Color.Transparent;
            RegisterAccountLabel.FillPressColor = Color.Transparent;
            RegisterAccountLabel.FillSelectedColor = Color.Transparent;
            RegisterAccountLabel.Font = new Font("Arial", 12F);
            RegisterAccountLabel.ForeColor = Color.LightSkyBlue;
            RegisterAccountLabel.Location = new Point(364, 331);
            RegisterAccountLabel.Margin = new Padding(4, 2, 4, 2);
            RegisterAccountLabel.MinimumSize = new Size(1, 1);
            RegisterAccountLabel.Name = "RegisterAccountLabel";
            RegisterAccountLabel.RectColor = Color.Transparent;
            RegisterAccountLabel.RectHoverColor = Color.LightSkyBlue;
            RegisterAccountLabel.RectPressColor = Color.DodgerBlue;
            RegisterAccountLabel.RectSelectedColor = Color.FromArgb(184, 64, 64);
            RegisterAccountLabel.Size = new Size(195, 36);
            RegisterAccountLabel.Style = UIStyle.Custom;
            RegisterAccountLabel.Symbol = 0;
            RegisterAccountLabel.SymbolHoverColor = Color.DodgerBlue;
            RegisterAccountLabel.TabIndex = 3;
            RegisterAccountLabel.TabStop = false;
            RegisterAccountLabel.Text = "Create Account";
            RegisterAccountLabel.TipsColor = Color.FromArgb(128, 255, 128);
            RegisterAccountLabel.TipsFont = new Font("Arial", 9F);
            RegisterAccountLabel.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            RegisterAccountLabel.Click += Login_Registertab_Click;
            // 
            // login_error_label
            // 
            LoginErrorLabel.Font = new Font("Arial", 9F);
            LoginErrorLabel.ForeColor = Color.DodgerBlue;
            LoginErrorLabel.Location = new Point(120, 200);
            LoginErrorLabel.Margin = new Padding(4, 0, 4, 0);
            LoginErrorLabel.Name = "login_error_label";
            LoginErrorLabel.Size = new Size(391, 20);
            LoginErrorLabel.Style = UIStyle.Custom;
            LoginErrorLabel.TabIndex = 15;
            LoginErrorLabel.Text = "Error message";
            LoginErrorLabel.TextAlign = ContentAlignment.MiddleCenter;
            LoginErrorLabel.Visible = false;
            LoginErrorLabel.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            // 
            // AccountTextBox
            // 
            AccountTextBox.ButtonFillColor = Color.FromArgb(230, 80, 80);
            AccountTextBox.ButtonFillHoverColor = Color.FromArgb(235, 115, 115);
            AccountTextBox.ButtonFillPressColor = Color.FromArgb(184, 64, 64);
            AccountTextBox.ButtonForeColor = Color.Transparent;
            AccountTextBox.ButtonForeHoverColor = Color.Transparent;
            AccountTextBox.ButtonForePressColor = Color.Transparent;
            AccountTextBox.ButtonRectColor = Color.FromArgb(230, 80, 80);
            AccountTextBox.ButtonRectHoverColor = Color.FromArgb(235, 115, 115);
            AccountTextBox.ButtonRectPressColor = Color.FromArgb(184, 64, 64);
            AccountTextBox.Cursor = Cursors.IBeam;
            AccountTextBox.FillColor = Color.FromArgb(64, 64, 64);
            AccountTextBox.FillColor2 = Color.Transparent;
            AccountTextBox.Font = new Font("Arial", 12F);
            AccountTextBox.ForeColor = Color.DodgerBlue;
            AccountTextBox.Location = new Point(120, 159);
            AccountTextBox.Margin = new Padding(4, 5, 4, 5);
            AccountTextBox.MinimumSize = new Size(1, 14);
            AccountTextBox.Name = "AccountTextBox";
            AccountTextBox.RectColor = Color.LightSkyBlue;
            AccountTextBox.RectDisableColor = Color.White;
            AccountTextBox.RectReadOnlyColor = Color.White;
            AccountTextBox.ScrollBarBackColor = Color.Transparent;
            AccountTextBox.ScrollBarColor = Color.Transparent;
            AccountTextBox.ShowText = false;
            AccountTextBox.Size = new Size(190, 36);
            AccountTextBox.Style = UIStyle.Custom;
            AccountTextBox.Symbol = 61447;
            AccountTextBox.SymbolColor = Color.LightSkyBlue;
            AccountTextBox.SymbolSize = 22;
            AccountTextBox.TabIndex = 0;
            AccountTextBox.TextAlignment = ContentAlignment.MiddleLeft;
            AccountTextBox.Watermark = "Account Name";
            AccountTextBox.WatermarkActiveColor = Color.DodgerBlue;
            AccountTextBox.WatermarkColor = Color.LightSkyBlue;
            AccountTextBox.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            // 
            // RegistrationTab
            // 
            RegistrationTab.BackColor = Color.Transparent;
            RegistrationTab.BackgroundImage = (Image)resources.GetObject("RegistrationTab.BackgroundImage");
            RegistrationTab.BackgroundImageLayout = ImageLayout.None;
            RegistrationTab.Controls.Add(pictureBox3);
            RegistrationTab.Controls.Add(Register_Back_To_LoginBtn);
            RegistrationTab.Controls.Add(RegistrationErrorLabel);
            RegistrationTab.Controls.Add(Register_AccountBtn);
            RegistrationTab.Controls.Add(Register_ReferralCodeTextBox);
            RegistrationTab.Controls.Add(Register_SecretAnswerTextBox);
            RegistrationTab.Controls.Add(Register_PasswordTextBox);
            RegistrationTab.Controls.Add(Register_QuestionTextBox);
            RegistrationTab.Controls.Add(Register_AccountNameTextBox);
            RegistrationTab.Location = new Point(0, 15);
            RegistrationTab.Margin = new Padding(0);
            RegistrationTab.Name = "RegistrationTab";
            RegistrationTab.Size = new Size(200, 85);
            RegistrationTab.TabIndex = 1;
            RegistrationTab.Text = "Create Account";
            // 
            // pictureBox3
            // 
            pictureBox3.Location = new Point(553, 23);
            pictureBox3.Margin = new Padding(0);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(22, 22);
            pictureBox3.TabIndex = 21;
            pictureBox3.TabStop = false;
            pictureBox3.MouseClick += pictureBox3_MouseClick;
            // 
            // Register_Back_To_LoginBtn
            // 
            Register_Back_To_LoginBtn.BackgroundImage = (Image)resources.GetObject("Register_Back_To_LoginBtn.BackgroundImage");
            Register_Back_To_LoginBtn.Cursor = Cursors.Hand;
            Register_Back_To_LoginBtn.FillColor = Color.Transparent;
            Register_Back_To_LoginBtn.FillColor2 = Color.Transparent;
            Register_Back_To_LoginBtn.FillHoverColor = Color.FromArgb(235, 115, 115);
            Register_Back_To_LoginBtn.FillPressColor = Color.FromArgb(184, 64, 64);
            Register_Back_To_LoginBtn.FillSelectedColor = Color.FromArgb(184, 64, 64);
            Register_Back_To_LoginBtn.Font = new Font("Arial", 12F);
            Register_Back_To_LoginBtn.ForeColor = Color.LightSkyBlue;
            Register_Back_To_LoginBtn.Location = new Point(120, 330);
            Register_Back_To_LoginBtn.Margin = new Padding(4, 2, 4, 2);
            Register_Back_To_LoginBtn.MinimumSize = new Size(1, 1);
            Register_Back_To_LoginBtn.Name = "Register_Back_To_LoginBtn";
            Register_Back_To_LoginBtn.RectColor = Color.Transparent;
            Register_Back_To_LoginBtn.RectHoverColor = Color.LightSkyBlue;
            Register_Back_To_LoginBtn.RectPressColor = Color.DodgerBlue;
            Register_Back_To_LoginBtn.RectSelectedColor = Color.FromArgb(184, 64, 64);
            Register_Back_To_LoginBtn.Size = new Size(391, 36);
            Register_Back_To_LoginBtn.Style = UIStyle.Custom;
            Register_Back_To_LoginBtn.Symbol = 0;
            Register_Back_To_LoginBtn.SymbolHoverColor = Color.DodgerBlue;
            Register_Back_To_LoginBtn.TabIndex = 20;
            Register_Back_To_LoginBtn.TabStop = false;
            Register_Back_To_LoginBtn.Text = "Return To Login";
            Register_Back_To_LoginBtn.TipsColor = Color.FromArgb(128, 255, 128);
            Register_Back_To_LoginBtn.TipsFont = new Font("Arial", 9F);
            Register_Back_To_LoginBtn.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            Register_Back_To_LoginBtn.Click += RegisterBackToLogin_Click;
            // 
            // RegistrationErrorLabel
            // 
            RegistrationErrorLabel.Font = new Font("Arial", 9F);
            RegistrationErrorLabel.ForeColor = Color.DodgerBlue;
            RegistrationErrorLabel.Location = new Point(120, 246);
            RegistrationErrorLabel.Margin = new Padding(4, 0, 4, 0);
            RegistrationErrorLabel.Name = "RegistrationErrorLabel";
            RegistrationErrorLabel.Size = new Size(391, 20);
            RegistrationErrorLabel.Style = UIStyle.Custom;
            RegistrationErrorLabel.TabIndex = 17;
            RegistrationErrorLabel.Text = "Error message";
            RegistrationErrorLabel.TextAlign = ContentAlignment.MiddleCenter;
            RegistrationErrorLabel.Visible = false;
            RegistrationErrorLabel.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            // 
            // Register_AccountBtn
            // 
            Register_AccountBtn.BackgroundImage = (Image)resources.GetObject("Register_AccountBtn.BackgroundImage");
            Register_AccountBtn.Cursor = Cursors.Hand;
            Register_AccountBtn.FillColor = Color.Transparent;
            Register_AccountBtn.FillColor2 = Color.Transparent;
            Register_AccountBtn.FillDisableColor = Color.Transparent;
            Register_AccountBtn.FillHoverColor = Color.Transparent;
            Register_AccountBtn.FillPressColor = Color.Transparent;
            Register_AccountBtn.FillSelectedColor = Color.Transparent;
            Register_AccountBtn.Font = new Font("Arial", 12F);
            Register_AccountBtn.ForeColor = Color.LightSkyBlue;
            Register_AccountBtn.Location = new Point(120, 290);
            Register_AccountBtn.Margin = new Padding(4, 2, 4, 2);
            Register_AccountBtn.MinimumSize = new Size(1, 1);
            Register_AccountBtn.Name = "Register_AccountBtn";
            Register_AccountBtn.RectColor = Color.Transparent;
            Register_AccountBtn.RectHoverColor = Color.LightSkyBlue;
            Register_AccountBtn.RectPressColor = Color.DodgerBlue;
            Register_AccountBtn.RectSelectedColor = Color.FromArgb(184, 64, 64);
            Register_AccountBtn.Size = new Size(391, 36);
            Register_AccountBtn.Style = UIStyle.Custom;
            Register_AccountBtn.Symbol = 0;
            Register_AccountBtn.SymbolHoverColor = Color.DodgerBlue;
            Register_AccountBtn.TabIndex = 16;
            Register_AccountBtn.TabStop = false;
            Register_AccountBtn.Text = "Create New Account";
            Register_AccountBtn.TipsColor = Color.FromArgb(128, 255, 128);
            Register_AccountBtn.TipsFont = new Font("Arial", 9F);
            Register_AccountBtn.TipsForeColor = Color.LightSkyBlue;
            Register_AccountBtn.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            Register_AccountBtn.Click += RegisterAccount_Click;
            // 
            // Register_ReferralCodeTextBox
            // 
            Register_ReferralCodeTextBox.ButtonFillColor = Color.FromArgb(230, 80, 80);
            Register_ReferralCodeTextBox.ButtonFillHoverColor = Color.FromArgb(235, 115, 115);
            Register_ReferralCodeTextBox.ButtonFillPressColor = Color.FromArgb(184, 64, 64);
            Register_ReferralCodeTextBox.ButtonRectColor = Color.FromArgb(230, 80, 80);
            Register_ReferralCodeTextBox.ButtonRectHoverColor = Color.FromArgb(235, 115, 115);
            Register_ReferralCodeTextBox.ButtonRectPressColor = Color.FromArgb(184, 64, 64);
            Register_ReferralCodeTextBox.Cursor = Cursors.IBeam;
            Register_ReferralCodeTextBox.FillColor = Color.FromArgb(64, 64, 64);
            Register_ReferralCodeTextBox.FillColor2 = Color.Transparent;
            Register_ReferralCodeTextBox.Font = new Font("Arial", 12F);
            Register_ReferralCodeTextBox.ForeColor = Color.DodgerBlue;
            Register_ReferralCodeTextBox.Location = new Point(120, 229);
            Register_ReferralCodeTextBox.Margin = new Padding(4, 5, 4, 5);
            Register_ReferralCodeTextBox.MinimumSize = new Size(1, 14);
            Register_ReferralCodeTextBox.Name = "Register_ReferralCodeTextBox";
            Register_ReferralCodeTextBox.RectColor = Color.LightSkyBlue;
            Register_ReferralCodeTextBox.ScrollBarColor = Color.FromArgb(230, 80, 80);
            Register_ReferralCodeTextBox.ShowText = false;
            Register_ReferralCodeTextBox.Size = new Size(391, 36);
            Register_ReferralCodeTextBox.Style = UIStyle.Custom;
            Register_ReferralCodeTextBox.Symbol = 61447;
            Register_ReferralCodeTextBox.SymbolColor = Color.LightSkyBlue;
            Register_ReferralCodeTextBox.SymbolSize = 22;
            Register_ReferralCodeTextBox.TabIndex = 1;
            Register_ReferralCodeTextBox.TextAlignment = ContentAlignment.MiddleLeft;
            Register_ReferralCodeTextBox.Watermark = "Referral Code - Optional";
            Register_ReferralCodeTextBox.WatermarkActiveColor = Color.DodgerBlue;
            Register_ReferralCodeTextBox.WatermarkColor = Color.LightSkyBlue;
            Register_ReferralCodeTextBox.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            // 
            // Register_SecretAnswerTextBox
            // 
            Register_SecretAnswerTextBox.ButtonFillColor = Color.FromArgb(230, 80, 80);
            Register_SecretAnswerTextBox.ButtonFillHoverColor = Color.FromArgb(235, 115, 115);
            Register_SecretAnswerTextBox.ButtonFillPressColor = Color.FromArgb(184, 64, 64);
            Register_SecretAnswerTextBox.ButtonRectColor = Color.FromArgb(230, 80, 80);
            Register_SecretAnswerTextBox.ButtonRectHoverColor = Color.FromArgb(235, 115, 115);
            Register_SecretAnswerTextBox.ButtonRectPressColor = Color.FromArgb(184, 64, 64);
            Register_SecretAnswerTextBox.Cursor = Cursors.IBeam;
            Register_SecretAnswerTextBox.FillColor = Color.FromArgb(64, 64, 64);
            Register_SecretAnswerTextBox.FillColor2 = Color.Transparent;
            Register_SecretAnswerTextBox.Font = new Font("Arial", 12F);
            Register_SecretAnswerTextBox.ForeColor = Color.DodgerBlue;
            Register_SecretAnswerTextBox.Location = new Point(120, 188);
            Register_SecretAnswerTextBox.Margin = new Padding(4, 5, 4, 5);
            Register_SecretAnswerTextBox.MinimumSize = new Size(1, 14);
            Register_SecretAnswerTextBox.Name = "Register_SecretAnswerTextBox";
            Register_SecretAnswerTextBox.PasswordChar = '*';
            Register_SecretAnswerTextBox.RectColor = Color.LightSkyBlue;
            Register_SecretAnswerTextBox.ScrollBarColor = Color.FromArgb(230, 80, 80);
            Register_SecretAnswerTextBox.ShowText = false;
            Register_SecretAnswerTextBox.Size = new Size(391, 36);
            Register_SecretAnswerTextBox.Style = UIStyle.Custom;
            Register_SecretAnswerTextBox.Symbol = 61530;
            Register_SecretAnswerTextBox.SymbolColor = Color.LightSkyBlue;
            Register_SecretAnswerTextBox.SymbolSize = 22;
            Register_SecretAnswerTextBox.TabIndex = 4;
            Register_SecretAnswerTextBox.TextAlignment = ContentAlignment.MiddleLeft;
            Register_SecretAnswerTextBox.Watermark = "Please enter a security answer";
            Register_SecretAnswerTextBox.WatermarkActiveColor = Color.DodgerBlue;
            Register_SecretAnswerTextBox.WatermarkColor = Color.LightSkyBlue;
            Register_SecretAnswerTextBox.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            // 
            // Register_PasswordTextBox
            // 
            Register_PasswordTextBox.ButtonFillColor = Color.FromArgb(230, 80, 80);
            Register_PasswordTextBox.ButtonFillHoverColor = Color.FromArgb(235, 115, 115);
            Register_PasswordTextBox.ButtonFillPressColor = Color.FromArgb(184, 64, 64);
            Register_PasswordTextBox.ButtonRectColor = Color.FromArgb(230, 80, 80);
            Register_PasswordTextBox.ButtonRectHoverColor = Color.FromArgb(235, 115, 115);
            Register_PasswordTextBox.ButtonRectPressColor = Color.FromArgb(184, 64, 64);
            Register_PasswordTextBox.Cursor = Cursors.IBeam;
            Register_PasswordTextBox.FillColor = Color.FromArgb(64, 64, 64);
            Register_PasswordTextBox.FillColor2 = Color.Transparent;
            Register_PasswordTextBox.Font = new Font("Arial", 12F);
            Register_PasswordTextBox.ForeColor = Color.DodgerBlue;
            Register_PasswordTextBox.Location = new Point(120, 106);
            Register_PasswordTextBox.Margin = new Padding(4, 5, 4, 5);
            Register_PasswordTextBox.MinimumSize = new Size(1, 14);
            Register_PasswordTextBox.Name = "Register_PasswordTextBox";
            Register_PasswordTextBox.PasswordChar = '*';
            Register_PasswordTextBox.RectColor = Color.LightSkyBlue;
            Register_PasswordTextBox.ScrollBarColor = Color.FromArgb(230, 80, 80);
            Register_PasswordTextBox.ShowText = false;
            Register_PasswordTextBox.Size = new Size(391, 36);
            Register_PasswordTextBox.Style = UIStyle.Custom;
            Register_PasswordTextBox.Symbol = 61475;
            Register_PasswordTextBox.SymbolColor = Color.LightSkyBlue;
            Register_PasswordTextBox.SymbolSize = 22;
            Register_PasswordTextBox.TabIndex = 2;
            Register_PasswordTextBox.TextAlignment = ContentAlignment.MiddleLeft;
            Register_PasswordTextBox.Watermark = "Please enter a password";
            Register_PasswordTextBox.WatermarkActiveColor = Color.DodgerBlue;
            Register_PasswordTextBox.WatermarkColor = Color.LightSkyBlue;
            Register_PasswordTextBox.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            // 
            // Register_QuestionTextBox
            // 
            Register_QuestionTextBox.ButtonFillColor = Color.FromArgb(230, 80, 80);
            Register_QuestionTextBox.ButtonFillHoverColor = Color.FromArgb(235, 115, 115);
            Register_QuestionTextBox.ButtonFillPressColor = Color.FromArgb(184, 64, 64);
            Register_QuestionTextBox.ButtonRectColor = Color.FromArgb(230, 80, 80);
            Register_QuestionTextBox.ButtonRectHoverColor = Color.FromArgb(235, 115, 115);
            Register_QuestionTextBox.ButtonRectPressColor = Color.FromArgb(184, 64, 64);
            Register_QuestionTextBox.Cursor = Cursors.IBeam;
            Register_QuestionTextBox.FillColor = Color.FromArgb(64, 64, 64);
            Register_QuestionTextBox.FillColor2 = Color.Transparent;
            Register_QuestionTextBox.Font = new Font("Arial", 12F);
            Register_QuestionTextBox.ForeColor = Color.DodgerBlue;
            Register_QuestionTextBox.Location = new Point(120, 147);
            Register_QuestionTextBox.Margin = new Padding(4, 5, 4, 5);
            Register_QuestionTextBox.MinimumSize = new Size(1, 14);
            Register_QuestionTextBox.Name = "Register_QuestionTextBox";
            Register_QuestionTextBox.RectColor = Color.LightSkyBlue;
            Register_QuestionTextBox.ScrollBarColor = Color.FromArgb(230, 80, 80);
            Register_QuestionTextBox.ShowText = false;
            Register_QuestionTextBox.Size = new Size(391, 36);
            Register_QuestionTextBox.Style = UIStyle.Custom;
            Register_QuestionTextBox.Symbol = 61530;
            Register_QuestionTextBox.SymbolColor = Color.LightSkyBlue;
            Register_QuestionTextBox.SymbolSize = 22;
            Register_QuestionTextBox.TabIndex = 3;
            Register_QuestionTextBox.TextAlignment = ContentAlignment.MiddleLeft;
            Register_QuestionTextBox.Watermark = "Please enter a security question";
            Register_QuestionTextBox.WatermarkActiveColor = Color.DodgerBlue;
            Register_QuestionTextBox.WatermarkColor = Color.LightSkyBlue;
            Register_QuestionTextBox.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            // 
            // Register_AccountNameTextBox
            // 
            Register_AccountNameTextBox.ButtonFillColor = Color.FromArgb(230, 80, 80);
            Register_AccountNameTextBox.ButtonFillHoverColor = Color.FromArgb(235, 115, 115);
            Register_AccountNameTextBox.ButtonFillPressColor = Color.FromArgb(184, 64, 64);
            Register_AccountNameTextBox.ButtonRectColor = Color.FromArgb(230, 80, 80);
            Register_AccountNameTextBox.ButtonRectHoverColor = Color.FromArgb(235, 115, 115);
            Register_AccountNameTextBox.ButtonRectPressColor = Color.FromArgb(184, 64, 64);
            Register_AccountNameTextBox.Cursor = Cursors.IBeam;
            Register_AccountNameTextBox.FillColor = Color.FromArgb(64, 64, 64);
            Register_AccountNameTextBox.FillColor2 = Color.Transparent;
            Register_AccountNameTextBox.Font = new Font("Arial", 12F);
            Register_AccountNameTextBox.ForeColor = Color.DodgerBlue;
            Register_AccountNameTextBox.Location = new Point(120, 65);
            Register_AccountNameTextBox.Margin = new Padding(4, 5, 4, 5);
            Register_AccountNameTextBox.MinimumSize = new Size(1, 14);
            Register_AccountNameTextBox.Name = "Register_AccountNameTextBox";
            Register_AccountNameTextBox.RectColor = Color.LightSkyBlue;
            Register_AccountNameTextBox.ScrollBarColor = Color.FromArgb(230, 80, 80);
            Register_AccountNameTextBox.ShowText = false;
            Register_AccountNameTextBox.Size = new Size(391, 36);
            Register_AccountNameTextBox.Style = UIStyle.Custom;
            Register_AccountNameTextBox.Symbol = 61447;
            Register_AccountNameTextBox.SymbolColor = Color.LightSkyBlue;
            Register_AccountNameTextBox.SymbolSize = 22;
            Register_AccountNameTextBox.TabIndex = 1;
            Register_AccountNameTextBox.TextAlignment = ContentAlignment.MiddleLeft;
            Register_AccountNameTextBox.Watermark = "Please enter an account name";
            Register_AccountNameTextBox.WatermarkActiveColor = Color.DodgerBlue;
            Register_AccountNameTextBox.WatermarkColor = Color.LightSkyBlue;
            Register_AccountNameTextBox.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            // 
            // ChangePasswordTab
            // 
            ChangePasswordTab.BackColor = Color.Transparent;
            ChangePasswordTab.BackgroundImage = (Image)resources.GetObject("ChangePasswordTab.BackgroundImage");
            ChangePasswordTab.BackgroundImageLayout = ImageLayout.None;
            ChangePasswordTab.Controls.Add(pictureBox4);
            ChangePasswordTab.Controls.Add(Modify_Back_To_LoginBtn);
            ChangePasswordTab.Controls.Add(Modify_ErrorLabel);
            ChangePasswordTab.Controls.Add(Modify_PasswordBtn);
            ChangePasswordTab.Controls.Add(Modify_AnswerTextBox);
            ChangePasswordTab.Controls.Add(Modify_PasswordTextBox);
            ChangePasswordTab.Controls.Add(Modify_QuestionTextBox);
            ChangePasswordTab.Controls.Add(Modify_AccountNameTextBox);
            ChangePasswordTab.Location = new Point(0, 15);
            ChangePasswordTab.Margin = new Padding(0);
            ChangePasswordTab.Name = "ChangePasswordTab";
            ChangePasswordTab.Size = new Size(200, 85);
            ChangePasswordTab.TabIndex = 2;
            ChangePasswordTab.Text = "Change Password";
            // 
            // pictureBox4
            // 
            pictureBox4.Location = new Point(553, 23);
            pictureBox4.Margin = new Padding(0);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(22, 22);
            pictureBox4.TabIndex = 25;
            pictureBox4.TabStop = false;
            pictureBox4.Click += pictureBox4_Click;
            // 
            // Modify_Back_To_LoginBtn
            // 
            Modify_Back_To_LoginBtn.BackgroundImage = (Image)resources.GetObject("Modify_Back_To_LoginBtn.BackgroundImage");
            Modify_Back_To_LoginBtn.Cursor = Cursors.Hand;
            Modify_Back_To_LoginBtn.FillColor = Color.Transparent;
            Modify_Back_To_LoginBtn.FillColor2 = Color.Transparent;
            Modify_Back_To_LoginBtn.FillDisableColor = Color.Transparent;
            Modify_Back_To_LoginBtn.FillHoverColor = Color.Transparent;
            Modify_Back_To_LoginBtn.FillPressColor = Color.Transparent;
            Modify_Back_To_LoginBtn.FillSelectedColor = Color.Transparent;
            Modify_Back_To_LoginBtn.Font = new Font("Arial", 12F);
            Modify_Back_To_LoginBtn.ForeColor = Color.LightSkyBlue;
            Modify_Back_To_LoginBtn.Location = new Point(120, 330);
            Modify_Back_To_LoginBtn.Margin = new Padding(4, 2, 4, 2);
            Modify_Back_To_LoginBtn.MinimumSize = new Size(1, 1);
            Modify_Back_To_LoginBtn.Name = "Modify_Back_To_LoginBtn";
            Modify_Back_To_LoginBtn.RectColor = Color.Transparent;
            Modify_Back_To_LoginBtn.RectHoverColor = Color.LightSkyBlue;
            Modify_Back_To_LoginBtn.RectPressColor = Color.DodgerBlue;
            Modify_Back_To_LoginBtn.RectSelectedColor = Color.FromArgb(184, 64, 64);
            Modify_Back_To_LoginBtn.Size = new Size(391, 36);
            Modify_Back_To_LoginBtn.Style = UIStyle.Custom;
            Modify_Back_To_LoginBtn.Symbol = 0;
            Modify_Back_To_LoginBtn.TabIndex = 24;
            Modify_Back_To_LoginBtn.TabStop = false;
            Modify_Back_To_LoginBtn.Text = "Return To Login";
            Modify_Back_To_LoginBtn.TipsColor = Color.FromArgb(128, 255, 128);
            Modify_Back_To_LoginBtn.TipsFont = new Font("Arial", 9F);
            Modify_Back_To_LoginBtn.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            Modify_Back_To_LoginBtn.Click += Modify_BackToLogin_Click;
            // 
            // Modify_ErrorLabel
            // 
            Modify_ErrorLabel.Font = new Font("Arial", 9F);
            Modify_ErrorLabel.ForeColor = Color.DodgerBlue;
            Modify_ErrorLabel.Location = new Point(120, 246);
            Modify_ErrorLabel.Margin = new Padding(4, 0, 4, 0);
            Modify_ErrorLabel.Name = "Modify_ErrorLabel";
            Modify_ErrorLabel.Size = new Size(391, 20);
            Modify_ErrorLabel.Style = UIStyle.Custom;
            Modify_ErrorLabel.TabIndex = 22;
            Modify_ErrorLabel.Text = "Error message";
            Modify_ErrorLabel.TextAlign = ContentAlignment.MiddleCenter;
            Modify_ErrorLabel.Visible = false;
            Modify_ErrorLabel.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            // 
            // Modify_PasswordBtn
            // 
            Modify_PasswordBtn.BackgroundImage = (Image)resources.GetObject("Modify_PasswordBtn.BackgroundImage");
            Modify_PasswordBtn.Cursor = Cursors.Hand;
            Modify_PasswordBtn.FillColor = Color.Transparent;
            Modify_PasswordBtn.FillColor2 = Color.Transparent;
            Modify_PasswordBtn.FillDisableColor = Color.Transparent;
            Modify_PasswordBtn.FillHoverColor = Color.Transparent;
            Modify_PasswordBtn.FillPressColor = Color.Transparent;
            Modify_PasswordBtn.FillSelectedColor = Color.Transparent;
            Modify_PasswordBtn.Font = new Font("Arial", 12F);
            Modify_PasswordBtn.ForeColor = Color.LightSkyBlue;
            Modify_PasswordBtn.Location = new Point(120, 290);
            Modify_PasswordBtn.Margin = new Padding(4, 2, 4, 2);
            Modify_PasswordBtn.MinimumSize = new Size(1, 1);
            Modify_PasswordBtn.Name = "Modify_PasswordBtn";
            Modify_PasswordBtn.RectColor = Color.Transparent;
            Modify_PasswordBtn.RectHoverColor = Color.LightSkyBlue;
            Modify_PasswordBtn.RectPressColor = Color.DodgerBlue;
            Modify_PasswordBtn.RectSelectedColor = Color.FromArgb(184, 64, 64);
            Modify_PasswordBtn.Size = new Size(391, 36);
            Modify_PasswordBtn.Style = UIStyle.Custom;
            Modify_PasswordBtn.Symbol = 0;
            Modify_PasswordBtn.TabIndex = 21;
            Modify_PasswordBtn.TabStop = false;
            Modify_PasswordBtn.Text = "Change Password";
            Modify_PasswordBtn.TipsColor = Color.FromArgb(128, 255, 128);
            Modify_PasswordBtn.TipsFont = new Font("Arial", 9F);
            Modify_PasswordBtn.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            Modify_PasswordBtn.Click += Modify_ChangePassword_Click;
            // 
            // Modify_AnswerTextBox
            // 
            Modify_AnswerTextBox.ButtonFillColor = Color.FromArgb(230, 80, 80);
            Modify_AnswerTextBox.ButtonFillHoverColor = Color.FromArgb(235, 115, 115);
            Modify_AnswerTextBox.ButtonFillPressColor = Color.FromArgb(184, 64, 64);
            Modify_AnswerTextBox.ButtonRectColor = Color.FromArgb(230, 80, 80);
            Modify_AnswerTextBox.ButtonRectHoverColor = Color.FromArgb(235, 115, 115);
            Modify_AnswerTextBox.ButtonRectPressColor = Color.FromArgb(184, 64, 64);
            Modify_AnswerTextBox.Cursor = Cursors.IBeam;
            Modify_AnswerTextBox.FillColor = Color.FromArgb(64, 64, 64);
            Modify_AnswerTextBox.FillColor2 = Color.Transparent;
            Modify_AnswerTextBox.Font = new Font("Arial", 12F);
            Modify_AnswerTextBox.ForeColor = Color.DodgerBlue;
            Modify_AnswerTextBox.Location = new Point(120, 205);
            Modify_AnswerTextBox.Margin = new Padding(4, 5, 4, 5);
            Modify_AnswerTextBox.MinimumSize = new Size(1, 14);
            Modify_AnswerTextBox.Name = "Modify_AnswerTextBox";
            Modify_AnswerTextBox.PasswordChar = '*';
            Modify_AnswerTextBox.RectColor = Color.LightSkyBlue;
            Modify_AnswerTextBox.ScrollBarColor = Color.FromArgb(230, 80, 80);
            Modify_AnswerTextBox.ShowText = false;
            Modify_AnswerTextBox.Size = new Size(391, 36);
            Modify_AnswerTextBox.Style = UIStyle.Custom;
            Modify_AnswerTextBox.Symbol = 61530;
            Modify_AnswerTextBox.SymbolColor = Color.LightSkyBlue;
            Modify_AnswerTextBox.SymbolSize = 22;
            Modify_AnswerTextBox.TabIndex = 20;
            Modify_AnswerTextBox.TextAlignment = ContentAlignment.MiddleLeft;
            Modify_AnswerTextBox.Watermark = "Please Enter your Security Answer";
            Modify_AnswerTextBox.WatermarkActiveColor = Color.DodgerBlue;
            Modify_AnswerTextBox.WatermarkColor = Color.LightSkyBlue;
            Modify_AnswerTextBox.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            // 
            // Modify_PasswordTextBox
            // 
            Modify_PasswordTextBox.ButtonFillColor = Color.FromArgb(230, 80, 80);
            Modify_PasswordTextBox.ButtonFillHoverColor = Color.FromArgb(235, 115, 115);
            Modify_PasswordTextBox.ButtonFillPressColor = Color.FromArgb(184, 64, 64);
            Modify_PasswordTextBox.ButtonRectColor = Color.FromArgb(230, 80, 80);
            Modify_PasswordTextBox.ButtonRectHoverColor = Color.FromArgb(235, 115, 115);
            Modify_PasswordTextBox.ButtonRectPressColor = Color.FromArgb(184, 64, 64);
            Modify_PasswordTextBox.Cursor = Cursors.IBeam;
            Modify_PasswordTextBox.FillColor = Color.FromArgb(64, 64, 64);
            Modify_PasswordTextBox.FillColor2 = Color.Transparent;
            Modify_PasswordTextBox.Font = new Font("Arial", 12F);
            Modify_PasswordTextBox.ForeColor = Color.DodgerBlue;
            Modify_PasswordTextBox.Location = new Point(120, 113);
            Modify_PasswordTextBox.Margin = new Padding(4, 5, 4, 5);
            Modify_PasswordTextBox.MinimumSize = new Size(1, 14);
            Modify_PasswordTextBox.Name = "Modify_PasswordTextBox";
            Modify_PasswordTextBox.PasswordChar = '*';
            Modify_PasswordTextBox.RectColor = Color.LightSkyBlue;
            Modify_PasswordTextBox.ScrollBarColor = Color.FromArgb(230, 80, 80);
            Modify_PasswordTextBox.ShowText = false;
            Modify_PasswordTextBox.Size = new Size(391, 36);
            Modify_PasswordTextBox.Style = UIStyle.Custom;
            Modify_PasswordTextBox.Symbol = 61475;
            Modify_PasswordTextBox.SymbolColor = Color.LightSkyBlue;
            Modify_PasswordTextBox.SymbolSize = 22;
            Modify_PasswordTextBox.TabIndex = 18;
            Modify_PasswordTextBox.TextAlignment = ContentAlignment.MiddleLeft;
            Modify_PasswordTextBox.Watermark = "Please Enter a New Password";
            Modify_PasswordTextBox.WatermarkActiveColor = Color.DodgerBlue;
            Modify_PasswordTextBox.WatermarkColor = Color.LightSkyBlue;
            Modify_PasswordTextBox.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            // 
            // Modify_QuestionTextBox
            // 
            Modify_QuestionTextBox.ButtonFillColor = Color.FromArgb(230, 80, 80);
            Modify_QuestionTextBox.ButtonFillHoverColor = Color.FromArgb(235, 115, 115);
            Modify_QuestionTextBox.ButtonFillPressColor = Color.FromArgb(184, 64, 64);
            Modify_QuestionTextBox.ButtonRectColor = Color.FromArgb(230, 80, 80);
            Modify_QuestionTextBox.ButtonRectHoverColor = Color.FromArgb(235, 115, 115);
            Modify_QuestionTextBox.ButtonRectPressColor = Color.FromArgb(184, 64, 64);
            Modify_QuestionTextBox.Cursor = Cursors.IBeam;
            Modify_QuestionTextBox.FillColor = Color.FromArgb(64, 64, 64);
            Modify_QuestionTextBox.FillColor2 = Color.Transparent;
            Modify_QuestionTextBox.Font = new Font("Arial", 12F);
            Modify_QuestionTextBox.ForeColor = Color.DodgerBlue;
            Modify_QuestionTextBox.Location = new Point(120, 159);
            Modify_QuestionTextBox.Margin = new Padding(4, 5, 4, 5);
            Modify_QuestionTextBox.MinimumSize = new Size(1, 14);
            Modify_QuestionTextBox.Name = "Modify_QuestionTextBox";
            Modify_QuestionTextBox.RectColor = Color.LightSkyBlue;
            Modify_QuestionTextBox.ScrollBarColor = Color.FromArgb(230, 80, 80);
            Modify_QuestionTextBox.ShowText = false;
            Modify_QuestionTextBox.Size = new Size(391, 36);
            Modify_QuestionTextBox.Style = UIStyle.Custom;
            Modify_QuestionTextBox.Symbol = 61530;
            Modify_QuestionTextBox.SymbolColor = Color.LightSkyBlue;
            Modify_QuestionTextBox.SymbolSize = 22;
            Modify_QuestionTextBox.TabIndex = 19;
            Modify_QuestionTextBox.TextAlignment = ContentAlignment.MiddleLeft;
            Modify_QuestionTextBox.Watermark = "Please Enter your Security Question";
            Modify_QuestionTextBox.WatermarkActiveColor = Color.DodgerBlue;
            Modify_QuestionTextBox.WatermarkColor = Color.LightSkyBlue;
            Modify_QuestionTextBox.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            // 
            // Modify_AccountNameTextBox
            // 
            Modify_AccountNameTextBox.ButtonFillColor = Color.FromArgb(230, 80, 80);
            Modify_AccountNameTextBox.ButtonFillHoverColor = Color.FromArgb(235, 115, 115);
            Modify_AccountNameTextBox.ButtonFillPressColor = Color.FromArgb(184, 64, 64);
            Modify_AccountNameTextBox.ButtonRectColor = Color.FromArgb(230, 80, 80);
            Modify_AccountNameTextBox.ButtonRectHoverColor = Color.FromArgb(235, 115, 115);
            Modify_AccountNameTextBox.ButtonRectPressColor = Color.FromArgb(184, 64, 64);
            Modify_AccountNameTextBox.Cursor = Cursors.IBeam;
            Modify_AccountNameTextBox.FillColor = Color.FromArgb(64, 64, 64);
            Modify_AccountNameTextBox.FillColor2 = Color.Transparent;
            Modify_AccountNameTextBox.Font = new Font("Arial", 12F);
            Modify_AccountNameTextBox.ForeColor = Color.DodgerBlue;
            Modify_AccountNameTextBox.Location = new Point(120, 67);
            Modify_AccountNameTextBox.Margin = new Padding(4, 5, 4, 5);
            Modify_AccountNameTextBox.MinimumSize = new Size(1, 14);
            Modify_AccountNameTextBox.Name = "Modify_AccountNameTextBox";
            Modify_AccountNameTextBox.RectColor = Color.LightSkyBlue;
            Modify_AccountNameTextBox.ScrollBarColor = Color.FromArgb(230, 80, 80);
            Modify_AccountNameTextBox.ShowText = false;
            Modify_AccountNameTextBox.Size = new Size(391, 36);
            Modify_AccountNameTextBox.Style = UIStyle.Custom;
            Modify_AccountNameTextBox.Symbol = 61447;
            Modify_AccountNameTextBox.SymbolColor = Color.LightSkyBlue;
            Modify_AccountNameTextBox.SymbolSize = 22;
            Modify_AccountNameTextBox.TabIndex = 17;
            Modify_AccountNameTextBox.TextAlignment = ContentAlignment.MiddleLeft;
            Modify_AccountNameTextBox.Watermark = "Please Enter your Existing Account Name";
            Modify_AccountNameTextBox.WatermarkActiveColor = Color.DodgerBlue;
            Modify_AccountNameTextBox.WatermarkColor = Color.LightSkyBlue;
            Modify_AccountNameTextBox.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            // 
            // StartGameTab
            // 
            StartGameTab.BackColor = Color.Transparent;
            StartGameTab.BackgroundImage = (Image)resources.GetObject("StartGameTab.BackgroundImage");
            StartGameTab.BackgroundImageLayout = ImageLayout.None;
            StartGameTab.Controls.Add(uiCheckBox2);
            StartGameTab.Controls.Add(uiCheckBox1);
            StartGameTab.Controls.Add(label1);
            StartGameTab.Controls.Add(pictureBox5);
            StartGameTab.Controls.Add(activate_account);
            StartGameTab.Controls.Add(start_selected_zone);
            StartGameTab.Controls.Add(GameServerList);
            StartGameTab.Controls.Add(Launcher_enterGameBtn);
            StartGameTab.Location = new Point(0, 15);
            StartGameTab.Margin = new Padding(0);
            StartGameTab.Name = "StartGameTab";
            StartGameTab.Size = new Size(200, 85);
            StartGameTab.TabIndex = 3;
            StartGameTab.Text = "Start Game";
            // 
            // uiCheckBox2
            // 
            uiCheckBox2.Font = new Font("Microsoft YaHei", 12F);
            uiCheckBox2.ForeColor = Color.LightSkyBlue;
            uiCheckBox2.Location = new Point(399, 296);
            uiCheckBox2.MinimumSize = new Size(1, 1);
            uiCheckBox2.Name = "uiCheckBox2";
            uiCheckBox2.Padding = new Padding(22, 0, 0, 0);
            uiCheckBox2.Size = new Size(150, 29);
            uiCheckBox2.Style = UIStyle.Custom;
            uiCheckBox2.TabIndex = 29;
            uiCheckBox2.Text = "64Bit";
            uiCheckBox2.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            uiCheckBox2.CheckedChanged += uiCheckBox2_CheckedChanged;
            // 
            // uiCheckBox1
            // 
            uiCheckBox1.Font = new Font("Microsoft YaHei", 12F);
            uiCheckBox1.ForeColor = Color.LightSkyBlue;
            uiCheckBox1.Location = new Point(399, 271);
            uiCheckBox1.MinimumSize = new Size(1, 1);
            uiCheckBox1.Name = "uiCheckBox1";
            uiCheckBox1.Padding = new Padding(22, 0, 0, 0);
            uiCheckBox1.Size = new Size(150, 29);
            uiCheckBox1.Style = UIStyle.Custom;
            uiCheckBox1.TabIndex = 28;
            uiCheckBox1.Text = "32Bit";
            uiCheckBox1.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            uiCheckBox1.CheckedChanged += uiCheckBox1_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 8.25F);
            label1.ForeColor = Color.LightSkyBlue;
            label1.Location = new Point(270, 97);
            label1.Name = "label1";
            label1.Size = new Size(87, 14);
            label1.TabIndex = 27;
            label1.Text = "Server Selection";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox5
            // 
            pictureBox5.Location = new Point(553, 23);
            pictureBox5.Margin = new Padding(0);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(22, 22);
            pictureBox5.TabIndex = 26;
            pictureBox5.TabStop = false;
            pictureBox5.Click += pictureBox5_Click;
            // 
            // activate_account
            // 
            activate_account.Cursor = Cursors.Hand;
            activate_account.Enabled = false;
            activate_account.FillColor = Color.Transparent;
            activate_account.FillColor2 = Color.Transparent;
            activate_account.FillDisableColor = Color.Transparent;
            activate_account.FillHoverColor = Color.Transparent;
            activate_account.FillPressColor = Color.Transparent;
            activate_account.FillSelectedColor = Color.Transparent;
            activate_account.Font = new Font("Arial", 12F);
            activate_account.ForeColor = Color.LightSkyBlue;
            activate_account.ForeDisableColor = Color.LightSkyBlue;
            activate_account.ForeHoverColor = Color.LightSkyBlue;
            activate_account.ForePressColor = Color.LightSkyBlue;
            activate_account.ForeSelectedColor = Color.LightSkyBlue;
            activate_account.Location = new Point(153, 60);
            activate_account.Margin = new Padding(4, 2, 4, 2);
            activate_account.MinimumSize = new Size(1, 1);
            activate_account.Name = "activate_account";
            activate_account.Radius = 15;
            activate_account.RectColor = Color.Transparent;
            activate_account.RectDisableColor = Color.Transparent;
            activate_account.RectHoverColor = Color.Transparent;
            activate_account.RectPressColor = Color.Transparent;
            activate_account.RectSelectedColor = Color.Transparent;
            activate_account.Size = new Size(295, 37);
            activate_account.Style = UIStyle.Custom;
            activate_account.Symbol = 57607;
            activate_account.SymbolColor = Color.LightSkyBlue;
            activate_account.SymbolDisableColor = Color.LightSkyBlue;
            activate_account.SymbolHoverColor = Color.LightSkyBlue;
            activate_account.SymbolPressColor = Color.LightSkyBlue;
            activate_account.SymbolSelectedColor = Color.LightSkyBlue;
            activate_account.TabIndex = 9;
            activate_account.Text = "Account Name";
            activate_account.TipsFont = new Font("Arial", 9F);
            activate_account.TipsForeColor = Color.LightSkyBlue;
            activate_account.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            // 
            // start_selected_zone
            // 
            start_selected_zone.ActiveLinkColor = Color.DodgerBlue;
            start_selected_zone.Enabled = false;
            start_selected_zone.Font = new Font("Arial", 12F);
            start_selected_zone.ImageAlign = ContentAlignment.MiddleRight;
            start_selected_zone.LinkBehavior = LinkBehavior.NeverUnderline;
            start_selected_zone.LinkColor = Color.DodgerBlue;
            start_selected_zone.Location = new Point(411, 157);
            start_selected_zone.Margin = new Padding(4, 0, 4, 0);
            start_selected_zone.Name = "start_selected_zone";
            start_selected_zone.Size = new Size(138, 31);
            start_selected_zone.Style = UIStyle.Custom;
            start_selected_zone.TabIndex = 7;
            start_selected_zone.TabStop = true;
            start_selected_zone.Text = "Server";
            start_selected_zone.TextAlign = ContentAlignment.MiddleCenter;
            start_selected_zone.Visible = false;
            start_selected_zone.VisitedLinkColor = Color.DodgerBlue;
            start_selected_zone.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            // 
            // GameServerList
            // 
            GameServerList.BackColor = Color.FromArgb(64, 64, 64);
            GameServerList.DrawMode = DrawMode.OwnerDrawVariable;
            GameServerList.Font = new Font("Arial", 10.5F);
            GameServerList.ForeColor = Color.LightSkyBlue;
            GameServerList.FormattingEnabled = true;
            GameServerList.ItemHeight = 20;
            GameServerList.Items.AddRange(new object[] { "Dragon Server", "Phoenix Server" });
            GameServerList.Location = new Point(241, 113);
            GameServerList.Margin = new Padding(4, 2, 4, 2);
            GameServerList.Name = "GameServerList";
            GameServerList.Size = new Size(139, 213);
            GameServerList.TabIndex = 4;
            GameServerList.TabStop = false;
            GameServerList.DrawItem += StartupChoosegameServer_DrawItem;
            GameServerList.SelectedIndexChanged += StartupChooseGS_SelectedIndexChanged;
            // 
            // Launcher_enterGameBtn
            // 
            Launcher_enterGameBtn.BackgroundImage = (Image)resources.GetObject("Launcher_enterGameBtn.BackgroundImage");
            Launcher_enterGameBtn.Cursor = Cursors.Hand;
            Launcher_enterGameBtn.FillColor = Color.Transparent;
            Launcher_enterGameBtn.FillColor2 = Color.Transparent;
            Launcher_enterGameBtn.FillDisableColor = Color.Transparent;
            Launcher_enterGameBtn.FillHoverColor = Color.Transparent;
            Launcher_enterGameBtn.FillPressColor = Color.Transparent;
            Launcher_enterGameBtn.FillSelectedColor = Color.Transparent;
            Launcher_enterGameBtn.Font = new Font("Arial", 12F);
            Launcher_enterGameBtn.ForeColor = Color.LightSkyBlue;
            Launcher_enterGameBtn.ForeHoverColor = Color.DodgerBlue;
            Launcher_enterGameBtn.Location = new Point(120, 330);
            Launcher_enterGameBtn.Margin = new Padding(4, 2, 4, 2);
            Launcher_enterGameBtn.MinimumSize = new Size(1, 1);
            Launcher_enterGameBtn.Name = "Launcher_enterGameBtn";
            Launcher_enterGameBtn.RectColor = Color.Transparent;
            Launcher_enterGameBtn.RectHoverColor = Color.LightSkyBlue;
            Launcher_enterGameBtn.RectPressColor = Color.DodgerBlue;
            Launcher_enterGameBtn.RectSelectedColor = Color.FromArgb(204, 70, 28);
            Launcher_enterGameBtn.Size = new Size(391, 36);
            Launcher_enterGameBtn.Style = UIStyle.Custom;
            Launcher_enterGameBtn.TabIndex = 1;
            Launcher_enterGameBtn.TabStop = false;
            Launcher_enterGameBtn.Text = "Enter Game";
            Launcher_enterGameBtn.TipsFont = new Font("Arial", 9F);
            Launcher_enterGameBtn.ZoomScaleRect = new Rectangle(0, 0, 0, 0);
            Launcher_enterGameBtn.Click += Launch_EnterGame_Click;
            // 
            // InterfaceUpdateTimer
            // 
            InterfaceUpdateTimer.Interval = 30000;
            InterfaceUpdateTimer.Tick += UIUnlock;
            // 
            // DataProcessTimer
            // 
            DataProcessTimer.Enabled = true;
            DataProcessTimer.Tick += PacketProcess;
            // 
            // minimizeToTray
            // 
            minimizeToTray.ContextMenuStrip = TrayRightClickMenu;
            minimizeToTray.Icon = (Icon)resources.GetObject("minimizeToTray.Icon");
            minimizeToTray.Text = "Mir3D Launcher";
            minimizeToTray.MouseClick += TrayRestoreFromTaskBar;
            // 
            // TrayRightClickMenu
            // 
            TrayRightClickMenu.Items.AddRange(new ToolStripItem[] { OpenToolStripMenuItem, QuitToolStripMenuItem });
            TrayRightClickMenu.Name = "TrayRightClickMenu";
            TrayRightClickMenu.Size = new Size(104, 48);
            // 
            // OpenToolStripMenuItem
            // 
            OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            OpenToolStripMenuItem.Size = new Size(103, 22);
            OpenToolStripMenuItem.Text = "Open";
            OpenToolStripMenuItem.Click += Tray_Restore;
            // 
            // QuitToolStripMenuItem
            // 
            QuitToolStripMenuItem.Name = "QuitToolStripMenuItem";
            QuitToolStripMenuItem.Size = new Size(103, 22);
            QuitToolStripMenuItem.Text = "Quit";
            QuitToolStripMenuItem.Click += TrayCloseLauncher;
            // 
            // GameProcessTimer
            // 
            GameProcessTimer.Enabled = true;
            GameProcessTimer.Tick += GameProgressCheck;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(600, 430);
            Controls.Add(MainTab);
            DoubleBuffered = true;
            ForeColor = Color.Transparent;
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 2, 4, 2);
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            TransparencyKey = Color.Black;
            FormClosing += MainForm_FormClosing;
            MainTab.ResumeLayout(false);
            AccountLoginTab.ResumeLayout(false);
            AccountLoginTab.PerformLayout();
            ((ISupportInitialize)pictureBox2).EndInit();
            ((ISupportInitialize)pictureBox1).EndInit();
            RegistrationTab.ResumeLayout(false);
            ((ISupportInitialize)pictureBox3).EndInit();
            ChangePasswordTab.ResumeLayout(false);
            ((ISupportInitialize)pictureBox4).EndInit();
            StartGameTab.ResumeLayout(false);
            StartGameTab.PerformLayout();
            ((ISupportInitialize)pictureBox5).EndInit();
            TrayRightClickMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        private IContainer components;
        private global::System.Windows.Forms.TabPage AccountLoginTab;
        private UILinkLabel ForgotPasswordLabel;
        private UISymbolButton RegisterAccountLabel;
        private UISymbolButton LoginAccountLabel;
        private UITextBox AccountPasswordTextBox;
        private UITextBox AccountTextBox;
        private global::System.Windows.Forms.TabPage RegistrationTab;
        private global::System.Windows.Forms.TabPage ChangePasswordTab;
        private global::System.Windows.Forms.TabPage StartGameTab;
        private UISymbolButton Register_AccountBtn;
        private UITextBox Register_ReferralCodeTextBox;
        private UITextBox Register_SecretAnswerTextBox;
        private UITextBox Register_PasswordTextBox;
        private UITextBox Register_QuestionTextBox;
        private UITextBox Register_AccountNameTextBox;
        private UISymbolButton Modify_PasswordBtn;
        private UITextBox Modify_AnswerTextBox;
        private UITextBox Modify_PasswordTextBox;
        private UITextBox Modify_QuestionTextBox;
        private UITextBox Modify_AccountNameTextBox;
        private UIButton Launcher_enterGameBtn;
        private UILabel RegistrationErrorLabel;
        private UILabel Modify_ErrorLabel;
        private global::System.Windows.Forms.ListBox GameServerList;
        private UISymbolButton Register_Back_To_LoginBtn;
        private UISymbolButton Modify_Back_To_LoginBtn;
        private System.Windows.Forms.Timer InterfaceUpdateTimer;
        public UITabControl MainTab;
        public UILabel LoginErrorLabel;
        private System.Windows.Forms.Timer DataProcessTimer;
        private UISymbolButton activate_account;
        private global::System.Windows.Forms.NotifyIcon minimizeToTray;
        private global::System.Windows.Forms.ContextMenuStrip TrayRightClickMenu;
        private global::System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem;
        private global::System.Windows.Forms.ToolStripMenuItem QuitToolStripMenuItem;
        private System.Windows.Forms.Timer GameProcessTimer;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
        private UILinkLabel start_selected_zone;
        private PictureBox pictureBox5;
        private Label label1;
        private UICheckBox uiCheckBox2;
        private UICheckBox uiCheckBox1;
        public UILabel ConnectionStatusLabel;
    }
}