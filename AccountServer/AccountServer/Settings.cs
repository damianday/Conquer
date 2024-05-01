using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

public class Settings
{
    public static Settings Default = new Settings();

    [JsonIgnore]
    public const string SettingFile = "!Settings.txt";

    public string LocalListeningIP = "127.0.0.1";
    public ushort LocalListeningPort = 7_000;
    public ushort TicketSendingPort = 6_678;

    public void Load()
    {
        if (!File.Exists(SettingFile))
            return;

        var json = File.ReadAllText(SettingFile);
        var settings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };

        Default = JsonConvert.DeserializeObject<Settings>(json, settings);
    }

    public void Save()
    {
        var settings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.None,
            Formatting = Formatting.Indented
        };

        var json = JsonConvert.SerializeObject(Default, settings);
        File.WriteAllText(SettingFile, json);
    }
}
