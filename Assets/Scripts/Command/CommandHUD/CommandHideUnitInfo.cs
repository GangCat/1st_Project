using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHideUnitInfo : Command
{
    public CommandHideUnitInfo(CanvasUnitInfo _canvasUnitInfo)
    {
        canvasUnitInfo = _canvasUnitInfo;
    }

    public override void Execute(params object[] _objects)
    {
        canvasUnitInfo.HideDisplay();
    }

    private CanvasUnitInfo canvasUnitInfo = null;
}
