using System.IO;
using System.Collections.Generic;

public sealed class MsgFilter
{
    private List<string> mAbuseList;

    public MsgFilter()
    {
        mAbuseList = new List<string>();
    }

    public bool Load(string path)
    {
        Clear();

        if (File.Exists(path))
        {
            try
            {
                var lines = File.ReadAllLines(path);
                Add(lines);
                return true;
            }
            catch
            {
            }
        }

        return false;
    }

    public void Add(IEnumerable<string> collection)
    {
        mAbuseList.AddRange(collection);
    }

    public void Clear()
    {
        mAbuseList.Clear();
    }

    public bool IsAbusive(string message)
    {
        foreach (var filter in mAbuseList)
        {
            if (message.Contains(filter))
                return true;
        }
        return false;
    }

    public void Filter(ref string message)
    {
        foreach (var filter in mAbuseList)
        {
            if (message.Contains(filter))
            {
                var replace = new string('*', filter.Length);
                message = message.Replace(filter, replace);
            }
        }
    }
}