using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace AccountServer.AccountServer
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
            this.Load += ConfigForm_Load;
        }
        private void ConfigForm_Load(object sender, EventArgs e)
        {
            string serverInfoFilePath = "!ServerInfo.txt";
            string settingsFilePath = "!Settings.txt";

            try
            {
                if (File.Exists(serverInfoFilePath))
                {
                    string serverInfoJsonContent = File.ReadAllText(serverInfoFilePath);

                    JArray serverInfoArray = JArray.Parse(serverInfoJsonContent);

                    if (serverInfoArray.Count > 0)
                    {
                        JObject serverInfo = (JObject)serverInfoArray.First;

                        ServerNameBox.Text = (string)serverInfo["ServerName"];
                        TicketIPBox.Text = (string)serverInfo["TicketAddressIP"];
                        TicketPortBox.Text = ((int)serverInfo["TicketAddressPort"]).ToString();
                        PublicIPBox.Text = (string)serverInfo["PublicAddressIP"];
                        PublicPortBox.Text = ((int)serverInfo["PublicAddressPort"]).ToString();
                    }
                    else
                    {
                        MessageBox.Show("No server info found in the array.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Server info text file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (File.Exists(settingsFilePath))
                {
                    string settingsJsonContent = File.ReadAllText(settingsFilePath);

                    JObject settingsInfo = JObject.Parse(settingsJsonContent);

                    LocalIPBox.Text = (string)settingsInfo["LocalListeningIP"];
                    LocalPortBox.Text = ((int)settingsInfo["LocalListeningPort"]).ToString();

                }
                else
                {
                    MessageBox.Show("Settings text file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading info: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
