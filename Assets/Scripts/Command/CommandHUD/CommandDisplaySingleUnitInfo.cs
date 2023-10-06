using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplaySingleUnitInfo : Command
{
    public CommandDisplaySingleUnitInfo(CanvasUnitInfo _canvasInfo, CanvasSpawnUnitInfo _canvasSpawn)
    {
        canvasInfo = _canvasInfo;
        //canvasSpawn = _canvasSpawn;
    }
    public override void Execute(params object[] _objects)
    {
        //canvasSpawn.SetActive(false);
        canvasInfo.DisplaySingleUnitInfo();
    }

    private CanvasUnitInfo canvasInfo = null;
    //private CanvasSpawnUnitInfo canvasSpawn = null;
}
