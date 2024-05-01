using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        if (Settings.Default.DBMethod == 0)
        {
            var path = Settings.Default.GameDataPath + "\\System\\Npc\\Dialogs\\";
            if (!Directory.Exists(path))
                return;

            var array = Serializer.Deserialize<NpcDialog>(path);
            foreach (var obj in array)
                DataSheet.Add(obj.ID, obj.Content);
        }

        if (Settings.Default.DBMethod == 1)
        {
            if (!DBAgent.X.Connected)
                return;

            try
            {
                var qstr = "SELECT * FROM DialogueData";
                using (var connection = DBAgent.X.DB.GetConnection())
                {
                    using var command = DBAgent.X.DB.GetCommand(connection, qstr);

                    using var reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read() == true)
                        {
                            var obj = new NpcDialog
                            {
                                ID = reader.GetInt32("ID"),
                                Content = reader.GetString("Content"),
                            };
                            DataSheet.Add(obj.ID, obj.Content);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                SMain.AddSystemLog(err.ToString());
            }
        }

        if (Settings.Default.DBMethod == 2)
        {
            var path = Settings.Default.GameDataPath + "\\System";
            if (!Directory.Exists(path))
                return;

            using var reader = new StreamReader(path + "\\DialogueData.csv", Encoding.GetEncoding("GB18030"));
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            csvReader.Read();
            csvReader.ReadHeader();
            try
            {
                while (csvReader.Read())
                {
                    var obj = new NpcDialog
                    {
                        ID = csvReader.GetField<int>("ID"),
                        Content = csvReader.GetField<string>("Content")
                    };
                    DataSheet.Add(obj.ID, obj.Content);
                }
            }
            catch (Exception ex)
            {
                SEngine.AddSystemLog(ex.Message);
            }
        }
    }
}
