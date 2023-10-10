using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateSpawnNuclearTime : Command
{
    public CommandUpdateSpawnNuclearTime(CanvasSpawnNuclearInfo _canvasNuclear)
    {
        canvasNuclear = _canvasNuclear;
    }

    public override void Execute(params object[] _objects)
    {
        canvasNuclear.UpdateSpawnNuclearTime((float)_objects[0]);
    }

    private CanvasSpawnNuclearInfo canvasNuclear = null;
}
