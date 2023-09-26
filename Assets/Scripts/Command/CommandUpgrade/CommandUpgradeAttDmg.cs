using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgradeAttDmg : Command
{
    public CommandUpgradeAttDmg(FriendlyObject _object)
    {
        friendlyObject = _object;
    }

    public override void Execute(params object[] _objects)
    {
        friendlyObject.UpgradeAttackDmg((float)_objects[0]);
    }

    private FriendlyObject friendlyObject = null;
}
