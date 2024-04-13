using System;

namespace GameServer;

[AttributeUsage(AttributeTargets.Field)]
public sealed class FieldDescription : Attribute
{
    public int Index;

    public FieldDescription(int 排序 = 0)
    {
        this.Index = 排序;
    }
}
