using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateConstructTime : Command
{
    public CommandUpdateConstructTime(CanvasConstructInfo _canvas)
    {
        canvasConstruct = _canvas;
    }
    public override void Execute(params object[] _objects)
    {
        canvasConstruct.UpdateConstructTime((float)_objects[0]);
    }

    private CanvasConstructInfo canvasConstruct = null;
}
