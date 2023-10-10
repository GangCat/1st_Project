using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateDemolishTime : Command
{
    public CommandUpdateDemolishTime(CanvasDemolishInfo _canvasDemolish)
    {
        canvasDemolish = _canvasDemolish;
    }

    public override void Execute(params object[] _objects)
    {
        canvasDemolish.UpdateDemolishTime((float)_objects[0]);
    }

    private CanvasDemolishInfo canvasDemolish = null;
}
