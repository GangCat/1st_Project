using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateDemolishStructure : Command
{
    public CommandUpdateDemolishStructure(CanvasDemolishInfo _canvasDemolish)
    {
        canvasDemolish = _canvasDemolish;
    }
    public override void Execute(params object[] _objects)
    {
        canvasDemolish.UpdateUnit((EObjectType)_objects[0]);
    }

    private CanvasDemolishInfo canvasDemolish = null;
}
