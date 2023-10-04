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
        canvasInfo.DisplaySIngleUnitInfo((SUnitInfo)_objects[0]);
    }

    private CanvasUnitInfo canvasInfo = null;
}
