using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AccountServer;

public partial class SMain
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new Container();
        MainTabControl = new TabControl();
        LogsTabPage = new TabPage();
        LogTextBox = new RichTextBox();
        StartServiceButton = new Button();
        StopServiceButton = new Button();
        ExistingAccountsLabel = new Label();
        NewAccountsLabel = new Label();
        TicketsGeneratedLabel = new Label();
        BytesSentLabel = new Label();
        BytesReceivedLabel = new Label();
        LocalListeningPortEdit = new NumericUpDown();
        LocalListeningPortLabel = new Label();
        TicketSendingPortLabel = new Label();
        TicketSendingPortEdit = new NumericUpDown();
        TrayIcon = new NotifyIcon(components);
        托盘右键菜单 = new ContextMenuStrip(components);
        打开ToolStripMenuItem = new ToolStripMenuItem();
        退出ToolStripMenuItem = new ToolStripMenuItem();
        OpenConfigurationButton = new Button();
        ViewAccountButton = new Button();
        LoadConfigurationButton = new Button();
        LoadAccountButton = new Button();
        加载更新按钮 = new Button();
        打开更新按钮 = new Button();
        ViewPatchButton = new Button();
        MainTabControl.SuspendLayout();
        LogsTabPage.SuspendLayout();
        ((ISupportInitialize)LocalListeningPortEdit).BeginInit();
        ((ISupportInitialize)TicketSendingPortEdit).BeginInit();
        托盘右键菜单.SuspendLayout();
        SuspendLayout();
        // 
        // MainTabControl
        // 
        MainTabControl.Controls.Add(LogsTabPage);
        MainTabControl.ItemSize = new Size(535, 22);
        MainTabControl.Location = new Point(0, 37);
        MainTabControl.Margin = new Padding(4, 2, 4, 2);
        MainTabControl.Name = "MainTabControl";
        MainTabControl.SelectedIndex = 0;
        MainTabControl.Size = new Size(630, 623);
        MainTabControl.SizeMode = TabSizeMode.Fixed;
        MainTabControl.TabIndex = 0;
        // 
        // LogsTabPage
        // 
        LogsTabPage.BackColor = Color.FromArgb(224, 224, 224);
        LogsTabPage.BorderStyle = BorderStyle.Fixed3D;
        LogsTabPage.Controls.Add(LogTextBox);
        LogsTabPage.Location = new Point(4, 26);
        LogsTabPage.Margin = new Padding(4, 2, 4, 2);
        LogsTabPage.Name = "LogsTabPage";
        LogsTabPage.Padding = new Padding(4, 2, 4, 2);
        LogsTabPage.Size = new Size(622, 593);
        LogsTabPage.TabIndex = 0;
        LogsTabPage.Text = "Log";
        // 
        // LogTextBox
        // 
        LogTextBox.BackColor = Color.Gainsboro;
        LogTextBox.Location = new Point(0, 0);
        LogTextBox.Margin = new Padding(4, 2, 4, 2);
        LogTextBox.Name = "LogTextBox";
        LogTextBox.ReadOnly = true;
        LogTextBox.Size = new Size(618, 571);
        LogTextBox.TabIndex = 0;
        LogTextBox.Text = "";
        // 
        // StartServiceButton
        // 
        StartServiceButton.BackColor = Color.YellowGreen;
        StartServiceButton.Font = new Font("Microsoft YaHei", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 134);
        StartServiceButton.Location = new Point(631, 474);
        StartServiceButton.Margin = new Padding(4, 2, 4, 2);
        StartServiceButton.Name = "StartServiceButton";
        StartServiceButton.Size = new Size(187, 91);
        StartServiceButton.TabIndex = 1;
        StartServiceButton.Text = "Start Service";
        StartServiceButton.UseVisualStyleBackColor = false;
        StartServiceButton.Click += StartServiceButton_Click;
        // 
        // StopServiceButton
        // 
        StopServiceButton.BackColor = Color.Crimson;
        StopServiceButton.Font = new Font("Microsoft YaHei", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 134);
        StopServiceButton.Location = new Point(630, 564);
        StopServiceButton.Margin = new Padding(4, 2, 4, 2);
        StopServiceButton.Name = "StopServiceButton";
        StopServiceButton.Size = new Size(187, 91);
        StopServiceButton.TabIndex = 2;
        StopServiceButton.Text = "Stop Service";
        StopServiceButton.UseVisualStyleBackColor = false;
        StopServiceButton.Click += StopServiceButton_Click;
        // 
        // ExistingAccountsLabel
        // 
        ExistingAccountsLabel.AutoSize = true;
        ExistingAccountsLabel.Location = new Point(638, 63);
        ExistingAccountsLabel.Margin = new Padding(4, 0, 4, 0);
        ExistingAccountsLabel.Name = "ExistingAccountsLabel";
        ExistingAccountsLabel.Size = new Size(69, 15);
        ExistingAccountsLabel.TabIndex = 3;
        ExistingAccountsLabel.Text = "Accounts: 0";
        // 
        // NewAccountsLabel
        // 
        NewAccountsLabel.AutoSize = true;
        NewAccountsLabel.Location = new Point(638, 78);
        NewAccountsLabel.Margin = new Padding(4, 0, 4, 0);
        NewAccountsLabel.Name = "NewAccountsLabel";
        NewAccountsLabel.Size = new Size(96, 15);
        NewAccountsLabel.TabIndex = 4;
        NewAccountsLabel.Text = "New Accounts: 0";
        // 
        // TicketsGeneratedLabel
        // 
        TicketsGeneratedLabel.AutoSize = true;
        TicketsGeneratedLabel.Location = new Point(638, 93);
        TicketsGeneratedLabel.Margin = new Padding(4, 0, 4, 0);
        TicketsGeneratedLabel.Name = "TicketsGeneratedLabel";
        TicketsGeneratedLabel.Size = new Size(55, 15);
        TicketsGeneratedLabel.TabIndex = 5;
        TicketsGeneratedLabel.Text = "Tickets: 0";
        // 
        // BytesSentLabel
        // 
        BytesSentLabel.AutoSize = true;
        BytesSentLabel.Location = new Point(638, 108);
        BytesSentLabel.Margin = new Padding(4, 0, 4, 0);
        BytesSentLabel.Name = "BytesSentLabel";
        BytesSentLabel.Size = new Size(73, 15);
        BytesSentLabel.TabIndex = 6;
        BytesSentLabel.Text = "Bytes Sent: 0";
        // 
        // BytesReceivedLabel
        // 
        BytesReceivedLabel.AutoSize = true;
        BytesReceivedLabel.Location = new Point(638, 123);
        BytesReceivedLabel.Margin = new Padding(4, 0, 4, 0);
        BytesReceivedLabel.Name = "BytesReceivedLabel";
        BytesReceivedLabel.Size = new Size(97, 15);
        BytesReceivedLabel.TabIndex = 7;
        BytesReceivedLabel.Text = "Bytes Received: 0";
        // 
        // LocalListeningPortEdit
        // 
        LocalListeningPortEdit.Location = new Point(105, 5);
        LocalListeningPortEdit.Margin = new Padding(4, 3, 4, 3);
        LocalListeningPortEdit.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        LocalListeningPortEdit.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
        LocalListeningPortEdit.Name = "LocalListeningPortEdit";
        LocalListeningPortEdit.Size = new Size(102, 23);
        LocalListeningPortEdit.TabIndex = 8;
        LocalListeningPortEdit.TextAlign = HorizontalAlignment.Center;
        LocalListeningPortEdit.Value = new decimal(new int[] { 8001, 0, 0, 0 });
        // 
        // LocalListeningPortLabel
        // 
        LocalListeningPortLabel.AutoSize = true;
        LocalListeningPortLabel.Location = new Point(30, 7);
        LocalListeningPortLabel.Margin = new Padding(4, 0, 4, 0);
        LocalListeningPortLabel.Name = "LocalListeningPortLabel";
        LocalListeningPortLabel.Size = new Size(63, 15);
        LocalListeningPortLabel.TabIndex = 9;
        LocalListeningPortLabel.Text = "Local Port:";
        // 
        // TicketSendingPortLabel
        // 
        TicketSendingPortLabel.AutoSize = true;
        TicketSendingPortLabel.Location = new Point(247, 7);
        TicketSendingPortLabel.Margin = new Padding(4, 0, 4, 0);
        TicketSendingPortLabel.Name = "TicketSendingPortLabel";
        TicketSendingPortLabel.Size = new Size(66, 15);
        TicketSendingPortLabel.TabIndex = 11;
        TicketSendingPortLabel.Text = "Ticket Port:";
        // 
        // TicketSendingPortEdit
        // 
        TicketSendingPortEdit.Location = new Point(327, 5);
        TicketSendingPortEdit.Margin = new Padding(4, 3, 4, 3);
        TicketSendingPortEdit.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        TicketSendingPortEdit.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
        TicketSendingPortEdit.Name = "TicketSendingPortEdit";
        TicketSendingPortEdit.Size = new Size(102, 23);
        TicketSendingPortEdit.TabIndex = 10;
        TicketSendingPortEdit.TextAlign = HorizontalAlignment.Center;
        TicketSendingPortEdit.Value = new decimal(new int[] { 6678, 0, 0, 0 });
        // 
        // TrayIcon
        // 
        TrayIcon.ContextMenuStrip = 托盘右键菜单;
        TrayIcon.Text = "账号服务器";
        TrayIcon.MouseClick += 恢复窗口_Click;
        // 
        // 托盘右键菜单
        // 
        托盘右键菜单.ImageScalingSize = new Size(20, 20);
        托盘右键菜单.Items.AddRange(new ToolStripItem[] { 打开ToolStripMenuItem, 退出ToolStripMenuItem });
        托盘右键菜单.Name = "托盘右键菜单";
        托盘右键菜单.Size = new Size(99, 48);
        // 
        // 打开ToolStripMenuItem
        // 
        打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
        打开ToolStripMenuItem.Size = new Size(98, 22);
        打开ToolStripMenuItem.Text = "打开";
        打开ToolStripMenuItem.Click += 恢复窗口_Click;
        // 
        // 退出ToolStripMenuItem
        // 
        退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
        退出ToolStripMenuItem.Size = new Size(98, 22);
        退出ToolStripMenuItem.Text = "退出";
        退出ToolStripMenuItem.Click += 结束进程_Click;
        // 
        // OpenConfigurationButton
        // 
        OpenConfigurationButton.BackColor = Color.FromArgb(192, 255, 192);
        OpenConfigurationButton.Font = new Font("Microsoft YaHei", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
        OpenConfigurationButton.Location = new Point(631, 197);
        OpenConfigurationButton.Margin = new Padding(4, 2, 4, 2);
        OpenConfigurationButton.Name = "OpenConfigurationButton";
        OpenConfigurationButton.Size = new Size(187, 37);
        OpenConfigurationButton.TabIndex = 12;
        OpenConfigurationButton.Text = "Open Server Configuration";
        OpenConfigurationButton.UseVisualStyleBackColor = false;
        OpenConfigurationButton.Click += OpenConfigurationButton_Click;
        // 
        // ViewAccountButton
        // 
        ViewAccountButton.BackColor = Color.FromArgb(192, 255, 192);
        ViewAccountButton.Font = new Font("Microsoft YaHei", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
        ViewAccountButton.Location = new Point(631, 278);
        ViewAccountButton.Margin = new Padding(4, 2, 4, 2);
        ViewAccountButton.Name = "ViewAccountButton";
        ViewAccountButton.Size = new Size(187, 37);
        ViewAccountButton.TabIndex = 13;
        ViewAccountButton.Text = "Open Account Directory";
        ViewAccountButton.UseVisualStyleBackColor = false;
        ViewAccountButton.Click += ViewAccountButton_Click;
        // 
        // LoadConfigurationButton
        // 
        LoadConfigurationButton.BackColor = Color.FromArgb(192, 255, 192);
        LoadConfigurationButton.Font = new Font("Microsoft YaHei", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
        LoadConfigurationButton.Location = new Point(631, 238);
        LoadConfigurationButton.Margin = new Padding(4, 2, 4, 2);
        LoadConfigurationButton.Name = "LoadConfigurationButton";
        LoadConfigurationButton.Size = new Size(187, 37);
        LoadConfigurationButton.TabIndex = 14;
        LoadConfigurationButton.Text = "Load Configuration";
        LoadConfigurationButton.UseVisualStyleBackColor = false;
        LoadConfigurationButton.Click += LoadConfigurationButton_Click;
        // 
        // LoadAccountButton
        // 
        LoadAccountButton.BackColor = Color.FromArgb(192, 255, 192);
        LoadAccountButton.Font = new Font("Microsoft YaHei", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
        LoadAccountButton.Location = new Point(632, 317);
        LoadAccountButton.Margin = new Padding(4, 2, 4, 2);
        LoadAccountButton.Name = "LoadAccountButton";
        LoadAccountButton.Size = new Size(187, 37);
        LoadAccountButton.TabIndex = 15;
        LoadAccountButton.Text = "Load Accounts";
        LoadAccountButton.UseVisualStyleBackColor = false;
        LoadAccountButton.Click += LoadAccountButton_Click;
        // 
        // 加载更新按钮
        // 
        加载更新按钮.BackColor = Color.FromArgb(192, 255, 192);
        加载更新按钮.Font = new Font("Microsoft YaHei", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
        加载更新按钮.Location = new Point(632, 440);
        加载更新按钮.Margin = new Padding(4, 2, 4, 2);
        加载更新按钮.Name = "加载更新按钮";
        加载更新按钮.Size = new Size(187, 37);
        加载更新按钮.TabIndex = 17;
        加载更新按钮.Text = "加载更新配置";
        加载更新按钮.UseVisualStyleBackColor = false;
        加载更新按钮.Click += 加载更新按钮_Click;
        // 
        // 打开更新按钮
        // 
        打开更新按钮.BackColor = Color.FromArgb(192, 255, 192);
        打开更新按钮.Font = new Font("Microsoft YaHei", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
        打开更新按钮.Location = new Point(632, 358);
        打开更新按钮.Margin = new Padding(4, 2, 4, 2);
        打开更新按钮.Name = "打开更新按钮";
        打开更新按钮.Size = new Size(187, 37);
        打开更新按钮.TabIndex = 16;
        打开更新按钮.Text = "打开更新配置";
        打开更新按钮.UseVisualStyleBackColor = false;
        打开更新按钮.Click += 打开更新按钮_Click;
        // 
        // ViewPatchButton
        // 
        ViewPatchButton.BackColor = Color.FromArgb(192, 255, 192);
        ViewPatchButton.Font = new Font("Microsoft YaHei", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
        ViewPatchButton.Location = new Point(632, 399);
        ViewPatchButton.Margin = new Padding(4, 2, 4, 2);
        ViewPatchButton.Name = "ViewPatchButton";
        ViewPatchButton.Size = new Size(187, 37);
        ViewPatchButton.TabIndex = 18;
        ViewPatchButton.Text = "Open Patch Directory";
        ViewPatchButton.UseVisualStyleBackColor = false;
        ViewPatchButton.Click += ViewPatchButton_Click;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(833, 658);
        Controls.Add(ViewPatchButton);
        Controls.Add(加载更新按钮);
        Controls.Add(打开更新按钮);
        Controls.Add(LoadAccountButton);
        Controls.Add(LoadConfigurationButton);
        Controls.Add(ViewAccountButton);
        Controls.Add(OpenConfigurationButton);
        Controls.Add(TicketSendingPortLabel);
        Controls.Add(TicketSendingPortEdit);
        Controls.Add(LocalListeningPortLabel);
        Controls.Add(LocalListeningPortEdit);
        Controls.Add(BytesReceivedLabel);
        Controls.Add(BytesSentLabel);
        Controls.Add(TicketsGeneratedLabel);
        Controls.Add(NewAccountsLabel);
        Controls.Add(ExistingAccountsLabel);
        Controls.Add(StopServiceButton);
        Controls.Add(StartServiceButton);
        Controls.Add(MainTabControl);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Margin = new Padding(4, 2, 4, 2);
        MaximizeBox = false;
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "AccountServer";
        FormClosing += FormClosing_Click;
        MainTabControl.ResumeLayout(false);
        LogsTabPage.ResumeLayout(false);
        ((ISupportInitialize)LocalListeningPortEdit).EndInit();
        ((ISupportInitialize)TicketSendingPortEdit).EndInit();
        托盘右键菜单.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TabControl MainTabControl;
    private TabPage LogsTabPage;
    private RichTextBox LogTextBox;
    private Label ExistingAccountsLabel;
    private Label NewAccountsLabel;
    private Label TicketsGeneratedLabel;
    private Label BytesSentLabel;
    private Label BytesReceivedLabel;
    private Label LocalListeningPortLabel;
    private Label TicketSendingPortLabel;
    private Button StartServiceButton;
    private Button StopServiceButton;
    private Button OpenConfigurationButton;
    private Button ViewAccountButton;
    private Button LoadConfigurationButton;
    private Button LoadAccountButton;
    private NumericUpDown LocalListeningPortEdit;
    private NumericUpDown TicketSendingPortEdit;
    private ContextMenuStrip 托盘右键菜单;
    private ToolStripMenuItem 打开ToolStripMenuItem;
    private ToolStripMenuItem 退出ToolStripMenuItem;
    private NotifyIcon TrayIcon;
}