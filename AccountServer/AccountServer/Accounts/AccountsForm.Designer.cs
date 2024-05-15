namespace AccountServer.AccountServer.Accounts
{
    partial class AccountsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountsForm));
            AccountsListView = new System.Windows.Forms.ListView();
            NameHeader = new System.Windows.Forms.ColumnHeader();
            PasswordHeader = new System.Windows.Forms.ColumnHeader();
            QuestionHeader = new System.Windows.Forms.ColumnHeader();
            AnswerHeader = new System.Windows.Forms.ColumnHeader();
            DateHeader = new System.Windows.Forms.ColumnHeader();
            PromoHeader = new System.Windows.Forms.ColumnHeader();
            ReferrerHeader = new System.Windows.Forms.ColumnHeader();
            AccountIDLabel = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            ArchiveAccountButton = new System.Windows.Forms.Button();
            DeleteAccountButton = new System.Windows.Forms.Button();
            CreationDateBox = new System.Windows.Forms.TextBox();
            ReferrerCodeBox = new System.Windows.Forms.TextBox();
            PromoCodeBox = new System.Windows.Forms.TextBox();
            AnswerBox = new System.Windows.Forms.TextBox();
            QuestionBox = new System.Windows.Forms.TextBox();
            PasswordBox = new System.Windows.Forms.TextBox();
            AccountIDBox = new System.Windows.Forms.TextBox();
            CreationDateLabel = new System.Windows.Forms.Label();
            ReferrerCodeLabel = new System.Windows.Forms.Label();
            PromoCodeLabel = new System.Windows.Forms.Label();
            AnswerLabel = new System.Windows.Forms.Label();
            QuestionLabel = new System.Windows.Forms.Label();
            PasswordLabel = new System.Windows.Forms.Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // AccountsListView
            // 
            AccountsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { NameHeader, PasswordHeader, QuestionHeader, AnswerHeader, DateHeader, PromoHeader, ReferrerHeader });
            AccountsListView.Dock = System.Windows.Forms.DockStyle.Top;
            AccountsListView.FullRowSelect = true;
            AccountsListView.GridLines = true;
            AccountsListView.Location = new System.Drawing.Point(0, 0);
            AccountsListView.Name = "AccountsListView";
            AccountsListView.Size = new System.Drawing.Size(749, 361);
            AccountsListView.TabIndex = 1;
            AccountsListView.UseCompatibleStateImageBehavior = false;
            AccountsListView.View = System.Windows.Forms.View.Details;
            AccountsListView.SelectedIndexChanged += AccountsListView_SelectedIndexChanged;
            // 
            // NameHeader
            // 
            NameHeader.Text = "Account Name";
            NameHeader.Width = 100;
            // 
            // PasswordHeader
            // 
            PasswordHeader.Text = "Password";
            PasswordHeader.Width = 120;
            // 
            // QuestionHeader
            // 
            QuestionHeader.Text = "Question";
            QuestionHeader.Width = 100;
            // 
            // AnswerHeader
            // 
            AnswerHeader.Text = "Answer";
            AnswerHeader.Width = 100;
            // 
            // DateHeader
            // 
            DateHeader.Text = "Creation Date";
            DateHeader.Width = 140;
            // 
            // PromoHeader
            // 
            PromoHeader.Text = "PromoCode";
            PromoHeader.Width = 80;
            // 
            // ReferrerHeader
            // 
            ReferrerHeader.Text = "ReferrerCode";
            ReferrerHeader.Width = 90;
            // 
            // AccountIDLabel
            // 
            AccountIDLabel.AutoSize = true;
            AccountIDLabel.Location = new System.Drawing.Point(25, 25);
            AccountIDLabel.Name = "AccountIDLabel";
            AccountIDLabel.Size = new System.Drawing.Size(69, 15);
            AccountIDLabel.TabIndex = 2;
            AccountIDLabel.Text = "Account ID:";
            // 
            // panel1
            // 
            panel1.Controls.Add(ArchiveAccountButton);
            panel1.Controls.Add(DeleteAccountButton);
            panel1.Controls.Add(CreationDateBox);
            panel1.Controls.Add(ReferrerCodeBox);
            panel1.Controls.Add(PromoCodeBox);
            panel1.Controls.Add(AnswerBox);
            panel1.Controls.Add(QuestionBox);
            panel1.Controls.Add(PasswordBox);
            panel1.Controls.Add(AccountIDBox);
            panel1.Controls.Add(CreationDateLabel);
            panel1.Controls.Add(ReferrerCodeLabel);
            panel1.Controls.Add(PromoCodeLabel);
            panel1.Controls.Add(AnswerLabel);
            panel1.Controls.Add(QuestionLabel);
            panel1.Controls.Add(PasswordLabel);
            panel1.Controls.Add(AccountIDLabel);
            panel1.Location = new System.Drawing.Point(12, 379);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(725, 236);
            panel1.TabIndex = 3;
            // 
            // ArchiveAccountButton
            // 
            ArchiveAccountButton.Location = new System.Drawing.Point(282, 53);
            ArchiveAccountButton.Name = "ArchiveAccountButton";
            ArchiveAccountButton.Size = new System.Drawing.Size(113, 23);
            ArchiveAccountButton.TabIndex = 17;
            ArchiveAccountButton.Text = "Archive Account";
            ArchiveAccountButton.UseVisualStyleBackColor = true;
            ArchiveAccountButton.Click += ArchiveAccountButton_Click;
            // 
            // DeleteAccountButton
            // 
            DeleteAccountButton.Location = new System.Drawing.Point(282, 22);
            DeleteAccountButton.Name = "DeleteAccountButton";
            DeleteAccountButton.Size = new System.Drawing.Size(113, 23);
            DeleteAccountButton.TabIndex = 16;
            DeleteAccountButton.Text = "Delete Account";
            DeleteAccountButton.UseVisualStyleBackColor = true;
            DeleteAccountButton.Click += DeleteAccountButton_Click;
            // 
            // CreationDateBox
            // 
            CreationDateBox.Location = new System.Drawing.Point(113, 194);
            CreationDateBox.Name = "CreationDateBox";
            CreationDateBox.ReadOnly = true;
            CreationDateBox.Size = new System.Drawing.Size(117, 23);
            CreationDateBox.TabIndex = 15;
            // 
            // ReferrerCodeBox
            // 
            ReferrerCodeBox.Location = new System.Drawing.Point(113, 165);
            ReferrerCodeBox.Name = "ReferrerCodeBox";
            ReferrerCodeBox.Size = new System.Drawing.Size(117, 23);
            ReferrerCodeBox.TabIndex = 14;
            // 
            // PromoCodeBox
            // 
            PromoCodeBox.Location = new System.Drawing.Point(113, 136);
            PromoCodeBox.Name = "PromoCodeBox";
            PromoCodeBox.Size = new System.Drawing.Size(117, 23);
            PromoCodeBox.TabIndex = 13;
            // 
            // AnswerBox
            // 
            AnswerBox.Location = new System.Drawing.Point(113, 107);
            AnswerBox.Name = "AnswerBox";
            AnswerBox.Size = new System.Drawing.Size(117, 23);
            AnswerBox.TabIndex = 12;
            // 
            // QuestionBox
            // 
            QuestionBox.Location = new System.Drawing.Point(113, 79);
            QuestionBox.Name = "QuestionBox";
            QuestionBox.Size = new System.Drawing.Size(117, 23);
            QuestionBox.TabIndex = 11;
            // 
            // PasswordBox
            // 
            PasswordBox.Location = new System.Drawing.Point(113, 50);
            PasswordBox.Name = "PasswordBox";
            PasswordBox.Size = new System.Drawing.Size(117, 23);
            PasswordBox.TabIndex = 10;
            // 
            // AccountIDBox
            // 
            AccountIDBox.Location = new System.Drawing.Point(113, 22);
            AccountIDBox.Name = "AccountIDBox";
            AccountIDBox.Size = new System.Drawing.Size(117, 23);
            AccountIDBox.TabIndex = 9;
            // 
            // CreationDateLabel
            // 
            CreationDateLabel.AutoSize = true;
            CreationDateLabel.Location = new System.Drawing.Point(25, 196);
            CreationDateLabel.Name = "CreationDateLabel";
            CreationDateLabel.Size = new System.Drawing.Size(82, 15);
            CreationDateLabel.TabIndex = 8;
            CreationDateLabel.Text = "Creation Date:";
            // 
            // ReferrerCodeLabel
            // 
            ReferrerCodeLabel.AutoSize = true;
            ReferrerCodeLabel.Location = new System.Drawing.Point(25, 170);
            ReferrerCodeLabel.Name = "ReferrerCodeLabel";
            ReferrerCodeLabel.Size = new System.Drawing.Size(82, 15);
            ReferrerCodeLabel.TabIndex = 7;
            ReferrerCodeLabel.Text = "Referrer Code:";
            // 
            // PromoCodeLabel
            // 
            PromoCodeLabel.AutoSize = true;
            PromoCodeLabel.Location = new System.Drawing.Point(25, 140);
            PromoCodeLabel.Name = "PromoCodeLabel";
            PromoCodeLabel.Size = new System.Drawing.Size(77, 15);
            PromoCodeLabel.TabIndex = 6;
            PromoCodeLabel.Text = "Promo Code:";
            // 
            // AnswerLabel
            // 
            AnswerLabel.AutoSize = true;
            AnswerLabel.Location = new System.Drawing.Point(25, 112);
            AnswerLabel.Name = "AnswerLabel";
            AnswerLabel.Size = new System.Drawing.Size(49, 15);
            AnswerLabel.TabIndex = 5;
            AnswerLabel.Text = "Answer:";
            // 
            // QuestionLabel
            // 
            QuestionLabel.AutoSize = true;
            QuestionLabel.Location = new System.Drawing.Point(25, 84);
            QuestionLabel.Name = "QuestionLabel";
            QuestionLabel.Size = new System.Drawing.Size(58, 15);
            QuestionLabel.TabIndex = 4;
            QuestionLabel.Text = "Question:";
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.Location = new System.Drawing.Point(25, 53);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new System.Drawing.Size(60, 15);
            PasswordLabel.TabIndex = 3;
            PasswordLabel.Text = "Password:";
            // 
            // AccountsForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(749, 623);
            Controls.Add(panel1);
            Controls.Add(AccountsListView);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "AccountsForm";
            Text = "Accounts";
            FormClosing += AccountsForm_FormClosing;
            FormClosed += AccountsForm_FormClosed;
            Load += AccountsForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListView AccountsListView;
        private System.Windows.Forms.ColumnHeader NameHeader;
        private System.Windows.Forms.ColumnHeader PasswordHeader;
        private System.Windows.Forms.ColumnHeader QuestionHeader;
        private System.Windows.Forms.ColumnHeader AnswerHeader;
        private System.Windows.Forms.ColumnHeader DateHeader;
        private System.Windows.Forms.ColumnHeader PromoHeader;
        private System.Windows.Forms.ColumnHeader ReferrerHeader;
        private System.Windows.Forms.Label AccountIDLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label ReferrerCodeLabel;
        private System.Windows.Forms.Label PromoCodeLabel;
        private System.Windows.Forms.Label AnswerLabel;
        private System.Windows.Forms.Label QuestionLabel;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.TextBox ReferrerCodeBox;
        private System.Windows.Forms.TextBox PromoCodeBox;
        private System.Windows.Forms.TextBox AnswerBox;
        private System.Windows.Forms.TextBox QuestionBox;
        private System.Windows.Forms.TextBox PasswordBox;
        private System.Windows.Forms.TextBox AccountIDBox;
        private System.Windows.Forms.Label CreationDateLabel;
        private System.Windows.Forms.TextBox CreationDateBox;
        private System.Windows.Forms.Button DeleteAccountButton;
        private System.Windows.Forms.Button ArchiveAccountButton;
    }
}