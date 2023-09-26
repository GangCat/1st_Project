using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgradeAttRange : Command
{
    public CommandUpgradeAttRange(FriendlyObject _object)
    {
        friendlyObject = _object;
    }

    public override void Execute(params object[] _objects)
    {
        friendlyObject.UpgradeAttackRange((float)_objects[0]);
    }

    private FriendlyObject friendlyObject = null;
}
