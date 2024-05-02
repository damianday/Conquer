using Newtonsoft.Json;
using System;
using System.IO;

public class Settings
{
    public static Settings Default = new Settings();

    private static readonly JsonSerializerSettings JsonSettings;

    static Settings()
    {
        JsonSettings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };
    }

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
        Default = JsonConvert.DeserializeObject<Settings>(json, JsonSettings);
    }

    public void Save()
    {
        var json = JsonConvert.SerializeObject(Default, JsonSettings);
        File.WriteAllText(SettingFile, json);
    }
}
