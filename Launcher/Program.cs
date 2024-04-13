using System;
using System.Threading;
using System.Windows.Forms;

using GamePackets;

namespace Launcher
{
    internal static class Program
    {
        private static Mutex MyMutex { get; set; }

        [STAThread]
        private static void Main()
        {
            bool createdNew;

            MyMutex = new Mutex(false, "CY_Launcher_Mutex", out createdNew);
            if (createdNew || 1 == 1)
            {
                Application.EnableVisualStyles();
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.SetCompatibleTextRenderingDefault(false);

                GamePacket.Configure(typeof(Network));

                Application.Run((Form)new MainForm());
            }
            else
            {
                int num = (int)MessageBox.Show("Launcher Already Running");
                Environment.Exit(0);
            }
        }


    }
}
