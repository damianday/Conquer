using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Text;

namespace GameServer.Template;

public sealed class Recharge
{
    private static List<string> RechargeList;
    private static DateTime LastRechargeTime;

    public static int ReadAccount(int id, string accountName)
    {
        if (SEngine.CurrentTime >= LastRechargeTime)
        {
            Refresh();
            LastRechargeTime = SEngine.CurrentTime.AddSeconds(30.0);
        }

        if (RechargeList.Count == 0)
            return 0;

        int foundVal = 0;
        var found = false;
        for (var i = 0; i < RechargeList.Count; i++)
        {
            var line = RechargeList[i];
            if (string.IsNullOrEmpty(line)) continue;
            if (line.StartsWith(';')) continue;

            var arr = line.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (arr == null || arr.Length != 3) continue;

            // Wrong ID, skip
            if (arr[0] != id.ToString()) continue;

            if (string.Equals(arr[1], accountName, StringComparison.OrdinalIgnoreCase))
            {
                found = true; // Found user
                RechargeList.RemoveAt(i);

                if (int.TryParse(arr[2], out var number))
                    foundVal = number;
                break;
            }
        }

        if (found)
            Update();

        return foundVal;
    }

    private static void Refresh()
    {
        var path = Path.Combine(Settings.Default.平台接入目录, "RechargeList.txt");

        if (!File.Exists(path))
            return;

        var lines = File.ReadAllLines(path, Encoding.UTF8);
        RechargeList.Clear();
        RechargeList.AddRange(lines);
    }

    private static void Update()
    {
        var path = Path.Combine(Settings.Default.平台接入目录, "RechargeList.txt");

        if (!File.Exists(path))
            return;

        File.WriteAllLines(path, RechargeList, Encoding.UTF8);
    }
}
