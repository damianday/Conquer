namespace Launcher
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
            ServerIPLabel = new System.Windows.Forms.Label();
            ServerPortLabel = new System.Windows.Forms.Label();
            ServerNameLabel = new System.Windows.Forms.Label();
            AccountNameLabel = new System.Windows.Forms.Label();
            ServerIPBox = new System.Windows.Forms.TextBox();
            ServerPortBox = new System.Windows.Forms.TextBox();
            AccountNameBox = new System.Windows.Forms.TextBox();
            ServerNameBox = new System.Windows.Forms.TextBox();
            SuspendLayout();
            // 
            // ServerIPLabel
            // 
            ServerIPLabel.AutoSize = true;
            ServerIPLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            ServerIPLabel.ForeColor = System.Drawing.SystemColors.Window;
            ServerIPLabel.Location = new System.Drawing.Point(12, 19);
            ServerIPLabel.Name = "ServerIPLabel";
            ServerIPLabel.Size = new System.Drawing.Size(56, 14);
            ServerIPLabel.TabIndex = 0;
            ServerIPLabel.Text = "Server IP:";
            // 
            // ServerPortLabel
            // 
            ServerPortLabel.AutoSize = true;
            ServerPortLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            ServerPortLabel.ForeColor = System.Drawing.SystemColors.Window;
            ServerPortLabel.Location = new System.Drawing.Point(12, 45);
            ServerPortLabel.Name = "ServerPortLabel";
            ServerPortLabel.Size = new System.Drawing.Size(67, 14);
            ServerPortLabel.TabIndex = 1;
            ServerPortLabel.Text = "Server Port:";
            // 
            // ServerNameLabel
            // 
            ServerNameLabel.AutoSize = true;
            ServerNameLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            ServerNameLabel.ForeColor = System.Drawing.SystemColors.Window;
            ServerNameLabel.Location = new System.Drawing.Point(12, 98);
            ServerNameLabel.Name = "ServerNameLabel";
            ServerNameLabel.Size = new System.Drawing.Size(78, 14);
            ServerNameLabel.TabIndex = 3;
            ServerNameLabel.Text = "Server Name:";
            // 
            // AccountNameLabel
            // 
            AccountNameLabel.AutoSize = true;
            AccountNameLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            AccountNameLabel.ForeColor = System.Drawing.SystemColors.Window;
            AccountNameLabel.Location = new System.Drawing.Point(12, 72);
            AccountNameLabel.Name = "AccountNameLabel";
            AccountNameLabel.Size = new System.Drawing.Size(52, 14);
            AccountNameLabel.TabIndex = 2;
            AccountNameLabel.Text = "Account:";
            // 
            // ServerIPBox
            // 
            ServerIPBox.BackColor = System.Drawing.SystemColors.WindowText;
            ServerIPBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            ServerIPBox.ForeColor = System.Drawing.SystemColors.Window;
            ServerIPBox.Location = new System.Drawing.Point(71, 19);
            ServerIPBox.Margin = new System.Windows.Forms.Padding(0);
            ServerIPBox.Name = "ServerIPBox";
            ServerIPBox.Size = new System.Drawing.Size(116, 16);
            ServerIPBox.TabIndex = 4;
            // 
            // ServerPortBox
            // 
            ServerPortBox.BackColor = System.Drawing.SystemColors.WindowText;
            ServerPortBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            ServerPortBox.ForeColor = System.Drawing.SystemColors.Window;
            ServerPortBox.Location = new System.Drawing.Point(82, 43);
            ServerPortBox.Margin = new System.Windows.Forms.Padding(0);
            ServerPortBox.Name = "ServerPortBox";
            ServerPortBox.Size = new System.Drawing.Size(65, 16);
            ServerPortBox.TabIndex = 5;
            // 
            // AccountNameBox
            // 
            AccountNameBox.BackColor = System.Drawing.SystemColors.WindowText;
            AccountNameBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            AccountNameBox.ForeColor = System.Drawing.SystemColors.Window;
            AccountNameBox.Location = new System.Drawing.Point(67, 71);
            AccountNameBox.Margin = new System.Windows.Forms.Padding(0);
            AccountNameBox.Name = "AccountNameBox";
            AccountNameBox.Size = new System.Drawing.Size(120, 16);
            AccountNameBox.TabIndex = 6;
            // 
            // ServerNameBox
            // 
            ServerNameBox.BackColor = System.Drawing.SystemColors.WindowText;
            ServerNameBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            ServerNameBox.ForeColor = System.Drawing.SystemColors.Window;
            ServerNameBox.Location = new System.Drawing.Point(93, 98);
            ServerNameBox.Margin = new System.Windows.Forms.Padding(0);
            ServerNameBox.Name = "ServerNameBox";
            ServerNameBox.Size = new System.Drawing.Size(94, 16);
            ServerNameBox.TabIndex = 7;
            // 
            // ConfigForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            ClientSize = new System.Drawing.Size(196, 130);
            Controls.Add(ServerNameBox);
            Controls.Add(AccountNameBox);
            Controls.Add(ServerPortBox);
            Controls.Add(ServerIPBox);
            Controls.Add(ServerNameLabel);
            Controls.Add(AccountNameLabel);
            Controls.Add(ServerPortLabel);
            Controls.Add(ServerIPLabel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConfigForm";
            ShowIcon = false;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label ServerIPLabel;
        private System.Windows.Forms.Label ServerPortLabel;
        private System.Windows.Forms.Label ServerNameLabel;
        private System.Windows.Forms.Label AccountNameLabel;
        private System.Windows.Forms.TextBox ServerIPBox;
        private System.Windows.Forms.TextBox ServerPortBox;
        private System.Windows.Forms.TextBox AccountNameBox;
        private System.Windows.Forms.TextBox ServerNameBox;
    }
}