using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplaySpawnNuclearInfo : Command
{
    public CommandDisplaySpawnNuclearInfo(CanvasSpawnNuclearInfo _canvasNuclear)
    {
        canvasNuclear = _canvasNuclear;
    }

    public override void Execute(params object[] _objects)
    {
        canvasNuclear.ShowDisplay();
    }

    private CanvasSpawnNuclearInfo canvasNuclear = null;
}
