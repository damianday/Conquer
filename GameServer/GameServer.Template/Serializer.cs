using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GameServer.Template;

public static class Serializer
{
    private static readonly JsonSerializerSettings Settings;
    private static readonly Dictionary<string, string> Replacements;

    static Serializer()
    {
        Settings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };
        Replacements = new Dictionary<string, string> { ["Assembly-CSharp"] = "GameServer" };

        var types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in types)
            if (type.IsSubclassOf(typeof(SkillTask)))
                Replacements[type.Name] = type.FullName;
    }

    public static T[] Deserialize<T>(string path)
    {
        if (Directory.Exists(path))
        {
            var queue = new ConcurrentQueue<T>();
            var files = new DirectoryInfo(path).GetFiles();
            for (var i = 0; i < files.Length; i++)
            {
                var text = File.ReadAllText(files[i].FullName);
                foreach (var item in Replacements)
                    text = text.Replace(item.Key, item.Value);

                var obj = JsonConvert.DeserializeObject<T>(text, Settings);
                if (obj != null)
                    queue.Enqueue(obj);
            }
            return queue.ToArray();
        }
        return [];
    }

    public static byte[] CompressBytes(byte[] data)
    {
        var ms = new MemoryStream();
        using (var output = new DeflaterOutputStream(ms))
        {
            output.Write(data, 0, data.Length);
            output.Close();
        }
        return ms.ToArray();
    }

    public static byte[] DecompressBytes(byte[] data)
    {
        var dest = new MemoryStream();
        using (var ms = new MemoryStream(data))
        {
            using (var inflater = new InflaterInputStream(ms))
                inflater.CopyTo(dest);
        }
        return dest.ToArray();
    }
}
