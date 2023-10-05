using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplaySpawnUnitInfo : Command
{
    public CommandDisplaySpawnUnitInfo(CanvasSpawnUnitInfo _canvas)
    {
        canvasSpawn = _canvas;
    }
    public override void Execute(params object[] _objects)
    {
        canvasSpawn.SetActive(true);
    }

    private CanvasSpawnUnitInfo canvasSpawn = null;
}
