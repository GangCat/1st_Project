using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgradePopulationComplete : Command
{
    public CommandUpgradePopulationComplete(PopulationManager _popMng)
    {
        popMng = _popMng;
    }

    public override void Execute(params object[] _objects)
    {
        popMng.UpgradeMaxPopulation();
    }

    private PopulationManager popMng = null;
}