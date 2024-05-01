using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

public class Settings
{
    [JsonIgnore]
    public const string SettingFile = "!Settings.txt";

    public static string LocalListeningIP = "127.0.0.1";
    public static ushort LocalListeningPort = 7_000;
    public static ushort TicketSendingPort = 6_678;

    public static void Load()
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

        var source = JsonConvert.DeserializeObject<JToken>(json, settings);
        var destinationProperties = typeof(Settings).GetFields(BindingFlags.Public | BindingFlags.Static);
        foreach (JProperty prop in source)
        {
            var destinationProp = destinationProperties
                .SingleOrDefault(p => p.Name.Equals(prop.Name, StringComparison.OrdinalIgnoreCase));
            if (destinationProp == null) continue;
            if (destinationProp.IsLiteral && !destinationProp.IsInitOnly) continue; // Is a const field
            var value = ((JValue)prop.Value).Value;
            //The ChangeType is required because JSON.Net will deserialise
            //numbers as long by default
            destinationProp.SetValue(null, Convert.ChangeType(value, destinationProp.FieldType));
        }
    }

    public static void Save()
    {
        var settings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.None,
            Formatting = Formatting.Indented
        };

        var myType = typeof(Settings);
        var TypeBlob = myType.GetFields()
            .Where(x => !(x.IsLiteral && !x.IsInitOnly))
            .ToDictionary(x => x.Name, x => x.GetValue(null));
        var json = JsonConvert.SerializeObject(TypeBlob, settings);
        File.WriteAllText(SettingFile, json);
    }
}
