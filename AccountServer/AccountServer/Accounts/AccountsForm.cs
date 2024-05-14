using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountServer.AccountServer.Accounts
{
    public partial class AccountsForm : Form
    {
        //private bool isDataChanged = false; //Refuses to save
        public AccountsForm()
        {
            InitializeComponent();

            AccountsListView.SelectedIndexChanged += AccountsListView_SelectedIndexChanged;
            //this.FormClosing += AccountsForm_FormClosing; //Refuses to save
        }

        private void AccountsForm_Load(object sender, EventArgs e)
        {
            AccountsListView.Items.Clear();

            foreach (var accountInfo in SAccounts.Accounts.Values)
            {
                var item = new ListViewItem(accountInfo.AccountName);
                item.SubItems.Add(accountInfo.Password);
                item.SubItems.Add(accountInfo.SecurityQuestion);
                item.SubItems.Add(accountInfo.SecurityAnswer);
                item.SubItems.Add(accountInfo.CreationDate.ToString("yyyy-MM-dd HH:mm:ss"));
                item.SubItems.Add(accountInfo.PromoCode);
                item.SubItems.Add(accountInfo.ReferrerCode);
                AccountsListView.Items.Add(item);
            }

            //AccountIDBox.TextChanged += DataChanged; //Refuses to save
            //PasswordBox.TextChanged += DataChanged;
            //QuestionBox.TextChanged += DataChanged;
            //AnswerBox.TextChanged += DataChanged;
            //CreationDateBox.TextChanged += DataChanged;
            //PromoCodeBox.TextChanged += DataChanged;
            //ReferrerCodeBox.TextChanged += DataChanged;
        }
        private void DataChanged(object sender, EventArgs e) //Refuses to save
        {
            //isDataChanged = true;
        }
        private void SaveChanges() //Refuses to save
        {
            //try
            //{
            //    string filePath = SAccounts.AccountDirectory;

            //    using (StreamWriter writer = new StreamWriter(filePath))
            //    {
            //        foreach (ListViewItem item in AccountsListView.Items)
            //        {
            //            writer.WriteLine($"{item.SubItems[0].Text},{item.SubItems[1].Text},{item.SubItems[2].Text},{item.SubItems[3].Text},{item.SubItems[4].Text},{item.SubItems[5].Text},{item.SubItems[6].Text}");
            //        }
            //    }

            //    MessageBox.Show("Changes saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    isDataChanged = false;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Error saving changes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void AccountsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AccountsListView.SelectedItems.Count > 0)
            {
                var selectedItem = AccountsListView.SelectedItems[0];
                AccountIDBox.Text = selectedItem.SubItems[0].Text; // Account Name
                PasswordBox.Text = selectedItem.SubItems[1].Text; // Password
                QuestionBox.Text = selectedItem.SubItems[2].Text; // Security Question
                AnswerBox.Text = selectedItem.SubItems[3].Text; // Security Answer
                CreationDateBox.Text = selectedItem.SubItems[4].Text; // Creation Date
                PromoCodeBox.Text = selectedItem.SubItems[5].Text; // Promo Code
                ReferrerCodeBox.Text = selectedItem.SubItems[6].Text; // Referrer Code

            }
        }

        private void AccountsForm_FormClosed(object sender, FormClosedEventArgs e) //Refuses to save
        {
            //AccountIDBox.TextChanged -= DataChanged;
            //PasswordBox.TextChanged -= DataChanged;
            //QuestionBox.TextChanged -= DataChanged;
            //AnswerBox.TextChanged -= DataChanged;
            //PromoCodeBox.TextChanged -= DataChanged;
            //ReferrerCodeBox.TextChanged -= DataChanged;
        }

        private void AccountsForm_FormClosing(object sender, FormClosingEventArgs e) //Refuses to save
        {
            //if (isDataChanged)
            //{
            //    var result = MessageBox.Show("Do you want to save changes?", "Save Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            //    if (result == DialogResult.Yes)
            //    {
            //        SaveChanges();
            //    }
            //    else if (result == DialogResult.Cancel)
            //    {
            //        e.Cancel = true; // Cancel closing the form
            //    }
            //}
        }

        private void DeleteAccountButton_Click(object sender, EventArgs e)
        {
            if (AccountsListView.SelectedItems.Count > 0)
            {
                var selectedItem = AccountsListView.SelectedItems[0];
                string accountName = selectedItem.SubItems[0].Text;
                string filePath = Path.Combine(SAccounts.AccountDirectory, $"{accountName}.txt");

                var result = MessageBox.Show($"Are you sure you want to delete the account '{accountName}'?", "Delete Account", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        File.Delete(filePath);
                        MessageBox.Show($"Account '{accountName}' has been successfully deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        AccountsListView.Items.Remove(selectedItem);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting account: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an account to delete.", "Delete Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
