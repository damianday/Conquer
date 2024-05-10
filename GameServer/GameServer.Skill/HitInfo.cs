using System.Collections.Generic;
using System.IO;
using System.Linq;

using GameServer.Map;
using GameServer.Template;

namespace GameServer.Skill;

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

    public static byte[] HitDescription(Dictionary<int, HitInfo> hitters, int latency)
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);

        writer.Write((byte)hitters.Count);
        foreach (var kvp in hitters.ToList())
        {
            writer.Write(kvp.Value.Target.ObjectID);
            writer.Write((ushort)kvp.Value.SkillFeedback);
            writer.Write((ushort)latency);
        }
        return ms.ToArray();
    }
}
