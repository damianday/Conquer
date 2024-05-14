namespace AccountServer.AccountServer
{
    partial class ConfigForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
            TicketPortLabel = new System.Windows.Forms.Label();
            TicketPortBox = new System.Windows.Forms.NumericUpDown();
            LocalListeningPortLabel = new System.Windows.Forms.Label();
            LocalPortBox = new System.Windows.Forms.NumericUpDown();
            ServerNameLabel = new System.Windows.Forms.Label();
            TicketIPLabel = new System.Windows.Forms.Label();
            PublicIPLabel = new System.Windows.Forms.Label();
            PublicPortLabel = new System.Windows.Forms.Label();
            PublicPortBox = new System.Windows.Forms.NumericUpDown();
            LocalIPLabel = new System.Windows.Forms.Label();
            ServerNameBox = new System.Windows.Forms.TextBox();
            TicketIPBox = new System.Windows.Forms.TextBox();
            PublicIPBox = new System.Windows.Forms.TextBox();
            LocalIPBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)TicketPortBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LocalPortBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PublicPortBox).BeginInit();
            SuspendLayout();
            // 
            // TicketPortLabel
            // 
            TicketPortLabel.AutoSize = true;
            TicketPortLabel.Location = new System.Drawing.Point(12, 82);
            TicketPortLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            TicketPortLabel.Name = "TicketPortLabel";
            TicketPortLabel.Size = new System.Drawing.Size(66, 15);
            TicketPortLabel.TabIndex = 15;
            TicketPortLabel.Text = "Ticket Port:";
            // 
            // TicketPortBox
            // 
            TicketPortBox.Location = new System.Drawing.Point(95, 80);
            TicketPortBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TicketPortBox.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            TicketPortBox.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            TicketPortBox.Name = "TicketPortBox";
            TicketPortBox.Size = new System.Drawing.Size(102, 23);
            TicketPortBox.TabIndex = 14;
            TicketPortBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TicketPortBox.Value = new decimal(new int[] { 6678, 0, 0, 0 });
            // 
            // LocalListeningPortLabel
            // 
            LocalListeningPortLabel.AutoSize = true;
            LocalListeningPortLabel.Location = new System.Drawing.Point(12, 200);
            LocalListeningPortLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LocalListeningPortLabel.Name = "LocalListeningPortLabel";
            LocalListeningPortLabel.Size = new System.Drawing.Size(63, 15);
            LocalListeningPortLabel.TabIndex = 13;
            LocalListeningPortLabel.Text = "Local Port:";
            // 
            // LocalPortBox
            // 
            LocalPortBox.Location = new System.Drawing.Point(96, 199);
            LocalPortBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            LocalPortBox.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            LocalPortBox.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            LocalPortBox.Name = "LocalPortBox";
            LocalPortBox.Size = new System.Drawing.Size(102, 23);
            LocalPortBox.TabIndex = 12;
            LocalPortBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            LocalPortBox.Value = new decimal(new int[] { 8001, 0, 0, 0 });
            // 
            // ServerNameLabel
            // 
            ServerNameLabel.AutoSize = true;
            ServerNameLabel.Location = new System.Drawing.Point(12, 25);
            ServerNameLabel.Name = "ServerNameLabel";
            ServerNameLabel.Size = new System.Drawing.Size(77, 15);
            ServerNameLabel.TabIndex = 16;
            ServerNameLabel.Text = "Server Name:";
            // 
            // TicketIPLabel
            // 
            TicketIPLabel.AutoSize = true;
            TicketIPLabel.Location = new System.Drawing.Point(12, 50);
            TicketIPLabel.Name = "TicketIPLabel";
            TicketIPLabel.Size = new System.Drawing.Size(54, 15);
            TicketIPLabel.TabIndex = 17;
            TicketIPLabel.Text = "Ticket IP:";
            // 
            // PublicIPLabel
            // 
            PublicIPLabel.AutoSize = true;
            PublicIPLabel.Location = new System.Drawing.Point(12, 112);
            PublicIPLabel.Name = "PublicIPLabel";
            PublicIPLabel.Size = new System.Drawing.Size(56, 15);
            PublicIPLabel.TabIndex = 18;
            PublicIPLabel.Text = "Public IP:";
            // 
            // PublicPortLabel
            // 
            PublicPortLabel.AutoSize = true;
            PublicPortLabel.Location = new System.Drawing.Point(12, 145);
            PublicPortLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            PublicPortLabel.Name = "PublicPortLabel";
            PublicPortLabel.Size = new System.Drawing.Size(68, 15);
            PublicPortLabel.TabIndex = 20;
            PublicPortLabel.Text = "Public Port:";
            // 
            // PublicPortBox
            // 
            PublicPortBox.Location = new System.Drawing.Point(96, 141);
            PublicPortBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PublicPortBox.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            PublicPortBox.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            PublicPortBox.Name = "PublicPortBox";
            PublicPortBox.Size = new System.Drawing.Size(102, 23);
            PublicPortBox.TabIndex = 19;
            PublicPortBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            PublicPortBox.Value = new decimal(new int[] { 8001, 0, 0, 0 });
            // 
            // LocalIPLabel
            // 
            LocalIPLabel.AutoSize = true;
            LocalIPLabel.Location = new System.Drawing.Point(12, 175);
            LocalIPLabel.Name = "LocalIPLabel";
            LocalIPLabel.Size = new System.Drawing.Size(51, 15);
            LocalIPLabel.TabIndex = 21;
            LocalIPLabel.Text = "Local IP:";
            // 
            // ServerNameBox
            // 
            ServerNameBox.Location = new System.Drawing.Point(95, 22);
            ServerNameBox.Name = "ServerNameBox";
            ServerNameBox.Size = new System.Drawing.Size(122, 23);
            ServerNameBox.TabIndex = 22;
            // 
            // TicketIPBox
            // 
            TicketIPBox.Location = new System.Drawing.Point(95, 50);
            TicketIPBox.Name = "TicketIPBox";
            TicketIPBox.Size = new System.Drawing.Size(122, 23);
            TicketIPBox.TabIndex = 23;
            // 
            // PublicIPBox
            // 
            PublicIPBox.Location = new System.Drawing.Point(95, 109);
            PublicIPBox.Name = "PublicIPBox";
            PublicIPBox.Size = new System.Drawing.Size(122, 23);
            PublicIPBox.TabIndex = 24;
            // 
            // LocalIPBox
            // 
            LocalIPBox.Location = new System.Drawing.Point(95, 170);
            LocalIPBox.Name = "LocalIPBox";
            LocalIPBox.Size = new System.Drawing.Size(122, 23);
            LocalIPBox.TabIndex = 25;
            // 
            // ConfigForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(349, 241);
            Controls.Add(LocalIPBox);
            Controls.Add(PublicIPBox);
            Controls.Add(TicketIPBox);
            Controls.Add(ServerNameBox);
            Controls.Add(LocalIPLabel);
            Controls.Add(PublicPortLabel);
            Controls.Add(PublicPortBox);
            Controls.Add(PublicIPLabel);
            Controls.Add(TicketIPLabel);
            Controls.Add(ServerNameLabel);
            Controls.Add(TicketPortLabel);
            Controls.Add(TicketPortBox);
            Controls.Add(LocalListeningPortLabel);
            Controls.Add(LocalPortBox);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "ConfigForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Config";
            ((System.ComponentModel.ISupportInitialize)TicketPortBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)LocalPortBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)PublicPortBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label TicketPortLabel;
        private System.Windows.Forms.NumericUpDown TicketPortBox;
        private System.Windows.Forms.Label LocalListeningPortLabel;
        private System.Windows.Forms.NumericUpDown LocalPortBox;
        private System.Windows.Forms.Label ServerNameLabel;
        private System.Windows.Forms.Label TicketIPLabel;
        private System.Windows.Forms.Label PublicIPLabel;
        private System.Windows.Forms.Label PublicPortLabel;
        private System.Windows.Forms.NumericUpDown PublicPortBox;
        private System.Windows.Forms.Label LocalIPLabel;
        private System.Windows.Forms.TextBox ServerNameBox;
        private System.Windows.Forms.TextBox TicketIPBox;
        private System.Windows.Forms.TextBox PublicIPBox;
        private System.Windows.Forms.TextBox LocalIPBox;
    }
}