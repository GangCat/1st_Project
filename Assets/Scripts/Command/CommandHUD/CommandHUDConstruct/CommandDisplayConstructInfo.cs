using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplayConstructInfo : Command
{
    public CommandDisplayConstructInfo(CanvasConstructInfo _canvas)
    {
        canvasConstruct = _canvas;
    }
    public override void Execute(params object[] _objects)
    {
        canvasConstruct.ShowDisplay();
    }

    private CanvasConstructInfo canvasConstruct = null;
}
