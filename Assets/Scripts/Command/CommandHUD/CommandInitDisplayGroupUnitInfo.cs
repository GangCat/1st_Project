using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInitDisplayGroupUnitInfo : Command
{
    public CommandInitDisplayGroupUnitInfo(CanvasUnitInfo _canvasUnitInfo)
    {
        canvasUnitInfo = _canvasUnitInfo;
    }
    public override void Execute(params object[] _objects)
    {
        canvasUnitInfo.InitGroupUnitInfo((List<SFriendlyUnitInfo>)_objects[0]);
    }

    private CanvasUnitInfo canvasUnitInfo = null;
}
