using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateCurMaxPopulationHUD : Command
{
    public CommandUpdateCurMaxPopulationHUD(UIManager _uiMng)
    {
        uiMng = _uiMng;
    }

    public override void Execute(params object[] _objects)
    {
        uiMng.UpdateCurMaxPopulation((uint)_objects[0]);
    }

    private UIManager uiMng = null;
}
