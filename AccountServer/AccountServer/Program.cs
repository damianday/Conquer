using System;
using System.Windows.Forms;

using AccountServer.Networking;

using GamePackets;

namespace AccountServer;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);

        GamePacket.Configure(typeof(SConnection));

        Application.Run(new SMain());
	}
}
