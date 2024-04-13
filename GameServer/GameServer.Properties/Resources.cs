using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace GameServer.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
    private static ResourceManager resourceMan;

    private static CultureInfo resourceCulture;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
        get
        {
            if (resourceMan == null)
            {
                ResourceManager resourceManager = (resourceMan = new ResourceManager("游戏服务器.Properties.Resources", typeof(Resources).Assembly));
            }
            return resourceMan;
        }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
        get
        {
            return resourceCulture;
        }
        set
        {
            resourceCulture = value;
        }
    }

    internal static Bitmap 保存
    {
        get
        {
            object @object = ResourceManager.GetObject("保存", resourceCulture);
            return (Bitmap)@object;
        }
    }

    internal static Bitmap 停止
    {
        get
        {
            object @object = ResourceManager.GetObject("停止", resourceCulture);
            return (Bitmap)@object;
        }
    }

    internal static Bitmap 启动
    {
        get
        {
            object @object = ResourceManager.GetObject("启动", resourceCulture);
            return (Bitmap)@object;
        }
    }

    internal Resources()
    {
    }
}
