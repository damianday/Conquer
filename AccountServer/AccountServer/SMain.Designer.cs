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
        LogTextBox = new RichTextBox();
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
        accountsToolStripMenuItem = new ToolStripMenuItem();
        optionsToolStripMenuItem = new ToolStripMenuItem();
        openServerConfigurationToolStripMenuItem = new ToolStripMenuItem();
        OpenUpdateConfigurationToolStripMenuItem = new ToolStripMenuItem();
        openPatchDirectoryToolStripMenuItem = new ToolStripMenuItem();
        openAccountDirectoryToolStripMenuItem = new ToolStripMenuItem();
        configToolStripMenuItem = new ToolStripMenuItem();
        statusStrip1 = new StatusStrip();
        ExistingAccountsLabel = new ToolStripStatusLabel();
        NewAccountsLabel = new ToolStripStatusLabel();
        TicketsGeneratedLabel = new ToolStripStatusLabel();
        BytesSentLabel = new ToolStripStatusLabel();
        BytesReceivedLabel = new ToolStripStatusLabel();
        GameServerLabel = new ToolStripStatusLabel();
        TrayContextMenu.SuspendLayout();
        menuStrip1.SuspendLayout();
        statusStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // LogTextBox
        // 
        LogTextBox.BackColor = Color.Gainsboro;
        LogTextBox.Location = new Point(13, 26);
        LogTextBox.Margin = new Padding(4, 2, 4, 2);
        LogTextBox.Name = "LogTextBox";
        LogTextBox.ReadOnly = true;
        LogTextBox.Size = new Size(739, 362);
        LogTextBox.TabIndex = 0;
        LogTextBox.Text = "";
        // 
        // TrayIcon
        // 
        TrayIcon.ContextMenuStrip = TrayContextMenu;
        TrayIcon.Icon = (Icon)resources.GetObject("TrayIcon.Icon");
        TrayIcon.Text = "Account Server";
        TrayIcon.MouseClick += RestoreWindow_Click;
        // 
        // TrayContextMenu
        // 
        TrayContextMenu.ImageScalingSize = new Size(20, 20);
        TrayContextMenu.Items.AddRange(new ToolStripItem[] { OpenToolStripMenuItem, QuitToolStripMenuItem });
        TrayContextMenu.Name = "TrayContextMenu";
        TrayContextMenu.Size = new Size(104, 48);
        // 
        // OpenToolStripMenuItem
        // 
        OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
        OpenToolStripMenuItem.Size = new Size(103, 22);
        OpenToolStripMenuItem.Text = "Open";
        OpenToolStripMenuItem.Click += RestoreWindowMenuItem_Click;
        // 
        // QuitToolStripMenuItem
        // 
        QuitToolStripMenuItem.Name = "QuitToolStripMenuItem";
        QuitToolStripMenuItem.Size = new Size(103, 22);
        QuitToolStripMenuItem.Text = "Quit";
        QuitToolStripMenuItem.Click += EndProcessMenuItem_Click;
        // 
        // menuStrip1
        // 
        menuStrip1.Items.AddRange(new ToolStripItem[] { serviceToolStripMenuItem, accountsToolStripMenuItem, optionsToolStripMenuItem, configToolStripMenuItem });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(765, 24);
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
        stopServiceToolStripMenuItem.Enabled = false;
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
        loadConfigurationToolStripMenuItem.Size = new Size(189, 22);
        loadConfigurationToolStripMenuItem.Text = "Configuration";
        // 
        // loadAccountsToolStripMenuItem
        // 
        loadAccountsToolStripMenuItem.Name = "loadAccountsToolStripMenuItem";
        loadAccountsToolStripMenuItem.Size = new Size(189, 22);
        loadAccountsToolStripMenuItem.Text = "Accounts";
        // 
        // LoadUpdateConfigurationToolStripMenuItem
        // 
        LoadUpdateConfigurationToolStripMenuItem.Name = "LoadUpdateConfigurationToolStripMenuItem";
        LoadUpdateConfigurationToolStripMenuItem.Size = new Size(189, 22);
        LoadUpdateConfigurationToolStripMenuItem.Text = "Update Configuration";
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
        // accountsToolStripMenuItem
        // 
        accountsToolStripMenuItem.Name = "accountsToolStripMenuItem";
        accountsToolStripMenuItem.Size = new Size(69, 20);
        accountsToolStripMenuItem.Text = "Accounts";
        accountsToolStripMenuItem.Click += accountsToolStripMenuItem_Click;
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
        // configToolStripMenuItem
        // 
        configToolStripMenuItem.Name = "configToolStripMenuItem";
        configToolStripMenuItem.Size = new Size(55, 20);
        configToolStripMenuItem.Text = "Config";
        configToolStripMenuItem.Click += configToolStripMenuItem_Click;
        // 
        // statusStrip1
        // 
        statusStrip1.Items.AddRange(new ToolStripItem[] { ExistingAccountsLabel, NewAccountsLabel, TicketsGeneratedLabel, BytesSentLabel, BytesReceivedLabel, GameServerLabel });
        statusStrip1.Location = new Point(0, 401);
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Size = new Size(765, 22);
        statusStrip1.TabIndex = 21;
        statusStrip1.Text = "statusStrip1";
        // 
        // ExistingAccountsLabel
        // 
        ExistingAccountsLabel.Name = "ExistingAccountsLabel";
        ExistingAccountsLabel.Size = new Size(69, 17);
        ExistingAccountsLabel.Text = "Accounts: 0";
        // 
        // NewAccountsLabel
        // 
        NewAccountsLabel.Name = "NewAccountsLabel";
        NewAccountsLabel.Size = new Size(96, 17);
        NewAccountsLabel.Text = "New Accounts: 0";
        // 
        // TicketsGeneratedLabel
        // 
        TicketsGeneratedLabel.Name = "TicketsGeneratedLabel";
        TicketsGeneratedLabel.Size = new Size(55, 17);
        TicketsGeneratedLabel.Text = "Tickets: 0";
        // 
        // BytesSentLabel
        // 
        BytesSentLabel.Name = "BytesSentLabel";
        BytesSentLabel.Size = new Size(73, 17);
        BytesSentLabel.Text = "Bytes Sent: 0";
        // 
        // BytesReceivedLabel
        // 
        BytesReceivedLabel.Name = "BytesReceivedLabel";
        BytesReceivedLabel.Size = new Size(97, 17);
        BytesReceivedLabel.Text = "Bytes Received: 0";
        // 
        // GameServerLabel
        // 
        GameServerLabel.Name = "GameServerLabel";
        GameServerLabel.Size = new Size(76, 17);
        GameServerLabel.Text = "Game Server:";
        // 
        // SMain
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(765, 423);
        Controls.Add(LogTextBox);
        Controls.Add(statusStrip1);
        Controls.Add(menuStrip1);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MainMenuStrip = menuStrip1;
        Margin = new Padding(4, 2, 4, 2);
        MaximizeBox = false;
        Name = "SMain";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "AccountServer";
        FormClosing += FormClosing_Click;
        Load += SMain_Load;
        TrayContextMenu.ResumeLayout(false);
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        statusStrip1.ResumeLayout(false);
        statusStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private RichTextBox LogTextBox;
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
    private ToolStripSeparator toolStripMenuItem2;
    private ToolStripMenuItem reloadToolStripMenuItem;
    private ToolStripMenuItem loadConfigurationToolStripMenuItem;
    private ToolStripMenuItem loadAccountsToolStripMenuItem;
    private ToolStripMenuItem LoadUpdateConfigurationToolStripMenuItem;
    private StatusStrip statusStrip1;
    private ToolStripStatusLabel ExistingAccountsLabel;
    private ToolStripStatusLabel NewAccountsLabel;
    private ToolStripStatusLabel TicketsGeneratedLabel;
    private ToolStripStatusLabel BytesSentLabel;
    private ToolStripStatusLabel BytesReceivedLabel;
    private ToolStripStatusLabel GameServerLabel;
    private ToolStripMenuItem configToolStripMenuItem;
    private ToolStripMenuItem accountsToolStripMenuItem;
}