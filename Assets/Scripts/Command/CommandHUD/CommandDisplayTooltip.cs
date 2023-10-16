using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplayTooltip : Command
{
    public CommandDisplayTooltip(CanvasTooltip _canvasTooltip)
    {
        canvasTooltip = _canvasTooltip;
    }

    public override void Execute(params object[] _objects)
    {
        canvasTooltip.DisplayTooltip((string)_objects[0], (string)_objects[1], (string)_objects[2], (ECurrencyType)_objects[3], (Vector3)_objects[4]);
    }

    private CanvasTooltip canvasTooltip = null;
}
