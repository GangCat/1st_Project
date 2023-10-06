using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplaySpawnUnitInfo : Command
{
    public CommandDisplaySpawnUnitInfo(CanvasSpawnUnitInfo _canvasSpawn)
    {
        canvasSpawn = _canvasSpawn;
    }
    public override void Execute(params object[] _objects)
    {
        canvasSpawn.ShowDisplay();
    }

    private CanvasSpawnUnitInfo canvasSpawn = null;
}
