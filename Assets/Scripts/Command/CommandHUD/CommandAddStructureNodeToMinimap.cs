using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandAddStructureNodeToMinimap : Command
{
    public CommandAddStructureNodeToMinimap(UIManager _uiMng)
    {
        uiMng = _uiMng;
    }

    public override void Execute(params object[] _objects)
    {
        for(int i = 0; i < _objects.Length; ++i)
        {
            uiMng.AddStructureNodeToMinimap((PF_Node)_objects[i]);
        }
    }

    private UIManager uiMng = null;
}
