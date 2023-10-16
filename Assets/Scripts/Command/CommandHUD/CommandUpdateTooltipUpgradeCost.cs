using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateTooltipUpgradeCost : Command
{
    public CommandUpdateTooltipUpgradeCost(CanvasStructureBaseFunc _canvasStructureBaseFunc)
    {
        canvasStructureBaseFunc = _canvasStructureBaseFunc;
    }

    public override void Execute(params object[] _objects)
    {
        canvasStructureBaseFunc.ChangeUpgradeStructureCost((int)_objects[0]);
    }

    private CanvasStructureBaseFunc canvasStructureBaseFunc = null;
}
