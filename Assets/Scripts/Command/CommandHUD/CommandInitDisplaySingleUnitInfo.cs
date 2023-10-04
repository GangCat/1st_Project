using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInitDisplaySingleUnitInfo : Command
{
    public CommandInitDisplaySingleUnitInfo(CanvasUnitInfo _canvas)
    {
        canvasUnitInfo = _canvas;
    }
    public override void Execute(params object[] _objects)
    {
        canvasUnitInfo.InitSingleUnitInfo((UnitInfoContainer)_objects[0]);
    }

    private CanvasUnitInfo canvasUnitInfo = null;
}
