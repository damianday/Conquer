using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using GameServer.Networking;

using GamePackets;

namespace GameServer;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        try
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            GamePacket.Configure(typeof(SConnection));

            Application.Run(new SMain());
        }
        catch (Exception ex)
        {
            var n = 0;
        }
    }

    static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
        var msg = e.Exception.Message;
    }

    static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var msg = (e.ExceptionObject as Exception).Message;
    }
}
