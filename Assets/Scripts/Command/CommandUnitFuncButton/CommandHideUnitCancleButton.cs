using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHideUnitCancleButton : Command
{
    public CommandHideUnitCancleButton(CanvasUnitBaseFunc _canvasUnitBase)
    {
        canvasUnitBase = _canvasUnitBase;
    }

    public override void Execute(params object[] _objects)
    {
        canvasUnitBase.HideCancleButton();
    }

    private CanvasUnitBaseFunc canvasUnitBase = null;
}
