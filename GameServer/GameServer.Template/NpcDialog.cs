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
        if (DataById.TryGetValue(id, out var value))
        {
            return value;
        }
        if (DataSheet.TryGetValue(id, out var value2))
        {
            return DataById[id] = Encoding.UTF8.GetBytes(value2 + "\0");
        }
        return new byte[0];
    }

    public static byte[] 合并数据(int 对话编号, string 内容)
    {
        if (DataById.TryGetValue(对话编号, out var value))
        {
            return Encoding.UTF8.GetBytes(内容).Concat(value).ToArray();
        }
        if (DataSheet.TryGetValue(对话编号, out var value2))
        {
            byte[] bytes = Encoding.UTF8.GetBytes(内容);
            byte[] array = (DataById[对话编号] = Encoding.UTF8.GetBytes(value2 + "\0"));
            return bytes.Concat(array).ToArray();
        }
        return new byte[0];
    }

    public static void LoadData()
    {
        DataSheet = new Dictionary<int, string>();
        DataById = new Dictionary<int, byte[]>();

        var path = Config.GameDataPath + "\\System\\Npc\\Dialogs\\";
        if (!Directory.Exists(path))
            return;

        var array = Serializer.Deserialize<NpcDialog>(path);
        foreach (var obj in array)
            DataSheet.Add(obj.ID, obj.Content);
    }
}
