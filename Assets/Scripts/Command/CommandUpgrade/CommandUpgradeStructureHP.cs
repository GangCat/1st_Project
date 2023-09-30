using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgradeStructureHP : Command
{
    public CommandUpgradeStructureHP(StatusHp _statusHp)
    {
        statusHp = _statusHp;
    }
    public override void Execute(params object[] _objects)
    {
        statusHp.UpgradeHp((float)_objects[0]);
    }

    private StatusHp statusHp = null;
}
