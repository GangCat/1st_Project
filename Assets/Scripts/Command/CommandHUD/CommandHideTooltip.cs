using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHideTooltip : Command
{
    public CommandHideTooltip(CanvasTooltip _canvasTooltip)
    {
        canvasTooltip = _canvasTooltip;
    }

    public override void Execute(params object[] _objects)
    {
        canvasTooltip.HideTooltip();
    }

    private CanvasTooltip canvasTooltip = null;
}
