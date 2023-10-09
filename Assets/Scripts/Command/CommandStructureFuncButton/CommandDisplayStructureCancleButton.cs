using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplayStructureCancleButton : Command
{
    public CommandDisplayStructureCancleButton(CanvasStructureCancleFunc _canvasCancle)
    {
        canvasCancle = _canvasCancle;
    }

    public override void Execute(params object[] _objects)
    {
        canvasCancle.DisplayCancleButton();
    }

    private CanvasStructureCancleFunc canvasCancle = null;
}
