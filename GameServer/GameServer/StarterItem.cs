using System;

namespace GameServer;

public class StarterItem
{
    public string ItemName;
    public GameObjectGender RequiredGender;
    public GameObjectRace RequiredRace;

    public GameObjectGender BlockedGender = GameObjectGender.Any;
    public GameObjectRace BlockedRace = GameObjectRace.Any;
}
