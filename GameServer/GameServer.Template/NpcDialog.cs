using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GameServer.Template;

public sealed class NpcDialog
{
    public static Dictionary<int, string> DataSheet;
    public static Dictionary<int, byte[]> DataById;

    public int ID;
    public string Content;

    public static byte[] GetBufferFromDialogID(int id)
    {
        if (DataById.TryGetValue(id, out var buffer))
            return buffer;

        if (DataSheet.TryGetValue(id, out var text))
            return DataById[id] = Encoding.UTF8.GetBytes(text + "\0");

        return new byte[0];
    }

    public static byte[] ConcatData(int id, string str)
    {
        var buffer = GetBufferFromDialogID(id);
        if (buffer.Length > 0)
            return Encoding.UTF8.GetBytes(str).Concat(buffer).ToArray();

        return new byte[0];
    }

    public static void LoadData()
    {
        DataSheet = new Dictionary<int, string>();
        DataById = new Dictionary<int, byte[]>();

        var path = Settings.Default.GameDataPath + "\\System\\Npc\\Dialogs\\";
        if (!Directory.Exists(path))
            return;

        var array = Serializer.Deserialize<NpcDialog>(path);
        foreach (var obj in array)
            DataSheet.Add(obj.ID, obj.Content);
    }
}
