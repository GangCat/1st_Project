using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateSpawnUnitList : Command
{
    public CommandUpdateSpawnUnitList(CanvasSpawnUnitInfo _canvasSpawn)
    {
        canvasSpawn = _canvasSpawn;
    }
    public override void Execute(params object[] _objects)
    {
        canvasSpawn.UpdateSpawnList((List<EUnitType>)_objects[0]);
    }

    private CanvasSpawnUnitInfo canvasSpawn = null;
}
