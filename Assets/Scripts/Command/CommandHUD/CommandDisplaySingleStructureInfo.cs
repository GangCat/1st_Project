using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplaySingleStructureInfo : Command
{
    public CommandDisplaySingleStructureInfo(CanvasUnitInfo _canvasInfo)
    {
        canvasInfo = _canvasInfo;
    }
    public override void Execute(params object[] _objects)
    {
        canvasInfo.DisplaySingleUnitInfo((string)_objects[0], (string)_objects[1], (int)_objects[2]);
    }

    private CanvasUnitInfo canvasInfo = null;
}
