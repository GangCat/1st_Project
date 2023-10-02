using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateMinimap : Command
{
    public CommandUpdateMinimap(UIManager _uiMng)
    {
        uiMng = _uiMng;
    }

    public override void Execute(params object[] _objects)
    {
        uiMng.UpdateMinimap((EObjectType)_objects[0], (PF_Node)_objects[1], (PF_Node)_objects[2]);
    }

    private UIManager uiMng = null;
}
