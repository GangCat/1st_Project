using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInitWaveTime : Command
{
    public CommandInitWaveTime(CanvasWaveInfo _canvasWaveInfo)
    {
        canvasWaveInfo = _canvasWaveInfo;
    }

    public override void Execute(params object[] _objects)
    {
        canvasWaveInfo.Init((float)_objects[0]);
    }

    private CanvasWaveInfo canvasWaveInfo = null;
}
