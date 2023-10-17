using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMoveCurStateIndicator : Command
{
    public CommandMoveCurStateIndicator(DebugModeManager _debugMng)
    {
        debugMng = _debugMng;
    }

    public override void Execute(params object[] _objects)
    {
        if(debugMng.isActive)
            debugMng.DisplayCurState((Vector3)_objects[0], (EState)_objects[1]);
    }

    private DebugModeManager debugMng = null;
}
