using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateCoreHUD : Command
{
    public CommandUpdateCoreHUD(UIManager _uiMng)
    {
        uiMng = _uiMng;
    }
    public override void Execute(params object[] _objects)
    {
        uiMng.UpdateCore((uint)_objects[0]);
    }

    private UIManager uiMng = null;
}
