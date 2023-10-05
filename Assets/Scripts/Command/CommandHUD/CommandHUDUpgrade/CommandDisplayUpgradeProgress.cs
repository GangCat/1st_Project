using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplayUpgradeProgress : Command
{
    public CommandDisplayUpgradeProgress(CanvasUpgradeInfo _canvasUpgrade)
    {
        canvasUpgrade = _canvasUpgrade;
    }
    public override void Execute(params object[] _objects)
    {
        canvasUpgrade.DisplayUpgradeInfo();
    }
    private CanvasUpgradeInfo canvasUpgrade = null;
}
