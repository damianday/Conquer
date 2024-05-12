using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace GameServer.Template;

public sealed class Administrator
{
    public static Dictionary<string, Administrator> DataSheet = new Dictionary<string, Administrator>();

    public UserDegree Degree;
    public string UserName;

    public static void LoadData()
    {
        DataSheet.Clear();

        var path = Settings.Default.GameDataPath + "\\System";
        if (!Directory.Exists(path))
            return;

        var reader = new StreamReader(path + "\\Admins.csv", Encoding.UTF8);
        var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
        csvReader.Read();
        csvReader.ReadHeader();
        try
        {
            while (csvReader.Read())
            {
                var obj = new Administrator();
                obj.Degree = (UserDegree)csvReader.GetField<byte>("Degree");
                obj.UserName = csvReader.GetField<string>("UserName");

                DataSheet.Add(obj.UserName, obj);
            }
        }
        catch (Exception ex)
        {
            SEngine.AddSystemLog(ex.Message);
        }
    }
}
