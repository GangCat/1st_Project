using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateCoreDisplay : Command
{
    public CommandUpdateCoreDisplay(UIManager _uiMng)
    {
        uiMng = _uiMng;
    }
    public override void Execute(params object[] _objects)
    {
        uiMng.UpdateCore((uint)_objects[0]);
    }

    private UIManager uiMng = null;
}
