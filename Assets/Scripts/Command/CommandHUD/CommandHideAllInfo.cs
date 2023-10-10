using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHideAllInfo : Command
{
    public CommandHideAllInfo(DisplayHUDManager _displayMng)
    {
        displayMng = _displayMng;
    }

    public override void Execute(params object[] _objects)
    {
        displayMng.HideDisplay();
    }

    private DisplayHUDManager displayMng = null;
    private CanvasUnitInfo canvasUnit = null;
    private CanvasSpawnUnitInfo canvasSpawn = null;
    private CanvasUpgradeInfo canvasUpgrade = null;
    private CanvasConstructInfo canvasConstruct = null;
}
