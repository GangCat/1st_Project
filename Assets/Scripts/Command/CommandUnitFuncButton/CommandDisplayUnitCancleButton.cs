using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplayUnitCancleButton : Command
{
    public CommandDisplayUnitCancleButton(CanvasUnitBaseFunc _canvasUnitBase)
    {
        canvasUnitBase = _canvasUnitBase;
    }

    public override void Execute(params object[] _objects)
    {
        canvasUnitBase.DisplayCancleButton();
    }

    private CanvasUnitBaseFunc canvasUnitBase = null;
}
