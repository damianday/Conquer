using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameServer.Map;

namespace GameServer.Template;

public class HitInfo
{
    public int SkillDamage;
    public ushort ParryDamage;
    public MapObject Target;
    public SkillHitFeedback SkillFeedback;

    public HitInfo(MapObject target)
    {
        Target = target;
    }

    public HitInfo(MapObject target, SkillHitFeedback feedback)
    {
        Target = target;
        SkillFeedback = feedback;
    }

    public static byte[] 命中描述(Dictionary<int, HitInfo> hitters, int 命中延迟)
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);

        writer.Write((byte)hitters.Count);
        foreach (var kvp in hitters.ToList())
        {
            writer.Write(kvp.Value.Target.ObjectID);
            writer.Write((ushort)kvp.Value.SkillFeedback);
            writer.Write((ushort)命中延迟);
        }
        return ms.ToArray();
    }
}
