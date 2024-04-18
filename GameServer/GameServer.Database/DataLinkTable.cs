using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace GameServer.Database;

public static class DataLinkTable
{
    private struct 数据关联参数
    {
        public DBObject 数据;
        public DBValue 字段;
        public object 监视器;
        public Type Type;
        public int Index;

        public 数据关联参数(DBObject obj, DBValue value, object 监视器, Type 数据类型, int index)
        {
            this.数据 = obj;
            this.字段 = value;
            this.监视器 = 监视器;
            this.Type = 数据类型;
            this.Index = index;
        }
    }

    private struct 列表关联参数
    {
        public DBObject 数据;
        public DBValue 字段;
        public IList 内部列表;
        public Type Type;
        public int Index;

        public 列表关联参数(DBObject obj, DBValue value, IList 内部列表, Type 数据类型, int index)
        {
            this.数据 = obj;
            this.字段 = value;
            this.内部列表 = 内部列表;
            this.Type = 数据类型;
            this.Index = index;
        }
    }

    private struct 哈希关联参数<T>
    {
        public DBObject 数据;
        public DBValue 字段;
        public ISet<T> 内部列表;
        public int Index;

        public 哈希关联参数(DBObject obj, DBValue value, ISet<T> 内部列表, int index)
        {
            this.数据 = obj;
            this.字段 = value;
            this.内部列表 = 内部列表;
            this.Index = index;
        }
    }

    private struct 字典关联参数
    {
        public DBObject 数据;
        public DBValue 字段;
        public Type 键类型;
        public Type 值类型;
        public int 键索引;
        public int 值索引;
        public object 字典键;
        public object 字典值;
        public IDictionary 内部字典;

        public 字典关联参数(DBObject obj, DBValue value, IDictionary 内部字典, object 字典键, object 字典值, Type 键类型, Type 值类型, int 键索引, int 值索引)
        {
            this.数据 = obj;
            this.字段 = value;
            this.内部字典 = 内部字典;
            this.字典键 = 字典键;
            this.字典值 = 字典值;
            this.键类型 = 键类型;
            this.值类型 = 值类型;
            this.键索引 = 键索引;
            this.值索引 = 值索引;
        }
    }

    private static readonly ConcurrentQueue<数据关联参数> 数据任务表;
    private static readonly ConcurrentQueue<列表关联参数> 列表任务表;
    private static readonly ConcurrentQueue<字典关联参数> 字典任务表;
    private static readonly ConcurrentQueue<哈希关联参数<PetInfo>> 哈希宠物表;
    private static readonly ConcurrentQueue<哈希关联参数<CharacterInfo>> 哈希角色表;
    private static readonly ConcurrentQueue<哈希关联参数<MailInfo>> 哈希邮件表;

    public static void 添加任务(DBObject 数据, DBValue 字段, object 监视器, Type 数据类型, int 数据索引)
    {
        数据任务表.Enqueue(new 数据关联参数(数据, 字段, 监视器, 数据类型, 数据索引));
    }

    public static void 添加任务(DBObject 数据, DBValue 字段, IList 内部列表, Type 数据类型, int 数据索引)
    {
        列表任务表.Enqueue(new 列表关联参数(数据, 字段, 内部列表, 数据类型, 数据索引));
    }

    public static void 添加任务<T>(DBObject 数据, DBValue 字段, ISet<T> 内部列表, int 数据索引)
    {
        if (内部列表 is ISet<PetInfo> 内部列表2)
        {
            哈希宠物表.Enqueue(new 哈希关联参数<PetInfo>(数据, 字段, 内部列表2, 数据索引));
        }
        else if (内部列表 is ISet<CharacterInfo> 内部列表3)
        {
            哈希角色表.Enqueue(new 哈希关联参数<CharacterInfo>(数据, 字段, 内部列表3, 数据索引));
        }
        else if (内部列表 is ISet<MailInfo> 内部列表4)
        {
            哈希邮件表.Enqueue(new 哈希关联参数<MailInfo>(数据, 字段, 内部列表4, 数据索引));
        }
        else
        {
            MessageBox.Show("添加哈希关联任务失败");
        }
    }

    public static void 添加任务(DBObject 数据, DBValue 字段, IDictionary 内部字典, object 字典键, object 字典值, Type 键类型, Type 值类型, int 键索引, int 值索引)
    {
        字典任务表.Enqueue(new 字典关联参数(数据, 字段, 内部字典, 字典键, 字典值, 键类型, 值类型, 键索引, 值索引));
    }

