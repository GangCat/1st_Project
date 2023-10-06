using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHideAllInfo : Command
{
    public CommandHideAllInfo(
        CanvasUnitInfo _canvasUnit, 
        CanvasSpawnUnitInfo _canvasSpawn, 
        CanvasUpgradeInfo _canvasUpgrade,
        CanvasConstructInfo _canvasConstruct)
    {
        canvasUnit = _canvasUnit;
        canvasSpawn = _canvasSpawn;
        canvasUpgrade = _canvasUpgrade;
        canvasConstruct = _canvasConstruct;
    }

    public override void Execute(params object[] _objects)
    {
        canvasUnit.HideDisplay();
        canvasSpawn.HideDisplay();
        canvasUpgrade.HideDisplay();
        canvasConstruct.HideDisplay();
    }

    private CanvasUnitInfo canvasUnit = null;
    private CanvasSpawnUnitInfo canvasSpawn = null;
    private CanvasUpgradeInfo canvasUpgrade = null;
    private CanvasConstructInfo canvasConstruct = null;
}
