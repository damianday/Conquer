using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

	public static Settings Default => defaultInstance;

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("7000")]
	public ushort LocalListeningPort
	{
		get
		{
			return (ushort)this["LocalListeningPort"];
		}
		set
		{
			this["LocalListeningPort"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("6678")]
	public ushort TicketSendingPort
	{
		get
		{
			return (ushort)this["TicketSendingPort"];
		}
		set
		{
			this["TicketSendingPort"] = value;
		}
	}
}