    public static void ProcessTasks()
    {
        int num = 0;
        Dictionary<Type, Dictionary<string, int>> dictionary = new Dictionary<Type, Dictionary<string, int>>();
        SMain.AddSystemLog("Start working on data correlation tasks...");
        while (!数据任务表.IsEmpty)
        {
            if (!数据任务表.TryDequeue(out var result) || result.Index == 0)
            {
                continue;
            }
            DBObject 游戏数据2 = Session.Tables[result.Type][result.Index];
            if (游戏数据2 == null)
            {
                if (!dictionary.ContainsKey(result.数据.DataType))
                {
                    dictionary[result.数据.DataType] = new Dictionary<string, int>();
                }
                if (!dictionary[result.数据.DataType].ContainsKey(result.字段.FieldName))
                {
                    dictionary[result.数据.DataType][result.字段.FieldName] = 0;
                }
                dictionary[result.数据.DataType][result.字段.FieldName]++;
            }
            else
            {
                result.监视器.GetType().GetField("m_Value", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(result.监视器, 游戏数据2);
                num++;
            }
        }
        while (!列表任务表.IsEmpty)
        {
            if (!列表任务表.TryDequeue(out var result2) || result2.Index == 0)
            {
                continue;
            }
            DBObject 游戏数据3 = Session.Tables[result2.Type][result2.Index];
            if (游戏数据3 == null)
            {
                if (!dictionary.ContainsKey(result2.数据.DataType))
                {
                    dictionary[result2.数据.DataType] = new Dictionary<string, int>();
                }
                if (!dictionary[result2.数据.DataType].ContainsKey(result2.字段.FieldName))
                {
                    dictionary[result2.数据.DataType][result2.字段.FieldName] = 0;
                }
                dictionary[result2.数据.DataType][result2.字段.FieldName]++;
            }
            else
            {
                result2.内部列表.Add(游戏数据3);
                num++;
            }
        }
        while (!字典任务表.IsEmpty)
        {
            if (!字典任务表.TryDequeue(out var result3) || (result3.字典键 == null && result3.键索引 == 0) || (result3.字典值 == null && result3.值索引 == 0))
            {
                continue;
            }
            object obj = result3.字典键 ?? Session.Tables[result3.键类型][result3.键索引];
            object obj2 = result3.字典值 ?? Session.Tables[result3.值类型][result3.值索引];
            if (obj != null && obj2 != null)
            {
                result3.内部字典[obj] = obj2;
                num++;
                continue;
            }
            if (!dictionary.ContainsKey(result3.数据.DataType))
            {
                dictionary[result3.数据.DataType] = new Dictionary<string, int>();
            }
            if (!dictionary[result3.数据.DataType].ContainsKey(result3.字段.FieldName))
            {
                dictionary[result3.数据.DataType][result3.字段.FieldName] = 0;
            }
            dictionary[result3.数据.DataType][result3.字段.FieldName]++;
        }
        while (!哈希宠物表.IsEmpty)
        {
            if (!哈希宠物表.TryDequeue(out var result4) || result4.Index == 0)
            {
                continue;
            }
            if (!(Session.Tables[typeof(PetInfo)][result4.Index] is PetInfo item))
            {
                if (!dictionary.ContainsKey(result4.数据.DataType))
                {
                    dictionary[result4.数据.DataType] = new Dictionary<string, int>();
                }
                if (!dictionary[result4.数据.DataType].ContainsKey(result4.字段.FieldName))
                {
                    dictionary[result4.数据.DataType][result4.字段.FieldName] = 0;
                }
                dictionary[result4.数据.DataType][result4.字段.FieldName]++;
            }
            else
            {
                result4.内部列表.Add(item);
                num++;
            }
        }
        while (!哈希角色表.IsEmpty)
        {
            if (!哈希角色表.TryDequeue(out var result5) || result5.Index == 0)
            {
                continue;
            }
            if (!(Session.Tables[typeof(CharacterInfo)][result5.Index] is CharacterInfo item2))
            {
                if (!dictionary.ContainsKey(result5.数据.DataType))
                {
                    dictionary[result5.数据.DataType] = new Dictionary<string, int>();
                }
                if (!dictionary[result5.数据.DataType].ContainsKey(result5.字段.FieldName))
                {
                    dictionary[result5.数据.DataType][result5.字段.FieldName] = 0;
                }
                dictionary[result5.数据.DataType][result5.字段.FieldName]++;
            }
            else
            {
                result5.内部列表.Add(item2);
                num++;
            }
        }
        while (!哈希邮件表.IsEmpty)
        {
            if (!哈希邮件表.TryDequeue(out var result6) || result6.Index == 0)
            {
                continue;
            }
            if (!(Session.Tables[typeof(MailInfo)][result6.Index] is MailInfo item3))
            {
                if (!dictionary.ContainsKey(result6.数据.DataType))
                {
                    dictionary[result6.数据.DataType] = new Dictionary<string, int>();
                }
                if (!dictionary[result6.数据.DataType].ContainsKey(result6.字段.FieldName))
                {
                    dictionary[result6.数据.DataType][result6.字段.FieldName] = 0;
                }
                dictionary[result6.数据.DataType][result6.字段.FieldName]++;
            }
            else
            {
                result6.内部列表.Add(item3);
                num++;
            }
        }
        SMain.AddSystemLog($"Data correlation task processing completed, total number of tasks:{num}");
        dictionary.Sum((KeyValuePair<Type, Dictionary<string, int>> x) => x.Value.Sum((KeyValuePair<string, int> o) => o.Value));
        foreach (KeyValuePair<Type, Dictionary<string, int>> item4 in dictionary)
        {
            foreach (KeyValuePair<string, int> item5 in item4.Value)
            {
                SMain.AddSystemLog($"Data Type:[{item4.Key.Name}], Internal Fields:[{item5.Key}], Total[{item5.Value}]Failed to correlate data");
            }
        }
    }

    static DataLinkTable()
    {
        数据任务表 = new ConcurrentQueue<数据关联参数>();
        列表任务表 = new ConcurrentQueue<列表关联参数>();
        字典任务表 = new ConcurrentQueue<字典关联参数>();
        哈希宠物表 = new ConcurrentQueue<哈希关联参数<PetInfo>>();
        哈希角色表 = new ConcurrentQueue<哈希关联参数<CharacterInfo>>();
        哈希邮件表 = new ConcurrentQueue<哈希关联参数<MailInfo>>();
    }
}
