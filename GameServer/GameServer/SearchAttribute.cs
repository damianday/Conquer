using System;

namespace GameServer;

[AttributeUsage(AttributeTargets.Class)]
public class SearchAttribute : Attribute
{
    public string SearchName;
}
