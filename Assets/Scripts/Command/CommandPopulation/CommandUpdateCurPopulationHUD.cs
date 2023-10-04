using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class CommandUpdateCurPopulationHUD : Command
{
    public CommandUpdateCurPopulationHUD(UIManager _uiMng)
    {
        uiMng = _uiMng;
    }

    public override void Execute(params object[] _objects)
    {
        uiMng.UpdateCurPopulation((uint)_objects[0]);
    }

    private UIManager uiMng = null;
}
