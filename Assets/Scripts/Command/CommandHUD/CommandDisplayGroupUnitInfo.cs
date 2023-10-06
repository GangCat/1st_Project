using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplayGroupUnitInfo : Command
{
    public CommandDisplayGroupUnitInfo(CanvasUnitInfo _canvasInfo, CanvasSpawnUnitInfo _canvasSpawn)
    {
        canvasInfo = _canvasInfo;
    }
    public override void Execute(params object[] _objects)
    {
        canvasInfo.DisplayGroupUnitInfo((int)_objects[0]);
    }

    private CanvasUnitInfo canvasInfo = null;
}
