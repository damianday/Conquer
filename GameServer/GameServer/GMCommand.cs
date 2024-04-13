using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameServer;

public abstract class GMCommand
{
    private static readonly Dictionary<string, Type> 命令字典;
    private static readonly Dictionary<string, FieldInfo[]> 字段列表;
    private static readonly Dictionary<Type, Func<string, object>> 字段写入方法表;

    public static readonly Dictionary<string, string> Commands;
    public abstract ExecutionPriority Priority { get; }

    static GMCommand()
    {
        命令字典 = new Dictionary<string, Type>();
        Commands = new Dictionary<string, string>();
        字段列表 = new Dictionary<string, FieldInfo[]>();
        Type[] types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (Type type in types)
        {
            if (!type.IsSubclassOf(typeof(GMCommand)))
                continue;
            Dictionary<FieldInfo, int> 字段集合 = new Dictionary<FieldInfo, int>();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (FieldInfo fieldInfo in fields)
            {
                FieldDescription customAttribute = fieldInfo.GetCustomAttribute<FieldDescription>();
                if (customAttribute != null)
                {
                    字段集合.Add(fieldInfo, customAttribute.Index);
                }
            }
            命令字典[type.Name] = type;
            字段列表[type.Name] = 字段集合.Keys.OrderBy(x => 字段集合[x]).ToArray();
            Commands[type.Name] = "@" + type.Name;
            fields = 字段列表[type.Name];
            foreach (FieldInfo fieldInfo2 in fields)
            {
                Dictionary<string, string> dictionary = Commands;
                string name = type.Name;
                dictionary[name] = dictionary[name] + " " + fieldInfo2.Name;
            }
        }
        字段写入方法表 = new Dictionary<Type, Func<string, object>>
        {
            [typeof(string)] = (string s) => s,
            [typeof(int)] = (string s) => Convert.ToInt32(s),
            [typeof(uint)] = (string s) => Convert.ToUInt32(s),
            [typeof(byte)] = (string s) => Convert.ToByte(s),
            [typeof(bool)] = (string s) => Convert.ToBoolean(s),
            [typeof(float)] = (string s) => Convert.ToSingle(s),
            [typeof(decimal)] = (string s) => Convert.ToDecimal(s)
        };
    }

    public static bool ParseCommand(string text, out GMCommand command)
    {
        var array = text.Trim('@').Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var cmdText = array[0];

        if (命令字典.TryGetValue(cmdText, out var value) && 字段列表.TryGetValue(cmdText, out var value2))
        {
            if (array.Length <= 字段列表[cmdText].Length)
            {
                SMain.AddCommandLog("<= @Parameter length is incorrect, Please see format: " + Commands[cmdText]);
                command = null;
                return false;
            }
            var cmd = Activator.CreateInstance(value) as GMCommand;
            for (int i = 0; i < value2.Length; i++)
            {
                try
                {
                    value2[i].SetValue(cmd, 字段写入方法表[value2[i].FieldType](array[i + 1]));
                }
                catch
                {
                    SMain.AddCommandLog("<= @参数转换错误. 不能将字符串 '" + array[i + 1] + "' 转换为参数 '" + value2[i].Name + "' 所需要的数据类型");
                    command = null;
                    return false;
                }
            }
            command = cmd;
            return true;
        }
        SMain.AddCommandLog("<= @命令解析错误, '" + cmdText + "' 不是支持的GM命令");
        command = null;
        return false;
    }

    public abstract void ExecuteCommand();
}
