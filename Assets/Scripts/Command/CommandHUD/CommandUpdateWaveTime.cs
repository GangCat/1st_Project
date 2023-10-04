using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateWaveTime : Command
{
    public CommandUpdateWaveTime(CanvasWaveInfo _canvasWaveInfo)
    {
        canvasWaveInfo = _canvasWaveInfo;
    }

    public override void Execute(params object[] _objects)
    {
        canvasWaveInfo.UpdateWaveTime((float)_objects[0]);
    }

    private CanvasWaveInfo canvasWaveInfo = null;
}
