using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GameServer.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.2.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
    private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

    public static Settings Default => defaultInstance;

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("8701")]
    public ushort UserConnectionPort
    {
        get
        {
            return (ushort)this["UserConnectionPort"];
        }
        set
        {
            this["UserConnectionPort"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("6678")]
    public ushort TicketReceivePort
    {
        get
        {
            return (ushort)this["TicketReceivePort"];
        }
        set
        {
            this["TicketReceivePort"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public ushort PacketLimit
    {
        get
        {
            return (ushort)this["PacketLimit"];
        }
        set
        {
            this["PacketLimit"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("30")]
    public ushort AutoSaveInterval
    {
        get
        {
            return (ushort)this["AutoSaveInterval"];
        }
        set
        {
            this["AutoSaveInterval"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("45")]
    public ushort 自动保存日志
    {
        get
        {
            return (ushort)this["自动保存日志"];
        }
        set
        {
            this["自动保存日志"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("500")]
    public int MaxUserConnections
    {
        get
        {
            return (int)this["MaxUserConnections"];
        }
        set
        {
            this["MaxUserConnections"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 掉落贵重物品颜色
    {
        get
        {
            return (int)this["掉落贵重物品颜色"];
        }
        set
        {
            this["掉落贵重物品颜色"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 掉落沃玛物品颜色
    {
        get
        {
            return (int)this["掉落沃玛物品颜色"];
        }
        set
        {
            this["掉落沃玛物品颜色"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 掉落祖玛物品颜色
    {
        get
        {
            return (int)this["掉落祖玛物品颜色"];
        }
        set
        {
            this["掉落祖玛物品颜色"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 掉落赤月物品颜色
    {
        get
        {
            return (int)this["掉落赤月物品颜色"];
        }
        set
        {
            this["掉落赤月物品颜色"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 掉落魔龙物品颜色
    {
        get
        {
            return (int)this["掉落魔龙物品颜色"];
        }
        set
        {
            this["掉落魔龙物品颜色"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 掉落苍月物品颜色
    {
        get
        {
            return (int)this["掉落苍月物品颜色"];
        }
        set
        {
            this["掉落苍月物品颜色"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 掉落星王物品颜色
    {
        get
        {
            return (int)this["掉落星王物品颜色"];
        }
        set
        {
            this["掉落星王物品颜色"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 掉落城主物品颜色
    {
        get
        {
            return (int)this["掉落城主物品颜色"];
        }
        set
        {
            this["掉落城主物品颜色"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 掉落书籍物品颜色
    {
        get
        {
            return (int)this["掉落书籍物品颜色"];
        }
        set
        {
            this["掉落书籍物品颜色"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int DropPlayerNameColor
    {
        get
        {
            return (int)this["DropPlayerNameColor"];
        }
        set
        {
            this["DropPlayerNameColor"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 狂暴击杀玩家颜色
    {
        get
        {
            return (int)this["狂暴击杀玩家颜色"];
        }
        set
        {
            this["狂暴击杀玩家颜色"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 狂暴被杀玩家颜色
    {
        get
        {
            return (int)this["狂暴被杀玩家颜色"];
        }
        set
        {
            this["狂暴被杀玩家颜色"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 沃玛分解元宝
    {
        get
        {
            return (int)this["沃玛分解元宝"];
        }
        set
        {
            this["沃玛分解元宝"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("200")]
    public int 祖玛分解元宝
    {
        get
        {
            return (int)this["祖玛分解元宝"];
        }
        set
        {
            this["祖玛分解元宝"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("300")]
    public int 赤月分解元宝
    {
        get
        {
            return (int)this["赤月分解元宝"];
        }
        set
        {
            this["赤月分解元宝"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("400")]
    public int 魔龙分解元宝
    {
        get
        {
            return (int)this["魔龙分解元宝"];
        }
        set
        {
            this["魔龙分解元宝"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("500")]
    public int 苍月分解元宝
    {
        get
        {
            return (int)this["苍月分解元宝"];
        }
        set
        {
            this["苍月分解元宝"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("600")]
    public int 星王分解元宝
    {
        get
        {
            return (int)this["星王分解元宝"];
        }
        set
        {
            this["星王分解元宝"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("700")]
    public int 城主分解元宝
    {
        get
        {
            return (int)this["城主分解元宝"];
        }
        set
        {
            this["城主分解元宝"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("800")]
    public int 神秘分解元宝
    {
        get
        {
            return (int)this["神秘分解元宝"];
        }
        set
        {
            this["神秘分解元宝"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 屠魔组队人数
    {
        get
        {
            return (int)this["屠魔组队人数"];
        }
        set
        {
            this["屠魔组队人数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000000")]
    public int 屠魔令回收经验
    {
        get
        {
            return (int)this["屠魔令回收经验"];
        }
        set
        {
            this["屠魔令回收经验"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("13")]
    public byte 武斗场时间一
    {
        get
        {
            return (byte)this["武斗场时间一"];
        }
        set
        {
            this["武斗场时间一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("19")]
    public byte 武斗场时间二
    {
        get
        {
            return (byte)this["武斗场时间二"];
        }
        set
        {
            this["武斗场时间二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10000")]
    public int 武斗场经验小
    {
        get
        {
            return (int)this["武斗场经验小"];
        }
        set
        {
            this["武斗场经验小"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("50000")]
    public int 武斗场经验大
    {
        get
        {
            return (int)this["武斗场经验大"];
        }
        set
        {
            this["武斗场经验大"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("20")]
    public byte 沙巴克开启
    {
        get
        {
            return (byte)this["沙巴克开启"];
        }
        set
        {
            this["沙巴克开启"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("21")]
    public byte 沙巴克结束
    {
        get
        {
            return (byte)this["沙巴克结束"];
        }
        set
        {
            this["沙巴克结束"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("80")]
    public int 祝福油幸运1机率
    {
        get
        {
            return (int)this["祝福油幸运1机率"];
        }
        set
        {
            this["祝福油幸运1机率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("15")]
    public int 祝福油幸运2机率
    {
        get
        {
            return (int)this["祝福油幸运2机率"];
        }
        set
        {
            this["祝福油幸运2机率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 祝福油幸运3机率
    {
        get
        {
            return (int)this["祝福油幸运3机率"];
        }
        set
        {
            this["祝福油幸运3机率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("8")]
    public int 祝福油幸运4机率
    {
        get
        {
            return (int)this["祝福油幸运4机率"];
        }
        set
        {
            this["祝福油幸运4机率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 祝福油幸运5机率
    {
        get
        {
            return (int)this["祝福油幸运5机率"];
        }
        set
        {
            this["祝福油幸运5机率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("3")]
    public int 祝福油幸运6机率
    {
        get
        {
            return (int)this["祝福油幸运6机率"];
        }
        set
        {
            this["祝福油幸运6机率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 祝福油幸运7机率
    {
        get
        {
            return (int)this["祝福油幸运7机率"];
        }
        set
        {
            this["祝福油幸运7机率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int PKYellowNamePoint
    {
        get
        {
            return (int)this["PKYellowNamePoint"];
        }
        set
        {
            this["PKYellowNamePoint"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("300")]
    public int PKRedNamePoint
    {
        get
        {
            return (int)this["PKRedNamePoint"];
        }
        set
        {
            this["PKRedNamePoint"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("800")]
    public int PKCrimsonNamePoint
    {
        get
        {
            return (int)this["PKCrimsonNamePoint"];
        }
        set
        {
            this["PKCrimsonNamePoint"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 锻造成功倍数
    {
        get
        {
            return (int)this["锻造成功倍数"];
        }
        set
        {
            this["锻造成功倍数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int PK死亡幸运开关
    {
        get
        {
            return (int)this["PK死亡幸运开关"];
        }
        set
        {
            this["PK死亡幸运开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0.01")]
    public float 死亡掉落身上几率
    {
        get
        {
            return (float)this["死亡掉落身上几率"];
        }
        set
        {
            this["死亡掉落身上几率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0.01")]
    public float 死亡掉落背包几率
    {
        get
        {
            return (float)this["死亡掉落背包几率"];
        }
        set
        {
            this["死亡掉落背包几率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 屠魔副本次数
    {
        get
        {
            return (int)this["屠魔副本次数"];
        }
        set
        {
            this["屠魔副本次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("20000000")]
    public int 升级经验模块一
    {
        get
        {
            return (int)this["升级经验模块一"];
        }
        set
        {
            this["升级经验模块一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("40000000")]
    public int 升级经验模块二
    {
        get
        {
            return (int)this["升级经验模块二"];
        }
        set
        {
            this["升级经验模块二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("80000000")]
    public int 升级经验模块三
    {
        get
        {
            return (int)this["升级经验模块三"];
        }
        set
        {
            this["升级经验模块三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("160000000")]
    public int 升级经验模块四
    {
        get
        {
            return (int)this["升级经验模块四"];
        }
        set
        {
            this["升级经验模块四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("320000000")]
    public int 升级经验模块五
    {
        get
        {
            return (int)this["升级经验模块五"];
        }
        set
        {
            this["升级经验模块五"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("640000000")]
    public int 升级经验模块六
    {
        get
        {
            return (int)this["升级经验模块六"];
        }
        set
        {
            this["升级经验模块六"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1280000000")]
    public int 升级经验模块七
    {
        get
        {
            return (int)this["升级经验模块七"];
        }
        set
        {
            this["升级经验模块七"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块八
    {
        get
        {
            return (int)this["升级经验模块八"];
        }
        set
        {
            this["升级经验模块八"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块九
    {
        get
        {
            return (int)this["升级经验模块九"];
        }
        set
        {
            this["升级经验模块九"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块十
    {
        get
        {
            return (int)this["升级经验模块十"];
        }
        set
        {
            this["升级经验模块十"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块十一
    {
        get
        {
            return (int)this["升级经验模块十一"];
        }
        set
        {
            this["升级经验模块十一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块十二
    {
        get
        {
            return (int)this["升级经验模块十二"];
        }
        set
        {
            this["升级经验模块十二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块十三
    {
        get
        {
            return (int)this["升级经验模块十三"];
        }
        set
        {
            this["升级经验模块十三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块十四
    {
        get
        {
            return (int)this["升级经验模块十四"];
        }
        set
        {
            this["升级经验模块十四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块十五
    {
        get
        {
            return (int)this["升级经验模块十五"];
        }
        set
        {
            this["升级经验模块十五"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块十六
    {
        get
        {
            return (int)this["升级经验模块十六"];
        }
        set
        {
            this["升级经验模块十六"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块十七
    {
        get
        {
            return (int)this["升级经验模块十七"];
        }
        set
        {
            this["升级经验模块十七"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块十八
    {
        get
        {
            return (int)this["升级经验模块十八"];
        }
        set
        {
            this["升级经验模块十八"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块十九
    {
        get
        {
            return (int)this["升级经验模块十九"];
        }
        set
        {
            this["升级经验模块十九"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块二十
    {
        get
        {
            return (int)this["升级经验模块二十"];
        }
        set
        {
            this["升级经验模块二十"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块二十一
    {
        get
        {
            return (int)this["升级经验模块二十一"];
        }
        set
        {
            this["升级经验模块二十一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块二十二
    {
        get
        {
            return (int)this["升级经验模块二十二"];
        }
        set
        {
            this["升级经验模块二十二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块二十三
    {
        get
        {
            return (int)this["升级经验模块二十三"];
        }
        set
        {
            this["升级经验模块二十三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块二十四
    {
        get
        {
            return (int)this["升级经验模块二十四"];
        }
        set
        {
            this["升级经验模块二十四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块二十五
    {
        get
        {
            return (int)this["升级经验模块二十五"];
        }
        set
        {
            this["升级经验模块二十五"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块二十六
    {
        get
        {
            return (int)this["升级经验模块二十六"];
        }
        set
        {
            this["升级经验模块二十六"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块二十七
    {
        get
        {
            return (int)this["升级经验模块二十七"];
        }
        set
        {
            this["升级经验模块二十七"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块二十八
    {
        get
        {
            return (int)this["升级经验模块二十八"];
        }
        set
        {
            this["升级经验模块二十八"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块二十九
    {
        get
        {
            return (int)this["升级经验模块二十九"];
        }
        set
        {
            this["升级经验模块二十九"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000000")]
    public int 升级经验模块三十
    {
        get
        {
            return (int)this["升级经验模块三十"];
        }
        set
        {
            this["升级经验模块三十"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("80")]
    public int 高级祝福油幸运机率
    {
        get
        {
            return (int)this["高级祝福油幸运机率"];
        }
        set
        {
            this["高级祝福油幸运机率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("90235")]
    public int 雕爷使用物品
    {
        get
        {
            return (int)this["雕爷使用物品"];
        }
        set
        {
            this["雕爷使用物品"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10000")]
    public int 雕爷使用金币
    {
        get
        {
            return (int)this["雕爷使用金币"];
        }
        set
        {
            this["雕爷使用金币"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("131")]
    public int 称号范围拾取判断
    {
        get
        {
            return (int)this["称号范围拾取判断"];
        }
        set
        {
            this["称号范围拾取判断"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int TitleRangePickUpDistance
    {
        get
        {
            return (int)this["TitleRangePickUpDistance"];
        }
        set
        {
            this["TitleRangePickUpDistance"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("20")]
    public int 行会申请人数限制
    {
        get
        {
            return (int)this["行会申请人数限制"];
        }
        set
        {
            this["行会申请人数限制"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("75")]
    public int 万年雪霜HP
    {
        get
        {
            return (int)this["万年雪霜HP"];
        }
        set
        {
            this["万年雪霜HP"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("110")]
    public int 万年雪霜MP
    {
        get
        {
            return (int)this["万年雪霜MP"];
        }
        set
        {
            this["万年雪霜MP"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("160")]
    public int 疗伤药MP
    {
        get
        {
            return (int)this["疗伤药MP"];
        }
        set
        {
            this["疗伤药MP"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("110")]
    public int 疗伤药HP
    {
        get
        {
            return (int)this["疗伤药HP"];
        }
        set
        {
            this["疗伤药HP"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 元宝金币回收设定
    {
        get
        {
            return (int)this["元宝金币回收设定"];
        }
        set
        {
            this["元宝金币回收设定"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 元宝金币传送设定
    {
        get
        {
            return (int)this["元宝金币传送设定"];
        }
        set
        {
            this["元宝金币传送设定"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("101")]
    public int 快捷传送一编号
    {
        get
        {
            return (int)this["快捷传送一编号"];
        }
        set
        {
            this["快捷传送一编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 快捷传送一货币
    {
        get
        {
            return (int)this["快捷传送一货币"];
        }
        set
        {
            this["快捷传送一货币"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 快捷传送一等级
    {
        get
        {
            return (int)this["快捷传送一等级"];
        }
        set
        {
            this["快捷传送一等级"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("101")]
    public int 快捷传送二编号
    {
        get
        {
            return (int)this["快捷传送二编号"];
        }
        set
        {
            this["快捷传送二编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 快捷传送二货币
    {
        get
        {
            return (int)this["快捷传送二货币"];
        }
        set
        {
            this["快捷传送二货币"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 快捷传送二等级
    {
        get
        {
            return (int)this["快捷传送二等级"];
        }
        set
        {
            this["快捷传送二等级"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("101")]
    public int 快捷传送三编号
    {
        get
        {
            return (int)this["快捷传送三编号"];
        }
        set
        {
            this["快捷传送三编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 快捷传送三货币
    {
        get
        {
            return (int)this["快捷传送三货币"];
        }
        set
        {
            this["快捷传送三货币"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 快捷传送三等级
    {
        get
        {
            return (int)this["快捷传送三等级"];
        }
        set
        {
            this["快捷传送三等级"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("101")]
    public int 快捷传送四编号
    {
        get
        {
            return (int)this["快捷传送四编号"];
        }
        set
        {
            this["快捷传送四编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 快捷传送四货币
    {
        get
        {
            return (int)this["快捷传送四货币"];
        }
        set
        {
            this["快捷传送四货币"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 快捷传送四等级
    {
        get
        {
            return (int)this["快捷传送四等级"];
        }
        set
        {
            this["快捷传送四等级"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("101")]
    public int 快捷传送五编号
    {
        get
        {
            return (int)this["快捷传送五编号"];
        }
        set
        {
            this["快捷传送五编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 快捷传送五货币
    {
        get
        {
            return (int)this["快捷传送五货币"];
        }
        set
        {
            this["快捷传送五货币"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 快捷传送五等级
    {
        get
        {
            return (int)this["快捷传送五等级"];
        }
        set
        {
            this["快捷传送五等级"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 狂暴货币格式
    {
        get
        {
            return (int)this["狂暴货币格式"];
        }
        set
        {
            this["狂暴货币格式"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("135")]
    public byte 狂暴称号格式
    {
        get
        {
            return (byte)this["狂暴称号格式"];
        }
        set
        {
            this["狂暴称号格式"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("91730")]
    public int 狂暴开启物品名称
    {
        get
        {
            return (int)this["狂暴开启物品名称"];
        }
        set
        {
            this["狂暴开启物品名称"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("20")]
    public int 狂暴开启物品数量
    {
        get
        {
            return (int)this["狂暴开启物品数量"];
        }
        set
        {
            this["狂暴开启物品数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 狂暴杀死物品数量
    {
        get
        {
            return (int)this["狂暴杀死物品数量"];
        }
        set
        {
            this["狂暴杀死物品数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 狂暴开启元宝数量
    {
        get
        {
            return (int)this["狂暴开启元宝数量"];
        }
        set
        {
            this["狂暴开启元宝数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("50000")]
    public int 狂暴杀死元宝数量
    {
        get
        {
            return (int)this["狂暴杀死元宝数量"];
        }
        set
        {
            this["狂暴杀死元宝数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("50000")]
    public int 狂暴杀死金币数量
    {
        get
        {
            return (int)this["狂暴杀死金币数量"];
        }
        set
        {
            this["狂暴杀死金币数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 狂暴开启金币数量
    {
        get
        {
            return (int)this["狂暴开启金币数量"];
        }
        set
        {
            this["狂暴开启金币数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("龍纹兑换石")]
    public string 狂暴名称
    {
        get
        {
            return (string)this["狂暴名称"];
        }
        set
        {
            this["狂暴名称"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("自定义物品一")]
    public string 自定义物品内容一
    {
        get
        {
            return (string)this["自定义物品内容一"];
        }
        set
        {
            this["自定义物品内容一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("自定义物品二")]
    public string 自定义物品内容二
    {
        get
        {
            return (string)this["自定义物品内容二"];
        }
        set
        {
            this["自定义物品内容二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("自定义物品三")]
    public string 自定义物品内容三
    {
        get
        {
            return (string)this["自定义物品内容三"];
        }
        set
        {
            this["自定义物品内容三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("自定义物品四")]
    public string 自定义物品内容四
    {
        get
        {
            return (string)this["自定义物品内容四"];
        }
        set
        {
            this["自定义物品内容四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("自定义物品五")]
    public string 自定义物品内容五
    {
        get
        {
            return (string)this["自定义物品内容五"];
        }
        set
        {
            this["自定义物品内容五"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 自定义物品数量一
    {
        get
        {
            return (int)this["自定义物品数量一"];
        }
        set
        {
            this["自定义物品数量一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 自定义物品数量二
    {
        get
        {
            return (int)this["自定义物品数量二"];
        }
        set
        {
            this["自定义物品数量二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 自定义物品数量三
    {
        get
        {
            return (int)this["自定义物品数量三"];
        }
        set
        {
            this["自定义物品数量三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 自定义物品数量四
    {
        get
        {
            return (int)this["自定义物品数量四"];
        }
        set
        {
            this["自定义物品数量四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 自定义物品数量五
    {
        get
        {
            return (int)this["自定义物品数量五"];
        }
        set
        {
            this["自定义物品数量五"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("101")]
    public byte 自定义称号内容一
    {
        get
        {
            return (byte)this["自定义称号内容一"];
        }
        set
        {
            this["自定义称号内容一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("101")]
    public byte 自定义称号内容二
    {
        get
        {
            return (byte)this["自定义称号内容二"];
        }
        set
        {
            this["自定义称号内容二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("101")]
    public byte 自定义称号内容三
    {
        get
        {
            return (byte)this["自定义称号内容三"];
        }
        set
        {
            this["自定义称号内容三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("101")]
    public byte 自定义称号内容四
    {
        get
        {
            return (byte)this["自定义称号内容四"];
        }
        set
        {
            this["自定义称号内容四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("101")]
    public byte 自定义称号内容五
    {
        get
        {
            return (byte)this["自定义称号内容五"];
        }
        set
        {
            this["自定义称号内容五"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 泡点秒数控制
    {
        get
        {
            return (int)this["泡点秒数控制"];
        }
        set
        {
            this["泡点秒数控制"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 祖玛战装备佩戴数量
    {
        get
        {
            return (int)this["祖玛战装备佩戴数量"];
        }
        set
        {
            this["祖玛战装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 祖玛法装备佩戴数量
    {
        get
        {
            return (int)this["祖玛法装备佩戴数量"];
        }
        set
        {
            this["祖玛法装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 祖玛道装备佩戴数量
    {
        get
        {
            return (int)this["祖玛道装备佩戴数量"];
        }
        set
        {
            this["祖玛道装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 祖玛弓装备佩戴数量
    {
        get
        {
            return (int)this["祖玛弓装备佩戴数量"];
        }
        set
        {
            this["祖玛弓装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 祖玛刺装备佩戴数量
    {
        get
        {
            return (int)this["祖玛刺装备佩戴数量"];
        }
        set
        {
            this["祖玛刺装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 祖玛枪装备佩戴数量
    {
        get
        {
            return (int)this["祖玛枪装备佩戴数量"];
        }
        set
        {
            this["祖玛枪装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 赤月战装备佩戴数量
    {
        get
        {
            return (int)this["赤月战装备佩戴数量"];
        }
        set
        {
            this["赤月战装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 赤月法装备佩戴数量
    {
        get
        {
            return (int)this["赤月法装备佩戴数量"];
        }
        set
        {
            this["赤月法装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 赤月道装备佩戴数量
    {
        get
        {
            return (int)this["赤月道装备佩戴数量"];
        }
        set
        {
            this["赤月道装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 赤月弓装备佩戴数量
    {
        get
        {
            return (int)this["赤月弓装备佩戴数量"];
        }
        set
        {
            this["赤月弓装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 赤月刺装备佩戴数量
    {
        get
        {
            return (int)this["赤月刺装备佩戴数量"];
        }
        set
        {
            this["赤月刺装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 赤月枪装备佩戴数量
    {
        get
        {
            return (int)this["赤月枪装备佩戴数量"];
        }
        set
        {
            this["赤月枪装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 魔龙战装备佩戴数量
    {
        get
        {
            return (int)this["魔龙战装备佩戴数量"];
        }
        set
        {
            this["魔龙战装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 魔龙法装备佩戴数量
    {
        get
        {
            return (int)this["魔龙法装备佩戴数量"];
        }
        set
        {
            this["魔龙法装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 魔龙道装备佩戴数量
    {
        get
        {
            return (int)this["魔龙道装备佩戴数量"];
        }
        set
        {
            this["魔龙道装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 魔龙弓装备佩戴数量
    {
        get
        {
            return (int)this["魔龙弓装备佩戴数量"];
        }
        set
        {
            this["魔龙弓装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 魔龙刺装备佩戴数量
    {
        get
        {
            return (int)this["魔龙刺装备佩戴数量"];
        }
        set
        {
            this["魔龙刺装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 魔龙枪装备佩戴数量
    {
        get
        {
            return (int)this["魔龙枪装备佩戴数量"];
        }
        set
        {
            this["魔龙枪装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 苍月战装备佩戴数量
    {
        get
        {
            return (int)this["苍月战装备佩戴数量"];
        }
        set
        {
            this["苍月战装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 苍月法装备佩戴数量
    {
        get
        {
            return (int)this["苍月法装备佩戴数量"];
        }
        set
        {
            this["苍月法装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 苍月道装备佩戴数量
    {
        get
        {
            return (int)this["苍月道装备佩戴数量"];
        }
        set
        {
            this["苍月道装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 苍月弓装备佩戴数量
    {
        get
        {
            return (int)this["苍月弓装备佩戴数量"];
        }
        set
        {
            this["苍月弓装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 苍月刺装备佩戴数量
    {
        get
        {
            return (int)this["苍月刺装备佩戴数量"];
        }
        set
        {
            this["苍月刺装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 苍月枪装备佩戴数量
    {
        get
        {
            return (int)this["苍月枪装备佩戴数量"];
        }
        set
        {
            this["苍月枪装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 星王战装备佩戴数量
    {
        get
        {
            return (int)this["星王战装备佩戴数量"];
        }
        set
        {
            this["星王战装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 星王法装备佩戴数量
    {
        get
        {
            return (int)this["星王法装备佩戴数量"];
        }
        set
        {
            this["星王法装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 星王道装备佩戴数量
    {
        get
        {
            return (int)this["星王道装备佩戴数量"];
        }
        set
        {
            this["星王道装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 星王弓装备佩戴数量
    {
        get
        {
            return (int)this["星王弓装备佩戴数量"];
        }
        set
        {
            this["星王弓装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 星王刺装备佩戴数量
    {
        get
        {
            return (int)this["星王刺装备佩戴数量"];
        }
        set
        {
            this["星王刺装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 星王枪装备佩戴数量
    {
        get
        {
            return (int)this["星王枪装备佩戴数量"];
        }
        set
        {
            this["星王枪装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 特殊1战装备佩戴数量
    {
        get
        {
            return (int)this["特殊1战装备佩戴数量"];
        }
        set
        {
            this["特殊1战装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 特殊1法装备佩戴数量
    {
        get
        {
            return (int)this["特殊1法装备佩戴数量"];
        }
        set
        {
            this["特殊1法装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 特殊1道装备佩戴数量
    {
        get
        {
            return (int)this["特殊1道装备佩戴数量"];
        }
        set
        {
            this["特殊1道装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 特殊1弓装备佩戴数量
    {
        get
        {
            return (int)this["特殊1弓装备佩戴数量"];
        }
        set
        {
            this["特殊1弓装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 特殊1刺装备佩戴数量
    {
        get
        {
            return (int)this["特殊1刺装备佩戴数量"];
        }
        set
        {
            this["特殊1刺装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 特殊1枪装备佩戴数量
    {
        get
        {
            return (int)this["特殊1枪装备佩戴数量"];
        }
        set
        {
            this["特殊1枪装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 特殊2战装备佩戴数量
    {
        get
        {
            return (int)this["特殊2战装备佩戴数量"];
        }
        set
        {
            this["特殊2战装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 特殊2法装备佩戴数量
    {
        get
        {
            return (int)this["特殊2法装备佩戴数量"];
        }
        set
        {
            this["特殊2法装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 特殊2道装备佩戴数量
    {
        get
        {
            return (int)this["特殊2道装备佩戴数量"];
        }
        set
        {
            this["特殊2道装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 特殊2弓装备佩戴数量
    {
        get
        {
            return (int)this["特殊2弓装备佩戴数量"];
        }
        set
        {
            this["特殊2弓装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 特殊2刺装备佩戴数量
    {
        get
        {
            return (int)this["特殊2刺装备佩戴数量"];
        }
        set
        {
            this["特殊2刺装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 特殊2枪装备佩戴数量
    {
        get
        {
            return (int)this["特殊2枪装备佩戴数量"];
        }
        set
        {
            this["特殊2枪装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 特殊3战装备佩戴数量
    {
        get
        {
            return (int)this["特殊3战装备佩戴数量"];
        }
        set
        {
            this["特殊3战装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 特殊3法装备佩戴数量
    {
        get
        {
            return (int)this["特殊3法装备佩戴数量"];
        }
        set
        {
            this["特殊3法装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 特殊3道装备佩戴数量
    {
        get
        {
            return (int)this["特殊3道装备佩戴数量"];
        }
        set
        {
            this["特殊3道装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 特殊3弓装备佩戴数量
    {
        get
        {
            return (int)this["特殊3弓装备佩戴数量"];
        }
        set
        {
            this["特殊3弓装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 特殊3刺装备佩戴数量
    {
        get
        {
            return (int)this["特殊3刺装备佩戴数量"];
        }
        set
        {
            this["特殊3刺装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 特殊3枪装备佩戴数量
    {
        get
        {
            return (int)this["特殊3枪装备佩戴数量"];
        }
        set
        {
            this["特殊3枪装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 装备技能开关
    {
        get
        {
            return (int)this["装备技能开关"];
        }
        set
        {
            this["装备技能开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 御兽属性开启
    {
        get
        {
            return (int)this["御兽属性开启"];
        }
        set
        {
            this["御兽属性开启"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 可摆摊地图编号
    {
        get
        {
            return (int)this["可摆摊地图编号"];
        }
        set
        {
            this["可摆摊地图编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("935")]
    public int 可摆摊地图坐标X
    {
        get
        {
            return (int)this["可摆摊地图坐标X"];
        }
        set
        {
            this["可摆摊地图坐标X"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("522")]
    public int 可摆摊地图坐标Y
    {
        get
        {
            return (int)this["可摆摊地图坐标Y"];
        }
        set
        {
            this["可摆摊地图坐标Y"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 可摆摊地图范围
    {
        get
        {
            return (int)this["可摆摊地图范围"];
        }
        set
        {
            this["可摆摊地图范围"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 可摆摊货币选择
    {
        get
        {
            return (int)this["可摆摊货币选择"];
        }
        set
        {
            this["可摆摊货币选择"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("7")]
    public int 可摆摊等级
    {
        get
        {
            return (int)this["可摆摊等级"];
        }
        set
        {
            this["可摆摊等级"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("60")]
    public int ReviveInterval
    {
        get
        {
            return (int)this["ReviveInterval"];
        }
        set
        {
            this["ReviveInterval"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0.01")]
    public float 自定义麻痹几率
    {
        get
        {
            return (float)this["自定义麻痹几率"];
        }
        set
        {
            this["自定义麻痹几率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 怪物掉落广播开关
    {
        get
        {
            return (int)this["怪物掉落广播开关"];
        }
        set
        {
            this["怪物掉落广播开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 怪物掉落窗口开关
    {
        get
        {
            return (int)this["怪物掉落窗口开关"];
        }
        set
        {
            this["怪物掉落窗口开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 珍宝阁提示开关
    {
        get
        {
            return (int)this["珍宝阁提示开关"];
        }
        set
        {
            this["珍宝阁提示开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public ushort PetUpgradeXPLevel1
    {
        get
        {
            return (ushort)this["PetUpgradeXPLevel1"];
        }
        set
        {
            this["PetUpgradeXPLevel1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public ushort PetUpgradeXPLevel2
    {
        get
        {
            return (ushort)this["PetUpgradeXPLevel2"];
        }
        set
        {
            this["PetUpgradeXPLevel2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("15")]
    public ushort PetUpgradeXPLevel3
    {
        get
        {
            return (ushort)this["PetUpgradeXPLevel3"];
        }
        set
        {
            this["PetUpgradeXPLevel3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("20")]
    public ushort PetUpgradeXPLevel4
    {
        get
        {
            return (ushort)this["PetUpgradeXPLevel4"];
        }
        set
        {
            this["PetUpgradeXPLevel4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("25")]
    public ushort PetUpgradeXPLevel5
    {
        get
        {
            return (ushort)this["PetUpgradeXPLevel5"];
        }
        set
        {
            this["PetUpgradeXPLevel5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("30")]
    public ushort PetUpgradeXPLevel6
    {
        get
        {
            return (ushort)this["PetUpgradeXPLevel6"];
        }
        set
        {
            this["PetUpgradeXPLevel6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("35")]
    public ushort PetUpgradeXPLevel7
    {
        get
        {
            return (ushort)this["PetUpgradeXPLevel7"];
        }
        set
        {
            this["PetUpgradeXPLevel7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("40")]
    public ushort PetUpgradeXPLevel8
    {
        get
        {
            return (ushort)this["PetUpgradeXPLevel8"];
        }
        set
        {
            this["PetUpgradeXPLevel8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("45")]
    public ushort PetUpgradeXPLevel9
    {
        get
        {
            return (ushort)this["PetUpgradeXPLevel9"];
        }
        set
        {
            this["PetUpgradeXPLevel9"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 下马击落机率
    {
        get
        {
            return (int)this["下马击落机率"];
        }
        set
        {
            this["下马击落机率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int AllowRaceDragonLance
    {
        get
        {
            return (int)this["AllowRaceDragonLance"];
        }
        set
        {
            this["AllowRaceDragonLance"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int AllowRaceWarrior
    {
        get
        {
            return (int)this["AllowRaceWarrior"];
        }
        set
        {
            this["AllowRaceWarrior"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int AllowRaceWizard
    {
        get
        {
            return (int)this["AllowRaceWizard"];
        }
        set
        {
            this["AllowRaceWizard"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int AllowRaceTaoist
    {
        get
        {
            return (int)this["AllowRaceTaoist"];
        }
        set
        {
            this["AllowRaceTaoist"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int AllowRaceAssassin
    {
        get
        {
            return (int)this["AllowRaceAssassin"];
        }
        set
        {
            this["AllowRaceAssassin"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int AllowRaceArcher
    {
        get
        {
            return (int)this["AllowRaceArcher"];
        }
        set
        {
            this["AllowRaceArcher"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 泡点等级开关
    {
        get
        {
            return (int)this["泡点等级开关"];
        }
        set
        {
            this["泡点等级开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 泡点当前经验
    {
        get
        {
            return (int)this["泡点当前经验"];
        }
        set
        {
            this["泡点当前经验"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("40")]
    public int 泡点限制等级
    {
        get
        {
            return (int)this["泡点限制等级"];
        }
        set
        {
            this["泡点限制等级"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 杀人PK红名开关
    {
        get
        {
            return (int)this["杀人PK红名开关"];
        }
        set
        {
            this["杀人PK红名开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 元宝金币传送设定2
    {
        get
        {
            return (int)this["元宝金币传送设定2"];
        }
        set
        {
            this["元宝金币传送设定2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("101")]
    public int 快捷传送一编号2
    {
        get
        {
            return (int)this["快捷传送一编号2"];
        }
        set
        {
            this["快捷传送一编号2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 快捷传送一货币2
    {
        get
        {
            return (int)this["快捷传送一货币2"];
        }
        set
        {
            this["快捷传送一货币2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 快捷传送一等级2
    {
        get
        {
            return (int)this["快捷传送一等级2"];
        }
        set
        {
            this["快捷传送一等级2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("101")]
    public int 快捷传送二编号2
    {
        get
        {
            return (int)this["快捷传送二编号2"];
        }
        set
        {
            this["快捷传送二编号2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 快捷传送二货币2
    {
        get
        {
            return (int)this["快捷传送二货币2"];
        }
        set
        {
            this["快捷传送二货币2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 快捷传送二等级2
    {
        get
        {
            return (int)this["快捷传送二等级2"];
        }
        set
        {
            this["快捷传送二等级2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("101")]
    public int 快捷传送三编号2
    {
        get
        {
            return (int)this["快捷传送三编号2"];
        }
        set
        {
            this["快捷传送三编号2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 快捷传送三货币2
    {
        get
        {
            return (int)this["快捷传送三货币2"];
        }
        set
        {
            this["快捷传送三货币2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 快捷传送三等级2
    {
        get
        {
            return (int)this["快捷传送三等级2"];
        }
        set
        {
            this["快捷传送三等级2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("101")]
    public int 快捷传送四编号2
    {
        get
        {
            return (int)this["快捷传送四编号2"];
        }
        set
        {
            this["快捷传送四编号2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 快捷传送四货币2
    {
        get
        {
            return (int)this["快捷传送四货币2"];
        }
        set
        {
            this["快捷传送四货币2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 快捷传送四等级2
    {
        get
        {
            return (int)this["快捷传送四等级2"];
        }
        set
        {
            this["快捷传送四等级2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("101")]
    public int 快捷传送五编号2
    {
        get
        {
            return (int)this["快捷传送五编号2"];
        }
        set
        {
            this["快捷传送五编号2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 快捷传送五货币2
    {
        get
        {
            return (int)this["快捷传送五货币2"];
        }
        set
        {
            this["快捷传送五货币2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 快捷传送五等级2
    {
        get
        {
            return (int)this["快捷传送五等级2"];
        }
        set
        {
            this["快捷传送五等级2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 快捷传送六等级2
    {
        get
        {
            return (int)this["快捷传送六等级2"];
        }
        set
        {
            this["快捷传送六等级2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("101")]
    public int 快捷传送六编号2
    {
        get
        {
            return (int)this["快捷传送六编号2"];
        }
        set
        {
            this["快捷传送六编号2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 快捷传送六货币2
    {
        get
        {
            return (int)this["快捷传送六货币2"];
        }
        set
        {
            this["快捷传送六货币2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("20000")]
    public int 秘宝广场元宝
    {
        get
        {
            return (int)this["秘宝广场元宝"];
        }
        set
        {
            this["秘宝广场元宝"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("600")]
    public int 每周特惠礼包一元宝
    {
        get
        {
            return (int)this["每周特惠礼包一元宝"];
        }
        set
        {
            this["每周特惠礼包一元宝"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("3000")]
    public int 每周特惠礼包二元宝
    {
        get
        {
            return (int)this["每周特惠礼包二元宝"];
        }
        set
        {
            this["每周特惠礼包二元宝"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("12800")]
    public int 特权玛法名俊元宝
    {
        get
        {
            return (int)this["特权玛法名俊元宝"];
        }
        set
        {
            this["特权玛法名俊元宝"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("28800")]
    public int 特权玛法豪杰元宝
    {
        get
        {
            return (int)this["特权玛法豪杰元宝"];
        }
        set
        {
            this["特权玛法豪杰元宝"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("28800")]
    public int 特权玛法战将元宝
    {
        get
        {
            return (int)this["特权玛法战将元宝"];
        }
        set
        {
            this["特权玛法战将元宝"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("20")]
    public byte BOSS一时间
    {
        get
        {
            return (byte)this["BOSS一时间"];
        }
        set
        {
            this["BOSS一时间"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("30")]
    public byte BOSS一分钟
    {
        get
        {
            return (byte)this["BOSS一分钟"];
        }
        set
        {
            this["BOSS一分钟"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int BOSS一地图编号
    {
        get
        {
            return (int)this["BOSS一地图编号"];
        }
        set
        {
            this["BOSS一地图编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999")]
    public int BOSS一坐标X
    {
        get
        {
            return (int)this["BOSS一坐标X"];
        }
        set
        {
            this["BOSS一坐标X"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999")]
    public int BOSS一坐标Y
    {
        get
        {
            return (int)this["BOSS一坐标Y"];
        }
        set
        {
            this["BOSS一坐标Y"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 屠魔爆率开关
    {
        get
        {
            return (int)this["屠魔爆率开关"];
        }
        set
        {
            this["屠魔爆率开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 武斗场次数限制
    {
        get
        {
            return (int)this["武斗场次数限制"];
        }
        set
        {
            this["武斗场次数限制"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("6")]
    public int AutoPickUpInventorySpace
    {
        get
        {
            return (int)this["AutoPickUpInventorySpace"];
        }
        set
        {
            this["AutoPickUpInventorySpace"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 自动整理背包开关
    {
        get
        {
            return (int)this["自动整理背包开关"];
        }
        set
        {
            this["自动整理背包开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 自动整理背包计时
    {
        get
        {
            return (int)this["自动整理背包计时"];
        }
        set
        {
            this["自动整理背包计时"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 称号叠加开关
    {
        get
        {
            return (int)this["称号叠加开关"];
        }
        set
        {
            this["称号叠加开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 称号叠加模块一
    {
        get
        {
            return (byte)this["称号叠加模块一"];
        }
        set
        {
            this["称号叠加模块一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 称号叠加模块二
    {
        get
        {
            return (byte)this["称号叠加模块二"];
        }
        set
        {
            this["称号叠加模块二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 称号叠加模块五
    {
        get
        {
            return (byte)this["称号叠加模块五"];
        }
        set
        {
            this["称号叠加模块五"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 称号叠加模块三
    {
        get
        {
            return (byte)this["称号叠加模块三"];
        }
        set
        {
            this["称号叠加模块三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 称号叠加模块四
    {
        get
        {
            return (byte)this["称号叠加模块四"];
        }
        set
        {
            this["称号叠加模块四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 称号叠加模块六
    {
        get
        {
            return (byte)this["称号叠加模块六"];
        }
        set
        {
            this["称号叠加模块六"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 称号叠加模块七
    {
        get
        {
            return (byte)this["称号叠加模块七"];
        }
        set
        {
            this["称号叠加模块七"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 称号叠加模块八
    {
        get
        {
            return (byte)this["称号叠加模块八"];
        }
        set
        {
            this["称号叠加模块八"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 沙城传送货币开关
    {
        get
        {
            return (int)this["沙城传送货币开关"];
        }
        set
        {
            this["沙城传送货币开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 沙城快捷货币一
    {
        get
        {
            return (int)this["沙城快捷货币一"];
        }
        set
        {
            this["沙城快捷货币一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 沙城快捷货币二
    {
        get
        {
            return (int)this["沙城快捷货币二"];
        }
        set
        {
            this["沙城快捷货币二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 沙城快捷货币三
    {
        get
        {
            return (int)this["沙城快捷货币三"];
        }
        set
        {
            this["沙城快捷货币三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 沙城快捷货币四
    {
        get
        {
            return (int)this["沙城快捷货币四"];
        }
        set
        {
            this["沙城快捷货币四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("45")]
    public int 沙城快捷等级一
    {
        get
        {
            return (int)this["沙城快捷等级一"];
        }
        set
        {
            this["沙城快捷等级一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("45")]
    public int 沙城快捷等级二
    {
        get
        {
            return (int)this["沙城快捷等级二"];
        }
        set
        {
            this["沙城快捷等级二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("45")]
    public int 沙城快捷等级三
    {
        get
        {
            return (int)this["沙城快捷等级三"];
        }
        set
        {
            this["沙城快捷等级三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("45")]
    public int 沙城快捷等级四
    {
        get
        {
            return (int)this["沙城快捷等级四"];
        }
        set
        {
            this["沙城快捷等级四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int BOSS刷新提示开关
    {
        get
        {
            return (int)this["BOSS刷新提示开关"];
        }
        set
        {
            this["BOSS刷新提示开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("99")]
    public byte 猎魔暗使称号一
    {
        get
        {
            return (byte)this["猎魔暗使称号一"];
        }
        set
        {
            this["猎魔暗使称号一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("111023")]
    public int 猎魔暗使材料一
    {
        get
        {
            return (int)this["猎魔暗使材料一"];
        }
        set
        {
            this["猎魔暗使材料一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("500")]
    public int 猎魔暗使数量一
    {
        get
        {
            return (int)this["猎魔暗使数量一"];
        }
        set
        {
            this["猎魔暗使数量一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("102")]
    public byte 猎魔暗使称号二
    {
        get
        {
            return (byte)this["猎魔暗使称号二"];
        }
        set
        {
            this["猎魔暗使称号二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("111025")]
    public int 猎魔暗使材料二
    {
        get
        {
            return (int)this["猎魔暗使材料二"];
        }
        set
        {
            this["猎魔暗使材料二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("500")]
    public int 猎魔暗使数量二
    {
        get
        {
            return (int)this["猎魔暗使数量二"];
        }
        set
        {
            this["猎魔暗使数量二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("107")]
    public byte 猎魔暗使称号三
    {
        get
        {
            return (byte)this["猎魔暗使称号三"];
        }
        set
        {
            this["猎魔暗使称号三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("111027")]
    public int 猎魔暗使材料三
    {
        get
        {
            return (int)this["猎魔暗使材料三"];
        }
        set
        {
            this["猎魔暗使材料三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("500")]
    public int 猎魔暗使数量三
    {
        get
        {
            return (int)this["猎魔暗使数量三"];
        }
        set
        {
            this["猎魔暗使数量三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("59")]
    public byte 猎魔暗使称号四
    {
        get
        {
            return (byte)this["猎魔暗使称号四"];
        }
        set
        {
            this["猎魔暗使称号四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("90220")]
    public int 猎魔暗使材料四
    {
        get
        {
            return (int)this["猎魔暗使材料四"];
        }
        set
        {
            this["猎魔暗使材料四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("500")]
    public int 猎魔暗使数量四
    {
        get
        {
            return (int)this["猎魔暗使数量四"];
        }
        set
        {
            this["猎魔暗使数量四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("138")]
    public byte 猎魔暗使称号五
    {
        get
        {
            return (byte)this["猎魔暗使称号五"];
        }
        set
        {
            this["猎魔暗使称号五"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("90228")]
    public int 猎魔暗使材料五
    {
        get
        {
            return (int)this["猎魔暗使材料五"];
        }
        set
        {
            this["猎魔暗使材料五"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("500")]
    public int 猎魔暗使数量五
    {
        get
        {
            return (int)this["猎魔暗使数量五"];
        }
        set
        {
            this["猎魔暗使数量五"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("60")]
    public byte 猎魔暗使称号六
    {
        get
        {
            return (byte)this["猎魔暗使称号六"];
        }
        set
        {
            this["猎魔暗使称号六"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("90236")]
    public int 猎魔暗使材料六
    {
        get
        {
            return (int)this["猎魔暗使材料六"];
        }
        set
        {
            this["猎魔暗使材料六"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("500")]
    public int 猎魔暗使数量六
    {
        get
        {
            return (int)this["猎魔暗使数量六"];
        }
        set
        {
            this["猎魔暗使数量六"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("19")]
    public byte 世界BOSS时间
    {
        get
        {
            return (byte)this["世界BOSS时间"];
        }
        set
        {
            this["世界BOSS时间"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("30")]
    public byte 世界BOSS分钟
    {
        get
        {
            return (byte)this["世界BOSS分钟"];
        }
        set
        {
            this["世界BOSS分钟"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("战将特权礼包")]
    public string 战将特权礼包
    {
        get
        {
            return (string)this["战将特权礼包"];
        }
        set
        {
            this["战将特权礼包"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("豪杰特权礼包")]
    public string 豪杰特权礼包
    {
        get
        {
            return (string)this["豪杰特权礼包"];
        }
        set
        {
            this["豪杰特权礼包"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("魔火龙")]
    public string 世界BOSS名字
    {
        get
        {
            return (string)this["世界BOSS名字"];
        }
        set
        {
            this["世界BOSS名字"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("BOSS名称")]
    public string BOSS名字一
    {
        get
        {
            return (string)this["BOSS名字一"];
        }
        set
        {
            this["BOSS名字一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("地图名称")]
    public string BOSS一地图名字
    {
        get
        {
            return (string)this["BOSS一地图名字"];
        }
        set
        {
            this["BOSS一地图名字"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("20")]
    public byte BOSS二时间
    {
        get
        {
            return (byte)this["BOSS二时间"];
        }
        set
        {
            this["BOSS二时间"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("30")]
    public byte BOSS二分钟
    {
        get
        {
            return (byte)this["BOSS二分钟"];
        }
        set
        {
            this["BOSS二分钟"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int BOSS二地图编号
    {
        get
        {
            return (int)this["BOSS二地图编号"];
        }
        set
        {
            this["BOSS二地图编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999")]
    public int BOSS二坐标X
    {
        get
        {
            return (int)this["BOSS二坐标X"];
        }
        set
        {
            this["BOSS二坐标X"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999")]
    public int BOSS二坐标Y
    {
        get
        {
            return (int)this["BOSS二坐标Y"];
        }
        set
        {
            this["BOSS二坐标Y"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("BOSS名称")]
    public string BOSS名字二
    {
        get
        {
            return (string)this["BOSS名字二"];
        }
        set
        {
            this["BOSS名字二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("地图名称")]
    public string BOSS二地图名字
    {
        get
        {
            return (string)this["BOSS二地图名字"];
        }
        set
        {
            this["BOSS二地图名字"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("20")]
    public byte BOSS三时间
    {
        get
        {
            return (byte)this["BOSS三时间"];
        }
        set
        {
            this["BOSS三时间"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("30")]
    public byte BOSS三分钟
    {
        get
        {
            return (byte)this["BOSS三分钟"];
        }
        set
        {
            this["BOSS三分钟"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int BOSS三地图编号
    {
        get
        {
            return (int)this["BOSS三地图编号"];
        }
        set
        {
            this["BOSS三地图编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999")]
    public int BOSS三坐标X
    {
        get
        {
            return (int)this["BOSS三坐标X"];
        }
        set
        {
            this["BOSS三坐标X"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999")]
    public int BOSS三坐标Y
    {
        get
        {
            return (int)this["BOSS三坐标Y"];
        }
        set
        {
            this["BOSS三坐标Y"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("BOSS名称")]
    public string BOSS名字三
    {
        get
        {
            return (string)this["BOSS名字三"];
        }
        set
        {
            this["BOSS名字三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("地图名称")]
    public string BOSS三地图名字
    {
        get
        {
            return (string)this["BOSS三地图名字"];
        }
        set
        {
            this["BOSS三地图名字"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("20")]
    public byte BOSS四时间
    {
        get
        {
            return (byte)this["BOSS四时间"];
        }
        set
        {
            this["BOSS四时间"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("30")]
    public byte BOSS四分钟
    {
        get
        {
            return (byte)this["BOSS四分钟"];
        }
        set
        {
            this["BOSS四分钟"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int BOSS四地图编号
    {
        get
        {
            return (int)this["BOSS四地图编号"];
        }
        set
        {
            this["BOSS四地图编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999")]
    public int BOSS四坐标X
    {
        get
        {
            return (int)this["BOSS四坐标X"];
        }
        set
        {
            this["BOSS四坐标X"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999")]
    public int BOSS四坐标Y
    {
        get
        {
            return (int)this["BOSS四坐标Y"];
        }
        set
        {
            this["BOSS四坐标Y"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("BOSS名称")]
    public string BOSS名字四
    {
        get
        {
            return (string)this["BOSS名字四"];
        }
        set
        {
            this["BOSS名字四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("地图名称")]
    public string BOSS四地图名字
    {
        get
        {
            return (string)this["BOSS四地图名字"];
        }
        set
        {
            this["BOSS四地图名字"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("20")]
    public byte BOSS五时间
    {
        get
        {
            return (byte)this["BOSS五时间"];
        }
        set
        {
            this["BOSS五时间"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("30")]
    public byte BOSS五分钟
    {
        get
        {
            return (byte)this["BOSS五分钟"];
        }
        set
        {
            this["BOSS五分钟"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int BOSS五地图编号
    {
        get
        {
            return (int)this["BOSS五地图编号"];
        }
        set
        {
            this["BOSS五地图编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999")]
    public int BOSS五坐标X
    {
        get
        {
            return (int)this["BOSS五坐标X"];
        }
        set
        {
            this["BOSS五坐标X"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999")]
    public int BOSS五坐标Y
    {
        get
        {
            return (int)this["BOSS五坐标Y"];
        }
        set
        {
            this["BOSS五坐标Y"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("BOSS名称")]
    public string BOSS名字五
    {
        get
        {
            return (string)this["BOSS名字五"];
        }
        set
        {
            this["BOSS名字五"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("地图名称")]
    public string BOSS五地图名字
    {
        get
        {
            return (string)this["BOSS五地图名字"];
        }
        set
        {
            this["BOSS五地图名字"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 资源包开关
    {
        get
        {
            return (int)this["资源包开关"];
        }
        set
        {
            this["资源包开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public byte StartingLevel
    {
        get
        {
            return (byte)this["StartingLevel"];
        }
        set
        {
            this["StartingLevel"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("屠魔令")]
    public string 城主分解物品一
    {
        get
        {
            return (string)this["城主分解物品一"];
        }
        set
        {
            this["城主分解物品一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("天书残页")]
    public string 城主分解物品二
    {
        get
        {
            return (string)this["城主分解物品二"];
        }
        set
        {
            this["城主分解物品二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("装备碎片")]
    public string 城主分解物品三
    {
        get
        {
            return (string)this["城主分解物品三"];
        }
        set
        {
            this["城主分解物品三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("龍纹兑换石")]
    public string 城主分解物品四
    {
        get
        {
            return (string)this["城主分解物品四"];
        }
        set
        {
            this["城主分解物品四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("70")]
    public int 城主分解几率一
    {
        get
        {
            return (int)this["城主分解几率一"];
        }
        set
        {
            this["城主分解几率一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("85")]
    public int 城主分解几率二
    {
        get
        {
            return (int)this["城主分解几率二"];
        }
        set
        {
            this["城主分解几率二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("95")]
    public int 城主分解几率三
    {
        get
        {
            return (int)this["城主分解几率三"];
        }
        set
        {
            this["城主分解几率三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 城主分解几率四
    {
        get
        {
            return (int)this["城主分解几率四"];
        }
        set
        {
            this["城主分解几率四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 城主分解数量一
    {
        get
        {
            return (int)this["城主分解数量一"];
        }
        set
        {
            this["城主分解数量一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 城主分解数量二
    {
        get
        {
            return (int)this["城主分解数量二"];
        }
        set
        {
            this["城主分解数量二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 城主分解数量三
    {
        get
        {
            return (int)this["城主分解数量三"];
        }
        set
        {
            this["城主分解数量三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 城主分解数量四
    {
        get
        {
            return (int)this["城主分解数量四"];
        }
        set
        {
            this["城主分解数量四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("3")]
    public int 城主分解开关
    {
        get
        {
            return (int)this["城主分解开关"];
        }
        set
        {
            this["城主分解开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("屠魔令")]
    public string 星王分解物品一
    {
        get
        {
            return (string)this["星王分解物品一"];
        }
        set
        {
            this["星王分解物品一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("天书残页")]
    public string 星王分解物品二
    {
        get
        {
            return (string)this["星王分解物品二"];
        }
        set
        {
            this["星王分解物品二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("装备碎片")]
    public string 星王分解物品三
    {
        get
        {
            return (string)this["星王分解物品三"];
        }
        set
        {
            this["星王分解物品三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("龍纹兑换石")]
    public string 星王分解物品四
    {
        get
        {
            return (string)this["星王分解物品四"];
        }
        set
        {
            this["星王分解物品四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("70")]
    public int 星王分解几率一
    {
        get
        {
            return (int)this["星王分解几率一"];
        }
        set
        {
            this["星王分解几率一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("85")]
    public int 星王分解几率二
    {
        get
        {
            return (int)this["星王分解几率二"];
        }
        set
        {
            this["星王分解几率二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("95")]
    public int 星王分解几率三
    {
        get
        {
            return (int)this["星王分解几率三"];
        }
        set
        {
            this["星王分解几率三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 星王分解几率四
    {
        get
        {
            return (int)this["星王分解几率四"];
        }
        set
        {
            this["星王分解几率四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 星王分解数量一
    {
        get
        {
            return (int)this["星王分解数量一"];
        }
        set
        {
            this["星王分解数量一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 星王分解数量二
    {
        get
        {
            return (int)this["星王分解数量二"];
        }
        set
        {
            this["星王分解数量二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 星王分解数量三
    {
        get
        {
            return (int)this["星王分解数量三"];
        }
        set
        {
            this["星王分解数量三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 星王分解数量四
    {
        get
        {
            return (int)this["星王分解数量四"];
        }
        set
        {
            this["星王分解数量四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("3")]
    public int 星王分解开关
    {
        get
        {
            return (int)this["星王分解开关"];
        }
        set
        {
            this["星王分解开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("屠魔令")]
    public string 苍月分解物品一
    {
        get
        {
            return (string)this["苍月分解物品一"];
        }
        set
        {
            this["苍月分解物品一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("天书残页")]
    public string 苍月分解物品二
    {
        get
        {
            return (string)this["苍月分解物品二"];
        }
        set
        {
            this["苍月分解物品二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("装备碎片")]
    public string 苍月分解物品三
    {
        get
        {
            return (string)this["苍月分解物品三"];
        }
        set
        {
            this["苍月分解物品三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("龍纹兑换石")]
    public string 苍月分解物品四
    {
        get
        {
            return (string)this["苍月分解物品四"];
        }
        set
        {
            this["苍月分解物品四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("70")]
    public int 苍月分解几率一
    {
        get
        {
            return (int)this["苍月分解几率一"];
        }
        set
        {
            this["苍月分解几率一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("85")]
    public int 苍月分解几率二
    {
        get
        {
            return (int)this["苍月分解几率二"];
        }
        set
        {
            this["苍月分解几率二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("95")]
    public int 苍月分解几率三
    {
        get
        {
            return (int)this["苍月分解几率三"];
        }
        set
        {
            this["苍月分解几率三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 苍月分解几率四
    {
        get
        {
            return (int)this["苍月分解几率四"];
        }
        set
        {
            this["苍月分解几率四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 苍月分解数量一
    {
        get
        {
            return (int)this["苍月分解数量一"];
        }
        set
        {
            this["苍月分解数量一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 苍月分解数量二
    {
        get
        {
            return (int)this["苍月分解数量二"];
        }
        set
        {
            this["苍月分解数量二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 苍月分解数量三
    {
        get
        {
            return (int)this["苍月分解数量三"];
        }
        set
        {
            this["苍月分解数量三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 苍月分解数量四
    {
        get
        {
            return (int)this["苍月分解数量四"];
        }
        set
        {
            this["苍月分解数量四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("3")]
    public int 苍月分解开关
    {
        get
        {
            return (int)this["苍月分解开关"];
        }
        set
        {
            this["苍月分解开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("屠魔令")]
    public string 魔龙分解物品一
    {
        get
        {
            return (string)this["魔龙分解物品一"];
        }
        set
        {
            this["魔龙分解物品一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("天书残页")]
    public string 魔龙分解物品二
    {
        get
        {
            return (string)this["魔龙分解物品二"];
        }
        set
        {
            this["魔龙分解物品二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("装备碎片")]
    public string 魔龙分解物品三
    {
        get
        {
            return (string)this["魔龙分解物品三"];
        }
        set
        {
            this["魔龙分解物品三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("龍纹兑换石")]
    public string 魔龙分解物品四
    {
        get
        {
            return (string)this["魔龙分解物品四"];
        }
        set
        {
            this["魔龙分解物品四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("70")]
    public int 魔龙分解几率一
    {
        get
        {
            return (int)this["魔龙分解几率一"];
        }
        set
        {
            this["魔龙分解几率一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("85")]
    public int 魔龙分解几率二
    {
        get
        {
            return (int)this["魔龙分解几率二"];
        }
        set
        {
            this["魔龙分解几率二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("95")]
    public int 魔龙分解几率三
    {
        get
        {
            return (int)this["魔龙分解几率三"];
        }
        set
        {
            this["魔龙分解几率三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 魔龙分解几率四
    {
        get
        {
            return (int)this["魔龙分解几率四"];
        }
        set
        {
            this["魔龙分解几率四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 魔龙分解数量一
    {
        get
        {
            return (int)this["魔龙分解数量一"];
        }
        set
        {
            this["魔龙分解数量一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 魔龙分解数量二
    {
        get
        {
            return (int)this["魔龙分解数量二"];
        }
        set
        {
            this["魔龙分解数量二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 魔龙分解数量三
    {
        get
        {
            return (int)this["魔龙分解数量三"];
        }
        set
        {
            this["魔龙分解数量三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 魔龙分解数量四
    {
        get
        {
            return (int)this["魔龙分解数量四"];
        }
        set
        {
            this["魔龙分解数量四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("3")]
    public int 魔龙分解开关
    {
        get
        {
            return (int)this["魔龙分解开关"];
        }
        set
        {
            this["魔龙分解开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("屠魔令")]
    public string 赤月分解物品一
    {
        get
        {
            return (string)this["赤月分解物品一"];
        }
        set
        {
            this["赤月分解物品一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("天书残页")]
    public string 赤月分解物品二
    {
        get
        {
            return (string)this["赤月分解物品二"];
        }
        set
        {
            this["赤月分解物品二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("装备碎片")]
    public string 赤月分解物品三
    {
        get
        {
            return (string)this["赤月分解物品三"];
        }
        set
        {
            this["赤月分解物品三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("龍纹兑换石")]
    public string 赤月分解物品四
    {
        get
        {
            return (string)this["赤月分解物品四"];
        }
        set
        {
            this["赤月分解物品四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("70")]
    public int 赤月分解几率一
    {
        get
        {
            return (int)this["赤月分解几率一"];
        }
        set
        {
            this["赤月分解几率一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("85")]
    public int 赤月分解几率二
    {
        get
        {
            return (int)this["赤月分解几率二"];
        }
        set
        {
            this["赤月分解几率二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("95")]
    public int 赤月分解几率三
    {
        get
        {
            return (int)this["赤月分解几率三"];
        }
        set
        {
            this["赤月分解几率三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 赤月分解几率四
    {
        get
        {
            return (int)this["赤月分解几率四"];
        }
        set
        {
            this["赤月分解几率四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 赤月分解数量一
    {
        get
        {
            return (int)this["赤月分解数量一"];
        }
        set
        {
            this["赤月分解数量一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 赤月分解数量二
    {
        get
        {
            return (int)this["赤月分解数量二"];
        }
        set
        {
            this["赤月分解数量二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 赤月分解数量三
    {
        get
        {
            return (int)this["赤月分解数量三"];
        }
        set
        {
            this["赤月分解数量三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 赤月分解数量四
    {
        get
        {
            return (int)this["赤月分解数量四"];
        }
        set
        {
            this["赤月分解数量四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("3")]
    public int 赤月分解开关
    {
        get
        {
            return (int)this["赤月分解开关"];
        }
        set
        {
            this["赤月分解开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("屠魔令")]
    public string 祖玛分解物品一
    {
        get
        {
            return (string)this["祖玛分解物品一"];
        }
        set
        {
            this["祖玛分解物品一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("天书残页")]
    public string 祖玛分解物品二
    {
        get
        {
            return (string)this["祖玛分解物品二"];
        }
        set
        {
            this["祖玛分解物品二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("装备碎片")]
    public string 祖玛分解物品三
    {
        get
        {
            return (string)this["祖玛分解物品三"];
        }
        set
        {
            this["祖玛分解物品三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("龍纹兑换石")]
    public string 祖玛分解物品四
    {
        get
        {
            return (string)this["祖玛分解物品四"];
        }
        set
        {
            this["祖玛分解物品四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("70")]
    public int 祖玛分解几率一
    {
        get
        {
            return (int)this["祖玛分解几率一"];
        }
        set
        {
            this["祖玛分解几率一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("85")]
    public int 祖玛分解几率二
    {
        get
        {
            return (int)this["祖玛分解几率二"];
        }
        set
        {
            this["祖玛分解几率二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("95")]
    public int 祖玛分解几率三
    {
        get
        {
            return (int)this["祖玛分解几率三"];
        }
        set
        {
            this["祖玛分解几率三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 祖玛分解几率四
    {
        get
        {
            return (int)this["祖玛分解几率四"];
        }
        set
        {
            this["祖玛分解几率四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 沙巴克重置系统
    {
        get
        {
            return (int)this["沙巴克重置系统"];
        }
        set
        {
            this["沙巴克重置系统"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 祖玛分解数量一
    {
        get
        {
            return (int)this["祖玛分解数量一"];
        }
        set
        {
            this["祖玛分解数量一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 祖玛分解数量二
    {
        get
        {
            return (int)this["祖玛分解数量二"];
        }
        set
        {
            this["祖玛分解数量二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 祖玛分解数量三
    {
        get
        {
            return (int)this["祖玛分解数量三"];
        }
        set
        {
            this["祖玛分解数量三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 祖玛分解数量四
    {
        get
        {
            return (int)this["祖玛分解数量四"];
        }
        set
        {
            this["祖玛分解数量四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("3")]
    public int 祖玛分解开关
    {
        get
        {
            return (int)this["祖玛分解开关"];
        }
        set
        {
            this["祖玛分解开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int BOSS卷轴地图编号
    {
        get
        {
            return (int)this["BOSS卷轴地图编号"];
        }
        set
        {
            this["BOSS卷轴地图编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int BOSS卷轴地图开关
    {
        get
        {
            return (int)this["BOSS卷轴地图开关"];
        }
        set
        {
            this["BOSS卷轴地图开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("血手")]
    public string BOSS卷轴怪物四
    {
        get
        {
            return (string)this["BOSS卷轴怪物四"];
        }
        set
        {
            this["BOSS卷轴怪物四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string BOSS卷轴怪物一
    {
        get
        {
            return (string)this["BOSS卷轴怪物一"];
        }
        set
        {
            this["BOSS卷轴怪物一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("黄泉教主")]
    public string BOSS卷轴怪物二
    {
        get
        {
            return (string)this["BOSS卷轴怪物二"];
        }
        set
        {
            this["BOSS卷轴怪物二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("魔龙教主")]
    public string BOSS卷轴怪物三
    {
        get
        {
            return (string)this["BOSS卷轴怪物三"];
        }
        set
        {
            this["BOSS卷轴怪物三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("魔火龙")]
    public string BOSS卷轴怪物五
    {
        get
        {
            return (string)this["BOSS卷轴怪物五"];
        }
        set
        {
            this["BOSS卷轴怪物五"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("赤月恶魔")]
    public string BOSS卷轴怪物六
    {
        get
        {
            return (string)this["BOSS卷轴怪物六"];
        }
        set
        {
            this["BOSS卷轴怪物六"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string BOSS卷轴怪物七
    {
        get
        {
            return (string)this["BOSS卷轴怪物七"];
        }
        set
        {
            this["BOSS卷轴怪物七"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string BOSS卷轴怪物八
    {
        get
        {
            return (string)this["BOSS卷轴怪物八"];
        }
        set
        {
            this["BOSS卷轴怪物八"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string BOSS卷轴怪物九
    {
        get
        {
            return (string)this["BOSS卷轴怪物九"];
        }
        set
        {
            this["BOSS卷轴怪物九"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string BOSS卷轴怪物十
    {
        get
        {
            return (string)this["BOSS卷轴怪物十"];
        }
        set
        {
            this["BOSS卷轴怪物十"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string BOSS卷轴怪物11
    {
        get
        {
            return (string)this["BOSS卷轴怪物11"];
        }
        set
        {
            this["BOSS卷轴怪物11"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string BOSS卷轴怪物12
    {
        get
        {
            return (string)this["BOSS卷轴怪物12"];
        }
        set
        {
            this["BOSS卷轴怪物12"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string BOSS卷轴怪物13
    {
        get
        {
            return (string)this["BOSS卷轴怪物13"];
        }
        set
        {
            this["BOSS卷轴怪物13"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string BOSS卷轴怪物14
    {
        get
        {
            return (string)this["BOSS卷轴怪物14"];
        }
        set
        {
            this["BOSS卷轴怪物14"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string BOSS卷轴怪物15
    {
        get
        {
            return (string)this["BOSS卷轴怪物15"];
        }
        set
        {
            this["BOSS卷轴怪物15"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string BOSS卷轴怪物16
    {
        get
        {
            return (string)this["BOSS卷轴怪物16"];
        }
        set
        {
            this["BOSS卷轴怪物16"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 未知暗点副本价格
    {
        get
        {
            return (int)this["未知暗点副本价格"];
        }
        set
        {
            this["未知暗点副本价格"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 未知暗点二层价格
    {
        get
        {
            return (int)this["未知暗点二层价格"];
        }
        set
        {
            this["未知暗点二层价格"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 幽冥海副本价格
    {
        get
        {
            return (int)this["幽冥海副本价格"];
        }
        set
        {
            this["幽冥海副本价格"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("45")]
    public int 未知暗点副本等级
    {
        get
        {
            return (int)this["未知暗点副本等级"];
        }
        set
        {
            this["未知暗点副本等级"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("45")]
    public int 未知暗点二层等级
    {
        get
        {
            return (int)this["未知暗点二层等级"];
        }
        set
        {
            this["未知暗点二层等级"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("45")]
    public int 幽冥海副本等级
    {
        get
        {
            return (int)this["幽冥海副本等级"];
        }
        set
        {
            this["幽冥海副本等级"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 御兽切换开关
    {
        get
        {
            return (int)this["御兽切换开关"];
        }
        set
        {
            this["御兽切换开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("875000")]
    public int 每周特惠二物品1
    {
        get
        {
            return (int)this["每周特惠二物品1"];
        }
        set
        {
            this["每周特惠二物品1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2750000")]
    public int 每周特惠二物品2
    {
        get
        {
            return (int)this["每周特惠二物品2"];
        }
        set
        {
            this["每周特惠二物品2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1500130")]
    public int 每周特惠二物品3
    {
        get
        {
            return (int)this["每周特惠二物品3"];
        }
        set
        {
            this["每周特惠二物品3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("91156")]
    public int 每周特惠二物品4
    {
        get
        {
            return (int)this["每周特惠二物品4"];
        }
        set
        {
            this["每周特惠二物品4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("80200")]
    public int 每周特惠二物品5
    {
        get
        {
            return (int)this["每周特惠二物品5"];
        }
        set
        {
            this["每周特惠二物品5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("165000")]
    public int 每周特惠一物品1
    {
        get
        {
            return (int)this["每周特惠一物品1"];
        }
        set
        {
            this["每周特惠一物品1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("500000")]
    public int 每周特惠一物品2
    {
        get
        {
            return (int)this["每周特惠一物品2"];
        }
        set
        {
            this["每周特惠一物品2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1500230")]
    public int 每周特惠一物品3
    {
        get
        {
            return (int)this["每周特惠一物品3"];
        }
        set
        {
            this["每周特惠一物品3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("80203")]
    public int 每周特惠一物品4
    {
        get
        {
            return (int)this["每周特惠一物品4"];
        }
        set
        {
            this["每周特惠一物品4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("80007")]
    public int 每周特惠一物品5
    {
        get
        {
            return (int)this["每周特惠一物品5"];
        }
        set
        {
            this["每周特惠一物品5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 法阵卡BUG清理
    {
        get
        {
            return (int)this["法阵卡BUG清理"];
        }
        set
        {
            this["法阵卡BUG清理"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public ushort AbnormalBlockTime
    {
        get
        {
            return (ushort)this["AbnormalBlockTime"];
        }
        set
        {
            this["AbnormalBlockTime"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public ushort DisconnectTime
    {
        get
        {
            return (ushort)this["DisconnectTime"];
        }
        set
        {
            this["DisconnectTime"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("40")]
    public byte MaxUserLevel
    {
        get
        {
            return (byte)this["MaxUserLevel"];
        }
        set
        {
            this["MaxUserLevel"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public decimal SpecialRepairDiscount
    {
        get
        {
            return (decimal)this["SpecialRepairDiscount"];
        }
        set
        {
            this["SpecialRepairDiscount"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public decimal 怪物额外爆率
    {
        get
        {
            return (decimal)this["怪物额外爆率"];
        }
        set
        {
            this["怪物额外爆率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public decimal MonsterExperienceMultiplier
    {
        get
        {
            return (decimal)this["MonsterExperienceMultiplier"];
        }
        set
        {
            this["MonsterExperienceMultiplier"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public byte 减收益等级差
    {
        get
        {
            return (byte)this["减收益等级差"];
        }
        set
        {
            this["减收益等级差"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0.1")]
    public decimal 收益减少比率
    {
        get
        {
            return (decimal)this["收益减少比率"];
        }
        set
        {
            this["收益减少比率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("120")]
    public ushort 怪物诱惑时长
    {
        get
        {
            return (ushort)this["怪物诱惑时长"];
        }
        set
        {
            this["怪物诱惑时长"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public byte ItemDisappearTime
    {
        get
        {
            return (byte)this["ItemDisappearTime"];
        }
        set
        {
            this["ItemDisappearTime"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("3")]
    public byte 物品归属时间
    {
        get
        {
            return (byte)this["物品归属时间"];
        }
        set
        {
            this["物品归属时间"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("..\\Database")]
    public string GameDataPath
    {
        get
        {
            return (string)this["GameDataPath"];
        }
        set
        {
            this["GameDataPath"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("..\\文件")]
    public string 平台接入目录
    {
        get
        {
            return (string)this["平台接入目录"];
        }
        set
        {
            this["平台接入目录"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue(".\\Backup")]
    public string DataBackupPath
    {
        get
        {
            return (string)this["DataBackupPath"];
        }
        set
        {
            this["DataBackupPath"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("")]
    public string 系统公告内容
    {
        get
        {
            return (string)this["系统公告内容"];
        }
        set
        {
            this["系统公告内容"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte NoobSupportLevel
    {
        get
        {
            return (byte)this["NoobSupportLevel"];
        }
        set
        {
            this["NoobSupportLevel"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 分解称号选项
    {
        get
        {
            return (byte)this["分解称号选项"];
        }
        set
        {
            this["分解称号选项"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 挂机称号选项
    {
        get
        {
            return (byte)this["挂机称号选项"];
        }
        set
        {
            this["挂机称号选项"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 新手出售货币值
    {
        get
        {
            return (int)this["新手出售货币值"];
        }
        set
        {
            this["新手出售货币值"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱一几率8
    {
        get
        {
            return (int)this["随机宝箱一几率8"];
        }
        set
        {
            this["随机宝箱一几率8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱一物品8
    {
        get
        {
            return (int)this["随机宝箱一物品8"];
        }
        set
        {
            this["随机宝箱一物品8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱一几率7
    {
        get
        {
            return (int)this["随机宝箱一几率7"];
        }
        set
        {
            this["随机宝箱一几率7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱一物品7
    {
        get
        {
            return (int)this["随机宝箱一物品7"];
        }
        set
        {
            this["随机宝箱一物品7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱一几率6
    {
        get
        {
            return (int)this["随机宝箱一几率6"];
        }
        set
        {
            this["随机宝箱一几率6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱一物品6
    {
        get
        {
            return (int)this["随机宝箱一物品6"];
        }
        set
        {
            this["随机宝箱一物品6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱一几率5
    {
        get
        {
            return (int)this["随机宝箱一几率5"];
        }
        set
        {
            this["随机宝箱一几率5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱一物品5
    {
        get
        {
            return (int)this["随机宝箱一物品5"];
        }
        set
        {
            this["随机宝箱一物品5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱一几率4
    {
        get
        {
            return (int)this["随机宝箱一几率4"];
        }
        set
        {
            this["随机宝箱一几率4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱一物品4
    {
        get
        {
            return (int)this["随机宝箱一物品4"];
        }
        set
        {
            this["随机宝箱一物品4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱一几率3
    {
        get
        {
            return (int)this["随机宝箱一几率3"];
        }
        set
        {
            this["随机宝箱一几率3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱一物品3
    {
        get
        {
            return (int)this["随机宝箱一物品3"];
        }
        set
        {
            this["随机宝箱一物品3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱一几率2
    {
        get
        {
            return (int)this["随机宝箱一几率2"];
        }
        set
        {
            this["随机宝箱一几率2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱一物品2
    {
        get
        {
            return (int)this["随机宝箱一物品2"];
        }
        set
        {
            this["随机宝箱一物品2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱一几率1
    {
        get
        {
            return (int)this["随机宝箱一几率1"];
        }
        set
        {
            this["随机宝箱一几率1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱一物品1
    {
        get
        {
            return (int)this["随机宝箱一物品1"];
        }
        set
        {
            this["随机宝箱一物品1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱二几率8
    {
        get
        {
            return (int)this["随机宝箱二几率8"];
        }
        set
        {
            this["随机宝箱二几率8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱二物品8
    {
        get
        {
            return (int)this["随机宝箱二物品8"];
        }
        set
        {
            this["随机宝箱二物品8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱二几率7
    {
        get
        {
            return (int)this["随机宝箱二几率7"];
        }
        set
        {
            this["随机宝箱二几率7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱二物品7
    {
        get
        {
            return (int)this["随机宝箱二物品7"];
        }
        set
        {
            this["随机宝箱二物品7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱二几率6
    {
        get
        {
            return (int)this["随机宝箱二几率6"];
        }
        set
        {
            this["随机宝箱二几率6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱二物品6
    {
        get
        {
            return (int)this["随机宝箱二物品6"];
        }
        set
        {
            this["随机宝箱二物品6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱二几率5
    {
        get
        {
            return (int)this["随机宝箱二几率5"];
        }
        set
        {
            this["随机宝箱二几率5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱二物品5
    {
        get
        {
            return (int)this["随机宝箱二物品5"];
        }
        set
        {
            this["随机宝箱二物品5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱二几率4
    {
        get
        {
            return (int)this["随机宝箱二几率4"];
        }
        set
        {
            this["随机宝箱二几率4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱二物品4
    {
        get
        {
            return (int)this["随机宝箱二物品4"];
        }
        set
        {
            this["随机宝箱二物品4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱二几率3
    {
        get
        {
            return (int)this["随机宝箱二几率3"];
        }
        set
        {
            this["随机宝箱二几率3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱二物品3
    {
        get
        {
            return (int)this["随机宝箱二物品3"];
        }
        set
        {
            this["随机宝箱二物品3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱二几率2
    {
        get
        {
            return (int)this["随机宝箱二几率2"];
        }
        set
        {
            this["随机宝箱二几率2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱二物品2
    {
        get
        {
            return (int)this["随机宝箱二物品2"];
        }
        set
        {
            this["随机宝箱二物品2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱二几率1
    {
        get
        {
            return (int)this["随机宝箱二几率1"];
        }
        set
        {
            this["随机宝箱二几率1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱二物品1
    {
        get
        {
            return (int)this["随机宝箱二物品1"];
        }
        set
        {
            this["随机宝箱二物品1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱三几率8
    {
        get
        {
            return (int)this["随机宝箱三几率8"];
        }
        set
        {
            this["随机宝箱三几率8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱三物品8
    {
        get
        {
            return (int)this["随机宝箱三物品8"];
        }
        set
        {
            this["随机宝箱三物品8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱三几率7
    {
        get
        {
            return (int)this["随机宝箱三几率7"];
        }
        set
        {
            this["随机宝箱三几率7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱三物品7
    {
        get
        {
            return (int)this["随机宝箱三物品7"];
        }
        set
        {
            this["随机宝箱三物品7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱三几率6
    {
        get
        {
            return (int)this["随机宝箱三几率6"];
        }
        set
        {
            this["随机宝箱三几率6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱三物品6
    {
        get
        {
            return (int)this["随机宝箱三物品6"];
        }
        set
        {
            this["随机宝箱三物品6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱三几率5
    {
        get
        {
            return (int)this["随机宝箱三几率5"];
        }
        set
        {
            this["随机宝箱三几率5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱三物品5
    {
        get
        {
            return (int)this["随机宝箱三物品5"];
        }
        set
        {
            this["随机宝箱三物品5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱三几率4
    {
        get
        {
            return (int)this["随机宝箱三几率4"];
        }
        set
        {
            this["随机宝箱三几率4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱三物品4
    {
        get
        {
            return (int)this["随机宝箱三物品4"];
        }
        set
        {
            this["随机宝箱三物品4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱三几率3
    {
        get
        {
            return (int)this["随机宝箱三几率3"];
        }
        set
        {
            this["随机宝箱三几率3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱三物品3
    {
        get
        {
            return (int)this["随机宝箱三物品3"];
        }
        set
        {
            this["随机宝箱三物品3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱三几率2
    {
        get
        {
            return (int)this["随机宝箱三几率2"];
        }
        set
        {
            this["随机宝箱三几率2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱三物品2
    {
        get
        {
            return (int)this["随机宝箱三物品2"];
        }
        set
        {
            this["随机宝箱三物品2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 随机宝箱三几率1
    {
        get
        {
            return (int)this["随机宝箱三几率1"];
        }
        set
        {
            this["随机宝箱三几率1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱三物品1
    {
        get
        {
            return (int)this["随机宝箱三物品1"];
        }
        set
        {
            this["随机宝箱三物品1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱三数量8
    {
        get
        {
            return (int)this["随机宝箱三数量8"];
        }
        set
        {
            this["随机宝箱三数量8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱三数量7
    {
        get
        {
            return (int)this["随机宝箱三数量7"];
        }
        set
        {
            this["随机宝箱三数量7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱三数量6
    {
        get
        {
            return (int)this["随机宝箱三数量6"];
        }
        set
        {
            this["随机宝箱三数量6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱三数量5
    {
        get
        {
            return (int)this["随机宝箱三数量5"];
        }
        set
        {
            this["随机宝箱三数量5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱三数量4
    {
        get
        {
            return (int)this["随机宝箱三数量4"];
        }
        set
        {
            this["随机宝箱三数量4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱三数量3
    {
        get
        {
            return (int)this["随机宝箱三数量3"];
        }
        set
        {
            this["随机宝箱三数量3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱三数量2
    {
        get
        {
            return (int)this["随机宝箱三数量2"];
        }
        set
        {
            this["随机宝箱三数量2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱三数量1
    {
        get
        {
            return (int)this["随机宝箱三数量1"];
        }
        set
        {
            this["随机宝箱三数量1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱二数量8
    {
        get
        {
            return (int)this["随机宝箱二数量8"];
        }
        set
        {
            this["随机宝箱二数量8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱二数量7
    {
        get
        {
            return (int)this["随机宝箱二数量7"];
        }
        set
        {
            this["随机宝箱二数量7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱二数量6
    {
        get
        {
            return (int)this["随机宝箱二数量6"];
        }
        set
        {
            this["随机宝箱二数量6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱二数量5
    {
        get
        {
            return (int)this["随机宝箱二数量5"];
        }
        set
        {
            this["随机宝箱二数量5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱二数量4
    {
        get
        {
            return (int)this["随机宝箱二数量4"];
        }
        set
        {
            this["随机宝箱二数量4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱二数量3
    {
        get
        {
            return (int)this["随机宝箱二数量3"];
        }
        set
        {
            this["随机宝箱二数量3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱二数量2
    {
        get
        {
            return (int)this["随机宝箱二数量2"];
        }
        set
        {
            this["随机宝箱二数量2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱二数量1
    {
        get
        {
            return (int)this["随机宝箱二数量1"];
        }
        set
        {
            this["随机宝箱二数量1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱一数量8
    {
        get
        {
            return (int)this["随机宝箱一数量8"];
        }
        set
        {
            this["随机宝箱一数量8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱一数量7
    {
        get
        {
            return (int)this["随机宝箱一数量7"];
        }
        set
        {
            this["随机宝箱一数量7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱一数量6
    {
        get
        {
            return (int)this["随机宝箱一数量6"];
        }
        set
        {
            this["随机宝箱一数量6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱一数量5
    {
        get
        {
            return (int)this["随机宝箱一数量5"];
        }
        set
        {
            this["随机宝箱一数量5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱一数量4
    {
        get
        {
            return (int)this["随机宝箱一数量4"];
        }
        set
        {
            this["随机宝箱一数量4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱一数量3
    {
        get
        {
            return (int)this["随机宝箱一数量3"];
        }
        set
        {
            this["随机宝箱一数量3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱一数量2
    {
        get
        {
            return (int)this["随机宝箱一数量2"];
        }
        set
        {
            this["随机宝箱一数量2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 随机宝箱一数量1
    {
        get
        {
            return (int)this["随机宝箱一数量1"];
        }
        set
        {
            this["随机宝箱一数量1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("45")]
    public int 沙城地图保护
    {
        get
        {
            return (int)this["沙城地图保护"];
        }
        set
        {
            this["沙城地图保护"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("15")]
    public int NoobProtectionLevel
    {
        get
        {
            return (int)this["NoobProtectionLevel"];
        }
        set
        {
            this["NoobProtectionLevel"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 新手地图保护1
    {
        get
        {
            return (int)this["新手地图保护1"];
        }
        set
        {
            this["新手地图保护1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 新手地图保护2
    {
        get
        {
            return (int)this["新手地图保护2"];
        }
        set
        {
            this["新手地图保护2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 新手地图保护3
    {
        get
        {
            return (int)this["新手地图保护3"];
        }
        set
        {
            this["新手地图保护3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 新手地图保护4
    {
        get
        {
            return (int)this["新手地图保护4"];
        }
        set
        {
            this["新手地图保护4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 新手地图保护5
    {
        get
        {
            return (int)this["新手地图保护5"];
        }
        set
        {
            this["新手地图保护5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 新手地图保护6
    {
        get
        {
            return (int)this["新手地图保护6"];
        }
        set
        {
            this["新手地图保护6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 新手地图保护7
    {
        get
        {
            return (int)this["新手地图保护7"];
        }
        set
        {
            this["新手地图保护7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 新手地图保护8
    {
        get
        {
            return (int)this["新手地图保护8"];
        }
        set
        {
            this["新手地图保护8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 新手地图保护9
    {
        get
        {
            return (int)this["新手地图保护9"];
        }
        set
        {
            this["新手地图保护9"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 新手地图保护10
    {
        get
        {
            return (int)this["新手地图保护10"];
        }
        set
        {
            this["新手地图保护10"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 沙巴克停止开关
    {
        get
        {
            return (int)this["沙巴克停止开关"];
        }
        set
        {
            this["沙巴克停止开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 沙巴克称号领取开关
    {
        get
        {
            return (int)this["沙巴克称号领取开关"];
        }
        set
        {
            this["沙巴克称号领取开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("144")]
    public byte 沙巴克城主称号
    {
        get
        {
            return (byte)this["沙巴克城主称号"];
        }
        set
        {
            this["沙巴克城主称号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("145")]
    public byte 沙巴克成员称号
    {
        get
        {
            return (byte)this["沙巴克成员称号"];
        }
        set
        {
            this["沙巴克成员称号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("23")]
    public int 重置屠魔副本时间
    {
        get
        {
            return (int)this["重置屠魔副本时间"];
        }
        set
        {
            this["重置屠魔副本时间"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 通用6装备佩戴数量
    {
        get
        {
            return (int)this["通用6装备佩戴数量"];
        }
        set
        {
            this["通用6装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 通用5装备佩戴数量
    {
        get
        {
            return (int)this["通用5装备佩戴数量"];
        }
        set
        {
            this["通用5装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 通用4装备佩戴数量
    {
        get
        {
            return (int)this["通用4装备佩戴数量"];
        }
        set
        {
            this["通用4装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 通用3装备佩戴数量
    {
        get
        {
            return (int)this["通用3装备佩戴数量"];
        }
        set
        {
            this["通用3装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 通用2装备佩戴数量
    {
        get
        {
            return (int)this["通用2装备佩戴数量"];
        }
        set
        {
            this["通用2装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 通用1装备佩戴数量
    {
        get
        {
            return (int)this["通用1装备佩戴数量"];
        }
        set
        {
            this["通用1装备佩戴数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 屠魔令回收数量
    {
        get
        {
            return (int)this["屠魔令回收数量"];
        }
        set
        {
            this["屠魔令回收数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("500000")]
    public int 元宝袋新创数量1
    {
        get
        {
            return (int)this["元宝袋新创数量1"];
        }
        set
        {
            this["元宝袋新创数量1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000000")]
    public int 元宝袋新创数量2
    {
        get
        {
            return (int)this["元宝袋新创数量2"];
        }
        set
        {
            this["元宝袋新创数量2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5000000")]
    public int 元宝袋新创数量3
    {
        get
        {
            return (int)this["元宝袋新创数量3"];
        }
        set
        {
            this["元宝袋新创数量3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10000000")]
    public int 元宝袋新创数量4
    {
        get
        {
            return (int)this["元宝袋新创数量4"];
        }
        set
        {
            this["元宝袋新创数量4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("50000000")]
    public int 元宝袋新创数量5
    {
        get
        {
            return (int)this["元宝袋新创数量5"];
        }
        set
        {
            this["元宝袋新创数量5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 新手上线赠送开关
    {
        get
        {
            return (int)this["新手上线赠送开关"];
        }
        set
        {
            this["新手上线赠送开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 新手上线赠送物品1
    {
        get
        {
            return (int)this["新手上线赠送物品1"];
        }
        set
        {
            this["新手上线赠送物品1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 新手上线赠送物品2
    {
        get
        {
            return (int)this["新手上线赠送物品2"];
        }
        set
        {
            this["新手上线赠送物品2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 新手上线赠送物品3
    {
        get
        {
            return (int)this["新手上线赠送物品3"];
        }
        set
        {
            this["新手上线赠送物品3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 新手上线赠送物品4
    {
        get
        {
            return (int)this["新手上线赠送物品4"];
        }
        set
        {
            this["新手上线赠送物品4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 新手上线赠送物品5
    {
        get
        {
            return (int)this["新手上线赠送物品5"];
        }
        set
        {
            this["新手上线赠送物品5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 新手上线赠送物品6
    {
        get
        {
            return (int)this["新手上线赠送物品6"];
        }
        set
        {
            this["新手上线赠送物品6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 新手上线赠送称号1
    {
        get
        {
            return (int)this["新手上线赠送称号1"];
        }
        set
        {
            this["新手上线赠送称号1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 初级赞助礼包1
    {
        get
        {
            return (int)this["初级赞助礼包1"];
        }
        set
        {
            this["初级赞助礼包1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 初级赞助礼包2
    {
        get
        {
            return (int)this["初级赞助礼包2"];
        }
        set
        {
            this["初级赞助礼包2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 初级赞助礼包3
    {
        get
        {
            return (int)this["初级赞助礼包3"];
        }
        set
        {
            this["初级赞助礼包3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 初级赞助礼包4
    {
        get
        {
            return (int)this["初级赞助礼包4"];
        }
        set
        {
            this["初级赞助礼包4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 初级赞助礼包5
    {
        get
        {
            return (int)this["初级赞助礼包5"];
        }
        set
        {
            this["初级赞助礼包5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 初级赞助礼包6
    {
        get
        {
            return (int)this["初级赞助礼包6"];
        }
        set
        {
            this["初级赞助礼包6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 初级赞助礼包7
    {
        get
        {
            return (int)this["初级赞助礼包7"];
        }
        set
        {
            this["初级赞助礼包7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 初级赞助礼包8
    {
        get
        {
            return (int)this["初级赞助礼包8"];
        }
        set
        {
            this["初级赞助礼包8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 初级赞助称号1
    {
        get
        {
            return (int)this["初级赞助称号1"];
        }
        set
        {
            this["初级赞助称号1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 中级赞助礼包1
    {
        get
        {
            return (int)this["中级赞助礼包1"];
        }
        set
        {
            this["中级赞助礼包1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 中级赞助礼包2
    {
        get
        {
            return (int)this["中级赞助礼包2"];
        }
        set
        {
            this["中级赞助礼包2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 中级赞助礼包3
    {
        get
        {
            return (int)this["中级赞助礼包3"];
        }
        set
        {
            this["中级赞助礼包3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 中级赞助礼包4
    {
        get
        {
            return (int)this["中级赞助礼包4"];
        }
        set
        {
            this["中级赞助礼包4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 中级赞助礼包5
    {
        get
        {
            return (int)this["中级赞助礼包5"];
        }
        set
        {
            this["中级赞助礼包5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 中级赞助礼包6
    {
        get
        {
            return (int)this["中级赞助礼包6"];
        }
        set
        {
            this["中级赞助礼包6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 中级赞助礼包7
    {
        get
        {
            return (int)this["中级赞助礼包7"];
        }
        set
        {
            this["中级赞助礼包7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 中级赞助礼包8
    {
        get
        {
            return (int)this["中级赞助礼包8"];
        }
        set
        {
            this["中级赞助礼包8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 中级赞助称号1
    {
        get
        {
            return (int)this["中级赞助称号1"];
        }
        set
        {
            this["中级赞助称号1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 高级赞助礼包1
    {
        get
        {
            return (int)this["高级赞助礼包1"];
        }
        set
        {
            this["高级赞助礼包1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 高级赞助礼包2
    {
        get
        {
            return (int)this["高级赞助礼包2"];
        }
        set
        {
            this["高级赞助礼包2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 高级赞助礼包3
    {
        get
        {
            return (int)this["高级赞助礼包3"];
        }
        set
        {
            this["高级赞助礼包3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 高级赞助礼包4
    {
        get
        {
            return (int)this["高级赞助礼包4"];
        }
        set
        {
            this["高级赞助礼包4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 高级赞助礼包5
    {
        get
        {
            return (int)this["高级赞助礼包5"];
        }
        set
        {
            this["高级赞助礼包5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 高级赞助礼包6
    {
        get
        {
            return (int)this["高级赞助礼包6"];
        }
        set
        {
            this["高级赞助礼包6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 高级赞助礼包7
    {
        get
        {
            return (int)this["高级赞助礼包7"];
        }
        set
        {
            this["高级赞助礼包7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 高级赞助礼包8
    {
        get
        {
            return (int)this["高级赞助礼包8"];
        }
        set
        {
            this["高级赞助礼包8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 高级赞助称号1
    {
        get
        {
            return (int)this["高级赞助称号1"];
        }
        set
        {
            this["高级赞助称号1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 自动BOSS1界面1开关
    {
        get
        {
            return (int)this["自动BOSS1界面1开关"];
        }
        set
        {
            this["自动BOSS1界面1开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 自动BOSS1界面2开关
    {
        get
        {
            return (int)this["自动BOSS1界面2开关"];
        }
        set
        {
            this["自动BOSS1界面2开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 自动BOSS1界面3开关
    {
        get
        {
            return (int)this["自动BOSS1界面3开关"];
        }
        set
        {
            this["自动BOSS1界面3开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 自动BOSS1界面4开关
    {
        get
        {
            return (int)this["自动BOSS1界面4开关"];
        }
        set
        {
            this["自动BOSS1界面4开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 自动BOSS1界面5开关
    {
        get
        {
            return (int)this["自动BOSS1界面5开关"];
        }
        set
        {
            this["自动BOSS1界面5开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 平台开关模式
    {
        get
        {
            return (int)this["平台开关模式"];
        }
        set
        {
            this["平台开关模式"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10000")]
    public int 平台金币充值模块
    {
        get
        {
            return (int)this["平台金币充值模块"];
        }
        set
        {
            this["平台金币充值模块"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 平台元宝充值模块
    {
        get
        {
            return (int)this["平台元宝充值模块"];
        }
        set
        {
            this["平台元宝充值模块"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 九层妖塔数量1
    {
        get
        {
            return (int)this["九层妖塔数量1"];
        }
        set
        {
            this["九层妖塔数量1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 九层妖塔数量2
    {
        get
        {
            return (int)this["九层妖塔数量2"];
        }
        set
        {
            this["九层妖塔数量2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 九层妖塔数量3
    {
        get
        {
            return (int)this["九层妖塔数量3"];
        }
        set
        {
            this["九层妖塔数量3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 九层妖塔数量4
    {
        get
        {
            return (int)this["九层妖塔数量4"];
        }
        set
        {
            this["九层妖塔数量4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 九层妖塔数量5
    {
        get
        {
            return (int)this["九层妖塔数量5"];
        }
        set
        {
            this["九层妖塔数量5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 九层妖塔数量6
    {
        get
        {
            return (int)this["九层妖塔数量6"];
        }
        set
        {
            this["九层妖塔数量6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 九层妖塔数量7
    {
        get
        {
            return (int)this["九层妖塔数量7"];
        }
        set
        {
            this["九层妖塔数量7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 九层妖塔数量8
    {
        get
        {
            return (int)this["九层妖塔数量8"];
        }
        set
        {
            this["九层妖塔数量8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 九层妖塔数量9
    {
        get
        {
            return (int)this["九层妖塔数量9"];
        }
        set
        {
            this["九层妖塔数量9"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string 九层妖塔BOSS1
    {
        get
        {
            return (string)this["九层妖塔BOSS1"];
        }
        set
        {
            this["九层妖塔BOSS1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string 九层妖塔BOSS2
    {
        get
        {
            return (string)this["九层妖塔BOSS2"];
        }
        set
        {
            this["九层妖塔BOSS2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string 九层妖塔BOSS3
    {
        get
        {
            return (string)this["九层妖塔BOSS3"];
        }
        set
        {
            this["九层妖塔BOSS3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string 九层妖塔BOSS4
    {
        get
        {
            return (string)this["九层妖塔BOSS4"];
        }
        set
        {
            this["九层妖塔BOSS4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string 九层妖塔BOSS5
    {
        get
        {
            return (string)this["九层妖塔BOSS5"];
        }
        set
        {
            this["九层妖塔BOSS5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string 九层妖塔BOSS6
    {
        get
        {
            return (string)this["九层妖塔BOSS6"];
        }
        set
        {
            this["九层妖塔BOSS6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string 九层妖塔BOSS7
    {
        get
        {
            return (string)this["九层妖塔BOSS7"];
        }
        set
        {
            this["九层妖塔BOSS7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string 九层妖塔BOSS8
    {
        get
        {
            return (string)this["九层妖塔BOSS8"];
        }
        set
        {
            this["九层妖塔BOSS8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string 九层妖塔BOSS9
    {
        get
        {
            return (string)this["九层妖塔BOSS9"];
        }
        set
        {
            this["九层妖塔BOSS9"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("白野猪")]
    public string 九层妖塔精英1
    {
        get
        {
            return (string)this["九层妖塔精英1"];
        }
        set
        {
            this["九层妖塔精英1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("白野猪")]
    public string 九层妖塔精英2
    {
        get
        {
            return (string)this["九层妖塔精英2"];
        }
        set
        {
            this["九层妖塔精英2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("白野猪")]
    public string 九层妖塔精英3
    {
        get
        {
            return (string)this["九层妖塔精英3"];
        }
        set
        {
            this["九层妖塔精英3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("白野猪")]
    public string 九层妖塔精英4
    {
        get
        {
            return (string)this["九层妖塔精英4"];
        }
        set
        {
            this["九层妖塔精英4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("白野猪")]
    public string 九层妖塔精英5
    {
        get
        {
            return (string)this["九层妖塔精英5"];
        }
        set
        {
            this["九层妖塔精英5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("白野猪")]
    public string 九层妖塔精英6
    {
        get
        {
            return (string)this["九层妖塔精英6"];
        }
        set
        {
            this["九层妖塔精英6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("白野猪")]
    public string 九层妖塔精英7
    {
        get
        {
            return (string)this["九层妖塔精英7"];
        }
        set
        {
            this["九层妖塔精英7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("白野猪")]
    public string 九层妖塔精英8
    {
        get
        {
            return (string)this["九层妖塔精英8"];
        }
        set
        {
            this["九层妖塔精英8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("白野猪")]
    public string 九层妖塔精英9
    {
        get
        {
            return (string)this["九层妖塔精英9"];
        }
        set
        {
            this["九层妖塔精英9"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 九层妖塔副本次数
    {
        get
        {
            return (int)this["九层妖塔副本次数"];
        }
        set
        {
            this["九层妖塔副本次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("40")]
    public int 九层妖塔副本等级
    {
        get
        {
            return (int)this["九层妖塔副本等级"];
        }
        set
        {
            this["九层妖塔副本等级"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 九层妖塔副本物品
    {
        get
        {
            return (int)this["九层妖塔副本物品"];
        }
        set
        {
            this["九层妖塔副本物品"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 九层妖塔副本数量
    {
        get
        {
            return (int)this["九层妖塔副本数量"];
        }
        set
        {
            this["九层妖塔副本数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 九层妖塔副本时间小
    {
        get
        {
            return (int)this["九层妖塔副本时间小"];
        }
        set
        {
            this["九层妖塔副本时间小"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("23")]
    public int 九层妖塔副本时间大
    {
        get
        {
            return (int)this["九层妖塔副本时间大"];
        }
        set
        {
            this["九层妖塔副本时间大"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("25")]
    public byte AutoBattleLevel
    {
        get
        {
            return (byte)this["AutoBattleLevel"];
        }
        set
        {
            this["AutoBattleLevel"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 禁止背包铭文洗练
    {
        get
        {
            return (byte)this["禁止背包铭文洗练"];
        }
        set
        {
            this["禁止背包铭文洗练"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public byte 沙巴克禁止随机
    {
        get
        {
            return (byte)this["沙巴克禁止随机"];
        }
        set
        {
            this["沙巴克禁止随机"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000000")]
    public int 冥想丹自定义经验
    {
        get
        {
            return (int)this["冥想丹自定义经验"];
        }
        set
        {
            this["冥想丹自定义经验"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 沙巴克爆装备开关
    {
        get
        {
            return (byte)this["沙巴克爆装备开关"];
        }
        set
        {
            this["沙巴克爆装备开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文龙枪1挡1次数
    {
        get
        {
            return (int)this["铭文龙枪1挡1次数"];
        }
        set
        {
            this["铭文龙枪1挡1次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文龙枪1挡2次数
    {
        get
        {
            return (int)this["铭文龙枪1挡2次数"];
        }
        set
        {
            this["铭文龙枪1挡2次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文龙枪1挡3次数
    {
        get
        {
            return (int)this["铭文龙枪1挡3次数"];
        }
        set
        {
            this["铭文龙枪1挡3次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 铭文龙枪1挡1概率
    {
        get
        {
            return (int)this["铭文龙枪1挡1概率"];
        }
        set
        {
            this["铭文龙枪1挡1概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 铭文龙枪1挡2概率
    {
        get
        {
            return (int)this["铭文龙枪1挡2概率"];
        }
        set
        {
            this["铭文龙枪1挡2概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("99")]
    public int 铭文龙枪1挡3概率
    {
        get
        {
            return (int)this["铭文龙枪1挡3概率"];
        }
        set
        {
            this["铭文龙枪1挡3概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文龙枪1挡技能编号
    {
        get
        {
            return (int)this["铭文龙枪1挡技能编号"];
        }
        set
        {
            this["铭文龙枪1挡技能编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 铭文龙枪1挡技能铭文
    {
        get
        {
            return (int)this["铭文龙枪1挡技能铭文"];
        }
        set
        {
            this["铭文龙枪1挡技能铭文"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文龙枪2挡1次数
    {
        get
        {
            return (int)this["铭文龙枪2挡1次数"];
        }
        set
        {
            this["铭文龙枪2挡1次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文龙枪2挡2次数
    {
        get
        {
            return (int)this["铭文龙枪2挡2次数"];
        }
        set
        {
            this["铭文龙枪2挡2次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文龙枪2挡3次数
    {
        get
        {
            return (int)this["铭文龙枪2挡3次数"];
        }
        set
        {
            this["铭文龙枪2挡3次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 铭文龙枪2挡1概率
    {
        get
        {
            return (int)this["铭文龙枪2挡1概率"];
        }
        set
        {
            this["铭文龙枪2挡1概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 铭文龙枪2挡2概率
    {
        get
        {
            return (int)this["铭文龙枪2挡2概率"];
        }
        set
        {
            this["铭文龙枪2挡2概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("99")]
    public int 铭文龙枪2挡3概率
    {
        get
        {
            return (int)this["铭文龙枪2挡3概率"];
        }
        set
        {
            this["铭文龙枪2挡3概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文龙枪2挡技能编号
    {
        get
        {
            return (int)this["铭文龙枪2挡技能编号"];
        }
        set
        {
            this["铭文龙枪2挡技能编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 铭文龙枪2挡技能铭文
    {
        get
        {
            return (int)this["铭文龙枪2挡技能铭文"];
        }
        set
        {
            this["铭文龙枪2挡技能铭文"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文龙枪3挡1次数
    {
        get
        {
            return (int)this["铭文龙枪3挡1次数"];
        }
        set
        {
            this["铭文龙枪3挡1次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文龙枪3挡2次数
    {
        get
        {
            return (int)this["铭文龙枪3挡2次数"];
        }
        set
        {
            this["铭文龙枪3挡2次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文龙枪3挡3次数
    {
        get
        {
            return (int)this["铭文龙枪3挡3次数"];
        }
        set
        {
            this["铭文龙枪3挡3次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 铭文龙枪3挡1概率
    {
        get
        {
            return (int)this["铭文龙枪3挡1概率"];
        }
        set
        {
            this["铭文龙枪3挡1概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 铭文龙枪3挡2概率
    {
        get
        {
            return (int)this["铭文龙枪3挡2概率"];
        }
        set
        {
            this["铭文龙枪3挡2概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("99")]
    public int 铭文龙枪3挡3概率
    {
        get
        {
            return (int)this["铭文龙枪3挡3概率"];
        }
        set
        {
            this["铭文龙枪3挡3概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文龙枪3挡技能编号
    {
        get
        {
            return (int)this["铭文龙枪3挡技能编号"];
        }
        set
        {
            this["铭文龙枪3挡技能编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 铭文龙枪3挡技能铭文
    {
        get
        {
            return (int)this["铭文龙枪3挡技能铭文"];
        }
        set
        {
            this["铭文龙枪3挡技能铭文"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文弓手1挡1次数
    {
        get
        {
            return (int)this["铭文弓手1挡1次数"];
        }
        set
        {
            this["铭文弓手1挡1次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文弓手1挡2次数
    {
        get
        {
            return (int)this["铭文弓手1挡2次数"];
        }
        set
        {
            this["铭文弓手1挡2次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文弓手1挡3次数
    {
        get
        {
            return (int)this["铭文弓手1挡3次数"];
        }
        set
        {
            this["铭文弓手1挡3次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 铭文弓手1挡1概率
    {
        get
        {
            return (int)this["铭文弓手1挡1概率"];
        }
        set
        {
            this["铭文弓手1挡1概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 铭文弓手1挡2概率
    {
        get
        {
            return (int)this["铭文弓手1挡2概率"];
        }
        set
        {
            this["铭文弓手1挡2概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("99")]
    public int 铭文弓手1挡3概率
    {
        get
        {
            return (int)this["铭文弓手1挡3概率"];
        }
        set
        {
            this["铭文弓手1挡3概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文弓手1挡技能编号
    {
        get
        {
            return (int)this["铭文弓手1挡技能编号"];
        }
        set
        {
            this["铭文弓手1挡技能编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 铭文弓手1挡技能铭文
    {
        get
        {
            return (int)this["铭文弓手1挡技能铭文"];
        }
        set
        {
            this["铭文弓手1挡技能铭文"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文弓手2挡1次数
    {
        get
        {
            return (int)this["铭文弓手2挡1次数"];
        }
        set
        {
            this["铭文弓手2挡1次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文弓手2挡2次数
    {
        get
        {
            return (int)this["铭文弓手2挡2次数"];
        }
        set
        {
            this["铭文弓手2挡2次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文弓手2挡3次数
    {
        get
        {
            return (int)this["铭文弓手2挡3次数"];
        }
        set
        {
            this["铭文弓手2挡3次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 铭文弓手2挡1概率
    {
        get
        {
            return (int)this["铭文弓手2挡1概率"];
        }
        set
        {
            this["铭文弓手2挡1概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 铭文弓手2挡2概率
    {
        get
        {
            return (int)this["铭文弓手2挡2概率"];
        }
        set
        {
            this["铭文弓手2挡2概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("99")]
    public int 铭文弓手2挡3概率
    {
        get
        {
            return (int)this["铭文弓手2挡3概率"];
        }
        set
        {
            this["铭文弓手2挡3概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文弓手2挡技能编号
    {
        get
        {
            return (int)this["铭文弓手2挡技能编号"];
        }
        set
        {
            this["铭文弓手2挡技能编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 铭文弓手2挡技能铭文
    {
        get
        {
            return (int)this["铭文弓手2挡技能铭文"];
        }
        set
        {
            this["铭文弓手2挡技能铭文"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文弓手3挡1次数
    {
        get
        {
            return (int)this["铭文弓手3挡1次数"];
        }
        set
        {
            this["铭文弓手3挡1次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文弓手3挡2次数
    {
        get
        {
            return (int)this["铭文弓手3挡2次数"];
        }
        set
        {
            this["铭文弓手3挡2次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文弓手3挡3次数
    {
        get
        {
            return (int)this["铭文弓手3挡3次数"];
        }
        set
        {
            this["铭文弓手3挡3次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 铭文弓手3挡1概率
    {
        get
        {
            return (int)this["铭文弓手3挡1概率"];
        }
        set
        {
            this["铭文弓手3挡1概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 铭文弓手3挡2概率
    {
        get
        {
            return (int)this["铭文弓手3挡2概率"];
        }
        set
        {
            this["铭文弓手3挡2概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("99")]
    public int 铭文弓手3挡3概率
    {
        get
        {
            return (int)this["铭文弓手3挡3概率"];
        }
        set
        {
            this["铭文弓手3挡3概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文弓手3挡技能编号
    {
        get
        {
            return (int)this["铭文弓手3挡技能编号"];
        }
        set
        {
            this["铭文弓手3挡技能编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 铭文弓手3挡技能铭文
    {
        get
        {
            return (int)this["铭文弓手3挡技能铭文"];
        }
        set
        {
            this["铭文弓手3挡技能铭文"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文刺客1挡1次数
    {
        get
        {
            return (int)this["铭文刺客1挡1次数"];
        }
        set
        {
            this["铭文刺客1挡1次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文刺客1挡2次数
    {
        get
        {
            return (int)this["铭文刺客1挡2次数"];
        }
        set
        {
            this["铭文刺客1挡2次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文刺客1挡3次数
    {
        get
        {
            return (int)this["铭文刺客1挡3次数"];
        }
        set
        {
            this["铭文刺客1挡3次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 铭文刺客1挡1概率
    {
        get
        {
            return (int)this["铭文刺客1挡1概率"];
        }
        set
        {
            this["铭文刺客1挡1概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 铭文刺客1挡2概率
    {
        get
        {
            return (int)this["铭文刺客1挡2概率"];
        }
        set
        {
            this["铭文刺客1挡2概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("99")]
    public int 铭文刺客1挡3概率
    {
        get
        {
            return (int)this["铭文刺客1挡3概率"];
        }
        set
        {
            this["铭文刺客1挡3概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文刺客1挡技能编号
    {
        get
        {
            return (int)this["铭文刺客1挡技能编号"];
        }
        set
        {
            this["铭文刺客1挡技能编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 铭文刺客1挡技能铭文
    {
        get
        {
            return (int)this["铭文刺客1挡技能铭文"];
        }
        set
        {
            this["铭文刺客1挡技能铭文"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文刺客2挡1次数
    {
        get
        {
            return (int)this["铭文刺客2挡1次数"];
        }
        set
        {
            this["铭文刺客2挡1次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文刺客2挡2次数
    {
        get
        {
            return (int)this["铭文刺客2挡2次数"];
        }
        set
        {
            this["铭文刺客2挡2次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文刺客2挡3次数
    {
        get
        {
            return (int)this["铭文刺客2挡3次数"];
        }
        set
        {
            this["铭文刺客2挡3次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 铭文刺客2挡1概率
    {
        get
        {
            return (int)this["铭文刺客2挡1概率"];
        }
        set
        {
            this["铭文刺客2挡1概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 铭文刺客2挡2概率
    {
        get
        {
            return (int)this["铭文刺客2挡2概率"];
        }
        set
        {
            this["铭文刺客2挡2概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("99")]
    public int 铭文刺客2挡3概率
    {
        get
        {
            return (int)this["铭文刺客2挡3概率"];
        }
        set
        {
            this["铭文刺客2挡3概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文刺客2挡技能编号
    {
        get
        {
            return (int)this["铭文刺客2挡技能编号"];
        }
        set
        {
            this["铭文刺客2挡技能编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 铭文刺客2挡技能铭文
    {
        get
        {
            return (int)this["铭文刺客2挡技能铭文"];
        }
        set
        {
            this["铭文刺客2挡技能铭文"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文刺客3挡1次数
    {
        get
        {
            return (int)this["铭文刺客3挡1次数"];
        }
        set
        {
            this["铭文刺客3挡1次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文刺客3挡2次数
    {
        get
        {
            return (int)this["铭文刺客3挡2次数"];
        }
        set
        {
            this["铭文刺客3挡2次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文刺客3挡3次数
    {
        get
        {
            return (int)this["铭文刺客3挡3次数"];
        }
        set
        {
            this["铭文刺客3挡3次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 铭文刺客3挡1概率
    {
        get
        {
            return (int)this["铭文刺客3挡1概率"];
        }
        set
        {
            this["铭文刺客3挡1概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 铭文刺客3挡2概率
    {
        get
        {
            return (int)this["铭文刺客3挡2概率"];
        }
        set
        {
            this["铭文刺客3挡2概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("99")]
    public int 铭文刺客3挡3概率
    {
        get
        {
            return (int)this["铭文刺客3挡3概率"];
        }
        set
        {
            this["铭文刺客3挡3概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文刺客3挡技能编号
    {
        get
        {
            return (int)this["铭文刺客3挡技能编号"];
        }
        set
        {
            this["铭文刺客3挡技能编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 铭文刺客3挡技能铭文
    {
        get
        {
            return (int)this["铭文刺客3挡技能铭文"];
        }
        set
        {
            this["铭文刺客3挡技能铭文"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文道士1挡1次数
    {
        get
        {
            return (int)this["铭文道士1挡1次数"];
        }
        set
        {
            this["铭文道士1挡1次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文道士1挡2次数
    {
        get
        {
            return (int)this["铭文道士1挡2次数"];
        }
        set
        {
            this["铭文道士1挡2次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文道士1挡3次数
    {
        get
        {
            return (int)this["铭文道士1挡3次数"];
        }
        set
        {
            this["铭文道士1挡3次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 铭文道士1挡1概率
    {
        get
        {
            return (int)this["铭文道士1挡1概率"];
        }
        set
        {
            this["铭文道士1挡1概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 铭文道士1挡2概率
    {
        get
        {
            return (int)this["铭文道士1挡2概率"];
        }
        set
        {
            this["铭文道士1挡2概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("99")]
    public int 铭文道士1挡3概率
    {
        get
        {
            return (int)this["铭文道士1挡3概率"];
        }
        set
        {
            this["铭文道士1挡3概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文道士1挡技能编号
    {
        get
        {
            return (int)this["铭文道士1挡技能编号"];
        }
        set
        {
            this["铭文道士1挡技能编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 铭文道士1挡技能铭文
    {
        get
        {
            return (int)this["铭文道士1挡技能铭文"];
        }
        set
        {
            this["铭文道士1挡技能铭文"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文道士2挡1次数
    {
        get
        {
            return (int)this["铭文道士2挡1次数"];
        }
        set
        {
            this["铭文道士2挡1次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文道士2挡2次数
    {
        get
        {
            return (int)this["铭文道士2挡2次数"];
        }
        set
        {
            this["铭文道士2挡2次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文道士2挡3次数
    {
        get
        {
            return (int)this["铭文道士2挡3次数"];
        }
        set
        {
            this["铭文道士2挡3次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 铭文道士2挡1概率
    {
        get
        {
            return (int)this["铭文道士2挡1概率"];
        }
        set
        {
            this["铭文道士2挡1概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 铭文道士2挡2概率
    {
        get
        {
            return (int)this["铭文道士2挡2概率"];
        }
        set
        {
            this["铭文道士2挡2概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("99")]
    public int 铭文道士2挡3概率
    {
        get
        {
            return (int)this["铭文道士2挡3概率"];
        }
        set
        {
            this["铭文道士2挡3概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文道士2挡技能编号
    {
        get
        {
            return (int)this["铭文道士2挡技能编号"];
        }
        set
        {
            this["铭文道士2挡技能编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 铭文道士2挡技能铭文
    {
        get
        {
            return (int)this["铭文道士2挡技能铭文"];
        }
        set
        {
            this["铭文道士2挡技能铭文"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文道士3挡1次数
    {
        get
        {
            return (int)this["铭文道士3挡1次数"];
        }
        set
        {
            this["铭文道士3挡1次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文道士3挡2次数
    {
        get
        {
            return (int)this["铭文道士3挡2次数"];
        }
        set
        {
            this["铭文道士3挡2次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文道士3挡3次数
    {
        get
        {
            return (int)this["铭文道士3挡3次数"];
        }
        set
        {
            this["铭文道士3挡3次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 铭文道士3挡1概率
    {
        get
        {
            return (int)this["铭文道士3挡1概率"];
        }
        set
        {
            this["铭文道士3挡1概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 铭文道士3挡2概率
    {
        get
        {
            return (int)this["铭文道士3挡2概率"];
        }
        set
        {
            this["铭文道士3挡2概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("99")]
    public int 铭文道士3挡3概率
    {
        get
        {
            return (int)this["铭文道士3挡3概率"];
        }
        set
        {
            this["铭文道士3挡3概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文道士3挡技能编号
    {
        get
        {
            return (int)this["铭文道士3挡技能编号"];
        }
        set
        {
            this["铭文道士3挡技能编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 铭文道士3挡技能铭文
    {
        get
        {
            return (int)this["铭文道士3挡技能铭文"];
        }
        set
        {
            this["铭文道士3挡技能铭文"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文法师1挡1次数
    {
        get
        {
            return (int)this["铭文法师1挡1次数"];
        }
        set
        {
            this["铭文法师1挡1次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文法师1挡2次数
    {
        get
        {
            return (int)this["铭文法师1挡2次数"];
        }
        set
        {
            this["铭文法师1挡2次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文法师1挡3次数
    {
        get
        {
            return (int)this["铭文法师1挡3次数"];
        }
        set
        {
            this["铭文法师1挡3次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 铭文法师1挡1概率
    {
        get
        {
            return (int)this["铭文法师1挡1概率"];
        }
        set
        {
            this["铭文法师1挡1概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 铭文法师1挡2概率
    {
        get
        {
            return (int)this["铭文法师1挡2概率"];
        }
        set
        {
            this["铭文法师1挡2概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("99")]
    public int 铭文法师1挡3概率
    {
        get
        {
            return (int)this["铭文法师1挡3概率"];
        }
        set
        {
            this["铭文法师1挡3概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文法师1挡技能编号
    {
        get
        {
            return (int)this["铭文法师1挡技能编号"];
        }
        set
        {
            this["铭文法师1挡技能编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 铭文法师1挡技能铭文
    {
        get
        {
            return (int)this["铭文法师1挡技能铭文"];
        }
        set
        {
            this["铭文法师1挡技能铭文"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文法师2挡1次数
    {
        get
        {
            return (int)this["铭文法师2挡1次数"];
        }
        set
        {
            this["铭文法师2挡1次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文法师2挡2次数
    {
        get
        {
            return (int)this["铭文法师2挡2次数"];
        }
        set
        {
            this["铭文法师2挡2次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文法师2挡3次数
    {
        get
        {
            return (int)this["铭文法师2挡3次数"];
        }
        set
        {
            this["铭文法师2挡3次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 铭文法师2挡1概率
    {
        get
        {
            return (int)this["铭文法师2挡1概率"];
        }
        set
        {
            this["铭文法师2挡1概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 铭文法师2挡2概率
    {
        get
        {
            return (int)this["铭文法师2挡2概率"];
        }
        set
        {
            this["铭文法师2挡2概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("99")]
    public int 铭文法师2挡3概率
    {
        get
        {
            return (int)this["铭文法师2挡3概率"];
        }
        set
        {
            this["铭文法师2挡3概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文法师2挡技能编号
    {
        get
        {
            return (int)this["铭文法师2挡技能编号"];
        }
        set
        {
            this["铭文法师2挡技能编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 铭文法师2挡技能铭文
    {
        get
        {
            return (int)this["铭文法师2挡技能铭文"];
        }
        set
        {
            this["铭文法师2挡技能铭文"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文法师3挡1次数
    {
        get
        {
            return (int)this["铭文法师3挡1次数"];
        }
        set
        {
            this["铭文法师3挡1次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文法师3挡2次数
    {
        get
        {
            return (int)this["铭文法师3挡2次数"];
        }
        set
        {
            this["铭文法师3挡2次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文法师3挡3次数
    {
        get
        {
            return (int)this["铭文法师3挡3次数"];
        }
        set
        {
            this["铭文法师3挡3次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 铭文法师3挡1概率
    {
        get
        {
            return (int)this["铭文法师3挡1概率"];
        }
        set
        {
            this["铭文法师3挡1概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 铭文法师3挡2概率
    {
        get
        {
            return (int)this["铭文法师3挡2概率"];
        }
        set
        {
            this["铭文法师3挡2概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("99")]
    public int 铭文法师3挡3概率
    {
        get
        {
            return (int)this["铭文法师3挡3概率"];
        }
        set
        {
            this["铭文法师3挡3概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文法师3挡技能编号
    {
        get
        {
            return (int)this["铭文法师3挡技能编号"];
        }
        set
        {
            this["铭文法师3挡技能编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 铭文法师3挡技能铭文
    {
        get
        {
            return (int)this["铭文法师3挡技能铭文"];
        }
        set
        {
            this["铭文法师3挡技能铭文"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文战士1挡1次数
    {
        get
        {
            return (int)this["铭文战士1挡1次数"];
        }
        set
        {
            this["铭文战士1挡1次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文战士1挡2次数
    {
        get
        {
            return (int)this["铭文战士1挡2次数"];
        }
        set
        {
            this["铭文战士1挡2次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文战士1挡3次数
    {
        get
        {
            return (int)this["铭文战士1挡3次数"];
        }
        set
        {
            this["铭文战士1挡3次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 铭文战士1挡1概率
    {
        get
        {
            return (int)this["铭文战士1挡1概率"];
        }
        set
        {
            this["铭文战士1挡1概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 铭文战士1挡2概率
    {
        get
        {
            return (int)this["铭文战士1挡2概率"];
        }
        set
        {
            this["铭文战士1挡2概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("99")]
    public int 铭文战士1挡3概率
    {
        get
        {
            return (int)this["铭文战士1挡3概率"];
        }
        set
        {
            this["铭文战士1挡3概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文战士1挡技能编号
    {
        get
        {
            return (int)this["铭文战士1挡技能编号"];
        }
        set
        {
            this["铭文战士1挡技能编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 铭文战士1挡技能铭文
    {
        get
        {
            return (int)this["铭文战士1挡技能铭文"];
        }
        set
        {
            this["铭文战士1挡技能铭文"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文战士2挡1次数
    {
        get
        {
            return (int)this["铭文战士2挡1次数"];
        }
        set
        {
            this["铭文战士2挡1次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文战士2挡2次数
    {
        get
        {
            return (int)this["铭文战士2挡2次数"];
        }
        set
        {
            this["铭文战士2挡2次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文战士2挡3次数
    {
        get
        {
            return (int)this["铭文战士2挡3次数"];
        }
        set
        {
            this["铭文战士2挡3次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 铭文战士2挡1概率
    {
        get
        {
            return (int)this["铭文战士2挡1概率"];
        }
        set
        {
            this["铭文战士2挡1概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 铭文战士2挡2概率
    {
        get
        {
            return (int)this["铭文战士2挡2概率"];
        }
        set
        {
            this["铭文战士2挡2概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("99")]
    public int 铭文战士2挡3概率
    {
        get
        {
            return (int)this["铭文战士2挡3概率"];
        }
        set
        {
            this["铭文战士2挡3概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文战士2挡技能编号
    {
        get
        {
            return (int)this["铭文战士2挡技能编号"];
        }
        set
        {
            this["铭文战士2挡技能编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 铭文战士2挡技能铭文
    {
        get
        {
            return (int)this["铭文战士2挡技能铭文"];
        }
        set
        {
            this["铭文战士2挡技能铭文"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文战士3挡1次数
    {
        get
        {
            return (int)this["铭文战士3挡1次数"];
        }
        set
        {
            this["铭文战士3挡1次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文战士3挡2次数
    {
        get
        {
            return (int)this["铭文战士3挡2次数"];
        }
        set
        {
            this["铭文战士3挡2次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 铭文战士3挡3次数
    {
        get
        {
            return (int)this["铭文战士3挡3次数"];
        }
        set
        {
            this["铭文战士3挡3次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 铭文战士3挡1概率
    {
        get
        {
            return (int)this["铭文战士3挡1概率"];
        }
        set
        {
            this["铭文战士3挡1概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 铭文战士3挡2概率
    {
        get
        {
            return (int)this["铭文战士3挡2概率"];
        }
        set
        {
            this["铭文战士3挡2概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("99")]
    public int 铭文战士3挡3概率
    {
        get
        {
            return (int)this["铭文战士3挡3概率"];
        }
        set
        {
            this["铭文战士3挡3概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 铭文战士3挡技能编号
    {
        get
        {
            return (int)this["铭文战士3挡技能编号"];
        }
        set
        {
            this["铭文战士3挡技能编号"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 铭文战士3挡技能铭文
    {
        get
        {
            return (int)this["铭文战士3挡技能铭文"];
        }
        set
        {
            this["铭文战士3挡技能铭文"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 铭文弓手保底开关
    {
        get
        {
            return (int)this["铭文弓手保底开关"];
        }
        set
        {
            this["铭文弓手保底开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 铭文刺客保底开关
    {
        get
        {
            return (int)this["铭文刺客保底开关"];
        }
        set
        {
            this["铭文刺客保底开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 铭文法师保底开关
    {
        get
        {
            return (int)this["铭文法师保底开关"];
        }
        set
        {
            this["铭文法师保底开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 铭文道士保底开关
    {
        get
        {
            return (int)this["铭文道士保底开关"];
        }
        set
        {
            this["铭文道士保底开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 铭文龙枪保底开关
    {
        get
        {
            return (int)this["铭文龙枪保底开关"];
        }
        set
        {
            this["铭文龙枪保底开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 铭文战士保底开关
    {
        get
        {
            return (int)this["铭文战士保底开关"];
        }
        set
        {
            this["铭文战士保底开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int DropRateModifier
    {
        get
        {
            return (int)this["DropRateModifier"];
        }
        set
        {
            this["DropRateModifier"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("12")]
    public int 魔虫窟副本时间大
    {
        get
        {
            return (int)this["魔虫窟副本时间大"];
        }
        set
        {
            this["魔虫窟副本时间大"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("12")]
    public int 魔虫窟副本时间小
    {
        get
        {
            return (int)this["魔虫窟副本时间小"];
        }
        set
        {
            this["魔虫窟副本时间小"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 魔虫窟副本次数
    {
        get
        {
            return (int)this["魔虫窟副本次数"];
        }
        set
        {
            this["魔虫窟副本次数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("40")]
    public int 魔虫窟副本等级
    {
        get
        {
            return (int)this["魔虫窟副本等级"];
        }
        set
        {
            this["魔虫窟副本等级"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 魔虫窟副本物品
    {
        get
        {
            return (int)this["魔虫窟副本物品"];
        }
        set
        {
            this["魔虫窟副本物品"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10000")]
    public int 魔虫窟副本数量
    {
        get
        {
            return (int)this["魔虫窟副本数量"];
        }
        set
        {
            this["魔虫窟副本数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("天书残页")]
    public string 书店商贩物品
    {
        get
        {
            return (string)this["书店商贩物品"];
        }
        set
        {
            this["书店商贩物品"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 幸运洗练次数保底
    {
        get
        {
            return (int)this["幸运洗练次数保底"];
        }
        set
        {
            this["幸运洗练次数保底"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2")]
    public int 幸运洗练点数
    {
        get
        {
            return (int)this["幸运洗练点数"];
        }
        set
        {
            this["幸运洗练点数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 武器强化消耗货币值
    {
        get
        {
            return (int)this["武器强化消耗货币值"];
        }
        set
        {
            this["武器强化消耗货币值"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 武器强化消耗货币开关
    {
        get
        {
            return (int)this["武器强化消耗货币开关"];
        }
        set
        {
            this["武器强化消耗货币开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 武器强化取回时间
    {
        get
        {
            return (int)this["武器强化取回时间"];
        }
        set
        {
            this["武器强化取回时间"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("12")]
    public int 幸运额外1值
    {
        get
        {
            return (int)this["幸运额外1值"];
        }
        set
        {
            this["幸运额外1值"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("15")]
    public int 幸运额外2值
    {
        get
        {
            return (int)this["幸运额外2值"];
        }
        set
        {
            this["幸运额外2值"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("18")]
    public int 幸运额外3值
    {
        get
        {
            return (int)this["幸运额外3值"];
        }
        set
        {
            this["幸运额外3值"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("20")]
    public int 幸运额外4值
    {
        get
        {
            return (int)this["幸运额外4值"];
        }
        set
        {
            this["幸运额外4值"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999")]
    public int 幸运额外5值
    {
        get
        {
            return (int)this["幸运额外5值"];
        }
        set
        {
            this["幸运额外5值"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1.1")]
    public float 幸运额外1伤害
    {
        get
        {
            return (float)this["幸运额外1伤害"];
        }
        set
        {
            this["幸运额外1伤害"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1.3")]
    public float 幸运额外2伤害
    {
        get
        {
            return (float)this["幸运额外2伤害"];
        }
        set
        {
            this["幸运额外2伤害"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1.5")]
    public float 幸运额外3伤害
    {
        get
        {
            return (float)this["幸运额外3伤害"];
        }
        set
        {
            this["幸运额外3伤害"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1.8")]
    public float 幸运额外4伤害
    {
        get
        {
            return (float)this["幸运额外4伤害"];
        }
        set
        {
            this["幸运额外4伤害"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2.2")]
    public float 幸运额外5伤害
    {
        get
        {
            return (float)this["幸运额外5伤害"];
        }
        set
        {
            this["幸运额外5伤害"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("输入密码")]
    public string 挂机权限选项
    {
        get
        {
            return (string)this["挂机权限选项"];
        }
        set
        {
            this["挂机权限选项"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 暗之门地图1
    {
        get
        {
            return (int)this["暗之门地图1"];
        }
        set
        {
            this["暗之门地图1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 暗之门地图2
    {
        get
        {
            return (int)this["暗之门地图2"];
        }
        set
        {
            this["暗之门地图2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 暗之门地图3
    {
        get
        {
            return (int)this["暗之门地图3"];
        }
        set
        {
            this["暗之门地图3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 暗之门地图4
    {
        get
        {
            return (int)this["暗之门地图4"];
        }
        set
        {
            this["暗之门地图4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 暗之门杀怪触发
    {
        get
        {
            return (int)this["暗之门杀怪触发"];
        }
        set
        {
            this["暗之门杀怪触发"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 暗之门全服提示
    {
        get
        {
            return (int)this["暗之门全服提示"];
        }
        set
        {
            this["暗之门全服提示"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("60")]
    public int 暗之门时间
    {
        get
        {
            return (int)this["暗之门时间"];
        }
        set
        {
            this["暗之门时间"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("魔火龙")]
    public string 暗之门地图4BOSS
    {
        get
        {
            return (string)this["暗之门地图4BOSS"];
        }
        set
        {
            this["暗之门地图4BOSS"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("牛魔王")]
    public string 暗之门地图3BOSS
    {
        get
        {
            return (string)this["暗之门地图3BOSS"];
        }
        set
        {
            this["暗之门地图3BOSS"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("黄泉教主")]
    public string 暗之门地图2BOSS
    {
        get
        {
            return (string)this["暗之门地图2BOSS"];
        }
        set
        {
            this["暗之门地图2BOSS"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("尸王")]
    public string 暗之门地图1BOSS
    {
        get
        {
            return (string)this["暗之门地图1BOSS"];
        }
        set
        {
            this["暗之门地图1BOSS"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1401")]
    public int 暗之门地图4X
    {
        get
        {
            return (int)this["暗之门地图4X"];
        }
        set
        {
            this["暗之门地图4X"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("554")]
    public int 暗之门地图4Y
    {
        get
        {
            return (int)this["暗之门地图4Y"];
        }
        set
        {
            this["暗之门地图4Y"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1401")]
    public int 暗之门地图3X
    {
        get
        {
            return (int)this["暗之门地图3X"];
        }
        set
        {
            this["暗之门地图3X"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("554")]
    public int 暗之门地图3Y
    {
        get
        {
            return (int)this["暗之门地图3Y"];
        }
        set
        {
            this["暗之门地图3Y"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1401")]
    public int 暗之门地图2X
    {
        get
        {
            return (int)this["暗之门地图2X"];
        }
        set
        {
            this["暗之门地图2X"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("554")]
    public int 暗之门地图2Y
    {
        get
        {
            return (int)this["暗之门地图2Y"];
        }
        set
        {
            this["暗之门地图2Y"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1401")]
    public int 暗之门地图1X
    {
        get
        {
            return (int)this["暗之门地图1X"];
        }
        set
        {
            this["暗之门地图1X"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("554")]
    public int 暗之门地图1Y
    {
        get
        {
            return (int)this["暗之门地图1Y"];
        }
        set
        {
            this["暗之门地图1Y"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 暗之门开关
    {
        get
        {
            return (int)this["暗之门开关"];
        }
        set
        {
            this["暗之门开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 监狱货币类型
    {
        get
        {
            return (int)this["监狱货币类型"];
        }
        set
        {
            this["监狱货币类型"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 监狱货币
    {
        get
        {
            return (int)this["监狱货币"];
        }
        set
        {
            this["监狱货币"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 魔虫窟分钟限制
    {
        get
        {
            return (int)this["魔虫窟分钟限制"];
        }
        set
        {
            this["魔虫窟分钟限制"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 充值模块格式
    {
        get
        {
            return (int)this["充值模块格式"];
        }
        set
        {
            this["充值模块格式"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 自定义元宝兑换05
    {
        get
        {
            return (int)this["自定义元宝兑换05"];
        }
        set
        {
            this["自定义元宝兑换05"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 自定义元宝兑换04
    {
        get
        {
            return (int)this["自定义元宝兑换04"];
        }
        set
        {
            this["自定义元宝兑换04"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 自定义元宝兑换03
    {
        get
        {
            return (int)this["自定义元宝兑换03"];
        }
        set
        {
            this["自定义元宝兑换03"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 自定义元宝兑换02
    {
        get
        {
            return (int)this["自定义元宝兑换02"];
        }
        set
        {
            this["自定义元宝兑换02"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 自定义元宝兑换01
    {
        get
        {
            return (int)this["自定义元宝兑换01"];
        }
        set
        {
            this["自定义元宝兑换01"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("15")]
    public int 直升等级1
    {
        get
        {
            return (int)this["直升等级1"];
        }
        set
        {
            this["直升等级1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("20")]
    public int 直升等级2
    {
        get
        {
            return (int)this["直升等级2"];
        }
        set
        {
            this["直升等级2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("25")]
    public int 直升等级3
    {
        get
        {
            return (int)this["直升等级3"];
        }
        set
        {
            this["直升等级3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("30")]
    public int 直升等级4
    {
        get
        {
            return (int)this["直升等级4"];
        }
        set
        {
            this["直升等级4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("35")]
    public int 直升等级5
    {
        get
        {
            return (int)this["直升等级5"];
        }
        set
        {
            this["直升等级5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("40")]
    public int 直升等级6
    {
        get
        {
            return (int)this["直升等级6"];
        }
        set
        {
            this["直升等级6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("45")]
    public int 直升等级7
    {
        get
        {
            return (int)this["直升等级7"];
        }
        set
        {
            this["直升等级7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("50")]
    public int 直升等级8
    {
        get
        {
            return (int)this["直升等级8"];
        }
        set
        {
            this["直升等级8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("55")]
    public int 直升等级9
    {
        get
        {
            return (int)this["直升等级9"];
        }
        set
        {
            this["直升等级9"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 直升经验1
    {
        get
        {
            return (int)this["直升经验1"];
        }
        set
        {
            this["直升经验1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 直升经验2
    {
        get
        {
            return (int)this["直升经验2"];
        }
        set
        {
            this["直升经验2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 直升经验3
    {
        get
        {
            return (int)this["直升经验3"];
        }
        set
        {
            this["直升经验3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 直升经验4
    {
        get
        {
            return (int)this["直升经验4"];
        }
        set
        {
            this["直升经验4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 直升经验5
    {
        get
        {
            return (int)this["直升经验5"];
        }
        set
        {
            this["直升经验5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 直升经验6
    {
        get
        {
            return (int)this["直升经验6"];
        }
        set
        {
            this["直升经验6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 直升经验7
    {
        get
        {
            return (int)this["直升经验7"];
        }
        set
        {
            this["直升经验7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 直升经验8
    {
        get
        {
            return (int)this["直升经验8"];
        }
        set
        {
            this["直升经验8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 直升经验9
    {
        get
        {
            return (int)this["直升经验9"];
        }
        set
        {
            this["直升经验9"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 直升物品1
    {
        get
        {
            return (int)this["直升物品1"];
        }
        set
        {
            this["直升物品1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 直升物品2
    {
        get
        {
            return (int)this["直升物品2"];
        }
        set
        {
            this["直升物品2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 直升物品3
    {
        get
        {
            return (int)this["直升物品3"];
        }
        set
        {
            this["直升物品3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 直升物品4
    {
        get
        {
            return (int)this["直升物品4"];
        }
        set
        {
            this["直升物品4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 直升物品5
    {
        get
        {
            return (int)this["直升物品5"];
        }
        set
        {
            this["直升物品5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 直升物品6
    {
        get
        {
            return (int)this["直升物品6"];
        }
        set
        {
            this["直升物品6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 直升物品7
    {
        get
        {
            return (int)this["直升物品7"];
        }
        set
        {
            this["直升物品7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 直升物品8
    {
        get
        {
            return (int)this["直升物品8"];
        }
        set
        {
            this["直升物品8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 直升物品9
    {
        get
        {
            return (int)this["直升物品9"];
        }
        set
        {
            this["直升物品9"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int UpgradeXPLevel1
    {
        get
        {
            return (int)this["UpgradeXPLevel1"];
        }
        set
        {
            this["UpgradeXPLevel1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("200")]
    public int UpgradeXPLevel2
    {
        get
        {
            return (int)this["UpgradeXPLevel2"];
        }
        set
        {
            this["UpgradeXPLevel2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("300")]
    public int UpgradeXPLevel3
    {
        get
        {
            return (int)this["UpgradeXPLevel3"];
        }
        set
        {
            this["UpgradeXPLevel3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("400")]
    public int UpgradeXPLevel4
    {
        get
        {
            return (int)this["UpgradeXPLevel4"];
        }
        set
        {
            this["UpgradeXPLevel4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("600")]
    public int UpgradeXPLevel5
    {
        get
        {
            return (int)this["UpgradeXPLevel5"];
        }
        set
        {
            this["UpgradeXPLevel5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("900")]
    public int UpgradeXPLevel6
    {
        get
        {
            return (int)this["UpgradeXPLevel6"];
        }
        set
        {
            this["UpgradeXPLevel6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1200")]
    public int UpgradeXPLevel7
    {
        get
        {
            return (int)this["UpgradeXPLevel7"];
        }
        set
        {
            this["UpgradeXPLevel7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1700")]
    public int UpgradeXPLevel8
    {
        get
        {
            return (int)this["UpgradeXPLevel8"];
        }
        set
        {
            this["UpgradeXPLevel8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2500")]
    public int UpgradeXPLevel9
    {
        get
        {
            return (int)this["UpgradeXPLevel9"];
        }
        set
        {
            this["UpgradeXPLevel9"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("6000")]
    public int UpgradeXPLevel10
    {
        get
        {
            return (int)this["UpgradeXPLevel10"];
        }
        set
        {
            this["UpgradeXPLevel10"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("8000")]
    public int UpgradeXPLevel11
    {
        get
        {
            return (int)this["UpgradeXPLevel11"];
        }
        set
        {
            this["UpgradeXPLevel11"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10000")]
    public int UpgradeXPLevel12
    {
        get
        {
            return (int)this["UpgradeXPLevel12"];
        }
        set
        {
            this["UpgradeXPLevel12"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("15000")]
    public int UpgradeXPLevel13
    {
        get
        {
            return (int)this["UpgradeXPLevel13"];
        }
        set
        {
            this["UpgradeXPLevel13"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("30000")]
    public int UpgradeXPLevel14
    {
        get
        {
            return (int)this["UpgradeXPLevel14"];
        }
        set
        {
            this["UpgradeXPLevel14"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("40000")]
    public int UpgradeXPLevel15
    {
        get
        {
            return (int)this["UpgradeXPLevel15"];
        }
        set
        {
            this["UpgradeXPLevel15"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("50000")]
    public int UpgradeXPLevel16
    {
        get
        {
            return (int)this["UpgradeXPLevel16"];
        }
        set
        {
            this["UpgradeXPLevel16"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("70000")]
    public int UpgradeXPLevel17
    {
        get
        {
            return (int)this["UpgradeXPLevel17"];
        }
        set
        {
            this["UpgradeXPLevel17"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int UpgradeXPLevel18
    {
        get
        {
            return (int)this["UpgradeXPLevel18"];
        }
        set
        {
            this["UpgradeXPLevel18"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("120000")]
    public int UpgradeXPLevel19
    {
        get
        {
            return (int)this["UpgradeXPLevel19"];
        }
        set
        {
            this["UpgradeXPLevel19"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("140000")]
    public int UpgradeXPLevel20
    {
        get
        {
            return (int)this["UpgradeXPLevel20"];
        }
        set
        {
            this["UpgradeXPLevel20"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("250000")]
    public int UpgradeXPLevel21
    {
        get
        {
            return (int)this["UpgradeXPLevel21"];
        }
        set
        {
            this["UpgradeXPLevel21"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("300000")]
    public int UpgradeXPLevel22
    {
        get
        {
            return (int)this["UpgradeXPLevel22"];
        }
        set
        {
            this["UpgradeXPLevel22"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("350000")]
    public int UpgradeXPLevel23
    {
        get
        {
            return (int)this["UpgradeXPLevel23"];
        }
        set
        {
            this["UpgradeXPLevel23"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("400000")]
    public int UpgradeXPLevel24
    {
        get
        {
            return (int)this["UpgradeXPLevel24"];
        }
        set
        {
            this["UpgradeXPLevel24"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("500000")]
    public int UpgradeXPLevel25
    {
        get
        {
            return (int)this["UpgradeXPLevel25"];
        }
        set
        {
            this["UpgradeXPLevel25"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("700000")]
    public int UpgradeXPLevel26
    {
        get
        {
            return (int)this["UpgradeXPLevel26"];
        }
        set
        {
            this["UpgradeXPLevel26"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000000")]
    public int UpgradeXPLevel27
    {
        get
        {
            return (int)this["UpgradeXPLevel27"];
        }
        set
        {
            this["UpgradeXPLevel27"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1400000")]
    public int UpgradeXPLevel28
    {
        get
        {
            return (int)this["UpgradeXPLevel28"];
        }
        set
        {
            this["UpgradeXPLevel28"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1800000")]
    public int UpgradeXPLevel29
    {
        get
        {
            return (int)this["UpgradeXPLevel29"];
        }
        set
        {
            this["UpgradeXPLevel29"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000000")]
    public int UpgradeXPLevel30
    {
        get
        {
            return (int)this["UpgradeXPLevel30"];
        }
        set
        {
            this["UpgradeXPLevel30"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2400000")]
    public int UpgradeXPLevel31
    {
        get
        {
            return (int)this["UpgradeXPLevel31"];
        }
        set
        {
            this["UpgradeXPLevel31"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2800000")]
    public int UpgradeXPLevel32
    {
        get
        {
            return (int)this["UpgradeXPLevel32"];
        }
        set
        {
            this["UpgradeXPLevel32"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("3200000")]
    public int UpgradeXPLevel33
    {
        get
        {
            return (int)this["UpgradeXPLevel33"];
        }
        set
        {
            this["UpgradeXPLevel33"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("3600000")]
    public int UpgradeXPLevel34
    {
        get
        {
            return (int)this["UpgradeXPLevel34"];
        }
        set
        {
            this["UpgradeXPLevel34"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("4000000")]
    public int UpgradeXPLevel35
    {
        get
        {
            return (int)this["UpgradeXPLevel35"];
        }
        set
        {
            this["UpgradeXPLevel35"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("4800000")]
    public int UpgradeXPLevel36
    {
        get
        {
            return (int)this["UpgradeXPLevel36"];
        }
        set
        {
            this["UpgradeXPLevel36"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5600000")]
    public int UpgradeXPLevel37
    {
        get
        {
            return (int)this["UpgradeXPLevel37"];
        }
        set
        {
            this["UpgradeXPLevel37"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("8200000")]
    public int UpgradeXPLevel38
    {
        get
        {
            return (int)this["UpgradeXPLevel38"];
        }
        set
        {
            this["UpgradeXPLevel38"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("9000000")]
    public int UpgradeXPLevel39
    {
        get
        {
            return (int)this["UpgradeXPLevel39"];
        }
        set
        {
            this["UpgradeXPLevel39"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int DefaultSkillLevel
    {
        get
        {
            return (int)this["DefaultSkillLevel"];
        }
        set
        {
            this["DefaultSkillLevel"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("屠魔令")]
    public string 沃玛分解物品一
    {
        get
        {
            return (string)this["沃玛分解物品一"];
        }
        set
        {
            this["沃玛分解物品一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("天书残页")]
    public string 沃玛分解物品二
    {
        get
        {
            return (string)this["沃玛分解物品二"];
        }
        set
        {
            this["沃玛分解物品二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("装备碎片")]
    public string 沃玛分解物品三
    {
        get
        {
            return (string)this["沃玛分解物品三"];
        }
        set
        {
            this["沃玛分解物品三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("龍纹兑换石")]
    public string 沃玛分解物品四
    {
        get
        {
            return (string)this["沃玛分解物品四"];
        }
        set
        {
            this["沃玛分解物品四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("70")]
    public int 沃玛分解几率一
    {
        get
        {
            return (int)this["沃玛分解几率一"];
        }
        set
        {
            this["沃玛分解几率一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("85")]
    public int 沃玛分解几率二
    {
        get
        {
            return (int)this["沃玛分解几率二"];
        }
        set
        {
            this["沃玛分解几率二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("95")]
    public int 沃玛分解几率三
    {
        get
        {
            return (int)this["沃玛分解几率三"];
        }
        set
        {
            this["沃玛分解几率三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 沃玛分解几率四
    {
        get
        {
            return (int)this["沃玛分解几率四"];
        }
        set
        {
            this["沃玛分解几率四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 沃玛分解数量一
    {
        get
        {
            return (int)this["沃玛分解数量一"];
        }
        set
        {
            this["沃玛分解数量一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 沃玛分解数量二
    {
        get
        {
            return (int)this["沃玛分解数量二"];
        }
        set
        {
            this["沃玛分解数量二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 沃玛分解数量三
    {
        get
        {
            return (int)this["沃玛分解数量三"];
        }
        set
        {
            this["沃玛分解数量三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 沃玛分解数量四
    {
        get
        {
            return (int)this["沃玛分解数量四"];
        }
        set
        {
            this["沃玛分解数量四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("3")]
    public int 沃玛分解开关
    {
        get
        {
            return (int)this["沃玛分解开关"];
        }
        set
        {
            this["沃玛分解开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("屠魔令")]
    public string 其他分解物品一
    {
        get
        {
            return (string)this["其他分解物品一"];
        }
        set
        {
            this["其他分解物品一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("天书残页")]
    public string 其他分解物品二
    {
        get
        {
            return (string)this["其他分解物品二"];
        }
        set
        {
            this["其他分解物品二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("装备碎片")]
    public string 其他分解物品三
    {
        get
        {
            return (string)this["其他分解物品三"];
        }
        set
        {
            this["其他分解物品三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("龍纹兑换石")]
    public string 其他分解物品四
    {
        get
        {
            return (string)this["其他分解物品四"];
        }
        set
        {
            this["其他分解物品四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("70")]
    public int 其他分解几率一
    {
        get
        {
            return (int)this["其他分解几率一"];
        }
        set
        {
            this["其他分解几率一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("85")]
    public int 其他分解几率二
    {
        get
        {
            return (int)this["其他分解几率二"];
        }
        set
        {
            this["其他分解几率二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("95")]
    public int 其他分解几率三
    {
        get
        {
            return (int)this["其他分解几率三"];
        }
        set
        {
            this["其他分解几率三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    public int 其他分解几率四
    {
        get
        {
            return (int)this["其他分解几率四"];
        }
        set
        {
            this["其他分解几率四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 其他分解数量一
    {
        get
        {
            return (int)this["其他分解数量一"];
        }
        set
        {
            this["其他分解数量一"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 其他分解数量二
    {
        get
        {
            return (int)this["其他分解数量二"];
        }
        set
        {
            this["其他分解数量二"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 其他分解数量三
    {
        get
        {
            return (int)this["其他分解数量三"];
        }
        set
        {
            this["其他分解数量三"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 其他分解数量四
    {
        get
        {
            return (int)this["其他分解数量四"];
        }
        set
        {
            this["其他分解数量四"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("3")]
    public int 其他分解开关
    {
        get
        {
            return (int)this["其他分解开关"];
        }
        set
        {
            this["其他分解开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 拾取地图控制1
    {
        get
        {
            return (int)this["拾取地图控制1"];
        }
        set
        {
            this["拾取地图控制1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 拾取地图控制2
    {
        get
        {
            return (int)this["拾取地图控制2"];
        }
        set
        {
            this["拾取地图控制2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 拾取地图控制3
    {
        get
        {
            return (int)this["拾取地图控制3"];
        }
        set
        {
            this["拾取地图控制3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 拾取地图控制4
    {
        get
        {
            return (int)this["拾取地图控制4"];
        }
        set
        {
            this["拾取地图控制4"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 拾取地图控制5
    {
        get
        {
            return (int)this["拾取地图控制5"];
        }
        set
        {
            this["拾取地图控制5"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 拾取地图控制6
    {
        get
        {
            return (int)this["拾取地图控制6"];
        }
        set
        {
            this["拾取地图控制6"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 拾取地图控制7
    {
        get
        {
            return (int)this["拾取地图控制7"];
        }
        set
        {
            this["拾取地图控制7"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("147")]
    public int 拾取地图控制8
    {
        get
        {
            return (int)this["拾取地图控制8"];
        }
        set
        {
            this["拾取地图控制8"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 沙城捐献货币类型
    {
        get
        {
            return (int)this["沙城捐献货币类型"];
        }
        set
        {
            this["沙城捐献货币类型"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000000")]
    public int 沙城捐献支付数量
    {
        get
        {
            return (int)this["沙城捐献支付数量"];
        }
        set
        {
            this["沙城捐献支付数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 沙城捐献获得物品1
    {
        get
        {
            return (int)this["沙城捐献获得物品1"];
        }
        set
        {
            this["沙城捐献获得物品1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 沙城捐献获得物品2
    {
        get
        {
            return (int)this["沙城捐献获得物品2"];
        }
        set
        {
            this["沙城捐献获得物品2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 沙城捐献获得物品3
    {
        get
        {
            return (int)this["沙城捐献获得物品3"];
        }
        set
        {
            this["沙城捐献获得物品3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 沙城捐献物品数量1
    {
        get
        {
            return (int)this["沙城捐献物品数量1"];
        }
        set
        {
            this["沙城捐献物品数量1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 沙城捐献物品数量2
    {
        get
        {
            return (int)this["沙城捐献物品数量2"];
        }
        set
        {
            this["沙城捐献物品数量2"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 沙城捐献物品数量3
    {
        get
        {
            return (int)this["沙城捐献物品数量3"];
        }
        set
        {
            this["沙城捐献物品数量3"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 沙城捐献赞助人数
    {
        get
        {
            return (int)this["沙城捐献赞助人数"];
        }
        set
        {
            this["沙城捐献赞助人数"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 沙城捐献赞助金额
    {
        get
        {
            return (int)this["沙城捐献赞助金额"];
        }
        set
        {
            this["沙城捐献赞助金额"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("131")]
    public int 称号范围拾取判断1
    {
        get
        {
            return (int)this["称号范围拾取判断1"];
        }
        set
        {
            this["称号范围拾取判断1"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("6000")]
    public int 雕爷激活灵符需求
    {
        get
        {
            return (int)this["雕爷激活灵符需求"];
        }
        set
        {
            this["雕爷激活灵符需求"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    public int 雕爷1号位灵符
    {
        get
        {
            return (int)this["雕爷1号位灵符"];
        }
        set
        {
            this["雕爷1号位灵符"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("2000")]
    public int 雕爷2号位灵符
    {
        get
        {
            return (int)this["雕爷2号位灵符"];
        }
        set
        {
            this["雕爷2号位灵符"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("3000")]
    public int 雕爷3号位灵符
    {
        get
        {
            return (int)this["雕爷3号位灵符"];
        }
        set
        {
            this["雕爷3号位灵符"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 雕爷1号位铭文石
    {
        get
        {
            return (int)this["雕爷1号位铭文石"];
        }
        set
        {
            this["雕爷1号位铭文石"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 雕爷2号位铭文石
    {
        get
        {
            return (int)this["雕爷2号位铭文石"];
        }
        set
        {
            this["雕爷2号位铭文石"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("5")]
    public int 雕爷3号位铭文石
    {
        get
        {
            return (int)this["雕爷3号位铭文石"];
        }
        set
        {
            this["雕爷3号位铭文石"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 九层妖塔统计开关
    {
        get
        {
            return (int)this["九层妖塔统计开关"];
        }
        set
        {
            this["九层妖塔统计开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 沙巴克每周攻沙时间
    {
        get
        {
            return (int)this["沙巴克每周攻沙时间"];
        }
        set
        {
            this["沙巴克每周攻沙时间"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("40")]
    public int 沙巴克皇宫传送等级
    {
        get
        {
            return (int)this["沙巴克皇宫传送等级"];
        }
        set
        {
            this["沙巴克皇宫传送等级"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 沙巴克皇宫传送物品
    {
        get
        {
            return (int)this["沙巴克皇宫传送物品"];
        }
        set
        {
            this["沙巴克皇宫传送物品"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 沙巴克皇宫传送数量
    {
        get
        {
            return (int)this["沙巴克皇宫传送数量"];
        }
        set
        {
            this["沙巴克皇宫传送数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("输入密码")]
    public string 合成模块控件
    {
        get
        {
            return (string)this["合成模块控件"];
        }
        set
        {
            this["合成模块控件"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 系统窗口发送
    {
        get
        {
            return (int)this["系统窗口发送"];
        }
        set
        {
            this["系统窗口发送"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 龙卫效果提示
    {
        get
        {
            return (int)this["龙卫效果提示"];
        }
        set
        {
            this["龙卫效果提示"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 充值平台切换
    {
        get
        {
            return (int)this["充值平台切换"];
        }
        set
        {
            this["充值平台切换"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 坐骑骑乘切换
    {
        get
        {
            return (int)this["坐骑骑乘切换"];
        }
        set
        {
            this["坐骑骑乘切换"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 坐骑属性切换
    {
        get
        {
            return (int)this["坐骑属性切换"];
        }
        set
        {
            this["坐骑属性切换"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 珍宝模块切换
    {
        get
        {
            return (int)this["珍宝模块切换"];
        }
        set
        {
            this["珍宝模块切换"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 称号属性切换
    {
        get
        {
            return (int)this["称号属性切换"];
        }
        set
        {
            this["称号属性切换"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("30")]
    public int 全服红包等级
    {
        get
        {
            return (int)this["全服红包等级"];
        }
        set
        {
            this["全服红包等级"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("19")]
    public int 全服红包时间
    {
        get
        {
            return (int)this["全服红包时间"];
        }
        set
        {
            this["全服红包时间"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("3")]
    public int 全服红包货币类型
    {
        get
        {
            return (int)this["全服红包货币类型"];
        }
        set
        {
            this["全服红包货币类型"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10000")]
    public int 全服红包货币数量
    {
        get
        {
            return (int)this["全服红包货币数量"];
        }
        set
        {
            this["全服红包货币数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("85")]
    public int 龙卫蓝色词条概率
    {
        get
        {
            return (int)this["龙卫蓝色词条概率"];
        }
        set
        {
            this["龙卫蓝色词条概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 龙卫紫色词条概率
    {
        get
        {
            return (int)this["龙卫紫色词条概率"];
        }
        set
        {
            this["龙卫紫色词条概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 龙卫橙色词条概率
    {
        get
        {
            return (int)this["龙卫橙色词条概率"];
        }
        set
        {
            this["龙卫橙色词条概率"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int 自定义初始货币类型
    {
        get
        {
            return (int)this["自定义初始货币类型"];
        }
        set
        {
            this["自定义初始货币类型"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 自动回收设置
    {
        get
        {
            return (int)this["自动回收设置"];
        }
        set
        {
            this["自动回收设置"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 购买狂暴之力
    {
        get
        {
            return (int)this["购买狂暴之力"];
        }
        set
        {
            this["购买狂暴之力"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 会员满血设置
    {
        get
        {
            return (int)this["会员满血设置"];
        }
        set
        {
            this["会员满血设置"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 全屏拾取开关
    {
        get
        {
            return (int)this["全屏拾取开关"];
        }
        set
        {
            this["全屏拾取开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 打开随时仓库
    {
        get
        {
            return (int)this["打开随时仓库"];
        }
        set
        {
            this["打开随时仓库"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 幸运保底开关
    {
        get
        {
            return (int)this["幸运保底开关"];
        }
        set
        {
            this["幸运保底开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 会员物品对接
    {
        get
        {
            return (int)this["会员物品对接"];
        }
        set
        {
            this["会员物品对接"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("40")]
    public int 变性等级
    {
        get
        {
            return (int)this["变性等级"];
        }
        set
        {
            this["变性等级"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 变性货币类型
    {
        get
        {
            return (int)this["变性货币类型"];
        }
        set
        {
            this["变性货币类型"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000000")]
    public int 变性货币值
    {
        get
        {
            return (int)this["变性货币值"];
        }
        set
        {
            this["变性货币值"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 变性物品ID
    {
        get
        {
            return (int)this["变性物品ID"];
        }
        set
        {
            this["变性物品ID"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 变性物品数量
    {
        get
        {
            return (int)this["变性物品数量"];
        }
        set
        {
            this["变性物品数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("输入密码")]
    public string 变性内容控件
    {
        get
        {
            return (string)this["变性内容控件"];
        }
        set
        {
            this["变性内容控件"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 称号叠加模块9
    {
        get
        {
            return (byte)this["称号叠加模块9"];
        }
        set
        {
            this["称号叠加模块9"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 称号叠加模块10
    {
        get
        {
            return (byte)this["称号叠加模块10"];
        }
        set
        {
            this["称号叠加模块10"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 称号叠加模块11
    {
        get
        {
            return (byte)this["称号叠加模块11"];
        }
        set
        {
            this["称号叠加模块11"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 称号叠加模块12
    {
        get
        {
            return (byte)this["称号叠加模块12"];
        }
        set
        {
            this["称号叠加模块12"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 称号叠加模块13
    {
        get
        {
            return (byte)this["称号叠加模块13"];
        }
        set
        {
            this["称号叠加模块13"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 称号叠加模块14
    {
        get
        {
            return (byte)this["称号叠加模块14"];
        }
        set
        {
            this["称号叠加模块14"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 称号叠加模块15
    {
        get
        {
            return (byte)this["称号叠加模块15"];
        }
        set
        {
            this["称号叠加模块15"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public byte 称号叠加模块16
    {
        get
        {
            return (byte)this["称号叠加模块16"];
        }
        set
        {
            this["称号叠加模块16"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 龙卫焰焚烈火剑法
    {
        get
        {
            return (int)this["龙卫焰焚烈火剑法"];
        }
        set
        {
            this["龙卫焰焚烈火剑法"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 红包开关
    {
        get
        {
            return (int)this["红包开关"];
        }
        set
        {
            this["红包开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 安全区收刀开关
    {
        get
        {
            return (int)this["安全区收刀开关"];
        }
        set
        {
            this["安全区收刀开关"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("25")]
    public int 屠魔殿等级限制
    {
        get
        {
            return (int)this["屠魔殿等级限制"];
        }
        set
        {
            this["屠魔殿等级限制"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("40")]
    public int 职业等级
    {
        get
        {
            return (int)this["职业等级"];
        }
        set
        {
            this["职业等级"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 职业货币类型
    {
        get
        {
            return (int)this["职业货币类型"];
        }
        set
        {
            this["职业货币类型"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1000000")]
    public int 职业货币值
    {
        get
        {
            return (int)this["职业货币值"];
        }
        set
        {
            this["职业货币值"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("999999")]
    public int 职业物品ID
    {
        get
        {
            return (int)this["职业物品ID"];
        }
        set
        {
            this["职业物品ID"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("10")]
    public int 职业物品数量
    {
        get
        {
            return (int)this["职业物品数量"];
        }
        set
        {
            this["职业物品数量"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("输入密码")]
    public string 转职内容控件
    {
        get
        {
            return (string)this["转职内容控件"];
        }
        set
        {
            this["转职内容控件"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("100000")]
    public int 武斗场杀人经验
    {
        get
        {
            return (int)this["武斗场杀人经验"];
        }
        set
        {
            this["武斗场杀人经验"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int 武斗场杀人开关
    {
        get
        {
            return (int)this["武斗场杀人开关"];
        }
        set
        {
            this["武斗场杀人开关"] = value;
        }
    }
}
