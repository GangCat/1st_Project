using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateEnergyDisplay : Command
{
    public CommandUpdateEnergyDisplay(UIManager _uiMng)
    {
        uiMng = _uiMng;
    }
    public override void Execute(params object[] _objects)
    {
        uiMng.UpdateEnergy((uint)_objects[0]);
    }

    private UIManager uiMng = null;
}
