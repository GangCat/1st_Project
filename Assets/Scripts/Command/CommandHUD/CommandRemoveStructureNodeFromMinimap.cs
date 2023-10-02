using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRemoveStructureNodeFromMinimap : Command
{
    public CommandRemoveStructureNodeFromMinimap(UIManager _uiMng)
    {
        uiMng = _uiMng;
    }

    public override void Execute(params object[] _objects)
    {
        for(int i = 0; i < _objects.Length; ++i)
            uiMng.RemoveStructureNodeFromMinimap((PF_Node)_objects[i]);
    }

    private UIManager uiMng = null;
}
