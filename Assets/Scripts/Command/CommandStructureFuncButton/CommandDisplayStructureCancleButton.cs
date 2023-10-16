using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplayStructureCancleButton : Command
{
    public CommandDisplayStructureCancleButton(CanvasStructureBaseFunc _canvasStructure)
    {
        canvasStructure = _canvasStructure;
    }

    public override void Execute(params object[] _objects)
    {
        canvasStructure.DisplayCancleButton();
    }

    private CanvasStructureBaseFunc canvasStructure = null;
}
