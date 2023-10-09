using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHideStructureCancleButton : Command
{
    public CommandHideStructureCancleButton(CanvasStructureCancleFunc _canvasCancle)
    {
        canvasCancle = _canvasCancle;
    }

    public override void Execute(params object[] _objects)
    {
        canvasCancle.HideCancleButton();
    }

    private CanvasStructureCancleFunc canvasCancle = null;
}
