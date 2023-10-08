using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplayDemolishInfo : Command
{
    public CommandDisplayDemolishInfo(CanvasDemolishInfo _canvasDemolish)
    {
        canvasDemolish = _canvasDemolish;
    }

    public override void Execute(params object[] _objects)
    {
        canvasDemolish.ShowDisplay();
    }

    private CanvasDemolishInfo canvasDemolish = null;
}
