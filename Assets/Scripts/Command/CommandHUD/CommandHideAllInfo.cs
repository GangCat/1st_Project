using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHideAllInfo : Command
{
    public CommandHideAllInfo(CanvasUnitInfo _canvasUnit, CanvasSpawnUnitInfo _canvasSpawn, CanvasUpgradeInfo _canvasUpgrade)
    {
        canvasUnit = _canvasUnit;
        canvasSpawn = _canvasSpawn;
        canvasUpgrade = _canvasUpgrade;
    }

    public override void Execute(params object[] _objects)
    {
        canvasUnit.HideDisplay();
        canvasSpawn.HideDisplay();
        canvasUpgrade.HideDisplay();
    }

    private CanvasUnitInfo canvasUnit;
    private CanvasSpawnUnitInfo canvasSpawn;
    private CanvasUpgradeInfo canvasUpgrade;
}
