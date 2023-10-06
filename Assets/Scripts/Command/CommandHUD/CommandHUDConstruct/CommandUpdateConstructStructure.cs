using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommandUpdateConstructStructure : Command
{
    public CommandUpdateConstructStructure(CanvasConstructInfo _canvas)
    {
        canvasConstruct = _canvas;
    }

    public override void Execute(params object[] _objects)
    {
        canvasConstruct.UpdateUnit((EObjectType)_objects[0]);
    }

    private CanvasConstructInfo canvasConstruct = null;
}
