using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateUpgradeProgress : Command
{
    public CommandUpdateUpgradeProgress(CanvasUpgradeInfo _canvasUpgrade)
    {
        canvasUpgrade = _canvasUpgrade;
    }
    public override void Execute(params object[] _objects)
    {
        canvasUpgrade.UpdateUpgradeProgress((float)_objects[0]);
    }

    private CanvasUpgradeInfo canvasUpgrade = null;
}
