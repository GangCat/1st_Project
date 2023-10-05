using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateSpawnUnitProgress : Command
{
    public CommandUpdateSpawnUnitProgress(CanvasSpawnUnitInfo _canvas)
    {
        canvasSpawnUnit = _canvas;
    }
    public override void Execute(params object[] _objects)
    {
        canvasSpawnUnit.Updateprogress((float)_objects[0]);
    }

    private CanvasSpawnUnitInfo canvasSpawnUnit = null;
}
