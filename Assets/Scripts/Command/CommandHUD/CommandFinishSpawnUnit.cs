using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFinishSpawnUnit : Command
{
    public CommandFinishSpawnUnit(CanvasSpawnUnitInfo _canvasSpawnInfo)
    {
        canvasSpawn = _canvasSpawnInfo;
    }

    public override void Execute(params object[] _objects)
    {
        canvasSpawn.SpawnFinish();
    }

    private CanvasSpawnUnitInfo canvasSpawn = null;
}
