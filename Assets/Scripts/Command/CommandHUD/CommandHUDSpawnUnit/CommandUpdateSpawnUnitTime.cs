using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateSpawnUnitTime : Command
{
    public CommandUpdateSpawnUnitTime(CanvasSpawnUnitInfo _canvasSpawn)
    {
        canvasSpawn = _canvasSpawn;
    }
    public override void Execute(params object[] _objects)
    {
        canvasSpawn.UpdateTime((float)_objects[0]);
    }

    private CanvasSpawnUnitInfo canvasSpawn = null;
}
