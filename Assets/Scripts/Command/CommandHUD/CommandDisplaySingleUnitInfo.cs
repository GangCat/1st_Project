using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplaySingleUnitInfo : Command
{
    public CommandDisplaySingleUnitInfo(CanvasUnitInfo _canvasInfo)
    {
        canvasInfo = _canvasInfo;
    }
    public override void Execute(params object[] _objects)
    {
        canvasInfo.DisplaySingleUnitInfo();
    }

    private CanvasUnitInfo canvasInfo = null;
}
