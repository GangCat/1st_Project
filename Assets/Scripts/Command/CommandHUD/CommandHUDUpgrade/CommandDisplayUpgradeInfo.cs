using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplayUpgradeInfo : Command
{
    public CommandDisplayUpgradeInfo(CanvasUpgradeInfo _canvasUpgrade)
    {
        canvasUpgrade = _canvasUpgrade;
    }
    public override void Execute(params object[] _objects)
    {
        canvasUpgrade.DisplayUpgradeInfo((EUpgradeType)_objects[0]);
    }

    private CanvasUpgradeInfo canvasUpgrade = null;
}
