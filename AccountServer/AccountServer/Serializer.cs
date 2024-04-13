using System;
using System.Collections.Concurrent;
using System.IO;
using Newtonsoft.Json;

namespace AccountServer;

public static class Serializer
{
	private static readonly JsonSerializerSettings Settings;

	static Serializer()
	{
		Settings = new JsonSerializerSettings
		{
			DefaultValueHandling = DefaultValueHandling.Ignore,
			NullValueHandling = NullValueHandling.Ignore,
			TypeNameHandling = TypeNameHandling.Auto,
			Formatting = Formatting.Indented
		};
	}

	public static string Serialize(object obj)
	{
		return JsonConvert.SerializeObject(obj, Settings);
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
                var obj = JsonConvert.DeserializeObject<T>(text, Settings);
                if (obj != null)
                    queue.Enqueue(obj);
            }
            return queue.ToArray();
        }
        return Array.Empty<T>();
    }
}
