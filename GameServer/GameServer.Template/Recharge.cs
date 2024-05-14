using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Text;

namespace GameServer.Template;

public sealed class Recharge
{
    public static int ReadAccount(int id, string accountName)
    {
        var path = Path.Combine(Settings.Default.平台接入目录, "RechargeList.txt");

        if (!File.Exists(path))
            return 0;

        int foundVal = 0;
        var list = File.ReadAllLines(path, Encoding.UTF8).ToList();
        var found = false;
        for (var i = 0; i < list.Count; i++)
        {
            var line = list[i];
            if (string.IsNullOrEmpty(line)) continue;
            if (line.StartsWith(';')) continue;

            var arr = line.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (arr == null || arr.Length != 3) continue;

            // Wrong ID, skip
            if (arr[0] != id.ToString()) continue;

            if (string.Equals(arr[1], accountName, StringComparison.OrdinalIgnoreCase))
            {
                found = true; // Found user
                list.RemoveAt(i);

                if (int.TryParse(arr[2], out var number))
                    foundVal = number;
                break;
            }
        }

        if (found)
            File.WriteAllLines(path, list, Encoding.UTF8);

        return foundVal;
    }
}
