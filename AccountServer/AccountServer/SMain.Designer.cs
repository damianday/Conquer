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
        ComponentResourceManager resources = new ComponentResourceManager(typeof(SMain));
        MainTabControl = new TabControl();
        LogsTabPage = new TabPage();
        LogTextBox = new RichTextBox();
        BytesSentLabel = new Label();
        BytesReceivedLabel = new Label();
        LocalListeningPortEdit = new NumericUpDown();
        LocalListeningPortLabel = new Label();
        TicketSendingPortLabel = new Label();
        TicketSendingPortEdit = new NumericUpDown();
        TrayIcon = new NotifyIcon(components);
        TrayContextMenu = new ContextMenuStrip(components);
        OpenToolStripMenuItem = new ToolStripMenuItem();
        QuitToolStripMenuItem = new ToolStripMenuItem();
        menuStrip1 = new MenuStrip();
        serviceToolStripMenuItem = new ToolStripMenuItem();
        startServiceToolStripMenuItem = new ToolStripMenuItem();
        stopServiceToolStripMenuItem = new ToolStripMenuItem();
        toolStripMenuItem2 = new ToolStripSeparator();
        reloadToolStripMenuItem = new ToolStripMenuItem();
        loadConfigurationToolStripMenuItem = new ToolStripMenuItem();
        loadAccountsToolStripMenuItem = new ToolStripMenuItem();
        LoadUpdateConfigurationToolStripMenuItem = new ToolStripMenuItem();
        toolStripMenuItem1 = new ToolStripSeparator();
        exitToolStripMenuItem = new ToolStripMenuItem();
        optionsToolStripMenuItem = new ToolStripMenuItem();
        openServerConfigurationToolStripMenuItem = new ToolStripMenuItem();
        OpenUpdateConfigurationToolStripMenuItem = new ToolStripMenuItem();
        openPatchDirectoryToolStripMenuItem = new ToolStripMenuItem();
        openAccountDirectoryToolStripMenuItem = new ToolStripMenuItem();
        groupBox1 = new GroupBox();
        TicketsGeneratedLabel = new Label();
        NewAccountsLabel = new Label();
        ExistingAccountsLabel = new Label();
        MainTabControl.SuspendLayout();
        LogsTabPage.SuspendLayout();
        ((ISupportInitialize)LocalListeningPortEdit).BeginInit();
        ((ISupportInitialize)TicketSendingPortEdit).BeginInit();
        TrayContextMenu.SuspendLayout();
        menuStrip1.SuspendLayout();
        groupBox1.SuspendLayout();
        SuspendLayout();
        // 
        // MainTabControl
        // 
        MainTabControl.Controls.Add(LogsTabPage);
        MainTabControl.ItemSize = new Size(60, 22);
        MainTabControl.Location = new Point(12, 172);
        MainTabControl.Margin = new Padding(4, 2, 4, 2);
        MainTabControl.Name = "MainTabControl";
        MainTabControl.SelectedIndex = 0;
        MainTabControl.Size = new Size(519, 420);
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
        LogsTabPage.Size = new Size(511, 390);
        LogsTabPage.TabIndex = 0;
        LogsTabPage.Text = "Log";
        // 
        // LogTextBox
        // 
        LogTextBox.BackColor = Color.Gainsboro;
        LogTextBox.Dock = DockStyle.Fill;
        LogTextBox.Location = new Point(4, 2);
        LogTextBox.Margin = new Padding(4, 2, 4, 2);
        LogTextBox.Name = "LogTextBox";
        LogTextBox.ReadOnly = true;
        LogTextBox.Size = new Size(499, 382);
        LogTextBox.TabIndex = 0;
        LogTextBox.Text = "";
        // 
        // BytesSentLabel
        // 
        BytesSentLabel.AutoSize = true;
        BytesSentLabel.ForeColor = SystemColors.ControlText;
        BytesSentLabel.Location = new Point(274, 19);
        BytesSentLabel.Margin = new Padding(4, 0, 4, 0);
        BytesSentLabel.Name = "BytesSentLabel";
        BytesSentLabel.Size = new Size(73, 15);
        BytesSentLabel.TabIndex = 6;
        BytesSentLabel.Text = "Bytes Sent: 0";
        // 
        // BytesReceivedLabel
        // 
        BytesReceivedLabel.AutoSize = true;
        BytesReceivedLabel.ForeColor = SystemColors.ControlText;
        BytesReceivedLabel.Location = new Point(250, 34);
        BytesReceivedLabel.Margin = new Padding(4, 0, 4, 0);
        BytesReceivedLabel.Name = "BytesReceivedLabel";
        BytesReceivedLabel.Size = new Size(97, 15);
        BytesReceivedLabel.TabIndex = 7;
        BytesReceivedLabel.Text = "Bytes Received: 0";
        // 
        // LocalListeningPortEdit
        // 
        LocalListeningPortEdit.Location = new Point(429, 115);
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
        LocalListeningPortLabel.Location = new Point(358, 117);
        LocalListeningPortLabel.Margin = new Padding(4, 0, 4, 0);
        LocalListeningPortLabel.Name = "LocalListeningPortLabel";
        LocalListeningPortLabel.Size = new Size(63, 15);
        LocalListeningPortLabel.TabIndex = 9;
        LocalListeningPortLabel.Text = "Local Port:";
        // 
        // TicketSendingPortLabel
        // 
        TicketSendingPortLabel.AutoSize = true;
        TicketSendingPortLabel.Location = new Point(355, 146);
        TicketSendingPortLabel.Margin = new Padding(4, 0, 4, 0);
        TicketSendingPortLabel.Name = "TicketSendingPortLabel";
        TicketSendingPortLabel.Size = new Size(66, 15);
        TicketSendingPortLabel.TabIndex = 11;
        TicketSendingPortLabel.Text = "Ticket Port:";
        // 
        // TicketSendingPortEdit
        // 
        TicketSendingPortEdit.Location = new Point(429, 144);
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
        TrayIcon.ContextMenuStrip = TrayContextMenu;
        TrayIcon.Text = "Account Server";
        TrayIcon.MouseClick += RestoreWindow_Click;
        // 
        // TrayContextMenu
        // 
        TrayContextMenu.ImageScalingSize = new Size(20, 20);
        TrayContextMenu.Items.AddRange(new ToolStripItem[] { OpenToolStripMenuItem, QuitToolStripMenuItem });
        TrayContextMenu.Name = "TrayContextMenu";
        TrayContextMenu.Size = new Size(99, 48);
        // 
        // OpenToolStripMenuItem
        // 
        OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
        OpenToolStripMenuItem.Size = new Size(98, 22);
        OpenToolStripMenuItem.Text = "Open";
        OpenToolStripMenuItem.Click += RestoreWindowMenuItem_Click;
        // 
        // QuitToolStripMenuItem
        // 
        QuitToolStripMenuItem.Name = "QuitToolStripMenuItem";
        QuitToolStripMenuItem.Size = new Size(98, 22);
        QuitToolStripMenuItem.Text = "Quit";
        QuitToolStripMenuItem.Click += EndProcessMenuItem_Click;
        // 
        // menuStrip1
        // 
        menuStrip1.Items.AddRange(new ToolStripItem[] { serviceToolStripMenuItem, optionsToolStripMenuItem });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(543, 24);
        menuStrip1.TabIndex = 19;
        menuStrip1.Text = "menuStrip1";
        // 
        // serviceToolStripMenuItem
        // 
        serviceToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { startServiceToolStripMenuItem, stopServiceToolStripMenuItem, toolStripMenuItem2, reloadToolStripMenuItem, toolStripMenuItem1, exitToolStripMenuItem });
        serviceToolStripMenuItem.Name = "serviceToolStripMenuItem";
        serviceToolStripMenuItem.Size = new Size(56, 20);
        serviceToolStripMenuItem.Text = "&Service";
        // 
        // startServiceToolStripMenuItem
        // 
        startServiceToolStripMenuItem.Name = "startServiceToolStripMenuItem";
        startServiceToolStripMenuItem.Size = new Size(138, 22);
        startServiceToolStripMenuItem.Text = "Start Service";
        startServiceToolStripMenuItem.Click += startServiceToolStripMenuItem_Click;
        // 
        // stopServiceToolStripMenuItem
        // 
        stopServiceToolStripMenuItem.Name = "stopServiceToolStripMenuItem";
        stopServiceToolStripMenuItem.Size = new Size(138, 22);
        stopServiceToolStripMenuItem.Text = "Stop Service";
        stopServiceToolStripMenuItem.Click += stopServiceToolStripMenuItem_Click;
        // 
        // toolStripMenuItem2
        // 
        toolStripMenuItem2.Name = "toolStripMenuItem2";
        toolStripMenuItem2.Size = new Size(135, 6);
        // 
        // reloadToolStripMenuItem
        // 
        reloadToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loadConfigurationToolStripMenuItem, loadAccountsToolStripMenuItem, LoadUpdateConfigurationToolStripMenuItem });
        reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
        reloadToolStripMenuItem.Size = new Size(138, 22);
        reloadToolStripMenuItem.Text = "&Reload";
        // 
        // loadConfigurationToolStripMenuItem
        // 
        loadConfigurationToolStripMenuItem.Name = "loadConfigurationToolStripMenuItem";
        loadConfigurationToolStripMenuItem.Size = new Size(218, 22);
        loadConfigurationToolStripMenuItem.Text = "Load Configuration";
        // 
        // loadAccountsToolStripMenuItem
        // 
        loadAccountsToolStripMenuItem.Name = "loadAccountsToolStripMenuItem";
        loadAccountsToolStripMenuItem.Size = new Size(218, 22);
        loadAccountsToolStripMenuItem.Text = "Load Accounts";
        // 
        // LoadUpdateConfigurationToolStripMenuItem
        // 
        LoadUpdateConfigurationToolStripMenuItem.Name = "LoadUpdateConfigurationToolStripMenuItem";
        LoadUpdateConfigurationToolStripMenuItem.Size = new Size(218, 22);
        LoadUpdateConfigurationToolStripMenuItem.Text = "Load Update Configuration";
        // 
        // toolStripMenuItem1
        // 
        toolStripMenuItem1.Name = "toolStripMenuItem1";
        toolStripMenuItem1.Size = new Size(135, 6);
        // 
        // exitToolStripMenuItem
        // 
        exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        exitToolStripMenuItem.Size = new Size(138, 22);
        exitToolStripMenuItem.Text = "E&xit";
        exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
        // 
        // optionsToolStripMenuItem
        // 
        optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openServerConfigurationToolStripMenuItem, OpenUpdateConfigurationToolStripMenuItem, openPatchDirectoryToolStripMenuItem, openAccountDirectoryToolStripMenuItem });
        optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
        optionsToolStripMenuItem.Size = new Size(61, 20);
        optionsToolStripMenuItem.Text = "Options";
        // 
        // openServerConfigurationToolStripMenuItem
        // 
        openServerConfigurationToolStripMenuItem.Name = "openServerConfigurationToolStripMenuItem";
        openServerConfigurationToolStripMenuItem.Size = new Size(221, 22);
        openServerConfigurationToolStripMenuItem.Text = "Open Server Configuration";
        openServerConfigurationToolStripMenuItem.Click += openServerConfigurationToolStripMenuItem_Click;
        // 
        // OpenUpdateConfigurationToolStripMenuItem
        // 
        OpenUpdateConfigurationToolStripMenuItem.Name = "OpenUpdateConfigurationToolStripMenuItem";
        OpenUpdateConfigurationToolStripMenuItem.Size = new Size(221, 22);
        OpenUpdateConfigurationToolStripMenuItem.Text = "Open Update Configuration";
        OpenUpdateConfigurationToolStripMenuItem.Click += OpenUpdateConfigurationToolStripMenuItem_Click;
        // 
        // openPatchDirectoryToolStripMenuItem
        // 
        openPatchDirectoryToolStripMenuItem.Name = "openPatchDirectoryToolStripMenuItem";
        openPatchDirectoryToolStripMenuItem.Size = new Size(221, 22);
        openPatchDirectoryToolStripMenuItem.Text = "Open Patch Directory";
        openPatchDirectoryToolStripMenuItem.Click += openPatchDirectoryToolStripMenuItem_Click;
        // 
        // openAccountDirectoryToolStripMenuItem
        // 
        openAccountDirectoryToolStripMenuItem.Name = "openAccountDirectoryToolStripMenuItem";
        openAccountDirectoryToolStripMenuItem.Size = new Size(221, 22);
        openAccountDirectoryToolStripMenuItem.Text = "Open Account Directory";
        openAccountDirectoryToolStripMenuItem.Click += openAccountDirectoryToolStripMenuItem_Click;
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(TicketsGeneratedLabel);
        groupBox1.Controls.Add(NewAccountsLabel);
        groupBox1.Controls.Add(ExistingAccountsLabel);
        groupBox1.Controls.Add(BytesSentLabel);
        groupBox1.Controls.Add(BytesReceivedLabel);
        groupBox1.ForeColor = Color.Firebrick;
        groupBox1.Location = new Point(12, 27);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(519, 82);
        groupBox1.TabIndex = 20;
        groupBox1.TabStop = false;
        groupBox1.Text = "Statistics";
        // 
        // TicketsGeneratedLabel
        // 
        TicketsGeneratedLabel.AutoSize = true;
        TicketsGeneratedLabel.ForeColor = SystemColors.ControlText;
        TicketsGeneratedLabel.Location = new Point(48, 49);
        TicketsGeneratedLabel.Margin = new Padding(4, 0, 4, 0);
        TicketsGeneratedLabel.Name = "TicketsGeneratedLabel";
        TicketsGeneratedLabel.Size = new Size(55, 15);
        TicketsGeneratedLabel.TabIndex = 8;
        TicketsGeneratedLabel.Text = "Tickets: 0";
        // 
        // NewAccountsLabel
        // 
        NewAccountsLabel.AutoSize = true;
        NewAccountsLabel.ForeColor = SystemColors.ControlText;
        NewAccountsLabel.Location = new Point(7, 34);
        NewAccountsLabel.Margin = new Padding(4, 0, 4, 0);
        NewAccountsLabel.Name = "NewAccountsLabel";
        NewAccountsLabel.Size = new Size(96, 15);
        NewAccountsLabel.TabIndex = 7;
        NewAccountsLabel.Text = "New Accounts: 0";
        // 
        // ExistingAccountsLabel
        // 
        ExistingAccountsLabel.AutoSize = true;
        ExistingAccountsLabel.ForeColor = SystemColors.ControlText;
        ExistingAccountsLabel.Location = new Point(34, 19);
        ExistingAccountsLabel.Margin = new Padding(4, 0, 4, 0);
        ExistingAccountsLabel.Name = "ExistingAccountsLabel";
        ExistingAccountsLabel.Size = new Size(69, 15);
        ExistingAccountsLabel.TabIndex = 6;
        ExistingAccountsLabel.Text = "Accounts: 0";
        // 
        // SMain
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(543, 598);
        Controls.Add(groupBox1);
        Controls.Add(menuStrip1);
        Controls.Add(TicketSendingPortLabel);
        Controls.Add(TicketSendingPortEdit);
        Controls.Add(LocalListeningPortLabel);
        Controls.Add(LocalListeningPortEdit);
        Controls.Add(MainTabControl);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MainMenuStrip = menuStrip1;
        Margin = new Padding(4, 2, 4, 2);
        MaximizeBox = false;
        Name = "SMain";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "AccountServer";
        FormClosing += FormClosing_Click;
        MainTabControl.ResumeLayout(false);
        LogsTabPage.ResumeLayout(false);
        ((ISupportInitialize)LocalListeningPortEdit).EndInit();
        ((ISupportInitialize)TicketSendingPortEdit).EndInit();
        TrayContextMenu.ResumeLayout(false);
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TabControl MainTabControl;
    private TabPage LogsTabPage;
    private RichTextBox LogTextBox;
    private Label BytesSentLabel;
    private Label BytesReceivedLabel;
    private Label LocalListeningPortLabel;
    private Label TicketSendingPortLabel;
    private NumericUpDown LocalListeningPortEdit;
    private NumericUpDown TicketSendingPortEdit;
    private ContextMenuStrip TrayContextMenu;
    private ToolStripMenuItem OpenToolStripMenuItem;
    private ToolStripMenuItem QuitToolStripMenuItem;
    private NotifyIcon TrayIcon;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem serviceToolStripMenuItem;
    private ToolStripMenuItem startServiceToolStripMenuItem;
    private ToolStripMenuItem stopServiceToolStripMenuItem;
    private ToolStripSeparator toolStripMenuItem1;
    private ToolStripMenuItem exitToolStripMenuItem;
    private ToolStripMenuItem optionsToolStripMenuItem;
    private ToolStripMenuItem openServerConfigurationToolStripMenuItem;
    private ToolStripMenuItem openAccountDirectoryToolStripMenuItem;
    private ToolStripMenuItem OpenUpdateConfigurationToolStripMenuItem;
    private ToolStripMenuItem openPatchDirectoryToolStripMenuItem;
    private GroupBox groupBox1;
    private Label TicketsGeneratedLabel;
    private Label NewAccountsLabel;
    private Label ExistingAccountsLabel;
    private ToolStripSeparator toolStripMenuItem2;
    private ToolStripMenuItem reloadToolStripMenuItem;
    private ToolStripMenuItem loadConfigurationToolStripMenuItem;
    private ToolStripMenuItem loadAccountsToolStripMenuItem;
    private ToolStripMenuItem LoadUpdateConfigurationToolStripMenuItem;
}