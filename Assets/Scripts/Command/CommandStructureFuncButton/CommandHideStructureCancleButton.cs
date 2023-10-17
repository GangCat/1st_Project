using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHideStructureCancleButton : Command
{
    public CommandHideStructureCancleButton(CanvasStructureBaseFunc _canvasStructure)
    {
        canvasStructure = _canvasStructure;
    }

    public override void Execute(params object[] _objects)
    {
        canvasStructure.HideCancleButton();
    }

    private CanvasStructureBaseFunc canvasStructure = null;
}
